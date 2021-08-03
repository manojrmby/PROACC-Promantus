CREATE TYPE [dbo].[UploadBillingDocument] AS TABLE (
    [CompanyCode]      NVARCHAR (MAX) NULL,
    [Currency]         NVARCHAR (MAX) NULL,
    [FKREL]            NVARCHAR (MAX) NULL,
    [BillingCreatedby] NVARCHAR (MAX) NULL,
    [NoofDocuments]    NVARCHAR (MAX) NULL);

