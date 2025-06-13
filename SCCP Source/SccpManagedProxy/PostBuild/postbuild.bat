@echo off

if %1 == "" goto help

if not exist x:\Build			mkdir x:\Build
if not exist x:\Build\AppServer mkdir x:\Build\AppServer

xcopy %1\SccpManagedProxy.dll x:\contrib\sccp				> postbuild.txt

goto end

:help

echo Usage postbuild.bat <configuration name>

:end