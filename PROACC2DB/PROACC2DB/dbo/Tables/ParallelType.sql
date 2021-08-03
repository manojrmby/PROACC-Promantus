CREATE TABLE [dbo].[ParallelType] (
    [ParallelId]    UNIQUEIDENTIFIER NOT NULL,
    [ParallelName]  INT              NOT NULL,
    [Parallel_Name] VARCHAR (100)    NULL,
    [isActive]      BIT              CONSTRAINT [DF_ParallelType_isActive] DEFAULT ((1)) NULL,
    [Cre_on]        DATETIME         CONSTRAINT [DF_ParallelType_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]        UNIQUEIDENTIFIER CONSTRAINT [DF_ParallelType_Cre_By] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Modified_On]   DATETIME         NULL,
    [Modified_By]   UNIQUEIDENTIFIER NULL,
    [IsDeleted]     BIT              CONSTRAINT [DF_ParallelType_IsDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_ParallelType_1] PRIMARY KEY CLUSTERED ([ParallelId] ASC)
);



