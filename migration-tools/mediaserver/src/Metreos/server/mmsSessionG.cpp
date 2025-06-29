//
// MmsSessionG.cpp
// 
// Session operation stop media/adjust play/digit support
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
#include "mmsAudioFileDescriptor.h"
#include "mmsCommandTypes.h"
#include "mmsAsr.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// stop media operation/media query support
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


int MmsSession::Op::cancelMedia(const int eventDisposition, const int isAsync)             
{ 
  // Cancel session async media if any. We may or may not not wish to cancel
  // the associated event, since if client is asking to cancel voice media
  // such as an announcement, client likely wants to receive the termination
  // event for the canceled play command.

  // #include "mmsMediaEvent.h"
  // MmsEventRegistry::instance()->dump("event table before cancelMedia");
  const int mode = isAsync? MmsMediaDevice::ASYNC: MmsMediaDevice::SYNC;
  int result = 0, isHandled = 0;

  switch(this->cmdid)
  { // We should have a cleaner way of identifying operations which are not async
    // voice, and canceling those operations. However for the present we will 
    // look at the individual command IDs to do the identification.
    case COMMANDTYPE_MONITOR_CALL_STATE:
    case COMMANDTYPE_VOICEREC:
         //cancelAsyncEventNotification(0xE0 /*TEC_STREAM*/);
         result = this->stopMediaOperation(mode);
         isHandled = TRUE;
  }

  if (!isHandled)
  {
      const int isIpEvent = IS_IP_EVENT(waitInfo.eventType);
                                             
      if (eventDisposition != MMS_RETAIN_EVENT) // Cancel event notification?                      
          this->cancelAsyncEventNotification
               (waitInfo.eventType, isIpEvent, eventDisposition);
                                                // Cancel async voice meida
      if (!isIpEvent) result = this->stopMediaOperation(mode); 
  }

  const int sessionID = session? sID(): 0;
       
  if  (result == -1)   
       MMSLOG((LM_ERROR,"SMGR could not cancel media on session %d op %d\n", 
               sessionID, opid)); 
  else MMSLOG((LM_INFO, "SMGR cancel media on session %d op %d\n", 
               sessionID, opid));

  return result;
}



int MmsSession::Op::stopMediaOperation(int concurrencymode) 
{   
  // Stop the voice media operation in progress. We do not register an event
  // here, since the event that will fire as a result of the stop operation
  // is the event we registered for when that media operation was launched.
  // This operation may be requested by client COMMANDTYPE_STOP_OPERATION,
  // or internally, in order to cancel an operation as a result of timeout.
  // If executed on client request, this method is invoked directly by 
  // session manager, and not assigned to a service pool thread. We therefore
  // Do not do the usual session state transitions here, as session state
  // is reflected by the currently executing command, and its parameter
  // package, not by the stop request and its associated package. If this stop
  // operation occurs prior to the firing of some other termination event on
  // the operation, client will see a termination condition of "userstop" on 
  // return of the package for the canceled command. Client also receives
  // an acknowledgement of the COMMANDTYPE_STOP_OPERATION itself.

  ACE_Guard<ACE_Thread_Mutex> x(session->extopCriticalSection);

  switch(this->cmdid)
  { 
    case COMMANDTYPE_MONITOR_CALL_STATE:
         return this->handleCstTimeExpired(); // Stop monitoring CST events
  }

  MmsDeviceVoice* deviceVoice = this->assignedVoiceDevice(); 
  if (!deviceVoice) return 0;  
 
  const int wasVoiceMediaExecuting = this->isVoiceMediaPlaying(TRUE);   

  if (this->flags & MMS_OP_FLAG_BLOCK_REQUESTED) 
  {
      // Ordinarily a StopMediaOperation command will not block. We do this
      // because the dx_stopch is expensive and client usually would prefer
      // to have it return immediately. However this presents a problem when 
      // commands are issued back to back, because the dx_stopch may not have 
      // completed and the second command could fail. Client may therefore
      // override the concurrency mode with the "block" boolean parameter.  

      concurrencymode = MmsMediaDevice::SYNC;
  }

  int result = deviceVoice->stopMediaOperation(concurrencymode);

  if (this->isStreaming()) this->stopVoiceRecOperation();

  if (result == -1 && !wasVoiceMediaExecuting) 
      result = 0;                           // No error when nothing to stop    


  return result == -1? MMS_ERROR_DEVICE: 0;
}



