CREATE TABLE [dbo].[Log_History] (
    [LogId]       UNIQUEIDENTIFIER NOT NULL,
    [UserId]      UNIQUEIDENTIFIER NOT NULL,
    [LoginTime]   DATETIME         NOT NULL,
    [LogoutTime]  DATETIME         NULL,
    [isActive]    BIT              CONSTRAINT [DF_Log_History_isActive] DEFAULT ((1)) NOT NULL,
    [Browser]     NVARCHAR (50)    NULL,
    [IP]          NVARCHAR (500)    NULL,
    [MachineName] NVARCHAR (50)    NULL,
    [Cre_on]      DATETIME         CONSTRAINT [DF_Log_History_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]      UNIQUEIDENTIFIER NOT NULL,
    [Modified_On] DATETIME         NULL,
    [Modified_by] UNIQUEIDENTIFIER NULL,
    [IsDeleted]   BIT              CONSTRAINT [DF_Log_History_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Log_History_1] PRIMARY KEY CLUSTERED ([LogId] ASC),
    CONSTRAINT [FK_Log_History_UserMaster] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserMaster] ([UserId])
);

