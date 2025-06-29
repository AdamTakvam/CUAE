@echo off

if "%CUAEWORKSPACE%"=="" set CUAEWORKSPACE=X:
call %CUAEWORKSPACE%\autobuild\SetBuildVariables.bat

setlocal enableextensions

set TestDir=%DeployDir%\FunctionalTest 

set AssemblyPrefix=Metreos.TestBank

set MaxAppGenTool=%FwToolsDir%\MaxAppsGen.exe

REM ******* Create directories *******
if not exist %TestDir%                      		mkdir %TestDir%                                                                         >   PostBuild.txt
if not exist %TestDir%\FunctionalTests      		mkdir %TestDir%\FunctionalTests                                                         >>  Postbuild.txt
if not exist %TestDir%\FunctionalTests\TestNamingAssemblies       	mkdir %TestDir%\FunctionalTests\TestNamingAssemblies                    >>  Postbuild.txt
if not exist %TestDir%\CompiledTests                mkdir %TestDir%\CompiledTests                                                           >>  Postbuild.txt

for /D %%i in (..\TestBank\*) do %MaxAppGenTool% -b:..\TestBank\%%i -n:%AssemblyPrefix%.%%i -f:%TestDir%\FunctionalTests\TestNamingAssemblies\ -o:%%i.cs -t:%%i >> PostBuild.txt
