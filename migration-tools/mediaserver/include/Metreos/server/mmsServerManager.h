//
// mmsServerManager.h
//
#ifndef MMS_SERVERMANAGER_H
#define MMS_SERVERMANAGER_H

#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include "mms.h"
#include "mmsTask.h"
#include "mmsConfig.h"
#include "mmsLogger.h"
#include "mmsSessionManager.h"
#include "mmsTimerHandler.h"
#include <vector>



class MmsServerManager: public MmsBasicTask
{
  protected:

  virtual int handleMessage(MmsMsg* msg); 
  virtual int onThreadStarted();             
  virtual int close(unsigned long); 
         
  int  onServerInitialized(); 
  void shutdownMediaServer();
  void onServerCommand(MmsMsg* msg);
  void onServerCommandReturn(MmsMsg* msg);
  void onServerControl(MmsMsg* msg);
  void onRegister(MmsMsg* msg);
  void onUnregister(MmsMsg* msg);
  void onRefreshConfiguration();
  void doMediaServerShutdownSequence();
  void onTimerPoolRegister(MmsMsg* msg);
  void onAck(MmsMsg* msg);
  void onTimer(MmsMsg* msg);
  void onPingback(MmsMsg* msg);
  void onServerPush(MmsMsg* msg);
  void onThreadedPoliceDirectories();

  int  preallocateSessionResources();
  int  deallocateSessionResources();
  int  getMaxHmpResourceCounts(mmsHmpRegistryResourceCounts*);
  void initTtsServices();
  void closeTtsServices();
  void initAsrServices();
  void closeAsrServices();

  int  registerAdapter(MmsTask*);           // Register an adapter
  int  unregisterAdapter(const MmsTask*);
  int  isRegistered(const MmsTask*);        // Is adapter registered
  int  isServerInitialized() { return m_isServerInitialized; }               
                                            // Broadcast to all adapters
  int  notifyAll(const unsigned int msg, const long param=0);
                                            // Pass payload on to subtask
  int  shutdownAdapters();                  // Shut down all adapters
  void onTeardown(MmsMsg* msg);             // Tear down adapter's sessions
                                            // Turn client message around
  void returnClientMessage (MmsMsg* msg, int reason);
  void discardClientMessage(MmsMsg* msg);

  int  setHeartbeatTimer();                  
  void cancelHeartbeatTimer();
  void onHeartbeat();
  void monitorConfigurationChanges(MmsConfig*);
  void monitorReporterConnection();
  void registerMsgLevelChange();
  int  conferencingDevice();
  void monitorQueueCapacity();
  void monitorExpiredFiles();
  void monitorThreadState();
  void policeAudioDirectory(int isGetLock=TRUE);
  void policeLogDirectory();
  void cycleLogfile();
  void flushLogfile();
  void closeLogger();
  void enableAlarmsAndStats(const int bEnable);
  void forceServer(char* task=0, char* msg=0);
  void scanDirectories();

  MmsDeviceConference* m_deviceConf;        // For resource stats 
  HmpResourceManager*  m_resourceMgr;
  MmsSessionManager*   m_sessionManager;
  MmsSessionPool*      m_sessionPool;
  MmsTimerHandler*     m_timers;
  MmsTask*             m_threadPool;
  MmsTask*             m_reporter;
  MmsConfig* config;
  static MmsServerManager* m_this;
  ACE_Thread_Mutex  adapterslock;
  std::vector<MmsTask*> adapters;           // Active adapter registry

  #define MONITORED_TASK_COUNT 6             
  struct  MonitoredTask { MmsTask* task; int count; };
  MonitoredTask monitoredTasks[MONITORED_TASK_COUNT];
  void clearMonitoredTasks() 
  { memset(monitoredTasks, 0, sizeof(MonitoredTask) * MONITORED_TASK_COUNT);
  }
  void setMonitoredTasks();

  int m_isServerInitialized;
  int m_isIpResourcesLoaded;
  int m_shutdownState;
  int m_shutdownInProgress;  
  int m_heartbeatTimerID; 
  int m_heartbeats;  
  int m_currentLogMessageLevel; 
  int m_isScanningDirectories; 
                 
  public:
  MmsSessionPool* sessionPool() { return m_sessionPool; }
                                            // Ctor
  MmsServerManager(MmsTask::InitialParams* params);

  int  init(MmsSessionManager*, MmsTask*);

  int  shutdownState() { return m_shutdownState; }

  MmsTask* reporter()  { return m_reporter; }

  static void policeDirectoriesCallback(void*);   

  virtual ~MmsServerManager();

  enum shutdownStates 
  { SHUTDOWN_BEGIN=1, SHUTDOWN_TIMERS, SHUTDOWN_THREADPOOL, 
    SHUTDOWN_REPORTER,SHUTDOWN_SESSIONMGR, SHUTDOWN_SELF, SHUTDOWN_PANIC
  };
};  



struct TEARDOWNPARAMS
{ void* adapter;
  void* handle;
  int   isHeapAlloc;
  TEARDOWNPARAMS(void* a, void* h, int b) { adapter=a; handle=h; isHeapAlloc=b; }
};



class MmsAudioDirectoryWalker: public MmsDirectoryRecursor
{ public:
  MmsAudioDirectoryWalker(int maxdirs=0): MmsDirectoryRecursor(maxdirs), m_count(0), m_erased(0) { }
  int  filecallback(char* path, void* param);
  int  count()  { return m_count;  }
  int  erased() { return m_erased; }
  int  m_count, m_erased;  
};



class MmsLogDirectoryScanner: public MmsDirectoryRecursor
{ public:
  MmsLogDirectoryScanner(): m_count(0), m_erased(0) { }
  int  filecallback(char* path, void* param);
  int  count()  { return m_count;  }
  int  erased() { return m_erased; }
  int  m_count, m_erased;  
};


#endif // #ifndef MMS_SERVERMANAGER_H
