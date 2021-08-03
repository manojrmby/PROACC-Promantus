CREATE TABLE [dbo].[RA_RiskOwner] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [Risk Owner]  VARCHAR (50)     NULL,
    [isActive]    BIT              CONSTRAINT [DF_RA_RiskOwner_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]      DATETIME         CONSTRAINT [DF_RA_RiskOwner_Cre_on] DEFAULT (getdate()) NOT NULL,
    [Cre_By]      UNIQUEIDENTIFIER NULL,
    [Modified_on] DATETIME         NULL,
    [Modified_by] UNIQUEIDENTIFIER NULL,
    [isDeleted]   BIT              CONSTRAINT [DF_RA_RiskOwner_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_RA_RiskOwner] PRIMARY KEY CLUSTERED ([Id] ASC)
);

