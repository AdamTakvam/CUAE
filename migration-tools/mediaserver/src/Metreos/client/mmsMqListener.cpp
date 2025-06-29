//
// MmsMqListener.cpp 
//
// Queue object and listener thread for a MMS MSMQ receive queue
// 
#include "StdAfx.h"
#pragma warning(disable:4786)
#include "mmsMqListener.h"
#include "mmsMqWriter.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION



int MmsMqListener::svc()                    // Listener thread procedure
{
  setThreadInfo(); 
  if  (onThreadStarted() < 0) return 0; 

  while(1)                                  // MQ receive queue event loop
  {
    if  (this->isBailing) break;                                                   

    DWORD timeoutmsecs  = this->config->clientParams.msmqTimeoutMsecs;
    if   (timeoutmsecs < 1 || timeoutmsecs > 5000) timeoutmsecs = 3000;
                                            // Block until msg or timeout
    const int result = this->getMqMessage(timeoutmsecs);

    switch(result)
    {                                       
      case 1:                               // A message was dequeued. so ...                                                        
                                            // wrap it & let parent handle it
           this->parent->postMessage(MMSM_DATA, 
                (long) new MmsAppMessage(this->szBody));     
           break;                           // ... and continue listening

      case 0:                               // Msg or queue not yet available
           break;                           // ... so continue listening

      case MMS_MQRESULT_INTERNAL_QUIT:      // Quit notice from adapter, so ...
           return 0;                        // exit event loop & cancel thread

      case -1:                              
      default:                              // Unrecoverable MSMQ error
           this->parent->postMessage(MMSM_ERROR, MMS_ERROR_LISTENER_DOWN);
           this->handleTaskMessages();      // ... so exit event loop
           return 0;                        // and cancel listener thread 
    }
  }

  return 0;
}



int MmsMqListener::getMqMessage(DWORD timeoutms)
{
  // Block until a MSMQ message arrives or timeout. Returns 1 if a message was
  // dequeued into szBody, zero if timeout or other continue condtition,
  // or -1 if error is unrecoverable.

  // Note that we expect MSMQ communication from client to be unicode, with
  // the message payload encoded independently as a UTF-8 string. Payload is
  // converted from UTF-8 to ansi as it is wrapped in a MmsAppMessage.

  #ifdef MMS_LINK_WITH_MSMQ

  memset(szBody, 0, MMS_MAX_XMLMESSAGESIZE); 

  this->initMqMessageProperties();
  
  HRESULT hr = MQReceiveMessage              
        (this->hq, timeoutms, MQ_ACTION_RECEIVE, &msgprops, 0, 0, 0, 0);

  switch(hr)
  {
    case MQ_OK:  
    case MQ_INFORMATION_PROPERTY:  
         break;                             // Found a message

    case MQ_ERROR_IO_TIMEOUT:  
         if (this->logliveTimer.countdown() == -1)          
             MMSLOG((LM_DEBUG,"MQLI listening on %x ...\n", this->hq));           
         return 0;                          // Timed out ... continue

    case MQ_ERROR_OPERATION_CANCELLED:      // Lost by MQ ... continue
         MMSLOG((LM_ERROR,"MQLI message on %x canceled by msmq\n", this->hq));
         return 0;

    default:                                // Unrecoverable error
         MMSLOG((LM_ERROR,"MQLI MQReceiveMessage failed with %x\n", hr)); 
         showresult(hr);  
         #if 0
         for(int i=0; i < MMS_MQ_RECEIVE_MAX_PROPERTIES; i++) 
         {   char buf[48]; wsprintf(buf,"MQLI prop %d %08x", i, aMsgStatus[i]);
             MMSLOG((LM_ERROR,"%s\n", buf));
         }
         #endif
         return -1;                         // Bail
  } 


  MMSLOG((LM_TRACE,"MQLI message received\n")); 
  consecutiveTimeoutCount = 0;         
                                            // Retrieve time of message
  this->messageSentTime = msgprops.aPropVar[this->pnSenttime].ulVal;
                                            // Retrieve size of message body
  this->bodySize = msgprops.aPropVar[this->pnBodysize].ulVal; 

  if  (this->isAdapterQuitMessage())        // Is this a notice from adapter to
       this->isBailing = TRUE;              // shut down the listener?

  #else   // #ifdef MMS_LINK_WITH_MSMQ
  MMSLOG((LM_ERROR,"MQLI message queueing disabled on this server\n"));
  this->isBailing = TRUE;
  #endif  // #ifdef MMS_LINK_WITH_MSMQ

  return this->isBailing? MMS_MQRESULT_INTERNAL_QUIT: 1;
} 


                                            // Specify MSMQ msg properties
