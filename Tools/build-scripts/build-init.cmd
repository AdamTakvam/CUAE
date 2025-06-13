@echo off
rem -------------------------------------------------
rem Initialize environment for ant-based builds
rem
rem Created 5/20/05 (jdixson@metreos.com)
rem
rem -------------------------------------------------

rem -- Initialize Metreos Control envvars
if "%CUAEWORKSPACE%"==""        set CUAEWORKSPACE=X:
if "%MetreosWorkspaceRoot%"=="" set MetreosWorkspaceRoot=%CUAEWORKSPACE%
if "%MetreosContribRoot%"==""   set MetreosContribRoot=%MetreosWorkspaceRoot%\Contrib
if "%MetreosToolsRoot%"==""     set MetreosToolsRoot=%MetreosWorkspaceRoot%\Tools
if "%MetresoBuildRoot%"==""     set MetreosBuildRoot=%MetreosWorkspaceRoot%\Build
if "%MetreosReleaseType%"==""   set MetreosReleaseType=DEV
if "%MetreosBuildNumber%"==""   set MetreosBuildNumber=0000
if "%MetreosBuildPlatform%"=="" set MetreosBuildPlatform=win32
if "%MetreosMaxDesigner%"==""   set MetreosMaxDesigner=%MetreosBuildRoot%\MaxDesigner\MaxDesigner.exe

if "%CiscoWorkspaceRoot%"=="" set CiscoWorkspaceRoot=%CUAEWORKSPACE%
if "%CiscoContribRoot%"==""   set CiscoContribRoot=%CiscoWorkspaceRoot%\Contrib
if "%CiscoToolsRoot%"==""     set CiscoToolsRoot=%CiscoWorkspaceRoot%\Tools
if "%CiscoBuildRoot%"==""     set CiscoBuildRoot=%CiscoWorkspaceRoot%\Build
if "%CiscoReleaseType%"==""   set CiscoReleaseType=DEV
if "%CiscoBuildNumber%"==""   set CiscoBuildNumber=0000
if "%CiscoBuildPlatform%"=="" set CiscoBuildPlatform=win32
if "%CiscoMaxDesigner%"==""   set CiscoMaxDesigner=%CiscoBuildRoot%\MaxDesigner\MaxDesigner.exe

rem -- Configure ANT
set ANT_HOME=%CiscoToolsRoot%\apache-ant-1.6.5
set ANT=%ANT_HOME%\bin\ant
