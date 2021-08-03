CREATE TYPE [dbo].[UploadSimplificationType] AS TABLE (
    [Title]              NVARCHAR (MAX) NULL,
    [Category]           NVARCHAR (MAX) NULL,
    [Relevance]          NVARCHAR (MAX) NULL,
    [LoB/Technology]     NVARCHAR (MAX) NULL,
    [Business Area]      NVARCHAR (MAX) NULL,
    [Consistency Status] NVARCHAR (MAX) NULL,
    [Manual Status]      NVARCHAR (MAX) NULL,
    [SAP Notes]          NVARCHAR (MAX) NULL,
    [Relevance Summary]  NVARCHAR (MAX) NULL);

