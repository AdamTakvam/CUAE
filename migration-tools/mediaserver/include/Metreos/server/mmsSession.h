//
// mmsSession.h  
//
// The concept of a session, as it translates to our media processing
// environment, exists for the duration of an HMP IP connection.
//
#ifndef MMS_SESSION_H
#define MMS_SESSION_H
#ifdef  MMS_WINPLATFORM
#pragma once 
#pragma warning(disable:4786)
#endif
#include <set>
#include <minmax.h>
#include "ace/Hash_Map_Manager.h"
#include "mms.h"
#include "mmsConfig.h"
#include "mmsFlatMap.h"
#include "mmsMediaEvent.h"
#include "mmsDeviceIP.h"
#include "mmsDeviceVoice.h"
#include "mmsDeviceConference.h"
#include "mmsServerCmdHeader.h"
#include "mmsMediaResourceMgr.h"
#include "mmsCallStateMonitor.h"
#include "mmsAudioFileDescriptor.h"
#include "mmsTts.h"
#include "mmsAsrChannel.h"

#define MMS_RECORDFILENAMESIZE 8
#define MMS_RECORD_PROPERTIES_FILE_EXTENSION ".mms"
#define MMS_COMMAND_EXECUTING MMS_ERROR_NOERROR 
#define MMS_COMMAND_WAITING   MMS_ERROR_NOERROR2 
#define DIGITPATTERN_COMPLETE '-'
#define MMS_SIZE_DIGITLIST 16
#define MMS_SIZE_DIGITCACHE 31

#define MMS_RETAIN_EVENT       0
#define MMS_CANCEL_EVENT       1
#define MMS_CANCEL_EVENT_NOLOG 2

#define MMS_DIGIT_PATTERN_TERMCOND_MET 1
#define MMS_HMP_TERMCOND_MET           2

#define MMS_ADJPLAY_LOG              0x1
#define MMS_ADJPLAY_INHERIT_ADJTYPE  0x2


#define MMS_SESSION_FLAG_SESSION_ACTIVE              0x1  
#define MMS_SESSION_FLAG_HANDLING_COMMAND            0x2 
#define MMS_SESSION_FLAG_TEARDOWN_IN_PROGRESS        0x4
#define MMS_SESSION_FLAG_ABANDON_IN_PROGRESS         0x8

#define MMS_SESSION_FLAG_EVENT_FIRED                0x10
#define MMS_SESSION_FLAG_HANDLING_EVENT             0x20
#define MMS_SESSION_FLAG_EVENT_HANDLER_DONE         0x40
 
#define MMS_SESSION_FLAG_HALF_CONNECTED            0x100 
#define MMS_SESSION_FLAG_IS_CONFERENCE_MONITOR     0x200 
#define MMS_SESSION_FLAG_IS_CODER_RESERVED         0x400  
#define MMS_SESSION_FLAG_IS_DISCONNECTING          0x800

#define MMS_SESSION_FLAG_IS_ASYNC_CONNECT_PENDING 0x1000 
#define MMS_SESSION_FLAG_RETRANSMIT_CONNECTION    0x2000 
#define MMS_SESSION_FLAG_IS_EXTOP_EXECUTING       0x4000
#define MMS_SESSION_FLAG_ASR_ACTIVE               0x8000

#define MMS_SESSION_FLAG_G711_RESOURCE_SPENT     0x10000
#define MMS_SESSION_FLAG_G729_RESOURCE_SPENT     0x20000

#define MMS_SESSION_FLAG_EXIT_LOGGED            0x100000
#define MMS_SESSION_FLAG_EXIT_SUPRESSED         0x200000


#define MMS_OP_FLAG_COMMAND_INITIALIZED              0x1 
#define MMS_OP_FLAG_EVENT_HANDLED                    0x2 
#define MMS_OP_FLAG_BLOCK_REQUESTED                  0x4
#define MMS_OP_FLAG_DIGITBUFFER_CLEARED              0x8

#define MMS_OP_FLAG_DIGPATTERN_TERM_SET             0x10
#define MMS_OP_FLAG_DIGPATTERN_TERM_MET             0x20 
#define MMS_OP_FLAG_HMP_TERM_MET                    0x40
#define MMS_OP_FLAG_CMD_TIMEOUT                     0x80

#define MMS_OP_FLAG_VOICE_RESOURCE_SPENT           0x100
#define MMS_OP_FLAG_CSP_RESOURCE_RESERVED          0x200


#define MMS_MAX_SESSION_OPERATIONS 3

#define MMS_OPERROR_OPTABLE_OVERFLOW  (-1)
#define MMS_OPERROR_NO_SUCH_OPERATION (-2)

