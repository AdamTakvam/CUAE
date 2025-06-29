// mqIpcClient.h

#ifndef _MQIPCCLIENT_H_
#define _MQIPCCLIENT_H_

#ifdef WIN32
#   pragma warning(disable:4786)
#   pragma warning(disable:4251)
#endif

#include "ace/Task.h"
#include "ace/Thread_Manager.h"
#include "ipc/FlatmapIpcclient.h"
#include "Flatmap.h"
#include "mqIpcMessage.h"

using namespace Metreos;
using namespace Metreos::IPC;

class mqIpcClient : public FlatMapIpcClient, public ACE_Task<ACE_MT_SYNCH>
{
public:
  mqIpcClient();
  ~mqIpcClient();

  static mqIpcClient* Instance();

  // From ACE_Task
  virtual int open(void *args = 0);
  virtual int close (u_long flags = 0);
  virtual int svc(void);

  inline bool IsConnected() { return bConnected; }
  inline bool IsShutdown() { return bShutdown; }
  inline bool IsClearing() { return bClearing; }

  BOOL WriteIpcMessage(char* body, const int length);
  mqIpcMessage* GetIpcMessage(int timeout);
  void ClearQueue();

protected:
  // From FlatMapIpcClient
  virtual void OnIncomingFlatMapMessage(const int messageType, const FlatMapReader& reader);
  virtual void OnSessionStop(int id);
  static ACE_THR_FUNC_RETURN ConnectToServerThreadFunc(void* data);
  bool ConnectToServer();

  static mqIpcClient* instance;

private:
  bool bConnected;
  bool bShutdown;
  bool bClearing;
  ACE_thread_t hThreadConnectToServer;
};

#endif
