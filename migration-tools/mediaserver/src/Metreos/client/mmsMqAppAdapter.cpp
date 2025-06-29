//
// MmsMqAppAdapter.cpp 
// XML-based server adapter
//
#include "StdAfx.h"
#pragma warning(disable:4786)
#include "mmsMqAppAdapter.h"
#include <minmax.h>

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION



void MmsMqAppAdapter::onData(void* data) 
{
  // MMSM_DATA handler: client XML payload received from listener 
  // Start the protocol conversion/server command sequence 
  // printf("%s\n",((MmsAppMessage*)data)->getNarrowMessage());
  
  this->onProtocolDataReceived(data);        
}
 


int MmsMqAppAdapter::preparseCommand(void* protocolData)
{
  // Inspect data received over the network and determine server command
  // encapsulated therein. Return the command ID to base class, which will
  // dispatch the indicated virtual command handler.
    
  this->xmlmsg = (MmsAppMessage*)protocolData;

  int  command = xmlmsg->getCommand();

  if  (!isConnected() && command != MmsAppMessageX::MMSMSG_CONNECT)
  {    this->turnMqMessageAround(MMS_ERROR_NOT_CONNECTED, 0, TRUE);
       return -1;
  }

       
  switch(command)                  
  {                                         
    case MmsAppMessageX::MMSMSG_CONNECT: 
         return logCommand(COMMANDTYPE_CONNECT, "CONNECT");
    case MmsAppMessageX::MMSMSG_DISCONNECT: 
         return logCommand(COMMANDTYPE_DISCONNECT, "DISCONNECT");
    case MmsAppMessageX::MMSMSG_HEARTBEAT: 
         return logCommand(COMMANDTYPE_HEARTBEAT,"heartbeat ack", LM_TRACE);
    case MmsAppMessageX::MMSMSG_SERVER:     
         return logCommand(COMMANDTYPE_SERVER, "SERVER");
    case MmsAppMessageX::MMSMSG_PLAY:       
         return logCommand(COMMANDTYPE_PLAY, "PLAY");
    case MmsAppMessageX::MMSMSG_PLAYTONE:       
         return logCommand(COMMANDTYPE_PLAYTONE, "PLAYTONE");
    case MmsAppMessageX::MMSMSG_GETDIGITS:  
         return logCommand(COMMANDTYPE_RECEIVE_DIGITS, "GETDIGITS");
    case MmsAppMessageX::MMSMSG_SENDDIGITS: 
         return logCommand(COMMANDTYPE_SEND_DIGITS, "SENDDIGITS");
    case MmsAppMessageX::MMSMSG_STOPMEDIA:  
         return logCommand(COMMANDTYPE_STOP_OPERATION, "STOPOPERATION");
    case MmsAppMessageX::MMSMSG_RECORD:     
         return logCommand(COMMANDTYPE_RECORD, "RECORD");
    case MmsAppMessageX::MMSMSG_CONFEREESETATTR:      
         return logCommand(COMMANDTYPE_CONFEREE_SETATTRIBUTE, "CONFEREESETATTR");
    case MmsAppMessageX::MMSMSG_MONITOR_CALL_STATE:
         return logCommand(COMMANDTYPE_MONITOR_CALL_STATE, "MONITORCALLSTATE");
    case MmsAppMessageX::MMSMSG_ADJUST_PLAY:
         return logCommand(COMMANDTYPE_ADJUST_PLAY, "ADJUSTPLAY");
    case MmsAppMessageX::MMSMSG_VOICEREC:
         return logCommand(COMMANDTYPE_VOICEREC, "VOICEREC");

    #if 0
    case MmsAppMessage::MMSMSG_RECORDTRANSACTION:    
         return logCommand(COMMANDTYPE_RECORD_TRANSACTION, "RECORDTRANSX");
    case MmsAppMessage::MMSMSG_CONFRESREMAINING:     
         return logCommand(COMMANDTYPE_CONFERENCE_RESOURCES, "CONFREMAIN");
    case MmsAppMessage::MMSMSG_CONFENABLEVOLCONTROL: 
         return logCommand(COMMANDTYPE_CONFEREE_ENABLE_VOL, "CONFENABVOL");
    case MmsAppMessage::MMSMSG_ASSIGNVOLDIGIT:       
         return logCommand(COMMANDTYPE_ASSIGN_VOLADJ_DIGIT, "CONFASGNVOL");
    case MmsAppMessage::MMSMSG_ASSIGNSPEEDDIGIT:
         return logCommand(COMMANDTYPE_ASSIGN_SPEEDADJ_DIGIT, "CONFASGNSPEED");
    case MmsAppMessage::MMSMSG_ADJUSTVOL:
         return logCommand(COMMANDTYPE_ADJUST_VOLUME, "CONFADJVOL");
    case MmsAppMessage::>MMSMSG_ADJUSTSPEED:
         return logCommand(COMMANDTYPE_ADJUST_SPEED, "CONFADJSPEED");
    case MmsAppMessage::MMSMSG_CLEARVOLSPEED:
         return logCommand(COMMANDTYPE_CLEAR_VS_ADJUSTMENTS, "CONFCLRADJ");
    #endif
  }

  MMSLOG((LM_ERROR,"APMQ unrecognized command '%s'\n", xmlmsg->paramValue()));                                            
  this->turnMqMessageAround(MMS_ERROR_NO_SUCH_COMMAND, 0, 0);  
  return -1;
}



