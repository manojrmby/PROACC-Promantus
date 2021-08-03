
CREATE PROCEDURE [dbo].[SP_ActivitiesReport] 
	@Type Varchar(50),
	@Input varchar(50)=NULL,
	@InstanceId varchar(50)=null,
	@Phase varchar(50)=null,
	@condition varchar(50)=null
AS
BEGIN
	DECLARE @FIleuploadID varchar(50)
	SET @FIleuploadID=(SELECT top 1 ID from FileUploadMaster WHERE  InstanceID=@InstanceId AND FileType=(select id from fileMaster where [file]='Activities'))
	If @Type='GetDropdown'
	BEGIN
	   Select DISTINCT  [Phase] As Phase  from SAPInput_Activities  WHERE  FileUploadID=@FIleuploadID
	   Select DISTINCT  [Condition] As Condition  from SAPInput_Activities WHERE  FileUploadID=@FIleuploadID
	END
	IF @Type='ALL'
	BEGIN
		IF @Phase ='ALL' AND @condition='ALL'
		BEGIN
			SELECT [Activities] AS ACT_NAME,COUNT([Activities])as _Count
			FROM SAPInput_Activities WHERE  FileUploadID=@FIleuploadID  group by [Activities] order by ACT_NAME
		END
		IF @Phase !='ALL' AND @condition!='ALL'
		BEGIN
			SELECT [Activities] AS ACT_NAME,COUNT([Activities])as _Count
			FROM SAPInput_Activities WHERE  FileUploadID=@FIleuploadID AND Phase=@Phase AND Condition=@condition  group by [Activities] order by ACT_NAME
		END
		IF @Phase ='ALL' AND @condition!='ALL'
		BEGIN
			SELECT [Activities] AS ACT_NAME,COUNT([Activities])as _Count
			FROM SAPInput_Activities WHERE  FileUploadID=@FIleuploadID AND Condition=@condition  group by [Activities] order by ACT_NAME
		END
		IF @Phase !='ALL' AND @condition='ALL'
		BEGIN
			SELECT [Activities] AS ACT_NAME,COUNT([Activities])as _Count
			FROM SAPInput_Activities WHERE  FileUploadID=@FIleuploadID AND Phase=@Phase  group by [Activities] order by ACT_NAME
		END
		
	END
	--IF @Type='Single'
	--BEGIN
	--SELECT [LoB/Technology] AS LOB_NAME, count(*) as _Count
	--from SAPInput_SimplificationReport WHERE [LoB/Technology]=@Input AND  FileUploadID=@FIleuploadID group by [LoB/Technology] 
		
	--END
	IF @Type='ACT_Table'
	BEGIN
	IF @Phase ='ALL' AND @condition='ALL'
		BEGIN
			SELECT * FROM SAPInput_Activities WHERE  FileUploadID=@FIleuploadID
		END
	IF @Phase !='ALL' AND @condition!='ALL'
		BEGIN
			SELECT * FROM SAPInput_Activities WHERE  FileUploadID=@FIleuploadID AND Phase=@Phase AND Condition=@condition
		END
	IF @Phase ='ALL' AND @condition!='ALL'
		BEGIN
			SELECT * FROM SAPInput_Activities WHERE  FileUploadID=@FIleuploadID AND Condition=@condition
		END
	IF @Phase !='ALL' AND @condition='ALL'
		BEGIN
			SELECT * FROM SAPInput_Activities WHERE  FileUploadID=@FIleuploadID AND Phase=@Phase
		END
	END
END
