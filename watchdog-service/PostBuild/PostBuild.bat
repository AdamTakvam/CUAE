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
set ProjectRoot=%MetreosWorkspaceRoot%\watchdog-service
set PostBuildLog=%ProjectRoot%\postbuild\postbuild.txt
set DeployDir=%MetreosBuildRoot%\WatchdogService
set SymbolDir=%DeployDir%\Symbols

rem -- Clear post-build log
if exist %PostBuildLog% del %PostBuildLog% /F /Q
@echo ** PostBuild Beginning > %PostBuildLog%    

rem -- Create deploy directory
if not exist %DeployDir%           mkdir %DeployDir%
if not exist %SymbolDir%           mkdir %SymbolDir%

@echo ****** Installing executables ****** >> %PostBuildLog%
xcopy %ProjectRoot%\WatchdogService\bin\%BuildTarget%\WatchdogService.exe        %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\WatchdogService\bin\%BuildTarget%\WatchdogService.exe.config %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\Watchdog\bin\%BuildTarget%\Metreos.Watchdog.dll              %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\WatchdogService\bin\%BuildTarget%\WatchdogService.pdb        %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\Watchdog\bin\%BuildTarget%\Metreos.Watchdog.pdb              %SymbolDir% /Y >> %PostBuildLog%

rem -- Install debug symbols
if /i NOT "%BuildTarget%"=="debug" if /i NOT "%BuildTarget%"=="debugmethodcalltrace" goto removePdb
@echo ** Installing debug symbols                   >> %PostBuildLog%

goto done
rem -- Remove debug symbol files if not a debug build
:removePdb
@echo ** Removing debug symbols                     >> %PostBuildLog%
goto done

:usage
echo Usage: PostBuild.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).
goto done

:done
echo ** PostBuild Complete                         >> %PostBuildLog%
type %PostBuildLog%
