//
// MmsMqWriter.cpp  
//
// MSMQ send queue object
//
#ifndef MMS_MQWRITER_H
#define MMS_MQWRITER_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include "mmsMq.h"
         
#define MMS_MQ_SEND_PROPERTY_COUNT 3  


class MmsMqWriter: public MmsMq
{ 
  public:
  MmsMqWriter(const int flags=0);
  virtual ~MmsMqWriter() {}
  void setname();

  int  putMqMessage(char* body, const int length, const char* name=0, const int rc=0);

  unsigned int  heartbeats;
  unsigned int  heartbeatInterval;
  unsigned int  heartbeatAck;     // 215
  unsigned int  serverID;                   // Client's ID for this server port
  unsigned int  isHeartbeatPayloadMediaResources;
  void acksUpToDate() { this->heartbeatAck = this->heartbeats; }

  protected:
  MQMSGPROPS    msgprops;                   // MQ message properties
  MSGPROPID     aMsgPropId [MMS_MQ_SEND_PROPERTY_COUNT];
  MQPROPVARIANT aMsgPropVar[MMS_MQ_SEND_PROPERTY_COUNT];
  HRESULT       aMsgStatus [MMS_MQ_SEND_PROPERTY_COUNT];
  char objname[16];

  void initMqMessageProperties(char* body, const int bodysize);
};



#endif
