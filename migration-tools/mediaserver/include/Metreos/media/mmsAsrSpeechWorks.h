// mmsAsrSpeechWorks.cpp
// ScanSoft SpeechWorks integration

#ifndef MMS_ASRSPEECHWORKS_H
#define MMS_ASRSPEECHWORKS_H

#include "mmsAsr.h"

#if ASR_ENGINE_IN_USE == ASR_ENGINE_SCANSOFT_SPEECHWORKS    

#include "SWIrecAPI.h"
#include "SWIepAPI.h"

// Data structure to hold everything needed to perform recognition for a channel
typedef struct AsrChannelSpeechWorks {
  SWIepDetector *eph;                 // Endpointer object 
  SWIepState epState;                 // State of endpointer 
  SWIrecRecognizer *rech;             // Recognizer object 
  SWIrecRecognizerStatus recStatus;   // Status from Compute() 
  SWIrecResultData *resultData;       // Recognition results data 
  ASRState asrState;                  // Recognition state 
  //Thread threadId;                  // Thread handle 
  int computeActive;                  // Is compute thread active? 
} AsrChannelSpeechWorks;

// MmsAsrSpeechWorks: ScanSoft SpeechWorks OSR integration
class MmsAsrSpeechWorks: public MmsAsrServer
{
public:     
  MmsAsrSpeechWorks(): MmsAsrServer(ASR_ENGINE_SCANSOFT_SPEECHWORKS) { inited = false; }
  int init();
  void cleanup();
  int isAsrServerAvailable();
  AsrChannel* createChannel();
  void destroyChannel(AsrChannel* pChanExt);
  int activateChannel(AsrChannel* pChanExt, mmsDeviceHandle vdh); 
  int deactivateChannel(AsrChannel* pChanExt); 
  int start(AsrChannel* pChanExt);
  int stop(AsrChannel* pChanExt);
  int promptDone(AsrChannel* pChanExt);
  int loadGrammar(AsrChannel* pChanExt, char* pGrammarType, char* pGrammarData);
  int handleData(AsrChannel* pChanExt, char* buffer, UINT length);
  int computeData(AsrChannel* pChanExt);
  int getAnswer(AsrChannel* pChanExt, wchar_t** answer, int* score);
  ASRState getState(AsrChannel* pChanExt);
  int cleanupResources();
  bool isInited() { return inited; }
  
protected:
  void getSpeechDetectorWriteStatus(SWIepState epState, char *buf);
  void getSpeechDetectorErrorText(SWIepFuncResult err, char *buf);
  void getSpeechRecognizerComputeStatus(SWIrecRecognizerStatus recStatus, char *buf);
  void getSpeechRecognizerErrorText(SWIrecFuncResult err, char *buf);

private: 
  bool inited;
};

#endif 
#endif