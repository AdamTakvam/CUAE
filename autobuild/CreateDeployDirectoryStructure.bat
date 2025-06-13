@echo off

if "%CUAEWORKSPACE%"=="" set CUAEWORKSPACE=X:
call %CUAEWORKSPACE%\autobuild\SetBuildVariables.bat

REM ******* Create directories *******

if not exist %DeployDir%                    mkdir %DeployDir%
if not exist %AppServerDir%                 mkdir %AppServerDir%
if not exist %AppServerDir%\Libs            mkdir %AppServerDir%\Libs
if not exist %AppServerDir%\Deploy          mkdir %AppServerDir%\Deploy
if not exist %AppServerDir%\Applications    mkdir %AppServerDir%\Applications
if not exist %AppServerDir%\Providers       mkdir %AppServerDir%\Providers
if not exist %AppServerDir%\TmScripts       mkdir %AppServerDir%\TmScripts

if not exist %MediaServerDir%               mkdir %MediaServerDir%

if not exist %M_FrameworkRootDir%           mkdir %M_FrameworkRootDir%
if not exist %M_FrameworkDir%               mkdir %M_FrameworkDir%
if not exist %FwCoreDir%                    mkdir %FwCoreDir%
if not exist %FwPackageDir%                 mkdir %FwPackageDir%
if not exist %FwToolsDir%                   mkdir %FwToolsDir%
if not exist %FwActionDir%                  mkdir %FwActionDir%
if not exist %FwTypeDir%                    mkdir %FwTypeDir%

if not exist %SftpDir%                      mkdir %SftpDir%

if not exist %UnitTestDir%                  mkdir %UnitTestDir%

REM Put the common CUAE config file into place.
xcopy %Root%\autobuild\cuae-common.config %DeployDir% /Y

