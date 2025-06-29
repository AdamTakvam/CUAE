@echo off

setlocal

if "%1" == "" goto usage

call x:\autobuild\CreateDeployDirectoryStructure.bat

rem echo on

set ActionDir=..\..\..\NativeActions\Database\bin
set TypesDir=..\..\..\NativeTypes\ClickToTalk\bin
set OutlookSrcDir=..\..\..\OutlookClient\OutlookClientSetup
set StandaloneClientDir=..\..\..\StandaloneClient
if not exist %StandaloneClientDir% mkdir %StandaloneClientDir%

set ClickTalkDir=X:\Build\AppSuite\ClickToTalk
if not exist %ClickTalkDir% mkdir %ClickTalkDir%

set OutlookDir=%ClickTalkDir%\OutlookPlugin
if not exist %OutlookDir%	mkdir %OutlookDir%

set StandaloneOutputDir=%ClickTalkDir%\StandaloneClient
if not exist %StandaloneOutputDir%	mkdir %StandaloneOutputDir%

if exist postbuild.txt del postbuild.txt /F /Q

echo.                                                                                    > postbuild.txt

echo ****** Copy Native Actions ******													>> postbuild.txt
xcopy %ActionDir%\%1\Metreos.Native.ClickToTalk.dll				%ClickTalkDir%   /Y		>> postbuild.txt

echo.																					>> postbuild.txt
   
echo ****** Copy Native Types ******													>> postbuild.txt
xcopy %TypesDir%\%1\Metreos.Types.ClickToTalk.dll				%ClickTalkDir%   /Y		>> postbuild.txt

echo.

echo ****** Copy Outlook Plugin Installer ******										>> postbuild.txt
xcopy %OutlookSrcDir%\%1\*.*									%OutlookDir%	   /Y   >> postbuild.txt

REM -------------------------------------------------------------------------------
REM Install debug symbols if we are doing a debug build
REM -------------------------------------------------------------------------------

if /i NOT "%1"=="debug" if /i NOT "%1"=="debugmethodcalltrace" goto RemovePdb

echo.																					>> postbuild.txt

echo ****** Copy Native Action Debug Symbols ******										>> postbuild.txt
xcopy %ActionDir%\%1\Metreos.Native.ClickToTalk.pdb				%ClickTalkDir%   /Y		>> postbuild.txt

echo.																					>> postbuild.txt
   
echo ****** Copy Native Types Debug Symbols ******										>> postbuild.txt
xcopy %TypesDir%\%1\Metreos.Types.ClickToTalk.pdb				%ClickTalkDir%   /Y		>> postbuild.txt

echo.
echo %StandaloneOutputDir%
echo ****** Copy Standalone Client ******                                               >> postbuild.txt
xcopy %StandaloneClientDir%\bin\%1\StandaloneClient.*           %StandaloneOutputDir% /Y    >> postbuild.txt

REM  ****** We need the Metreos.Utilities.dll (and its dependencies) to make this truly standalone                              
xcopy %FwCoreDir%\Metreos.Utilities.dll                         %StandaloneOutputDir% /Y    >> postbuild.txt
xcopy %FWCoreDir%\Metreos.Interfaces.dll                        %StandaloneOutputDir% /Y    >> postbuild.txt
xcopy %FwCoreDir%\ChilkatDotNet.dll                             %StandaloneOutputDir% /Y    >> postbuild.txt

goto done

:RemovePdb

if exist %AppSuiteDir%\*.pdb del /s %AppSuiteDir%\*.pdb									>> postbuild.txt  

goto done

:usage
echo Usage: PostBuild.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).

:done
echo.																					>> postbuild.txt
echo ****** DONE ******                                                                 >> postbuild.txt                            
