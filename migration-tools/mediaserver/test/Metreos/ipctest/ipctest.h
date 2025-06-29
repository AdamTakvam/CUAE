#ifndef IPC_TEST_H
#define IPC_TEST_H

#include "ipc/FlatmapIpcClient.h"
#include "mmsAppMessage.h"

using namespace Metreos;
using namespace Metreos::IPC;

class IpcTestClient : public FlatMapIpcClient
{
public:
  IpcTestClient();
  ~IpcTestClient();

  virtual void OnIncomingFlatMapMessage(const int messageType, const FlatMapReader& flatmap);
  virtual void OnConnected();
  virtual void OnDisconnected();
  virtual void OnFailure();

  void SendServerConnect();
  void SendServerDiconnect();
  void SendHeartbeatAck(const int heartbeatID);

protected:
  int IsHeartbeatMessage(MmsAppMessage* xmlmsg);
  int GetHeartbeatID();
  BOOL WriteMessage(char* body, const int length);

private:
  MmsAppMessage* appxml;
  MmsAppMessage* outxml;
  void* GetMessageClientID();
};

#endif // IPC_TEST_H