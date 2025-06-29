@echo off

setlocal

if "%CUAEWORKSPACE%"=="" set CUAEWORKSPACE=X:
set Root=%CUAEWORKSPACE%
set BuildDir=%Root%\Build

set DeployDir=%BuildDir%

set MmsDir=%DeployDir%\MediaServer

set PostBuildTxt=..\..\postbuild\postbuild.txt

REM ******* Create directories *******

if not exist %MmsDir%                   mkdir %MmsDir%
if not exist %MmsDir%\Audio             mkdir %MmsDir%\Audio
if not exist %MmsDir%\dump              mkdir %MmsDir%\dump

if exist %PostBuildTxt% del %PostBuildTxt% /F /Q >nul

echo ****** Installing common Framework assemblies ******                           	>> %PostBuildTxt%
xcopy ..\..\bin\mmsserver.exe                       	%MmsDir%                /Y      >> %PostBuildTxt%
xcopy ..\..\bin\mmswin.exe                          	%MmsDir%                /Y      >> %PostBuildTxt%

:acedebug
if exist %MmsDir%\aced.dll del /F /Q					%MmsDir%\aced.dll
xcopy %Root%\contrib\ACE_Wrappers\bin\aced.dll      	%MmsDir%                /Y

if exist %MmsDir%\libttsapi.dll del /F /Q 				%MmsDir%\libttsapi.dll          >> %PostBuildTxt%
xcopy %Root%\contrib\neospeech\libttsapi.dll 				%MmsDir% 				/Y      >> %PostBuildTxt%


:done
echo.																					>> %PostBuildTxt%
echo ****** DONE ******                                                             	>> %PostBuildTxt%
