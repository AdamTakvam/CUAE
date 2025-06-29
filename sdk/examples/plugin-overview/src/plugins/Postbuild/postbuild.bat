@echo off

setlocal

if "%1" == "" goto usage

set bin=..\..\..\..\bin
if not exist %bin% mkdir %bin%

set PgenToolDev="C:\Program Files\Cisco Systems\Unified Application Designer\Framework\1.0\pgen.exe"

echo Generating Packages
%PgenToolDev% -y -src:"..\..\..\NativeActions\bin\%1\Metreos.Native.DatabaseScraper.dll" -ref:"..\..\..\NativeTypes\bin\%1\Metreos.DatabaseScraper.Common.dll" -dest:"%bin%"
%PgenToolDev% -y -src:"..\..\..\NativeTypes\bin\%1\Metreos.Types.DatabaseScraper.dll"  -ref:"..\..\..\NativeTypes\bin\%1\Metreos.DatabaseScraper.Common.dll" -dest:"%bin%"
%PgenToolDev% -y -src:"..\..\..\Provider\bin\%1\Metreos.Providers.DatabaseScraper.dll" -ref:"..\..\..\NativeTypes\bin\%1\Metreos.DatabaseScraper.Common.dll" -dest:"%bin%" 

echo Copying Assemblies
xcopy ..\..\..\NativeActions\bin\%1\Metreos.Native.DatabaseScraper.* %bin% /Y
xcopy ..\..\..\Provider\bin\%1\Metreos.Providers.DatabaseScraper.* %bin% /Y
xcopy ..\..\..\NativeTypes\bin\%1\Metreos.DatabaseScraper.Common.* %bin% /Y
xcopy ..\..\..\NativeTypes\bin\%1\Metreos.Types.DatabaseScraper.* %bin% /Y

goto done

:usage
echo Usage: GeneratePackages.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).

:done