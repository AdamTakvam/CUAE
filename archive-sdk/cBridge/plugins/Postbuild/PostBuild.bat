@echo off

setlocal

if "%1" == "" goto usage

call x:\application-suite\CreateDeployDirectoryStructure.bat

rem echo on

set ActionDir=..\..\..\NativeActions\bin
set TypesDir=..\..\..\NativeTypes\bin

if exist postbuild.txt del postbuild.txt /F /Q

echo.                                                                                    > postbuild.txt

echo ****** Copy Native Actions ******													>> postbuild.txt
xcopy %ActionDir%\%1\Metreos.Native.cBridge.dll			%cBridgeDir%   /Y		>> postbuild.txt

echo.																					>> postbuild.txt
   
REM echo ****** Copy Native Types ******													>> postbuild.txt
REM xcopy %TypesDir%\%1\Metreos.Types.cBridge.dll			%cBridgeDir%   /Y		>> postbuild.txt

echo.

REM -------------------------------------------------------------------------------
REM Install debug symbols if we are doing a debug build
REM -------------------------------------------------------------------------------

if /i NOT "%1"=="debug" if /i NOT "%1"=="debugmethodcalltrace" goto RemovePdb

echo.																					>> postbuild.txt

echo ****** Copy Native Action Debug Symbols ******										>> postbuild.txt
xcopy %ActionDir%\%1\Metreos.Native.cBridge.pdb				%cBridgeDir%   /Y		>> postbuild.txt

echo.																					>> postbuild.txt
   
REM echo ****** Copy Native Types Debug Symbols ******										>> postbuild.txt
REM xcopy %TypesDir%\%1\Metreos.Types.cBridge.pdb				%cBridgeDir%   /Y		>> postbuild.txt

goto done

:RemovePdb

if exist %AppSuiteDir%\*.pdb del /s %AppSuiteDir%\*.pdb									>> postbuild.txt  

goto done

:usage
echo Usage: PostBuild.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).

:done
echo.																					>> postbuild.txt
echo ****** DONE ******                                                                 >> postbuild.txt                            
