//
// MmsFlatmapIpcAppAdapter.cpp 
//
#include "StdAfx.h"
#pragma warning(disable:4786)
#include "MmsFlatmapIpcAppAdapter.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

#define defmsgname  "Metreos Ipc Message";        // default message name


// Default constructor
MmsFlatmapIpcAppAdapter::MmsFlatmapIpcAppAdapter(const int port) 
: MmsMqAppAdapter(NULL), FlatMapIpcServer(port)
{
  this->init();
}


// Constructor with initialization parameters
MmsFlatmapIpcAppAdapter::MmsFlatmapIpcAppAdapter(const int port, MmsTask::InitialParams* params)
: MmsMqAppAdapter(params), FlatMapIpcServer(port)
{
  this->init();
}


// Destructor
MmsFlatmapIpcAppAdapter::~MmsFlatmapIpcAppAdapter()
{
  this->onShutdown();
}



void MmsFlatmapIpcAppAdapter::init()
{
  this->logMessageCount = this->logMessageSequence = 0;
}




// For each client, if the client's designated heartbeat interval is reached, 
// post client a heartbeat message.
void MmsFlatmapIpcAppAdapter::postClientsHeartbeat()
{
  ++heartbeats;
  if (clientMap.size() == 0) 
      return;

  map<int, MmsFlatmapIpcClient*>::iterator i;
  int id = -1;

  for(i=clientMap.begin(); i!= clientMap.end(); i++)
  {   
    MmsFlatmapIpcClient* c = NULL;    
    if (NULL == (c = i->second)) 
        continue;

    const int intervalseconds = c->getHeartBeatInterval() ? 
               c->getHeartBeatInterval() : this->config->clientParams.heartbeatIntervalSecs;

    if ((heartbeats % intervalseconds) == 0)  
    {                                     
      if (-1 == postClientHeartbeat(c->getSessionId()))
      {
           // This particular unregistration client is non-responsive
          id = c->getSessionId();
      }
    }
  }

  // Disconnect comatose client, one client at most will be disconnected per pulse
  if (id != -1)                
  {    
      MMSLOG((LM_ERROR, "IPCF IPC client %d appears down: disconnecting\n", id));
      unregisterClient(id);
  }
}



// If client is responsive, send client a heartbeat message and return 0; 
// otherwise return -1
int MmsFlatmapIpcAppAdapter::postClientHeartbeat(int sessionId)
{
  int nResult = -1;

  map<int, MmsFlatmapIpcClient*>::iterator index;
  index = clientMap.find(sessionId);
  if (index == clientMap.end())
    return nResult;

  MmsFlatmapIpcClient* c = index->second;    
  if (c == NULL)
    return nResult;

  int isNonResponder = (this->monitorHeartbeatAcks(c) == -1);
  if (isNonResponder) 
    return nResult;

  MmsAppMessage*  xml = new MmsAppMessage;
  xml->putMessageID(MMS_HEARTBEAT_NAME);    
  xml->putServerID(c->getServerId());        
  xml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::HEARTBEAT_ID], c->addHeartBeat()); 

  // Payload requested?
  if  (c->isHeartbeatPayloadMediaResources())   
       this->formatQueryMediaResources(xml);
   
  xml->terminateReturnMessage(sessionId);                                               
  nResult = (putFlatmapIpcMessage(sessionId, xml->getNarrowMessage(), 
             xml->narrowMsglength(), MMS_HEARTBEAT_NAME) == 0) ? 0 : -1;
  delete xml;
  return nResult;
}



// Check if we're up to date on heartbeat acknowledgements. If this client 
// has not responded within its specified interval, so indicate.
int MmsFlatmapIpcAppAdapter::monitorHeartbeatAcks(MmsFlatmapIpcClient* c)
{
  int result = 0; 

  if (!config->clientParams.heartbeatAckExpected) 
      return result; 

  const int heartbeatsMissable = max(config->clientParams.heartbeatAcksMissable, 1); 
  const int heartbeatsMissed   = c->getHeartBeats() - c->getHeartBeatAck();
  
  if (heartbeatsMissed >= heartbeatsMissable)
  {
      MMSLOG((LM_ERROR,"IPCF no response from IPC client %d in %d pulses\n",
              c->getSessionId(), heartbeatsMissed));

      result = -1;
  }

  return result;
}



