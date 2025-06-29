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
set ProjectRoot=%MetreosWorkspaceRoot%\db-migration
set PostBuildLog=%ProjectRoot%\postbuild\postbuild.txt

REM set ProviderPrefix=Metreos.Providers
set AssemblyPrefix=Metreos.AppServer
set DeployDir=%FwToolsDir%
set SymbolDir=%DeployDir%\Symbols

rem -- Clear post-build log
if exist %PostBuildLog% del %PostBuildLog% /F /Q
@echo ** PostBuild Beginning > %PostBuildLog%    

rem -- Create deploy directory
if not exist %DeployDir%           mkdir %DeployDir%
if not exist %SymbolDir%           mkdir %SymbolDir%

rem -- Install files
@echo ** Installing files in deployment directory   >> %PostBuildLog%

REM Clear shadow copies of dlls
%CLR_gacutil% /cdl >> %PostBuildLog%

@echo ** Installing assemblies ** >> %PostBuildLog%
xcopy %ProjectRoot%\dbmigrationtool\bin\%BuildTarget%\migration-core.dll            %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\dbmigrationtool\bin\%BuildTarget%\migration-defs.dll            %DeployDir% /Y >> %PostBuildLog%

@echo ** Installing executables ** >> %PostBuildLog%
xcopy %ProjectRoot%\dbmigrationtool\bin\%BuildTarget%\cuae-dbtool.exe               %DeployDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\dbmigrationtool\bin\%BuildTarget%\cuae-dbtool.exe.config        %DeployDir% /Y >> %PostBuildLog%

@echo ** Installing PDBs ** >> %PostBuildLog%
xcopy %ProjectRoot%\dbmigrationtool\bin\%BuildTarget%\cuae-dbtool.pdb               %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\dbmigrationtool\bin\%BuildTarget%\migration-core.pdb            %SymbolDir% /Y >> %PostBuildLog%
xcopy %ProjectRoot%\dbmigrationtool\bin\%BuildTarget%\migration-defs.pdb            %SymbolDir% /Y >> %PostBuildLog%


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
