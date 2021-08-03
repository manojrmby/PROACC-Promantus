CREATE TABLE [dbo].[SAPInput_PreConvertion] (
    [FileUploadID]            UNIQUEIDENTIFIER NOT NULL,
    [Relevance]               NVARCHAR (MAX)   NULL,
    [Last Consistency Result] NVARCHAR (MAX)   NULL,
    [Exemption Possible]      NVARCHAR (MAX)   NULL,
    [ID]                      NVARCHAR (MAX)   NULL,
    [Title]                   NVARCHAR (MAX)   NULL,
    [Lob/Technology]          NVARCHAR (MAX)   NULL,
    [Business Area]           NVARCHAR (MAX)   NULL,
    [Catetory]                NVARCHAR (MAX)   NULL,
    [Component]               NVARCHAR (MAX)   NULL,
    [Status]                  NVARCHAR (MAX)   NULL,
    [Note]                    NVARCHAR (MAX)   NULL,
    [Application Area]        NVARCHAR (MAX)   NULL,
    [Summary]                 NVARCHAR (MAX)   NULL
);

