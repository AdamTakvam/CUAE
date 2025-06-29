//
// mmsSessionManager.h
//
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include "mms.h"
#include "mmsTask.h"
#include "mmsConfig.h"
#include "mmsLogger.h"
#include "mmsSession.h"
#include "mmsTimerHandler.h"
#include "mmsMediaresourceMgr.h"
#include "mmsConferenceManager.h"
#include <list>


class MmsSessionManager: public MmsBasicTask
{
  public:
  virtual int handleMessage(MmsMsg* msg);

  virtual int onThreadStarted()             // Thread startup hook
  { 
    MMSLOG((LM_INFO,"%s thread %t started at priority %d\n", 
            taskName,taskID,osPriority)); 
    return 0;
  } 

  virtual int close(unsigned long)          // Thread exit hook
  {
    MMSLOG((LM_DEBUG,"%s thread %t exit\n",taskName));
    return 0;
  }
         
  struct SessionManagerParams: public MmsTask::InitialParams  
  {                                            
    MmsSessionPool*       sessionPool;
    MmsTask*              threadPool;
    MmsTimerHandler*      timerManager;
    HmpResourceManager*   resourceMgr;
    MmsConferenceManager* conferenceMgr;

    SessionManagerParams(const int t, int g, MmsTask* p): MmsTask::InitialParams(t,g,p)
    { sessionPool = 0; threadPool = 0; timerManager = 0; 
      resourceMgr = 0; conferenceMgr = 0; 
      length = sizeof(SessionManagerParams);
    }
  };
                                            // Ctor
  MmsSessionManager(SessionManagerParams* params);
  virtual ~MmsSessionManager();

  MmsSessionPool* sessionPool()             { return m_sessionPool;    }
  MmsConferenceManager* conferenceManager() { return m_conferenceMgr;  }
  ACE_Thread_Mutex& sessionPoolLock()       { return m_sessionPool->sessionPoolLock();}
  ACE_Thread_Mutex  audiolock;

  void  logResourceState();

  int   postErrorReturn (char* map, int returncode, MmsSession::Op* op=0);
  int   postNormalReturn(char* map, MmsSession* session=0);
  int   postProvisionalReturn(char* map, MmsSession::Op* op=0);

  int   bargeDisconnect      (char* map, MmsSession*);
  int   isSessionDisconnect  (char* map, MmsSession*);
  int   isBargeDisconnect    (char* map, MmsSession*);
  int   isBusyDisconnect     (char* map, MmsSession*);
  int   executeCommandInline (char* map, MmsSession*);
  int   doExternalOperation  (char* map, MmsSession*);

  int   onAbandonConference(char* map);   
  int   onAbandonSessionsBackground(void* client, void* handle=0);
  int   onAbandonSessions     (void* client, void* handle=0, const int conferenceID=0);
  int   abandonedSessionsCount(void* client, void* handle=0, const int conferenceID=0);

  void  terminateCallStateOperation(int sessionID);

  enum opCaps 
  { OPCAPS_NOTHING_ONGOING=0, OPCAPS_ONGOING_ASYNC=1, OPCAPS_ONGOING_SYNC=2, 
    OPCAPS_ONGOING_EXTOP=4,   OPCAPS_INCOMMANDCODE=8, OPCAPS_AYSNC_OK=16, 
    OPCAPS_SYNC_OK=32
  };
 
  unsigned int getConcurrentOpCaps(const int commandID, MmsSession*);
  int   isConcurrentOperationOK(const int commandID, MmsSession*, MmsSession::Op*);
  int   isClientDisconnecting(char* map);
  int   isClientDisconnecting(void* clienthandle);

  MmsTimerHandler* timerManager() { return m_timers; }

  struct SuperTaskParams
  { // Container to pass system command parameters from session manager to service pool
    enum tasks{ ABANDON_SESSIONS=1, ABANDON_CONFERENCE, STOPMEDIA, RECONNECT, 
                ADJUSTPLAY, POLICE_DIRECTORIES, VOICEREC_START_COMPTHREAD, VOICEREC_DONE };
    enum{sig = 0xcafefeed};
    unsigned int signature;
    int          sessionManagerTask;
    int          operationID;
    int          conferenceID;
    int          result;
    unsigned int tag;
    char*        flatmap;
    MmsTask*     client;
    void*        handle;
    MmsSession*  session;
    void clear() { memset(this,0,sizeof(SuperTaskParams)); signature=sig; }
    SuperTaskParams() { clear(); }
    SuperTaskParams(int t, char* f, MmsSession* s, int op)
    { clear(); sessionManagerTask=t; flatmap=f; session=s; operationID=op; 
    }
    int isvalid() { return signature == sig; }
  };

  struct TaskResultParams
  { // Container to return result data from service pool to session manager  
    enum{sig = 0xbaddecaf};
    unsigned int signature;
    MmsSession*  session;
    int          operationID;
    int isvalid(){ return signature == sig; }
    void clear() { memset(this,0,sizeof(TaskResultParams)); signature=sig; }
    TaskResultParams(MmsSession* s, int id) { clear(); session=s; operationID=id; }
  };

  protected:

  int   onSessionTask(MmsMsg* msg);
  int   onMediaEvent (MmsMsg* msg);
  int   onServicePoolTaskReturn  (MmsMsg* msg);
  int   onServicePoolTaskExReturn(MmsMsg* msg);
  int   onCspDataReady(MmsMsg* msg);
  int   onStartComputationThread(MmsMsg* msg);
  void  onServerControl(MmsMsg* msg);
  void  onInitTask() {}
  void  onNotify(MmsMsg* msg);
  int   isGoodConnectionState(MmsSession* session, const int command);
  int   prevalidateServerCommand(char* flatmap, int* action);

  void  onTimer(MmsMsg* msg);
  bool  monitorSessionPool();
  void  onCommandTimeout(MmsSession::Op*);
  void  onSessionTimeout(MmsSession*);
  int   postConnectionBusyReturn(MmsSession*, char* flatmap);
  static int isDisconnectEntireConference(char* flatmap);
  int  isConferenceOperation(char* map) 
  {    return map && isFlatmapFlagSet(map,MmsServerCmdHeader::IS_CONFERENCE);
  }
  int  isCommandError(char* map) 
  {    return map && isFlatmapFlagSet(map,MmsServerCmdHeader::IS_ERROR);
  }
  MmsCurrentCommand* currentCommand(int cmd, MmsSession*, MmsSession::Op*, char* map, int flags=0);
  MmsSession* getSessionByID(const int connectionID);

  int   discoListAdd(void*);
  int   discoListRemove(void*);

  MmsSessionManager() { }

  enum sessionactiontypes{EXISTING_SESSION=0, NEW_SESSION, MULTI_SESSION};
                   
  MmsConfig*       m_config;
  MmsSessionPool*  m_sessionPool;
  MmsTask*         m_threadPool;
  MmsTask*         m_reporter;
  MmsTimerHandler* m_timers;
  MmsManualTimer   m_sysstatTimer;
  MmsManualTimer   m_resstatTimer;
  HmpResourceManager*   m_resourceMgr;
  MmsConferenceManager* m_conferenceMgr;
  int m_isShutdownRequest;
  ACE_Thread_Mutex m_logStateLock;
  ACE_Thread_Mutex m_discosLock;
  std::list<void*> m_discos;                // Currently disconnecting client handles 
};
