@echo off
:: = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
::
:: HMP license installer
::
:: Applies a "gold" universal license to an HMP installation. This script takes 
:: the same actions as does the CUAE core installer in licensing HMP; you would
:: use this script rather than the installer when you want to manually install
:: and license HMP, for example if you need to uninstall and reinstall HMP.
::
:: To use this script:
:: 1.  Create a directory C:\hmplicfiles
:: 2.  Into C:\hmplicfiles, copy the following files:
:: 2a.   CUAEUtl1.dll (release build)
:: 2b.   CUAEUtl2.dll (release build)
:: 2c.   The HMP license file
:: 2d.   CUAEhmputil.exe (release build)
:: 2e.   ACE.dll
:: 3.  Edit this script, replacing the name of the license file with whatever
::     name the new license file has
:: 4.  Run the script. It will pause after each action until you hit enter
::     or control+C out. The actions it takes are:
::     a. Stops HMP license manager service, if started
::     b. Copies CUAEUtl1.dll and CUAEUtl2.dll to where HMP can find them
::     c. Copies the license file to HMP expected location
::     d. Applies the license, as if you had done it manually in HMP license mgr
::     e. Restarts the HMP license manager, which creates the HMP fcd file etc.
::     f. Stops the HMP DCM, if started
::     g. Validates the license using cuaehmputil
::     h. Applies license to registry using cuaehmputil
::
:: = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
:A-1 
goto INIT  

:A-2 
if %retval% GTR 99 goto ERRXIT
pause

:B-1
echo.
echo %prognam% stopping HMP license manager service ...
echo %prognam% net stop "INTEL HMP License Manager"
net stop "INTEL HMP License Manager"
echo.
echo %prognam% action succeeded
pause

:C-1
echo.
echo %prognam% copying CUAE utility 1 DLL to HMP lib directory ...
echo %prognam% copy "c:\hmplicfiles\cuaeutl1.dll" "c:\program files\Cisco Systems\Unified Application Environment\LicenseServer\cuaeutl1.dll"
copy "c:\hmplicfiles\cuaeutl1.dll" "c:\program files\Cisco Systems\Unified Application Environment\LicenseServer\cuaeutl1.dll"
if errorlevel 1 goto ERRXIT
echo.
echo %prognam% action succeeded
pause

:C-2
echo.
echo %prognam% copying CUAE utility 2 DLL to HMP lib directory ...
echo %prognam% copy "c:\hmplicfiles\cuaeutl2.dll" "c:\program files\Cisco Systems\Unified Application Environment\LicenseServer\cuaeutl2.dll"
copy "c:\hmplicfiles\cuaeutl2.dll" "c:\program files\Cisco Systems\Unified Application Environment\LicenseServer\cuaeutl2.dll"
if errorlevel 1 goto ERRXIT
echo.
echo %prognam% action succeeded
pause

:C-3
echo.
echo %prognam% copying HMP license file to HMP data directory  ...
echo %prognam% copy "c:\hmplicfiles\240r240v200e240c240s240i_pur.lic" "c:\program files\intel\hmp\data\240r240v200e240c240s240i_pur.lic"
copy "c:\hmplicfiles\240r240v200e240c240s240i_pur.lic" "c:\program files\intel\hmp\data\240r240v200e240c240s240i_pur.lic"
if errorlevel 1 goto ERRXIT
echo.
echo %prognam% action succeeded
pause

:D-1
echo.
echo %prognam% applying HMP license step 1 ...
echo %prognam% "c:\program files\intel\hmp\bin\installs.exe" -r -n "INTEL HMP License Manager"
"c:\program files\intel\hmp\bin\installs.exe" -r -n "INTEL HMP License Manager"
if errorlevel 1 goto ERRXIT