void MmsMqListener::initMqMessageProperties()
{
  ULONG ulBufferSize = MMS_MAX_XMLMESSAGESIZE;                                            

  aMsgPropId [0] = PROPID_M_BODY_SIZE;      // 0. body size            
  aMsgPropVar[0].vt = VT_NULL;   
  this->pnBodysize = 0;

  aMsgPropId [1] = PROPID_M_BODY;           // 1. body text          
  aMsgPropVar[1].vt = VT_VECTOR | VT_UI1;        
  aMsgPropVar[1].caub.pElems = (unsigned char*)szBody;   
  aMsgPropVar[1].caub.cElems = ulBufferSize; 
  this->pnBodytext = 1; 
                                            
  aMsgPropId [2] = PROPID_M_BODY_TYPE;      // 2. body type       
  aMsgPropVar[2].vt = VT_NULL;
  this->pnBodytype  = 2;

  aMsgPropId [3] = PROPID_M_SENTTIME;       // 3. sent time  
  aMsgPropVar[3].vt = VT_NULL; 
  this->pnSenttime  = 3;                                               
                                            // Initialize MQMSGPROPS 
  msgprops.cProp    = MMS_MQ_RECEIVE_MAX_PROPERTIES;             
  msgprops.aPropID  = aMsgPropId;           // IDs of the message properties
  msgprops.aPropVar = aMsgPropVar;          // Values of the message properties
  msgprops.aStatus  = aMsgStatus;           // Error reports
}


                                            
int MmsMqListener::handleTaskMessages()     // Get & handle task messages
{                                           // See comments at handleMessage()
  while(1)                                  // While messages in queue ...
  {  
    MmsMsg* msg = getq(TIMEOUT_IMMED);      // Get a msg from q w/o blocking                                          
    if  (msg == NULL) break;

    const int msgtype = msg->type();   
           
    if  (handleMessage(msg));               // Handle this message
    else defHandleMessage(msg);              

    MMSMSG_RELEASE(msg);                    // Decrement ref count; delete if 0 

    if  (MMSM_QUIT == msgtype) break;     
  }

  return 0;
}


                                            // Handle a task queue message
int MmsMqListener::handleMessage(MmsMsg* msg)
{                                           // We currently only check for the
  switch(msg->type())                       // initial MMSM_INITASK. Subsequent 
  {                                         // internal communication (quit msg)
    case MMSM_SHUTDOWN:                     // arrives via the MSMQ queue.
         this->postMessage(MMSM_QUIT);              
         break;                             // These messages are no longer
    case MMSM_QUIT:                         // sent. We expect a quit message  
         this->isBailing = TRUE;            // via the MSMQ queue instead
         break;                   
    default: return 0;
  } 
 
  return 1;
}



int MmsMqListener::purge()                   
{
  int count = 0;
  this->initMqMessageProperties();

  #ifdef MMS_LINK_WITH_MSMQ
  HRESULT hr = 0;
  
  while(1)
  {  
    hr = MQReceiveMessage(this->hq, 0, MQ_ACTION_RECEIVE, &msgprops, 0, 0, 0, 0);
    if  (hr != MQ_OK && hr != MQ_INFORMATION_PROPERTY) break;   
    count++; 
  } 

  #endif // #ifdef MMS_LINK_WITH_MSMQ                                

  return count;
}



int MmsMqListener::isAdapterQuitMessage()
{ 
  return memcmp(szBody, MMS_MQLISTENER_INTERNAL_QUIT, 
                 sizeof(MMS_MQLISTENER_INTERNAL_QUIT)) == 0; 
} 



int MmsMqListener::onThreadStarted()        // Thread startup hook
{ 
  MMSLOG((LM_INFO,"MQLI thread %t started at priority %d\n", osPriority)); 
  this->islistening = TRUE;
  this->handleTaskMessages();              
  return 0;
} 



int MmsMqListener::close(unsigned long)     // Thread exit hook
{
  MMSLOG((LM_INFO,"MQLI thread %t exit\n"));
  this->islistening = FALSE;
  return 0;
}



int MmsMqListener::shutdown()               // Kill listener thread
{
  if  (!this->islistening) return -1;
  MMSLOG((LM_INFO,"%s stop mq listener %x\n",taskName,this->handle()));
  MmsMqWriter* tempwriter = new MmsMqWriter(NOCREATE);
                                            // Open our receive queue for write
  if  (-1 != tempwriter->openQueue(this->machineName(), this->queueName())) 
  {
       tempwriter->putMqMessage(MMS_MQLISTENER_INTERNAL_QUIT, 
            sizeof(MMS_MQLISTENER_INTERNAL_QUIT, MMS_MQLISTENER_INTERNAL_QUIT));

       tempwriter->closeQueue();
  }

  delete tempwriter;
  this->isBailing = TRUE;
  return 0;
}


                                          // Ctor
MmsMqListener::MmsMqListener(InitialParams* params): 
  MmsTask((InitialParams*) params), MmsMq(READER) 
{
  time(&this->mmsStartTime);
  this->config = (MmsConfig*) params->config;   
  ACE_OS::strcpy(this->taskName, "MQLI");
  isBailing = islistening = consecutiveTimeoutCount = 0;
  this->logliveTimer.reset(config->serverParams.systemStatsLogIntervalMsecs * 4);
}




