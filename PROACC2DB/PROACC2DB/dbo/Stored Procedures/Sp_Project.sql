-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Sp_Project] 
	-- Add the parameters for the stored procedure here
@Type varchar(50),
@CustomerId uniqueidentifier=null,
@ProjectName varchar(150)=null,
@Description varchar(150)=null,
@ProjectManagerID uniqueidentifier=null,
@Project_ID uniqueidentifier=null,
@Scenario_ID int=null,
@Cre_By uniqueidentifier=null,
@Cre_On datetime=null,
@ModifiedOn datetime=null,
@ModifiedBy uniqueidentifier=null,
@isActive bit=null,
@isDelete bit=null,
@RoleId int=null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
 IF @Type='CreateProject'
	BEGIN
		Declare @EmailTo varchar(100),@ProjectId uniqueidentifier =NewId(),@User uniqueidentifier
		
		Insert into Project(Project_Id,Project_Name,Customer_Id,ProjectManager_Id,ScenarioId,Cre_By,Cre_on,Description,isActive) 
		  values (@ProjectId,@ProjectName,@CustomerId,@ProjectManagerID,@Scenario_ID,@Cre_By,GETUTCDATE(),@Description,@isActive)

		set @EmailTo=(select [EMail] from UserMaster where UserId=(select ProjectManager_Id from Project where Project_Id=@ProjectId))
	    set @User=(select ProjectManager_Id from Project where Project_Id=@ProjectId)

		 EXEC SP_Mail @Type='ProjectCreate',@UserID=@User,@Q_Mail= @EmailTo,@ProjectID = @ProjectId ,@Cre_By=@Cre_By

	END


	IF @Type='UpdateProject'
	BEGIN
		Update Project SET
	Project_Name=@ProjectName,
	ProjectManager_Id=@ProjectManagerID,
	Description=@Description,
	Modified_On=GETUTCDATE(),
	Modified_by=@ModifiedBy,
	isActive=@isActive,
	ScenarioId=@Scenario_ID,
	Customer_Id=@CustomerId
	Where Project_Id=@Project_ID
		END

IF @Type='Delete'  
	BEGIN
	Declare @PmCount int,@Ins_Count int
	SET @Ins_Count=(Select Count(I.Instance_id) From Instance I where I.Project_ID=@Project_ID and I.isActive=1)
	SET @PmCount=(select COUNT(*) from [PMTaskMonitor ] where isActive=1 and ProjectId=@Project_ID and StatusId not in(1,3))
	if @PmCount=0 and @Ins_Count=0
	BEGIN
	Update Project SET isActive=0,IsDeleted=1 Where Project_Id=@Project_ID
	END
	Else
	BEGIN
	Select @Ins_Count as InsCount,@PmCount as PMCount
	END
	END
IF @Type='GetProjectDetail'
	BEGIN

Select *,
(select u.Name from  UserMaster u  where u.UserId = a.ProjectManager_Id)[ProjectManagerName],
(select b.ScenarioName from ScenarioMaster b where b.ScenarioId=a.ScenarioId)[ScenarioName],
(select c.Company_Name from Customer c where c.Customer_ID=a.Customer_Id)[Company_Name],
(select Block_Name + ',' from BuildingBlock where block_ID in (select * from STRING_SPLIT((select BuildingBlock_Id from ScenarioMaster where ScenarioId=1), ',')) for Xml Path ('') )[BuildingBlockName],
(select CASE WHEN (select count(*) from ProjectMonitor where InstanceID in (select Instance_id from Instance where Project_ID=@Project_ID) AND isActive=1)>0 THEN 'TRUE' ELSE 'FALSE'END ) [Scenario_Status]
from Project a where a.isActive=1 and a.Project_Id=@Project_ID

END

IF @Type='GetProjectDrpList'
BEGIN
Select Project_Id,Project_Name
from Project a where a.isActive='1'and a.Project_Id=@Project_ID
END


IF @Type='InstanceCount'
BEGIN
Select Count(I.Instance_id) as [Count] From Instance I where I.Project_ID=@Project_ID and I.isActive='1'
END

IF @Type='RoleCount'
Begin
Select Count(U.RoleID) as [RId] from UserMaster U where U.RoleID=@RoleId and U.isActive=1
End

IF @Type='GetProject'
BEGIN
Select *,
(select u.Name from  UserMaster u  where u.UserId = a.ProjectManager_Id)[ProjectManagerName],
(select b.ScenarioName from ScenarioMaster b where b.ScenarioId=a.ScenarioId)[ScenarioName],
(select c.Company_Name from Customer c where c.Customer_ID=a.Customer_Id)[Company_Name],
(select Count(I.Instance_id) From Instance I where I.Project_ID=a.Project_Id and I.isActive='1')[Count],
(select Count(P.Project_Id) From Project P where P.isActive='1')[ProjCount]
from Project a where a.isActive=1 order by Cre_on desc

END

IF @Type='GetInstance'
BEGIN
SELECT Distinct * , 
(select u.Name from  UserMaster u  where u.UserId = a.Cre_By)[UserName],
(select p.Project_Id from  Project p  where p.Project_Id = a.Project_ID)[PID],
(select V.Version_Name from  AMVersion V  where V.ID = a.Version_ID)[Version_Name],
(select V.ID from  AMVersion V  where V.ID = a.Version_ID)[VID]
from Instance a where a.isActive=1 order by Cre_on desc
END

IF @Type='GetScenario_BuildingBlock'
BEGIN
DECLARE @BuildingBlock_Id varchar(50)
set @BuildingBlock_Id=(select BuildingBlock_Id from ScenarioMaster where ScenarioId=@Scenario_ID)
select Block_Name from BuildingBlock where block_ID in (select * from STRING_SPLIT(@BuildingBlock_Id, ','))
END

END
