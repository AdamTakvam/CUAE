@ECHO OFF

IF NOT EXIST "pwlib.sln" GOTO nopwlib

SET BISONFLEX=X:\Tools\OpenH323
SET BISON_SIMPLE=%BISONFLEX%\Share\bison.simple
SET BISON_HAIRY=%BISONFLEX%\Share\bison.hairy
SET PWLIB=%CD%

SET PATH=%BISONFLEX%;%PWLIB%\Lib;%PATH%
SET LIB=%LIB%;%PWLIB\Lib%
set INCLUDE=%INCLUDE%;%PWLIB%\Include

IF NOT EXIST "%PWLIB%\Include\ptbuildopts.h" configure.exe --no-search --disable-video --disable-stun --disable-wavfile --disable-socks --disable-ftp --disable-snmp --disable-telnet --disable-remconn --disable-serial --disable-pop3smtp --disable-audio

devenv pwlib.sln /build Release /project "Console" /useenv
devenv pwlib.sln /build Release /project "Console Components" /useenv
devenv pwlib.sln /build Release /project "PTLib" /useenv

devenv pwlib.sln /build Debug /project "Console" /useenv
devenv pwlib.sln /build Debug /project "Console Components" /useenv
devenv pwlib.sln /build Debug /project "PTLib" /useenv

GOTO done

:nopwlib
ECHO Execute this script from the PWLib root directory.

:done
