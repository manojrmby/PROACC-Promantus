CREATE PROCEDURE [dbo].[SP_Instance]
@Type varchar(50),
@Id uniqueidentifier=null,
@InstaceName varchar(150)=null,
@CustProjconfigID uniqueidentifier=null,
@Cre_By uniqueidentifier=null,
@Project_ID uniqueidentifier=null,
@Version_ID uniqueidentifier=null,
@Previous_Instance uniqueidentifier=null,
@Cre_On uniqueidentifier=null,
@LastUpdate datetime=null,
@isActive bit=null,
@ModifiedBy uniqueidentifier=null,
@ModifiedOn datetime=null,
@Description varchar(50)=null,
@isDelete bit=null,
@LoginID uniqueidentifier=null,
@InstanceID uniqueidentifier=null,
--SAP Connection
@Sys_Landscape varchar(150)=null,
@DestinationName varchar(150)=null,
@AppServerHost varchar(150)=null,
@SystemNumber varchar(150)=null,
@SAPRouter varchar(150)=null,
@SAPUser varchar(150)=null,
@SAPPassword varchar(150)=null,
@Client varchar(150)=null,
@Language varchar(150)=null,
@PoolSize varchar(150)=null,
@tblSalesDoc UploadSalesDocument READONLY,
@tblBillingDoc UploadBillingDocument READONLY,
@tblOrderDoc UploadOrderDocument READONLY,
@tblMatScore UploadMaterialityScore READONLY,
@tblRFCFM UploadRFCFMType READONLY,
@tblSOFTWARECOMPONENT UploadINSTALLEDSOFTWARECOMPONENT READONLY,
@tblPRODUCTVERSIONS UploadINSTALLEDPRODUCTVERSIONS READONLY,
@tblSFW5REPORT UploadSFW5REPORT READONLY

AS
BEGIN
	--IF @Type='AddInstance'
	--BEGIN
	--	INSERT INTO ProjectInstanceConfig(Id,InstaceName,CustProjconfigID,LastUpdated_Dt,Cre_By)
	--	VALUES(@Id,@InstaceName,@CustProjconfigID,GETDATE(),@Cre_By)
	--END
IF @Type='GetInstanceData'
BEGIN
Declare @sysLandscape varchar(50)
SET @sysLandscape=(select Sys_Landscape from Instance where Instance_id=@Id)

IF @sysLandscape='Automation'
BEGIN
SELECT *,
(select u.[Name] from  UserMaster u  where u.UserId = a.Cre_By)[UserName],
(select p.Project_Id from  Project p  where p.Project_Id = a.Project_ID)[PID],
(select V.Version_Name from  AMVersion V  where V.ID = a.Version_ID)[Version_Name],
(select V.ID from  AMVersion V  where V.ID = a.Version_ID)[VID]
from Instance a join SAPConnection SC on a.Instance_id=SC.Instance_Id
where a.Instance_id=@Id and a.isActive=1
END
ELSE
BEGIN
SELECT *,
(select u.Name from  UserMaster u  where u.UserId = a.Cre_By)[UserName],
(select p.Project_Id from  Project p  where p.Project_Id = a.Project_ID)[PID],
(select V.Version_Name from  AMVersion V  where V.ID = a.Version_ID)[Version_Name],
(select V.ID from  AMVersion V  where V.ID = a.Version_ID)[VID]
from Instance a where a.Instance_id=@Id and a.isActive=1
END

END

IF @Type='GetProjectName'
BEGIN
Select Distinct Project_ID, 
(select pr.Project_Name from Project pr where pr.Project_Id=insta.Project_ID and pr.isActive='1')[ProjectName]
From Instance insta where insta.isActive='1'
END

