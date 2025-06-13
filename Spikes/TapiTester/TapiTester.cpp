#include "stdafx.h"
#include <iostream>

#include <tapi.h>

#using <mscorlib.dll>

using namespace System;

VOID FAR PASCAL lineCallbackFunc(
  DWORD hDevice,
  DWORD dwMsg,
  DWORD dwCallbackInstance,
  DWORD dwParam1,
  DWORD dwParam2,
  DWORD dwParam3
)
{
    std::cout << "Callback function invoked" << std::endl;
}

int _tmain()
{
    unsigned long apiVersion = 0x00020001;
    unsigned long numDevices;

    HLINEAPP lineApp;
    LINEINITIALIZEEXPARAMS lineInit;

    long result;

    memset(&lineInit, 0, sizeof(lineInit));

    lineInit.dwTotalSize = sizeof(lineInit);

    result = lineInitializeEx(&lineApp, NULL, lineCallbackFunc, "TapiTester", &numDevices, &apiVersion, &lineInit);

    std::cout << "lineInitializeEx returned: " << result << std::endl;
    std::cout << "Number of Devices returned: " << numDevices << std::endl;

    lineOpen(lineApp, 

    result = lineShutdown(lineApp);

    std::cout << "lineShutdown returned: " << result << std::endl;

	return 0;
}



