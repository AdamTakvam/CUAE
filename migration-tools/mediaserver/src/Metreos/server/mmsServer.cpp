// 
// mmsServer.cpp  
//
// Cisco Unified Media Engine entry point/main thread
//
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
// To run a memory leak check on the media engine, do the following:
//
// 1. Uncomment the line #define MMS_ENABLE_MEMORY_LEAK_DETECTION
//    in mmsLeakTest.h (in the mms project). This switch permits leak detection
//    code to be compiled into the solution at various places.  
//
// 2. Do a "Rebuild All" on the entire solution.
//
// 3. Start media engine in the debugger (F5)
//
// 4. Run an application which uses the media engine, such as one test from 
//    the mqClientTest test suite.
//
// 5. When the application completes, shut down the media engine (using mmswin.exe 
//    or mmsstop.exe, if possible, rather than simply doing a Stop Debugging,  
//    in order that the media engine code runs to completion).
//
// 6. Open the Visual Studio Output Window. Any leaks will be displayed at
//    the end of the Output Window text, with the source file name and line
//    number of each. At least one leak will be displayed, as shown following,
//    a 20-byte allocation generated in main() below, containing the text,
//    "IGNORE THIS LEAK". This leak is generated only when leak detection is 
//    enabled, in order that you can verify that the project is built such that 
//    leaks are displayed. Any leaks other than this should be found and fixed.
//
//    Detected memory leaks!
//    Dumping objects ->
//    \x\mediaserver\src\Metreos\server\mmsServer.cpp(759) : {178} ... 20 bytes long
//    Data: <IGNORE THIS LEAK> 49 47 4E 4F 52 45 20 54 48 49 53 20 4C 45 41 4B 
//    Object dump complete.
// 
// 7. The number in brackets ( {178} in the example above) is the sequential number  
//    of the heap allocation which leaked. If you want to debug break at this allo-
//    cation, set config item Diagnostics.debugBreakAtAllocation = 178. If you need
//    to break at an allocation occurring prior to the point that the config file 
//    is read, uncomment the _CrtSetBreakAlloc in main() below, and recompile, for
//    example _CrtSetBreakAlloc(178);  
//
//    Note that any allocations not using operator new are not currently detected. 
//    Also note that only leaks in this solution are detected. DLLs since separately 
//    compiled, are presumably not invoking the debug heap allocator, and thus any 
//    leaks therein will not be displayed.
//
// 8. When through with memory leak detection. re-comment the line
//    // #define MMS_ENABLE_MEMORY_LEAK_DETECTION in mmsLeakTest.h
//    and do a "Rebuild All" on the entire solution. 
// 
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
#include "StdAfx.h"

#define BUILDBANNER "Cisco Unified Media Engine v2.4.0000\n" 

#pragma warning(disable:4786)
#include "mms.h"
#include "mmsCmdline.h"
#include "mmsConfig.h"
#include "mmsLogger.h"
#include "mmsReporter.h"
#include "mmsMsgTypes.h"
#include "mmsServerManager.h"
#include "mmsSessionManager.h"
#include "mmsConferenceManager.h"
#include "mmsMediaEvent.h"
#include "mmsMediaResourceMgr.h"
#include "mmsServerCmdHeader.h"
#include "mmsReactorFactory.h"
#include "mmsTimerHandler.h"
#include "mmsThreadPool.h"
#include "mmsException.h"

#include "mmsServerControlAdapterW.h"
#include "mmsMqAppAdapter.h"
#include "mmsMSMQAppAdapter.h"
#include "mmsFlatmapIpcAppAdapter.h"
#include "mmsHmpControl.h"
#include "ace/Trace.h"
#include "mmsAsr.h"

// Console title must be #defined identically to this in mmswin 
#define MMSCONTITLE "Cisco Unified Media Engine Console" 

#define MAX_IPC_ADAPTERS 6
#undef  MMS_USING_MESSAGE_FACTORY  

#include <conio.h>
#include <sys\stat.h>

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION     // mmsLeakTest.h 
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

ReactorFactory reactorFactory;
const char* adapterGlutMsg = "MAIN too many adapters\n";
const char* adapterOKmask  = "MAIN installed adapter '%s'\n";
const char* adapterErrmask = "MAIN error installing adapter %s\n";
const char* couldNotStart  = "media engine could not be started";
const char* hmpStartError  = "HMP media firmware could not be initialized";
int  isShutdown, maxShutdownHmpWaitSecs, serverStartedOK, isHmpIssue;
char c;

bool runningInServiceMode = false;
MmsCmdline* pCmdline = 0;

#ifdef  MMS_WINPLATFORM                     // Windows service 
#define MMS_SERVICE_ACTION_REGISTER 1
#define MMS_SERVICE_ACTION_START    2
#define MMS_SERVICE_ACTION_RUN      3
#define MMS_SERVICE_ACTION_STOP     4
#define MMS_SERVICE_ACTION_SHUTDOWN 5

