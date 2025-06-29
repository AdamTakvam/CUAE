@echo off

REM something odd about MSBee--requires these two copies

xcopy "c:\program files\cisco systems\Unified Application Designer\Framework\1.0\CoreAssemblies\Metreos.ApplicationFramework.dll"  ..\NativeAction\bin\FX1_1\Debug\ /Y        
xcopy "c:\program files\cisco systems\Unified Application Designer\Framework\1.0\CoreAssemblies\Metreos.PackageGeneratorCore.dll"  ..\NativeAction\bin\FX1_1\Debug\ /Y  

C:\Windows\Microsoft.NET\Framework\v2.0.50727\msbuild ..\NativeAction\NativeAction.csproj /p:TargetFX1_1=true /p:CustomAfterMicrosoftCommonTargets="C:\Program Files\MSBuild\MSBee\MSBuildExtras.Fx1_1.CSharp.targets" 
