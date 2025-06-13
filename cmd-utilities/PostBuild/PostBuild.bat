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
rem --    CLR_gacutil            (fullqualified path of gacutil)
rem
if "%CUAEWORKSPACE%"=="" set CUAEWORKSPACE=X:
call %CUAEWORKSPACE%\Tools\build-scripts\postbuild-init.cmd
call %MetreosWorkspaceRoot%\autobuild\CreateDeployDirectoryStructure.bat
set ProjectRoot=%MetreosWorkspaceRoot%\cmd-utilities
set PostBuildLog=%ProjectRoot%\postbuild\postbuild.txt
set AssemblyPrefix=Metreos
set DeployDir=%FwToolsDir%

rem -- Clear post-build log
if exist %PostBuildLog% del %PostBuildLog% /F /Q
@echo ** PostBuild Beginning > %PostBuildLog%    

rem -- Create deploy directory
if not exist %DeployDir% mkdir %DeployDir%

rem -- Install files
@echo ** Installing files in deployment directory >> %PostBuildLog%

REM Clear shadow copies of dlls
%CLR_gacutil% /cdl >> %PostBuildLog%

xcopy %ProjectRoot%\PackageGenerator\bin\%BuildTarget%\pgen.exe        %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\PackageGenerator\bin\%BuildTarget%\pgen.exe.config %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\AppArchiver\bin\%BuildTarget%\mca.exe              %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\AppArchiver\bin\%BuildTarget%\mca.exe.config       %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\RemoteConsoleViewer\bin\%BuildTarget%\rconsole.exe %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\SftpConsoleClient\bin\%BuildTarget%\msftp.exe	   %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\WebServicesConsumer\bin\%BuildTarget%\mws.exe      %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\AppDeployTool\bin\%BuildTarget%\mcadeploy.exe      %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\CypherUtil\bin\%BuildTarget%\mcypher.exe           %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\InstallProvider\bin\%BuildTarget%\instprov.exe     %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\MibGenerator\bin\%BuildTarget%\mibgen.exe          %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\MibGenerator\bin\%BuildTarget%\mibgen.exe.config   %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\ProviderPackager\bin\%BuildTarget%\mcp.exe         %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\DocGenerator\bin\%BuildTarget%\docgen.exe          %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\DebugConsole\bin\%BuildTarget%\mdbg.exe            %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\LicenseValidator\bin\%BuildTarget%\LicenseValidator.exe %DeployDir% /Y >> %PostBuildLog%

rem -- Create MIB file
if not exist %M_FrameworkDir%\MIB mkdir %M_FrameworkDir%\MIB                          >> %PostBuildLog%
pushd %cd%
cd %DeployDir%
%DeployDir%\mibgen.exe -t:%M_FrameworkDir%\MIB                                        >> %PostBuildLog%
popd

rem -- Copy xslt file to framework directory
if not exist %M_FrameworkDir%\XSLT mkdir %M_FrameworkDir%\XSLT                        >> %PostBuildLog%
xcopy %ProjectRoot%\DocGenerator\xslt\package2docbook.xsl         %DeployDir%\XSLT /Y >> %PostBuildLog%

rem -- Install debug symbols
if /i NOT "%BuildTarget%"=="debug" if /i NOT "%BuildTarget%"=="debugmethodcalltrace" goto removePdb
@echo ** Install debug symbols >> %PostBuildLog%

xcopy %ProjectRoot%\PackageGenerator\bin\%BuildTarget%\pgen.pdb        %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\AppArchiver\bin\%BuildTarget%\mca.pdb              %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\RemoteConsoleViewer\bin\%BuildTarget%\rconsole.pdb %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\SftpConsoleClient\bin\%BuildTarget%\msftp.pdb	   %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\WebServicesConsumer\bin\%BuildTarget%\mws.pdb      %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\AppDeployTool\bin\%BuildTarget%\mcadeploy.pdb      %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\CypherUtil\bin\%BuildTarget%\mcypher.pdb           %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\InstallProvider\bin\%BuildTarget%\instprov.pdb     %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\MibGenerator\bin\%BuildTarget%\mibgen.pdb          %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\ProviderPackager\bin\%BuildTarget%\mcp.pdb         %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\DocGenerator\bin\%BuildTarget%\docgen.pdb          %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\DebugConsole\bin\%BuildTarget%\mdbg.pdb            %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\LicenseValidator\bin\%BuildTarget%\LicenseValidator.pdb    %DeployDir% /Y >> %PostBuildLog%

goto done
rem -- Remove debug symbol files if not a debug build
:removePdb
@echo ** Removing debug symbols                     >> %PostBuildLog%
if exist %DeployDir%\*.pdb del /s %DeployDir%\*.pdb	>> %PostBuildLog%
goto done

:usage
echo Usage: PostBuild.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).
goto done

:done
@echo ** PostBuild Complete                         >> %PostBuildLog%
type %PostBuildLog%
