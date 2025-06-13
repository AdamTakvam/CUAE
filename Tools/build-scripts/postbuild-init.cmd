@echo off
rem -------------------------------------------------
rem Initialize environment for postbuild-operations 
rem -------------------------------------------------

rem -- Initialize Metreos Control envvars
if "%CUAEWORKSPACE%"=="" set CUAEWORKSPACE=X:
call %CUAEWORKSPACE%\Tools\build-scripts\build-init.cmd

rem -- Tools --
set CLR_gacutil="C:\Program Files\Microsoft Visual Studio 8\SDK\v2.0\Bin\gacutil.exe"
