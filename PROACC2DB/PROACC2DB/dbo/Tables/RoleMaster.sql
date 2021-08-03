CREATE TABLE [dbo].[RoleMaster] (
    [RoleId]      INT              IDENTITY (1, 1) NOT NULL,
    [RoleName]    VARCHAR (50)     NOT NULL,
    [isActive]    BIT              CONSTRAINT [DF_RoleMaster_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]      DATETIME         CONSTRAINT [DF_RoleMaster_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]      UNIQUEIDENTIFIER CONSTRAINT [DF_RoleMaster_Cre_By] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Modified_On] DATETIME         CONSTRAINT [DF_RoleMaster_Modified_On] DEFAULT (getutcdate()) NULL,
    [Modified_by] UNIQUEIDENTIFIER NULL,
    [IsDeleted]   BIT              CONSTRAINT [DF_RoleMaster_IsDeleted] DEFAULT ((0)) NOT NULL,
    [Description] VARCHAR (50)     NULL,
    [TS]          ROWVERSION       NOT NULL,
    CONSTRAINT [PK_RoleMaster] PRIMARY KEY CLUSTERED ([RoleId] ASC)
);






GO
Create TRIGGER [dbo].[Trg_RoleMaster]
	ON dbo.RoleMaster
	FOR DELETE, INSERT, UPDATE
	AS
	BEGIN
		SET NOCOUNT ON
		DECLARE @UserId uniqueidentifier;
		DECLARE @Summary varchar(max);
		DECLARE @Tablename varchar(150)='RoleMaster';
		DECLARE @TablID varchar(150);
		DECLARE @Action varchar(150);

		DECLARE @Operation varchar(7) = 
		CASE WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) 
			THEN 'UPDATE'
		WHEN EXISTS(SELECT * FROM inserted) 
			THEN 'INSERT'
		WHEN EXISTS(SELECT * FROM deleted)
			THEN 'DELETE'
		ELSE 
			NULL --Unknown
		END;

		IF @Operation='UPDATE' OR @Operation='INSERT'
			BEGIN
				--SELECT * from UserMaster
				SET @Summary=(
				SELECT 
					ISNULL(CAST(i.RoleId as varchar(max)),'')+','
					+ISNULL(CAST(i.RoleName as varchar(max)),'')+','
					

					+ISNULL(CAST(i.isActive as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_on as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_By as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_On as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_by as varchar(max)),'')+','+
					ISNULL(CAST(i.IsDeleted as varchar(max)),'')+','+
				    ISNULL(CAST(i.[Description] as varchar(max)),'')+','AS summary
					from RoleMaster R inner join inserted i on R.RoleId=i.RoleId)
				SELECT  
				@UserId= 
				CASE 
				WHEN @Operation='INSERT' OR @Operation='UPDATE' THEN  i.Cre_By
				ELSE i.Modified_by  END,
				@TablID=i.RoleId,
				@Action=@Operation
				from RoleMaster R inner join inserted i on R.RoleId=i.RoleId
			END
		Else IF @Operation='DELETE'
		BEGIN
		SET @Summary=(
				SELECT 
					ISNULL(CAST(i.RoleId as varchar(max)),'')+','
					+ISNULL(CAST(i.RoleName as varchar(max)),'')+','
				--	+ISNULL(CAST(i.[Description] as varchar(max)),'')+','

					+ISNULL(CAST(i.isActive as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_on as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_By as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_On as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_by as varchar(max)),'')+','+
					ISNULL(CAST(i.IsDeleted as varchar(max)),'')+','+
					ISNULL(CAST(i.[Description] as varchar(max)),'')+',' AS summary
					from deleted i)
			SET @UserId=(select cast(cast(0 as binary) as uniqueidentifier));
				SELECT  
				@TablID=i.RoleId,
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