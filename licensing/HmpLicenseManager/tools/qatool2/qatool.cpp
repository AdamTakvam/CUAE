// qatool.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <conio.h>
#include <windows.h>
#include <stdio.h>
#include "LicKeyDll.h"
#pragma comment(lib,"CUAEUtl2.lib") 

using namespace std; 

typedef void (*dllFunction)(unsigned long*);

int _tmain(int argc, _TCHAR* argv[])
{
	dllFunction decryptKey;
	HINSTANCE hinstLib = LoadLibrary("X:\\licensing\\LicKeyDll\\lib\\CUAEUtl2.dll");
	//HINSTANCE hinstLib = LoadLibrary("CUAEUtl2.dll");
	if (hinstLib == NULL) {
		std::cout << "hinstLib null" << '\n';
		return 1;
	}
	decryptKey = (dllFunction)GetProcAddress(hinstLib, "decrypt");
	if (decryptKey == NULL) {
		std::cout << "decryptKey null" << '\n';
		FreeLibrary(hinstLib);
		return 1;
	}
	unsigned long code[32];
	decryptKey(code);
	for (int i=0;i<32;i++) {
		std::cout << "data = " << code[i] << '\n';
	}
	FreeLibrary(hinstLib);
	return 0;
}

