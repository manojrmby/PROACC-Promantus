CREATE TABLE [dbo].[FileMaster] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [File]        VARCHAR (500)    NOT NULL,
    [Filefor]     VARCHAR (50)     NULL,
    [isActive]    BIT              CONSTRAINT [DF_FileMaster_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]      DATETIME         CONSTRAINT [DF_FileMaster_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]      UNIQUEIDENTIFIER NOT NULL,
    [Modified_On] DATETIME         NULL,
    [Modified_by] UNIQUEIDENTIFIER NULL,
    [IsDeleted]   BIT              CONSTRAINT [DF_FileMaster_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_FileMaster] PRIMARY KEY CLUSTERED ([Id] ASC)
);



