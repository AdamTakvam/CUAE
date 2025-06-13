#pragma once
#include <stdlib.h>
#include <stdio.h>
#include <stdarg.h>

class CDLPLog
{
private:
    FILE *fp;
    char tempBuffer[1024];
	char pFileName[255];

public:
    CDLPLog(char *logfilename);
    ~CDLPLog(void);
    void vPrintDebug(const char* fmt,...);
    
};
