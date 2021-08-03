
CREATE PROCEDURE [dbo].[SP_Role]
	@Type varchar(50)=null,
	@RoleId int=null,
	@RoleName  varchar(50)=null,
	@isActive bit=null,
	@ModifiedBy uniqueidentifier=null,
	@ModifiedOn datetime=null,
	@CreatedBy uniqueidentifier=null,
	@CreatedOn datetime=null,
	@Description varchar(50)=null,
	@isDelete bit=null,
	@UserName varchar(50)=null,
	@Ts timestamp=null
	
	--@Password varchar(50)=null
AS
BEGIN

	IF @Type='CreateRole'
	BEGIN
		Insert into RoleMaster(RoleName,isActive,Cre_By,Cre_On,Description) 
		  values (@RoleName,@isActive,@CreatedBy,GETUTCDATE(),@Description)
	END
	
	
	IF @Type='GetRoleDetail'
	BEGIN
	Select *from RoleMaster Where isActive='1' and RoleId=@RoleId
	END

	IF @Type='GetRole'
	BEGIN
	Select *from RoleMaster Where isActive='1' and RoleId!=1 and RoleId!=2 and RoleId!=3 and RoleId!= 4
	END
	
	IF @Type='UpdateRole'
	BEGIN
	DECLARE @t TABLE (myKey int);  
	Update RoleMaster SET RoleName=@RoleName,Modified_by=@ModifiedBy,[Description]=@Description,Modified_On=GETUTCDATE(),isActive=@isActive
	OUTPUT inserted.RoleId INTO @t(myKey)
	Where RoleId=@RoleId and TS=@Ts
	if (select count(*) from @t)>0
	BEGIN
	RETURN 0
	END
	ELSE IF(select count(*) from RoleMaster where RoleId=@RoleId)=0
	BEGIN
	RETURN 1
	END
	ELSE IF(select count(*) from RoleMaster where RoleId=@RoleId and TS=@Ts)=0
	BEGIN
	RETURN 2
	END

	END
	
	IF @Type='Delete'
	BEGIN
	Update RoleMaster SET
	IsDeleted=@isDelete,
	isActive=@isActive
	Where RoleId=@RoleId
	END
	IF @Type='LoginRole'
	BEGIN
		DECLARE @UserType int;
		select @UserType=usertypeid from UserMaster  where isActive=1 AND IsDeleted=0 AND loginid=@UserName
		SELECT UserType from User_Type   where isActive=1 AND  UserTypeid=@UserType
	END
	
	
END
