@echo off

setlocal

if "%1" == "" goto usage

rem echo on
set DoNotCallDir=X:\Build\AppSuite\DoNotCall
if not exist %DoNotCallDir% mkdir %DoNotCallDir%    
set ActionDir=..\..\..\VendorDbNativeActions\bin


if exist postbuild.txt del postbuild.txt /F /Q

echo.                                                                                    > postbuild.txt

echo ****** Copy Native Actions ******													>> postbuild.txt
copy %ActionDir%\%1\Metreos.Native.VendorDb.dll					%DoNotCallDir%   		>> postbuild.txt

echo.

REM -------------------------------------------------------------------------------
REM Install debug symbols if we are doing a debug build
REM -------------------------------------------------------------------------------

if /i NOT "%1"=="debug" if /i NOT "%1"=="debugmethodcalltrace" goto RemovePdb

echo.																					>> postbuild.txt

echo ****** Copy Native Action Debug Symbols ******										>> postbuild.txt
copy %ActionDir%\%1\Metreos.Native.VendorDb.pdb					%DoNotCallDir%   		>> postbuild.txt

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
