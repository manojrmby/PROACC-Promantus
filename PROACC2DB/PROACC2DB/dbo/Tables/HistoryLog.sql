CREATE TABLE [dbo].[HistoryLog] (
    [HistoryLogId]   INT              IDENTITY (1, 1) NOT NULL,
    [IssueTrackId]   UNIQUEIDENTIFIER NOT NULL,
    [CreatedDate]    DATETIME         NULL,
    [HistoryComment] VARCHAR (500)    NULL,
    [Description]    VARCHAR (MAX)    NULL,
    [AssignedTo]     UNIQUEIDENTIFIER NULL,
    [isActive]       BIT              CONSTRAINT [DF_HistoryLog_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]         DATETIME         CONSTRAINT [DF_HistoryLog_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]         UNIQUEIDENTIFIER NOT NULL,
    [Modified_On]    DATETIME         NULL,
    [Modified_by]    UNIQUEIDENTIFIER NULL,
    [IsDeleted]      BIT              CONSTRAINT [DF_HistoryLog_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_HistoryLog] PRIMARY KEY CLUSTERED ([HistoryLogId] ASC),
    CONSTRAINT [FK_HistoryLog_Issuetrack] FOREIGN KEY ([IssueTrackId]) REFERENCES [dbo].[Issuetrack] ([Issuetrack_Id])
);

