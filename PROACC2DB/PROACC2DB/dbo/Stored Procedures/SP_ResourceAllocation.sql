-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_ResourceAllocation] 
	-- Add the parameters for the stored procedure here
@Type varchar(50),
@InstanceId uniqueidentifier=null,
@ProjectId uniqueidentifier=null,
@PhaseId int=null,
@RoleId int=null,
@CreBy uniqueidentifier=null,
@UserId uniqueidentifier=null,
@Id uniqueidentifier=null,
@UserType nvarchar(MAX) =null
AS
BEGIN
DECLARE @VID uniqueidentifier=null
IF @Type='GetPhase'
BEGIN
Select Distinct A.PhaseId As [Id] ,
(select Pm.PhaseName from PhaseMaster PM where Pm.Id=A.PhaseId and Pm.isActive=1)[PhaseName] from ProjectMonitor A 
join Instance I On I.Instance_id=@InstanceId
join Project P On P.Project_Id=@ProjectId
where A.isActive=1 
END


IF @Type='GetResource'
BEGIN

Select AVM.Task As [Task],BB.Block_Name As [BuildingBlock],PM.PhaseName As[Phase] ,UM.Name As [Owner],UM.UserId As[OwnerId],SS.StatusName As  [Status], AAM.ApplicationArea As [Applicationarea],RM.RoleName As [RoleName] from ActivityMaster AVM
		    join BuildingBlock BB on BB.block_ID =AVM.BuildingBlock_id 
			left join PhaseMaster PM on PM.Id=AVM.PhaseID
		    left join ApplicationAreaMaster AAM on AAM.Id=AVM.ApplicationAreaID
		    left join RoleMaster RM on RM.RoleId=AVM.RoleID
			join ProjectMonitor P on AVM.Activity_ID=p.ActivityID 
			left join UserMaster UM on UM.UserId=P.UserID
			left join StatusMaster SS on SS.Id=P.StatusId 
			where P.InstanceID=@InstanceId and P.isActive=1

END
IF @Type='GetPhaseResource'
BEGIN

Select AVM.Task As [Task],BB.Block_Name As [BuildingBlock],PM.PhaseName As[Phase] ,UM.Name As [Owner],UM.UserId As[OwnerId],SS.StatusName As  [Status], AAM.ApplicationArea As [Applicationarea],RM.RoleName As [RoleName],AVM.RoleID As[RoleId] from ActivityMaster AVM
		    join BuildingBlock BB on BB.block_ID =AVM.BuildingBlock_id 
			left join PhaseMaster PM on PM.Id=AVM.PhaseID
		    left join ApplicationAreaMaster AAM on AAM.Id=AVM.ApplicationAreaID
	        left join RoleMaster RM on RM.RoleId=AVM.RoleID
			join ProjectMonitor P on AVM.Activity_ID=P.ActivityID 
			left join UserMaster UM on UM.UserId=P.UserID
			left join StatusMaster SS on SS.Id=P.StatusId 
			where PM.Id=@PhaseId and P.isActive=1

END

IF @Type ='CheckPreviousPhase'
	Begin
	Declare @UploadStatus bit =null
	set @UploadStatus=(select AssessmentUploadStatus from Instance where Instance_id=@InstanceId)
	print @UploadStatus
	if @UploadStatus=1
	Begin
		SELECT *,A.Sequence_Num as Sequence_Num from   ProjectMonitor P LEFT JOIN ActivityMaster A On A.Activity_ID=P.ActivityID
		WHERE P.PhaseID=(@PhaseId-1)
		AND InstanceID=@InstanceId
		AND P.isActive=1
		AND P.StatusId NOT IN(1,3)
		AND A.Sequence_Num IS NOT NULL 
	End
    Else
	Begin
	SELECT CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER) [Guid]
	End
End

