//
// MmsSession.cpp
//
// Session operation object command handlers
//
#include "StdAfx.h"
#pragma warning(disable:4786)
#ifdef MMS_WINPLATFORM
#include <minmax.h>
#endif
#include "mmsSession.h"
#include "mmsParameterMap.h"
#include "mmsMediaEvent.h"
#include "mmsSessionManager.h"
#include "mmsCommandTypes.h"
#include "mmsAudioFileDescriptor.h"
#include "mmsTts.h"
#include "mmsAsr.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

// Command handlers are normally executed in a service thread context.
// These will return zero if command completed normally and synchronously,
// MMS_COMMAND_EXECUTING (1) if command is executing asynchronously; otherwise
// an error indicator to be returned to client adapter (4 or greater).


int MmsSession::Op::handleConnect(MmsCurrentCommand& cmd)              
{
  // Handle a session connection, and initial conference hookup if so indicated.
  // IP start media is executed asynchronously but can be configured otherwise.

  // A number of options exist on connect. We can start remote session or not, 
  // sync or async; and connect a new or existing connection with a new or 
  // existing conference. Options are indicated by the presence or absence
  // of connection ID, conference ID, and port parameters, and whether those
  // parameters if specified are zero-valued or not. Session manager has made
  // the determination and set flags in parameter map to so indicate -- see 
  // decision table in session manager for specifics.
 
  char* conxID = 0, *confID = 0, *ipAddr = 0, *pport = 0, *paltc = 0; 
  char* premoteattrs = 0, *plocalattrs = 0, *psestimeout = 0;
  int   cmdtimeoutms=0, sestimeoutsecs=0, iplen=0, result=0, length=0;
  unsigned int remoteattrs=0, localattrs=0, isHalfConnect = 0;
                                            // Identify parameters supplied
  length = parameterMap.find(MMSP_CONFERENCE_ID,&confID);
  iplen  = parameterMap.find(MMSP_IP_ADDRESS,   &ipAddr);
  length = parameterMap.find(MMSP_PORT_NUMBER,  &pport);
  length = parameterMap.find(MMSP_SESSION_TIMEOUT_SECS,   &psestimeout);
  length = parameterMap.find(MMSP_REMOTE_CONX_ATTRIBUTES, &premoteattrs);
  length = parameterMap.find(MMSP_LOCAL_CONX_ATTRIBUTES,  &plocalattrs);
  length = parameterMap.find(MMSP_ALTERNATE_CODER, &paltc);                                           

  const int connectionID = getFlatmapConnectionID(this->flatmap());
  const int conferenceID = confID?  *((unsigned int*)confID): 0;
  const int port     = pport? *((int*)pport): 0; 
  const int altcoder = paltc? *((int*)paltc): 0; 
  if (premoteattrs) remoteattrs = *((unsigned int*)premoteattrs);
  if (plocalattrs)  localattrs  = *((unsigned int*)plocalattrs);  

  if (confID && session->isInConference())  // Disallow conference ID
      return MMS_ERROR_ALREADY_CONNECTED;   // when already conferenced   
                                            
  if  (psestimeout) sestimeoutsecs = *((int*)psestimeout);
  if  (session->sessionTimeoutSecs == 0)    // Assign session timeout once only
       session->onAssignSessionTimeout(sestimeoutsecs); 

  this->cacheConnectionTag(); 
                                   
  MmsDeviceIP* deviceIP = session->ipDevice(); 
  const int localIpLen  = ACE_OS::strlen(deviceIP->localIP()); 
                                            // Check if conx is to self-listen 
  const int mapflags = getFlatmapFlags(this->flatmap());
  if  (mapflags & MmsServerCmdHeader::IS_REXMIT_CONNECTION)
       session->flags |= MMS_SESSION_FLAG_RETRANSMIT_CONNECTION;

  if ((mapflags & MmsServerCmdHeader::IS_EXISTING_CONNECTION) == 0)
       isHalfConnect = (mapflags & MmsServerCmdHeader::IS_NOSTARTMEDIA) ? 1: 0;  

  if  (isHalfConnect)
  {
       // Half-connect retrieves local IP and port w/o starting remote session 
       if (!deviceIP || !ipAddr || !pport) return MMS_ERROR_TOO_FEW_PARAMETERS;

       *((int*)(pport)) = deviceIP->localPort();
       ACE_OS::memset(ipAddr, 0, MMS_SIZEOF_IPADDRESS);   
       ACE_OS::memcpy(ipAddr, deviceIP->localIP(), min(localIpLen, iplen-1));

       // LICX G711OUT (2 of 3) G711- (1 of 2): burn a G711 resource license
       if (MmsAs::g711(MmsAs::RESX_ISZERO, MmsAs::RESX_RESERVE)) 
           return this->raiseG711ExhaustedAlarm();
                                            // Mark G.711 license consumed
       session->flags |= MMS_SESSION_FLAG_G711_RESOURCE_SPENT;

       session->setDefaultConnectAttributes(remoteattrs, localattrs, TRUE); 
                                            // Reserve coder if LBR 
       if (-1 == session->verifyCoderAvailabilityEx(TRUE, TRUE, flatmap()))
           return MMS_ERROR_RESOURCE_UNAVAILABLE;

       session->flags |= MMS_SESSION_FLAG_HALF_CONNECTED;

       return 0;
  }

  // Start remote IP session if indicated. Situations where we do not are:
  // a) half-connect with return of local IP/port, handled above; and 
  // b) connecting an already-connected session to a conference 

  const int isStartingRemoteSession 
        = !(isFlatmapFlagSet(this->flatmap(), MmsServerCmdHeader::IS_NOSTARTMEDIA));

  if  (isStartingRemoteSession) 
  {                                          
       if (0 != (result = session->editIpAddress(ipAddr))) return result;

       const unsigned int currRemoteAttrs = session->remoteIpAttrs;
                                            // Default attrs if not supplied
       session->setDefaultConnectAttributes(remoteattrs, localattrs); 
                                            // Are we connecting async?
       int  mode = Config()->serverParams.connectAsync? 
                   MmsMediaDevice::ASYNC: MmsMediaDevice::SYNC; 

       if  (deviceIP->isStarted())          // If IP session already started ... 
       {                                    // If IP/port/attribute change ...
            if  (session->isRemoteMediaChange(ipAddr, port, remoteattrs, currRemoteAttrs))
            {                               // ... stop IP & force sync connect
                 session->holdRemoteSession(this);  
                 mode = MmsMediaDevice::SYNC;                     
            }                               // If media change parameters passed 
            else                            // which are the same as existing ...
            if  (this->isMediaParameterChangeRequest())
            {                               // ... return OK 
                 putMapHeader(setReasonCode, MMS_REASON_NO_CHANGE);
                 return 0;
            }                               
            else return MMS_ERROR_ALREADY_CONNECTED;
       }                                     
       else                                 // Not previously half-connected?
       if  (0 == (session->flags & MMS_SESSION_FLAG_G711_RESOURCE_SPENT))
       {    
            // LICX G711OUT (3 of 3) G711- (2 of 2): burn a G711 resource license
            if (MmsAs::g711(MmsAs::RESX_ISZERO, MmsAs::RESX_RESERVE)) 
                return this->raiseG711ExhaustedAlarm();  

            session->flags |= MMS_SESSION_FLAG_G711_RESOURCE_SPENT;
       }    
                                            
       if  (isSynchronous(mode))
            this->commandTimeoutMs = 0;  
       else
       if  (registerForAsyncEventNotification(IPMEV_STARTMEDIA, 1) == -1)  
            return MMS_ERROR_EVENT_REGISTRATION;               
                           
       if  (session->isCoderReserved()) 
            mode |= MmsMediaDevice::NOAQUIRE_RESOURCE;
                                            // Set command timeout and
       const int defaultCommandTimeout      // transition session state
               = Config()->serverParams.commandTimeoutMsecsConnect;   

       this->onCommandServiceStart(defaultCommandTimeout);  
       this->onCommandEnter(isAsync(mode));

                                            // Start remote session
       result = deviceIP->start(ipAddr, port, remoteattrs, localattrs, mode); 


       if  (result == MMS_ERROR_RESOURCE_UNAVAILABLE)
       {                                    // Out of LBR coders
            putMapHeader(setReasonCode, MMS_REASON_CODER_NOT_AVAILABLE);
 
            if (altcoder)                   // Try alternate coder if requested
            {   session->replaceCoderBits(remoteattrs, localattrs, altcoder);           
                result = deviceIP->start(ipAddr, port, remoteattrs, localattrs, mode); 
            }
            if (result == MMS_ERROR_RESOURCE_UNAVAILABLE) return result;
       }

       if  (result == -1)                   // Cancel pending event if error
       {    if (isAsync(mode)) cancelAsyncEventNotification(IPMEV_STARTMEDIA, TRUE);
            return MMS_ERROR_DEVICE;
       }
                                            // At this point we're connected
                                            // Return local IP and port
       *((int*)(pport)) = deviceIP->localPort();   
       ACE_OS::memset(ipAddr, 0, MMS_SIZEOF_IPADDRESS);   
       ACE_OS::memcpy(ipAddr, deviceIP->localIP(), min(localIpLen, iplen-1));

       session->resumeMediaIfReconnect();    
                                            // If async connect, go wait for event
       if  (isAsync(mode)) 
       {
            session->flags |= MMS_SESSION_FLAG_IS_ASYNC_CONNECT_PENDING;
            return MMS_COMMAND_EXECUTING;
       }
  }
       
                                            
  if  (this->isConferenceOperation())       // If conferencing, hook it up
       result = session->handleConferenceConnect(this->parameterMap);
   
  return result == -1? MMS_ERROR_RESOURCE_UNAVAILABLE: 0; 
}



