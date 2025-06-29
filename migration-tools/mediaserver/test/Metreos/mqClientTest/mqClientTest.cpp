// 
// mqClientTest.cpp - client process to exercise the MmsMqAppAdapter  
//
#include "StdAfx.h"
#pragma warning(disable:4786)
#include "mqClientTest.h"
#include "mqIpcClient.h"
#include "mqIpcMessage.h"
#include <conio.h>

MmsConfig*   config  = NULL;
MmsMqWriter* writer  = NULL;
MmsMq*       reader  = NULL;
MmsAppMessage* appxml = NULL;
MmsAppMessage* outxml = NULL;
mqIpcClient* mqClient = NULL;
QUEUEHANDLE clientID = NULL;
int  state, command, transID, provisoConxID, returnPort, returnPort1, interCommandSleepMs, returnScore;  
int  isShowHeartbeatContent, heartbeatInterval, isNoShowInboundXml;
int  isShowProvisionalContent, isPassthruProvisional; 
char queueName[32], machineName[128], returnIP[32], returnIP1[32], returnMeaning[512];

MQMSGPROPS    msgprops;                     // MQ received message properties
MSGPROPID     aMsgPropId [MMS_MQ_RECEIVE_MAX_PROPERTIES];
MQPROPVARIANT aMsgPropVar[MMS_MQ_RECEIVE_MAX_PROPERTIES];
HRESULT       aMsgStatus [MMS_MQ_RECEIVE_MAX_PROPERTIES];

char   szBody[MMS_MQ_MAX_XMLMESSAGESIZE];   // Dequeued MQ message body
char   buf[192], consoletitle[32];

FILE*  logfile;
time_t mmsStartTime;  
time_t messageSentTime;
int    bodySize, pnSenttime, pnBodytype, pnBodysize, pnBodytext, isShutdown;
int    consecutiveHeartbeatCount, currentTest, appInstanceNumber, n, logGeneration;
int    ipcType = IPC_ADAPTER_METREOS; // IPC_ADAPTER_MSMQ;


                                           
void initMqReceivedMessageProperties()
{
  unsigned long ulBufferSize = MMS_MQ_MAX_XMLMESSAGESIZE;                                            

  aMsgPropId [0] = PROPID_M_BODY_SIZE;      // 0. body size            
  aMsgPropVar[0].vt = VT_NULL;   
  pnBodysize = 0;

  aMsgPropId [1] = PROPID_M_BODY;           // 1. body text          
  aMsgPropVar[1].vt = VT_VECTOR | VT_UI1;        
  aMsgPropVar[1].caub.pElems = (unsigned char*)szBody;   
  aMsgPropVar[1].caub.cElems = ulBufferSize; 
  pnBodytext = 1; 
                                            
  aMsgPropId [2] = PROPID_M_BODY_TYPE;      // 2. body type       
  aMsgPropVar[2].vt = VT_NULL;
  pnBodytype  = 2;

  aMsgPropId [3] = PROPID_M_SENTTIME;       // 3. sent time  
  aMsgPropVar[3].vt = VT_NULL; 
  pnSenttime  = 3;                                               
                                            // Initialize MQMSGPROPS 
  msgprops.cProp    = MMS_MQ_RECEIVE_MAX_PROPERTIES;             
  msgprops.aPropID  = aMsgPropId;           // IDs of the message properties
  msgprops.aPropVar = aMsgPropVar;          // Values of the message properties
  msgprops.aStatus  = aMsgStatus;           // Error reports
}



int getMqMessage(QUEUEHANDLE hq, DWORD timeoutms)
{
  // Block until a MSMQ message arrives or timeout. Returns 1 if a message was
  // dequeued into szBody, zero if timeout or other continue condition,
  // or -1 if error is unrecoverable.

  memset(szBody, 0, MMS_MQ_MAX_XMLMESSAGESIZE); 
  static int consecutiveTimeoutCount = 0; 

  initMqReceivedMessageProperties(); 
  HRESULT hr = -1;

  #ifdef MMS_LINK_WITH_MSMQ  
  
  hr = MQReceiveMessage              
      (hq, timeoutms, MQ_ACTION_RECEIVE, &msgprops, 0, 0, 0, 0);

  #else  // #ifdef MMS_LINK_WITH_MSMQ  
  printf("TEST message queueing is not present in this build\n");
  return -1;
  #endif // #ifdef MMS_LINK_WITH_MSMQ  

  switch(hr)
  {
    case MQ_OK:  
    case MQ_INFORMATION_PROPERTY:   
         break;                             // Found a message

    case MQ_ERROR_IO_TIMEOUT:  
         if  ((consecutiveTimeoutCount++ % 10) == 0)  
         {     log("TEST waiting for server ...\n");
               if   (consecutiveTimeoutCount > BAIL_AFTER_N_TIMEOUTS)  
               {     log("TEST no response ...bailing\n");
                     return -1;
               } 
         } 

         return 0;                          // Timed out ... continue

    default:                                // Unrecoverable error
         sprintf(buf,"TEST MQReceiveMessage failed with %x\n",hr); log(buf);
         writer->showresult(hr);  
         for(int i=0; i < MMS_MQ_RECEIVE_MAX_PROPERTIES; i++) 
         {   sprintf(buf,"TEST prop %d %08x\n",i,aMsgStatus[i]);
             log(buf);
         }
         return -1;                         // Bail
  } 


  log("TEST MQ message received\n"); 
  consecutiveTimeoutCount = 0;         
                                            // Retrieve time of message
  messageSentTime = msgprops.aPropVar[pnSenttime].ulVal;
                                            // Retrieve size of message body
  bodySize = msgprops.aPropVar[pnBodysize].ulVal;                                               

  return 1;
} 



