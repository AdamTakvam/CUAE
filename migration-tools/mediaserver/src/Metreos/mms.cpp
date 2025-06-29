// 
// mms.cpp 
//
#include "StdAfx.h"
#include "mms.h"
#include "mmsCommandTypes.h"
#include "mmsFlatMap.h"
#include "ACE/Lib_Find.h"
#include "ACE/Stats.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#pragma message("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -")
#pragma message("MEMORY LEAK DETECTION ENABLED (DEBUG HEAP LINKED & NEW REDEFINED") 
#pragma message("Run media server in the debugger -- any memory leaks encountered") 
#pragma message("during the run will be displayed in the VC++ Output Window after")
#pragma message("the media server is shut down. Memory leak detection should be")
#pragma message("disabled after the leak testing is complete. To do so comment out")
#pragma message("the line #define MMS_ENABLE_MEMORY_LEAK_DETECTION in mmsLeakTest.h") 
#pragma message("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -")
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

// Start static initialization
MmsTime  Mms::NOTIME = MmsTime(0);
MmsTime* Mms::TIMEOUT_IMMED = &Mms::NOTIME; // Do not block: return immediately
MmsTime* Mms::TIMEOUT_NEVER = NULL;         // Block up to forever
MmsTime* Mms::TIMEOUT_BLOCK = NULL;         // Block up to forever

unsigned int Mms::alertedTask = 0;  
unsigned int Mms::ticks = 0; 
unsigned int Mms::publicConferenceID = 0;
int Mms::licensedEnhancedRTP = 0; 
int Mms::lowBitrateResourcesInUse = 0;

int MmsAs::licG711  = 0; int MmsAs::maxG711  = 0; int MmsAs::avlG711  = 0; int MmsAs::insG711 = 0;
int MmsAs::licVox   = 0; int MmsAs::maxVox   = 0; int MmsAs::avlVox   = 0; int MmsAs::insVox  = 0;
int MmsAs::licG729  = 0; int MmsAs::maxG729  = 0; int MmsAs::avlG729  = 0; int MmsAs::insG729 = 0;
int MmsAs::licConf  = 0; int MmsAs::maxConf  = 0; int MmsAs::avlConf  = 0; int MmsAs::insConf = 0;
int MmsAs::licTTS   = 0; int MmsAs::maxTTS   = 0; int MmsAs::avlTTS   = 0; int MmsAs::insTTS  = 0;
int MmsAs::licASR   = 0; int MmsAs::maxASR   = 0; int MmsAs::avlASR   = 0; int MmsAs::insASR  = 0;
int MmsAs::licCSP   = 0; int MmsAs::maxCSP   = 0; int MmsAs::avlCSP   = 0; int MmsAs::insCSP  = 0;
int MmsAs::openG711 = 0; int MmsAs::openG729 = 0;             
int MmsAs::openVox  = 0; int MmsAs::openCSP  = 0;
int MmsAs::licensedResourceCeiling = 0;
int MmsAs::isLoggingResxUsage = 0;

char*  MmsAs::szResxExhausted = new char[sizeof(MMS_RESX_EXHAUSTED_MSG)];

ACE_Thread_Mutex* Mms::lbrCountLock = new ACE_Thread_Mutex();
ACE_Thread_Mutex* Mms::confIdLock   = new ACE_Thread_Mutex();
ACE_Thread_Mutex* MmsAs::lockG711   = new ACE_Thread_Mutex();
ACE_Thread_Mutex* MmsAs::lockVox    = new ACE_Thread_Mutex();
ACE_Thread_Mutex* MmsAs::lockG729   = new ACE_Thread_Mutex();
ACE_Thread_Mutex* MmsAs::lockConf   = new ACE_Thread_Mutex();
ACE_Thread_Mutex* MmsAs::lockTTS    = new ACE_Thread_Mutex();
ACE_Thread_Mutex* MmsAs::lockASR    = new ACE_Thread_Mutex();
ACE_Thread_Mutex* MmsAs::lockCSP    = new ACE_Thread_Mutex();

REPORTERCALLBACK MmsAs::reporterCallback = NULL;   
// End static initialization



