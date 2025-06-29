@echo off

setlocal

call "..\..\CreateDeployDirectoryStructure.bat"

%TestDir%\maketest.exe -n:Test4 -l -g:ARE -s:script -sig:script.blast -e:script.Event1 -sig:script.blast2 -e: -d:x:\functional-tests\TestBank