int MmsSession::Op::handleDisconnect(MmsCurrentCommand& cmd)           
{                            
  // Handle a disconnect command. Disconnect may be from session, 
  // conference, or both

  this->onCommandServiceStart(); 
  this->onCommandEnter();
  
  char* map = this->flatmap();
  if (!Mms::canDeref(map)) return -1;

  if  (session->isRfc2833Registered())
       this->onRfc2833Disconnect();
                                            
  if  (this->isConferenceOperation())       // If disconnect conf only ...
                                            // disconnect from conference
       session->handleConferenceDisconnect();
                                          
  else session->releaseResources();         // Stop IP, release vox or conf
   
  return 0;
}



int MmsSession::handleConferenceConnect(MmsFlatMapReader& parameterMap)    
{
  // Sets up session to join a conference and hook up the IP to a conferencing
  // resource. Conference may be new or existing. Conferee may be an active 
  // participant or monitor. The media server provides access to the monitoring
  // functionality transparently at connect and disconnect times, upon recog-
  // nition of confereeAttributes & MMS_CONFEREE_MONITOR bit.
  // Conference ID may be supplied as zero, indicating new conference. Actual
  // conference ID is returned in map param. 

  if (this->isReconnect())                  // Reconnect on IP switch
      return this->reconnectToConference(NULL);
                                            // Also checked in handleConnect
  if (this->isInConference()) 
      return MMS_ERROR_ALREADY_CONNECTED;     
   
  char* pattrsconference=0, *pattrsconferee=0, *pconfID=0;
  int result, mmsAttrsConference=0, mmsAttrsConferee=0, mmsFlags=0, conferenceID=0;
  unsigned int hmpAttrsConference=0, hmpAttrsConferee=0;

  result = parameterMap.find(MMSP_CONFERENCE_ID,         &pconfID);
  result = parameterMap.find(MMSP_CONFERENCE_ATTRIBUTES, &pattrsconference);
  result = parameterMap.find(MMSP_CONFEREE_ATTRIBUTES,   &pattrsconferee);

  if (pattrsconference) mmsAttrsConference = *((int*)pattrsconference);
  if (pattrsconferee)   mmsAttrsConferee   = *((int*)pattrsconferee);
  if (pconfID)          conferenceID       = *((int*)pconfID);

  this->encodeConferenceAttributes(mmsAttrsConference, mmsAttrsConferee,
                                   hmpAttrsConference, hmpAttrsConferee);
                                            // Look up connect operation
  MmsSession::Op* operation = this->getOperation(parameterMap, TRUE);
  if (operation == NULL) return MMS_ERROR_SERVER_INTERNAL;

  if (mmsAttrsConferee & MMS_CONFEREE_MONITOR)
      mmsFlags |= MmsConferenceManager::MONITOR;

  MmsConferenceManager* conferenceMgr = sessionMgr->conferenceManager();

  result = conferenceMgr->joinConference
          (operation, conferenceID, mmsFlags, hmpAttrsConferee, hmpAttrsConference);

  if (result != -1)                         // Return conference ID
      operation->putMapHeader(setParam, this->confInfo.id);

  return result;
}


                                            
int MmsSession::handleConferenceDisconnect()   
{   
  // Move a session out of a conference
  // See mmsNotes.txt for conference teardown logic diagram

  MmsConferenceManager* conferenceMgr = sessionMgr->conferenceManager();
  const int conferenceID = this->confInfo.id;

  if (!conferenceID || !conferenceMgr->isActiveConference(conferenceID)) 
      return 0;
                                            
  this->resetSessionTimer();                // Give session a new clock  
                                             
  int result = conferenceMgr->leaveConference(this, conferenceID); 

  return result;
}



