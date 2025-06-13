@echo off

setlocal

if "%1" == "" goto usage

call x:\application-suite\CreateDeployDirectoryStructure.bat

rem echo on
set cBargeDir=X:\Builds\AppSuite\cBarge
if not exist %cBargeDir% then mkdir %cBargeDir%
    
set ActionDir=..\..\..\NativeActions\bin
set TypesDir=..\..\..\NativeTypes\bin

if exist postbuild.txt del postbuild.txt /F /Q

echo.                                                                                    > postbuild.txt

echo ****** Copy Native Actions ******													>> postbuild.txt
copy %ActionDir%\%1\Metreos.Native.cBarge.dll			%cBargeDir%   		>> postbuild.txt

echo.																					>> postbuild.txt
   
REM echo ****** Copy Native Types ******													>> postbuild.txt
REM copy %TypesDir%\%1\Metreos.Types.cBarge.dll			%cBargeDir%   		>> postbuild.txt

echo.

REM -------------------------------------------------------------------------------
REM Install debug symbols if we are doing a debug build
REM -------------------------------------------------------------------------------

if /i NOT "%1"=="debug" if /i NOT "%1"=="debugmethodcalltrace" goto RemovePdb

echo.																					>> postbuild.txt

echo ****** Copy Native Action Debug Symbols ******										>> postbuild.txt
copy %ActionDir%\%1\Metreos.Native.cBarge.pdb				%cBargeDir%   		>> postbuild.txt

echo.																					>> postbuild.txt
   
REM echo ****** Copy Native Types Debug Symbols ******										>> postbuild.txt
REM copy %TypesDir%\%1\Metreos.Types.cBarge.pdb				%cBargeDir%   		>> postbuild.txt

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
