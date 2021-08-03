-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_Master] 
@Type varchar(50),
@Id uniqueidentifier=null,
@RoleName varchar(50)=null,
@PMtaskName varchar(50)=null,
@ProjectId uniqueidentifier=null,
@PmId uniqueidentifier=null,
@CustomerId uniqueidentifier=null,
@ScenarioId int=null,
@InstanceName varchar(50)=null,
@ProjectName  varchar(50)=null,
@RoleId int=null,
@UserId uniqueidentifier=null,
@Runningid int=null,
@Name varchar(50)=null,
@Email varchar(50)=null,
@Password varchar(50)=null,
@UserName varchar(50)=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF @Type='GetProjectManager'
	BEGIN
	Select Distinct  Userid,[Name] from UserMaster where isActive=1 and UserTypeID=4
	END
IF @Type='GetCustomerName'
	BEGIN
	Select Distinct Customer_ID,Company_Name from Customer where isActive=1 order by Company_Name
END

IF @Type='GetScenario'
	BEGIN
	Select Distinct ScenarioId,ScenarioName from ScenarioMaster where isActive='1'
	END

IF @Type='GetRole'
BEGIN
Select Distinct RoleName  From RoleMaster where RoleName=@RoleName and RoleId=@RoleId and isActive='1'
END

IF @Type='GetRoleAvailable'
BEGIN
Select Distinct RoleName  From RoleMaster where RoleName=@RoleName  and isActive='1'
END

IF @Type='GetPmTask'
BEGIN
Select Distinct PMTaskName  From PMTaskMaster where PMTaskName=@PMtaskName and PMTaskId=@PmId and isActive='1'
END

IF @Type='GetPmTaskAvailable'
BEGIN
Select Distinct PMTaskName  From PMTaskMaster where PMTaskName=@PMtaskName  and isActive='1'
END

IF @Type='GetProjectInstance'
BEGIN
Select Distinct InstaceName From Instance where Project_ID=@ProjectId and InstaceName=@InstanceName and Instance_id=@Id and isActive='1'
END

IF @Type='GetInstanceAvailability'
BEGIN
Select Distinct InstaceName From Instance where Project_ID=@ProjectId and InstaceName=@InstanceName  and isActive='1'
END


IF @Type='GetProjectName'
BEGIN
Select Distinct Project_Name From Project where Project_Name=@ProjectName and Project_Id=@Id  and isActive='1'
END

IF @Type='GetProjectAvailability'
BEGIN
Select Distinct Project_Name From Project where Project_Name=@ProjectName  and isActive='1'
END

IF @Type='GetRoleList'
BEGIN
Select Distinct * From RoleMaster where  isActive='1'
END

IF @Type='GetProjectManagerId'
BEGIN
select Distinct Top 1 P.ProjectManager_Id from ProjectMonitor PM join Instance I on PM.InstanceID=I.Instance_id
join Project P on I.Project_ID=P.Project_Id where P.isActive=1
END

IF @Type='GetProjectManagerEmail'
BEGIN
--Select Distinct Email from UserMaster where isActive=1 and RoleID=@RoleId and UserId=@UserId
select EMail from UserMaster where RoleID=(select RoleId from RoleMaster where RoleName='Project Manager') and 
UserId=(select Distinct P.ProjectManager_Id from ProjectMonitor PM join Instance I on PM.InstanceID=I.Instance_id 
join Project P on P.Project_Id=I.Project_ID where I.Instance_id=@Id)

END


IF @Type='GetEmailStatus'
BEGIN
Select MailStatus From MailMaster where Running_ID=@Runningid 
END

IF @Type='CheckNameEmail'
BEGIN
SELECT UserId, LoginId,EMail From UserMaster where isActive=1 and LoginId=@Name and EMail=@Email
END

IF @Type='GetResetList'
BEGIN
SELECT * From ResetPassword where isActive=1 and ResetId=@Id and Status=0
END

IF @Type='GetEmailList'
BEGIN
Select EMail,UserId from UserMaster where isActive=1
END

IF @Type='Passwordcheck'
BEGIN
Select [Password] from UserMaster where isActive=1 and [Password]=@Password and UserId=@UserId 
END
IF @Type='ResetPasswordcheck'
BEGIN
Select [Password] from UserMaster where isActive=1 and [Password]=@Password and LoginId=@UserName
END
IF @Type='GetStatus'
BEGIN
Select * from StatusMaster where isActive=1 
END
IF @Type='GetAllParallelType'
BEGIN
Select * from ParallelType where isActive=1  order by Parallel_Name
END
END

