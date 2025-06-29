REM Number of Devices both in and out
set iNumDevices=200

REM Time between interations of script in seconds
set iLwaitTime=15

REM Time to wait between each call placed in seconds.  Place a call every 1 second.
set iRampTime=1

REM Call Duration
set iCallDuration=600

REM Prefix to dial for incoming calls
set iPrefix=73

REM Test type (in or inout)
set sTestType=inout

REM Number to dial
REM set iDialNum=4600

REM If there is a number to dial, increment each number to dial by this much.
REM set iDialNumIncr=1

REM Script name that will be produced to be used for SimClient
set sCallScriptName=dCallScript.csv

REM Call duration pattern (stagger|none) default = none
REM set sCallTimePattern=stagger

REM If sCallTimePattern is stagger, stagger the call duration by this much
REM set iStaggerTime=1

if EXIST %sCallScriptName% del %sCallScriptName%

call CallScriptGen.cmd -d %iNumDevices% -l %iLwaitTime% -r %iRampTime% -p %iPrefix% -c %iCallDuration% -x %sCallTimePattern% -s %iStaggerTime% > %sCallScriptName%