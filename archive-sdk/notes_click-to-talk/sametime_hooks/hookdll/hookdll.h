
#ifndef HOOKDLL_H
#define HOOKDLL_H

#include <windows.h>

#define	DLL_EXPORT	__declspec(dllexport)

DLL_EXPORT void InstallHook(void);

DLL_EXPORT void	UninstallHook(void);

int WriteToLog(char* str);

void SendEvent(HWND hWnd);

void StartWorker();

#endif