class  MmsSessionManager;
class  MmsConferenceManager;
class  MmsTtsSessionData;
class  MmsReconnectInfo;
struct MmsCurrentCommand;
struct MmsStopMediaParams;
struct MmsPlaylistParams;
struct MmsLocaleParams;
struct MMSPLAYFILEINFO; 
struct MmsWaitInfo { int eventType; int eventID; };
                                            // Conx ID to session ID hash
typedef ACE_Hash_Map_Manager_Ex<int, int, ACE_Hash<int>, 
        ACE_Equal_To<int>, ACE_Null_Mutex> CONNECTIONID_HASHMAP;
         
typedef int MmsOperationID;                  
typedef int MmsConnectionID;                 



class MmsSession
{
  friend class MmsServerManager; 
  friend class MmsSessionManager;
  friend class MmsSessionPool;
  friend class MmsThreadPool;
  friend class MmsReconnectInfo;

  enum states{DOWN, AVAIL, IDLE, BUSY, PEND}; 

  public:  
  int   sessionID()         { return this->ordinal; }
  int   connectionID()      { return this->conxID;  }
  int   operationCount()    { return this->opcount; }
                                             
  int   isInUse()           { return this->state > AVAIL; }
  int   isBusy()            { return this->state > IDLE;  }
  void  markIdle()          { this->state = IDLE; }
  void  markInUse(int b=1)  { this->state = b? IDLE: AVAIL; }
  void  stamp()             { time(&this->timestamp); }
  int   isUtilitySession()  { return isValidDeviceHandle(this->hIP) == 0; }
  unsigned int& Flags()     { return flags; }
  int   isAsyncConnectPending() { return flags & MMS_SESSION_FLAG_IS_ASYNC_CONNECT_PENDING; }
  int   isExternalOpExecuting() { return flags & MMS_SESSION_FLAG_IS_EXTOP_EXECUTING; }
  int   isExitLogged()          { return flags & MMS_SESSION_FLAG_EXIT_LOGGED;    }
  int   isExitSupressed()       { return flags & MMS_SESSION_FLAG_EXIT_SUPRESSED; }
  void  markExitLogged()        { flags |= MMS_SESSION_FLAG_EXIT_LOGGED;    }
  void  markExitSupressed()     { flags |= MMS_SESSION_FLAG_EXIT_SUPRESSED; }
  int   isHandlingCommand(); 
  int   isStreaming();
  int   isCallStateMonitoring();
  int   isClientDisconnecting();
  int   isRunningDuplexOperation();
  void  markHandlingCommand(int b=1);
  int   isSessionClosed();
                                            
  mmsDeviceHandle ipResource()       { return this->hIP;  }
  void ipResource(mmsDeviceHandle h) { this->hIP  = h; }
  MmsMediaDevice* getMediaDevice(mmsDeviceHandle hdev);
  MmsDeviceIP*    ipDevice();
  MmsDeviceConference* conferenceDevice();
  HmpResourceManager*  resourceManager() { return this->resourceMgr; }    
  void  releaseResources(); 
  void  releaseSessionVox(); 
  enum  ListenDirection { FULLDUPLEX, HALFDUPLEX };  
          
  MmsSession();
  virtual ~MmsSession();             
  int   clear();                             
  void  clearCommon();
  void  init(const int ordinal, MmsConfig*);                       
  void  resetSessionTimer(int secs=0); 
  int   countdownSession(); 
  int   isInfiniteSessionTimeout()     { return this->sessionTimeoutSecs == 0;}
  char* name() { return this->objname; }
  MmsConfig* Config() { return config; }
  MmsSessionManager* sessionManager()  { return this->sessionMgr; }
                                            
  int   onSessionStart();                    
  void  onAssignSessionTimeout(int timeoutsecs);
  int   onCommandStart(char* flatmap, void** outOp=0); 
  void  onSessionEnd();

  int   handleSendDigits(const char* flatmap, const int mode = MmsMediaDevice::ASYNC); 
  int   stopAllOperations(MmsTask* serverMgr, const int exceptOperation=0); 
  int   stopOperation
         (const int operationID, MmsTask* serverMgr, const int isAsync, const int islock); 
  int   stopStreamingOperation(const int mode = MmsMediaDevice::SYNC);

  int   lockVoxAll(const int isLocking=TRUE, const int needLock=TRUE);

