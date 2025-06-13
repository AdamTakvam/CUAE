@echo off
rem -----------------------------------------------------------
rem Build a standard ANT solution
rem
rem Created: 5/16/05 (jdixson@metreos.com)
rem
rem Command Line arguments:
rem    %1 -> the target to build
rem -----------------------------------------------------------

setlocal
rem -- initialize build tools --
if "%CUAEWORKSPACE%"==""    set CUAEWORKSPACE=X:
call %CUAEWORKSPACE%\Tools\build-scripts\build-init.cmd

rem -- process args --
set TARGET=%1
set BUILDFILE=%2
if "%BUILDFILE%"=="" set BUILDFILE="build.xml"

rem -- call ant --
echo BUILD STARTING
call %ANT% -quiet -buildfile %BUILDFILE% %TARGET%
echo ERRORLEVEL = %ERRORLEVEL%
rem exit %ERRORLEVEL%
