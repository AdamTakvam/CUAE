//
// mmsProtocolAdapter.cpp
//
#include "StdAfx.h"
#pragma warning(disable:4786)
#include "mmsProtocolAdapter.h"
#include "mmsParameterMap.h"
#include "mmsCommandTypes.h"
#include "dxtables.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION



int MmsIpcAdapter::handleMessage(MmsMsg* msg)           
{   
  switch(msg->type())
  { 
    case MMSM_HEARTBEAT: 
         if  (m_isActive)
              onHeartbeat();
         break;

    case MMSM_SERVERCMD_RETURN:             // Return from server command
         onServerCommandReturn(msg);
         break;

    case MMSM_DATA:                         // Adapter protocol data
         onData((void*)msg->param());
         break;

    case MMSM_ACK:                          // Acknowledgement from server
         onAck(msg);
         break; 

    case MMSM_ERROR:
         onError(msg->param());
         break;

    case MMSM_INITTASK:
         onInitTask();
         break;  

    case MMSM_STOP:
         m_isActive=0;
         onStopAdapter();
         break;

    case MMSM_SHUTDOWN:                      
                                            // Tell server to unregister us
         this->postServerMessage(MMSM_UNREGISTER, (long)this);

         this->onShutdown();                // Let implementation clean up 
         this->postMessage(MMSM_QUIT);      // Shut down message pump and exit
         break; 

    case MMSM_SERVERPUSH:
         this->onServerPush(msg);
         break;

    default: return 0;
  } 

  return 1;
}



int MmsIpcAdapter::onProtocolDataReceived(void* protocolData)
{
  // Inspect data received over the network and determine server command
  // represented by that data. Route control to appropriate command parser.

  if  (!m_isActive) return -1;
  int  result = 0;

  int  commandID = this->preparseCommand(protocolData);
  if  (commandID == -1) return -1;

  switch(commandID)
  {
    case COMMANDTYPE_CONNECT:
         this->onCommandConnect(protocolData);
         break;
                
    case COMMANDTYPE_DISCONNECT: 
         this->onCommandDisconnect(protocolData);
         break;

    case COMMANDTYPE_HEARTBEAT: 
         this->onCommandHeartbeatAck(protocolData);
         break;

    case COMMANDTYPE_SERVER: 
         this->onCommandServer(protocolData);
         break;
                         
    case COMMANDTYPE_PLAY: 
         this->onCommandPlay(protocolData);
         break;
              
    case COMMANDTYPE_RECORD: 
         this->onCommandRecord(protocolData);
         break;
              
    case COMMANDTYPE_RECORD_TRANSACTION:  
         this->onCommandRecordTransaction(protocolData);
         break;
              
    case COMMANDTYPE_PLAYTONE: 
         this->onCommandPlaytone(protocolData);
         break;
              
    case COMMANDTYPE_RECEIVE_DIGITS: 
         this->onCommandReceiveDigits(protocolData);
         break;

    case COMMANDTYPE_SEND_DIGITS: 
         this->onCommandSendDigits(protocolData);
         break;

    case COMMANDTYPE_STOP_OPERATION: 
         this->onCommandStopMediaOperation(protocolData);
         break;

    case COMMANDTYPE_MONITOR_CALL_STATE: 
         this->onCommandMonitorCallState(protocolData);
         break;

    case COMMANDTYPE_ADJUST_PLAY:  
         this->onCommandAdjustPlay(protocolData);
         break;
              
    case COMMANDTYPE_ASSIGN_VOLADJ_DIGIT:
         this->onCommandAssignVolumeAdjustmentDigit(protocolData);
         break;

    case COMMANDTYPE_ASSIGN_SPEEDADJ_DIGIT: 
         this->onCommandAssignSpeedAdjustmentDigit(protocolData);
         break;

    case COMMANDTYPE_ADJUST_VOLUME:  
         this->onCommandAdjustVolume(protocolData);
         break;

    case COMMANDTYPE_ADJUST_SPEED: 
         this->onCommandAdjustSpeed(protocolData);
         break;

    case COMMANDTYPE_CLEAR_VS_ADJUSTMENTS: 
         this->onCommandClearVolumeSpeedAdjustments(protocolData);
         break;             
              
    case COMMANDTYPE_CONFERENCE_RESOURCES: 
         this->onCommandConferenceResourcesRemaining(protocolData);
         break;  

    case COMMANDTYPE_CONFEREE_SETATTRIBUTE: 
         this->onCommandConfereeSetAttribute(protocolData);                                    
              
    case COMMANDTYPE_CONFEREE_ENABLE_VOL:  
         this->onCommandConfereeEnableVolumeControl(protocolData);
         break;

    case COMMANDTYPE_VOICEREC: 
         this->onCommandVoiceRec(protocolData);
         break;

    default: 
         MMSLOG((LM_ERROR,"%s unrecognized command %d\n",taskName,commandID));
         result = -1;
  }

  return result;
}