  int   handleConferenceConnect (MmsFlatMapReader&);   
  int   handleConferenceDisconnect();                                               
  int   setDigitsReceivedReturn (MmsDeviceVoice*);             
  int   getDigitMaskParameters  (MmsDeviceVoice*, MmsFlatMapReader&, unsigned short&);
  int   queryStopMediaParameters(MmsStopMediaParams&);
  int   selfListen(char* flatmap);
  static ListenDirection getListenDirectionByCommandID(const int commandID);

  int   getRecordFileExpiration();
  int   getPlayRecordParameters(MmsDeviceVoice::MMS_PLAYRECINFO& playrecinfo, 
        MMSPLAYFILEINFO&, int isPlay=1);
  int   addToPlaylist(MmsPlaylistParams&, MmsDeviceVoice*);
  int   buildPlayfileFullPath(char* fullpath, char* subpath, 
          char* appname, char* locale, const int isRecord, const int isTts=0);
  int   buildPlayfileFullPath(char* fullpath, char* subpath, 
          MmsLocaleParams&, const int isRecord, const int isTts=0);
  int   buildPlayfileSubpath();
  int   makePropertiesFilePath(MMSPLAYFILEINFO*);
  int   patternMatchDigitsEx(char* incoming);
  int   handleDigitLingering(MmsMediaDevice*, ListenDirection);

  int   isRfc2833Registered() { return rfc2833Registered; } 
  void  registerRfc2833(int enable) { rfc2833Registered = enable; }
  int   handleRfc2833SignalReceived(MmsEventRegistry::DispatchMap*, MmsTask*);
  int   postPushRfc2833Digit(MmsTask* serverMgr, const int digit);

  static void createFilename(char* buf, const int ft=MMS_FILETYPE_VOX, 
              const int isNamepartPresent=0);
  static int  removeRecordFile(char* path);
  static int  getFormatAttribute  (const int bitstring);
  static int  getRateAttribute    (const int bitstring);
  static int  getSamplesizeAttribute    (const int bitstring);
  static int  getFiletypeAttribute(const int bitstring);
  int         getFileTypeParameter();
  static void dumpParameterMap(void*);

  int  setDefaultConnectAttributes  
      (unsigned int& rattrs, unsigned int& lattrs, const int isHalfConnect=0);
  int  setDefaultReconnectAttributes(unsigned int& rattrs, unsigned int& lattrs); 
  int  isRemoteConnectionAttributeChange
      (const unsigned newRemoteAttrs, const unsigned currentRemoteAttrs, const int log);
 
  int  coderStringToMms(const char* szcoder); 
  int  framesizeToMms(const int framesizevalue);
  int  writeDescriptorFile
      (MmsDeviceVoice::MMS_PLAYRECINFO&, MMSPLAYFILEINFO*, const int exp=0); 
  int  setTerminationIf(MmsDeviceVoice*, MMS_DV_TPT_LIST&, DV_TPT& condition); 
  int  adjustPlay(char* flatmap, const int isLog=TRUE);
  DV_TPT* findTerminationCondition(MMS_DV_TPT_LIST&, int condition);

  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  // Session operations
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

  class Op                                  // Operation object
  {    
    friend class MmsSession;

    public:
    enum states { IDLE, RSVD, WAIT, LIVE, PEND };
    int  isBusy() { return state  > RSVD; }
    int  isIdle() { return state == IDLE; }
    int  isInit() { return (flags & MMS_OP_FLAG_COMMAND_INITIALIZED) != 0; }
    int  State()  { return state; }
    int  setState(const states state);
    void reserve(){ state = RSVD; }

    void  init(MmsSession*, const int opID);
    int   opID()       { return this->opid; } 
    int   cmdID()      { return this->cmdid;}
    int   sID()        { return session->sessionID(); }
    int   sessionID()  { return session->sessionID(); }

    char* flatmap()    { return (char*)this->parameterMap.header(); }
    MmsFlatMapReader& mapReader() { return this->parameterMap; }
    void  putMapHeader(const int operation, const int value);

    MmsSession* parent()     { return session;  }
    MmsSession* Session()    { return session;  }
    MmsWaitInfo& waitinfo()  { return waitInfo; }  
    int   isWaiting()        { return waitInfo.eventID != 0; }
    int   isStreaming()      { return this->asrChan != NULL; }
    void  markWaiting(int type, int id=0);
    int   IsMapFlagSet(const unsigned int bitflag);
    int   isInServiceThread(){ return this->svcThreadHandle != 0; }
    int   isClientDisconnecting();
    MmsConfig* Config()      { return session->Config(); }
  
