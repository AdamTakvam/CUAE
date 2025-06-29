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
set ProjectRoot=%MetreosWorkspaceRoot%\h323-service
set PostBuildLog=%ProjectRoot%\postbuild\postbuild.txt
set DeployDir=%MetreosBuildRoot%\H323Service
set SymbolDir=%DeployDir%\Symbols

set OpenH323DllPath=%MetreosContribRoot%\openh323\lib
set PtLibDllPath=%MetreosContribRoot%\pwlib\lib
set AceDllPath=%MetreosContribRoot%\ace-5.4\lib

if /i "%BuildTarget%" == "debug" ( 
    @set OpenH323=%OpenH323DllPath%\OpenH323d) else ( 
    @set OpenH323=%OpenH323DllPath%\OpenH323)

if /i "%BuildTarget%" == "debug" (
    @set PtLib=%PtLibDllPath%\PtLibd) else ( 
    @set PtLib=%PtLibDllPath%\PtLib)
    
if /i "%BuildTarget%" == "debug" (
    @set AceDll=%AceDllPath%\aced) else (
    @set AceDll=%AceDllPath%\ace)
    
if /i "%BuildTarget%" == "debug" (
    @set CppCoreDll=%FwToolsDir%\cpp-cored) else (
    @set CppCoreDll=%FwToolsDir%\cpp-core)

if /i "%BuildTarget%" == "debug" (
    @set CppCoreSymbol=%FwToolsDir%\Symbols\cpp-cored) else (
    @set CppCoreSymbol=%FwToolsDir%\Symbols\cpp-core)
        
rem -- Clear post-build log
if exist %PostBuildLog% del %PostBuildLog% /F /Q
@echo ** PostBuild Beginning > %PostBuildLog%    

rem -- Create deploy directory
if not exist %DeployDir%			mkdir %DeployDir%
if not exist %SymbolDir%			mkdir %SymbolDir%

rem -- Install files
@echo ** Installing files in deployment directory   >> %PostBuildLog%
xcopy %CppCoreDll%.dll			 %DeployDir% /Y >> %PostBuildLog%
xcopy %OpenH323%.dll			 %DeployDir% /Y >> %PostBuildLog%
xcopy %PtLib%.dll				 %DeployDir% /Y >> %PostBuildLog%
xcopy %AceDll%.dll               %DeployDir% /Y >> %PostBuildLog%

xcopy %CppCoreSymbol%.pdb		 %SymbolDir% /Y >> %PostBuildLog%
xcopy %OpenH323%.pdb			 %SymbolDir% /Y >> %PostBuildLog%
xcopy %PtLib%.pdb				 %SymbolDir% /Y >> %PostBuildLog%
xcopy %AceDll%.pdb               %SymbolDir% /Y >> %PostBuildLog%

@echo ****** Copying H323 runtime ******  >> %PostBuildLog%
xcopy %ProjectRoot%\projects\h323-runtime\%BuildTarget%\*.exe %DeployDir% /Y >> %PostBuildLog%

xcopy %ProjectRoot%\projects\h323-runtime\%BuildTarget%\*.pdb %SymbolDir% /Y >> %PostBuildLog%

rem -- Install debug symbols
if /i NOT "%BuildTarget%"=="debug" if /i NOT "%BuildTarget%"=="debugmethodcalltrace" goto removePdb
@echo ** Installing debug symbols                   >> %PostBuildLog%

goto done
rem -- Remove debug symbol files if not a debug build
:removePdb
@echo ** Removing debug symbols                                     >> %PostBuildLog%
if exist %DeployDir%\cpp-cored.* del /s %DeployDir%\cpp-cored.* >> %PostBuildLog%
if exist %DeployDir%\openh323d.* del /s %DeployDir%\openh323d.* >> %PostBuildLog%
if exist %DeployDir%\ptlibd.*    del /s %DeployDir%\ptlibd.*    >> %PostBuildLog%
if exist %DeployDir%\aced.*      del /s %DeployDir%\aced.*      >> %PostBuildLog%
goto done

:usage
echo Usage: PostBuild.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).
goto done

:done
echo ** PostBuild Complete                         >> %PostBuildLog%
type %PostBuildLog%
