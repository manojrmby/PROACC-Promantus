CREATE TABLE [dbo].[SAPConnection] (
    [Id]              UNIQUEIDENTIFIER NOT NULL,
    [Instance_Id]     UNIQUEIDENTIFIER NOT NULL,
    [DestinationName] VARCHAR (150)    NULL,
    [AppServerHost]   VARCHAR (150)    NULL,
    [SystemNumber]    VARCHAR (150)    NULL,
    [SAPRouter]       VARCHAR (150)    NULL,
    [SAPUser]         VARCHAR (150)    NULL,
    [SAPPassword]     VARCHAR (150)    NULL,
    [Client]          VARCHAR (150)    NULL,
    [Language]        VARCHAR (150)    NULL,
    [PoolSize]        VARCHAR (150)    NULL,
    [isActive]        BIT              CONSTRAINT [DF_SAPConnection_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_on]          DATETIME         CONSTRAINT [DF_SAPConnection_Cre_on] DEFAULT (getdate()) NOT NULL,
    [Cre_By]          UNIQUEIDENTIFIER NOT NULL,
    [Modified_on]     DATETIME         NULL,
    [Modified_By]     UNIQUEIDENTIFIER NULL,
    [isDeleted]       BIT              CONSTRAINT [DF_SAPConnection_isDeleted] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_SAPConnection] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_SAPConnection_Instance] FOREIGN KEY ([Instance_Id]) REFERENCES [dbo].[Instance] ([Instance_id])
);

