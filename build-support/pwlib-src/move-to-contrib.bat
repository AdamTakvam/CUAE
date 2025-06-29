@ECHO OFF

ECHO Moving necessary files to X:\Contrib\PWLib

xcopy version.h     x:\Contrib\PWLib\            /R /Y       >  move-to-contrib.txt
xcopy mpl-1.0.htm   x:\Contrib\PWLib\            /R /Y       >> move-to-contrib.txt
xcopy include\*.*   x:\Contrib\PWLib\include\    /E /R /Y    >> move-to-contrib.txt
xcopy lib\*.lib     x:\Contrib\PWLib\lib\        /R /Y       >> move-to-contrib.txt
xcopy lib\*.dll     x:\Contrib\PWLib\lib\        /R /Y       >> move-to-contrib.txt
xcopy lib\*.pdb     x:\Contrib\PWLib\lib\        /R /Y       >> move-to-contrib.txt

ECHO Moving necessary files to X:\build-support\Contrib\PWLib

xcopy version.h     x:\build-support\Contrib\PWLib\            /R /Y       >> move-to-contrib.txt
xcopy mpl-1.0.htm   x:\build-support\Contrib\PWLib\            /R /Y       >> move-to-contrib.txt
xcopy include\*.*   x:\build-support\Contrib\PWLib\include\    /E /R /Y    >> move-to-contrib.txt
xcopy lib\*.lib     x:\build-support\Contrib\PWLib\lib\        /R /Y       >> move-to-contrib.txt
xcopy lib\*.dll     x:\build-support\Contrib\PWLib\lib\        /R /Y       >> move-to-contrib.txt
xcopy lib\*.pdb     x:\build-support\Contrib\PWLib\lib\        /R /Y       >> move-to-contrib.txt

ECHO Done
