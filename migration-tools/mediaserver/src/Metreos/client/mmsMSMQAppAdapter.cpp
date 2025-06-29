//
// MmsMSMQAppAdapter.cpp 
//
// mmsMSMQAppAdapter.cpp -- Micrsoft Message Queue transport adapter
//
#include "StdAfx.h"
#pragma warning(disable:4786)
#include "mmsMSMQAppAdapter.h"
#include <minmax.h>

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION



MmsMSMQAppAdapter::MmsMSMQAppAdapter(MmsTask::InitialParams* params): MmsMqAppAdapter(params)
{ 
    this->mqListener = NULL;
}



MmsMSMQAppAdapter::~MmsMSMQAppAdapter()
{
  this->onShutdown();
}



void MmsMSMQAppAdapter::postClientsHeartbeat()
{          
  // For each client, if the client's designated heartbeat interval is reached, 
  // post client a heartbeat message. We receive one pulse/sec from server.
  // If a client has not been responding to heartbeats, disconnect the client.
  // We will not disconnect more than one such client per pulse.

  ++heartbeats;
  if  (clientQueues.size() == 0) return;
  QUEUEHANDLE nonresponder = NULL;
 
  std::map<QUEUEHANDLE, MmsMqWriter*>::iterator i;         
  MmsMqWriter* writer = NULL;
                                            // For each client ...
  for(i = this->clientQueues.begin(); i != clientQueues.end(); i++)
  {   
      if  (NULL == (writer = i->second)) continue;

      const int intervalseconds             
              = writer->heartbeatInterval? writer->heartbeatInterval:  
                        this->config->clientParams.heartbeatIntervalSecs;
                                            
      if ((heartbeats % intervalseconds) == 0)  
      {                                     // 626 fix
          QUEUEHANDLE nonrespondingClient = postClientHeartbeat(writer); 
          if (nonresponder == NULL && nonrespondingClient != NULL)
              nonresponder = nonrespondingClient;
      }
             
  }

  if  (nonresponder != NULL)                // Disconnect comatose client
  {    MMSLOG((LM_ERROR,"%s %x appears down: disconnecting\n",
               taskName, nonresponder));
       this->unregisterClient(nonresponder);
  }
}



QUEUEHANDLE MmsMSMQAppAdapter::postClientHeartbeat(MmsMqWriter* writer)
{    
  // If client is responsive, send client a heartbeat message; otherwise
  // return client's queue as nonrepsonsive
 
  const QUEUEHANDLE clientID = writer->handle();
  int  isNonResponder =(this->monitorHeartbeatAcks(writer) == -1);
  if  (isNonResponder) return clientID;

  MmsAppMessage*  xml = new MmsAppMessage;
  xml->putMessageID(MMS_HEARTBEAT_NAME);    
  xml->putServerID(writer->serverID);        
  xml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::HEARTBEAT_ID], 
       writer->heartbeats++); 
                                            // Payload requested?
  if  (writer->isHeartbeatPayloadMediaResources)   
       this->formatQueryMediaResources(xml);
   
  xml->terminateReturnMessage(clientID); 
                                              
  writer->putMqMessage
      (xml->getNarrowMessage(), xml->narrowMsglength(), MMS_HEARTBEAT_NAME);

  delete xml; 
  return NULL;                              // Normal return
}



int MmsMSMQAppAdapter::monitorHeartbeatAcks(MmsMqWriter* writer)
{           
  // Check if we're up to date on heartbeat acknowledgements. If this client 
  // has not responded within its specified interval, so indicate.

  if  (!config->clientParams.heartbeatAckExpected) return 0; 
  int   result = 0; 
  const int heartbeatsMissable = max(config->clientParams.heartbeatAcksMissable,1); 
  const int heartbeatsMissed   = writer->heartbeats - writer->heartbeatAck;
  
  if  (heartbeatsMissed >= heartbeatsMissable)
  {
       MMSLOG((LM_ERROR,"%s no response from %x in %d pulses\n",
               taskName, writer->handle(), heartbeatsMissed));

       result = -1;
  }

  return result;
}



