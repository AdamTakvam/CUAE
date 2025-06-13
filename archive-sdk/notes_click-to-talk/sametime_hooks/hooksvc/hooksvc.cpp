// hooksvc.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <windows.h>
#include <stdio.h>
#include <Winsvc.h>
#include "hooksvc.h"

/////////////////////////////////////////////////////////////////////
#define SERVICE_DISPLAY_NAME	"Metreos Hook Service"
#define SERVICE_NAME			"MetreosHookService"
#define APPLICATION_NAME		"hooksvc.exe"
#define SLEEP_TIME				1000
#define LOGFILE					"c:\\hooksvc.log"

/////////////////////////////////////////////////////////////////////
SERVICE_STATUS ServiceStatus; 
SERVICE_STATUS_HANDLE hStatus; 

/////////////////////////////////////////////////////////////////////
typedef VOID (CALLBACK* installhook)();
typedef VOID (CALLBACK* uninstallhook)();

/////////////////////////////////////////////////////////////////////
static HINSTANCE hinstDLL = NULL; 
installhook ih;
uninstallhook uh;

 
/////////////////////////////////////////////////////////////////////
int main(int argc, char* argv[]) 
{ 
	if(argc > 1)
	{
		if(strcmp(argv[1], "-i") == 0)
		{
			// Install the service
			if(InstallService())
				printf("\n\nService Installed Sucessfully.\n");
			else
				printf("\n\nInstall Service Failed.\n");
		}
		else if(strcmp(argv[1],"-d")==0)
		{
			// Uninstall the service
			if(DeleteService())
				printf("\n\nService Uninstalled Sucessfully.\n");
			else
				printf("\n\nUninstalling Service Failed.\n");
		}
		else
		{
			printf("\n\nTo Install Service use hooksvc -i\n\nTo Uninstall Service use hooksvc -d\n");
		}
	}
	else
	{
		// Running the service
		SERVICE_TABLE_ENTRY ServiceTable[2];
		ServiceTable[0].lpServiceName = SERVICE_NAME;
		ServiceTable[0].lpServiceProc = (LPSERVICE_MAIN_FUNCTION)ServiceMain;

		ServiceTable[1].lpServiceName = NULL;
		ServiceTable[1].lpServiceProc = NULL;

		// Start the control dispatcher thread for our service
		StartServiceCtrlDispatcher(ServiceTable);  
	}

	return 0;
}

/////////////////////////////////////////////////////////////////////
void ServiceMain(int argc, char** argv) 
{ 
    ServiceStatus.dwServiceType = SERVICE_WIN32; 
    ServiceStatus.dwCurrentState = SERVICE_START_PENDING; 
    ServiceStatus.dwControlsAccepted = SERVICE_ACCEPT_STOP | SERVICE_ACCEPT_SHUTDOWN;
    ServiceStatus.dwWin32ExitCode = 0; 
    ServiceStatus.dwServiceSpecificExitCode = 0; 
    ServiceStatus.dwCheckPoint = 0; 
    ServiceStatus.dwWaitHint = 0; 
 
    hStatus = RegisterServiceCtrlHandler(SERVICE_NAME, (LPHANDLER_FUNCTION)ControlHandler); 
    if (hStatus == (SERVICE_STATUS_HANDLE)0) 
    { 
		WriteToLog("Registering Control Handler failed.");
        return; 
    }  

    // Initialize Service 
    int error = InitService(); 
    if (error) 
    {
        ServiceStatus.dwCurrentState = SERVICE_STOPPED; 
        ServiceStatus.dwWin32ExitCode = -1; 
        SetServiceStatus(hStatus, &ServiceStatus);
		WriteToLog("Initialization failed.");
        return; 
    } 

    // We report the running status to SCM. 
    ServiceStatus.dwCurrentState = SERVICE_RUNNING; 
    SetServiceStatus (hStatus, &ServiceStatus);

	WriteToLog("Service is running.");
 
    // The worker loop of a service
    while (ServiceStatus.dwCurrentState == SERVICE_RUNNING)
	{
		// TODO: Fill in some action if necessary

		Sleep(SLEEP_TIME);
	}
    return; 
}
 
