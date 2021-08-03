


CREATE PROCEDURE [dbo].[SP_Mail]
	@Type varchar(50)=null,
	@PhaseID int =null,
	@InstanceID uniqueidentifier=null,
    @ProjectID uniqueidentifier=null,
	@PM_ID uniqueidentifier =null,
	@Q_Subject varchar(100)=null,
	@FileName_ID uniqueidentifier =null,
	@Running_ID int=null,
	@Cre_By uniqueidentifier=null,
	@UserID uniqueidentifier=null, 
	@Issuetrack_Id uniqueidentifier=null, 
	@Q_Mail varchar(100)=null,
	@Q_UserID int=null,
	@Q_Name varchar(max)=null
AS
BEGIN

DECLARE @SQ_ID int
DECLARE @Next_PM_ID uniqueidentifier
DECLARE @New_InstanceID uniqueidentifier
DECLARE @New_UserID uniqueidentifier
DECLARE @ProjectManagerID uniqueidentifier

DECLARE @PM_Mail varchar(100)
DECLARE @Con_Mail varchar(100)
Declare @Body_PM varchar(Max)
Declare @Body_CON varchar(Max)

DECLARE @Counter INT 
DECLARE @MaxCounter INT 
DECLARE @New_TempId uniqueidentifier
DeCLARE @LocalID INT
DECLARE @statusId int

DECLARE @PM_LINEID uniqueidentifier
DECLARE @UserID_V uniqueidentifier
DECLARE @MailID varchar(100)

