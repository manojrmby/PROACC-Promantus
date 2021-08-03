CREATE TYPE [dbo].[UploadUserlistType] AS TABLE (
    [User]               NVARCHAR (MAX) NULL,
    [User Type]          NVARCHAR (MAX) NULL,
    [Valid from]         DATETIME       NULL,
    [Valid through]      DATETIME       NULL,
    [Date of Last Logon] DATETIME       NULL,
    [System]             NVARCHAR (MAX) NULL,
    [Catergory]          NVARCHAR (MAX) NULL);

