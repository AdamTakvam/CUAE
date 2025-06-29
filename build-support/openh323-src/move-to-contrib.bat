@ECHO OFF

ECHO Moving necessary files to X:\Contrib\OpenH323

xcopy version.h     x:\Contrib\OpenH323\            /R /Y       >  move-to-contrib.txt
xcopy mpl-1.0.htm   x:\Contrib\OpenH323\            /R /Y       >> move-to-contrib.txt
xcopy include\*.*   x:\Contrib\OpenH323\include\    /E /R /Y    >> move-to-contrib.txt
xcopy lib\*.lib     x:\Contrib\OpenH323\lib\        /R /Y       >> move-to-contrib.txt
xcopy lib\*.dll     x:\Contrib\OpenH323\lib\        /R /Y       >> move-to-contrib.txt
xcopy lib\*.pdb     x:\Contrib\OpenH323\lib\        /R /Y       >> move-to-contrib.txt

ECHO Moving necessary files to X:\build-support\Contrib\OpenH323

xcopy version.h     x:\build-support\Contrib\OpenH323\            /R /Y       >> move-to-contrib.txt
xcopy mpl-1.0.htm   x:\build-support\Contrib\OpenH323\            /R /Y       >> move-to-contrib.txt
xcopy include\*.*   x:\build-support\Contrib\OpenH323\include\    /E /R /Y    >> move-to-contrib.txt
xcopy lib\*.lib     x:\build-support\Contrib\OpenH323\lib\        /R /Y       >> move-to-contrib.txt
xcopy lib\*.dll     x:\build-support\Contrib\OpenH323\lib\        /R /Y       >> move-to-contrib.txt
xcopy lib\*.pdb     x:\build-support\Contrib\OpenH323\lib\        /R /Y       >> move-to-contrib.txt

ECHO Done