int MmsSession::Op::handlePlay(MmsCurrentCommand& cmd) 
{ 
  // Play a specified list of files, which are in the specified format, 
  // at the specified bit rate, terminating on the specified list of conditions.

  int  result = 0;

  this->session->clearDigitCache();

  MmsDeviceVoice* deviceVoice = this->voiceDevice(TRUE, TRUE);
  if  (!deviceVoice) return MMS_ERROR_RESOURCE_UNAVAILABLE;
                                             
  if (session->isConferenceParty())         // If playing to single conferee ...
  {                                         //... switch to voice listen
      result = session->switchFromConferenceToVoice(deviceVoice);
      if (result) return result;
  }  
  else
  {   if (session->isDigitLingering())
          session->handleDigitLingering(deviceVoice, FULLDUPLEX);

      if (session->isUtilitySession())      // If playing to conference ...
      {                                     // ... temporarily join conference
          MmsConferenceManager* conferenceMgr = sessionManager()->conferenceManager();
          const int confslot  = conferenceMgr->joinUtilitySession(this->session);
          if  (confslot == -1)  return MMS_ERROR_NO_SUCH_CONFERENCE;    
      }
  }

  const int defaultCommandTimeout = Config()->serverParams.commandTimeoutMsecsPlay;
  this->onCommandServiceStart(defaultCommandTimeout); 

  MmsVolumeSpeedEncoder volSpeedCoder;      // Get volume/speed parameters if any
  const int volSpeedCount = this->getVolumeSpeedParameters(volSpeedCoder);

  MmsLocaleParams localeParams;              
  this->getLocaleParameters(localeParams);  // Get locale parameters   

                                            // Get TTS playlist entries if any
  this->ttsData = new MmsTtsSessionData(Config(), MMS_MAX_PLAYLIST_SIZE);                   
  int playfileCount = this->preparePlaylist();
  if (playfileCount == 0) return MMS_ERROR_TOO_FEW_PARAMETERS;

                                            // Render TTS sound files if any                                            
  if ((this->ttsData->ttsStrings)->count() > 0)           
  {
      MmsTtsRenderData ttsRenderData(localeParams.appname, localeParams.locale); 

      result = Tts::instance()->render((void*)this, &ttsRenderData);
      if (result != 0) return result < 0? MMS_ERROR_DEVICE: result;
  }

  
  MMSPLAYFILEINFO fileinfo(localeParams); 
  fileinfo.isPlay = TRUE; 
                                            // Open files in playlist
  playfileCount = this->openPlaylist(deviceVoice, &fileinfo);
  if (playfileCount == -1)  return MMS_ERROR_FILEOPEN;
  if (playfileCount ==  0)  return MMS_ERROR_TOO_FEW_PARAMETERS;

  MMS_DV_TPT_LIST terminationConditions;
  int  conditionCount                       // Get termination conditions 
     = this->getTerminationConditionParameters(deviceVoice, terminationConditions); 
  if  (conditionCount == -1) return MMS_ERROR_TERMINATION_CONDITION;                                 
     
  fileinfo.ldata.set(localeParams);                                       
  MmsDeviceVoice::MMS_PLAYRECINFO playinfo; 
                                            // Set play/record file attributes
  if (-1 == this->getPlayRecordParameters(playinfo, fileinfo))
      return MMS_ERROR_SERVER_INTERNAL;
                                            // Ask to be notified of play complete
  if (-1 == registerForAsyncEventNotification(TDX_PLAY))  
      return MMS_ERROR_EVENT_REGISTRATION;

  unsigned int mode = MmsMediaDevice::ASYNC;
  if (this->isDigitBufferCleared())
      mode |= MmsMediaDevice::NOCLEAR;

  this->onCommandEnter(TRUE); 

  if (volSpeedCoder.isParamSet())          // Adjust playback vol and/or speed
      this->adjustPlay(volSpeedCoder, MMS_ADJPLAY_LOG | MMS_ADJPLAY_INHERIT_ADJTYPE);        

  MMSLOG((LM_INFO,"%s op %d begin play 1 of %d\n", logkey, opid, playfileCount));
                                            // Start playing the playlist
  result = deviceVoice->playMultiple(&terminationConditions, &playinfo, mode);

  if (result == -1)  
      cancelAsyncEventNotification(TDX_PLAY);

  return result == -1? MMS_ERROR_DEVICE: MMS_COMMAND_EXECUTING;              
}



