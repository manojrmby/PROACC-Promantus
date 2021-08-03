
CREATE PROCEDURE [dbo].[SP_RFCFMReport] 
	@Type Varchar(50),
	@Input varchar(50)=NULL,
	@InstanceId varchar(50)=null,
	@Landscape varchar(100)=null,
	@FileUploadID_ uniqueidentifier=null
AS
BEGIN

	DECLARE @FIleuploadID varchar(50)
	if @Landscape='Manual'
	SET @FIleuploadID=(SELECT top 1 ID from FileUploadMaster WHERE  InstanceID=@InstanceId AND FileType=(select id from FileMaster where [File]='RFCFM') order by Cre_on desc)
	else if @Landscape='Automation'
	SET @FileUploadID= (SELECT top 1 ID from SAPAutomationDownload WHERE  InstanceID=@InstanceId AND AutoList_ID=(select id from SAPAutoList where [Name]='RFCFM') order by Cre_on desc)

	IF @Type='CoreSystemDetails'
	BEGIN
		SELECT Parameter,[Value]
		FROM SAPInput_RFCFM WHERE  FileUploadID=@FIleuploadID 
	END

	IF @Type='summery_sys_report'
	BEGIN
	SELECT
		(select [SP_Level] from SAPInput_INSTALLEDSOFTWARECOMPONENT where Component='SAP_APPL' AND FileUploadID=@FIleuploadID )SAPECC,	
		(select [Release] from SAPInput_INSTALLEDSOFTWARECOMPONENT where Component='SAP_BASIS' AND FileUploadID=@FIleuploadID ) NetWeaver,
		(SELECT [VAlue] from SAPInput_RFCFM WHERE Parameter='Unicode Exist' AND FileUploadID=@FIleuploadID) TypeVersion,
		(SELECT [VAlue] from SAPInput_RFCFM WHERE Parameter='database system' AND FileUploadID=@FIleuploadID) [Database], 
		(SELECT [VAlue] from SAPInput_RFCFM WHERE Parameter='operating system' AND FileUploadID=@FIleuploadID) OS 
	END

	IF @Type='InstalledSoftwareComponent'
	BEGIN
		SELECT * FROM SAPInput_INSTALLEDSOFTWARECOMPONENT WHERE  FileUploadID=@FIleuploadID 
	END
	
	IF @Type='InstalledProductVersions'
	BEGIN
		SELECT * FROM SAPInput_INSTALLEDPRODUCTVERSIONS WHERE  FileUploadID=@FIleuploadID 
	END

	IF @Type='SFW5REPORT'
	BEGIN
		SELECT * FROM SAPInput_SFW5REPORT WHERE  FileUploadID=@FIleuploadID 
	END

	IF @Type='BillingDocuments'
	BEGIN
	if @Landscape='Manual'
	 SET @FileUploadID_= (SELECT top 1 ID from FileUploadMaster WHERE  InstanceID=@InstanceId AND FileType=(select id from FileMaster where [File]='BillingDocuments') order by Cre_on desc)
	else IF @Landscape='Automation'
	 SET @FileUploadID_= (SELECT top 1 ID from SAPAutomationDownload WHERE  InstanceID=@InstanceId AND AutoList_ID=(select id from SAPAutoList where [Name]='BillingDocuments') and isActive=1 order by Cre_on desc)

		SELECT * FROM BillingDocuments WHERE  FileUploadID=@FileUploadID_
	END

	IF @Type='OrderDocuments'
	BEGIN
	
	if @Landscape='Manual'
	 SET @FileUploadID_= (SELECT top 1 ID from FileUploadMaster WHERE  InstanceID=@InstanceId AND FileType=(select id from FileMaster where [File]='OrderDocuments')   order by Cre_on desc)
	else IF @Landscape='Automation'
	 SET @FileUploadID_= (SELECT top 1 ID from SAPAutomationDownload WHERE  InstanceID=@InstanceId AND AutoList_ID=(select id from SAPAutoList where [Name]='OrderDocuments') and isActive=1 order by Cre_on desc)

		SELECT * FROM OrderDocuments WHERE  FileUploadID=@FileUploadID_
	END
	IF @Type='SalesDocuments'
	BEGIN
	
	if @Landscape='Manual'
	 SET @FileUploadID_= (SELECT top 1 ID from FileUploadMaster WHERE  InstanceID=@InstanceId AND FileType=(select id from FileMaster where [File]='SalesDocuments')  order by Cre_on desc)
	else IF @Landscape='Automation'
	 SET @FileUploadID_= (SELECT top 1 ID from SAPAutomationDownload WHERE  InstanceID=@InstanceId AND AutoList_ID=(select id from SAPAutoList where [Name]='SalesDocuments') and isActive=1 order by Cre_on desc)
		SELECT * FROM SalesDocuments WHERE  FileUploadID=@FileUploadID_
	END
	IF @Type='ComplexityAnalysis'
	BEGIN
		SELECT * FROM ComplexityAnalysis WHERE  FileUploadID=(SELECT top 1 ID from FileUploadMaster WHERE  InstanceID=@InstanceId AND FileType=(select id from FileMaster where [File]='ComplexityAnalysis') )
	END
	IF @Type='MaterialityScoreDocuments'
	BEGIN
		DECLARE @FileType int
		set @FileType=(select Id from FileMaster where [File]='MaterialityScore' and isActive=1)
		if @Landscape='Manual'
		 SET @FileUploadID_= (SELECT top 1 ID from FileUploadMaster WHERE  InstanceID=@InstanceId AND FileType=@FileType order by Cre_on desc)
		else IF @Landscape='Automation'
		 SET @FileUploadID_= (SELECT top 1 ID from SAPAutomationDownload WHERE  InstanceID=@InstanceId AND AutoList_ID=(select id from SAPAutoList where [Name]='MaterialityScore') and isActive=1 order by Cre_on desc)

		SELECT * FROM MaterialityScore WHERE  FileUploadID=@FileUploadID_
	END
END