int listenForMqMessage()
{
  while(1)                                  // MQ receive queue event loop
  {
    if  (isShutdown) return 0;

    DWORD timeoutmsecs  = config->clientParams.msmqTimeoutMsecs;
    if   (timeoutmsecs < 1 || timeoutmsecs > 5000) timeoutmsecs = 2000;
                                            // Block until msg or timeout
    switch(getMqMessage(reader->handle(), timeoutmsecs))
    { 
      case  1:                              // A message was dequeued ...
            if  (appxml) delete appxml;     // ... so wrap it up ...
            appxml = new MmsAppMessage(szBody); 
                                            // .. and filter it:
            if  (isHeartbeatMessage(appxml))// Unless it is a heartbeat ...
            {   
                 log("TEST heartbeat from server\n"); 
                 int  heartbeatID = showHeartbeatContent(!isShowHeartbeatContent); 
                 if  (++consecutiveHeartbeatCount > BAIL_AFTER_N_HEARTBEATS)  
                 {    log("TEST no response ...bailing\n");
                      return -1;
                 }
                                            // Send ack if so configged
                 if  (config->clientParams.heartbeatAckExpected) 
                      sendHeartbeatAck(heartbeatID);

                 heartbeatID = heartbeatID; // Breakpoint anchor noop
            }
            else                            // .. or unless it is a proviso ...
            if  (isProvisionalServerResponse() && !isPassthruProvisional)
            {                                
                 appxml->getCommand();
                 sprintf(buf, "TEST %s provisional response received\n",appxml->commandName()); 
                 log(buf);
                 provisoConxID = getMessageConnectionID(); // Save conx ID
                 if  (isShowProvisionalContent)  
                 {    sprintf(buf, "%s\n\n", appxml->getNarrowMessage()); 
                      log(buf);           
                 }
            }           
            else return 1;                  // ... return message for handling

            break;

      case  0:                              // Timeout
            break;

      case -1: 
            return -1;

    }       // switch(getMqMessage...)
  }         // while(1)

  return 0;                                 // Discard and continue 
} 


int sendHeartbeatAck(const int heartbeatID)
{
  MmsAppMessage* outxml = new MmsAppMessage; 
  outxml->putMessageID(MmsAppMessageX::MMSMSG_HEARTBEAT);    
  outxml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::HEARTBEAT_ID], heartbeatID);
  void* clientID = getMessageClientID();
  outxml->terminateReturnMessage(clientID, MMS_OMIT_RESULTCODE);
  log("TEST sending heartbeat ack\n"); 
  // sprintf(buf, "\n\n%s\n\n", outxml->getNarrowMessage()); log(buf);
  putMessage(outxml->getNarrowMessage(), outxml->narrowMsglength());
  delete outxml;
  return 0;
}


int isMessageElderThan(const int secs) 
{ 
  int    diff = (int) difftime(messageSentTime, mmsStartTime); 
  sprintf(buf, "TEST MQ message age is %d\n",diff); log(buf);
  return diff > secs;
}



int getMessageCommand()
{
  if  (!appxml) return -1;
  int  n = appxml->getCommand();
  return n;
}



int getMessageTransactionID()
{
  if  (!appxml) return -1;
  char* pid = appxml->getvaluefor(MMS_TRANSACTIONID_NAME);
  if  (!pid)    return -1;
  int  n = atoi(pid);
  return n;
}



int getMessageResultCode()
{
  if  (!appxml) return -1;
  char* prc = appxml->getvaluefor(MMS_RESULTCODE_NAME);
  if  (!prc)    return 0;
  int  rc = atoi(prc);
  return rc;
}



void* getMessageClientID()
{
  if  (!appxml) return NULL;
  const char* paramname = MmsAppMessageX::paramnames[MmsAppMessageX::CLIENT_ID];
  const char* pid = appxml->getvaluefor(paramname);
  if  (!pid)  return NULL;
  void*  id = (void*)atoi(pid);
  sprintf(buf,"TEST client ID is %x\n",id); log(buf);
  return id;
}



int showResultCode()
{
  int rc = getMessageResultCode();
  sprintf(buf,"TEST result code was %d\n",rc); log(buf);               
  return rc;
}



int checkResultCode(int code1, int code2)
{
  int  rc = getMessageResultCode();
  if  (rc == 0 || rc == code1 || rc == code2) return rc;
  sprintf(buf,"TEST result code was %d ... bailing\n",rc); log(buf);
  isShutdown = TRUE;
  return -1;
}



int showTerminatingConditions()
{
  if  (!appxml) { log("TEST no appxml\n"); return -1; }
  int   count = 0;
  char* searchstart = appxml->firstparam();
  char* paramname   = MmsAppMessageX::paramnames[MmsAppMessageX::TERMINATION_CONDITION];

  while(1)                                  // Find all terminating conditions
  {                                         // supplied in the XML message
    char* bufpos  = appxml->findparam(paramname, searchstart);
    if   (bufpos == NULL) break;
    searchstart   = bufpos; 

    int  tcno = appxml->isolateParameterValue(bufpos);
    if  (tcno == -1)  
         sprintf(buf,"TEST bad term cond '%s'\n",appxml->paramValue()); 
    else sprintf(buf,"TEST terminating condition '%s'\n",appxml->paramValue());
    log(buf);
    count++;
  }
    
  //if   (!count) log("TEST no term condition\n");
  return count;
}



int showHeartbeatContent(const int idonly)
{
  int  heartbeatID = -1, serverID = -1;
  if  (!appxml) return heartbeatID;
  if  (!idonly) { sprintf(buf, "%s\n\n", appxml->getNarrowMessage()); log(buf); }

  const char* paramnameHBID = MmsAppMessageX::paramnames[MmsAppMessageX::HEARTBEAT_ID];
  if  (appxml->findparam(paramnameHBID))
       heartbeatID = atoi(appxml->paramValue());
  if  (idonly) return heartbeatID;
  if  (heartbeatID == -1)
       sprintf(buf,"TEST missing heartbeatID\n");
  else sprintf(buf,"TEST heartbeatID %d\n",heartbeatID);
  log(buf);

  const char* paramnameSID = MmsAppMessageX::paramnames[MmsAppMessageX::SERVER_ID];
  if  (appxml->findparam(paramnameSID))
       serverID = atoi(appxml->paramValue());
  if  (serverID == -1)
       sprintf(buf,"TEST serverID not present\n");
  else sprintf(buf,"TEST serverID %d\n",serverID);
  log(buf);

   
  char* searchstart = appxml->firstparam();
  const char* paramnameMR = MmsAppMessageX::serverQueryNames[MmsAppMessageX::MEDIA_RESOURCES];

  while(1)
  {
    char* bufpos  = appxml->findparam(paramnameMR, searchstart);
    if   (bufpos == NULL) break;
    searchstart   = bufpos; 

    const int result = appxml->isolateParameterValue(bufpos);
    if  (result != -1)  
    {    sprintf(buf,"TEST heartbeat payload '%s'\n",appxml->paramValue());
         log(buf);
    }
  } 

  return heartbeatID; 
}



