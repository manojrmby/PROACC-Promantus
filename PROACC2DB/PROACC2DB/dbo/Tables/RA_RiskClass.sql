CREATE TABLE [dbo].[RA_RiskClass] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [Risk Class]  VARCHAR (50)     NULL,
    [isActive]    BIT              CONSTRAINT [DF_RA_RiskClass_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]      DATETIME         CONSTRAINT [DF_RA_RiskClass_Cre_on] DEFAULT (getdate()) NOT NULL,
    [Cre_By]      UNIQUEIDENTIFIER NULL,
    [Modified_on] DATETIME         NULL,
    [Modified_By] UNIQUEIDENTIFIER NULL,
    [isDeleted]   BIT              CONSTRAINT [DF_RA_RiskClass_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_RA_RiskClass] PRIMARY KEY CLUSTERED ([Id] ASC)
);

