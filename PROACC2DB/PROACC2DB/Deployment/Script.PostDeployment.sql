﻿/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

--DEFAULT DATA START


CREATE PROCEDURE [dbo].[Populate_dbo_User_Type]
AS
BEGIN
/*
	Table's data:    [dbo].[User_Type]
	Data Source:     [PRO_IIS\SQLEXPRESS].[ProAcc2_Dev]
	Created on:      7/19/2021 9:25:02 PM
*/


IF NOT EXISTS (SELECT TOP (1) * FROM [dbo].[User_Type])
BEGIN
	PRINT 'Populating data into [dbo].[User_Type]';

	SET IDENTITY_INSERT [dbo].[User_Type] ON;

	;WITH cte_data
	as (SELECT [UserTypeID], [UserType], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted] FROM 
	(VALUES
	  (1,	'Admin',	1,	'20200411 16:33:55.070',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (2,	'Consultant',	1,	'20200411 16:34:30.317',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (3,	'Customer',	1,	'20200411 16:34:41.260',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (4,	'Project Manager',	1,	'20200531 00:00:00.000',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	) as v ([UserTypeID], [UserType], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted])
)
	INSERT INTO [dbo].[User_Type]
	([UserTypeID], [UserType], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted])
	SELECT [UserTypeID], [UserType], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted]
	FROM cte_data;

	SET IDENTITY_INSERT [dbo].[User_Type] OFF;

END

-- End data of table: [dbo].[User_Type] --
END
GO
CREATE PROCEDURE [dbo].[Populate_dbo_UserMaster]
AS
BEGIN
/*
	Table's data:    [dbo].[UserMaster]
	Data Source:     [PRO_IIS\SQLEXPRESS].[PROACC2QA]
	Created on:      7/19/2021 9:40:44 PM
	Scripted by:     sa
	Generated by:    Data Script Writer - ver. 2.3.0.0
	GitHub repo URL: https://github.com/SQLPlayer/DataScriptWriter/
*/


IF NOT EXISTS (SELECT TOP (1) * FROM [dbo].[UserMaster])
BEGIN
	PRINT 'Populating data into [dbo].[UserMaster]';
	;WITH cte_data
	as (SELECT [UserId], [Name], [EMail], [Countrycode], [DialCode], [Phone], [LoginId], [Password], [RoleID], [UserTypeID], [Customer_Id], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted] FROM 
	(VALUES
	  ('b40c3e77-2673-4dd3-a32b-ca42908dd6a0',	N'Admin',	N'admin@gmail.com',	null,	null,	N'9876543210',	'Admin',	N'KpSGQ4j8b5v2PUVMXJ/rBQ==',	1,	1,	'00000000-0000-0000-0000-000000000000',	1,	'20201015 13:39:38.997',	'00000000-0000-0000-0000-000000000000',	'20210308 10:08:21.270',	'b40c3e77-2673-4dd3-a32b-ca42908dd6a0',	0)
	) as v ([UserId], [Name], [EMail], [Countrycode], [DialCode], [Phone], [LoginId], [Password], [RoleID], [UserTypeID], [Customer_Id], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted])
)
	INSERT INTO [dbo].[UserMaster]
	([UserId], [Name], [EMail], [Countrycode], [DialCode], [Phone], [LoginId], [Password], [RoleID], [UserTypeID], [Customer_Id], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted])
	SELECT [UserId], [Name], [EMail], [Countrycode], [DialCode], [Phone], [LoginId], [Password], [RoleID], [UserTypeID], [Customer_Id], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted]
	FROM cte_data;

END

-- End data of table: [dbo].[UserMaster] --
END
GO

CREATE PROCEDURE [dbo].[Populate_dbo_RoleMaster]
AS
BEGIN
/*
	Table's data:    [dbo].[RoleMaster]
	Data Source:     [PRO_IIS\SQLEXPRESS].[PROACC2QA]
	Created on:      7/19/2021 9:49:29 PM
	Scripted by:     sa
	Generated by:    Data Script Writer - ver. 2.3.0.0
	GitHub repo URL: https://github.com/SQLPlayer/DataScriptWriter/
*/


