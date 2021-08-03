CREATE TYPE [dbo].[UploadSAPUserList] AS TABLE (
    [User]          NVARCHAR (MAX) NULL,
    [UserType]      NVARCHAR (MAX) NULL,
    [Valid_from]    DATETIME       NULL,
    [Valid_through] DATETIME       NULL,
    [Last_Logon]    NVARCHAR (MAX) NULL,
    [System]        NVARCHAR (MAX) NULL,
    [Catergory]     NVARCHAR (MAX) NULL);

