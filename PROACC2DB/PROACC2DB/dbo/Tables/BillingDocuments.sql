CREATE TABLE [dbo].[BillingDocuments] (
    [FileUploadID]     UNIQUEIDENTIFIER NOT NULL,
    [CompanyCode]      VARCHAR (50)     NULL,
    [Currency]         VARCHAR (50)     NULL,
    [FKREL]            VARCHAR (500)    NULL,
    [BillingCreatedby] VARCHAR (500)    NULL,
    [NoofDocuments]    VARCHAR (50)     NULL,
    [isActive]         BIT              CONSTRAINT [DF_BillingDocuments_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_On]           DATETIME         CONSTRAINT [DF_BillingDocuments_Cre_On] DEFAULT (getdate()) NOT NULL,
    [Cre_By]           UNIQUEIDENTIFIER NOT NULL,
    [Modified_on]      DATETIME         NULL,
    [Modified_by]      UNIQUEIDENTIFIER NULL,
    [isDeleted]        BIT              CONSTRAINT [DF_BillingDocuments_isDeleted] DEFAULT ((0)) NOT NULL
);

