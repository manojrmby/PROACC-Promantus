CREATE TABLE [dbo].[PMTaskCategory] (
    [ID]          INT              IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (50)    NOT NULL,
    [isActive]    BIT              CONSTRAINT [DF_PMTaskCategory_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]      DATETIME         CONSTRAINT [DF_PMTaskCategory_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]      UNIQUEIDENTIFIER CONSTRAINT [DF_PMTaskCategory_Cre_By] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Modified_On] DATETIME         NULL,
    [Modified_by] UNIQUEIDENTIFIER NULL,
    [IsDeleted]   BIT              CONSTRAINT [DF_PMTaskCategory_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_PMTaskCategory] PRIMARY KEY CLUSTERED ([ID] ASC)
);