int showReturnedPortAndIP()
{
  returnPort = 0; memset(returnIP, 0, sizeof(returnIP));
  if  (!appxml) return -1;
  char* pport = NULL, *pip = NULL; 

  pport = appxml->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::PORT]);
  if  (pport) returnPort = atoi(appxml->paramValue());
  sprintf(buf, "TEST returned port is %04d\n",returnPort); log(buf);

  pip = appxml->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::IP_ADDRESS]);
  if (!pip) return 0;
  int length = appxml->isolateParameterValue(pip);
  if (length) strncpy(returnIP, appxml->paramValue(), sizeof(returnIP)-2);
  sprintf(buf,"TEST returned ipad is %s\n",returnIP); log(buf);
  return 0;
}



char* getReturnedRecordPath()
{
  if  (!appxml) return NULL;
  char* ppath =  appxml->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::FILE_NAME]);
  if  (!ppath)  return NULL;
  sprintf(buf,"TEST returned record filename is %s\n",appxml->paramValue()); log(buf);
  return appxml->paramValue();
}



int getMessageConnectionID(char** outpconxid)
{
  if  (!appxml)  return -1;
  char* pconxID = appxml->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ID]);
  if  (!pconxID) return -1;
  if  (outpconxid) *outpconxid = pconxID;
  int  nid = atoi(pconxID);
  return nid;
}



int getMessageConferenceID(char** outpconfid=0)
{
  if  (!appxml)  return -1;
  char* pconfID = appxml->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::CONFERENCE_ID]);
  if  (!pconfID) return -1;
  if  (outpconfid) *outpconfid = pconfID;
  int  nid = atoi(pconfID);
  return nid;
}



int getMessageServerID(char** outpsid)
{
  if  (!appxml) return -1;
  char* psidID = appxml->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::SERVER_ID]);
  if  (!psidID) return -1;
  if  (outpsid) *outpsid = psidID;
  int  nid = atoi(psidID);
  return nid;
}



int getMessageOperationID(char** outid)
{
  if  (!appxml) return -1;
  char* pid = appxml->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::OPERATION_ID]);
  if  (!pid) return -1;
  if  (outid) *outid = pid;
  int  nid = atoi(pid);
  return nid;
}



int getExpectedConnectionID(char** outpconxid)
{
   int  id = getMessageConnectionID(outpconxid);
   if  (id < 1) 
        log("TEST connection ID missing ... bailing\n");
   return id;        
}



int getExpectedConferenceID(char** outpconfid)
{
   int  id = getMessageConferenceID(outpconfid);
   if  (id < 1) 
        sprintf(buf,"TEST conference ID missing ... bailing\n");
   else sprintf(buf,"TEST returned conference ID is %d\n",id);
   log(buf);
   return id;        
}



int getExpectedOperationID(char** outid)
{
   int  id = getMessageOperationID(outid);
   if  (id < 1) 
        sprintf(buf,"TEST operation ID missing ... bailing\n");
   else sprintf(buf,"TEST returned operation ID is %d\n",id);
   log(buf);
   return id;        
}




int isHeartbeatMessage(MmsAppMessage* xmlmsg)
{
  if  (!xmlmsg || xmlmsg->getCommand() == -1) return 0;
  const char*   msgname = xmlmsg->commandName();
  sprintf(buf,"\nTEST message is '%s'\n",msgname); log(buf);
  return strcmp(msgname, MMS_HEARTBEAT_NAME) == 0;
}



int isProvisionalServerResponse()
{
  return getMessageResultCode() == 1; 
}



MmsConfig* getConfigurationInfo()           // Config file object
{                                            
  MmsConfig* config = new MmsConfig;         

  if  (-1 == config->readLocalConfigFile())  
  {    log("TEST could not read config file\n");
       delete config;
       config = NULL;
  }

  return config;
}



int clearReceiveQueue()
{
   int count = 0;
   initMqReceivedMessageProperties(); 

   #ifdef MMS_LINK_WITH_MSMQ  
  
   while(1)
   {  
     HRESULT hr = MQReceiveMessage(reader->handle(), 0, 
             MQ_ACTION_RECEIVE, &msgprops, 0, 0, 0, 0);
     if (hr != MQ_OK && hr != MQ_INFORMATION_PROPERTY) break;   
     count++; 
   }  

  #endif

  return count;
}



int switchIpcAdapter()
{
  if (ipcType == IPC_ADAPTER_MSMQ)
  {
      ipcType = IPC_ADAPTER_METREOS;
      sprintf(buf, "IPC Adapter switched to METREOS IPC"); log(buf);
  }
  else
  {
      ipcType = IPC_ADAPTER_MSMQ;
      sprintf(buf, "IPC Adapter switched to MSMQ"); log(buf);
  }

  return -1;
}



void sendServerConnect(MmsAppMessage* xml, int serverID)
{
  clientID = NULL;
  xml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);  
  xml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::HEARTBEAT_INTERVAL], 60);
  #ifndef USING_DEFAULT_APP_QUEUE             
  xml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::MACHINE_NAME], machineName);
  xml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::QUEUE_NAME],   queueName);
  xml->putParameter(MmsAppMessageX::paramnames  [MmsAppMessageX::CLIENT_ID], 0);
  if  (serverID >= 0)
       xml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::SERVER_ID], serverID);
  #endif
  log("TEST sending media server connect\n");       
}



