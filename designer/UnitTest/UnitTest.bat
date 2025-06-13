@echo off
rem -- UnitTest.bat
rem -- 
rem -- Implement this script to execute unit tests
rem --
setlocal
set BuildTarget=%1
if "%BuildTarget%" == "" set BuildTarget=Debug

rem -- Include 'unittest-init.cmd'
rem -- Provided vars:
rem --    MetreosWorkspaceRoot   (e.g., X:\)
rem --    MetreosToolsRoot       (e.g., X:\Tools)
rem --    MetreosContribRoot     (e.g., X:\Contrib)
rem --    CLR_gacutil            (fullqualified path of gacutil)
rem
if "%CUAEWORKSPACE%"=="" set CUAEWORKSPACE=X:
call %CUAEWORKSPACE%\Tools\build-scripts\unittest-init.cmd
rem -- replace 'module-name' with the module directory name
set ProjectRoot=%MetreosWorkspaceRoot%\designer
set UnitTestLog=%ProjectRoot%\UnitTest\unittest.txt

rem -- Clear unit-test log
if exist %UnitTestLog% del %UnitTestLog% /F /Q
@echo ** UnitTest Beginning > %UnitTestLog%    

rem -- Invoke unit test applications
rem -- each unit test application my invoke multiple unit tests
rem -- all output of the unit test applications should be to STDOUT
rem -- no 'windowed' operation
rem -- All application output should conform to the following structure:
rem 
rem    UNITTEST: MODULE-NAME.Test beginning
rem    TEST: MODULE-NAME.Test.blahblahblah01
rem    <optional output/log information for test MODULE-NAME.Test.blahblahblah />
rem    RESULT: PASS|FAIL|SKIPPED
rem    TEST: MODULE-NAME.Test.blahblahblah02
rem    <optional log info />
rem    RESULT: PASS|FAIL|SKIPPED
rem    ...
rem    UNITTESTCOMPLETE: n PASS, n FAIL, n SKIPPED
rem
rem -- The identified 'MODULE-NAME.Test' should match the module tested,
rem -- e.g. 'alarm-service' and the test application run 'alarmunit', i.e.,
rem -- "Alarm-Service.AlarmUnit01'
rem 
rem -- The identifier 'blahblahblah01' can be anything as long as it is
rem -- unique in the context of MODULE-NAME.Test
rem 
rem -- More than one unit test application may be invoked
rem
rem -- If the unit test application fails to load its tests or a test throws
rem -- an exception. The unit test application should catch the exception,
rem -- set its exit code to '1' and output the following lines:
rem
rem    UNITTESTEXCEPTION: 
rem    <text describing exception/error in unit test application>
rem    UNITTESTCOMPLETE: n PASS, n FAIL, n SKIPPED
rem
rem -- Here the PASS, FAIL, and SKIPPED numbers only need to include what
rem -- was attempted up to the exception.
rem
rem -- Finally, all output from the unit test application(s) should
rem -- be redirected to the %UnitTestLog% file

rem ** Execute Unit Tests
echo UNITTEST: MODULE-NAME.StubTest.Stub01          >> %UnitTestLog%
echo stub unit test, replace with something real    >> %UnitTestLog%
echo RESULT: PASS                                   >> %UnitTestLog%
echo UNITTESTCOMPLETE: 1 PASS, 0 FAIL, 0 SKIPPED    >> %UnitTestLog%

rem ** Unit test complete
goto done

:usage
echo Usage: UnitTest.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).
goto done

:done
echo ** UnitTest Complete                         >> %UnitTestLog%
type %UnitTestLog%