    void  clear();
    int   countdownOperation();
    void  idleResources();
    void  releaseResources();
    int   voiceResourceRelease(); 
    int   isInfiniteOperationTimeout(){ return this->commandTimeoutMs == 0;  }
    int   wasEventCanceled();                                                                   
                                            // Operation voice resource 
    MmsDeviceVoice* voiceDevice(const int isConnect, const int isAcquire, 
          const ListenDirection ld=FULLDUPLEX, const int capabilities=0);
    mmsDeviceHandle hVoice()       { return hvox; }
    void  hVoice(mmsDeviceHandle h){ hvox = h;    }
    MmsDeviceVoice* assignedVoiceDevice();
    MmsDeviceVoice* voiceResourceAcquire(const int capabilities=0);
    int   voiceResourceIdle();
                                            // Command handlers
    int   handleConnect(MmsCurrentCommand&);                    
    int   handleDisconnect(MmsCurrentCommand&);
    int   handlePlay(MmsCurrentCommand&);    
    int   handleRecord(MmsCurrentCommand&);  
    int   handleReceiveDigits(MmsCurrentCommand&); 
    int   handleRecordTransaction(MmsCurrentCommand&);
    int   handlePlaytone(MmsCurrentCommand&);
    int   handleAdjustments(MmsCurrentCommand&);
    int   handleMonitorCallState(MmsCurrentCommand&);
    int   handleConferenceResourcesRemaining(MmsCurrentCommand&);
    int   handleConferenceEnableVolumeControl(MmsCurrentCommand&);
    int   handleConfereeSetAttribute(MmsCurrentCommand&);
    int   handleSendDigits(const char* flatmap, const int mode = MmsMediaDevice::ASYNC); 
    int   handleStopOperation(MmsCurrentCommand&);
    int   handleVoiceRec(MmsCurrentCommand&);
    int   stopMediaOperation(int mode = MmsMediaDevice::ASYNC);
    int   stopVoiceRecOperation(int mode = MmsMediaDevice::ASYNC);

    int   adjustPlay(const int isLog=0);
    int   adjustPlay(const unsigned int packed, const int isLog=0);
    int   adjustPlayPerConfig(MmsDeviceVoice* deviceVoice, const int isLog=0);
    int   adjustPlay(char* flatmap, const int isLog=1);
    int   adjustPlay(MmsVolumeSpeedEncoder& coder, const int flags=1);
                                            // Termination event handler
    int   handleEventVoice(MmsEventRegistry::DispatchMap*);
    int   handleEventVoxDigitsReceived   (MmsEventRegistry::DispatchMap*);
    int   handleEventIpDigitsReceived    (MmsEventRegistry::DispatchMap*);
    int   handleEventCallStateTransition (MmsEventRegistry::DispatchMap*); 
    int   handleEventIpStopped  (MmsEventRegistry::DispatchMap*); 
    int   handleEventStartMedia (MmsEventRegistry::DispatchMap*); 
    int   handleCstTimeExpired();  
    int   handleMmsTerminatingConditionsMet(); 
    int   handleEventVoiceRecPromptDone(MmsEventRegistry::DispatchMap*);

    void  onCommandStart(char* newflatmap); // State transtition handlers
    void  onCommandServiceStart(const int defaultCommandTimeoutMs=0);
    void  onCommandEnter(int isExecutedAsync=0);
    void  onEventSink();
    void  onServiceThreadEntry();
    int   onEventServiceStart(char* which, const int reset=TRUE); 
    void  onServiceReturn(); 
    void  onServiceThreadExit();
    void  onCommandEnd(const int commandID=0);
                                            // Command parameter handlers
    int   getVolumeSpeedParameters(MmsVolumeSpeedEncoder&, char* flatmap=0);  
    int   getVolumeSpeedParameters(MmsVolumeSpeedEncoder&, MmsFlatMapReader&);
    int   isHmpTerminationMet() { return (flags & MMS_OP_FLAG_HMP_TERM_MET) != 0; }
    int   preparePlaylist(); 
    int   openPlaylist(MmsDeviceVoice*, MMSPLAYFILEINFO* p=0, const int isRecord=0);
    int   getTerminationConditionParameters(MmsDeviceVoice*, MMS_DV_TPT_LIST&);
    int   getPlayRecordParameters(MmsDeviceVoice::MMS_PLAYRECINFO&, MMSPLAYFILEINFO&, const int isPlay=1);
    int   getDigitListParameters (MmsDeviceVoice*, MmsFlatMapReader&);
    int   getLocaleParameters(MmsLocaleParams&);
    int   registerForAsyncEventNotification(const int eventType, const int isIP=0);
    int   cancelAsyncEventNotification     (const int eventType, const int isIP=0,
          const int eventDisposition=MMS_CANCEL_EVENT);
    int   registerForRfc2833EventNotification();
    int   cancelRfc2833EventNotification();
                                            // Operation utility methods
    int   cancelMedia(const int eventDisposition=MMS_RETAIN_EVENT, const int isAsync=1); 
    int   resetOperationTimer(int ms=0);  
    int   isDigitBufferCleared() { return (flags & MMS_OP_FLAG_DIGITBUFFER_CLEARED) != 0; }
    void  setCommandTimeout()    { flags |= MMS_OP_FLAG_CMD_TIMEOUT; }
    void  markDigitPatternComplete(){ if (flags & MMS_OP_FLAG_DIGPATTERN_TERM_SET) *digitlist = DIGITPATTERN_COMPLETE;}
    int   setDigitsReceivedReturn(MmsDeviceVoice* deviceVoice, int returndigitcount = 0);
    int   setTermReasonReturn(MmsDeviceVoice* deviceVoice);   
    int   setElapsedTimeReturn(MmsDeviceVoice*);
    int   isVoiceMediaPlaying(const int isIgnoreWait=0);
    int   isMediaParameterChangeRequest();
    int   isConferenceOperation();
    int   onRfc2833Disconnect();

