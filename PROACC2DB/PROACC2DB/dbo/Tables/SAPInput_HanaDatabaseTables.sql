CREATE TABLE [dbo].[SAPInput_HanaDatabaseTables] (
    [FileUploadID]      UNIQUEIDENTIFIER NOT NULL,
    [Name]              VARCHAR (250)    NULL,
    [Store Type]        VARCHAR (50)     NULL,
    [Data Size in GB]   VARCHAR (50)     NULL,
    [Number of Records] VARCHAR (50)     NULL,
    [isActive]          BIT              CONSTRAINT [DF_SAPInput_HanaDatabaseTables_isActive] DEFAULT ((1)) NULL,
    [Cre_on]            DATETIME         CONSTRAINT [DF_SAPInput_HanaDatabaseTables_Cre_on] DEFAULT (getdate()) NOT NULL,
    [Cre_By]            UNIQUEIDENTIFIER NULL,
    [Modified_on]       DATETIME         NULL,
    [Modified_by]       UNIQUEIDENTIFIER NULL,
    [isDeleted]         BIT              CONSTRAINT [DF_SAPInput_HanaDatabaseTables_isDeleted] DEFAULT ((0)) NOT NULL
);

