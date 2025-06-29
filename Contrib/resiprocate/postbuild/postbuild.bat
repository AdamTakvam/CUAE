echo off

REM goto done

setlocal

if "%1" == "" goto usage

set ResiprocateBase=X:\Contrib\resiprocate
set ResiprocateLib=%ResiprocateBase%\lib

set SipRuntimeBase=X:\sip-service\projects\sip-runtime

set Root=X:
set BuildDir=%Root%\Build

set DeployDir=%BuildDir%\SipService
set PostBuildTxt=..\..\postbuild\postbuild.txt

REM ******* Create directories *******

if not exist %DeployDir%    mkdir %DeployDir%

if exist %PostBuildTxt% del %PostBuildTxt% /F /Q >nul

echo ****** Copying files ******                                        >  %PostBuildTxt%
xcopy %ResiprocateBase%\contrib\ares\Debug\ares.lib      %ResiprocateLib%     /Y      >> %PostBuildTxt%
xcopy %ResiprocateBase%\contrib\ares\Debug\ares.pdb      %ResiprocateLib%     /Y      >> %PostBuildTxt%

xcopy %ResiprocateBase%\rutil\Debug\rutil.lib      %ResiprocateLib%     /Y      >> %PostBuildTxt%
xcopy %ResiprocateBase%\rutil\Debug\rutil.pdb      %ResiprocateLib%     /Y      >> %PostBuildTxt%

xcopy %ResiprocateBase%\resip\stack\Debug\resiprocate.lib      %ResiprocateLib%     /Y      >> %PostBuildTxt%
xcopy %ResiprocateBase%\resip\stack\Debug\resiprocate.pdb      %ResiprocateLib%     /Y      >> %PostBuildTxt%

xcopy %ResiprocateBase%\resip\dum\Debug\dum.lib      %ResiprocateLib%     /Y      >> %PostBuildTxt%
xcopy %ResiprocateBase%\resip\dum\Debug\dum.pdb      %ResiprocateLib%     /Y      >> %PostBuildTxt%

xcopy %ResiprocateLib%\* %SipRuntimeBase%\Debug\	/Y >>%PostBuildTxt%

if /i "%1" == "RELEASE" goto cleanDebugSymbols

goto done

:cleanDebugSymbols
if exist %DeployDir%\*.pdb              del /s  %DeployDir%\*.pdb               >> %PostBuildTxt%
goto done

:usage
echo.
echo Usage: postbuid.bat <Configuration>
echo Where <Cofiguration> is "DEBUG" or "RELEASE"

:done
echo.                                                      >> %PostBuildTxt%
echo ****** DONE ******                                    >> %PostBuildTxt%
