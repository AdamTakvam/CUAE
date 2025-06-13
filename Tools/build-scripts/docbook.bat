@echo off

REM  Docbook utility script
REM  Created by Seth Call 12/12/2006

REM  Reference x:\docs\docbook\deployment_steps.txt to learn what needs to be in place for this script to work.

REM  Required Params:
REM  In  Param (%1)   Full or Relative Path
REM  Out Param (%2)   Full or Relative Path
REM  Type      (%3)   Type = | API | DevArticle | DevBook | AdminArticle |

if "%CUAEWORKSPACE%"=="" set CUAEWORKSPACE=X:
call %CUAEWORKSPACE%\Tools\build-scripts\build-init.cmd

if %1=="" goto missingargs
if %2=="" goto missingargs
if %3=="" goto missingargs

set inpath=%1
set outpath=%2
set type=%3

set CLASSPATH=%CiscoToolsRoot%\xalan-j_2_7_0\xalan.jar;X:\Tools\xalan-j_2_7_0\xml-apis.jar;%CiscoToolsRoot%\xalan-j_2_7_0\xercesImpl.tar;%CiscoToolsRoot%\docbook-xsl-1.71.1\extensions\xalan27.jar

if %type%==API goto APIProc
if %type%==DevArticle goto DevArticleProc
if %type%==DevBook goto DevBookProc
if %type%==AdminArticle goto AdminArticleProc

REM No matching type!
goto missingargs

REM  Process the XML file as API document.
:APIProc
REM java -Djava.net.useSystemProxies=true org.apache.xalan.xslt.Process  -out %outpath%.html -in %inpath% -xsl %CUAEWORKSPACE%\docs\docbook\xslt\api\cuae-chunk.xsl -param use.extensions 1
java -Djava.net.useSystemProxies=true org.apache.xalan.xslt.Process  -out %outpath%.html -in %inpath% -xsl %CUAEWORKSPACE%\docs\docbook\xslt\api\cuae-chunk.xsl -param use.extensions 1 

REM I don't like behavior of this param.  Everything *including* index.html goes into it's own directy: -param base.dir c:/workspace/head/docs/cuae-developer-api-reference/obj/cuae-api-reference-guide/content/

goto done

REM  Process the XML file as a Developer Article
:DevArticleProc
java -Djava.net.useSystemProxies=true org.apache.xalan.xslt.Process  -out %outpath%.html -in %inpath% -xsl %CUAEWORKSPACE%\docs\docbook\xslt\dev_article\dev_article.xsl  -param use.extensions 1 -param html.stylesheet dev_article.css
goto done

REM Process the XML file as a Developer Book
:DevBookProc
REM Generate XHTML
java -Djava.net.useSystemProxies=true org.apache.xalan.xslt.Process  -out %outpath%.html -in %inpath% -xsl %CUAEWORKSPACE%\docs\docbook\xslt\dev_book\dev_book.xsl  -param use.extensions 1 -param html.stylesheet dev_book.css
goto done

REM  Process the XML file as a Administrator Article
:AdminArticleProc
java -Djava.net.useSystemProxies=true org.apache.xalan.xslt.Process  -out %outpath%.html -in %inpath% -xsl %CUAEWORKSPACE%\docs\docbook\xslt\admin_article\admin_article.xsl  -param use.extensions 1 -param html.stylesheet admin_article.css
goto done


REM  Missing Argument Warning!
:missingargs
echo Missing arguments:
echo In  Param (%1)   Full or Relative Path to Docbook XML File
echo Out Param (%2)   Full or Relative Path to output file with no extension.
echo Type      (%3)   Type = | API | DevArticle | DevBook | AdminArticle |
goto done

REM  Done!
:done 
echo Done
