The test licenses are in the "TestLicenses" directory - copy them to somewhere your license server has access to them.
Start the license server: lmgrd -c PATH_TO_TEST_LICENSE_DIR -z
Build the solution. Copy bin\debug\LicensingFlexTrivial.exe where you want it to be.
Copy X:\Contrib\FLEXlm\lmgr10.dll to the same location as LicensingFlexTrivial.exe
execute LicensingFlexTrivial.exe to see list of command line params.
Use output of both the spike and the license server to see what's going on.
When you want to shut down the license server, use its 'lmutil.exe lmdown -c -c PATH_TO_TEST_LICENSE_DIR -all'