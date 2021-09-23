CREATE PROCEDURE [dbo].[SP_IssueDumpUpload]
	@Type varchar(50)=null,
	@Createdby uniqueidentifier=null,
	@FileName uniqueidentifier=null,
	@ProjectId uniqueidentifier=null,
	@SAPIssueTrackUpload UploadSAPIssueTrackUpload READONLY
AS
BEGIN
	IF @Type='SAPIssueTrackUpload'
	BEGIN
	
	DECLARE @MyCursor CURSOR;
	DECLARE @IssueNo varchar(500),@IssueName varchar(500),@Category varchar(500),@Priority varchar(500),@Assignee varchar(500),@RaisedBy varchar(500),
	@ApplicationArea varchar(500),@OpenDt varchar(500) ,@CloseDt varchar(500),@SAPIssueDumpStatus varchar(500),@Resolution varchar(500),@Comments varchar(500)
	
	SET @MyCursor = CURSOR FOR select  
	[IssueNo],
	[IssueName],
	[Category],
	[Priority],
	[Assignee],
	[RaisedBy],
	[ApplicationArea],
	[OpenDt],
	[CloseDt],
	[SAPIssueDumpStatus],
	[Resolution],
	[Comments]	
	from @SAPIssueTrackUpload 
		
	DECLARE @SAPDumpIssuetrack_Id uniqueidentifier=NEWID()
	DECLARE @RID int,@R_ID int;
	set @RID =(select count(*) from SAPDumpIssuetrack)
	if (@RID is NULL)
	Begin
	set @R_ID=1;
	end
	ELSE
	Begin
	set @R_ID=@RID+1;		
	end


		OPEN @MyCursor 
		FETCH NEXT FROM @MyCursor 
		INTO @IssueNo,@IssueName,@Category,@Priority,@Assignee,@RaisedBy,@ApplicationArea,@OpenDt,@CloseDt,@SAPIssueDumpStatus,@Resolution,@Comments	
		WHILE @@FETCH_STATUS = 0
		BEGIN
			--DECLARE @Block_ID int =(select block_ID from BuildingBlock where Block_Name =@BuldingBlock)
			--DECLARE @Phase_ID int =(select Id from PhaseMaster where PhaseName =@Phase)
			--DECLARE @Role_ID int =(select RoleId from RoleMaster where RoleName =@Role)
			--DECLARE @ApplicationArea_ID int =(select Id from ApplicationAreaMaster where ApplicationArea =@ApplicationArea)
			--DECLARE @Task_ID int =(select TaskId from TaskType where TaskName =@TaskType)
			--PRINT @Task_ID
			INSERT INTO SAPDumpIssuetrack 
			(Id,RunningID,IssueNo,IssueName,
			Category_Id,
			Priority_Id,
			Assignee,RaisedBy,
			ApplicationArea_Id,
			OpenDt,CloseDt,
			SAPIssueDumpStatus_Id,
			Project_Id,
			Resolution,Comments,
			Cre_By)
			values(NEWID(),@R_ID,@IssueNo,@IssueName
			,(select Id from SAPIssuetrackCategory where CategoryName =@Category)
			,(select Id from SAPIssuetrackpriority where PriorityName = @Priority)
			,@Assignee,@RaisedBy
			,(select Id from SAPIssuetrackApplicationarea where ApplicationAreaName = @ApplicationArea)
			--,(SELECT CONVERT(DATETIME,@OpenDt,103))
			--,(SELECT CONVERT(DATETIME,@CloseDt,103))
			,@OpenDt,@CloseDt
			,(select Id from SAPIssuetrackStatus where StatusName = @SAPIssueDumpStatus)
			,@ProjectId
			,@Resolution
			,@Comments
			,@Createdby
			)
			set @R_ID=@R_ID+1;
			FETCH NEXT FROM @MyCursor 
			INTO @IssueNo,@IssueName,@Category,@Priority,@Assignee,@RaisedBy,@ApplicationArea,@OpenDt,@CloseDt,@SAPIssueDumpStatus,@Resolution,@Comments	
		END; 
		CLOSE @MyCursor;
		DEALLOCATE @MyCursor;
	END
--	--BEGIN Transaction A
--	--		BEGIN TRY
			