IF @Type='GetProgressMonitor'
BEGIN
		select *,AM.Task,BB.Block_Name,RM.RoleName,
		(select [Name] from UserMaster UM where UM.UserID=PM.UserID) [Name],
		(select StatusName from StatusMaster SM where SM.Id= PM.StatusId)[Status],
		(select TaskName from TaskType TT where TT.TaskId=AM.Task_id)[TaskType],
		(select count(*) from Comments c where c.ProjectMonitor_Id=PM.Id )[Comments_Count],
		CONVERT(VARCHAR(10), cast(PM.Planed__St_Date as date), 103)[PlanedDate],
		CONVERT(VARCHAR(10), cast(PM.Actual_St_Date as date), 103)[ActualDate],
		CONVERT(VARCHAR(10), cast(PM.Planed__En_Date as date), 103)[PlanedEn_Date],
		CONVERT(VARCHAR(10), cast(PM.Actual_En_Date as date), 103)[ActualEn_Date],
		CONVERT(VARCHAR(10), cast(PM.Modified_On as date), 103)[Modified_ON]

		from ProjectMonitor PM 
		join ActivityMaster AM on AM.Activity_ID=PM.ActivityID
		join BuildingBlock BB on BB.block_ID=AM.BuildingBlock_id
		join RoleMaster RM on RM.RoleId=PM.RoleId
		LEFT JOIN ParallelType PT on PT.ParallelId=AM.Parallel_Id
		where PM.PhaseId=@PhaseId and PM.InstanceID=@InstanceId order by Sequence_Num
		
END

IF @Type='GetProgressMonitorTaskCount'
BEGIN
DECLARE @TotalTask int, @Completed int, @WIP int, @ONHOLD int,@YetToStart int, @StartDate nvarchar(MAX),
@EndDate nvarchar(MAX),@PlannedEndDate nvarchar(MAX),@Colour nvarchar(MAX),@ActEndDate nvarchar(MAX),@PlanedEndDate nvarchar(MAX)

select @TotalTask= COUNT(*) from ProjectMonitor where isActive=1 and phaseId=@PhaseId and InstanceID=@InstanceId And StatusId!=(select id from statusmaster where statusname='Not Applicable') 
select @Completed= COUNT(*) from ProjectMonitor where isActive=1 and phaseId=@PhaseId and InstanceID=@InstanceId And StatusId=(select id from statusmaster where statusname='Completed') 
select @WIP= COUNT(*) from ProjectMonitor where isActive=1 and phaseId=@PhaseId and InstanceID=@InstanceId And StatusId=(select id from statusmaster where statusname='Work In Progress') 
select @ONHOLD= COUNT(*) from ProjectMonitor where isActive=1 and phaseId=@PhaseId and InstanceID=@InstanceId And StatusId=(select id from statusmaster where statusname='on hold') 
select @YetToStart= COUNT(*) from ProjectMonitor where isActive=1 and phaseId=@PhaseId and InstanceID=@InstanceId And StatusId=(select id from statusmaster where statusname='Yet To Start') 
select @StartDate=CONVERT(VARCHAR(10), cast(Cre_on as date), 103) from Instance where isActive=1 and Instance_id=@InstanceId
set @EndDate=(select Top(1) CONVERT(VARCHAR(10), cast(Actual_En_Date as date), 103) from ProjectMonitor where InstanceID=@InstanceId order by Actual_En_Date desc)
set @PlannedEndDate=(select Top(1) CONVERT(VARCHAR(10), cast(Planed__En_Date as date), 103) from ProjectMonitor where InstanceID=@InstanceId order by Planed__En_Date desc)


IF(@Completed=0)
Begin
 set @Colour=0--Grey
 End
