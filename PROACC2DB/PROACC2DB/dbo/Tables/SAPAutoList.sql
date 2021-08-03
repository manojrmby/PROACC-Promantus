CREATE TABLE [dbo].[SAPAutoList] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (250)    NOT NULL,
    [isActive]    BIT              CONSTRAINT [DF_SAPAutoList_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]      DATETIME         NULL,
    [Cre_by]      UNIQUEIDENTIFIER NULL,
    [Modified_On] DATETIME         NULL,
    [Modified_by] UNIQUEIDENTIFIER NULL,
    [isDeleted]   BIT              CONSTRAINT [DF_SAPAutoList_isDeleted] DEFAULT ((0)) NULL
);

