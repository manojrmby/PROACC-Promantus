CREATE TABLE [dbo].[Instance] (
    [Instance_id]            UNIQUEIDENTIFIER NOT NULL,
    [InstaceName]            NVARCHAR (50)    NOT NULL,
    [Project_ID]             UNIQUEIDENTIFIER NOT NULL,
    [Version_ID]             NVARCHAR (50)    NOT NULL,
    [LastUpdated_Dt]         DATETIME         NOT NULL,
    [AssessmentUploadStatus] BIT              CONSTRAINT [DF_ProjectInstanceConfig_UploadStatus] DEFAULT ((0)) NOT NULL,
    [PreConvertionIsActive]  BIT              CONSTRAINT [DF_ProjectInstanceConfig_PreConvertionIsActive] DEFAULT ((0)) NOT NULL,
    [isActive]               BIT              CONSTRAINT [DF_ProjectInstanceConfig_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]                 DATETIME         CONSTRAINT [DF_ProjectInstanceConfig_Cre_on] DEFAULT (getdate()) NOT NULL,
    [Cre_By]                 UNIQUEIDENTIFIER NOT NULL,
    [Modified_On]            DATETIME         NULL,
    [Modified_by]            UNIQUEIDENTIFIER NULL,
    [IsDeleted]              BIT              CONSTRAINT [DF_ProjectInstanceConfig_IsDeleted] DEFAULT ((0)) NOT NULL,
    [Description]            VARCHAR (50)     NULL,
    [Sys_Landscape]          VARCHAR (50)     NULL,
    CONSTRAINT [PK__ProjectI__3214EC07A02CAF2C] PRIMARY KEY CLUSTERED ([Instance_id] ASC)
);








GO
CREATE TRIGGER [dbo].[Trg_Instance]
	ON [dbo].[Instance]
	FOR DELETE, INSERT, UPDATE
	AS
	BEGIN
		SET NOCOUNT ON
		DECLARE @UserId uniqueidentifier;
		DECLARE @Summary varchar(max);
		DECLARE @Tablename varchar(150)='Instance';
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
					ISNULL(CAST(i.Instance_id as varchar(max)),'')+','
					+ISNULL(CAST(i.InstaceName as varchar(max)),'')+','
					+ISNULL(CAST(i.Project_ID as varchar(max)),'')+','
					+ISNULL(CAST(i.Version_ID as varchar(max)),'')+','
					+ISNULL(CAST(i.LastUpdated_Dt as varchar(max)),'')+','
					+ISNULL(CAST(i.AssessmentUploadStatus as varchar(max)),'')+','
					+ISNULL(CAST(i.PreConvertionIsActive as varchar(max)),'')+','
					+ISNULL(CAST(i.[Description] as varchar(max)),'')+','+
					+ISNULL(CAST(i.Sys_Landscape as varchar(max)),'')+','+

					+ISNULL(CAST(i.isActive as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_on as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_By as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_On as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_by as varchar(max)),'')+','+
					ISNULL(CAST(i.IsDeleted as varchar(max)),'') AS summary
					from Instance A inner join inserted i on A.Instance_id=i.Instance_id)
				SELECT  
				@UserId= 
				CASE 
				WHEN @Operation='INSERT' OR @Operation='UPDATE' THEN  i.Cre_By
				ELSE i.Modified_by  END,
				@TablID=i.Instance_id,
				@Action=@Operation
				from Instance A inner join inserted i on A.Instance_id=i.Instance_id
			END
		Else IF @Operation='DELETE'
		BEGIN
		SET @Summary=(
				SELECT 
					ISNULL(CAST(i.Instance_id as varchar(max)),'')+','
					+ISNULL(CAST(i.InstaceName as varchar(max)),'')+','
					+ISNULL(CAST(i.Project_ID as varchar(max)),'')+','
					+ISNULL(CAST(i.Version_ID as varchar(max)),'')+','
					+ISNULL(CAST(i.LastUpdated_Dt as varchar(max)),'')+','
					+ISNULL(CAST(i.AssessmentUploadStatus as varchar(max)),'')+','
					+ISNULL(CAST(i.PreConvertionIsActive as varchar(max)),'')+','
					+ISNULL(CAST(i.[Description] as varchar(max)),'')+','+
					+ISNULL(CAST(i.Sys_Landscape as varchar(max)),'')+','+

					+ISNULL(CAST(i.isActive as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_on as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_By as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_On as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_by as varchar(max)),'')+','+
					ISNULL(CAST(i.IsDeleted as varchar(max)),'') AS summary
					from deleted i)
			SET @UserId=(select cast(cast(0 as binary) as uniqueidentifier));
				SELECT  
				@TablID=i.Instance_id,
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