CREATE TABLE [dbo].[PhaseMaster] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [PhaseName]   VARCHAR (100)    NOT NULL,
    [isActive]    BIT              CONSTRAINT [DF_PhaseMaster_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]      DATETIME         CONSTRAINT [DF_PhaseMaster_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]      UNIQUEIDENTIFIER NOT NULL,
    [Modified_On] DATETIME         NULL,
    [Modified_by] UNIQUEIDENTIFIER NULL,
    [IsDeleted]   BIT              CONSTRAINT [DF_PhaseMaster_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_PhaseMaster] PRIMARY KEY CLUSTERED ([Id] ASC)
);

