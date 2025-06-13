@rem -------------------------------------------------------------
@rem Build a standard Visual Studio .NET solution.
@rem
@rem Command line arguments:
@rem    %1 -> The filename of the solution to build
@rem    %2 -> The configuration to build
@rem -------------------------------------------------------------

@setlocal
set VsDotNet=devenv.com
if "%VsDotNetDir%"==""  set VsDotNetDir=%VS71COMNTOOLS%
call "%VsDotNetDir%\vsvars32.bat" >nul

@if /i "%2"=="clean" goto clean
@if /i "%2"=="Clean" goto clean

%VsDotNet% %1 /build %2
@goto end

:clean
%VsDotNet% %1 /clean Debug
@if NOT %ERRORLEVEL%==0 @goto end

%VsDotNet% %1 /clean Release
@goto end

:end
@echo.
@echo ERRORLEVEL = %ERRORLEVEL%
@exit %ERRORLEVEL%
