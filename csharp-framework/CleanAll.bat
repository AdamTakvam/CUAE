@ECHO OFF

REM Clean up the various binary files.

ECHO Cleaning all solution and project output files.

if exist bin rmdir /S /Q bin

for /R . %%i IN (Metreos.*.dll) DO if NOT "%%~nxi"=="sqlite.dll" del %%i
for /R . %%i IN (Metreos.*.pdb) DO del %%i

SET ERRORLEVEL=0