void Mms::destroy()
{
  // Free static memory allocations

  if (Mms::lbrCountLock) delete Mms::lbrCountLock;
  if (Mms::confIdLock)   delete Mms::confIdLock;    
  if (MmsAs::lockG711)   delete MmsAs::lockG711;   
  if (MmsAs::lockVox)    delete MmsAs::lockVox;    
  if (MmsAs::lockG729)   delete MmsAs::lockG729;    
  if (MmsAs::lockConf)   delete MmsAs::lockConf;    
  if (MmsAs::lockTTS)    delete MmsAs::lockTTS;     
  if (MmsAs::lockASR)    delete MmsAs::lockASR;    
  if (MmsAs::lockCSP)    delete MmsAs::lockCSP; 

  if (MmsAs::szResxExhausted) delete[] MmsAs::szResxExhausted; 
}



int Mms::getTempfileFullpath(char* userbuf, const int buflen, const char* namepart)     
{ 
  // Given a filename, returns a full operating-system-specific path to a 
  // temporary file having that name.  
                                         
  ACE_ASSERT(userbuf && buflen && namepart && *namepart); 
  char  localbuf[MAXPATHLEN+MAXPATHLEN];

  ACE_Lib_Find::get_temp_dir(localbuf, MAXPATHLEN);

  ACE_OS::strcat(localbuf, namepart);
  const int userbuflen  = buflen - 1;
  const int localstrlen = ACE_OS::strlen(localbuf);
  const int copylen = localstrlen > userbuflen? userbuflen: localstrlen+1;
  ACE_OS::strncpy(userbuf, localbuf, copylen);
  return copylen;
}



void Mms::createFilename(char* namebuf)
{
  // Construct unique filename part for a record file, format a1b2c3d4
  // Includes no filetype, but buffer space for filetype is assumed present
  
  memset(namebuf, 0, MMS_RECORD_FILENAME_BUFFERSIZE);
  unsigned long tc = Mms::getTickCount();
  ACE_OS::sprintf(namebuf,"%08X", tc); 
}



void Mms::logMemUsage()
{
  #ifdef MMS_WINPLATFORM

  #if(0) // Old VC6 code for memory < 4 gig

  static char* mask = "SERV memory usage %-4.1f \n";
  static const double threshold = 95.0;
  double phys = 0.0;

  MEMORYSTATUS mem;
  mem.dwLength = sizeof(MEMORYSTATUS);

  GlobalMemoryStatus(&mem);

  if (mem.dwTotalPhys > 0) 
      phys =  100.0 * (1.0 - ((double)mem.dwAvailPhys / (double)mem.dwTotalPhys));

  MMSLOG((phys > threshold? LM_ERROR: LM_INFO, mask, phys));

  #else  // New code permitting memory exceeding 4 gig

  static char* mask = "SERV memory usage pct %d\n";
  static const unsigned int threshold = 95;

  MEMORYSTATUSEX mem;
  mem.dwLength = sizeof(MEMORYSTATUSEX);

  GlobalMemoryStatusEx(&mem);
  MMSLOG((mem.dwMemoryLoad > threshold? LM_ERROR: LM_INFO, mask, mem.dwMemoryLoad));

  #endif // #if(0)
  #endif // #ifdef MMS_WINPLATFORM
}  



int Mms::isFlatmapReferenced(void* p, const int loc, const int sessionID, const int opID)
{
  int result = (Mms::canDeref(p) && isValidFlatmapSignature(p));

  if (!result) 
      if  (sessionID)
           MMSLOG((LM_ERROR, "SERV session %d op %d invalid map reference %x at %d\n", 
                   sessionID, opID, p, loc));
      else MMSLOG((LM_ERROR, "SERV invalid map reference %x at %d\n", p, loc));

  return result;
}



int Mms::canDeref(void* p)
{            
  int result = 0;

  if (p)  
  {
    try  
    {
      int value = *(int*)p;
      result = 1;
    }
    catch(...)  { }
  }

  return result;
}




void Mms::crashServer()
{
  int n; n = 0; 
  int divByZero = 1 / n;
}



unsigned long Mms::getTickCount()
{
  unsigned long result = 0;

  #ifdef MMS_WINPLATFORM
  result = GetTickCount();
  #else
  result = clock();
  #endif

  return result;
}



