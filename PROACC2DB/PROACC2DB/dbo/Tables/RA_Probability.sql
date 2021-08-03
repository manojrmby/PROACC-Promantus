CREATE TABLE [dbo].[RA_Probability] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [Probability] VARCHAR (50)     NULL,
    [isActive]    BIT              CONSTRAINT [DF_RA_Probability_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]      DATETIME         CONSTRAINT [DF_RA_Probability_Cre_on] DEFAULT (getdate()) NOT NULL,
    [Cre_By]      UNIQUEIDENTIFIER NULL,
    [Modified_on] DATETIME         NULL,
    [Modified_By] UNIQUEIDENTIFIER NULL,
    [isDeleted]   BIT              CONSTRAINT [DF_RA_Probability_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_RA_Probability] PRIMARY KEY CLUSTERED ([Id] ASC)
);

