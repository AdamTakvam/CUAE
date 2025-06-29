//
// mmsTtsRvoice.cpp
//
// Rhetorical rVoice Text to speech  
//
#include "StdAfx.h"
#include "mmsTts.h"

#if TTS_ENGINE_IN_USE == TTS_ENGINE_RHETORICAL_RVOICE

#include "mmsTtsRvoice.h"

#ifdef MMS_WINPLATFORM
#pragma warning(disable:4786)
#include <minmax.h>
#endif

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION


MmsRvoiceWavData::MmsRvoiceWavData(): MmsTtsWavData()
{
}


MmsRvoiceWavData::MmsRvoiceWavData(MmsTtsRenderData* rd): MmsTtsWavData(rd)
{
}


int MmsTtsRvoice::init()
{
  MmsConfig* config   = Tts::instance()->config();
  char* ttsServerIP   = config->media.ttsServerIP;
  int   ttsServerPort = config->media.ttsServerPort;
                    
  // Set the servers to be queried for rVoice voices. It should be invoked
  // before any rAPI handles are created to prevent rAPI looking in the registry
  // for known servers. The list is of the form ip1:port1;ip2:port2;ipn:portn
  char ipPort[64];
  ACE_OS::sprintf (ipPort, "%s:%d", ttsServerIP, ttsServerPort);

  long result  = rAPI_Set_Servers(ipPort); 

  if  (result != RAPI_SUCCESS)  
  {
       MMSLOG((LM_ERROR,"TTSX error rAPI_Set_Servers\n")); 
       return -1;
  }

  return 0;
}



int MmsTtsRvoice::render(MmsSession::Op* sessionOp, MmsTtsRenderData& data, unsigned int params)
{
  // Render TTS strings as WAV files.    
  // Returns count of files rendered, or -1 if error.
  MmsSession::Op* op = (MmsSession::Op*)sessionOp;
  if (!op || !op->ttsData) return -1;
  MmsStringArray* ttsStrings = op->ttsData->ttsStrings;
  MmsStringArray* ttsFiles   = op->ttsData->ttsFiles;
  if (!ttsStrings || !ttsFiles || ttsFiles->maxEntries() < ttsStrings->count()) 
      return -1;

  MmsRvoiceWavData* wavData = new MmsRvoiceWavData();
  wavData->quality = quality;
  wavData->voice   = voice;
  wavData->pparamA = op;                    // 2.1.5

  wavData->iparamA = this->setVoiceFormat(wavData);
  int  renderCount = 0;

  if (params != 0xffffffff)  // Any changes to rate or volume?
      this->setVoiceParams(params, wavData); 

  // Although rAPI_Speak results in callbacks for each rendered WAV chunk,
  // the call will not return until the entire sample has been rendered, so
  // we can loop in this manner, without resorting to chaining renderings.

  for(int i=0; i < ttsStrings->count(); i++)
  {
      if (0 == this->renderTtsText(wavData, i)) 
          renderCount++;
  }
  
  return renderCount;
}


int MmsTtsRvoice::renderTtsText(MmsRvoiceWavData* data, const int i)
{
  // Render a single TTS string as a WAV file, returning 0 or -1.
  int result = 0;

  MmsSession::Op* op = (MmsSession::Op*)data->pparamA;
  MmsStringArray* ttsStrings = op->ttsData->ttsStrings;
  MmsStringArray* ttsFiles   = op->ttsData->ttsFiles;

  data->handle = rAPI_Create_Handle(0, data->iparamA, MmsTtsRvoice::ttsCallback, data);  
  if (!data->handle)  
  {
      MMSLOG((LM_ERROR,"TTSX session %d error rAPI_Create_Handle\n", session->sessionID())); 
      return -1;
  }

  if (data->cparamA == NULL)                // Voice not yet selected?   
      result = (int)this->setVoice(data);
       
  if (result == 0) 
  {
      char* wavFilePath = Tts::instance()->makeWavFilePath(data);
      ttsFiles->add(wavFilePath);           // Create WAV path and save with session
                                            // Render the text string as WAV file
      long result  = rAPI_Speak(data->handle, data->cparamA, ttsStrings->getAt(i)); 

      if  (result != RAPI_SUCCESS)  
      {                                      
           MMSLOG((LM_ERROR,"TTSX session %d error rAPI_Speak\n", op->sessionID())); 
           result = -1;
      }
  }
     
  rAPI_Close_Handle(data->handle);

  return result;
}



