//
// HmpLicMgr.h -- HMP licensing manager interface
//
#include "LicenseManagerOEM.h"
#include <iostream>

#ifdef  OEMFUNC_EXPORTS
#define OEMFUNC_API __declspec(dllexport)

#else
#define OEMFUNC_API __declspec(dllimport)
#endif

extern "C" 
{
	char* dllname = "C:\\Program Files\\Cisco Systems\\Unified Application Environment\\LicenseServer\\CUAEUtl2.dll";
	const int nBufferSize = 500;
	CHAR pLogFile[nBufferSize+1];
	void WriteLog(char* pFile, char* pMsg);	
}

//extern "C" OEMFUNC_API LICoemStatus fnOemfunc(long *output);
extern "C" OEMFUNC_API LICoemStatus utilityfunc(long *output);
