//
// mmsFlatmapIpcAppAdapter.h
// Metreos IPC Transport Adapter
//
#ifndef MMS_FLATMAPIPC_APPADAPTER_H
#define MMS_FLATMAPIPC_APPADAPTER_H

#ifdef MMS_WINPLATFORM
#pragma once
#endif

#include "mmsMqAppAdapter.h"
#include "MmsFlatmapIpcClient.h"
#include "mmsAppMessage.h"
#include "ipc/FlatmapIpcServer.h"

using namespace std;
using namespace Metreos;
using namespace Metreos::IPC;

// IPC Session ID, IPC Client instance pair
typedef pair <int, MmsFlatmapIpcClient*> Session_Client_Pair;

#define FLATMAPIPC_TYPE_MMS   1001    // MMS IPC message type
#define FLATMAPIPC_MMS_BODY   100     // XML body message ID


class MmsFlatmapIpcAppAdapter: public MmsMqAppAdapter, public FlatMapIpcServer
{
public:
  MmsFlatmapIpcAppAdapter(const int port);
  MmsFlatmapIpcAppAdapter(const int port, MmsTask::InitialParams* params);
  virtual ~MmsFlatmapIpcAppAdapter();

  void postClientsHeartbeat();                        // post heartbeat to all clients
  void pushServerData(MmsMsg* msg);                   // push server data to client
  int  postClientHeartbeat(int sessionId);            // post heartbeat to a single client by sessionId
  int  monitorHeartbeatAcks(MmsFlatmapIpcClient* c);  // monitor client heartbeat status
  int  putFlatmapIpcMessage(int sessionId, char* body, const int length, const char* name=0); // send message to IPC client

  int  turnMqMessageAround(const int resultcode, const int, const int isdeletexml=TRUE);  
  void turnServerConnectMessageAround(const int, const int, const int isdeletexml=TRUE);
  char* getRemoteAddress(const int ipcSessionID);
  int   getTransactionID(MmsAppMessage*);

protected:
  map<int, MmsFlatmapIpcClient*> clientMap;           // client map, client id/IPC client pair 

  virtual void OnClientConnected(int sessionId);
  virtual void OnClientDisconnected(int sessionId);
  virtual void OnSocketFailure(int errorNumber, int sessionId);
  virtual void OnIncomingFlatMapMessage
   (const int messageType, const FlatMapReader& flatmap, const char* data, size_t length, int sessionId);
  virtual int getServerIdFromClientId(const int clientID);

  void* getClientID(MmsAppMessage*);   
  virtual int isConnected(); 
  void init();
  int  isConnected(int sessionID);     
  int  registerClient(int which, MmsAppMessage* xmlmsg=0);
  int  unregisterClient(const int sessionId);
  int  clientClose(const int sessionId=0, const int isShutdown=0);
  void handleQueryMediaResources(int sessionID = -1);
  void logOutboundMessage(char* body, const int length, const char* name, const int client);
  int  logMessageCount, logMessageSequence;

  virtual int  postClientReturnMessage(MmsAppMessage**, char* flatmap);
  virtual int  buildCommonParameters(MmsFlatMapWriter&, MmsAppMessage*, 
                                     MmsServerCmdHeader& cmdHeader, unsigned int* flags=NULL);
  virtual int  onShutdown();
  virtual void onStartAdapter(); 
  virtual void onStopAdapter(); 
  virtual void onCommandServer(void* data);
  virtual void onCommandHeartbeatAck(void* data);
};

#endif  // MMS_FLATMAPIPC_APPADAPTER_H
