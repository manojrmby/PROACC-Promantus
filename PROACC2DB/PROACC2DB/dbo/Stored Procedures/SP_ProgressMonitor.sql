-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_ProgressMonitor] 
	-- Add the parameters for the stored procedure here
@Type varchar(50),
@EST_hours decimal(4,2) =null,
@Actual_St_hours decimal(4,2) =null,
@StatusId int =null,
@PhaseID int =null,
@InstanceID uniqueidentifier =null,
@Id uniqueidentifier =null,
@Task varchar(Max)=null,
@ActivityId varchar(50)=null,
@ApplicationAreaID int =null,
@UserID varchar(250)=null,
@PreviousId int =null,
@BuildingBlock_id int =null,
@Task_Id int =null,
@RoleId int=null,
@Parallel_Id uniqueidentifier =null,
@Parallel_Name varchar(100)=null,
@Cre_By uniqueidentifier =null,
@Notes varchar(250)=null,

@loginId uniqueidentifier =null,
@Planed__St_Date DateTime=null,
@Planed__En_Date DateTime=null,
@Actual_St_Date DateTime=null,
@Actual_En_Date DateTime=null,
@UserTypeID int=null,
@comments varchar(MAX)=null,
@Ts timestamp=null
AS
BEGIN
DECLARE  @VersionID uniqueidentifier=null
IF @Type='ProjectNamesByUser'
BEGIN
 IF @UserTypeID=1--admin
 BEGIN
    select * from project P where isActive=1 --join UserMaster UM on UM.Customer_Id=P.Customer_Id where UM.UserId = @loginId and P.isActive=1 order by Project_Name
	END

 ELSE IF @UserTypeID=3--Customer
 BEGIN
    select * from project P join UserMaster UM on UM.Customer_Id=P.Customer_Id where UM.UserId = @loginId and P.isActive=1 order by Project_Name
	END
ELSE IF @UserTypeID=4--Project Manager
 BEGIN
    select * from project P join UserMaster UM on UM.UserId=P.ProjectManager_Id where UM.UserId = @loginId and P.isActive=1 order by Project_Name
	END	
END

IF @Type='UpdateTask'
BEGIN
DECLARE @t TABLE (myKey uniqueidentifier);  
DECLARE @HoldStatus bit=0;  

if(select count(*) from ProjectMonitor where Id=@Id AND StatusId=4)>0
BEGIN
	SET @HoldStatus=1
END

update ProjectMonitor set EST_hours=@EST_hours,Actual_St_hours=@Actual_St_hours,Planed__St_Date=@Planed__St_Date,Planed__En_Date=@Planed__En_Date, Actual_St_Date=@Actual_St_Date,
Actual_En_Date=@Actual_En_Date, StatusId=@StatusId, Modified_by=@Cre_By, Modified_On=GETUTCDATE()
OUTPUT inserted.Id INTO @t(myKey)
where Id=@Id and isActive=1 and Ts=@Ts
IF @HoldStatus=1
	BEGIN
		EXEC SP_Mail @Type='ReleaseHoldAddMail', @PM_ID=@Id,@Cre_By=@Cre_By
	END
if (select count(*) from @t)>0
	BEGIN
	RETURN 0
	END
	ELSE IF(select count(*) from ProjectMonitor where Id=@Id)=0
	BEGIN
	RETURN 1
	END
	ELSE IF(select count(*) from ProjectMonitor where Id=@Id and TS=@Ts)=0
	BEGIN
	RETURN 2
	END

	
END

