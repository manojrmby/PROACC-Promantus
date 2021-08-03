CREATE TYPE [dbo].[UploadComplexityAnalysis] AS TABLE (
    [CompanyCode]            NVARCHAR (MAX) NULL,
    [NewGL]                  NVARCHAR (MAX) NULL,
    [Leadingledger]          NVARCHAR (MAX) NULL,
    [SpecialPurposeLedger]   NVARCHAR (MAX) NULL,
    [Treasury_SWF5_FSCM_CLM] NVARCHAR (MAX) NULL,
    [Treasury_SWF5_FSCM_BNK] NVARCHAR (MAX) NULL,
    [Multicurrencymodel]     NVARCHAR (MAX) NULL,
    [NewAssetAccounting]     NVARCHAR (MAX) NULL,
    [BusinessPartner]        NVARCHAR (MAX) NULL,
    [BPActive]               NVARCHAR (MAX) NULL,
    [MaterialLedger]         NVARCHAR (MAX) NULL,
    [Active]                 NVARCHAR (MAX) NULL);

