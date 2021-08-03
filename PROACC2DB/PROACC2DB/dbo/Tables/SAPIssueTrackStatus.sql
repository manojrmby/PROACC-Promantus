CREATE TABLE [dbo].[SAPIssueTrackStatus] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [StatusName]  VARCHAR (50)     NOT NULL,
    [isActive]    BIT              CONSTRAINT [DF_SAPIssueTrackStatus_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]      DATETIME         CONSTRAINT [DF_SAPIssueTrackStatus_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]      UNIQUEIDENTIFIER NOT NULL,
    [Modified_On] DATETIME         NULL,
    [Modified_by] UNIQUEIDENTIFIER NULL,
    [IsDeleted]   BIT              CONSTRAINT [DF_SAPIssueTrackStatus_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SAPIssueTrackStatus] PRIMARY KEY CLUSTERED ([Id] ASC)
);

