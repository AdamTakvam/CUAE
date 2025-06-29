@echo off

setlocal

if "%1" == "" goto usage

echo Generating Native Action Packages
%PgenTool% -y -src:"..\..\..\NativeActions\bin\%1\Metreos.Native.AmazonWebServices.dll" -dest:"%PluginsDeployDir%"

goto done

:usage
echo Usage: GeneratePackages.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).

:done