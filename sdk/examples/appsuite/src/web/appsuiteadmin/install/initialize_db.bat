@echo off

REM MySQL database initialization file.  Set the variables below.

set USERNAME=root
set PASSWORD=metreos
set MYSQL="C:\Program Files\MySQL\MySQL Server 4.1\bin\mysql.exe"

if not "%1"=="" set USERNAME=%1
if not "%2"=="" set PASSWORD=%2

echo Searching for MySQL installation directory...
echo.

if exist %MYSQL% goto FoundMySQL

set MYSQL="C:\MySQL\bin\mysql.exe"
if exist %MYSQL% goto FoundMySQL

goto :ErrorNoMySQL

:FoundMySQL
echo MySQL located. Initializing database.
echo.
echo Using the following credentials:
echo   Username: %USERNAME%
echo   Password: %PASSWORD%
cd %~dp0
%MYSQL% -u %USERNAME% --password=%PASSWORD% < CreateDatabase.sql
goto done

:ErrorNoMySQL
echo MySQL could not be located.  Database not initialized.
goto done

:done
echo Done.