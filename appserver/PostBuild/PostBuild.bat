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
set ProjectRoot=%MetreosWorkspaceRoot%\appserver
set PostBuildLog=%ProjectRoot%\postbuild\postbuild.txt

REM set ProviderPrefix=Metreos.Providers
set AssemblyPrefix=Metreos.AppServer
set DeployDir=%AppServerDir%
set SymbolDir=%DeployDir%\Symbols

rem -- Clear post-build log
if exist %PostBuildLog% del %PostBuildLog% /F /Q
@echo ** PostBuild Beginning > %PostBuildLog%    

rem -- Create deploy directory
if not exist %DeployDir%           mkdir %DeployDir%
if not exist %DeployDir%\Libs      mkdir %DeployDir%\Libs
if not exist %DeployDir%\TmScripts mkdir %DeployDir%\TmScripts
if not exist %SymbolDir%           mkdir %SymbolDir%

rem -- Install files
@echo ** Installing files in deployment directory   >> %PostBuildLog%

REM Clear shadow copies of dlls
%CLR_gacutil% /cdl >> %PostBuildLog%

@echo ** Installing core assemblies ** >> %PostBuildLog%
xcopy %ProjectRoot%\ARE\bin\%BuildTarget%\%AssemblyPrefix%.ARE.dll                           %DeployDir%\Libs /Y >> %PostBuildLog%
xcopy %ProjectRoot%\CommonRuntime\bin\%BuildTarget%\%AssemblyPrefix%.CommonRuntime.dll       %DeployDir%\Libs /Y >> %PostBuildLog%
xcopy %ProjectRoot%\ApplicationManager\bin\%BuildTarget%\%AssemblyPrefix%.ApplicationManager.dll %DeployDir%\Libs /Y >> %PostBuildLog%
xcopy %ProjectRoot%\EventRouter\bin\%BuildTarget%\%AssemblyPrefix%.EventRouter.dll           %DeployDir%\Libs /Y >> %PostBuildLog%
xcopy %ProjectRoot%\ProviderManager\bin\%BuildTarget%\%AssemblyPrefix%.ProviderManager.dll   %DeployDir%\Libs /Y >> %PostBuildLog%
xcopy %ProjectRoot%\TelephonyManager\bin\%BuildTarget%\%AssemblyPrefix%.TelephonyManager.dll %DeployDir%\Libs /Y >> %PostBuildLog%
xcopy %ProjectRoot%\Clustering\bin\%BuildTarget%\%AssemblyPrefix%.Clustering.dll             %DeployDir%\Libs /Y >> %PostBuildLog%
xcopy %ProjectRoot%\IPC\bin\%BuildTarget%\%AssemblyPrefix%.Management.dll                    %DeployDir%\Libs /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\FLEXlm\LicenseServer\lmgr10.dll                                   %DeployDir%      /Y >> %PostBuildLog%

@echo ** Installing TelephonyManager Scripts ** >> %PostBuildLog%
xcopy %ProjectRoot%\TelephonyManager\Scripts\*.* %DeployDir%\TmScripts /Y >> %PostBuildLog%

@echo ** Installing executables ** >> %PostBuildLog%
xcopy %ProjectRoot%\ConsoleRuntime\bin\%BuildTarget%\AppServer.exe               %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\ConsoleRuntime\bin\%BuildTarget%\AppServer.exe.config        %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\ServiceRuntime\bin\%BuildTarget%\AppServerService.exe        %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\ServiceRuntime\bin\%BuildTarget%\AppServerService.exe.config %DeployDir% /Y >> %PostBuildLog%

xcopy %ProjectRoot%\ARE\bin\%BuildTarget%\%AssemblyPrefix%.ARE.pdb                               %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\CommonRuntime\bin\%BuildTarget%\%AssemblyPrefix%.CommonRuntime.pdb           %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\ApplicationManager\bin\%BuildTarget%\%AssemblyPrefix%.ApplicationManager.pdb %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\EventRouter\bin\%BuildTarget%\%AssemblyPrefix%.EventRouter.pdb               %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\ProviderManager\bin\%BuildTarget%\%AssemblyPrefix%.ProviderManager.pdb       %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\TelephonyManager\bin\%BuildTarget%\%AssemblyPrefix%.TelephonyManager.pdb     %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\Clustering\bin\%BuildTarget%\%AssemblyPrefix%.Clustering.pdb                 %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\IPC\bin\%BuildTarget%\%AssemblyPrefix%.Management.pdb                        %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\ConsoleRuntime\bin\%BuildTarget%\AppServer.pdb                               %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\ServiceRuntime\bin\%BuildTarget%\AppServerService.pdb                        %SymbolDir% /Y >> %PostBuildLog%

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
