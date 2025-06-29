@echo off
rem -- PostBuild.bat
rem -- 
rem -- Implement this script to copy build products from the 
rem -- local module directory to the product-level 'Build' directory
rem --
setlocal
set BuildTarget=%1
if "%BuildTarget%" == "" set BuildTarget=Debug
set configuration=max
if "%BuildTarget%" == "DebugNoMax" set configuration=nomax
if "%BuildTarget%" == "DebugNoMax" set BuildTarget=Debug
if "%BuildTarget%" == "ReleaseNoMax" set configuration=nomax
if "%BuildTarget%" == "ReleaseNoMax" set BuildTarget=Release

rem -- Include 'postbuild-init.cmd'
rem -- Provided vars:
rem --    MetreosWorkspaceRoot   (e.g., X:\)
rem --    MetreosToolsRoot       (e.g., X:\Tools)
rem --    MetreosContribRoot     (e.g., X:\Contrib)
rem --    CLR_gacutil            (fullqualified path of gacutil)
rem
call X:\Tools\build-scripts\postbuild-init.cmd
call %MetreosWorkspaceRoot%\autobuild\CreateDeployDirectoryStructure.bat
rem -- replace 'module-name' with the module directory name
set ProjectRoot=%MetreosWorkspaceRoot%\functional-tests
set PostBuildLog=%ProjectRoot%\postbuild\postbuild.txt
set DeployDir=%MetreosBuildRoot%\FunctionalTest
set SymbolDir=%DeployDir%\Symbols

set AssemblyPrefix=Metreos.FunctionalTests
set TestBankPrefix=Metreos.TestBank
set MaxAppGenTool=%DeployDir%\MaxAppsGen.exe
set Max=%MetreosBuildRoot%\MaxDesigner\MaxDesigner.exe

rem -- Clear post-build log
if exist %PostBuildLog% del %PostBuildLog% /F /Q
@echo ** PostBuild Beginning > %PostBuildLog%   

rem -- Create deploy directory
echo ** Create directories >> %PostBuildLog%
if not exist %DeployDir%                      mkdir %DeployDir%
if not exist %DeployDir%\FunctionalTests      mkdir %DeployDir%\FunctionalTests
if not exist %DeployDir%\TestSupportFiles     mkdir %DeployDir%\TestSupportFiles
if not exist %DeployDir%\AppScripts           mkdir %DeployDir%\AppScripts
if not exist %DeployDir%\CompiledTests        mkdir %DeployDir%\CompiledTests
if not exist %DeployDir%\Installers           mkdir %DeployDir%\Installers
if not exist %DeployDir%\DbScripts            mkdir %DeployDir%\DbScripts
if not exist %DeployDir%\NativeActions        mkdir %DeployDir%\NativeActions
if not exist %DeployDir%\NativeTypes          mkdir %DeployDir%\NativeTypes
if not exist %DeployDir%\Templates            mkdir %DeployDir%\Templates
if not exist %DeployDir%\FunctionalTests\TestNamingAssemblies mkdir %DeployDir%\FunctionalTests\TestNamingAssemblies
if not exist %SymbolDir%                      mkdir %SymbolDir%

@echo ** Copy test database creation script >>  %PostBuildLog%
xcopy %ProjectRoot%\databasesetup.sql %DeployDir%\DbScripts /Y >> %PostBuildLog%	

rem @echo ** Generate Intellisense >> %PostBuildLog%
rem %MaxAppGenTool% -b:%ProjectRoot%\TestBank\ARE -n:%TestBankPrefix%.ARE -f:%DeployDir%\FunctionalTests\TestNamingAssemblies -o:ARE.cs -t:ARE >> %PostBuildLog%
rem %MaxAppGenTool% -b:%ProjectRoot%\TestBank\Core -n:%TestBankPrefix%.Core -f:%DeployDir%\FunctionalTests\TestNamingAssemblies -o:Core.cs -t:Core >> %PostBuildLog%
rem %MaxAppGenTool% -b:%ProjectRoot%\TestBank\Max -n:%TestBankPrefix%.Max -f:%DeployDir%\FunctionalTests\TestNamingAssemblies -o:Max.cs -t:Max >> %PostBuildLog%
rem %MaxAppGenTool% -b:%ProjectRoot%\TestBank\SMA -n:%TestBankPrefix%.SMA -f:%DeployDir%\FunctionalTests\TestNamingAssemblies -o:SMA.cs -t:SMA >> %PostBuildLog%
rem %MaxAppGenTool% -b:%ProjectRoot%\TestBank\Provider -n:%TestBankPrefix%.Provider -f:%DeployDir%\FunctionalTests\TestNamingAssemblies -o:Provider.cs -t:Provider >> %PostBuildLog%
rem %MaxAppGenTool% -b:%ProjectRoot%\TestBank\IVT -n:%TestBankPrefix%.IVT -f:%DeployDir%\FunctionalTests\TestNamingAssemblies -o:IVT.cs -t:IVT >> %PostBuildLog%
rem %MaxAppGenTool% -b:%ProjectRoot%\TestBank\App -n:%TestBankPrefix%.App -f:%DeployDir%\FunctionalTests\TestNamingAssemblies -o:App.cs -t:App >> %PostBuildLog%