void sendServerDisconnect(MmsAppMessage* xml)
{
  xml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
  isShutdown = TRUE;                 // Signal final message
  log("TEST sending media server disconnect\n");              
} 


void putParam(MmsAppMessage* xml, const int nameindex, const char* val)
{ 
  xml->putParameter(MmsAppMessageX::paramnames[nameindex], (char*)val);
}


void putParam(MmsAppMessage* xml, const int nameindex, const int val)
{ 
  xml->putParameter(MmsAppMessageX::paramnames[nameindex], (int)val);
}


void putConnectParam(MmsAppMessage* xml, char* param)
{
  xml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::CONNECTION_ATTRIBUTE], param); 
  sprintf(buf,"TEST connect param '%s'\n", param); log(buf);      
}



void putConnectCommand(MmsAppMessage* xml)
{
  xml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_CONNECT]);
}



void putDiscoCommand(MmsAppMessage* xml)
{
  xml->putMessageID(MmsAppMessageX::messagenames[MmsAppMessageX::MMSMSG_DISCONNECT]);
}



void putDefaultLocaleParameters(MmsAppMessage* xml)
{
  xml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::APP_NAME], TEST_APP_NAME);
  xml->putParameter(MmsAppMessageX::paramnames[MmsAppMessageX::LOCALE],   TEST_LOCALE_DIR);
}



DWORD getMediaServerThreadID()
{
  HKEY  hkey;
  DWORD dwResult, dwNeed, dwType, dwThreadID=0; 
  #define MMS_REG_THREADID "ThreadID"
                                            
  dwResult = RegOpenKeyEx(HKEY_LOCAL_MACHINE, MMS_REGKEY, 0, KEY_ALL_ACCESS, &hkey);
  dwResult = RegQueryValueEx(hkey, MMS_REG_THREADID, 0, &dwType, (BYTE*)&dwThreadID, &dwNeed);
  RegCloseKey(hkey);

  if  (dwResult != ERROR_SUCCESS)            
       log("Could not find media server thread ID\n");

  return dwThreadID;
}

        

BOOL CALLBACK findOurMostRecentConsole(HWND hwnd, LPARAM lParam)
{
  TCHAR buf[MAX_PATH];
  GetWindowText(hwnd, buf, MAX_PATH);
  static int PREFIXLENGTH = sizeof(CONSOLE_TITLE_PREFIX) - 1;

  int  isOurConsole  = memcmp(buf, CONSOLE_TITLE_PREFIX, PREFIXLENGTH) == 0;
  if  (isOurConsole)
  {    const char* p = buf + PREFIXLENGTH;
       const int   n = atoi(p);
       if (n > *((int*)lParam))
               *((int*)lParam) = n;      
  }

  return TRUE;                         
}



int getHighestConsoleNumber()   
{
  int  highestconsole = 0 ;
  EnumWindows(findOurMostRecentConsole, (LPARAM)&highestconsole);
  return highestconsole;
}


void log() { log(buf); }


void log(char* msg)
{
  static int lines;

  printf(msg);

  if (logfile)
  {   fwrite(msg, strlen(msg), 1, logfile); 
      fflush(logfile);

      if (lines++ == MMSTEST_MAXLOGSIZE)
      {   openLog();
          lines = 0;
      }
  }
}


FILE* openLog()
{
  closeLog();

  char path[MAX_PATH];
  sprintf(path,"%s%s-%d-%d%s", MMSTEST_LOGFILE_DIR, MMSTEST_LOGFILE_NAME, 
          appInstanceNumber, ++logGeneration, MMSTEST_LOGFILE_EXT);

  return (logfile = fopen(path, "w"));
} 


void  closeLog() { if (logfile) fclose(logfile); logfile = NULL; }


char* getbuf()   { return buf; }
   


