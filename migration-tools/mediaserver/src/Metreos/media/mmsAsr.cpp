//
// mmsAsr.cpp
// Automatic Speech Recognition abstraction layer
//
#include "StdAfx.h"
#include "mmsAsr.h"
#include "mmsAsrSpeechWorks.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

Asr* Asr::m_instance = 0;  


///////////////////////////////////////////////////////////
// MmsAsrServer default constructor
MmsAsrServer::MmsAsrServer(int asrType)
{
  this->asrType = asrType;
  this->lasterr = this->init();
}


///////////////////////////////////////////////////////////
// MmsAsrServer destructor
MmsAsrServer::~MmsAsrServer() 
{ 
  this->cleanup(); 
}


 
Asr::~Asr()
{
}


///////////////////////////////////////////////////////////
// Verify if ASR feature is enabled and has at least one active ASR engine
int Asr::isAsrServerAvailable()
{
  // Indicate if ASR services are currently available
  if (!this->enabled || !m_server) 
      return 0;

  return this->m_server->isAsrServerAvailable();
}


///////////////////////////////////////////////////////////
// Initialize ASR service as specified in config file
int Asr::init(MmsConfig* config)
{    
  m_config = config;

  this->enabled = config->media.asrEnable;
  if (!enabled) 
      return ASR_ERROR_INIT_DISABLED;

  char* configAsrEngine = config->media.asrEngine;

  if (stricmp(configAsrEngine, ASR_ENGINE_NAME_SCANSOFT_SPEECHWORKS) == 0)
      this->engineID = ASR_ENGINE_SCANSOFT_SPEECHWORKS;
  else 
      this->engineID = ASR_ENGINE_NONE;

  int result = 0;

  switch(this->engineID)
  {
    case ASR_ENGINE_NONE: 
      MMSLOG((LM_ERROR, "ASRX configured ASR server '%s' not supported\n", configAsrEngine)); 
      result = ASR_ERROR_INIT_ENGINENAME;
      break;

    case ASR_ENGINE_SCANSOFT_SPEECHWORKS:
      result = this->initSpeechWorks();
      break;
  }
  
  this->enabled = result == 0; // Enable ASR service if OK 
  return result;
}


///////////////////////////////////////////////////////////
// Initialize ScanSoft SpeechWorks OSR engine
int Asr::initSpeechWorks()
{
  // Initialize the configured ScanSoft SpeechWorks server
  #if ASR_ENGINE_IN_USE == ASR_ENGINE_SCANSOFT_SPEECHWORKS

  m_server = new MmsAsrSpeechWorks();
  return m_server->init();

  #else

  return ASR_ERROR_INIT_ENGINE_NOTSUPPORTED;

  #endif
}


///////////////////////////////////////////////////////////
// ASR service clean up
void Asr::dispose()
{
  if (m_server) 
      m_server->cleanup();

  if (m_server)      
      delete m_server;

  m_server = NULL;
}



void Asr::destroy()
{
  if (Asr::m_instance)
  {
      Asr::m_instance->dispose();
      delete Asr::m_instance;
      Asr::m_instance = NULL;
  }
}



///////////////////////////////////////////////////////////
// Create ASR channel for the session
MmsAsrChannel* Asr::createChannel(unsigned long sessionId, unsigned long opId)
{
  MmsAsrChannel* asrChan = new MmsAsrChannel(sessionId, opId);

  if  (asrChan)
       MMSLOG((LM_DEBUG,"ASRX VR channel created on session %d\n", sessionId)); 
  else
  {    MMSLOG((LM_ERROR,"ASRX could not create ASR channel\n"));
       return NULL;
  }

  asrChan->setAsrChannel(this->m_server->createChannel());
  return asrChan;
}


///////////////////////////////////////////////////////////
// Remove ASR channel for the session
int Asr::destroyChannel(MmsAsrChannel* asrChan)
{
  if (asrChan == NULL) return 0;

  this->m_server->destroyChannel(asrChan->getAsrChannel());

  delete asrChan;

  return 0;
}


