// ipctest.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "ipctest.h"
#include "Flatmap.h"
#include "ipc/FlatmapIpcClient.h"

using namespace Metreos;
using namespace Metreos::IPC;

static bool bConnected = false;
static int transID = 0;
static void* clientID = 0;

#define FLATMAPIPC_SERVER     "127.0.0.1" 
#define FLATMAPIPC_PORT       9530 
#define FLATMAPIPC_TYPE_MMS   1001
#define FLATMAPIPC_MMS_BODY   100
#define MMS_HEARTBEAT_NAME    "heartbeat"

IpcTestClient::IpcTestClient()
{
  appxml = NULL;
  outxml = NULL;
}

IpcTestClient::~IpcTestClient()
{
  if (appxml)
    delete appxml;
  appxml = NULL;

  if (outxml)
    delete outxml;
  outxml = NULL;
}

void IpcTestClient::OnIncomingFlatMapMessage(const int messageType, const FlatMapReader& flatmap)
{
  if (messageType != FLATMAPIPC_TYPE_MMS)
    return;

  FlatMapReader r;	
  r = flatmap;

  char* pData = NULL;
  int type = FlatMap::STRING;
  int len = r.find(FLATMAPIPC_MMS_BODY, &pData, &type, 0);
  if (len > 0)
  {
    // pass XML data to handler
    char* pFlatmapData = new char[len+1];
    ACE_OS::memset(pFlatmapData, 0, len);
    ACE_OS::memcpy(pFlatmapData, pData, len);
    pFlatmapData[len] = 0;

    if  (appxml) 
      delete appxml;
    appxml = new MmsAppMessage(pData); 
     
    if(IsHeartbeatMessage(appxml))
    {   
      std::cout << "Heartbeat from MMS." << std::endl;
      int  heartbeatID = GetHeartbeatID(); 
      SendHeartbeatAck(heartbeatID);
    }
    else
    {
      clientID = GetMessageClientID();
      std::cout << "Message from MMS." << std::endl;
    }

    delete pFlatmapData;
  }
}

BOOL IpcTestClient::WriteMessage(char* body, const int length)
{
  if (!bConnected)
    return FALSE;

  FlatMapWriter map;
  map.insert(FLATMAPIPC_MMS_BODY, FlatMap::STRING, length, body);
  return this->Write(FLATMAPIPC_TYPE_MMS, map);
}

void IpcTestClient::OnConnected()
{
  std::cout << "Connected to MMS!" << std::endl;
  bConnected = true;
}

void IpcTestClient::OnDisconnected()
{
  std::cout << "Disconnected from MMS!" << std::endl;
  clientID = 0;
  bConnected = false;
}

void IpcTestClient::OnFailure()
{
  std::cout << "IPC failure!" << std::endl;
}

int IpcTestClient::IsHeartbeatMessage(MmsAppMessage* xmlmsg)
{
  if  (!xmlmsg || xmlmsg->getCommand() == -1) 
    return 0;

  const char*   msgname = xmlmsg->commandName();
  return strcmp(msgname, MMS_HEARTBEAT_NAME) == 0;
}

int IpcTestClient::GetHeartbeatID()
{
  int  heartbeatID = -1, serverID = -1;
  if  (!appxml) 
    return heartbeatID;

  const char* paramnameHBID = MmsAppMessageX::paramnames[MmsAppMessageX::HEARTBEAT_ID];
  if  (appxml->findparam(paramnameHBID))
       heartbeatID = atoi(appxml->paramValue());
  return heartbeatID;
}

void* IpcTestClient::GetMessageClientID()
{
  if  (!appxml) 
    return NULL;
  const char* paramname = MmsAppMessageX::paramnames[MmsAppMessageX::CLIENT_ID];
  const char* pid = appxml->getvaluefor(paramname);
  if  (!pid)  
    return NULL;
  void*  id = (void*)atoi(pid);
  return id;
}

void IpcTestClient::SendHeartbeatAck(const int heartbeatID)
{
  if (outxml)
    delete outxml;
  outxml = new MmsAppMessage; 
  outxml->putMessageID(MmsAppMessageX::MMSMSG_HEARTBEAT);    
  outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::HEARTBEAT_ID], heartbeatID);
  outxml->terminateReturnMessage(clientID, MMS_OMIT_RESULTCODE);
  WriteMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
}

void IpcTestClient::SendServerConnect()
{
  if (outxml)
    delete outxml;
  outxml = new MmsAppMessage;
  outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);  
  outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::HEARTBEAT_INTERVAL], 1);
  outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::MACHINE_NAME], "MyMachine");
  outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::QUEUE_NAME], "MyQueue");
  outxml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CLIENT_ID], 0);
  outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
  outxml->terminateReturnMessage(clientID);
  WriteMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
}

void IpcTestClient::SendServerDiconnect()
{
  if (outxml)
    delete outxml;
  outxml = new MmsAppMessage;
  outxml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
  outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID], ++transID);
  outxml->terminateReturnMessage(clientID);
  WriteMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
}


int main(int argc, char* argv[])
{    
  IpcTestClient client;

  while (1)
  {
    if (!bConnected)
    {
      bConnected = client.Connect(FLATMAPIPC_SERVER, FLATMAPIPC_PORT);
      client.SendServerConnect();
    }
    ACE_OS::sleep(1);
  }
  client.SendServerDiconnect();
  client.Disconnect();
  return 0;
}
