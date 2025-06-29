@echo off

if "%CUAEWORKSPACE%"=="" CUAEWORKSPACE=X:
call %CUAEWORKSPACE%\autobuild\CreateDeployDirectoryStructure.bat

setlocal enableextensions
set ProjectRoot=%CUAEWORKSPACE%\functional-tests
rem set TestDir=%MetreosWorkspaceRoot%\Build\FunctionalTest
set TestDir=%DeployDir%\FunctionalTest

set AssemblyPrefix=Metreos.TestBank

set MaxAppGenTool=%TestDir%\MaxAppsGen.exe

REM ******* Create directories *******
if not exist %TestDir%                      		mkdir %TestDir%                                                                         >   PostBuild.txt
if not exist %TestDir%\Templates                    mkdir %TestDir%\Templates																>>  Postbuild.txt
if not exist %TestDir%\FunctionalTests      		mkdir %TestDir%\FunctionalTests                                                         >>  Postbuild.txt
if not exist %TestDir%\FunctionalTests\TestNamingAssemblies       	mkdir %TestDir%\FunctionalTests\TestNamingAssemblies                    >>  Postbuild.txt 

echo ****** ARE Test Intellisense File Being Generated ******
%MaxAppGenTool% -b:%ProjectRoot%\TestBank\ARE -n:%AssemblyPrefix%.ARE -f:%TestDir%\FunctionalTests\TestNamingAssemblies\ -o:ARE.cs -t:ARE >> PostBuild.txt

echo ****** Core Test Intellisense File Being Generated ******
%MaxAppGenTool% -b:%ProjectRoot%\TestBank\Core -n:%AssemblyPrefix%.Core -f:%TestDir%\FunctionalTests\TestNamingAssemblies\ -o:Core.cs -t:Core >> PostBuild.txt

echo ****** Max Test Intellisense File Being Generated ******
%MaxAppGenTool% -b:%ProjectRoot%\TestBank\Max -n:%AssemblyPrefix%.Max -f:%TestDir%\FunctionalTests\TestNamingAssemblies\ -o:Max.cs -t:Max >> PostBuild.txt

echo ****** SMA Test Intellisense File Being Generated ******
%MaxAppGenTool% -b:%ProjectRoot%\TestBank\SMA -n:%AssemblyPrefix%.SMA -f:%TestDir%\FunctionalTests\TestNamingAssemblies\ -o:SMA.cs -t:SMA >> PostBuild.txt

echo ****** Provider Test Intellisense File Being Generated ******
%MaxAppGenTool% -b:%ProjectRoot%\TestBank\Provider -n:%AssemblyPrefix%.Provider -f:%TestDir%\FunctionalTests\TestNamingAssemblies\ -o:Provider.cs -t:Provider >> PostBuild.txt

echo ****** IVT Test Intellisense File Being Generated ******
%MaxAppGenTool% -b:%ProjectRoot%\TestBank\IVT -n:%AssemblyPrefix%.IVT -f:%TestDir%\FunctionalTests\TestNamingAssemblies\ -o:IVT.cs -t:IVT >> PostBuild.txt

echo ****** Application Test Intellisense File Being Generated ******
%MaxAppGenTool% -b:%ProjectRoot%\TestBank\App -n:%AssemblyPrefix%.App -f:%TestDir%\FunctionalTests\TestNamingAssemblies\ -o:App.cs -t:App >> PostBuild.txt

REM for /D %%i in (..\TestBank\*) do %MaxAppGenTool% -b:..\TestBank\%%i -n:%AssemblyPrefix%.%%i -f:%TestDir%\FunctionalTests\TestNamingAssemblies\ -o:%%i.cs -t:%%i >> PostBuild.txt
