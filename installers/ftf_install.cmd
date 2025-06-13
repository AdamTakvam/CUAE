@echo off
setlocal

set METREOSHOME="C:\Program Files\Metreos"
set DOTNET=%SystemRoot%\Microsoft.NET\Framework\v1.1.4322
set METREOSFRAMEWORK=%METREOSHOME%\Framework\1.0

REM ** Copying Provider files
copy Metreos.Providers.FunctionalTest.dll %METREOSHOME%\AppServer\Providers
copy Metreos.Providers.FunctionalTest.pdb %METREOSHOME%\AppServer\Providers
copy Metreos.Providers.TestCallControl.dll %METREOSHOME%\AppServer\Providers
copy Metreos.Providers.TestCallControl.pdb %METREOSHOME%\AppServer\Providers

REM ** Copying CoreAssembly files
copy Metreos.Samoa.FunctionalTestFramework.dll %METREOSFRAMEWORK%\CoreAssemblies
copy Metreos.Samoa.FunctionalTestFramework.pdb %METREOSFRAMEWORK%\CoreAssemblies
%DOTNET%\gacutil /i %METREOSFRAMEWORK%\CoreAssemblies\Metreos.Samoa.FunctionalTestFramework.dll
