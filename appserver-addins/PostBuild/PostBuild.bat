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
set ProjectRoot=%MetreosWorkspaceRoot%\appserver-addins
set PostBuildLog=%ProjectRoot%\postbuild\postbuild.txt
set SymbolDir=%MetreosWorkspaceRoot%\Build\Framework\1.0\Symbols

set ProviderPrefix=Metreos.Providers
set CCProviderPrefix=Metreos.CallControl
set ActionPrefix=Metreos.Native
set TypePrefix=Metreos.Types

if not exist %SymbolDir% mkdir %SymbolDir%\

rem set DeployDir=??

rem -- Clear post-build log
if exist %PostBuildLog% del %PostBuildLog% /F /Q
@echo ** PostBuild Beginning > %PostBuildLog%    

rem -- Create deploy directory
rem if not exist %DeployDir%           mkdir %DeployDir%

rem -- Install files
@echo ** Installing files in deployment directory   >> %PostBuildLog%

REM Clear shadow copies of dlls
%CLR_gacutil% /cdl >> %PostBuildLog%

REM ****** Copy native action dll's *******
xcopy %ProjectRoot%\NativeActions\ApplicationControl\bin\%BuildTarget%\Metreos.ApplicationControl.dll  %FwActionDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\CiscoDeviceList\bin\%BuildTarget%\%ActionPrefix%.CiscoDeviceList.dll %FwActionDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\CiscoIpPhone\bin\%BuildTarget%\%ActionPrefix%.CiscoIpPhone.dll       %FwActionDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\Conditional\bin\%BuildTarget%\%ActionPrefix%.Conditional.dll         %FwActionDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\DialPlan\bin\%BuildTarget%\%ActionPrefix%.DialPlan.dll               %FwActionDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\Log\bin\%BuildTarget%\%ActionPrefix%.Log.dll                         %FwActionDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\Mail\bin\%BuildTarget%\%ActionPrefix%.Mail.dll                       %FwActionDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\Database\bin\%BuildTarget%\%ActionPrefix%.Database.dll               %FwActionDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\ImageBuilder\bin\%BuildTarget%\%ActionPrefix%.ImageBuilder.dll       %FwActionDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\CiscoExtensionMobility\bin\%BuildTarget%\%ActionPrefix%.CiscoExtensionMobility.dll %FwActionDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\CiscoExtensionMobility\bin\%BuildTarget%\%ActionPrefix%.CiscoExtensionMobility.dll %FwActionDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\AxlSoap\3.3.3\bin\%BuildTarget%\%ActionPrefix%.AxlSoap333.dll        %FwActionDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\AxlSoap\4.1.3\bin\%BuildTarget%\%ActionPrefix%.AxlSoap413.dll        %FwActionDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\AxlSoap\5.0.4\bin\%BuildTarget%\%ActionPrefix%.AxlSoap504.dll        %FwActionDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\AxlSoap\6.0.1\bin\%BuildTarget%\%ActionPrefix%.AxlSoap601.dll        %FwActionDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\Ldap\bin\%BuildTarget%\%ActionPrefix%.Ldap.dll                       %FwActionDir% /Y >> %PostBuildLog%

REM ******* Copy native type dll's *******
xcopy %ProjectRoot%\NativeTypes\CiscoIPPhoneXml\bin\%BuildTarget%\%TypePrefix%.CiscoIpPhone.dll    %FwTypeDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\Standard\bin\%BuildTarget%\%TypePrefix%.dll                        %FwTypeDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\Database\bin\%BuildTarget%\%TypePrefix%.Database.dll               %FwTypeDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\Http\bin\%BuildTarget%\%TypePrefix%.Http.dll                       %FwTypeDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\Imaging\bin\%BuildTarget%\%TypePrefix%.Imaging.dll                 %FwTypeDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\CiscoExtensionMobility\bin\%BuildTarget%\%TypePrefix%.CiscoExtensionMobility.dll %FwTypeDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\AxlSoap\3.3.3\bin\%BuildTarget%\%TypePrefix%.AxlSoap333.dll        %FwTypeDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\AxlSoap\4.1.3\bin\%BuildTarget%\%TypePrefix%.AxlSoap413.dll        %FwTypeDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\AxlSoap\5.0.4\bin\%BuildTarget%\%TypePrefix%.AxlSoap504.dll        %FwTypeDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\AxlSoap\6.0.1\bin\%BuildTarget%\%TypePrefix%.AxlSoap601.dll        %FwTypeDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\CiscoDeviceList\bin\%BuildTarget%\%TypePrefix%.CiscoDeviceList.dll %FwTypeDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\Presence\bin\%BuildTarget%\%TypePrefix%.Presence.dll               %FwTypeDir% /Y >> %PostBuildLog%

