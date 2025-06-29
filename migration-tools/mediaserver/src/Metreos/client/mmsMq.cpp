// 
// MmsMq.cpp 
//
// MMS MSMQ message queue object
// 
#include "StdAfx.h"
#pragma warning(disable:4786)
#include "mmsMq.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION



int MmsMq::openQueue(const char* machineName, const char* queueName, const DWORD mode)
{                            
  #ifdef MMS_LINK_WITH_MSMQ 
              
  if ((mode & CREATEIF) && !this->disallowCreate)                                         
       if  (-1 == createq(machineName, queueName, NULL, this->label(mode), TRUE))  
            return -1;  

  DWORD mqAccessMode = 0;
  if  (mode & SENDACCESS)     mqAccessMode |= MQ_SEND_ACCESS; else
  if  (mode & RECEIVEACCESS)  mqAccessMode |= MQ_RECEIVE_ACCESS;

  this->hq = this->openq(machineName, queueName, mqAccessMode, &this->formatname);

  return NULL == this->hq? -1: 0;

  #else   // #ifdef MMS_LINK_WITH_MSMQ
  MMSLOG((LM_ERROR,"MQLI message queueing disabled on this server\n"));
  return -1;
  #endif  // #ifdef MMS_LINK_WITH_MSMQ
}



int MmsMq::openQueue(const char* machineName, const char* queueName)
{
  int result = -1;
  DWORD flags = this->queuetype == READER? RECEIVEACCESS: SENDACCESS;
  if  (!this->disallowCreate) flags |= CREATEIF;

  #ifdef MMS_LINK_WITH_MSMQ

  result = this->openQueue(machineName, queueName, flags); 

  if  (result != -1)
  {    this->cachedMachineName = new char[strlen(machineName)+1];
       this->cachedQueueName   = new char[strlen(queueName)+1];
       ACE_OS::strcpy(this->cachedMachineName, machineName); 
       ACE_OS::strcpy(this->cachedQueueName,   queueName); 
  } 

  #endif  // #ifdef MMS_LINK_WITH_MSMQ

  return result;
}



QUEUEHANDLE MmsMq::openq(const char* machineName, const char* queueName, 
  const DWORD mode, MmsMqFormatName* p)
{
  mqFormatName(machineName, queueName, p); 
  QUEUEHANDLE qh = NULL;

  #ifdef MMS_LINK_WITH_MSMQ

  HRESULT hr = MQOpenQueue(p->wszFormatName, mode, MQ_DENY_NONE, &qh);

  if  (FAILED(hr)) 
  {    MMSLOG((LM_ERROR,"MQMQ could not open %s\n", p->szFormatName));
       showresult(hr);
       qh = NULL;
  }
  else MMSLOG((LM_NOTICE,"MQMQ opened %s as %x\n", p->szFormatName, qh));

  #endif  // #ifdef MMS_LINK_WITH_MSMQ

  return qh;
}



int MmsMq::createq(const char* machineName, const char* queueName, 
  PSECURITY_DESCRIPTOR psecurity, const char* label, const int isIgnoreError)
{
  const int MAXQUEUEPROPERTIES = 2;
  MmsMqFormatName fname;
                                            // Machine name can be "."
  LPWSTR wszPathName = mqPathName(machineName, queueName, &fname); 
                          
  MQQUEUEPROPS  QueueProps;                 // Queue property structs 
  MQPROPVARIANT aQueuePropVar[MAXQUEUEPROPERTIES];
  QUEUEPROPID   aQueuePropId [MAXQUEUEPROPERTIES];
  HRESULT       aQueueStatus [MAXQUEUEPROPERTIES], hr = -1;
                                        
  aQueuePropId [0] = PROPID_Q_PATHNAME;            
  aQueuePropVar[0].vt = VT_LPWSTR;
  aQueuePropVar[0].pwszVal = wszPathName;

  WCHAR wszLabel[MQ_MAX_Q_LABEL_LEN];
  MultiByteToWideChar(CP_ACP, 0, label, strlen(label)+1,
        wszLabel, sizeof(wszLabel) / sizeof(wszLabel[0]));
      
  aQueuePropId [1] = PROPID_Q_LABEL;
  aQueuePropVar[1].vt = VT_LPWSTR;
  aQueuePropVar[1].pwszVal = wszLabel;
                                            // Initialize MQQUEUEPROPS 
  QueueProps.cProp    = MAXQUEUEPROPERTIES; // Number of properties
  QueueProps.aPropID  = aQueuePropId;       // IDs of the queue properties
  QueueProps.aPropVar = aQueuePropVar;      // Values of the queue properties
  QueueProps.aStatus  = aQueueStatus;       // Pointer to the return status

  WCHAR wszFormatNameBuf[256];              
  DWORD dwFormatNameBufLen = sizeof(wszFormatNameBuf) / sizeof(wszFormatNameBuf[0]);

  #ifdef MMS_LINK_WITH_MSMQ
                                            // Create the queue
  hr  = MQCreateQueue(psecurity,            // Security descriptor
                      &QueueProps,          // Queue property structure
                      wszFormatNameBuf,     // Out: format name  
                      &dwFormatNameBufLen); // Out: queue's format name length

  #endif  // #ifdef MMS_LINK_WITH_MSMQ

  int result = 0;

  switch(hr)
  { case MQ_ERROR_QUEUE_EXISTS:
         MMSLOG((LM_INFO,"MQMQ found %s\n",fname.szFormatName));
         break;

    case MQ_OK:
    case MQ_INFORMATION_PROPERTY: 
         MMSLOG((LM_INFO,"MQMQ created %s\n",fname.szFormatName));
         break;

    default: 
         if  (isIgnoreError) break;
         MMSLOG((LM_ERROR,"MQMQ could not create %s\n",fname.szFormatName));
         showresult(hr);
         result = -1;        
  }

  return result;
}