MmsMqWriter* MmsMSMQAppAdapter::getClientQueue(const int n)  
{
  // Return the n'th client queue  

  if  (clientQueues.size() == 0) return NULL;
  std::map<QUEUEHANDLE, MmsMqWriter*>::iterator i; 
  int  count = 0;  
       
  for(i = clientQueues.begin(); i != clientQueues.end(); i++)
      if (count++ == n) break; 

  MmsMqWriter* writer = (i == clientQueues.end())? NULL: i->second;

  return writer;
}
  

 
MmsMqWriter* MmsMSMQAppAdapter::getClientQueue(const QUEUEHANDLE hq)  
{
  // Look up client queue from client ID (queue handle) 

  MmsMqWriter* writer = NULL;
  if  (clientQueues.size() == 0) return writer;
  std::map<QUEUEHANDLE, MmsMqWriter*>::iterator i;  
        
  if  (clientQueues.end() != (i = clientQueues.find(hq)))
       writer = i->second;

  return writer;
}



MmsMqWriter* MmsMSMQAppAdapter::getClientQueueEx(const QUEUEHANDLE hq)  
{
  // Look up client queue from client ID (queue handle), and log an 
  // error message if not found

  MmsMqWriter* writer = NULL;

  if  (hq)
       writer = getClientQueue(hq);
  else
  if  (!this->isExpectingClientToken)
       writer = getClientQueue((int)0);

  if  (writer == NULL)
       MMSLOG((LM_ERROR,"%s queue %x is not registered\n",taskName,hq)); 
       
  return writer; 
}  



MmsMqWriter* MmsMSMQAppAdapter::getClientQueue(MmsAppMessage* xml)  
{
  // If we are in a multi-client situation, look up client queue using
  // client ID token from client's xml message; otherwise use the first
  // and only queue

  MmsMqWriter* writer = NULL;

  if  (this->isExpectingClientToken)
  {    QUEUEHANDLE clientID = xml->getClientID();
       writer = getClientQueue(clientID);
  }
  else writer = getClientQueue((int)0);
 
  return writer;
}



MmsMqWriter* MmsMSMQAppAdapter::getClientQueue(const char* queueName)  
{
  // Return the client queue matching supplied queue name 626 

  if  (clientQueues.size() == 0 || !queueName) return NULL;
  std::map<QUEUEHANDLE, MmsMqWriter*>::iterator i; 
  MmsMqWriter* writer = NULL;
       
  for(i = clientQueues.begin(); i != clientQueues.end(); i++)
  {
    if ((NULL != (writer = i->second)) && (writer->isMatchQueueName(queueName)))
         break;
    else writer = NULL;
  }    

  return writer;
}


                                             
int MmsMSMQAppAdapter::getClientQueueName(MmsAppMessage* xmlmsg, MMSMQNAME& mqname) 
{
  // Look in XML for queue/machine name parameters
  const char* paramnameMN = MmsAppMessageX::paramnames[MmsAppMessageX::MACHINE_NAME]; 
  const char* paramnameQN = MmsAppMessageX::paramnames[MmsAppMessageX::QUEUE_NAME]; 
  char* mname = xmlmsg->findparam(paramnameMN);
  char* qname = xmlmsg->findparam(paramnameQN);

  if  (mname && qname)                      
  {    int  length;                        // Found name in XML
       length = xmlmsg->isolateParameterValue(mname);
       ACE_OS::strncpy(mqname.machinename, 
                       xmlmsg->paramValue(), sizeof(mqname.machinename));
       length = xmlmsg->isolateParameterValue(qname);
       ACE_OS::strncpy(mqname.queuename,   
                       xmlmsg->paramValue(), sizeof(mqname.queuename));
       return 0;
  }

  if  (this->isConnected()) return -1;      // Can only default first client
                                            // If default not permitted, not OK
  if  (!config->clientParams.permitDefaultAppQueue) return -1;
                                            // Get name from config
  mname = config->clientParams.msmqAppMachineName;
  qname = config->clientParams.msmqAppQueueName;
  if  (mname && qname); else return -1;     
                                            // Found name in config
  ACE_OS::strncpy(mqname.machinename, mname, sizeof(mqname.machinename));
  ACE_OS::strncpy(mqname.queuename,   qname, sizeof(mqname.queuename));
  return 0;
}



int MmsMSMQAppAdapter::getServerIdFromClientId(const int clientID)
{
  int serverID = -1;
  MmsMqWriter* writer  = getClientQueue((QUEUEHANDLE)clientID);
  if (writer) serverID = writer->serverID;
  return serverID;
}



