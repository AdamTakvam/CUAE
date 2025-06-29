@ECHO OFF

REM Clean up various binary files.

ECHO Cleaning all solution and project output files.

if exist bin rmdir /S /Q bin

for /R . %%i IN (*.dll) DO del %%i
for /R . %%i IN (*.pdb) DO del %%i
for /R . %%i IN (obj) DO rmdir /S /Q %%i
for /R . %%i IN (bin) DO rmdir /S /Q %%i

SET ERRORLEVEL=0
