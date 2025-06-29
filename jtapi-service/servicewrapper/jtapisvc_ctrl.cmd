@echo off
setlocal

set COMMAND=%1
set PASSWORD=%2
set SVCOPT=""
set _WRAPPER_EXE=@MetreosFrameworkDirectory@\wrapper.exe
set _REALPATH=%~dp0

if "%COMMAND%"=="register"   set SVCOPT="-i"
if "%COMMAND%"=="unregister" set SVCOPT="-r"
if "%COMMAND%"=="start"      set SVCOPT="-t"
if "%COMMAND%"=="stop"       set SVCOPT="-p"
if %SVCOPT%==""            goto :error

if %SVCOPT%=="-i" if not "%PASSWORD%"=="" goto execwithaccount

:exec
CD @MetreosJtapiServiceDirectory@
for %%w in (wrapper-*.conf) do "%_WRAPPER_EXE%" %SVCOPT% "%_REALPATH%%%w"
goto :done

:execwithaccount
CD @MetreosJtapiServiceDirectory@
echo wrapper.ntservice.account=.\CiscoUAE > password.conf
echo wrapper.ntservice.password=%PASSWORD% >> password.conf
for %%w in (wrapper-*.conf) do "%_WRAPPER_EXE%" %SVCOPT% "%_REALPATH%%%w"
del password.conf
goto :done

:error
echo "Usage: jtapisvc_ctrl.cmd {register [SERVICE_USER_PASSWORD]| unregister | start | stop }"
echo
goto :done

:done
echo .
