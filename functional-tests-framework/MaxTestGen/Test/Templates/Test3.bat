@echo off

setlocal

call "..\..\CreateDeployDirectoryStructure.bat"

%TestDir%\maketest.exe -n:Test3 -l -g:ARE -s:script -sig:script.blast -trigger:script.Metreos.Providers.Http.GotRequest -sig:script.blast2 -e: -d:x:\functional-tests\TestBank