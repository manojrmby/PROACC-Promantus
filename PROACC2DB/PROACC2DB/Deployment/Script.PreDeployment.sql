/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

DECLARE @RID varchar(50)=(select replace(convert(varchar, getdate(),101),'/','') + replace(convert(varchar, getdate(),108),':',''))
EXEC Default_Backup_Database @databaseName='PROACC2DEV', @backupDirectory='D:\BackUp\DBBACKUP', @releaseid=@RID