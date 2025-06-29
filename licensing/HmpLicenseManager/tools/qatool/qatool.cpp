// qatool.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <conio.h>
#include <windows.h>
#include "LicKeyDll.h"
#pragma comment(lib,"CUAEUtl2.lib") 

using namespace std; 

int _tmain(int argc, _TCHAR* argv[])
{
	unsigned long code[32];
	decrypt(code);
	for (int i=0;i<32;i++) {
		std::cout << "data = " << code[i] << '\n';
	}
	return 0;
}

