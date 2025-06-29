@ECHO OFF

SETLOCAL

IF NOT EXIST "openh323.sln" GOTO noopenh323

IF EXIST "move-to-contrib.txt" del move-to-contrib.txt

devenv openh323.sln /clean Release 
devenv openh323.sln /clean Debug

GOTO done

:noopenh323
ECHO Execute this script from the OpenH323 root directory.

:done