#define SERVICE_DELAY_TWO_MINUTES 120000
#define MMS_SERVICE_STARTUP_WAITTIME  SERVICE_DELAY_TWO_MINUTES
                   
#define MMS_SERVICE_DISPLAY_NAME "Cisco Unified Media Engine Service"  
#define MMS_SERVICE_NAME         "MediaServerService"  
#define MMS_APPLICATION_NAME     "mmsserver.exe"

int  core_main();
void mmsServiceControlHandler(DWORD request);

MmsTask* mmsServerManager;
SERVICE_STATUS ServiceStatus;
SERVICE_STATUS_HANDLE hStatus;                        
#endif  // #ifdef MMS_WINPLATFORM



class MmsInstalledIpcAdapters              // IPC adapters manager
{
  public:

  int insert(MmsIpcAdapter* adapter)
  {
    int  index  = installedCount >= MAX_IPC_ADAPTERS? -1: installedCount;
    if  (index != -1)
         installedAdapters[installedCount++] = adapter;
    return index;
  }

  void deleteAll()
  {
    MmsIpcAdapter** adapter = &installedAdapters[0];
    for(int i=0; i < installedCount; i++, adapter++)
        if  (*adapter)
             delete *adapter;
  }

  int isSlotAvailable() { return installedCount < MAX_IPC_ADAPTERS; }

  MmsIpcAdapter* installedAdapters[MAX_IPC_ADAPTERS];
  int installedCount;

  MmsInstalledIpcAdapters(): installedCount(0) 
  {
    memset(installedAdapters, 0, sizeof(MmsIpcAdapter*) * MAX_IPC_ADAPTERS);
  }
};



int verifyDirectory(char* path)
{
  char dirpath[MAXPATHLEN]; ACE_OS::strcpy(dirpath, path);
  stripLogFilename(dirpath);

  struct stat statinfo; memset(&statinfo, 0, sizeof(struct stat));
  stat(dirpath, &statinfo);
  if ((statinfo.st_mode & S_IFMT) != S_IFDIR) // if dir nonexistent ...
       return (0 == _mkdir(dirpath))? 1: -1;  // ... create directory    
         
  return 0;      
}



void verifyLogDirectory(MmsConfig* config)
{
  // Verifies that the configured log file directory exists, and if not creates
  // a directory with that name. If this fails it verifies and if necessary 
  // creates the default log directory as defined in mmsconfig.h

  char* dir   = config->serverLogger.filepath;
  int  result = verifyDirectory(dir);
  char altpath[] = MMS_SERVER_LOGGER_LOGFILEPATH;

  if (result < 0)
      if (1 == (result = verifyDirectory(altpath)))
          ACE_OS::strcpy(config->serverLogger.filepath, altpath);       

  switch(result)
  { case  1: ACE_OS::printf("MAIN log directory did not exist and was created\n"); break;
    case -1: ACE_OS::printf("MAIN log directory did not exist and could not be created\n");
  }
}
  


MmsLogger* createLogger(MmsConfig* config)
{ 
  MmsLogger::LoggerParams params(0, GROUPID_LOGGER, NULL); 
  ACE_OS::strcpy(params.taskName, "LOGX"); 
  
  params.threadCount     = config->serverLogger.numThreads;
  params.defaultMsgLevel = config->serverLogger.defaultMessageLevel;   
  params.currentMsgLevel = config->serverLogger.globalMessageLevel; 
  params.isTimestamped   = config->serverLogger.timestamp;
  params.config = config;
  verifyLogDirectory(config);
                                            
  if  (config->serverLogger.destStdout)     // Specify log destinations
       params.destinationFlags |= MmsLogDestination::STDOUT; 
 
  if  (config->serverLogger.destDebug)      
       params.destinationFlags |= MmsLogDestination::DEBUG; 

  if  (config->serverLogger.destFile)  
  {   
       params.destinationFlags |= MmsLogDestination::LOCALFILE; 
       if  (config->serverLogger.filepath[0])   
       {  
            ACE_OS::strcpy(params.path, config->serverLogger.filepath); 
            params.isFullPath = config->serverLogger.isFullpath; 
       }
  } 

  if  (config->serverLogger.destSocket) 
  {
       params.destinationFlags |= MmsLogDestination::SOCKET; 
       params.remoteport = config->serverLogger.port;
  }   

  if  (config->serverLogger.destLogServer) 
  {
       params.destinationFlags |= MmsLogDestination::LOGSERVER; 
  }     
                                         
                                            // Instantiate logger & open logs
  MmsLogger* logger = new MmsLogger(&params);
  return logger;
}



MmsConfig* getConfigurationInfo()           // Config file object
{                                            
  MmsConfig* config = new MmsConfig;         

  if  (-1 == config->readLocalConfigFile())  
  {    ACE_OS::printf("MAIN could not read config file\n");
       delete config;
       config = NULL;
  }
  else
  {
       #ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION   
       const int whichalloc = config->diagnostics.debugBreakAtAllocation;
       if  (whichalloc)
            _CrtSetBreakAlloc(whichalloc);
       #endif
  }

  return config;
}



