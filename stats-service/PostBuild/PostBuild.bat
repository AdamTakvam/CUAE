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
set ProjectRoot=%MetreosWorkspaceRoot%\stats-service
set PostBuildLog=%ProjectRoot%\postbuild\postbuild.txt
set DeployDir=%MetreosBuildRoot%\StatsService
set SymbolDir=%DeployDir%\Symbols

set CoreAssemblyDir=%FwCoreDir%
set IPWorksDir=%MetreosContribRoot%\IPWorks
set RRDToolDir=%MetreosContribRoot%\rrdtool

rem -- Clear post-build log
if exist %PostBuildLog% del %PostBuildLog% /F /Q
@echo ** PostBuild Beginning > %PostBuildLog%    

rem -- Create deploy directory
if not exist %DeployDir%           mkdir %DeployDir%
if not exist %DeployDir%\RRD       mkdir %DeployDir%\RRD
if not exist %SymbolDir%           mkdir %SymbolDir%

rem -- Install files
@echo ** Installing files in deployment directory   >> %PostBuildLog%
xcopy %ProjectRoot%\ServiceFacade\bin\%BuildTarget%\StatsServerService.exe.config           %DeployDir%	/Y	>> %PostBuildLog%
xcopy %ProjectRoot%\ServiceFacade\bin\%BuildTarget%\StatsServerService.exe		            %DeployDir%	/Y  >> %PostBuildLog%
xcopy %ProjectRoot%\ConsoleFacade\bin\%BuildTarget%\StatsServerConsole.exe.config           %DeployDir%	/Y	>> %PostBuildLog%
xcopy %ProjectRoot%\ConsoleFacade\bin\%BuildTarget%\StatsServerConsole.exe		            %DeployDir%	/Y  >> %PostBuildLog%
xcopy %ProjectRoot%\StatsCore\bin\%BuildTarget%\Metreos.StatsCore.dll						%DeployDir%	/Y  >> %PostBuildLog%
xcopy %ProjectRoot%\StatsCore\licenses.licx	                                                %DeployDir%	/Y	>> %PostBuildLog%
xcopy %IPWorksDir%\nsoftware.IPWorks.dll		                                            %DeployDir%	/Y  >> %PostBuildLog%
xcopy %IPWorksDir%\nsoftware.System.dll			                                            %DeployDir%	/Y	>> %PostBuildLog%
xcopy %RRDToolDir%\rrdtool.exe          		                                            %DeployDir%	/Y  >> %PostBuildLog%

xcopy %ProjectRoot%\ServiceFacade\bin\%BuildTarget%\StatsServerService.pdb		            %SymbolDir%	/Y  >> %PostBuildLog%
xcopy %ProjectRoot%\ConsoleFacade\bin\%BuildTarget%\StatsServerConsole.pdb		            %SymbolDir%	/Y  >> %PostBuildLog%
xcopy %ProjectRoot%\StatsCore\bin\%BuildTarget%\Metreos.StatsCore.pdb						%SymbolDir%	/Y  >> %PostBuildLog%

rem -- Install debug symbols
if /i NOT "%BuildTarget%"=="debug" if /i NOT "%BuildTarget%"=="debugmethodcalltrace" goto removePdb
@echo ** Installing debug symbols                                                                           >> %PostBuildLog%

@echo ** Installing IPWorks license file                                                                    >> %PostBuildLog%
xcopy %ProjectRoot%\StatsCore\licenses.licx                                                 %DeployDir%	/Y  >> %PostBuildLog%

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
