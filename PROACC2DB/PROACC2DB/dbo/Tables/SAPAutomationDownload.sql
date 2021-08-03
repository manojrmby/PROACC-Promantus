CREATE TABLE [dbo].[SAPAutomationDownload] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [InstanceID]  UNIQUEIDENTIFIER NOT NULL,
    [AutoList_ID] INT              NOT NULL,
    [isActive]    BIT              CONSTRAINT [DF_SAPAutomtationDownload_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]      DATETIME         CONSTRAINT [DF_SAPAutomationDownload_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_by]      UNIQUEIDENTIFIER NOT NULL,
    [Modified_on] DATETIME         NULL,
    [Modified_by] UNIQUEIDENTIFIER NULL,
    [isDeleted]   BIT              CONSTRAINT [DF_SAPAutomtationDownload_isDeleted] DEFAULT ((0)) NOT NULL
);