MmsAppMessage* MmsMqAppAdapter::writeStandardClientMessageContent
( char* flatmap, const int commandno)
{
  // Create message wrapper for xml return message, and write to it 
  // the return parameters common to all media server commands

  // Note that if this is a provisional result, the server side could be
  // writing to the parameter map during execution of this code, so we
  // should depend only on that information which will not change (e.g.
  // session), and on that information which server side guarantees not
  // to change after sending a provisional (xflags).

  MmsAppMessage*  outxml = new MmsAppMessage; 
  char* command = outxml->commandName(commandno);

  if (!Mms::isFlatmapReferenced(flatmap, 32)) 
      return outxml;
   
  const int mapparam = getFlatmapParam(flatmap);
  const int retcode  = getFlatmapRetcode(flatmap);
  const int session  = getFlatmapSessionID(flatmap);
  const int wasError = isFlatmapFlagSet(flatmap, MmsServerCmdHeader::IS_ERROR);
  void* clientID     = getFlatmapClientHandle(flatmap);

  #if(0)
  int  isHalfConnect                     
    = (isFlatmapFlagSet(flatmap, MmsServerCmdHeader::IS_NOSTARTMEDIA)  
  && !(isFlatmapFlagSet(flatmap, MmsServerCmdHeader::IS_EXISTING_CONNECTION)));
  #endif
       
  int serverID = getFlatmapServerID(flatmap);
  if (serverID == 0 && commandno <= COMMANDTYPE_CONNECT)                                  
      serverID = this->insertServerID(flatmap);             

  outxml->destination(clientID);            // Pass back destination queue 
  outxml->serverID(serverID);

  const int isProvisionalResponse = isFlatmapXflagSet
           (flatmap, MmsServerCmdHeader::IS_PROVISIONAL_RESULT);

  if  (isProvisionalResponse)
       MMSLOG((LM_INFO,"APMQ session %d %s async ack\n", session, command));
  else MMSLOG((LM_INFO,"APMQ session %d %s result %d\n", session, command, retcode));
                                            
  outxml->putMessageID(command);            // Reconstruct the command XML

  MmsFlatMapReader map(flatmap);            // Check for return parameters:
                                            // Transaction ID:
  unsigned int transactionID = getFlatmapTransID(flatmap);
  if (transactionID)                         
      outxml->putTransactionID(transactionID);
                                            // Connection ID:
  int connectionID = getFlatmapConnectionID(flatmap);
                                            // Overlay server ID
  connectionID = this->insertServerID(connectionID, serverID);
  outxml->putParameter(MmsAppMessageX::CONNECTION_ID, connectionID);
                                            // Operation ID:
  int operationID  = getFlatmapOperationID(flatmap);
  if (operationID)                          // Overlay server ID
  {   operationID  = this->insertServerID(operationID, serverID);
      outxml->putParameter(MmsAppMessageX::OPERATION_ID, operationID);
  }

  if (isProvisionalResponse && commandno == MmsAppMessageX::MMSMSG_RECORD)
      this->insertFilename(outxml,flatmap); // Insert recorded file path

  char* pconfID = NULL;                     // Check for conference ID    
  map.find(MMSP_CONFERENCE_ID, &pconfID);

  // If a conference was just created, client will have specified conference
  // ID zero on the connect command. If this is the case, the parameter map
  // still contains the conference ID zero entry, but the media server has
  // returned the new conference ID in the map's param slot; and so we will 
  // not xml the new conference ID here, but rather back in onReturnConnect

  if  (pconfID)                             // If conference ID in map ...
  {                                         
       const int isNewConferenceID           
          = (commandno == MmsAppMessageX::MMSMSG_CONNECT) && mapparam;
                                            // If not a new ID ...
       if  (!isNewConferenceID)             // ... xml the old ID
       {       
             int confID = *((int*)pconfID);  
             confID = this->insertServerIdExcludeZero(confID, serverID);
             outxml->putParameter(MmsAppMessageX::CONFERENCE_ID, confID);  
       }
  }   

  if  (isProvisionalResponse)               // If provisional response ...
  {                                         // ... that's all we need ...
       this->postClientReturnMessage(&outxml, flatmap);
       outxml = NULL;                       // ... so post message now
  }                                         // otherwise ...                                            
  else                                      // ... check terminating conditions
  {    this->writeTerminationReasons(outxml, flatmap);  

       long elapsed = getFlatmapElapsedTime(flatmap);
       if  (elapsed)                        // ... and return media time
            outxml->putParameter(MmsAppMessageX::MEDIA_ELAPSED_TIME, elapsed);
  }

  return outxml;
}