--			SET NOCOUNT ON;
--		DECLARE @MyCursor CURSOR;DECLARE @MyCursorP1 CURSOR;DECLARE @CursorActivity CURSOR
--		DECLARE @Task varchar(500),@BuldingBlock varchar(500),@Phase varchar(500),@Role varchar(500),@ApplicationArea varchar(500),@TaskType varchar(500),@ParallelId varchar(500);
--		DECLARE @Seq int, @Sequence_num int, @Blockid int,@PhaseID int,@RoleID int,@ApplicationAreaID int,@TaskTypeID int;
--		DECLARE @LASTID UNIQUEIDENTIFIER =NEWID()
--		BEGIN
--			SET @MyCursor = CURSOR FOR
--			select  Task,BuildingBlock,Phase,[Role],ApplicationArea,TaskType,ParallelId from @ActivityMaster 

--			DECLARE @T1 TABLE (
--			[Activity_ID] [int] IDENTITY(1,1) NOT NULL,
--			[Task] [varchar](max)  NULL,
--			[BuildingBlock_id] [int] NULL,
--			[PhaseID] [int]  NULL,
--			[Sequence_Num] [int] NULL,
--			[ApplicationAreaID] [int]  NULL,
--			[RoleID] [int]  NULL,
--			[EST_hours] [decimal](4, 2) NULL,
--			[PM_Add] [bit] NULL,
--			[Task_id] [int] NULL,
--			ParallelId [varchar](max)  NULL
--			)
--			DECLARE @T2 TABLE (
--			[Activity_ID] [int] IDENTITY(1,1) NOT NULL,
--			[Task] [varchar](max)  NULL,
--			[BuildingBlock_id] [int] NULL,
--			[PhaseID] [int]  NULL,
--			[Sequence_Num] [int] NULL,
--			[ApplicationAreaID] [int]  NULL,
--			[RoleID] [int]  NULL,
--			[EST_hours] [decimal](4, 2) NULL,
--			[PM_Add] [bit] NULL,
--			[Task_id] [int] NULL,
--			ParallelId [varchar](max)  NULL
--			)
--			DECLARE @R1 TABLE (
--			[Activity_ID] [int] IDENTITY(1,1) NOT NULL,
--			[Task] [varchar](max)  NULL,
--			[BuildingBlock_id] [int] NULL,
--			[PhaseID] [int]  NULL,
--			[Sequence_Num] [int] NULL,
--			[ApplicationAreaID] [int]  NULL,
--			[RoleID] [int]  NULL,
--			[EST_hours] [decimal](4, 2) NULL,
--			[PM_Add] [bit] NULL,
--			[Task_id] [int] NULL,
--			ParallelId [varchar](max)  NULL
--			)

--			OPEN @MyCursor 
--			FETCH NEXT FROM @MyCursor 
--			INTO @Task,@BuldingBlock,@Phase,@Role,@ApplicationArea,@TaskType,@ParallelId
--			WHILE @@FETCH_STATUS = 0
--				 BEGIN
		
--				DECLARE @Block_ID int =(select block_ID from BuildingBlock where Block_Name =@BuldingBlock)
--				DECLARE @Phase_ID int =(select Id from PhaseMaster where PhaseName =@Phase)
--				DECLARE @Role_ID int =(select RoleId from RoleMaster where RoleName =@Role)
--				DECLARE @ApplicationArea_ID int =(select Id from ApplicationAreaMaster where ApplicationArea =@ApplicationArea)
--				DECLARE @Task_ID int =(select TaskId from TaskType where TaskName =@TaskType)
--				PRINT @Task_ID
--				insert INTO @T1 (
--				Task,
--				BuildingBlock_id,
--				PhaseID,
--				ApplicationAreaID,
--				EST_hours,
--				PM_Add,
--				RoleID,
--				Task_id,
--				ParallelId
--				)values( 
--				@Task,
--				@block_ID,
--				@Phase_ID,
--				@ApplicationArea_ID,
--				'0.00',
--				0,
--				@Role_ID,
--				@Task_ID,
--				@ParallelId
--				)
--			  /*
--				 YOUR ALGORITHM GOES HERE   
--			  */
--			  FETCH NEXT FROM @MyCursor 
--			  INTO @Task,@BuldingBlock,@Phase,@Role,@ApplicationArea,@TaskType,@ParallelId
--			END; 
--			CLOSE @MyCursor;
--			DEALLOCATE @MyCursor;