    int   loadGrammarList();
    MmsAsrChannel* createAsrChannel();                            // Create ASR channel
    int   activateAsrChannel(mmsDeviceHandle handle);             // Enable ASR channel
    int   isAsrChannelActive() { return (flags & MMS_SESSION_FLAG_ASR_ACTIVE) != 0; } 
    void  startVoicerecCompThread();
    int   startComputation();
    int   toggleRfc2833Event(const int enable);
    int   isNoPromptVoiceRec();
    MmsAsrChannel* asrChan;                 // ASR channel object to handle VR activities

    MmsTtsSessionData* ttsData;
    void  stamp() { time(&this->timestamp); }
    ACE_Thread_Mutex dataLock;              // Lock operation instance data

    protected:
    int   opid;
    int   cmdid;
    int   state;
    int   commandTimeoutMs;                    
    unsigned int     flags;
    unsigned int     tickstart, matchcount;  
    MmsWaitInfo      waitInfo;              // Registered wait event ID and type 
    MmsSession*      session;
    mmsDeviceHandle  hvox;
    ACE_hthread_t    svcThreadHandle;
    MmsFlatMapReader parameterMap;    
    time_t           timestamp;                       
    MmsManualTimer   opTimeoutTimer;
    MmsSessionManager* sessionManager() { return session->sessionMgr; }
    MmsDeviceVoice*  raiseVoxExhaustedAlarm();
    int   raiseVoiceRecResourcesExhaustedAlarm();
    int   raiseG711ExhaustedAlarm();

    char  digitlist[MMS_SIZE_DIGITLIST];    // Monitored digit pattern or list
    char  logkey[5];
    char* logKey()  { return logkey; }    

    int   getFileTypeParameter();
    int   isTwoWayListen(MmsDeviceVoice*);
    int   patternMatchDigits(char* incoming);
    char* getDigitsImmediate(MmsDeviceVoice*, MMS_DV_TPT_LIST* tpt=0, int* outcond=0); 
    int   isDigitTerminationPreexisting(MmsDeviceVoice*, MMS_DV_TPT_LIST&);
    int   isWatchingDigitPattern() { return (*digitlist != 0) && (flags & MMS_OP_FLAG_DIGPATTERN_TERM_SET); }
    int   isCompleteDigitPattern() { return (*digitlist == DIGITPATTERN_COMPLETE) && (flags & MMS_OP_FLAG_DIGPATTERN_TERM_SET); }
    int   isCommandTimeout()       { return (flags & MMS_OP_FLAG_CMD_TIMEOUT); }
    int   isCspResourceReserved()  { return (flags & MMS_OP_FLAG_CSP_RESOURCE_RESERVED); }
    int   cacheConnectionTag();
    int   isVoiceBargeIn();
    int   isCancelOnDigit();
  }; // class Op

  Op   optable[MMS_MAX_SESSION_OPERATIONS]; // Session operation table
  Op*  findByOpID(const int opID, const int needLock=TRUE);
  Op*  findByEventID(const int eventID, const int needLock=TRUE);
  Op*  findStreamingOperation();
  Op*  findCallStateOperation();
  Op*  getAvailableOp();
  Op*  first();
  Op*  getOperation(MmsFlatMapReader&, const int isLog=0);
  int  clearOptable(const int isLog=1);
  int  opcount;
  int  voiceResourceCount(const int needLock=TRUE);      
  int  closeOperation(const int opID, const int commandID=0, const int isLog=1);
  int  stopOperation(MmsSession::Op*, MmsTask* serverMgr, const int isAsync); 
  int  getNumActiveOperations(const int voiceOpsOnly=0);
  int  hasOperation(int cmdId);
  void dumpOpTable();
  ACE_Thread_Mutex optableLock; 
 
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  // Session conference
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -     

