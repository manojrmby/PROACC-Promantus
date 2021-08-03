CREATE TABLE [dbo].[StatusMaster] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [StatusName]  VARCHAR (50)     NOT NULL,
    [isActive]    BIT              CONSTRAINT [DF_StatusMaster_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]      DATETIME         CONSTRAINT [DF_StatusMaster_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]      UNIQUEIDENTIFIER NOT NULL,
    [Modified_On] DATETIME         NULL,
    [Modified_by] UNIQUEIDENTIFIER NULL,
    [IsDeleted]   BIT              CONSTRAINT [DF_StatusMaster_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_StatusMaster] PRIMARY KEY CLUSTERED ([Id] ASC)
);

