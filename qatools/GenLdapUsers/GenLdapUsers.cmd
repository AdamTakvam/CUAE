@echo off

cls

REM You must send the output to a file
REM \>genldapusers > myuserfile.csv

setlocal

set sFirst=test
set sLast=user
set sUserName=tuser
set sPassword=metreos
set iPin=1111
set iNumUsers=200
set iUser=1

:START_GEN_USERS
if %iUser% GEQ %iNumUsers% goto EOF
echo test%iUser%,user%iUser%,tuser%iUser%,metreos%iUser%,,,11%iUser%,,,,

set /A iUser+=1
goto START_GEN_USERS

:EOF