int Mms::modifyLbrAvailableCount(const int delta, const int isNeedLock)
{
  // Adds to or subtracts from LBR resources in use counter. Caller typically
  // obtains the lbrCountLock such that the availability test and the resource
  // reservation may be made atomically.
  // Returns new count if OK; -1 if action would overflow or underflow counter.

  int result = -1;
  if (isNeedLock) Mms::lbrCountLock->acquire();

  const int provisionalResult = MmsAs::avlG729 + delta;

  if (provisionalResult >= 0 && provisionalResult <= MmsAs::maxG729) 
  {
      switch(delta)
      {
         case  1: result = MmsAs::g729(MmsAs::RESX_INC); break;
         case -1: result = MmsAs::g729(MmsAs::RESX_DEC); break;
         default: result = MmsAs::g729(MmsAs::RESX_SET, provisionalResult);
      }
  } 

  if (isNeedLock) Mms::lbrCountLock->release();
  return result;
}



unsigned int Mms::getNewPublicConferenceID() 
{
  // Dispense next public conference ID in sequence -- range is 1 to 65535
  Mms::confIdLock->acquire();

  ++publicConferenceID; 
  if (publicConferenceID > 0xffff || publicConferenceID == 0) publicConferenceID = 1;

  Mms::confIdLock->release();
  return publicConferenceID;
}



void stripLogFilename(char* path)
{
  const char* fn = "\\mms.log";
  char* p = ACE_OS::strstr(path, fn);            
  if (p && (0 == memcmp(p, fn, ACE_OS::strlen(fn)+1)))
     *p = '\0'; // if at end of string, strip filename
}



int disableLogTimestamps() 
{
  int logflags = ACE_Log_Msg::instance()->flags();         
  ACE_Log_Msg::instance()->clr_flags(ACE_Log_Msg::VERBOSE_LITE);
  return logflags; 
}



char* MmsAguid::generate()                           
{ 
  // Ersatz UUID. The ACE portable UUID code is not yet ready; however this
  // will do the job for a situation such as ours in which the identifiers
  // are transient.  
  unsigned long a; unsigned short b, c; static unsigned char d[8];
  a =(r() << 16) | r(); b = r(); c = r();
  if (!*d) for(int i=0; i < 8; i++) d[i] = rand() % 0x100;

  ACE_OS::sprintf(uuidstring,"%08X-%04X-%04X-%02X%02X-%02X%02X%02X%02X%02X%02X",
          a,b,c,d[0],d[1],d[2],d[3],d[4],d[5],d[6],d[7]);
  return uuidstring;
} 



