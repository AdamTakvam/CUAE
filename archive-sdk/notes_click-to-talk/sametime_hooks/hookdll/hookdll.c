// hookdll.c

#include <windows.h>
#include <stdio.h>
#include <process.h>
#include "hookdll.h"
#include "udpclient.h"

/////////////////////////////////////////////////////////////////////
#define INI_FILE				"metreos.ini"
#define DEFAULT_PORT			8080
#define DEFAULT_TITLE			"Metreos Click to Talk"
#define DEFAULT_SERVER			"127.0.0.1"
#define LOGFILE					"c:\\hookdll.log"
#define DEFAULT_CLASS			"#32770"
#define DEFAULT_LOG				0
#define DEFAULT_CLOSE_WINDOW	1	

/////////////////////////////////////////////////////////////////////
static BOOL bHooked = FALSE;
static HHOOK CBT = 0;
static HINSTANCE hInst = NULL;
static char szDefaultServer[256];
static char szDefaultTitle[256];
static char szDefaultClass[256];
static int iDefaultPort;
static BOOL bLook = FALSE;
static int iLog;
static int iCloseWindow;


/////////////////////////////////////////////////////////////////////
LRESULT CALLBACK CBTProc(int code, WPARAM wParam, LPARAM lParam);

/////////////////////////////////////////////////////////////////////
BOOL WINAPI	DllMain(HINSTANCE hinstDLL,DWORD fdwReason, LPVOID lpvReserved)
{
	switch (fdwReason)
	{
		case DLL_PROCESS_ATTACH:
			hInst = hinstDLL;
			iLog = GetPrivateProfileInt("CLICK2TALK", "LOG", DEFAULT_LOG, INI_FILE);
			iCloseWindow = GetPrivateProfileInt("CLICK2TALK", "CLOSE_WINDOW", DEFAULT_CLOSE_WINDOW, INI_FILE);
			break;
		case DLL_THREAD_ATTACH:
			break;
		case DLL_THREAD_DETACH:
			break;
		case DLL_PROCESS_DETACH:
			break;
		default:
			break;
	}
	return TRUE;
}

/////////////////////////////////////////////////////////////////////
DLL_EXPORT void InstallHook(void)
{
	if(!bHooked)
	{
		// Hook it!
		CBT = SetWindowsHookEx(WH_CBT, (HOOKPROC)CBTProc, hInst, (DWORD)NULL); 

		if (CBT == NULL)
			WriteToLog("SetWindowsHookEx failed.");
		else
			WriteToLog("SetWindowsHookEx success.");

		bHooked = TRUE;			
	}
}

/////////////////////////////////////////////////////////////////////
DLL_EXPORT void UninstallHook(void)
{
	if(bHooked)
	{
		UnhookWindowsHookEx(CBT);
		WriteToLog("UnhookWindowsHookEx success.");
	}
}

/////////////////////////////////////////////////////////////////////
LRESULT CALLBACK CBTProc(int nCode, WPARAM wParam, LPARAM lParam)
{
	char szTitle[256]; 
	char szClassName[256];
	HWND hParent = NULL;

	if ((nCode==HCBT_ACTIVATE)||
		(nCode==HCBT_CREATEWND)||
		(nCode==HCBT_SETFOCUS))
	{
		memset(&szTitle, 0, sizeof(szTitle));
		memset(&szClassName, 0, sizeof(szClassName));
		GetClassName((HWND)wParam, szClassName, sizeof(szClassName)); 
		PostMessage((HWND)wParam, WM_GETTEXT, sizeof(szTitle), (LPARAM)(void*)szTitle);

		memset(&szDefaultTitle, 0, sizeof(szDefaultTitle));
		GetPrivateProfileString("CLICK2TALK", "TITLE", DEFAULT_TITLE, szDefaultTitle, sizeof(szDefaultTitle), INI_FILE);
		memset(&szDefaultClass, 0, sizeof(szDefaultClass));
		GetPrivateProfileString("CLICK2TALK", "CLASS", DEFAULT_CLASS, szDefaultClass, sizeof(szDefaultClass), INI_FILE);

		if (nCode == HCBT_CREATEWND)
		{
			WriteToLog("HCBT_CREATEWND");
			WriteToLog(szClassName);
			if (bLook && strstr(szClassName, szDefaultClass) != NULL)
			{
				bLook = FALSE;
				//hParent = ((CBT_CREATEWND *) lParam)->lpcs->hwndParent;
				StartWorker(NULL);
				return CallNextHookEx(CBT, nCode, wParam, lParam);
			}
		}
		else if (nCode == HCBT_ACTIVATE)
			WriteToLog("HCBT_ACTIVATE");
		else if (nCode == HCBT_SETFOCUS)
		{
			WriteToLog("HCBT_SETFOCUS");
			if (lstrlen(szTitle) == 0)
			{
				GetWindowText((HWND)wParam, szTitle, sizeof(szTitle));
				if (strstr(szTitle, "Tree2") != NULL)
				{
					bLook = TRUE;
				}
			}
		}

		WriteToLog(szTitle);


		// TODO: Would be nice if we know the default class name to reduce operation
		if (lstrlen(szTitle) != 0 && lstrlen(szDefaultTitle) != 0)
		{
			if (strstr(szTitle, szDefaultTitle) != NULL)
			{
				WriteToLog("Target window found from window title.");
				SendEvent((HWND)wParam);
			}
		}
	}
	return CallNextHookEx(CBT, nCode, wParam, lParam);
}


