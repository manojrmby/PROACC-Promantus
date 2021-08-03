CREATE TABLE [dbo].[TaskType] (
    [TaskId]      INT              NOT NULL,
    [TaskName]    NVARCHAR (50)    NULL,
    [isActive]    BIT              CONSTRAINT [DF_TaskType_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]      DATETIME         NULL,
    [Cre_By]      UNIQUEIDENTIFIER NULL,
    [Modified_On] DATETIME         NULL,
    [Modified_By] UNIQUEIDENTIFIER NULL,
    [IsDeleted]   BIT              CONSTRAINT [DF_TaskType_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_TaskType] PRIMARY KEY CLUSTERED ([TaskId] ASC)
);