int MmsSession::Op::stopVoiceRecOperation(int concurrencymode) 
{
  if (this->asrChan)
  {       
      if (this->isAsrChannelActive())
      {
          Asr::instance()->stop(this->asrChan);
          Asr::instance()->deactivateChannel(this->asrChan);        
      }

      Asr::instance()->destroyChannel(this->asrChan);
      this->asrChan = NULL;

      // Un-burn the CSP license we burnt when creating the channel 
      MmsAs::csp(MmsAs::RESX_INC);              // LICX CSP+ (1 of 1) 
      this->flags &= ~MMS_OP_FLAG_CSP_RESOURCE_RESERVED;
  }

  return 0;
}



int MmsSession::stopStreamingOperation(const int mode)
{
   MmsSession::Op* operation = this->findStreamingOperation();

   int result = 0;
   if (operation && operation->isCancelOnDigit())
   {
       result = operation->stopMediaOperation();
       operation->stopVoiceRecOperation();  // result always 0
       this->closeOperation(operation->opID(), operation->cmdID());
       return result;
   }
   else
       result = -1;

   return result;
}



int MmsSession::stopOperation     
( const int opID, MmsTask* serverMgr, const int isAsync, const int isLock)  
{              
  MmsSession::Op* op = this->findByOpID(opID, isLock);

  return this->stopOperation(op, serverMgr, isAsync);
}


                                
int MmsSession::stopOperation(MmsSession::Op* op, MmsTask* serverMgr, const int isAsync)  
{              
  // Stop async operation, executed either in the context of session manager, 
  // or thread pool. Termination events, if any, are expected to be blocked, 
  // and if firing after we set the barrier but before we cancel the event, 
  // they will discard their results once unblocked. We end the canceled 
  // operation and return a final result here.

  if (op && op->isBusy() && (op->isWaiting() || op->isStreaming())); else return -1;

  char* canceledCommandMap = op->flatmap(); 
  const int opID = op->opID();
  int   cmdID=0;

  // Determine if this is a StopMediaOperation client command, or if media server
  // generated the stop media internally as part of a disconnect or timeout,
  // and set the termination reason to be returned to client, accordingly.
  const int isOperationTimeout = (op->flags & MMS_OP_FLAG_CMD_TIMEOUT) != 0; 
  const int isClientStopMediaRequest = !isOperationTimeout
     && (this->flags & MMS_SESSION_FLAG_IS_DISCONNECTING) == 0;  
  
  // Since stopOperation releases the associated vox and discards pending voice 
  // events, we will reconnect to conference here instead of at HandleEventVoice.

  if (this->isConferenceParty() && isClientStopMediaRequest)   
      if (-1 == this->reconnectToConference(op->assignedVoiceDevice()))
          MMSLOG((LM_ERROR,"SMGR session %d could not reconnect to conference\n", 
                  this->ordinal)); 

  MmsDeviceVoice* deviceVoice = op->assignedVoiceDevice();

  const int terminationReason = isClientStopMediaRequest?
            TM_USRSTOP: TM_METREOS_AUTOSTOP;                               
                                             // Stop the media operation
  const int result = op->cancelMedia(MMS_CANCEL_EVENT_NOLOG, isAsync);
     
  if (Mms::isFlatmapReferenced(canceledCommandMap, 15, ordinal, opID))
  {   
      // Return *final result* for the canceled operation to client                                      
      op->dataLock.acquire();               // Thread-safely set result data
      cmdID = getFlatmapCommand(canceledCommandMap);

      setFlatmapTermReason(canceledCommandMap, terminationReason); 
      this->logTerminatingCondition(terminationReason);
      setFlatmapRetcode(canceledCommandMap, 0);

      long elapsed = deviceVoice? deviceVoice->elapsedTimeMs(): 0;
      setFlatmapElapsedTime(canceledCommandMap, elapsed? elapsed: 1);

      op->dataLock.release(); 

      serverMgr->postMessage(MMSM_SERVERCMD_RETURN, (long)canceledCommandMap);
  }
  else MMSLOG((LM_ERROR,"SMGR session %d no final result sent for op %d\n",
               this->ordinal, opID)); 
   
  this->closeOperation(opID, cmdID); 

  return 0;
}



