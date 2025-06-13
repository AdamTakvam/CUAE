@echo off

setlocal

SET APPSUITEADMIN_DIR="C:\Program Files\Metreos\appsuiteadmin"

net stop Apache
cd %~dp0
mkdir %APPSUITEADMIN_DIR%
xcopy /E /Y * %APPSUITEADMIN_DIR%
copy /Y %APPSUITEADMIN_DIR%"\install\apache\conf\appsuite.conf" "C:\Program Files\Metreos\System\apache\conf\appsuite.conf"
call %APPSUITEADMIN_DIR%"\install\initialize_db.bat"
net start Apache

exit 0