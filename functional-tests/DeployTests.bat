@echo off

if "%CUAEWORKSPACE%"=="" set CUAEWORKSPACE=X:
call %CUAEWORKSPACE%\autobuild\CreateDeployDirectoryStructure.bat

setlocal enableextensions

set TestDir=%DeployDir%\FunctionalTest

set AssemblyPrefix=Metreos.TestBank

@echo Copy Compiled Tests
for /D %%i in (.\TestBank\ARE\*) do xcopy %%i\bin\*.mca  /Y %TestDir%\CompiledTests\ARE\ >> PostBuild.txt
for /D %%i in (.\TestBank\Core\*) do xcopy %%i\bin\*.mca  /Y %TestDir%\CompiledTests\Core\ >> PostBuild.txt
for /D %%i in (.\TestBank\Max\*) do xcopy %%i\bin\*.mca  /Y %TestDir%\CompiledTests\Max\ >> PostBuild.txt
for /D %%i in (.\TestBank\SMA\*) do xcopy %%i\bin\*.mca /Y %TestDir%\CompiledTests\SMA\ >> Postbuild.txt
for /D %%i in (.\TestBank\Provider\*) do xcopy %%i\bin\*.mca /Y %TestDir%\CompiledTests\Provider\ >> Postbuild.txt
for /D %%i in (.\TestBank\IVT\*) do xcopy %%i\bin\*.mca /Y %TestDir%\CompiledTests\IVT\ >> Postbuild.txt
for /D %%i in (.\TestBank\App\*) do xcopy %%i\bin\*.mca /Y %TestDir%\CompiledTests\App\ >> Postbuild.txt
