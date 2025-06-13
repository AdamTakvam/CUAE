set DESTDIR=%1

copy x:\contrib\ace-5.4\lib\ace.dll    %DESTDIR%
copy x:\contrib\ace-5.4\lib\aced.dll   %DESTDIR%

copy ..\..\lib\cpp-core.dll                 %DESTDIR%
copy ..\..\lib\cpp-cored.dll                %DESTDIR%