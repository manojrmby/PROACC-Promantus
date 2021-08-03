-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_User]
	@Type varchar(100) =null,
	@Id uniqueidentifier =null,
	@Name varchar(100) =null,
	@LoginId varchar(100) =null,
	@Password varchar(100) =null,
	@Countrycode varchar(Max) =null,
	@DialCode varchar(Max)=null,
	@Phone varchar(100) =null,
	@Email varchar(100) =null,
	@UserTypeID int =null,
	@RoleID int =null,
	@Customer_Id uniqueidentifier =null,
	@Cre_By uniqueidentifier =null,
	@Ts timestamp=null
AS
BEGIN
	
	SET NOCOUNT ON;
	if @Type='GetUsers'
	BEGIN
	select *,
	(select UserType from User_Type UT where UT.UserTypeID = UM.UserTypeID)[UserType],
	(select Company_Name from Customer C where C.Customer_ID = UM.Customer_Id)[Customer_Name],
	(select RoleName from RoleMaster R where R.RoleId = UM.RoleID)[RoleName]
	from UserMaster UM where [Name]!='Admin' order by isActive desc,Cre_on desc
	END	

	if @Type='GetUserTypeList'
	BEGIN
	select * from User_Type where isActive=1 order by UserType
	END

	if @Type='GetRoleMaster'
	BEGIN
	select * from RoleMaster where RoleName NOT IN ('Admin','Consultant','Project Manager','Customer') and isActive=1 order by RoleId
	END

	if @Type='userCreate'
	BEGIN
	if(@UserTypeID=(select UserTypeID from User_Type where UserType='Project Manager'))
	BEGIN
	set @RoleID=(select RoleId from RoleMaster where RoleName='Project Manager')
	END
	insert into UserMaster (UserId, [Name],LoginId,[Password],Countrycode,DialCode,Phone,Email,UserTypeID,RoleID,Customer_Id,Cre_By,Cre_on)
	values (NEWID(), @Name,@LoginId,@Password,@Countrycode,@DialCode,@Phone,@Email,@UserTypeID,@RoleID,@Customer_Id,@Cre_By,GETUTCDATE())
	END
	
	if @Type='NameAvailability'
	BEGIN
	select * from UserMaster where isActive=1 and [Name]=@Name
	END

	if @Type='NameAvailability1'
	BEGIN
	select * from UserMaster where isActive=1 and [Name]=@Name and UserId!=@Id
	END

	if @Type='UserNameAvailability'
	BEGIN
	select * from UserMaster where isActive=1 and LoginId=@LoginId
	END

	if @Type='UserNameAvailability1'
	BEGIN
	select * from UserMaster where isActive=1 and LoginId=@LoginId and UserId!=@Id
	END

	if @Type='GetUserById'
	BEGIN
	select * ,
	(select Company_Name from Customer C where C.Customer_ID= UM.Customer_Id) [Customer_Name],
	(select RoleName from RoleMaster R where R.RoleId= UM.RoleID) [RoleName]
	from UserMaster UM where UserId=@Id and isActive=1
	END
	
	if @Type='userUpdate'
	BEGIN
	DECLARE @t TABLE (myKey uniqueidentifier);  
	Update UserMaster set [Name]=@Name, @LoginId=@LoginId, [Password]=@Password, Phone=@Phone, Countrycode=@Countrycode ,DialCode=@DialCode,Email=@Email,UserTypeID=@UserTypeID,
	RoleID=@RoleID, Customer_Id=@Customer_Id, Modified_by= @Cre_By,Modified_On=GETUTCDATE() 
	OUTPUT inserted.UserId INTO @t(myKey)
	where UserId=@Id and TS=@Ts
	if (select count(*) from @t)>0
	BEGIN
	RETURN 0
	END
	ELSE IF(select count(*) from UserMaster where UserId=@Id)=0
	BEGIN
	RETURN 1
	END
	ELSE IF(select count(*) from UserMaster where UserId=@Id and TS=@Ts)=0
	BEGIN
	RETURN 2
	END
	END
	
	if @Type='DeleteUserById'
	Begin
	Declare @a int , @b int
	set @a=(select COUNT(PM.Id) from UserMaster UM join ProjectMonitor PM on UM.UserId=PM.UserID where UM.isActive=1 and PM.isActive=1 
			and UM.UserId=@Id and PM.StatusId not in (1,3))
	set @b=(select COUNT(P.Project_Id) from UserMaster UM join Project P on UM.UserId=P.ProjectManager_Id
			where UM.UserID=@Id and UM.isActive=1 and P.isActive=1)
	if(@a!=0 or @b!=0)
	BEGIN
	SELECT CAST(CAST(0 AS BINARY) AS UNIQUEIDENTIFIER)
	END
	Else
	BEGIN
	Update UserMaster set isActive=0,IsDeleted=1 where UserId=@Id
	END
	End
END



 