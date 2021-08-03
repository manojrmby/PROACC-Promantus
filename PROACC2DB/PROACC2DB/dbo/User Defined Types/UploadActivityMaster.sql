CREATE TYPE [dbo].[UploadActivityMaster] AS TABLE (
    [Task]            NVARCHAR (MAX) NULL,
    [BuildingBlock]   NVARCHAR (MAX) NULL,
    [Phase]           NVARCHAR (MAX) NULL,
    [Role]            NVARCHAR (MAX) NULL,
    [ApplicationArea] NVARCHAR (MAX) NULL,
    [TaskType]        NVARCHAR (MAX) NULL,
    [ParallelId]      NVARCHAR (MAX) NULL);





