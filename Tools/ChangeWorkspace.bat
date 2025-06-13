rem @echo off

setlocal

SET SED="C:\UnxUtils\usr\local\wbin\sed.exe"

if "%1"=="" goto usage

echo Setting up drive substitution...
subst /d x:
subst x: "%1"

if not exist %SED% goto noSed

echo Updating Apache config
copy "%1\mceadmin\install\httpd.conf" "C:\Program Files\Apache Group\Apache\conf"

copy "C:\Metreos\System\Apache\conf\mce.conf" "C:\Metreos\System\Apache\mce.conf.old"
sed -e "s,REPLACE_ME_WITH_WORKPATH/Build,%1,g" "%1\mceadmin\install\apache\conf\mce-dev.conf" > "C:\Metreos\System\Apache\conf\mce.conf"

copy "C:\Metreos\System\Apache\conf\appsuite.conf" "C:\Metreos\System\Apache\appsuite.conf.old"
sed -e "s,REPLACE_ME_WITH_WORKPATH,%1,g" "%1\appsuiteadmin\install\apache\conf\appsuite-dev.conf" > "C:\Metreos\System\Apache\conf\appsuite.conf"

copy "C:\Metreos\System\Apache\conf\metreos_http.conf" "C:\Metreos\System\Apache\metreos_http.conf.old"
sed -e "s,REPLACE_ME_WITH_WORKPATH,%1,g" "%1\mod_metreos_http\apache-1.3\conf\metreos_http-dev.conf" > "C:\Metreos\System\Apache\conf\metreos_http.conf"

goto done

:usage
echo.
echo Usage   : ChangeWorkspace.bat [workspace directory]
echo Example : ChangeWorkspace.bat c:\workspace-1.0
echo.
pause
goto done

:noSed
echo.
echo ERROR: sed.exe was not found.
echo This program must be located at: %SED%
echo Download the UnxUtils package from http://sourceforge.net/projects/unxutils/
echo.
pause
goto done

:done