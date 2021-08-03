


CREATE PROCEDURE [dbo].[SP_PMTask]
	@Type varchar(50)=null,
	@PM_id uniqueidentifier=null,
	@ProjectID uniqueidentifier=null,
	@PMTaskId uniqueidentifier=null,
	@PMTaskName varchar(MAX)=null,
	@PMTaskCategoryID int=null,
	@EST_hours decimal(4,2)=null,
	@Actual_St_hours decimal(4,2)=null,
	@Cre_By uniqueidentifier=null,
	@isActive bit=null,
	@isDelete bit=null,
	@ModifiedBy uniqueidentifier=null,
	@LoginId uniqueidentifier=null,
	@Planed__St_Date datetime =null,
	@Planed__En_Date datetime =null,
	@Actual_St_Date datetime =null,
	@Actual_En_Date datetime =null,
	@StatusId int =null,
	@Notes varchar(MAX)=null,
	@Ts timestamp=null,
	-- Risk Assessment
	@RiskDescription varchar(MAX)=null,
	@RiskManagement varchar(MAX)=null,
	@Probability_Id int =null,
	@Consequence varchar(MAX)=null,
	@RiskClass_Id int =null,
	@RiskOwner_Id int =null,
	@Area varchar(MAX)=null,
	@RiskAssessment_ID uniqueidentifier=null

