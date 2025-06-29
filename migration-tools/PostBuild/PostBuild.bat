@echo off
rem -- PostBuild.bat
rem -- 
rem -- Implement this script to copy build products from the 
rem -- local module directory to the product-level 'Build' directory
rem --
setlocal
set BuildTarget=%1
if "%BuildTarget%" == "" set BuildTarget=Debug

rem -- Include 'postbuild-init.cmd'
rem -- Provided vars:
rem --    MetreosWorkspaceRoot   (e.g., X:\)
rem --    MetreosToolsRoot       (e.g., X:\Tools)
rem --    MetreosContribRoot     (e.g., X:\Contrib)
rem --    CLR_gacutil            (fullqualified path of gacutil)
rem
if "%CUAEWORKSPACE%"=="" set CUAEWORKSPACE=X:
call %CUAEWORKSPACE%\Tools\build-scripts\postbuild-init.cmd
call %MetreosWorkspaceRoot%\autobuild\CreateDeployDirectoryStructure.bat
set ProjectRoot=%MetreosWorkspaceRoot%\migration-tools
set PostBuildLog=%ProjectRoot%\postbuild\postbuild.txt

REM set ProviderPrefix=Metreos.Providers
set AssemblyPrefix=Metreos.AppServer
set DeployDir=%FwToolsDir%

rem -- Clear post-build log
if exist %PostBuildLog% del %PostBuildLog% /F /Q
@echo ** PostBuild Beginning > %PostBuildLog%    

rem -- Create deploy directory
if not exist %DeployDir%           mkdir %DeployDir%

rem -- Install files
@echo ** Installing files in deployment directory   >> %PostBuildLog%

REM Clear shadow copies of dlls
%CLR_gacutil% /cdl >> %PostBuildLog%

@echo ** Installing assemblies ** >> %PostBuildLog%
xcopy %ProjectRoot%\Behaviors\bin\%BuildTarget%\backup-restore.dll                 %DeployDir% /Y >> %PostBuildLog%

@echo ** Installing executables ** >> %PostBuildLog%
xcopy %ProjectRoot%\cuae-backup\bin\%BuildTarget%\cuae-backup.exe             %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\cuae-backup\bin\%BuildTarget%\cuae-backup.exe.config      %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\cuae-restore\bin\%BuildTarget%\cuae-restore.exe           %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\cuae-restore\bin\%BuildTarget%\cuae-restore.exe.config    %DeployDir% /Y >> %PostBuildLog%

rem -- Install debug symbols
if /i NOT "%BuildTarget%"=="debug" if /i NOT "%BuildTarget%"=="debugmethodcalltrace" goto removePdb
@echo ** Installing debug symbols                   >> %PostBuildLog%

xcopy %ProjectRoot%\Behaviors\bin\%BuildTarget%\backup-restore.pdb               %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\cuae-backup\bin\%BuildTarget%\cuae-backup.pdb           %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\cuae-restore\bin\%BuildTarget%\cuae-restore.pdb         %DeployDir% /Y >> %PostBuildLog%

goto done
rem -- Remove debug symbol files if not a debug build
:removePdb
@echo ** Removing debug symbols                     >> %PostBuildLog%
if exist %DeployDir%\*.pdb del /s %DeployDir%\*.pdb	>> %PostBuildLog%
goto done

:usage
echo Usage: PostBuild.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).
goto done

:done
echo ** PostBuild Complete                         >> %PostBuildLog%
type %PostBuildLog%
