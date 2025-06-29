//
// MmsMqListener.h  
//
// Queue object and listener thread for a MMS MSMQ receive queue
//
#ifndef MMS_MQLISTENER_H
#define MMS_MQLISTENER_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include "mmsMq.h"
#include "mmsTask.h"
#include "mmsConfig.h"
#include "mmsAppMessage.h"

#define MMS_MQLISTENER_INTERNAL_QUIT "quit"
#define MMS_ERROR_LISTENER_DOWN    1
#define MMS_MQRESULT_INTERNAL_QUIT 2
#define MMS_MQ_RECEIVE_MAX_PROPERTIES 4  



class MmsMqListener: public MmsTask, public MmsMq 
{ 
  public:
  MmsMqListener(MmsTask::InitialParams* params);
  virtual ~MmsMqListener() {}

  int purge();                              // Purge queue
      
  int shutdown();                           // Kill listener thread
  int isListening() { return this->islistening; }

  int isMessageElderThan(const int secs) 
  { 
    return difftime(messageSentTime, mmsStartTime) > secs;
  } 

  protected:
                  
  MmsConfig* config;
  MmsManualTimer logliveTimer;
  time_t mmsStartTime;  
  time_t messageSentTime;
  int    bodySize, pnSenttime, pnBodytype, pnBodysize, pnBodytext;
  int    islistening, isBailing, consecutiveTimeoutCount;

  char szBody[MMS_MAX_XMLMESSAGESIZE];      // Dequeued MQ message body

  MQMSGPROPS    msgprops;                   // MQ message properties
  MSGPROPID     aMsgPropId [MMS_MQ_RECEIVE_MAX_PROPERTIES];
  MQPROPVARIANT aMsgPropVar[MMS_MQ_RECEIVE_MAX_PROPERTIES];
  HRESULT       aMsgStatus [MMS_MQ_RECEIVE_MAX_PROPERTIES];

  MmsMqListener() { }   
 
  virtual int svc();                        // Listener thread procedure  

  virtual int handleMessage(MmsMsg* msg);   // Handle task messages
                                            
  int  getMqMessage(DWORD timeoutms);       // Dequeue MQ message

  int  handleTaskMessages();                // Get task msgs while available  

  inline int isAdapterQuitMessage();        // Is message an internal quit

  void initMqMessageProperties();           // Specify desired properties

  virtual int onThreadStarted();            // Thread startup hook

  virtual int close(unsigned long);         // Thread exit hook
};



#endif
