CREATE TABLE [dbo].[User_Type] (
    [UserTypeID]  INT              IDENTITY (1, 1) NOT NULL,
    [UserType]    VARCHAR (50)     NOT NULL,
    [isActive]    BIT              NOT NULL,
    [Cre_on]      DATETIME         CONSTRAINT [DF_User_Type_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]      UNIQUEIDENTIFIER CONSTRAINT [DF_User_Type_Cre_By] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Modified_On] DATETIME         CONSTRAINT [DF_User_Type_Modified_On] DEFAULT (getutcdate()) NULL,
    [Modified_by] UNIQUEIDENTIFIER CONSTRAINT [DF_User_Type_Modified_by] DEFAULT ('00000000-0000-0000-0000-000000000000') NULL,
    [IsDeleted]   BIT              CONSTRAINT [DF_User_Type_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_User_Type] PRIMARY KEY CLUSTERED ([UserTypeID] ASC)
);

