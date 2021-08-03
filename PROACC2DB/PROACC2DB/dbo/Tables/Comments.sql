CREATE TABLE [dbo].[Comments] (
    [Comments_Id]       UNIQUEIDENTIFIER NOT NULL,
    [User_Id]           UNIQUEIDENTIFIER NOT NULL,
    [ProjectMonitor_Id] UNIQUEIDENTIFIER NOT NULL,
    [Comments]          VARCHAR (MAX)    NOT NULL,
    [isActive]          BIT              CONSTRAINT [DF_Commants_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]            DATETIME         CONSTRAINT [DF_Commants_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]            UNIQUEIDENTIFIER CONSTRAINT [DF_Commants_Cre_By] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Modified_On]       DATETIME         CONSTRAINT [DF_Commants_Modified_On] DEFAULT (getutcdate()) NULL,
    [Modified_by]       UNIQUEIDENTIFIER NULL,
    [IsDeleted]         BIT              CONSTRAINT [DF_Commants_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Commants] PRIMARY KEY CLUSTERED ([Comments_Id] ASC)
);

