@echo off
rem -- PostBuild.bat
rem -- 
rem -- Implement this script to copy build products from the 
rem -- local module directory to the product-level 'Build' directory
rem --
setlocal
set BuildTarget=%1


if "%BuildTarget%" == "" set BuildTarget=Debug

if "%CUAEWORKSPACE%"=="" set CUAEWORKSPACE=X:
call %CUAEWORKSPACE%\Tools\build-scripts\postbuild-init.cmd
call %CUAEWORKSPACE%\autobuild\CreateDeployDirectoryStructure.bat

echo %BuildTarget% 

set ProjectRoot=%CUAEWORKSPACE%\snmp-agent\CUAEAgent
set ProjectRoot1=%CUAEWORKSPACE%\snmp-agent\RegUpdate\RegUpdate\bin\%BuildTarget%
set PostBuildLog=%CUAEWORKSPACE%\snmp-agent\postbuild\postbuild.txt

set DeployDir=%MetreosBuildRoot%\StatsService\
set SymbolDir=%DeployDir%\Symbols

if not exist %DeployDir%          mkdir %DeployDir%
if not exist %SymbolDir%          mkdir %SymbolDir%

mkdir %CUAEWORKSPACE%\snmp-agent\postbuild
rem -- Clear post-build log
if exist %PostBuildLog% del %PostBuildLog% /F /Q
@echo ** PostBuild Beginning > %PostBuildLog%    

xcopy %ProjectRoot%\CUAEAgent.dll				 %DeployDir% 	/Y				>> %PostBuildLog%
xcopy %ProjectRoot%\%BuildTarget%\CUAEAgent.pdb  %SymbolDir% 	/Y				>> %PostBuildLog%

if /i "%BuildTarget%"=="release" goto releaseSymbols

xcopy %M_FrameworkDir%\cpp-core_wind.dll			%DeployDir% /Y			>> %PostBuildLog%
xcopy %M_FrameworkDir%\Symbols\cpp-core_wind.pdb	%SymbolDir% /Y			>> %PostBuildLog%
xcopy %MetreosContribRoot%\ace-5.4\lib\aced.dll		%DeployDir% /Y			>> %PostBuildLog%
xcopy %MetreosContribRoot%\ace-5.4\lib\aced.pdb		%SymbolDir% /Y			>> %PostBuildLog% 
goto done

:releaseSymbols
xcopy %M_FrameworkDir%\cpp-core_win.dll			%DeployDir% /Y			>> %PostBuildLog%
xcopy %M_FrameworkDir%\Symbols\cpp-core_win.pdb	%SymbolDir% /Y			>> %PostBuildLog%
xcopy %MetreosContribRoot%\ace-5.4\lib\ace.dll	%DeployDir% /Y			>> %PostBuildLog% 
xcopy %MetreosContribRoot%\ace-5.4\lib\ace.pdb	%SymbolDir% /Y			>> %PostBuildLog% 

:done
@echo ** PostBuild done > %PostBuildLog%    

