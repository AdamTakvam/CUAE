set iNumDevices=400
set iLwaitTime=5
set iRampTime=1
set iCallDuration=600
set sTestType=in
set iDialNum=26000
set iDialNumIncr=1
set sCallTimePattern=stagger
set sCallScriptName=dTest.csv
set iStaggerTime=1

if EXIST %sCallScriptName% del %sCallScriptName%

call CallScriptGen.cmd -d %iNumDevices% -l %iLwaitTime% -r %iRampTime% -c %iCallDuration% -t %sTestType% -n %iDialNum% -i %iDialNumIncr% -x %sCallTimePattern% -s %iStaggerTime% > %sCallScriptName%