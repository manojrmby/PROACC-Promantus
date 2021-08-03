CREATE TABLE [dbo].[SAPInput_RFCFM] (
    [FileUploadID] UNIQUEIDENTIFIER NOT NULL,
    [Parameter]    VARCHAR (MAX)    NULL,
    [Value]        VARCHAR (MAX)    NULL,
    [isActive]     BIT              CONSTRAINT [DF_SAPInput_RFCFM_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]       DATETIME         CONSTRAINT [DF_SAPInput_RFCFM_Cre_on] DEFAULT (getdate()) NOT NULL,
    [Cre_By]       UNIQUEIDENTIFIER NOT NULL,
    [Modified_on]  DATETIME         NULL,
    [Modified_by]  UNIQUEIDENTIFIER NULL,
    [isDeleted]    BIT              CONSTRAINT [DF_SAPInput_RFCFM_isDeleted] DEFAULT ((0)) NOT NULL
);

