@echo off

if "%1"=="add" goto add
if "%1"=="change" goto change
goto usage 

:add
if "%2"=="" goto usage
CD %~dp0
NET ACCOUNTS /MAXPWAGE:UNLIMITED
NET USER CiscoUAE %2 /ADD /passwordchg:no /passwordreq:yes /expires:never
NET LOCALGROUP Administrators CiscoUAE /ADD 
NTRIGHTS -u CiscoUAE +r SeServiceLogonRight
goto end

:change
if "%2"=="" goto usage
CD %~dp0
NET USER CiscoUAE %2 /passwordchg:no /passwordreq:yes /expires:never
NTRIGHTS -u CiscoUAE +r SeServiceLogonRight
goto end

:usage
echo "Usage: %0 [add|change] <password>"
pause

:end
exit 0