#define MMS_USING_LEGACY_RESOURCES_MESSAGING // Should not ordinarily be defined

#ifdef MMS_USING_LEGACY_RESOURCES_MESSAGING  // Should not ordinarily be defined

// Old (pre-statistics and licensing) formatQueryMediaResources() and
// formatAvailableMediaResources(). This old code is here in order to verify
// that the newer resources payload is not the culprit in a client crash.

void MmsMqAppAdapter::formatQueryMediaResources(MmsAppMessage* thisxml)
{
  // Queries media server for mediaResources, formats and inserts results
  // to specified xml message. Results are formatted similar to: 
  // <field name="mediaResources">ipResourcesAvailable 64</field>

  // We may possibly wish to write some code to synchronize the various
  // clients heartbeat pulses, if they have the same interval, in order 
  // to reduce server query overhead.    
                                            // Save "mediaResources" name
  const char* fieldName = MmsAppMessageX::serverQueryNames[MmsAppMessageX::MEDIA_RESOURCES];
  const int  PAIRBUFLEN = 128;
  char  namevalpair[PAIRBUFLEN];            
              
  static const int ids[] =                  // Make an id to value mapping
  { MmsAppMessageX::IP_RESOURCES_INSTALLED,    MmsAppMessageX::IP_RESOURCES_AVAILABLE, 
    MmsAppMessageX::VOICE_RESOURCES_INSTALLED, MmsAppMessageX::VOICE_RESOURCES_AVAILABLE,
    MmsAppMessageX::CONF_RESOURCES_INSTALLED,  MmsAppMessageX::CONF_RESOURCES_AVAILABLE,
    MmsAppMessageX::LOBIT_RESOURCES_INSTALLED, MmsAppMessageX::LOBIT_RESOURCES_AVAILABLE
  };
  const int counts[] =                      
  { MmsAs::insG711, MmsAs::avlG711,
    MmsAs::insVox,  MmsAs::avlVox,
    MmsAs::insConf, MmsAs::avlConf,
    MmsAs::insG729, MmsAs::avlG729 
  };

  const int countcount = sizeof(counts) / sizeof(int);

  for(int i=0; i < countcount; i++)         // For each resource count ...
  {
      int   buflen = PAIRBUFLEN;            // <field name="mediaResources">
      char* fieldNameTag = thisxml->makeFieldName(fieldName);
                                            // Concat name/space/value
      int   length = thisxml->makeNameValuePair
           (MmsAppMessageX::mediaResourceNames[ids[i]], counts[i], namevalpair, &buflen);
      if   (buflen > PAIRBUFLEN) continue;

      thisxml->put(fieldNameTag);           // Buffer the xml
      thisxml->put(namevalpair);
      thisxml->put(FIELD_END_TAG);
  }
}


// Old (pre-statistics and licensing) formatQueryMediaResources() and
// formatAvailableMediaResources(). This old code is here in order to verify
// that the newer resources payload is not the culprit in a client crash.


