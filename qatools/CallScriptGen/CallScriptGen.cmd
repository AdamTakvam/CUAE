@echo off
cls

setlocal

REM START DEFINE CONSTANTS 
REM Don't change these.  I really just want to remember what they are.
REM Later the script will be more flexible.

set sLwaitStr=set:lwait=
set sPrefixStr=set:prefix=
set sWaitStr=wait:=
set sOffHookStr=ofhk
set sDialStr=dial:
set sOnHookStr=onhk
set sDialCalcStr=dial:+

REM END DEFINE CONSTANTS

REM Format inout
REM   1,set:lwait=30 set:prefix=73 wait:5 ofhk dial:+1 wait:600 onhk
REM   2,pickup wait:10000000 onhk

REM Format in
REM   1,set:prefix=73 wait:1 ofhk dial:+1 wait:600 onhk
REM   2,

REM Variable values will change

set iCurrentRampTime = 0

REM Time to wait between test runs. This gives the calls time to hangup.
set iLwait=0

REM Prefix to append to all incoming calls to app (Number to dial). 0 means no prefix
set iPrefix=0

REM Number of phones needed to test both incoming and outgoing
set iNumDevices=2

REM The type of test.  Incoming to app or in to app and appdials (inandout app).
set sTestType=inout

REM Call duration
set iCallDuration=5

REM The rate in seconds to wait between calls
set iRampTime=1

REM How to vary the call times (stagger, rand, none)
set sCallTimePattern=none

REM If sCallTimePattern is stagger, the amount of seconds to stagger hangups.  0, do not stagger
set iStaggerTime=0

REM Are you using a calculation when dialing?  dial:+1 or dial:+100
set bDialCalc=TRUE

REM Call the next phone in the call script
set iDialCalcNum=1

REM Number to actually call
set iDialNum=0

REM Increment value for numbers to call if iDialNum is not 0
set iDialNumIncr=1

REM Call script name
REM set sCallScriptName=dCallScript.csv

REM del %sCallScriptName%

set argNum=1

:START_GET_PARMS

if x%1x EQU x-helpx goto DISPLAY_HELP
if x%1x EQU x-hx    goto DISPLAY_HELP
if x%1x EQU x-?x    goto DISPLAY_HELP
if x%1x EQU xhx     goto DISPLAY_HELP
if x%1x EQU x?x	    goto DISPLAY_HELP
if x%1x EQU xx      goto END_GET_PARMS

call :PARSE_PARAMETER %argNum% %1 %2
shift /1
shift /1
set /A argNum+=1
goto START_GET_PARMS

:END_GET_PARMS

set iDeviceNum=1
set iCurrentRampTime=%iRampTime%

if %iDialNum% NEQ 0 set iCurrentDialNum=%iDialNum%

set iCurrentCallDuration=%iCallDuration%

:START_GEN_CALLSCRIPT

if %iDeviceNum% GEQ %iNumDevices% goto EOF

if %iPrefix% NEQ 0 (
REM Caller format for inout
if %sTestType% EQU inout echo %iDeviceNum%,set:lwait=%iLwait% set:prefix=%iPrefix% wait:%iCurrentRampTime% ofhk dial:+%iDialCalcNum% wait:%iCurrentCallDuration% onhk

REM Caller format for in
if %sTestType% EQU in echo %iDeviceNum%,set:lwait=%iLwait% set:prefix=%iPrefix% wait:%iCurrentRampTime% ofhk dial:%iCurrentDialNum% wait:%iCurrentCallDuration% onhk
)

if %iPrefix% EQU 0 (
REM Caller format for inout
if %sTestType% EQU inout echo %iDeviceNum%,set:lwait=%iLwait% wait:%iCurrentRampTime% ofhk dial:+%iDialNumIncr% wait:%iCurrentCallDuration% onhk

REM Caller format for in
if %sTestType% EQU in echo %iDeviceNum%,set:lwait=%iLwait% wait:%iCurrentRampTime% ofhk dial:%iCurrentDialNum% wait:%iCurrentCallDuration% onhk
)

set /A iDeviceNum+=1

REM inout Callee format
if %sTestType% EQU inout echo %iDeviceNum%,pickup wait:10000000 onhk

REM in Callee format
if %sTestType% EQU in echo %iDeviceNum%,

set /A iDeviceNum+=1
set /A iCurrentRampTime+=%iRampTime%
if %iDialNum% NEQ 0 set /A iCurrentDialNum+=%iDialNumIncr%

