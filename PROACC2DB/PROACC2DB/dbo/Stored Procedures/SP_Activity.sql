
CREATE PROCEDURE [dbo].[SP_Activity]
	@Type varchar(50)=null,
	@RoleId int=null,
	@RoleName  varchar(50)=null,
	@isActive bit=null,
	@ModifiedBy uniqueidentifier=null,
	@ModifiedOn datetime=null,   
	@CreatedBy uniqueidentifier=null,
	@CreatedOn datetime=null,
	@Description varchar(50)=null,
	@isDelete bit=null,
	@phase int =null,
	@TaskId int =null,
	@Parallel_Id uniqueidentifier=null,
	@VersionID uniqueidentifier=null,
	@Parallel_Name varchar(max)=null,
	
	@Activity_ID int=null,
	@Id int=null,
	@Task varchar(250)=null,
	@BuildingBlock_id int=null,
	@PhaseID int=null,
	@PreviousId int =null,
	@ApplicationAreaID int=null,
	--@RoleID int =null,
	@EST_hours decimal(4,2)=null,
	@Cre_By varchar(50)=null,
	@PM_Add bit=null,
	@Task_Id int =null,
	@InstanceID uniqueidentifier=null,
	@ParallelTypeName varchar(50)=null
	
	--@Parallel_Id uniqueidentifier =null,
	--@Parallel_Name varchar(100)=null
