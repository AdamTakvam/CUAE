// hmpsvc.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <windows.h>
#include <stdio.h>
#include <Winsvc.h>
#include <srllib.h>
#include <gclib.h>
#include <ipmlib.h>
#include <dtilib.h>
#include <dxxxlib.h>
#include <msilib.h>
#include <dcblib.h>
#include <errno.h>
#include <iostream>

#include "hmpsvc.h"

/////////////////////////////////////////////////////////////////////
#define SERVICE_DISPLAY_NAME	"HMP Test Service"
#define SERVICE_NAME			"HMPTestService"
#define APPLICATION_NAME		"hmpsvc.exe"
#define SLEEP_TIME				1000
#define LOGFILE					"c:\\hmpsvc.log"

/////////////////////////////////////////////////////////////////////
SERVICE_STATUS ServiceStatus; 
SERVICE_STATUS_HANDLE hStatus; 

/////////////////////////////////////////////////////////////////////
int  result, handleIP, handleConf, conferenceID;
long iptimeslotnum, listentimeslot;
IPM_MEDIA_INFO mediaInfoLocal;
SC_TSINFO slotinfo; 
MS_CDT cdt; 
char* ipKey="ipmB1C1", *confKey="dcbB1D1";
typedef long (*HMPEVENTHANDLER)(unsigned long);
long hmpEventHandler(long whatever) { return 0; }

 
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
			printf("\n\nTo Install Service use hmpsvc -i\n\nTo Uninstall Service use hmpsvc -d\n");
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
int registerHMPEventHandler(bool b)        
{ 
  return b?                         
  sr_enbhdlr(EV_ANYDEV, EV_ANYEVT, (HMPEVENTHANDLER)hmpEventHandler):     
  sr_dishdlr(EV_ANYDEV, EV_ANYEVT, (HMPEVENTHANDLER)hmpEventHandler);  
}

/////////////////////////////////////////////////////////////////////
int initHMP()
{
    registerHMPEventHandler(true);
    if  (sr_getboardcnt("ipm", &result) == -1) 
        return -1;
    if  (sr_getboardcnt(DEV_CLASS_VOICE, &result) == -1) 
        return -1;
    if  (sr_getboardcnt("dcb", &result) == -1) 
        return -1;
    if  (-1 == (handleIP = ipm_Open("ipmB1", 0, EV_SYNC))) 
        return -1;
    ipm_Close(handleIP, EV_SYNC);
    if  (-1 == (handleConf = dcb_open("dcbB1",0))) 
        return -1;
    dcb_close(handleConf);
    return 0;
}

/////////////////////////////////////////////////////////////////////
int StartHMP()
{
    if (-1 == initHMP())
    {
        WriteToLog("Coulnd not init HMP.");
        return -1;
    }
    
    handleIP = ipm_Open(ipKey, 0, EV_SYNC);              
    if (handleIP == -1) 
    {
        WriteToLog("ipm_Open failed.");
        return -1;
    }

    result = ipm_GetLocalMediaInfo(handleIP, &mediaInfoLocal, EV_SYNC);
    if (result == -1)
    {
        WriteToLog("ipm_GetLocalMediaInfo failed.");
        return -1;
    }

    handleConf = dcb_open(confKey, 0); 
    if (handleConf == -1) 
    {
        WriteToLog("dcb_open failed.");
        return -1;
    }

    slotinfo.sc_numts = 1;
    slotinfo.sc_tsarrayp = &iptimeslotnum;

    if (ipm_GetXmitSlot(handleIP, &slotinfo, EV_SYNC) == -1) 
    {
        WriteToLog("ipm_GetXmitSlot failed.");
        return -1;
    }

    cdt.chan_num  = iptimeslotnum;
    cdt.chan_sel  = MSPN_TS;
    cdt.chan_attr = MSPA_NULL;
                                             
    result = dcb_estconf(handleConf, &cdt, 1, 0, &conferenceID);
    if  (result == -1) 
    {
        WriteToLog("dcb_estconf failed.");
        return -1;
    }

    listentimeslot = cdt.chan_lts;   
    slotinfo.sc_tsarrayp = &listentimeslot;

    if  (ipm_Listen(handleIP, &slotinfo, EV_SYNC) == -1) 
    {
        WriteToLog("ipm_Listen failed.");
        return -1;
    }

    return 0;
}

/////////////////////////////////////////////////////////////////////
int StopHMP()
{
    if (ipm_UnListen(handleIP, EV_SYNC) == -1) 
    {
        WriteToLog("ipm_UnListen failed.");
        return -1;
    }

    if (ipm_Stop(handleIP, STOP_ALL, EV_SYNC) == -1)
    {
        WriteToLog("ipm_Stop failed.");
        return -1;
    }
   
    if  (dcb_delconf(handleConf, conferenceID) == -1) 
    {
        WriteToLog("dcb_delconf failed.");
        return -1;
    }

    if  (dcb_close(handleConf)  == -1)  
    {
        WriteToLog("dcb_close failed.");
        return -1;
    }

    if  (ipm_Close(handleIP, 0) == -1)
    {
        WriteToLog("ipm_Close failed.");
        return -1;
    }

	return 0;
}
 
/////////////////////////////////////////////////////////////////////
int InitService() 
{ 
	return StartHMP();
} 

/////////////////////////////////////////////////////////////////////
void ControlHandler(DWORD request) 
{ 
    switch(request) 
    { 
        case SERVICE_CONTROL_STOP: 
			StopHMP();
			ServiceStatus.dwWin32ExitCode = 0; 
			ServiceStatus.dwCurrentState  = SERVICE_STOPPED; 
			SetServiceStatus (hStatus, &ServiceStatus);
			return; 
 
        case SERVICE_CONTROL_SHUTDOWN: 
			StopHMP();
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


