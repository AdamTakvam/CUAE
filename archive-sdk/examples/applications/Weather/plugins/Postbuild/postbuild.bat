@echo off

setlocal

if "%1" == "" goto usage

call X:\Samoa\SetBuildVariables.bat

set AppDeployDir=..\..\..\..\bin
set McaDeployDir=%AppDeployDir%\mca
set PluginsDeployDir=%AppDeployDir%\plugins

if not exist %AppDeployDir%                    mkdir %AppDeployDir%
if not exist %McaDeployDir%                    mkdir %McaDeployDir%
if not exist %PluginsDeployDir%                mkdir %PluginsDeployDir%

echo ****** Copying native type and action dlls for max build use core assemblies ****** 
copy   ..\..\..\NativeActions\bin\%1\Metreos.Native.Weather.dll 	%PluginsDeployDir%           > postbuild.txt
copy   ..\..\..\NativeActions\bin\%1\Metreos.Native.Weather.pdb     %PluginsDeployDir%    /Y     >> postbuild.txt

echo ************ Copying dependencies for Type/Action dll's to Build Dir and to MaxProject/Native Dir ********
copy   ..\..\..\Common\bin\%1\Metreos.Weather.Common.dll            %PluginsDeployDir%    /Y  >> postbuild.txt
copy   ..\..\..\Common\bin\%1\Metreos.Weather.Common.pdb            %PluginsDeployDir%    /Y  >> postbuild.txt   

call ..\..\GeneratePackages.bat %1