IF NOT EXISTS (SELECT TOP (1) * FROM [dbo].[RoleMaster])
BEGIN
	PRINT 'Populating data into [dbo].[RoleMaster]';
	SET IDENTITY_INSERT [dbo].[RoleMaster] ON;

	;WITH cte_data
	as (SELECT [RoleId], [RoleName], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted], [Description] FROM 
	(VALUES
	  (1,	'Admin',	1,	'20200518 06:36:06.700',	'25fb1790-a743-464e-9e75-0c48a75cf308',	null,	null,	0,	null)
	, (2,	'Consultant',	1,	'20200519 19:13:17.680',	'25fb1790-a743-464e-9e75-0c48a75cf308',	'20200617 10:28:55.607',	'b40c3e77-2673-4dd3-a32b-ca42908dd6a0',	0,	null)
	, (3,	'Project Manager',	1,	'20200518 06:36:06.700',	'25fb1790-a743-464e-9e75-0c48a75cf308',	'20200826 12:49:45.997',	'772ddfd7-c99d-49dd-b962-dc25f10bb21f',	0,	null)
	, (4,	'Customer',	1,	'20200701 13:39:08.117',	'b40c3e77-2673-4dd3-a32b-ca42908dd6a0',	'20200728 08:24:35.973',	'b40c3e77-2673-4dd3-a32b-ca42908dd6a0',	0,	null)
	, (5,	'Basis Consultant',	1,	'20200518 06:36:06.700',	'25fb1790-a743-464e-9e75-0c48a75cf308',	'20210401 06:34:21.877',	'b40c3e77-2673-4dd3-a32b-ca42908dd6a0',	0,	'')
	, (6,	'Basis & Functional',	1,	'20200519 19:13:17.680',	'25fb1790-a743-464e-9e75-0c48a75cf308',	'20200617 10:28:55.607',	'b40c3e77-2673-4dd3-a32b-ca42908dd6a0',	0,	null)
	, (7,	'Security Consultant',	1,	'20200518 06:36:06.700',	'25fb1790-a743-464e-9e75-0c48a75cf308',	'20200826 12:49:45.997',	'772ddfd7-c99d-49dd-b962-dc25f10bb21f',	0,	null)
	, (8,	'ABAP Consultant',	1,	'20200519 19:13:17.680',	'25fb1790-a743-464e-9e75-0c48a75cf308',	'20200827 06:46:46.853',	'772ddfd7-c99d-49dd-b962-dc25f10bb21f',	0,	null)
	, (9,	'Functional Consultant',	1,	'20200521 17:23:37.550',	'25fb1790-a743-464e-9e75-0c48a75cf308',	'20200902 07:06:33.407',	'772ddfd7-c99d-49dd-b962-dc25f10bb21f',	0,	null)
	) as v ([RoleId], [RoleName], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted], [Description])
)
	INSERT INTO [dbo].[RoleMaster]
	([RoleId], [RoleName], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted], [Description])
	SELECT [RoleId], [RoleName], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted], [Description]
	FROM cte_data;

	SET IDENTITY_INSERT [dbo].[RoleMaster] OFF;

END

-- End data of table: [dbo].[RoleMaster] --
END
GO

CREATE PROCEDURE [dbo].[Populate_dbo_IndustrySector]
AS
BEGIN
/*
	Table's data:    [dbo].[IndustrySector]
	Data Source:     [PRO_IIS\SQLEXPRESS].[PROACC2QA]
	Created on:      7/20/2021 12:43:18 PM
	Scripted by:     sa
	Generated by:    Data Script Writer - ver. 2.3.0.0
	GitHub repo URL: https://github.com/SQLPlayer/DataScriptWriter/
*/


IF NOT EXISTS (SELECT TOP (1) * FROM [dbo].[IndustrySector])
BEGIN
	PRINT 'Populating data into [dbo].[IndustrySector]';
	SET IDENTITY_INSERT [dbo].[IndustrySector] ON;

	;WITH cte_data
	as (SELECT [IndustrySector_ID], [Industry_Sector], [IsActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted] FROM 
	(VALUES
	  (1,	N'Chemical Industry',	1,	'20201019 12:24:28.967',	'b40c3e77-2673-4dd3-a32b-ca42908dd6a0',	null,	null,	0)
	, (2,	N'Mechanical Industry',	1,	'20201019 12:24:28.967',	'b40c3e77-2673-4dd3-a32b-ca42908dd6a0',	null,	null,	0)
	, (3,	N'Pharmaceuticals',	1,	'20201019 12:24:28.967',	'b40c3e77-2673-4dd3-a32b-ca42908dd6a0',	null,	null,	0)
	, (4,	N'Construction',	1,	'20201019 12:24:28.967',	'b40c3e77-2673-4dd3-a32b-ca42908dd6a0',	null,	null,	0)
	, (5,	N'Retail',	1,	'20201019 12:24:28.967',	'b40c3e77-2673-4dd3-a32b-ca42908dd6a0',	null,	null,	0)
	) as v ([IndustrySector_ID], [Industry_Sector], [IsActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted])
)
	INSERT INTO [dbo].[IndustrySector]
	([IndustrySector_ID], [Industry_Sector], [IsActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted])
	SELECT [IndustrySector_ID], [Industry_Sector], [IsActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted]
	FROM cte_data;

	SET IDENTITY_INSERT [dbo].[IndustrySector] OFF;

END

-- End data of table: [dbo].[IndustrySector] --
END
GO

CREATE PROCEDURE [dbo].[Populate_dbo_BuildingBlock]
AS
BEGIN
/*
	Table's data:    [dbo].[BuildingBlock]
	Data Source:     [PRO_IIS\SQLEXPRESS].[PROACC2QA]
	Created on:      7/20/2021 12:50:37 PM
	Scripted by:     sa
	Generated by:    Data Script Writer - ver. 2.3.0.0
	GitHub repo URL: https://github.com/SQLPlayer/DataScriptWriter/
*/


