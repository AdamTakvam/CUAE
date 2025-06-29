//
// mmsThreadPool.h  
//
#ifndef MMS_THREADPOOL_H
#define MMS_THREADPOOL_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#pragma warning(disable:4786)
#include "mms.h"
#include "mmsConfig.h"
#include "mmsFlatMap.h"
#include "mmsServerCmdHeader.h"
#include "mmsSessionManager.h"
#include "mmsMediaEvent.h"
#define MMS_POLL_DEPENDENCY_WAIT_EVERY_MS 200
// We want to eventually add addThreads() and removeThreads() methods to this  
// class so that we can adjust the number of service pool threads dynamically.



class MmsThreadPool: public MmsBasicTask
{
  public:
                                            // Ctor
  MmsThreadPool(MmsTask::InitialParams* params);

  virtual ~MmsThreadPool() {}

  void sessionManager(MmsSessionManager* sm) { m_sessionManager = sm; }

  static GENERICCALLBACK policeDirectoriesCallback;  

  protected:
  MmsThreadPool() {}
                 
  HmpResourceManager*   m_resourceManager; 
  MmsSessionManager*    m_sessionManager;          
  MmsTask*              m_serverManager;                                          
  MmsConfig* config;

  int  isShutdownRequest;

  virtual int handleMessage(MmsMsg*);

  virtual int onThreadStarted();            // Thread startup hook

  virtual int close(unsigned long);         // Thread exit hook
                                            
  int  onServicePoolTask  (MmsMsg* msg);    // New work
  int  onServicePoolEvent (MmsMsg* msg);    // Continuing work
  int  onServicePoolTaskEx(MmsMsg* msg);    // Extended task (multisession)

  int  postNormalReturn(MmsSession::Op*);
  int  postErrorReturn (MmsSession::Op*, int retcode);
  int  postProvisionalReturn(MmsSession::Op*);
  int  getHmpAsyncEventError(MmsSession::Op*, MmsEventRegistry::DispatchMap*);

  int  handleEvent(MmsSession::Op*, MmsEventRegistry::DispatchMap*);

  int  handleRfc2833SignalEvent(MmsSession*, MmsEventRegistry::DispatchMap*);
  int  handleLowBitrateErrors(MmsSession::Op* operation, const int eventType, const int hmpError);

  int  releaseAndReturn(const int result, const int isLocked, ACE_Thread_Mutex&);
  int  waitForDependency(MmsSession::Op*);
};  



#endif
