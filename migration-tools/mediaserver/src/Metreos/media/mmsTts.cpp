//
// MmsTts.cpp
//
// Text to speech
//
#include "StdAfx.h"
#include "mms.h"
#include "mmsSession.h"
#include "mmsReporter.h"
#include "mmsServerCmdHeader.h"
#include "mmsTtsRvoice.h"
#include "mmsTtsVoiceware.h"

#ifdef  MMS_WINPLATFORM
#pragma warning(disable:4786)
#include <minmax.h>
#endif

#include "mmsTts.h"
#include "mmsAudioFileDescriptor.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

Tts* Tts::m_instance = 0;  


int Tts::isTtsServerAvailable()
{
   // Indicate if TTS services are currently available
   if (!this->enabled || !m_server) return 0;
   return m_server->isTtsServerAvailable();
}


int Tts::init(MmsConfig* config) // Initialize MMS TTS services
{    
    m_config  = config;
    m_ttsServerIP   = config->media.ttsServerIP;
    m_ttsServerPort = config->media.ttsServerPort;

    this->enabled = config->media.ttsEnable;
    if  (!enabled) return TTS_ERROR_INIT_DISABLED;

    char* configTtsEngine = config->media.ttsEngine;

    if  (stricmp(configTtsEngine, TTS_ENGINE_NAME_RHETORICAL_RVOICE) == 0)
         this->engineID  = TTS_ENGINE_RHETORICAL_RVOICE;
    else
    if  (stricmp(configTtsEngine, TTS_ENGINE_NAME_LOQUENDO) == 0)
         this->engineID  = TTS_ENGINE_LOQUENDO;
    else
    if  (stricmp(configTtsEngine, TTS_ENGINE_NAME_NEOSPEECH) == 0)
         this->engineID  = TTS_ENGINE_NEOSPEECH;

    else this->engineID  = TTS_ENGINE_NONE;
    int result = 0;

    switch(this->engineID)
    {
      case TTS_ENGINE_NONE: 
           MMSLOG((LM_ERROR,"TTSX config TTS server '%s' not recognized\n", configTtsEngine)); 
           result = TTS_ERROR_INIT_ENGINENAME;
           break;

      case TTS_ENGINE_RHETORICAL_RVOICE:
           result = this->initRvoice();
           break;

      case TTS_ENGINE_LOQUENDO:
           result = this->initLoquendo();
           break;

      case TTS_ENGINE_NEOSPEECH:
           result = this->initNeoSpeech();
           break;
    }

    this->enabled = result == 0;
    return result;
}



int Tts::render(void* op, MmsTtsRenderData* renderData, unsigned int params)
{
  // Invoke configured TTS server to render session's TTS string to WAV files
  MmsSession::Op* operation = (MmsSession::Op*)op;

  if (!this->isTtsServerAvailable()) return MMS_ERROR_RESOURCE_UNAVAILABLE;
                                            // Ensure TTS strings exist
  if (!operation->ttsData || operation->ttsData->ttsStrings->count() == 0)
      return MMS_ERROR_PARAMETER_VALUE;
                    
  // Here we monitor available TTS port licenses. If we are bypassing licensing
  // facilities, we assume here that the TTS engine will queue the request. 
  // We atomically check TTS port available count and reserve one if available: 
  // LICX TTS- (1 of 1)
  const int isTtsExhausted = MmsAs::tts(MmsAs::RESX_ISZERO, MmsAs::RESX_RESERVE);
  const int isOverride = HmpResourceManager::instance()->isInternalLicensingOverridePresent();
                                      
  if (isTtsExhausted && !isOverride)        // LICX TTSOUT 
      return this->raiseTtsPortsExhaustedAlarm(operation->sID(), operation->opID());

  operation->ttsData->alloc();              // Allocate file paths 

  if (renderData->quality == 0) 
      renderData->quality = m_config->media.ttsQualityBits;

  short speed  = m_config->media.ttsVoiceRate;
  short volume = m_config->media.ttsVolume;

  if (speed != 0 || volume != 0)
  {
      if (speed  >  10)  speed  =  10; else
      if (speed  <(-10)) speed  =(-10);
      if (volume >  10)  volume =  10; else
      if (volume <(-10)) volume =(-10);

      speed  += TTS_PARAM_OFFSET;           // Negative value normalization 
      volume += TTS_PARAM_OFFSET;
      params  = (speed << 16) | volume;
  }
                                            // Get back count of files rendered
  const int result = m_server->render(op, renderData, params); 

  // Unburn the TTS port license (we assume the particular TTS engine renders 
  // the TTS text with a synchronous call) 
  MmsAs::tts(MmsAs::RESX_INC);             // LICX TTS+ (1 of 1)

  return result > 0? 0: MMS_ERROR_DEVICE;
}



