@echo off

xcopy SccpProvider.* x:\build\appserver				> postbuild.txt
xcopy x:\contrib\sccp\SccpManagedProxy.dll x:\build\appserver				>> postbuild.txt
xcopy x:\contrib\sccp\SccpManagedProxy.pdb x:\build\appserver				>> postbuild.txt


:end