int createResourceManager(MmsConfig* config, MmsTask* reporter, HmpResourceManager** outresmgr)
{                                           // Instantiate resource manager
  HmpResourceManager* resourceManager = HmpResourceManager::instance();
                                            // Inventory & open media resources
  const int result = resourceManager && (-1 != resourceManager->init(config))? 0: -1; 

  if  (result == -1) 
       ACE_OS::printf("MAIN could not open media resources\n");
  else resourceManager->reporterQueue = reporter;
     
  *outresmgr = resourceManager;     
  return result;
}



MmsTimerHandler* createTimerManager(MmsTask* parent,
  ACE_Reactor* reactor, MmsLogger* logger, MmsConfig* config)        
{ 
  MmsTask::InitialParams params(0, GROUPID_TIMERMON, parent);
  ACE_OS::strcpy(params.taskName, "TIM1");
  params.baselinePriority = logger->getOsPriority();
  params.relativePriority = MmsTask::InitialParams::threadPriority::ABOVENORMAL;
                                            // Dedicated reactor for timers
  params.reactor     = reactor;  
  params.logCallback = logger;              // Routing for log records
  params.config      = config;
  params.defaultMsgLevel = config->serverLogger.defaultMessageLevel;   
  params.currentMsgLevel = config->serverLogger.globalMessageLevel; 

  MmsTimerHandler* timerhandler = new MmsTimerHandler(&params);
  return timerhandler;
}



MmsConferenceManager* createConferenceManager
( MmsConfig* config, HmpResourceManager* resmgr, MmsTask* smgr)       
{ 
  MmsConferenceManager* conferenceMgr = new MmsConferenceManager(config, resmgr, smgr);
  return conferenceMgr;
}



MmsReporter* createReporter(MmsLogger* logger, MmsConfig* config)       
{ 
  MmsTask::InitialParams params(0, GROUPID_REPORTER, NULL);
  ACE_OS::strcpy(params.taskName, "REPO");
  params.config = config;
  params.logCallback = logger;

  MmsReporter* reporter = new MmsReporter(&params);
  return reporter;
}



MmsThreadPool* createThreadPool(int numThreads, MmsLogger* logger, 
  MmsConfig* config, MmsServerManager* serverMgr, HmpResourceManager* resourceMgr)
{
  MmsTask::InitialParams params(0, GROUPID_THREADPOOL, serverMgr);
  ACE_OS::strcpy(params.taskName, "POOL");
  params.logCallback = logger;
  params.config      = config;
  params.parent      = serverMgr;
  params.user        =(unsigned long)resourceMgr;
  params.threadCount = numThreads;          // Service threads do their own
  params.isThreadFiltersLogMsgs = TRUE;     // log priority threshold testing
  params.defaultMsgLevel = config->serverLogger.defaultMessageLevel;   
  params.currentMsgLevel = config->serverLogger.globalMessageLevel; 

  MmsThreadPool* threadPool = new MmsThreadPool(&params);

  MmsThreadPool::policeDirectoriesCallback = MmsServerManager::policeDirectoriesCallback;
  return threadPool;
}



MmsServerManager* createServerManager
( MmsLogger* logger, MmsConfig* config, HmpResourceManager* resourceManager, MmsTask* reporter)
{
  MmsTask::InitialParams params(0, GROUPID_SERVER_MANAGER, NULL);
  ACE_OS::strcpy(params.taskName, "SERV");
  params.logCallback = logger;
  params.config   = config;
  params.reporter = reporter;
  params.user =(unsigned long)resourceManager;
  params.defaultMsgLevel = config->serverLogger.defaultMessageLevel;   
  params.currentMsgLevel = config->serverLogger.globalMessageLevel; 

  MmsServerManager*  serverMgr = new MmsServerManager(&params);
  mmsServerManager = serverMgr;  // supply messaging capability to service
  return serverMgr;
}



MmsSessionManager* createSessionManager
( MmsLogger* logger, MmsConfig* config, MmsServerManager* serverMgr, 
  MmsThreadPool* threadPool, MmsTimerHandler* timerMgr, 
  HmpResourceManager* resourceMgr, MmsConferenceManager* conferenceMgr, MmsTask* reporter)
{
  MmsSessionManager::SessionManagerParams params(0, GROUPID_SESSION_MANAGER, NULL);
  ACE_OS::strcpy(params.taskName, "SMGR");
  params.logCallback   = logger;
  params.config        = config;
  params.reporter      = reporter;
  params.parent        = serverMgr;
  params.sessionPool   = serverMgr->sessionPool();  
  params.threadPool    = threadPool;
  params.timerManager  = timerMgr;
  params.resourceMgr   = resourceMgr;
  params.conferenceMgr = conferenceMgr;
  params.defaultMsgLevel = config->serverLogger.defaultMessageLevel;   
  params.currentMsgLevel = config->serverLogger.globalMessageLevel; 

  MmsSessionManager* sessionMgr = new MmsSessionManager(&params);
  return sessionMgr;
}



