CREATE TABLE [dbo].[MailMaster] (
    [Running_ID]  INT              IDENTITY (1, 1) NOT NULL,
    [UserID]      UNIQUEIDENTIFIER NULL,
    [To]          VARCHAR (50)     NULL,
    [Subject]     VARCHAR (100)    NULL,
    [Body]        VARCHAR (MAX)    NULL,
    [MailStatus]  BIT              NULL,
    [Type]        VARCHAR (100)    NULL,
    [LineID]      UNIQUEIDENTIFIER NULL,
    [Q_UserID]    INT              CONSTRAINT [DF_MailMaster_Q_UserID] DEFAULT ((0)) NOT NULL,
    [TemplateID]  INT              NULL,
    [isActive]    BIT              CONSTRAINT [DF_MailMaster_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]      DATETIME         CONSTRAINT [DF_MailMaster_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]      UNIQUEIDENTIFIER CONSTRAINT [DF_MailMaster_Cre_By] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Modified_On] DATETIME         NULL,
    [Modified_by] UNIQUEIDENTIFIER NULL,
    [IsDeleted]   BIT              CONSTRAINT [DF_MailMaster_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MailMaster] PRIMARY KEY CLUSTERED ([Running_ID] ASC)
);

