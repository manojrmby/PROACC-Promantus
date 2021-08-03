CREATE TABLE [dbo].[ScenarioMaster] (
    [ScenarioId]       INT              IDENTITY (1, 1) NOT NULL,
    [ScenarioName]     VARCHAR (50)     NOT NULL,
    [BuildingBlock_Id] VARCHAR (50)     NULL,
    [isActive]         BIT              CONSTRAINT [DF_ScenarioMaster_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]           DATETIME         CONSTRAINT [DF_ScenarioMaster_Cre_on] DEFAULT (getutcdate()) NOT NULL,
    [Cre_By]           UNIQUEIDENTIFIER CONSTRAINT [DF_ScenarioMaster_Cre_By] DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Modified_On]      DATETIME         CONSTRAINT [DF_ScenarioMaster_Modified_On] DEFAULT (getutcdate()) NULL,
    [Modified_by]      UNIQUEIDENTIFIER NULL,
    [IsDeleted]        BIT              CONSTRAINT [DF_ScenarioMaster_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ScenarioMaster] PRIMARY KEY CLUSTERED ([ScenarioId] ASC)
);