int installGuiAdapterWin(MmsInstalledIpcAdapters& adapters, MmsServerManager* serverMgr, 
  int ordinal, MmsConfig* config, MmsLogger* logger, MmsTask* reporter)
{
  if  (!adapters.isSlotAvailable()) 
  {    ACE_OS::printf(adapterGlutMsg);
       return -1;
  }

  char adapterLogName[5] = "WGUI";
  MmsTask::InitialParams params(ordinal, GROUPID_IPC_LISTENER, serverMgr);
  ACE_OS::strcpy(params.taskName, adapterLogName);
  params.user = (unsigned long)serverMgr;
  params.logCallback = logger;
  params.config   = config;  
  params.reporter = reporter; 
  params.defaultMsgLevel = config->serverLogger.defaultMessageLevel;   
  params.currentMsgLevel = config->serverLogger.globalMessageLevel; 

  MmsIpcAdapter* adapter = new MmsServerControlAdapterW(&params);
  if  (adapter == NULL)  
  {    ACE_OS::printf(adapterErrmask, adapterLogName);
       return -1;
  }

  ACE_OS::printf(adapterOKmask, adapterLogName);
  adapters.insert(adapter);
                                             
  adapter->start();                          

  mmsSleep(MMS_N_MS(500));
  return 0;
}



int installMetreosMqAdapter(MmsInstalledIpcAdapters& adapters, MmsServerManager* serverMgr, 
  int ordinal, MmsConfig* config, MmsLogger* logger, MmsTask* reporter)
{
  if  (!adapters.isSlotAvailable()) 
  {    ACE_OS::printf(adapterGlutMsg);
       return -1;
  }

  char adapterLogName[5] = "APMQ";
  MmsTask::InitialParams params(ordinal, GROUPID_IPC_LISTENER, serverMgr);
  ACE_OS::strcpy(params.taskName, adapterLogName);
  params.user = (unsigned long)serverMgr;
  params.logCallback = logger;
  params.config   = config;
  params.reporter = reporter;
  params.defaultMsgLevel = config->serverLogger.defaultMessageLevel;   
  params.currentMsgLevel = config->serverLogger.globalMessageLevel; 

  MmsIpcAdapter* adapter = new MmsMSMQAppAdapter(&params);
  if  (adapter == NULL)  
  {    ACE_OS::printf(adapterErrmask, adapterLogName);
       return -1;
  }

  ACE_OS::printf(adapterOKmask, adapterLogName);
  adapters.insert(adapter);
                                             
  adapter->start();                          

  mmsSleep(1);
  return 0;
}



int installFlatmapIpcAdapter(MmsInstalledIpcAdapters& adapters, MmsServerManager* serverMgr, 
  int ordinal, MmsConfig* config, MmsLogger* logger, MmsTask* reporter)
{
  if  (!adapters.isSlotAvailable()) 
  {    ACE_OS::printf(adapterGlutMsg);
       return -1;
  }

  char adapterLogName[5] = "IPCF";
  MmsTask::InitialParams params(ordinal, GROUPID_IPC_LISTENER, serverMgr);
  ACE_OS::strcpy(params.taskName, adapterLogName);
  params.user = (unsigned long)serverMgr;
  params.logCallback = logger;
  params.config   = config;
  params.reporter = reporter;
  params.defaultMsgLevel = config->serverLogger.defaultMessageLevel;   
  params.currentMsgLevel = config->serverLogger.globalMessageLevel; 

  MmsIpcAdapter* adapter = new MmsFlatmapIpcAppAdapter(config->serverParams.defaultFlatmapIpcPort, &params);
  if  (adapter == NULL)  
  {    ACE_OS::printf(adapterErrmask, adapterLogName);
       return -1;
  }

  ACE_OS::printf(adapterOKmask, adapterLogName);
  adapters.insert(adapter);
                                             
  adapter->start();                          

  mmsSleep(1);
  return 0;
}



int installProtocolAdapters(MmsInstalledIpcAdapters& adapters, 
  MmsServerManager* serverMgr, MmsConfig* config, MmsLogger* logger, MmsTask* reporter)
{
  int numAdaptersInstalled = 0;
  unsigned long requestedAdapters = config->clientParams.ipcAdapters;

  // The primary adapter should be first in list, since this is the 
  // adapter which will be pinged periodically by server manager

  if (requestedAdapters & MSMQ_METREOS)           
  {
      if  (-1 == installMetreosMqAdapter
          (adapters, serverMgr, numAdaptersInstalled, config, logger, reporter))
           return -1;
      else ++numAdaptersInstalled;
  } 


  if (requestedAdapters & FLATMAP_IPC)           
  {
      if  (-1 == installFlatmapIpcAdapter
          (adapters, serverMgr, numAdaptersInstalled, config, logger, reporter))
           return -1;
      else ++numAdaptersInstalled;
  }                    


  if (requestedAdapters & GUI_METREOS_SERVERCONTROL_WINDOWS)      
  {
      if  (-1 == installGuiAdapterWin
          (adapters, serverMgr, numAdaptersInstalled, config, logger, reporter))
           return -1;
      else ++numAdaptersInstalled;
  } 
                          

  if  (numAdaptersInstalled == 0)
       ACE_OS::printf("MAIN no adapters installed, cannot continue\n");

  return numAdaptersInstalled;
}