QUEUEHANDLE MmsMSMQAppAdapter::getClientID(MmsAppMessage* xml)  
{
  MmsMqWriter* writer = this->getClientQueue(xml);
  return writer? writer->handle(): NULL;
}



int MmsMSMQAppAdapter::registerClient(int isRegister, MmsAppMessage* xmlmsg)
{
  // Connect a client to, or disconnect a client from, the media server.
  // If connecting, open client's queue, and check if a heartbeat interval
  // is specified and/or a heartbeat payload is requested. 
  // Returns an MMS_ERROR_XXX code, or zero.  

  if  (isRegister)
  {    
       // Register a new queue writer and open its queue. Check connect
       // parameters and set writer properties accordingly. 

       // If the initial client is not exchanging the client ID token, 
       // no more clients may register. In a situation where multiple
       // clients wish to register simultaneously, they may include a zero
       // client ID token with the connect request in order to establish 
       // a multi-client token passing server state.  

       if  (xmlmsg->findparam(MmsAppMessageX::paramnames[MmsAppMessageX::CLIENT_ID]))
            this->isExpectingClientToken = TRUE;

       if  (this->isConnected() && !this->isExpectingClientToken)
            return MMS_ERROR_RESOURCE_UNAVAILABLE;

       MMSMQNAME mqname;
       int  result = 0;

       if  (this->getClientQueueName(xmlmsg, mqname) == -1)
            return MMS_ERROR_TOO_FEW_PARAMETERS; 

       const int incomingServerID = xmlmsg->getServerID();
                                            
       MmsMqWriter* writer = this->clientOpen
           (mqname.machinename, mqname.queuename, incomingServerID, &result);                                             

       if  (NULL == writer)                 
            return result? result: MMS_ERROR_PARAMETER_VALUE;
       else                                 // 0103 
       if  (result == MMS_ERROR_ALREADY_CONNECTED) 
       {    // When client sends a connect to a queue client is already using,
            // we previously returned an error. Instead we now return success,
            // but only if the incoming connect does not purport to change
            // any immutable connection parameter, such as server ID.
            // We should set a reason code as well, indicating that the 
            // connection already exists; however to do so would require
            // that we alter the incoming XML before turning it around,
            // so we'll put that on the to do list. 0103  
 
            if  (writer->serverID == incomingServerID)             
                 MMSLOG((LM_INFO,"%s server connect honored on existing queue\n", taskName));
            else MMSLOG((LM_ERROR,"%s server connect failed on existing queue\n", taskName));

            return (writer->serverID == incomingServerID)? 0: result;                  
       }       
                  
       writer->serverID = incomingServerID; // Set ID of server in cluster
 
       MMSLOG((LM_INFO,"%s %x connected to server %d\n",
               taskName, writer->handle(), writer->serverID));
                                            // Pass back writer handle
       xmlmsg->destination(writer->handle());
                                            // Heartbeat interval?
       writer->heartbeatInterval = this->getHeartbeatIntervalParam(xmlmsg); 
                                            // Heartbeat payload?
       writer->isHeartbeatPayloadMediaResources
             = this->getHeartbeatPayloadParam(xmlmsg);
  } 
  else // Close client queue; unregister and destroy queue writer. Close out                                    
  {    // any sessions and/or conferences left open by disconnecting party

       QUEUEHANDLE handle = this->getClientID(xmlmsg);   
                           
       if  (this->unregisterClient(handle) == -1)
            return MMS_ERROR_TOO_FEW_PARAMETERS;    
  }

  return 0;
} 



int MmsMSMQAppAdapter::unregisterClient(const QUEUEHANDLE handle)
{
  const int closedcount = NULL == handle? 0: this->clientClose(handle);
 
  if  (closedcount)
  {                                          
       this->postServerMessage(MMSM_TEARDOWN,              
             (long) new TEARDOWNPARAMS(this, handle, TRUE));

       MMSLOG((LM_INFO,"%s %x disconnected from server\n", taskName, handle));
  }
  else MMSLOG((LM_INFO,"%s server disconnect failed\n", taskName));

  return closedcount? 0: -1;
}