  struct ConfInfo                           
  { int id, handle, flags;                          
    mmsDeviceHandle confResx;
    enum bitflags { ISMONITOR=1, ISHAIRPIN=2, ISMUTED=4, ISTTONE=8 };
    void clear()  { id = handle = flags = confResx = 0; } 
    void setmonitor(int b=1) {if(b) flags |= ISMONITOR; else flags &= ~ISMONITOR;}
    void sethairpin(int b=1) {if(b) flags |= ISHAIRPIN; else flags &= ~ISHAIRPIN;}
    void setttone  (int b=1) {if(b) flags |= ISTTONE;   else flags &= ~ISTTONE;  }
    void setmuted  (int b=1) {if(b) flags |= ISMUTED;   else flags &= ~ISMUTED;  }
  };

  int   isInConference();
  int   isConferenceParty()    { return confInfo.id && !isUtilitySession(); }
  int   isConferenceMonitor()  { return(confInfo.flags & ConfInfo::ISMONITOR) != 0; } 
  int   isHairpinConference()  { return(confInfo.flags & ConfInfo::ISHAIRPIN) != 0; } 
  int   isMutedConferee()      { return(confInfo.flags & ConfInfo::ISMUTED)   != 0; } 
  int   isRemoteMediaChange(const char* ipaddr, const int port, 
          const unsigned int newRemoteAttrs, const unsigned int currentRemoteAttrs);
  int   isRemoteSessionStarted();
  int   holdRemoteSession(Op*);
  int   switchFromConferenceToVoice(MmsMediaDevice*, ListenDirection dir = FULLDUPLEX);
  int   switchFromHairpinToVoice(MmsMediaDevice*, unsigned int mode);
  int   reconnectToConference(MmsMediaDevice*); 
  int   reconnectToHairpin(); 
  int   reconnectOutOfBand(const int operationID);
  void* getMmsConferenceObject(const int confID);
  void  encodeConferenceAttributes
        (const unsigned int, const unsigned int, unsigned int&, unsigned int&);
  MmsHmpConference* MmsSession::getHmpConferenceObject
        (MmsDeviceConference* deviceConf, const int isLog=1);

  ConfInfo& confinfo() { return confInfo; }
	int  conferenceId()  { return confInfo.id ; }
  int  muteHairpinned(const int isMuting=1);
  int  promoteConference(char* flatmap=0);
  int  demoteConference();

  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  protected:
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -  
  int   ordinal;                            // 1-based session key  
  int   state;                              // Session object state
  int   sessionTimeoutSecs;                 // If no activity in n seconds
  void* client;                             // Adapter address of session 'owner'
  void* clientHandle;                       // Subclient ID e.g. queue handle
  char  digitCache[MMS_SIZE_DIGITCACHE+1];  // For digits cache
  char  objname[5];                         // Log file name of session object
  ConfInfo confInfo;                        // Conference id & conferee handle
  unsigned int remoteIpAttrs, localIpAttrs; // Current IP device configuration
  unsigned int flags;                       // Bit flags
  time_t  timestamp;                        // Session start or end time
  time_t  senddigitstimestamp;              // Send digits time stamp

  mmsDeviceHandle hIP;                      // IP resource handle
  MmsConfig* config;
  MmsSessionManager*   sessionMgr;
  HmpResourceManager*  resourceMgr;
  MmsCallStateMonitor* callStateMon;         
  MmsReconnectInfo*    reconnectInfo;        
  char*   pendingDisconnectMap;
  MmsManualTimer timerSession;

  int  getEffectiveDefaultCommandTimeout(char* flatmap);
  int  isReconnect() { return reconnectInfo != NULL; }
  int  resumeMediaIfReconnect();
  void clearReconnectState();

  void replaceCoderBits(unsigned int&, unsigned int&, const unsigned int); 
  int  verifyCoderAvailabilityEx(const int reserveIfAvailable, const int isLog, char* map);
  int  reserveLowBitrateCoderEx (const int isReserving=TRUE, const int isModifying=TRUE);
  int  isLowBitrateCoder(const unsigned int attrs);
  int  isLowBitrateCoder(const unsigned int remotaeattrs, const unsigned int localattrs);
  unsigned int getDefaultFramesizeForSpecifiedCoder(const unsigned int attrs);
  int  isValidFramesizeForSpecifiedCoder(const unsigned int coderbits, const int frsize);

