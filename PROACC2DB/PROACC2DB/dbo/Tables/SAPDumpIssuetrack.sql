CREATE TABLE [dbo].[SAPDumpIssuetrack] (
    [Id]                    UNIQUEIDENTIFIER NOT NULL,
    [RunningID]             INT              NULL,
    [IssueNo]               INT              NULL,
    [IssueName]             VARCHAR (500)    NULL,
    [Category_Id]           INT              NOT NULL,
    [Priority_Id]           INT              NOT NULL,
    [Assignee]              VARCHAR (500)    NULL,
    [RaisedBy]              VARCHAR (500)    NULL,
    [ApplicationArea_Id]    INT              NOT NULL,
    [OpenDt]                DATETIME         NULL,
    [CloseDt]               DATETIME         NULL,
    [SAPIssueDumpStatus_Id] INT              NOT NULL,
    [Project_Id]            UNIQUEIDENTIFIER NULL,
    [Resolution]            VARCHAR (500)    NULL,
    [Comments]              VARCHAR (MAX)    NULL,
    [isActive]              BIT              CONSTRAINT [DF_SAPDumpIssuetrack_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]                DATETIME         CONSTRAINT [DF_SAPDumpIssuetrack_Cre_on] DEFAULT (getdate()) NOT NULL,
    [Cre_By]                UNIQUEIDENTIFIER NULL,
    [Modified_On]           DATETIME         NULL,
    [Modified_by]           UNIQUEIDENTIFIER NULL,
    [IsDeleted]             BIT              CONSTRAINT [DF_SAPDumpIssuetrack_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SAPDumpIssuetrack] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SAPDumpIssuetrack_SAPIssueTrackApplicationArea] FOREIGN KEY ([ApplicationArea_Id]) REFERENCES [dbo].[SAPIssueTrackApplicationArea] ([Id]),
    CONSTRAINT [FK_SAPDumpIssuetrack_SAPIssueTrackCategory] FOREIGN KEY ([Category_Id]) REFERENCES [dbo].[SAPIssueTrackCategory] ([Id]),
    CONSTRAINT [FK_SAPDumpIssuetrack_SAPIssueTrackPriority] FOREIGN KEY ([Priority_Id]) REFERENCES [dbo].[SAPIssueTrackPriority] ([Id]),
    CONSTRAINT [FK_SAPDumpIssuetrack_SAPIssueTrackStatus] FOREIGN KEY ([SAPIssueDumpStatus_Id]) REFERENCES [dbo].[SAPIssueTrackStatus] ([Id])
);





