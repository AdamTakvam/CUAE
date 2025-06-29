#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include <conio.h>
#include <minmax.h>
#include "mmsTask.h"
#include "mmsConfig.h"
#include "mmsAppMessage.h"
#include "mmsMqWriter.h"
#include "mmsMqListener.h"

#define WAITFORKEY(p) do{char c=0;printf(p);while(!c)c=_getch();}while(0)
#define MMSM_STATE (MMSM_USER+300)
#define FINALSTATE 4
#define QNAMESENDMASK "mmsTestClient%d"

#define NUMCLIENTS 1
#define QNAMESIZE 32


                                            
class QueueManager: public MmsBasicTask    // Listener task class
{ 
  public:
  int  handleMessage(MmsMsg*);               
  QueueManager(MmsTask::InitialParams* p);
  virtual ~QueueManager() { }
  void onInitTask(MmsMsg* msg); 
  void onFinalState(); 
  void nextstate();
  MmsConfig* config;                        // Queue and machine names
  char* qNameListen, *mNameListen, *mNameSend;
  int   queuehandle, state;
  MmsAppMessage* appxml;

  int  openReceiveQueue();
  int  openSendQueues();
  void sendConnect(const int n);
  void closeWriter(const int n);
  void killMqListener();
  void closeReceiveQueue();
};

