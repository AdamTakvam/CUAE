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
set ProjectRoot=%MetreosWorkspaceRoot%\csharp-framework
set PostBuildLog=%ProjectRoot%\PostBuild\postbuild.txt
set SymbolDir=%MetreosWorkspaceRoot%\Build\Framework\1.0\Symbols

set AssemblyPrefix=Metreos

if not exist %SymbolDir% mkdir %SymbolDir%\

rem -- Clear post-build log
if exist %PostBuildLog% del %PostBuildLog% /F /Q
@echo ** PostBuild Beginning > %PostBuildLog%    

REM Clear shadow copies of dlls
%CLR_gacutil% /cdl >> %PostBuildLog%

@echo ****** Removing framework assemblies from GAC ****** >> %PostBuildLog%
%CLR_gacutil% /uf %AssemblyPrefix%.StatsClient             >> %PostBuildLog%
%CLR_gacutil% /uf %AssemblyPrefix%.LogSinks                >> %PostBuildLog%
%CLR_gacutil% /uf %AssemblyPrefix%.AppArchiveCore          >> %PostBuildLog%
%CLR_gacutil% /uf %AssemblyPrefix%.ApplicationFramework    >> %PostBuildLog%
%CLR_gacutil% /uf %AssemblyPrefix%.Configuration           >> %PostBuildLog%
%CLR_gacutil% /uf %AssemblyPrefix%.Core                    >> %PostBuildLog%
%CLR_gacutil% /uf %AssemblyPrefix%.DocGeneratorCore        >> %PostBuildLog%
%CLR_gacutil% /uf %AssemblyPrefix%.Interfaces              >> %PostBuildLog%
%CLR_gacutil% /uf %AssemblyPrefix%.LicensingFramework      >> %PostBuildLog%
%CLR_gacutil% /uf %AssemblyPrefix%.LoggingFramework        >> %PostBuildLog%
%CLR_gacutil% /uf %AssemblyPrefix%.Messaging               >> %PostBuildLog%
%CLR_gacutil% /uf %AssemblyPrefix%.PackageGeneratorCore    >> %PostBuildLog%
%CLR_gacutil% /uf %AssemblyPrefix%.ProviderPackagerCore    >> %PostBuildLog%
%CLR_gacutil% /uf %AssemblyPrefix%.ProviderFramework       >> %PostBuildLog%
%CLR_gacutil% /uf %AssemblyPrefix%.DebugFramework          >> %PostBuildLog%
%CLR_gacutil% /uf %AssemblyPrefix%.Utilities               >> %PostBuildLog%
%CLR_gacutil% /uf %AssemblyPrefix%.WebServicesConsumerCore >> %PostBuildLog%
%CLR_gacutil% /uf %AssemblyPrefix%.AxlSoap                 >> %PostBuildLog%
%CLR_gacutil% /uf %AssemblyPrefix%.SccpStack               >> %PostBuildLog%
%CLR_gacutil% /uf SecureBlackbox                           >> %PostBuildLog%
%CLR_gacutil% /uf SecureBlackbox.PKI                       >> %PostBuildLog%
%CLR_gacutil% /uf SecureBlackbox.SFTP                      >> %PostBuildLog%
%CLR_gacutil% /uf SecureBlackbox.SFTPCommon                >> %PostBuildLog%
%CLR_gacutil% /uf SecureBlackbox.SFTPServer                >> %PostBuildLog%
%CLR_gacutil% /uf SecureBlackbox.SSHCommon                 >> %PostBuildLog%
%CLR_gacutil% /uf SecureBlackbox.SSHClient                 >> %PostBuildLog%
%CLR_gacutil% /uf SecureBlackbox.SSHServer                 >> %PostBuildLog%
%CLR_gacutil% /uf Novell.Directory.Ldap                    >> %PostBuildLog%
%CLR_gacutil% /uf MySql.Data                               >> %PostBuildLog%
%CLR_gacutil% /uf nsoftware.System                         >> %PostBuildLog%
%CLR_gacutil% /uf nsoftware.IPWorks                        >> %PostBuildLog%
%CLR_gacutil% /uf nsoftware.IPWorksSSL                     >> %PostBuildLog%
%CLR_gacutil% /uf nsoftware.IPWorksZip                     >> %PostBuildLog%
%CLR_gacutil% /uf ICSharpCode.SharpZipLib                  >> %PostBuildLog%