int MmsFlatmapIpcAppAdapter::putFlatmapIpcMessage
( int sessionId, char* body, const int length, const char* name)
{   
  // Send IPC message to client
  int nResult = -1;

  if (!m_isStarted || !m_isActive) return -1;
  const char* msgname = name ? name: defmsgname;
      
  // JLD added this check, which was not present previously, then removed it.
  // However, it still seems as if this test should be here. 
  // if (!this->isConnected(sessionId))
  // {
  //     MMSLOG((LM_DEBUG, "IPCF '%s' for unknown IPC client %d discarded\n", 
  //             msgname, sessionId));
  //     return -1;
  // }

  FlatMapWriter map;
  char* pFlatmapData = new char[length+1];
  // ACE_OS::memset(pFlatmapData, 0, length);  // superflous
  ACE_OS::memcpy(pFlatmapData, body, length);
  pFlatmapData[length] = 0;

  map.insert(FLATMAPIPC_MMS_BODY, FlatMap::STRING, length+1, pFlatmapData);

  if (this->config->diagnostics.logOutboundMessages > 0)
      this->logOutboundMessage(body, length, name, sessionId);

  nResult = Write(FLATMAPIPC_TYPE_MMS, map, sessionId) ? 0 : -1;  // socket write

  if (nResult < 0)       
      MMSLOG((LM_ERROR, "IPCF %s socket write failed for IPC client %d\n", 
              msgname, sessionId));   
  else
  {    
      ACE_Log_Priority pri 
        = name && ACE_OS::strcmp(name, MMS_HEARTBEAT_NAME) == 0 ? LM_TRACE: LM_DEBUG;   
      MMSLOG((pri, "IPCF %s posted to IPC client %d\n", msgname, sessionId));
  }

  delete[] pFlatmapData;

  return nResult;
}



void MmsFlatmapIpcAppAdapter::logOutboundMessage
( char* body, const int length, const char* name, const int clientID)
{
  // When configured, log outbound XML to nnnnnnn.log. The total number of files
  // is dependent on config->diagnostics.logOutboundMessages. After that many
  // files are written, the file names start over from 1 and replace previous.

  if (body == NULL) return;
  const int bodylen = strlen(body);

  #ifdef MMS_WINPLATFORM

  const char* logdir 
    = "C:\\Program Files\\Cisco Systems\\Unified Application Environment\\MediaServer\\diag"; 

  static int isDirOK, dircount;
  if (dircount > 1) return;

  if (!isDirOK)
  {   
      dircount++;
      struct stat statinfo; memset(&statinfo, 0, sizeof(struct stat));
      stat(logdir, &statinfo);
      if ((statinfo.st_mode & S_IFMT) != S_IFDIR) // if dir nonexistent ...
           isDirOK = (0 == _mkdir(logdir));       // ... create directory 
      else isDirOK = TRUE;
  }


  if (!isDirOK) return;

  const int period = this->config->diagnostics.logOutboundMessages;
  if ((this->logMessageCount % 1024) == 0)
       MMSLOG((LM_INFO,"IPCF WARNING message logging enabled - count %d\n", logMessageCount));
  this->logMessageCount++;
  this->logMessageSequence++;
  if (this->logMessageSequence > period)   
      this->logMessageSequence = 1;

  const char* fnmask = "\\%07d.log";
  char filename[16];
  sprintf(filename, fnmask, this->logMessageSequence);
  const char* pname = (name == NULL || strlen(name) > 20)? "?": name;
  //                         
  //                        0            1                  2    3        4               5
  //                        0            0                  2    0        0               6
  const char* headermask = "MSG %02d%02d %02d%02d%02d.%03d %07d len=%05d client=%04d msg=%s: ";
  char header[80];

  SYSTEMTIME st;
  GetSystemTime(&st);
    
  sprintf(header, headermask, 
    st.wMonth, st.wDay,
    st.wHour,  st.wMinute, st.wSecond, 
    st.wMilliseconds, this->logMessageCount,
    length, clientID, pname);

  const int hdrlen = strlen(header);

  char path[MAXPATHLEN]; strcpy(path, logdir);
  strcat(path, filename);

  FILE* f = NULL;
 
  if (f = fopen(path,"wb"))
  {   
      fwrite(header, 1, hdrlen,  f);
      fwrite(body,   1, bodylen, f);
      fclose(f);
  }

  #endif MMS_WINPLATFORM
}



// Event handler when IPC client connected.
void MmsFlatmapIpcAppAdapter::OnClientConnected(int sessionId)
{
  MMSLOG((LM_INFO,"IPCF IPC client %d connected\n", sessionId));
}



