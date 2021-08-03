CREATE TABLE [dbo].[UploadActivityMaster] (
    [Activity_ID]           INT              NOT NULL,
    [Activity_FileUploadID] UNIQUEIDENTIFIER NULL,
    [Task]                  VARCHAR (MAX)    NOT NULL,
    [BuildingBlock_id]      INT              NOT NULL,
    [PhaseID]               INT              NOT NULL,
    [Sequence_Num]          INT              NULL,
    [ApplicationAreaID]     INT              NULL,
    [RoleID]                INT              NULL,
    [EST_hours]             DECIMAL (4, 2)   NULL,
    [PM_Add]                BIT              NULL,
    [Task_id]               INT              NULL,
    [Parallel_Id]           UNIQUEIDENTIFIER NULL,
    [isActive]              BIT              CONSTRAINT [DF_UploadActivityMaster_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]                DATETIME         CONSTRAINT [DF_UploadActivityMaster_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]                UNIQUEIDENTIFIER CONSTRAINT [DF_UploadActivityMaster_Cre_By] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Modified_On]           DATETIME         NULL,
    [Modified_by]           UNIQUEIDENTIFIER NULL,
    [IsDeleted]             BIT              CONSTRAINT [DF_UploadActivityMaster_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_UploadActivityMaster_1] PRIMARY KEY CLUSTERED ([Activity_ID] ASC),
    CONSTRAINT [FK_UploadActivityMaster_PhaseMaster] FOREIGN KEY ([PhaseID]) REFERENCES [dbo].[PhaseMaster] ([Id]),
    CONSTRAINT [FK_UploadActivityMaster_RoleMaster] FOREIGN KEY ([RoleID]) REFERENCES [dbo].[RoleMaster] ([RoleId])
);

