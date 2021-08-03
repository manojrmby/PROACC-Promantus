
CREATE PROCEDURE [dbo].[SP_Dashboard_Card]
	@Type varchar(50)=null,
	@InstanceID uniqueidentifier=null
AS
BEGIN
	IF @Type='Get_CardDetails'
	BEGIN
		DROP TABLE IF EXISTS tempdb.dbo.#ReturnTable_
		CREATE TABLE #ReturnTable_ (
		customerId uniqueidentifier,
		customerName varchar(500),

		ProjectId uniqueidentifier,
		ProjectName varchar(500),

		InstanceId uniqueidentifier,
		InstanceName varchar(500),

		ReadinessCheck int,

		PhaseId int,
		Total_Task int,
		Comp_Task int,
		Top5Con_ID varchar(Max),
		Top5Con_Name varchar(Max),
		
		Color nvarchar(MAX),
		Start_Dt nvarchar(MAX),
		End_Dt nvarchar(MAX),
		Completed int,
		TotalTask int,
		YetToStart int
		);

		DROP TABLE IF EXISTS tempdb.dbo.#Temp_Taskcount
				CREATE TABLE #Temp_Taskcount (
				Colour nvarchar(MAX),
				TotalTask int,
				Completed int,
				WIP int,
				ONHOLD int,
				YetToStart int ,
				StartDate nvarchar(MAX),
				EndDate nvarchar(MAX)
				);


		DECLARE @counter INT 
		DECLARE @co_total INT,@co_Comp int,@ReadinessCheck int
		DECLARE @ProjectName varchar(500),@InstanceName varchar(500),@Top5Con_ID varchar(500),@Top5Con_Name varchar(500),@customerName varchar(500)
		DECLARE @Proj_Id uniqueidentifier,@customerId uniqueidentifier

		DECLARE @Colors nvarchar(MAX),@Start_Dt nvarchar(MAX) ,@End_Dt nvarchar(MAX),@Completed_ int,@TotalTask_ int,@YetToStart_ int
		
		select @Proj_Id= p.Project_Id,@ProjectName=p.Project_Name,@InstanceName=i.InstaceName from Project p LEFT JOIN Instance I ON i.Project_ID=P.Project_Id where i.Instance_id=@InstanceID
		select @customerName=C.Company_Name ,@customerId=c.Customer_ID from Customer C LEFT JOIN Project P  ON C.Customer_ID=P.Customer_Id WHERE P.Project_Id=@Proj_Id
		select @ReadinessCheck=AssessmentUploadStatus from Instance where Instance_id=@InstanceID
		--print 'ProjectID='+CONVERT(varchar(100),@Proj_Id)
		--Print 'CustoID='+@customerName
		SET @counter=1
			WHILE ( @counter <= 4)
			BEGIN
				


				SET @co_total=0 SET @co_Comp=0 SET @Top5Con_ID='' SET @Top5Con_Name='' 
				select @co_total=COUNT(*) from ProjectMonitor where isActive=1 AND IsDeleted=0 AND  PhaseId=@counter AND InstanceID=@InstanceID AND  StatusId != (select id from statusmaster where statusname='Not Applicable')
				select @co_Comp=COUNT(*) from ProjectMonitor where isActive=1 AND IsDeleted=0 AND  PhaseId=@counter AND InstanceID=@InstanceID AND StatusId=1
				
				DROP TABLE IF EXISTS tempdb.dbo.#UserList
				select DISTINCT TOP(5)(P.USERID),M.[Name] INTO #UserList from ProjectMonitor P LEFT JOIN UserMaster M ON M.UserId=P.UserID
					where P.isActive=1 AND P.IsDeleted=0 AND  P.PhaseId=@counter AND P.InstanceID=@InstanceID AND P.USERID !='00000000-0000-0000-0000-000000000000'

				
				
				SELECT @Top5Con_ID =(STUFF((SELECT  ', ' + CONVERT(varchar(100),UserID) FROM(SELECT UserID FROM #UserList) AS T FOR XML PATH('')),1,1,'')),
						@Top5Con_Name=(STUFF((SELECT  ', ' + CONVERT(varchar(100),[Name])FROM(SELECT [Name] FROM #UserList) AS T FOR XML PATH('')),1,1,''))
				
				--select @Colour as Colour,@TotalTask As TotalTask, @Completed As Completed, @WIP As WIP,@ONHOLD As ONHOLD,@YetToStart As YetToStart,@StartDate As StartDate, @EndDate As EndDate
				

				--INSERT INTO #Temp_Taskcount EXEC SP_ResourceAllocation  @Type ='GetProgressMonitorTaskCount',@InstanceID=@InstanceID,@PhaseId=@counter  --GRAB DATE AND STATUS
				--SELECT @Color=Colour,@Start_Dt=StartDate,@End_Dt=EndDate from #Temp_Taskcount
				--TRUNCATE TABLE #Temp_Taskcount


				DECLARE @PhaseId int=@counter,@Id uniqueidentifier
				---CHECK ME 
				
					DECLARE @TotalTask int, @Completed int, @WIP int, @ONHOLD int,@YetToStart int, @StartDate nvarchar(MAX),
					@EndDate nvarchar(MAX),@PlannedEndDate nvarchar(MAX),@Colour nvarchar(MAX),@ActEndDate nvarchar(MAX),@PlanedEndDate nvarchar(MAX)

					select @TotalTask= COUNT(*) from ProjectMonitor where isActive=1 and phaseId=@PhaseId and InstanceID=@InstanceId And StatusId!=(select id from statusmaster where statusname='Not Applicable') 
					select @Completed= COUNT(*) from ProjectMonitor where isActive=1 and phaseId=@PhaseId and InstanceID=@InstanceId And StatusId=(select id from statusmaster where statusname='Completed') 
					select @WIP= COUNT(*) from ProjectMonitor where isActive=1 and phaseId=@PhaseId and InstanceID=@InstanceId And StatusId=(select id from statusmaster where statusname='Work In Progress') 
					select @ONHOLD= COUNT(*) from ProjectMonitor where isActive=1 and phaseId=@PhaseId and InstanceID=@InstanceId And StatusId=(select id from statusmaster where statusname='on hold') 
					select @YetToStart= COUNT(*) from ProjectMonitor where isActive=1 and phaseId=@PhaseId and InstanceID=@InstanceId And StatusId=(select id from statusmaster where statusname='Yet To Start') 
					select @StartDate=CONVERT(VARCHAR(15), cast(Cre_on as date), 106) from Instance where isActive=1 and Instance_id=@InstanceId
					set @EndDate=(select Top(1) CONVERT(VARCHAR(15), cast(Actual_En_Date as date), 106) from ProjectMonitor where InstanceID=@InstanceId order by Actual_En_Date desc)
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
					
					--select @Colour as Colour,@TotalTask As TotalTask, @Completed As Completed, @WIP As WIP,@ONHOLD As ONHOLD,@YetToStart As YetToStart,@StartDate As StartDate, @EndDate As EndDate
--CHECK ME 
	SET @Colors=@Colour
	SET @Start_Dt=@StartDate 
	SET @End_Dt=@EndDate
	SET @Completed_=@Completed
	SET @TotalTask_=@TotalTask
	SET @YetToStart_=@YetToStart









				--EXEC SP_ResourceAllocation  @Type ='GetProgressMonitorTaskCount',@InstanceID=@InstanceID,@PhaseId=@counter,N'@Colors nvarchar(MAX) output',@Colors output;

				INSERT INTO #ReturnTable_ (customerId,customerName,ProjectId,ProjectName, InstanceId,InstanceName,ReadinessCheck, PhaseId, Total_Task,Comp_Task,Top5Con_ID,Top5Con_Name
				,Color,Start_Dt,End_Dt,Completed,TotalTask,YetToStart) 
				SELECT @customerId,@customerName, @Proj_Id,@ProjectName,@InstanceID,@InstanceName,@ReadinessCheck, @counter, @co_total,@co_Comp,@Top5Con_ID,@Top5Con_Name
				,@Colors,@Start_Dt,@End_Dt,@Completed_,@TotalTask_,@YetToStart_


				--DECLARE @xmltmp xml = (SELECT * FROM #ReturnTable_ FOR XML AUTO)
				--PRINT CONVERT(NVARCHAR(MAX), @xmltmp)
				 
			SET @Counter  = @Counter  + 1
			END
			SELECT * from #ReturnTable_
	END
END

