CREATE TABLE [dbo].[SAPInput_BWExtractors] (
    [FileUploadID]                 UNIQUEIDENTIFIER NOT NULL,
    [Extractor Name]               VARCHAR (500)    NULL,
    [Application Area]             VARCHAR (500)    NULL,
    [Related Simplification Items] VARCHAR (500)    NULL,
    [Additional Information]       VARCHAR (500)    NULL,
    [isActive]                     BIT              CONSTRAINT [DF_SAPInput_BWExtractors_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]                       DATETIME         CONSTRAINT [DF_SAPInput_BWExtractors_Cre_on] DEFAULT (getdate()) NOT NULL,
    [Cre_By]                       UNIQUEIDENTIFIER NOT NULL,
    [Modified_on]                  DATETIME         NULL,
    [Modified_by]                  UNIQUEIDENTIFIER NULL,
    [isDeleted]                    BIT              CONSTRAINT [DF_SAPInput_BWExtractors_isDeleted] DEFAULT ((0)) NOT NULL
);