// Event handler when IPC client disconnected.
void MmsFlatmapIpcAppAdapter::OnClientDisconnected(int sessionId)
{
  MMSLOG((LM_INFO,"IPCF IPC client %d disconnected\n", sessionId));
}



// Event handler when low level socket failure occurs.
void MmsFlatmapIpcAppAdapter::OnSocketFailure(int errorNumber, int sessionId)
{
  MMSLOG((LM_ERROR,"IPCF IPC client %d error %d\n", sessionId, errorNumber));
}



// Event handler for incoming flatmap messages
void MmsFlatmapIpcAppAdapter::OnIncomingFlatMapMessage
( const int messageType, const FlatMapReader& flatmap, const char* data, size_t length, int sessionId)
{
  if (!m_isStarted || !m_isActive) return;
  if (FLATMAPIPC_TYPE_MMS != messageType) return;

  FlatMapReader r;	
  r = flatmap;
	
  // Locate XML message body.
	char* pData = NULL;
  int type = FlatMap::datatype::STRING;

	int len = r.find(FLATMAPIPC_MMS_BODY, &pData, &type, 0); 
  if (len == 0) return;
  
  char* pFlatmapData = new char[len+1];
  ACE_OS::memset(pFlatmapData, 0, len);
  ACE_OS::memcpy(pFlatmapData, pData, len);
  pFlatmapData[len] = 0;
  MmsAppMessage* xmlmsg = new MmsAppMessage(pFlatmapData);

  // Ensure that the client ID provided by client matches the IPC session ID.
  if (xmlmsg->findparam(MmsAppMessageX::paramnames[MmsAppMessageX::CLIENT_ID]))
      xmlmsg->remove(MmsAppMessageX::CLIENT_ID);

  xmlmsg->backupToEndTag();    
  xmlmsg->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CLIENT_ID], sessionId);
  xmlmsg->put(MMS_MSG_END_TAG);

  // Post message to task event handler
  this->postMessage(MMSM_DATA, (long)xmlmsg);

  delete[] pFlatmapData;  
}



int MmsFlatmapIpcAppAdapter::turnMqMessageAround
( const int resultcode, const int reason, const int isdeletexml)
{
  if (xmlmsg == NULL) return -1;

  int result = -1;
  void* sessionId = NULL;
  char* commandName = xmlmsg->commandName();

	if  (xmlmsg->destination() >= 0)
		   sessionId = xmlmsg->getClientID();
	else sessionId = isExpectingClientToken ? xmlmsg->getClientID(): (void*)(-1);

  const int isNonexistenClient    // 070529 check if client exists
      = sessionId == (void*)(-1) || !this->isConnected((int)sessionId); 

  if (MMS_ISCOMMANDERROR(resultcode))  // 070529 log transaction ID
      MMSLOG((LM_ERROR,"IPCF error %d on transx %d '%s' from client %d\n", 
              resultcode, this->getTransactionID(xmlmsg), commandName, sessionId));

  // JDL, 06/04/07, allow messages to send to client even "logical" connection does not exist.
  // IPCServer is passive in terms of connect/disconnect, so let client handles the disconnection if 
  // there is a problem in physical connection.

  // if (isNonexistenClient);  // 070529 don't return message if bad client ID
  // else // Post message to client queue

  if (xmlmsg->makeTurnaroundMessage(resultcode, reason, sessionId) != -1) 
  {
      result = putFlatmapIpcMessage((int)sessionId, xmlmsg->getNarrowMessage(), 
                  xmlmsg->narrowMsglength(), xmlmsg->commandName());
  }

  if (isdeletexml || isNonexistenClient) // JDL, 06/04/07, delete XML content if logical connection does not exist 
  {    
      if (xmlmsg) delete xmlmsg;
      xmlmsg = NULL;
  }

  return result;
}



// Turn client connect request message around with annotated data.
void MmsFlatmapIpcAppAdapter::turnServerConnectMessageAround
( const int resultcode, const int reason, const int isdelete)
{
  if (resultcode == MMS_ERROR_PARAMETER_VALUE)
  {
      MMSLOG((LM_ERROR,"IPCF no IPC session - cannot respond\n"));
  }
  else 
  if (xmlmsg != NULL)
  {    
      xmlmsg->remove(MmsAppMessageX::CLIENT_ID);
                                      
      xmlmsg->backupToEndTag();    
      this->formatQueryMediaResources(xmlmsg);
      xmlmsg->put(MMS_MSG_END_TAG);

      this->turnMqMessageAround(resultcode, reason, FALSE);        
  }

  if (isdelete && xmlmsg)
  {    
      delete xmlmsg;
      xmlmsg = NULL;
  }
}