AS
BEGIN
	DECLARE @ID uniqueidentifier, @UserType int,@Name varchar(50),@Cont int;
	DECLARE @Date date=GETUTCDATE();
	IF @Type='GetPMTask'
	BEGIN
	SELECT @Cont= count(*) from PMTaskMonitor WHERE projectid=@ProjectID
	IF @Cont=0
		BEGIN
			INSERT INTO [dbo].[PMTaskMonitor ]
           ([Id]
           ,[PMTaskID]
           ,[ProjectId]
		   ,[StatusId]
		   ,[EST_hours]
		   ,[Planed__St_Date]
		   ,[Planed__En_Date]
		   ,[Actual_St_Date]
		   ,[Actual_En_Date]
		   ,[Cre_on]
		   ,[Cre_By]
           )
		SELECT 
		NEWID(),PMTaskid,@ProjectID,'5',EST_hours
		,@Date
		,[dbo].[WeakDayCount](GETUTCDATE(),(CAST(ROUND(([EST_hours]),0)AS INT)/8.001))
		--,DATEADD(DAY,CAST(ROUND(([EST_hours]),0)AS INT)/8.001,GETUTCDATE())
		--,DATEADD(DAY, CAST(ROUND(([EST_hours]/8),0) AS INT), GETUTCDATE())
		--,@Date
		,@Date
		,@Date
		,@Date
		,Cre_By
		from PMTaskMaster WHERE isActive=1 AND isDeleted=0  
		END
		Select PM.PMTaskName,P.*,PMC.[Name],
		(select StatusName from StatusMaster where Id=P.StatusId)[StatusName],
		CONVERT(VARCHAR(10), cast(P.Planed__St_Date as date), 103)[PlanedDate],
		CONVERT(VARCHAR(10), cast(P.Actual_St_Date as date), 103)[ActualDate],
		CONVERT(VARCHAR(10), cast(P.Planed__En_Date as date), 103)[PlanedEn_Date],
		CONVERT(VARCHAR(10), cast(P.Actual_En_Date as date), 103)[ActualEn_Date]
		from PMTaskMonitor P 
		LEFT JOIN PMTaskMaster PM ON PM.PMTaskId =P.PMtaskid 
		LEFT JOIN PMTaskCategory PMC on PMC.ID=PM.PMTaskCategoryID
		WHERE p.projectid=@ProjectID order by PM.PMTaskName 
	END

	
	IF @Type='GetPMTask1'
	BEGIN
		Select PM.PMTaskName,P.*,PMC.[Name],
		(select StatusName from StatusMaster where Id=P.StatusId)[StatusName],
		CONVERT(VARCHAR(10), cast(P.Planed__St_Date as date), 103)[PlanedDate],
		CONVERT(VARCHAR(10), cast(P.Actual_St_Date as date), 103)[ActualDate],
		CONVERT(VARCHAR(10), cast(P.Planed__En_Date as date), 103)[PlanedEn_Date],
		CONVERT(VARCHAR(10), cast(P.Actual_En_Date as date), 103)[ActualEn_Date]
		from PMTaskMonitor P 
		LEFT JOIN PMTaskMaster PM ON PM.PMTaskId =P.PMtaskid  
		LEFT JOIN PMTaskCategory PMC on PMC.ID=PM.PMTaskCategoryID
		WHERE p.projectid=@ProjectID order by PM.PMTaskName 
	END

	IF @Type='GetTaskDetail'
	BEGIN
		Select  ID ,Name From PMTaskCategory where isActive='1'
		select * from PMTaskMaster
	END

	IF @Type='Delete'
	BEGIN
	Update PMTaskMaster SET
	IsDeleted=@isDelete,
	isActive=@isActive
	Where PMTaskId=@PMTaskId
	END

	IF @Type='GetPMTaskMonitorById'
	BEGIN
	select *,
	(select StatusName from StatusMaster SM where PTM.StatusId=SM.Id) [StatusName],
	CONVERT(VARCHAR(11), cast(PTM.Planed__St_Date as date), 103)[minDate],
	CONVERT(VARCHAR(11), cast(PTM.Planed__St_Date as date), 106)[PlanedDate],
	CONVERT(VARCHAR(11), cast(PTM.Actual_St_Date as date), 106)[ActualDate],
	CONVERT(VARCHAR(11), cast(PTM.Planed__En_Date as date), 106)[PlanedEn_Date],
	CONVERT(VARCHAR(11), cast(PTM.Actual_En_Date as date), 106)[ActualEn_Date]
	from PMTaskMonitor PTM
	join PMTaskMaster PM on PM.PMTaskId=PTM.PMTaskID
	Where Id=@PMTaskId and PTM.isActive=1
	END
	
	If @Type='EditPMTaskById'	
	BEGIN
	SELECT *,
	(select Pm.Name From PMTaskCategory Pm where Pm.ID=a.PMTaskCategoryID)[PmTaskCatName]
	from PMTaskMaster a where a.isActive=1 and a.PMTaskId=@PMTaskId
	END

	
	If @Type='GetPMTaskById'	
	BEGIN
	SELECT a.PMTaskCategoryID,a.PMTaskId,
	(select Pm.Name From PMTaskCategory Pm where Pm.ID=a.PMTaskCategoryID)[PmTaskCatName]
	from PMTaskMaster a where a.isActive=1 
	END


	if @Type='PMAdd'
	BEGIN
	--Update PMTaskMaster set PMTaskName=@PMTaskName, PMTaskCategoryID=@PMTaskCategoryID, EST_hours=@EST_hours,Modified_by=@Cre_By,Modified_On=GETUTCDATE() where PMTaskId=@PMTaskId
	Insert into PMTaskMaster (PMTaskId,PMTaskName,PMTaskCategoryID,EST_hours,Cre_on,Cre_By,isActive) values(NEWID(),@PMTaskName,@PMTaskCategoryID,@EST_hours,GETUTCDATE(),@Cre_By,@isActive)
	END

	if @Type='PMUpdate'
	BEGIN
	DECLARE @t TABLE (myKey uniqueidentifier); 
	Update PMTaskMaster set PMTaskName=@PMTaskName, PMTaskCategoryID=@PMTaskCategoryID, EST_hours=@EST_hours,Modified_by=@ModifiedBy,Modified_On=GETUTCDATE() 
	OUTPUT inserted.PMTaskId INTO @t(myKey)
	where PMTaskId=@PMTaskId and TS=@Ts
	if (select count(*) from @t)>0
	BEGIN
	RETURN 0
	END
	ELSE IF(select count(*) from PMTaskMaster where PMTaskId=@PMTaskId)=0
	BEGIN
	RETURN 1
	END
	ELSE IF(select count(*) from PMTaskMaster where PMTaskId=@PMTaskId and TS=@Ts)=0
	BEGIN
	RETURN 2
	END

	END

	if @Type='PMData'
	BEGIN
	 select *,
	 (select [Name] from PMTaskCategory where ID=p.PMTaskCategoryID)[PMTaskCategory]
	 from PMTaskMaster p where isActive=1 order by Cre_on desc
	END

	if @Type='UpdatePMTaskByProjectID'
	BEGIN
	Update [PMTaskMonitor ] set isActive=0,IsDeleted=1,Modified_by=@Cre_By,Modified_On=GETUTCDATE() WHERE projectid=@ProjectID
	END
	
	if @Type='GetPMProjectList'
	BEGIN
	  select DISTINCT P.Project_Id,P.Project_Name from project P LEFT JOIN UserMaster UM on UM.UserId=P.ProjectManager_Id
	   where UM.UserId=@LoginId and P.isActive=1 order by P.Project_Name
	END

	if @Type='UpdatePMTaskMonitor'
	BEGIN
	  Update [PMTaskMonitor ] set EST_hours=@EST_hours,Actual_St_hours=@Actual_St_hours, StatusId=@StatusId ,Planed__St_Date=@Planed__St_Date, Planed__En_Date=@Planed__En_Date,
	  Actual_St_Date=@Actual_St_Date, Actual_En_Date=@Actual_En_Date,Notes=@Notes,Modified_by=@Cre_By,Modified_On=GETUTCDATE() where Id=@PM_id
	END

	IF @Type='RA_ProbabilityData'
	BEGIN
	select * from RA_Probability where isActive=1
	END

	IF @Type='RA_RiskClassData'
	BEGIN
	select * from RA_RiskClass where isActive=1
	END

	IF @Type='RA_RiskOwnerData'
	BEGIN
	select * from RA_RiskOwner where isActive=1
	END

	IF @Type='SubmitRiskAssessment'
	BEGIN
	--DECLARE @RiskId int =0
	--SET @RiskId=(select max(RiskId) as RId from RiskAssessment where Project_Id=@ProjectID)
	--PRint @RiskId
	DECLARE @RiskId int =0
	IF ((select max(RiskId) from RiskAssessment where Project_Id=@ProjectID) IS NULL)
	SET @RiskId=0
	ELSE IF((select max(RiskId) from RiskAssessment where Project_Id=@ProjectID) IS NOT NULL)
	SET @RiskId =(select max(RiskId) as RId from RiskAssessment where Project_Id=@ProjectID)
	PRINT @RiskId+1

	IF @RiskClass_Id=1
		SET @Consequence=1
	ELSE IF @RiskClass_Id=2
		SET @Consequence=2
	ELSE IF @RiskClass_Id=3
		SET @Consequence=3
	ELSE IF @RiskClass_Id=4
		SET @Consequence=5
	ELSE IF @RiskClass_Id=5
		SET @Consequence=8

	INSERT INTO RiskAssessment (ID,RiskId,Project_Id,RiskDescription,RiskManagement,Probability_Id, Consequence, RiskClass_Id,RiskOwner_Id,Area,Cre_By) 
	values (NEWID(),@RiskId+1,@ProjectID,@RiskDescription,@RiskManagement,@Probability_Id, @Consequence, @RiskClass_Id,@RiskOwner_Id,@Area,@Cre_By)
	END

	IF @Type='GetRiskAssessmentData'
	BEGIN
	select *,(select Probability from RA_Probability where Id=RA.Probability_Id) [Probability],
	(select [Risk Class] from RA_RiskClass where Id=RA.RiskClass_Id) [RiskClass],
	(select [Risk Owner] from RA_RiskOwner where Id=RA.RiskOwner_Id) [RiskOwner]
	from RiskAssessment RA where Project_Id=@ProjectID
	order by RA.RiskId 
	END

	IF @Type='GetRiskAssessmentDiagram'
	BEGIN
	select 
	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=1 and Consequence=1)a)[a1],
	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=1 and Consequence=2)a)[a2],
	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=1 and Consequence=3)a)[a3],
	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=1 and Consequence=5)a)[a4],
	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=1 and Consequence=8)a)[a5],

	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=2 and Consequence=1)a)[b1],
	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=2 and Consequence=2)a)[b2],
	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=2 and Consequence=3)a)[b3],
	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=2 and Consequence=5)a)[b4],
	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=2 and Consequence=8)a)[b5],

	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=3 and Consequence=1)a)[c1],
	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=3 and Consequence=2)a)[c2],
	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=3 and Consequence=3)a)[c3],
	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=3 and Consequence=5)a)[c4],
	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=3 and Consequence=8)a)[c5],

	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=4 and Consequence=1)a)[d1],
	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=4 and Consequence=2)a)[d2],
	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=4 and Consequence=3)a)[d3],
	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=4 and Consequence=5)a)[d4],
	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=4 and Consequence=8)a)[d5],

	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=5 and Consequence=1)a)[e1],
	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=5 and Consequence=2)a)[e2],
	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=5 and Consequence=3)a)[e3],
	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=5 and Consequence=5)a)[e4],
	(select STRING_AGG (a.str,',') FROM (SELECT CONCAT( 'R',RiskId ) as str from RiskAssessment where Project_Id=@ProjectID and Probability_Id=5 and Consequence=8)a)[e5]

		
	END

	IF @Type='GetRiskAssessmentById'
	BEGIN
	select * from RiskAssessment where ID=@RiskAssessment_ID and isActive=1
	END

	IF @Type='UpdateRiskAssessment'
	BEGIN
	IF @RiskClass_Id=1
		SET @Consequence=1
	ELSE IF @RiskClass_Id=2
		SET @Consequence=2
	ELSE IF @RiskClass_Id=3
		SET @Consequence=3
	ELSE IF @RiskClass_Id=4
		SET @Consequence=5
	ELSE IF @RiskClass_Id=5
		SET @Consequence=8

	Update RiskAssessment set RiskDescription=@RiskDescription,RiskManagement=@RiskManagement,Probability_Id=@Probability_Id, Consequence=@Consequence, 
	RiskClass_Id=@RiskClass_Id,RiskOwner_Id=@RiskOwner_Id,Area=@Area,Modified_by=@Cre_By, Modified_on=GETUTCDATE() where ID=@RiskAssessment_ID
	END

END
