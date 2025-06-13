@rem -------------------------------------------------------------
@rem Build a standard MSBuild solution.
@rem
@rem Command line arguments:
@rem    %1 -> The filename of the solution to build
@rem    %2 -> The configuration to build
@rem -------------------------------------------------------------

@setlocal
@set LIB=
@set INCLUDE=

@rem todo


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