void MmsIpcAdapter::onServerCommandReturn(MmsMsg* msg)
{      
  // Routes to the appropriate handler for a command return from server

  char* map = (char*)msg->param();         

  if  (!Mms::isFlatmapReferenced(map,1)) return;     
                                            // If provisional response ...
  if  (getFlatmapRetcode(map) == MMS_COMMAND_EXECUTING      
   && !config->clientParams.respondOnAsyncExecute) 
       return;                              // ... ensure client wants them

  if  (m_isClientConnected)                 // if client connected ...

       this->onServerCommandReturn(map);    // ... give event to client

  else delete[] map;                        // ... otherwise free map memory
}



void MmsIpcAdapter::onServerCommandReturn(char* map)
{ 
  // Routes to the appropriate handler for a command return from server

  int commandID = getFlatmapCommand(map);

  switch(commandID)
  {  
    case COMMANDTYPE_CONNECT:
         onReturnConnect(map);
         break;
                
    case COMMANDTYPE_DISCONNECT: 
         onReturnDisconnect(map);
         break;
                         
    case COMMANDTYPE_PLAY: 
         onReturnPlay(map);         
         break;
              
    case COMMANDTYPE_RECORD: 
         onReturnRecord(map);       
         break;
              
    case COMMANDTYPE_RECORD_TRANSACTION:  
         onReturnRecordTransaction(map);
         break;
              
    case COMMANDTYPE_PLAYTONE: 
         onReturnPlaytone(map);      
         break;

    case COMMANDTYPE_VOICEREC: 
         onReturnVoiceRec(map);      
         break;
              
    case COMMANDTYPE_RECEIVE_DIGITS: 
         onReturnReceiveDigits(map);
         break;

    case COMMANDTYPE_SEND_DIGITS: 
         onReturnSendDigits(map);
         break;

    case COMMANDTYPE_STOP_OPERATION:
         // Server does not generate a return package on a stop media.
         // so this is unused. Adapter will receive the termination event
         // for the media operation which was canceled.
         onReturnStopMediaOperation(map);
         break;

    case COMMANDTYPE_ADJUST_PLAY:
         onReturnAdjustPlay(map);
         break;

    case COMMANDTYPE_MONITOR_CALL_STATE:
         onReturnMonitorCallState(map);
         break;
              
    case COMMANDTYPE_ASSIGN_VOLADJ_DIGIT:   
    case COMMANDTYPE_ASSIGN_SPEEDADJ_DIGIT:  
    case COMMANDTYPE_ADJUST_VOLUME:   
    case COMMANDTYPE_ADJUST_SPEED: 
    case COMMANDTYPE_CLEAR_VS_ADJUSTMENTS: 
         onReturnAdjustments(map);
         break;             
              
    case COMMANDTYPE_CONFERENCE_RESOURCES: 
         onReturnConferenceResourcesRemaining(map);
         break;                          
              
    case COMMANDTYPE_CONFEREE_SETATTRIBUTE:  
         onReturnConfereeSetAttribute(map);
         break;
              
    case COMMANDTYPE_CONFEREE_ENABLE_VOL:  
         onReturnConfereeEnableVolumeControl(map);
         break;

    default: MMSLOG((LM_ERROR,"%s bad command ID %d\n",taskName,commandID));
  } 
}


void MmsIpcAdapter::onCommandConnect(void* data) {}
void MmsIpcAdapter::onCommandDisconnect(void* data) {}
void MmsIpcAdapter::onCommandServer(void* data) {}
void MmsIpcAdapter::onCommandPlay(void* data) {}
void MmsIpcAdapter::onCommandRecord(void* data) {}
void MmsIpcAdapter::onCommandRecordTransaction(void* data) {}
void MmsIpcAdapter::onCommandPlaytone(void* data) {}
void MmsIpcAdapter::onCommandReceiveDigits(void* data) {}
void MmsIpcAdapter::onCommandSendDigits(void* data) {}
void MmsIpcAdapter::onCommandStopMediaOperation(void* data) { }
void MmsIpcAdapter::onCommandAdjustPlay(void* data) { }
void MmsIpcAdapter::onCommandAssignVolumeAdjustmentDigit(void* data) {}
void MmsIpcAdapter::onCommandAssignSpeedAdjustmentDigit(void* data) {}
void MmsIpcAdapter::onCommandAdjustVolume(void* data) {}  
void MmsIpcAdapter::onCommandAdjustSpeed(void* data) {}
void MmsIpcAdapter::onCommandClearVolumeSpeedAdjustments(void* data) {}
void MmsIpcAdapter::onCommandConferenceResourcesRemaining(void* data) {}
void MmsIpcAdapter::onCommandConfereeSetAttribute(void* data) {}
void MmsIpcAdapter::onCommandConfereeEnableVolumeControl(void* data) {}
void MmsIpcAdapter::onCommandHeartbeatAck(void* data) {}
void MmsIpcAdapter::onCommandMonitorCallState(void* data) {}
void MmsIpcAdapter::onCommandVoiceRec(void* data) {}
 