int MmsSession::stopAllOperations(MmsTask* serverMgr, const int exceptOperation)   
{          
  // Cancel all async operations on this session. The suspend termination lock is
  // expected to be set, since we can't permit termination events to proceed while
  // we are here. Parameter exceptOperation, if supplied, will specify the caller's
  // operation; that is, the operation executing the stop media.
           
  int result = 0, count = 0, operationID = 0;
  ACE_Guard<ACE_Thread_Mutex> x(this->optableLock);
  MmsSession::Op* op = &this->optable[0]; 

  for(int i=0; i < MMS_MAX_SESSION_OPERATIONS; i++, op++) 
  {      
      if (!op->isBusy()) continue;

      if (exceptOperation == (operationID = op->opID())) continue;

      if (0 == this->stopOperation(op, serverMgr, FALSE)) 
          count++;
  } 

  return count;
}


                       
int MmsSession::queryStopMediaParameters(MmsStopMediaParams& params) 
{
  // Check stop media command parameter map; set flags accordingly.  
  // Note that stop media barges a current session, so the parameter map arriving 
  // with the stop media command is not the same as that of the session it is barging.

  char* pmap = params.flatmap; if (pmap == NULL) return -1;  
  // MmsSession::dumpParameterMap(pmap);   
  MmsFlatMapReader map(pmap);
  char* p = NULL;
  params.isSynchronous = params.operationID = 0;

  map.find(MMSP_BLOCK, &p);                 // Client asked for synchronous execution?
  if (p) params.isSynchronous = *(int*)p;

  p = NULL;
  map.find(MMSP_OPERATION_ID, &p);          // Stopping a particular operation?
  if (p) params.operationID = *(int*)p;

  return 0;
}



int MmsSession::Op::isVoiceMediaPlaying(const int isIgnoreWait)
{
  // Determine if operation is executing voice media w/o querying device
  int  isvoicemediaplaying = FALSE;

  if  (this->isBusy() && (isIgnoreWait || this->isWaiting()))  
  {                                          
       char* parametermap = this->flatmap(); 
       if (!Mms::isFlatmapReferenced(parametermap,7,sessionID(),opid)) return FALSE;

       const int commandID = getFlatmapCommand(parametermap);
                                            // Is it a voice command?
       isvoicemediaplaying = IS_ASYNC_VOICE_COMMAND(commandID); 
  }    

  return isvoicemediaplaying;
}



