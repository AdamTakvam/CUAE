@echo off
rem -- PostBuild.bat
rem -- 
rem -- Implement this script to copy build products from the 
rem -- local module directory to the product-level 'Build' directory
rem --
setlocal
set BuildTarget=%1
if "%BuildTarget%" == "" set BuildTarget="Debug"

rem -- Include 'postbuild-init.cmd'
rem -- Provided vars:
rem --    MetreosWorkspaceRoot   (e.g., X:\)
rem --    MetreosToolsRoot       (e.g., X:\Tools)
rem --    MetreosContribRoot     (e.g., X:\Contrib)
rem --    CLR_gacutil            (fullqualified path of gacutil)
rem
call X:\Tools\build-scripts\postbuild-init.cmd
call %MetreosWorkspaceRoot%\autobuild\CreateDeployDirectoryStructure.bat
set PostBuildLog=%ProjectRoot%\postbuild\postbuild.txt
rem -- replace 'module-name' with the module directory name
set ProjectRoot=%MetreosWorkspaceRoot%\module-name
set DeployDir=%MetreosWorkspaceRoot%\Build\ModuleName

rem -- Clear post-build log
if exist %PostBuildLog% del %PostBuildLog% /F /Q

rem -- Create deploy directory
if not exist %DeployDir% mkdir %DeployDir%

rem -- Install files
@echo ** Installing files in deployment directory   >> %PostBuildLog%

rem -- Use XCOPY to copy dlls/exes


rem -- Install debug symbols
if /i NOT "%BuildTarget%"=="debug" if /i NOT "%BuildTarget%"=="debugmethodcalltrace" goto removePdb
@echo ** Installing debug symbols                   >> %PostBuildLog%

rem --- Use XCOPY to copy debug symbols


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
