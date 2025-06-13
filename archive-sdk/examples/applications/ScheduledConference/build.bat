@echo off

setlocal

if "%DesignerPath%"=="" set DesignerPath="C:\Program Files\Metreos\MaxDesigner\MaxDesigner.exe"

echo Build Started...


echo Building Max Project File...

call %DesignerPath% mca/ScheduledConference.max /b 

if not exist bin		      mkdir bin
if not exist bin\mca                  mkdir bin\mca

copy   mca\bin\ScheduledConference.mca      bin\mca    /Y > NUL

echo Finished Building Max Project File
echo Finished Build