// Get client id data from XML stream.
void* MmsFlatmapIpcAppAdapter::getClientID(MmsAppMessage* xml)  
{
  return xml->getClientID();
}



// Is any IPC client connected.
int MmsFlatmapIpcAppAdapter::isConnected()    
{ 
  return m_isActive && clientMap.size() > 0; 
}



// Is IPC client connected.
int MmsFlatmapIpcAppAdapter::isConnected(int sessionId)    
{ 
  return clientMap.find(sessionId) != clientMap.end();
}



// Register and start tracking an IPC client object.
int MmsFlatmapIpcAppAdapter::registerClient(int isRegister, MmsAppMessage* xmlmsg)
{
  MmsFlatmapIpcClient* client = NULL;

  if (isRegister)
  {    
      if (xmlmsg->findparam(MmsAppMessageX::paramnames[MmsAppMessageX::CLIENT_ID]))
          this->isExpectingClientToken = TRUE;

      if (this->isConnected() && !this->isExpectingClientToken)
          return MMS_ERROR_RESOURCE_UNAVAILABLE;

      const int sessionId = (int)this->getClientID(xmlmsg);

      if (clientMap.find(sessionId) == clientMap.end())
      {                                     // Client ID not found, insert new client
          client = new MmsFlatmapIpcClient(sessionId);
          clientMap.insert(Session_Client_Pair(sessionId, client));
      }
                                            // Look up client by client ID
      map<int, MmsFlatmapIpcClient*>::iterator index;
      index = clientMap.find(sessionId);

      if (index == clientMap.end())
          return MMS_ERROR_RESOURCE_UNAVAILABLE;

      if (NULL == (client = index->second))    
          return MMS_ERROR_RESOURCE_UNAVAILABLE;

      client->assignServerId(xmlmsg->getServerID());

      char* fromIP = this->getRemoteAddress(sessionId);

      MMSLOG((LM_INFO,"IPCF client %d (%s) connected to server %d\n", 
              sessionId, fromIP, client->getServerId()));       

      xmlmsg->destination((void*)sessionId);
      client->assignHeartBeatInterval(getHeartbeatIntervalParam(xmlmsg)); 
      client->assignHeartBeatPayloadMediaResources(getHeartbeatPayloadParam(xmlmsg));
      m_isClientConnected = TRUE;                 
  } 
  else
  {   
      const int sessionId = (int)this->getClientID(xmlmsg);                              
      if (this->unregisterClient(sessionId) == -1)
          return MMS_ERROR_TOO_FEW_PARAMETERS;    
  }

  return 0;
} 



int MmsFlatmapIpcAppAdapter::unregisterClient(const int sessionId)
{
  // Unregister a client.
  const int closedcount = -1 == sessionId? 0: this->clientClose(sessionId);
 
  if (closedcount)
  {                                          
      this->postServerMessage(MMSM_TEARDOWN,              
           (long) new TEARDOWNPARAMS(this, (void*)sessionId, TRUE));

      MMSLOG((LM_INFO,"IPCF IPC client %d disconnected from server\n", sessionId));
  }
  else 
  {
      MMSLOG((LM_INFO,"IPCF server disconnect failed for IPC client %d\n", sessionId));
  }

  return closedcount? 0: -1;
}



int MmsFlatmapIpcAppAdapter::clientClose(const int sessionId, const int isShutdown)  
{
  // Stop tracking an IPC client object 

  if (clientMap.size() == 0) return 0;

  MmsFlatmapIpcClient* client = NULL;
  map<int, MmsFlatmapIpcClient*>::iterator i;
  int closecount = 0;         

  if ((sessionId > 0) && !isShutdown)
  {     
      if (clientMap.end() != (i = clientMap.find(sessionId)))
      {    
          if (NULL != (client = i->second))
          {   
              delete client;
              closecount = 1;
          }

          clientMap.erase(sessionId);
      }          
  }
  else  // Clean out all clients
  { 
      for(i = clientMap.begin(); i != clientMap.end(); i++)
      {   
          if (NULL != (client = i->second))  
          {   
              delete client;
              closecount++;
          }
      }

      clientMap.erase(clientMap.begin(), clientMap.end());
  }

  if (clientMap.size() == 0) 
      m_isClientConnected = FALSE;              

  return closecount;
}