IF @Type='GetAllProjectNames'
BEGIN
select * from project where isActive=1 order by Project_Name asc
END

	IF @Type='GetInstance_Analysis'
	BEGIN
		SELECT Instance_Id As Id,InstaceName from instance where AssessmentUploadStatus=0 AND isActive=1 AND project_id=@Id order by InstaceName
	END

	IF @Type='GetInstance_Readiness'
	BEGIN
		SELECT Instance_Id As Id,InstaceName from instance where AssessmentUploadStatus=1 AND isActive=1 AND project_id=@Id order by InstaceName
	END
	IF @Type='GetInstanceStatus'
	BEGIN
		DECLARE @TOTAL int=null,@Actual int=NULL
		select @Actual=COUNT(*) from ProjectMonitor WHERE InstanceID=@Previous_Instance AND PhaseId=1 AND StatusId!=1 AND StatusId!=3
		select @TOTAL=count(*) from ProjectMonitor WHERE InstanceID=@Previous_Instance
		SELECT @Actual AS Actual,@TOTAL AS Total
	END
	--IF @Type='GetPerConvertionUploadInstance'
	--BEGIN
	--	SELECT Id,InstaceName from ProjectInstanceConfig where UploadStatus=1 AND PreConvertionIsActive=0 AND CustProjconfigID=@Id
	--END
	IF @Type='CreateInstance'
	BEGIN
	Declare @EmailTo varchar(100),@Instance_id uniqueidentifier =NewId(),@User uniqueidentifier, @SAPAutoDownloadId uniqueidentifier =NewId(),
	@AutoList_ID int

	set @EmailTo=(select [EMail] from UserMaster where UserId=(select ProjectManager_Id from Project where Project_Id=@Project_ID))
	set @User=(select ProjectManager_Id from Project where Project_Id=@Project_ID)

	Insert into Instance (Instance_id,InstaceName,Project_ID,Version_ID,LastUpdated_Dt,Cre_By,Description,Sys_Landscape) 
		  values (@Instance_id,@InstaceName,@Project_ID,@Version_ID,GETUTCDATE(),@Cre_By,@Description,@Sys_Landscape)

    
	if @Sys_Landscape='Automation'
	BEGIN
	INSERT into SAPConnection (Id,Instance_Id,DestinationName,AppServerHost,SystemNumber,SAPRouter,SAPUser,SAPPassword,Client,Language,PoolSize,Cre_on,Cre_By)
	   values(NewId(),@Instance_id,@DestinationName,@AppServerHost,@SystemNumber,@SAPRouter,@SAPUser,@SAPPassword,@Client,@Language,@PoolSize,GETUTCDATE(),@Cre_By)
	
	SET @AutoList_ID=(select Id from SAPAutoList where [Name]='SalesDocuments')
    INSERT INTO SAPAutomationDownload(Id,InstanceID,AutoList_ID,Cre_By)
			values(@SAPAutoDownloadId,@Instance_id,@AutoList_ID,@Cre_By)
     
	 INSERT SalesDocuments(FileUploadID,CCbilled,SalesOrg,DistChannel,Division,Plant,NoofDocuments,Cre_By)
			SELECT @SAPAutoDownloadId, CCbilled,SalesOrg,DistChannel,Division,Plant,NoofDocuments,@Cre_By
			FROM @tblSalesDoc;

	SET @AutoList_ID=(select Id from SAPAutoList where [Name]='BillingDocuments')
    INSERT INTO SAPAutomationDownload(Id,InstanceID,AutoList_ID,Cre_By)
			values(@SAPAutoDownloadId,@Instance_id,@AutoList_ID,@Cre_By)
			
	INSERT BillingDocuments(FileUploadID,CompanyCode,Currency,FKREL,BillingCreatedby,NoofDocuments,Cre_By)
			SELECT @SAPAutoDownloadId, CompanyCode,Currency,FKREL,BillingCreatedby,NoofDocuments,@Cre_By
			FROM @tblBillingDoc;
	
	SET @AutoList_ID=(select Id from SAPAutoList where [Name]='OrderDocuments')
    INSERT INTO SAPAutomationDownload(Id,InstanceID,AutoList_ID,Cre_By)
			values(@SAPAutoDownloadId,@Instance_id,@AutoList_ID,@Cre_By)

	INSERT OrderDocuments(FileUploadID,ControllingArea,CCbilled,Plant,DocCategory,NoofDocuments,Cre_By)
	SELECT @SAPAutoDownloadId, ControllingArea,CCbilled,Plant,DocCategory,NoofDocuments,@Cre_By
	FROM @tblOrderDoc;

	SET @AutoList_ID=(select Id from SAPAutoList where [Name]='MaterialityScore')
    INSERT INTO SAPAutomationDownload(Id,InstanceID,AutoList_ID,Cre_By)
			values(@SAPAutoDownloadId,@Instance_id,@AutoList_ID,@Cre_By)

	INSERT MaterialityScore(FileUploadID,[FunctionalArea],[Count],[Percentage],Cre_By)
	SELECT @SAPAutoDownloadId,[FunctionalArea],[Count],[Percentage],@Cre_By
	FROM @tblMatScore;

	SET @AutoList_ID=(select Id from SAPAutoList where [Name]='RFCFM')
    INSERT INTO SAPAutomationDownload(Id,InstanceID,AutoList_ID,Cre_By)
	values(@SAPAutoDownloadId,@Instance_id,@AutoList_ID,@Cre_By)
				
	INSERT SAPInput_RFCFM(FileUploadID,Parameter,[Value],Cre_By)
	SELECT @SAPAutoDownloadId, [Parameter],[Value],@Cre_By
	FROM @tblRFCFM;

	INSERT SAPInput_INSTALLEDSOFTWARECOMPONENT(FileUploadID,Component,Release,SP_Level,Comp_Type,[Description],Cre_By)
	SELECT @SAPAutoDownloadId, Component,Release,SP_Level,Comp_Type,[Description],@Cre_By
	FROM @tblSOFTWARECOMPONENT;

	INSERT SAPInput_INSTALLEDPRODUCTVERSIONS(FileUploadID,Product,Release,SP_Stack,Vendor,[Description],Cre_By)
	SELECT @SAPAutoDownloadId,Product,Release,SP_Stack,Vendor,[Description],@Cre_By
	FROM @tblPRODUCTVERSIONS;

	INSERT SAPInput_SFW5REPORT(FileUploadID,[Group],BF_Name,BF_Description,Dependency,SW_Component,SW_Release,Cre_By)
	SELECT @SAPAutoDownloadId,[Group],BF_Name,BF_Description,Dependency,SW_Component,SW_Release,@Cre_By
	FROM @tblSFW5REPORT;

	END

	EXEC SP_Mail @Type='InstanceCreate',@UserID=@User,@Q_Mail= @EmailTo,@InstanceID=@Instance_id ,@Cre_By=@Cre_By
	END
	
	IF @Type='UpdateInstance'
	BEGIN
	Update Instance SET InstaceName=@InstaceName,Project_ID=@Project_ID, [Description]=@Description,
	Modified_On=GETUTCDATE(),Modified_by=@ModifiedBy Where Instance_id=@Id
	IF @Sys_Landscape='Automation'
	BEGIN
	UPDATE SAPConnection set DestinationName=@DestinationName, AppServerHost=@AppServerHost,SystemNumber=@SystemNumber,SAPRouter=@SAPRouter,SAPUser=@SAPUser,SAPPassword=@SAPPassword,Client=@Client
	where Instance_Id=@Id
	END
	END
	
	IF @Type='Delete'
	BEGIN
	 Declare @Status int

	 set @Status=(select Count(PM.InstanceID) from Instance I join ProjectMonitor PM on I.Instance_id=PM.InstanceID where I.isActive=1 and PM.isActive=1 and
	 I.Instance_id=@Id and PM.StatusId In (2,4,5))

	 IF @Status=0
	 BEGIN
	 Update Instance SET IsDeleted=@isDelete, isActive=@isActive Where Instance_id=@Id
	 Update ProjectMonitor set isActive=0, IsDeleted=1 where InstanceID=@Id
	 END	
	 Else
	 Begin
	 SELECT CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER)
	 End
	
	END

	IF @Type='GetProjectByCustomer'
	BEGIN
	--Select Distinct * from Project where Customer_Id= and isActive=1 order by Project_Name
	Select Distinct * from Project where Customer_Id=@Id and