@echo ****** Installing common Framework assemblies ****** >> %PostBuildLog%
xcopy %ProjectRoot%\StatsClient\bin\%BuildTarget%\%AssemblyPrefix%.StatsClient.dll                          %FwCoreDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\LogSinks\bin\%BuildTarget%\%AssemblyPrefix%.LogSinks.dll                                %FwCoreDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\AppPackageCore\bin\%BuildTarget%\%AssemblyPrefix%.AppArchiveCore.dll                    %FwCoreDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\ApplicationFramework\bin\%BuildTarget%\%AssemblyPrefix%.ApplicationFramework.dll        %FwCoreDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\Configuration\bin\%BuildTarget%\%AssemblyPrefix%.Configuration.dll                      %FwCoreDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\Core\bin\%BuildTarget%\%AssemblyPrefix%.Core.dll                                        %FwCoreDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\DocGeneratorCore\bin\%BuildTarget%\%AssemblyPrefix%.DocGeneratorCore.dll                %FwCoreDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\Interfaces\bin\%BuildTarget%\%AssemblyPrefix%.Interfaces.dll                            %FwCoreDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\LicensingFramework\bin\%BuildTarget%\%AssemblyPrefix%.LicensingFramework.dll            %FwCoreDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\LoggingFramework\bin\%BuildTarget%\%AssemblyPrefix%.LoggingFramework.dll                %FwCoreDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\Messaging\bin\%BuildTarget%\%AssemblyPrefix%.Messaging.dll                              %FwCoreDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\PackageGeneratorCore\bin\%BuildTarget%\%AssemblyPrefix%.PackageGeneratorCore.dll        %FwCoreDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\ProviderPackagerCore\bin\%BuildTarget%\%AssemblyPrefix%.ProviderPackagerCore.dll        %FwCoreDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\ProviderFramework\bin\%BuildTarget%\%AssemblyPrefix%.ProviderFramework.dll              %FwCoreDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\DebugFramework\bin\%BuildTarget%\%AssemblyPrefix%.DebugFramework.dll                    %FwCoreDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\Utilities\bin\%BuildTarget%\%AssemblyPrefix%.Utilities.dll                              %FwCoreDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\WebServicesConsumerCore\bin\%BuildTarget%\%AssemblyPrefix%.WebServicesConsumerCore.dll  %FwCoreDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\AxlSoap\bin\%BuildTarget%\%AssemblyPrefix%.AxlSoap.dll                                  %FwCoreDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\SccpStack\bin\%BuildTarget%\%AssemblyPrefix%.SccpStack.dll                              %FwCoreDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\SecureBlackbox.NET\SecureBlackbox.dll                                            %FwCoreDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\SecureBlackbox.NET\SecureBlackbox.PKI.dll                                        %FwCoreDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\SecureBlackbox.NET\SecureBlackbox.SFTP.dll                                       %FwCoreDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\SecureBlackbox.NET\SecureBlackbox.SFTPCommon.dll                                 %FwCoreDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\SecureBlackbox.NET\SecureBlackbox.SFTPServer.dll                                 %FwCoreDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\SecureBlackbox.NET\SecureBlackbox.SSHCommon.dll                                  %FwCoreDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\SecureBlackbox.NET\SecureBlackbox.SSHClient.dll                                  %FwCoreDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\SecureBlackbox.NET\SecureBlackbox.SSHServer.dll                                  %FwCoreDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\Novell\Novell.Directory.Ldap.dll                                                 %FwCoreDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\MySQL\MySql.Data.dll											                    %FwCoreDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\IPWorks\nsoftware.System.dll									                    %FwCoreDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\IPWorks\nsoftware.IPWorks.dll								                    %FwCoreDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\IPWorks\nsoftware.IPWorksSSL.dll								                    %FwCoreDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\IPWorks\nsoftware.IPWorksZip.dll								                    %FwCoreDir% /Y >> %PostBuildLog%
xcopy %MetreosContribRoot%\SharpZipLib\ICSharpCode.SharpZipLib.dll  					                    %FwCoreDir% /Y >> %PostBuildLog%

xcopy %ProjectRoot%\UnitTest\bin\%BuildTarget%\%AssemblyPrefix%.UnitTest.dll %UnitTestDir% /Y >> %PostBuildLog%

