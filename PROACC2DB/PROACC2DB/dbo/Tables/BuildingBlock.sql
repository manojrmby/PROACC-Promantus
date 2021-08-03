CREATE TABLE [dbo].[BuildingBlock] (
    [block_ID]    INT              IDENTITY (1, 1) NOT NULL,
    [Block_Name]  NVARCHAR (50)    NOT NULL,
    [isActive]    BIT              CONSTRAINT [DF_Buldingblock_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]      DATETIME         CONSTRAINT [DF_Buldingblock_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]      UNIQUEIDENTIFIER CONSTRAINT [DF_Buldingblock_Cre_By] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Modified_On] DATETIME         NULL,
    [Modified_by] UNIQUEIDENTIFIER NULL,
    [IsDeleted]   BIT              CONSTRAINT [DF_Buldingblock_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Buldingblock] PRIMARY KEY CLUSTERED ([block_ID] ASC)
);