IF @Type='GetTaskFromPhase'
	Begin	
	select A.Activity_ID,A.Task from ActivityMaster A 
	join ProjectMonitor PM on A.Activity_ID=PM.ActivityID
	where A.PhaseID=@PhaseID and PM.InstanceID=@InstanceID
	order by Sequence_Num
	End

	IF @Type='GetTask'
	BEGIN
	Declare @PM_Add bit
	PRINT @Parallel_Id
	PRINT 'GetTask'
	set @VersionID=(select Version_ID from Instance where Instance_id=@InstanceID)
	exec SP_Activity @Type=[Add], @Task=@Task,@BuildingBlock_id=@BuildingBlock_id,@PhaseID=@PhaseId,
	@ApplicationAreaID=@ApplicationAreaID,@RoleID=@RoleId,@EST_hours=@EST_hours,@Cre_By=@Cre_By,@PM_Add=1,@PreviousId=@PreviousId,@VersionID=@VersionID,
	@Task_Id=@Task_Id,@Parallel_Id=@Parallel_Id,@Parallel_Name=@Parallel_Name

	
	set @ActivityId=(SELECT TOP 1 Activity_ID FROM ActivityMaster ORDER BY Activity_ID DESC)
	--set @ActivityId=SCOPE_IDENTITY()
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
	  ,Notes
	  ,Version_Id
      ,[Cre_By]
	  )
	 SELECT
	   NEWID()
	  ,[Activity_ID]
	  ,@InstanceID
	  ,[PhaseID]
	  ,RoleID
	  ,@UserID
	  ,(select id from StatusMaster WHERE StatusName='Yet To Start')
	  ,GETUTCDATE()
	  ,[dbo].[WeakDayCount](GETUTCDATE(),(CAST(ROUND(([EST_hours]),0)AS INT)/8.001))
	  --,DATEADD(DAY, CAST(ROUND(([EST_hours]/8),0) AS INT), GETUTCDATE())
	  --,GETUTCDATE()
	  ,GETUTCDATE()
	  ,GETUTCDATE()
	  ,[EST_hours]
	  ,@Notes
	  ,@VersionID
	  ,@Cre_By
	  FROM ActivityMaster where isactive=1 AND Activity_ID=@ActivityId and PM_Add=1
	END

	IF @Type='GetMasterAdd'
	BEGIN
		
		DECLARE @IDS nvarchar(max)
		Declare @sql nvarchar(max)
		Declare @BuildingBlock nvarchar(max)
		DECLARE @Count1 int
		SET @VersionID =(select Version_ID from Instance WHERE Instance_id=@InstanceID)

		SET @Count1=(select count(*) from ProjectMonitor WHERE  InstanceID =@InstanceID AND isActive=1)
		IF @Count1>0
			BEGIN
				SET @IDS=
				(SELECT STRING_AGG(t1.ActivityID, ',') As ActivityID  FROM ProjectMonitor t1
				where  t1.PhaseId= @PhaseID AND t1.InstanceID=@InstanceID AND t1.isActive=1)
				set @BuildingBlock= (select BuildingBlock_Id from ScenarioMaster where  ScenarioId=(select ScenarioId from Project where Project_Id=(select Project_ID from Instance where Instance_id=@InstanceID)))
				print @IDS
				print @PhaseID
				SET @sql='SELECT  * FROM ActivityMaster WHERE PhaseID='+CAST(@PhaseID as nvarchar(25))+' AND Activity_ID NOT IN('+@IDS+') AND BuildingBlock_id in ('+@BuildingBlock+') AND PM_Add!=1 AND isActive=1 AND Sequence_Num IS NOT NULL AND Version_ID='''+CAST(@VersionID as nvarchar(50))+'''  order by Sequence_Num'
				EXEC sp_executesql @sql
			END
		ELSE
			BEGIN
			SELECT * FROM ActivityMaster WHERE Activity_ID=0
			END
	END

	IF @Type='AddResource'
	BEGIN
	DECLARE @VID uniqueidentifier=(SELECT version_id from instance WHERE Instance_id=@InstanceID)
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
	  ,Version_Id
      ,[Cre_By]
	  )
	 SELECT
	   NEWID()
	  ,[Activity_ID]
	  ,@InstanceID
	  ,[PhaseID]
	  ,RoleID
	  ,'00000000-0000-0000-0000-000000000000'
	  ,(select id from StatusMaster WHERE StatusName='Yet To Start')
	  ,GETUTCDATE()
	  --,GETUTCDATE()
	  ,[dbo].[WeakDayCount](GETUTCDATE(),(CAST(ROUND(([EST_hours]),0)AS INT)/8.001))
	  ,GETUTCDATE()
	  ,DATEADD(DAY, CAST(([EST_hours]/8) AS INT), GETUTCDATE())
	  ,[EST_hours]
	  ,@VID
	  ,@Cre_By
	  FROM ActivityMaster where isactive=1 AND Activity_ID=@ActivityId and PM_Add=0
	END

	IF @Type='GetComments'
	BEGIN
	Select *,convert(varchar,c.cre_on,101) as coments_Date,
	(select [Name] from UserMaster UM where UM.UserId=c.[User_Id])[Name],
	(select [USERID] from UserMaster UM where UM.UserId=c.Cre_By)USERID
	from Comments c where ProjectMonitor_Id=@Id and isActive=1 order by Cre_on 
	END

	IF @Type='InsertComments'
	BEGIN
	Insert into Comments (Comments_Id,[User_Id],ProjectMonitor_Id,comments,Cre_on,Cre_By)
	values(NEWID(),@Cre_By,@Id,@comments,GETUTCDATE(),@Cre_By)
	
	END
END