int MmsSession::Op::handleRecord(MmsCurrentCommand& cmd)
{ 
  // Record to a specified list of files, which are in the specified format, 
  // at the specified bit rate, terminating on the specified list of conditions, 

  this->session->clearDigitCache();

  int   result = 0;
  const int defaultCommandTimeout = Config()->serverParams.commandTimeoutMsecsRecord;
  this->onCommandServiceStart(defaultCommandTimeout);
 
  MmsDeviceVoice* deviceVoice = this->voiceDevice(TRUE, TRUE, HALFDUPLEX);
  if  (!deviceVoice) return MMS_ERROR_RESOURCE_UNAVAILABLE;

  if (session->isConferenceParty())         // If recording a single conferee ...
  {                                         //... switch to voice listen
      result = session->switchFromConferenceToVoice(deviceVoice, HALFDUPLEX);
      if  (result) return result;
  }
  else
  {
      if (session->isDigitLingering())
          session->handleDigitLingering(deviceVoice, HALFDUPLEX);

      if (session->isUtilitySession())          // If recording to conference ...
      {                                         // ... temporarily join conference
          MmsConferenceManager* conferenceMgr = sessionManager()->conferenceManager();
          const int confslot  = conferenceMgr->joinUtilitySession(this->session);
          if  (confslot == -1)  return MMS_ERROR_NO_SUCH_CONFERENCE;    
      }
  }

  MmsLocaleParams localeParams;              
  this->getLocaleParameters(localeParams);  // Get locale parameters   

  MMSPLAYFILEINFO fileinfo(localeParams);   // Get playlist:

  int  playfileCount = this->openPlaylist(deviceVoice, &fileinfo, TRUE);  

  if  (playfileCount == -1)  return MMS_ERROR_FILEOPEN;
  if  (playfileCount ==  0)  return MMS_ERROR_TOO_FEW_PARAMETERS;


  MMS_DV_TPT_LIST terminationConditions;
  int  conditionCount                        
     = this->getTerminationConditionParameters(deviceVoice, terminationConditions); 
  if  (conditionCount == -1) return MMS_ERROR_TERMINATION_CONDITION;

  MmsDeviceVoice::MMS_PLAYRECINFO recinfo;  // Set play/record file attributes

  this->getPlayRecordParameters  (recinfo, fileinfo, FALSE);    


  if  (registerForAsyncEventNotification(TDX_RECORD) == -1)  
       return MMS_ERROR_EVENT_REGISTRATION;

  unsigned int mode = MmsMediaDevice::ASYNC;

  if (this->isDigitBufferCleared()) mode |= MmsMediaDevice::NOCLEAR;

  this->onCommandEnter(TRUE);  
 
                                            // Start recording
  result = deviceVoice->recordMultiple(&terminationConditions, &recinfo, mode);

  if  (result == -1)                         
       cancelAsyncEventNotification(TDX_RECORD);
                                            // Write companion file
  else session->writeDescriptorFile(recinfo, &fileinfo);  
                                            
  return result == -1? MMS_ERROR_DEVICE: MMS_COMMAND_EXECUTING;              
}



int MmsSession::Op::handleRecordTransaction(MmsCurrentCommand& cmd) 
{ 
  // Record from two IP streams to a specified list of files, yada yada,  
  // terminating on the specified list of conditions.

  #if(0)
  const int defaultCommandTimeout 
      = Config()->serverParams.commandTimeoutMsecsRecordTransaction;
  this->onCommandServiceStart(defaultCommandTimeout);

  MmsDeviceVoice* deviceVoice = this->voiceDevice(TRUE, TRUE);
  if (!deviceVoice) return MMS_ERROR_RESOURCE_UNAVAILABLE;

  char* pConxID = NULL;
  int result = parameterMap.find(MMSP_CONNECTION_ID, &pConxID);
  if (pConxID == NULL) return MMS_ERROR_TOO_FEW_PARAMETERS;
  int secondConnectionID = *((int*)pConxID);

  MmsSession* secondSession                 // Look up second session
     = this->sessionManager()->sessionPool()->findByConnectionID(secondConnectionID);
  if (secondSession  == NULL) return MMS_ERROR_NO_SUCH_CONNECTION;

  MmsDeviceIP* secondipDevice = secondSession->ipDevice();
  if (secondipDevice == NULL) return MMS_ERROR_NO_SUCH_CONNECTION;
   
  MMSPLAYFILEINFO fileinfo;
  int playfileCount = this->openPlaylist(deviceVoice, &fileinfo, TRUE);
  if (playfileCount == -1)  return MMS_ERROR_FILEOPEN;
  if (playfileCount ==  0)  return MMS_ERROR_TOO_FEW_PARAMETERS;

  MMS_DV_TPT_LIST terminationConditions;
  int  conditionCount                        
     = this->getTerminationConditionParameters(deviceVoice, terminationConditions); 
  if  (conditionCount == -1) return MMS_ERROR_TERMINATION_CONDITION;                                            
   
  MmsDeviceVoice::MMS_PLAYRECINFO recinfo;  // Set play/record file attributes
  this->getPlayRecordParameters  (recinfo, fileinfo, FALSE);

  if  (registerForAsyncEventNotification(TDX_RECORD) == -1)  
       return MMS_ERROR_EVENT_REGISTRATION;

  unsigned int mode = MmsMediaDevice::ASYNC;
  if (this->isDigitBufferCleared()) mode |= MmsMediaDevice::NOCLEAR;

  this->onCommandEnter(TRUE);   

  result = deviceVoice->recordTransaction(session->ipDevice(), secondipDevice,    
          &terminationConditions, &recinfo, mode);

  if  (result == -1)  
       cancelAsyncEventNotification(TDX_RECORD);
  else session->writeDescriptorFile(recinfo, &fileinfo);  
                                           
  return result == -1? MMS_ERROR_DEVICE: MMS_COMMAND_EXECUTING; 
  #endif

  return MMS_ERROR_NO_SUCH_COMMAND;   
}



