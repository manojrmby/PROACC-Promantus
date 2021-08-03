CREATE TABLE [dbo].[ExceptionLogging] (
    [Logid]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [ExceptionMsg]    VARCHAR (100)  NULL,
    [ExceptionType]   VARCHAR (100)  NULL,
    [ExceptionSource] NVARCHAR (MAX) NULL,
    [ExceptionURL]    VARCHAR (100)  NULL,
    [Logdate]         DATETIME       NULL,
    CONSTRAINT [PK_ExceptionLogging] PRIMARY KEY CLUSTERED ([Logid] ASC)
);