DECLARE @Mail_Template_ID1 int
DECLARE @Mail_Template_ID2 int

 IF @Type='ProjectMonitor'
	BEGIN
		SET @statusId=(SELECT StatusId from ProjectMonitor WHERE id=@PM_ID)
		print @statusId
		IF @statusId=1--Completed
		BEGIN
			SELECT @New_InstanceID=InstanceID from ProjectMonitor WHERE id=@PM_ID
			SET @ProjectManagerID=(
						Select  P.ProjectManager_Id from Instance i 
						join Project p on p.Project_Id=i.Project_ID
						WHERE Instance_id=@New_InstanceID)
			SET @PM_Mail=(
					Select EMail from UserMaster where [UserId] =@ProjectManagerID)

			IF OBJECT_ID(N'tempdb..#TempTable') IS NOT NULL
			BEGIN
			DROP TABLE #TempTable
			END	

			SELECT IDENTITY(int, 1, 1) AS 'RowID',
			P.Id As PM_ID,P.PhaseId As PM_Phase,
			P.StatusId,A.Activity_ID As [AM_ActivityId],
			A.PhaseID As[AM_PhaseId],A.Sequence_Num
			,LAG(P.Id) OVER ( ORDER BY Sequence_Num ) AS PreviousID
			,LEAD(P.Id) OVER ( ORDER BY Sequence_Num ) AS NextID
			INTO #TempTable
			from   ProjectMonitor P 
			LEFT JOIN ActivityMaster A On A.Activity_ID=P.ActivityID
			WHERE  InstanceID=@InstanceID AND P.isActive=1 AND A.Sequence_Num IS NOT NULL 
			AND
			P.Statusid !=3 AND P.StatusId!=4 AND P.StatusId!=1
			order by A.Sequence_Num

			
			SELECT  @MaxCounter=count(*) from #TempTable
			Select @Next_PM_ID= NextID from #TempTable  Where PM_ID=@PM_ID
			select @Counter=RowID from #TempTable WHERE PM_ID=@Next_PM_ID
			WHILE ( @Counter <= @MaxCounter)
			BEGIN
			
			Select @LocalID=StatusId from #TempTable Where RowID=@Counter
			IF @LocalID=5 OR @LocalID=2
				BEGIN 
					Select @Next_PM_ID=PM_ID from #TempTable Where RowID=@Counter 
					BREAK
				END
			SET @Counter  = @Counter  + 1
			END

			
			SELECT @New_UserID=UserID from ProjectMonitor WHERE id=@Next_PM_ID

			SET @Con_Mail=(
			Select EMail from UserMaster where [UserId] =@New_UserID)

			

			SELECT  @Body_PM=
			A.Task +
			','+Ph.PhaseName+
			','+i.InstaceName+
			','+pr.Project_Name
			from ProjectMonitor P
			LEFT JOIN ActivityMaster A ON A.Activity_ID=P.ActivityID
			LEFT JOIN PhaseMaster Ph ON A.PhaseID=Ph.Id
			left join instance i on p.InstanceID=i.Instance_id
			left join project pr on pr.Project_Id=i.Project_ID
			Where P.id=@PM_ID

			SELECT  @Body_CON=
				A.Task +
			','+Ph.PhaseName+
			','+i.InstaceName+
			','+pr.Project_Name
			from ProjectMonitor P
			LEFT JOIN ActivityMaster A ON A.Activity_ID=P.ActivityID
			LEFT JOIN PhaseMaster Ph ON A.PhaseID=Ph.Id
			left join instance i on p.InstanceID=i.Instance_id
			left join project pr on pr.Project_Id=i.Project_ID
			Where P.id=@Next_PM_ID

			SET @Mail_Template_ID1=(select id from MailTemplate where TemplateName='Completed')
			SET @Mail_Template_ID2=(select id from MailTemplate where TemplateName='Next Task ')

			INSERT INTO [dbo].[MailMaster]
			   ([UserID]
			   ,[To]
			   ,[Subject]
			   ,[Body]
			   ,[MailStatus]
			   ,[Type]
			   ,LineID
			   ,TemplateID
			   ,[Cre_By]
			   )
		 VALUES 
		 (@ProjectManagerID,@PM_Mail,'Auto Mail-Task Completed',@Body_PM,0,'ProjectMonitor',@PM_ID,@Mail_Template_ID1,@Cre_By),
		 (@New_UserID,@Con_Mail,'Auto Mail-Next Task',@Body_CON,0,'ProjectMonitor',@PM_ID,@Mail_Template_ID2,@Cre_By)
		END
		ELSE IF @statusId= 3 OR @statusId= 4--Not Applicable,On Hold
		BEGIN
			IF OBJECT_ID(N'tempdb..#TempTable1') IS NOT NULL
			BEGIN
			DROP TABLE #TempTable1
			END	

			SELECT IDENTITY(int, 1, 1) AS 'RowID',
			P.Id As PM_ID,P.PhaseId As PM_Phase,
			P.StatusId,A.Activity_ID As [AM_ActivityId],
			A.PhaseID As[AM_PhaseId],A.Sequence_Num
			,LAG(P.Id) OVER ( ORDER BY Sequence_Num ) AS PreviousID
			,LEAD(P.Id) OVER ( ORDER BY Sequence_Num ) AS NextID
			INTO #TempTable1
			from   ProjectMonitor P LEFT JOIN ActivityMaster A On A.Activity_ID=P.ActivityID
			WHERE  InstanceID=@InstanceID AND P.isActive=1 AND A.Sequence_Num IS NOT NULL 
			--AND
			--P.Statusid !=3 AND P.StatusId!=4
			order by A.Sequence_Num
			SELECT  @MaxCounter=count(*) from #TempTable1
			Select @Next_PM_ID= NextID from #TempTable1  Where PM_ID=@PM_ID
			select @Counter=RowID from #TempTable1 WHERE PM_ID=@Next_PM_ID
			WHILE ( @Counter <= @MaxCounter)
			BEGIN
			Select @LocalID=StatusId from #TempTable1 Where RowID=@Counter
			IF @LocalID=5 OR @LocalID=2
				BEGIN 
					Select @Next_PM_ID=PM_ID from #TempTable1 Where RowID=@Counter 
					BREAK
				END
			SET @Counter  = @Counter  + 1
			END
			
			SELECT @New_UserID=UserID from ProjectMonitor WHERE id=@Next_PM_ID

			SET @Con_Mail=(
			Select EMail from UserMaster where [UserId] =@New_UserID)

			

			SELECT  @Body_PM=
			A.Task +
			','+Ph.PhaseName
			from ProjectMonitor P
			LEFT JOIN ActivityMaster A ON A.Activity_ID=P.ActivityID
			LEFT JOIN PhaseMaster Ph ON A.PhaseID=Ph.Id
			Where P.id=@PM_ID

			--SELECT  @Body_CON=
			--	A.Task +
			--','+Ph.PhaseName
			--from ProjectMonitor P
			--LEFT JOIN ActivityMaster A ON A.Activity_ID=P.ActivityID
			--LEFT JOIN PhaseMaster Ph ON A.PhaseID=Ph.Id
			--Where P.id=@Next_PM_ID

			SELECT  @Body_CON=
				A.Task +
			','+Ph.PhaseName+
			','+i.InstaceName+
			','+pr.Project_Name
			from ProjectMonitor P
			LEFT JOIN ActivityMaster A ON A.Activity_ID=P.ActivityID
			LEFT JOIN PhaseMaster Ph ON A.PhaseID=Ph.Id
			left join instance i on p.InstanceID=i.Instance_id
			left join project pr on pr.Project_Id=i.Project_ID
			Where P.id=@Next_PM_ID
			
			SET @Mail_Template_ID2=(select id from MailTemplate where TemplateName='Next Task ')

			INSERT INTO [dbo].[MailMaster]
			   ([UserID]
			   ,[To]
			   ,[Subject]
			   ,[Body]
			   ,[MailStatus]
			   ,[Type]
			   ,LineID
			   ,TemplateID
			   ,[Cre_By]
			   )
		 VALUES 
		 (@New_UserID,@Con_Mail,'Auto Mail-Next Task',@Body_CON,0,'ProjectMonitor',@PM_ID,@Mail_Template_ID2,@Cre_By)
		END
	END
 IF @Type='MailList'
 BEGIN
	  select U.[Name],Running_ID,[To],M.[Subject] ,T.[FileName],M.Body,M.Q_UserID,M.LineID
	  from MailMaster M
	  Left JOIN MailTemplate T on M.TemplateID=T.id 
	  LEFT JOIN UserMaster U on U.UserId=M.UserID
	  where [TO]IS NOT NULL AND MailStatus=0 AND M.isActive=1
 END
 IF @Type='UpdateMailList'
 BEGIN
	Update MailMaster SET MailStatus=1,Modified_On=GETUTCDATE() Where Running_ID=@Running_ID 
	 
 END
 IF @Type='AddFirstMail'
 BEGIN
		select TOP 1 @PM_LINEID=P.Id
		from ProjectMonitor P
		INNER JOIN ActivityMaster A on A.Activity_ID=P.ActivityID
		where InstanceID=@InstanceID ORDER by Sequence_Num
		IF @PM_LINEID=@PM_ID
		BEGIN
		  SELECT @UserID_V=Userid from ProjectMonitor WHERE id=@PM_LINEID
		  IF @UserID_V!='00000000-0000-0000-0000-000000000000'
		  BEGIN
		  SET @MailID=(Select EMail from UserMaster where [UserId] =@UserID_V) 

		  SELECT  @Body_CON=
				A.Task +
			','+Ph.PhaseName+
			','+i.InstaceName+
			','+pr.Project_Name
			from ProjectMonitor P
			LEFT JOIN ActivityMaster A ON A.Activity_ID=P.ActivityID
			LEFT JOIN PhaseMaster Ph ON A.PhaseID=Ph.Id
			left join instance i on p.InstanceID=i.Instance_id
			left join project pr on pr.Project_Id=i.Project_ID
			Where P.id=@PM_LINEID

			SET @Mail_Template_ID1=(select id from MailTemplate where TemplateName='First Task')

		  INSERT INTO [dbo].[MailMaster]
			   ([UserID]
			   ,[To]
			   ,[Subject]
			   ,[Body]
			   ,[MailStatus]
			   ,[Type]
			   ,LineID
			   ,TemplateID
			   ,[Cre_By]
			   )
		 VALUES 
		 (@UserID_V,@MailID,'Auto Mail-Task New Task',@Body_CON,0,'Resource Allocation',@PM_ID,@Mail_Template_ID1,@Cre_By)
		  END
		END
 END
 IF @Type='ReleaseHoldAddMail'
 BEGIN
		  SELECT @UserID_V=Userid from ProjectMonitor WHERE id=@PM_ID
		  IF @UserID_V!='00000000-0000-0000-0000-000000000000'
		  BEGIN
		  SET @MailID=(Select EMail from UserMaster where [UserId] =@UserID_V) 
		  SELECT  @Body_CON=
				A.Task +
			','+Ph.PhaseName+
			','+i.InstaceName+
			','+pr.Project_Name
			from ProjectMonitor P
			LEFT JOIN ActivityMaster A ON A.Activity_ID=P.ActivityID
			LEFT JOIN PhaseMaster Ph ON A.PhaseID=Ph.Id
			left join instance i on p.InstanceID=i.Instance_id
			left join project pr on pr.Project_Id=i.Project_ID
			Where P.id=@PM_ID

			SET @Mail_Template_ID1=(select id from MailTemplate where TemplateName='Hold Released')

		  INSERT INTO [dbo].[MailMaster]
			   ([UserID]
			   ,[To]
			   ,[Subject]
			   ,[Body]
			   ,[MailStatus]
			   ,[Type]
			   ,LineID
			   ,TemplateID
			   ,[Cre_By]
			   )
		 VALUES 
		 (@UserID_V,@MailID,'Auto Mail-Task Hold Released',@Body_CON,0,'Resource Allocation',@PM_ID,@Mail_Template_ID1,@Cre_By)
		  END
 END
 IF @Type='Questionnaire'