int MmsMq::deleteQueue()
{
  return this->deleteq(this->cachedMachineName, this->cachedQueueName);
}



int MmsMq::deleteq(const char* machineName, const char* queueName) 
{
  if  (machineName == NULL || queueName == NULL) return -1;
  MmsMqFormatName fname;
  int  result = 0;
  HRESULT hr = -1;
                                            // Machine name can be "."
  LPWSTR wszFormatName = mqFormatName(machineName, queueName, &fname); 

  #ifdef MMS_LINK_WITH_MSMQ
                          
  hr = MQDeleteQueue(wszFormatName);

  #endif  // #ifdef MMS_LINK_WITH_MSMQ

  switch(hr)
  { case MQ_OK:
         MMSLOG((LM_DEBUG,"MQMQ deleted %s\n",fname.szFormatName));
         break;

    default: 
         MMSLOG((LM_ERROR,"MQMQ could not delete %s\n",fname.szFormatName));
         showresult(hr);
         result = -1;
  }

  return result;
}



int MmsMq::closeQueue()
{
  if  (!this->hq) return -1;
  int  result = 0;
  HRESULT hr = -1;


  #ifdef MMS_LINK_WITH_MSMQ

  hr = MQCloseQueue(this->hq);

  #endif

  switch(hr)
  { case MQ_OK:
         MMSLOG((LM_NOTICE,"MQMQ closed queue %x\n",this->hq));
         break;

    default: 
         MMSLOG((LM_ERROR,"MQMQ could not close %x\n",this->hq));
         showresult(hr);
         result = -1;
  }

  this->hq = NULL;
  return result;
}


                                           
char* MmsMq::label(const DWORD mode)        // Default impl
{
  static char* labr = "MMS receive", *labs = "MMS client", *labd = "MMS";
  return mode & RECEIVEACCESS? labr: mode & SENDACCESS? labs: labd;
}


                                            // Ctor
MmsMq::MmsMq(const int flags): hq(0), queuetype(0), disallowCreate(0)     
{
  if  (flags & READER) this->queuetype = READER; else
  if  (flags & WRITER) this->queuetype = WRITER;  
  if  (flags & NOCREATE) this->disallowCreate = TRUE;
  cachedMachineName = cachedQueueName = NULL;
}



MmsMq::~MmsMq()                             // Dtor
{
  this->closeQueue();
  if  (cachedMachineName) delete[] cachedMachineName;
  if  (cachedQueueName)   delete[] cachedQueueName;
}



