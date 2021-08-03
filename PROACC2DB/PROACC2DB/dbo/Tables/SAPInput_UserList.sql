CREATE TABLE [dbo].[SAPInput_UserList] (
    [FileUploadID] UNIQUEIDENTIFIER NOT NULL,
    [Users]        VARCHAR (500)    NULL,
    [User_Type]    VARCHAR (500)    NULL,
    [Valid_From]   DATETIME         NULL,
    [Valid_To]     DATETIME         NULL,
    [Last_logon]   DATETIME         NULL,
    [System]       VARCHAR (500)    NULL,
    [Catergory]    VARCHAR (500)    NULL,
    [Inserted_on]  DATETIME         CONSTRAINT [DF_SAPInput_UserList_Inserted_on] DEFAULT (getutcdate()) NOT NULL
);

