CREATE TABLE [dbo].[ApplicationAreaMaster] (
    [Id]              INT              IDENTITY (1, 1) NOT NULL,
    [ApplicationArea] VARCHAR (500)    NOT NULL,
    [isActive]        BIT              CONSTRAINT [DF_ApplicationArea_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]          DATETIME         CONSTRAINT [DF_ApplicationAreaMaster_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]          UNIQUEIDENTIFIER CONSTRAINT [DF_ApplicationAreaMaster_Cre_By] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Modified_On]     DATETIME         NULL,
    [Modified_by]     UNIQUEIDENTIFIER NULL,
    [IsDeleted]       BIT              CONSTRAINT [DF_ApplicationArea_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ApplicationAreaMaster] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ApplicationAreaMaster_ApplicationAreaMaster] FOREIGN KEY ([Id]) REFERENCES [dbo].[ApplicationAreaMaster] ([Id])
);