--			INSERT INTO @T2 (
--				Task,
--				BuildingBlock_id,
--				PhaseID,
--				ApplicationAreaID,
--				EST_hours,
--				PM_Add,
--				RoleID,
--				Task_id,
--				ParallelId
--				) select Task,BuildingBlock_id,PhaseID,ApplicationAreaID,EST_hours,PM_Add,RoleID,Task_id,ParallelId from @T1 ORDER BY  PhaseID
	
--			DECLARE @i int =0
--			WHILE @i<4
--			BEGIN
--				SET @i = @i + 1
	
--				SET @MyCursorP1 = CURSOR FOR
--				SELECT Task,BuildingBlock_id,PhaseID,Sequence_Num,ApplicationAreaID,RoleID,Task_id,ParallelId FROM @T2 WHERE PhaseID=@i

--				OPEN @MyCursorP1 
--				FETCH NEXT FROM @MyCursorP1 
--				INTO @Task,@Blockid,@PhaseID,@Sequence_num,@ApplicationAreaID,@RoleID,@TaskTypeID,@ParallelId

--				IF @i=1
--					SET @Seq=0
--				ELSE IF @i=2
--					SET @Seq=1000
--				ELSE IF @i=3
--					SET @Seq=2000
--				ELSE IF @i=4
--					SET @Seq=3000
	
--				WHILE @@FETCH_STATUS=0
--				BEGIN
--					set @Seq=@Seq+1;
	
--					INSERT INTO @R1 (
--					Task,
--					BuildingBlock_id,
--					PhaseID,
--					Sequence_Num,
--					ApplicationAreaID,
--					EST_hours,
--					PM_Add,
--					RoleID,
--					Task_id,
--					ParallelId
--					)values( 
--					@Task,
--					@Blockid,
--					@PhaseID,
--					@Seq,
--					@ApplicationAreaID,
--					'0.00',
--					0,
--					@RoleID,
--					@TaskTypeID,
--					@ParallelId
--					)

--					FETCH NEXT FROM @MyCursorP1 
--					INTO @Task,@Blockid,@PhaseID,@Sequence_num,@ApplicationAreaID,@RoleID,@TaskTypeID,@ParallelId

--				END
--				CLOSE @MyCursorP1 ;
--				DEALLOCATE @MyCursorP1;
--			END

--			DECLARE @Count int =(select COUNT(*) from AMVersion)
--			INSERT INTO AMVersion(Id,Version_Name,[FileName])VALUES(@LASTID,'V'+CAST(@Count+1 AS varchar(25)),@FileName)


--			--INSERT INTO AMVersion(Id)VALUES(NEWID ( ))
--			--DECLARE @AMVersionID uniqueidentifier = (SELECT @@IDENTITY)

--			SET @CursorActivity = CURSOR FOR
--			select  Task,BuildingBlock_id,PhaseID,Sequence_Num,ApplicationAreaID,RoleID,Task_id,ParallelId from @R1 


--			OPEN @CursorActivity 
--			FETCH NEXT FROM @CursorActivity 
--			INTO @Task,@Blockid,@PhaseID,@Sequence_num,@ApplicationAreaID,@RoleID,@TaskTypeID,@ParallelId
--			WHILE @@FETCH_STATUS = 0
--				BEGIN
				
--				--INSERT INTO AMVersion(Id)VALUES(NEWID())

--				INSERT INTO ActivityMaster
--				(Task,BuildingBlock_id,PhaseID,Sequence_Num,ApplicationAreaID,RoleID,EST_hours,PM_Add,Task_id,Version_Id,Parallel_Id,Cre_By)
--				Values
--				(@Task,@Blockid,@PhaseID,@Sequence_num,@ApplicationAreaID,@RoleID,'0.00','0',@TaskTypeID,@LASTID,@ParallelId,@Createdby)
				
--				FETCH NEXT FROM @CursorActivity 
--				INTO @Task,@Blockid,@PhaseID,@Sequence_num,@ApplicationAreaID,@RoleID,@TaskTypeID,@ParallelId
				
--				END
--				Update AMVersion SET isActive=1,isDeleted=0 WHERE id=@LASTID
--				--Update ActivityMasterTest set isActive=0,isdeleted=1 WHERE Version_id!=@LASTID 
--			CLOSE @CursorActivity;
--			DEALLOCATE @CursorActivity;

----			SELECT * FROM @R1
    




--		END;
--		--commit transaction A
--		--	END TRY
--		--	BEGIN CATCH 
--		--		rollback transaction A
--		--	END CATCH	
--	END
END