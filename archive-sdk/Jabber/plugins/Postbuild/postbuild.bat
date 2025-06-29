@echo off

setlocal

if "%1" == "" goto usage

set bin=..\..\..\..\bin
if not exist %bin% mkdir %bin%

set PgenToolDev="C:\Program Files\Cisco Systems\Unified Application Designer\Framework\1.0\pgen.exe"

echo Generating Packages
%PgenToolDev% -y -src:"..\..\..\JabberTypes\bin\%1\Metreos.Types.JabberProvider.dll" -dest:"%bin%"
%PgenToolDev% -y -src:"..\..\..\JabberProvider\bin\%1\Metreos.Providers.JabberProvider.dll" -dest:"%bin%" 

echo Copying Assemblies
xcopy ..\..\..\JabberProvider\bin\%1\Metreos.Providers.JabberProvider.* %bin% /Y
xcopy ..\..\..\JabberTypes\bin\%1\Metreos.Types.JabberProvider.* %bin% /Y

goto done

:usage
echo Usage: GeneratePackages.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).

:done