int verifyHmpService(MmsConfig* config, MmsCmdline& cmdline)
{
  // Check state of HMP and start if necessary and if requested
  
  if (cmdline.startHmpService == NP_START_HMP_NEVER) return 0;
  int verify = cmdline.startHmpService == NP_START_HMP_IFSTOP;
  int result = hmpStart(config, verify);    // mmsHmpControl
  if (result < 0) ACE_OS::printf("MAIN could not start service\n");
  return result;
}



int setExceptionTrap(MmsConfig* config)
{
  // Set handler to write dump and force server on fatal exception
  int result = config->serverParams.setUnhandledExceptionTrap? -1: 0;

  #ifdef MMS_WINPLATFORM                      
  if (config->serverParams.setUnhandledExceptionTrap)
  {                                                                                     
      MmsUnhandledExcpTrap* excpHandler = MmsUnhandledExcpTrap::instance();

      result = excpHandler->setFilter
              (config->serverParams.overwriteDumpFile, config->serverParams.dumpBasePath); 
           
      if (!result) ACE_OS::printf("EXCP could not set exception trap\n");
  }
  #endif 

  return result;
}



void clearExceptionTrap()
{
   #ifdef MMS_WINPLATFORM 
   MmsUnhandledExcpTrap::destroy();
   #endif 
}



#ifdef MMS_WINPLATFORM


BOOL WINAPI controlHandler(DWORD which)     // Control-C trap 
{   
  #if 0
  switch(which)                             // We have no need to address
  { case CTRL_C_EVENT:                      // individual events, we simply
    case CTRL_BREAK_EVENT:                  // treat them all the same
    case CTRL_CLOSE_EVENT:       
    case CTRL_LOGOFF_EVENT:            
    case CTRL_SHUTDOWN_EVENT: 
         break;          
  } 
  #endif
                                            // Only let event thru to console
  return !isShutdown;                       // if server has shut down
} 



void removeLocalMessageQueue(MmsConfig* config)
{
  #ifdef MMS_LINK_WITH_MSMQ 

  MmsMq::MmsMqFormatName fname;  
  const char* qname = config->clientParams.msmqMmsQueueName;
            
  LPWSTR wszFormatName = MmsMq::mqFormatName
    (config->clientParams.msmqMmsMachineName, qname, &fname);                    
     
  HRESULT hr = MQDeleteQueue(wszFormatName);
  if (FAILED(hr)) printf("MAIN could not remove local message queue\n"); 

  #endif // #ifdef MMS_LINK_WITH_MSMQ  
}



void deleteStaticAllocations()
{
  // Delete memory allocated for global static items.
  // It is not necessary to do this, but if we do not, and we subsequently run a
  // debug heap memory leak test, these benign leaks will show up along with any
  // serious leaks, and it is not obvious which are benign and which are serious,
  // that is, leaks which accumulate.

  MmsAppMessageX::destroy();   
  Mms::destroy();
}



int mmsServiceControl(const int action)
{  
  switch(action)
  {
    case MMS_SERVICE_ACTION_REGISTER:
         hStatus = RegisterServiceCtrlHandler
           (MMS_SERVICE_NAME, (LPHANDLER_FUNCTION)mmsServiceControlHandler); 
         if (hStatus == (SERVICE_STATUS_HANDLE)0) 
             return -1;

         ServiceStatus.dwServiceType = SERVICE_WIN32_OWN_PROCESS; 
         ServiceStatus.dwCurrentState = SERVICE_START_PENDING; 
         ServiceStatus.dwControlsAccepted = 0;
         ServiceStatus.dwWin32ExitCode = 0; 
         ServiceStatus.dwServiceSpecificExitCode = 0; 
         ServiceStatus.dwCheckPoint = 1; 
         ServiceStatus.dwWaitHint = MMS_SERVICE_STARTUP_WAITTIME;   
         break;

    case MMS_SERVICE_ACTION_START:
         {
         SERVICE_TABLE_ENTRY ServiceTable[2];
         ServiceTable[0].lpServiceName = MMS_SERVICE_NAME;
         ServiceTable[0].lpServiceProc = (LPSERVICE_MAIN_FUNCTION)core_main;
         ServiceTable[1].lpServiceName = NULL;
         ServiceTable[1].lpServiceProc = NULL;

         // Start control dispatcher thread for media server service
         return StartServiceCtrlDispatcher(ServiceTable)? 0: -1;
         }

    case MMS_SERVICE_ACTION_RUN:
         ServiceStatus.dwServiceType = SERVICE_WIN32_OWN_PROCESS; 
         ServiceStatus.dwCurrentState = SERVICE_RUNNING; 
         ServiceStatus.dwControlsAccepted 
           = SERVICE_ACCEPT_STOP | SERVICE_ACCEPT_PAUSE_CONTINUE | SERVICE_ACCEPT_SHUTDOWN;
         ServiceStatus.dwWin32ExitCode = 0; 
         ServiceStatus.dwServiceSpecificExitCode = 0; 
         ServiceStatus.dwCheckPoint = 0; 
         ServiceStatus.dwWaitHint = 0; 
         break;

    case MMS_SERVICE_ACTION_STOP:
         ServiceStatus.dwCurrentState = SERVICE_STOPPED; 
         break;

    case MMS_SERVICE_ACTION_SHUTDOWN:

         if (!mmsServerManager)
         {
            // Server manager is not assigned, just treat it as stopped
            ServiceStatus.dwCurrentState = SERVICE_STOPPED; 
         }
         else
         {
            mmsServerManager->postMessage(MMSM_SHUTDOWN);

            ServiceStatus.dwCurrentState = SERVICE_STOP_PENDING; 
            ServiceStatus.dwControlsAccepted = 0;
            ServiceStatus.dwWin32ExitCode = 0; 
            ServiceStatus.dwServiceSpecificExitCode = 0; 
            ServiceStatus.dwCheckPoint = 1; 
            ServiceStatus.dwWaitHint = MMS_SERVICE_STARTUP_WAITTIME; 
         }
         break;

    default: return -1;
  }

  SetServiceStatus(hStatus, &ServiceStatus);
  return 0;
}



