@echo OFF
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
set ProjectRoot=%MetreosWorkspaceRoot%\mediaserver
set PostBuildLog=%ProjectRoot%\postbuild\postbuild.txt
set DeployDir=%MetreosBuildRoot%\MediaServer
set SymbolDir=%DeployDir%\Symbols

rem -- Clear post-build log
if exist %PostBuildLog% del %PostBuildLog% /F /Q
@echo ** PostBuild Beginning > %PostBuildLog%    

rem -- Create deploy directory
if not exist %DeployDir%         mkdir %DeployDir%
if not exist %DeployDir%\Audio   mkdir %DeployDir%\Audio
if not exist %DeployDir%\Grammar mkdir %DeployDir%\Grammar
if not exist %DeployDir%\dump    mkdir %DeployDir%\dump
if not exist %SymbolDir%         mkdir %SymbolDir%

@echo ****** Installing common Framework assemblies ****** >> %PostBuildLog%
xcopy %ProjectRoot%\bin\mmsserver.exe %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\bin\mmswin.exe    %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\bin\dcmutil.exe   %DeployDir% /Y >> %PostBuildLog%

@echo ****** Added PDB ****** >> %PostBuildLog%

if exist %DeployDir%\libttsapi.dll del /F /Q %DeployDir%\libttsapi.dll >> %PostBuildLog%
if exist %DeployDir%\*.dll         del /F /Q %DeployDir%\*.dll         >> %PostBuildLog%

xcopy %MetreosContribRoot%\neospeech\libttsapi.dll        %DeployDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\ScanSoft\SpeechWorks\bin\*.dll %DeployDir% /Y >> %PostBuildLog%
xcopy "%MetreosContribRoot%\ScanSoft\SpeechWorks\OpenSpeech Recognizer\bin\*.dll" %DeployDir% /Y >> %PostBuildLog%

rem -- Install debug symbols
rem if /i NOT "%BuildTarget%"=="debug" if /i NOT "%BuildTarget%"=="debugmethodcalltrace" goto removePdb
@echo ** Installing debug symbols                   >> %PostBuildLog%


rem ---
if /i NOT "%BuildTarget%"=="debug" goto releaseSym

xcopy %M_FrameworkDir%\cpp-cored.dll                            %DeployDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\ace-5.4\lib\aced.dll                 %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\projects\server\%BuildTarget%\mmsserver.pdb %SymbolDir% /Y >> %PostBuildLog%
xcopy %M_FrameworkDir%\Symbols\cpp-cored.pdb                    %SymbolDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\ace-5.4\lib\aced.pdb                 %SymbolDir% /Y >> %PostBuildLog%
goto done

:releaseSym
xcopy %M_FrameworkDir%\cpp-core.dll                             %DeployDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\ace-5.4\lib\ace.dll                  %DeployDir% /Y >> %PostBuildLog%                                                     
xcopy %ProjectRoot%\projects\server\%BuildTarget%\mmsserver.pdb %SymbolDir% /Y >> %PostBuildLog%
xcopy %M_FrameworkDir%\Symbols\cpp-core.pdb                     %SymbolDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\ace-5.4\lib\ace.pdb                  %SymbolDir% /Y >> %PostBuildLog%                                                     
goto done

rem -- Remove debug symbol files if not a debug build
:removePdb
rem @echo ** Removing debug symbols                                        >> %PostBuildLog%
rem if exist %DeployDir%\cpp-cored.dll del /F /Q %DeployDir%\cpp-cored.dll >> %PostBuildLog%
rem if exist %DeployDir%\aced.dll      del /F /Q %DeployDir%\aced.dll      >> %PostBuildLog%
goto done

:usage
echo Usage: PostBuild.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).
goto done

:done
echo ** PostBuild complete                         >> %PostBuildLog%
type %PostBuildLog%
