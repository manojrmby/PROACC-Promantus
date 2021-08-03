CREATE TABLE [dbo].[RiskAssessment] (
    [ID]              UNIQUEIDENTIFIER NOT NULL,
    [RiskId]          INT              NOT NULL,
    [Project_Id]      UNIQUEIDENTIFIER NULL,
    [RiskDescription] VARCHAR (500)    NULL,
    [RiskManagement]  VARCHAR (500)    NULL,
    [Probability_Id]  INT              NOT NULL,
    [Consequence]     INT              NULL,
    [RiskClass_Id]    INT              NOT NULL,
    [RiskOwner_Id]    INT              NOT NULL,
    [Area]            VARCHAR (500)    NULL,
    [isActive]        BIT              CONSTRAINT [DF_RiskAssessment_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]          DATETIME         CONSTRAINT [DF_RiskAssessment_Cre_on] DEFAULT (getdate()) NOT NULL,
    [Cre_By]          UNIQUEIDENTIFIER NOT NULL,
    [Modified_on]     DATETIME         NULL,
    [Modified_by]     UNIQUEIDENTIFIER NULL,
    [IsDeleted]       BIT              CONSTRAINT [DF_RiskAssessment_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_RiskAssessment] PRIMARY KEY CLUSTERED ([ID] ASC)
);

