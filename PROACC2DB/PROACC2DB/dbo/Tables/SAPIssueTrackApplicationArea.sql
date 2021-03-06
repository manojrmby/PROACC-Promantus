CREATE TABLE [dbo].[SAPIssueTrackApplicationArea] (
    [Id]                  INT              IDENTITY (1, 1) NOT NULL,
    [ApplicationAreaName] VARCHAR (50)     NOT NULL,
    [isActive]            BIT              CONSTRAINT [DF_SAPIssueTrackApplicationArea_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]              DATETIME         CONSTRAINT [DF_SAPIssueTrackApplicationArea_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]              UNIQUEIDENTIFIER NOT NULL,
    [Modified_On]         DATETIME         NULL,
    [Modified_by]         UNIQUEIDENTIFIER NULL,
    [IsDeleted]           BIT              CONSTRAINT [DF_SAPIssueTrackApplicationArea_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SAPIssueTrackApplicationArea] PRIMARY KEY CLUSTERED ([Id] ASC)
);