xcopy %ProjectRoot%\StatsClient\bin\%BuildTarget%\%AssemblyPrefix%.StatsClient.pdb                          %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\LogSinks\bin\%BuildTarget%\%AssemblyPrefix%.LogSinks.pdb                                %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\AppPackageCore\bin\%BuildTarget%\%AssemblyPrefix%.AppArchiveCore.pdb                    %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\ApplicationFramework\bin\%BuildTarget%\%AssemblyPrefix%.ApplicationFramework.pdb        %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\Configuration\bin\%BuildTarget%\%AssemblyPrefix%.Configuration.pdb                      %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\Core\bin\%BuildTarget%\%AssemblyPrefix%.Core.pdb                                        %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\DocGeneratorCore\bin\%BuildTarget%\%AssemblyPrefix%.DocGeneratorCore.pdb                %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\Interfaces\bin\%BuildTarget%\%AssemblyPrefix%.Interfaces.pdb                            %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\LicensingFramework\bin\%BuildTarget%\%AssemblyPrefix%.LicensingFramework.pdb            %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\LoggingFramework\bin\%BuildTarget%\%AssemblyPrefix%.LoggingFramework.pdb                %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\Messaging\bin\%BuildTarget%\%AssemblyPrefix%.Messaging.pdb                              %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\PackageGeneratorCore\bin\%BuildTarget%\%AssemblyPrefix%.PackageGeneratorCore.pdb        %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\ProviderPackagerCore\bin\%BuildTarget%\%AssemblyPrefix%.ProviderPackagerCore.pdb        %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\ProviderFramework\bin\%BuildTarget%\%AssemblyPrefix%.ProviderFramework.pdb              %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\DebugFramework\bin\%BuildTarget%\%AssemblyPrefix%.DebugFramework.pdb                    %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\Utilities\bin\%BuildTarget%\%AssemblyPrefix%.Utilities.pdb                              %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\WebServicesConsumerCore\bin\%BuildTarget%\%AssemblyPrefix%.WebServicesConsumerCore.pdb  %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\AxlSoap\bin\%BuildTarget%\%AssemblyPrefix%.AxlSoap.pdb                                  %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\SccpStack\bin\%BuildTarget%\%AssemblyPrefix%.SccpStack.pdb                              %SymbolDir% /Y >> %PostBuildLog%

%CLR_gacutil% /i %FwCoreDir%\%AssemblyPrefix%.StatsClient.dll             >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\%AssemblyPrefix%.LogSinks.dll                >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\%AssemblyPrefix%.AppArchiveCore.dll          >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\%AssemblyPrefix%.ApplicationFramework.dll    >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\%AssemblyPrefix%.Configuration.dll           >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\%AssemblyPrefix%.Core.dll                    >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\%AssemblyPrefix%.DocGeneratorCore.dll        >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\%AssemblyPrefix%.Interfaces.dll              >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\%AssemblyPrefix%.LicensingFramework.dll      >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\%AssemblyPrefix%.LoggingFramework.dll        >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\%AssemblyPrefix%.Messaging.dll               >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\%AssemblyPrefix%.PackageGeneratorCore.dll    >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\%AssemblyPrefix%.ProviderPackagerCore.dll    >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\%AssemblyPrefix%.ProviderFramework.dll       >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\%AssemblyPrefix%.DebugFramework.dll          >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\%AssemblyPrefix%.Utilities.dll               >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\%AssemblyPrefix%.WebServicesConsumerCore.dll >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\%AssemblyPrefix%.AxlSoap.dll                 >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\%AssemblyPrefix%.SccpStack.dll               >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\SecureBlackbox.dll                           >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\SecureBlackbox.PKI.dll                       >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\SecureBlackbox.SFTP.dll                      >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\SecureBlackbox.SFTPCommon.dll                >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\SecureBlackbox.SFTPServer.dll                >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\SecureBlackbox.SSHCommon.dll                 >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\SecureBlackbox.SSHClient.dll                 >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\SecureBlackbox.SSHServer.dll                 >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\Novell.Directory.Ldap.dll                    >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\ICSharpCode.SharpZipLib.dll                  >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\ChilkatDotNet.dll                            >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\MySql.Data.dll                               >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\nsoftware.System.dll                         >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\nsoftware.IPWorks.dll                        >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\nsoftware.IPWorksSSL.dll                     >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\nsoftware.IPWorksZip.dll                     >> %PostBuildLog%
%CLR_gacutil% /i %FwCoreDir%\ICSharpCode.SharpZipLib.dll                  >> %PostBuildLog%

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
