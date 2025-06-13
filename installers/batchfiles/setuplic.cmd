@echo off

REM Set up some path variables
SETLOCAL
SET CUAEPATH=C:\Program Files\Cisco Systems\Unified Application Environment
SET HMPPATH=C:\Program Files\Intel\HMP
SET LICENSEFILE=240r240v200e240c240s240i_pur.lic

REM Install TTS License Manager Service
cd "%CUAEPATH%\LicenseServer"
ttslicmanager.exe /sI
sc config "VT Server" depend= "TTSLicenseServer"

REM Copy default HMP License
copy "%CUAEPATH%\LicenseServer\%LICENSEFILE%" "%HMPPATH%\data\"

REM Remove HMP License Manager
cd "%HMPPATH%\bin"
net stop "INTEL HMP License Manager"
installs.exe -r -n "INTEL HMP License Manager"

REM Reinstall HMP License Manager pointing to the new license
installs.exe -e lmgrd.exe -c "%HMPPATH%\data\%LICENSEFILE%" -l "%HMPPATH%\log\Flex.dl" -n "INTEL HMP License Manager" -k -local
net start "INTEL HMP License Manager"

REM Use our utility to validate and register the license
cd "%CUAEPATH%\LicenseServer"
cuaehmputil -stop
cuaehmputil -restore
cuaehmputil -register

echo .