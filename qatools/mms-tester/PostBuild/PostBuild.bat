@ECHO OFF

if "%1" == "" goto usage

if not exist ..\bin\ mkdir ..\bin\
if not exist ..\bin\Adapters\ mkdir ..\bin\Adapters\
if not exist ..\bin\Clients\ mkdir ..\bin\Clients\
if not exist ..\bin\VisualInterfaces\ mkdir ..\bin\VisualInterfaces


del postbuild.txt /F /Q

rem Copy dlls
xcopy ..\Core\bin\%1\Metreos.MmsTester.* /Y ..\bin\
xcopy ..\Interfaces\bin\%1\Metreos.MmsTester.* /Y ..\bin\
xcopy ..\AdapterManager\bin\%1\Metreos.MmsTester.* /Y ..\bin\
xcopy ..\ClientFramework\bin\%1\Metreos.MmsTester.* /Y ..\bin\
xcopy ..\ClientManager\bin\%1\Metreos.MmsTester.* /Y ..\bin\
xcopy ..\Conduit\bin\%1\Metreos.MmsTester.* /Y ..\bin\
xcopy ..\MmsTestToolMain\bin\%1\Metreos.MmsTester.* /Y ..\bin\
xcopy ..\VisualInterfaceFramework\bin\%1\Metreos.MmsTester.* /Y ..\bin\
xcopy ..\VisualInterfaceManager\bin\%1\Metreos.MmsTester.* /Y ..\bin\


rem Place custom libraries into the appropriate folders
xcopy ..\Adapters\bin\%1\Metreos.MmsTester.Custom.Adapters.dll /Y ..\bin\Adapters\
xcopy ..\Clients\bin\%1\Metreos.MmsTester.Custom.Clients.dll /Y ..\bin\Clients\
xcopy ..\VisualInterfaces\bin\%1\Metreos.MmsTester.Custom.VisualInterfaces.dll /Y ..\bin\VisualInterfaces\


goto done

:usage
echo Usage: PostBuild.bat [CONFIGURATION]
echo Configuration is any valid VS.NET configuration (Debug, Release, etc).

goto done

:done
