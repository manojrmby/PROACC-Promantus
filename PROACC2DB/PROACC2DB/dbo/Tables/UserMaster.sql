CREATE TABLE [dbo].[UserMaster] (
    [UserId]      UNIQUEIDENTIFIER NOT NULL,
    [Name]        NVARCHAR (50)    NOT NULL,
    [EMail]       NVARCHAR (50)    NOT NULL,
    [Countrycode] NVARCHAR (50)    NULL,
    [DialCode]    NCHAR (10)       NULL,
    [Phone]       NVARCHAR (50)    NOT NULL,
    [LoginId]     VARCHAR (20)     NOT NULL,
    [Password]    NVARCHAR (150)   NOT NULL,
    [RoleID]      INT              NOT NULL,
    [UserTypeID]  INT              NOT NULL,
    [Customer_Id] UNIQUEIDENTIFIER CONSTRAINT [DF_UserMaster_Customer_Id] DEFAULT ('00000000-0000-0000-0000-000000000000') NULL,
    [isActive]    BIT              CONSTRAINT [DF_UserMaster_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]      DATETIME         CONSTRAINT [DF_UserMaster_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]      UNIQUEIDENTIFIER NOT NULL,
    [Modified_On] DATETIME         NULL,
    [Modified_by] UNIQUEIDENTIFIER NULL,
    [IsDeleted]   BIT              CONSTRAINT [DF_UserMaster_IsDeleted] DEFAULT ((0)) NOT NULL,
    [TS]          ROWVERSION       NOT NULL,
    CONSTRAINT [PK_User_1] PRIMARY KEY CLUSTERED ([UserId] ASC)
);






GO
Create TRIGGER [dbo].[Trg_UserMaster]
	ON dbo.UserMaster
	FOR DELETE, INSERT, UPDATE
	--FOR INSERT 
	AS
	BEGIN
		SET NOCOUNT ON
		DECLARE @UserId uniqueidentifier;
		DECLARE @Summary varchar(max);
		DECLARE @Tablename varchar(150)='UserMaster';
		DECLARE @TablID varchar(150);
		DECLARE @Action varchar(150);

		DECLARE @Operation varchar(7) = 
		CASE WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) 
			THEN 'UPDATE'
		WHEN EXISTS(SELECT * FROM inserted) 
			THEN 'INSERT'
		--WHEN EXISTS(SELECT * FROM deleted)
		--	THEN 'DELETE'
		ELSE 
			NULL --Unknown
		END;

		IF @Operation='UPDATE' OR @Operation='INSERT'
			BEGIN
				--SELECT * from UserMaster
				SET @Summary=(
				SELECT 
					ISNULL(CAST(i.Userid as varchar(max)),'')+','
					+ISNULL(CAST(i.[Name] as varchar(max)),'')+','
					+ISNULL(CAST(i.EMail as varchar(max)),'')+','
					+ISNULL(CAST(i.Countrycode as varchar(max)),'')+','
					+ISNULL(CAST(i.DialCode as varchar(max)),'')+','
					+ISNULL(CAST(i.Phone as varchar(max)),'')+','
					+ISNULL(CAST(i.loginid as varchar(max)),'')+','
					+ISNULL(CAST(i.[password] as varchar(max)),'')+','
					+ISNULL(CAST(i.[RoleID] as varchar(max)),'')+','
					+ISNULL(CAST(i.[UserTypeid] as varchar(max)),'')+','
					+ISNULL(CAST(i.[Customer_id] as varchar(max)),'')+','

					+ISNULL(CAST(i.isActive as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_on as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_By as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_On as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_by as varchar(max)),'')+','+
					ISNULL(CAST(i.IsDeleted as varchar(max)),'') AS summary
					from UserMaster A inner join inserted i on A.UserID=i.UserID)
				SELECT  
				@UserId= 
				CASE 
				WHEN @Operation='INSERT' OR @Operation='UPDATE' THEN  i.Cre_By
				ELSE i.Modified_by  END,
				@TablID=i.UserID,
				@Action=@Operation
				from UserMaster A inner join inserted i on A.UserID=i.UserID
			END
		Else IF @Operation='DELETE'
		BEGIN
		SET @Summary=(
				SELECT 
					ISNULL(CAST(i.Userid as varchar(max)),'')+','
					+ISNULL(CAST(i.[Name] as varchar(max)),'')+','
					+ISNULL(CAST(i.EMail as varchar(max)),'')+','
				    +ISNULL(CAST(i.Countrycode as varchar(max)),'')+','
					+ISNULL(CAST(i.DialCode as varchar(max)),'')+','
					+ISNULL(CAST(i.Phone as varchar(max)),'')+','
					+ISNULL(CAST(i.loginid as varchar(max)),'')+','
					+ISNULL(CAST(i.[password] as varchar(max)),'')+','
					+ISNULL(CAST(i.[RoleID] as varchar(max)),'')+','
					+ISNULL(CAST(i.[UserTypeid] as varchar(max)),'')+','
					+ISNULL(CAST(i.[Customer_id] as varchar(max)),'')+','

					+ISNULL(CAST(i.isActive as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_on as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_By as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_On as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_by as varchar(max)),'')+','+
					ISNULL(CAST(i.IsDeleted as varchar(max)),'') AS summary
					from deleted i)
			SET @UserId=(select cast(cast(0 as binary) as uniqueidentifier));
				SELECT  
				@TablID=i.UserID,
				@Action=@Operation
				from deleted i
		END
		IF @Operation IS NOT NULL
		BEGIN
		EXEC SP_Audit 
		@Type='AddToAduit',
		@Summary=@Summary,
		@UserId=@UserId,
		@Tablename=@Tablename,
		@TablID=@TablID,
		@Action=@Action
		END
	END