int MmsSession::Op::handleSendDigits(const char* flatmap, const int concurrencymode) 
{   
  // Send digits over the IP connection and bus. If executed on client request,
  // this method is invoked directly by session manager, and not assigned to
  // a service pool thread. We therefore do not do the usual session state 
  // transitions here. If this is executed async, we submit and forget, 
  // since we do not currently register for or handle this event. We do however
  // generate the normal client return result message.

  // This operation's parameter map is the parameter set for the currently 
  // executing command to which this digit event is being supplied, such as
  // an announcement play; therefore we have passed in the parameter map that 
  // arrived with this sendDigits command, in order to extract the digit string.

  // Prevent concurrent inline operations, e.g. interleaved sendDigits 
  ACE_Guard<ACE_Thread_Mutex> x(session->extopCriticalSection);

  MmsDeviceIP* deviceIP = session->ipDevice();
  if  (!deviceIP) return MMS_ERROR_DEVICE;
  const int sid   = session->sessionID();
  char* newdigits = NULL;
  int   numdigits = 0, result = 0;                       

  setFlatmapSessionID(flatmap, sid);
                                            // Examine sendDigits parameters
  MmsFlatMapReader   nonresidentParameters(flatmap);
  int  paramlength = nonresidentParameters.find(MMSP_DIGITLIST, &newdigits);

  if  (newdigits && paramlength) 
       numdigits  = ACE_OS::strlen(newdigits);
  if  (numdigits == 0) return MMS_ERROR_TOO_FEW_PARAMETERS;
 
  session->patternMatchDigitsEx(newdigits);

  if  (session->isStreaming())
  {
       // Currently the only streaming operation is voice rec. A digit arriving
       // on a session doing voice recognition presumably indicates that a user
       // has chosen to respond via keypad rather than voice, and we therefore
       // end the voice rec operation.
       MMSLOG((LM_DEBUG,"%s digits '%s' preempts streaming operation\n", 
               logkey, newdigits)); 

       result = session->stopStreamingOperation();

       // Fall thru to send out received digit if no error.
       if (result == -1) return MMS_ERROR_NO_SUCH_OPERATION;
  }

  // The assumption is that we are executing in the session manager thread here,
  // since send digits is a frequent, fast, out of band operation. If the media
  // operation to which the digit applies, e.g. play, is currently listening up
  // its voice resource to its IP, the digit operation could occur while listen
  // state is incomplete, were we not to wait for the listens to complete. This
  // lock also synchronizes access to the voice device's digit buffer such that
  // a service thread can't modify the digit buffer concurrently with our digit.
  // It also ensures that the voice resource is not released while lock is held.

  const int voiceDeviceCount = session->lockVoxAll(TRUE);
  
  MMSLOG((LM_DEBUG,"%s sending '%s' via IP%d to %d vox\n", 
          logkey, newdigits, deviceIP->handle(), voiceDeviceCount));  

  if (-1 == deviceIP->sendDigits(newdigits, numdigits, concurrencymode)) 
      result = MMS_ERROR_DEVICE;  
  else
  {
      this->session->stampSendDigits();

      if (voiceDeviceCount == 0)            // No vox attached -- cache digits
      {
          if (-1 == session->cacheDigits(newdigits, numdigits))
          {   
              MMSLOG((LM_ERROR,"%s op %d vox device not ready\n",logkey,opid));
              return MMS_ERROR_RESOURCE_UNAVAILABLE; 
          }
          else return 0;       
      }

      if (this->session->getNumActiveOperations(1))
      {
          if (-1 == session->cacheDigits(newdigits, numdigits))
              return MMS_ERROR_RESOURCE_UNAVAILABLE; 
      }
  }

  const int unlockedDeviceCount = session->lockVoxAll(FALSE);

  return result;
}



int MmsSession::Op::handleReceiveDigits(MmsCurrentCommand& cmd) 
{ 
  // Begin collecting digits, continuing until a termination condition fires.
  // Digits are returned to client in a reserved parameter map area, if one
  // was supplied. See handleEventVoxDigitsReceived for details.

  const int defaultCommandTimeout = Config()->serverParams.commandTimeoutMsecsGetDigits;
  this->onCommandServiceStart(defaultCommandTimeout);
                                            // Half-duplexed vox
  MmsDeviceVoice* deviceVoice = this->voiceDevice(TRUE, TRUE, HALFDUPLEX); 
  if (!deviceVoice) return MMS_ERROR_RESOURCE_UNAVAILABLE;  
                   
  MMS_DV_TPT_LIST terminationConditions;

  int  conditionCount                        
     = this->getTerminationConditionParameters(deviceVoice, terminationConditions); 

  if  (conditionCount == -1) return MMS_ERROR_TERMINATION_CONDITION;

  int terminatingCondition = 0, patternMatched = 0;

  if  (this->flags & MMS_OP_FLAG_DIGPATTERN_TERM_SET)    
  {
       // Ordinarily HMP will immediately check if a termination condition is met.
       // Since the digit patter termination condition is one that we, not HMP,
       // define and monitor, we check to see if that condtion has already been 
       // met, by executing a synchronous getdigits. If met, we set the flag which 
       // will cause this command to be terminated immediately after the provisional 
       // result has been fired.

       // Client could supply termination conditions in addition to digitpattern.
       // We therefore must run a synchronous getdigits with all supplied
       // termination conditions, plus a near-instantaneous TM_MAXTIME condition,
       // replacing caller's TM_MAXTIME if caller indeed specified one.
       // If the getdigits terminates on maxtime, we know that client's conditions
       // were met, so we do not need to wait for a subsequent async getdigits.
       
       char* digitsAlreadyCollected = this->getDigitsImmediate
          (deviceVoice, &terminationConditions, &terminatingCondition);
  
       if (digitsAlreadyCollected && this->patternMatchDigits(digitsAlreadyCollected))
           patternMatched = TRUE;
       else 
       if (this->session->digitCacheSize() > 0 
        && this->patternMatchDigits(this->session->digitCache))
           patternMatched = TRUE;

       if (patternMatched)
       {  
           MMSLOG((LM_INFO,"%s op %d pattern matched on '%s'\n",logkey,opid,digitlist));
           this->flags |= MMS_OP_FLAG_DIGPATTERN_TERM_MET;                  
       }

       // If some other client termination condition was met during the synchronous
       // check for existing digits matching the pattern, indicate that the command
       // should end and the final result sent at the time the provisional is sent

       if (terminatingCondition != TM_MAXTIME)
       {   
           // Indicate that final result should immediately follow provisional 
           this->flags |= MMS_OP_FLAG_HMP_TERM_MET;
           this->putMapHeader(setTermReason,terminatingCondition);  
           session->logTerminatingCondition(terminatingCondition);  
               
           return MMS_COMMAND_EXECUTING;    // Send provisional regardless                    
       }
  }
                                            // JDL check for digtype/maxdigits met
  if (this->isDigitTerminationPreexisting(deviceVoice, terminationConditions))
      return MMS_COMMAND_EXECUTING;         // If so, send provisional regardless  

  if (registerForAsyncEventNotification(TDX_GETDIG) == -1)  
      return MMS_ERROR_EVENT_REGISTRATION;

  deviceVoice->logDigitBuffer();

  unsigned int mode = MmsMediaDevice::ASYNC;
  if (this->isDigitBufferCleared()) mode |= MmsMediaDevice::NOCLEAR;

  this->onCommandEnter(TRUE); 

                                            // Start receiving digits
  const int result = deviceVoice->receiveDigits(&terminationConditions, mode);

  if (result == -1) 
      cancelAsyncEventNotification(TDX_GETDIG);
                                            
  return result == -1? MMS_ERROR_DEVICE: MMS_COMMAND_EXECUTING;
}



