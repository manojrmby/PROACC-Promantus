
CREATE PROCEDURE [dbo].[SP_FioriAppsReport] 
	@Type Varchar(50),
	@Input varchar(50)=NULL,
	@InstanceId varchar(50)=null
	--@Phase varchar(50)=null,
	--@condition varchar(50)=null
AS
BEGIN
	DECLARE @FIleuploadID varchar(50)
	SET @FIleuploadID=(SELECT top 1 ID from FileUploadMaster WHERE  InstanceID=@InstanceId AND FileType=(select id from FileMaster where [File]='RecommendedFioriApp'))
	If @Type='GetDropdown'
	BEGIN
	   Select DISTINCT  [Role] As Roles  from SAPInput_FioriApps  WHERE  FileUploadID=@FIleuploadID
	END
	IF @Type='ALL'
	BEGIN
		IF @Input ='ALL'
		BEGIN
			SELECT [Role] AS Roles,COUNT([Role])as _Count
			FROM SAPInput_FioriApps WHERE  FileUploadID=@FIleuploadID  group by [Role] order by Roles
		END
		IF @Input !='ALL'
		BEGIN
			SELECT [Role] AS Roles,COUNT([Role])as _Count
			FROM SAPInput_FioriApps WHERE  FileUploadID=@FIleuploadID AND [Role]=@Input group by [Role] order by Roles
		END
		
		
	END
	
	IF @Type='FioriApps_Table'
	BEGIN
		SELECT * FROM SAPInput_FioriApps WHERE  FileUploadID=@FIleuploadID
	END
	IF @Type='FioriApps_Table_Single'
	BEGIN
		SELECT * FROM SAPInput_FioriApps WHERE  FileUploadID=@FIleuploadID AND  [Role]=@Input
	END
END