void mmsServiceControlHandler(DWORD request) 
{ 
  switch(request) 
  { 
    case SERVICE_CONTROL_SHUTDOWN: 
    case SERVICE_CONTROL_STOP: 
         mmsServiceControl(MMS_SERVICE_ACTION_SHUTDOWN);
         break;
  } 
} 



bool mmsInstallService()
{
  char strDir[1024];
  memset(&strDir, 0, sizeof(strDir));
  GetCurrentDirectory(sizeof(strDir), strDir);
  strcat(strDir, "\\");
  strcat(strDir, MMS_APPLICATION_NAME);
  strcat(strDir, " /sS /noprompt /fVX");  // no prompt, start if stopped, stop on the way out

	SC_HANDLE schSCManager = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS);   
	if (schSCManager == NULL) return false;

  LPCTSTR lpszBinaryPathName = strDir;

  SC_HANDLE schService = CreateService(
            schSCManager,
            MMS_SERVICE_NAME, 
            MMS_SERVICE_DISPLAY_NAME,
            SERVICE_ALL_ACCESS,			// desired access 
            SERVICE_WIN32_OWN_PROCESS | SERVICE_INTERACTIVE_PROCESS, // service type 
            SERVICE_AUTO_START,			// start type 
            SERVICE_ERROR_NORMAL,		// error control type 
            lpszBinaryPathName,			// service's binary 
            NULL,						        // no load ordering group 
            NULL,						        // no tag identifier 
            NULL,						        // no dependencies 
            NULL,						        // LocalSystem account 
            NULL);				          // no password 

  if (schService == NULL) return false;  

  CloseServiceHandle(schService); 
  return true;
}



bool mmsDeleteService()
{
	SC_HANDLE schSCManager = OpenSCManager(NULL, NULL, SC_MANAGER_ALL_ACCESS); 
	if (schSCManager == NULL) return false;	

	SC_HANDLE  hService = OpenService(schSCManager, MMS_SERVICE_NAME, SERVICE_ALL_ACCESS);
	if (hService == NULL) return false;

	if (DeleteService(hService) == 0) return false;

	if (CloseServiceHandle(hService) == 0) return false;

	return true;
}

          
#endif // #ifdef MMS_WINPLATFORM 
 