MmsMqWriter* MmsMSMQAppAdapter::clientOpen
( const char* mname, const char* qname, const int serverID, int* resultCode) 
{
  // Open a client queue object and map queue handle to object; return object

  MmsMqWriter* writer = this->getClientQueue(qname);
  if (NULL != writer)
  {   // If client is already connected on this queue, consider open to be   
      // successful, but only if caller is inspecting the result code. 0103
      MMSLOG((LM_INFO,"%s queue name already registered\n", taskName));
      if (resultCode) *resultCode = MMS_ERROR_ALREADY_CONNECTED;  
      return resultCode? writer: NULL;        
  }

  writer = new MmsMqWriter();               // Instantiate queue writer
                                            // Open writer's queue
  int result = writer->openQueue(mname, qname); 
  if (result == -1) 
  {   delete writer;
      return NULL;
  }

  writer->setname();

  QUEUEHANDLE  hq  = writer->handle();
  clientQueues[hq] = writer;                // Register queue writer  

  m_isClientConnected = TRUE;                 
  return writer;
}



int MmsMSMQAppAdapter::clientClose(const QUEUEHANDLE hq)  
{
  // Close/unmap one or all client queue objects; return count of clients closed

  if  (clientQueues.size() == 0) return 0;
  MmsMqWriter* writer = NULL;
  std::map<QUEUEHANDLE, MmsMqWriter*>::iterator i; 
  int  closecount = 0;         

  if  (hq)                                  // Close single client  
  {                                         // Look up queue writer
    if  (clientQueues.end() != (i = clientQueues.find(hq)))
    {    if (NULL != (writer = i->second))
         {   writer->closeQueue();          // Close writer's queue
             delete writer;                 // Destroy queue writer
             closecount = 1;
         }
         clientQueues.erase(i);             // Unregister queue writer
    }          
  }
  else                                      // Close and destroy all 
  { for(i = clientQueues.begin(); i != clientQueues.end(); i++)
    {   if (NULL != (writer = i->second))  
        {   writer->closeQueue();
            delete writer;
            closecount++;
        }
    }
    clientQueues.erase(clientQueues.begin(),clientQueues.end());
  }

  if  (clientQueues.size() == 0) m_isClientConnected = FALSE;              
  return closecount;
}



int MmsMSMQAppAdapter::openServerQueue()  
{
  // Instantiate and open the receive queue on which we expect to receive
  // messaging from all clients of this adapter

  char* qNameListen = config->clientParams.msmqMmsQueueName;
  char* mNameListen = config->clientParams.msmqMmsMachineName;

  MmsTask::InitialParams params(0, GROUPID_MQLISTENER, this);                                           
  params.config = this->config;  
  params.logCallback = this->logger; 
  this->mqListener = new MmsMqListener(&params); 

  int  result = this->mqListener->openQueue(mNameListen, qNameListen); 
  if  (result == -1) return -1;         
  
  int  purged = this->mqListener->purge();  // Clear out queue
  if  (purged)  
       MMSLOG((LM_INFO,"%s purged %d from %x\n",
               taskName, purged, mqListener->handle())); 
  return 0;
}



int MmsMSMQAppAdapter::closeServerQueue()  
{
  // Close and destroy the one and only receive queue listener and object

  if  (this->mqListener == NULL) return 0;
  this->shutdownListener();
  this->mqListener->closeQueue();
  delete this->mqListener;
  this->mqListener = NULL;
  return 1;
}



void MmsMSMQAppAdapter::handleQueryMediaResources(QUEUEHANDLE clientID)
{
  // Handles a server query for "mediaResources". Appends the query results
  // to inbound xml message and sends it back.

  MmsMqWriter* writer = NULL;
  this->formatQueryMediaResources(xmlmsg);

  xmlmsg->terminateReturnMessage(0);                                            

  if  (NULL != (writer = this->getClientQueue(clientID)))  
       writer->putMqMessage(xmlmsg->getNarrowMessage(), 
                            xmlmsg->narrowMsglength(), xmlmsg->commandName()); 

  delete xmlmsg; xmlmsg = NULL;  
}



void MmsMSMQAppAdapter::shutdownListener()    // Shut down MQ listener thread
{
  if  (this->mqListener && this->mqListener->isListening());
  else return;
                                           
  MMSLOG((LM_DEBUG,"%s closing mq listener\n",taskName)); 

  this->mqListener->shutdown();  

  ACE_Thread_Manager::instance()->wait_task(this->mqListener);         
}