int Tts::raiseTtsPortsExhaustedAlarm(const int sessionID, const int opID)
{
  // Fire an alarm indicating TTS ports exhausted, also log the condition

  char alarmDescription[MmsAlarmParams::maxTextLength+1];
  sprintf(alarmDescription, MmsAs::szResxExhausted, "TTS");

  MMSLOG((LM_ERROR,"TTSX session %d %s\n", sessionID, alarmDescription));

  MmsAlarmParams* params = new MmsAlarmParams(MmsReporter::NDX_NO_MORE_TTS_PORTS_FAILS, 
     MMS_STAT_CATEGORY_TTS, MmsReporter::severityTypeSevere);
  params->settext(alarmDescription);

  MmsReporter::instance()->postMessage(MMSM_ALARMREQUEST, (long)params);

  return MMS_ERROR_RESOURCE_UNAVAILABLE;
}



int Tts::raiseTtsInitFailureAlarm(const int retval)
{
  char* msg = "TTS services could not be initialized - TTS will be unavailable";
  MmsReporter::raiseServerAlarm(MmsReporter::NDX_UNEXPECTED_CONDITION, msg);
  return retval;
}



int Tts::raiseTtsConxFailureAlarm(const int retval)
{
  char* msg = "could not contact TTS service - TTS is unavailable";
  MmsReporter::raiseServerAlarm(MmsReporter::NDX_UNEXPECTED_CONDITION, msg);
  return retval;
}



char* Tts::makeWavFilePath(MmsTtsWavData* data, const int omitExtension)
{
  // Construct a full path to a WAV file. Filename format is xxxxxxxx.wav.
  // The path string gets freed in when the session command ends. 

  char namebuf[8 + 1 + 3 + 1];
  MmsSession::Op* operation = (MmsSession::Op*)data->pparamA;
  MmsSession* session = operation? operation->Session(): NULL;
  
  const int extension = omitExtension? MMS_OMIT_EXTENSION: MMS_FILETYPE_WAV;
  if (session)
      session->createFilename(namebuf, extension);

  char* fullpath = new char[MAXPATHLEN]; memset(fullpath, 0, MAXPATHLEN);

  if (session)
      session->buildPlayfileFullPath
         (fullpath, namebuf, data->rdata.ldata, TRUE, TRUE);

  data->wavFilePath = fullpath;
  return fullpath;
}



void Tts::getNameFromPath(const char* path, char* name, const int namelen)
{
  // Given path string, place the non-directory part into <name>
  if (!path || !name || !namelen) return;
  char* p = strrchr(path, ACE_DIRECTORY_SEPARATOR_CHAR_A);
  if (!p) return;
  strncpy(name, ++p, namelen);
  char* q = strchr(name, '.');              // Lose extension if any
  if (q) *q = '\0';
}



char Tts::removeNameFromPath(const char* path, const int removeTrailingSlash)
{
  // Given path string, move the null terminator ahead of the filename part
  // returning the character which was removed
  char* q = NULL, c = 0;
  if (path) q = strrchr(path, ACE_DIRECTORY_SEPARATOR_CHAR_A);
  if (!q) return NULL;
  if (!removeTrailingSlash) q++;
  c  = *q;
  *q = '\0';
  return c;
}



int Tts::writeWavFileDescriptor(MmsTtsWavData* data)
{
  // Write a media server file descriptor for the TTS WAV file.
  // The purpose of this file is to permit cleanup of TTS WAV files
  // if media server is configured to not remove, or fails to remove,
  // the TTS WAV files once the play command has completed.

  MmsSession* session = (MmsSession*)data->pparamA;

  MMSPLAYFILEINFO pfinfo;
  pfinfo.isTtsText = TRUE;
  pfinfo.path = data->wavFilePath;
  pfinfo.pathlength = ACE_OS::strlen(pfinfo.path);
  session->makePropertiesFilePath(&pfinfo);

  MmsDeviceVoice::MMS_PLAYRECINFO prinfo; 
  prinfo.rate       = data->rate?  data->rate:    
                      MmsDeviceVoice::RATE_8KHZ;
  prinfo.samplesize = data->rdata.quality? data->rdata.quality: 
                      MmsDeviceVoice::SIZE_16BIT;
  prinfo.mode       = MmsDeviceVoice::PCM;
  prinfo.filetype   = MmsDeviceVoice::WAV;

  // Write a descriptor which expires today. It and its companion WAV
  // file will, if not cleaned up after completion of play, be deleted
  // during the next media server file policing, which by default
  // occurs every four hours.
  session->writeDescriptorFile(prinfo, &pfinfo, -1);

  return 0;
}