IF NOT EXISTS (SELECT TOP (1) * FROM [dbo].[BuildingBlock])
BEGIN
	PRINT 'Populating data into [dbo].[BuildingBlock]';
	SET IDENTITY_INSERT [dbo].[BuildingBlock] ON;

	;WITH cte_data
	as (SELECT [block_ID], [Block_Name], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted] FROM 
	(VALUES
	  (1,	N'Application Overview',	1,	'20201021 08:12:25.057',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (2,	N'Assessment Reports',	1,	'20201021 08:12:25.057',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (3,	N'Clean-up Custom code',	1,	'20201021 08:12:25.057',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (4,	N'Custom Code Adaption',	1,	'20201021 08:12:25.057',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (5,	N'Custom Code Analysis',	1,	'20201021 08:12:25.057',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (6,	N'Data Management',	1,	'20201021 08:12:25.057',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (7,	N'Follow-up Activites',	1,	'20201021 08:12:25.057',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (8,	N'Implement SI Checks',	1,	'20201021 08:12:25.057',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (9,	N'Implement SI Checks, CVI',	1,	'20201021 08:12:25.057',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (10,	N'Integration Testing and UAT',	1,	'20201021 08:12:25.057',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (11,	N'Interface Checks',	1,	'20201021 08:12:25.057',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (12,	N'PRE-SUM',	1,	'20201021 08:12:25.057',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (13,	N'Readiness Check',	1,	'20201021 08:12:25.057',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (14,	N'SAP Fiori Config',	1,	'20201021 08:12:25.057',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (15,	N'SAP Security',	1,	'20201021 08:12:25.057',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (16,	N'SPDD/SPAU SPAU_ENH',	1,	'20201021 08:12:25.057',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (17,	N'SUM (DMO)',	1,	'20201021 08:12:25.057',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	) as v ([block_ID], [Block_Name], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted])
)
	INSERT INTO [dbo].[BuildingBlock]
	([block_ID], [Block_Name], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted])
	SELECT [block_ID], [Block_Name], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted]
	FROM cte_data;

	SET IDENTITY_INSERT [dbo].[BuildingBlock] OFF;

END

-- End data of table: [dbo].[BuildingBlock] --
END
GO

CREATE PROCEDURE [dbo].[Populate_dbo_PhaseMaster]
AS
BEGIN
/*
	Table's data:    [dbo].[PhaseMaster]
	Data Source:     [PRO_IIS\SQLEXPRESS].[PROACC2QA]
	Created on:      7/20/2021 1:53:41 PM
	Scripted by:     sa
	Generated by:    Data Script Writer - ver. 2.3.0.0
	GitHub repo URL: https://github.com/SQLPlayer/DataScriptWriter/
*/


IF NOT EXISTS (SELECT TOP (1) * FROM [dbo].[PhaseMaster])
BEGIN
	PRINT 'Populating data into [dbo].[PhaseMaster]';
	SET IDENTITY_INSERT [dbo].[PhaseMaster] ON;

	;WITH cte_data
	as (SELECT [Id], [PhaseName], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted] FROM 
	(VALUES
	  (1,	'Assessment',	1,	'20200518 12:28:20.200',	'25fb1790-a743-464e-9e75-0c48a75cf308',	null,	null,	0)
	, (2,	'Pre Conversion',	1,	'20200520 19:03:14.407',	'25fb1790-a743-464e-9e75-0c48a75cf308',	null,	null,	0)
	, (3,	'Conversion',	1,	'20200520 19:03:14.407',	'25fb1790-a743-464e-9e75-0c48a75cf308',	null,	null,	0)
	, (4,	'Post Conversion',	1,	'20200520 19:03:46.423',	'25fb1790-a743-464e-9e75-0c48a75cf308',	null,	null,	0)
	, (5,	'Validation Testing',	0,	'20200521 19:57:56.987',	'25fb1790-a743-464e-9e75-0c48a75cf308',	'20200521 19:58:01.947',	'25fb1790-a743-464e-9e75-0c48a75cf308',	0)
	) as v ([Id], [PhaseName], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted])
)
	INSERT INTO [dbo].[PhaseMaster]
	([Id], [PhaseName], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted])
	SELECT [Id], [PhaseName], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted]
	FROM cte_data;

	SET IDENTITY_INSERT [dbo].[PhaseMaster] OFF;

END

-- End data of table: [dbo].[PhaseMaster] --
END
GO

CREATE PROCEDURE [dbo].[Populate_dbo_ApplicationAreaMaster]
AS
BEGIN
/*
	Table's data:    [dbo].[ApplicationAreaMaster]
	Data Source:     [PRO_IIS\SQLEXPRESS].[PROACC2QA]
	Created on:      7/20/2021 1:57:12 PM
	Scripted by:     sa
	Generated by:    Data Script Writer - ver. 2.3.0.0
	GitHub repo URL: https://github.com/SQLPlayer/DataScriptWriter/
*/


IF NOT EXISTS (SELECT TOP (1) * FROM [dbo].[ApplicationAreaMaster])
BEGIN

	PRINT 'Populating data into [dbo].[ApplicationAreaMaster]';
	SET IDENTITY_INSERT [dbo].[ApplicationAreaMaster] ON;

	;WITH cte_data
	as (SELECT [Id], [ApplicationArea], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted] FROM 
	(VALUES
	  (1,	'Functional - All',	1,	'20200805 17:30:11.670',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (2,	'Technical - ABAP',	1,	'20200805 17:30:11.670',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (3,	'Technical - BASIS',	1,	'20200805 17:30:11.670',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (4,	'Technical - Infrastructure',	1,	'20200805 17:30:11.670',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (5,	'Technical - Interface',	1,	'20200805 17:30:11.670',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (6,	'Technical - Security',	1,	'20200805 17:30:11.670',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (7,	'Logistics',	1,	'20201021 13:38:28.210',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (8,	'SAP Security',	1,	'20201021 13:38:28.210',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (9,	'Testing',	1,	'20201021 13:38:28.210',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (10,	'Application Testing',	1,	'20201021 13:38:28.210',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (11,	'Finance',	1,	'20201021 13:38:28.210',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (12,	'SAP Fiori',	1,	'20201021 13:38:28.210',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	) as v ([Id], [ApplicationArea], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted])
)
	INSERT INTO [dbo].[ApplicationAreaMaster]
	([Id], [ApplicationArea], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted])
	SELECT [Id], [ApplicationArea], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted]
	FROM cte_data;

	SET IDENTITY_INSERT [dbo].[ApplicationAreaMaster] OFF;

END

-- End data of table: [dbo].[ApplicationAreaMaster] --
END
GO

CREATE PROCEDURE [dbo].[Populate_dbo_TaskType]
AS
BEGIN
/*
	Table's data:    [dbo].[TaskType]
	Data Source:     [PRO_IIS\SQLEXPRESS].[PROACC2QA]
	Created on:      7/20/2021 1:59:07 PM
	Scripted by:     sa
	Generated by:    Data Script Writer - ver. 2.3.0.0
	GitHub repo URL: https://github.com/SQLPlayer/DataScriptWriter/
*/


IF NOT EXISTS (SELECT TOP (1) * FROM [dbo].[TaskType])
BEGIN
	PRINT 'Populating data into [dbo].[TaskType]';
	;WITH cte_data
	as (SELECT [TaskId], [TaskName], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_By], [IsDeleted] FROM 
	(VALUES
	  (1,	N'Sequential',	1,	null,	null,	null,	null,	0)
	, (2,	N'Parallel',	1,	null,	null,	null,	null,	0)
	, (3,	N'Independent',	1,	null,	null,	null,	null,	0)
	) as v ([TaskId], [TaskName], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_By], [IsDeleted])
)
	INSERT INTO [dbo].[TaskType]
	([TaskId], [TaskName], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_By], [IsDeleted])
	SELECT [TaskId], [TaskName], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_By], [IsDeleted]
	FROM cte_data;

END

-- End data of table: [dbo].[TaskType] --
END
GO


CREATE PROCEDURE [dbo].[Populate_dbo_ParallelType]
AS
BEGIN
/*
	Table's data:    [dbo].[ParallelType]
	Data Source:     [PRO_IIS\SQLEXPRESS].[PROACC2QA]
	Created on:      7/20/2021 2:24:30 PM
	Scripted by:     sa
	Generated by:    Data Script Writer - ver. 2.3.0.0
	GitHub repo URL: https://github.com/SQLPlayer/DataScriptWriter/
*/


IF NOT EXISTS (SELECT TOP (1) * FROM [dbo].[ParallelType])
BEGIN
	
	PRINT 'Populating data into [dbo].[ParallelType]';
	;WITH cte_data
	as (SELECT [ParallelId], [ParallelName], [Parallel_Name], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_By], [IsDeleted] FROM 
	(VALUES
	 ('7af6bc1c-86f0-418b-80d8-765ce7fdf619',	1,	'P01',	1,	'20201208 16:57:07.460',	'd6bba950-9027-469b-bac6-ef8d6906cf9e',	null,	null,	0)
	) as v ([ParallelId], [ParallelName], [Parallel_Name], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_By], [IsDeleted])
)
	INSERT INTO [dbo].[ParallelType]
	([ParallelId], [ParallelName], [Parallel_Name], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_By], [IsDeleted])
	SELECT [ParallelId], [ParallelName], [Parallel_Name], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_By], [IsDeleted]
	FROM cte_data;

END

-- End data of table: [dbo].[ParallelType] --
END
GO

CREATE PROCEDURE [dbo].[Populate_dbo_StatusMaster]
AS
BEGIN
/*
	Table's data:    [dbo].[StatusMaster]
	Data Source:     [PRO_IIS\SQLEXPRESS].[PROACC2QA]
	Created on:      7/20/2021 2:56:48 PM
	Scripted by:     sa
	Generated by:    Data Script Writer - ver. 2.3.0.0
	GitHub repo URL: https://github.com/SQLPlayer/DataScriptWriter/
*/


IF NOT EXISTS (SELECT TOP (1) * FROM [dbo].[StatusMaster])
BEGIN
	PRINT 'Populating data into [dbo].[StatusMaster]';
	SET IDENTITY_INSERT [dbo].[StatusMaster] ON;

	;WITH cte_data
	as (SELECT [Id], [StatusName], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted] FROM 
	(VALUES
	  (1,	'Completed',	1,	'20200520 19:20:15.927',	'25fb1790-a743-464e-9e75-0c48a75cf308',	null,	null,	0)
	, (2,	'Work In Progress',	1,	'20200520 19:20:45.587',	'25fb1790-a743-464e-9e75-0c48a75cf308',	null,	null,	0)
	, (3,	'Not Applicable',	1,	'20200520 19:21:33.077',	'25fb1790-a743-464e-9e75-0c48a75cf308',	null,	null,	0)
	, (4,	'On Hold',	1,	'20200520 19:21:52.057',	'25fb1790-a743-464e-9e75-0c48a75cf308',	null,	null,	0)
	, (5,	'Yet To Start',	1,	'20200521 17:21:36.073',	'25fb1790-a743-464e-9e75-0c48a75cf308',	null,	null,	0)
	) as v ([Id], [StatusName], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted])
)
	INSERT INTO [dbo].[StatusMaster]
	([Id], [StatusName], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted])
	SELECT [Id], [StatusName], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted]
	FROM cte_data;

	SET IDENTITY_INSERT [dbo].[StatusMaster] OFF;

END

-- End data of table: [dbo].[StatusMaster] --
END
GO
CREATE PROCEDURE [dbo].[Populate_dbo_PMTaskCategory]
AS
BEGIN
/*
	Table's data:    [dbo].[PMTaskCategory]
	Data Source:     [PRO_IIS\SQLEXPRESS].[ProAcc2_Dev]
	Created on:      7/21/2021 5:24:57 PM
	Scripted by:     sa
	Generated by:    Data Script Writer - ver. 2.3.0.0
	GitHub repo URL: https://github.com/SQLPlayer/DataScriptWriter/
*/


IF NOT EXISTS (SELECT TOP (1) * FROM [dbo].[PMTaskCategory])
BEGIN
	PRINT 'Populating data into [dbo].[PMTaskCategory]';
	SET IDENTITY_INSERT [dbo].[PMTaskCategory] ON;

	;WITH cte_data
	as (SELECT [ID], [Name], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted] FROM 
	(VALUES
	  (1,	N'Project Initiation',	1,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (2,	N'Establish Project Governance',	1,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (3,	N'Plan Project',	1,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (4,	N'Project Kick-Off and On-Boarding',	1,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (5,	N'Project Standards/Infrastructure and Solution',	1,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (6,	N'Test Planning',	1,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (7,	N'Execution/Monitoring and Controlling of Results',	1,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (8,	N'Organizational Change Management Roadmap',	1,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (9,	N'Phase Closure and Sign-Off Phase Deliverables',	1,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (10,	N'Learning Design',	1,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (11,	N'Fit-to-Standard Workshop Preparation',	1,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (12,	N'Operations Impact Evaluation',	1,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (13,	N'Execution / Monitoring / Controlling Results',	0,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	1)
	, (14,	N'Release and Sprint Plan',	1,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (15,	N'Change Impact Analysis',	1,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (16,	N'Phase Closure and Sign-Off phase Deliverables',	0,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	1)
	, (17,	N'Phase Initiation',	0,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	1)
	, (18,	N'Plan Realize Phase',	1,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (19,	N'Learning Realization',	1,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (20,	N'Sprint Initiation (Iterative)',	1,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (21,	N'Execution Plan for Realize Phase',	1,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (22,	N'Sprint Closing',	1,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (23,	N'Execution/Monitoring and Controlling Results',	1,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (24,	N'Organizational Alignment',	1,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (25,	N'Phase Closure and Sign-Off Phase Deliverables',	0,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	1)
	, (26,	N'Execution / Monitoring and Controlling Results',	0,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	1)
	, (27,	N'Release Closing',	0,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	1)
	, (28,	N'Production Cutover',	0,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	1)
	, (29,	N'Project Closure and Sign-Off Project Deliverables',	0,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	1)
	, (30,	N'Operate Solution',	0,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	1)
	, (31,	N'Improve and Innovate Solution',	0,	'20200729 13:57:41.827',	'00000000-0000-0000-0000-000000000000',	null,	null,	1)
	) as v ([ID], [Name], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted])
)
	INSERT INTO [dbo].[PMTaskCategory]
	([ID], [Name], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted])
	SELECT [ID], [Name], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted]
	FROM cte_data;

	SET IDENTITY_INSERT [dbo].[PMTaskCategory] OFF;

END

-- End data of table: [dbo].[PMTaskCategory] --
END
GO
CREATE PROCEDURE [dbo].[Populate_dbo_FileMaster]
AS
BEGIN
/*
	Table's data:    [dbo].[FileMaster]
	Data Source:     [PRO_IIS\SQLEXPRESS].[ProAcc2_Dev]
	Created on:      7/21/2021 6:39:49 PM
	Scripted by:     sa
	Generated by:    Data Script Writer - ver. 2.3.0.0
	GitHub repo URL: https://github.com/SQLPlayer/DataScriptWriter/
*/


IF NOT EXISTS (SELECT TOP (1) * FROM [dbo].[FileMaster])
BEGIN
	PRINT 'Populating data into [dbo].[FileMaster]';
	SET IDENTITY_INSERT [dbo].[FileMaster] ON;

	;WITH cte_data
	as (SELECT [Id], [File], [Filefor], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted] FROM 
	(VALUES
	  (1,	'Activities',	'',	1,	'20200504 13:43:28.537',	'42dc1071-caae-4585-ab73-9adcbe85fdd5',	null,	null,	0)
	, (2,	'Bwextractors',	'',	1,	'20200504 13:45:40.147',	'42dc1071-caae-4585-ab73-9adcbe85fdd5',	null,	null,	0)
	, (3,	'CustomCode',	'',	1,	'20200504 13:45:40.147',	'42dc1071-caae-4585-ab73-9adcbe85fdd5',	null,	null,	0)
	, (4,	'HanaDatabaseTables',	'',	1,	'20200504 13:45:40.147',	'42dc1071-caae-4585-ab73-9adcbe85fdd5',	null,	null,	0)
	, (5,	'RecommendedFioriApp',	'',	1,	'20200504 13:45:40.147',	'42dc1071-caae-4585-ab73-9adcbe85fdd5',	null,	null,	0)
	, (6,	'RelevantSimplificationIte',	'',	1,	'20200504 13:45:40.147',	'42dc1071-caae-4585-ab73-9adcbe85fdd5',	null,	null,	0)
	, (7,	'SAPReadinessCheck',	'',	1,	'20200504 13:45:40.147',	'42dc1071-caae-4585-ab73-9adcbe85fdd5',	null,	null,	0)
	, (8,	'PreConvertion',	'',	1,	'20200504 13:46:39.973',	'42dc1071-caae-4585-ab73-9adcbe85fdd5',	null,	null,	0)
	, (9,	'Comprehensive Assessment & Readiness Check Report',	'PM',	1,	'20200504 13:46:39.973',	'42dc1071-caae-4585-ab73-9adcbe85fdd5',	null,	null,	0)
	, (10,	'Landscape & System Management',	'PM',	1,	'20200818 20:57:38.320',	'42dc1071-caae-4585-ab73-9adcbe85fdd5',	null,	null,	0)
	, (11,	'Custom Code Analysis',	'PM',	1,	'20200818 20:57:38.320',	'42dc1071-caae-4585-ab73-9adcbe85fdd5',	null,	null,	0)
	, (12,	'License Optimization',	'PM',	1,	'20200818 20:57:38.320',	'42dc1071-caae-4585-ab73-9adcbe85fdd5',	null,	null,	0)
	, (13,	'Current State & Vision: Roadmap Guide & Migration Scenario',	'PM',	0,	'20200818 20:57:38.320',	'42dc1071-caae-4585-ab73-9adcbe85fdd5',	null,	null,	0)
	, (14,	'Profile',	'',	1,	'20201109 08:27:56.040',	'42dc1071-caae-4585-ab73-9adcbe85fdd5',	null,	null,	0)
	, (15,	'RFCFM',	'',	1,	'20210223 06:56:56.040',	'42dc1071-caae-4585-ab73-9adcbe85fdd5',	null,	null,	0)
	, (16,	'BillingDocuments',	' ',	1,	'20210304 12:39:05.297',	'42dc1071-caae-4585-ab73-9adcbe85fdd5',	null,	null,	0)
	, (17,	'OrderDocuments',	' ',	1,	'20210304 15:39:05.297',	'42dc1071-caae-4585-ab73-9adcbe85fdd5',	null,	null,	0)
	, (18,	'SalesDocuments',	' ',	1,	'20210305 11:41:05.297',	'42dc1071-caae-4585-ab73-9adcbe85fdd5',	null,	null,	0)
	, (19,	'ComplexityAnalysis',	' ',	1,	'20210305 14:41:05.297',	'42dc1071-caae-4585-ab73-9adcbe85fdd5',	null,	null,	0)
	, (20,	'SAPUserList',	' ',	1,	'20210308 14:00:05.297',	'42dc1071-caae-4585-ab73-9adcbe85fdd5',	null,	null,	0)
	, (21,	'MaterialityScore',	' ',	1,	'20210405 14:00:05.297',	'42dc1071-caae-4585-ab73-9adcbe85fdd5',	null,	null,	0)
	) as v ([Id], [File], [Filefor], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted])
)
	INSERT INTO [dbo].[FileMaster]
	([Id], [File], [Filefor], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted])
	SELECT [Id], [File], [Filefor], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted]
	FROM cte_data;

	SET IDENTITY_INSERT [dbo].[FileMaster] OFF;

END

-- End data of table: [dbo].[FileMaster] --
END
GO

CREATE PROCEDURE [dbo].[Populate_dbo_RA_RiskOwner]
AS
BEGIN
/*
	Table's data:    [dbo].[RA_RiskOwner]
	Data Source:     [PRO_IIS\SQLEXPRESS].[ProAcc2_Dev]
	Created on:      7/21/2021 8:25:46 PM
	Scripted by:     sa
	Generated by:    Data Script Writer - ver. 2.3.0.0
	GitHub repo URL: https://github.com/SQLPlayer/DataScriptWriter/
*/


IF NOT EXISTS (SELECT TOP (1) * FROM [dbo].[RA_RiskOwner])
BEGIN

	PRINT 'Populating data into [dbo].[RA_RiskOwner]';
	SET IDENTITY_INSERT [dbo].[RA_RiskOwner] ON;

	;WITH cte_data
	as (SELECT [Id], [Risk Owner], [isActive], [Cre_on], [Cre_By], [Modified_on], [Modified_by], [isDeleted] FROM 
	(VALUES
	  (1,	'SI(System Integrator)',	1,	'20210520 12:45:37.963',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	) as v ([Id], [Risk Owner], [isActive], [Cre_on], [Cre_By], [Modified_on], [Modified_by], [isDeleted])
)
	INSERT INTO [dbo].[RA_RiskOwner]
	([Id], [Risk Owner], [isActive], [Cre_on], [Cre_By], [Modified_on], [Modified_by], [isDeleted])
	SELECT [Id], [Risk Owner], [isActive], [Cre_on], [Cre_By], [Modified_on], [Modified_by], [isDeleted]
	FROM cte_data;

	SET IDENTITY_INSERT [dbo].[RA_RiskOwner] OFF;

END

-- End data of table: [dbo].[RA_RiskOwner] --
END
GO
CREATE PROCEDURE [dbo].[Populate_dbo_RA_RiskClass]
AS
BEGIN
/*
	Table's data:    [dbo].[RA_RiskClass]
	Data Source:     [PRO_IIS\SQLEXPRESS].[ProAcc2_Dev]
	Created on:      7/21/2021 8:25:46 PM
	Scripted by:     sa
	Generated by:    Data Script Writer - ver. 2.3.0.0
	GitHub repo URL: https://github.com/SQLPlayer/DataScriptWriter/
*/


IF NOT EXISTS (SELECT TOP (1) * FROM [dbo].[RA_RiskClass])
BEGIN
	PRINT 'Populating data into [dbo].[RA_RiskClass]';
	SET IDENTITY_INSERT [dbo].[RA_RiskClass] ON;

	;WITH cte_data
	as (SELECT [Id], [Risk Class], [isActive], [Cre_on], [Cre_By], [Modified_on], [Modified_By], [isDeleted] FROM 
	(VALUES
	  (1,	'Insignificant',	1,	'20210520 12:44:40.850',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (2,	'Minor',	1,	'20210520 12:44:55.430',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (3,	'Moderate',	1,	'20210521 16:32:41.840',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (4,	'Major',	1,	'20210524 13:22:01.783',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (5,	'Disaster',	1,	'20210524 13:22:20.930',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	) as v ([Id], [Risk Class], [isActive], [Cre_on], [Cre_By], [Modified_on], [Modified_By], [isDeleted])
)
	INSERT INTO [dbo].[RA_RiskClass]
	([Id], [Risk Class], [isActive], [Cre_on], [Cre_By], [Modified_on], [Modified_By], [isDeleted])
	SELECT [Id], [Risk Class], [isActive], [Cre_on], [Cre_By], [Modified_on], [Modified_By], [isDeleted]
	FROM cte_data;

	SET IDENTITY_INSERT [dbo].[RA_RiskClass] OFF;

END

-- End data of table: [dbo].[RA_RiskClass] --
END
GO
CREATE PROCEDURE [dbo].[Populate_dbo_RA_Probability]
AS
BEGIN
/*
	Table's data:    [dbo].[RA_Probability]
	Data Source:     [PRO_IIS\SQLEXPRESS].[ProAcc2_Dev]
	Created on:      7/21/2021 8:25:45 PM
	Scripted by:     sa
	Generated by:    Data Script Writer - ver. 2.3.0.0
	GitHub repo URL: https://github.com/SQLPlayer/DataScriptWriter/
*/

IF NOT EXISTS (SELECT TOP (1) * FROM [dbo].[RA_Probability])
BEGIN
	PRINT 'Populating data into [dbo].[RA_Probability]';

	SET IDENTITY_INSERT [dbo].[RA_Probability] ON;

	;WITH cte_data
	as (SELECT [Id], [Probability], [isActive], [Cre_on], [Cre_By], [Modified_on], [Modified_By], [isDeleted] FROM 
	(VALUES
	  (1,	'<10%',	1,	'20210520 12:41:13.810',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (2,	'10-20%',	1,	'20210520 12:42:22.260',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (3,	'20-40%',	1,	'20210521 17:26:16.837',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (4,	'40-60%',	1,	'20210521 17:26:46.337',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (5,	'60-80%',	1,	'20210521 17:26:56.363',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	) as v ([Id], [Probability], [isActive], [Cre_on], [Cre_By], [Modified_on], [Modified_By], [isDeleted])
)
	INSERT INTO [dbo].[RA_Probability]
	([Id], [Probability], [isActive], [Cre_on], [Cre_By], [Modified_on], [Modified_By], [isDeleted])
	SELECT [Id], [Probability], [isActive], [Cre_on], [Cre_By], [Modified_on], [Modified_By], [isDeleted]
	FROM cte_data;

	SET IDENTITY_INSERT [dbo].[RA_Probability] OFF;

END

-- End data of table: [dbo].[RA_Probability] --
END
GO


CREATE PROCEDURE [dbo].[Populate_dbo_MailTemplate]
AS
BEGIN
/*
	Table's data:    [dbo].[MailTemplate]
	Data Source:     [PRO_IIS\SQLEXPRESS].[PROACC2QA]
	Created on:      8/2/2021 3:34:23 PM
	Scripted by:     sa
	Generated by:    Data Script Writer - ver. 2.3.0.0
	GitHub repo URL: https://github.com/SQLPlayer/DataScriptWriter/
*/


IF NOT EXISTS (SELECT TOP (1) * FROM [dbo].[MailTemplate])
BEGIN
	PRINT 'Populating data into [dbo].[MailTemplate]';
	SET IDENTITY_INSERT [dbo].[MailTemplate] ON;

	;WITH cte_data
	as (SELECT [id], [TemplateName], [FileName], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted] FROM 
	(VALUES
	  (1,	N'Completed ',	N'T1        ',	1,	'20200708 09:59:03.003',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (2,	N'Next Task ',	N'T2        ',	1,	'20200711 16:16:54.693',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (3,	N'First Task',	N'T3        ',	1,	'20200711 16:16:54.693',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (4,	N'Hold Released',	N'T4',	1,	'20200721 20:26:17.243',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (5,	N'Questionnaire',	N'T5',	1,	'20200804 01:08:47.550',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (6,	N'IssueTrack',	N'T6',	1,	'20200815 12:44:56.373',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (7,	N'SendCustomerMail',	N'T7',	1,	'20200819 10:07:00.010',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (8,	N'Reset Mail',	N'T8',	1,	'20200916 13:02:50.937',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (9,	N'Instance Created',	N'T9',	1,	'20201208 08:40:15.687',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (10,	N'Project Created',	N'T10',	1,	'20201208 08:40:15.687',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	, (11,	N'Pdf Attachment Mail',	N'T11',	1,	'20201208 08:40:15.687',	'00000000-0000-0000-0000-000000000000',	null,	null,	0)
	) as v ([id], [TemplateName], [FileName], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted])
)
	INSERT INTO [dbo].[MailTemplate]
	([id], [TemplateName], [FileName], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted])
	SELECT [id], [TemplateName], [FileName], [isActive], [Cre_on], [Cre_By], [Modified_On], [Modified_by], [IsDeleted]
	FROM cte_data;

	SET IDENTITY_INSERT [dbo].[MailTemplate] OFF;

END

-- End data of table: [dbo].[MailTemplate] --
END
GO


CREATE PROCEDURE [dbo].[Populate_dbo_SAPAutoList]
AS
BEGIN
/*
	Table's data:    [dbo].[SAPAutoList]
	Data Source:     [PRO_IIS\SQLEXPRESS].[ProAcc2_Dev]
	Created on:      8/2/2021 3:31:39 PM
	Scripted by:     sa
	Generated by:    Data Script Writer - ver. 2.3.0.0
	GitHub repo URL: https://github.com/SQLPlayer/DataScriptWriter/
*/


IF NOT EXISTS (SELECT TOP (1) * FROM [dbo].[SAPAutoList])
BEGIN
	PRINT 'Populating data into [dbo].[SAPAutoList]';
	SET IDENTITY_INSERT [dbo].[SAPAutoList] ON;

	;WITH cte_data
	as (SELECT [Id], [Name], [isActive], [Cre_on], [Cre_by], [Modified_On], [Modified_by], [isDeleted] FROM 
	(VALUES
	  (1,	'SalesDocuments',	1,	'20210422 15:58:13.690',	'b40c3e77-2673-4dd3-a32b-ca42908dd6a0',	null,	null,	0)
	, (2,	'BillingDocuments',	1,	'20210423 11:27:18.513',	'b40c3e77-2673-4dd3-a32b-ca42908dd6a0',	null,	null,	0)
	, (3,	'OrderDocuments',	1,	'20210423 11:28:00.190',	'b40c3e77-2673-4dd3-a32b-ca42908dd6a0',	null,	null,	0)
	, (4,	'MaterialityScore',	1,	'20210423 12:52:28.333',	'b40c3e77-2673-4dd3-a32b-ca42908dd6a0',	null,	null,	0)
	, (5,	'RFCFM',	1,	'20210423 15:27:09.803',	'b40c3e77-2673-4dd3-a32b-ca42908dd6a0',	null,	null,	0)
	) as v ([Id], [Name], [isActive], [Cre_on], [Cre_by], [Modified_On], [Modified_by], [isDeleted])
)
	INSERT INTO [dbo].[SAPAutoList]
	([Id], [Name], [isActive], [Cre_on], [Cre_by], [Modified_On], [Modified_by], [isDeleted])
	SELECT [Id], [Name], [isActive], [Cre_on], [Cre_by], [Modified_On], [Modified_by], [isDeleted]
	FROM cte_data;

	SET IDENTITY_INSERT [dbo].[SAPAutoList] OFF;

END

-- End data of table: [dbo].[SAPAutoList] --
END
GO







EXEC Default_Disable_ENABLE_Triggers @disable=1  --Disable

EXEC Populate_dbo_User_Type
EXEC Populate_dbo_UserMaster
EXEC Populate_dbo_RoleMaster
EXEC Populate_dbo_IndustrySector
EXEC Populate_dbo_BuildingBlock
EXEC Populate_dbo_PhaseMaster
EXEC Populate_dbo_ApplicationAreaMaster
EXEC Populate_dbo_TaskType
EXEC Populate_dbo_ParallelType
EXEC Populate_dbo_StatusMaster
EXEC Populate_dbo_PMTaskCategory
EXEC Populate_dbo_FileMaster
EXEC Populate_dbo_RA_RiskOwner
EXEC Populate_dbo_RA_RiskClass
EXEC Populate_dbo_RA_Probability
EXEC Populate_dbo_MailTemplate
EXEC Populate_dbo_SAPAutoList

DROP PROCEDURE Populate_dbo_User_Type
DROP PROCEDURE Populate_dbo_UserMaster
DROP PROCEDURE Populate_dbo_RoleMaster
DROP PROCEDURE Populate_dbo_IndustrySector
DROP PROCEDURE Populate_dbo_BuildingBlock
DROP PROCEDURE Populate_dbo_PhaseMaster
DROP PROCEDURE Populate_dbo_ApplicationAreaMaster
DROP PROCEDURE Populate_dbo_TaskType
DROP PROCEDURE Populate_dbo_ParallelType
DROP PROCEDURE Populate_dbo_StatusMaster
DROP PROCEDURE Populate_dbo_PMTaskCategory
DROP PROCEDURE Populate_dbo_FileMaster
DROP PROCEDURE Populate_dbo_RA_RiskOwner
DROP PROCEDURE Populate_dbo_RA_RiskClass
DROP PROCEDURE Populate_dbo_RA_Probability
DROP PROCEDURE Populate_dbo_MailTemplate
DROP PROCEDURE Populate_dbo_SAPAutoList



EXEC Default_Disable_ENABLE_Triggers @disable=0  --Enable
DISABLE TRIGGER Trg_ProjectMonitor on ProjectMonitor
--DEFAULT DATA END

--EXEC Default_Disable_ENABLE_Triggers @disable=1 
--delete  ProjectMonitor
--delete HistoryLog
--delete Issuetrack
--delete  Instance
--delete Project

--delete ActivityMaster
--EXEC Default_Disable_ENABLE_Triggers @disable=0 