int core_main()
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
//  
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
{
  #ifdef MMS_WINPLATFORM 

  if (runningInServiceMode && mmsServiceControl(MMS_SERVICE_ACTION_REGISTER) == -1) 
      return 0;
                   
  SetConsoleTitle(MMSCONTITLE);             // Install ctrl-c trap
  SetConsoleCtrlHandler(controlHandler, TRUE); 

  #ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION   // mmsLeakTest.h 
  _CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF|_CRTDBG_LEAK_CHECK_DF);
  // When memory leak testing is compiled in, we generate the memory leak following
  // in order that we can verify (in the output window, after media server shutdown)
  // that the debug heap is in fact linked in as expected. Note that if we have set 
  // this compile switch but not done a rebuild all, leaks would not be displayed,
  // this we generate a leak here in order to verify that leak detection is enabled.
  char*  forcedMemoryLeak = new char[20];    
  strcpy(forcedMemoryLeak, "IGNORE THIS LEAK"); 
  #endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

  #endif // MMS_WINPLATFORM 

  ACE_Trace::stop_tracing(); 
  MmsInstalledIpcAdapters adapters;
  Hmp* hmp = NULL;
  MmsConfig* config = NULL;
  MmsLogger* logger = NULL;
  MmsConferenceManager* conferenceManager = NULL;
  HmpResourceManager* resourceManager = NULL;
  MmsSessionManager* sessionManager = NULL;
  MmsEventRegistry* eventDispatcher = NULL;
  MmsServerManager* serverManager = NULL;
  MmsTimerHandler* timerManager = NULL;
  MmsThreadPool* threadPool = NULL;
  ACE_Reactor* timerReactor = NULL;
  MmsReporter* reporter = NULL;
  ACE_OS::printf(BUILDBANNER);
        
  MmsCmdline cmdline(pCmdline->getArgc(), pCmdline->getArgv());
  cmdline.Parse();

  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
  // Start server 
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

  do
  {                                         // Get defaults & read config file
    if  (NULL == (config = getConfigurationInfo())) 
         break;                             
                                            // Instantiate & start logger
    if  (NULL == (logger = createLogger(config)))   
         break; 
                                            // Instantiate alarms & stats 
    if  (NULL == (reporter = createReporter(logger, config)))
         break;

    setExceptionTrap(config);               // Set excp trap if configured 
    maxShutdownHmpWaitSecs = config->hmp.startSvcMaxWaitSecs;

    logger->start();                        // Start up logging 
    reporter->start();                      // Start up alarms & stats 
    reporter->postMessage(MMSM_START);      // Connect to stat server
    isHmpIssue = TRUE;     
                                            // Start HMP svc if requested
    if  (0 > verifyHmpService(config, cmdline))     
         break;
                                            // Instantiate everything else
    hmp = Hmp::instance();
    if  (!hmp || (-1 == hmp->init()))
         break;
    else isHmpIssue = FALSE;

    if  (NULL == (eventDispatcher = MmsEventRegistry::instance()))
         break;

    eventDispatcher->setConfig(config);

    if  (-1 == createResourceManager(config, reporter, &resourceManager))
         break; 
                                            // Assign resource manager instance to ASR
    Asr::instance()->m_resourceManager = resourceManager;
                                                              
    if  (NULL == (serverManager  = createServerManager
        (logger, config, resourceManager, reporter))) 
         break;

    const int threadPoolSize  = config->calculated.threadPoolSize;
    const int sessionPoolSize = MmsAs::maxG711; // config->calculated.maxConnections;
    ACE_OS::printf("MAIN session pool size %d\n",sessionPoolSize);
    ACE_OS::printf("MAIN service pool size %d\n",threadPoolSize);

    if  (NULL == (threadPool = createThreadPool
        (threadPoolSize, logger, config, serverManager, resourceManager))) 
         break; 
                   
    if  (NULL == (timerReactor = reactorFactory.create())) 
         break;
               
    if  (NULL == (timerManager = createTimerManager
        (serverManager, timerReactor, logger, config)))
         break;

    if  (NULL == (conferenceManager = createConferenceManager
        (config, resourceManager, serverManager)))
         break;

    if  (NULL == (sessionManager = createSessionManager
        (logger, config, serverManager, threadPool, timerManager, 
         resourceManager, conferenceManager, reporter)))
         break;   
                                            // Relay mutual references
    Asr::instance()->m_task = sessionManager;
    if (serverManager->init(sessionManager, threadPool) == -1) break;
    threadPool->sessionManager(sessionManager);

    ACE_OS::printf("MAIN starting service thread pool ...\n");
    threadPool->start();                    // Start service thread pool
    mmsSleep(MMS_N_MS(300 + (50 * threadPoolSize)));
    ACE_OS::printf("MAIN service thread pool started\n");

    ACE_OS::printf("MAIN starting media engine ...\n");
    sessionManager->start();                // Start session manager
    mmsSleep(MMS_N_MS(500)); 

    serverManager->start();                 // Start server manager
    mmsSleep(MMS_N_MS(750)); 
                   
    timerManager->start();                  // Start timer manager
    mmsSleep(MMS_N_MS(750));                // Reactive task requires activation
    timerManager->postMessage(MMSM_START);   
    timerManager->msg_queue()->notify();    // Server is now active 
                                            // Install adapter listeners
    if (installProtocolAdapters(adapters, serverManager, config, logger, reporter) < 1) 
    {    
        ACE_OS::printf("MAIN %s\n", couldNotStart);
        serverManager->postMessage(MMSM_SHUTDOWN); 
    }
    else 
    {   ACE_OS::printf("MAIN media engine started\n");
        serverStartedOK = TRUE;
    }

    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  
    // At this point server is running - we block this thread until server exits
    // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  

    serverManager->postMessage(MMSM_SERVERCONTROL, MMS_SERVERCTRL_FLUSHLOG);

    #ifdef MMS_WINPLATFORM                     
    if (runningInServiceMode) mmsServiceControl(MMS_SERVICE_ACTION_RUN);
    #endif                                            
                                            // Block until server exits
    ACE_Thread_Manager::instance()->wait_task(serverManager);

    ACE_OS::printf("MAIN closing logger\n");// Close logger
    logger->postMessage(MMSM_QUIT);         
    ACE_Thread_Manager::instance()->wait_task(logger);

  } while(0);

  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
  // At this point media server has exited
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

  if (!serverStartedOK && reporter)         
  {    
      // This was a useful idea, but we cannot raise an alarm here since serverManager 
      // has closed the reporter and a new alarm would kick off another stats connect.                                      
      // char* msg = isHmpIssue? (char*)hmpStartError: (char*)couldNotStart;
      // reporter->raiseServerAlarm(MmsReporter::NDX_UNSCHEDULED_SHUTDOWN, msg);
  }
  
  if (serverManager &&                      // Exit prematurely if hosed
      serverManager->shutdownState() == MmsServerManager::SHUTDOWN_PANIC)
      ExitProcess(-1);                     
						                      
  if (eventDispatcher)                      // Cancel event handling
      eventDispatcher->close();  

  adapters.deleteAll();                     // Free all installed adapters 

  if (resourceManager)
      resourceManager->shutdown();          // Stop & remove all media devices

  if (hmp)
      hmp->shutdown(); 
 
  removeLocalMessageQueue(config);
 
  if  (timerManager)      delete timerManager;
  if  (threadPool)        delete threadPool;
  if  (sessionManager)    delete sessionManager;
  if  (conferenceManager) delete conferenceManager;
  if  (serverManager)     delete serverManager;
  if  (resourceManager)   resourceManager->destroy();
  if  (eventDispatcher)   eventDispatcher->destroy();
  if  (reporter)          reporter->destroy();
  if  (logger)            delete logger;
  if  (config)            delete config;

  isShutdown = TRUE; 
  ACE_OS::printf("MAIN media engine shutdown complete\n");

  if (cmdline.stopHmpService > 0)           // Stop hmp if cmdline request
      if (hmpStop(maxShutdownHmpWaitSecs) < 0)             
          ACE_OS::printf("MAIN could not stop firmware service\n");

  deleteStaticAllocations();

  if (cmdline.prompt)
  {   ACE_OS::printf("any key ..."); while(!c) c = _getch();
  }

  #ifdef MMS_WINPLATFORM                    // Uninstall ctrl-c trap  
  SetConsoleCtrlHandler(controlHandler, FALSE);  
 
  if (runningInServiceMode) mmsServiceControl(MMS_SERVICE_ACTION_STOP);
  #endif

  clearExceptionTrap();
  return 0;
}  

        

