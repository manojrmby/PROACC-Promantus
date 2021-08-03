CREATE TABLE [dbo].[MaterialityScore] (
    [FileUploadID]   UNIQUEIDENTIFIER NOT NULL,
    [FunctionalArea] NVARCHAR (500)   NULL,
    [Count]          INT              NULL,
    [Percentage]     DECIMAL (4, 2)   NULL,
    [isActive]       BIT              CONSTRAINT [DF_MaterialityScore_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]         DATETIME         CONSTRAINT [DF_MaterialityScore_Cre_on] DEFAULT (getdate()) NOT NULL,
    [Cre_By]         UNIQUEIDENTIFIER NOT NULL,
    [Modified_on]    DATETIME         NULL,
    [Modified_by]    UNIQUEIDENTIFIER NULL,
    [isDeleted]      BIT              CONSTRAINT [DF_MaterialityScore_isDeleted] DEFAULT ((0)) NOT NULL
);