void Tts::writeWavFile
( char* pcmdata, long length, int KHz, char* path, const int dispose)
{
  // Write a WAV file using supplied raw PCM data and length, and resolution
  // (8 or 16 KHz), to the supplied file path. Caller must specify TRUE for 
  // parameter 4 (dispose) if this method is to free the PCM data memory.

  MmsWavHeader* header = new MmsWavHeader(KHz, length);
  FILE* f = NULL;
 
  if (f = fopen(path,"wb"))
  {   
      fwrite(header, 1, sizeof(MmsWavHeader), f);
      fwrite(pcmdata,1, length, f);
      fclose(f);
  }

  delete[] header;

  if (dispose)
      delete[] pcmdata;
}



void Tts::dispose()
{
  if (m_server) 
      m_server->cleanup();

  if (m_server)      
      delete m_server;

  m_server = NULL;
}



void Tts::destroy()
{
  if (Tts::m_instance)
  {
      Tts::m_instance->dispose();
      delete Tts::m_instance;
      Tts::m_instance = NULL;
  }
}



int Tts::initRvoice()
{
  // Initialize the configured Rhetorical rVoice TTS server
  #if TTS_ENGINE_IN_USE == TTS_ENGINE_RHETORICAL_RVOICE

  m_server = new MmsTtsRvoice();
  return m_server->init();

  #else
  return TTS_ERROR_INIT_ENGINE_NOTSUPPORTED;
  #endif
}



int Tts::initLoquendo()
{
  // Initialize the configured Loquendo TTS server
  #if TTS_ENGINE_IN_USE == TTS_ENGINE_LOQUENDO

  m_server = new MmsTtsLoquendo();
  return m_server->init();

  #else
  return TTS_ERROR_INIT_ENGINE_NOTSUPPORTED;
  #endif
}



int Tts::initNeoSpeech()
{
  // Initialize the configured NeoSpeech VoiceText TTS server
  #if TTS_ENGINE_IN_USE == TTS_ENGINE_NEOSPEECH

  m_server = new MmsTtsVoiceware();
  return m_server->init();

  #else
  return TTS_ERROR_INIT_ENGINE_NOTSUPPORTED;
  #endif
}


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
// MmsTtsSessionData
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

void MmsTtsSessionData::dispose()
{
  if (ttsStrings) delete ttsStrings;

  if (ttsFiles)
  {   // If configured to remove TTS WAV files immediately after use,
      // delete them now, rather than wait for periodic expiration.
      if (config->media.ttsExpireDays == 0)
          this->deleteReferencedFiles();

      delete ttsFiles;
  }

  ttsStrings = ttsFiles = NULL;
}



void MmsTtsSessionData::deleteReferencedFiles()
{
  for(int i=0; i < ttsFiles->count(); i++)
  {
    char* path = ttsFiles->getAt(i);        // Remove .wav file
    if (!path || _access(path, 2) == -1) continue;  
    ACE_OS::unlink(path); 
                                            
    char* p = path + ACE_OS::strlen(path);  // Find .wav file extension
    while(p > path && *p != '.') p--;      
    if (p <= path || ACE_OS::strlen(p) != 4) continue;
                                            // Change file extension to .mms
    ACE_OS::strcpy(p, MMS_RECORD_PROPERTIES_FILE_EXTENSION);              
    if (_access(path, 2) != -1)             // If .mms companion file exists ...
        ACE_OS::unlink(path);               // ... remove that file also.
  }
}


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
// MmsTtsWavData
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

void MmsTtsWavData::clear()
{
  this->wavData = this->cparamA = this->cparamB = 0;
  this->iparamA = this->iparamB = this->iparamC = 0;
  this->pparamA = this->pparamB = 0;
  this->volume  = this->rate    = 0;
  this->wavDataLength = this->handle = 0;
  this->rdata.clear();
  this->sig = signature;
}


MmsTtsWavData::MmsTtsWavData(MmsTtsRenderData* rd)  
{ 
  clear(); 
  memcpy(&rdata, rd, sizeof(MmsTtsRenderData)); 
}


MmsTtsWavData::~MmsTtsWavData()  
{ 
  this->dispose();

  if (wavData) delete[] wavData;
}



