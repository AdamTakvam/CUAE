#define SERVICE_INSTALL         "/sI"
#define SERVICE_UNINSTALL       "/sD"
#define SERVICE_RUN             "/sS"

#define BUF_SIZE 0x200

#define TTS_LICENSE_SERVICE_DISPLAY_NAME	"Cisco UAE TTS License Manager"  
#define TTS_LICENSE_SERVICE_NAME			"TTSLicenseServer"  
#define TTS_LICENSE_APPLICATION_NAME		"ttslicmanager.exe"
#define TTS_SERVER_NAME						"VT Server"

const int nBufferSize = 500;

CHAR pLogFile[nBufferSize+1];

bool runningInServiceMode = false;

BOOL DeleteTheFile( LPCTSTR lpFileName );

void WINAPI ServiceMain(DWORD argc, LPTSTR *argv);
SERVICE_STATUS m_ServiceStatus;
SERVICE_STATUS_HANDLE m_ServiceStatusHandle;
BOOL bRunning = true;
CRITICAL_SECTION ttsLicMgr;

void processLic();
