
CREATE PROCEDURE [dbo].[SP_ReadinessReport]
	@Type varchar(50)=null,
	@InstanceId varchar(50)=null
	
AS
BEGIN
	DECLARE @FIleuploadID varchar(50)
	DECLARE @Planed Date
	DECLARE @Actual Date
	IF @Type='Simple_Donut'
	BEGIN
		
		SET @FIleuploadID=(SELECT top 1 ID from FileUploadMaster WHERE  InstanceID=@InstanceId AND FileType=(select id from FileMaster where [File]='RelevantSimplificationIte'))
		
		SELECT
		(
		SELECT COUNT(*) AS RC  from SAPInput_SimplificationReport WHERE Relevance='Relevance to be Checked' AND FileUploadID=@FIleuploadID ) RC ,
		(
		SELECT COUNT(*)AS RC_NON from SAPInput_SimplificationReport Where Relevance='Relevance to be Checked (Non-strategic)'AND FileUploadID=@FIleuploadID)RC_NON ,
		(
		SELECT COUNT(*)AS R_NON from SAPInput_SimplificationReport Where Relevance='Relevant (Non-strategic)'AND FileUploadID=@FIleuploadID)R_NON ,
		(
		SELECT COUNT(*)AS R from SAPInput_SimplificationReport WHERE Relevance='Relevant'AND FileUploadID=@FIleuploadID)R 
	END
	IF @Type='Activities_Bar1'
	Begin
		
		SET @FIleuploadID=(SELECT top 1 ID from FileUploadMaster WHERE  InstanceID=@InstanceId AND FileType=(select id from FileMaster where [File]='Activities'))
		SELECT Activities AS ACT_NAME,COUNT(Activities)as _Count
			FROM SAPInput_Activities WHERE  FileUploadID=@FIleuploadID group by Activities
	END
	IF @Type='Activities_Bar2'
	Begin
		SET @FIleuploadID=(SELECT top 1 ID from FileUploadMaster WHERE  InstanceID=@InstanceId AND FileType=(select id from FileMaster where [File]='Activities'))
		SELECT Phase AS Phase,COUNT(Phase)as _Count
			FROM SAPInput_Activities WHERE  FileUploadID=@FIleuploadID group by Phase
	END
	IF @Type='Activities_Donut'
	Begin
	SET @FIleuploadID=(SELECT top 1 ID from FileUploadMaster WHERE  InstanceID=@InstanceId AND FileType=(select id from FileMaster where [File]='Activities'))
		SELECT Condition AS Condition,COUNT(Condition)as _Count
			FROM SAPInput_Activities WHERE FileUploadID=@FIleuploadID group by Condition
	END
	IF @Type='Fiori_Bar'
	BEGIN
		SET @FIleuploadID=(SELECT top 1 ID from FileUploadMaster WHERE  InstanceID=@InstanceId AND FileType=(select id from FileMaster where [File]='RecommendedFioriApp'))
		SELECT [Role] AS Application_Area,COUNT([Role])as _Count
			FROM SAPInput_FioriApps WHERE FileUploadID=@FIleuploadID group by [Role] order by _Count desc
	END
	IF @Type='CustomCode_Bar'
	BEGIN
		SET @FIleuploadID=(SELECT top 1 ID from FileUploadMaster WHERE  InstanceID=@InstanceId AND FileType=(select id from FileMaster where [File]='CustomCode'))
		SELECT [Status] AS _Status,COUNT([Status])as _Count
			FROM SAPInput_CustomCode WHERE FileUploadID=@FIleuploadID group by [Status] order by _Count desc
	END

	IF @Type='PlannedVsActual'
	BEGIN
			select id,ActivityID,A.Task,
			convert(varchar, Planed__St_Date, 111) AS Planed__St_Date,
			convert(varchar, Planed__En_Date, 111) AS Planed__En_Date,
			convert(varchar, Actual_St_Date, 111) AS Actual_St_Date,
			convert(varchar, Actual_En_Date, 111) AS Actual_En_Date,
			Actual_St_hours from ProjectMonitor PM 
			LEFT JOIN ActivityMaster A on PM.ActivityID=A.Activity_ID
			where InstanceID=@InstanceId order by Actual_En_Date

			--select @Planed= max(Planed__En_Date) from ProjectMonitor where InstanceID=@InstanceId
			--select @Actual= max(Actual_En_Date) from ProjectMonitor where InstanceID=@InstanceId

			--select @Planed,@Actual

	END
	IF @Type='Planned_Dates'
	BEGIN
		CREATE  TABLE #Return_Table(
		Id int,
		Phase varchar(50),
		Planed_StDate varchar(50),
		Planed_EnDate varchar(50)
		)

		DECLARE @COUNT INT ,@Planned_st datetime,@Planned_end datetime,@Phase varchar(50)
		SET @COUNT=1
		WHILE (@COUNT<=4)
		BEGIN

			select top(1) @Planned_st=Planed__St_Date from ProjectMonitor
		where PhaseId=@COUNT AND InstanceID=@InstanceId
		order by Planed__St_Date desc

			select top(1) @Planned_end=Planed__En_Date from ProjectMonitor
		where PhaseId=@COUNT AND InstanceID=@InstanceId
		order by Planed__En_Date desc

			if @COUNT=1
			BEGIN
				SET @Phase='Assessment'
			END
			ELSE if @COUNT=2
			BEGIN
				SET @Phase='Pre-Conversion'
			END
			ELSE if @COUNT=3
			BEGIN
				SET @Phase='Conversion'
			END
			ELSE if @COUNT=4
			BEGIN
				SET @Phase='Post-Conversion'
			END

		--SELECT @count as id,@Phase As Phase,@Planned_st AS ST,@Planned_end AS EN

		INSERT INTO #Return_Table values(@count,@Phase,convert(varchar, @Planned_st, 111),convert(varchar, @Planned_end, 111))

		SET @COUNT  = @COUNT  + 1
		END
		SELECT * from #Return_Table
	END
	IF @Type='ReadinssChaeckOrStatus'
	BEGIN
	DECLARE @Completed int, @Pending int,@UploadStatus bit

		SELECT @UploadStatus=AssessmentUploadStatus from Instance where Instance_id=@InstanceId
		select @Completed=count(*) from ProjectMonitor where InstanceID=@InstanceId  AND StatusId=1
		select @Pending=count(*) from ProjectMonitor where InstanceID=@InstanceId  AND StatusId NOT IN (3,1)

		select @Completed as Completed,@Pending as Pending,@UploadStatus as UploadStatus
	END

	IF @Type='ECC_HanaCount'
	BEGIN
	DECLARE @FileUpload uniqueidentifier
	SET @FileUpload=(SELECT top 1 ID from FileUploadMaster WHERE  InstanceID=@InstanceId AND FileType=(select id from FileMaster where [File]='SAPUserList') AND isActive=1 order by Cre_on desc)
	select
	(
	select COUNT(*) from SAPUserList where Last_Logon!='Not in Use' and 
	(CONVERT(date,Last_Logon) >= (DATEADD(year, -1, Cre_on))) and
	(CAST(Cre_on as date) >CAST(Valid_from as date) and CAST(Cre_on as date) <CAST(Valid_through as date) or (Valid_from IS NULL and Valid_through IS NULL) ) 
	and FileUploadID=@FileUpload) Hana,
	(
	select COUNT(*) as ECC from SAPUserList where FileUploadID=@FileUpload) ECC
	END

	IF @Type='ECC_HanaCount_Table'
	BEGIN
	DECLARE @FileType int
	SET @FileType=(select Id from FileMaster where [File]='SAPUserList' and isActive=1) 
	select * from SAPUserList where  FileUploadID=(SELECT top 1 ID from FileUploadMaster WHERE InstanceID=@InstanceId AND FileType=@FileType order by Cre_on desc)
	END

	IF @Type='BwExtractors'
	BEGIN
	DECLARE @FileType_ int
	SET @FileType_=(select Id from FileMaster where [File]='BwExtractors' and isActive=1) 
	select * from SAPInput_BWExtractors where  FileUploadID=(SELECT top 1 ID from FileUploadMaster WHERE InstanceID=@InstanceId AND FileType=@FileType_ order by Cre_on desc)
	END

	IF @Type='hanaDatabaseReport'
	BEGIN
	DECLARE @FileType__ int
	SET @FileType__=(select Id from FileMaster where [File]='HanaDatabaseTables' and isActive=1) 
	select * from SAPInput_HanaDatabaseTables where FileUploadID=(select top 1 ID from FileUploadMaster where InstanceID=@InstanceId AND FileType=@FileType__ order by Cre_on desc)
	END

END


--select * from Instance 
--update Instance set AssessmentUploadStatus=1 where Instance_id='729c44ea-3c36-44c0-8857-5cd0002a789d'  

--select * from ProjectMonitor where InstanceID='729c44ea-3c36-44c0-8857-5cd0002a789d'  AND StatusId=5
--select * from ProjectMonitor where InstanceID='729c44ea-3c36-44c0-8857-5cd0002a789d'  AND StatusId NOT IN (2,5)

--select * from StatusMaster
--select * from FileMaster