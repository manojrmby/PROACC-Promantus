CREATE TABLE [dbo].[Project] (
    [Project_Id]        UNIQUEIDENTIFIER NOT NULL,
    [Project_Name]      NVARCHAR (200)   NOT NULL,
    [Description]       NVARCHAR (MAX)   NULL,
    [Customer_Id]       UNIQUEIDENTIFIER NOT NULL,
    [ProjectManager_Id] UNIQUEIDENTIFIER NOT NULL,
    [ScenarioId]        INT              NULL,
    [isActive]          BIT              CONSTRAINT [DF_Project_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]            DATETIME         CONSTRAINT [DF_Project_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]            UNIQUEIDENTIFIER NOT NULL,
    [Modified_On]       DATETIME         NULL,
    [Modified_by]       UNIQUEIDENTIFIER NULL,
    [IsDeleted]         BIT              CONSTRAINT [DF_Project_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED ([Project_Id] ASC),
    CONSTRAINT [FK_Custome] FOREIGN KEY ([Customer_Id]) REFERENCES [dbo].[Customer] ([Customer_ID]),
    CONSTRAINT [FK_Scenario] FOREIGN KEY ([ScenarioId]) REFERENCES [dbo].[ScenarioMaster] ([ScenarioId])
);






GO
Create TRIGGER [dbo].[Trg_Project]
	ON [dbo].[Project]
	FOR DELETE, INSERT, UPDATE
	AS
	BEGIN
		SET NOCOUNT ON
		DECLARE @UserId uniqueidentifier;
		DECLARE @Summary varchar(max);
		DECLARE @Tablename varchar(150)='Project';
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
					ISNULL(CAST(i.Project_Id as varchar(max)),'')+','
					+ISNULL(CAST(i.Project_Name as varchar(max)),'')+','
					+ISNULL(CAST(i.[Description] as varchar(max)),'')+','
					+ISNULL(CAST(i.Customer_Id as varchar(max)),'')+','
					+ISNULL(CAST(i.ProjectManager_Id as varchar(max)),'')+','
					+ISNULL(CAST(i.ScenarioId as varchar(max)),'')+','

					+ISNULL(CAST(i.isActive as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_on as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_By as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_On as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_by as varchar(max)),'')+','+
					ISNULL(CAST(i.IsDeleted as varchar(max)),'') AS summary
					from Project A inner join inserted i on A.Project_Id=i.Project_Id)
				SELECT  
				@UserId= 
				CASE 
				WHEN @Operation='INSERT' OR @Operation='UPDATE' THEN  i.Cre_By
				ELSE i.Modified_by  END,
				@TablID=i.Project_Id,
				@Action=@Operation
				from Project A inner join inserted i on A.Project_Id=i.Project_Id
			END
		Else IF @Operation='DELETE'
		BEGIN
		SET @Summary=(
				SELECT 
					ISNULL(CAST(i.Project_Id as varchar(max)),'')+','
					+ISNULL(CAST(i.Project_Name as varchar(max)),'')+','
					+ISNULL(CAST(i.[Description] as varchar(max)),'')+','
					+ISNULL(CAST(i.Customer_Id as varchar(max)),'')+','
					+ISNULL(CAST(i.ProjectManager_Id as varchar(max)),'')+','
					+ISNULL(CAST(i.ScenarioId as varchar(max)),'')+','

					+ISNULL(CAST(i.isActive as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_on as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_By as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_On as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_by as varchar(max)),'')+','+
					ISNULL(CAST(i.IsDeleted as varchar(max)),'') AS summary
					from deleted i)
			SET @UserId=(select cast(cast(0 as binary) as uniqueidentifier));
				SELECT  
				@TablID=i.Project_Id,
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