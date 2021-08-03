--select * from ScenarioMaster
CREATE PROCEDURE [dbo].[SP_Scenario]
	@Type varchar(50)=null,
	@ScenarioId int=null,
	@ScenarioName varchar(50) =null,
	@BuildingBlock_Id varchar(50) =null,
	@Cre_By varchar(50)=null,
	@ModifiedBy varchar(50)=null
AS
BEGIN

IF @Type='GetScenario'
	BEGIN
	SELECT ScenarioId,ScenarioName,BuildingBlock_Id from ScenarioMaster where isActive=1 AND IsDeleted=0 order by Cre_On desc
	END
IF @Type='EditScenarioById'
	BEGIN
	SELECT ScenarioId,ScenarioName from ScenarioMaster where ScenarioId=@ScenarioId AND isActive=1 AND IsDeleted=0
	END
IF @Type='GetBuildingBlock'
	BEGIN
	SELECT block_ID,Block_Name from BuildingBlock  where isActive=1 AND IsDeleted=0
	END
IF @Type='GetScenarioName'
	BEGIN
	if @ScenarioId>0
		BEGIN
		Select Distinct ScenarioName  From ScenarioMaster where ScenarioName=@ScenarioName AND ScenarioId!=@ScenarioId and isActive='1'
		END
	ELSE
		BEGIN
		Select Distinct ScenarioName  From ScenarioMaster where ScenarioName=@ScenarioName and isActive='1'
		END
	END
IF @Type='GetScenariobyid'
	BEGIN
	select ScenarioId,ScenarioName,BuildingBlock_Id from ScenarioMaster where ScenarioId=@ScenarioId AND isActive=1
	END
IF @Type='CreateScenario'
	BEGIN
	insert into ScenarioMaster (ScenarioName,BuildingBlock_Id,Cre_By)values(@ScenarioName,@BuildingBlock_Id,@Cre_By)

	END
IF @Type='UpdateScenario'
	BEGIN
	Update ScenarioMaster set ScenarioName=@ScenarioName , BuildingBlock_Id=@BuildingBlock_Id,Modified_On=GETDATE(),Modified_by=@ModifiedBy where ScenarioId=@ScenarioId
	RETURN 0
	END
IF @Type='Sp_DeleteScenario'
	BEGIN
	Update ScenarioMaster set isActive=0,IsDeleted=0, Modified_On=GETDATE(),Modified_by=@ModifiedBy where ScenarioId=@ScenarioId
	RETURN 0
	END
END
IF @Type='GetScenario_Status'
	BEGIN
		select DISTINCT(ScenarioId) from Project where Project_Id IN(select DISTINCT(Project_ID) from Instance where Instance_id in(select DISTINCT(InstanceID) from ProjectMonitor where isActive=1 AND StatusId !=1) )
	END