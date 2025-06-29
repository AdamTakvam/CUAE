//   
// mms.h 
// primary media server header included by all mms projects including tests
//
#ifndef MMS_H
#define MMS_H

#ifdef  WIN32
#pragma once
#define MMS_WINPLATFORM 32
#pragma warning(disable:4786)
#endif

#include "ace/OS.h"
#include "ace/Synch.h"
#include "ace/Task.h"
#include "ace/Reactor.h"
#include "ace/Log_Record.h"

#include "mmsCommon.h"

// Compile switch to link in Micrsoft message queuing. The MQ adapter itself remains
// present in the build; however the MSMQ calls are now compiled out by default. 

// #define MMS_LINK_WITH_MSMQ  // Uncomment to link in MSMQ calls

#ifdef  WIN32
#define MMS_REGKEY "Software\\Metreos\\MediaServer"
#include "mmsLeakTest.h"       // Memory leak test support

#else   // #ifdef WIN32
// Assume Linux if not Windows
typedef int64_t _int64, __int64;
#endif  // #ifdef WIN32

#undef  TRUE
#define TRUE 1
#undef  FALSE
#define FALSE 0

typedef ACE_Time_Value     MmsTime;
typedef ACE_Event_Handler  MmsEventHandler;
typedef ACE_Countdown_Time MmsCountdown;  
#define mmsSleep ACE_OS::sleep 
#define mmsYield ACE_OS::thr_yield

#define MMSLOG(X)     do{ACE_Log_Msg::instance()->log X;}while(0)
#define MMSDEBUG(X)   do{ACE_Log_Msg::instance()->log X;}while(0)
#define WAITFORINPUT  do{char c;cout << "Any character ...";cin >> c;}while(0)
#define WAITFORKEY(p) do{char c=0;printf(p);while(!c)c=_getch();}while(0)
                          
#define MMS_QUARTER_SECOND MmsTime(0,25000) 
#define MMS_HALF_SECOND    MmsTime(0,50000)           
#define MMS_ONE_MS         MmsTime(0,1000)
#define MMS_N_MS(n)        MmsTime(0,1000*(n))
                                            // Convert ACE LM_ value to MMS or
#define lmToMsgPriority(n) (ACE::log2(n))   // ACE message level (log priority) 
#define msgPriorityToLm(n) (1<<((n)-1))     // value (1-11), or vice-versa

void stripLogFilename(char* path);
int disableLogTimestamps(); 
#define restoreLogTimestamps(n) ACE_Log_Msg::instance()->set_flags(n) 

#define MMS_TIMERID_SMGR_HEARTBEAT   1
#define MMS_TIMERID_CALLSTATE        2
#define MMS_CALLSTATE_SESSSIONID_TIMERID_OFFSET 100
#define MMS_RECORD_FILENAME_BUFFERSIZE (8 + 1 + 3 + 1)

#ifndef MMS_WINPLATFORM
#define stricmp strcasecmp
#define MMS_MAX_PATH MAX_PATH
#else
#define MMS_MAX_PATH 260
#endif

#define MMS_IS_DIALOGIC_OEM_LICENSING       // Define to enable OEM licensing logic

#define MMS_SDK_RESXS_G711 6                // G711 resource count for SDK version
#define MMS_SDK_RESXS_VOX  6                // Voice resource count for SDK version
#define MMS_SDK_RESXS_G729 0                // G729 resource count for SDK version
#define MMS_SDK_RESXS_CONF 6                // Converence resource count for SDK version
#define MMS_SDK_RESXS_TTS  1                // TTS port count for SDK version
#define MMS_SDK_RESXS_ASR  0                // Speech rec resource count for SDK version
#define MMS_SDK_RESXS_CSP  0                // Continuous speech resource count for SDK version

#define MMS_BAD_PTR_MSG "invalid memory reference"
#define MMS_RESX_EXHAUSTED_MSG "%s media resources exhausted"

typedef void (*REPORTERCALLBACK)(void*);    // Alarms/stats reporter callback signature
typedef void (*GENERICCALLBACK)(void*);     // Generic callback signature


enum threadGroups                           // Who the #$% has registered group 2?
{ GROUPID_APP=1, 
  GROUPID_MONITOR=3, 
  GROUPID_THREADPOOL, 
  GROUPID_TIMERMON, 
  GROUPID_LOGGER, 
  GROUPID_IPC_LISTENER,
  GROUPID_SERVER_MANAGER,
  GROUPID_SESSION_MANAGER,
  GROUPID_MQLISTENER,
  GROUPID_SOCKLISTENER,
  GROUPID_REPORTER,
}; 



