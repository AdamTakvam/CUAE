@echo off

setlocal

if "%1" == "" goto usage

set AppDeployDir=..\..\..\..\bin
set McaDeployDir=%AppDeployDir%\mca
set PluginsDeployDir=%AppDeployDir%\plugins

if not exist %AppDeployDir%                    mkdir %AppDeployDir%
if not exist %McaDeployDir%                    mkdir %McaDeployDir%
if not exist %PluginsDeployDir%                mkdir %PluginsDeployDir%

echo ****** Copying native type and action dlls for max build use core assemblies ****** 
copy   ..\..\..\NativeActions\bin\%1\Metreos.Native.LdapDirectory.dll	    %PluginsDeployDir%           > postbuild.txt
copy   ..\..\..\NativeActions\bin\%1\Metreos.Native.LdapDirectory.pdb       %PluginsDeployDir%    /Y     >> postbuild.txt

copy   ..\..\..\NativeActions\bin\%1\Metreos.LdapDirectory.Common.dll	    %PluginsDeployDir%           >> postbuild.txt
copy   ..\..\..\NativeActions\bin\%1\Metreos.LdapDirectory.Common.pdb       %PluginsDeployDir%    /Y     >> postbuild.txt

copy   ..\..\..\NativeActions\bin\%1\Novell.Directory.Ldap.dll              %PluginsDeployDir%    /Y     >> postbuild.txt

copy   ..\..\..\NativeTypes\bin\%1\Metreos.Types.LdapDirectory.dll     		%PluginsDeployDir%    /Y     >> postbuild.txt
copy   ..\..\..\NativeTypes\bin\%1\Metreos.Types.LdapDirectory.pdb     		%PluginsDeployDir%    /Y     >> postbuild.txt

call ..\..\GeneratePackages.bat %1

goto done

:usage
echo Usage: PostBuild.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).

:done                                                                                           