int MmsSession::Op::handlePlaytone(MmsCurrentCommand& cmd)    
{ 
  // Play a 1 or 2-frequency tone of specified frequency/ies and amplitude/s
  // for specified length of time, terminating on specified conditions

  this->session->clearDigitCache();

  int   result = 0;
  const int defaultCommandTimeout = Config()->serverParams.defaultMaxSecsTone * 1000;
  this->onCommandServiceStart(defaultCommandTimeout);

  MmsDeviceVoice* deviceVoice = this->voiceDevice(TRUE, TRUE);
  if  (!deviceVoice) return MMS_ERROR_RESOURCE_UNAVAILABLE;

  if (session->isConferenceParty())        // If playing to single conferee ...
  {                                         //... switch to voice listen
      result = session->switchFromConferenceToVoice(deviceVoice);
      if  (result) return result;
  }
  else
  {
      if (session->isDigitLingering())
          session->handleDigitLingering(deviceVoice, FULLDUPLEX);

      if  (session->isUtilitySession())         // If playing tone to conference ...
      {                                         // ... temporarily join conference
          MmsConferenceManager* conferenceMgr = sessionManager()->conferenceManager();
          const int confslot  = conferenceMgr->joinUtilitySession(this->session);
          if  (confslot == -1)  return MMS_ERROR_NO_SUCH_CONFERENCE;    
      }
  }

  MMS_DV_TPT_LIST terminationConditions;
  int  conditionCount                        
     = this->getTerminationConditionParameters(deviceVoice, terminationConditions); 
  if  (conditionCount == -1) return MMS_ERROR_TERMINATION_CONDITION;
                                            // Get parameters:
  int f[2]={0,0}, a[2]={0,0}, duration=500;

  for(int i=0; i < 2; i++)
  {                                         // Get next occurrence of 
    char* faflatmap = NULL;                 // MMSP_FREQUENCY_AMPLITUDE
    result = parameterMap.find(MMSP_FREQUENCY_AMPLITUDE, &faflatmap, NULL, i+1);
    if  (faflatmap == NULL) break;
                                            
    MmsFlatMapReader famap(faflatmap);      // Extract frequency/amplitude
    char* pf = NULL, *pa = NULL;
    result = famap.find(MMSP_FREQUENCY, &pf);
    result = famap.find(MMSP_AMPLITUDE, &pa);
    if  (pf) f[i] = *((int*)pf);
    if  (pa) a[i] = *((int*)pa);
  }

  char* pduration = NULL;                   // Extract duration
  result = parameterMap.find(MMSP_DURATION, &pduration);
  if  (pduration) 
       duration = *((int*)pduration);           
  duration *= 10;                           

  if  (registerForAsyncEventNotification(TDX_PLAYTONE) == -1)  
       return MMS_ERROR_EVENT_REGISTRATION;

  unsigned int mode = MmsMediaDevice::ASYNC;
  if (this->isDigitBufferCleared())
      mode |= MmsMediaDevice::NOCLEAR;

  this->onCommandEnter(TRUE);  

  result = deviceVoice->playtone(f[0], f[1], a[0], a[1], duration,       
           &terminationConditions, mode);

  if  (result == -1)  
       cancelAsyncEventNotification(TDX_PLAYTONE);
                                            
  return result == -1? MMS_ERROR_DEVICE: MMS_COMMAND_EXECUTING;
} 



int MmsSession::Op::handleVoiceRec(MmsCurrentCommand& cmd)
{
  // Handle recognize voice command

  if (!this->session) return MMS_ERROR_SERVER_INTERNAL;

  this->session->clearDigitCache();
  
  if (!Config()->media.asrEnable)
  {
      MMSLOG((LM_ERROR,"%s ASR services not configured\n", logkey));
      return MMS_ERROR_DEVICE;
  }

  // LICX CSP- (1 of 2)
  // Atomically check CSP resources available count and reserve one if available 
  const int isCspExhausted = MmsAs::csp(MmsAs::RESX_ISZERO, MmsAs::RESX_RESERVE);  
  const int isOverride = HmpResourceManager::instance()->isInternalLicensingOverridePresent();
                                      
  if (isCspExhausted && !isOverride)        // LICX CSPOUT 
      return this->raiseVoiceRecResourcesExhaustedAlarm();
 
  this->flags |= MMS_OP_FLAG_CSP_RESOURCE_RESERVED;
  this->asrChan = NULL;
  int result = 0;

  // Execute single turn of voice recognition:
  // Play prompt, accept voice input, wait for result   
  MmsAsrChannel* asrChannel = this->createAsrChannel();
  if (!asrChannel) return MMS_ERROR_DEVICE;

  // If there is no prompt we do not need a full duplex connection
  ListenDirection ld = this->isNoPromptVoiceRec()? HALFDUPLEX : FULLDUPLEX;
  // MMSLOG((LM_DEBUG,"%s voiceRec connection mode is %d\n", logkey, ld));

  MmsDeviceVoice* deviceVoice = this->voiceDevice(TRUE, TRUE, ld, MmsDeviceVoice::DEVICECAP_CSP);
  if (!deviceVoice || !deviceVoice->isCspDevice()) 
  {
      MMSLOG((LM_ERROR,"%s could not acquire streaming device\n", logkey));
      return MMS_ERROR_RESOURCE_UNAVAILABLE;
  }

  if (-1 == this->activateAsrChannel(deviceVoice->handle()))
  {
      MMSLOG((LM_ERROR,"%s could not activate ASR channel\n", logkey));
      return MMS_ERROR_DEVICE;
  }

  deviceVoice->setOwned(TRUE);
           
  if (session->isConferenceParty())         // If playing to single conferee ...
  {                                         //... switch to voice listen
      result = session->switchFromConferenceToVoice(deviceVoice, ld);
      if (result) return result;
  }
  else   
  if (session->isDigitLingering())
      session->handleDigitLingering(deviceVoice, ld);
   
  int defaultCommandTimeout = Config()->serverParams.commandTimeoutMsecsVoiceRec;
  this->onCommandServiceStart(defaultCommandTimeout);

  MMS_DV_TPT_LIST terminationConditions;
  int  conditionCount                       // Get termination conditions 
     = this->getTerminationConditionParameters(deviceVoice, terminationConditions); 
  if  (conditionCount == -1) return MMS_ERROR_TERMINATION_CONDITION;                                 

  MmsLocaleParams localeParams;              
  this->getLocaleParameters(localeParams);  // Get locale parameters   

  MMSPLAYFILEINFO fileinfo(localeParams);
  fileinfo.isPlay = TRUE; 
                                            // Open files in playlist
  int playfileCount = this->openPlaylist(deviceVoice, &fileinfo);
  if (playfileCount == -1)  return MMS_ERROR_FILEOPEN;

                                            // Start streaming on CSP device
  if (-1 == deviceVoice->streamCsp(isVoiceBargeIn()))
  {
      MMSLOG((LM_ERROR,"%s could not set audio streaming parameters\n", logkey));
      return MMS_ERROR_DEVICE;
  }

  if (isVoiceBargeIn())
      deviceVoice->startStreaming();

  if (playfileCount ==  0)
  {
      this->startVoicerecCompThread();
      // If no voice prompt specified, go straight to recognition task.
      this->onCommandEnter(TRUE);   
      return MMS_COMMAND_EXECUTING;
  }
                                            
  MmsDeviceVoice::MMS_PLAYRECINFO playinfo; // Set play file attributes

  if (-1 == this->getPlayRecordParameters(playinfo, fileinfo))
      return MMS_ERROR_SERVER_INTERNAL;
                                            // Ask to be notified of play complete
  if (-1 == registerForAsyncEventNotification(TDX_PLAY))  
      return MMS_ERROR_EVENT_REGISTRATION;

  unsigned int mode = MmsMediaDevice::ASYNC;
  if (this->flags & MMS_OP_FLAG_DIGITBUFFER_CLEARED)
      mode |= MmsMediaDevice::NOCLEAR;

  this->onCommandEnter(TRUE);   

  MMSLOG((LM_INFO,"%s begin play 1 of %d\n", logkey, playfileCount));
                                            // Start playing the playlist
  result = deviceVoice->playMultiple(&terminationConditions, &playinfo, mode);

  if  (result == -1)  
       cancelAsyncEventNotification(TDX_PLAY);
                                            
  return result == -1? MMS_ERROR_DEVICE: MMS_COMMAND_EXECUTING;              
}