int sendNextMqMessage()
{
  switch(currentTest)
  { 
    case TEST_ANNOUNCE_AND_SEND_DIGITS:   return testAnnounceAndSendDigitsSequence();
    case TEST_AUDIO_DESCRIPTOR_FILE:      return testAudioDescriptorFileSequence();
    case TEST_ANNOUNCE_AND_GET_DIGITS:    return testAnnounceAndGetDigitsSequence(); 
    case TEST_HALF_CONNECT:               return sendHalfConnectSequence();
    case TEST_RECORD_PLAYBACK:            return recordAudioAndPlaybackSequence();
    case TEST_CONCURRENT_COMMANDS:        return sendConcurrentCommandSequence();
    case TEST_CONFERENCING_A:             return conferenceSequenceA();
    case TEST_CONFERENCING_B:             return conferenceSequenceB();
    case TEST_CONFERENCING_C:             return conferenceSequenceC();
    case TEST_CONFERENCING_D:             return conferenceSequenceD();
    case TEST_CONFERENCING_E:             return conferenceSequenceE();
    case TEST_MULT_CONCURRENT_CONF:       return conferenceSequenceF();
    case TEST_CONFERENCING_G:             return conferenceSequenceG();
    case TEST_SERVER_QUERY_A:             return serverQuerySequenceA();
    case TEST_SETH_A:                     return sethsTestA(); 
    case RECORD_CONFEREE_CMD_TIMEOUT:     return recordConfereeTestA();
    case TEST_2PARTY_NOANNOUNCE:          return doNotAnnounceTwoPartyConference();
    case TEST_2PARTY_WITHANNOUNCE:        return announceAndJoinTwoPartyConference();
    case TEST_HEARTBEAT_A:                return heartbeatTestA();
    case TEST_CONFERENCE_DISCO_A:         return conferenceDisconnectTestA();
    case TEST_CONFEREE_SET_ATTR:          return confereeSetAttrA();
    case TEST_STOP_MEDIA_OPERATION:       return stopMediaOperationTestA(); // return getDigitsTimeout(); 
    case TEST_PLAY_TO_CONFEREES:          return conferencePlayToConfereesA();
    case TEST_PLAY_TO_CONFEREES_B:        return conferencePlayToConfereesB();
    case TEST_PLAY_TO_CONFERENCE:         return conferencePlayToAll();
    case TEST_RECORD_CONFERENCE:          return conferenceRecordAll();
    case TEST_RECORD_CONFERENCE_BARGEIN:  return conferenceRecordBargeInEx();
    case TEST_PLAY_TO_CONFERENCE_TIMEOUT: return conferenceTestPlayTimeout();
    case TEST_DISCONNECT_WITH_BARGEIN:    return disconnectBargeInTestC();
    case TEST_DISCO_CONFEREE_WITH_BARGEIN:return disconnectBargeInTestB();
    case TEST_PLAY_ON_HALF_CONNECT:       return attemptCommandOnHalfConnectedSession(); 
    case TEST_RECONNECT:                  return reconnectRemoteIP(); 
    case TEST_RECONNECT_TO_CONFERENCE:    return reconnectRemoteIPToConference(); 
    case TEST_CONNECT_PARAMETER_STATE:    return testConnectParameterState();
    case TEST_CONFERENCE_ABANDON_RECORD:  return conferenceAbandonRecording();
    case TEST_CONFERENCE_RECORD_CANCEL:   return conferenceRecordAndCancel();
    case TEST_CONFERENCE_RECORD_TERMINATE:return conferenceRecordTerminationTest();
    case TEST_RECORD_TERMINATE_ON_DIGIT:  return testRecordTerminationOnDigit();
    case TEST_GET_CONFEREE_DIGITS:        return confereeGetDigitsTest();
    case TEST_DIGITPATTERN:               return digitPatternTest();
    case TEST_CONF_REC_PROMPT_DISCO:      return conferenceRecordPromptAndDisco();
    case CONSECUTIVEPLAYWITHDIGITTERM:    return twoPlaysWithGetDigitPlusDelay();
    case CANCEL_DIGITPATTERN:             return cancelDigitPattern();
    case TEST_DIGITLIST:                  return digitListTest();
    case TEST_CALLSTATE:                  return monitorCallState();
    case CONFEREE_STOPMEDIA_AND_REPLAY:   return confereeStopMediaAndReplay(); 
    case FLOOD_DIGITS:                    return digitsOverManyConnections();
    case VOICE_TUNNEL_LOOP:               return voiceTunnelLoop();
    //se VOICE_TUNNEL_LOOP:               return voiceTunnelNoConference();
    case SCHEDULED_CONFERENCE_SIM:        return scheduledConferenceSim();
    //se SCHEDULED_CONFERENCE_SIM:        return confereeRecordWhileBusy();
    case DIGIT_PATTERN_IMMEDIATE:         return digitPatternTestB();
    case TEST_PLAYTONE:                   return testPlaytone();
    case TEST_PLAY_2_CONFEREE_GETDIGITS:  return playTwoToConfereeAndGetDigits();
    case TEST_RETRANSMIT_CONNECTION:      return testRetransmitConnection();
    case TEST_PLAY_AND_GETDIGITS:         return playAndGetDigitsTest();
    case TEST_TTS_A:                      return ttsTestA();
    case SWITCH_IPC_ADAPTER:              return switchIpcAdapter();
    case ABANDON_PLAY_TO_LONE_CONFEREE:   return abandonPlayToLoneConferee();
    case RECONNECT_OVER_MEDIA:            return reconnectOverMedia();
    case CONFRECORD_WHILE_PLAYCONFEREE:   return conferenceRecordWhilePlayToConferee();
    case TEST_MAX_CONNECTIONS:            return testMaxConnectionsD(); // test G729 with A, B, C
    case TEST_HAIRPIN_A:                  return hairpinTestA();
    case TEST_SIMPLE_LBR:                 return simpleLBRTest();
    case TEST_DIGIT_PATTERN_C:            return digitPatternTestC();
    case TEST_VOLSPEED:                   return testVolumeSpeedControl();
    case TEST_VOICEREC:                   return voiceRecTest();
    case TEST_CONCURRENT_PLAY_GET_DIGITS: return concurrentPlayGetDigits();
    case TEST_CONCURRENT_VR_GET_DIGITS:   return concurrentVoiceRecGetDigits();
    case TEST_CONCURRENT_STOPMEDIA:       return stopMediaOnConcurrentOperations();
    case TEST_COACH_PUPIL:                return coachPupilTestB();
    case TEST_CONFSTOPMEDIA_UTILSESSION:  return conferenceStopMediaOnUtilitySession();
    case TEST_CONF_DELETE_UTILSESSION:    return conferenceDeleteUtilitySession();
    case TEST_CONF_EMPTY_PLAY_AND_RECORD: return conferenceEmptyPlayAndRecord();
    case TEST_HALF_CONNECT_LOOP:          return halfConnectLoop();
    case TEST_CONCURRENT_TTS:             return testConcurrentTTS();
  }

  return -1;  
}



int doMqMediaServerAdapterTestSequence()
{
  while(1)
  {
     if  (isShutdown) break;

     if  (interCommandSleepMs == USE_DEFAULT_INTERCOMMAND_DELAY)
          mmsSleep(CLIENTTEST_INTERCOMMAND_DELAYSECONDS);
     else
     if  (interCommandSleepMs > 0)
          mmsSleep(MmsTime(0,interCommandSleepMs * 100));

     int result = sendNextMqMessage();      // Fire off next xml message
     switch(result)                         // -1, 0, or 1
     {
        case -1:                            // Done or error
             return -1;
        case 0:                             // No reply expected 
             Sleep(1000);                   // Why do we do sleep here?                   
             continue;       
     }       

     if  (ipcType == IPC_ADAPTER_MSMQ)      // Wait for a reply
          result = listenForMqMessage();    // -1, 0, or 1     
     else result = listenForFlatmapIpcMessage();  

     if (result == 1 && appxml != NULL)
     {  
        if (!isNoShowInboundXml)            // Display xml                            
        {   printf("\n\nInbound XML\n");
            sprintf(buf, "%s\n", appxml->getNarrowMessage());
            log(buf);
        }

        consecutiveHeartbeatCount = 0;
        sprintf(buf, "TEST %s result code was %d\n",  
                appxml->commandName(), getMessageResultCode());
        log(buf);
        command = getMessageCommand();

        sprintf(buf,"TEST trans ID was %d\n", getMessageTransactionID()); log(buf);
        showTerminatingConditions();
     }   

     switch(result)                           
     {                                       
        case -1: return -1;                  // msg receive error
        case  0: printf("LOOP message send result zero (timeout)\n");
     }
  }
  return 0;
}


