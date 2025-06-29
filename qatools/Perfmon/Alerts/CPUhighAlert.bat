@echo off

cd "C:\Documents and Settings\Administrator\Desktop"

REM if exist CPUhighAlert.log goto end

set hh=%time:~0,2%

REM Since there is no leading zero for times before 10 am, have to put in
REM a zero when this is run before 10 am.

if "%time:~0,1%"==" " set hh=0%hh:~1,1%

set stamp=%date:~12,2%%date:~4,2%%date:~7,2%_%hh%%time:~3,2%%time:~6,2%

echo ---- CPU Usage above 45% !! : %date% %time%  >> CPUhighAlert.log