ELSE
BEGIN

	IF(@Completed=@TotalTask)
	Begin
		--If(@PlannedEndDate>@EndDate)
	set @ActEndDate=(select Top(1) Actual_En_Date from ProjectMonitor  where InstanceID=@InstanceId order by Actual_En_Date desc)
	set @PlanedEndDate=(select Top(1) Planed__En_Date  from ProjectMonitor where InstanceID=@InstanceId order by Planed__En_Date desc)
		IF((DATEDIFF(DAY,@PlanedEndDate,@ActEndDate))<1)
		Begin
		set @Colour=1--Green
		End

		--Else IF( @PlannedEndDate<@EndDate)
		ELSE
		Begin
		set @Colour=2--Red
		End
	End

	Else If(@Completed!=@TotalTask)
	Begin
	select @Id= (select Top(1) Id from ProjectMonitor where InstanceID=@InstanceId and isActive=1 and phaseId=@PhaseId order by Actual_En_Date desc)
	--set @EndDate=(select Top(1) CONVERT(VARCHAR(10), cast(Actual_En_Date as date), 103) from ProjectMonitor where Id=@Id)
	--set @PlannedEndDate=(select Top(1) CONVERT(VARCHAR(10), cast(Planed__En_Date as date), 103) from ProjectMonitor where Id=@Id)
	set @ActEndDate=(select Top(1) Actual_En_Date from ProjectMonitor where Id=@Id)
	set @PlanedEndDate=(select Top(1) Planed__En_Date  from ProjectMonitor where Id=@Id)

		--IF(@PlannedEndDate>@EndDate)
		IF((DATEDIFF(DAY,@PlanedEndDate,@ActEndDate))<1)
		Begin
		set @Colour=1--Green
		End

		--Else IF(@PlannedEndDate<@EndDate)
		ELSE
		Begin
		set @Colour=2--Red
		End
	End
END

select @Colour as Colour,@TotalTask As TotalTask, @Completed As Completed, @WIP As WIP,@ONHOLD As ONHOLD,@YetToStart As YetToStart,@StartDate As StartDate, @EndDate As EndDate
END

IF @Type='GetOwnerList'
BEGIN
 Select Distinct Rl.RoleName[RName], Um.Name[Role],Um.UserId[OwnerID] from  ActivityMaster P
 Join RoleMaster Rl on Rl.RoleId=@RoleId
 join UserMaster Um on Um.RoleID=@RoleId
where P.isActive=1 and Rl.RoleId!=1 
END

IF @Type='GetRoleByPhase'
BEGIN
Select  Distinct RoleID As [OwnerID] from ActivityMaster where PhaseID=@PhaseId and isActive=1 
END

IF @Type='GetRoleByInstance'
BEGIN
Select  Distinct A.RoleID As [OwnerID] from ActivityMaster A
Join ProjectMonitor Pm on Pm.ActivityID=A.Activity_ID
where Pm.InstanceID=@InstanceId and A.isActive=1 and Pm.isActive=1
END

IF @Type='GetResourceAllocationData'
BEGIN
SELECT *,A.Sequence_Num as Sequence_Num,PT.ParallelName ,
(select Block_Name from buildingBlock BB where BB.block_ID = A.BuildingBlock_id) [BuildingBlock],
(select PhaseName from PhaseMaster PM where PM.Id=P.PhaseId) [Phase],
(select [Name] from UserMaster U where U.UserId=P.UserID)[Owner],
(select RoleName from RoleMaster R where R.RoleId=P.RoleId)[RoleName],
(select ApplicationArea from ApplicationAreaMaster AAM where AAM.Id=A.ApplicationAreaID)[ApplicationArea],
(select StatusName from StatusMaster SM where SM.Id= P.StatusId)[Status],
CONVERT(VARCHAR(10), cast(P.Cre_on as date), 103)[Cre_on_str]
   from  ProjectMonitor P 
    LEFT JOIN ActivityMaster A On A.Activity_ID=P.ActivityID 
	LEFT JOIN ParallelType PT on PT.ParallelId=A.Parallel_Id
	WHERE P.PhaseID=@PhaseId AND P.InstanceID=@InstanceId AND  P.isActive=1 AND 
	A.Sequence_Num IS NOT NULL order by A.Sequence_Num
END

