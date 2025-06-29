@echo off

setlocal

if "%CUAEWORKSPACE%"=="" set CUAEWORKSPACE=X:
call %CUAEWORKSPACE%\autobuild\CreateDeployDirectoryStructure.bat

set ProviderDir=%AppServerDir%\Providers
set FunctionalTestDir=%DeployDir%\FunctionalTest

echo Generating FTProvider Package
%PgenTool% -y -src:"%ProviderDir%\Metreos.Providers.FunctionalTest.dll" -dest:"%FwPackageDir%"

echo Generating FunctionalTest MCP Package
%McpTool% "%ProviderDir%\Metreos.Providers.FunctionalTest.dll" -o:"%ProviderDir%\Metreos.Providers.FunctionalTest.mcp" -r:"%FunctionalTestDir%\Metreos.samoa.FunctionalTestFramework.dll"

echo Generating TestCallControl MCP Package
%McpTool% "%ProviderDir%\Metreos.Providers.TestCallControl.dll" -o:"%ProviderDir%\Metreos.Providers.TestCallControl.mcp" -r:"%FunctionalTestDir%\Metreos.samoa.FunctionalTestFramework.dll"

:done 