  void logTerminatingCondition(const unsigned int termcond);
  void logDigitPatternTermination();
  static int wasFilenameAssigned(char* namebuf, MmsFlatMapReader& map);
  int  raiseLowBitrateExhaustedAlarm();

  int  editIpAddress(char* ipAddr, const int isZeroOK=1, const int isEmptyOK=0);
  static void ipParser(const std::string& ip, 
         std::vector<std::string>& octets, const std::string& delims);

  int  isDisconnecting()     { return (flags & MMS_SESSION_FLAG_IS_DISCONNECTING) != 0; }
  int  isCoderReserved()     { return (flags & MMS_SESSION_FLAG_IS_CODER_RESERVED)!= 0; }
  int  isDisconnectCached()  { return this->pendingDisconnectMap != NULL; }
  int  isDigitBufferCleared(){ return (flags & MMS_OP_FLAG_DIGITBUFFER_CLEARED) != 0; }
  int  cacheDigits(const char* digits, const int numdigits);
  int  sendCachedDigits(int voxcount=0, const int clearcache=1);
  int  clearDigitCache(int cleardigitcount = 0);
  int  digitCacheSize()      { return strlen(digitCache); }
  void stampSendDigits()     { time(&this->senddigitstimestamp); }
  void cacheCommandHeader(char* flatmap);
  int  isDigitLingering(); 

  static int isAsync(const int mode)       { return (mode & MmsMediaDevice::ASYNC) != 0; }
  static int isSynchronous(const int mode) { return (mode & MmsMediaDevice::ASYNC) == 0; }

  // Convenience methods for testing media termination event state
  int isTerminationEventBlocked()           // Is termination event waiting at barrier
  { return (flags & MMS_SESSION_FLAG_EVENT_FIRED) 
        & !(flags &(MMS_SESSION_FLAG_HANDLING_EVENT | MMS_SESSION_FLAG_EVENT_HANDLER_DONE));
  }
  int isTerminationEventInHandler()         // Is termination event in event handler
  { return (flags & MMS_SESSION_FLAG_HANDLING_EVENT) != 0;  
  }
  int isTerminationEventProceeding()        // Is termination event proceeding
  { return (flags & MMS_SESSION_FLAG_HANDLING_EVENT) != 0  
        || (flags & MMS_SESSION_FLAG_EVENT_HANDLER_DONE) != 0;
  }                                         // Is termination result handled
  int isTerminationEventReturning()         // and in process of returning to client
  { return (flags & MMS_SESSION_FLAG_EVENT_HANDLER_DONE) != 0;  
  }
  int isTerminationEventBegun()             // Is any termination activity in progress
  { return (flags & (MMS_SESSION_FLAG_EVENT_FIRED | MMS_SESSION_FLAG_HANDLING_EVENT 
                   | MMS_SESSION_FLAG_EVENT_HANDLER_DONE)) != 0;  
  }
  int isTerminationEventClear()             // Is no termination activity in progress
  { return (flags & (MMS_SESSION_FLAG_EVENT_FIRED | MMS_SESSION_FLAG_HANDLING_EVENT 
                   | MMS_SESSION_FLAG_EVENT_HANDLER_DONE)) == 0;  
  }

  ACE_Thread_Mutex slock;                   // Session data access lock
  ACE_Thread_Mutex rlock;                   // Session disconnect wait
  ACE_Thread_Mutex sessionDataLock;         // Lock on session content
  ACE_Thread_Mutex suspendTerminationLock;  // Barrier at media termination handler
  ACE_Thread_Mutex extopCriticalSection;    // External operation critical section
  ACE_Thread_Mutex atomicOperationLock;     // General atomicity guard
  ACE_Thread_Mutex handlingCommandLock;     // Only used in markHandlingCommand  
  ACE_Thread_Mutex threadPoolSerializer;    // Serialize pool ops, event handlers, etc.
  MmsConnectionID  conxID;                                     
  MmsFlatMapReader parameterMap;

  int rfc2833Registered;
  char* routingGuid;
  MmsServerCmdHeader* cmdHeader;
};



class MmsSessionPool
{
  public:

