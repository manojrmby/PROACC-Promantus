CREATE TABLE [dbo].[PMTaskMonitor ] (
    [Id]                     UNIQUEIDENTIFIER NOT NULL,
    [PMTaskID]               UNIQUEIDENTIFIER NOT NULL,
    [ProjectId]              UNIQUEIDENTIFIER NOT NULL,
    [Task_Other_Environment] BIT              CONSTRAINT [DF_PMTaskMonitor_Task_Other_Environment] DEFAULT ((0)) NULL,
    [Dependency]             BIT              CONSTRAINT [DF_PMTaskMonitor_Dependency] DEFAULT ((0)) NULL,
    [Pending]                NVARCHAR (MAX)   NULL,
    [Delay_occurred]         BIT              CONSTRAINT [DF_PMTaskMonitor_Delay_occurred] DEFAULT ((0)) NULL,
    [DelayedReason]          NVARCHAR (MAX)   NULL,
    [StatusId]               INT              CONSTRAINT [DF_PMTaskMonitor_StatusId] DEFAULT ((0)) NOT NULL,
    [EST_hours]              DECIMAL (4, 2)   CONSTRAINT [DF_PMTaskMonitor_EST_hours] DEFAULT ((0)) NULL,
    [Actual_St_hours]        DECIMAL (4, 2)   CONSTRAINT [DF_PMTaskMonitor_Actual_St_hours] DEFAULT ((0)) NULL,
    [Planed__St_Date]        DATETIME         CONSTRAINT [DF_PMTaskMonitor_Planed__St_Date] DEFAULT (((1753)-(1))-(1)) NULL,
    [Actual_St_Date]         DATETIME         CONSTRAINT [DF_PMTaskMonitor_Actual_St_Date] DEFAULT (((1753)-(1))-(1)) NULL,
    [Planed__En_Date]        DATETIME         CONSTRAINT [DF_PMTaskMonitor_Planed__En_Date] DEFAULT (((1753)-(1))-(1)) NULL,
    [Actual_En_Date]         DATETIME         CONSTRAINT [DF_PMTaskMonitor_Actual_En_Date] DEFAULT (((1753)-(1))-(1)) NULL,
    [Notes]                  VARCHAR (MAX)    NULL,
    [isActive]               BIT              CONSTRAINT [DF_PMTaskMonitor_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]                 DATETIME         CONSTRAINT [DF_PMTaskMonitor_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]                 UNIQUEIDENTIFIER CONSTRAINT [DF_PMTaskMonitor_Cre_By] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Modified_On]            DATETIME         CONSTRAINT [DF_PMTaskMonitor_Modified_On] DEFAULT (((1753)-(1))-(1)) NULL,
    [Modified_by]            UNIQUEIDENTIFIER CONSTRAINT [DF_PMTaskMonitor_Modified_by] DEFAULT ('00000000-0000-0000-0000-000000000000') NULL,
    [IsDeleted]              BIT              CONSTRAINT [DF_PMTaskMonitor_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_PMTaskMonitor] PRIMARY KEY CLUSTERED ([Id] ASC)
);






GO
Create TRIGGER [dbo].[Trg_PMTaskMonitor]
	ON [dbo].[PMTaskMonitor ]
	FOR DELETE, UPDATE --INSERT
	AS
	BEGIN
		SET NOCOUNT ON
		DECLARE @UserId uniqueidentifier;
		DECLARE @Summary varchar(max);
		DECLARE @Tablename varchar(150)='PMTaskMonitor';
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
					ISNULL(CAST(i.Id as varchar(max)),'')+','
					+ISNULL(CAST(i.PMTaskID as varchar(max)),'')+','
					+ISNULL(CAST(i.ProjectId as varchar(max)),'')+','
					+ISNULL(CAST(i.Task_Other_Environment as varchar(max)),'')+','
					+ISNULL(CAST(i.Dependency as varchar(max)),'')+','
					+ISNULL(CAST(i.Pending as varchar(max)),'')+','
					+ISNULL(CAST(i.Delay_occurred as varchar(max)),'')+','
					+ISNULL(CAST(i.DelayedReason as varchar(max)),'')+','
					+ISNULL(CAST(i.StatusId as varchar(max)),'')+','
					+ISNULL(CAST(i.EST_hours as varchar(max)),'')+','
					+ISNULL(CAST(i.Actual_St_hours as varchar(max)),'')+','
					+ISNULL(CAST(i.Planed__St_Date as varchar(max)),'')+','
					+ISNULL(CAST(i.Actual_St_Date as varchar(max)),'')+','
					+ISNULL(CAST(i.Planed__En_Date as varchar(max)),'')+','
					+ISNULL(CAST(i.Actual_En_Date as varchar(max)),'')+','
					+ISNULL(CAST(i.Notes as varchar(max)),'')+','

					+ISNULL(CAST(i.isActive as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_on as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_By as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_On as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_by as varchar(max)),'')+','+
					ISNULL(CAST(i.IsDeleted as varchar(max)),'') AS summary
					from PMTaskMonitor A inner join inserted i on A.Id=i.Id)
				SELECT  
				@UserId= 
				CASE 
				WHEN @Operation='INSERT' OR @Operation='UPDATE' THEN  i.Cre_By
				ELSE i.Modified_by  END,
				@TablID=i.Id,
				@Action=@Operation
				from PMTaskMonitor A inner join inserted i on A.Id=i.Id
			END
		Else IF @Operation='DELETE'
		BEGIN
		SET @Summary=(
				SELECT 
					ISNULL(CAST(i.Id as varchar(max)),'')+','
					+ISNULL(CAST(i.PMTaskID as varchar(max)),'')+','
					+ISNULL(CAST(i.ProjectId as varchar(max)),'')+','
					+ISNULL(CAST(i.Task_Other_Environment as varchar(max)),'')+','
					+ISNULL(CAST(i.Dependency as varchar(max)),'')+','
					+ISNULL(CAST(i.Pending as varchar(max)),'')+','
					+ISNULL(CAST(i.Delay_occurred as varchar(max)),'')+','
					+ISNULL(CAST(i.DelayedReason as varchar(max)),'')+','
					+ISNULL(CAST(i.StatusId as varchar(max)),'')+','
					+ISNULL(CAST(i.EST_hours as varchar(max)),'')+','
					+ISNULL(CAST(i.Actual_St_hours as varchar(max)),'')+','
					+ISNULL(CAST(i.Planed__St_Date as varchar(max)),'')+','
					+ISNULL(CAST(i.Actual_St_Date as varchar(max)),'')+','
					+ISNULL(CAST(i.Planed__En_Date as varchar(max)),'')+','
					+ISNULL(CAST(i.Actual_En_Date as varchar(max)),'')+','
					+ISNULL(CAST(i.Notes as varchar(max)),'')+','

					+ISNULL(CAST(i.isActive as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_on as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_By as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_On as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_by as varchar(max)),'')+','+
					ISNULL(CAST(i.IsDeleted as varchar(max)),'') AS summary
					from deleted i)
			SET @UserId=(select cast(cast(0 as binary) as uniqueidentifier));
				SELECT  
				@TablID=i.Id,
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