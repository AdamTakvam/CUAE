@echo off

REM MySQL database initialization script.
REM Last updated: 09/24/2004 (marascio)

set USERNAME=root
set PASSWORD=metreos

if not "%1"=="" set USERNAME=%1
if not "%2"=="" set PASSWORD=%2

echo Searching for MySQL installation directory.
echo.

set MYSQL="C:\Program Files\MySQL\MySQL Server 4.1\bin\mysql.exe"
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
%MYSQL% -u %USERNAME% --password=%PASSWORD% < add_config.sql
goto done

:ErrorNoMySQL
echo MySQL could not be located.  Database not initialized.
goto done

:done
echo Done.