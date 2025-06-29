//
// mmsAsrSpeechWorks.cpp
// ScanSoft SpeechWorks integration
//
#include "StdAfx.h"
#include "mmsAsr.h"

#if ASR_ENGINE_IN_USE == ASR_ENGINE_SCANSOFT_SPEECHWORKS

#include "mmsAsrSpeechWorks.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

#define APPLICATION_NAME        L"MMS"
#define OSR_MAX_WRITE_BYTES     4000
#define SPEECHWORK_KEY          "SOFTWARE\\SpeechWorks International\\OpenSpeech Recognizer\\3.0"


///////////////////////////////////////////////////////////
int MmsAsrSpeechWorks::init()
{
  int result = -1;

  #ifdef MMS_WINPLATFORM                    // If windows ...
  HKEY  hkey;                               // ... try to get path information
                                             
  if (ERROR_SUCCESS != RegOpenKeyEx(HKEY_LOCAL_MACHINE, SPEECHWORK_KEY, 0, KEY_ALL_ACCESS, &hkey))                                            
  {
    MMSLOG((LM_ERROR, "%s SpeechWorks is not installed properly, ASR engine disabled\n", ASR_TASK)); 
    return -1;
  }
  else
    RegCloseKey(hkey);
  #endif // MMS_WINPLATFORM


  SWIepFuncResult  epRc = SWIep_RESULT_SUCCESS;
  SWIrecFuncResult recRc = SWIrec_SUCCESS;

  // Initialize recognizer
  recRc = SWIrecInit(NULL);

  // Initialize detector
  epRc = SWIepInit();
  if (epRc != SWIep_RESULT_SUCCESS) 
  {
    MMSLOG((LM_ERROR, "%s unable to initialize Speech Detector\n", ASR_TASK)); 
    return result;
  }

  result = 0; 

  inited = true;

  MMSLOG((LM_DEBUG, "%s speechworks engine initialized\n", ASR_TASK)); 

  return result;
}

///////////////////////////////////////////////////////////
void MmsAsrSpeechWorks::cleanup() 
{
}

///////////////////////////////////////////////////////////
AsrChannel* MmsAsrSpeechWorks::createChannel()
{
  if (!this->isInited())
      init();

  if (!this->isInited())
      return NULL;

  AsrChannelSpeechWorks *pChan = NULL;

  pChan = (AsrChannelSpeechWorks*)ACE_OS::malloc(sizeof(AsrChannelSpeechWorks));
  if (pChan == NULL)
  {
    MMSLOG((LM_ERROR, "%s unable to create a new channel\n", ASR_TASK)); 
    return NULL;
  }

  memset(pChan, 0, sizeof(*pChan));

  MMSLOG((LM_DEBUG, "%s new voice recognition channel created\n", ASR_TASK)); 

  return (AsrChannel*)pChan;
}

///////////////////////////////////////////////////////////
void MmsAsrSpeechWorks::destroyChannel(AsrChannel* pChanExt)
{
  AsrChannelSpeechWorks* pChanSpeechWorks = (AsrChannelSpeechWorks*)pChanExt;
  if (!pChanSpeechWorks)
  {
    MMSLOG((LM_ERROR, "%s attempt to destroy invalid channel\n", ASR_TASK)); 
    return;
  }

  SWIepFuncResult epRc = SWIep_RESULT_SUCCESS;
  SWIrecFuncResult recRc = SWIrec_SUCCESS;

  if (pChanSpeechWorks->rech)
  {
    recRc = SWIrecRecognizerDestroy(pChanSpeechWorks->rech);
    if (recRc != SWIrec_SUCCESS)
      MMSLOG((LM_ERROR, "%s unable to destroy recognizer\n", ASR_TASK)); 
    pChanSpeechWorks->rech = NULL;
  }

  if (pChanSpeechWorks->eph)
  {
    epRc = SWIepDetectorDestroy(pChanSpeechWorks->eph);
    if (epRc != SWIep_RESULT_SUCCESS)
      MMSLOG((LM_ERROR, "%s unable to destroy detector\n", ASR_TASK)); 
    pChanSpeechWorks->eph = NULL;
  }

  MMSLOG((LM_DEBUG, "%s voice recognition channel destroyed\n", ASR_TASK)); 

  ACE_OS::free(pChanSpeechWorks);

  pChanSpeechWorks = NULL;
}

