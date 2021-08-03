CREATE TABLE [dbo].[SAPInput_SimplificationReport] (
    [FileUploadID]       UNIQUEIDENTIFIER NULL,
    [Title]              NVARCHAR (255)   NULL,
    [Category]           NVARCHAR (255)   NULL,
    [Relevance]          NVARCHAR (255)   NULL,
    [LoB/Technology]     NVARCHAR (255)   NULL,
    [Business Area]      NVARCHAR (255)   NULL,
    [Consistency Status] NVARCHAR (255)   NULL,
    [Manual Status]      NVARCHAR (255)   NULL,
    [SAP Notes]          NVARCHAR (255)   NULL,
    [Relevance Summary]  NVARCHAR (255)   NULL
);

