@echo off

setlocal

call "..\..\CreateDeployDirectoryStructure.bat"

%TestDir%\maketest.exe -n:Test2 -l -g:ARE -s:script -sig:script1.blast -sig:script1.blast2 -s:script2 -sig:script2.blast -sig:script2.blast2 -d:x:\functional-tests\TestBank