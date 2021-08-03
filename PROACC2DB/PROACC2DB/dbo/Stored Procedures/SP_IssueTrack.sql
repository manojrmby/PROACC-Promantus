CREATE PROCEDURE [dbo].[SP_IssueTrack]
@Type varchar(50),
--@Issuetrack_Id uniqueidentifier=null,
@ProjectInstance_Id uniqueidentifier=null,
@IssueName varchar(150)=null,
@PhaseID int=null,
@TaskId int=null,
@StartDate datetime=null,
@EndDate datetime=null,
@LastUpdatedDate datetime=null,
@AssignedTo uniqueidentifier=null,
@Status varchar(150)=null,
@IsApproved bit=null,
@Cre_By uniqueidentifier=null,
@Comments varchar(500)=null,
@Description varchar(MAX)=null,
@Project_Id uniqueidentifier=null,
@Instance_Id uniqueidentifier=null,
@Customer_Id uniqueidentifier=null,
@Id uniqueidentifier=null,

@Table_Id uniqueidentifier=null,
@Table_Name varchar(MAX)=null,
@UserId uniqueidentifier=null,
@Body varchar(MAX)=null,
@Link varchar(MAX)=null,
@tblSAPIssueTrackUpload UploadSAPIssueTrackUpload READONLY

AS
BEGIN
	IF @Type='AddIssueTrack'
	BEGIN
	SET NOCOUNT ON;
	DECLARE @Issuetrack_Id uniqueidentifier=NEWID();
	DECLARE @RID int;
	DECLARE @RunningID int;
		
		set @RID =(select max(RunningID) from Issuetrack where PhaseID=@PhaseID)
			if (@RID is NULL)
			Begin
			set @RunningID=1;
			end
			ELSE
			Begin
			set @RunningID=@RID+1;		
			end

			INSERT INTO Issuetrack(Issuetrack_Id,RunningID,IssueName,PhaseID,TaskId,ProjectInstance_Id,StartDate,EndDate,LastUpdatedDate,AssignedTo,[Status],IsApproved,Cre_By)
			VALUES(@Issuetrack_Id,@RunningID,@IssueName,@PhaseID,@TaskId,@ProjectInstance_Id,GETUTCDATE(),GETUTCDATE(),GETUTCDATE(),@AssignedTo,@Status,@IsApproved,@Cre_By)
		 
			Insert into HistoryLog(IssueTrackId,CreatedDate,HistoryComment,[Description],AssignedTo,Cre_By)
			Values(@Issuetrack_Id,GETUTCDATE(),@Comments,@Description,@AssignedTo,@Cre_By)

		 EXEC SP_Mail @Type='IssueTrack',@Issuetrack_Id=@Issuetrack_Id,@UserID=@AssignedTo,@Cre_By=@Cre_By,@InstanceID=@ProjectInstance_Id

		 EXEC SP_IssueTrack @Type='InsertBell',@Table_Id=@Issuetrack_Id,@Table_Name='IssueTrack',@UserId=@AssignedTo,
		       @Body='',@Link='',@Cre_By=@Cre_By
	END

	IF @Type='EditIssueTrack'
	BEGIN
	SET NOCOUNT ON;	 
		select *,
		(select a.Task from ActivityMaster a
		 where a.Activity_ID=I.TaskId)[Task],
		 (select p.PhaseName from PhaseMaster p
		 where p.Id=I.PhaseID)[Phase],
		  (select u.[Name] from UserMaster u
		 where u.UserId=I.AssignedTo)[Assigned]
		from Issuetrack I
		join HistoryLog H on I.Issuetrack_Id = H.IssueTrackId
		where I.Issuetrack_Id=@Id
	END

	IF @Type='UpdateIssueTrack'
	BEGIN
	SET NOCOUNT ON;
	   DECLARE @Modified_by uniqueidentifier=null;
	   DECLARE @Modified uniqueidentifier=null;
	   set @Modified=(select AssignedTo from Issuetrack where Issuetrack_Id=@Id)
	   if @Modified != @AssignedTo
	   Begin
		   Update Issuetrack set [Status] =@Status,AssignedTo=@AssignedTo, Modified_by=@Cre_By,Modified_On=GETUTCDATE(),EndDate=GETUTCDATE(),LastUpdatedDate=GETUTCDATE()
			where Issuetrack_Id=@Id
			 EXEC SP_Mail @Type='IssueTrack',@Issuetrack_Id=@Id,@UserID=@AssignedTo,@Cre_By=@Cre_By,@InstanceID=@ProjectInstance_Id
	   End
		Else
		Begin
			Update Issuetrack set [Status] =@Status,AssignedTo=@AssignedTo, Modified_by=@Cre_By,Modified_On=GETUTCDATE(),EndDate=GETUTCDATE(),LastUpdatedDate=GETUTCDATE()
			where Issuetrack_Id=@Id
		End
		--Insert into HistoryLog(IssueTrackId,CreatedDate,HistoryComment,[Description],AssignedTo,Cre_By) 
		--Values(@Id,GETDATE(),@Comments,@Description,@AssignedTo,@Cre_By)

		if @Comments!=''
		Begin
		Insert into HistoryLog(IssueTrackId,CreatedDate,HistoryComment,[Description],AssignedTo,Cre_By) 
		Values(@Id,GETUTCDATE(),@Comments,@Description,@AssignedTo,@Cre_By)
		end
	END

	IF @Type='GetIssueTrackTask'
	BEGIN	
	select PM.ActivityID,AM.Task from ProjectMonitor PM join ActivityMaster AM on PM.ActivityID=AM.Activity_ID
	 where PM.isActive=1 and PM.InstanceID=@Instance_Id and PM.PhaseId=@PhaseID order by AM.Task
	END

	IF @Type='PullData'
	BEGIN
	SET NOCOUNT ON;
	
	DECLARE @NewLine NCHAR(1) = NCHAR(10);
	
	if @Project_Id = '00000000-0000-0000-0000-000000000000' and @ProjectInstance_Id = '00000000-0000-0000-0000-000000000000'
	Begin
		SELECT  Distinct SS.*,
       (SELECT  CONVERT(nvarchar(30),US.CreatedDate, 103)+'-'+US.HistoryComment+@NewLine
        FROM HistoryLog US WHERE US.IssueTrackId = SS.Issuetrack_Id FOR XML PATH('')) [Comments],
		(select  a.Task from ActivityMaster a where a.Activity_ID=ss.TaskId)[Task],
		(select  p.PhaseName from PhaseMaster p where p.Id=ss.PhaseID)[Phase],
		(select  u.[Name] from UserMaster u where u.UserId=ss.AssignedTo)[Assigned],
		I.InstaceName as [Instance],
		P.Project_Name as  [Project],
         C.Company_Name as [Customer],
		 (select [Name] from UserMaster where UserId=SS.Cre_By )[Created_By]
		 From Issuetrack SS
		 join Instance I on I.Instance_id = SS.ProjectInstance_Id
		 join Project P on P.Project_Id=I.Project_ID
		 join Customer C on P.Customer_Id=C.Customer_ID
		 join UserMaster um on um.customer_Id=P.Customer_Id
		 where SS.isActive=1 and (SS.AssignedTo=@Id or SS.Cre_By=@Id)
		order by SS.PhaseID,RunningID
	end
	else if @Project_Id is not null and @ProjectInstance_Id = '00000000-0000-0000-0000-000000000000'
	Begin
	SELECT  Distinct SS.*,
       (SELECT  CONVERT(nvarchar(30),US.CreatedDate, 103)+'-'+US.HistoryComment+@NewLine
        FROM HistoryLog US WHERE US.IssueTrackId = SS.Issuetrack_Id FOR XML PATH('')) [Comments],
		(select  a.Task from ActivityMaster a where a.Activity_ID=ss.TaskId)[Task],
		(select  p.PhaseName from PhaseMaster p where p.Id=ss.PhaseID)[Phase],
		(select  u.[Name] from UserMaster u where u.UserId=ss.AssignedTo)[Assigned],
		I.InstaceName as [Instance],
		P.Project_Name as  [Project],
         C.Company_Name as [Customer],
		 (select [Name] from UserMaster where UserId=SS.Cre_By )[Created_By]
		 From Issuetrack SS
		 join Instance I on I.Instance_id = SS.ProjectInstance_Id
		 join Project P on P.Project_Id=I.Project_ID
		 join Customer C on P.Customer_Id=C.Customer_ID
		 join UserMaster um on um.customer_Id=P.Customer_Id
		 where SS.isActive=1 and (SS.AssignedTo=@Id or SS.Cre_By=@Id) and P.Project_Id=@Project_Id
		order by SS.PhaseID,RunningID
	end
	else if @Project_Id is not null and @ProjectInstance_Id is not null
	Begin
	SELECT  Distinct SS.*,
       (SELECT  CONVERT(nvarchar(30),US.CreatedDate, 103)+'-'+US.HistoryComment+@NewLine
        FROM HistoryLog US WHERE US.IssueTrackId = SS.Issuetrack_Id FOR XML PATH('')) [Comments],
		(select  a.Task from ActivityMaster a where a.Activity_ID=ss.TaskId)[Task],
		(select  p.PhaseName from PhaseMaster p where p.Id=ss.PhaseID)[Phase],
		(select  u.[Name] from UserMaster u where u.UserId=ss.AssignedTo)[Assigned],
		I.InstaceName as [Instance],
		P.Project_Name as  [Project],
         C.Company_Name as [Customer],
		 (select [Name] from UserMaster where UserId=SS.Cre_By )[Created_By]
		 From Issuetrack SS
		 join Instance I on I.Instance_id = SS.ProjectInstance_Id
		 join Project P on P.Project_Id=I.Project_ID
		 join Customer C on P.Customer_Id=C.Customer_ID
		 join UserMaster um on um.customer_Id=P.Customer_Id
		 where SS.isActive=1 and (SS.AssignedTo=@Id or SS.Cre_By=@Id) and P.Project_Id=@Project_Id and ProjectInstance_Id=@ProjectInstance_Id
		order by SS.PhaseID,RunningID
	end
	END

	IF @Type='Pull_PM_Data'
	BEGIN
	SET NOCOUNT ON;
	if @Project_Id = '00000000-0000-0000-0000-000000000000' and @ProjectInstance_Id = '00000000-0000-0000-0000-000000000000'
	Begin
		SELECT  Distinct SS.*,
		   (SELECT  CONVERT(nvarchar(30),US.CreatedDate, 103)+'-'+US.HistoryComment+@NewLine
			FROM HistoryLog US WHERE US.IssueTrackId = SS.Issuetrack_Id FOR XML PATH('')) [Comments],
			(select  a.Task from ActivityMaster a where a.Activity_ID=ss.TaskId)[Task],
			(select  p.PhaseName from PhaseMaster p where p.Id=ss.PhaseID)[Phase],
			(select  u.[Name] from UserMaster u where u.UserId=ss.AssignedTo)[Assigned],
			I.InstaceName as [Instance],
			P.Project_Name as  [Project],
			 C.Company_Name as [Customer],
			 (select [Name] from UserMaster where UserId=SS.Cre_By )[Created_By]
			 From Issuetrack SS
			 join Instance I on I.Instance_id = SS.ProjectInstance_Id
			 join Project P on P.Project_Id=I.Project_ID
			 join Customer C on P.Customer_Id=C.Customer_ID
			 --join UserMaster um on um.customer_Id=P.Customer_Id		 
			 where SS.isActive=1 and (P.ProjectManager_Id=@Id or SS.Cre_By=@Id)
			order by SS.PhaseID,RunningID
	End
	else if @Project_Id is not null and @ProjectInstance_Id = '00000000-0000-0000-0000-000000000000'
	Begin
	SELECT  Distinct SS.*,
		   (SELECT  CONVERT(nvarchar(30),US.CreatedDate, 103)+'-'+US.HistoryComment+@NewLine
			FROM HistoryLog US WHERE US.IssueTrackId = SS.Issuetrack_Id FOR XML PATH('')) [Comments],
			(select  a.Task from ActivityMaster a where a.Activity_ID=ss.TaskId)[Task],
			(select  p.PhaseName from PhaseMaster p where p.Id=ss.PhaseID)[Phase],
			(select  u.[Name] from UserMaster u where u.UserId=ss.AssignedTo)[Assigned],
			I.InstaceName as [Instance],
			P.Project_Name as  [Project],
			 C.Company_Name as [Customer],
			 (select [Name] from UserMaster where UserId=SS.Cre_By )[Created_By]
			 From Issuetrack SS
			 join Instance I on I.Instance_id = SS.ProjectInstance_Id
			 join Project P on P.Project_Id=I.Project_ID
			 join Customer C on P.Customer_Id=C.Customer_ID
			 --join UserMaster um on um.customer_Id=P.Customer_Id		 
			 where SS.isActive=1 and (P.ProjectManager_Id=@Id or SS.Cre_By=@Id) and P.Project_Id=@Project_Id
			order by SS.PhaseID,RunningID
	end
	else if @Project_Id is not null and @ProjectInstance_Id is not null
	Begin
	SELECT  Distinct SS.*,
		   (SELECT  CONVERT(nvarchar(30),US.CreatedDate, 103)+'-'+US.HistoryComment+@NewLine
			FROM HistoryLog US WHERE US.IssueTrackId = SS.Issuetrack_Id FOR XML PATH('')) [Comments],
			(select  a.Task from ActivityMaster a where a.Activity_ID=ss.TaskId)[Task],
			(select  p.PhaseName from PhaseMaster p where p.Id=ss.PhaseID)[Phase],
			(select  u.[Name] from UserMaster u where u.UserId=ss.AssignedTo)[Assigned],
			I.InstaceName as [Instance],
			P.Project_Name as  [Project],
			 C.Company_Name as [Customer],
			 (select [Name] from UserMaster where UserId=SS.Cre_By )[Created_By]
			 From Issuetrack SS
			 join Instance I on I.Instance_id = SS.ProjectInstance_Id
			 join Project P on P.Project_Id=I.Project_ID
			 join Customer C on P.Customer_Id=C.Customer_ID
			 --join UserMaster um on um.customer_Id=P.Customer_Id		 
			 where SS.isActive=1 and (P.ProjectManager_Id=@Id or SS.Cre_By=@Id) and P.Project_Id=@Project_Id and ProjectInstance_Id=@ProjectInstance_Id
			order by SS.PhaseID,RunningID
	end
	END

	IF @Type='Pull_Consultant_Data'
	
	BEGIN
	SET NOCOUNT ON;	
	if @Customer_Id='00000000-0000-0000-0000-000000000000' and @Project_Id = '00000000-0000-0000-0000-000000000000' and @ProjectInstance_Id = '00000000-0000-0000-0000-000000000000'
	Begin

	SELECT  Distinct SS.*,
       (SELECT  CONVERT(nvarchar(30),US.CreatedDate, 103)+'-'+US.HistoryComment+@NewLine
        FROM HistoryLog US WHERE US.IssueTrackId = SS.Issuetrack_Id FOR XML PATH('')) [Comments],
		(select  a.Task from ActivityMaster a where a.Activity_ID=ss.TaskId)[Task],
		(select  p.PhaseName from PhaseMaster p where p.Id=ss.PhaseID)[Phase],
		(select  u.[Name] from UserMaster u where u.UserId=ss.AssignedTo)[Assigned],
		(select I.InstaceName from Instance I where I.Instance_id=SS.ProjectInstance_Id) [Instance],		 
		(select P.Project_Name from project P where Project_Id=(select Project_ID from Instance where Instance_id=SS.ProjectInstance_Id )) [Project],
		(select C.Company_Name from Customer C where Customer_ID=(select Customer_Id from Project where Project_Id=(select Project_ID from Instance where Instance_id=SS.ProjectInstance_Id)))[Customer],
		(select [Name] from UserMaster where UserId=SS.Cre_By )[Created_By]
		From Issuetrack SS
		 where SS.isActive=1 and (SS.AssignedTo=@Id or SS.Cre_By=@Id)
		order by SS.PhaseID,RunningID
	end
	else if @Customer_Id is not null and @Project_Id = '00000000-0000-0000-0000-000000000000' and @ProjectInstance_Id = '00000000-0000-0000-0000-000000000000'
	Begin
	SELECT  Distinct SS.*,
       (SELECT  CONVERT(nvarchar(30),US.CreatedDate, 103)+'-'+US.HistoryComment+@NewLine
        FROM HistoryLog US WHERE US.IssueTrackId = SS.Issuetrack_Id FOR XML PATH('')) [Comments],
		(select  a.Task from ActivityMaster a where a.Activity_ID=ss.TaskId)[Task],
		(select  p.PhaseName from PhaseMaster p where p.Id=ss.PhaseID)[Phase],
		(select  u.[Name] from UserMaster u where u.UserId=ss.AssignedTo)[Assigned],
		I.InstaceName as [Instance],
		P.Project_Name as  [Project],
		(select C.Company_Name from Customer C where Customer_ID=(select Customer_Id from Project where Project_Id=(select Project_ID from Instance where Instance_id=SS.ProjectInstance_Id)))[Customer],
		(select [Name] from UserMaster where UserId=SS.Cre_By )[Created_By]
		 From Issuetrack SS		
		 join Instance I on I.Instance_id = SS.ProjectInstance_Id
		 join Project P on P.Project_Id=I.Project_ID
		 where SS.isActive=1 and (SS.AssignedTo=@Id or SS.Cre_By=@Id) and Customer_Id=@Customer_Id
		order by SS.PhaseID,RunningID
	end
	else if @Customer_Id is not null and @Project_Id is not null and @ProjectInstance_Id = '00000000-0000-0000-0000-000000000000'
	Begin
	SELECT  Distinct SS.*,
       (SELECT  CONVERT(nvarchar(30),US.CreatedDate, 103)+'-'+US.HistoryComment+@NewLine
        FROM HistoryLog US WHERE US.IssueTrackId = SS.Issuetrack_Id FOR XML PATH('')) [Comments],
		(select  a.Task from ActivityMaster a where a.Activity_ID=ss.TaskId)[Task],
		(select  p.PhaseName from PhaseMaster p where p.Id=ss.PhaseID)[Phase],
		(select  u.[Name] from UserMaster u where u.UserId=ss.AssignedTo)[Assigned],
		I.InstaceName as [Instance],
		P.Project_Name as  [Project],
		(select C.Company_Name from Customer C where Customer_ID=(select Customer_Id from Project where Project_Id=(select Project_ID from Instance where Instance_id=SS.ProjectInstance_Id)))[Customer],
		(select [Name] from UserMaster where UserId=SS.Cre_By )[Created_By]
		 From Issuetrack SS		
		 join Instance I on I.Instance_id = SS.ProjectInstance_Id
		 join Project P on P.Project_Id=I.Project_ID
		 where SS.isActive=1 and (SS.AssignedTo=@Id or SS.Cre_By=@Id) and Customer_Id=@Customer_Id and P.Project_Id=@Project_Id
		order by SS.PhaseID,RunningID
	end
	else if @Customer_Id is not null and @Project_Id is not null and @ProjectInstance_Id is not null
	Begin
	SELECT  Distinct SS.*,
       (SELECT  CONVERT(nvarchar(30),US.CreatedDate, 103)+'-'+US.HistoryComment+@NewLine
        FROM HistoryLog US WHERE US.IssueTrackId = SS.Issuetrack_Id FOR XML PATH('')) [Comments],
		(select  a.Task from ActivityMaster a where a.Activity_ID=ss.TaskId)[Task],
		(select  p.PhaseName from PhaseMaster p where p.Id=ss.PhaseID)[Phase],
		(select  u.[Name] from UserMaster u where u.UserId=ss.AssignedTo)[Assigned],
		I.InstaceName as [Instance],
		P.Project_Name as  [Project],
		(select C.Company_Name from Customer C where Customer_ID=(select Customer_Id from Project where Project_Id=(select Project_ID from Instance where Instance_id=SS.ProjectInstance_Id)))[Customer],
		(select [Name] from UserMaster where UserId=SS.Cre_By )[Created_By]
		 From Issuetrack SS	
		 join Instance I on I.Instance_id = SS.ProjectInstance_Id
		 join Project P on P.Project_Id=I.Project_ID
		 where SS.isActive=1 and (SS.AssignedTo=@Id or SS.Cre_By=@Id) and Customer_Id=@Customer_Id and P.Project_Id=@Project_Id and ProjectInstance_Id=@ProjectInstance_Id
		order by SS.PhaseID,RunningID
	end
	END

	IF @Type='AssignedTo'
	BEGIN
	SET NOCOUNT ON;
	
		select distinct u.UserId,u.Name from UserMaster u
		join ProjectMonitor p on p.UserID = u.UserId
		join Instance i on i.Instance_id=p.InstanceID
		join Project pr on pr.Project_Id =i.Project_ID
		where pr.Project_Id=@Project_Id and u.UserId!=@Id
		and u.isActive=1 and p.isActive=1 and i.isActive=1 and pr.isActive=1
	END

	IF @Type='EditAssignedTo'
	BEGIN
	SET NOCOUNT ON;
	
		select distinct u.UserId,u.Name from UserMaster u
		join ProjectMonitor p on p.UserID = u.UserId
		join Instance i on i.Instance_id=p.InstanceID
		join Project pr on pr.Project_Id =i.Project_ID
		--where i.Instance_id=@Instance_Id
		where pr.Project_Id=(select Project_Id from Project where Project_Id=(select Project_ID from Instance where Instance_id=@Instance_Id))
		and u.UserId!=(select Cre_By from Issuetrack where Issuetrack_Id=@Id)
		and u.isActive=1 and p.isActive=1 and i.isActive=1 and pr.isActive=1
	END

	IF @Type='GetComments'
	BEGIN
	Select *,
	(select [Name] from UserMaster UM where UM.UserId=c.Cre_By)[Name],(select [USERID] from UserMaster UM where UM.UserId=c.Cre_By)USERID
	from HistoryLog c where IssueTrackId=@Id and isActive=1 order by Cre_on 
	END

	IF @Type='InsertComments'
	BEGIN
	Insert into HistoryLog(IssueTrackId,CreatedDate,HistoryComment,[Description],AssignedTo,Cre_By)
			Values(@Id,GETUTCDATE(),@Comments,@Description,@AssignedTo,@Cre_By)
	END

	IF @Type='AssigneeCount'
	BEGIN
	select * from Issuetrack i join Project p on i.Cre_By=p.ProjectManager_Id where i.Cre_By=@Id
	END

	IF @Type='InsertBell'
	BEGIN
	insert into Bell(Id,Table_Id,Table_Name,UserId,Body,Link,Cre_on,Cre_By) values
	(NEWID(),@Table_Id,@Table_Name,@UserId,@Body,@Link,GETUTCDATE(),@Cre_By)
	END

	IF @Type='GetMessage'
	Begin
	Select CONVERT(nvarchar(30),B.Cre_on, 103)as[CreatedOn],B.Id,
	--B.Cre_on as[CreatedOn],
	I.IssueName as IssueName,
	(select Name from UserMaster where UserId=B.UserId) as UserName,
	(select Name from UserMaster where UserId=B.Cre_By) as ProjectManagerName from  Bell B
	join Issuetrack I on B.Table_Id= I.Issuetrack_Id
	 where B.UserId=@UserId and B.isActive=1 order by B.Cre_on desc
	End


	IF @Type='DeleteMessage'
	BEGIN
	Update Bell set isActive=0 where Id=@UserId
	END

	IF @Type='SAPIssueTrackUpload'
	BEGIN
	DECLARE @SAPDumpIssuetrack_Id uniqueidentifier=NEWID()
	DECLARE @R_ID int;
		
		set @RID =(select max(RunningID) from SAPDumpIssuetrack)
			if (@RID is NULL)
			Begin
			set @R_ID=1;
			end
			ELSE
			Begin
			set @R_ID=@RID+1;		
			end

	--set @Project_Id=(select Project_ID from Instance where Instance_id=@ProjectInstance_Id)
	
	MERGE dbo.SAPDumpIssuetrack AS trg
    USING @tblSAPIssueTrackUpload AS src
      ON src.IssueNo = trg.IssueNo and trg.Project_Id =@Project_Id
	  WHEN MATCHED THEN
	  UPDATE SET SAPIssueDumpStatus_Id=(select Id from SAPIssuetrackStatus where StatusName = src.SAPIssueDumpStatus)
   
   WHEN NOT MATCHED BY TARGET THEN
	
	insert (Id,RunningID,IssueNo,IssueName,Category_Id,Priority_Id,Assignee,RaisedBy,ApplicationArea_Id,OpenDt,CloseDt,SAPIssueDumpStatus_Id,
	Project_Id, Resolution,Comments,Cre_By)
	values( NEWID(),@R_ID,src.IssueNo,src.IssueName,
	(select Id from SAPIssuetrackCategory where CategoryName = src.Category),
	(select Id from SAPIssuetrackpriority where PriorityName = src.[Priority]),
	src.Assignee,src.RaisedBy,
	(select Id from SAPIssuetrackApplicationarea where ApplicationAreaName = src.ApplicationArea),
	CAST(src.OpenDt as date),CAST(src.CloseDt as date),
	--NULLIF(CONVERT(varchar,CONVERT(datetime, src.OpenDt),101),''),NULLIF(CONVERT(varchar,CONVERT(datetime, src.CloseDt),101),''),
	(select Id from SAPIssuetrackStatus where StatusName = src.SAPIssueDumpStatus),
	@Project_Id,src.Comments,src.Resolution,@Cre_By);
	

	--select NEWID(),@R_ID,IssueNo,IssueName,c.Id,p.Id,Assignee,RaisedBy,a.Id,NULLIF(CONVERT(varchar,CONVERT(datetime, OpenDt),101),''),NULLIF(CONVERT(varchar,CONVERT(datetime, CloseDt),101),''),s.Id,@Project_Id,Resolution,@Cre_By
	--from @tblSAPIssueTrackUpload t 
	--join SAPIssuetrackCategory c on t.Category = c.CategoryName
	--join SAPIssuetrackpriority p on t.[Priority]= p.PriorityName
	--join SAPIssuetrackApplicationarea a on t.ApplicationArea=a.ApplicationAreaName
	--join SAPIssuetrackStatus s on t.SAPIssueDumpStatus=s.StatusName

	END
	
	IF @Type='LoadSAPIssueTrack'
	BEGIN
	--set @Project_Id=(select Project_ID from Instance where Instance_id=@ProjectInstance_Id)
	select t.*,CONVERT(varchar,CONVERT(datetime, t.OpenDt),103)[OpenDt1],CONVERT(varchar,CONVERT(datetime, t.CloseDt),103)[CloseDt1],c.CategoryName,p.PriorityName,a.ApplicationAreaName,s.StatusName from SAPDumpIssuetrack t
	join SAPIssuetrackCategory c on t.Category_Id = c.Id
	join SAPIssuetrackpriority p on t.Priority_Id= p.Id
	join SAPIssuetrackApplicationarea a on t.ApplicationArea_Id=a.Id
	join SAPIssuetrackStatus s on t.SAPIssueDumpStatus_Id=s.Id
	 where t.Project_Id=@Project_Id order by IssueNo asc
	END

	IF @Type='SAPEditIssueTrack'
	BEGIN
	select * from SAPDumpIssuetrack where Id=@Id
	END

	IF @Type='GetSAPStatus'
	BEGIN
	select * from SAPIssuetrackStatus where isActive=1
	END

	IF @Type='UpdateSAPIssueTrack'
	BEGIN
	Update SAPDumpIssuetrack set SAPIssueDumpStatus_Id=@Status,Comments=@Comments where Id=@Id
	END

END


--select * from SAPDumpIssuetrack order by cre_on desc

--select * from SAPIssuetrackpriority
--select * from SAPIssuetrackCategory
--select * from SAPIssuetrackApplicationarea
--select * from SAPIssuetrackStatus