if %sCallTimePattern% EQU stagger set /A iCurrentCallDuration-=%iStaggerTime%
if %iCurrentCallDuration% LEQ 0 set /A iCurrentCallDuration=%iCallDuration%

goto START_GEN_CALLSCRIPT

goto EOF

:PARSE_PARAMETER
REM Parse parameter function
set parmNum=%1
set parmFlag=%2
set parmVal=%3

REM echo parm number %parmNum%
REM echo parm flag %parmFlag%
REM echo parm value %parmVal%

if x%parmVal%x EQU xx echo Invalid number of parameters. All parameters are in -f value pairs.

if x%parmFlag%x EQU x-lx (
set iLwait=%parmVal%
goto END_PARSE_PARAMETER
)

if x%parmFlag%x EQU x-px (
set iPrefix=%parmVal%
goto END_PARSE_PARAMETER
)

if x%parmFlag%x EQU x-tx (
set sTestType=%parmVal%
goto END_PARSE_PARAMETER
)

if x%parmFlag%x EQU x-dx (
set iNumDevices=%parmVal%
goto END_PARSE_PARAMETER
)

if x%parmFlag%x EQU x-cx (
set iCallDuration=%parmVal%
goto END_PARSE_PARAMETER
)

if x%parmFlag%x EQU x-rx (
set iRampTime=%parmVal%
goto END_PARSE_PARAMETER
)

if x%parmFlag%x EQU x-xx (
set sCallTimePattern=%parmVal%
goto END_PARSE_PARAMETER
)

if x%parmFlag%x EQU x-sx (
set iStaggerTime=%parmVal%
goto END_PARSE_PARAMETER
)

if x%parmFlag%x EQU x-yx (
set bDialCalc=%parmVal%
goto END_PARSE_PARAMETER
)

if x%parmFlag%x EQU x-zx (
set iDialCalcNum=%parmVal%
goto END_PARSE_PARAMETER
)

if x%parmFlag%x EQU x-nx (
set iDialNum=%parmVal%
goto END_PARSE_PARAMETER
)

if x%parmFlag%x EQU x-ix (
set iDialNumIncr=%parmVal%
goto END_PARSE_PARAMETER
)

if x%parmFlag%x EQU x-fx (
set sCallScriptName=%parmVal%
goto END_PARSE_PARAMETER
)

REM *** default if a parameter does not get a match ***
echo INVALID parameter -- %parmFlag% --
goto EOF
:END_PARSE_PARAMETER

goto :EOF
:DISPLAY_HELP
echo .
echo %~n0 [-d iNumDevices] [-l iLwaitTime] [-p iPrefix] [-t sTestType] 
echo      [-c iCallDuration] [-r iRampTime] [-x sCallTimePattern] 
echo      [-s iStaggerTime] [-y bDialCalc] [-z iDialCalcNum]
echo      [-n iDialNum] [-i iDialNumIncr] [-f sCallScriptName]
echo      %~n0 -help: Display this help
echo .
echo iNumDevices - The total number of devices needed for the test in and out. Default = 2.
echo .
echo iLwaitTime - Time to wait between test runs. Time to hangup calls cleanly. 
echo .
echo iPrefix - Prefix to append to all incoming calls to app. Prefix digits. 0 means no prefix.  Default is 0.
echo .
echo sTestType - The type of test.  Incoming to app or in to app and appdials (inandout app).  (in or inandout). Default = inandout. 
echo . 
echo iCallDuration - Call duration in seconds. Default = 5.
echo .
echo iRampTime - The number of seconds to wait in between calls. Default = 1 seconds
echo . 
echo sCallTimePattern - How to vary call times. (stagger or none)
echo .
echo iStaggerTime - If sCallTimePattern is stagger, the amount of time in seconds to stagger hangups. 0, do not stagger hangups Default = 0.
echo . 
echo bDialCalc - Whether you are using a calculation for the dial number. Default = TRUE. You may explicitly specify a number to dial.
echo .
echo iDialCalcNum - If bDialCalc is TRUE, the number to use in the calculation. Default = 1.
echo .
echo iDialNum - Number to explicitly dial. 0, no number specified, using calculation.
echo .
echo iDialNumIncr - If iDialNum is not equal to 0, the number to increment the next device by. Example: iNumDevices = 100, iDialNum = 2000, iDialIncrNum = 1 would result in Directory numbers 2000-2099.
echo .
echo sCallScriptName - Not supported at this time.
echo .

:EOF


