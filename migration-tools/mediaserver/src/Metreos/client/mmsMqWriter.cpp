//
// MmsMqWriter.cpp  
//
// MSMQ send queue object
//
#include "StdAfx.h"
#pragma warning(disable:4786)
#include "mmsMqWriter.h"
#include "mmsAppMessage.h"
#include "mmsServerCmdHeader.h"
const char* defmsgname = "MQ message";

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION



int MmsMqWriter::putMqMessage
( char* szBody, const int bodylen, const char* name, const int retcode)
{
  // Although message itself is unicode, the message body is encoded separately.
  // Metreos client expects UTF-8, and so our szBody is presumed to address a
  // UTF-8-encoded string. 

  #ifdef MMS_LINK_WITH_MSMQ

  this->initMqMessageProperties(szBody, bodylen);
  const char* msgname = name? name: defmsgname;
  
  HRESULT hr = MQSendMessage(this->hq, &msgprops, MQ_NO_TRANSACTION);                    
  
  if  (FAILED(hr)) 
  {    MMSLOG((LM_ERROR,"%s MQSendMessage failed with %x\n",objname,hr));
       showresult(hr);
       #if 0
       for(int i=0; i < MMS_MQ_SEND_PROPERTY_COUNT; i++) 
       {   char buf[48]; wsprintf(buf,"%s prop %d %08x",objname,i,aMsgStatus[i]);
           MMSLOG((LM_ERROR,"%s\n", buf));
       }
       #endif
  } 
  else 
  if  (MMS_ISCOMMANDERROR(retcode))
       MMSLOG((LM_DEBUG,"%s %s (%d) posted to %x\n", 
               objname, msgname, retcode, this->hq));
  else
  {    ACE_Log_Priority pri = name && ACE_OS::strcmp(name, MMS_HEARTBEAT_NAME) == 0?
               LM_TRACE: LM_DEBUG;   
       MMSLOG((pri,"%s %s posted to %x\n", objname, msgname, this->hq));
  }

  return (FAILED(hr))? -1: 0;

  #else  // #ifdef MMS_LINK_WITH_MSMQ
  MMSLOG((LM_ERROR,"MQLI message queueing disabled on this server\n"));
  return -1;
  #endif // #ifdef MMS_LINK_WITH_MSMQ
}


                                             
void MmsMqWriter::initMqMessageProperties(char* szBody, const int bodysize)
{                                           // Specify msg properties to be sent                                           
  aMsgPropId [0] = PROPID_M_LABEL;          // 0. message label
  aMsgPropVar[0].vt = VT_LPWSTR;            // Specified as unicode string 
  aMsgPropVar[0].pwszVal = L"Metreos MediaServer";     

  // Note that the body type is a separate message property; it is not set 
  // using the "type indicator" of the PROPID_M_BODY property. 
  // If the sending application does not set the PROPID_M_BODY_TYPE property, 
  // msmq sets the body type to VT_EMPTY. 

  // Note (see PROPVARIANT) that there is not an explicit body specification  
  // for UTF-formatted. Since UTF-8 is an 8-bit encoding, it is a VT_LPSTR
              
  DWORD dwBodyType = VT_LPSTR;              // 1. body type                
  aMsgPropId [1] = PROPID_M_BODY_TYPE;
  aMsgPropVar[1].vt = VT_UI4;               // Data type of body type
  aMsgPropVar[1].ulVal = dwBodyType;        // Body type

  aMsgPropId [2] = PROPID_M_BODY;           // 2. message body
  aMsgPropVar[2].vt = VT_VECTOR | VT_UI1;    
  aMsgPropVar[2].caub.pElems = (unsigned char*)szBody;
  aMsgPropVar[2].caub.cElems = (bodysize + 1) * sizeof(char);
             
                                            // Initialize MQMSGPROPS 
  msgprops.cProp    = MMS_MQ_SEND_PROPERTY_COUNT;             
  msgprops.aPropID  = aMsgPropId;           // IDs of the message properties
  msgprops.aPropVar = aMsgPropVar;          // Values of the message properties
  msgprops.aStatus  = aMsgStatus;           // Error reports
}



void MmsMqWriter::setname()
{
  ACE_OS::sprintf(this->objname, "%04X", this->handle());
}


                                            // Ctor
MmsMqWriter::MmsMqWriter(const int flags): MmsMq(flags | WRITER)                  
{
  heartbeatInterval = heartbeats = heartbeatAck 
                    = isHeartbeatPayloadMediaResources = 0;   
                 
  ACE_OS::strcpy(this->objname,"MQWR");
}

