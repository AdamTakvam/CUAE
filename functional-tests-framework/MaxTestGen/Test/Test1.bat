@echo off

setlocal

call "..\..\CreateDeployDirectoryStructure.bat"

%TestDir%\maketest.exe -n:Test1 -l -g:ARE -s:scripty -sig:scripty.Blast -sig:scripty.Blast2 -d:x:\functional-tests\TestBank