BEGIN
SET @Mail_Template_ID1=(select id from MailTemplate where TemplateName='Questionnaire')

	INSERT INTO [dbo].[MailMaster]
			   ([UserID]
			   ,[To]
			   ,[Subject]
			   ,[Body]
			   ,[MailStatus]
			   ,[Type]
			   ,TemplateID
			   ,Q_UserID
			   ,[Cre_By]
			   )
		 VALUES 
		 ('00000000-0000-0000-0000-000000000000',@Q_Mail,'Questionnaire',@Q_UserID,0,'Questionnaire',@Mail_Template_ID1,@Q_UserID,@Cre_By)
END

IF @Type='IssueTrack'
BEGIN
	
	SET @MailID=(Select EMail from UserMaster where [UserId] =@UserID) 
	--select @Body_CON=
	--		I.IssueName+','+Ph.PhaseName from Issuetrack I
	--		LEFT JOIN PhaseMaster Ph ON I.PhaseID=Ph.Id
	--		where I.Issuetrack_Id=@Issuetrack_Id
			select @Body_CON=
			I.IssueName+','+Ph.PhaseName+','+ins.InstaceName+','+
			(select Project_Name from Project where Project_Id=(select Project_ID from Instance where Instance_id=I.ProjectInstance_Id))
			from Issuetrack I
			LEFT JOIN PhaseMaster Ph ON I.PhaseID=Ph.Id
			left join instance ins on I.ProjectInstance_Id=ins.Instance_id			
			where I.Issuetrack_Id=@Issuetrack_Id

    DECLARE @InstanceID_ uniqueidentifier
	set @InstanceID_=(select ProjectInstance_Id from Issuetrack where Issuetrack_Id=@Issuetrack_Id)

	SET @Mail_Template_ID1=(select id from MailTemplate where TemplateName='IssueTrack')

	INSERT INTO [dbo].[MailMaster]
			   ([UserID]
			   ,[To]
			   ,[Subject]
			   ,[Body]
			   ,[MailStatus]
			   ,[Type]
			   ,[LineID]
			   ,TemplateID			  
			   ,[Cre_By]
			   )
		 VALUES 
		 (@UserID,@MailID,'IssueTrack',@Body_CON,0,'IssueTrack',@InstanceID_,@Mail_Template_ID1,@Cre_By)
