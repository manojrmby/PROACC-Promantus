CREATE TABLE [dbo].[SAPInput_SFW5REPORT] (
    [FileUploadID]   UNIQUEIDENTIFIER NOT NULL,
    [Group]          VARCHAR (500)    NULL,
    [BF_Name]        VARCHAR (500)    NULL,
    [BF_Description] VARCHAR (500)    NULL,
    [Dependency]     VARCHAR (500)    NULL,
    [SW_Component]   VARCHAR (500)    NULL,
    [SW_Release]     VARCHAR (500)    NULL,
    [isActive]       BIT              CONSTRAINT [DF_SAPInput_SFW5REPORT_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_On]         DATETIME         CONSTRAINT [DF_SAPInput_SFW5REPORT_Cre_On] DEFAULT (getdate()) NOT NULL,
    [Cre_By]         UNIQUEIDENTIFIER NOT NULL,
    [Modified_on]    DATETIME         NULL,
    [Modified_by]    UNIQUEIDENTIFIER NULL,
    [isDeleted]      BIT              CONSTRAINT [DF_SAPInput_SFW5REPORT_isDeleted] DEFAULT ((0)) NOT NULL
);

