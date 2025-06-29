//
// CUAELicMgr.cpp -- licensing manager interface
//
#include "stdafx.h"
#include <string>
#include <iostream>
#include <errno.h>
#include <map>
#include <tchar.h>
#include "HmpLicMgr.h"
#include "LicenseManagerOEM.h"
#include <sys\types.h> 
#include <sys\stat.h> 
#include <stdio.h>

typedef void (*dllFunction)(unsigned long*);

extern "C" __declspec(dllimport)void decrypt(unsigned long* code);

///////////////////////////////////////////////////////////
//  DllMain
//  
//  DLL Main
//  
//
///////////////////////////////////////////////////////////
BOOL APIENTRY DllMain( HANDLE hModule, 
                       DWORD  ul_reason_for_call, 
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	  case DLL_PROCESS_ATTACH:
	  case DLL_THREAD_ATTACH:
	  case DLL_THREAD_DETACH:
	  case DLL_PROCESS_DETACH:
		break;
	}
    return TRUE;
}

///////////////////////////////////////////////////////////
//  LICoemStatus
//  
//  YoungJins Function
//  
//
///////////////////////////////////////////////////////////

OEMFUNC_API LICoemStatus utilityfunc(long *output)
{
	dllFunction decryptKey;
	HINSTANCE hinstLib = LoadLibrary(dllname);
	//HINSTANCE hinstLib = LoadLibrary("CUAEUtl2.dll");
	if (hinstLib == NULL) {
		//std::cout << "hinstLib null" << '\n';
		return LIC_OEM_NOT_RUNNING;
	}
	//SYSTEMTIME oT;
	//GetLocalTime(&oT);
	//sprintf(pLogFile, _T("%04d-%02d-%02d-HmpLicMgr.log"), oT.wYear, oT.wMonth, oT.wDay);
	//char pTemp[121];
	//sprintf(pTemp, "LIC running1"); 
	//WriteLog(pLogFile, pTemp);
	decryptKey = (dllFunction)GetProcAddress(hinstLib, "decrypt");
	if (decryptKey == NULL) {
		//std::cout << "decryptKey null" << '\n';
		FreeLibrary(hinstLib);
		return LIC_OEM_NOT_RUNNING;
	}
	//sprintf(pTemp, "LIC running2"); 
	//WriteLog(pLogFile, pTemp);
	int i;
	unsigned long code[32];
	// copy table to output
	//SYSTEMTIME oT;
	//GetLocalTime(&oT);
	//sprintf(pLogFile, _T("%04d-%02d-%02d-CUAELicMgr2.log"), oT.wYear, oT.wMonth, oT.wDay);
	decryptKey(code);
	//sprintf(pTemp, "LIC decrypted"); 
	//WriteLog(pLogFile, pTemp);
	for (i=0; i<32; i++) {
		output[i]=code[i];	
		//char pTemp[121];
		//sprintf(pTemp, "\nDATA [%02d] %10d\n", i, code[i]); 
		//WriteLog(pLogFile, pTemp);
	}
	return LIC_OEM_SUCCESS;
}

///////////////////////////////////////////////////////////
//  WriteLog
//  
//  Write Log
//  
//
///////////////////////////////////////////////////////////
void WriteLog(char* pFile, char* pMsg) {
	try {
		SYSTEMTIME oT;
		GetLocalTime(&oT);
		FILE* pLog = fopen(pFile,"a");
		fprintf(pLog, "\n%02d:%02d:%02d\n    %s",oT.wHour,oT.wMinute,oT.wSecond,pMsg);
		fclose(pLog);
	} catch(...) {}
}