END

IF @Type='SendCustomerMail'
BEGIN
	
	SET @Mail_Template_ID1=(select id from MailTemplate where TemplateName='SendCustomerMail')

	INSERT INTO [dbo].[MailMaster]
			   ([UserID]
			   ,[To]
			   ,[Subject]
			   ,[Body]
			   ,[MailStatus]
			   ,[Type]
			   ,TemplateID			  
			   ,[Cre_By]
			   )
		 VALUES 
		 ('00000000-0000-0000-0000-000000000000',@Q_Mail,'ProAcc2_Dev Survey Mail',@Q_Name,0,'ProAcc2_Dev Survey Mail',@Mail_Template_ID1,@Cre_By)
END
IF @Type ='ResetMail'
BEGIN
	SET @Mail_Template_ID1=(select id from MailTemplate where TemplateName='Reset Mail')

	INSERT INTO [dbo].[MailMaster]
			   ([UserID]
			   ,[To]
			   ,[Subject]
			   ,[Body]
			   ,[MailStatus]
			   ,[Type]
			   ,[LineID]
			   ,TemplateID			  
			   ,[Cre_By]
			   )
		 VALUES 
		 (@UserID,@Q_Mail,'ResetMail',@Q_Name,0,'ResetMail',@PM_ID,@Mail_Template_ID1,@Cre_By)
