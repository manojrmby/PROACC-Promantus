
CREATE PROCEDURE [dbo].[SP_SimplificationReport] 
	@Type Varchar(50),
	@Input varchar(50)=NULL,
	@InstanceId varchar(50)=null
AS
BEGIN
	DECLARE @FIleuploadID varchar(50)
	
	SET @FIleuploadID=(SELECT top 1 ID from FileUploadMaster WHERE  InstanceID=@InstanceId AND FileType=(select id from FileMaster where [File]='RelevantSimplificationIte'))
	If @Type='GetDropdown'
	BEGIN
	   Select DISTINCT  [LoB/Technology] As LOB  from SAPInput_SimplificationReport  WHERE  FileUploadID=@FIleuploadID
	END
	IF @Type='ALL'
	BEGIN
		SELECT [LoB/Technology] AS LOB_NAME,COUNT([LoB/Technology])as _Count
		FROM SAPInput_SimplificationReport WHERE  FileUploadID=@FIleuploadID group by [LoB/Technology] 
	END
	IF @Type='Single'
	BEGIN
	SELECT [LoB/Technology] AS LOB_NAME, count(*) as _Count
	from SAPInput_SimplificationReport WHERE [LoB/Technology]=@Input AND  FileUploadID=@FIleuploadID group by [LoB/Technology] 
		
	END
	IF @Type='SR_Table_ALL'
	BEGIN
		SELECT * FROM SAPInput_SimplificationReport WHERE  FileUploadID=@FIleuploadID
	END
	IF @Type='SR_Table_Single'
	BEGIN
		SELECT * FROM SAPInput_SimplificationReport WHERE  FileUploadID=@FIleuploadID AND  [LoB/Technology]=@Input
	END
END

