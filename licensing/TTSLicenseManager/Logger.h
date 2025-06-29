#include "stdafx.h" 

// Runtime Includes
#include <iostream>
#include <cstring>
#include <iomanip>
#include <tchar.h>

#include <stdio.h>
#include <windows.h>

using namespace std; 

class Logger {
	string task;
	HANDLE hOut;
	DWORD nOut;
	DWORD dwSize;
public:
	Logger(string task);
	~Logger();
	void log(string state);
};