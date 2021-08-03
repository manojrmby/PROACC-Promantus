CREATE TABLE [dbo].[Bell] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Table_Id]    UNIQUEIDENTIFIER NOT NULL,
    [Table_Name]  VARCHAR (MAX)    NOT NULL,
    [UserId]      UNIQUEIDENTIFIER NOT NULL,
    [Body]        NVARCHAR (MAX)   NULL,
    [Link]        NVARCHAR (MAX)   NULL,
    [isActive]    BIT              CONSTRAINT [DF_Bell_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]      DATETIME         NOT NULL,
    [Cre_By]      UNIQUEIDENTIFIER NOT NULL,
    [Modified_On] DATETIME         NULL,
    [Modified_by] UNIQUEIDENTIFIER NULL,
    [isDeleted]   BIT              CONSTRAINT [DF_Bell_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Bell] PRIMARY KEY CLUSTERED ([Id] ASC)
);

