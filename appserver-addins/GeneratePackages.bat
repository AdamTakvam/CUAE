@echo off

setlocal
if "%1"=="Clean" goto end
if "%CUAEWORKSPACE%"=="" set CUAEWORKSPACE=X:
call %CUAEWORKSPACE%\autobuild\CreateDeployDirectoryStructure.bat

if exist %FwPackageDir%\*.xml del /Q %FwPackageDir%\*.xml >nul

echo Generating Provider Action/Event Packages
for /R %AppServerDir%\Providers %%i in (*.dll) do %PgenTool% -y -src:"%%i" -dest:"%FwPackageDir%" -search:"%AppServerDir%"

echo Generating Native Action Packages
for %%i in (%FwActionDir%\*.dll) do %PgenTool% -y -src:"%%i" -dest:"%FwPackageDir%"

echo Generating Native Type Packages
for %%i in (%FwTypeDir%\*.dll) do %PgenTool% -y -src:"%%i" -dest:"%FwPackageDir%"

echo Generating AppControl
x:\Tools\pgen-special-appcontrol.exe %FwPackageDir%\Metreos.ApplicationControl.xml

echo Generating CallControl
x:\Tools\pgen-special-callcontrol.exe %FwPackageDir%\Metreos.CallControl.xml

:end
echo Done.