///////////////////////////////////////////////////////////
int MmsAsrSpeechWorks::activateChannel(AsrChannel* pChanExt, mmsDeviceHandle vdh)
{
  int result = -1;

  AsrChannelSpeechWorks* pChanSpeechWorks = (AsrChannelSpeechWorks*)pChanExt;
  if (!pChanSpeechWorks)
    return result;

  SWIepFuncResult epRc = SWIep_RESULT_SUCCESS;
  SWIrecFuncResult recRc = SWIrec_SUCCESS;

  if (pChanSpeechWorks->rech || pChanSpeechWorks->eph) 
  {
    MMSLOG((LM_DEBUG,"%s begin activation of VR channel on device %d\n", ASR_TASK, vdh)); 
    deactivateChannel(pChanExt);      // Deactivate the channel first if in progress
  }

  recRc = SWIrecRecognizerCreate(&pChanSpeechWorks->rech, NULL, NULL);
  if (recRc != SWIrec_SUCCESS) 
  {
    MMSLOG((LM_ERROR, "%s unable to create recognizer for device %d\n", ASR_TASK, vdh));
    this->destroyChannel(pChanExt);
    inited = false;
    return result;
  }

  wchar_t chanName[64] = L"";
  swprintf(chanName, L"CSP%03d", vdh);
  recRc = SWIrecRecognizerSetSessionName(pChanSpeechWorks->rech, chanName, APPLICATION_NAME);
  if (recRc != SWIrec_SUCCESS) 
  {
    MMSLOG((LM_ERROR,"%s unable to set recognizer session name for VR device %d\n", ASR_TASK, vdh)); 
    return result;
  }

  epRc = SWIepDetectorCreate(&pChanSpeechWorks->eph);
  if (epRc != SWIep_RESULT_SUCCESS) 
  {
    MMSLOG((LM_ERROR,"%s unable to create detector for VR device %d\n", ASR_TASK, vdh)); 
    return result;
  }

  // Deactivate all grammars so we have a clean slate
  recRc = SWIrecGrammarDeactivate(pChanSpeechWorks->rech, NULL);
  if (recRc != SWIrec_SUCCESS) 
  {
    MMSLOG((LM_ERROR,"%s unable to deactivate grammar\n", ASR_TASK)); 
    return result;
  }

  result = 0;

  MMSLOG((LM_INFO, "%s VR channel activated for device %d\n", ASR_TASK, vdh)); 

  return result;
}

///////////////////////////////////////////////////////////
int MmsAsrSpeechWorks::deactivateChannel(AsrChannel* pChanExt)
{
  int result = -1;

  AsrChannelSpeechWorks* pChanSpeechWorks = (AsrChannelSpeechWorks*)pChanExt;
  if (!pChanSpeechWorks)
    return result;

  if (pChanSpeechWorks->rech == NULL || pChanSpeechWorks->eph == NULL) 
    return result;

  SWIepFuncResult epRc = SWIep_RESULT_SUCCESS;
  SWIrecFuncResult recRc = SWIrec_SUCCESS;

  if (pChanSpeechWorks->eph) 
  {
    epRc = SWIepDetectorDestroy(pChanSpeechWorks->eph);
    if (epRc != SWIep_RESULT_SUCCESS) 
      MMSLOG((LM_ERROR, "%s unable to destroy VR detector\n", ASR_TASK)); 
    pChanSpeechWorks->eph = NULL;
  }

  if (pChanSpeechWorks->rech != NULL) 
  {
    recRc = SWIrecRecognizerDestroy(pChanSpeechWorks->rech);
    if (recRc != SWIrec_SUCCESS) 
      MMSLOG((LM_ERROR, "%s unable to destroy VR recognizer\n", ASR_TASK)); 
    pChanSpeechWorks->rech = NULL;
  }

  result = 0;

  MMSLOG((LM_DEBUG, "%s VR channel deactivated\n", ASR_TASK)); 

  return result;
}

///////////////////////////////////////////////////////////
int MmsAsrSpeechWorks::start(AsrChannel* pChanExt)
{
  int result = -1;

  AsrChannelSpeechWorks* pChanSpeechWorks = (AsrChannelSpeechWorks*)pChanExt;
  if (!pChanSpeechWorks)
    return result;

  if (pChanSpeechWorks->rech == NULL || pChanSpeechWorks->eph == NULL) 
    return result;

  SWIepFuncResult epRc = SWIep_RESULT_SUCCESS;
  SWIrecFuncResult recRc = SWIrec_SUCCESS;

  // Stop recognizer & endpointer in case they are still going from previous recognition attempt.
  if (pChanSpeechWorks->computeActive) 
    stop(pChanExt);

  // Initialize state variables
  pChanSpeechWorks->epState   = SWIep_LOOKING_FOR_SPEECH;
  pChanSpeechWorks->recStatus = SWIrec_STATUS_INCOMPLETE;
  pChanSpeechWorks->asrState  = ASR_STATE_BEFORE_SPEECH;

  recRc = SWIrecRecognizerStart(pChanSpeechWorks->rech);
  if (recRc != SWIrec_SUCCESS) 
  {
    MMSLOG((LM_ERROR, "%s unable to start VR recognizer\n", ASR_TASK)); 
    return result;
  }

  epRc = SWIepStart(pChanSpeechWorks->eph);
  if (epRc != SWIep_RESULT_SUCCESS) 
  {
    MMSLOG((LM_ERROR, "%s unable to start VR endpoint\n", ASR_TASK)); 
    return result;
  }

  result = 0;

  return result;
}

