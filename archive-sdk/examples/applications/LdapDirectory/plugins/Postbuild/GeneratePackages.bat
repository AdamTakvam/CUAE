@echo off

setlocal

call x:\samoa\SetBuildVariables.bat

if "%1" == "" goto usage
if "%PgenTool%" == "" goto usage

echo Generating Native Action Packages
%PgenTool% -y -src:"..\..\..\NativeActions\bin\%1\Metreos.Native.LdapDirectory.dll" -dest:"%PluginsDeployDir%"

echo Generating Native Type Packages
%PgenTool% -y -src:"..\..\..\NativeTypes\bin\%1\Metreos.Types.LdapDirectory.dll" -dest:"%PluginsDeployDir%"

goto done

:usage
echo Usage: GeneratePackages.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).
echo.
echo Note that the 'PgenTool' environment variable must point to the Metreos pgen.exe tool.

:done