echo.
echo %prognam% applying HMP license step 2 ...
echo %prognam% "c:\program files\intel\hmp\bin\installs.exe" -e "C:\Program Files\Intel\HMP\bin\lmgrd.exe" -c "C:\Program Files\Intel\HMP\data\240r240v200e240c240s240i_pur.lic" -l "C:\Program Files\Intel\HMP\log\Flex.dl" -n "INTEL HMP License Manager" -k "-local"
"c:\program files\intel\hmp\bin\installs.exe" -e "C:\Program Files\Intel\HMP\bin\lmgrd.exe" -c "C:\Program Files\Intel\HMP\data\240r240v200e240c240s240i_pur.lic" -l "C:\Program Files\Intel\HMP\log\Flex.dl" -n "INTEL HMP License Manager" -k "-local"
if errorlevel 1 goto ERRXIT
echo.
echo %prognam% action succeeded
pause

:E-1
echo.
echo %prognam% starting HMP license manager service ...
echo %prognam% HMP fcd, pcd, and config will be generated ...
echo %prognam% net start "INTEL HMP License Manager"
net start "INTEL HMP License Manager"
if errorlevel 1 goto ERRXIT
echo.
echo %prognam% action succeeded
pause

:E-2
:: we could verify .fcd and .pcd exist here

:F-1
echo.
echo %prognam% stopping HMP DCM ...
echo %prognam% c:\hmplicfiles\cuaehmputil.exe -stop
c:\hmplicfiles\cuaehmputil.exe -stop
if errorlevel 0 goto G-1
echo.
echo %prognam% error stopping HMP DCM  
echo %prognam% try placing ACED.dll in the path
goto ERRXIT

:G-1
echo.
echo %prognam% action succeeded
echo %prognam% validating the new license ...
echo %prognam% c:\hmplicfiles\cuaehmputil.exe -restore
c:\hmplicfiles\cuaehmputil.exe -restore
if errorlevel 1 goto ERRXIT

:H-1
echo.
echo %prognam% action succeeded
echo %prognam% registering the new license ...
echo %prognam% c:\hmplicfiles\cuaehmputil.exe -register
c:\hmplicfiles\cuaehmputil.exe -register
if errorlevel 1 goto ERRXIT

echo. 
echo %prognam% action succeeded
echo.
echo %prognam% license installation complete
goto :NORMXIT



:: - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
:INIT
:: - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
set retval=0
set prognam=HMPLIC

echo.
echo %prognam% This script licenses HMP by copying DLLs to their 
echo %prognam% expected locations and running approriate scripts. 
echo %prognam% This script will pause after each activity,   
echo %prognam% at which point you can either type CTRL+C to exit,  
echo %prognam% or any other key to continue. 
echo.
echo %prognam% This script expects all installation files to reside
echo %prognam% in directory c:\hmplicfiles, including hmputl1.dll,
echo %prognam% hmputl2.dll, cuaehmputl.exe, and the HMP license file.
echo. 
echo %prognam% The name of the license file is hard coded into this 
echo %prognam% script in five places. Whenever the name of the license 
echo %prognam% file changes, this script code must therefore be updated 
echo %prognam% with the new license file name.
echo.
echo %prognam% At any pause point, hit CTRL+C to exit this script.
echo.

set curpath=c:\hmplicfiles\cuaeutl1.dll
if not exist %curpath% goto :ERRFILE

set curpath=c:\hmplicfiles\cuaeutl2.dll
if not exist %curpath% goto :ERRFILE

set curpath=c:\hmplicfiles\240r240v200e240c240s240i_pur.lic
if not exist %curpath% goto :ERRFILE

set curpath=c:\hmplicfiles\cuaehmputil.exe
if not exist %curpath% goto :ERRFILE

set curpath=c:\hmplicfiles\ACE.dll
if not exist %curpath% goto :ERRFILE

set curpath="c:\program files\intel\hmp\bin\installs.exe" 
if not exist %curpath% goto :ERRFILE

goto A-2


:: - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
:ERRFILE
:: - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
echo.
echo %prognam% missing file: "%curpath%" does not exist  
goto :NORMXIT

:: - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
:ERRXIT
:: - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
echo.
echo %prognam% error during previous action - result code is "%retval%"
goto :NORMXIT

:: - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
:NORMXIT
:: - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
echo.
echo %prognam% hit any key to exit ...
pause
goto :EOF

::        1         2         3         4         5         6         7         8
:: 3456789012345678901234567890123456789012345678901234567890123456789012345678901
