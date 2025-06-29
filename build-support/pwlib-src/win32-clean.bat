@ECHO OFF

SETLOCAL

IF NOT EXIST "pwlib.sln" GOTO noopenh323

IF EXIST "move-to-contrib.txt" del move-to-contrib.txt

devenv pwlib.sln /clean Release 
devenv pwlib.sln /clean Debug

GOTO done

:noopenh323
ECHO Execute this script from the PWLib root directory.

:done
