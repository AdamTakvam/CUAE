@echo off

if %1 == "" goto help
if %2 == "" goto help
if %3 == "" goto help

if not exist %AppServerDir%\Providers\%3 mkdir %AppServerDir%\Providers\%3                             >> %PostBuildLog%
if not exist %AppServerDir%\Providers\%3\Docs mkdir %AppServerDir%\Providers\%3\Docs                   >> %PostBuildLog%
if not exist %AppServerDir%\Providers\%3\Resources mkdir %AppServerDir%\Providers\%3\Resources         >> %PostBuildLog%
if not exist %AppServerDir%\Providers\%3\Service mkdir %AppServerDir%\Providers\%3\Service             >> %PostBuildLog%
if not exist %AppServerDir%\Providers\%3\Web mkdir %AppServerDir%\Providers\%3\Web                     >> %PostBuildLog%

if not exist %AppServerDir%\Symbols mkdir %AppServerDir%\Symbols                                       >> %PostBuildLog%

xcopy %1\%2\%3.dll            %AppServerDir%\Providers\%3                                           /Y >> %PostBuildLog%

xcopy %1\%2\%3.pdb            %AppServerDir%\Symbols                                                /Y >> %PostBuildLog%

if /i NOT "%2"=="debug" if /i NOT "%2"=="debugmethodcalltrace" goto removePdb

goto end

:removePdb

goto end

:help

echo Usage: CreateProviderStructure <source path> <build target> <provider name>

:end