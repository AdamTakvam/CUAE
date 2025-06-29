//
// mqIpcClient.cpp
//
#include "StdAfx.h"
#include <iostream>
#include "mqIpcMessage.h"
#include "mqIpcClient.h"
#include "ace/message_queue_t.h"

using namespace Metreos;

#define DEFAULT_WAIT      1		// sec
#define DEFAULT_PORT      9530
#define DEFAULT_SERVER    "127.0.0.1"

#define FLATMAPIPC_TYPE_MMS   1001
#define FLATMAPIPC_MMS_BODY   100

mqIpcClient* mqIpcClient::instance = 0;



mqIpcClient::mqIpcClient()
{
  bConnected = false;
  bShutdown  = false;
  bClearing  = false;
  hThreadConnectToServer = NULL;
}



mqIpcClient::~mqIpcClient()
{
  if (!IsShutdown())
      close();
}



mqIpcClient* mqIpcClient::Instance()
{
  if(instance == 0)
     instance = new mqIpcClient();
  return instance;
}



int mqIpcClient::svc(void)
{
	static ACE_Time_Value tv30 (0, 30*1000); // 30 ms 

	ACE_Message_Block* msg = 0;

	while(!IsShutdown())
	{
    if (IsClearing())
    {
	    ACE_Message_Block* msg = 0;
      if (msg_queue()->peek_dequeue_head(msg) != -1)
      {
	      getq(msg);
	      mqIpcMessage* wm = (mqIpcMessage*)(msg);
        delete wm;
      }

      bClearing = false;
    }
    else Sleep(500);
  }

  return 0;
}



int mqIpcClient::open(void *args)
{
  if (-1 == ACE_Thread_Manager::instance()->spawn(ConnectToServerThreadFunc, 
      this, THR_NEW_LWP | THR_JOINABLE, &hThreadConnectToServer))	 
		  return 0;
	 
	return 1;
}



int mqIpcClient::close (u_long flags)
{
  bShutdown = true;

  if (IsConnected())
  {
      this->Disconnect();
      bConnected = false;
  }

  ACE_Thread_Manager *tm = ACE_Thread_Manager::instance();

  if(tm->testresume(hThreadConnectToServer) == 1)
  {
     tm->join(hThreadConnectToServer);
  }

	return 1;
}



bool mqIpcClient::ConnectToServer()
{
	bConnected = this->Connect(DEFAULT_SERVER, DEFAULT_PORT);

	return bConnected;
}



void mqIpcClient::OnIncomingFlatMapMessage(const int messageType, const FlatMapReader& reader)
{
  if (messageType != FLATMAPIPC_TYPE_MMS) return;

  FlatMapReader r = reader;

  char* pData = NULL;
  int type = FlatMap::STRING;
  int len = r.find(FLATMAPIPC_MMS_BODY, &pData, &type, 0);

  if (len > 0)
  {
    char* pFlatmapData = new char[len+1];
    memset(pFlatmapData, 0, len);
    memcpy(pFlatmapData, pData, len);
    pFlatmapData[len] = 0;

    mqIpcMessage* msg = new mqIpcMessage(len, pFlatmapData);
    this->msg_queue()->enqueue(msg);

    if (pFlatmapData)
        delete [] pFlatmapData;
  }
}



void mqIpcClient::OnSessionStop(int id)
{
  bConnected = false;
}



ACE_THR_FUNC_RETURN mqIpcClient::ConnectToServerThreadFunc(void* data)
{
  mqIpcClient* client = static_cast<mqIpcClient*>(data);
  ACE_ASSERT(client != NULL);

  while(!client->IsShutdown())
  {
    while (!client->IsShutdown() && !client->IsConnected())
    {
      if (client->ConnectToServer())
          break;
      Sleep(1000);
    }

    Sleep(1000);
  }

  return 0;
}



BOOL mqIpcClient::WriteIpcMessage(char* body, const int length)
{
  if (!IsConnected()) return FALSE;

  FlatMapWriter map;
  map.insert(FLATMAPIPC_MMS_BODY, FlatMap::STRING, length, body);
  return this->Write(FLATMAPIPC_TYPE_MMS, map);
}



mqIpcMessage* mqIpcClient::GetIpcMessage(int timeout)
{
	ACE_Time_Value t1 (0, 1000*100); // 100 ms 

  int count = timeout/100;

	ACE_Message_Block* msg = 0;

	if (!IsShutdown())
  {
    for (int i=0; i<count; i++)
    {
			if (msg_queue()->peek_dequeue_head(msg) != -1)
			{
				getq(msg);
				mqIpcMessage* wm = (mqIpcMessage*)(msg);
        return wm;
			}
      Sleep(100);
    }
  }
  
  return NULL;
}



void mqIpcClient::ClearQueue()
{
  if (msg_queue()->message_count() > 0)
      bClearing = true;
}