REM ******* Copy PDB's *******
xcopy %ProjectRoot%\NativeActions\ApplicationControl\bin\%BuildTarget%\Metreos.ApplicationControl.pdb  %SymbolDir% /Y >> %PostBuildLog% 
xcopy %ProjectRoot%\NativeActions\CiscoDeviceList\bin\%BuildTarget%\%ActionPrefix%.CiscoDeviceList.pdb %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\CiscoIpPhone\bin\%BuildTarget%\%ActionPrefix%.CiscoIpPhone.pdb       %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\Conditional\bin\%BuildTarget%\%ActionPrefix%.Conditional.pdb         %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\DialPlan\bin\%BuildTarget%\%ActionPrefix%.DialPlan.pdb               %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\Log\bin\%BuildTarget%\%ActionPrefix%.Log.pdb                         %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\Mail\bin\%BuildTarget%\%ActionPrefix%.Mail.pdb                       %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\Database\bin\%BuildTarget%\%ActionPrefix%.Database.pdb               %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\ImageBuilder\bin\%BuildTarget%\%ActionPrefix%.ImageBuilder.pdb       %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\CiscoExtensionMobility\bin\%BuildTarget%\%ActionPrefix%.CiscoExtensionMobility.pdb %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\AxlSoap\3.3.3\bin\%BuildTarget%\%ActionPrefix%.AxlSoap333.pdb %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\AxlSoap\4.1.3\bin\%BuildTarget%\%ActionPrefix%.AxlSoap413.pdb %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\AxlSoap\5.0.4\bin\%BuildTarget%\%ActionPrefix%.AxlSoap504.pdb %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeActions\AxlSoap\6.0.1\bin\%BuildTarget%\%ActionPrefix%.AxlSoap601.pdb %SymbolDir% /Y >> %PostBuildLog%

xcopy %ProjectRoot%\NativeTypes\CiscoIPPhoneXml\bin\%BuildTarget%\%TypePrefix%.CiscoIpPhone.pdb %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\Standard\bin\%BuildTarget%\%TypePrefix%.pdb                     %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\Database\bin\%BuildTarget%\%TypePrefix%.Database.pdb            %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\Http\bin\%BuildTarget%\%TypePrefix%.Http.pdb                    %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\Imaging\bin\%BuildTarget%\%TypePrefix%.Imaging.pdb              %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\CiscoExtensionMobility\bin\%BuildTarget%\%TypePrefix%.CiscoExtensionMobility.pdb %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\CiscoDeviceList\bin\%BuildTarget%\%TypePrefix%.CiscoDeviceList.pdb %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\AxlSoap\3.3.3\bin\%BuildTarget%\%TypePrefix%.AxlSoap333.pdb        %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\AxlSoap\4.1.3\bin\%BuildTarget%\%TypePrefix%.AxlSoap413.pdb        %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\AxlSoap\5.0.4\bin\%BuildTarget%\%TypePrefix%.AxlSoap504.pdb        %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\AxlSoap\6.0.1\bin\%BuildTarget%\%TypePrefix%.AxlSoap601.pdb        %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\NativeTypes\Presence\bin\%BuildTarget%\%TypePrefix%.Presence.pdb               %SymbolDir% /Y >> %PostBuildLog%


REM ******* Remove existing provider install directories
rmdir /S /Q %AppServerDir%\Providers
mkdir %AppServerDir%\Providers

REM ******* Copy provider dll's *******
call CreateProviderStructure.bat %ProjectRoot%\Providers\MediaControl\bin %BuildTarget% Metreos.MediaControl
call CreateProviderStructure.bat %ProjectRoot%\Providers\Http\bin %BuildTarget% %ProviderPrefix%.Http
call CreateProviderStructure.bat %ProjectRoot%\Providers\TimerFacility\bin %BuildTarget% %ProviderPrefix%.TimerFacility
call CreateProviderStructure.bat %ProjectRoot%\Providers\Presence\bin %BuildTarget% %ProviderPrefix%.Presence
call CreateProviderStructure.bat %ProjectRoot%\Providers\Cisco.DeviceListX\bin %BuildTarget% %ProviderPrefix%.CiscoDeviceListX
call CreateProviderStructure.bat %ProjectRoot%\Providers\CallControl\H323\bin %BuildTarget% %CCProviderPrefix%.H323
call CreateProviderStructure.bat %ProjectRoot%\Providers\CallControl\Jtapi\bin %BuildTarget% %CCProviderPrefix%.JTapi
call CreateProviderStructure.bat %ProjectRoot%\Providers\CallControl\Sccp\bin %BuildTarget% %CCProviderPrefix%.Sccp
call CreateProviderStructure.bat %ProjectRoot%\Providers\CallControl\Sip\bin %BuildTarget% %CCProviderPrefix%.Sip

REM ******* Copy soul-sucking license file *******
xcopy %ProjectRoot%\licenses.licx %AppServerDir% /Y >> %PostBuildLog%

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