Project_Id in(select Distinct Project_ID from Instance where Instance_id in 
(select Distinct InstanceID  from ProjectMonitor where UserId=@LoginID)) and isActive=1 
   order by Project_Name
	END

	IF @Type='GetInstanceByProject'
	BEGIN
	Select Distinct * from Instance I
join Project P On I.Project_Id=P.Project_ID
join Customer c On C.Customer_ID=P.Customer_Id
where I.isActive=1 and I.Project_ID=@Id order by InstaceName
	END

	
	IF @Type='InstanceClone'
	BEGIN
	Declare @MemberId uniqueidentifier
	DECLARE @uid uniqueidentifier 
	SET @uid  = newid()
	    Insert into Instance (Instance_id,InstaceName,Project_ID,LastUpdated_Dt,Cre_By,Description) 
		  values (@uid,@InstaceName,@Project_ID,GETUTCDATE(),@Cre_By,@Description)
		DECLARE @userid uniqueidentifier = '00000000-0000-0000-0000-000000000000'
		DECLARE @innercount int=null
		--@Userid uniqueidentifier=null
		IF OBJECT_ID(N'tempdb..#temp_ProjectMonitor') IS NOT NULL
		BEGIN
		DROP TABLE #temp_ProjectMonitor
		END
		
		set @innercount=(select COUNT(*) from ProjectMonitor where InstanceID=@Previous_Instance)
		 if @innercount>0
		 BEGIN
			select * into #temp_ProjectMonitor from ProjectMonitor where InstanceID=@Previous_Instance
	
		INSERT ProjectMonitor( 
		[Id]
      ,[ActivityID]
      ,[InstanceID]
      ,[PhaseId]
	  ,RoleId
      ,[UserID]
      ,[StatusId]
	  ,Planed__St_Date
	  ,Planed__En_Date
	  ,Actual_St_Date
	  ,Actual_En_Date
	  ,EST_hours
      ,[Cre_By]
	  )
	 SELECT
	   NEWID()
	  ,[ActivityID]
	  ,@uid
	  ,[PhaseID]
	  ,RoleID
	  ,(SELECT
		CASE
		WHEN (select count(*) from UserMaster u where u.isActive=1 AND u.UserId=TP.UserId)=1 THEN UserId
		ELSE @userid
		END AS UserId)
		--,'00000000-0000-0000-0000-000000000000'
	  ,(select id from StatusMaster WHERE StatusName='Yet To Start')
	  ,GETUTCDATE()
	  ,GETUTCDATE()
	  ,GETUTCDATE()
	  ,GETUTCDATE()
	  ,[EST_hours]
	  ,@Cre_By
	  FROM #temp_ProjectMonitor TP where isactive=1

		 END
	END

	IF @Type='SAPConnectionData'
	BEGIN
	select * from SAPConnection where Instance_Id=@InstanceID
	END

	IF @Type='GetSysLandScape'
	BEGIN
	select Sys_Landscape from Instance where Instance_id=@InstanceID
	END

	IF @Type='GetSAPFileUploadStatus'
	BEGIN
	select * from Instance where PreConvertionIsActive=1 and Instance_id=@InstanceID and isActive=1
	END

	IF @Type='SetSapTables'
	BEGIN
	Declare @AllAutoList_Id varchar(50)=''
	 DECLARE @SAPAutoDownloadId_ uniqueidentifier =NewId(),@AutoList_ID_ int
	SET @AutoList_ID_=(select Id from SAPAutoList where [Name]='SalesDocuments')

	--Update SAPAutomationDownload set isActive=0,isDeleted=1, Modified_by=@Cre_By, Modified_on=GETDATE() 
	--where InstanceID=@Id and AutoList_ID=@AutoList_ID_ and Id!=@SAPAutoDownloadId_

    INSERT INTO SAPAutomationDownload(Id,InstanceID,AutoList_ID,Cre_By)
			values(@SAPAutoDownloadId_,@Id,@AutoList_ID_,@Cre_By)
     
	 INSERT SalesDocuments(FileUploadID,CCbilled,SalesOrg,DistChannel,Division,Plant,NoofDocuments,Cre_By)
			SELECT @SAPAutoDownloadId_, CCbilled,SalesOrg,DistChannel,Division,Plant,NoofDocuments,@Cre_By
			FROM @tblSalesDoc;

	SET @AllAutoList_Id=@AllAutoList_Id + Convert(varchar(20),@AutoList_ID_)
	
	SET @AutoList_ID_=(select Id from SAPAutoList where [Name]='BillingDocuments')
	
    INSERT INTO SAPAutomationDownload(Id,InstanceID,AutoList_ID,Cre_By)
			values(@SAPAutoDownloadId_,@Id,@AutoList_ID_,@Cre_By)
			
	INSERT BillingDocuments(FileUploadID,CompanyCode,Currency,FKREL,BillingCreatedby,NoofDocuments,Cre_By)
			SELECT @SAPAutoDownloadId_, CompanyCode,Currency,FKREL,BillingCreatedby,NoofDocuments,@Cre_By
			FROM @tblBillingDoc;
	SET @AllAutoList_Id=@AllAutoList_Id +','+ Convert(varchar(20),@AutoList_ID_)
	
	SET @AutoList_ID_=(select Id from SAPAutoList where [Name]='OrderDocuments')
	
    INSERT INTO SAPAutomationDownload(Id,InstanceID,AutoList_ID,Cre_By)
			values(@SAPAutoDownloadId_,@Id,@AutoList_ID_,@Cre_By)

	INSERT OrderDocuments(FileUploadID,ControllingArea,CCbilled,Plant,DocCategory,NoofDocuments,Cre_By)
	SELECT @SAPAutoDownloadId_, ControllingArea,CCbilled,Plant,DocCategory,NoofDocuments,@Cre_By
	FROM @tblOrderDoc;
	
	SET @AllAutoList_Id=@AllAutoList_Id +','+ Convert(varchar(20),@AutoList_ID_)

	SET @AutoList_ID_=(select Id from SAPAutoList where [Name]='MaterialityScore')
	
    INSERT INTO SAPAutomationDownload(Id,InstanceID,AutoList_ID,Cre_By)
			values(@SAPAutoDownloadId_,@Id,@AutoList_ID_,@Cre_By)

	INSERT MaterialityScore(FileUploadID,[FunctionalArea],[Count],[Percentage],Cre_By)
	SELECT @SAPAutoDownloadId_,[FunctionalArea],[Count],[Percentage],@Cre_By
	FROM @tblMatScore;

	SET @AllAutoList_Id=@AllAutoList_Id +','+ Convert(varchar(20),@AutoList_ID_)

	SET @AutoList_ID_=(select Id from SAPAutoList where [Name]='RFCFM')
	
    INSERT INTO SAPAutomationDownload(Id,InstanceID,AutoList_ID,Cre_By)
	values(@SAPAutoDownloadId_,@Id,@AutoList_ID_,@Cre_By)
				
	INSERT SAPInput_RFCFM(FileUploadID,Parameter,[Value],Cre_By)
	SELECT @SAPAutoDownloadId_, [Parameter],[Value],@Cre_By
	FROM @tblRFCFM;

	INSERT SAPInput_INSTALLEDSOFTWARECOMPONENT(FileUploadID,Component,Release,SP_Level,Comp_Type,[Description],Cre_By)
	SELECT @SAPAutoDownloadId_, Component,Release,SP_Level,Comp_Type,[Description],@Cre_By
	FROM @tblSOFTWARECOMPONENT;

	INSERT SAPInput_INSTALLEDPRODUCTVERSIONS(FileUploadID,Product,Release,SP_Stack,Vendor,[Description],Cre_By)
	SELECT @SAPAutoDownloadId_,Product,Release,SP_Stack,Vendor,[Description],@Cre_By
	FROM @tblPRODUCTVERSIONS;

	INSERT SAPInput_SFW5REPORT(FileUploadID,[Group],BF_Name,BF_Description,Dependency,SW_Component,SW_Release,Cre_By)
	SELECT @SAPAutoDownloadId_,[Group],BF_Name,BF_Description,Dependency,SW_Component,SW_Release,@Cre_By
	FROM @tblSFW5REPORT;

	Update SAPAutomationDownload set isActive=0,isDeleted=1, Modified_by=@Cre_By, Modified_on=GETDATE() 
	where InstanceID=@Id and AutoList_ID in (select Id from SAPAutoList) and Id!=@SAPAutoDownloadId_
	END
END

--select * from Instance where InstaceName='C11'
--select * from ProjectMonitor where InstanceID='D7A8A260-8E42-4E13-B999-3F940D90C831'
--select * from UserMaster
--select 
--case when userid ='' then '00000000-0000-0000-0000-000000000000'
--else userid
--end AS UserId
--from UserMaster where UserId=(select UserId from #temp_ProjectMonitor) and isActive=1




