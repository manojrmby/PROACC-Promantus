CREATE TABLE [dbo].[SAPUserList] (
    [FileUploadID]  UNIQUEIDENTIFIER NOT NULL,
    [User]          VARCHAR (500)    NULL,
    [UserType]      VARCHAR (500)    NULL,
    [Valid_from]    VARCHAR (500)    NULL,
    [Valid_through] VARCHAR (500)    NULL,
    [Last_Logon]    VARCHAR (500)    NULL,
    [System]        VARCHAR (500)    NULL,
    [Catergory]     VARCHAR (500)    NULL,
    [isActive]      BIT              CONSTRAINT [DF_SAPUserList_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]        DATETIME         CONSTRAINT [DF_SAPUserList_Cre_on] DEFAULT (getdate()) NOT NULL,
    [Cre_By]        UNIQUEIDENTIFIER NULL,
    [Modified_on]   DATETIME         NULL,
    [Modified_by]   UNIQUEIDENTIFIER NULL,
    [isDeleted]     BIT              CONSTRAINT [DF_SAPUserList_isDeleted] DEFAULT ((0)) NOT NULL
);

