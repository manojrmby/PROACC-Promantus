CREATE PROCEDURE [dbo].[SP_Version]
	@Type varchar(50)=null,
	@VersionId uniqueidentifier=null,
	@VersionName varchar(50)=null,
	@Modified_By uniqueidentifier=null
	--@RoleName  varchar(50)=null,
	--@isActive bit=null,
	--@ModifiedBy uniqueidentifier=null,
	--@ModifiedOn datetime=null,   
	--@CreatedBy uniqueidentifier=null,
	--@CreatedOn datetime=null,
	--@Description varchar(50)=null,
	--@isDelete bit=null,
	--@phase int =null,
	--@TaskId int =null,
	--@Parallel_Id uniqueidentifier=null,
	--@Parallel_Name varchar(max)=null,
	
	--@Activity_ID int=null,
	--@Id int=null,
	--@Task varchar(250)=null,
	--@BuildingBlock_id int=null,
	--@PhaseID int=null,
	--@PreviousId int =null,
	--@ApplicationAreaID int=null,
	----@RoleID int =null,
	--@EST_hours decimal(4,2)=null,
	--@Cre_By varchar(50)=null,
	--@PM_Add bit=null,
	--@Task_Id int =null,
	--@InstanceID uniqueidentifier=null,
	--@VersionID uniqueidentifier=null,
	--@ParallelTypeName varchar(50)=null
	--@Parallel_Id uniqueidentifier =null,
	--@Parallel_Name varchar(100)=null
AS
BEGIN
IF @Type='GetVersion'
	BEGIN
		select Id,Version_Name,[filename],isActive,Cre_On from AMVersion order by Cre_On desc
	END
	
IF @Type='GetVersionbyid'
BEGIN
select Id,Version_Name,[filename],isActive from AMVersion where id=@VersionId 

END
IF @Type='GetVersionName'
	BEGIN
	Select Version_Name from AMVersion where Id=@VersionID
	END
	IF @Type='UpdateVersion'
	BEGIN
	Update AMVersion set Version_Name=@VersionName ,Modified_on=GETUTCDATE(),Modified_By=@Modified_By WHERE ID=@VersionID
	END
END