CREATE TABLE [dbo].[SAPInput_INSTALLEDSOFTWARECOMPONENT] (
    [FileUploadID] UNIQUEIDENTIFIER NOT NULL,
    [Component]    VARCHAR (500)    NULL,
    [Release]      VARCHAR (500)    NULL,
    [SP_Level]     VARCHAR (500)    NULL,
    [Comp_Type]    VARCHAR (500)    NULL,
    [Description]  VARCHAR (500)    NULL,
    [isActive]     BIT              CONSTRAINT [DF_SAPInput_INSTALLEDSOFTWARECOMPONENT_isActive] DEFAULT ((1)) NOT NULL,
    [Cre_On]       DATETIME         CONSTRAINT [DF_SAPInput_INSTALLEDSOFTWARECOMPONENT_Cre_On] DEFAULT (getdate()) NOT NULL,
    [Cre_By]       UNIQUEIDENTIFIER NOT NULL,
    [Modified_on]  DATETIME         NULL,
    [Modified_by]  UNIQUEIDENTIFIER NULL,
    [isDeleted]    BIT              CONSTRAINT [DF_SAPInput_INSTALLEDSOFTWARECOMPONENT_isDeleted] DEFAULT ((0)) NOT NULL
);

