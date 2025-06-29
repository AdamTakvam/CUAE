@echo off

setlocal

if "%1" == "" goto usage

rem echo on

set ActionDir=..\..\..\NativeActions\bin
set MeetMeConfDir=X:\Build\AppSuite\ScheduledConference
if not exist %MeetMeConfDir% then mkdir %MeetMeConfDir%
    

if exist postbuild.txt del postbuild.txt /F /Q

echo.                                                                                    > postbuild.txt

echo ****** Copy Native Actions ******													>> postbuild.txt
copy %ActionDir%\%1\Metreos.Native.ScheduledConference.dll				%MeetMeConfDir%   		>> postbuild.txt

echo.

REM -------------------------------------------------------------------------------
REM Install debug symbols if we are doing a debug build
REM -------------------------------------------------------------------------------

if /i NOT "%1"=="debug" if /i NOT "%1"=="debugmethodcalltrace" goto RemovePdb

echo.																					>> postbuild.txt

echo ****** Copy Native Action Debug Symbols ******										>> postbuild.txt
copy %ActionDir%\%1\Metreos.Native.ScheduledConference.pdb				%MeetMeConfDir%   		>> postbuild.txt

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