///////////////////////////////////////////////////////////
int MmsAsrSpeechWorks::stop(AsrChannel* pChanExt)
{
  int result = -1;

  AsrChannelSpeechWorks* pChanSpeechWorks = (AsrChannelSpeechWorks*)pChanExt;
  if (!pChanSpeechWorks)
    return result;

  SWIepFuncResult epRc = SWIep_RESULT_SUCCESS;
  SWIrecFuncResult recRc = SWIrec_SUCCESS;

  if (pChanSpeechWorks->rech == NULL || pChanSpeechWorks->eph == NULL) 
    return result;

  epRc = SWIepStop(pChanSpeechWorks->eph);
  if (epRc != SWIep_RESULT_SUCCESS && epRc != SWIep_ERROR_INACTIVE) 
    MMSLOG((LM_ERROR, "%s unable to stop VR endpoint\n", ASR_TASK)); 

  recRc = SWIrecRecognizerStop(pChanSpeechWorks->rech);
  if (recRc != SWIrec_SUCCESS && recRc != SWIrec_ERROR_INACTIVE) 
    MMSLOG((LM_ERROR, "%s unable to stop VR recognizer\n", ASR_TASK)); 

  result = 0;

  return result;
}

///////////////////////////////////////////////////////////
int MmsAsrSpeechWorks::promptDone(AsrChannel* pChanExt)
{
  int result = -1;

  AsrChannelSpeechWorks* pChanSpeechWorks = (AsrChannelSpeechWorks*)pChanExt;
  if (!pChanSpeechWorks)
    return result;

  SWIepFuncResult epRc = SWIep_RESULT_SUCCESS;
  SWIrecFuncResult recRc = SWIrec_SUCCESS;

  if (pChanSpeechWorks->rech == NULL || pChanSpeechWorks->eph == NULL) 
    return result;

  epRc = SWIepPromptDone(pChanSpeechWorks->eph);
  if (epRc != SWIep_RESULT_SUCCESS) 
  {
    MMSLOG((LM_ERROR, "%s unable to notify VR prompt done\n", ASR_TASK)); 
    return result;
  }

  result = 0;

  return result;
}

///////////////////////////////////////////////////////////
int MmsAsrSpeechWorks::loadGrammar(AsrChannel* pChanExt, char* pGrammarType, char* pGrammarData)
{
  int result = -1;
  SWIrecGrammarData grammar;
  int preload = 0;

  AsrChannelSpeechWorks* pChanSpeechWorks = (AsrChannelSpeechWorks*)pChanExt;
  if (!pChanSpeechWorks)
    return result;

  SWIepFuncResult epRc = SWIep_RESULT_SUCCESS;
  SWIrecFuncResult recRc = SWIrec_SUCCESS;
  SWIrecRecognizer *rech = NULL;

  rech = pChanSpeechWorks->rech;
  if (rech == NULL) 
  {
    // create a recognizer just to load the grammar, then destroy it later
    recRc = SWIrecRecognizerCreate(&rech, NULL, NULL);
    if (recRc != SWIrec_SUCCESS)
    {
      MMSLOG((LM_ERROR, "%s unable to create VR recognizer\n", ASR_TASK)); 
      return result;
    }

    preload = 1;
  }

  wchar_t grammarType[16] = L"";
  wcscpy(grammarType, ACE_TEXT_ALWAYS_WCHAR(pGrammarType));

  wchar_t grammarData[256] = L""; 
  wcscpy(grammarData, ACE_TEXT_ALWAYS_WCHAR(pGrammarData));

  memset(&grammar, 0, sizeof(grammar));
  grammar.type = grammarType;
  grammar.data = grammarData;

  recRc = SWIrecGrammarLoad(rech, &grammar);
  if (recRc != SWIrec_SUCCESS) 
  {
    MMSLOG((LM_ERROR, "%s unable to load grammar\n", ASR_TASK)); 
    return result;
  }

  recRc = SWIrecGrammarActivate(pChanSpeechWorks->rech, &grammar, 1, pGrammarData);
  if (recRc != SWIrec_SUCCESS) 
  {
    MMSLOG((LM_ERROR, "%s unable to activate grammar\n", ASR_TASK)); 
    return result;
  }

  if (preload && pChanSpeechWorks != NULL && rech != NULL && pChanSpeechWorks->rech == NULL) 
  {
    //This means we created a recognizer here just to preload the grammar -- so
    // we must destroy this temporary recognizer before returning
    recRc = SWIrecRecognizerDestroy(rech);
    if (recRc != SWIrec_SUCCESS) 
      MMSLOG((LM_ERROR, "%s unable to destroy recognizer\n", ASR_TASK)); 

    rech = NULL;
  }

  result = 0;

  return result;
}

