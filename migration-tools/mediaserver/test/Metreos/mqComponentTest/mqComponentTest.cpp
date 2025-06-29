//
// mqComponentTest.cpp
// Validates MMS MSMQ objects MmsMqWriter and MmsMqListener
//

#include "StdAfx.h"
#include "mqComponentTest.h"


QUEUEHANDLE hqSend[NUMCLIENTS], hqReceive;                                           
char  qNameSend[NUMCLIENTS * QNAMESIZE];    // Global array of send queues
char* getqname(const int i) { return qNameSend + (i * QNAMESIZE); } 
MmsMqWriter*   mqwriters[NUMCLIENTS];
MmsMqListener* mqlistener;
                                           
                                            // Listener task message handler
int QueueManager::handleMessage(MmsMsg* msg)
{ 
  switch(msg->type())
  { 
    case MMSM_DATA:
         appxml = (MmsAppMessage*)msg->param();
         queuehandle = (int)appxml->param();
         this->nextstate();
         break;

    case MMSM_STATE:
         this->nextstate();
         break;

    case MMSM_INITTASK: 
         onInitTask(msg); 
         break;  
  
    case MMSM_QUIT: 
         MMSLOG((LM_INFO,"%s queue mgr exits\n",taskName)); 
         break;

    default: return 0;
  } 

  return 1;
}

 
void QueueManager::onInitTask(MmsMsg* msg)  // Queue mgr startup
{          
  qNameListen = config->clientParams.msmqMmsQueueName;
  mNameListen = config->clientParams.msmqMmsMachineName;                                             
  mNameSend   = mNameListen;                // Send machine same as receive
                                            // Open rcv queue & client queue(s)
  if ((-1 == this->openReceiveQueue()) || (-1 == this->openSendQueues())) return;  
  state = 0;

  printf("%s starting MQ listener on %x\n",taskName,mqlistener->handle());
  mqlistener->start();                      // Start MQ listener thread

  this->nextstate();                        // Launch state machine
}



void QueueManager::nextstate()
{
  int isForceNextState = FALSE;

  switch(state)
  {
    case 0:
         this->sendConnect(0);
         break;

    case 1:
         printf("%s MQ msg rcvd from %x\n", taskName, appxml->param());
         this->closeWriter(0);
         isForceNextState = TRUE;
         break;

    case 2:
         this->killMqListener();
         isForceNextState = TRUE;
         break;

    case 3:
         this->closeReceiveQueue();
         isForceNextState = TRUE;
         break;

    default: state = FINALSTATE;
  }

  state++;
  if  (state >= FINALSTATE)
       this->onFinalState();
  else
  if  (isForceNextState)
       this->postMessage(MMSM_STATE);
}


                                            // Ctor
QueueManager::QueueManager(MmsTask::InitialParams* p): MmsBasicTask(p) 
{ 
  config=(MmsConfig*)p->config; 
  state = FINALSTATE;
}



void QueueManager::sendConnect(const int n)
{
  QUEUEHANDLE hq = hqSend[n];
  MmsAppMessage* outxml = new MmsAppMessage(hq);
  outxml->putMessageID(outxml->messagenames[outxml->MMSMSG_CONNECT]);
  outxml->putParameter(outxml->paramnames  [outxml->PORT], 1000);
  outxml->putParameter(outxml->paramnames  [outxml->IP_ADDRESS], "127.0.0.1"); 
  outxml->terminateReturnMessage();
            
  printf("%s sending to %x\n",taskName,hq);
  MmsMqWriter* writer = mqwriters[n];                 
  writer->putMqMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  delete outxml;
}


void QueueManager::closeWriter(const int n)
{
  QUEUEHANDLE hq = hqSend[n];
  printf("%s closing send queue %x\n",taskName, hq);
  MmsMqWriter* writer = mqwriters[n];                 
  writer->closeQueue();
}


void QueueManager::killMqListener()          
{
  printf("%s killing MQ listener thread\n",taskName);
  mqlistener->shutdown();  
  ACE_Thread_Manager::instance()->wait_task(mqlistener);         
}


void QueueManager::closeReceiveQueue()
{
  QUEUEHANDLE hq = mqlistener->handle();
  printf("%s closing listener %x\n",taskName,hq);
  mqlistener->closeQueue();  
}


void QueueManager::onFinalState()
{
  this->postMessage(MMSM_QUIT);
}


int QueueManager::openReceiveQueue()
{
  MmsTask::InitialParams params(0, GROUPID_MQLISTENER, this);                                           
  params.config = this->config;   
  mqlistener    = new MmsMqListener(&params); 

  int  result = mqlistener->openQueue(mNameListen, qNameListen); 
  if  (result == -1) return -1;         
  
  int  purged = mqlistener->purge();        // Clear out local queue
  if  (purged)  MMSLOG((LM_INFO,"%s purged %d\n",taskName,purged)); 
  return 0;
}
  

int QueueManager::openSendQueues()
{
  for(int i=0; i < NUMCLIENTS; i++)
  {
      printf("%s instantiating MQ writer %d\n",taskName,i+1);
      MmsMqWriter* writer = new MmsMqWriter();
      char*   pname = getqname(i);        // When we test multiple send queues
      sprintf(pname, "%s", qNameListen);  // we'll mask in unique names here 
                                          // For now we use the same queue
      int  result = writer->openQueue(mNameSend, pname); 
      if  (result == -1) return -1;

      hqSend[i] = writer->handle();
      mqwriters[i] = writer;
  }

  return 0;
}



int main(int argc, char *argv[])
//-----------------------------------------------------------------------------
// main
//-----------------------------------------------------------------------------
{
  MmsConfig* config = new MmsConfig;       
  if  (config->readLocalConfigFile() == -1) 
  {    printf("Could not read config\n"); 
       return 0;
  }

  MmsTask::InitialParams params(0,0,0);     // Launch local queue listener
  params.config = config;
  strcpy(params.taskName,"TEST");
  QueueManager* server = new QueueManager(&params);
  MMSLOG((LM_INFO,"MAIN launching server thread\n"));
  server->start();                          // Block until thread exit
  ACE_Thread_Manager::instance()->wait_task(server);
 
  delete server;
  delete config;
  WAITFORKEY("\nany key ...");
  return 0;
}