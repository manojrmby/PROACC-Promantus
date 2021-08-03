CREATE TABLE [dbo].[IndustrySector] (
    [IndustrySector_ID] INT              IDENTITY (1, 1) NOT NULL,
    [Industry_Sector]   NVARCHAR (500)   NOT NULL,
    [IsActive]          BIT              NULL,
    [Cre_on]            DATETIME         NOT NULL,
    [Cre_By]            UNIQUEIDENTIFIER NOT NULL,
    [Modified_On]       DATETIME         NULL,
    [Modified_by]       UNIQUEIDENTIFIER NULL,
    [IsDeleted]         BIT              NULL,
    CONSTRAINT [PK_IndustrySector] PRIMARY KEY CLUSTERED ([IndustrySector_ID] ASC)
);

