CREATE TABLE [dbo].[ActivityMaster] (
    [Activity_ID]       INT              IDENTITY (1, 1) NOT NULL,
    [Task]              VARCHAR (MAX)    NOT NULL,
    [BuildingBlock_id]  INT              NULL,
    [PhaseID]           INT              NOT NULL,
    [Sequence_Num]      INT              NULL,
    [ApplicationAreaID] INT              NOT NULL,
    [RoleID]            INT              NOT NULL,
    [EST_hours]         DECIMAL (4, 2)   NULL,
    [PM_Add]            BIT              NULL,
    [Task_id]           INT              NULL,
    [Parallel_Id]       UNIQUEIDENTIFIER NULL,
    [Version_Id]        UNIQUEIDENTIFIER NULL,
    [isActive]          BIT              CONSTRAINT [DF_ActivityMaster_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]            DATETIME         CONSTRAINT [DF_ActivityMaster_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]            UNIQUEIDENTIFIER CONSTRAINT [DF_ActivityMaster_Cre_By] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Modified_On]       DATETIME         NULL,
    [Modified_by]       UNIQUEIDENTIFIER NULL,
    [IsDeleted]         BIT              CONSTRAINT [DF_ActivityMaster_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ActivityMaster] PRIMARY KEY CLUSTERED ([Activity_ID] ASC),
    CONSTRAINT [FK_ActivityMaster_AMVersion] FOREIGN KEY ([Version_Id]) REFERENCES [dbo].[AMVersion] ([Id]),
    CONSTRAINT [FK_ActivityMaster_PhaseMaster] FOREIGN KEY ([PhaseID]) REFERENCES [dbo].[PhaseMaster] ([Id]),
    CONSTRAINT [FK_ActivityMaster_RoleMaster] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[RoleMaster] ([RoleId])
);






GO
CREATE TRIGGER [dbo].[Trg_ActivityMaster]
	ON [dbo].[ActivityMaster]
	FOR DELETE, INSERT, UPDATE
	--FOR INSERT 
	AS
	BEGIN
		SET NOCOUNT ON
		DECLARE @UserId uniqueidentifier;
		DECLARE @Summary varchar(max);
		DECLARE @Tablename varchar(150)='ActivityMaster';
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
					ISNULL(CAST(i.Activity_ID as varchar(max)),'')+','
					+ISNULL(CAST(i.Task as varchar(max)),'')+','
					+ISNULL(CAST(i.BuildingBlock_id as varchar(max)),'')+','
					+ISNULL(CAST(i.PhaseID as varchar(max)),'')+','
					+ISNULL(CAST(i.Sequence_Num as varchar(max)),'')+','
					+ISNULL(CAST(i.ApplicationAreaID as varchar(max)),'')+','
					+ISNULL(CAST(i.RoleID as varchar(max)),'')+','
					+ISNULL(CAST(i.EST_hours as varchar(max)),'')+','
					+ISNULL(CAST(i.PM_Add as varchar(max)),'')+','
					+ISNULL(CAST(i.Task_id as varchar(max)),'')+','
					+ISNULL(CAST(i.Parallel_Id as varchar(max)),'')+','
					+ISNULL(CAST(i.Version_Id as varchar(max)),'')+','

					+ISNULL(CAST(i.isActive as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_on as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_By as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_On as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_by as varchar(max)),'')+','+
					ISNULL(CAST(i.IsDeleted as varchar(max)),'') AS summary
					from ActivityMaster A inner join inserted i on A.Activity_ID=i.Activity_ID)
				SELECT  
				@UserId= 
				CASE 
				WHEN @Operation='INSERT' OR @Operation='UPDATE' THEN  i.Cre_By
				ELSE i.Modified_by  END,
				@TablID=i.Activity_ID,
				@Action=@Operation
				from ActivityMaster A inner join inserted i on A.Activity_ID=i.Activity_ID
			END
		Else IF @Operation='DELETE'
		BEGIN
		SET @Summary=(
				SELECT 
					ISNULL(CAST(i.Activity_ID as varchar(max)),'')+','
					+ISNULL(CAST(i.Task as varchar(max)),'')+','
					+ISNULL(CAST(i.BuildingBlock_id as varchar(max)),'')+','
					+ISNULL(CAST(i.PhaseID as varchar(max)),'')+','
					+ISNULL(CAST(i.Sequence_Num as varchar(max)),'')+','
					+ISNULL(CAST(i.ApplicationAreaID as varchar(max)),'')+','
					+ISNULL(CAST(i.RoleID as varchar(max)),'')+','
					+ISNULL(CAST(i.EST_hours as varchar(max)),'')+','
					+ISNULL(CAST(i.PM_Add as varchar(max)),'')+','
					+ISNULL(CAST(i.Task_id as varchar(max)),'')+','
					+ISNULL(CAST(i.Parallel_Id as varchar(max)),'')+','
					+ISNULL(CAST(i.Version_Id as varchar(max)),'')+','

					+ISNULL(CAST(i.isActive as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_on as varchar(max)),'')+','+
					ISNULL(CAST(i.Cre_By as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_On as varchar(max)),'')+','+
					ISNULL(CAST(i.Modified_by as varchar(max)),'')+','+
					ISNULL(CAST(i.IsDeleted as varchar(max)),'') AS summary
					from deleted i)
			SET @UserId=(select cast(cast(0 as binary) as uniqueidentifier));
				SELECT  
				@TablID=i.Activity_ID,
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