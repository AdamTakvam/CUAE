@rem -------------------------------------------------------------
@rem Build a standard Visual Studio 6.0 solution.
@rem
@rem Command line arguments:
@rem    %1 -> The filename of the solution to build
@rem    %2 -> The configuration to build
@rem -------------------------------------------------------------

@setlocal
set Vs6=msdev.com
if "%Vs6Dir%"=="" set Vs6Dir=C:\Program Files\Microsoft Visual Studio
call "%Vs6Dir%\VC98\Bin\vcvars32.bat" >nul

@set LIB=
@set INCLUDE=

@set PROJECT=%3

@if /i "%2"=="clean" goto clean

@if /i "%PROJECT%"=="" @set PROJECT=ALL

%Vs6% %1 /make "%PROJECT% - %2"
@goto end

:clean
%Vs6% %1 /make /clean
@goto end

:end
@echo.
@echo ERRORLEVEL = %ERRORLEVEL%
@exit %ERRORLEVEL%