const char* Mms::commandName(const int commandType)
{
  static const char* COMMANDNAME_CONNECT               = "connect";
  static const char* COMMANDNAME_DISCONNECT            = "disconnect";
  static const char* COMMANDNAME_SERVER                = "server";   
  static const char* COMMANDNAME_PLAY                  = "play";  
  static const char* COMMANDNAME_RECORD                = "record";   
  static const char* COMMANDNAME_RECORD_TRANSACTION    = "record trans";   
  static const char* COMMANDNAME_PLAYTONE              = "playtone";    
  static const char* COMMANDNAME_VOICEREC              = "voicerec";                                          
  static const char* COMMANDNAME_MONITOR_CALL_STATE    = "monitorCallState";  
  static const char* COMMANDNAME_STOP_OPERATION        = "stopMedia";
  static const char* COMMANDNAME_RECEIVE_DIGITS        = "receiveDigits"; 
  static const char* COMMANDNAME_SEND_DIGITS           = "sendDigits";
  static const char* COMMANDNAME_ADJUST_PLAY           = "adjustPlay";
  static const char* COMMANDNAME_CONFERENCE_RESOURCES  = "confresx"; 
  static const char* COMMANDNAME_CONFEREE_SETATTRIBUTE = "confereeSetAttr";
  static const char* COMMANDNAME_HEARTBEAT             = "heartbeat";
  static const char* COMMANDNAME_NOT_RECOGNIZED        = "command";

  switch(commandType)
  {
    case COMMANDTYPE_CONNECT              : return COMMANDNAME_CONNECT;         
    case COMMANDTYPE_DISCONNECT           : return COMMANDNAME_DISCONNECT;      
    case COMMANDTYPE_SERVER               : return COMMANDNAME_SERVER;          
    case COMMANDTYPE_PLAY                 : return COMMANDNAME_PLAY;            
    case COMMANDTYPE_RECORD               : return COMMANDNAME_RECORD;            
    case COMMANDTYPE_RECORD_TRANSACTION   : return COMMANDNAME_RECORD_TRANSACTION;    
    case COMMANDTYPE_PLAYTONE             : return COMMANDNAME_PLAYTONE;         
    case COMMANDTYPE_VOICEREC             : return COMMANDNAME_VOICEREC;                                                 
    case COMMANDTYPE_MONITOR_CALL_STATE   : return COMMANDNAME_MONITOR_CALL_STATE;   
    case COMMANDTYPE_STOP_OPERATION       : return COMMANDNAME_STOP_OPERATION;  
    case COMMANDTYPE_RECEIVE_DIGITS       : return COMMANDNAME_RECEIVE_DIGITS;   
    case COMMANDTYPE_SEND_DIGITS          : return COMMANDNAME_SEND_DIGITS;     
    case COMMANDTYPE_ADJUST_PLAY          : return COMMANDNAME_ADJUST_PLAY;     
    case COMMANDTYPE_CONFERENCE_RESOURCES : return COMMANDNAME_CONFERENCE_RESOURCES;   
    case COMMANDTYPE_CONFEREE_SETATTRIBUTE: return COMMANDNAME_CONFEREE_SETATTRIBUTE; 
    case COMMANDTYPE_HEARTBEAT            : return COMMANDNAME_HEARTBEAT;       
  }

  return COMMANDNAME_NOT_RECOGNIZED;
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// MmsStringArray
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


MmsStringArray::MmsStringArray()
{ 
  this->initx(0); 
}


MmsStringArray::MmsStringArray(int maxentries) 
{ 
  this->initx(maxentries); 
}


int MmsStringArray::add(char* newstring)
{
  if (!newstring || (this->ncount >= this->maxentries)) return -1;
  this->base[this->ncount++] = newstring;
  return 0;
}
    

void MmsStringArray::clear()
{
  for(int i=0; i < this->ncount; i++)
  {
    char* thisstring = this->getAt(i);
    if (thisstring) delete[] thisstring;
  }

  if (this->base)
      delete[] this->base;
  this->base  = NULL;
  this->ncount = 0;
}


char* MmsStringArray::getAt(int i) 
{
  return i >= 0 && i < this->ncount? this->base[i]: NULL;
}


void MmsStringArray::init()
{
  this->ncount = 0; 
  this->base   = new char*[this->maxentries];
}


MmsStringArray::~MmsStringArray() 
{ 
  this->clear(); 
}


void MmsStringArray::initx(int n)
{
  this->maxentries = n > 0? n: MMS_STRING_ARRAY_MAX;    
  this->init();
}


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// MmsManualTimer
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  
  
MmsManualTimer::MmsManualTimer()
{ // Creates a timer which is initially off
  this->initialMs = 0;
  this->timer = NULL;
}
  

MmsManualTimer::MmsManualTimer(unsigned int ms)
{ // Creates a timer set to specified milliseconds, which automatically 
  // resets itself to this time value upon expiration.
  this->initialMs = ms;
  this->timer = NULL; 
  this->reset();     
}


int MmsManualTimer::remaining()
{ // Returns milliseconds remaining on timer
  return timeRemaining.msec();
}


int MmsManualTimer::countdown()
{ 
  // Reduces time remaining by time interval since last countdown or reset.
  // Returns time remaining in milliseconds, zero if timer is off (was never
  // set to an intial time), or -1 if no time now remains on timer.
  if (this->timer == NULL) return 0;
  ACE_Guard<ACE_Thread_Mutex> x(m_lock);
  this->timer->update();
  int result = (timeRemaining == Mms::NOTIME)? -1: timeRemaining.msec();
  if (result == -1) this->reset();
  return result;
}


int MmsManualTimer::reset(int ms)
{ 
  // Resets timer to specified milliseconds, or if zero time specified, 
  // previously-supplied initial milliseconds value. If specified time
  // is nonzero, this becomes the value for subsequent reset() commands.
  ACE_Guard<ACE_Thread_Mutex> x(m_lock);
  if  (ms == 0) 
       ms = this->initialMs;
  else this->initialMs = ms;

  if (this->timer) delete this->timer;  
 
  this->timeRemaining.set(ms/1000, (ms%1000)*1000); 
  this->timer = ms? new MmsCountdown(&this->timeRemaining): NULL;

  return this->timeRemaining.msec();
}



MmsManualTimer::~MmsManualTimer() 
{ // Not virtual by intention. We'll never inherit and don't want the vtable.
  if (!this->timer) return;
  m_lock.acquire();
  delete this->timer;
  m_lock.release();
}


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// MmsAs
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
const char* xG711 = "G711", *xG729 = "G729", *xVOX = "VOX", *xCONF = "CONF";
const char* xTTS  = "TTS",  *xASR  = "ASR",  *xCSP = "CSP";


int MmsAs::g711(int func, const int p1)
{
   // Atomically get/set/test available g.711 resource count
   const static char* oflowmsg = "MMSX G711 resx overflow\n";
   const static char* uflowmsg = "MMSX G711 resx underflow\n";
   int result = 0;
   MmsAs::lockG711->acquire();
   const int oldval = MmsAs::inuseG711();

   switch(func)
   {
     case RESX_GET:
          result = MmsAs::avlG711;  
          break;
     case RESX_INC:
          result = p1 > 1? (MmsAs::avlG711 += p1): ++MmsAs::avlG711;
          if (result > MmsAs::insG711)
          {   MMSLOG((LM_ERROR, oflowmsg));
              MmsAs::avlG711 = result = MmsAs::insG711;
          }
          break;
     case RESX_DEC:
          if  (MmsAs::avlG711 == 0)
               MMSLOG((LM_ERROR, uflowmsg));
          else result = --MmsAs::avlG711;
          break;
     case RESX_ISZERO:
          result = MmsAs::avlG711 == 0;
          if  (p1 == RESX_RESERVE && !result) // If asked to reserve resource ...
          {    MmsAs::avlG711--;   // ... reserve if available 
               func = RESX_DEC;
          }
          break;
     case RESX_ISEQ:
          result = MmsAs::avlG711 == p1;
          break;
     case RESX_ISLT:
          result = MmsAs::avlG711 < p1;
          break;
     case RESX_ISGT:
          result = MmsAs::avlG711 > p1;
          break; 
   }

   logResxUsageActivity(xG711, func, MmsAs::avlG711); 
   raiseStatsEvent(MMS_STAT_CATEGORY_RTP, func, oldval, MmsAs::inuseG711());    
   MmsAs::lockG711->release();
   return result;
}



int MmsAs::vox(int func, const int p1)
{
   // Atomically get/set/test available voice resource count
   int result = 0;
   MmsAs::lockVox->acquire();
   const static char* oflowmsg = "MMSX vox resx overflow\n";
   const static char* uflowmsg = "MMSX vox resx underflow\n";
   const int oldval = MmsAs::inuseVox();

   switch(func)
   {
     case RESX_GET:
          result = MmsAs::avlVox;
          break; 
     case RESX_INC:
          result = p1 > 1? (MmsAs::avlVox += p1): ++MmsAs::avlVox;
          if (result > MmsAs::insVox)
          {   MMSLOG((LM_ERROR, oflowmsg));
              MmsAs::avlVox = result = MmsAs::insVox;
          }
          break;
     case RESX_DEC:
          if  (MmsAs::avlVox == 0)
               MMSLOG((LM_ERROR, uflowmsg));
          else result = --MmsAs::avlVox;
          break;
     case RESX_ISZERO:
          result = MmsAs::avlVox == 0;
          if  (p1 == RESX_RESERVE && !result) // If asked to reserve resource ...
          {    MmsAs::avlVox--;   // ... reserve if available 
               func = RESX_DEC;
          }
          break;
     case RESX_ISEQ:
          result = MmsAs::avlVox == p1;
          break;
     case RESX_ISLT:
          result = MmsAs::avlVox < p1;
          break;
     case RESX_ISGT:
          result = MmsAs::avlVox > p1;
          break; 
   }

   logResxUsageActivity(xVOX, func, MmsAs::avlVox); 
   raiseStatsEvent(MMS_STAT_CATEGORY_VOX, func, oldval, MmsAs::inuseVox());
   MmsAs::lockVox->release();
   return result;
}



int MmsAs::g729(int func, const int p1)
{
   // Atomically get/set/test available G.729 resource count
   int result = 0;
   MmsAs::lockG729->acquire();
   const static char* oflowmsg = "MMSX G729 resx overflow\n";
   const static char* uflowmsg = "MMSX G729 resx underflow\n";
   const int oldval = MmsAs::inuseG729();

   switch(func)
   {
     case RESX_GET:
          result = MmsAs::avlG729;  
          break;
     case RESX_SET:
          MmsAs::avlG729 = result = p1;
          if (p1 < 0) 
          {   MMSLOG((LM_ERROR, uflowmsg));
              MmsAs::avlG729 = result = 0;
          }
          if (p1 > MmsAs::insG729)
          {   MMSLOG((LM_ERROR, oflowmsg));
              MmsAs::avlG729 = result = MmsAs::insG729;
          }
          break;
     case RESX_INC:
          result = p1 > 1? (MmsAs::avlG729 += p1): ++MmsAs::avlG729;
          if (result > MmsAs::insG729)
          {   MMSLOG((LM_ERROR, oflowmsg));
              MmsAs::avlG729 = result = MmsAs::insG729;
          }
          break;
     case RESX_DEC:
          if  (MmsAs::avlG729 == 0)
               MMSLOG((LM_ERROR, uflowmsg));
          else result = --MmsAs::avlG729;
          break;
     case RESX_ISZERO:
          if  (p1 == RESX_RESERVE && !result) // If asked to reserve resource ...
          {    MmsAs::avlG729--;   // ... reserve if available 
               func = RESX_DEC;
          }
          result = MmsAs::avlG729 == 0;
          break;
     case RESX_ISEQ:
          result = MmsAs::avlG729 == p1;
          break;
     case RESX_ISLT:
          result = MmsAs::avlG729 < p1;
          break;
     case RESX_ISGT:
          result = MmsAs::avlG729 > p1;
          break; 
   }

   logResxUsageActivity(xG729, func, MmsAs::avlG729); 
   raiseStatsEvent(MMS_STAT_CATEGORY_G729, func, oldval, MmsAs::inuseG729());
   MmsAs::lockG729->release();
   return result;
}



int MmsAs::conf(int func, const int p1)
{
   // Atomically get/set/test available conference resource count
   int result = 0;
   MmsAs::lockConf->acquire();
   const static char* oflowmsg = "MMSX conf resx overflow\n";
   const static char* uflowmsg = "MMSX conf resx underflow\n";
   const int oldval = MmsAs::inuseConf();

   switch(func)
   {
     case RESX_GET:
          result = MmsAs::avlConf;  
          break;
     case RESX_SET:
          MmsAs::avlConf = result = p1;
          if (p1 < 0) 
          {   MMSLOG((LM_ERROR, uflowmsg));
              MmsAs::avlConf = result = 0;
          }
          if (p1 > MmsAs::insConf)
          {   MMSLOG((LM_ERROR, oflowmsg));
              MmsAs::avlConf = result = MmsAs::insConf;
          }
          break;
     case RESX_INC:
          result = p1 > 1? (MmsAs::avlConf += p1): ++MmsAs::avlConf;
          if (result > MmsAs::insConf)
          {   MMSLOG((LM_ERROR, oflowmsg));
              MmsAs::avlConf = result = MmsAs::insConf;
          }
          break;
     case RESX_DEC:
          if  (MmsAs::avlConf == 0)
               MMSLOG((LM_ERROR, uflowmsg));
          else result = --MmsAs::avlConf;
          break;
     case RESX_ISZERO:
          result = MmsAs::avlConf == 0;
          if  (p1 == RESX_RESERVE && !result) // If asked to reserve resource ...
          {    MmsAs::avlConf--;   // ... reserve if available 
               func = RESX_DEC;
          }
          break;
     case RESX_ISEQ:
          result = MmsAs::avlConf == p1;
          break;
     case RESX_ISLT:
          result = MmsAs::avlConf < p1;
          break;
     case RESX_ISGT:
          result = MmsAs::avlConf > p1;
          break; 
   }

   logResxUsageActivity(xCONF, func, MmsAs::avlConf); 
   raiseStatsEvent(MMS_STAT_CATEGORY_CONFRESX, func, oldval, MmsAs::inuseConf());
   MmsAs::lockConf->release();
   return result;
}



int MmsAs::tts(int func, const int p1)
{
   // Atomically get/set/test available TTS resource count
   int result = 0;
   MmsAs::lockTTS->acquire();
   const static char* oflowmsg = "MMSX TTS resx overflow\n";
   const static char* uflowmsg = "MMSX TTS resx underflow\n";
   const int oldval = MmsAs::inuseTTS();

   switch(func)
   {
     case RESX_GET:
          result = MmsAs::avlTTS;  
          break;
     case RESX_SET:
          MmsAs::avlTTS = result = p1;
          break;
     case RESX_INC:
          if  (MmsAs::avlTTS >= MmsAs::insTTS)
               MMSLOG((LM_ERROR, oflowmsg));
          else ++MmsAs::avlTTS;
          result = MmsAs::avlTTS;           
          break;
     case RESX_DEC:
          if  (MmsAs::avlTTS == 0)
               MMSLOG((LM_ERROR, uflowmsg)); 
          else result = --MmsAs::avlTTS;
          break;
     case RESX_ISZERO:
          result = MmsAs::avlTTS == 0;
          if  (p1 == RESX_RESERVE && !result) // If asked to reserve resource ...
          {    MmsAs::avlTTS--;   // ... reserve if available 
               func = RESX_DEC;
          }
          break;
     case RESX_ISEQ:
          result = MmsAs::avlTTS == p1;
          break;
     case RESX_ISLT:
          result = MmsAs::avlTTS < p1;
          break;
     case RESX_ISGT:
          result = MmsAs::avlTTS > p1;
          break; 
   }

   logResxUsageActivity(xTTS, func, MmsAs::avlTTS); 
   raiseStatsEvent(MMS_STAT_CATEGORY_TTS, func, oldval, MmsAs::inuseTTS());
   MmsAs::lockTTS->release();
   return result;
}



int MmsAs::asr(int func, const int p1)
{
   // Atomically get/set/test available Speech Rec resource count
   int result = 0;
   MmsAs::lockASR->acquire();
   const static char* oflowmsg = "MMSX ASR resx overflow\n";
   const static char* uflowmsg = "MMSX ASR resx underflow\n";
   const int oldval = MmsAs::inuseCSP(); // See comment below

   switch(func)
   {
     case RESX_GET:
          result = MmsAs::avlASR;  
          break;
     case RESX_SET:
          MmsAs::avlASR = result = p1;
          break;
     case RESX_INC:
          if  (MmsAs::avlASR >= MmsAs::insASR)
               MMSLOG((LM_ERROR, oflowmsg));
          else ++MmsAs::avlASR;
          result = MmsAs::avlASR;           
          break;
     case RESX_DEC:
          if  (MmsAs::avlASR == 0)
               MMSLOG((LM_ERROR, uflowmsg)); 
          else result = --MmsAs::avlASR;
          break;
     case RESX_ISZERO:
          result = MmsAs::avlASR == 0;       
          break;
     case RESX_ISEQ:
          result = MmsAs::avlASR == p1;
          break;
     case RESX_ISLT:
          result = MmsAs::avlASR < p1;
          break;
     case RESX_ISGT:
          result = MmsAs::avlASR > p1;
          break; 
   }

   // Note that we are reporting CSP resources here
   logResxUsageActivity(xASR, func, MmsAs::avlCSP); 
   raiseStatsEvent(MMS_STAT_CATEGORY_CSP, func, oldval, MmsAs::inuseCSP());
   MmsAs::lockASR->release();
   return result;
}



int MmsAs::csp(int func, const int p1)
{
   // Atomically get/set/test HMP Continuous Speech Processing available resource count
   int result = 0;
   MmsAs::lockCSP->acquire();
   const static char* oflowmsg = "MMSX CSP resx overflow\n";
   const static char* uflowmsg = "MMSX CSP resx underflow\n";
   const int oldval = MmsAs::inuseCSP();

   switch(func)
   {
     case RESX_GET:
          result = MmsAs::avlCSP;  
          break;
     case RESX_SET:
          MmsAs::avlCSP = result = p1;
          break;
     case RESX_INC:
          if  (MmsAs::avlCSP >= MmsAs::insCSP)
               MMSLOG((LM_ERROR, oflowmsg));
          else ++MmsAs::avlCSP;
          result = MmsAs::avlCSP;    
          break;
     case RESX_DEC:
          if  (MmsAs::avlCSP == 0)
               MMSLOG((LM_ERROR, uflowmsg)); 
          else result = --MmsAs::avlCSP;
          break;
     case RESX_ISZERO:
          result = MmsAs::avlCSP == 0;
          if  (p1 == RESX_RESERVE && !result) // If asked to reserve resource ...
          {    MmsAs::avlCSP--;   // ... reserve if available 
               func = RESX_DEC;
          }  
          break;
     case RESX_ISEQ:
          result = MmsAs::avlCSP == p1;
          break;
     case RESX_ISLT:
          result = MmsAs::avlCSP < p1;
          break;
     case RESX_ISGT:
          result = MmsAs::avlCSP > p1;
          break; 
   }

   logResxUsageActivity(xCSP, func, MmsAs::avlCSP); 
   raiseStatsEvent(MMS_STAT_CATEGORY_CSP, func, oldval, MmsAs::inuseCSP());
   MmsAs::lockCSP->release();
   return result;
}


const char* acts[] = { "GET", "SET", "INC", "DEC" };



void MmsAs::logResxUsageActivity(const char* whichRes, const int whichAct, const int newval)
{
  // Log specific activity on the specified resource count

  if (MmsAs::isLoggingResxUsage)  
  {    
      char* szAct = whichAct < 0 || whichAct > 3? NULL: (char*)acts[whichAct];
      if (szAct)  
          MMSLOG((LM_DEBUG, "MMSX resx %s %s (%d)\n", whichRes, szAct, newval));           
  }  
}



void MmsAs::raiseStatsEvent(const int resxType, const int whichAct, const int oldval, const int newval)
{
  // Initiate raising of a statistics event by calling back into the stats reporter object.
  // We are static without access to mmsTask.h here, so we use the callback to send the 
  // MMSM_STATSREQUEST message to the reporter object. The callback returns as soon as the
  // message is placed on the reporter queue, so there is no delay here.
         
  if (whichAct < 1 || whichAct > 3) return; // Stats only for set, inc, and dec

  MmsStatsParams* datastruct = new MmsStatsParams(resxType, oldval, newval); 
   
  reporterCallback(datastruct);             // Call back into reporter (which frees datastruct)
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// MmsDirectoryRecursor
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

void MmsDirectoryRecursor::recurse(char* dir, void* param)
{
  // Platform-independent directory walker. Calls back with each file located

  this->scan(dir, param, TRUE);
}



void MmsDirectoryRecursor::scan(char* cdir, void* param, const int recursedir)
{
  // Platform-independent directory walker. Calls back with each file located
  // Recurses indicated directory if so specified

  ACE_DIR* handle = NULL;
  ACE_stat fileinfo;
  dirent*  fd = NULL;
  char     dir[MAXPATHLEN];
  ACE_OS::strncpy(dir, cdir, MAXPATHLEN-1);
  const char separator[2] = { ACE_DIRECTORY_SEPARATOR_CHAR_A, '\0' };

  #if 0
  // Turned this off since directories may have embedded dots,  
  // which makes distinguishing filename from directory name problematic
  int dots = 0;

  for(char* p = dir; *p; p++)                
      if  (*p == '\\') *p = '/'; else 
      if  (*p == '.' ) dots++;

  if  (dots)                                // If filename passed, lose it
  {    while(p > dir && *p != '/') p--;      
       if   (p > dir)   *p  = '\0';
  }
  #endif

  ++m_dircount;
  if  (this->isMaxDirectories()) return;
  if  (NULL == (handle = ACE_OS::opendir(dir))) return;
  if  (ACE_OS::stat(dir, &fileinfo) == -1) return;

  while(1)
  {
    if  (NULL ==(fd = ACE_OS::readdir(handle))) break; 
    if  (NULL == fd->d_name) continue;
    if  (*fd->d_name == '.') continue;
    int  isbadchar = 0;

    for (char* p = fd->d_name; *p; p++) 
         if (*p == '[' || *p == ']' || *p == '=' || *p == ',') 
         {   isbadchar = 1; 
             break; 
         }
    if  (isbadchar) continue;

    char path[MAXPATHLEN]; ACE_OS::strcpy(path, dir);
    ACE_OS::strcat(path, separator);
    ACE_OS::strcat(path, fd->d_name);
    ACE_OS::stat  (path, &fileinfo);

    if  (fileinfo.st_mode & S_IFDIR)    
         if  (recursedir) 
              this->recurse(path, param);
         else continue;  
    else this->filecallback(path, param);     
  } 
 
  ACE_OS::closedir(handle);
}



void MmsDirectoryRecursor::parsepath(PathInfo& info, char* path)
{
  char*  p = path;
  while(*p) p++;                            // Point at end of path string
  info.end = p;
  info.pathlength = p - path;

  while(p > path && *p != '.') p--;         // Find extension
  info.ext = *p == '.'? p: info.end;
  info.extlength = info.end - info.ext;     // Lowercase for comparisons
  for  (char* q = info.ext; *q; q++) *q = tolower(*q);
                                            // Find filename
  while(p > path && *p != '\\' && *p != '/') p--;
  info.filename = p > path? ++p: path;
  info.filenamelength = info.ext - info.filename;
  info.dirlength = info.filename - path;
}