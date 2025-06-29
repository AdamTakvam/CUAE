@echo off

setlocal

if "%1" == "" goto usage



rem echo on

set TypeDir=..\..\..\Types\bin
set ActionDir=..\..\..\NativeActions\bin
set LibDir=..\..\..\Libs

if exist postbuild.txt del postbuild.txt /F /Q
if not exist %LibDir%			mkdir %LibDir%

echo.                                                                                    > postbuild.txt

echo ****** Copy Types ******													>> postbuild.txt
copy %TypeDir%\%1\Metreos.Applications.cBarge.dll				                %LibDir%  		>> postbuild.txt

echo ****** Copy Native Actions ******													>> postbuild.txt
copy %ActionDir%\%1\Metreos.Applications.cBarge.NativeActions.dll				%LibDir%   		>> postbuild.txt

echo.

REM -------------------------------------------------------------------------------
REM Install debug symbols if we are doing a debug build
REM -------------------------------------------------------------------------------

if /i NOT "%1"=="debug" if /i NOT "%1"=="debugmethodcalltrace" goto RemovePdb

echo.																					>> postbuild.txt


echo ****** Copy Type Debug Symbols ******										>> postbuild.txt
copy %TypeDir%\%1\Metreos.Applications.cBarge.pdb				                %LibDir%   	>> postbuild.txt

echo ****** Copy Native Action Debug Symbols ******													>> postbuild.txt
copy %ActionDir%\%1\Metreos.Applications.cBarge.NativeActions.pdb				%LibDir%   /Y		>> postbuild.txt

goto done

:RemovePdb

if exist %LibDir%\*.pdb del /s %LibDir%\*.pdb									>> postbuild.txt  

goto done

:usage
echo Usage: PostBuild.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).

:done
echo.																					>> postbuild.txt
echo ****** DONE ******                                                                 >> postbuild.txt                            