void MmsMqAppAdapter::formatAvailableMediaResources(MmsAppMessage* thisxml)
{
  // Similar to formatQueryMediaResources except formats only available
  // resources that might be exhausted, and inserts same into XML
                                            // Save "mediaResources" name
  const char* fieldName = MmsAppMessageX::serverQueryNames[MmsAppMessageX::MEDIA_RESOURCES];
  const int  PAIRBUFLEN = 128;
  char  namevalpair[PAIRBUFLEN];            
              
  static const int ids[] =                  // Make an id to value mapping
  { MmsAppMessageX::IP_RESOURCES_AVAILABLE,
    MmsAppMessageX::CONF_RESOURCES_AVAILABLE,
    MmsAppMessageX::LOBIT_RESOURCES_AVAILABLE
  };

  const int counts[] =                      
  { 
    MmsAs::avlG711,
    MmsAs::avlConf,
    MmsAs::avlG729 
  };

  const int countcount = sizeof(counts) / sizeof(int);

  for(int i=0; i < countcount; i++)         // For each resource count ...
  {
      int   buflen = PAIRBUFLEN;            // <field name="mediaResources">
      char* fieldNameTag = thisxml->makeFieldName(fieldName);
                                            // Concat name/space/value
      int   length = thisxml->makeNameValuePair
           (MmsAppMessageX::mediaResourceNames[ids[i]], counts[i], namevalpair, &buflen);
      if   (buflen > PAIRBUFLEN) continue;

      thisxml->put(fieldNameTag);           // Buffer the xml
      thisxml->put(namevalpair);
      thisxml->put(FIELD_END_TAG);
  }
}



#else // MMS_USING_LEGACY_RESOURCES_MESSAGING


// Current formatQueryMediaResources() and formatAvailableMediaResources().
// This is the code that should normally be compiled

 
void MmsMqAppAdapter::formatQueryMediaResources(MmsAppMessage* thisxml)
{
  // Queries media server for mediaResources, formats and inserts results
  // to specified xml message. Results are formatted similar to: 
  // <field name="mediaResources">ipResourcesAvailable 64</field>

  // We may possibly wish to write some code to synchronize the various
  // clients heartbeat pulses, if they have the same interval, in order 
  // to reduce server query overhead.
                                            // Save "mediaResources" name
  const char* fieldName = MmsAppMessageX::serverQueryNames[MmsAppMessageX::MEDIA_RESOURCES];
  const int PAIRBUFLEN = 256;
  char  namevalpair[PAIRBUFLEN];                         

  static const int ids[] =                  // Build the id-to-value mapping
  { MmsAppMessageX::IP_RESOURCES_INSTALLED,    MmsAppMessageX::IP_RESOURCES_AVAILABLE, 
    MmsAppMessageX::VOICE_RESOURCES_INSTALLED, MmsAppMessageX::VOICE_RESOURCES_AVAILABLE,
    MmsAppMessageX::CONF_RESOURCES_INSTALLED,  MmsAppMessageX::CONF_RESOURCES_AVAILABLE,
    MmsAppMessageX::LOBIT_RESOURCES_INSTALLED, MmsAppMessageX::LOBIT_RESOURCES_AVAILABLE,
    MmsAppMessageX::TTS_RESOURCES_INSTALLED,   MmsAppMessageX::TTS_RESOURCES_AVAILABLE,
    MmsAppMessageX::CSP_RESOURCES_INSTALLED,   MmsAppMessageX::CSP_RESOURCES_AVAILABLE
  };

  const int counts[] =                      
  { MmsAs::insG711, MmsAs::avlG711,
    MmsAs::insVox,  MmsAs::avlVox,
    MmsAs::insConf, MmsAs::avlConf,
    MmsAs::insG729, MmsAs::avlG729,
    MmsAs::insTTS,  MmsAs::avlTTS,
    MmsAs::insCSP,  MmsAs::avlCSP
  };

  const int countcount = sizeof(counts) / sizeof(int);

  for(int i=0; i < countcount; i++)         // For each resource count ...
  {
      int   buflen = PAIRBUFLEN;            // <field name="mediaResources">
      char* fieldNameTag = thisxml->makeFieldName(fieldName);
                                            // Concat name/space/value
      int length = thisxml->makeNameValuePair
         (MmsAppMessageX::mediaResourceNames[ids[i]], counts[i], namevalpair, &buflen);
      // if (buflen < PAIRBUFLEN) continue; // 20070605 this bug has been here forever
      if (buflen > PAIRBUFLEN) continue;    // 20070605 fix bug

      thisxml->put(fieldNameTag);           // Buffer the xml
      thisxml->put(namevalpair);
      thisxml->put(FIELD_END_TAG);
  }
}



