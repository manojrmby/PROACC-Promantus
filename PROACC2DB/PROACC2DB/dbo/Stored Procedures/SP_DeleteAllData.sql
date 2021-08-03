


CREATE  PROCEDURE [dbo].[SP_DeleteAllData] 
	@Type Varchar(50)
AS
BEGIN
	IF @Type='DeleteALL'
	BEGIN
	--DISABLE TRIGGER Trg_Instance ON Instance;
	--DISABLE TRIGGER Trg_Project ON Project;  
	--DISABLE TRIGGER Trg_ProjectMonitor ON ProjectMonitor;
	--DISABLE TRIGGER Trg_UserMaster ON UserMaster;
	--DISABLE TRIGGER Trg_Customer ON Customer;
	--DISABLE TRIGGER Trg_Issuetrack ON Issuetrack
	--DISABLE TRIGGER Trg_ActivityMaster ON ActivityMaster

	delete HistoryLog
	delete Issuetrack
	delete Instance
	delete Project
	delete FileUploadMaster
	delete Comments
	delete ProjectMonitor
	delete SAPInput_Activities
	delete SAPInput_CustomCode
	delete SAPInput_FioriApps
	delete SAPInput_PreConvertion
	delete SAPInput_SimplificationReport
	delete PMTaskMaster
	delete [PMTaskMonitor ]
	
	delete Log_History
	delete UserMaster where UserId!='B40C3E77-2673-4DD3-A32B-CA42908DD6A0'
	delete Customer 
	
	--ENABLE TRIGGER Trg_Instance ON Instance; 
	--ENABLE TRIGGER Trg_Project ON Project;  
	--ENABLE TRIGGER Trg_ProjectMonitor ON ProjectMonitor;
	--ENABLE TRIGGER Trg_UserMaster ON UserMaster;
	--ENABLE TRIGGER Trg_Customer ON Customer;
	--ENABLE TRIGGER Trg_Issuetrack ON Issuetrack 
	--ENABLE TRIGGER Trg_ActivityMaster ON ActivityMaster

	TRUNCATE table  AuditData
	TRUNCATE table MailMaster
	truncate table exceptionlogging
	truncate table  ActivityMaster

Return 1
	END
	
END
