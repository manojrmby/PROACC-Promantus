@echo off

REM set /p DB="DB NAME :"
REM set /p User="USER NAME :"
REM set /p Pass="PASSWORD :"

set DB=%1
set User=%2
set Pass=%3
set ReleaseID=%4


for /f "delims=" %%a in ('wmic OS Get localdatetime ^| find "."') do set DateTime=%%a

set Yr=%DateTime:~0,4%
set Mon=%DateTime:~4,2%
set Day=%DateTime:~6,2%
set Hr=%DateTime:~8,2%
set Min=%DateTime:~10,2%
set Sec=%DateTime:~12,2%

set BackupName=Backup-Before-%ReleaseID%ON_%Day%_%Mon%_%Yr%__%Hr%_%Min%_%Sec%

mkdir D:\BackUp\PROACC\%BackupName%

powershell Compress-Archive D:\Hosting\%DB%  D:\BackUp\PROACC\%BackupName%\%DB%_%BackupName%.zip
sqlcmd -S PRO_IIS\SQLEXPRESS -U %User% -P %Pass% -Q "BACKUP DATABASE %DB% TO  DISK = N'D:\BackUp\PROACC\%BackupName%\%DB%_%BackupName%.bak' WITH NOFORMAT, NOINIT,  NAME = N'%DB%-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10"
