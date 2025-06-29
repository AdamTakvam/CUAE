// MmsFlatmapIpcClient.h 
// IPC client object implementation, also track heartbeat for connection well-check.

#ifndef MMS_FLATMAPIPC_CLIENT_H
#define MMS_FLATMAPIPC_CLIENT_H

#ifdef MMS_WINPLATFORM
#pragma once
#endif

class MmsFlatmapIpcClient
{
public:
  MmsFlatmapIpcClient(int sessionId);
  virtual ~MmsFlatmapIpcClient();

  void acksUpToDate() { this->heartbeatAck = this->heartbeats; }
  int getSessionId() { return sessionId; }
  unsigned int getServerId() { return serverId; }
  unsigned int getHeartBeats() { return heartbeats; }
  unsigned int getHeartBeatInterval() { return heartbeatInterval; }
  unsigned int getHeartBeatAck() { return heartbeatAck; }
  unsigned int isHeartbeatPayloadMediaResources() { return heartbeatPayloadMediaResources; }
  unsigned int addHeartBeat() { return heartbeats++; }

  void assignServerId(unsigned int n) { serverId = n; }
  void assignHeartBeatInterval(unsigned int n) { heartbeatInterval = n; }
  void assignHeartBeatPayloadMediaResources(unsigned int n) { heartbeatPayloadMediaResources = n; }

private:
  int sessionId;                                  // IPC session ID
  unsigned int serverId;
  unsigned int heartbeats;
  unsigned int heartbeatInterval;
  unsigned int heartbeatAck;
  unsigned int heartbeatPayloadMediaResources;
};

#endif

