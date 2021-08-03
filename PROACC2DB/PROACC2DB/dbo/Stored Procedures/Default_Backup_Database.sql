create procedure dbo.[Default_Backup_Database]
	@databaseName varchar(100),
	@backupDirectory varchar(1000),
	@releaseid varchar(100)
as
declare @backupFileName varchar(100),
	@databaseDataFilename varchar(100), @databaseLogFilename varchar(100),
	@databaseDataFile varchar(100), @databaseLogFile varchar(100),
	@execSql varchar(1000)
-- If the backup directory does not end with ''\'', append one
if charindex('\', reverse(@backupDirectory)) > 1
	set @backupDirectory = @backupDirectory + '\'
 
-- Create the backup file name based on the backup directory, the database name and today''s date
set @backupFileName = @backupDirectory + @databaseName + '-backup-before-release-' + @releaseid + '-date-' + format(getdate(),'yyyy-MM-dd-HH-mm-ss') + '.bak'
 
set @execSql = 
'backup database ' + @databaseName + '
to disk = ''' + @backupFileName + '''
with
  copy_only,
  noformat,
  noinit,
  name = ''' + @databaseName + ' backup '',
  norewind,
  nounload,
  skip;select 1'
 
exec(@execSql)