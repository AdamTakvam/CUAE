@echo off

setlocal

if "%DesignerPath%"=="" set DesignerPath="C:\Program Files\Metreos\MaxDesigner\MaxDesigner.exe"

echo Build Started...


echo Building Max Project File...

call %DesignerPath% mca/ExtensionMobility.max /b 

if not exist bin		      mkdir bin
if not exist bin\mca                  mkdir bin\mca

copy   mca\bin\ExtensionMobility.mca     bin\mca    /Y > NUL

echo Finished Building Max Project File
echo Finished Build