IF @Type='GetResourceAllocationDataFromActivity'
BEGIN
DECLARE @Count int
	SET @Count=(SELECT COUNT(*) from ProjectMonitor WHERE PhaseID=@PhaseId AND InstanceID=@InstanceId)
	IF @Count=0
		BEGIN
		exec Sp_InsertMasterActivityToProjectMonitor @InsID=@InstanceId,@PhaID=@PhaseId,@CreBy=@CreBy
		exec SP_ResourceAllocation @Type='GetResourceAllocationData', @PhaseId=@PhaseId ,@InstanceId=@InstanceId
		END
	
END

IF @Type='GetUserByRole'
BEGIN

--select * from UserMaster where isActive=1 and RoleID!=1 and RoleID=@RoleId
Declare @Customer_Id uniqueidentifier =null
set @Customer_Id=(select Customer_Id from Instance I join Project P on I.Project_ID=P.Project_Id where Instance_id=@InstanceId)
Print @Customer_Id
select * from UserMaster where isActive=1 and RoleID!=1 and RoleID=@RoleId and Customer_Id in (@Customer_Id,'00000000-0000-0000-0000-000000000000')
END

IF @Type='GetUserByRoleBulkAllocate'
BEGIN
select UM.* from ProjectMonitor PM join UserMaster UM on PM.UserID = UM.UserId where PM.InstanceID=@InstanceId and PM.RoleId=@RoleId and PM.isActive=1 and UM.isActive=1
END

IF @Type='UpdateBulkAllocationOwnerRA'
BEGIN
Update ProjectMonitor set UserID=@UserId where PhaseId=@PhaseId and InstanceID=@InstanceId

EXEC SP_Mail @Type=AddFirstMail,@PM_ID=@Id,@InstanceID=@InstanceId,@Cre_By=@CreBy
END

IF @Type='UpdateOwnerResourceAllocation'
BEGIN
Update ProjectMonitor set UserID=@UserId where PhaseId=@PhaseId and InstanceID=@InstanceId and Id=@Id
EXEC SP_Mail @Type=AddFirstMail,@PM_ID=@Id,@InstanceID=@InstanceId,@Cre_By=@CreBy
END

IF @Type='UnassingnedTaskCount'
BEGIN
select COUNT(*) as [Unassigned_Count] from ProjectMonitor where UserID='00000000-0000-0000-0000-000000000000' and PhaseId=@PhaseId and InstanceID=@InstanceId
END

IF @Type='GetBulkAllocateOwnerResourceAllocation'
BEGIN
	Declare @BulkCount int
	DECLARE @PM_LINEID uniqueidentifier

	SET @VID=(SELECT version_id from instance WHERE Instance_id=@InstanceId )
	set @BulkCount=(select COUNT(*) from ProjectMonitor where PhaseId=@PhaseId and InstanceID=@InstanceId and RoleId=@RoleId and UserID='00000000-0000-0000-0000-000000000000' AND Version_Id=@VID)
	
	if @BulkCount >0
		BEGIN
			Update ProjectMonitor set UserID=@UserId where PhaseId=@PhaseId and InstanceID=@InstanceId and RoleId=@RoleId and UserID='00000000-0000-0000-0000-000000000000' AND Version_Id=@VID

			select TOP 1 @PM_LINEID=P.Id
			from ProjectMonitor P
			INNER JOIN ActivityMaster A on A.Activity_ID=P.ActivityID
			where InstanceID=@InstanceID AND StatusId!=1 AND P.Version_id=@VID ORDER by Sequence_Num

			EXEC SP_Mail @Type=AddFirstMail,@PM_ID=@PM_LINEID,@InstanceID=@InstanceId,@Cre_By=@CreBy
		
		END
	ELSE 
		BEGIN
			SELECT CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER) [Guid]
		END

		
END

IF @Type='GetProgressMonitorData'
BEGIN
select *,AM.Task,
	CONVERT(VARCHAR(11), cast(PM.Planed__St_Date as date), 106)[PlanedDate],
	CONVERT(VARCHAR(11), cast(PM.Actual_St_Date as date), 106)[ActualDate],
	CONVERT(VARCHAR(11), cast(PM.Planed__En_Date as date), 106)[PlanedEn_Date],
	CONVERT(VARCHAR(11), cast(PM.Actual_En_Date as date), 106)[ActualEn_Date],
	CONVERT(VARCHAR(10), cast(PM.Cre_on as date), 103)[CreatedDate],
