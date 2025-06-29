@ECHO OFF

SETLOCAL

IF NOT EXIST "openh323.sln" GOTO noopenh323

SET BISONFLEX=X:\Tools\OpenH323
SET BISON_SIMPLE=%BISONFLEX%\Share\bison.simple
SET BISON_HAIRY=%BISONFLEX%\Share\bison.hairy
SET PWLIB=..\PWLib-src
SET OPENH323=%CD%

SET PATH=%BISONFLEX%;%PWLIB%\Lib;%OPENH323%\Lib;%PATH%
SET LIB=%LIB%;%PWLIB%\Lib;%OPENH323%\Lib
set INCLUDE=%INCLUDE%;%PWLIB%\Include;%OPENH323%\Include

REM IF NOT EXIST "%OPENH323%\Include\openh323buildopts.h" configure.exe --extern-dir=C:\
IF NOT EXIST "%OPENH323%\Include\openh323buildopts.h" configure.exe --no-search --disable-video --disable-h501 --disable-ixj

devenv openh323.sln /build Release /project "OpenH323Lib" /useenv
devenv openh323.sln /build Release /project "OpenH323Dll" /useenv

devenv openh323.sln /build Debug /project "OpenH323Lib" /useenv
devenv openh323.sln /build Debug /project "OpenH323Dll" /useenv

GOTO done

:noopenh323
ECHO Execute this script from the OpenH323 root directory.

:done
