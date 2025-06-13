@echo off
setlocal
SET CUAEPATH=C:\Program Files\Cisco Systems\Unified Application Environment

REM Stop some services
net stop "VT Server"
net stop "TTSLicenseServer"

REM Remove the TTS License Manager Service and stop HMP
cd "%CUAEPATH%\LicenseServer"
ttslicmanager.exe /sD
cuaehmputil -stop

echo .