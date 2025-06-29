@echo off

setlocal

if "%1" == "" goto usage

set DeployDir=X:\Build\AppSuite\RecordAgent
if NOT exist %DeployDir% mkdir %DeployDir%
if NOT exist %DeployDir%\en-US mkdir %DeployDir%\en-US

if exist postbuild.txt del postbuild.txt /F /Q

@echo ****** Installing executables and config ******																										>> postbuild.txt
xcopy ..\bin\%1\*.exe					%DeployDir%			/Y	>> postbuild.txt
xcopy ..\bin\%1\*.dll					%DeployDir%			/Y	>> postbuild.txt
xcopy ..\bin\%1\*.config				%DeployDir%			/Y	>> postbuild.txt
xcopy ..\bin\%1\en-US\*.dll				%DeployDir%\en-US   		/Y	>> postbuild.txt
xcopy ..\utilities\homer\bin\%1\*.exe	%DeployDir%					/Y	>> postbuild.txt
@echo.                                                                                          >> postbuild.txt

if /i NOT "%1"=="debug" if /i NOT "%1"=="debugmethodcalltrace" goto RemovePdb

REM -------------------------------------------------------------------------------
REM Install debug symbols if we are doing a debug build
REM -------------------------------------------------------------------------------

echo ****** Installing debug symbols ******                                                     >> postbuild.txt
xcopy ..\bin\%1\*.pdb					%DeployDir%			/Y	>> postbuild.txt
xcopy ..\utilities\homer\bin\%1\*.pdb			%DeployDir%			/Y	>> postbuild.txt
goto done

:RemovePdb
if exist %DeployDir%\*.pdb			del /s %DeployDir%\*.pdb															>> postbuild.txt

goto done

:usage
echo Usage: PostBuild.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).

goto done

:done
