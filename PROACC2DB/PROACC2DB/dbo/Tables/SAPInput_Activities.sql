CREATE TABLE [dbo].[SAPInput_Activities] (
    [FileUploadID]                 UNIQUEIDENTIFIER NOT NULL,
    [Related Simplification Items] NVARCHAR (MAX)   NULL,
    [Activities]                   NVARCHAR (MAX)   NULL,
    [Phase]                        NVARCHAR (MAX)   NULL,
    [Condition]                    NVARCHAR (MAX)   NULL,
    [Additional Information]       NVARCHAR (MAX)   NULL,
    [Inserted_on]                  DATETIME         CONSTRAINT [DF_SAPInput_Activities_Inserted_on] DEFAULT (getdate()) NOT NULL
);

