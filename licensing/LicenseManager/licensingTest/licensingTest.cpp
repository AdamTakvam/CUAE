// licensingTest.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <windows.h>
#include <stdio.h>
#include <conio.h>
#include "CUAELicMgr.h"

typedef int (*importFunction)(char*);

int _tmain(int argc, _TCHAR* argv[])
{
	importFunction licenseFunction;

  // HINSTANCE hinstLib = LoadLibrary("C:\\workspace\\2.2\\licensing\\LicenseManager\\Debug\\CUAELicMgr.dll");
	HINSTANCE hinstLib = LoadLibrary("..\\..\\LicenseManager\\Debug\\CUAELicMgr.dll");
    if (hinstLib == NULL) 
	{
            printf("ERROR: unable to load DLL\n");
            return 1;
    }

	// Get function pointer
    licenseFunction = (importFunction)GetProcAddress(hinstLib, "validateLicenseFile");
    if (licenseFunction == NULL) {
            printf("ERROR: unable to find DLL function\n");
            FreeLibrary(hinstLib);
            return 1;
    }

    /*
    LicenseInformationCUME licInfo;
    memset(&licInfo, 0, sizeof(LicenseInformationCUME));
    licInfo.length = sizeof(LicenseInformationCUME);
    licInfo.signature = LI_SIG;
*/
    int result = licenseFunction("c:\\jzdev-stdlic.lic");

	FreeLibrary(hinstLib);

	printf("The result was: %d\n", result);

  if (result == CUAE_UNHANDLED_EXCEPTION) printf("result indicates unhandled exception thrown\n");
  char c=0; printf("any key ..."); while(!c) c = _getch(); printf("\n");

	return 0;
}