int main(int argc, char* argv[]) 
{ 
    // The following three command line options are mutually exclusive. 
    // If one is present it must be the first parameter on the command line.
    // /sI = install service
    // /sD = delete service
    // /sS = run as service mode

  strcpy(MmsAs::szResxExhausted, MMS_RESX_EXHAUSTED_MSG);

  #ifdef MMS_WINPLATFORM 
  #ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION   // mms.h  
  _CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF|_CRTDBG_LEAK_CHECK_DF);
  // Uncomment and supply a positive integral memory allocation ordinal parameter for, 
  // the following _CrtSetBreakAlloc invocation, only if we need to break at a memory 
  // allocation occurring prior to reading of the config file, since otherwise we can
  // config a break using Diagnostics.debugBreakAtAllocation, obviating the necessity
  // to recompile.
  // _CrtSetBreakAlloc(0);
  #endif // MMS_ENABLE_MEMORY_LEAK_DETECTION
  #endif // MMS_WINPLATFORM 

  int isInstallRequest = 0, isUninstallRequest = 0;
  pCmdline = new MmsCmdline(argc, argv);

  #ifdef MMS_WINPLATFORM  

  if (argc < 2);
  else if (stricmp(argv[1], XP_SVC_INSTALL) == 0)   isInstallRequest   = TRUE;
  else if (stricmp(argv[1], XP_SVC_UNINSTALL) == 0) isUninstallRequest = TRUE;
  else if (stricmp(argv[1], XP_SVC_RUN) ==0)        runningInServiceMode = TRUE;  

  if (isInstallRequest)     
      if  (mmsInstallService())
           ACE_OS::printf("\nMAIN media server service installed\n");
      else ACE_OS::printf("\nMAIN could not install media server service\n");
  else
  if (isUninstallRequest)
      if  (mmsDeleteService())
           ACE_OS::printf("\nMAIN media server service uninstalled\n");
      else ACE_OS::printf("\nMAIN could not uninstall media server service\n");
    
  if (isInstallRequest || isUninstallRequest) return 0; 
                  
  if (runningInServiceMode)   // run media server in service mode
  {
      mmsServiceControl(MMS_SERVICE_ACTION_START); 
  }
  else // run media server in console mode
  {    
      core_main();
  }

  #else  // #ifdef MMS_WINPLATFORM 

  core_main();

  #endif // #ifdef MMS_WINPLATFORM 

  if (pCmdline) delete pCmdline;
  return 0;
}