///////////////////////////////////////////////////////////
int MmsAsrSpeechWorks::handleData(AsrChannel* pChanExt, char* buffer, UINT length)
{
  int result = -1;

  AsrChannelSpeechWorks* pChanSpeechWorks = (AsrChannelSpeechWorks*)pChanExt;
  SWIepFuncResult epRc = SWIep_RESULT_SUCCESS;
  SWIrecFuncResult recRc = SWIrec_SUCCESS;
  SWIrecAudioSamples recSamples;
  SWIepAudioSamples epSamples;
  int beginSample = -1, endSample = -1;
  int bytesRemaining = 0;
  unsigned char audioData[OSR_MAX_WRITE_BYTES];
  char epStateStr[64] = "";
  char* thisSampleStart = NULL;

  if (pChanSpeechWorks == NULL) 
    return result;

  if (pChanSpeechWorks->rech == NULL || pChanSpeechWorks->eph == NULL) 
    return result;

  // OSR has a limit on how much we can write any one time, so break it up into chunks if we have to
  thisSampleStart = buffer;
  bytesRemaining = length;

  // If endpointer has already gone idle, don't deliver any more audio
  if (pChanSpeechWorks->epState == SWIep_TIMEOUT ||
      pChanSpeechWorks->epState == SWIep_AUDIO_ERROR ||
      pChanSpeechWorks->epState == SWIep_MAX_SPEECH) 
  {
    bytesRemaining = 0;
  }

  while (bytesRemaining > 0) 
  {
    int thisSampleLength = bytesRemaining;

    if (thisSampleLength >= OSR_MAX_WRITE_BYTES) 
      thisSampleLength = OSR_MAX_WRITE_BYTES;

    memset(&epSamples, 0, sizeof(epSamples));
    epSamples.samples = thisSampleStart;
    epSamples.len     = thisSampleLength;
    epSamples.type    = L"audio/basic";

    // Adjust forward for next chunk if need be
    bytesRemaining -= thisSampleLength;
    thisSampleStart += thisSampleLength;

    // Always deliver audio to the endpointer, even after speech is detected
    epRc = SWIepWrite(pChanSpeechWorks->eph, &epSamples, &pChanSpeechWorks->epState, &beginSample, &endSample);
    if (epRc != SWIep_RESULT_SUCCESS) 
    {
      // Race condition could cause endpointer to go inactive as other thread
      // is shutting things down -- don't squawk about it
      if (epRc != SWIep_ERROR_INACTIVE) 
        return result;
    }

    getSpeechDetectorWriteStatus(pChanSpeechWorks->epState, epStateStr);

    // If BOS timeout has occurred, no further processing is required
    if (pChanSpeechWorks->epState == SWIep_TIMEOUT) 
      pChanSpeechWorks->asrState = ASR_STATE_TIMEOUT;

    // Read it back from the endpointer, see where we are in the utterance
    memset(&recSamples, 0, sizeof(recSamples));
    recSamples.samples = audioData;
    recSamples.type = L"audio/basic";

    epRc = SWIepRead(pChanSpeechWorks->eph, &recSamples, &pChanSpeechWorks->epState, sizeof(audioData));
    if (epRc != SWIep_RESULT_SUCCESS && epRc != SWIep_ERROR_INACTIVE) 
        return result;

    getSpeechDetectorWriteStatus(pChanSpeechWorks->epState, epStateStr);

    switch (pChanSpeechWorks->epState) 
    {
      case SWIep_LOOKING_FOR_SPEECH:
        break;

      case SWIep_IN_SPEECH:
      case SWIep_AFTER_SPEECH:
      case SWIep_MAX_SPEECH:
        // After onset of speech, deliver audio to recognizer too
        if (pChanSpeechWorks->asrState == ASR_STATE_BEFORE_SPEECH) 
          pChanSpeechWorks->asrState = ASR_STATE_SPEECH_DETECTED;

        if (recSamples.len > 0) 
        {
          recRc = SWIrecAudioWrite(pChanSpeechWorks->rech, &recSamples);
          if (recRc != SWIrec_SUCCESS && recRc != SWIrec_ERROR_INACTIVE) 
            return result;
        }
        break;

      case SWIep_TIMEOUT:
        recRc = SWIrecRecognizerStop(pChanSpeechWorks->rech);
        if (recRc != SWIrec_SUCCESS && recRc != SWIrec_ERROR_INACTIVE) 
          MMSLOG((LM_ERROR, "%s unable to stop recognizer\n", ASR_TASK)); 

        pChanSpeechWorks->asrState = ASR_STATE_TIMEOUT;
        break;

      case SWIep_AUDIO_ERROR:
        recRc = SWIrecRecognizerStop(pChanSpeechWorks->rech);
        if (recRc != SWIrec_SUCCESS && recRc != SWIrec_ERROR_INACTIVE) 
          MMSLOG((LM_ERROR, "%s unable to stop recognizer\n", ASR_TASK)); 

        pChanSpeechWorks->asrState = ASR_STATE_ERROR;
        break;

      default:
        recRc = SWIrecRecognizerStop(pChanSpeechWorks->rech);
        if (recRc != SWIrec_SUCCESS && recRc != SWIrec_ERROR_INACTIVE) 
          MMSLOG((LM_ERROR, "%s unable to stop recognizer\n", ASR_TASK)); 

        pChanSpeechWorks->asrState = ASR_STATE_ERROR;
        break;
    }
  }

  result = 0;

  return result;
}