int MmsMSMQAppAdapter::onShutdown()           // Adapter told to shut down
{
  if  (!m_isStarted) return 0;
  m_isStarted = m_isActive = FALSE;

  this->shutdownListener();

  this->clientClose();

  this->closeServerQueue();

  return 0;                                 
}



void MmsMSMQAppAdapter::onStartAdapter()      // Protocol adapter start hook
{                                           // Instantiate MQ writer
  if  (m_isStarted) return;
  else m_isStarted = TRUE;

  int  result = this->openServerQueue();    // Open receive queue  
  if  (result != -1)                        // If open ...
       this->mqListener->start();           // ... start listening for clients
   
  else 
  {
      // JDL, 03/16/06, keep the ball rolling and wait for IPC connection.
      //this->shutdownSelf();                // Otherwise shut down adapter 
      m_isStarted = m_isActive = FALSE;
      MMSLOG((LM_ERROR,"APMQ MSMQ adapter is not available or out of service\n"));
  }
} 



void MmsMSMQAppAdapter::onStopAdapter()
{
  // Ordinarily onStopAdapter would be interpreted as a method to stop
  // the adapter function with the provision that it could be restarted
  // if desired; however, since we have no need for this feature right 
  // now, we're using the notification to shut down the adapter listener
  // thread. It should be OK to remove this line of code and wait for
  // the MMSM_SHUTDOWN, which will stop the listener if it is not stopped.
    
  this->shutdownListener();
}



int MmsMSMQAppAdapter::isConnected()    
{ 
  return m_isActive && clientQueues.size() > 0; 
}



int MmsMSMQAppAdapter::isConnected(void* clientID)    
{ 
  return clientQueues.find(clientID) != clientQueues.end();
}



int MmsMSMQAppAdapter::buildCommonParameters(MmsFlatMapWriter& map, 
  MmsAppMessage* xmlmsg, MmsServerCmdHeader& commandHeader, unsigned int* flags)
{
  // Insert common parameters to map, these being:
  // transactionID, connectionID, conferenceID, clientID, serverID, timeout

  int  result = 0, value = 0;
                                            // If we're handling multiple 
  if  (this->isExpectingClientToken)        // clients, we expect client ID
  {    
       if  (NULL != (commandHeader.clienthandle = xmlmsg->getClientID()))
            if  (this->isConnected(commandHeader.clienthandle)); 
            else return MMS_ERROR_NOT_CONNECTED;
       else return MMS_ERROR_TOO_FEW_PARAMETERS;
  }
  else commandHeader.clienthandle = getClientID(xmlmsg);

  // On initial connect, the server ID (if any) arrived as a command parameter
  // and was placed into the xml message object property "serverID" in method
  // mmsMqAppAdapter.writeStandardClientMessageContent. On subsequent commands
  // server ID arrives as part of the connection ID, or of the conference ID.
  // This is the sole point of convergence at which we strip off the server ID
  // and insert it to the parameter map command header. The media server core
  // thus always receives IDs with the serverID stripped out, and in fact never
  // deals with serverID at all. When the command completes and its parameters
  // are written back to client, both in writeStandardClientMessageContent, and
  // in onReturnConnect, we will reinsert server ID into connection ID and/or 
  // conference ID.  

  commandHeader.serverID = xmlmsg->serverID(); 

                                          // Connection ID
  if  (xmlmsg->getvaluefor (MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID])) 
       value = ACE_OS::atoi(xmlmsg->paramValue());

  commandHeader.connectionID = this->stripServerID(value, commandHeader);
    
  char* confID = xmlmsg->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID]);
  if  (confID)                               
  {                                         // If conference ID supplied ... 
       int  conferenceID = ACE_OS::atoi(xmlmsg->paramValue()); 
       conferenceID = this->stripServerID(conferenceID, commandHeader);
                                            // Note that empty is implicit "0"
       if  (conferenceID < 0)               // More robust validation todo     
            result = MMS_ERROR_PARAMETER_VALUE; 
       else                                 
       {    map.insert(MMSP_CONFERENCE_ID, conferenceID); 
            if  (flags) *flags |= CONFERENCEID_PRESENT;
       }
  } 

  value = 0;                                // Command timeout
  if  (xmlmsg->getvaluefor (MmsAppMessageX::paramnames[MmsAppMessageX::COMMAND_TIMEOUT])) 
       value = ACE_OS::atoi(xmlmsg->paramValue());
  if  (value)
       map.insert(MMSP_COMMAND_TIMEOUT_MS, value); 

  value = 0;                                // Transaction ID
  if  (xmlmsg->getvaluefor (MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID])) 
       value = ACE_OS::atoi(xmlmsg->paramValue());
  commandHeader.transactionID = value;

  return result;
}



