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
rem -- replace 'module-name' with the module directory name
set ProjectRoot=%MetreosWorkspaceRoot%\mod-metreos-http
set PostBuildLog=%ProjectRoot%\postbuild\postbuild.txt
set DeployDir=%MetreosBuildRoot%\System\Apache

set confDir=%DeployDir%\conf
set modulesDir=%DeployDir%\modules
set SymbolDir=%modulesDir%\Symbols

rem -- Clear post-build log
if exist %PostBuildLog% del %PostBuildLog% /F /Q
@echo ** PostBuild Beginning > %PostBuildLog%    

rem -- Create deploy directory
if not exist %DeployDir%          mkdir %DeployDir%
if not exist %confDir%            mkdir %confDir%
if not exist %modulesDir%         mkdir %modulesDir%
if not exist %SymbolDir%          mkdir %SymbolDir%

@echo ****** Copying conf ****** >> %PostBuildLog%
xcopy %ProjectRoot%\apache-1.3\conf\metreos_http.conf	%confDir% /Y >> %PostBuildLog%
@echo ****** Copying modules ****** >> %PostBuildLog%
xcopy %ProjectRoot%\lib\*.dll							%modulesDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\lib\*.so							%modulesDir% /Y >> %PostBuildLog%

xcopy %ProjectRoot%\lib\*.pdb							%SymbolDir%  /Y >> %PostBuildLog%

rem -- Install debug symbols
if /i NOT "%BuildTarget%"=="debug" if /i NOT "%BuildTarget%"=="debugmethodcalltrace" goto removePdb
@echo ** Installing debug symbols                   >> %PostBuildLog%

xcopy %M_FrameworkDir%\cpp-core_wind.dll			%modulesDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\ace-5.4\lib\ACEd.dll		%modulesDir% /Y >> %PostBuildLog%
xcopy %M_FrameworkDir%\Symbols\cpp-core_wind.pdb    %SymbolDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\ace-5.4\lib\ACEd.pdb		%SymbolDir% /Y >> %PostBuildLog%

goto done

rem -- Remove debug symbol files if not a debug build
:removePdb
@echo ** Copy release libraries >> %PostBuildLog%
xcopy %M_FrameworkDir%\cpp-core_win.dll	        %modulesDir% /Y >> %PostBuildLog%
xcopy %M_FrameworkDir%\Symbols\cpp-core_win.pdb %SymbolDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\ace-5.4\lib\ACE.dll  %modulesDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\ace-5.4\lib\ACE.pdb  %SymbolDir% /Y >> %PostBuildLog%

@echo ** Removing debug symbols >> %PostBuildLog%
goto done

:usage
echo Usage: PostBuild.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).
goto done

:done
echo ** PostBuild Complete                         >> %PostBuildLog%
type %PostBuildLog%
