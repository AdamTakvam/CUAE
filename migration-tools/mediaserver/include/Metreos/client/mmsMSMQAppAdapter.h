//
// mmsMSMQAppAdapter.h -- Micrsoft Message Queue transport adapter
//  
#ifndef MMS_MSMQAPPADAPTER_H
#define MMS_MSMQAPPADAPTER_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif

#include "MmsMqAppAdapter.h"


class MmsMSMQAppAdapter: public MmsMqAppAdapter
{
  public:
  MmsMSMQAppAdapter() {};
  MmsMSMQAppAdapter(MmsTask::InitialParams* params);
  virtual ~MmsMSMQAppAdapter();

  virtual void postClientsHeartbeat();
  void pushServerData(MmsMsg* msg);
  QUEUEHANDLE postClientHeartbeat(MmsMqWriter*);    
  int  monitorHeartbeatAcks(MmsMqWriter*); 
  int  turnMqMessageAround(const int rc, const int reason, const int isdeletexml=TRUE);  
  void turnServerConnectMessageAround(const int, const int,const int isdeletexml=TRUE);

  protected:
  MmsMqListener* mqListener;                // Our receive queue listener
                                            // Client send queues table
  std::map<QUEUEHANDLE, MmsMqWriter*> clientQueues;  

  struct MMSMQNAME 
  { char machinename[128], queuename[128]; 
    MMSMQNAME() { memset(this,0,sizeof(MMSMQNAME)); }
  };

  MmsMqWriter* getClientQueue  (const int n);        
  MmsMqWriter* getClientQueue  (const QUEUEHANDLE);  
  MmsMqWriter* getClientQueueEx(const QUEUEHANDLE);  
  MmsMqWriter* getClientQueue  (MmsAppMessage*); 
  MmsMqWriter* getClientQueue  (const char* qname); 
  QUEUEHANDLE  getClientID     (MmsAppMessage*);   

  int getClientQueueName(MmsAppMessage*, MMSMQNAME&); 
  virtual int registerClient(int which, MmsAppMessage* xmlmsg=0);
  int unregisterClient(const QUEUEHANDLE handle);
  MmsMqWriter* clientOpen(const char* mname, const char* qname, const int serverID, int* rc=0);   
  int clientClose(const QUEUEHANDLE hq = NULL);   
  int openServerQueue();  
  int closeServerQueue();  
  void handleQueryMediaResources(QUEUEHANDLE clientID = NULL);
  void  shutdownListener();
  virtual int postClientReturnMessage(MmsAppMessage**, char* flatmap);
  virtual int getServerIdFromClientId(const int clientID);

  virtual int  isConnected(); 
  int isConnected(void* clientID);     

  virtual int  buildCommonParameters(MmsFlatMapWriter&, MmsAppMessage*, 
                                     MmsServerCmdHeader& cmdHeader, unsigned int* flags=NULL);
  virtual int onShutdown();
  virtual void onStartAdapter(); 
  virtual void onStopAdapter(); 
  virtual void onCommandServer(void* data);
  virtual void onCommandHeartbeatAck(void* data);
};

#endif