int MmsSession::Op::onRfc2833Disconnect()
{
  this->cancelRfc2833EventNotification();
  this->toggleRfc2833Event(false);
  return 0;
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// volume and speed adjustment
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


int MmsSession::adjustPlay(char* flatmap, const int isLog)
{
  ACE_Guard<ACE_Thread_Mutex> x(this->optableLock);
  MmsSession::Op* op = &this->optable[0]; 

  for(int i=0; i < MMS_MAX_SESSION_OPERATIONS; i++, op++) 
  {      
      if (op && IS_ASYNC_VOICE_COMMAND(op->cmdID()))
          op->adjustPlay(flatmap, isLog);
  }

  return 0;
}

   

int MmsSession::Op::adjustPlay(const unsigned packedVolSpeed, const int isLog)
{
  // This version of adjustPlay adjusts volume/speed according to the packed
  // volspeed settings supplied. 

  if (packedVolSpeed == 0) return 0;

  MmsVolumeSpeedEncoder unpacked(packedVolSpeed);

  const int flags = isLog? MMS_ADJPLAY_LOG: 0;

  return this->adjustPlay(unpacked, flags | MMS_ADJPLAY_INHERIT_ADJTYPE);
}



int MmsSession::Op::adjustPlay(const int isLog)
{  
  // Invoked to set volume and/or speed on operation's voice device.

  MmsVolumeSpeedEncoder coder;

  const int count = this->getVolumeSpeedParameters(coder);
  if (count == 0) return MMS_ERROR_TOO_FEW_PARAMETERS;

  return this->adjustPlay(coder, isLog);
}



int MmsSession::Op::adjustPlay(char* flatmap, const int isLog)
{  
  // Invoked to set volume and/or speed on operation's voice device.
  // See comments in AdjustPlay, above

  MmsVolumeSpeedEncoder coder;

  const int count = this->getVolumeSpeedParameters(coder, flatmap);
  if (count == 0) return MMS_ERROR_TOO_FEW_PARAMETERS;

  return this->adjustPlay(coder, isLog);
}



int MmsSession::Op::adjustPlay(MmsVolumeSpeedEncoder& coder, const int flags)
{  
  // Invoked to set volume and/or speed on a operation's voice device.
  // See comments in AdjustPlay, above

  //MmsDeviceVoice* deviceVoice = this->voiceDevice(FALSE, TRUE);
  //if (deviceVoice == NULL) return MMS_ERROR_RESOURCE_UNAVAILABLE;
  //deviceVoice->setOwned(TRUE);              // Keep voice resx with session

  MmsDeviceVoice* deviceVoice = this->assignedVoiceDevice();
  if (deviceVoice == NULL) return MMS_ERROR_RESOURCE_UNAVAILABLE;

  int vresult = 1, sresult = 1;             // 1 = unchanged, 0 = changed, -1 error
  const int newvolume = coder.volume(), newspeed = coder.speed();
  const int isLogging = (flags & MMS_ADJPLAY_LOG) != 0;
  const int isInheritPriorAdjustmentType = (flags & MMS_ADJPLAY_INHERIT_ADJTYPE) != 0;
  MmsVolumeSpeedEncoder currentSettings(deviceVoice->volspeed);

  // Execute requested changes to volume and/or speed, only if requests constitute 
  // changes to the current settings. If this volume/speed adjustment was specified
  // on a play, we will inherit the adjustment type from the most recent adjustPlay
  // command, if any.
       
  if (coder.isVolumeSet() /*&& newvolume != currentSettings.volume()*/) 
  {  
      if (isInheritPriorAdjustmentType)
          coder.copy(MmsVolumeSpeedEncoder::VSE_VADJTYPE, currentSettings);

      vresult = deviceVoice->adjustVolume
               (coder.vadjtype(), newvolume, coder.vtogtype());
  }
   
  if (coder.isSpeedSet() && vresult != -1 /*&& newspeed != currentSettings.speed()*/)
  {
      if (isInheritPriorAdjustmentType)     
          coder.copy(MmsVolumeSpeedEncoder::VSE_SADJTYPE, currentSettings);

      sresult = deviceVoice->adjustSpeed
               (coder.sadjtype(), newspeed, coder.stogtype()); 
  }  

  if (vresult == 0 || sresult == 0)         // Was either setting changed?
  {
      if  (vresult == 0 && sresult == 0)    // Both settings changed?
      {    if (isLogging) 
               MMSLOG((LM_DEBUG,"%sd adjust vox%d vol %d speed %d\n",
                       logkey, hvox, newvolume, newspeed));
      }     
      else
      if  (vresult == 0 && sresult != 0)    // Volume only changed?
      {    // If one setting changed and not the other, ensure that
           // we don't change saved value for the unchanged setting
           coder.copy(MmsVolumeSpeedEncoder::VSE_SPEED,  currentSettings); 

           if (isLogging) 
               if  (coder.vtogtype())
                    MMSLOG((LM_DEBUG,"%s toggle vox%d volume type %d\n", 
                           logkey, hvox, coder.vtogtype()));
               else MMSLOG((LM_DEBUG,"%s adjust vox%d volume %d\n", 
                            logkey, hvox, newvolume));
      }          
      else
      if  (sresult == 0 && vresult != 0)    // Speed only changed?
      {    
           coder.copy(MmsVolumeSpeedEncoder::VSE_VOLUME, currentSettings);

           if (isLogging) 
               if  (coder.stogtype())
                    MMSLOG((LM_DEBUG,"%s toggle vox%d speed type %d\n", 
                           logkey, hvox, coder.stogtype()));
               else MMSLOG((LM_DEBUG,"%s adjust vox%d speed %d\n", 
                            logkey, hvox, newspeed));
      }

      deviceVoice->volspeed = coder.pack(); // Save new settings 
  }
               
  return vresult < 0 || sresult < 0? -1: 0;              
}

                

int MmsSession::Op::adjustPlayPerConfig(MmsDeviceVoice* deviceVoice, const int isLog)
{
  // Adjust voice resx volume and speed to configured settings

  MmsConfig* config = session? session->Config(): NULL;
  if (!config) return -1;
  int vresult = 1, sresult = 1;
  int vol = config->hmp.volume;
  int spd = config->hmp.speed;

  if  (MmsVolumeSpeedEncoder::isDefaultVolumeSpeed(deviceVoice->volspeed));
  else deviceVoice->clearVolumeSpeedAdjustments();

  MmsVolumeSpeedEncoder configSettings;

  if (vol)
  {   vol = min(vol,10); vol = max(vol,-10);
      vresult = deviceVoice->adjustVolume(MMS_ADJTYPE_ABSOLUTE, vol);      
  }

  if (spd)
  {   spd = min(spd,10); spd = max(spd,-10);
      sresult = deviceVoice->adjustSpeed(MMS_ADJTYPE_ABSOLUTE, spd);      
  }

  if (isLog && (vresult == 0 || sresult == 0))   
      MMSLOG((LM_DEBUG,"%s op %d config vox %d vol %d speed %d\n",
              logkey, opid, deviceVoice->handle(), vol, spd));
   
  return (vresult < 0 || sresult < 0)? -1: 0;
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// digit support
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


int MmsSession::Op::patternMatchDigits(char* incoming)  
{
  // Match incoming digits against a digit pattern.

  const int ticks = Mms::getTicks();    
  if ((int)(ticks - this->tickstart) > Config()->serverParams.digitSequenceIntervalSeconds)
  {    this->matchcount = 0;       
       this->tickstart  = ticks;   
  }

  const int digitsInPattern = strlen(this->digitlist);
  char* p = incoming;
  char* q = &this->digitlist[this->matchcount];

  while(*p)
  {
     if  (*p == *q)  
          this->matchcount++;       
     else
     {    this->matchcount = 0;
          q = this->digitlist;
          if  (*p == *q) 
               matchcount++;
     }

     if  (matchcount == digitsInPattern) break;
     ++p;
  }

  return matchcount == digitsInPattern; 
}



int MmsSession::patternMatchDigitsEx(char* incoming)  
{
  ACE_Guard<ACE_Thread_Mutex> x(this->optableLock);
  MmsSession::Op* op = &this->optable[0]; 

  for(int i=0; i < MMS_MAX_SESSION_OPERATIONS; i++, op++) 
  {      
    if (op->isWatchingDigitPattern())
    {
      int isPatternMatched = op->patternMatchDigits(incoming);
      if (isPatternMatched)
      {
        // When the patten is matched, cancel the voice operation,
        // permitting the termination event to fire.  
        MMSLOG((LM_INFO,"%s %d pattern '%s' matched on '%s'\n", 
                op->logkey, op->opid, op->digitlist, incoming));

        op->markDigitPatternComplete(); // Trigger "digitpattern" result

        op->cancelMedia(MMS_RETAIN_EVENT); 
      }
    } 
  }

  return 0;
}



char* MmsSession::Op::getDigitsImmediate    
( MmsDeviceVoice* deviceVoice, MMS_DV_TPT_LIST* tptlist, int* outcond)
{  
  // Get current digits from channel digit buffer synchronously. We specify   
  // a maxtime termination condition with essentially no time, to force the 
  // operation to terminate immediately. 
 
  // Returns a pointer to digits received, and an HMP termination condition 
  // identifier constant. If no digits were received, NULL is returned.

  const int condcount = tptlist? tptlist->size: 0;
  int result = 0, terminatingCond = 0, maxtime = 0, isMaxtimeModified = 0;
  MMS_DV_TPT_LIST internalTpt, *ptptlist = tptlist;
  DV_TPT* tptMaxtime = NULL;
  #define MMS_INSTANTANEOUS_TERMINATION 1

  if (condcount > 0)
  {   
      // If caller specified termination conditions, see if maxtime was included      
      tptMaxtime = session->findTerminationCondition(*ptptlist, DX_MAXTIME);
                   
      // If caller specified a maxtime termination condition, save off caller's
      // maxtime value, and temporarily change caller's maxtime to be very short
      // such that the dx_getdig terminates effectively immediately                             
      if (tptMaxtime)                           
      {   maxtime = tptMaxtime->tp_length; 
          tptMaxtime->tp_length = MMS_INSTANTANEOUS_TERMINATION; 
          isMaxtimeModified = TRUE;        
      }                                         
  }     

  if (!tptMaxtime)                             
  {           
      // If caller did not specify maxtime, we'll create a maxtime termination
      // condition here. If user specified no termination conditions, we'll
      // use this internal TPT object for that purpose.  
      if (condcount == 0) ptptlist = &internalTpt;
  
      if (-1 == deviceVoice->setTerminationCondition
              (*ptptlist, DX_MAXTIME, MMS_INSTANTANEOUS_TERMINATION, 0, 0))
          return NULL; 
  }
   
  // Get digits from HMP synchronously and immediately. The termination condition
  // will be returned into terminatingCond, which will be DX_MAXTIME unless a
  // preexisting condition exists matching one of the supplied termination conditions.
  // The synchronous receiveDigits appears to take around 70ms to complete.
  result = deviceVoice->receiveDigits(ptptlist, MmsMediaDevice::SYNC, &terminatingCond);                
                                                                         
  if (isMaxtimeModified) tptMaxtime->tp_length = maxtime;
  if (result == -1) return NULL; 

  if (outcond) *outcond = terminatingCond;  // Return terminating condition
                                            // Return digits

  char*  deviceDigitBuf = deviceVoice->digitBuffer(); 
  return deviceDigitBuf && *deviceDigitBuf? deviceDigitBuf: NULL;
}



int MmsSession::Op::isDigitTerminationPreexisting
( MmsDeviceVoice* deviceVoice, MMS_DV_TPT_LIST& terminationConditions)
{
  // JDL 03/15/06 check if HMP's TM_DIGIT termination condition is met by searching 
  // user-defined termination digit in cached digits.  If the condition matched 
  // then set termination condition and fill in return digits before returning 
  // both provisional and final results

  MmsSession* session = this->session; 
  if (session->digitCacheSize() == 0) return FALSE; 
  int result = FALSE, terminatingCondition = 0, digitsCount = 0;   
  
  DV_TPT* digType = session->findTerminationCondition(terminationConditions, DX_DIGTYPE);

  if (digType)
  {
      char termDigit = (char)digType->tp_length;
      char *p = ACE_OS::strrchr(session->digitCache, termDigit);
      if (p)
      {   // Return only up to last termination digit.
          digitsCount = (int)(p - session->digitCache + 1);
          terminatingCondition = TM_DIGIT;                               
          result = TRUE;                       
      }
  }

  digType = result? NULL:
      session->findTerminationCondition(terminationConditions, DX_MAXDTMF);

  if (digType)
  {
      // Term condition is maxdigits so we return that number of digits  
      digitsCount = digType->tp_length;

      if (digitsCount != 0 && session->digitCacheSize() >= digitsCount)
      {    
          terminatingCondition = TM_MAXDTMF;
          result = TRUE;    
      }
  }

  // JDL, 05/24/06, check for matched digit in digit list
  digType = result? NULL:
      session->findTerminationCondition(terminationConditions, DX_DIGMASK);
  if (digType)
  {
      // Term condition is digit list, return only up to last matched digit
      int digitlistLen = ACE_OS::strlen(this->digitlist);
      for (int i=0; i<digitlistLen; i++)
      {
        char *p = ACE_OS::strrchr(session->digitCache, this->digitlist[i]);
        if (p)
        {   // Return only up to last matched digit.
            digitsCount = (int)(p - session->digitCache + 1);
            terminatingCondition = TM_DIGIT;                               
            result = TRUE;                       
            break;
        }
      }
  }
   
  if (result)
  {
      this->flags |= MMS_OP_FLAG_HMP_TERM_MET;
      this->putMapHeader(setTermReason,terminatingCondition); 
      session->logTerminatingCondition(terminatingCondition);
      this->setDigitsReceivedReturn(deviceVoice, digitsCount); 
      session->clearDigitCache(digitsCount);  
  }

  return result;
}

