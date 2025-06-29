//
// MmsSessionH.cpp
// 
// Session operation utility methods
//
#include "StdAfx.h"
#pragma warning(disable:4786)
#ifdef MMS_WINPLATFORM
#include <minmax.h>
#endif
#include "mmsSession.h"
#include "mmsReporter.h"
#include "mmsParameterMap.h"
#include "mmsMediaEvent.h"
#include "mmsSessionManager.h"
#include "mmsAudioFileDescriptor.h"
#include "mmsCommandTypes.h"
#include "mmsAsr.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION



void MmsSession::Op::putMapHeader(int operation, int value)
{
  // Synchronized write access to the operation parameter map's header. Note that 
  // serializing writes to the map becomes necessary only while a command is 
  // executing asynchronously on the session, since only then can multiple threads 
  // operate on the operation instance data concurrently.
  ACE_Guard<ACE_Thread_Mutex> x(this->dataLock);

  char* map = this->flatmap();
  if (!map) return;
  if (!Mms::isFlatmapReferenced(map, 6, sessionID(), opid)) return;    

  switch(operation)
  {                    
    case setSessionID:
         setFlatmapSessionID(map,value);
         break;
    case setConnectionID:
         setFlatmapConnectionID(map,value);
         break;
    case setTransID:
         setFlatmapTransID(map,value);
         break;
    case setParam:
         setFlatmapParam(map,value);
         break;
    case setFlag:
         setFlatmapFlag(map,value);
         break;
    case setRetcode:        
         setFlatmapRetcode(map,value);
         break;
    case setReasonCode:
         setFlatmapRescode(map,value);
         break;
    case setTermReason:
         setFlatmapTermReason(map,value);
         break;
    case setElapsedTime:
         setFlatmapElapsedTime(map,value);
         break;
    case setClientHandle:
         setFlatmapClientHandle(map,(void*)value);
         break;
    case setServerId:
         setFlatmapServerID(map,value);
         break;
    case setOperationId:
         setFlatmapOperationID(map,value);
         break;
  }
}



int MmsSession::Op::resetOperationTimer(int ms)       
{
  if (ms == 0) ms = this->commandTimeoutMs;
  ACE_Guard<ACE_Thread_Mutex> x(this->dataLock);

  const int remainingMs = this->opTimeoutTimer.reset(ms);
  return remainingMs;
}



int MmsSession::Op::countdownOperation()            
{  
  // Update time remaining -- return -1 if timed out. Timers reset on expiration. 
  if (isInfiniteOperationTimeout()) return 0;  
  ACE_Guard<ACE_Thread_Mutex> x(this->dataLock);

  int remainingMs = this->opTimeoutTimer.remaining();
  if (remainingMs == 0) return 0;

  remainingMs = this->opTimeoutTimer.countdown();
  return remainingMs == -1? -1: 0;
}



int MmsSession::Op::IsMapFlagSet(const unsigned int bitflag)
{
  // Determine if specified flag is set in map header

  MmsFlatMap::MapHeader* mapheader = parameterMap.header();
  if (!mapheader) return 0;

  MmsServerCmdHeader* cmdheader =
 (MmsServerCmdHeader*)((char*)mapheader + sizeof(MmsFlatMap::MapHeader));

  const int masked = cmdheader->flags & bitflag;
  return masked != 0;
}



int MmsSession::Op::isConferenceOperation()
{
  return this->IsMapFlagSet(MmsServerCmdHeader::IS_CONFERENCE);
}



int MmsSession::Op::setElapsedTimeReturn(MmsDeviceVoice* deviceVoice)   
{
  long elapsed = deviceVoice->elapsedTimeMs();

  putMapHeader(setElapsedTime, elapsed);
  return 0;
}



MmsSession::Op* MmsSession::findCallStateOperation()
{
  // Find and return the operation waiting on call state events
  // Note that there can be only one such operation per session

  ACE_Guard<ACE_Thread_Mutex> x(this->optableLock);
  MmsSession::Op* op = &this->optable[0];

  for(int i=0; i < MMS_MAX_SESSION_OPERATIONS; i++, op++)  
      if (op->cmdID() == COMMANDTYPE_MONITOR_CALL_STATE) break;

  return (i >= MMS_MAX_SESSION_OPERATIONS)? NULL: op;
}



