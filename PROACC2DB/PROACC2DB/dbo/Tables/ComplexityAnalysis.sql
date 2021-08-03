CREATE TABLE [dbo].[ComplexityAnalysis] (
    [FileUploadID]           UNIQUEIDENTIFIER NOT NULL,
    [CompanyCode]            VARCHAR (500)    NULL,
    [NewGL]                  VARCHAR (500)    NULL,
    [Leadingledger]          VARCHAR (500)    NULL,
    [SpecialPurposeLedger]   VARCHAR (500)    NULL,
    [Treasury_SWF5_FSCM_CLM] VARCHAR (500)    NULL,
    [Treasury_SWF5_FSCM_BNK] VARCHAR (500)    NULL,
    [Multicurrencymodel]     VARCHAR (500)    NULL,
    [NewAssetAccounting]     VARCHAR (500)    NULL,
    [BusinessPartner]        VARCHAR (500)    NULL,
    [BPActive]               VARCHAR (500)    NULL,
    [MaterialLedger]         VARCHAR (500)    NULL,
    [Active]                 VARCHAR (500)    NULL,
    [Cre_on]                 DATETIME         CONSTRAINT [DF_ComplexityAnalysis_Cre_on] DEFAULT (getdate()) NULL
);