class Mms
{ public:
  static MmsTime  NOTIME;
  static MmsTime* TIMEOUT_IMMED;            // Do not block: return immediately
  static MmsTime* TIMEOUT_NEVER;            // Block up to forever
  static MmsTime* TIMEOUT_BLOCK;            // Block up to forever
  static unsigned int alertedTask;          // Global alerted task
  static int licensedEnhancedRTP;           // Enhanced RTP licensed
  static int lowBitrateResourcesInUse;      // Enhanced RTP in use
  static unsigned int ticks;                // Global heartbeat count
  static unsigned int publicConferenceID;   // Global conference ID count
  static ACE_Thread_Mutex* lbrCountLock;    // Atomic op lock for LBR count
  static ACE_Thread_Mutex* confIdLock;      // Atomic op lock for conference ID assignment
  static int modifyLbrAvailableCount(const int delta, const int isNeedLock=1);
  static unsigned int getTicks() { return ticks; }
  static unsigned int getNewPublicConferenceID();
  static void tick() { ++ticks; }
  static int  countdown(const int whichtimer);
  static void resetCountdown(const unsigned int ms);
  static void logMemUsage();                // Write memory use to log
  static void crashServer();                // Generate a hard crash
  static int  canDeref(void*);              // Try/catch dereference
  static int  isFlatmapReferenced(void* p, const int loc=0, const int sID=0, const int opID=0); 
  static void createFilename(char* namebuf);
  static unsigned long getTickCount(); 
  static const char* commandName(const int commandID);
                                            // Return full path to a temp file
  static int getTempfileFullpath(char* buf, const int buflen, const char* namepart);

  static void destroy();
};



class MmsAs                                 // Licensing, Alarms & Stats global data and constants
{
  public:
  // Licensed/installed/calculated/available resource counts
  static int licG711, insG711, maxG711, avlG711; // G711 resources
  static int licVox,  insVox,  maxVox,  avlVox;  // Voice resources
  static int licG729, insG729, maxG729, avlG729; // Low bitrate resources
  static int licConf, insConf, maxConf, avlConf; // Conference resources
  static int licTTS,  insTTS,  maxTTS,  avlTTS;  // TTS resources
  static int licASR,  insASR,  maxASR,  avlASR;  // ASR ports (not used currently) 
  static int licCSP,  insCSP,  maxCSP,  avlCSP;  // HMP continuous speech resxs 

  static int inuseG711() { return MmsAs::insG711 - MmsAs::avlG711; }
  static int inuseVox()  { return MmsAs::insVox  - MmsAs::avlVox;  }
  static int inuseG729() { return MmsAs::insG729 - MmsAs::avlG729; }
  static int inuseConf() { return MmsAs::insConf - MmsAs::avlConf; }
  static int inuseTTS()  { return MmsAs::insTTS  - MmsAs::avlTTS;  }
  static int inuseASR()  { return MmsAs::insASR  - MmsAs::avlASR;  }
  static int inuseCSP()  { return MmsAs::insCSP  - MmsAs::avlCSP;  }

  static enum resxFuncs
  {
     RESX_GET=0, RESX_SET=1, RESX_INC=2, RESX_DEC=3, RESX_ISZERO=4, RESX_ISEQ=5, RESX_ISLT=6, RESX_ISGT=7,     
  };

  static enum resxParams
  {
     RESX_RESERVE=1,     
  };

  static int  g711(int func, const int p1=0);
  static int  vox (int func, const int p1=0);  
  static int  g729(int func, const int p1=0);
  static int  conf(int func, const int p1=0);
  static int  tts (int func, const int p1=0);
  static int  asr (int func, const int p1=0);
  static int  csp (int func, const int p1=0);
  static void logResxUsageActivity(const char* res, const int act, const int newval);
  static void raiseStatsEvent(const int resxtype, const int act, const int oldval, const int newval);

  static int openG711, openG729;            // RTP and enhanced RTP resources opened count
  static int openVox,  openCSP;             // Voice and CSP-enabled voice resources opened count
                                            // Throttle on resources for SDK, standard, enhanced, 
  static int licensedResourceCeiling;       // premium, etc. licensing modes
  static int isLoggingResxUsage;            // Static mirror to diagnostic switch 0x8000

  static enum alarmTypes                    // Values for MMSP_ALARM_TYPE
  {
    MMS_SERVER_COMPROMISED       = 10001,
    MMS_UNEXPECTED_CONDITION     = 10002,
    MMS_UNSCHEDULED_SHUTDOWN     = 10004,
    MMS_RESX_NOT_DEPLOYED        = 10008,   // Not on this server, e.g. no voicerec

    MMS_NO_MORE_CONNECTIONS      = 10256,   // Out of G.711
    MMS_HIWATER_CONNECTIONS      = 10258,
    MMS_LOWATER_CONNECTIONS      = 10259,
     
    MMS_NO_MORE_VOX              = 10260,   // Out of voice resources
    MMS_HIWATER_VOX              = 10262,
    MMS_LOWATER_VOX              = 10263,

    MMS_NO_MORE_G729_ETC         = 10264,   // Out of low bitrate resources
    MMS_HIWATER_G729_ETC         = 10266,
    MMS_LOWATER_G729_ETC         = 10267, 

