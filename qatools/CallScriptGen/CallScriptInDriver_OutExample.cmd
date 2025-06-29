set iNumDevices=200
set iLwaitTime=15
set iRampTime=1
set iCallDuration=600
set sTestType=inout
set iPrefix=73

set sCallScriptName=dJanCallScript.csv

if EXIST %sCallScriptName% del %sCallScriptName%

call callscriptgen.cmd -d %iNumDevices% -l %iLwaitTime% -r %iRampTime% -p %iPrefix% -c %iCallDuration% > %sCallScriptName%