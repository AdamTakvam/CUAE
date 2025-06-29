@echo off

setlocal

if "%1" == "" goto usage

set AssemblyPrefix=Metreos.ApplicationSuite
set ActionsDir=x:\build\AppSuite\Actions
set StorageDir=x:\build\AppSuite\Storage
REM set TypesDir=x:\build\AppSuite\Types

if exist postbuild.txt del postbuild.txt /F /Q

if not exist %ActionsDir% mkdir %ActionsDir%
if not exist %StorageDir% mkdir %StorageDir%
REM if not exist %TypesDir% mkdir %TypesDir%

@echo.                                                                                                                  >> postbuild.txt

@echo ****** Copy Native Actions ******                                                                                 >> postbuild.txt
xcopy ..\Actions\bin\%1\%AssemblyPrefix%.Actions.dll                                          %ActionsDir%   /Y  >> postbuild.txt

@echo.                                                                                                                  >> postbuild.txt
   
@echo ****** Copy Database Storage Framework ******                                                                                   >> postbuild.txt
xcopy ..\Storage\bin\%1\%AssemblyPrefix%.Storage.dll                                          %StorageDir%     /Y  >> postbuild.txt

REM @echo ****** Copy Native Types ******                                                                                   >> postbuild.txt
REM xcopy ..\Types\bin\%1\%AssemblyPrefix%.Types.dll                                          %TypesDir%     /Y  >> postbuild.txt

REM -------------------------------------------------------------------------------
REM Install debug symbols if we are doing a debug build
REM -------------------------------------------------------------------------------

if /i NOT "%1"=="debug" if /i NOT "%1"=="debugmethodcalltrace" goto RemovePdb

@echo.                                                                                                                  >> postbuild.txt

@echo ****** Copy Native Action Symbols ******                                                                          >> postbuild.txt
xcopy ..\Actions\bin\%1\%AssemblyPrefix%.Actions.pdb                                          %ActionsDir%   /Y  >> postbuild.txt

@echo.                                                                                                                  >> postbuild.txt

@echo ****** Copy Database Storage Framework ******                                                                                   >> postbuild.txt
xcopy ..\Storage\bin\%1\%AssemblyPrefix%.Storage.pdb                                          %StorageDir%     /Y  >> postbuild.txt

@echo.
   
REM @echo ****** Copy Native Types ******                                                                                   >> postbuild.txt
REM xcopy ..\Actions\bin\%1\%AssemblyPrefix%.Types.pdb                                        %TypesDir%    /Y  >> postbuild.txt

@goto done

:RemovePdb

if exist %AppSuiteDir%\*.pdb del /s %AppSuiteDir%\*.pdb                                                                 >> postbuild.txt  

@goto done

:usage
echo Usage: PostBuild.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).

:done
@echo.                                                                                                                  >> postbuild.txt
@echo ****** DONE ******                                                                                                >> postbuild.txt
