@echo off

setlocal

call ..\CreateDeployDirectoryStructure.bat

if "%DesignerPath%"=="" set DesignerPath="X:\Build\MaxDesigner\MaxDesigner.exe"

echo Build Started...
echo Building Plugins...

call "%VS71comntools%\vsvars32.bat" >nul

devenv.exe plugins\cBridge.sln /build DEBUG /out plugins\cBridge.sln.txt

echo Finished Building Plugins
echo Building Max Project File...

call %DesignerPath% mca/cBridge.max /b 

xcopy mca\bin\cBridge.mca %cBridgeDir%  /Y 

echo Finished Building Max Project File
echo Finished Build