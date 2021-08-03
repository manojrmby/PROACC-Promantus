CREATE TABLE [dbo].[AMVersion] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [Version_Name] NVARCHAR (50)    NULL,
    [FileName]     UNIQUEIDENTIFIER CONSTRAINT [DF_AMVertion_FileName] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [isActive]     BIT              CONSTRAINT [DF_ActivityMaster_FileUpload_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_On]       DATETIME         CONSTRAINT [DF_ActivityMaster_FileUpload_Cre_On] DEFAULT (getdate()) NOT NULL,
    [Cre_By]       UNIQUEIDENTIFIER CONSTRAINT [DF_AMVertion_Cre_By] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Modified_on]  DATETIME         NULL,
    [Modified_By]  UNIQUEIDENTIFIER NULL,
    [isDeleted]    BIT              CONSTRAINT [DF_ActivityMaster_FileUpload_isDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_AMVertion] PRIMARY KEY CLUSTERED ([Id] ASC)
);