@echo ** Copy Functional Tests 	>> %PostBuildLog%																
xcopy %ProjectRoot%\Standard\bin\%BuildTarget%\%AssemblyPrefix%.Standard.dll           %DeployDir%\FunctionalTests /Y >> %PostBuildLog%
xcopy %ProjectRoot%\Regression0.1\bin\%BuildTarget%\%AssemblyPrefix%.Regression0.1.dll %DeployDir%\FunctionalTests /Y >> %PostBuildLog%
xcopy %ProjectRoot%\Regression0.5\bin\%BuildTarget%\%AssemblyPrefix%.Regression0.5.dll %DeployDir%\FunctionalTests /Y >> %PostBuildLog%
xcopy %ProjectRoot%\IVT2.0\bin\%BuildTarget%\%AssemblyPrefix%.IVT2.0.dll               %DeployDir%\FunctionalTests /Y >> %PostBuildLog%
REM xcopy ..\Application\bin\%BuildTarget%\%AssemblyPrefix%.Application.dll                %DeployDir%\FunctionalTests /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\bin\%BuildTarget%\Metreos.Native.FunctionalTests.dll %DeployDir%\NativeActions /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\bin\%BuildTarget%\Metreos.Types.FunctionalTests.dll    %DeployDir%\NativeTypes /Y >> %PostBuildLog%

if "%configuration%" == "nomax" goto nomax

@echo ** Compile Tests >> %PostBuildLog%

for /D %%i in (%ProjectRoot%\TestBank\ARE\*)  do %Max% %%i\%%~ni.max /b  >> %PostBuildLog%
for /D %%i in (%ProjectRoot%\TestBank\Core\*) do %Max% %%i\%%~ni.max /b  >> %PostBuildLog%
for /D %%i in (%ProjectRoot%\TestBank\Max\*)  do %Max% %%i\%%~ni.max /b  >> %PostBuildLog%
for /D %%i in (%ProjectRoot%\TestBank\SMA\*)  do %Max% %%i\%%~ni.max /b  >> %PostBuildLog%
for /D %%i in (%ProjectRoot%\TestBank\Provider\*) do %Max% %%i\%%~ni.max /b >> %PostBuildLog%
for /D %%i in (%ProjectRoot%\TestBank\IVT\*)  do %Max% %%i\%%~ni.max /b  >> %PostBuildLog%
for /D %%i in (%ProjectRoot%\TestBank\App\*)  do %Max% %%i\%%~ni.max /b  >> %PostBuildLog%

:nomax

@echo Copy Compiled Tests >> %PostBuildLog%
for /D %%i in (%ProjectRoot%\TestBank\ARE\*)  do xcopy %%i\bin\*.mca %DeployDir%\CompiledTests\ARE\  /Y >> %PostBuildLog%
for /D %%i in (%ProjectRoot%\TestBank\Core\*) do xcopy %%i\bin\*.mca %DeployDir%\CompiledTests\Core\ /Y >> %PostBuildLog%
for /D %%i in (%ProjectRoot%\TestBank\Max\*)  do xcopy %%i\bin\*.mca %DeployDir%\CompiledTests\Max\  /Y >> %PostBuildLog%
for /D %%i in (%ProjectRoot%\TestBank\SMA\*)  do xcopy %%i\bin\*.mca %DeployDir%\CompiledTests\SMA\  /Y >> %PostBuildLog%
for /D %%i in (%ProjectRoot%\TestBank\Provider\*) do xcopy %%i\bin\*.mca %DeployDir%\CompiledTests\Provider\ /Y >> %PostBuildLog%
for /D %%i in (%ProjectRoot%\TestBank\IVT\*)  do xcopy %%i\bin\*.mca %DeployDir%\CompiledTests\IVT\  /Y >> %PostBuildLog%
for /D %%i in (%ProjectRoot%\TestBank\App\*)  do xcopy %%i\bin\*.mca %DeployDir%\CompiledTests\App\  /Y >> %PostBuildLog%

rem -- Install debug symbols
if /i NOT "%BuildTarget%"=="debug" if /i NOT "%BuildTarget%"=="debugmethodcalltrace" goto removePdb
rem @echo ** Installing debug symbols                   >> %PostBuildLog%

goto done
rem -- Remove debug symbol files if not a debug build
:removePdb
rem @echo ** Removing debug symbols                     >> %PostBuildLog%
rem if exist %DeployDir%\*.pdb del /s %DeployDir%\*.pdb	>> %PostBuildLog%
goto done

:usage
echo Usage: PostBuild.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).
goto done

:done
echo ** PostBuild Complete                         >> %PostBuildLog%
type %PostBuildLog%