#define WM_MMS_PING WM_USER + 259  



int main(int argc, char* argv[])
{
  appInstanceNumber = getHighestConsoleNumber() + 1;
  wsprintf(consoletitle, "%s%d", CONSOLE_TITLE_PREFIX, appInstanceNumber);
  wsprintf(queueName, INSTANCE_QUEUE_MASK, appInstanceNumber); 
  SetConsoleTitle(consoletitle);
  interCommandSleepMs = USE_DEFAULT_INTERCOMMAND_DELAY;
  n = sizeof(machineName); GetComputerName(machineName, (unsigned long*)&n);

  // Test the media server ping - we do not currently listen for WM_MMS_PINGBACK
  DWORD threadID = getMediaServerThreadID();
  if (threadID)    
      PostThreadMessage(threadID, WM_MMS_PING, 0, GetCurrentThreadId());

  time(&mmsStartTime);

  do {                  
                                          
  if  (NULL == (config = getConfigurationInfo())) 
       break; 
                                            // Media server queue
  char* sendMachinename    = config->clientParams.msmqMmsMachineName;
  char* sendQueuename      = config->clientParams.msmqMmsQueueName;
                                            
  #ifdef USING_DEFAULT_APP_QUEUE            // Use configged qname
  char* receiveMachinename = config->clientParams.msmqAppMachineName;  
  char* receiveQueuename   = config->clientParams.msmqAppQueueName; 
  #else
  char* receiveMachinename = machineName;   // Use instance qname
  char* receiveQueuename   = queueName; 
  #endif

  openLog(); 

  writer = new MmsMqWriter(); 

  mqClient = mqIpcClient::Instance();
	mqClient->open();
	mqClient->activate();
	Sleep(1000);


  #ifdef MMS_LINK_WITH_MSMQ  

  ipcType = IPC_ADAPTER_MSMQ;
  log("TEST opening send queue\n");

  int  result = writer->openQueue(sendMachinename, sendQueuename); 
  if  (result == -1) break;

  #else  // MMS_LINK_WITH_MSMQ

  ipcType = IPC_ADAPTER_METREOS;

  #endif // MMS_LINK_WITH_MSMQ 


  reader = new MmsMq(MmsMq::READER);


  #ifdef MMS_LINK_WITH_MSMQ                  
  log("TEST opening receive queue\n"); 
  result = reader->openQueue(receiveMachinename, receiveQueuename);                   
  if  (result == -1) break; 
  #endif // MMS_LINK_WITH_MSMQ  

  char c[2] = {0,0};
  int  selection = 0;

  while(1)
  {
    printf("\n");
    printf("a. Record and playback\n");    
    printf("b. Announce and get digits\n");    
    printf("c. Half connect\n");               
    printf("d. Connect, disco, reconnect\n");  
    printf("e. Announce and send digits\n");   
    printf("f. Audio descriptor file\n");      
    printf("g. Server query\n");              
    printf("h. Concurrent connect/play\n");     
    printf("i. Simple two-step conference\n");   
    printf("j. Simple one-step conference\n");    
    printf("k. Multiple one-step conference\n");   
    printf("l. Multiple two-step conference\n");  

    #ifdef  IS_MONITOR_TEST  
    printf("m. Announce to individual conference monitor\n");
    #else
    printf("m. Announce to individual conferee\n");
    #endif

    printf("n. Concurrent multi-conference\n");     
    printf("o. Conference disco all w server ID passing\n");     
    printf("p. Conference 2-party w announce\n");   
    printf("q. Conference 2-party no announce\n");  
    printf("r. Heartbeat payload\n");  
    printf("s. Conferee set attribute\n");  
    printf("t. Stop media operation\n");  
    printf("u. Announce & disco each conferee in turn\n");  
    printf("v. Mute, play to, and unmute conferee\n");  
    printf("w. Play audio to conference\n");  
    printf("x. Record conference - silence termination\n"); 
    printf("y. Record conference - program termination\n");  
    printf("z. Play to conference timeout via parameter\n");
    printf("0. Switch IPC adapter\n");      
    printf("1. Record conferee, terminate on cmd timeout\n"); 
    printf("2. Abandon play to lone conferee\n"); 
    printf("3. Reconnect over session busy with media\n"); 
    printf("4. Record conference while play to conferee\n");  
    printf("5. Connect maximum number of connections\n"); 
    printf("6. Hairpinning test (configurable)\n");
    printf("7. Volume/speed control test\n");
    printf("8. Voice Recognition test\n");
    printf("9. Concurrent play and getdigits test\n");
    printf("@. Concurrent voice recognition and getdigits test\n");
    printf("^. Concurrent operations stop media test\n");
    printf("!. GetDigits digit pattern plus HMP termination\n");
    printf("&. Coach/Pupil test\n");
    printf("`. Stop media on conference untility session\n");
    printf("(. Disconnect conference untility session\n");
    printf("). Empty a playing and recording conference\n");
    printf("~. Half-connect loop (configurable)\n");
    printf("+. TTS test (config iparamA = 53460, iparamb = test #)\n");
    printf("{. Concurrent 1kb TTS plays\n");
    printf("A. Play audio, disconnect with barge-in\n");  
    printf("B. Attempt play on half-connected session\n");   
    printf("C. Reconnect remote session, OK and failure\n");          
    printf("D. Reconnect remote session to conference\n");   
    printf("E. Test persist connect param state on reconnect\n");   
    printf("F. Abandon a recording conference\n");                 
    printf("G. Record a conference and cancel record session\n");  
    printf("H. Disco all conferees from a recorded conference\n");  
    printf("I. Record and terminate on digit\n"); 
    printf("J. Gather termination digits from conferee\n");     
    printf("K. Terminate on digit pattern\n");   
    printf("L. Record conference, disco party during announce\n");
    printf("M. Play, GetDigits with IDD, Play\n");  
    printf("N. Start digit pattern and cancel\n"); 
    printf("O. Announce with digitlist termination\n");  
    printf("P. Monitor call state (silence)\n");                                 
    printf("Q. Full disco conferee with barge-in\n");  
    printf("R. Stop media on conferee and play another\n"); 
    printf("S. Flood digits over n connections (configurable)\n"); 
    printf("T. Voice tunnel sim (configurable)\n"); 
    //printf("U. Scheduled conference sim\n"); 
    printf("U. Simple LBR test (include subtests)\n");
    printf("V. Digit pattern firing immediate\n");  
    printf("W. Play tone\n"); 
    printf("X. Play 2 files to conferee and get digits\n");   
    printf("Y. Listen a connection to itself\n"); 
    printf("Z. Play and get digits\n"); 

    printf(">> selection? ..."); 
    selection=0; c[0] = 0;
    while(!c[0]) c[0] = _getch();

    switch(c[0])       
    { 
      case '0': selection = SWITCH_IPC_ADAPTER;              break;
      case '1': selection = RECORD_CONFEREE_CMD_TIMEOUT;     break;
      case '2': selection = ABANDON_PLAY_TO_LONE_CONFEREE;   break;
      case '3': selection = RECONNECT_OVER_MEDIA;            break;
      case '4': selection = CONFRECORD_WHILE_PLAYCONFEREE;   break;
      case '5': selection = TEST_MAX_CONNECTIONS;            break;
      case '6': selection = TEST_HAIRPIN_A;                  break;
      case '7': selection = TEST_VOLSPEED;                   break;
      case '8': selection = TEST_VOICEREC;                   break;
      case '9': selection = TEST_CONCURRENT_PLAY_GET_DIGITS; break;
      case '@': selection = TEST_CONCURRENT_VR_GET_DIGITS;   break;
      case '^': selection = TEST_CONCURRENT_STOPMEDIA;       break;
      case '&': selection = TEST_COACH_PUPIL;                break;
      case '+': selection = TEST_TTS_A;                      break;  
      case '!': selection = TEST_DIGIT_PATTERN_C;            break;
      case '`': selection = TEST_CONFSTOPMEDIA_UTILSESSION;  break;
      case '(': selection = TEST_CONF_DELETE_UTILSESSION;    break;
      case ')': selection = TEST_CONF_EMPTY_PLAY_AND_RECORD; break;
      case '~': selection = TEST_HALF_CONNECT_LOOP;          break;
      case '{': selection = TEST_CONCURRENT_TTS;             break;
      case 'a': selection = TEST_RECORD_PLAYBACK;            break;
      case 'b': selection = TEST_ANNOUNCE_AND_GET_DIGITS;    break;     
      case 'c': selection = TEST_HALF_CONNECT;               break;     
      case 'd': selection = TEST_SETH_A;                     break;     
      case 'e': selection = TEST_ANNOUNCE_AND_SEND_DIGITS;   break;     
      case 'f': selection = TEST_AUDIO_DESCRIPTOR_FILE;      break;     
      case 'g': selection = TEST_SERVER_QUERY_A;             break;     
      case 'h': selection = TEST_CONCURRENT_COMMANDS;        break;     
      case 'i': selection = TEST_CONFERENCING_A;             break;     
      case 'j': selection = TEST_CONFERENCING_B;             break;     
      case 'k': selection = TEST_CONFERENCING_C;             break;     
      case 'l': selection = TEST_CONFERENCING_D;             break;     
      case 'm': selection = TEST_CONFERENCING_E;             break;     
      case 'n': selection = TEST_MULT_CONCURRENT_CONF;       break;     
      case 'o': selection = TEST_CONFERENCE_DISCO_A;         break;     
      case 'p': selection = TEST_2PARTY_WITHANNOUNCE;        break;     
      case 'q': selection = TEST_2PARTY_NOANNOUNCE;          break; 
      case 'r': selection = TEST_HEARTBEAT_A;                break;
      case 's': selection = TEST_CONFEREE_SET_ATTR;          break;
      case 't': selection = TEST_STOP_MEDIA_OPERATION;       break;
      case 'u': selection = TEST_PLAY_TO_CONFEREES;          break;
      case 'v': selection = TEST_PLAY_TO_CONFEREES_B;        break;
      case 'w': selection = TEST_PLAY_TO_CONFERENCE;         break;
      case 'x': selection = TEST_RECORD_CONFERENCE;          break;
      case 'y': selection = TEST_RECORD_CONFERENCE_BARGEIN;  break;
      case 'z': selection = TEST_PLAY_TO_CONFERENCE_TIMEOUT; break;
      case 'A': selection = TEST_DISCONNECT_WITH_BARGEIN;    break;
      case 'B': selection = TEST_PLAY_ON_HALF_CONNECT;       break;
      case 'C': selection = TEST_RECONNECT;                  break;
      case 'D': selection = TEST_RECONNECT_TO_CONFERENCE;    break;  
      case 'E': selection = TEST_CONNECT_PARAMETER_STATE;    break;  
      case 'F': selection = TEST_CONFERENCE_ABANDON_RECORD;  break;
      case 'G': selection = TEST_CONFERENCE_RECORD_CANCEL;   break;
      case 'H': selection = TEST_CONFERENCE_RECORD_TERMINATE;break;
      case 'I': selection = TEST_RECORD_TERMINATE_ON_DIGIT;  break;
      case 'J': selection = TEST_GET_CONFEREE_DIGITS;        break;
      case 'K': selection = TEST_DIGITPATTERN;               break; 
      case 'L': selection = TEST_CONF_REC_PROMPT_DISCO;      break; 
      case 'M': selection = CONSECUTIVEPLAYWITHDIGITTERM;    break;
      case 'N': selection = CANCEL_DIGITPATTERN;             break;
      case 'O': selection = TEST_DIGITLIST;                  break;
      case 'P': selection = TEST_CALLSTATE;                  break;
      case 'Q': selection = TEST_DISCO_CONFEREE_WITH_BARGEIN;break;
      case 'R': selection = CONFEREE_STOPMEDIA_AND_REPLAY;   break;
      case 'S': selection = FLOOD_DIGITS;                    break;
      case 'T': selection = VOICE_TUNNEL_LOOP;               break;
      //se 'U': selection = SCHEDULED_CONFERENCE_SIM;        break;
      case 'U': selection = TEST_SIMPLE_LBR;                 break;
      case 'V': selection = DIGIT_PATTERN_IMMEDIATE;         break;
      case 'W': selection = TEST_PLAYTONE;                   break;
      case 'X': selection = TEST_PLAY_2_CONFEREE_GETDIGITS;  break;
      case 'Y': selection = TEST_RETRANSMIT_CONNECTION;      break;
      case 'Z': selection = TEST_PLAY_AND_GETDIGITS;         break;
    }

    if  (selection)
    {   
         printf("\n");
         currentTest = selection;
         int  count  = clearReceiveQueue();
         if  (count)
         {    sprintf(buf,"TEST cleared %d messages from queue\n",count);                         
              log(buf);
         }

         mqClient->ClearQueue();

         state = isShutdown = 0;
         heartbeatInterval  = DEFAULT_HEARTBEAT_INTERVAL;
         doMqMediaServerAdapterTestSequence();  
    }
    else printf("bogus selection %s\n",c);

    c[0] = 0;
    printf("\nhit 'm' for menu, anything else to quit ...\n");
    while(!c[0]) c[0] = _getch(); printf("\n");
    if   ( c[0] != 'm') break;

  } // while(1)          
    

  } while(0);           


  if  (appxml) delete appxml;
  if  (outxml) delete outxml;

  if  (writer)
  {    log("TEST close send queue\n");
       writer->closeQueue();
       delete writer;
  }

  if  (reader)
  {    log("TEST close and delete instance queue\n");
       reader->closeQueue(); 
       reader->deleteQueue(); 
       delete reader;
  }

  if (mqClient)
  {
      mqClient->msg_queue()->deactivate();
      mqClient->close();
      delete mqClient;
  }

  closeLog();
  if  (config)  delete config;
  char c = 0; printf("any key ..."); while(!c) c = _getch();
  return 0;
}


