CREATE TABLE [dbo].[ResetPassword] (
    [ResetId]    UNIQUEIDENTIFIER NOT NULL,
    [UserId]     UNIQUEIDENTIFIER NOT NULL,
    [Status]     BIT              CONSTRAINT [DF_ResetPassword_Status] DEFAULT ((0)) NOT NULL,
    [CreatedBy]  UNIQUEIDENTIFIER NOT NULL,
    [CreatedOn]  DATETIME         CONSTRAINT [DF_ResetPassword_CreatedOn] DEFAULT (getutcdate()) NOT NULL,
    [ModifiedBy] UNIQUEIDENTIFIER NULL,
    [ModifiedOn] DATETIME         NULL,
    [IsActive]   BIT              CONSTRAINT [DF_ResetPassword_IsActive] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_ResetPassword] PRIMARY KEY CLUSTERED ([ResetId] ASC),
    CONSTRAINT [FK_ResetPassword_UserMaster] FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserMaster] ([UserId])
);