///////////////////////////////////////////////////////////
int MmsAsrSpeechWorks::computeData(AsrChannel* pChanExt)
{
  int result = 0;

  AsrChannelSpeechWorks* pChanSpeechWorks = (AsrChannelSpeechWorks*)pChanExt;
  SWIepFuncResult epRc = SWIep_RESULT_SUCCESS;
  SWIrecFuncResult recRc = SWIrec_SUCCESS;
  SWIrecResultType  resultType;

  if (pChanSpeechWorks == NULL) 
    return result;

  if (pChanSpeechWorks->rech == NULL || pChanSpeechWorks->eph == NULL) 
    return result;

  pChanSpeechWorks->computeActive = TRUE;
  pChanSpeechWorks->recStatus = SWIrec_STATUS_INCOMPLETE;

  while (recRc == SWIrec_SUCCESS &&
          pChanSpeechWorks->recStatus != SWIrec_STATUS_SUCCESS &&
          pChanSpeechWorks->recStatus != SWIrec_STATUS_FAILURE &&
          pChanSpeechWorks->recStatus != SWIrec_STATUS_MAX_CPU_TIME &&
          pChanSpeechWorks->recStatus != SWIrec_STATUS_MAX_SPEECH &&
          pChanSpeechWorks->recStatus != SWIrec_STATUS_STOPPED) 
  {
    recRc = SWIrecRecognizerCompute(pChanSpeechWorks->rech, 
                                    -1, 
                                    &pChanSpeechWorks->recStatus,
                                    &resultType, 
                                    &pChanSpeechWorks->resultData);
    if (recRc != SWIrec_SUCCESS) 
    {
      pChanSpeechWorks->asrState = ASR_STATE_ERROR;
      MMSLOG((LM_ERROR, "%s VR computation error\n", ASR_TASK)); 
    }
  }

  if (recRc == SWIrec_SUCCESS) 
  {
      char statusBuf[512] = "";
      switch(pChanSpeechWorks->recStatus) 
      {
        case SWIrec_STATUS_SUCCESS:
          MMSLOG((LM_DEBUG, "%s computation done with status SWIrec_STATUS_SUCCESS\n", ASR_TASK)); 
          pChanSpeechWorks->asrState = ASR_STATE_SUCCESS;
          break;

        case SWIrec_STATUS_MAX_CPU_TIME:
          MMSLOG((LM_DEBUG, "%s computation done with status SWIrec_STATUS_MAX_CPU_TIME\n", ASR_TASK)); 
          pChanSpeechWorks->asrState = ASR_STATE_SUCCESS;
          break;

        case SWIrec_STATUS_MAX_SPEECH:
          MMSLOG((LM_DEBUG, "%s computation done with status SWIrec_STATUS_MAX_SPEECH\n", ASR_TASK)); 
          pChanSpeechWorks->asrState = ASR_STATE_SUCCESS;
          break;

        case SWIrec_STATUS_REJECTED:
        case SWIrec_STATUS_NO_MATCH:
        case SWIrec_STATUS_NON_SPEECH_DETECTED:
        case SWIrec_STATUS_NO_SPEECH_FOUND:
          pChanSpeechWorks->asrState = ASR_STATE_NOMATCH;
          MMSLOG((LM_DEBUG, "%s computation done with no match\n", ASR_TASK)); 
          break;

        case SWIrec_STATUS_STOPPED:
          MMSLOG((LM_DEBUG, "%s computation done with status SWIrec_STATUS_STOPPED\n", ASR_TASK)); 
          break;

        case SWIrec_STATUS_INCOMPLETE:
        case SWIrec_STATUS_SPEECH_DETECTED:
        case SWIrec_STATUS_SPEECH_COMPLETE:
        default:
          getSpeechRecognizerComputeStatus(pChanSpeechWorks->recStatus, statusBuf);
          pChanSpeechWorks->asrState = ASR_STATE_ERROR;
          MMSLOG((LM_DEBUG, "%s computation error with status %s\n", ASR_TASK, statusBuf)); 
          break;
      }
  }
  else
  {
    MMSLOG((LM_ERROR, "%s VR computation error\n", ASR_TASK)); 
  }
  
  pChanSpeechWorks->computeActive = FALSE;

  return result;
}

