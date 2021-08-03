
CREATE PROCEDURE [dbo].[SP_CustomCode]
	@Type varchar(50)=null,
	@InstanceId varchar(50)=null
AS
BEGIN
	DECLARE @FIleuploadID varchar(50)
	IF @Type='CustomTable'
	BEGIN
		SET @FIleuploadID=(SELECT top 1 ID from FileUploadMaster WHERE  InstanceID=@InstanceId AND FileType=(select id from fileMaster where [file]='CustomCode'))
		SELECT * FROM SAPInput_CustomCode WHERE  FileUploadID=@FIleuploadID
	END
END