MmsSession::Op* MmsSession::findStreamingOperation()
{
  // Find and return the single streaming operation if one exists

  ACE_Guard<ACE_Thread_Mutex> x(this->optableLock);
  MmsSession::Op* op = &this->optable[0];

  for(int i=0; i < MMS_MAX_SESSION_OPERATIONS; i++, op++)  
      if (op->cmdID() == COMMANDTYPE_VOICEREC) break;

  return (i >= MMS_MAX_SESSION_OPERATIONS)? NULL: op;
}



int MmsSession::isCallStateMonitoring()
{
  return this->findCallStateOperation() != NULL;
}



int MmsSession::isStreaming()
{
  return this->findStreamingOperation() != NULL;
}



int MmsSession::isClientDisconnecting()
{
  // The session should be aware of its client. Once we rectify that 
  // oversight we should revisit this code and simplify it.
  Op* op = this->first();
  return op? this->sessionMgr->isClientDisconnecting(op->flatmap()): FALSE;
}



int MmsSession::Op::isClientDisconnecting()
{
  return session? sessionManager()->isClientDisconnecting(flatmap()): FALSE;
}



int MmsSession::isRunningDuplexOperation()
{
  // Determine if session is hosting any operations which listen 
  // bi-directionally on the HMP bus. Return count of such ops.

  ACE_Guard<ACE_Thread_Mutex> x(this->optableLock);
  MmsSession::Op* op = &this->optable[0];
  int count=0;

  for(int i=0; i < MMS_MAX_SESSION_OPERATIONS; i++, op++) 
  {
      if (!op->isBusy()) continue;

      switch(op->cmdID())
      {
        case COMMANDTYPE_PLAY:
        case COMMANDTYPE_PLAYTONE:
             count++;
             break;
        case COMMANDTYPE_VOICEREC:             
             if (!op->isNoPromptVoiceRec())  // voicerec w/o prompt is one-way
                  count++;
             break;
      }
  } 

  return count;
}



int MmsSession::Op::cacheConnectionTag()
{
  // Caches the "tag" arriving with connect parameters, and which is to  
  // be returned to client. Currently this is specific to the app server 
  // routing GUID, but we need to change this to be a general tag item.
  int result = 0;

  if (this->Config()->media.rfc2833Enable)
  {    
      char* prguid = 0; 
      const int guidlen = parameterMap.find(MMSP_ROUTING_GUID, &prguid);

      if (guidlen && prguid && !session->routingGuid)
      {
          session->routingGuid = new char[guidlen];
          ACE_OS::memcpy(session->routingGuid, prguid, guidlen); 

          if (!session->isRfc2833Registered())
          {
              registerForRfc2833EventNotification();
              this->toggleRfc2833Event(true);
          }
      }
  }

  return result;
}



int MmsSession::Op::setDigitsReceivedReturn(MmsDeviceVoice* deviceVoice, int returndigitcount)   
{
  // Copies collected digits into parameter map area identified by 
  // flatmap key MMS_PARAM_RECEIVE_DIGITS_RETURN_BUFFER
  // Client will determine the number of digits collected by querying
  // the null-terminated string length of this map parameter. 
  // This method returns the number of digits collected and returned,
  // or -1 if the return parameter was not present in the map.
  #define MMS_MAXGETDIGITS 31

  char* returnDigitBuf = NULL;

  ACE_Guard<ACE_Thread_Mutex> x(session->slock);
                                            // Locate digit return area in map
  int  buflength = parameterMap.find(MMSP_RECEIVE_DIGITS_RETURN_BUFFER, &returnDigitBuf); 
  if ((buflength < 1) || !returnDigitBuf) return -1;

  // char* receivedDigitBuf = deviceVoice->digitBuffer();
  // if  (!receivedDigitBuf) return 0;
  // int   receivedDigitCount = ACE_OS::strlen(receivedDigitBuf);

  // const int maxCopyCount = buflength-1;

  // if   (receivedDigitCount == 0) return 0;
  // if   (receivedDigitCount > MMS_MAXGETDIGITS 
  //    || maxCopyCount < receivedDigitCount)
  //       MMSLOG((LM_ERROR,"%s digit count/map buffer conflict\n", logkey));
                                          // Copy digits into parameter map 
  // ACE_OS::strncpy(returnDigitBuf, receivedDigitBuf, maxCopyCount);   

  // Return cached digit buffer instead of HMP digit buffer 
  // We may not want to return all cached digits
  const int maxCopyCount = returndigitcount == 0 ? buflength-1 : returndigitcount;  
  int receivedDigitCount = this->session->digitCacheSize();
  if (receivedDigitCount == 0) return 0;

                                          // Copy digits into parameter map 
  ACE_OS::strncpy(returnDigitBuf, this->session->digitCache, maxCopyCount);

  return receivedDigitCount;
}



