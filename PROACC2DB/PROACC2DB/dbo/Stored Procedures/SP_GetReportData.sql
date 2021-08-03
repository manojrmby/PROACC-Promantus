-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_GetReportData]
	-- Add the parameters for the stored procedure here
	@Type varchar(50),
	@InstunceID uniqueidentifier=null
AS
BEGIN
	IF @Type='GetReportData'
	BEGIN
SELECT *,A.Sequence_Num as Sequence_Num,
	CONVERT(VARCHAR(10), cast(P.Planed__St_Date as date), 103)[PlanedDate],
	CONVERT(VARCHAR(10), cast(P.Actual_St_Date as date), 103)[ActualDate],
	CONVERT(VARCHAR(10), cast(P.Planed__En_Date as date), 103)[PlanedEn_Date],
	CONVERT(VARCHAR(10), cast(P.Actual_En_Date as date), 103)[ActualEn_Date],
(select Block_Name from buildingBlock BB where BB.block_ID = A.BuildingBlock_id) [BuildingBlock],
(select PhaseName from PhaseMaster PM where PM.Id=P.PhaseId) [Phase],
(select [Name] from UserMaster U where U.UserId=P.UserID)[Owner],
(select RoleName from RoleMaster R where R.RoleId=P.RoleId)[RoleName],
(select ApplicationArea from ApplicationAreaMaster AAM where AAM.Id=A.ApplicationAreaID)[ApplicationArea],
(select StatusName from StatusMaster SM where SM.Id= P.StatusId)[Status]
from   ProjectMonitor P LEFT JOIN ActivityMaster A On A.Activity_ID=P.ActivityID
	WHERE InstanceID=@InstunceID AND P.isActive=1 AND A.Sequence_Num IS NOT NULL order by A.Sequence_Num
	END

	IF @Type='GetReportDataPDF'
	BEGIN
	--SELECT *,A.Sequence_Num as Sequence_Num from   ProjectMonitor P LEFT JOIN ActivityMaster A On A.Activity_ID=P.ActivityID
	--WHERE InstanceID=@InstunceID AND P.isActive=1 AND A.Sequence_Num IS NOT NULL order by A.Sequence_Num
	SELECT *,A.Sequence_Num as Sequence_Num,--A.Task,
	CONVERT(VARCHAR(10), cast(P.Planed__St_Date as date), 103)[PlanedDate],
	CONVERT(VARCHAR(10), cast(P.Actual_St_Date as date), 103)[ActualDate],
	CONVERT(VARCHAR(10), cast(P.Planed__En_Date as date), 103)[PlanedEn_Date],
	CONVERT(VARCHAR(10), cast(P.Actual_En_Date as date), 103)[ActualEn_Date],
	(select PhaseName from PhaseMaster where Id=P.PhaseId) [Phase],
	(select ApplicationArea from ApplicationAreaMaster where Id=A.ApplicationAreaID) [Applicationarea],
	(select Block_Name from BuildingBlock where block_ID=A.BuildingBlock_id) [BuildingBlock],
	(select [Name] from UserMaster where UserId=P.UserID) [Owner],
	(select [StatusName] from StatusMaster where Id=P.StatusId) [Status]
	from ProjectMonitor P LEFT JOIN ActivityMaster A On A.Activity_ID=P.ActivityID
	WHERE InstanceID=@InstunceID AND P.isActive=1 AND A.Sequence_Num IS NOT NULL order by A.Sequence_Num
	END


END
