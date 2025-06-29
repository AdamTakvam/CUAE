// NeoTTSLicMgr.cpp : Defines the entry point for the console application.
//
// The license encryptor will be added in the project file soon.
/*
C:\Documents and Settings\Administrator>sc config "VT Server" depend= "Cisco UAE License Manager"
[SC] ChangeServiceConfig SUCCESS

C:\Documents and Settings\Administrator>sc config "VT Server" depend= ""
[SC] ChangeServiceConfig SUCCESS
*/

#include "stdafx.h"
#include <stdio.h>
#include <windows.h>
#include <tchar.h>
#include "Decryptor.h"
#include "LicenseFiles.h"
#include "TTSLicMgr.h"
#include "Logger.h"

void WriteLog(char* pFile, char* pMsg) {
	//::EnterCriticalSection(&ttsLicMgr);
	try {
		SYSTEMTIME oT;
		GetLocalTime(&oT);
		FILE* pLog = fopen(pFile,"a");
		fprintf(pLog, "\n%02d:%02d:%02d\n    %s",oT.wHour,oT.wMinute,oT.wSecond,pMsg);
		fclose(pLog);
	} catch(...) {}
	//::LeaveCriticalSection(&ttsLicMgr);
}

bool ttsInstallService()
{
    char strDir[1024];
    SC_HANDLE schSCManager, schService;

    memset(&strDir, 0, sizeof(strDir));
    GetCurrentDirectory(sizeof(strDir), strDir);
    strcat(strDir, "\\");
    strcat(strDir, TTS_LICENSE_APPLICATION_NAME);
    strcat(strDir, " /sS");  // no prompt, start if stopped, stop on the way out

	schSCManager = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);  
 
	if (schSCManager == NULL) 
		return false;

    LPCTSTR lpszBinaryPathName = strDir;

    schService = CreateService(schSCManager,
                                TTS_LICENSE_SERVICE_NAME, 
                                TTS_LICENSE_SERVICE_DISPLAY_NAME,
                                SERVICE_ALL_ACCESS,			// desired access 
                                SERVICE_WIN32_OWN_PROCESS|SERVICE_INTERACTIVE_PROCESS, // service type 
                                SERVICE_AUTO_START,			// start type 
                                SERVICE_ERROR_NORMAL,		// error control type 
                                lpszBinaryPathName,			// service's binary 
                                NULL,						        // no load ordering group 
                                NULL,						        // no tag identifier 
                                NULL,						        // no dependencies 
                                NULL,						        // LocalSystem account 
                                NULL);						        // no password 

    if (schService == NULL) 
        return false;  

    CloseServiceHandle(schService); 

    return true;
}

bool ttsDeleteService()
{
	SC_HANDLE schSCManager;
	SC_HANDLE hService;

	schSCManager = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);
 
	if (schSCManager == NULL) 
		return false;	

	hService=OpenService(schSCManager, TTS_LICENSE_SERVICE_NAME, SERVICE_ALL_ACCESS);

	if (hService == NULL) 
		return false;

	if(DeleteService(hService) == 0)
		return false;

	if(CloseServiceHandle(hService) == 0)
		return false;

	return true;
}

void watchVTServer(char *serviceName) {
	SERVICE_STATUS ssStatus;

	SC_HANDLE schSCManager = ::OpenSCManager(NULL, 
		NULL, 
		SC_MANAGER_ALL_ACCESS);
	char pTemp[121];
    sprintf(pTemp, "\nVT Server Starting Service...\n"); 
	WriteLog(pLogFile, pTemp);
	SC_HANDLE schService = OpenService(
		schSCManager,
		TEXT(serviceName),
		SERVICE_ALL_ACCESS);
	if (schService == NULL) {
        char pTemp[121];
		sprintf(pTemp, "\nCan't open service....error = %d", GetLastError());
	    WriteLog(pLogFile, pTemp);
		return;
	}

	Sleep(10000);

	if (!QueryServiceStatus(
		schService,
		&ssStatus)) {
		char pTemp[121];
        sprintf(pTemp, "\nError in Query Service Status\n"); 
		WriteLog(pLogFile, pTemp);
		return;
	}

	while (ssStatus.dwCurrentState != SERVICE_RUNNING) {
		//Sleep(ssStatus.dwWaitHint);
		Sleep(10000);
		if (!QueryServiceStatus(
			schService,
			&ssStatus) && ssStatus.dwCurrentState == SERVICE_RUNNING) {
				break;
		}
		//char pTemp[121];
        //sprintf(pTemp, "."); 
		//WriteLog(pLogFile, pTemp);
	}
	if (ssStatus.dwCurrentState == SERVICE_RUNNING) {
		char pTemp[121];
        sprintf(pTemp, "\nVT Server just Started\n"); 
		WriteLog(pLogFile, pTemp);
	} else {
		char pTemp[121];
		sprintf(pTemp, " Current State: %d\n", ssStatus.dwCurrentState);
		sprintf(pTemp, " Exit Code: %d\n", ssStatus.dwWin32ExitCode);
		sprintf(pTemp, " Service Specific Exit Code: %d\n", ssStatus.dwServiceSpecificExitCode);
		sprintf(pTemp, " Check Point: %d\n", ssStatus.dwCheckPoint);
		sprintf(pTemp, " Wait Hint: %d\n", ssStatus.dwWaitHint);
		WriteLog(pLogFile, pTemp);
	}
	CloseServiceHandle(schService);
	CloseServiceHandle(schSCManager);

	Sleep(10000);

	DeleteTheFile(decrypted_server_license);
	DeleteTheFile(decrypted_engine_license);

    sprintf(pTemp, "\n2 License Files Deleted\n"); 
	WriteLog(pLogFile, pTemp);
}

