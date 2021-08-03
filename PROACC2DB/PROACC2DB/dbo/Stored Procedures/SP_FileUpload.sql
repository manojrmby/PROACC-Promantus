
CREATE PROCEDURE [dbo].[SP_FileUpload]
@Type varchar(50),
@FileUploadID uniqueidentifier=null,
@InstanceId uniqueidentifier=null,
@File_Type varchar(50)=null,
@FileName uniqueidentifier=null,
@Createdby uniqueidentifier=null,
@isActive bit=null,
@Parameter varchar(500)=null,
@Value varchar(500)=null,

@tblActivities UploadActivitiesType READONLY,
@tblCustomCode UploadCustomCodeType READONLY,
@tblFioriApps UploadFioriAppsType READONLY,
@tblSimplification UploadSimplificationType READONLY,
@tblPreConvertion UploadSAPPreConvertionType READONLY,
@tblBwExtractors UploadBwextractors READONLY,

@tblUserlist UploadUserlistType READONLY,
@tblRFCFM UploadRFCFMType READONLY,
@tblSOFTWARECOMPONENT UploadINSTALLEDSOFTWARECOMPONENT READONLY,
@tblPRODUCTVERSIONS UploadINSTALLEDPRODUCTVERSIONS READONLY,
@tblSFW5REPORT UploadSFW5REPORT READONLY,
@tblBillingDocument UploadBillingDocument READONLY,
@tblOrderDocument UploadOrderDocument READONLY,
@tblSalesDocument UploadSalesDocument READONLY,
@tblComplexityAnalysis UploadComplexityAnalysis READONLY,
@tblSAPUserList UploadSAPUserList READONLY,
@tblMaterialityScore UploadMaterialityScore READONLY,
@tblUploadActivityMaster UploadActivityMaster READONLY,
@tblHanaDatabaseTables UploadHanaDatabaseTables READONLY
AS
BEGIN
	Declare @OLD_ID int
	DECLARE @FMID int
	Declare @FileType int

	IF @Type='UploadPhoto'
	BEGIN 
	set @FileType=(select Id from FileMaster where [File]='Profile')
	INSERT INTO FileUploadMaster (Id,InstanceID,File_Type,_FileName,FileType,isActive,Cre_on,Cre_By)
				values(@FileUploadID,@InstanceId,@File_Type,@FileName,@FileType,@isActive,SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30'),@Createdby)
	END
	IF @Type='GetFullPath'
	BEGIN
		select Cre_on, Cre_By ,_FileName from FileUploadMaster where isActive=1 and Cre_By=@Createdby  order by Cre_on desc
	END
	IF @Type='up_Activities'
	BEGIN
	
		BEGIN Transaction T
			BEGIN TRY
			
			SET NOCOUNT ON;
			set @FileType=(select Id from FileMaster where [File]='Activities')
			INSERT INTO FileUploadMaster (id,instanceId,File_Type,_FileName,FileType,Cre_By)
			values(@FileUploadID,@instanceId,@File_Type,@fileName,@FileType,@Createdby)
				
			INSERT SAPInput_Activities(FileUploadID,[Related Simplification Items],[Activities],[Phase],[Condition],[Additional Information])
			SELECT @FileUploadID, [Related Simplification Items],[Activities],[Phase],[Condition],[Additional Information]
			FROM    @tblActivities;

			--Update instance set AssessmentUploadStatus=1,Modified_On=GETUTCDATE(),Modified_by=@Createdby WHERE Instance_id=@instanceId 
			commit transaction T
			END TRY
			BEGIN CATCH 
				rollback transaction T
			END CATCH
	END
	IF @Type='up_CustomCode'
	BEGIN
	
		BEGIN Transaction T
			BEGIN TRY
			
			SET NOCOUNT ON;
			set @FileType=(select Id from FileMaster where [File]='CustomCode')
			INSERT INTO FileUploadMaster (id,instanceId,File_Type,_FileName,FileType,Cre_By)
			values(@FileUploadID,@instanceId,@File_Type,@fileName,@FileType,@Createdby)
				
			INSERT SAPInput_CustomCode(FileUploadID,[Custom Code Topic],[Status],[Application Component],[Custom Code Note])
			SELECT @FileUploadID,[Custom Code Topic],[Status],[Application Component],[Custom Code Note]
			FROM    @tblCustomCode;

			--Update instance set AssessmentUploadStatus=1,Modified_On=GETUTCDATE(),Modified_by=@Createdby WHERE Instance_id=@instanceId 
			commit transaction T
			END TRY
			BEGIN CATCH 
				rollback transaction T
			END CATCH
	END
	IF @Type='up_FioriApps'
	BEGIN
	
		BEGIN Transaction T
			BEGIN TRY
			
			SET NOCOUNT ON;
			set @FileType=(select Id from FileMaster where [File]='RecommendedFioriApp')
			INSERT INTO FileUploadMaster (id,instanceId,File_Type,_FileName,FileType,Cre_By)
			values(@FileUploadID,@instanceId,@File_Type,@fileName,@FileType,@Createdby)
				
			INSERT SAPInput_FioriApps(FileUploadID,[Role],[Name],[Application Area],[Description])
			SELECT @FileUploadID,[Role],[Name],[Application Area],[Description]
			FROM    @tblFioriApps;

		--Update instance set AssessmentUploadStatus=1,Modified_On=GETUTCDATE(),Modified_by=@Createdby WHERE Instance_id=@instanceId 
			commit transaction T
			END TRY
			BEGIN CATCH 
				rollback transaction T
			END CATCH
	END
	IF @Type='up_Simplification'
	BEGIN
	
		BEGIN Transaction T
			BEGIN TRY
			
			SET NOCOUNT ON;
			set @FileType=(select Id from FileMaster where [File]='RelevantSimplificationIte')
			INSERT INTO FileUploadMaster (id,instanceId,File_Type,_FileName,FileType,Cre_By)
			values(@FileUploadID,@instanceId,@File_Type,@fileName,@FileType,@Createdby)
				
			INSERT SAPInput_SimplificationReport(FileUploadID,[Title],[Category],[Relevance],[LoB/Technology],[Business Area],[Consistency Status],[Manual Status],[SAP Notes],[Relevance Summary])
			SELECT @FileUploadID,[Title],[Category],[Relevance],[LoB/Technology],[Business Area],[Consistency Status],[Manual Status],[SAP Notes],[Relevance Summary]
			FROM    @tblSimplification;

			--Update instance set AssessmentUploadStatus=1,Modified_On=GETUTCDATE(),Modified_by=@Createdby WHERE Instance_id=@instanceId 
			commit transaction T
			END TRY
			BEGIN CATCH 
				rollback transaction T
			END CATCH
	END
	IF @Type='up_Bwextractors'
	BEGIN
	
		--BEGIN Transaction T
		--	BEGIN TRY
			
			--SET NOCOUNT ON;
			set @FileType=(select Id from FileMaster where [File]='Bwextractors')
			INSERT INTO FileUploadMaster (id,instanceId,File_Type,_FileName,FileType,Cre_By)
			values(@FileUploadID,@instanceId,@File_Type,@fileName,@FileType,@Createdby)
				
			INSERT SAPInput_BWExtractors(FileUploadID,[Extractor Name] ,[Application Area],[Related Simplification Items],[Additional Information],Cre_By)
			SELECT @FileUploadID,[Extractor Name],[Application Area],[Related Simplification Items],[Additional Information],@Createdby
			FROM @tblBwExtractors;

			--commit transaction T
			--END TRY
			--BEGIN CATCH 
			--	rollback transaction T
			--END CATCH
	END
	IF @Type='up_HanaDatabaseTables'
	BEGIN
	
		BEGIN Transaction T
			BEGIN TRY
			
			SET NOCOUNT ON;
			set @FileType=(select Id from FileMaster where [File]='HanaDatabaseTables')
			INSERT INTO FileUploadMaster (id,instanceId,File_Type,_FileName,FileType,Cre_By)
			values(@FileUploadID,@instanceId,@File_Type,@fileName,@FileType,@Createdby)
				
				INSERT SAPInput_HanaDatabaseTables(FileUploadID,[Name],[Store Type],[Data Size in GB],[Number of Records],Cre_By)
		        select @FileUploadID,[Name],[Store Type],[Data Size in GB],[Number of Records],@Createdby
				FROM @tblHanaDatabaseTables
			
			commit transaction T
			END TRY
			BEGIN CATCH 
				rollback transaction T
			END CATCH
	END
	IF @Type='up_SAPReadinessCheck'
	BEGIN
	
		BEGIN Transaction T
			BEGIN TRY
			
			SET NOCOUNT ON;
			set @FileType=(select Id from FileMaster where [File]='SAPReadinessCheck')
			INSERT INTO FileUploadMaster (id,instanceId,File_Type,_FileName,FileType,Cre_By)
			values(@FileUploadID,@instanceId,@File_Type,@fileName,@FileType,@Createdby)
				
			--INSERT SAPInput_SimplificationReport(FileUploadID,[Title],[Category],[Relevance],[LoB/Technology],[Business Area],[Consistency Status],[Manual Status],[SAP Notes],[Relevance Summary])
			--SELECT @FileUploadID,[Title],[Category],[Relevance],[LoB/Technology],[Business Area],[Consistency Status],[Manual Status],[SAP Notes],[Relevance Summary]
			--FROM    @tblSimplification;
			--Update instance set AssessmentUploadStatus=1,Modified_On=GETUTCDATE(),Modified_by=@Createdby WHERE Instance_id=@instanceId 
			commit transaction T
			END TRY
			BEGIN CATCH 
				rollback transaction T
			END CATCH
	END
	IF @Type='up_SAPPreConvertion'
	BEGIN
	
		BEGIN Transaction T
			BEGIN TRY
			
			SET NOCOUNT ON;
			set @FileType=(select Id from FileMaster where [File]='PreConvertion')
			INSERT INTO FileUploadMaster (id,instanceId,File_Type,_FileName,FileType,Cre_By)
			values(@FileUploadID,@instanceId,@File_Type,@fileName,@FileType,@Createdby)
				
			INSERT SAPInput_PreConvertion(
	   [FileUploadID]
      ,[Relevance]
      ,[Last Consistency Result]
      ,[Exemption Possible]
      ,[ID]
      ,[Title]
      ,[Lob/Technology]
      ,[Business Area]
      ,[Catetory]
      ,[Component]
      ,[Status]
      ,[Note]
      ,[Application Area]
      ,[Summary]
		)
	SELECT 
	@FileUploadID,
	[Relevance] ,
	[Last Consistency Result] ,
	[Exemption Possible] ,
	[ID] ,
	[Title] ,
	[Lob/Technology] ,
	[Business Area] ,
	[Catetory] ,
	[Component] ,
	[Status] ,
	[Note] ,
	[Application Area] ,
	[Summary] 
FROM    @tblPreConvertion;

			--Update instance set AssessmentUploadStatus=1,Modified_On=GETUTCDATE(),Modified_by=@Createdby WHERE Instance_id=@instanceId 
			commit transaction T
			END TRY
			BEGIN CATCH 
				rollback transaction T
			END CATCH


	END
	IF @Type='CreateAnalysis_UploadRevert'
	BEGIN
	SET @FileUploadID= (SELECT top 1(Id) from FileUploadMaster WHERE InstanceID=@InstanceID AND isActive=1 AND FileType=(SELECT id FROM FileMaster where [File]='Activities' ))
	 IF	@FileUploadID  IS NOT NULL
		BEGIN
		Delete from SAPInput_Activities where FileUploadID=@FileUploadID
		END 
	IF	@FileUploadID  IS NOT NULL
	 BEGIN
	 SET @FileUploadID= (SELECT top 1(Id) from FileUploadMaster WHERE InstanceID=@InstanceID AND isActive=1 AND FileType=(SELECT id FROM FileMaster where [File]='CustomCode' ))
	Delete from SAPInput_CustomCode where FileUploadID=@FileUploadID
	END
	IF	@FileUploadID  IS NOT NULL
	 BEGIN
	 SET @FileUploadID= (SELECT top 1(Id) from FileUploadMaster WHERE InstanceID=@InstanceID AND isActive=1 AND FileType=(SELECT id FROM FileMaster where [File]='RecommendedFioriApp' ))
	Delete from SAPInput_FioriApps where FileUploadID=@FileUploadID
	END
	IF	@FileUploadID  IS NOT NULL
	BEGIN
	SET @FileUploadID= (SELECT top 1(Id) from FileUploadMaster WHERE InstanceID=@InstanceID AND isActive=1 AND FileType=(SELECT id FROM FileMaster where [File]='RelevantSimplificationIte' ))
	Delete from SAPInput_SimplificationReport where FileUploadID=@FileUploadID
	END

	UPDATE FileUploadMaster set isActive=0 ,IsDeleted=1,Modified_On=GETUTCDATE(),Modified_by=@Createdby WHERE InstanceID=@InstanceID

	UPDATE Instance SET AssessmentUploadStatus=0, Modified_On=GETUTCDATE(),Modified_by=@Createdby WHERE Instance_id=@InstanceId 

	RETURN 1
	END
	If @Type='Confirm_AnalysisUpload'
	BEGIN
		DECLARE @Count int=1
		SET @FileUploadID= (SELECT top 1(Id) from FileUploadMaster WHERE InstanceID=@InstanceID AND isActive=1 AND FileType=(SELECT id FROM FileMaster where [File]='Activities' ))
		IF	@FileUploadID  IS NOT NULL
			BEGIN
			SET @Count=@Count+1
			END
		
		SET @FileUploadID= (SELECT top 1(Id) from FileUploadMaster WHERE InstanceID=@InstanceID AND isActive=1 AND FileType=(SELECT id FROM FileMaster where [File]='Bwextractors' ))
		IF	@FileUploadID  IS NOT NULL
		BEGIN
			SET @Count=@Count+1
		END
		
		SET @FileUploadID= (SELECT top 1(Id) from FileUploadMaster WHERE InstanceID=@InstanceID AND isActive=1 AND FileType=(SELECT id FROM FileMaster where [File]='CustomCode' ))
		IF	@FileUploadID  IS NOT NULL
		BEGIN
			SET @Count=@Count+1
		END
		
		SET @FileUploadID= (SELECT top 1(Id) from FileUploadMaster WHERE InstanceID=@InstanceID AND isActive=1 AND FileType=(SELECT id FROM FileMaster where [File]='HanaDatabaseTables' ))
		IF	@FileUploadID  IS NOT NULL
		BEGIN
			SET @Count=@Count+1
		END
		
		SET @FileUploadID= (SELECT top 1(Id) from FileUploadMaster WHERE InstanceID=@InstanceID AND isActive=1 AND FileType=(SELECT id FROM FileMaster where [File]='RecommendedFioriApp' ))
		IF	@FileUploadID  IS NOT NULL
		BEGIN
			SET @Count=@Count+1
		END
		
		SET @FileUploadID= (SELECT top 1(Id) from FileUploadMaster WHERE InstanceID=@InstanceID AND isActive=1 AND FileType=(SELECT id FROM FileMaster where [File]='RelevantSimplificationItems' ))
		IF	@FileUploadID  IS NOT NULL
		BEGIN
			SET @Count=@Count+1
		END
		
		SET @FileUploadID= (SELECT top 1(Id) from FileUploadMaster WHERE InstanceID=@InstanceID AND isActive=1 AND FileType=(SELECT id FROM FileMaster where [File]='SAPReadinessCheck' ))
		IF	@FileUploadID  IS NOT NULL
		BEGIN
			SET @Count=@Count+1
		END
		
		SET @FileUploadID= (SELECT top 1(Id) from FileUploadMaster WHERE InstanceID=@InstanceID AND isActive=1 AND FileType=(SELECT id FROM FileMaster where [File]='SAPUserList' ))
		IF	@FileUploadID  IS NOT NULL
		BEGIN
			SET @Count=@Count+1
		END
		PRINT @Count
		IF @Count=8
		BEGIN
			update Instance set AssessmentUploadStatus=1 where Instance_id=@InstanceId
		END

	END

	If @Type='Confirm_SAPFileUpload'
	BEGIN
		DECLARE @SAP_Count int=1

		SET @FileUploadID= (SELECT top 1(Id) from FileUploadMaster WHERE InstanceID=@InstanceID AND isActive=1 AND FileType=(SELECT id FROM FileMaster where [File]='RFCFM' ))
		IF	@FileUploadID  IS NOT NULL
			BEGIN
			SET @SAP_Count=@SAP_Count+1
			PRINT @SAP_Count
			END
		SET @FileUploadID= (SELECT top 1(Id) from FileUploadMaster WHERE InstanceID=@InstanceID AND isActive=1 AND FileType=(SELECT id FROM FileMaster where [File]='BillingDocuments' ))
		IF	@FileUploadID  IS NOT NULL
		BEGIN
			SET @SAP_Count=@SAP_Count+1
		END
		SET @FileUploadID= (SELECT top 1(Id) from FileUploadMaster  WHERE InstanceID=@InstanceID AND isActive=1  AND FileType=(SELECT id FROM FileMaster where [File]='OrderDocuments' ))
		IF	@FileUploadID  IS NOT NULL
		BEGIN
			SET @SAP_Count=@SAP_Count+1
		END
		SET @FileUploadID= (SELECT top 1(Id) from FileUploadMaster  WHERE InstanceID=@InstanceID AND isActive=1  AND FileType=(SELECT id FROM FileMaster where [File]='SalesDocuments' ))
		IF	@FileUploadID  IS NOT NULL
		BEGIN
			SET @SAP_Count=@SAP_Count+1
		END
		SET @FileUploadID= (SELECT top 1(Id) from FileUploadMaster  WHERE InstanceID=@InstanceID AND isActive=1  AND FileType=(SELECT id FROM FileMaster where [File]='ComplexityAnalysis' ))
		IF	@FileUploadID  IS NOT NULL
		BEGIN
			SET @SAP_Count=@SAP_Count+1
		END
		
		PRINT @SAP_Count
		IF @SAP_Count=6
		BEGIN
			update Instance set PreConvertionIsActive=1 where Instance_id=@InstanceId
		END

	END

	IF @Type='up_PMupload'
	BEGIN
	BEGIN Transaction T
	BEGIN TRY
		
		set @FMID=(select id from FileMaster where [File]=@File_Type AND isActive=1)
		SET @OLD_ID=(select count(*) from FileUploadMaster where instanceId=@instanceId AND FileType=@FMID AND isActive=1)
		IF @OLD_ID>0
			BEGIN
				UPDATE FileUploadMaster 
				SET instanceId=@instanceId,File_Type=@File_Type,_FileName=@fileName,FileType=@FMID,Cre_By=@Createdby
				WHERE id=(select top 1 id from FileUploadMaster where instanceId=@instanceId AND FileType=@FMID AND isActive=1)
			END
		ELSE 
		BEGIN
			INSERT INTO FileUploadMaster (id,instanceId,File_Type,_FileName,FileType,Cre_By)
			values(@FileUploadID,@instanceId,@File_Type,@fileName,@FMID,@Createdby)	
		END
		
		
	SET NOCOUNT ON;
	commit transaction T
	END TRY
	BEGIN CATCH 
		rollback transaction T
	END CATCH
	END
	IF @Type='GetPMuploadlist'
	BEGIN
		select * from  FileUploadMaster where isActive=1 and InstanceID=@instanceId AND FileType in(SELECT id from FileMaster where Filefor='PM') order by FileType desc
	END
	IF @Type='GetPMFileList'
	BEGIN
	  SELECT * from FileMaster where Filefor='PM' AND isActive=1 AND Id NOT IN (select  FileType from  FileUploadMaster where isActive=1
	and InstanceID=@instanceId 
	AND FileType in(SELECT id from FileMaster where Filefor='PM') ) order by [File] asc
	END

	IF @Type='up_RFCFM'
	BEGIN
	
		BEGIN Transaction T
			BEGIN TRY
			
			SET NOCOUNT ON;
			set @FileType=(select Id from FileMaster where [File]='RFCFM')
			INSERT INTO FileUploadMaster (id,instanceId,File_Type,_FileName,FileType,Cre_By)
			values(@FileUploadID,@instanceId,@File_Type,@fileName,@FileType,@Createdby)
				
			INSERT SAPInput_RFCFM(FileUploadID,Parameter,[Value],Cre_By)
			SELECT @FileUploadID, [Parameter],[Value],@Createdby
			FROM @tblRFCFM;

			INSERT SAPInput_INSTALLEDSOFTWARECOMPONENT(FileUploadID,Component,Release,SP_Level,Comp_Type,[Description],Cre_By)
			SELECT @FileUploadID, Component,Release,SP_Level,Comp_Type,[Description],@Createdby
			FROM @tblSOFTWARECOMPONENT;

			INSERT SAPInput_INSTALLEDPRODUCTVERSIONS(FileUploadID,Product,Release,SP_Stack,Vendor,[Description],Cre_By)
			SELECT @FileUploadID,Product,Release,SP_Stack,Vendor,[Description],@Createdby
			FROM @tblPRODUCTVERSIONS;

			INSERT SAPInput_SFW5REPORT(FileUploadID,[Group],BF_Name,BF_Description,Dependency,SW_Component,SW_Release,Cre_By)
			SELECT @FileUploadID,[Group],BF_Name,BF_Description,Dependency,SW_Component,SW_Release,@Createdby
			FROM @tblSFW5REPORT;

			
			commit transaction T
			END TRY
			BEGIN CATCH 
				rollback transaction T
			END CATCH
	END

	IF @Type='up_BillingDocuments'
	BEGIN
	
		BEGIN Transaction T
			BEGIN TRY
			
			SET NOCOUNT ON;
			set @FileType=(select Id from FileMaster where [File]='BillingDocuments')
			INSERT INTO FileUploadMaster (id,instanceId,File_Type,_FileName,FileType,Cre_By)
			values(@FileUploadID,@instanceId,@File_Type,@fileName,@FileType,@Createdby)
				
			INSERT BillingDocuments(FileUploadID,CompanyCode,Currency,FKREL,BillingCreatedby,NoofDocuments,Cre_By)
			SELECT @FileUploadID, CompanyCode,Currency,FKREL,BillingCreatedby,NoofDocuments,@Createdby
			FROM @tblBillingDocument;

			commit transaction T
			END TRY
			BEGIN CATCH 
				rollback transaction T
			END CATCH
	END
	IF @Type='up_OrderDocuments'
	BEGIN
	
		BEGIN Transaction T
			BEGIN TRY
			
			SET NOCOUNT ON;
			set @FileType=(select Id from FileMaster where [File]='OrderDocuments')
			INSERT INTO FileUploadMaster (id,instanceId,File_Type,_FileName,FileType,Cre_By)
			values(@FileUploadID,@instanceId,@File_Type,@fileName,@FileType,@Createdby)
				
			INSERT OrderDocuments(FileUploadID,ControllingArea,CCbilled,Plant,DocCategory,NoofDocuments,Cre_By)
			SELECT @FileUploadID, ControllingArea,CCbilled,Plant,DocCategory,NoofDocuments,@Createdby
			FROM @tblOrderDocument;

			commit transaction T
			END TRY
			BEGIN CATCH 
				rollback transaction T
			END CATCH
	END
	IF @Type='up_SalesDocuments'
	BEGIN
	
		BEGIN Transaction T
			BEGIN TRY
			
			SET NOCOUNT ON;
			set @FileType=(select Id from FileMaster where [File]='SalesDocuments')
			INSERT INTO FileUploadMaster (id,instanceId,File_Type,_FileName,FileType,Cre_By)
			values(@FileUploadID,@instanceId,@File_Type,@fileName,@FileType,@Createdby)
				
			INSERT SalesDocuments(FileUploadID,CCbilled,SalesOrg,DistChannel,Division,Plant,NoofDocuments,Cre_By)
			SELECT @FileUploadID, CCbilled,SalesOrg,DistChannel,Division,Plant,NoofDocuments,@Createdby
			FROM @tblSalesDocument;

			commit transaction T
			END TRY
			BEGIN CATCH 
				rollback transaction T
			END CATCH
	END

	IF @Type='up_ComplexityAnalysis'
	BEGIN
	
		BEGIN Transaction T
			BEGIN TRY
			
			SET NOCOUNT ON;
			set @FileType=(select Id from FileMaster where [File]='ComplexityAnalysis')
			INSERT INTO FileUploadMaster (id,instanceId,File_Type,_FileName,FileType,Cre_By)
			values(@FileUploadID,@instanceId,@File_Type,@fileName,@FileType,@Createdby)
				
			INSERT ComplexityAnalysis(FileUploadID,CompanyCode,NewGL,Leadingledger,SpecialPurposeLedger,Treasury_SWF5_FSCM_CLM,
			Treasury_SWF5_FSCM_BNK,Multicurrencymodel,NewAssetAccounting,BusinessPartner,BPActive,MaterialLedger,Active
			)
			SELECT @FileUploadID, CompanyCode,NewGL,Leadingledger,SpecialPurposeLedger,Treasury_SWF5_FSCM_CLM,
			Treasury_SWF5_FSCM_BNK,Multicurrencymodel,NewAssetAccounting,BusinessPartner,BPActive,MaterialLedger,Active
			FROM @tblComplexityAnalysis;

			commit transaction T
			END TRY
			BEGIN CATCH 
				rollback transaction T
			END CATCH
	END
	
	IF @Type='up_UserList'
	BEGIN
	
		--BEGIN Transaction T
		--	BEGIN TRY
			
		--	SET NOCOUNT ON;
			set @FileType=(select Id from FileMaster where [File]='SAPUserList')
			INSERT INTO FileUploadMaster (id,instanceId,File_Type,_FileName,FileType,Cre_By)
			values(@FileUploadID,@instanceId,@File_Type,@fileName,@FileType,@Createdby)				
			INSERT SAPUserList(FileUploadID,[User],UserType,Valid_from,Valid_through,Last_Logon,[System],Catergory,Cre_By
			)
			
			SELECT @FileUploadID, [User],UserType, NULLIF(CONVERT(varchar,CONVERT(datetime, Valid_from),101),''), NULLIF(CONVERT(varchar,CONVERT(datetime, Valid_through),101),''),Last_Logon,[System],Catergory,@Createdby
			FROM @tblSAPUserList;

			--SELECT @FileUploadID, [User],UserType,
			--NULLIF(CONVERT(varchar,Format(cast(Valid_from as datetime),'dd-mm-yyyy hh:mm:ss','en-us')),''),
			--NULLIF(CONVERT(varchar,Format(cast(Valid_through as datetime),'dd-mm-yyyy hh:mm:ss','en-us')),''),
			--Last_Logon,[System],Catergory
			--FROM @tblSAPUserList;

			--commit transaction T
			--END TRY
			--BEGIN CATCH 
			--	rollback transaction T
			--END CATCH
	END

	IF @Type='up_MaterialityScore'
	BEGIN
	
		--BEGIN Transaction T
		--	BEGIN TRY
			
		--	SET NOCOUNT ON;
			
			set @FileType=(select Id from FileMaster where [File]='MaterialityScore')
			INSERT INTO FileUploadMaster (id,instanceId,File_Type,_FileName,FileType,Cre_By)
			values(@FileUploadID,@instanceId,@File_Type,@fileName,@FileType,@Createdby)
				
			INSERT MaterialityScore(FileUploadID,[FunctionalArea],[Count],[Percentage],Cre_By
			)
			SELECT @FileUploadID,[FunctionalArea],[Count],[Percentage],@Createdby
			FROM @tblMaterialityScore;

			--commit transaction T
			--END TRY
			--BEGIN CATCH 
			--	rollback transaction T
			--END CATCH
	END

	IF @Type='up_ActivityFileUpload'
	BEGIN
	--BEGIN Transaction T
	--		BEGIN TRY
			
	--		SET NOCOUNT ON;
				set @FileType=(select Id from FileMaster where [File]='Activities')
				INSERT INTO FileUploadMaster (Id,InstanceID,File_Type,_FileName,FileType,isActive,Cre_on,Cre_By)
				values(@FileUploadID,'00000000-0000-0000-0000-000000000000',@File_Type,@FileName,@FileType,1,SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30'),@Createdby)
				
				EXEC SP_ActivityUpload @Type='ActivityUpload',@ActivityMaster=@tblUploadActivityMaster,@FileName=@FileName,@Createdby=@Createdby
				


		--commit transaction T
		--	END TRY
		--	BEGIN CATCH 
		--		rollback transaction T
		--	END CATCH		

	
	END
END