void MmsIpcAdapter::onReturnConnect(char* map)   {}
void MmsIpcAdapter::onReturnDisconnect(char* map) {}
void MmsIpcAdapter::onReturnPlay(char* map)   {}
void MmsIpcAdapter::onReturnRecord(char* map) {}
void MmsIpcAdapter::onReturnRecordTransaction(char* map) {}
void MmsIpcAdapter::onReturnPlaytone(char* map) {}
void MmsIpcAdapter::onReturnReceiveDigits(char* map) {}
void MmsIpcAdapter::onReturnSendDigits(char* map) {}
void MmsIpcAdapter::onReturnStopMediaOperation(char* map) {}  // Not used
void MmsIpcAdapter::onReturnAdjustments(char* map)   {}
void MmsIpcAdapter::onReturnAdjustPlay(char* map)   {}
void MmsIpcAdapter::onReturnConferenceResourcesRemaining(char* map) {}
void MmsIpcAdapter::onReturnConfereeSetAttribute(char* map) {}
void MmsIpcAdapter::onReturnConfereeEnableVolumeControl(char* map) {}
void MmsIpcAdapter::onReturnMonitorCallState(char* map) {}
void MmsIpcAdapter::onReturnVoiceRec(char* map) {}



void MmsIpcAdapter::onInitTask()
{    
  // The first thing we do on startup is to register ourself with the
  // server manager, so that we can receive broadcasts intended for all
  // adapters, and so that server manager will recognize our command 
  // return packages. 
                                       
  this->postServerMessage(MMSM_REGISTER, (long)this);
}
  


char* MmsIpcAdapter::mapComplete(MmsFlatMapWriter& map, int commandType)
{
  // Convenience method for implementations to invoke after parameter map
  // is complete, to insert the command header and commit the map to an
  // allocated buffer, which is then returned.

  MmsServerCmdHeader commandHeader;         // Make a command header for map  
  return this->mapComplete(map, commandHeader, commandType);
}



char* MmsIpcAdapter::mapComplete
( MmsFlatMapWriter& map, MmsServerCmdHeader& commandHeader, int commandType)
{
  // Convenience method for implementations to invoke after parameter map
  // is complete, to insert the command header and commit the map to an
  // allocated buffer, which is then returned.

  setCommandHeader(&commandHeader, commandType);
                                            // Commit parameter map 
  return this->mapCommit(&map, &commandHeader);
}


                                             
char* MmsIpcAdapter::mapCommit(MmsFlatMapWriter* map, MmsServerCmdHeader* cmdHdr)
{
  // Commits the supplied parameter map and header to a heap buffer.
  // No further copying of parameter data takes place from here on, since the 
  // server reads the map in this format. The reply destination (this pointer), 
  // is embedded in the map's command header (see setCommandHeader()) 
  // The server manager will use this information to route the map and 
  // server reply back to the adapter that dispatched it. 

  const int extraHeaderSize = sizeof(MmsServerCmdHeader);
  const int mapbufsize = map->length() + extraHeaderSize;
                                             
  char* buf = new char[mapbufsize];
                                            // Commit the map to char buffer
  int lengthWritten = map->marshal(buf, extraHeaderSize, (char*)cmdHdr);

  ACE_ASSERT(mapbufsize == lengthWritten);

  return buf;                      
}



int MmsIpcAdapter::postServerCommand(char* flatmap)
{
  // Posts a parameter map to server's work queue via MMSM_SERVERCMD 

  this->serverManager->postMessage(MMSM_SERVERCMD, (long)flatmap);  
  
  return 0;
} 


                                            
int MmsIpcAdapter::postServerMessage(unsigned int type, long param)
{
  // Post server a notification message.  

  return this->serverManager->postMessage(type, param);   
}



void MmsIpcAdapter::onCommandComplete(char* map)
{
  // Last stop for a command returning from server.  
   
  this->freeMapMemory(map);
}



void MmsIpcAdapter::freeMapMemory(void* heapmem)
{ 
  if  (config->diagnostics.flags & MMS_DIAG_LOG_MAP_LIFETIME)
       MMSLOG((LM_DEBUG,"ADAP free map memory %x\n",heapmem));

  if  (Mms::canDeref(heapmem))
        delete[] heapmem;

  else MMSLOG((LM_ERROR,"ADAP invalid map memory %x\n", heapmem));
}



