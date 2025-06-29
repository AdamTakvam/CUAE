@echo off
rem -- PostBuild.bat
rem -- 
rem -- Implement this script to copy build products from the 
rem -- local module directory to the product-level 'Build' directory
rem --
setlocal
set BuildTarget=%1
if "%BuildTarget%" == "" set BuildTarget=Debug

rem -- Include 'postbuild-init.cmd'
rem -- Provided vars:
rem --    MetreosWorkspaceRoot   (e.g., X:\)
rem --    MetreosToolsRoot       (e.g., X:\Tools)
rem --    MetreosContribRoot     (e.g., X:\Contrib)
rem --    CLR_%CLR_gacutil%           (fullqualified path of gacutil)
rem
if "%CUAEWORKSPACE%"=="" set CUAEWORKSPACE=X:
call %CUAEWORKSPACE%\Tools\build-scripts\postbuild-init.cmd
call %MetreosWorkspaceRoot%\autobuild\CreateDeployDirectoryStructure.bat
set ProjectRoot=%MetreosWorkspaceRoot%\functional-tests-framework
set PostBuildLog=%ProjectRoot%\postbuild\postbuild.txt
set DeployDir=%MetreosBuildRoot%\FunctionalTest
set AssemblyPrefix=Metreos.Samoa
set IntellisenseRoot=%MetreosWorkspaceRoot%\functional-tests
set SymbolDir=%DeployDir%\Symbols

rem -- Clear post-build log
if exist %PostBuildLog% del %PostBuildLog% /F /Q
@echo ** PostBuild Beginning > %PostBuildLog%    

rem -- Create deploy directory
if not exist %DeployDir%           mkdir %DeployDir%
if not exist %DeployDir%\Templates mkdir %DeployDir%\Templates
if not exist %DeployDir%\Providers mkdir %DeployDir%\Providers
if not exist %SymbolDir%           mkdir %SymbolDir%

@echo ** Removing shadow copies of dlls >> %PostBuildLog%
%CLR_gacutil% /cdl >> %PostBuildLog%

@echo ** Removing framework assemblies from GAC >> %PostBuildLog%
%CLR_gacutil% /uf %AssemblyPrefix%.FunctionalTestFramework >> %PostBuildLog%

@echo ** Posting Framework assemblies >> %PostBuildLog%
xcopy %ProjectRoot%\TestFramework\bin\%BuildTarget%\%AssemblyPrefix%.FunctionalTestFramework.dll %DeployDir% /Y >> %PostBuildLog%

xcopy %ProjectRoot%\FunctionalTestProvider\bin\%BuildTarget%\Metreos.Providers.FunctionalTest.dll %DeployDir%\Providers /Y >> %PostBuildLog%
xcopy %ProjectRoot%\FunctionalTestProvider\bin\%BuildTarget%\Metreos.Providers.FunctionalTest.pdb %SymbolDir% /Y >> %PostBuildLog%

@echo ** Installing functional test executables >> %PostBuildLog%
xcopy %ProjectRoot%\Runtime\bin\%BuildTarget%\FunctionalTestRuntime.dll     %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\WindowedRuntime\bin\%BuildTarget%\FTWindowed.exe        %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\ConsoleRuntime\bin\%BuildTarget%\FTConsole.exe          %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\WindowedRuntime\bin\%BuildTarget%\FTWindowed.exe.config %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\ConsoleRuntime\bin\%BuildTarget%\FTConsole.exe.config   %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\WindowedRuntime\bin\%BuildTarget%\FTWindowed.pdb        %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\ConsoleRuntime\bin\%BuildTarget%\FTConsole.pdb          %SymbolDir% /Y >> %PostBuildLog%

@echo ** Creating FTProvider package >> %PostBuildLog%
%PgenTool% -y -src:"%DeployDir%\Providers\Metreos.Providers.FunctionalTest.dll" -dest:"%DeployDir%\Providers" -ref:%DeployDir%\%AssemblyPrefix%.FunctionalTestFramework.dll >> %PostBuildLog%

@echo ** Copy Test Call Control Provider >> %PostBuildLog%
xcopy %ProjectRoot%\CallControlProvider\bin\%BuildTarget%\Metreos.Providers.TestCallControl.dll %DeployDir%\Providers /Y >> %PostBuildLog%
xcopy %ProjectRoot%\CallControlProvider\bin\%BuildTarget%\Metreos.Providers.TestCallControl.pdb %SymbolDir% /Y >> %PostBuildLog%
%PgenTool% -y -src:"%DeployDir%\Providers\Metreos.Providers.TestCallControl.dll" -dest:"%DeployDir%\Providers" >> %PostBuildLog%

@echo ** Copy XML packages to framework package dir .. do not want to, but must .. sigh
xcopy /Y %DeployDir%\Providers\*.xml %M_FrameworkDir%\Packages

@echo ** Copy MaxAppGen Tool and templates >> %PostBuildLog%
xcopy %ProjectRoot%\MaxAppsGen\bin\%BuildTarget%\MaxAppsGen.exe         %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\MaxTestGen\bin\%BuildTarget%\maketest.exe           %DeployDir% /Y >> %PostBuildLog%		
xcopy %ProjectRoot%\MaxAppsGen\bin\%BuildTarget%\MaxAppsGen.exe.config  %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\MaxTestGen\bin\%BuildTarget%\maketest.exe.config    %DeployDir% /Y >> %PostBuildLog%		
xcopy %ProjectRoot%\MaxAppsGen\bin\%BuildTarget%\MaxAppsGen.pdb         %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\MaxTestGen\bin\%BuildTarget%\maketest.pdb           %SymbolDir% /Y >> %PostBuildLog%		

xcopy %ProjectRoot%\TestFramework\Templates\*                 %DeployDir%\Templates /Y >> %PostBuildLog%

rem call "%ProjectRoot%\CreateIntellisense.bat"
rem call "%IntellisenseRoot%\CreateIntellisense.bat"

@echo ** Generating FunctionalTest MCP Package >> %PostBuildLog%
%McpTool% "%DeployDir%\Providers\Metreos.Providers.FunctionalTest.dll" -o:"%DeployDir%\Providers\Metreos.Providers.FunctionalTest.mcp" -r:"%DeployDir%\Metreos.samoa.FunctionalTestFramework.dll" >> %PostBuildLog%

@echo ** Generating TestCallControl MCP Package >> %PostBuildLog%
%McpTool% "%DeployDir%\Providers\Metreos.Providers.TestCallControl.dll" -o:"%DeployDir%\Providers\Metreos.Providers.TestCallControl.mcp" -r:"%DeployDir%\Metreos.samoa.FunctionalTestFramework.dll" >> %PostBuildLog%

xcopy %ProjectRoot%\TestFramework\bin\%BuildTarget%\%AssemblyPrefix%.FunctionalTestFramework.pdb %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\Runtime\bin\%BuildTarget%\FunctionalTestRuntime.pdb                          %SymbolDir% /Y >> %PostBuildLog%

rem -- Install debug symbols
if /i NOT "%BuildTarget%"=="debug" if /i NOT "%BuildTarget%"=="debugmethodcalltrace" goto removePdb
@echo ** Installing debug symbols                   >> %PostBuildLog%

goto done
rem -- Remove debug symbol files if not a debug build
:removePdb
@echo ** Removing debug symbols                     >> %PostBuildLog%
goto done

:usage
echo Usage: PostBuild.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).
goto done

:done
echo ** PostBuild Complete                         >> %PostBuildLog%
type %PostBuildLog%
