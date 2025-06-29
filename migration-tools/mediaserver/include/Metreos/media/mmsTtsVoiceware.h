//
// mmsTtsVoiceware.h
//
// NeoSpeech VoiceWare text to speech  
//
#ifndef MMS_TTS_VOICEWARE_H
#define MMS_TTS_VOICEWARE_H

#ifdef MMS_WINPLATFORM
#pragma once
#endif

#include "mmsTts.h"

#if TTS_ENGINE_IN_USE == TTS_ENGINE_NEOSPEECH    

class MmsVoicewareWavData;

// We specify a full path to TTS header files in the source, rather than
// adding an include library to the project, since we do not know until 
// compile time which TTS API we are compiling for, and we wish to be able
// to keep code in the project for tts engines which we are not currently 
// compiling and which may not currently have SDK files present.
#include "X:\\contrib\\neospeech\\libttsapi.h"
 


class MmsTtsVoiceware: public MmsTtsServer
{
  public:     

  MmsTtsVoiceware(): MmsTtsServer(TTS_ENGINE_NEOSPEECH) { }

  int  init();

  void cleanup();

  int  render(void* op, MmsTtsRenderData*, unsigned int params);

  int  isTtsServerAvailable();

  long setVoice(MmsTtsWavData* data); 

  long setVoiceFormat(MmsTtsWavData* data);

  long setVoiceParams(unsigned int params, MmsTtsWavData* data);

  int  fixupTextMarkup(char* s);

  protected:
  int   handle;
  int   isSsmlMarkup(char* text, const int texlen);
  char* errtext(const int);

  int  renderTtsText(MmsVoicewareWavData* data, const int i, const int logOK=0); 

  void buildVoicewareOutdir(MmsVoicewareWavData* data, char* buf);
  int  validateNeospeechConfigFile(const int isLog=0);
};



class MmsVoicewareWavData: public MmsTtsWavData
{
  public:
  MmsVoicewareWavData();
  MmsVoicewareWavData(MmsTtsRenderData* rd);
  void dispose() { }
};



class MmsVoicewareEventData: public MmsTtsEventData
{
  public:
  MmsVoicewareEventData(void* data);
  void dispose();
};


#endif // #if TTS_ENGINE_IN_USE == TTS_ENGINE_NEOSPEECH

#endif // #ifndef MMS_TTS_VOICEWARE_H


