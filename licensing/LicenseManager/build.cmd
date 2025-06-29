@echo off
rem -------------------------------
rem Ant build script wrapper
rem -------------------------------
setlocal
set TARGET=%1
set BUILDFILE=%2
if "%BUILDFILE%"=="" set BUILDFILE=build.xml

rem -- Initialize Environment
call ..\..\Tools\build-scripts\build-init.cmd

rem -- Call ANT
echo Executing ANT target '%TARGET%'.
echo.
call %ANT% -buildfile %BUILDFILE% %TARGET%

rem -- DONE