void MmsMSMQAppAdapter::onCommandServer(void* IpcData)
{  
  // Server queries are handled here in the adapter via a synchronous call   
  // to the server manager. Query results are appended to the inbound message
  // which is then turned around
  
  this->xmlmsg = (MmsAppMessage*)IpcData;
  int   result = 0;
  QUEUEHANDLE clientID = this->getClientID(xmlmsg);
                                            // Is command "query"?
  if  (!xmlmsg->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::QUERY])) 
       result = MMS_ERROR_TOO_FEW_PARAMETERS;
  else                                      // Is query "mediaResources"?
  if  (0 != ACE_OS::strcmp(xmlmsg->paramValue(), 
            MmsAppMessageX::serverQueryNames[MmsAppMessageX::MEDIA_RESOURCES])) 
       result = MMS_ERROR_TOO_FEW_PARAMETERS;
  else
  if   (xmlmsg->backupToEndTag() == -1)     // Overwrite </message>
        result = MMS_ERROR_MALFORMED_REQUEST;
  else
  if  (this->isExpectingClientToken && clientID == NULL)
       result = MMS_ERROR_TOO_FEW_PARAMETERS;

  if  (result)
       this->turnMqMessageAround(result, 1);    
  else this->handleQueryMediaResources(clientID);
}



void MmsMSMQAppAdapter::onCommandHeartbeatAck(void* IpcData) 
{
  // Act on a heartbeat response from client

  this->xmlmsg = (MmsAppMessage*)IpcData; 

  MmsMqWriter* writer = NULL;

  QUEUEHANDLE clientID = this->getClientID(xmlmsg);

  if  (NULL != (writer = this->getClientQueue(clientID)))  
       
       #if 0
       if  (xmlmsg->getvaluefor(xmlmsg->paramnames[xmlmsg->HEARTBEAT_ID]))                                              
            writer->heartbeatAck = ACE_OS::atoi(xmlmsg->paramValue());
       #else                                 
       // We're not concerned with which pulse is being acknowledged;
       // only that client is responding, so we'll bring the ack count
       // completely up to date. We should probably do the same whenever
       // we receive any message from client.
                                  
       writer->acksUpToDate();

       #endif

  delete this->xmlmsg; this->xmlmsg = NULL;
}



void MmsMSMQAppAdapter::turnServerConnectMessageAround
(const int resultcode, const int reason, const int isdelete)
{
  // Send initial connect message back with a resultcode and client ID, 
  // assuming the queue specified on the connect was successfully opened, 
  // otherwise we won't have a queue to return the message. Client ID zero
  // may have been specified in the xml, so we'll remove that tag from the
  // xml prior to appending the new client ID. We'll then insert server ID.

  if  (resultcode == MMS_ERROR_PARAMETER_VALUE)
       MMSLOG((LM_ERROR,"%s no queue cannot respond\n",taskName));
  else
  if  (xmlmsg != NULL)
  {    
       xmlmsg->remove(MmsAppMessageX::CLIENT_ID);
                                            
       xmlmsg->backupToEndTag();            // Pass back server resources  
       this->formatQueryMediaResources(xmlmsg);
       xmlmsg->put(MMS_MSG_END_TAG);        // Re-terminate message  

       this->turnMqMessageAround(resultcode, reason, isdelete);        
  }
}



