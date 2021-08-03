CREATE TABLE [dbo].[SAPInput_INSTALLEDPRODUCTVERSIONS] (
    [FileUploadID] UNIQUEIDENTIFIER NOT NULL,
    [Product]      VARCHAR (500)    NULL,
    [Release]      VARCHAR (500)    NULL,
    [SP_Stack]     VARCHAR (500)    NULL,
    [Vendor]       VARCHAR (500)    NULL,
    [Description]  VARCHAR (500)    NULL,
    [isActive]     BIT              CONSTRAINT [DF_SAPInput_INSTALLEDPRODUCTVERSIONS_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_On]       DATETIME         CONSTRAINT [DF_SAPInput_INSTALLEDPRODUCTVERSIONS_Cre_On] DEFAULT (getdate()) NOT NULL,
    [Cre_By]       UNIQUEIDENTIFIER NOT NULL,
    [Modified_on]  DATETIME         NULL,
    [Modified_by]  UNIQUEIDENTIFIER NULL,
    [isDeleted]    BIT              CONSTRAINT [DF_SAPInput_INSTALLEDPRODUCTVERSIONS_isDeleted] DEFAULT ((0)) NOT NULL
);

