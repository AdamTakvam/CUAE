#include "stdafx.h"
#include <stdlib.h>
#include <stdio.h>
#include <memory.h>
#include <string.h>
#include "DLPLog.h"

CDLPLog::CDLPLog(char *logfilename)
{
#ifdef LOG_ON
	memset(pFileName,0,255);
	strcpy(pFileName,logfilename);
#endif
}

CDLPLog::~CDLPLog(void)
{
#ifdef LOG_ON
#endif
}

void CDLPLog::vPrintDebug(const char* fmt,...)
{
#ifdef LOG_ON

	memset(tempBuffer,0,1024);

    va_list ap;
    va_start(ap,fmt);

    vsprintf(tempBuffer,fmt,ap);
	va_end(ap);

	try {
		SYSTEMTIME oT;
		GetLocalTime(&oT);
		FILE* pLog = fopen(pFileName,"a");
		fprintf(pLog, "[%02d:%02d:%02d]-%s",oT.wHour,oT.wMinute,oT.wSecond,tempBuffer);
		fclose(pLog);
	} catch(...) {}
#endif
}