char* MmsFlatmapIpcAppAdapter::getRemoteAddress(const int ipcSessionID) 
{
  // Get IPC client's IP
  const static char* unknownIP = "unknown IP";
  char* fromIP = NULL;

  IpcSession* session = this->activeSessions[ipcSessionID];

  if (session)       
      fromIP = (char*)session->GetPeerAddr().get_host_addr();

  return fromIP? fromIP: (char*)unknownIP;
}



int MmsFlatmapIpcAppAdapter::getServerIdFromClientId(const int clientID)
{
  int serverID = -1;
  MmsFlatmapIpcClient* client = NULL;

  map<int, MmsFlatmapIpcClient*>::iterator i = clientMap.find(clientID);
  if (i != clientMap.end()) client = i->second;
  if (client != NULL) serverID = client->getServerId();
  return serverID;
}


// Event handler for Query Media Resources.
void MmsFlatmapIpcAppAdapter::handleQueryMediaResources(int sessionId)
{
  this->formatQueryMediaResources(xmlmsg);
  xmlmsg->terminateReturnMessage(0);                                            
  putFlatmapIpcMessage(sessionId, xmlmsg->getNarrowMessage(), 
            xmlmsg->narrowMsglength(), xmlmsg->commandName()); 
  delete xmlmsg; 
  xmlmsg = NULL;  
}



int MmsFlatmapIpcAppAdapter::postClientReturnMessage(MmsAppMessage** xml, char* flatmap)
{
  if (!Mms::isFlatmapReferenced(flatmap, 28)) return -1; 

  MmsAppMessage* outxml = *xml; 
  const int isProvisionalResponse = isFlatmapXflagSet(flatmap, MmsServerCmdHeader::IS_PROVISIONAL_RESULT);
  int retcode = isProvisionalResponse? MMS_COMMAND_EXECUTING: getFlatmapRetcode(flatmap);
  int sessionId = (int)getFlatmapClientHandle(flatmap);      
  outxml->terminateReturnMessage(getFlatmapClientHandle(flatmap), retcode);  
                                             
  putFlatmapIpcMessage(sessionId, outxml->getNarrowMessage(), 
            outxml->narrowMsglength(), outxml->commandName());                                             
  *xml = NULL;
  delete outxml;
  outxml = NULL;
                
  clearFlatmapXflag(flatmap, MmsServerCmdHeader::IS_DEPENDENCY_PENDING);
                                            
  if (retcode == MMS_COMMAND_EXECUTING)
  {                                     
      clearFlatmapXflag(flatmap,MmsServerCmdHeader::IS_PROVISIONAL_RESULT);
  }       
  else this->onCommandComplete(flatmap);

  return 0;
}



// Build XML message body
int MmsFlatmapIpcAppAdapter::buildCommonParameters(MmsFlatMapWriter& map, 
  MmsAppMessage* xmlmsg, MmsServerCmdHeader& commandHeader, unsigned int* flags)
{
  int result = 0, value = 0;

  if (this->isExpectingClientToken)
  {    
      commandHeader.clienthandle = xmlmsg->getClientID();

      if (-1 != (int)commandHeader.clienthandle)
      {
          if  (this->isConnected((int)commandHeader.clienthandle));           
          else return MMS_ERROR_NOT_CONNECTED;          
      }
      else return MMS_ERROR_TOO_FEW_PARAMETERS;
  }
  else commandHeader.clienthandle = (void*)getClientID(xmlmsg);

  commandHeader.serverID = xmlmsg->serverID(); 

  if  (xmlmsg->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID])) 
       value = ACE_OS::atoi(xmlmsg->paramValue());

  commandHeader.connectionID = this->stripServerID(value, commandHeader);
    
  char* confID = xmlmsg->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID]);

  if (confID)                               
  {
      int  conferenceID = ACE_OS::atoi(xmlmsg->paramValue()); 
      conferenceID = this->stripServerID(conferenceID, commandHeader);
      if (conferenceID < 0) 
          result = MMS_ERROR_PARAMETER_VALUE; 
      else                                 
      {    
          map.insert(MMSP_CONFERENCE_ID, conferenceID); 
          if  (flags) 
              *flags |= CONFERENCEID_PRESENT;
      }
  }  

  value = 0;
  if  (xmlmsg->getvaluefor (MmsAppMessageX::paramnames[MmsAppMessageX::COMMAND_TIMEOUT])) 
       value = ACE_OS::atoi(xmlmsg->paramValue());
  if  (value)
       map.insert(MMSP_COMMAND_TIMEOUT_MS, value); 

  commandHeader.transactionID = this->getTransactionID(xmlmsg);

  return result;
}



