set iNumDevices=250
set iLwaitTime=5
set iRampTime=1
set iCallDuration=240
set sTestType=in
set iDialNum=4600
set iDialNumIncr=1
set sCallScriptName=dSethCallScript.csv

if EXIST %sCallScriptName% del %sCallScriptName%

call CallScriptGen.cmd -d %iNumDevices% -l %iLwaitTime% -r %iRampTime% -c %iCallDuration% -t %sTestType% -n %iDialNum% -i %iDialNumIncr% > %sCallScriptName%