void MmsTtsRvoice::ttsCallback(void* userData, long eventType,  const char* wavChunk, 
     long dataType, long dataLength, const char* phoneSet, 
     const char* cparam1, const char* cparam2)
{     
  // TTS server sends raw PCM chunks through this callback during a rAPI_Speak(). 
  // When TTS server indicates that this is the final chunk, we write the 
  // accumulated data to disk as a WAV file. rAPI frees memory for the PCM chunk.

  MmsTtsWavData* data = (MmsTtsWavData*) userData;
  if (!data || data->sig != MmsTtsWavData::signature) return;
  if (!wavChunk || !dataLength) return;
  
  char* ndata = (char*) ACE_OS::realloc(data->wavData, data->wavDataLength + dataLength);
  if (ndata) data->wavData = ndata;  
                                            // Double check this (dataLength)
  memcpy(&data->wavData[data->wavDataLength], wavChunk, dataLength);
  data->wavDataLength += dataLength;  
                                            // If done rendering text ...
  if (eventType == RAPI_FINISHED_EVENT && data->filePath)
  {                                         // ... write wav file, freeing
      Tts::instance()->writeWavFile         // the PCM data memory, ...
        (data->wavData, data->wavDataLength, data->quality, data->filePath, TRUE);
                                            // ... and write .mms file
      Tts::instance()->writeWavFileDescriptor(data);
  }
}


long MmsTtsRvoice::setVoiceFormat(MmsTtsWavData* data)
{
  // Set current voice quality to indicated quality, which if not 8 or 16 KHz,
  // defaults to 8 KHz.
  int rapiFormat = 0;

  switch(data->quality)                            
  {
    case 8:
    case 16: break;
    default: data->quality = Tts::instance()->config()->media.ttsQualityKHz;
  }

  switch(data->quality)
  {
    case 16: rapiFormat = RAPI_PCM_16bit_16KHz; break;
    default: rapiFormat = RAPI_PCM_16bit_8KHz;
  }

  return rapiFormat;
}



long MmsTtsRvoice::setVoice(MmsTtsWavData* data)  
{
  // Point wavData->cParamA to a text string naming the voice 
  // corresponding to the voice ordinal specified by caller

  if (data->cparamA) return 0;
  if (data->voice < 1) data->voice = 1;
  int sessionID = ((MmsSession*)data->pparamA)->sessionID();

  long voiceCount = rAPI_Enumerate_Voices(data->handle);

  if (0 == voiceCount)
  {
      MMSLOG((LM_ERROR,"TTSX session %d error rAPI_Enumerate_Voices\n", sessionID)); 
      return -1;
  }

  if (data->voice == 0) data->voice = 1;   

  if (data->voice > voiceCount)
  {
      MMSLOG((LM_WARNING,"TTSX session %d voice %d not available\n", sessionID));
      return -1;
  }

  for(int i = 1; i <= voiceCount; i++)
  {
      const char *voice = rAPI_Next_Voice(handle);
      if (!voice)
      {
          MMSLOG((LM_ERROR,"TTSX session %d error rAPI_Next_Voice\n",sessionID));
          return -1;
      }

      if (i == data->voice)
      {
          data->cparamA = (char*) voice;
          break;
      }
  }
    
  return 0;
}



long MmsTtsRvoice::setVoiceParams(unsigned int params, MmsTtsWavData* data)  
{            
  // Voice rate and volume are packed into render parameter params. If there
  // is to be no change (default) the segment will be all 1 bits, otherwise
  // the segment contains the new value for the parameter.
  // Voice rate is in the low 16 bits. Range is -10 <= rate <= +10, default 0.
  // Voice volume is in the high 16 bits. Range is 0 <= vol <= 100, default 50.

  int  rate   = params & 0xffff; 
  if  (rate  != 0xffff && RAPI_SUCCESS == rAPI_Set_Rate(data->handle, rate))
       data->rate = rate;
      
  int  volume = (params & 0xffff0000) >> 16;
  if  (volume != 0xffff && RAPI_SUCCESS == rAPI_Set_Volume(data->handle, volume))
       data->volume = volume;

  return 0;
}


void MmsTtsRvoice::cleanup() 
{
  // Housekeeping prior to media server shutdown
  rAPI_Clean_Up();
}



int MmsTtsRvoice::isTtsServerAvailable()
{
  // Unsure how to ping server. rAPI_Is_Server_Available  
  // requires a handle. Rhetorical tech support was no help.
  return 1;
}


#else // TTS_ENGINE_IN_USE == TTS_ENGINE_RHETORICAL_RVOICE

// Linker needs at least one public nonstatic symbol or it spews LNK4221
class TTSRVOICE_SUPPRESS_LNK4221 { public: void SUPPRESS_LNK4221_WARNING(); };
void  TTSRVOICE_SUPPRESS_LNK4221::SUPPRESS_LNK4221_WARNING() { }

#endif // TTS_ENGINE_IN_USE == TTS_ENGINE_RHETORICAL_RVOICE
