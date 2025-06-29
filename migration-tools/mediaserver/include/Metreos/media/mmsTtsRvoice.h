//
// mmsTtsRvoice.h
//
//
// Rhetorical rVoice Text to speech  
//
#ifndef MMS_TTS_RVOICE_H
#define MMS_TTS_RVOICE_H

#ifdef MMS_WINPLATFORM
#pragma once
#endif

#include "mmsTts.h"

#if TTS_ENGINE_IN_USE == TTS_ENGINE_RHETORICAL_RVOICE

class MmsRvoiceWavData;

#include "c:\\program files\\rhetorical systems\\rvoice\\rapi\\include\\rapi.h"
#include "c:\\program files\\rhetorical systems\\rvoice\\rapi\\include\\rapiw32.h"


class MmsTtsRvoice: public MmsTtsServer
{
  public:

  MmsTtsRvoice(): MmsTtsServer(TTS_ENGINE_RHETORICAL_RVOICE) { }

  int  init();

  void cleanup();

  int  render(MmsSession::Op*, MmsTtsRenderData*, unsigned int params);  

  int  isTtsServerAvailable();

  long setVoice(MmsTtsWavData* data); 

  long setVoiceFormat(MmsTtsWavData* data);

  long setVoiceParams(unsigned int params, MmsTtsWavData* data);

  static void ttsCallback(void* userData, long eventType,  const char* wavChunk,
              long dataType, long dataLength, const char* phoneSet, 
              const char* cparam1, const char* cparam2);
  protected:
  int handle;

  int renderTtsText(MmsRvoiceWavData* data, const int i); 
};



class MmsRvoiceWavData: public MmsTtsWavData
{
  public:
  MmsRvoiceWavData();
  void dispose() { }
};


class MmsRvoiceEventData: public MmsTtsEventData
{
  public:
  MmsRvoiceEventData(void* data);
  void dispose();
};


#endif // #if TTS_ENGINE_IN_USE == TTS_ENGINE_RHETORICAL_RVOICE

#endif // #ifndef MMS_TTS_RVOICE_H