/////////////////////////////////////////////////////////////////////
int InitService() 
{ 
	return StartHooks();
} 

/////////////////////////////////////////////////////////////////////
void ControlHandler(DWORD request) 
{ 
    switch(request) 
    { 
        case SERVICE_CONTROL_STOP: 
			StopHooks();
			ServiceStatus.dwWin32ExitCode = 0; 
			ServiceStatus.dwCurrentState  = SERVICE_STOPPED; 
			SetServiceStatus (hStatus, &ServiceStatus);
			return; 
 
        case SERVICE_CONTROL_SHUTDOWN: 
			StopHooks();
			ServiceStatus.dwWin32ExitCode = 0; 
			ServiceStatus.dwCurrentState  = SERVICE_STOPPED; 
			SetServiceStatus (hStatus, &ServiceStatus);
			return; 
        
        default:
            break;
    } 
 
    // Report current status
    SetServiceStatus (hStatus,  &ServiceStatus); 
    return; 
} 

/////////////////////////////////////////////////////////////////////
int StartHooks()
{
	hinstDLL = LoadLibrary("hookdll"); 

	if (hinstDLL == NULL)
	{
		WriteToLog("Unable to load hookdll.");
		return -1;
	}

	ih = (installhook)GetProcAddress(hinstDLL, "InstallHook"); 
	if (ih == NULL)
	{
		WriteToLog("Unable to get proc address for InstallHook.");
		return -1;
	}

	ih();	

	WriteToLog("Hook Started.");

	return 0;
}

/////////////////////////////////////////////////////////////////////
int StopHooks()
{
	if (hinstDLL != NULL)
	{
		uh = (uninstallhook)GetProcAddress(hinstDLL, "UninstallHook");

		if (uh != NULL)
			uh();	
		else
			WriteToLog("Unable to get proc address for UninstallHook.");

		FreeLibrary(hinstDLL);

		hinstDLL = NULL;
	}

	WriteToLog("Hook Stopped.");

	return 0;
}

/////////////////////////////////////////////////////////////////////
BOOL InstallService()
{
	char strDir[1024];
	SC_HANDLE schSCManager, schService;

	memset(&strDir, 0, sizeof(strDir));
	GetCurrentDirectory(1024, strDir);
	strcat(strDir, "\\");
	strcat(strDir, APPLICATION_NAME);

	schSCManager = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);  
 
	if (schSCManager == NULL) 
		return FALSE;

    LPCTSTR lpszBinaryPathName = strDir;
 
    schService = CreateService(schSCManager,
		SERVICE_NAME, 
		SERVICE_DISPLAY_NAME,
        SERVICE_ALL_ACCESS,			// desired access 
        SERVICE_WIN32_OWN_PROCESS|SERVICE_INTERACTIVE_PROCESS, // service type 
        SERVICE_AUTO_START,			// start type 
        SERVICE_ERROR_NORMAL,		// error control type 
        lpszBinaryPathName,			// service's binary 
        NULL,						// no load ordering group 
        NULL,						// no tag identifier 
        NULL,						// no dependencies 
        NULL,						// LocalSystem account 
        NULL);						// no password 
 
    if (schService == NULL) 
        return FALSE;  
 
    CloseServiceHandle(schService); 

	return TRUE;
}


/////////////////////////////////////////////////////////////////////
BOOL DeleteService()
{
	SC_HANDLE schSCManager;
	SC_HANDLE hService;

	schSCManager = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);
 
	if (schSCManager == NULL) 
		return FALSE;	

	hService=OpenService(schSCManager, SERVICE_NAME, SERVICE_ALL_ACCESS);

	if (hService == NULL) 
		return FALSE;

	if(DeleteService(hService) == 0)
		return FALSE;

	if(CloseServiceHandle(hService) == 0)
		return FALSE;
	else
		return TRUE;
}

/////////////////////////////////////////////////////////////////////
int WriteToLog(char* str)
{
#ifdef _DEBUG
	FILE* log;
	log = fopen(LOGFILE, "a+");
	if (log == NULL)
		return -1;

	fprintf(log, "%s\n", str);
	fclose(log);
#endif
	return 0;
}