void MmsIpcAdapter::onAck(MmsMsg* msg)
{                                           
  switch(this->ackState)
  {
    case ACK_INITIAL:
         // Acknowledgement from server manager that we're registered.
         // We fire a start event to implementation
         if  (m_isActive) break;
         else m_isActive = TRUE;

         this->onStartAdapter();
         break;
  }
}



int MmsIpcAdapter::onError(int whicherror)         
{
  // Implementation overrides this if it needs to handle MMSM_ERROR
  return 0;
}
      

                                            
int MmsIpcAdapter::onThreadStarted()   // Thread startup hook
{ 
  MMSLOG((LM_INFO,"%s thread %t started at priority %d\n", 
          taskName, osPriority)); 
  return 0;
} 
  

                                            
int MmsIpcAdapter::close(unsigned long)     // Thread exit hook
{
  MMSLOG((LM_DEBUG,"%s thread %t exit\n",taskName));
  return 0;
}



int MmsIpcAdapter::getMediaFullpath(char* fullpath, char* subpath, char* basepath, 
  MmsLocaleParams& ld, const int isIgnoreLocale)
{
  // Given a partial or full path to a file, and an optional base path,
  // return the full path to the media file. If partial path is a full
  // path, return that, otherwise return concatenation of basepath and 
  // partial path

  if (!fullpath || !subpath) return -1;

  const int isUsersFullPathToAudioFile = subpath[1] == ':';

  if (isUsersFullPathToAudioFile)
  {   // Full path supplied by client, no need to construct a path
      ACE_OS::strcpy(fullpath, subpath);
      return 0;
  }

  const static char* defaultDefaultLocale = "en-US";
  const static char  separator[2] = { ACE_DIRECTORY_SEPARATOR_CHAR_A, '\0' };

  const char* basePath = subpath[1] == ':'? "": basepath? basepath: "";        

  ACE_OS::strcpy(fullpath, basePath);

  if (*fullpath) ensureTrailingSlash(fullpath);    

  if (*subpath == ACE_DIRECTORY_SEPARATOR_CHAR_A) ++subpath;

  if (!isIgnoreLocale)
  {
      char* configLocale  = config->serverParams.defaultLocale;
      char* configAppNam  = config->serverParams.defaultAppName;

      char* defaultLocale = strlen(configLocale)? configLocale: (char*)defaultDefaultLocale;
      char* defaultAppNam = strlen(configAppNam)? configAppNam: "";

      char* appnam = ld.appname? ld.appname: defaultAppNam;
      char* locale = ld.locale?  ld.locale:  defaultLocale;

      ACE_OS::strcat(fullpath, appnam); 
      ACE_OS::strcat(fullpath, separator);
      ACE_OS::strcat(fullpath, locale); 
      ACE_OS::strcat(fullpath, separator);
  }

  ACE_OS::strcat(fullpath, subpath);
  return 0;
}


                                            // Diagnostic log display 
void MmsIpcAdapter::showTerminationReasons(char* flatmap)
{
  unsigned long termreasons = getFlatmapTermReason(flatmap);

  m_count=0;
  if  (termreasons == 0)         {showTerminationReason("normal"); return;}
  if  (termreasons & TM_MAXTIME)  showTerminationReason("timeout");
  if  (termreasons & TM_MAXDTMF)  showTerminationReason("max digits");
  if  (termreasons & TM_MAXSIL)   showTerminationReason("max silence");
  if  (termreasons & TM_MAXNOSIL) showTerminationReason("max nonsilence");
  if  (termreasons & TM_LCOFF)    showTerminationReason("loop current");
  if  (termreasons & TM_IDDTIME)  showTerminationReason("interdig delay");
  if  (termreasons & TM_DIGIT)    showTerminationReason("digit");
  if  (termreasons & TM_USRSTOP)  showTerminationReason("application stop");
  if  (termreasons & TM_EOD)      showTerminationReason("end of data");
  if  (termreasons & TM_TONE)     showTerminationReason("tone");
  if  (termreasons & TM_ERROR)    showTerminationReason("device error");
  if  (termreasons & TM_MAXDATA)  showTerminationReason("max data");
  if  (m_count == 0)              showTerminationReason("not recognized");
}  
  


void MmsIpcAdapter::showTerminationReason(char* reason)
{
  MMSLOG((LM_INFO,"%s termination reason: %s\n",taskName,reason)); m_count++;
}


                                            // ctor
MmsIpcAdapter::MmsIpcAdapter(MmsTask::InitialParams* params): MmsBasicTask(params)
{
  ACE_ASSERT(params->user && params->config);
  this->config   = (MmsConfig*)params->config;
  this->logger   = (MmsLogger*)params->logCallback;
  this->reporter = params->reporter;
  this->ackState = m_isActive = m_isStarted = m_isClientConnected = 0;
  this->serverManager = (MmsServerManager*)params->user;
}