int MmsSession::Op::handleMmsTerminatingConditionsMet()   
{
  // Determine if termination conditions defined by media server (not hmp)
  // were met before the current command was launched, and if so, cause
  // the executing command to terminate. We first yield the processor so 
  // as to permit any prior provisional result to complete.  

  if (this->flags & MMS_OP_FLAG_DIGPATTERN_TERM_MET)
  {
      mmsYield();
      if (session) session->logDigitPatternTermination(); 
      this->markDigitPatternComplete();     // Trigger "digitpattern" result
      this->cancelMedia(MMS_RETAIN_EVENT);  // Terminate command, allowing 
      return MMS_DIGIT_PATTERN_TERMCOND_MET;// termination event to fire
  }

  if (this->flags & MMS_OP_FLAG_HMP_TERM_MET)
  {
      mmsYield();                            
      return MMS_HMP_TERMCOND_MET;                              
  }

  return 0;
}


                                             
int MmsSession::Op::setTermReasonReturn(MmsDeviceVoice* deviceVoice)    
{
  // Copies the device operation termination reason bitflags    
  // into the parameter map header slot reserved for this purpose.

  // Termination reason are at this time specific to HMP. The bit masks for
  // termination reason extraction are defined in HMP header dxtables.h
  // Termination reason mask bits are shown following for reference:

  // TM_NORMTERM           0x00000  // Normal Termination 
  // TM_MAXDTMF            0x00001  // Max Number of Digits Recd 
  // TM_MAXSIL             0x00002  // Max Silence 
  // TM_MAXNOSIL           0x00004  // Max Non-Silence 
  // TM_LCOFF              0x00008  // Loop Current Off 
  // TM_IDDTIME            0x00010  // Inter Digit Delay 
  // TM_MAXTIME            0x00020  // Max Function Time Exceeded 
  // TM_DIGIT              0x00040  // Digit Mask or Digit Type Term.
  // TM_PATTERN            0x00080  // Pattern Match Silence Off 
  // TM_USRSTOP            0x00100  // Function Stopped by User 
  // TM_EOD                0x00200  // End of Data Reached on Playback
  // TM_METREOS_AUTOSTOP   0x00400  // Metreos-defined term cond 
  // TM_METREOS_DIGPATTERN 0x00800  // Metreos-defined term cond 
  // TM_METREOS_TIMEOUT    0x01000  // Metreos-defined term cond
  // TM_TONE               0x02000  // Tone On/Off Termination 
  // TM_BARGEIN            0x08000  // Play terminated due to Barge-in 
  // TM_ERROR              0x80000  // I/O Device Error 
  // TM_MAXDATA           0x100000  // Max Data reached for FSK  

  // Note that TM_METREOS_AUTOSTOP does not come through here, since
  // that reason accompanies ad-hoc final result generation.

  char* map = this->flatmap();      // Session may time out in debugger
  if (!Mms::isFlatmapReferenced(map,5,sessionID(),opid)) return -1; 

  const unsigned int terminationReason 
     = this->isCompleteDigitPattern()? TM_METREOS_DIGPATTERN: 
       this->isCommandTimeout()?       TM_METREOS_TIMEOUT:
       deviceVoice->terminationReason();

  putMapHeader(setTermReason, terminationReason);

  MmsConfig* config = session? session->Config(): NULL;

  if (config && config->serverLogger.globalMessageLevel >= 3)
  {                                         // Digit pattern logged elsewhere
      if (terminationReason != TM_METREOS_DIGPATTERN)  
          session->logTerminatingCondition(terminationReason);

      if (config->diagnostics.flags & MMS_DIAG_LOG_DIGBUF)  
          switch(terminationReason)
          { case TM_MAXDTMF:
            case TM_DIGIT:
            case TM_METREOS_DIGPATTERN:
                 deviceVoice->logDigitBuffer();
          }
  }

  return 0;
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Voice recognition
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

                                            // Create ASR channel
MmsAsrChannel* MmsSession::Op::createAsrChannel()
{
  if(!this->asrChan)                        // Ask AR engine to reserve an ASR channel
  {
      this->asrChan = Asr::instance()->createChannel(session->sessionID(), opID());

      // Burn a CSP license unless pre-reserved (which it always will have been)
      if (this->asrChan && !this->isCspResourceReserved())        
          MmsAs::csp(MmsAs::RESX_DEC);      // LICX CSP- (2 of 2) 
  }

  return this->asrChan;
}

   
                                            
int MmsSession::Op::activateAsrChannel(mmsDeviceHandle handle)
{
  // Create ASR channel for this session
  if (!this->asrChan)
      createAsrChannel();  

  if (Asr::instance()->activateChannel(this->asrChan, handle) == 0)
  {
      this->loadGrammarList();  // load grammar list for channel and start channel

      Asr::instance()->start(this->asrChan);

      this->flags |= MMS_SESSION_FLAG_ASR_ACTIVE;
  }
  else return -1;

  return 0;
}



int MmsSession::Op::handleEventVoiceRecPromptDone(MmsEventRegistry::DispatchMap* dispatchMap) 
{ 
  if (this->asrChan)
  {
    this->startVoicerecCompThread();
    // Notify VR engine that prompt is done, so listen up to the audio stream from user
    Asr::instance()->promptDone(this->asrChan);
  }
            
  // The VR command is not done yet, keep waiting on the streaming part
  return MMS_COMMAND_WAITING;
}



void MmsSession::Op::startVoicerecCompThread()
{
  MmsDeviceVoice* deviceVoice = this->assignedVoiceDevice(); 
  if (deviceVoice && !this->isVoiceBargeIn())
      deviceVoice->startStreaming();

  // Post message to session manager then session pool in order to start VR computation
  // thread for the ASR channel in this session
  Asr::instance()->m_task->postMessage(MMSM_START_COMPUTATION_THREAD, (long)this);
}



int MmsSession::Op::startComputation()
{
  // Start VR computation thread for this ASR channel.
  // The computation thread stops when one of the following conditions met:  
  // 1. VR failed in computation function
  // 2. Has a successful result
  // 3. Has a failure result
  // 4. VR engine reachs max CPU time
  // 5. VR engine reachs max speech
  // 6. VR engine stopped

  if (this->asrChan)
      Asr::instance()->computeData(this->asrChan);

  // cancelAsyncEventNotification(0xE0 /*TEC_STREAM*/);

  unsigned long termreasons = getFlatmapTermReason(this->flatmap());
  bool streamTerminated = false;
  if (termreasons & TM_MAXTIME || termreasons & TM_MAXSIL || termreasons & TM_MAXNOSIL)  
      streamTerminated = true;

  // Ask VR engine for last recognition result on this channel
  int score = -1;
  char *answer = new char[MMS_SIZEOF_VR_MEANING];
  ACE_OS::memset(answer, 0, sizeof(answer));

  if (this->asrChan)
      Asr::instance()->getAnswer(this->asrChan, &answer, &score);

  if (score <= 0)
  {
      score = 0;
      ACE_OS::strcpy(answer, "NULL");
  }

  MMSLOG((LM_DEBUG, "%s recognized meaning length %d, score %d\n", 
          ASR_TASK, ACE_OS::strlen(answer), score));
                                            
  char* returnBuf = NULL;                   // Locate reserved vr meaning area in map
  int buflength = parameterMap.find(MMSP_VR_MEANING, &returnBuf); 
  if (buflength >= 1 && returnBuf)          // Copy VR result meaning into map
      ACE_OS::strncpy(returnBuf, answer, MMS_SIZEOF_VR_MEANING-1);
  
  // JDL still need to verify why long answer can trigger access violation.
  if (answer) delete[] answer;

  char* pScore = 0;                         // Locate and put VR result socre into map
  parameterMap.find(MMSP_VR_SCORE, &pScore);
  if (pScore) 
    *(int*)pScore = score;

  if (!this->isCommandTimeout() && !session->isDisconnecting() && !streamTerminated)
  {
      // Notify VR done for session and result available
      MmsSessionManager::SuperTaskParams* params = NULL;
      params = new MmsSessionManager::SuperTaskParams;        
      params->sessionManagerTask = MmsSessionManager::SuperTaskParams::VOICEREC_DONE;
      params->session = session;
      setFlatmapRetcode(flatmap(), 0);
      params->flatmap = flatmap();
      params->operationID = this->opID();
      params->result = 0;
      Asr::instance()->m_task->postMessage(MMSM_SERVICEPOOLTASKEX_RETURN, (long)params);
  }

  return 0;
}



int MmsSession::Op::loadGrammarList()
{
  // Load grammar list comes with VR command in VR engine.
  char* grammarlistFlatmap = 0;
  int result = parameterMap.find(MMSP_GRAMMARLIST, &grammarlistFlatmap);
  if (!grammarlistFlatmap) 
      return 0;

  int numgrammars = 0;
  MmsFlatMapReader grammarlistMap(grammarlistFlatmap);

  while(1)
  {                                             // Get next occurrence of 
    char* grammarspecFlatmap = 0;               // MMSP_GRAMMARSPEC within map
    result = grammarlistMap.find(MMSP_GRAMMARSPEC, &grammarspecFlatmap, NULL, numgrammars+1);
    if (!grammarspecFlatmap) 
      break;
                                                // Extract grammar file
    MmsFlatMapReader grammarspecMap(grammarspecFlatmap);
    int grammarnamelength = 0;
    char* grammarname = 0; 

    grammarnamelength = grammarspecMap.find(MMSP_GRAMMARNAME, &grammarname);
    if (!grammarname || !(*grammarname)) 
      continue; 

    numgrammars++;                             // Load grammar into VR engine
    MMSLOG((LM_DEBUG,"%s loading grammar %d - %s\n", session->objname, numgrammars, grammarname));
    Asr::instance()->loadGrammar(this->asrChan, "uri", grammarname);
  }

  return numgrammars;
}



int MmsSession::Op::toggleRfc2833Event(const int isEnabling)
{  
  MmsDeviceIP* deviceIP = session->ipDevice(); if (!deviceIP) return -1;

  const int result = deviceIP->toggleRfc2833Events(isEnabling);

  if  (!isEnabling)
       session->registerRfc2833(FALSE);
  else 
  if  (result == 0)
       session->registerRfc2833(TRUE);

  return result;
}



void MmsSession::dumpOpTable()
{
  ACE_Guard<ACE_Thread_Mutex> x(this->optableLock);
  MmsSession::Op* op = &this->optable[0];

  for(int i=0; i < MMS_MAX_SESSION_OPERATIONS; i++, op++)  
  {
      if (op->opID() != 0)
          MMSLOG((LM_DEBUG,"%s optable id=%d cmd=%d\n", objname, op->opID(), op->cmdID()));
  }
}



int MmsSession::Op::isVoiceBargeIn()
{
  char* p = 0;
  int isBarge = 0;
  parameterMap.find(MMSP_VOICE_BARGEIN, &p);
  if (p) isBarge = *((int*)p);
  return isBarge;
}



int MmsSession::Op::isCancelOnDigit()
{
  char* p = 0;
  int isCancelOnDigit = 0;
  parameterMap.find(MMSP_CANCEL_ON_DIGIT, &p);
  if (p) isCancelOnDigit = *((int*)p);
  return isCancelOnDigit;
}



int MmsSession::Op::isNoPromptVoiceRec()
{   
  if (this->cmdid != COMMANDTYPE_VOICEREC) return FALSE;
  char* filelistFlatmap = NULL;
  this->parameterMap.find(MMSP_FILELIST, &filelistFlatmap);
  return (filelistFlatmap == NULL);
}



int MmsSession::handleDigitLingering(MmsMediaDevice* deviceVoice, ListenDirection dir)
{
  int result = 0;
  MmsDeviceIP* ipDevice = this->ipDevice();

  if (ipDevice)
  {
      const int mode = dir == FULLDUPLEX? MmsMediaDevice::FULLDUPLEX: MmsMediaDevice::HALFDUPLEX;
      ipDevice->unlisten();
      result = ipDevice->busConnect(deviceVoice, mode); 
  }

  return result;
}



int MmsSession::Op::raiseVoiceRecResourcesExhaustedAlarm()
{
  // Fire an alarm indicating CSP resources exhausted, also log the condition

  char alarmDescription[MmsAlarmParams::maxTextLength+1];
  sprintf(alarmDescription, MmsAs::szResxExhausted, "AVR (CSP)");

  MMSLOG((LM_ERROR,"%s %s\n", logkey, alarmDescription));

  MmsAlarmParams* params = new MmsAlarmParams(MmsReporter::NDX_NO_MORE_ASR_RESX, 
     MMS_STAT_CATEGORY_CSP, MmsReporter::severityTypeSevere);
  params->settext(alarmDescription);

  MmsReporter::instance()->postMessage(MMSM_ALARMREQUEST, (long)params);

  return -1;
}