    MMS_NO_MORE_CONFRESX_IN_SVC  = 10268,   // Out of conference resources on server
    MMS_HIWATER_CONFRESX_IN_SVC  = 10270,
    MMS_LOWATER_CONFRESX_IN_SVC  = 10271, 

    MMS_NO_MORE_SLOTS_IN_CONF    = 10272,   // Out of conferee slots in conference
    MMS_HIWATER_SLOTS_IN_CONF    = 10274,
    MMS_LOWATER_SLOTS_IN_CONF    = 10275,  

    MMS_NO_MORE_TTS_PORTS_FAILS  = 10276,   // Out of TTS ports - request rejected
    MMS_NO_MORE_TTS_PORTS_QUEUES = 10277,   // Out of TTS ports - request queued on TTS server
    MMS_HIWATER_TTS_PORTS        = 10278,
    MMS_LOWATER_TTS_PORTS        = 10279, 

    MMS_NO_MORE_ASR_RESX         = 10280,   // Out of ASR resources - request rejected
    MMS_HIWATER_ASR_RESX         = 10282,
    MMS_LOWATER_ASR_RESX         = 10283,  
  };

  static REPORTERCALLBACK reporterCallback; // Callback to alarms/stats reporter
                                             
  static enum statsTypes                    // Values for MMSP_STATS_TYPE
  {    
    MMS_RESOURCE_USED            = 10001,
    MMS_RESOURCE_FREE            = 10002,  
    MMS_STATS_BUNDLE             = 10003,   // Multiple resource reports (sub-maps)  
  };

  static char* szResxExhausted;

  static ACE_Thread_Mutex* lockG711;
  static ACE_Thread_Mutex* lockVox;
  static ACE_Thread_Mutex* lockG729;
  static ACE_Thread_Mutex* lockConf;
  static ACE_Thread_Mutex* lockTTS;
  static ACE_Thread_Mutex* lockASR;
  static ACE_Thread_Mutex* lockCSP;
};



class MmsAguid                                
{                                           // Almost globally-unique identifer 
  public:                                    
  enum {UUIDSIZE=36,SIZE=UUIDSIZE};            
  char* uuid() { return uuidstring; }        
  MmsAguid()   { srand(time(0)); generate(); } 
  void  clear(){ *uuidstring = 0; }                                           
  char* generate();
  static unsigned short r() { return rand() % 0xefff + 0x1000; }
  protected:
  char  uuidstring[UUIDSIZE + sizeof(char)];
};



#define ensureTrailingSlash(p) do{char* q = p + ACE_OS::strlen(p) - 1; \
 if(*q != ACE_DIRECTORY_SEPARATOR_CHAR_A) \
  {*++q = ACE_DIRECTORY_SEPARATOR_CHAR_A; *++q = '\0';}}while(0)



class MmsDirectoryRecursor
{
  public:
  void recurse(char* dir, void* param=0);
  void scan   (char* dir, void* param=0, const int recursedir=0);
  virtual int filecallback(char* path, void* param=0)=0; 

  struct PathInfo
  { char* filename, *ext, *end;
    int   pathlength, dirlength, filenamelength, extlength;
    PathInfo() { memset(this,0,sizeof(PathInfo)); }
  };
  static void parsepath(PathInfo&, char* path);

  MmsDirectoryRecursor(int maxdirs=0) 
  { m_maxdirs  = maxdirs;
    m_dircount = 0;
    this->setCurrentTime(); 
  }
  
  void resetDircount()    { m_dircount = 0; }
  int  isMaxDirectories() { return m_maxdirs && (m_dircount > m_maxdirs); }
  void setCurrentTime()   { currentTime.set(ACE_OS::gettimeofday().sec(),0); }

  int  m_dircount, m_maxdirs;
  ACE_Time_Value currentTime;
};



class MmsStringArray
{
  // A simple container implementing an array of C strings or other byte vectors.
  // Caller allocates each string but must not subsequently free them, since   
  // memory for each string in the array is freed in this destructor.
  #define MMS_STRING_ARRAY_MAX 16

  public:
  MmsStringArray();
  MmsStringArray(int maxentries);
  int  add(char* newstring);
  void clear();
  char* getAt(int i); 
  void init();
  int count()      { return this->ncount;    }
  int maxEntries() { return this->maxentries;}
  virtual ~MmsStringArray();

  protected:
  char** base;
  int    ncount, maxentries;
  void   initx(int n);
};



class MmsManualTimer
{
  // A wrapper for an ACE countdown object and its associated state, with construction,
  // countdown, remaining, and reset methods. All time is specified in milliseconds.
  protected:
  MmsCountdown* timer;
  MmsTime timeRemaining;
  unsigned int initialMs;
  ACE_Thread_Mutex m_lock;

  public:

  MmsManualTimer();

  MmsManualTimer(unsigned int ms);

  ~MmsManualTimer(); 

  int remaining();

  int countdown();

  int reset(int ms=0);
};


#endif
