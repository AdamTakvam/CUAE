@echo off

setlocal


if "%1" == "" goto usage
if "%1" == "Debug" goto skip
if "%1" == "Release" goto skip
if "%1" == "DebugForcePostBuild" set buildType=Debug
if "%1" == "ReleaseForcePostBuild" set buildType=Release

rem echo on
set CallMonitorDir=X:\Build\AppSuite\CallMonitor
if not exist %CallMonitorDir% mkdir %CallMonitorDir%    
set ActionDir=..\..\..\NativeActions\bin


if exist postbuild.txt del postbuild.txt /F /Q

echo.                                                                                    > postbuild.txt

echo ****** Copy Native Actions ******													>> postbuild.txt
copy %ActionDir%\%buildType%\Metreos.Native.CallMonitor.dll		%CallMonitorDir%   		>> postbuild.txt
echo.

REM -------------------------------------------------------------------------------
REM Install debug symbols if we are doing a debug build
REM -------------------------------------------------------------------------------

if /i NOT "%buildType%"=="debug" if /i NOT "%buildType%"=="debugmethodcalltrace"  if /i NOT "%buildType%"=="DebugForcePostBuild" goto RemovePdb

echo.																					>> postbuild.txt

echo ****** Copy Native Action Debug Symbols ******										>> postbuild.txt
copy %ActionDir%\%buildType%\Metreos.Native.CallMonitor.pdb				%CallMonitorDir%   		>> postbuild.txt

goto done

:RemovePdb

if exist %CallMonitorDir%\*.pdb del /s %CallMonitorDir%\*.pdb							>> postbuild.txt  

goto done

:usage
echo Usage: PostBuild.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).
goto done

:skip
echo Automatic Build - Postbuild Skipped

:done
echo.																					>> postbuild.txt
echo ****** DONE ******                                                                 >> postbuild.txt 