///////////////////////////////////////////////////////////
int MmsAsrSpeechWorks::getAnswer(AsrChannel* pChanExt, wchar_t** answer, int* score)
{
  int result = -1;

  AsrChannelSpeechWorks* pChanSpeechWorks = (AsrChannelSpeechWorks*)pChanExt;
  SWIrecFuncResult recRc = SWIrec_SUCCESS;
  unsigned int numAnswers = 0;

  if (pChanSpeechWorks == NULL) 
    return result;

  if (answer == NULL) 
    return result;

  if (pChanSpeechWorks->rech == NULL || pChanSpeechWorks->eph == NULL) 
    return result;

  if (pChanSpeechWorks->recStatus != SWIrec_STATUS_SUCCESS) 
    return result;

  if (pChanSpeechWorks->resultData == NULL)
    return result;

  recRc = SWIrecGetNumAnswers(pChanSpeechWorks->resultData, &numAnswers);
  if (recRc != SWIrec_SUCCESS) 
    return result;
  else if (numAnswers <= 0) 
    return result;

  recRc = SWIrecGetKeyValue(pChanSpeechWorks->resultData, 1, L"SWI_meaning", answer, score);
  if (recRc != SWIrec_SUCCESS) 
  {
    MMSLOG((LM_ERROR,"%s unable to retrieve recognition result\n", ASR_TASK));
    return result;
  }

  result = 0;

  return result;
}

///////////////////////////////////////////////////////////
ASRState MmsAsrSpeechWorks::getState(AsrChannel* pChanExt)
{
  ASRState asrState = ASR_STATE_ERROR;
  AsrChannelSpeechWorks* pChanSpeechWorks = (AsrChannelSpeechWorks*)pChanExt;

  if (!pChanSpeechWorks)
    return asrState;
  else if (pChanSpeechWorks->rech == NULL || pChanSpeechWorks->eph == NULL) 
    return asrState;
  else
    asrState = pChanSpeechWorks->asrState;

  return asrState;
}

///////////////////////////////////////////////////////////
int MmsAsrSpeechWorks::isAsrServerAvailable()
{
  return 1;
}

int MmsAsrSpeechWorks::cleanupResources()
{
  SWIrecFuncResult recRc = SWIrecThreadCleanup();
  if (recRc == SWIrec_SUCCESS) 
  {
    MMSLOG((LM_DEBUG, "%s clean up recognizer thread resources done\n", ASR_TASK)); 
    return 0;
  }
  else
  {
    MMSLOG((LM_ERROR, "%s unable to clean up recognizer thread resources\n", ASR_TASK)); 
    return -1;
  }
}

///////////////////////////////////////////////////////////
void MmsAsrSpeechWorks::getSpeechDetectorWriteStatus(SWIepState epState, char *buf)
{
  switch(epState) 
  {
    case SWIep_LOOKING_FOR_SPEECH: ACE_OS::strcpy(buf, "LOOKING_FOR_SPEECH"); break;
    case SWIep_IN_SPEECH:          ACE_OS::strcpy(buf, "IN_SPEECH"); break;
    case SWIep_AFTER_SPEECH:       ACE_OS::strcpy(buf, "AFTER_SPEECH"); break;
    case SWIep_TIMEOUT:            ACE_OS::strcpy(buf, "TIMEOUT"); break;
    case SWIep_AUDIO_ERROR:        ACE_OS::strcpy(buf, "AUDIO_ERROR"); break;
    case SWIep_MAX_SPEECH:         ACE_OS::strcpy(buf, "MAX_SPEECH"); break;
    default:                       ACE_OS::sprintf(buf, "?%d?", epState); break;
  }
}