AS
BEGIN
	--DECLARE @ActiveAMVertion_ID uniqueidentifier=(SELECT ID FROM AMVersion WHERE isActive=1)

	IF @Type='GetActivity'
	BEGIN
	DECLARE @AMVersionID uniqueidentifier
		IF @VersionID='00000000-0000-0000-0000-000000000000'
			SET @AMVersionID=(SELECT TOP 1 ID from AMVersion WHERE isActive=1 ORDER BY Cre_On)
		ELSE 
			SET @AMVersionID=@VersionID

		SELECT *,
		(select b.Block_Name from Buildingblock b where b.block_ID=a.BuildingBlock_id) [BuildingBlock],
		(select p.PhaseName from PhaseMaster p where p.Id=a.PhaseID) [Phase],
		(select R.RoleName from RoleMaster R where R.RoleId=a.RoleID) [Role],
		(select AA.ApplicationArea from ApplicationAreaMaster AA where AA.Id=a.ApplicationAreaID) [ApplicationArea],
		--(select PT.ParallelName from ParallelType PT where PT.ParallelId=a.Parallel_Id) [ParallelType],
		(select PT.Parallel_Name from ParallelType PT where PT.ParallelId=a.Parallel_Id) [Parallel_Name],
		(select TT.TaskName from TaskType TT where TT.TaskId=a.Task_id) [TaskType]
		from ActivityMaster a WHERE isActive=1 and PM_Add=0 AND Version_Id=@AMVersionID order by Sequence_Num
	END

	IF @Type='GetExcelActivity'
	BEGIN
		SELECT Task,
		(select b.Block_Name from Buildingblock b where b.block_ID=a.BuildingBlock_id) [BuildingBlock],
		(select p.PhaseName from PhaseMaster p where p.Id=a.PhaseID) [Phase],
		(select R.RoleName from RoleMaster R where R.RoleId=a.RoleID) [Role],
		(select AA.ApplicationArea from ApplicationAreaMaster AA where AA.Id=a.ApplicationAreaID) [ApplicationArea],
		(select TT.TaskName from TaskType TT where TT.TaskId=a.Task_id) [TaskType],
		(select PT.Parallel_Name from ParallelType PT where PT.ParallelId=a.Parallel_Id) [Parallel_Name]		
		from ActivityMaster a WHERE isActive=1 and PM_Add=0 AND a.Version_id=@VersionID order by Sequence_Num
	END
	
	IF @Type='GetBuildingBlock'
	BEGIN
	if @InstanceID Is null
	Select * From BuildingBlock where isActive=1
	else 
	Select * From BuildingBlock where isActive=1 and block_ID in (select * from STRING_SPLIT((select BuildingBlock_Id from ScenarioMaster where  ScenarioId=(select ScenarioId from Project where Project_Id=(select Project_ID from Instance where Instance_id=@InstanceID))),','))
	END

	IF @Type='GetApplicationArea'
	BEGIN
	Select * From ApplicationAreaMaster where isActive=1
	END

	IF @Type='GetPhase'
	BEGIN
	Select * From PhaseMaster where isActive=1
	END

	IF @Type='GetTaskType'
	BEGIN
	Select * From TaskType where isActive=1
	END

	IF @Type='GetParallelNameByPhase'
	BEGIN
	select Distinct ParallelId, Parallel_Name from ActivityMaster A 
	--select Distinct * from ActivityMaster A 
	join ParallelType P on A.Parallel_Id =p.ParallelId 
	join TaskType TT on A.Task_id =TT.TaskId
	where A.PhaseID= @phase and A.Task_id=@TaskId and A.PM_Add =0 and A.isActive=1 
	END

	IF @Type='GetParallelNameByPhase1'
	BEGIN
	select Distinct ParallelId, Parallel_Name from ActivityMaster A
	join ParallelType P on A.Parallel_Id =p.ParallelId 
	join TaskType TT on A.Task_id =TT.TaskId
	join ProjectMonitor PM on PM.ActivityID=A.Activity_ID
	where A.PhaseID= @phase and PM.InstanceID=@InstanceID and A.Task_id=@TaskId and A.isActive=1 and PM.isActive=1
	END

	IF @Type='GetTaskByParallelType'
	BEGIN
	Select * From ActivityMaster where isActive=1 and PhaseID=@phase and Parallel_Id=@Parallel_Id and PM_Add=0 AND Version_Id=@VersionID order by Sequence_Num 
	END
	
	IF @Type='GetTaskByParallelType1'
	BEGIN
	Select Distinct AM.* From ActivityMaster AM join ProjectMonitor PM on AM.Activity_ID=PM.ActivityID
	where 
	AM.isActive=1 and PM.isActive=1 and AM.PhaseID=@phase and PM.InstanceID=@InstanceID 
	and Parallel_Id=@Parallel_Id AND AM.Version_Id=(SELECT Version_ID from Instance WHERE  Instance_id=@InstanceID)
	
	order by Sequence_Num 
	END

	IF @Type='GetAllTask'
	BEGIN
	Select * From ActivityMaster where isActive=1 and PhaseID=@phase and PM_Add=0 AND Version_Id=@VersionID  order by Sequence_Num 
	END

	IF @Type='CheckTaskAvailability'
	BEGIN
	select Top(1) * from ActivityMaster where isActive=1 and Task=@Task AND Version_Id=@VersionID
	END

	IF @Type='CheckTaskAvailability1'
	BEGIN
		SET @VersionID=(select  Version_Id from ActivityMaster where Activity_ID=@Id)

	select Top(1) * from ActivityMaster where isActive=1 and Task=@Task and Activity_ID!=@Id AND Version_Id=@VersionID
	END
	IF @Type='CheckTaskAvailability2'
	BEGIN
	set @VersionID=(select Version_ID from Instance where Instance_id=@InstanceID)
	select Top(1) * from ActivityMaster where isActive=1 and Task=@Task AND Version_Id=@VersionID
	END
	IF @Type='CheckParallel_NameAvailability'
	BEGIN
	--Select * From ParallelType where isActive=1 and Parallel_Name=@Parallel_Name
	Select * From ParallelType PT join ActivityMaster AM on PT.ParallelId=AM.Parallel_Id
	where PT.isActive=1 and Parallel_Name=@Parallel_Name and AM.PM_Add=0
	END

	IF @Type='CheckParallel_NameAvailability1'
	BEGIN
	--Select * From ParallelType PT join ProjectMonitor PM on  where isActive=1 and Parallel_Name=@Parallel_Name
	Select Distinct PT.* From ParallelType PT 
	join ActivityMaster AM on PT.ParallelId=AM.Parallel_Id
	join ProjectMonitor PM on  PM.ActivityID=AM.Activity_ID
	where PT.isActive=1 and Parallel_Name=@Parallel_Name and PM.InstanceID=@InstanceID
	END
	
	IF @Type='EditActivityCreationById'
	BEGIN
		SELECT *,
		(select b.Block_Name from BuildingBlock b where b.block_ID=a.BuildingBlock_id) [Buildingblock],
		(select p.PhaseName from PhaseMaster p where p.Id=a.PhaseID) [Phase],
		(select R.RoleName from RoleMaster R where R.RoleId=a.RoleID) [Role],
		(select AA.ApplicationArea from ApplicationAreaMaster AA where AA.Id=a.ApplicationAreaID) [ApplicationArea],
		(select PT.ParallelName from ParallelType PT where PT.ParallelId=a.Parallel_Id) [ParallelType],
		(select PT.Parallel_Name from ParallelType PT where PT.ParallelId=a.Parallel_Id) [Parallel_Name],
		(select TT.TaskName from TaskType TT where TT.TaskId=a.Task_id) [TaskType]
		from ActivityMaster a WHERE isActive=1 and a.Activity_ID=@Activity_ID
	END

	IF @Type='DeleteActivityById'
	BEGIN
	DECLARE @Par_ID uniqueidentifier, @Par_count int
	Update ActivityMaster set isActive=0, IsDeleted=1 where Activity_ID=@Activity_ID
	set @Par_ID=(select Parallel_Id from ActivityMaster where Activity_ID=@Activity_ID and Task_id=2 and isActive=0)
	print @Par_ID
    set @Par_count=(select COUNT(*) from ActivityMaster where Parallel_Id=@Par_ID and isActive=1)
	print @Par_count
	if @Par_count=0
	BEGIN
	   update ParallelType set isActive=0, IsDeleted=1 where ParallelId=@Par_ID
	END

	END  
	
	DECLARE @PRE_SQUID int,@count int,@Start int,@innerCount int 
	DECLARE @PRE_SQU_Num int,@AC_ID int
	IF OBJECT_ID(N'tempdb..#temp_Activity') IS NOT NULL
	BEGIN
	DROP TABLE #temp_Activity
	END

	IF @Type='Add'
	BEGIN
	print 'After Add'
	BEGIN Transaction T 
		BEGIN TRY
		SET NOCOUNT ON;

		Declare @Temp_Parallel_Id uniqueidentifier
		set @Temp_Parallel_Id=@Parallel_Id
		IF @Task_Id=2 AND (@Parallel_Id='00000000-0000-0000-0000-000000000000' or @Parallel_Id is null)
		BEGIN
		Declare @ParallelId int
		Declare @uid uniqueidentifier
		set @uid=NEWID();
		Set @ParallelId=(select max(parallelName) from ParallelType)
		  if @ParallelId is NULL
		  BEGIN
		  set @ParallelId=0
		  END
		
		set @Parallel_Id=@uid
		END
		print 'before loop'
		print @Parallel_Id
		EXEC @PRE_SQUID= SP_Activity_Loop @PhaseID=@PhaseID,@PreviousId=@PreviousId,@Cre_By=@Cre_By,@Task_Id=@Task_Id,@Parallel_Id=@Parallel_Id,@VersionID=@VersionID
		PRINT @PRE_SQUID
		print 'After loop'
		if (select Count(*) from ParallelType where ParallelId=@Parallel_Id)=0 And @Task_Id=2
		Begin
		insert into ParallelType (ParallelId,parallelName,Parallel_Name,Cre_By) values(@Parallel_Id,(@ParallelId+1),@Parallel_Name,@Cre_By)		
		end
		ELSE if @Task_Id!=2 AND @Temp_Parallel_Id is null
		BEGIN		
		SET  @Parallel_Id=null
		END
		
		INSERT INTO [dbo].[ActivityMaster]
		([Task]
		,[BuildingBlock_id]
		,[PhaseID]
		,[Sequence_Num]
		,[ApplicationAreaID]
		,[RoleID]
		,[EST_hours]
		,PM_Add
		,Task_id
		,Parallel_Id
		,Version_Id
		,[Cre_By]		
		)
		VALUES
		(@Task,@BuildingBlock_id,@PhaseID,@PRE_SQUID+1,@ApplicationAreaID,@RoleID,@EST_hours,@PM_Add,@Task_Id,@Parallel_Id,@VersionID,@Cre_By)
		COMMIT Transaction T
		END TRY
		BEGIN CATCH 
		RollBack Transaction T
		END CATCH
	END

	IF @Type='Update'
	BEGIN
	BEGIN Transaction T 
		BEGIN TRY
		SET NOCOUNT ON;
		EXEC @PRE_SQUID= SP_Activity_Loop @PhaseID=@PhaseID,@PreviousId=@PreviousId,@Cre_By=@Cre_By,@VersionID=@VersionID
		Update ActivityMaster SET 
		Task =@Task,
		BuildingBlock_id=@BuildingBlock_id,
		PhaseID=@PhaseID,
		Sequence_Num=@PRE_SQUID+1,
		ApplicationAreaID=@ApplicationAreaID,
		RoleID=@RoleID,
		EST_hours=@EST_hours,
		Modified_On=GETUTCDATE(),
		Modified_by=@Cre_By
		WHERE Activity_ID=@Activity_ID

		COMMIT Transaction T1
		END TRY
		BEGIN CATCH 
		RollBack Transaction T1
		END CATCH
	END

	

	IF @Type='GetAllActiveParallelType'
	BEGIN
		SELECT ParallelId,ParallelName,Parallel_Name from ParallelType where isActive=1 AND IsDeleted=0
	END
	IF @Type='InsertParallelType'
	BEGIN
	
		INSERT INTO ParallelType 
		(ParallelId, ParallelName,Parallel_Name)
		values
		(NEWID(),(SELECT MAX(ParallelName)+1 from ParallelType),@ParallelTypeName)
	END

	
END