void MmsMqAppAdapter::formatAvailableMediaResources(MmsAppMessage* thisxml)
{
  // Similar to formatQueryMediaResources except formats only available
  // resources that might be exhausted, and inserts same into XML
  // static MMS_RESOURCECOUNTS rescounts;
                                            // Save "mediaResources" name
  const char* fieldName = MmsAppMessageX::serverQueryNames[MmsAppMessageX::MEDIA_RESOURCES];
  const int PAIRBUFLEN = 256;
  char  namevalpair[PAIRBUFLEN];            
              
  static const int ids[] =                  // Build the id-to-value mapping
  { MmsAppMessageX::IP_RESOURCES_AVAILABLE,
    MmsAppMessageX::VOICE_RESOURCES_AVAILABLE,
    MmsAppMessageX::CONF_RESOURCES_AVAILABLE,
    MmsAppMessageX::LOBIT_RESOURCES_AVAILABLE,
    MmsAppMessageX::TTS_RESOURCES_AVAILABLE,   
    MmsAppMessageX::CSP_RESOURCES_AVAILABLE,    
  };

  const int counts[] =                      
  { 
    MmsAs::avlG711,
    MmsAs::avlVox,
    MmsAs::avlConf,
    MmsAs::avlG729,
    MmsAs::avlTTS,
    MmsAs::avlCSP
  };

  const int countcount = sizeof(counts) / sizeof(int);

  for(int i=0; i < countcount; i++)         // For each resource count ...
  {
      int   buflen = PAIRBUFLEN;            // <field name="mediaResources">
      char* fieldNameTag = thisxml->makeFieldName(fieldName);
                                            // Concat name/space/value
      int length = thisxml->makeNameValuePair
         (MmsAppMessageX::mediaResourceNames[ids[i]], counts[i], namevalpair, &buflen);

      // if (buflen < PAIRBUFLEN) continue; // 20070605 this bug has been here forever
      if (buflen > PAIRBUFLEN) continue;    // 20070605 fix bug

      thisxml->put(fieldNameTag);           // Buffer the xml
      thisxml->put(namevalpair);
      thisxml->put(FIELD_END_TAG);
  }
}


#endif // MMS_USING_LEGACY_RESOURCES_MESSAGING
  
   
                                            
int MmsMqAppAdapter::getHeartbeatIntervalParam(MmsAppMessage* xmlmsg) 
{
  // Look in XML for heartbeat param
  const char* paramnameHBI = MmsAppMessageX::paramnames[MmsAppMessageX::HEARTBEAT_INTERVAL]; 
  int   heartbeatseconds = 0;
  if   (xmlmsg->getvaluefor(paramnameHBI)) 
        heartbeatseconds = ACE_OS::atoi(xmlmsg->paramValue());
  return heartbeatseconds; 
}
    

                                            
int MmsMqAppAdapter::getHeartbeatPayloadParam(MmsAppMessage* xmlmsg) 
{
  // Look in XML for heartbeat param
  const char* paramnameHBP = MmsAppMessageX::paramnames[MmsAppMessageX::HEARTBEAT_PAYLOAD];
  const char* paramnameMR  = MmsAppMessageX::serverQueryNames[MmsAppMessageX::MEDIA_RESOURCES];

  return (xmlmsg->getvaluefor(paramnameHBP) // Media resources off/on 
       && strcmp(xmlmsg->paramValue(), paramnameMR) == 0); 
}



