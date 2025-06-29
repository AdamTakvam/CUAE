//
// mmsTts.h
//
// Text to speech.  
//
#ifndef MMS_TTS_H
#define MMS_TTS_H

#ifdef MMS_WINPLATFORM
#pragma once
#endif

#include "mms.h"
#include "mmsConfig.h"
#include "mmsCommon.h"
#include "mmsSession.h"
#include "mmsWavHeader.h"


// TTS engine is identified in code by one of these integers
#define TTS_ENGINE_NONE              0
#define TTS_ENGINE_RHETORICAL_RVOICE 1
#define TTS_ENGINE_LOQUENDO          2
#define TTS_ENGINE_NEOSPEECH         3      // NeoSpeech VoiceWare

// Individual tts .cpp modules are compiled or not 
// depending upon the value of TTS_ENGINE_IN_USE
#define TTS_ENGINE_IN_USE TTS_ENGINE_NEOSPEECH

// TTS engine is specified in the config by one of these strings
#define TTS_ENGINE_NAME_RHETORICAL_RVOICE "rvoice"
#define TTS_ENGINE_NAME_LOQUENDO          "loquendo"
#define TTS_ENGINE_NAME_NEOSPEECH         "neospeech"

// Result codes of MMS TTS methods
#define TTS_OK                               0
#define TTS_ERROR_INIT_DISABLED            (-1)
#define TTS_ERROR_INIT_ENGINENAME          (-2)
#define TTS_ERROR_INIT_ENGINE_NOTSUPPORTED (-3)
#define TTS_ERROR_INIT_OS_NOTSUPPORTED     (-4)
#define TTS_ERROR_INIT_FAILED              (-5)

#define TTS_PARAM_OFFSET 1000

struct MmsTtsRenderData;
class  MmsTtsWavData;
class  MmsSession;


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// MmsTtsServer: base class wraps various vendors' text to speech functionality
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

class MmsTtsServer
{
  public:

  MmsTtsServer(int servertype)
  {
    this->ttstype = servertype;
    this->lasterr = this->init();
  }

  virtual ~MmsTtsServer() 
  { 
    this->cleanup(); 
  }

  virtual void cleanup()  { }

  virtual int  init() { return 0; }

  virtual int  isTtsServerAvailable()=0;

  virtual int render                        // Render text to WAV file
   (void* sessionOp, MmsTtsRenderData*, unsigned int params=0xffffffff)=0; 

  virtual long setVoice(MmsTtsWavData* data)=0; 

  virtual long setVoiceFormat(MmsTtsWavData* data)=0; 

  virtual long setVoiceParams(unsigned int params, MmsTtsWavData* data)=0;

  int lastError() { return this->lasterr; }
 
  protected:
  int   lasterr;
  int   ttstype; 
};


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// class Tts: singleton text to speech services manager
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

class Tts
{
  protected:
  static Tts* m_instance;                    
  Tts() { m_server = NULL; }
  Tts& operator=(const Tts&) { };

  public:
  static Tts* instance()
  {
    if   (!m_instance) m_instance = new Tts();  
    return m_instance;
  }

  int  render(void* operation, MmsTtsRenderData*, unsigned int params=0xffffffff);

  void writeWavFile(char* data, long length, int KHz, char* path, const int free=0);

  int init(MmsConfig*);
  int Tts::initRvoice();
  int Tts::initLoquendo();
  int Tts::initNeoSpeech();

  int isTtsServerAvailable();

  void  getNameFromPath   (const char* path, char* name, const int namelen);
  char  removeNameFromPath(const char* path, const int removeTrailingSlash=0);
  char* makeWavFilePath       (MmsTtsWavData* data, const int omitExtension=0); 
  int   writeWavFileDescriptor(MmsTtsWavData* data); 
  int   raiseTtsInitFailureAlarm(const int retval=0); 
  int   raiseTtsConxFailureAlarm(const int retval=0);

  MmsConfig* config() { return m_config; }

  void dispose();

  virtual ~Tts()
  {
  }

  static void destroy();
 
  MmsConfig*    m_config;
  MmsTtsServer* m_server;

  char* m_ttsServerIP;
  int   m_ttsServerPort;

  protected:
  int raiseTtsPortsExhaustedAlarm(const int sid, const int opid);
  int engineID;
  int enabled;
};



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// MmsTtsRenderData
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

struct MmsTtsRenderData
{     
  int   voice;
  int   quality;
  MmsLocaleParams ldata;
  void  clear() { memset(this, 0, sizeof(MmsTtsRenderData));      }
  MmsTtsRenderData(char* a, char* l)   { clear(); ldata.set(a,l); } 
  MmsTtsRenderData(MmsLocaleParams& p) { clear(); ldata.set(p);   } 
  MmsTtsRenderData() { clear(); }
};
     

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// MmsTtsWavData: PCM data accumulation block
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

class MmsTtsWavData
{
  public:
  unsigned int sig;
  long  wavDataLength;
  char* wavData;    
  char* wavFilePath;
  long  handle;
  long  rate;
  long  volume;
  long  iparamA;
  long  iparamB;
  long  iparamC;
  char* cparamA;
  char* cparamB;
  void* pparamA;
  void* pparamB;
  MmsTtsRenderData rdata;

  void clear();
  
  MmsTtsWavData()  { clear(); }

  MmsTtsWavData(MmsTtsRenderData* rd);  
  
  virtual ~MmsTtsWavData();

  virtual void dispose() { }
  enum { signature=0x1a3b5c7d }; 
};


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// MmsTtsEventData
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

 class MmsTtsEventData
{     
  public:
  virtual ~MmsTtsEventData() { this->dispose(); };
  virtual void dispose()=0;
};



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// MmsTtsSessionData: TTS strings and associated WAV file paths  
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

class MmsTtsSessionData
{     
  // Container for TTS string and WAV file information, which exists 
  // only for the duration of a session play command.
  public:
  MmsTtsSessionData(MmsConfig* config, int maxsize) 
  { 
    this->config = config;  
    this->ttsFiles = 0;
    this->ttsStrings = new MmsStringArray(maxsize); 
  }

  virtual ~MmsTtsSessionData() { this->dispose(); };

  MmsStringArray* ttsStrings;
  MmsStringArray* ttsFiles;

  void dispose();

  void deleteReferencedFiles();

  void alloc()
  {
    if (this->ttsStrings && !this->ttsFiles)
        this->ttsFiles = new MmsStringArray(this->ttsStrings->count()); 
  }

  MmsConfig* config;
};


#endif