END

IF @Type='InstanceCreate'
BEGIN
select @Body_CON =InstaceName+','+P.Project_Name from Instance I
		join Project P on I.Project_ID=P.Project_Id
		where I.Instance_id=@InstanceID
		SET @Mail_Template_ID1=(select id from MailTemplate where TemplateName='Instance Created')
INSERT INTO [dbo].[MailMaster]
([UserID]
,[To]
,[Subject]
,[Body]
,[MailStatus]
,[Type]
,TemplateID
,[Cre_By]
)
VALUES
(@UserID,@Q_Mail,'InstanceCreate',@Body_CON,0,'InstanceCreate',@Mail_Template_ID1,@Cre_By)
END
IF @Type='ProjectCreate'
BEGIN
select @Body_CON =Project_Name from Project where Project_Id=@ProjectID
SET @Mail_Template_ID1=(select id from MailTemplate where TemplateName='Project Created')
INSERT INTO [dbo].[MailMaster]
			   ([UserID]
			   ,[To]
			   ,[Subject]
			   ,[Body]
			   ,[MailStatus]
			   ,[Type]
			   ,TemplateID			  
			   ,[Cre_By]
			   )
		 VALUES 
		 (@UserID,@Q_Mail,'ProjectCreate',@Body_CON,0,'ProjectCreate',@Mail_Template_ID1,@Cre_By)
END
IF @Type ='PdfAttachmentMail'
BEGIN
SET @Mail_Template_ID1=(select id from MailTemplate where TemplateName='Pdf Attachment Mail')
	INSERT INTO [dbo].[MailMaster]
			   ([UserID]
			   ,[To]
			   ,[Subject]
			   ,[Body]
			   ,[MailStatus]
			   ,[Type]
			   ,[LineID]
			   ,[TemplateID]
			   ,[Cre_By]
			   )
		 VALUES 
		 (@UserID,@Q_Mail,@Q_Subject,@Q_Name,0,'Pdf Attachment Mail',@FileName_ID,@Mail_Template_ID1,@Cre_By)
END
END