int MmsSession::Op::handleStopOperation(MmsCurrentCommand& cmd) 
{                     
  // Handle explicit (non-internal) stop media request from client

  MmsStopMediaParams smp(cmd.flatmap);

  session->queryStopMediaParameters(smp);

  const int concurrencyMode = smp.isSynchronous? 
            MmsMediaDevice::SYNC: MmsMediaDevice::ASYNC;
        
  return this->stopMediaOperation(concurrencyMode);
}



int MmsSession::Op::handleMonitorCallState(MmsCurrentCommand& cmd)     
{
  // Listen to voice channel and report on occurrence of specified event

  const int defaultCmdTimeout = Config()->serverParams.commandTimeoutMsecsMonitorCallState;
  this->onCommandServiceStart(defaultCmdTimeout);

  // We are currently limited to one call state monitor at a time per session.
  // The reason is that timer events are keyed only to session. If we were to
  // implement user data on timers, we could remove this limitation, if in fact
  // there is a use case for doing so.
                                            // Can't have multiple CSM per session
  if (session->callStateMon) return MMS_ERROR_RESOURCE_UNAVAILABLE;

  int result = -1, duration = 0, callState = 0;
  char* pCallState = NULL, *pduration = NULL;
                                            // Get a receive-only vox connect
  MmsDeviceVoice* deviceVoice = this->voiceDevice(FALSE, TRUE);
  if (!deviceVoice) return MMS_ERROR_RESOURCE_UNAVAILABLE;

  result = session->ipDevice()->busConnect(deviceVoice, MmsMediaDevice::HALFDUPLEX);
  if  (result != 0) return MMS_ERROR_DEVICE;   

  result = parameterMap.find(MMSP_CALL_STATE, &pCallState);
  if (pCallState) callState = *((int*)pCallState);  
  if (callState == 0) return MMS_ERROR_TOO_FEW_PARAMETERS;

  result = parameterMap.find(MMSP_CALL_STATE_DURATION, &pduration);
  if (pduration) duration = *((int*)pduration);  
  if (duration == 0) duration = Config()->serverParams.defaultMonitorCallStateDuration;

                                            // Instantiate call state monitor
  session->callStateMon = new MmsCallStateMonitor(callState, sessionID(), opid, duration, 
        deviceVoice, sessionManager()->timerManager(), sessionManager(), Config());

  MmsCallStateMonitor* csm = session->callStateMon;

  if (csm->EventID() > 0)
  {                                         // Indicate session is pending an event ...
      this->markWaiting(csm->EventType(), csm->EventID());
      return MMS_COMMAND_WAITING;           // ... but not an async voice event
  }
  else return MMS_ERROR_EVENT_REGISTRATION;
}