///////////////////////////////////////////////////////////
// Activate ASR channel for the session
int Asr::activateChannel(MmsAsrChannel* chan, mmsDeviceHandle vdh)
{
  int result = -1;

  if (chan == NULL)
  {
    MMSLOG((LM_ERROR,"ASRX activate channel nonexistent for VR device %d\n", vdh));
    return result;
  }

  if (this->m_server && !this->enabled)
  {
    result = this->m_server->init();
    this->enabled = result == 0;
  }

  result = this->m_server->activateChannel(chan->getAsrChannel(), vdh);

  if (result != 0)
      this->enabled = false;

  return result;
}


///////////////////////////////////////////////////////////
// Remove ASR channel for the session
int Asr::deactivateChannel(MmsAsrChannel* chan)
{
  int result = 0;

  if (chan == NULL)
      return result;

  return this->m_server->deactivateChannel(chan->getAsrChannel());
}


///////////////////////////////////////////////////////////
// Start Recognizer
int Asr::start(MmsAsrChannel* chan)
{
  int result = -1;

  if (chan == NULL)
  {
    MMSLOG((LM_ERROR,"ASRX VR start channel does not exist\n"));
    return result;
  }

  return this->m_server->start(chan->getAsrChannel());
}


///////////////////////////////////////////////////////////
// Stop Recognizer
int Asr::stop(MmsAsrChannel* chan)
{
  int result = 0;

  if (chan == NULL)
      return result;

  return this->m_server->stop(chan->getAsrChannel());
}


///////////////////////////////////////////////////////////
// Handle end of prompt playing
int Asr::promptDone(MmsAsrChannel* chan)
{
  int result = -1;

  if (chan == NULL)
  {
    MMSLOG((LM_ERROR,"ASRX VR prompt end channel does not exist\n"));
    return result;
  }

  return this->m_server->promptDone(chan->getAsrChannel());
}


///////////////////////////////////////////////////////////
// Load grammar
int Asr::loadGrammar(MmsAsrChannel* chan, char* pGrammarType, char* pGrammarData)
{
  int result = -1;

  if (chan == NULL)
  {
    MMSLOG((LM_ERROR,"ASRX grammar load channel does not exist\n"));
    return result;
  }

  return this->m_server->loadGrammar(chan->getAsrChannel(), pGrammarType, pGrammarData);
}


///////////////////////////////////////////////////////////
// Handle data
int Asr::handleData(MmsAsrChannel* chan, char* buffer, UINT length)
{
  int result = -1;

  if (chan == NULL)
  {
    MMSLOG((LM_ERROR,"ASRX VR data input channel does not exist\n"));
    return result;
  }

  return this->m_server->handleData(chan->getAsrChannel(), buffer, length);
}


///////////////////////////////////////////////////////////
// Ask VR engine to compute input data
int Asr::computeData(MmsAsrChannel* chan)
{
  int result = -1;

  if (chan == NULL)
  {
    MMSLOG((LM_ERROR,"ASRX VR comp channel does not exist\n"));
    return result;
  }

  return this->m_server->computeData(chan->getAsrChannel());
}


///////////////////////////////////////////////////////////
// Ask for recognition result
int Asr::getAnswer(MmsAsrChannel* chan, char** answer, int* score)
{
  wchar_t *wanswer = 0;
  int wscore;
  this->m_server->getAnswer(chan->getAsrChannel(), &wanswer, &wscore);

  *score = wscore;

  char dest[MMS_SIZEOF_VR_MEANING];
  ACE_OS::memset(&dest, 0, sizeof(dest));

  try
  {
    if (wanswer)
        // memcpy(&dest, ACE_TEXT_WCHAR_TO_TCHAR(wanswer), sizeof(dest));
        WideCharToMultiByte(CP_ACP, 0, wanswer, -1, dest, sizeof(dest), NULL, NULL);

    ACE_OS::strncpy(*answer, dest, MMS_SIZEOF_VR_MEANING-1);
  }
  catch (...)
  {
  }

  return 0;
}


///////////////////////////////////////////////////////////
// Clean up thread resources
int Asr::cleanupResources()
{
  return this->m_server->cleanupResources();
}



