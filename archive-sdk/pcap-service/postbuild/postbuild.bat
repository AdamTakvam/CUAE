@echo off

setlocal

if "%1" == "" goto usage

REM ******* For C# project
call X:\appserver\CreateDeployDirectoryStructure.bat

set PtLibDllPath=X:\contrib\pwlib\lib
set AceDllPath=X:\Contrib\ace-5.4\lib

if /i "%1" == "debug" (
    @set PtLib=%PtLibDllPath%\PtLibd) else ( 
    @set PtLib=%PtLibDllPath%\PtLib)
    
if /i "%1" == "debug" (
    @set AceDll=%AceDllPath%\aced) else (
    @set AceDll=%AceDllPath%\ace)
    
set Root=X:
set BuildDir=%Root%\Build

set DeployDir=%BuildDir%\PCapService

set PostBuildTxt=postbuild.txt

if exist PostBuildTxt del PostBuildTxt /F /Q

REM ******* Create directories *******
if not exist %DeployDir%    mkdir %DeployDir%

if exist %PostBuildTxt% del %PostBuildTxt% /F /Q >nul

echo ****** Copying files ******                                        >> %PostBuildTxt%
xcopy x:\build\Framework\1.0\cpp-core*.dll      %DeployDir%     /Y      >> %PostBuildTxt%
xcopy x:\build\Framework\1.0\cpp-core*.pdb      %DeployDir%     /Y      >> %PostBuildTxt%
xcopy %PtLib%.dll								%DeployDir%     /Y      >> postbuild.txt
xcopy %PtLib%.pdb								%DeployDir%     /Y      >> postbuild.txt
xcopy %AceDll%.dll                              %DeployDir%     /Y      >> postbuild.txt

echo ****** Copying pcap-service runtime ******                         >> %PostBuildTxt%
xcopy ..\projects\pcap-runtime\%1\*.exe			%DeployDir%     /Y      >> %PostBuildTxt%

echo ****** Copying config file ******									>> %PostBuildTxt%
xcopy ..\config\pcap-service.config				%DeployDir%     /Y      >> %PostBuildTxt%

echo ******* Copying Utility program and config ******
xcopy ..\utilities\CallMonitorReport\bin\%1\*.exe		%DeployDir%     /Y      >> %PostBuildTxt%
xcopy ..\utilities\CallMonitorReport\bin\%1\*.config	%DeployDir%     /Y      >> %PostBuildTxt%
xcopy ..\utilities\NicSelector\%1\*.exe					%DeployDir%     /Y      >> %PostBuildTxt%

if /i "%1" == "RELEASE" goto cleanDebugSymbols

xcopy ..\projects\pcap-runtime\%1\*.pdb					%DeployDir%     /Y      >> %PostBuildTxt%
xcopy ..\utilities\CallMonitorReport\bin\%1\*.pdb		%DeployDir%     /Y      >> %PostBuildTxt%
xcopy ..\utilities\NicSelector\%1\*.pdb					%DeployDir%     /Y      >> %PostBuildTxt%
goto done

:cleanDebugSymbols
if exist %DeployDir%\*.pdb					del /s  %DeployDir%\*.pdb				>> %PostBuildTxt%
if exist %DeployDir%\cpp-cored.dll			del /s  %DeployDir%\cpp-cored.dll       >> %PostBuildTxt%
if exist %DeployDir%\ptlibd.dll				del /s  %DeployDir%\ptlibd.dll          >> %PostBuildTxt%
if exist %DeployDir%\aced.dll				del /s  %DeployDir%\aced.dll            >> %PostBuildTxt%
if exist %DeployDir%\pcap-service.config    del /s  %DeployDir%\pcap-service.config >> %PostBuildTxt%

if exist %DeployDir%\CallMonitorReport.exe.config    del /s  %DeployDir%\CallMonitorReport.exe.config	>> %PostBuildTxt%
if exist %DeployDir%\CallMonitorReport.exe			 del /s  %DeployDir%\CallMonitorReport.exe			>> %PostBuildTxt%
if exist %DeployDir%\NicSelector.exe				del /s  %DeployDir%\NicSelector.exe			>> %PostBuildTxt%
goto done

:usage
echo.
echo Usage: postbuid.bat <Configuration>
echo Where <Cofiguration> is "DEBUG" or "RELEASE"

:done
echo.                                                      >> %PostBuildTxt%
echo ****** DONE ******                                    >> %PostBuildTxt%