///////////////////////////////////////////////////////////
void MmsAsrSpeechWorks::getSpeechDetectorErrorText(SWIepFuncResult err, char *buf)
{
  switch(err) 
  {
    case SWIep_RESULT_SUCCESS:            ACE_OS::strcpy(buf, "SUCCESS"); break;
    case SWIep_ERROR_INIT_STATE_FAILED:   ACE_OS::strcpy(buf, "SWIep_ERROR_INIT_STATE_FAILED"); break;
    case SWIep_ERROR_OUT_OF_MEMORY:       ACE_OS::strcpy(buf, "SWIep_ERROR_OUT_OF_MEMORY"); break;
    case SWIep_ERROR_COLLECTION_ERROR:    ACE_OS::strcpy(buf, "SWIep_ERROR_COLLECTION_ERROR"); break;
    case SWIep_ERROR_BUFFER_OVERFLOW:     ACE_OS::strcpy(buf, "SWIep_ERROR_BUFFER_OVERFLOW"); break;
    case SWIep_ERROR_BUSY:                ACE_OS::strcpy(buf, "SWIep_ERROR_BUSY"); break;
    case SWIep_ERROR_INVALID_PARAMETER:   ACE_OS::strcpy(buf, "SWIep_ERROR_INVALID_PARAMETER"); break;
    case SWIep_ERROR_INACTIVE:            ACE_OS::strcpy(buf, "SWIep_ERROR_INACTIVE"); break;
    case SWIep_ERROR_INVALID_DATA:        ACE_OS::strcpy(buf, "SWIep_ERROR_INVALID_DATA"); break;
    case SWIep_ERROR_NO_LICENSE:          ACE_OS::strcpy(buf, "SWIep_ERROR_NO_LICENSE"); break;
    case SWIep_ERROR_LICENSE_ALLOCATED:   ACE_OS::strcpy(buf, "SWIep_ERROR_LICENSE_ALLOCATED"); break;
    case SWIep_ERROR_LICENSE_FREED:       ACE_OS::strcpy(buf, "SWIep_ERROR_LICENSE_FREED"); break;
    case SWIep_ERROR_UNSUPPORTED:         ACE_OS::strcpy(buf, "SWIep_ERROR_UNSUPPORTED"); break;
    case SWIep_ERROR_SYSTEM_ERROR:        ACE_OS::strcpy(buf, "SWIep_ERROR_SYSTEM_ERROR"); break;
    case SWIep_ERROR_NO_DATA:             ACE_OS::strcpy(buf, "SWIep_ERROR_NO_DATA"); break;
    case SWIep_ERROR_INVALID_PARAMETER_VALUE: ACE_OS::strcpy(buf, "SWIep_ERROR_INVALID_PARAMETER_VALUE"); break;
    case SWIep_ERROR_LICENSE_COMPROMISE:  ACE_OS::strcpy(buf, "SWIep_ERROR_LICENSE_COMPROMISE"); break;
    case SWIep_ERROR_GENERIC_FAILURE:     ACE_OS::strcpy(buf, "SWIep_ERROR_GENERIC_FAILURE"); break;
    case SWIep_ERROR_GENERIC_ERROR:       ACE_OS::strcpy(buf, "SWIep_ERROR_GENERIC_ERROR"); break;
    case SWIep_ERROR_GENERIC_FATAL_ERROR: ACE_OS::strcpy(buf, "SWIep_ERROR_GENERIC_FATAL_ERROR"); break;
    default:                              ACE_OS::sprintf(buf, "?%d?", err); break;
  }
}

///////////////////////////////////////////////////////////
void MmsAsrSpeechWorks::getSpeechRecognizerComputeStatus(SWIrecRecognizerStatus recStatus, char *buf)
{
  switch(recStatus) 
  {
    case SWIrec_STATUS_SUCCESS:             ACE_OS::strcpy(buf, "SUCCESS"); break;
    case SWIrec_STATUS_FAILURE:             ACE_OS::strcpy(buf, "FAILURE"); break;
    case SWIrec_STATUS_INCOMPLETE:          ACE_OS::strcpy(buf, "INCOMPLETE"); break;
    case SWIrec_STATUS_NON_SPEECH_DETECTED: ACE_OS::strcpy(buf, "NON_SPEECH_DETECTED"); break;
    case SWIrec_STATUS_SPEECH_DETECTED:     ACE_OS::strcpy(buf, "SPEECH_DETECTED"); break;
    case SWIrec_STATUS_SPEECH_COMPLETE:     ACE_OS::strcpy(buf, "SPEECH_COMPLETE"); break;
    case SWIrec_STATUS_MAX_CPU_TIME:        ACE_OS::strcpy(buf, "MAX_CPU_TIME"); break;
    case SWIrec_STATUS_MAX_SPEECH:          ACE_OS::strcpy(buf, "MAX_SPEECH"); break;
    case SWIrec_STATUS_STOPPED:             ACE_OS::strcpy(buf, "STOPPED"); break;
    default:                                ACE_OS::sprintf(buf, "?%d?", recStatus); break;
  }
}