int MmsFlatmapIpcAppAdapter::getTransactionID(MmsAppMessage* xmlmsg) 
{
  // Extract transaction ID from XML
  char* pxid = xmlmsg->getvaluefor (MmsAppMessageX::paramnames[MmsAppMessageX::TRANSACTION_ID]);
  const int xid = pxid? ACE_OS::atoi(xmlmsg->paramValue()): 0;
  return xid;
}



// Adapter told to shut down
int MmsFlatmapIpcAppAdapter::onShutdown()           
{
  m_isStarted = m_isActive = FALSE;

  return 0;                                 
}



// Event handler when this adapter started.
void MmsFlatmapIpcAppAdapter::onStartAdapter()
{
  if (!m_isStarted)  
      this->Start();

  m_isStarted = TRUE; 
}



// Event handler when this adapter stopped.
void MmsFlatmapIpcAppAdapter::onStopAdapter()
{
  this->clientClose(0, TRUE);

  this->Stop();
}



// Event handler for server command.
void MmsFlatmapIpcAppAdapter::onCommandServer(void* data)
{
  this->xmlmsg = (MmsAppMessage*)data;
  int result = 0;

  int sessionId = (int)this->getClientID(xmlmsg);
                                       
  if  (!xmlmsg->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::QUERY])) 
    result = MMS_ERROR_TOO_FEW_PARAMETERS;
  else if  (0 != ACE_OS::strcmp(xmlmsg->paramValue(), 
                 MmsAppMessageX::serverQueryNames[MmsAppMessageX::MEDIA_RESOURCES])) 
    result = MMS_ERROR_TOO_FEW_PARAMETERS;
  else if (xmlmsg->backupToEndTag() == -1)
    result = MMS_ERROR_MALFORMED_REQUEST;
  else if (this->isExpectingClientToken && sessionId < 0)
    result = MMS_ERROR_TOO_FEW_PARAMETERS;

  if (result)
      this->turnMqMessageAround(result, 0, TRUE);    
  else 
      this->handleQueryMediaResources(sessionId);
}



// Receive Hearbeat Ack from IPC client, reset client object counters.
void MmsFlatmapIpcAppAdapter::onCommandHeartbeatAck(void* data)
{
  this->xmlmsg = (MmsAppMessage*)data; 

  const int sessionId = (int)this->getClientID(xmlmsg);

  map<int, MmsFlatmapIpcClient*>::iterator index;
  index = clientMap.find(sessionId);
  if (index == clientMap.end()) return;

  MmsFlatmapIpcClient* c = index->second;    
  if (!c) return;

  c->acksUpToDate();

  delete this->xmlmsg; 
  this->xmlmsg = NULL;
}



void MmsFlatmapIpcAppAdapter::pushServerData(MmsMsg* msg)
{
  // Push info back to client via IPC
  char* map = (char*)msg->param();
  if  (!map || !Mms::isFlatmapReferenced(map, 3)) return;

  do
  { if (clientMap.size() == 0) break;
    const int clientId = (int)getFlatmapClientHandle(map);      
    std::map<int, MmsFlatmapIpcClient*>::iterator i = clientMap.find(clientId);
    if (i == clientMap.end()) break;

    MmsFlatmapIpcClient* client = i->second;    
    if (client == NULL) break;

    MmsFlatMapReader reader(map);
    char* rguid = NULL;
    int   rguidlength = reader.find(MMSP_ROUTING_GUID, &rguid);
    if  (!rguidlength || !(*rguid)) break;       // Routing guid missing or empty                                                                  
               
    char digits[2] = { 0, 0 };
    digits[0] = (char)getFlatmapParam(map);      // RFC2833 signal stored in param
                                                  
    int connectionID = getFlatmapConnectionID(map);
                                                 // Overlay server ID onto conx ID
    connectionID = this->insertServerID(connectionID, client->getServerId());

    MmsAppMessage*  xml = new MmsAppMessage;
    xml->putMessageID(MMS_RFC2833_SIGNAL_NAME);    
    xml->putParameter(MmsAppMessageX::CONNECTION_ID, connectionID);
    xml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::DIGITS], digits);
    xml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::ROUTING_GUID], rguid);
    xml->terminateReturnMessage(clientId); 
                                              
    putFlatmapIpcMessage(clientId, xml->getNarrowMessage(), 
           xml->narrowMsglength(), MMS_RFC2833_SIGNAL_NAME);

    delete xml;

  } while(0);

  delete[] map;
}

