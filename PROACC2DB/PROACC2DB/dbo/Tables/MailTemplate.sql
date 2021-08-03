CREATE TABLE [dbo].[MailTemplate] (
    [id]           INT              IDENTITY (1, 1) NOT NULL,
    [TemplateName] NVARCHAR (50)    NOT NULL,
    [FileName]     NVARCHAR (50)    NOT NULL,
    [isActive]     BIT              CONSTRAINT [DF_MailTemplate_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]       DATETIME         CONSTRAINT [DF_MailTemplate_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]       UNIQUEIDENTIFIER CONSTRAINT [DF_MailTemplate_Cre_By] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Modified_On]  DATETIME         NULL,
    [Modified_by]  UNIQUEIDENTIFIER NULL,
    [IsDeleted]    BIT              CONSTRAINT [DF_MailTemplate_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MailTemplate] PRIMARY KEY CLUSTERED ([id] ASC)
);