int putMessage(char* body, const int length)
{
  if (ipcType == IPC_ADAPTER_MSMQ)
  {
      log("TEST sending over MSMQ\n");
      return writer->putMqMessage(body, length);
  }
  else
  {
      log("TEST sending over Metreos IPC\n");
      return mqClient->WriteIpcMessage(body, length);
  }
}



int listenForFlatmapIpcMessage()
{
  if (isShutdown) return 0;
  DWORD timeoutmsecs  = config->clientParams.msmqTimeoutMsecs;
  if   (timeoutmsecs < 1 || timeoutmsecs > 5000) timeoutmsecs = 2000;


  while(1)                                  // receive event loop
  {
    if (isShutdown) return 0;

    mqIpcMessage* msg = mqClient->GetIpcMessage(timeoutmsecs);
    if (msg == NULL) break;
          
    memset(szBody, 0, MMS_MQ_MAX_XMLMESSAGESIZE); 
    memcpy(szBody, msg->data, msg->len);
    if (appxml) delete appxml;    
    appxml = new MmsAppMessage(szBody); 

    delete msg;
                        
    if (isHeartbeatMessage(appxml))         // Unless it is a heartbeat ...
    {   
        log("TEST heartbeat from server\n"); 
        int  heartbeatID = showHeartbeatContent(!isShowHeartbeatContent); 
        if (++consecutiveHeartbeatCount > BAIL_AFTER_N_HEARTBEATS)  
        {   log("TEST no response from server ... bailing\n");
            return -1;
        }
                                            // Send ack if so configged
        if (config->clientParams.heartbeatAckExpected) 
            sendHeartbeatAck(heartbeatID);

        heartbeatID = heartbeatID;         // Breakpoint anchor noop
    }
    else                                   // .. or unless it is a proviso ...
    if (isProvisionalServerResponse() && !isPassthruProvisional)
    {                                
        appxml->getCommand();
        sprintf(buf, "TEST %s provisional response received\n", appxml->commandName()); log(buf);
        provisoConxID = getMessageConnectionID(); // Save conx ID
        if (isShowProvisionalContent)  
        {   sprintf(buf, "%s\n\n", appxml->getNarrowMessage()); log(buf);           
        }
    }           
    else return 1;                          // ... return message for handling
          
  }  // while(1)

  return 0;                                 // Discard and continue 
}



int showVoiceRecognitionResult()
{
  returnScore = 0; 
  memset(returnMeaning, 0, sizeof(returnMeaning));
  if  (!appxml) return -1;
  char* pScore = NULL, *pMeaning = NULL; 

  pScore = appxml->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::VR_SCORE]);
  if (pScore) returnScore = atoi(appxml->paramValue());
  sprintf(buf, "TEST returned score is %d\n",returnScore); log(buf);

  pMeaning = appxml->getvaluefor(MmsAppMessageX::paramnames[MmsAppMessageX::VR_MEANING]);
  if (!pMeaning) return 0;
  int length = appxml->isolateParameterValue(pMeaning);
  if (length) strncpy(returnMeaning, appxml->paramValue(), sizeof(returnMeaning));
  sprintf(buf,"TEST returned meaning is %s\n", returnMeaning); log(buf);
  return 0;
}


