CREATE PROCEDURE [dbo].[SP_Login]
	@Type varchar(50)=null,
	@UserName varchar(50)=null,
	@Password varchar(50)=null,
	@UserId uniqueidentifier=null,

    @Status bit=null,
	@CreatedBy uniqueidentifier=null,
	@CreatedOn datetime=null,
	@ModifiedBy uniqueidentifier=null,
	@ModifiedOn datetime=null,
	@IsActive bit=null,

	@Browser varchar(50)=null,
	@IP varchar(50)=null,
	@MachineName varchar(50)=null,
	@Reset_Id uniqueidentifier=null,
	@LogId uniqueidentifier=null,
	@Email varchar(50)=null
AS
BEGIN
	DECLARE @ID uniqueidentifier, @UserType int,@Name varchar(50);
	DECLARE @ResetId uniqueidentifier=NEWID();
	
	IF @Type='Login'
	BEGIN
	DECLARE @cut int;
	DECLARE @NEW_ID uniqueidentifier=NEWID();
	SELECT  @cut= Count(*) from UserMaster where loginid=@UserName AND [password]=@Password AND isActive=1
	IF @cut=1
	BEGIN
	SELECT @ID=USERID from UserMaster where loginid=@UserName AND [password]=@Password AND isActive=1
		update Log_History SET isactive=0 ,Modified_On=GETUTCDATE() ,Modified_by=@ID Where USERID=@ID AND isactive=1
INSERT INTO [dbo].[Log_History]
           ([LogId]
           ,[UserId]
           ,[LoginTime]
           ,[Browser]
           ,[IP]
           ,[MachineName]
           ,[Cre_By]
           )
     values 
           (@NEW_ID,
           @ID,
           GETUTCDATE(),
		   @Browser,
		   @IP,
		   @MachineName,
		   @ID
		   )
		   
	END
	SELECT  USERID AS ID ,UserTypeID AS UserType,[Name],@NEW_ID AS logID from UserMaster where loginid=@UserName AND [password]=@Password AND isActive=1
	--	SELECT @ID=Id,@UserType=UserTypeID,@Name=[Name]  from Customer where UserName=@UserName AND [Password]=@Password
	--	IF @UserType IS NULL
	--		BEGIN
	--			SELECT @ID=Id,@UserType=UserTypeID,@Name=[Name]  from Consultant c  where UserName=@UserName AND [Password]=@Password
	--		END
	--SELECT @ID,@UserType,@Name
	SELECT * FROM User_Type
	END

	IF @Type='ResetPasswordStatus'
	BEGIN
	INSERT INTO ResetPassword
	(ResetId,UserId,CreatedBy)
	Values
	(@ResetId,@UserId,@CreatedBy)
	END
	
	IF @Type='ResetPassword'
	BEGIN
	 Update UserMaster set [Password] =@Password,Modified_by=@ModifiedBy,Modified_On=@ModifiedOn
			where UserId=@UserId
	update ResetPassword set [Status]=1,ModifiedBy=@ModifiedBy,ModifiedOn=@ModifiedOn where UserId=@UserId AND [Status]=0
	END


	IF @Type='FetchResetId'
	BEGIN
	Select ResetId FROM ResetPassword where UserId=@UserId AND IsActive=1 AND Status=0 order by CreatedOn desc
	END

	--IF @Type='Resetpwdbylink'
	--BEGIN
	--	Update UserMaster set [Password] =@Password,Modified_by=@ModifiedBy,Modified_On=@ModifiedOn
	--		where UserId=@UserId
	--		update ResetPassword set [Status]=1,ModifiedBy=@ModifiedBy,ModifiedOn=@ModifiedOn where UserId=@UserId --AND ResetId=@Reset_Id 
	--END
	IF @Type='Validate_Log'
	BEGIN
		SELECT * from Log_History WHERE LogId=@LogId AND isactive=1
	END
	IF @Type='Validate_Username'
	BEGIN
		SELECT LoginId from UserMaster WHERE LoginId=@LogId and EMail=@Email
	END
END