void MmsMq::showresult(HRESULT hr)
{
  switch(hr)
  {
    case MQ_OK:
         MMSLOG((LM_INFO, "MQMQ success\n"));
         break;
    case MQ_ERROR_IO_TIMEOUT:
         MMSLOG((LM_INFO, "MQMQ timeout\n"));
         break; 
    case MQ_ERROR_ACCESS_DENIED:
         MMSLOG((LM_ERROR, "MQMQ no MQ_SEND_ACCESS\n"));
         break;  
    case MQ_ERROR_QUEUE_NOT_FOUND: 
         MMSLOG((LM_ERROR, "MQMQ queue not found\n"));
         break;
    case MQ_ERROR_REMOTE_MACHINE_NOT_AVAILABLE:
         MMSLOG((LM_ERROR, "MQMQ remote machine not available\n"));
         break;
    case MQ_ERROR_SHARING_VIOLATION:
         MMSLOG((LM_ERROR, "MQMQ sharing violation\n"));
         break; 
    case MQ_ERROR_UNSUPPORTED_ACCESS_MODE:
         MMSLOG((LM_ERROR, "MQMQ bad access mode\n"));
         break;
    case MQ_ERROR_UNSUPPORTED_FORMATNAME_OPERATION:
         MMSLOG((LM_ERROR, "MQMQ not a formatname operation\n"));
         break;
    case MQ_ERROR_ILLEGAL_PROPERTY_VALUE :
         MMSLOG((LM_ERROR, "MQMQ bad property value\n"));
         break;
    case MQ_ERROR_ILLEGAL_FORMATNAME: 
         MMSLOG((LM_ERROR, "MQMQ bad format name\n"));
         break;
    case MQ_ERROR_ILLEGAL_QUEUE_PATHNAME :
         MMSLOG((LM_ERROR, "MQMQ PROPID_Q_PATHNAME has bad path name string\n"));
         break;
    case MQ_ERROR_ILLEGAL_SECURITY_DESCRIPTOR :
         MMSLOG((LM_ERROR, "MQMQ security descriptor has an invalid structure\n"));
         break;
    case MQ_ERROR_BAD_SECURITY_CONTEXT:
         MMSLOG((LM_ERROR, "MQMQ PROPID_M_SECURITY_CONTEXT) corrupt\n"));
         break;
    case MQ_ERROR_CORRUPTED_PERSONAL_CERT_STORE:
         MMSLOG((LM_ERROR, "MQMQ IE cert store corrupt\n"));
         break;
    case MQ_ERROR_COULD_NOT_GET_USER_SID:
         MMSLOG((LM_ERROR, "MQMQ could not get userid specified in PROPID_M_SENDERID\n"));
         break;
    case MQ_ERROR_DTC_CONNECT: 
         MMSLOG((LM_ERROR, "MQMQ cannot connect to MS DTC\n"));
         break;
    case MQ_ERROR_INVALID_HANDLE:
         MMSLOG((LM_ERROR, "MQMQ handle invalid\n"));
         break;
    case MQ_ERROR_MESSAGE_STORAGE_FAILED:
         MMSLOG((LM_ERROR, "MQMQ MQMSG_DELIVERY_RECOVERABLE could not store on local comp\n"));
         break;
    case MQ_ERROR_NO_INTERNAL_USER_CERT:
         MMSLOG((LM_ERROR, "MQMQ no/corrupt internal cert\n"));
         break;
    case MQ_ERROR_STALE_HANDLE: 
         MMSLOG((LM_ERROR, "MQMQ queue handle obtained in prior MQMQ session\n"));
         break;
    case MQ_ERROR_INSUFFICIENT_PROPERTIES :
         MMSLOG((LM_ERROR, "MQMQ No path name specified (PROPID_Q_PATHNAME).\n"));
         break;
    case MQ_ERROR_INVALID_OWNER :
         MMSLOG((LM_ERROR, "MQMQ pathname in PROPID_Q_PATHNAME has bad machine name, or msmq not installed\n"));
         break;
    case MQ_ERROR_INVALID_PARAMETER:
         MMSLOG((LM_ERROR, "MQMQ An invalid parameter was specified \n"));
         break;
    case MQ_ERROR_SENDERID_BUFFER_TOO_SMALL:
         MMSLOG((LM_ERROR, "MQMQ sender id buffer too small\n"));
         break;
    case MQ_ERROR_PROV_NAME_BUFFER_TOO_SMALL:
         MMSLOG((LM_ERROR, "MQMQ provider name buffer too small\n"));
         break;
    case MQ_ERROR_LABEL_BUFFER_TOO_SMALL:
         MMSLOG((LM_ERROR, "MQMQ label buffer too small\n"));
         break;
    case MQ_ERROR_MESSAGE_ALREADY_RECEIVED: 
         MMSLOG((LM_ERROR, "MQMQ msg already rcvd in another thread\n"));
         break;
    case MQ_ERROR_OPERATION_CANCELLED: 
         MMSLOG((LM_ERROR, "MQMQ operation canceled\n"));
         break;
    case MQ_ERROR_BUFFER_OVERFLOW: 
         MMSLOG((LM_ERROR, "MQMQ buffer overflow\n"));
         break;
    case MQ_ERROR_TRANSACTION_USAGE: 
         MMSLOG((LM_ERROR, "MQMQ transaction usage error\n"));
         break;    
    case MQ_ERROR_NO_DS:
         MMSLOG((LM_ERROR, "MQMQ Cannot access directory service\n"));
         break;
    case MQ_ERROR_PROPERTY :
         MMSLOG((LM_ERROR, "MQMQ 1+ properties resulted in error\n"));
         break;
    case MQ_ERROR_PROPERTY_NOTALLOWED:
         MMSLOG((LM_ERROR, "MQMQ a property is not valid on creation\n"));
         break;
    case MQ_ERROR_QUEUE_EXISTS:
         MMSLOG((LM_ERROR, "MQMQ queue exists\n"));
         break;
    case MQ_ERROR_SERVICE_NOT_AVAILABLE :
         MMSLOG((LM_ERROR, "MQMQ cannot connect to queue mgr msmq service unavailable\n"));
         break;
    case MQ_ERROR_WRITE_NOT_ALLOWED:
         MMSLOG((LM_ERROR, "MQMQ cannot write to DS\n"));
         break;
    case MQ_INFORMATION_FORMATNAME_BUFFER_TOO_SMALL :
         MMSLOG((LM_ERROR, "MQMQ OK but size of out buf too small\n"));
         break;
    case MQ_INFORMATION_OPERATION_PENDING:
         MMSLOG((LM_ERROR, "MQMQ an async op is pending\n"));
         break;
    case MQ_INFORMATION_PROPERTY:
         MMSLOG((LM_INFO, "MQMQ OK but 1+ properties resulted in warning\n"));
         break;
 }
}
