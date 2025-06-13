echo off

REM goto done

setlocal
if "%CUAEWORKSPACE%"=="" set CUAEWORKSPACE=X:

if "%1" == "" goto usage

set AceDllPath=%CUAEWORKSPACE%\Contrib\ace-5.4\lib
set ResiprocatePath=%CUAEWORKSPACE%\Contrib\resiprocate\lib
if /i "%1" == "release" (
    @set AceDll=%AceDllPath%\ace) else (
    @set AceDll=%AceDllPath%\aced)
    
if /i "%1%" == "debug" (
    @set CppCoreDll=%CUAEWORKSPACE%\build\Framework\1.0\cpp-cored) else (
    @set CppCoreDll=%CUAEWORKSPACE%\build\Framework\1.0\cpp-core)

if /i "%1%" == "debug" (
    @set CppCoreSymbol=%CUAEWORKSPACE%\build\Framework\1.0\Symbols\cpp-cored) else (
    @set CppCoreSymbol=%CUAEWORKSPACE%\build\Framework\1.0\Symbols\cpp-core)    

set Root=%CUAEWORKSPACE%
set BuildDir=%Root%\Build

set DeployDir=%BuildDir%\PresenceService
set SymbolDir=%DeployDir%\Symbols
set PostBuildTxt=..\..\postbuild\postbuild.txt

REM ******* Create directories *******

if not exist %DeployDir%    mkdir %DeployDir%
if not exist %SymbolDir%    mkdir %SymbolDir%

if exist %PostBuildTxt% del %PostBuildTxt% /F /Q >nul

echo ****** Copying files ******                    >> %PostBuildTxt%
xcopy %CppCoreDll%.dll      %DeployDir%     /Y      >> %PostBuildTxt%
xcopy %CppCoreSymbol%.pdb   %SymbolDir%     /Y      >> %PostBuildTxt%

xcopy %AceDll%.dll          %DeployDir%     /Y      >> postbuild.txt
xcopy %AceDll%.pdb          %SymbolDir%     /Y      >> postbuild.txt

echo ****** Copying Presence runtime ******                                 >> %PostBuildTxt%
xcopy ..\..\projects\presence-runtime\%1\*.exe      %DeployDir%     /Y      >> %PostBuildTxt%

xcopy ..\..\projects\presence-runtime\%1\*.pdb      %SymbolDir%     /Y      >> %PostBuildTxt%

if /i "%1" == "RELEASE" goto cleanDebugSymbols

goto done

:cleanDebugSymbols
if exist %DeployDir%\cpp-cored.dll      del /s  %DeployDir%\cpp-cored.dll       >> %PostBuildTxt%
if exist %DeployDir%\aced.dll           del /s  %DeployDir%\aced.dll            >> %PostBuildTxt%
goto done

:usage
echo.
echo Usage: postbuid.bat <Configuration>
echo Where <Cofiguration> is "DEBUG" or "RELEASE"

:done
echo.                                                      >> %PostBuildTxt%
echo ****** DONE ******                                    >> %PostBuildTxt%