int MmsMSMQAppAdapter::turnMqMessageAround
( const int resultcode, const int reason, const int isdeletexml)
{
  // Send message back to client as is, appending a result code to the XML
  // We append client ID as well, if (a) if we're turning around a server 
  // connect (in which case xml->destination now contains the queue handle 
  //(registerClient); or (b) if client ID was included in the xml by client

  if  (xmlmsg == NULL) return -1;
  int  result = -1;
  do {

  QUEUEHANDLE clientID                       
    = xmlmsg->destination()?        xmlmsg->destination():                                             
      this->isExpectingClientToken? xmlmsg->getClientID(): NULL;

  if  (xmlmsg->makeTurnaroundMessage(resultcode, reason, clientID) == -1) break;  
                                            // Get client queue
  MmsMqWriter* writer = clientID? this->getClientQueueEx(clientID):
                                  this->getClientQueue((int)0);
  if  (NULL == writer)  break;
                                            // Post to client queue
  result = writer->putMqMessage(xmlmsg->getNarrowMessage(), 
                   xmlmsg->narrowMsglength(), xmlmsg->commandName(), resultcode);
  } while(0);


  if  (isdeletexml)
  {    delete xmlmsg;
       xmlmsg = NULL;
  }

  return result;
}



int MmsMSMQAppAdapter::postClientReturnMessage(MmsAppMessage** xml, char* flatmap)
{
  // Terminate the xml return message, post the message to client, and free
  // memory for both xml message wrapper and server parameter map. Note that
  // we must not free map memory if this message is a provisional response
  // from server that a command has begun execution, since that map is still
  // resident in the server session.

  if  (!Mms::isFlatmapReferenced(flatmap, 28)) return -1; 

  MmsAppMessage* outxml = *xml; 

  const int isProvisionalResponse = isFlatmapXflagSet
           (flatmap, MmsServerCmdHeader::IS_PROVISIONAL_RESULT);

  int retcode = isProvisionalResponse? MMS_COMMAND_EXECUTING: 
                getFlatmapRetcode(flatmap);
                                            // Get this client's queue
  QUEUEHANDLE clientID = getFlatmapClientHandle(flatmap);      
                                            // Return result code
  outxml->terminateReturnMessage(clientID, retcode);  
                                             
  MmsMqWriter* writer = this->getClientQueueEx(clientID);
  if  (NULL != writer)                      // Send MQ message to client                                            
       writer->putMqMessage(outxml->getNarrowMessage(), outxml->narrowMsglength(),
                            outxml->commandName(), retcode);                                             
  *xml = NULL;                              // Void caller's copy of pointer
  delete outxml;                            // Free xml message memory
                                            // Permit term event to proceed
  clearFlatmapXflag(flatmap,MmsServerCmdHeader::IS_DEPENDENCY_PENDING);
                                            
  if  (retcode == MMS_COMMAND_EXECUTING)    // If parameter map still in use ...
  {                                         // ... clear the provisional flag
       clearFlatmapXflag(flatmap,MmsServerCmdHeader::IS_PROVISIONAL_RESULT);
  }       
  else this->onCommandComplete(flatmap);    // otherwise free map memory 

  return (writer == NULL)? -1: 0;
}

void MmsMSMQAppAdapter::pushServerData(MmsMsg* msg)
{ 
  char* map = (char*)msg->param();

  if  (clientQueues.size() == 0) 
    return;

  QUEUEHANDLE clientId = getFlatmapClientHandle(map);
  std::map<QUEUEHANDLE, MmsMqWriter*>::iterator index;         
  index = clientQueues.find(clientId);
  if (index == clientQueues.end())
    return;

  MmsMqWriter* c = index->second;    
  if (c == NULL)
    return;

  MmsFlatMapReader reader(map);
  char* rguid = 0;
  int rguidlength = reader.find(MMSP_ROUTING_GUID, &rguid);
  if (!rguidlength || !(*rguid)) return;       // no routing guid                                                                   

  char digits[2] = { 0, 0 };
  digits[0] = (char)getFlatmapParam(map);      // RFC2833 signal stored in param
                                               // Connection ID:
  int connectionID = getFlatmapConnectionID(map);
                                               // Overlay server ID
  connectionID = this->insertServerID(connectionID, c->serverID);


  MmsAppMessage*  xml = new MmsAppMessage;
  xml->putMessageID(MMS_RFC2833_SIGNAL_NAME);    
  xml->putParameter(MmsAppMessageX::CONNECTION_ID, connectionID);
  xml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::DIGITS], digits);   
  xml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::ROUTING_GUID], rguid);
  xml->terminateReturnMessage(clientId);                                               
                                              
  c->putMqMessage(xml->getNarrowMessage(), xml->narrowMsglength(), MMS_RFC2833_SIGNAL_NAME);

  delete xml; 
}


















