@echo off

setlocal

call x:\autobuild\CreateDeployDirectoryStructure.bat

set ProviderDir=%AppServerDir%\Providers

echo Generating FTProvider Package
%PgenTool% -y -src:"%ProviderDir%\Metreos.Providers.FunctionalTest.dll" -dest:"%FwPackageDir%"

:done 