int MmsSession::Op::handleAdjustments(MmsCurrentCommand& cmd)
{ 
  // Assign a digit to adjust volume or speed up or down a specified number
  // or ticks; or to immediately adjust volume or speed directly or relatively 
  // to a specified tick; or to clear previous adjustments and return to default.

  #if(0)
  this->onCommandServiceStart(); 
  MmsDeviceVoice* deviceVoice = this->voiceDevice();
  if  (!deviceVoice) return MMS_ERROR_RESOURCE_UNAVAILABLE;

  const int commandID = getFlatmapCommand(this->flatmap()); 
  unsigned int param  = 0;
  int   result = 0, digit = 0, adjval = 0, action = 0;
  char* pparam = NULL;  
  
  switch(commandID)                                                                            
  {
    case COMMANDTYPE_ASSIGN_VOLADJ_DIGIT: 
    case COMMANDTYPE_ASSIGN_SPEEDADJ_DIGIT:
         result = parameterMap.find(MMSP_VOLSPEED_DIGIT, &pparam);
         if  (!pparam) return MMS_ERROR_TOO_FEW_PARAMETERS;
         param  = *((unsigned int*)pparam);
         adjval = MMS_GET_ADJUSTMENT_VALUE(param); 
         digit  = MMS_GET_ADJUSTMENT_DIGIT(param); 
         result = commandID == COMMANDTYPE_ASSIGN_VOLADJ_DIGIT?
                  deviceVoice->assignVolumeAdjustmentDigit((char)digit, adjval):
                  deviceVoice->assignSpeedAdjustmentDigit ((char)digit, adjval);
         break;
              
    case COMMANDTYPE_ADJUST_VOLUME:              
    case COMMANDTYPE_ADJUST_SPEED: 
         result = parameterMap.find(MMSP_VOLSPEED_ADJUSTMENT, &pparam);
         if  (!pparam) return MMS_ERROR_TOO_FEW_PARAMETERS;
         param  = *((unsigned int*)pparam);
         adjval = MMS_GET_ADJUSTMENT_VALUE(param); 
         action = MMS_GET_ADJUSTMENT_ACTION(param); 
         result = commandID == COMMANDTYPE_ADJUST_VOLUME?
                  deviceVoice->assignVolumeAdjustmentDigit(action, adjval):
                  deviceVoice->assignSpeedAdjustmentDigit (action, adjval);
         break;
              
    case COMMANDTYPE_CLEAR_VS_ADJUSTMENTS:
         result = deviceVoice->clearVolumeSpeedAdjustments();
         break;        
  }

  return result == -1? MMS_ERROR_DEVICE: 0;
  #endif

  return MMS_ERROR_NO_SUCH_COMMAND;
}



int MmsSession::Op::handleConferenceResourcesRemaining(MmsCurrentCommand& cmd) 
{ 
  // Return number of conferencing resources remaining on the conference device
  // assigned to this session. Thus a conference must be in progress in order
  // to determine remaining resources via this method. The reason for this is
  // that currently all server commands are connection (session) oriented.
  // Resource count is returned in the parameter map header param.

  this->onCommandServiceStart(); 

  if  (!session->isInConference()) return MMS_ERROR_NO_SUCH_CONFERENCE;

  MmsDeviceConference* deviceConference = session->conferenceDevice();
  if  (!deviceConference) return MMS_ERROR_SERVER_INTERNAL;

  this->onCommandEnter();  

  int  resourcesRemaining = deviceConference->resourcesRemaining();

  if  (resourcesRemaining != -1)
       putMapHeader(setParam, resourcesRemaining);

  return resourcesRemaining == -1? MMS_ERROR_DEVICE: 0;
}



int MmsSession::Op::handleConfereeSetAttribute(MmsCurrentCommand& cmd) 
{
  // Set or reset one attribute for a conferee. Attributes include receive only,
  // tariff tone, coach, and pupil. 

  this->onCommandServiceStart(); 
  if (!session->isInConference()) return MMS_ERROR_NO_SUCH_CONFERENCE;

  MmsDeviceConference* deviceConference = session->conferenceDevice();
  if (!deviceConference) return MMS_ERROR_SERVER_INTERNAL;

  MmsDeviceIP* deviceIP = session->ipDevice();

  char* pattrs = NULL;
  int  result  = parameterMap.find(MMSP_CONFEREE_ATTRIBUTES, &pattrs);
  if  (pattrs == NULL) return MMS_ERROR_TOO_FEW_PARAMETERS;
  int  attrs   = *((int*)pattrs);

  // Only one attribute will be specified per request; however another bit  
  // in the conferee attributes may be set to indicate the request is to 
  // reset, rather than set, that attribute.
  const int isSetAttr = (attrs & MMS_CONFEREE_ATTRIBUTE_OFF) == 0;
  const int confID = session->confInfo.id;

  this->onCommandEnter(); 
  
  if (session->isHairpinConference())
  {
       // If hairpin conference and the attribute we're setting or resetting  
       // is "receive only", mute or unmute hairpinned party and we're done
       if (attrs & MMS_CONFEREE_RECEIVE_ONLY)
           return session->muteHairpinned(isSetAttr);

       // If hairpin conference and the attribute we're setting or resetting
       // is one which requires HMP, promote conference to an HMP conference
       if (-1 == session->promoteConference(this->flatmap()))
           return MMS_ERROR_RESOURCE_UNAVAILABLE;
  }
     
  if  (attrs & MMS_CONFEREE_RECEIVE_ONLY)
       result = deviceConference->setReceiveOnly(confID, deviceIP, isSetAttr);
  else
  if  (attrs & MMS_CONFEREE_TARIFF_TONE)  
       result = deviceConference->setTariffTone(confID, deviceIP, isSetAttr);
  else
  if  (attrs & MMS_CONFEREE_COACH) 
       result = deviceConference->setCoach(confID, deviceIP, isSetAttr);
  else
  if  (attrs & MMS_CONFEREE_PUPIL)  
       result = deviceConference->setPupil(confID, deviceIP, isSetAttr);
  else result = MMS_ERROR_TOO_FEW_PARAMETERS;

  return result == -1? MMS_ERROR_DEVICE: result;
}



int MmsSession::Op::handleConferenceEnableVolumeControl(MmsCurrentCommand& cmd) 
{
  // Enable or disable volume control for the conference. If enabling, the
  // digits for volume up, reset, and down are supplied.

  this->onCommandServiceStart(); 
  if  (!session->isInConference()) return MMS_ERROR_NO_SUCH_CONFERENCE;

  MmsDeviceConference* deviceConference = session->conferenceDevice();;
  if  (!deviceConference) return MMS_ERROR_SERVER_INTERNAL;

  char* pctrl  = NULL;
  int  result  = parameterMap.find(MMSP_CONFERENCE_VOLCONTROL, &pctrl);
  if  (pctrl  == NULL) return MMS_ERROR_TOO_FEW_PARAMETERS;
  int  volctrl = *((int*)pctrl);
                           
  int  onOrOff    = MMS_GET_ONOROFF(volctrl);
  int  digitUp    = MMS_GET_VOLUP_DIGIT(volctrl);
  int  digitReset = MMS_GET_VOLRESET_DIGIT(volctrl);
  int  digitDown  = MMS_GET_VOLDOWN_DIGIT(volctrl);

  this->onCommandEnter(); 

  result = deviceConference->enableVolumeControl
          (onOrOff, digitUp, digitReset, digitDown);

  return result == -1? MMS_ERROR_DEVICE: result;
}