  MmsSessionPool(MmsConfig* config);
  virtual ~MmsSessionPool();
  int  ipPoolSize()        { return this->poolSizeMax;}
  int  utilityPoolSize()   { return this->utilityTableSize;  } 
  int  size()              { return this->sessionTableSize;  }
  int  totalActive()       { return activeSessions.size(); }
  int  ipSessionsActive()  { return activeSessions.size() - utilityPoolActive; }
  int  ipSessionsAvail()   { return ipPoolSize() - ipSessionsActive(); }
  int  utilSessionsActive(){ return this->utilityPoolActive; }
  int  utilSessionsAvail() { return utilityTableSize - utilityPoolActive; }
  ACE_Thread_Mutex& sessionPoolLock() { return m_sessionPoolLock; }

  MmsSession* findAvailableSessionEx(char* map);
  MmsSession* findAvailableUtilitySession();
  MmsSession* findByConnectionID(const int connectionID);
  MmsSession* findBySessionID(const int sessionID);
  MmsSession* raiseG711RtpExhaustedAlarm();
  MmsSession* raiseUtilityPoolExhaustedAlarm();
  
  int  bindSessionToConnection(int sessionID, const void* clientID=NULL); 
  int  bindSessionToConnection(MmsSession* session, const void* clientID=NULL);
  int  returnSessionToAvailablePool(int sessionID);
  int  returnSessionToAvailablePool(MmsSession* session);
  int  generateOperationID();
  int  generateConnectionID(); 
  int  isUtilitySession(char* map) 
  {    return map && isFlatmapFlagSet(map,MmsServerCmdHeader::IS_UTILITY_SESSION);
  }
  void dump(const int ifCountLT=0);

  MmsSession* base() { return sessions; }

  friend class MmsSessionManager;

  protected:
  MmsSessionPool() { }
  char objname[16];
  MmsSession* mruSession;
  int  conxID;

  int  poolSizeMax;
  int  utilityTableSize;
  int  utilityPoolActive;
  int  sessionTableSize;

  std::set<int> activeSessions;             // Set of in-use session IDs

  MmsSession* sessions;                     // Session object pool array

  CONNECTIONID_HASHMAP* connectionIDs;      // Connection ID to session ID map

  ACE_Thread_Mutex m_sessionPoolLock;
  ACE_Thread_Mutex atomicOperationLock;
};



struct MmsPlaylistParams 
{
  int   fileoffset; 
  int   segmentlength; 
  int   isRecordFile;
  int   isTtsText;
  int   pathlength;
  char* fullpath;
  char* subpath;
  char* outpath;
  char* filespecFlatmap;  
  MmsLocaleParams ldata;
  void  clear() { memset(this,0,sizeof(MmsPlaylistParams)); }
  MmsPlaylistParams(const int isRec) { clear(); isRecordFile = isRec; }
};



struct MmsStopMediaParams 
{
  int   operationID;
  int   isSynchronous;
  char* flatmap;
  void  clear() { memset(this,0,sizeof(MmsStopMediaParams)); }
  MmsStopMediaParams(char* m) { clear(); flatmap = m; }
};



class MmsReconnectInfo
{
  // This structure maintains state for a "reconnect" operation, when IP/port/coder
  // information is being changed for a session (such as occurs on a hold/resume).
  // We remember if voice media was active before we hold IP, so that we can relisten
  // to the voice after we subsequently restart IP.
  public:
  MmsSession* session;
  int operationID, unlistenedCount;
  MmsReconnectInfo() { clear(); }

  struct OpInfo
  { int operationID;
    int wasVoiceMediaPlaying;
    int wasOwned;
  };

  OpInfo opInfo[MMS_MAX_SESSION_OPERATIONS];

  MmsReconnectInfo(MmsSession*, MmsSession::Op*); 
  int  relisten();
  void clear();
};



struct MmsCurrentCommand
{ 
  // Container to pass client command parameters from session manager to service pool
  int             command;
  char*           flatmap;
  unsigned int    flags;
  MmsSession*     session;
  MmsSession::Op* operation;

  MmsCurrentCommand() { memset(this, 0, sizeof(MmsCurrentCommand)); }
  MmsCurrentCommand(const int c, MmsSession* s, MmsSession::Op* op, char* m) 
  { command = c; session = s; operation = op; flatmap = m; flags = 0; 
  }

  void copy(MmsCurrentCommand& that);
  
  enum bitflags 
  { isConcurrentPoolOperationOK = 1,  // permit concurrent pool threads on session 
  };
};


#define isValidOperationID(n) ((n)>0)


typedef ACE_Hash_Map_Entry<int, int> CONXID_MAPENTRY;

typedef ACE_Hash_Map_Iterator_Ex<int, int, ACE_Hash<int>,  
        ACE_Equal_To<int>, ACE_Null_Mutex> CONXID_ITERATOR;


#endif
