// mmsAsr.h
// Metreos Media Server Automatic Speech Recognition Abstraction Layer

#ifndef MMS_ASR_H
#define MMS_ASR_H

#include "mms.h"
#include "mmsConfig.h"
#include "mmsAsrChannel.h"
#include "mmsTask.h"
#include "mmsMediaResourceMgr.h"

// ASR engine is identified in code by one of these integers
#define ASR_ENGINE_NONE                 0
#define ASR_ENGINE_SCANSOFT_SPEECHWORKS 1

// Individual ASR .cpp modules are compiled or not 
// depending upon the value of ASR_ENGINE_IN_USE
#define ASR_ENGINE_IN_USE   ASR_ENGINE_SCANSOFT_SPEECHWORKS

// ASR engine is specified in the config by one of these strings
#define ASR_ENGINE_NAME_SCANSOFT_SPEECHWORKS "speechworks"

#define ASR_TASK  "ASRX"

// Result codes of MMS ASR methods
#define ASR_OK                               0
#define ASR_ERROR_INIT_DISABLED            (-1)
#define ASR_ERROR_INIT_ENGINENAME          (-2)
#define ASR_ERROR_INIT_ENGINE_NOTSUPPORTED (-3)
#define ASR_ERROR_INIT_OS_NOTSUPPORTED     (-4)

// ASR states
typedef enum {
  ASR_STATE_BEFORE_SPEECH = 0,
  ASR_STATE_SPEECH_DETECTED,
  ASR_STATE_SUCCESS,
  ASR_STATE_NOMATCH,
  ASR_STATE_TIMEOUT,
  ASR_STATE_ERROR
} ASRState;

// MmsAsrServer: Base class wraps various vendors' asr functionality
class MmsAsrServer
{
public:
  MmsAsrServer(int asrType);
  virtual ~MmsAsrServer(); 

  virtual int init() { return 0; }
  virtual void cleanup() {}
  virtual int isAsrServerAvailable() = 0;
  virtual AsrChannel* createChannel() = 0;
  virtual void destroyChannel(AsrChannel* pChan) = 0;
  virtual int activateChannel(AsrChannel* pChan, mmsDeviceHandle vdh) = 0; 
  virtual int deactivateChannel(AsrChannel* pChan) = 0; 
  virtual int start(AsrChannel* pChanExt) = 0;
  virtual int stop(AsrChannel* pChanExt) = 0;
  virtual int promptDone(AsrChannel* pChanExt) = 0;
  virtual int loadGrammar(AsrChannel* pChanExt, char* pGrammarType, char* pGrammarData) = 0;
  virtual int handleData(AsrChannel* pChanExt, char* buffer, UINT length) = 0;
  virtual int computeData(AsrChannel* pChanExt) = 0;
  virtual int getAnswer(AsrChannel* pChanExt, wchar_t** answer, int* score) = 0;
  virtual ASRState getState(AsrChannel* pChanExt) = 0;
  virtual int cleanupResources() = 0;

  int lastError() { return this->lasterr; }
 
protected:
  int lasterr;
  int asrType; 
};

// Asr: Singleton speech recognition service manager
class Asr
{
protected:
  static Asr* m_instance;                    
  Asr() { m_server = NULL; }
  Asr& operator = (const Asr&) {};

public:
  static Asr* instance()
  {
    if (!m_instance) 
         m_instance = new Asr();  
    return m_instance;
  }
  virtual ~Asr();

  int  init(MmsConfig*);
  void dispose();
  static void destroy();

  int initSpeechWorks();
  int isAsrServerAvailable();

  MmsAsrChannel* createChannel(unsigned long sessionId, unsigned long opId);
  int destroyChannel(MmsAsrChannel* chan);
  int activateChannel(MmsAsrChannel* chan, mmsDeviceHandle vdh); 
  int deactivateChannel(MmsAsrChannel* chan); 
  int start(MmsAsrChannel* chan);
  int stop(MmsAsrChannel* chan);
  int promptDone(MmsAsrChannel* chan);
  int loadGrammar(MmsAsrChannel* chan, char* pGrammarType, char* pGrammarData);
  int handleData(MmsAsrChannel* chan, char* buffer, UINT length);
  int computeData(MmsAsrChannel* chan);
  int getAnswer(MmsAsrChannel* chan, char** answer, int* score);
  int cleanupResources();

  MmsConfig* config() { return m_config; }

  MmsConfig* m_config;
  MmsAsrServer* m_server;
  MmsTask* m_task;
  HmpResourceManager* m_resourceManager;

protected:
  int engineID;
  int enabled;
};


#endif