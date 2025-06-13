@ECHO OFF

REM Clean up the various binary files.

ECHO Cleaning all solution and project output files.

if exist bin rmdir /S /Q bin

for /R . %%i IN (*.dll) DO del %%i
for /R . %%i IN (*.pdb) DO del %%i

SET ERRORLEVEL=0