///////////////////////////////////////////////////////////
void MmsAsrSpeechWorks::getSpeechRecognizerErrorText(SWIrecFuncResult err, char *buf)
{
  switch(err) 
  {
    case SWIrec_SUCCESS:                  ACE_OS::strcpy(buf, "SUCCESS"); break;
    case SWIrec_ERROR_OUT_OF_MEMORY:      ACE_OS::strcpy(buf, "OUT_OF_MEMORY"); break;
    case SWIrec_ERROR_SYSTEM_ERROR:       ACE_OS::strcpy(buf, "SYSTEM_ERROR"); break;
    case SWIrec_ERROR_UNSUPPORTED:        ACE_OS::strcpy(buf, "UNSUPPORTED"); break;
    case SWIrec_ERROR_BUSY:               ACE_OS::strcpy(buf, "BUSY"); break;
    case SWIrec_ERROR_INITPROC:           ACE_OS::strcpy(buf, "INITPROC"); break;
    case SWIrec_ERROR_INVALID_PARAMETER:  ACE_OS::strcpy(buf, "INVALID_PARAMETER"); break;
    case SWIrec_ERROR_INVALID_PARAMETER_VALUE: ACE_OS::strcpy(buf, "INVALID_PARAMETER_VALUE"); break;
    case SWIrec_ERROR_INVALID_DATA:       ACE_OS::strcpy(buf, "INVALID_DATA"); break;
    case SWIrec_ERROR_INVALID_RULE:       ACE_OS::strcpy(buf, "INVALID_RULE"); break;
    case SWIrec_ERROR_INVALID_WORD:       ACE_OS::strcpy(buf, "INVALID_WORD"); break;
    case SWIrec_ERROR_INVALID_NBEST_INDEX: ACE_OS::strcpy(buf, "INVALID_NBEST_INDEX"); break;
    case SWIrec_ERROR_URI_NOT_FOUND:      ACE_OS::strcpy(buf, "URI_NOT_FOUND"); break;
    case SWIrec_ERROR_GRAMMAR_ERROR:      ACE_OS::strcpy(buf, "GRAMMAR_ERROR"); break;
    case SWIrec_ERROR_AUDIO_OVERFLOW:     ACE_OS::strcpy(buf, "AUDIO_OVERFLOW"); break;
    case SWIrec_ERROR_BUFFER_OVERFLOW:    ACE_OS::strcpy(buf, "BUFFER_OVERFLOW"); break;
    case SWIrec_ERROR_NO_ACTIVE_GRAMMARS: ACE_OS::strcpy(buf, "NO_ACTIVE_GRAMMARS"); break;
    case SWIrec_ERROR_GRAMMAR_NOT_ACTIVATED:  ACE_OS::strcpy(buf, "GRAMMAR_NOT_ACTIVATED"); break;
    case SWIrec_ERROR_NO_SESSION_NAME:    ACE_OS::strcpy(buf, "NO_SESSION_NAME"); break;
    case SWIrec_ERROR_INACTIVE:           ACE_OS::strcpy(buf, "INACTIVE"); break;
    case SWIrec_ERROR_NO_DATA:            ACE_OS::strcpy(buf, "NO_DATA"); break;
    case SWIrec_ERROR_SERVER_UNAVAILABLE: ACE_OS::strcpy(buf, "SERVER_UNAVAILABLE"); break;
    case SWIrec_ERROR_SERVER_CONNECTION_FAILED: ACE_OS::strcpy(buf, "SERVER_CONNECTION_FAILED"); break;
    case SWIrec_ERROR_SERVER_NO_CHANNELS_AVAILABLE: ACE_OS::strcpy(buf, "SERVER_NO_CHANNELS_AVAILABLE"); break;
    case SWIrec_ERROR_NO_LICENSE:         ACE_OS::strcpy(buf, "NO_LICENSE"); break;
    case SWIrec_ERROR_LICENSE_ALLOCATED:  ACE_OS::strcpy(buf, "LICENSE_ALLOCATED"); break;
    case SWIrec_ERROR_LICENSE_FREED:      ACE_OS::strcpy(buf, "LICENSE_FREED"); break;
    case SWIrec_ERROR_LICENSE_COMPROMISE: ACE_OS::strcpy(buf, "LICENSE_COMPROMISE"); break;
    case SWIrec_ERROR_URI_TIMEOUT:        ACE_OS::strcpy(buf, "URI_TIMEOUT"); break;
    case SWIrec_ERROR_URI_FETCH_ERROR:    ACE_OS::strcpy(buf, "URI_FETCH_ERROR"); break;
    case SWIrec_ERROR_INVALID_SYSTEM_CONFIGURATION: ACE_OS::strcpy(buf, "INVALID_SYSTEM_CONFIGURATION"); break;
    case SWIrec_ERROR_INVALID_LANGUAGE:   ACE_OS::strcpy(buf, "INVALID_LANGUAGE"); break;
    case SWIrec_ERROR_DUPLICATE_GRAMMAR:  ACE_OS::strcpy(buf, "DUPLICATE_GRAMMAR"); break;
    case SWIrec_ERROR_GENERIC_FAILURE:    ACE_OS::strcpy(buf, "GENERIC_FAILURE"); break;
    case SWIrec_ERROR_GENERIC_ERROR:      ACE_OS::strcpy(buf, "GENERIC_ERROR"); break;
    case SWIrec_ERROR_GENERIC_FATAL_ERROR: ACE_OS::strcpy(buf, "GENERIC_FATAL_ERROR"); break;
    default:                              ACE_OS::sprintf(buf, "?%d?", err); break;
  }
}

#endif