BOOL DeleteTheFile( LPCTSTR lpFileName )
{
	//Deletes the file
	BOOL rc = ::DeleteFile( lpFileName );
	
	if ( rc )
		printf( _T("Successfully deleted.\n") );
	else
		printf( _T("Couldn't delete. Error = %d\n"), GetLastError() );

	return rc;
}

static void DecryptLicenseFiles() {
	/* reading license from encrypted file => wrting decrypted file => delete the file
	// this has been commented out due to the change of way to handle license file
	// password in AES is 1234. It needs to be hidden.
	Decryptor *dec_server = new Decryptor(encrypted_server_license, decrypted_server_license, "1234");
	Decryptor *dec_engine = new Decryptor(encrypted_engine_license, decrypted_engine_license, "1234");
	*/
	Decryptor *dec_server = new Decryptor(decrypted_server_license);
	Decryptor *dec_engine = new Decryptor(decrypted_engine_license);   
	/* reading license from encrypted file => wrting decrypted file => delete the file
	// this has been commented out due to the change of way to handle license file
	dec_server->decrypt();
	dec_engine->decrypt();
	*/
	dec_server->decryptToHiddenFile();
	dec_engine->decryptToHiddenFile();
	
	// deallocate objects
	delete dec_server;
	delete dec_engine;
}

int _tmain( int argc, TCHAR* argv[] )
{
	//::InitializeCriticalSection(&ttsLicMgr);
	SYSTEMTIME oT;
	GetLocalTime(&oT);
	sprintf(pLogFile, _T("%04d-%02d-%02d-TTSLicMgr.log"), oT.wYear, oT.wMonth, oT.wDay);
	
    if(argc > 1)
    {
	    if(stricmp(argv[1], SERVICE_INSTALL) == 0)
	    {
            if(ttsInstallService())
                printf("\nMAIN service installed\n");
            else 
                printf("\nMAIN could not install service\n");
            return 0;
		}
        else if(stricmp(argv[1], SERVICE_UNINSTALL) == 0)
        {
            if(ttsDeleteService())
                printf("\nMAIN service uninstalled\n");
            else 
                printf("\nMAIN could not uninstall service\n");
            return 0;
		}
		else if(stricmp(argv[1], SERVICE_RUN) ==0 )
		{
            runningInServiceMode = true;
        }
    }
    if (runningInServiceMode)   // run in service mode
    {
		SERVICE_TABLE_ENTRY DispatchTable[] = {{TTS_LICENSE_SERVICE_NAME, ServiceMain}, {NULL, NULL}};
		StartServiceCtrlDispatcher(DispatchTable);
		return 0;
    }
    else // quit
    {   
        return 0;
    }
}

void WINAPI ServiceCtrlHandler(DWORD Opcode)
{
  switch(Opcode)
  {
    case SERVICE_CONTROL_PAUSE: 
      m_ServiceStatus.dwCurrentState = SERVICE_PAUSED;
      break;
    case SERVICE_CONTROL_CONTINUE:
      m_ServiceStatus.dwCurrentState = SERVICE_RUNNING;
      break;
    case SERVICE_CONTROL_STOP:
      m_ServiceStatus.dwWin32ExitCode = 0;
      m_ServiceStatus.dwCurrentState = SERVICE_STOPPED;
      m_ServiceStatus.dwCheckPoint = 0;
      m_ServiceStatus.dwWaitHint = 0;

      SetServiceStatus (m_ServiceStatusHandle,&m_ServiceStatus);
      bRunning=false;
      break;
    case SERVICE_CONTROL_INTERROGATE:
      break; 
  }
  return;
}

void WINAPI ServiceMain(DWORD argc, LPTSTR *argv)
{
	

	m_ServiceStatus.dwServiceType = SERVICE_WIN32;
	m_ServiceStatus.dwCurrentState = SERVICE_START_PENDING;
	m_ServiceStatus.dwControlsAccepted = SERVICE_ACCEPT_STOP;
	m_ServiceStatus.dwWin32ExitCode = 0;
	m_ServiceStatus.dwServiceSpecificExitCode = 0;
	m_ServiceStatus.dwCheckPoint = 0;
	m_ServiceStatus.dwWaitHint = 0;

	m_ServiceStatusHandle = RegisterServiceCtrlHandler(TTS_LICENSE_SERVICE_NAME, ServiceCtrlHandler); 
	if (m_ServiceStatusHandle == (SERVICE_STATUS_HANDLE)0)
	{
		return;
	}
	m_ServiceStatus.dwCurrentState = SERVICE_RUNNING;
	m_ServiceStatus.dwCheckPoint = 0;
	m_ServiceStatus.dwWaitHint = 0;
	if (!SetServiceStatus (m_ServiceStatusHandle, &m_ServiceStatus))
	{
	}

	DecryptLicenseFiles();

	//bRunning = true;
	//while(bRunning)
	//{
	Sleep(3000);
	processLic();
	//}
	char pTemp[121];
    sprintf(pTemp, "\nDone................\n"); 
	WriteLog(pLogFile, pTemp);
	return;
}

void processLic() {

	watchVTServer(TTS_SERVER_NAME);
	//bRunning = false;
	//::DeleteCriticalSection(&ttsLicMgr);
}



