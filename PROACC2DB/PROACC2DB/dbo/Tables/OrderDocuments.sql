CREATE TABLE [dbo].[OrderDocuments] (
    [FileUploadID]    UNIQUEIDENTIFIER NOT NULL,
    [ControllingArea] VARCHAR (500)    NULL,
    [CCbilled]        VARCHAR (500)    NULL,
    [plant]           VARCHAR (500)    NULL,
    [DocCategory]     VARCHAR (500)    NULL,
    [NoofDocuments]   VARCHAR (500)    NULL,
    [isActive]        BIT              CONSTRAINT [DF_OrderDocuments_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]          DATETIME         CONSTRAINT [DF_OrderDocuments_Cre_on] DEFAULT (getdate()) NOT NULL,
    [Cre_By]          UNIQUEIDENTIFIER NOT NULL,
    [Modified_on]     DATETIME         NULL,
    [Modified_by]     UNIQUEIDENTIFIER NULL,
    [isDeleted]       BIT              CONSTRAINT [DF_OrderDocuments_isDeleted] DEFAULT ((0)) NOT NULL
);

