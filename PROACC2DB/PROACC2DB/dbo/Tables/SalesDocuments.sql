CREATE TABLE [dbo].[SalesDocuments] (
    [FileUploadID]  UNIQUEIDENTIFIER NOT NULL,
    [CCbilled]      VARCHAR (500)    NULL,
    [SalesOrg]      VARCHAR (500)    NULL,
    [DistChannel]   VARCHAR (500)    NULL,
    [Division]      VARCHAR (500)    NULL,
    [plant]         VARCHAR (500)    NULL,
    [NoofDocuments] VARCHAR (500)    NULL,
    [isActive]      BIT              CONSTRAINT [DF_SalesDocuments_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]        DATETIME         CONSTRAINT [DF_SalesDocument_Cre_on] DEFAULT (getdate()) NOT NULL,
    [Cre_By]        UNIQUEIDENTIFIER NOT NULL,
    [Modified_on]   DATETIME         NULL,
    [Modified_by]   UNIQUEIDENTIFIER NULL,
    [isDeleted]     BIT              CONSTRAINT [DF_SalesDocuments_isDeleted] DEFAULT ((0)) NOT NULL
);

