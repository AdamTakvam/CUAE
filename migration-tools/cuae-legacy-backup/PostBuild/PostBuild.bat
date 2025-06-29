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
set ProjectRoot=%MetreosWorkspaceRoot%\migration-tools\cuae-legacy-backup
set ContribRoot=%MetreosWorkspaceRoot%\Contrib
set PostBuildLog=%ProjectRoot%\postbuild\postbuild.txt

set AssemblyPrefix=Metreos.AppServer
set DeployDir=%MetreosBuildRoot%\LegacyBackupUtil

rem -- Clear post-build log
if exist %PostBuildLog% del %PostBuildLog% /F /Q
@echo ** PostBuild Beginning > %PostBuildLog%    

rem -- Create deploy directory
if not exist %DeployDir%           mkdir %DeployDir%

@echo ****** Installing executables ******																		>> postbuild.txt
xcopy %ProjectRoot%\bin\%BuildTarget%\cuae-legacy-backup.exe		%DeployDir%			/Y						>> postbuild.txt
xcopy %ProjectRoot%\bin\%BuildTarget%\BehaviorCore.dll		        %DeployDir%			/Y						>> postbuild.txt
@echo.                                                                                                          >> postbuild.txt
xcopy %ContribRoot%\SharpZipLib\net11\ICSharpCode.SharpZipLib.dll	%DeployDir%			/Y      				>> postbuild.txt
xcopy %ContribRoot%\MySQL\Mysql.Data.dll                            %DeployDir%			/Y      				>> postbuild.txt


if /i NOT "%1"=="debug" if /i NOT "%1"=="debugmethodcalltrace" goto RemovePdb

REM -------------------------------------------------------------------------------
REM Install debug symbols if we are doing a debug build
REM -------------------------------------------------------------------------------

echo ****** Installing debug symbols ******                                                                     >> postbuild.txt
xcopy %ProjectRoot%\bin\%BuildTarget%\BehaviorCore.pdb				%DeployDir%			/Y      				>> postbuild.txt
xcopy %ProjectRoot%\bin\%BuildTarget%\cuae-legacy-backup.pdb		%DeployDir%			/Y      				>> postbuild.txt
goto done

:RemovePdb
if exist %DeployDir%\*.pdb			del /s %DeployDir%\*.pdb													>> postbuild.txt
goto done

:usage
echo Usage: PostBuild.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).
goto done

:done