void MmsMqAppAdapter::setCommandHeader(MmsServerCmdHeader* cmdHdr, int cmd)
{
  cmdHdr->command  = cmd;
  cmdHdr->sender   = this;
}


                                             
void MmsMqAppAdapter::writeTerminationReasons(MmsAppMessage* xmlmsg, char* flatmap)
{
  // For each termination condition bit returned from the media server, insert a
  // <field name="terminationCondition">name</field> entry to the XML message 

  unsigned long termreasons = getFlatmapTermReason(flatmap);
  if  (termreasons == 0) 
  {    // xmlmsg->putTerminationCondition("normterm");
       return;
  }

  if  (termreasons & TM_MAXTIME)  
       xmlmsg->putTerminationCondition
         (MmsAppMessageX::termcondnames[MmsAppMessageX::MAXTIME]);

  if  (termreasons & TM_MAXDTMF)  
       xmlmsg->putTerminationCondition
         (MmsAppMessageX::termcondnames[MmsAppMessageX::MAXDIGITS]);

  if  (termreasons & TM_MAXSIL)  
       xmlmsg->putTerminationCondition
         (MmsAppMessageX::termcondnames[MmsAppMessageX::SILENCE]);

  if  (termreasons & TM_MAXNOSIL)  
       xmlmsg->putTerminationCondition
         (MmsAppMessageX::termcondnames[MmsAppMessageX::NONSILENCE]);

  if  (termreasons & TM_DIGIT)              // Digitlist or digit 
       xmlmsg->putTerminationCondition
         (MmsAppMessageX::termcondnames[MmsAppMessageX::DIGIT]);

  if  (termreasons & TM_IDDTIME)  
       xmlmsg->putTerminationCondition
         (MmsAppMessageX::termcondnames[MmsAppMessageX::DIGITDELAY]);

  if  (termreasons & TM_METREOS_DIGPATTERN)  
       xmlmsg->putTerminationCondition
         (MmsAppMessageX::termcondnames[MmsAppMessageX::DIGITPATTERN]);
  else
  if  (termreasons & TM_METREOS_AUTOSTOP)  
       xmlmsg->putTerminationCondition("autostop");
  else
  if  (termreasons & TM_METREOS_TIMEOUT)  
       xmlmsg->putTerminationCondition("timeout");

  if  (termreasons & TM_USRSTOP)  xmlmsg->putTerminationCondition("userstop");    
  if  (termreasons & TM_EOD)      xmlmsg->putTerminationCondition("eod");
  if  (termreasons & TM_TONE)     xmlmsg->putTerminationCondition("tone");
  if  (termreasons & TM_ERROR)    xmlmsg->putTerminationCondition("deviceerror");
  if  (termreasons & TM_MAXDATA)  xmlmsg->putTerminationCondition("maxdata");
}  
  
                                                

int MmsMqAppAdapter::isZeros(char* sz)
{
  char*  p = sz;
  while(*p) if (*p++ != '0') return FALSE;
  return TRUE;
}
  


void MmsMqAppAdapter::onHeartbeat() 
{                                
  this->postClientsHeartbeat();          
}



int MmsMqAppAdapter::onError(int errorno)   // MMSM_ERROR handler 
{  
  switch(errorno)
  {
    case MMS_ERROR_LISTENER_DOWN:           // Local queue listener reports down 
         MMSLOG((LM_ERROR,"%s listener is down\n",taskName));           
         // this->shutdownSelf(); // JDL wait for IPC connection           
         m_isStarted = m_isActive = FALSE;
         MMSLOG((LM_INFO,"APMQ MSMQ adapter is out of service\n"));
         break;                              
  }                                         

  return 0;       
}



void MmsMqAppAdapter::shutdownSelf()
{
  MMSLOG((LM_ERROR,"%s forcing exit\n",taskName));
  this->postMessage(MMSM_STOP);   
  mmsSleep(1);
  this->postMessage(MMSM_SHUTDOWN);
}


                                            // Ctor
MmsMqAppAdapter::MmsMqAppAdapter(MmsTask::InitialParams* params): MmsIpcAdapter(params)
{
  this->xmlmsg     = NULL;
  this->heartbeats = 0; 
  this->isExpectingClientToken = 0;
  this->config = (MmsConfig*)params->config;
}



MmsMqAppAdapter::~MmsMqAppAdapter()         // Dtor
{
}



int MmsMqAppAdapter::logCommand(const int command, const char* cmd, const ACE_Log_Priority pri)
{
  MMSLOG((pri,"%s command is %s\n",taskName,cmd));
  return command;
}



void MmsMqAppAdapter::shutdownMediaServer()
{
  MMSLOG((LM_NOTICE,"APMQ application requests media server shutdown\n"));
  this->postServerMessage(MMSM_SERVERCONTROL, MMS_SERVERCTRL_SHUTDOWN);
}



void MmsMqAppAdapter::onServerPush(MmsMsg* msg) 
{                                
  // push data here
  this->pushServerData(msg);
}


