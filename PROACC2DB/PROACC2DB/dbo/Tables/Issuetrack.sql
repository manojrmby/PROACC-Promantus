CREATE TABLE [dbo].[Issuetrack] (
    [Issuetrack_Id]      UNIQUEIDENTIFIER NOT NULL,
    [RunningID]          INT              NOT NULL,
    [IssueName]          VARCHAR (500)    NOT NULL,
    [PhaseID]            INT              NULL,
    [TaskId]             INT              NULL,
    [ProjectInstance_Id] UNIQUEIDENTIFIER NOT NULL,
    [StartDate]          DATETIME         NOT NULL,
    [EndDate]            DATETIME         NOT NULL,
    [LastUpdatedDate]    DATETIME         NOT NULL,
    [AssignedTo]         UNIQUEIDENTIFIER NOT NULL,
    [Status]             VARCHAR (150)    NOT NULL,
    [IsApproved]         BIT              NOT NULL,
    [isActive]           BIT              CONSTRAINT [DF_Issuetrack_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]             DATETIME         CONSTRAINT [DF_Issuetrack_Cre_on] DEFAULT (getdate()) NOT NULL,
    [Cre_By]             UNIQUEIDENTIFIER NOT NULL,
    [Modified_On]        DATETIME         NULL,
    [Modified_by]        UNIQUEIDENTIFIER NULL,
    [IsDeleted]          BIT              CONSTRAINT [DF_Issuetrack_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Issuetrack] PRIMARY KEY CLUSTERED ([Issuetrack_Id] ASC),
    CONSTRAINT [FK_Issuetrack_Instance] FOREIGN KEY ([ProjectInstance_Id]) REFERENCES [dbo].[Instance] ([Instance_id])
);






GO
Create TRIGGER [dbo].[Trg_Issuetrack]
	ON [dbo].[Issuetrack]
	FOR DELETE, INSERT, UPDATE
	AS
	BEGIN
		SET NOCOUNT ON
		DECLARE @UserId uniqueidentifier;
		DECLARE @Summary varchar(max);
		DECLARE @Tablename varchar(150)='Issuetrack';
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
					ISNULL(CAST(i.Issuetrack_Id as varchar(max)),'')+','
					+ISNULL(CAST(i.RunningID as varchar(max)),'')+','
					+ISNULL(CAST(i.IssueName as varchar(max)),'')+','
					+ISNULL(CAST(i.PhaseID as varchar(max)),'')+','
					+ISNULL(CAST(i.TaskId as varchar(max)),'')+','
					+ISNULL(CAST(i.ProjectInstance_Id as varchar(max)),'')+','
					+ISNULL(CAST(i.StartDate as varchar(max)),'')+','
					+ISNULL(CAST(i.EndDate as varchar(max)),'')+','
					+ISNULL(CAST(i.LastUpdatedDate as varchar(max)),'')+','
					+ISNULL(CAST(i.AssignedTo as varchar(max)),'')+','
					+ISNULL(CAST(i.[Status] as varchar(max)),'')+','
					+ISNULL(CAST(i.IsApproved as varchar(max)),'')+','

					+ISNULL(CAST(i.isActive as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_on as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_By as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_On as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_by as varchar(max)),'')+','+
					ISNULL(CAST(i.IsDeleted as varchar(max)),'') AS summary
					from Issuetrack A inner join inserted i on A.Issuetrack_Id=i.Issuetrack_Id)
				SELECT  
				@UserId= 
				CASE 
				WHEN @Operation='INSERT' OR @Operation='UPDATE' THEN  i.Cre_By
				ELSE i.Modified_by  END,
				@TablID=i.Issuetrack_Id,
				@Action=@Operation
				from Issuetrack A inner join inserted i on A.Issuetrack_Id=i.Issuetrack_Id
			END
		Else IF @Operation='DELETE'
		BEGIN
		SET @Summary=(
				SELECT 
					ISNULL(CAST(i.Issuetrack_Id as varchar(max)),'')+','
					+ISNULL(CAST(i.RunningID as varchar(max)),'')+','
					+ISNULL(CAST(i.IssueName as varchar(max)),'')+','
					+ISNULL(CAST(i.PhaseID as varchar(max)),'')+','
					+ISNULL(CAST(i.TaskId as varchar(max)),'')+','
					+ISNULL(CAST(i.ProjectInstance_Id as varchar(max)),'')+','
					+ISNULL(CAST(i.StartDate as varchar(max)),'')+','
					+ISNULL(CAST(i.EndDate as varchar(max)),'')+','
					+ISNULL(CAST(i.LastUpdatedDate as varchar(max)),'')+','
					+ISNULL(CAST(i.AssignedTo as varchar(max)),'')+','
					+ISNULL(CAST(i.[Status] as varchar(max)),'')+','
					+ISNULL(CAST(i.IsApproved as varchar(max)),'')+','

					+ISNULL(CAST(i.isActive as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_on as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_By as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_On as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_by as varchar(max)),'')+','+
					ISNULL(CAST(i.IsDeleted as varchar(max)),'') AS summary
					from deleted i)
			SET @UserId=(select cast(cast(0 as binary) as uniqueidentifier));
				SELECT  
				@TablID=i.Issuetrack_Id,
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