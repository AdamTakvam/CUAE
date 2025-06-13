@echo off

rem - call "%VS80COMNTOOLS%\vsvars32.bat" >nul

if "%CUAEWORKSPACE%"=="" set CUAEWORKSPACE=X:
set Root=%CUAEWORKSPACE%
set BuildDir=%Root%\Build

set DeployDir=%BuildDir%

set AppServerDir=%DeployDir%\AppServer
set MediaServerDir=%DeployDir%\MediaServer
set SftpDir=%DeployDir%\SftpServer
set UnitTestDir=%DeployDir%\UnitTests

REM REFACTOR: Do something smarter with the framework version

set M_FrameworkRootDir=%DeployDir%\Framework

set M_FrameworkVersion=1.0
set M_FrameworkDir=%M_FrameworkRootDir%\%M_FrameworkVersion%
set FwCoreDir=%M_FrameworkDir%\CoreAssemblies
set FwPackageDir=%M_FrameworkDir%\Packages
set FwToolsDir=%M_FrameworkDir%
set FwActionDir=%M_FrameworkDir%\NativeActions
set FwTypeDir=%M_FrameworkDir%\NativeTypes

set McaTool=%FwToolsDir%\mca.exe
set PgenTool=%FwToolsDir%\pgen.exe
set McpTool=%FWToolsDir%\mcp.exe
