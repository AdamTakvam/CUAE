//
// MmsFlatmapIpcClient.cpp
//
#include "StdAfx.h"
#include "MmsFlatmapIpcClient.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION


MmsFlatmapIpcClient::MmsFlatmapIpcClient(int sessionId)
{
  this->sessionId = sessionId;  

  heartbeatInterval = heartbeats = heartbeatAck 
      = heartbeatPayloadMediaResources = serverId = 0;   
}


MmsFlatmapIpcClient::~MmsFlatmapIpcClient()
{

}