/////////////////////////////////////////////////////////////////////
void SendEvent(HWND hWnd)
{
	// Found it and close it!
	if (iCloseWindow > 0)
		PostMessage(hWnd, WM_CLOSE, 0L, 0L);

	memset(&szDefaultServer, 0, sizeof(szDefaultServer));
	GetPrivateProfileString("CLICK2TALK", "SERVER", DEFAULT_SERVER, szDefaultServer, sizeof(szDefaultServer), INI_FILE);
	iDefaultPort = GetPrivateProfileInt("CLICK2TALK", "PORT", 0, INI_FILE);
	if (iDefaultPort == 0)
		iDefaultPort = DEFAULT_PORT;

	// Socket init
	if (StartClient())
	{
		if (ConnectToServer(szDefaultServer, iDefaultPort))
		{
			if (!SendToServer())
				WriteToLog("Send UDP data packet filaed.");
			DisconnectFromServer();
		}
		else
			WriteToLog("Unable to connect to UDP server.");

		StopClient();
	}
	else
		WriteToLog("StartClient failed.");
}

DWORD WINAPI ThreadFunc( LPVOID lpParam ) 
{ 
	int i;
	HWND hNext = NULL;
	HWND hParent = (HWND)lpParam;
	char szTitle[256];
	BOOL bFound = FALSE;

	memset(&szDefaultTitle, 0, sizeof(szDefaultTitle));
	GetPrivateProfileString("CLICK2TALK", "TITLE", DEFAULT_TITLE, szDefaultTitle, sizeof(szDefaultTitle), INI_FILE);
	if (lstrlen(szDefaultTitle) == 0)
		return 0;

	memset(&szDefaultClass, 0, sizeof(szDefaultClass));
	GetPrivateProfileString("CLICK2TALK", "CLASS", DEFAULT_CLASS, szDefaultClass, sizeof(szDefaultClass), INI_FILE);
	if (lstrlen(szDefaultClass) == 0)
		return 0;

	for (i=0; i<100 && !bFound; i++)
	{
		Sleep(30);
		
		do
		{
			hNext = FindWindowEx(NULL, hNext, szDefaultClass, NULL);
			if (hNext)
			{
				memset(&szTitle, 0, sizeof(szTitle));
				GetWindowText(hNext, szTitle, sizeof(szTitle));
				if (lstrlen(szTitle) != 0)
				{
					if (strstr(szTitle, szDefaultTitle) != NULL)
					{
						WriteToLog("Target window found from class name.");
						SendEvent(hNext);
						bFound = TRUE;
					}
				}
			}
		} 
		while(hNext != NULL && !bFound);
	}
    return 0;
} 

void StartWorker(HWND hParent)
{
    DWORD dwThreadId;
	DWORD dwThrdParam = (DWORD)hParent; 
    HANDLE hThread; 

    hThread = CreateThread( 
        NULL,                        // default security attributes 
        0,                           // use default stack size  
        ThreadFunc,                  // thread function 
        &dwThrdParam,                // argument to thread function 
        0,                           // use default creation flags 
        &dwThreadId);                // returns the thread identifier  
}

/////////////////////////////////////////////////////////////////////
int WriteToLog(char* str)
{
	FILE* log;

	if (iLog == 0)
		return 0;

	log = fopen(LOGFILE, "a+");
	if (log == NULL)
		return -1;

	fprintf(log, "%s\n", str);
	fclose(log);
	return 0;
}