(select Block_Name from buildingBlock BB where BB.block_ID = AM.BuildingBlock_id) [BuildingBlock],
(select [Name] from UserMaster U where U.UserId=PM.UserID)[Owner],
(select RoleName from RoleMaster R where R.RoleId=PM.RoleId)[RoleName],
(select StatusName from StatusMaster SM where SM.Id= PM.StatusId)[Status],
(select count(*) from Comments c where c.ProjectMonitor_Id=PM.Id )[Comments_Count]
from ProjectMonitor PM join ActivityMaster AM on AM.Activity_ID=PM.ActivityID
LEFT JOIN ParallelType PT on PT.ParallelId=AM.Parallel_Id
where PM.isActive=1 and Id=@Id order by Sequence_Num
END

IF @Type='GetStatus_PM'
BEGIN
Select * from StatusMaster where isActive=1 and Id in (3,4,5)
END

IF @Type='GetStatus_cons_Cust'
BEGIN
Select * from StatusMaster where isActive=1 and Id in (1,2,5)
END

IF @Type='GetRoleFromRA'
BEGIN
select Distinct RM.RoleId,RM.RoleName from ProjectMonitor PM join RoleMaster RM on PM.RoleId=RM.RoleId where PM.InstanceID=@InstanceId and PM.PhaseId=@PhaseId
END

IF @Type='GetUserRole'
BEGIN
Declare @CustId uniqueidentifier
set @CustId=(select Distinct P.Customer_Id from instance I
join Project P on P.project_Id=I.project_ID
where P.isActive=1 and I.Instance_id=@InstanceId)

select Distinct * from UserMaster where isActive=1 and RoleID!=1 and UserTypeID in (2,3) and Customer_Id in (@CustId,'00000000-0000-0000-0000-000000000000') order by RoleID
--select Distinct UserId,[name],RoleID from UserMaster where isactive=1 and  UserTypeID in (2,3) order by RoleID

END

IF @Type='PullTaskButton'
BEGIN
DECLARE @Ass_Upload_Count bit, @ProjectMon_Status_Count int
set @Ass_Upload_Count=(select AssessmentUploadStatus from Instance where isActive=1 and Instance_id=@InstanceId)
Print @Ass_Upload_Count
Print @PhaseId
if @PhaseId=1 
BEGIN
    set @ProjectMon_Status_Count=(select Count(*) from ProjectMonitor where isActive=1 and InstanceID=@InstanceId and PhaseId=(@PhaseId+1) and StatusId!=(select Id from statusMaster where StatusName='Yet To Start'))
	print @ProjectMon_Status_Count
		if @Ass_Upload_Count=1
		BEGIN
		SELECT CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER) [Guid]
		END
		--Else if @PhaseId=1 and @Ass_Upload_Count=0
		--BEGIN
		--SELECT CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER) [Guid]
		--END
END
Else if @PhaseId in (2,3,4)
	BEGIN
	set @ProjectMon_Status_Count=(select Count(*) from ProjectMonitor where isActive=1 and InstanceID=@InstanceId and PhaseId=(@PhaseId+1) and StatusId!=(select Id from statusMaster where StatusName='Yet To Start'))
	
	select @TotalTask= COUNT(*) from ProjectMonitor where isActive=1 and phaseId=@PhaseId and InstanceID=@InstanceId And StatusId!=(select id from statusmaster where statusname='Not Applicable') 
	select @Completed= COUNT(*) from ProjectMonitor where isActive=1 and phaseId=@PhaseId and InstanceID=@InstanceId And StatusId=(select id from statusmaster where statusname='Completed') 

	print 'inside 2,3,4'
	print @ProjectMon_Status_Count
		if @ProjectMon_Status_Count!=0 and @TotalTask=@Completed
		BEGIN
		print 'inside'
		SELECT CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER) [Guid]
		END
	END

END

END

