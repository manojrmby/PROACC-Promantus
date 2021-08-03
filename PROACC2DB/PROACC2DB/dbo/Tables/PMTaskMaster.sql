CREATE TABLE [dbo].[PMTaskMaster] (
    [PMTaskId]         UNIQUEIDENTIFIER NOT NULL,
    [PMTaskName]       VARCHAR (MAX)    NOT NULL,
    [PMTaskCategoryID] INT              NOT NULL,
    [EST_hours]        DECIMAL (4, 2)   NULL,
    [isActive]         BIT              CONSTRAINT [DF_PMTaskMaster_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]           DATETIME         CONSTRAINT [DF_PMTaskMaster_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]           UNIQUEIDENTIFIER CONSTRAINT [DF_PMTaskMaster_Cre_By] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Modified_On]      DATETIME         CONSTRAINT [DF_PMTaskMaster_Modified_On] DEFAULT (getutcdate()) NULL,
    [Modified_by]      UNIQUEIDENTIFIER NULL,
    [IsDeleted]        BIT              CONSTRAINT [DF_PMTaskMaster_IsDeleted] DEFAULT ((0)) NOT NULL,
    [TS]               ROWVERSION       NOT NULL,
    CONSTRAINT [PK_PMTaskMaster] PRIMARY KEY CLUSTERED ([PMTaskId] ASC),
    CONSTRAINT [FK_PMTaskMaster_PMTaskCategory] FOREIGN KEY ([PMTaskCategoryID]) REFERENCES [dbo].[PMTaskCategory] ([ID])
);






GO
Create TRIGGER [dbo].[Trg_PMTaskMaster]
	ON dbo.PMTaskMaster
	FOR DELETE, INSERT, UPDATE
	AS
	BEGIN
		SET NOCOUNT ON
		DECLARE @UserId uniqueidentifier;
		DECLARE @Summary varchar(max);
		DECLARE @Tablename varchar(150)='PMTaskMaster';
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
					ISNULL(CAST(i.PMTaskId as varchar(max)),'')+','
					+ISNULL(CAST(i.PMTaskName as varchar(max)),'')+','
					+ISNULL(CAST(i.PMTaskCategoryID as varchar(max)),'')+','
					+ISNULL(CAST(i.EST_hours as varchar(max)),'')+','
					
					+ISNULL(CAST(i.isActive as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_on as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_By as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_On as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_by as varchar(max)),'')+','+
					ISNULL(CAST(i.IsDeleted as varchar(max)),'') AS summary
					from PMTaskMaster A inner join inserted i on A.PMTaskId=i.PMTaskId)
				SELECT  
				@UserId= 
				CASE 
				WHEN @Operation='INSERT' OR @Operation='UPDATE'  THEN  i.Cre_By
				ELSE i.Modified_by  END,
				@TablID=i.PMTaskId,
				@Action=@Operation
				from PMTaskMaster A inner join inserted i on A.PMTaskId=i.PMTaskId
			END
		Else IF @Operation='DELETE'
		BEGIN
		SET @Summary=(
				SELECT 
					ISNULL(CAST(i.PMTaskId as varchar(max)),'')+','
					+ISNULL(CAST(i.PMTaskName as varchar(max)),'')+','
					+ISNULL(CAST(i.PMTaskCategoryID as varchar(max)),'')+','
					+ISNULL(CAST(i.EST_hours as varchar(max)),'')+','

					+ISNULL(CAST(i.isActive as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_on as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_By as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_On as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_by as varchar(max)),'')+','+
					ISNULL(CAST(i.IsDeleted as varchar(max)),'') AS summary
					from deleted i)
			SET @UserId=(select cast(cast(0 as binary) as uniqueidentifier));
				SELECT  
				@TablID=i.PMTaskId,
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