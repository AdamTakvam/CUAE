@echo off
rem -- PostBuild.bat
rem -- 
rem -- Implement this script to copy build products from the 
rem -- local module directory to the product-level 'Build' directory
rem --
setlocal
set BuildTarget=%1
if "%BuildTarget%" == "" set BuildTarget=Debug

if "%CUAEWORKSPACE%"=="" set CUAEWORKSPACE=X:
call %CUAEWORKSPACE%\Tools\build-scripts\postbuild-init.cmd
call %CUAEWORKSPACE%\autobuild\CreateDeployDirectoryStructure.bat

echo Before PostBuild Init
call %CUAEWORKSPACE%\Tools\build-scripts\postbuild-init.cmd
echo Before CreateDeploy
call %CUAEWORKSPACE%\autobuild\CreateDeployDirectoryStructure.bat
echo After CreateDeploy

set ProjectRoot1=%CUAEWORKSPACE%\snmp-agent\RegUpdate\RegUpdate\bin\%BuildTarget%
set DeployDir=%MetreosBuildRoot%\StatsService\

echo Before Delete
del %ProjectRoot1%\snmp_agent_build.txt
echo Before Copy
echo %CUAEWORKSPACE%\snmp-agent\RegUpdate\RegUpdate\bin\%BuildTarget%\CUAEAgentRegUpdate.exe
xcopy %CUAEWORKSPACE%\snmp-agent\RegUpdate\RegUpdate\bin\%BuildTarget%\CUAEAgentRegUpdate.exe  %DeployDir% /Y >> %ProjectRoot1%\snmp_agent_build.txt
echo After Copy
