@echo off

setlocal

if "%DesignerPath%"=="" set DesignerPath="C:\Program Files\Metreos\MaxDesigner\MaxDesigner.exe"

echo Build Started...
echo Building Plugins...

call "%VS71comntools%\vsvars32.bat" >nul

devenv.exe plugins\LdapDirectory.sln /build DEBUG /out plugins\LdapDirectory.sln.txt

echo Finished Building Plugins
echo Building Max Project File...

call %DesignerPath% mca/LdapDirectory.max /b 

if not exist bin\mca                  mkdir bin\mca

copy   mca\bin\LdapDirectory.mca      bin\mca    /Y > NUL

echo Finished Building Max Project File
echo Finished Build