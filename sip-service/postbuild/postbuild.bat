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
set ProjectRoot=%MetreosWorkspaceRoot%\sip-service
set PostBuildLog=%ProjectRoot%\postbuild\postbuild.txt

set DeployDir=%MetreosWorkspaceRoot%\Build\SipService
set SymbolDir=%DeployDir%\Symbols
set AceDllPath=%MetreosContribRoot%\ace-5.4\lib
set ResiprocatePath=%MetreosContribRoot%\resiprocate\lib
if /i "%BuildTarget%" == "release" (
    @set AceDll=%AceDllPath%\ace) else (
    @set AceDll=%AceDllPath%\aced)
    
if /i "%BuildTarget%" == "debug" (
    @set CppCoreDll=%CUAEWORKSPACE%\build\Framework\1.0\cpp-cored) else (
    @set CppCoreDll=%CUAEWORKSPACE%\build\Framework\1.0\cpp-core)
    
if /i "%BuildTarget%" == "debug" (
    @set CppCoreSymbol=%CUAEWORKSPACE%\build\Framework\1.0\Symbols\cpp-cored) else (
    @set CppCoreSymbol=%CUAEWORKSPACE%\build\Framework\1.0\Symbols\cpp-core)    
    
rem -- Clear post-build log
if exist %PostBuildLog% del %PostBuildLog% /F /Q

rem -- Create deploy directory
if not exist %DeployDir%    mkdir %DeployDir%
if not exist %SymbolDir%    mkdir %SymbolDir%

@echo ** Installing files in deployment directory   >> %PostBuildLog%

xcopy %CppCoreDll%.dll      %DeployDir%     /Y      >> %PostBuildLog%
xcopy %CppCoreSymbol%.pdb   %SymbolDir%     /Y      >> %PostBuildLog%
xcopy %AceDll%.dll          %DeployDir%		/Y		>> %PostBuildLog%
xcopy %AceDll%.pdb          %SymbolDir%		/Y		>> %PostBuildLog%

@echo ** Copying Sip runtime                        >> %PostBuildLog%
xcopy %ProjectRoot%\projects\sip-runtime\%BuildTarget%\*.exe %DeployDir% /Y >> %PostBuildLog%

xcopy %ProjectRoot%\projects\sip-runtime\%BuildTarget%\*.pdb %SymbolDir% /Y >> %PostBuildLog%

rem -- Install debug symbols
if /i NOT "%BuildTarget%"=="debug" if /i NOT "%BuildTarget%"=="debugmethodcalltrace" goto removePdb
@echo ** Installing debug symbols                   >> %PostBuildLog%

xcopy %ProjectRoot%\projects\sip-runtime\%BuildTarget%\*.pdb %DeployDir% /Y >> %PostBuildLog%

goto done
rem -- Remove debug symbol files if not a debug build
:removePdb
@echo ** Removing debug symbols                                     >> %PostBuildLog%
if exist %DeployDir%\aced.dll      del /s %DeployDir%\aced.dll      >> %PostBuildLog%
if exist %DeployDir%\cpp-cored.dll del /s %DeployDir%\cpp-cored.dll >> %PostBuildLog%

goto done

:usage
echo Usage: PostBuild.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).
goto done

:done
echo ** PostBuild Complete                         >> %PostBuildLog%
type %PostBuildLog%
