//
// mmsTtsVoiceware.cpp
//
// NeoSpeech VoiceWare text to speech  
//
#include "StdAfx.h"
#include "mmsTts.h"

#if TTS_ENGINE_IN_USE == TTS_ENGINE_NEOSPEECH

#include "mmsTtsVoiceware.h"
#include <sys/stat.h>   

#ifdef MMS_WINPLATFORM
#pragma warning(disable:4786)
#include <minmax.h>
#include <WinSock2.h>
#endif

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

const char *mmsAudioDir = "audio";


MmsVoicewareWavData::MmsVoicewareWavData(): MmsTtsWavData()
{
}


MmsVoicewareWavData::MmsVoicewareWavData(MmsTtsRenderData* rd): MmsTtsWavData(rd)
{
}


int MmsTtsVoiceware::init()
{
  Tts*  tts = Tts::instance();
  const char* ip = tts->m_ttsServerIP;
  int   result = 0;

  if  (!ip || !*ip || !tts->m_ttsServerPort)  
  {
       MMSLOG((LM_ERROR,"TTSX error in configured TTS server IP/port\n")); 
       return TTS_ERROR_INIT_FAILED;
  }

  if (tts->config()->media.ttsValidateVendorConfig)
  {
      result = this->validateNeospeechConfigFile(TRUE);
      if  (result == -1)
      {
           MMSLOG((LM_ERROR,"TTSX validation of c:/ttssrv.ini failed\n")); 
           return tts->raiseTtsInitFailureAlarm(TTS_ERROR_INIT_FAILED);
      }

      MMSLOG((LM_DEBUG,"TTSX TTS vendor config found and validated\n")); 
  }

  #ifdef MMS_WINPLATFORM  // Replace with ACE way to init sockets

	WSADATA wsaData;                          
	if  (0 == WSAStartup((WORD)2, &wsaData)); 
  else result = TTS_ERROR_INIT_FAILED; 
 
  #endif // #ifdef MMS_WINPLATFORM

  return result;
}



int MmsTtsVoiceware::render(void* opx, MmsTtsRenderData* renderData, unsigned int params)
{
  // Render TTS strings as WAV files.        
  // Returns count of files rendered, or -1 if error.

  MmsSession::Op* op = (MmsSession::Op*) opx;
  if (!op || !op->ttsData) return -1;

  MmsStringArray* ttsStrings = op->ttsData->ttsStrings;
  MmsStringArray* ttsFiles   = op->ttsData->ttsFiles;

  if (!ttsStrings || !ttsFiles || ttsFiles->maxEntries() < ttsStrings->count()) 
      return -1;

  int renderCount = 0;
  MmsVoicewareWavData* wavData = new MmsVoicewareWavData(renderData);  
  // As of 3/2/2007 it was unclear where the above allocation got deleted. In that 
  // there do not appear to be any spots where we declare either a MmsTtsWavData* 
  // or a MmsVoicewareWavData*, other than here, and that there are also no spots 
  // where we cast some other pointer to either of these pointer types, this was 
  // apparently a memory leak which is now fixed by deleting the above allocation
  // prior to returning, below. We should keep an eye on this in case something
  // was overlooked.

  // We can configure wav output to 16 or 8-bit resolution
  wavData->rdata.quality = 0;                

  // We currently have no reason to configure voice, since we have only the one
  // voice installed, so we hard-code the voice so as not to risk a config file 
  // typo causing TTS to fail. Eventually we may want to config strings such as
  // "eng-am-fem-1" and map those to specific vendor's voices.
  wavData->rdata.voice = 0; 
           
  wavData->pparamA = op;                     

  wavData->iparamA = this->setVoiceFormat(wavData);
   
  // Since VoiceWare offers the option of writing a wav file direct to disk,
  // we do not currently ask for rendered data in chunks

  if (params != 0xffffffff)  // Any changes to rate or volume?
      this->setVoiceParams(params, wavData); 
   
  
  for(int i=0; i < ttsStrings->count(); i++)
  {   
      // Convert each TTS text string in the playlist to a wav file
      if (0 <= this->renderTtsText(wavData, i, TRUE)) 
          renderCount++;
  } 

  delete wavData; // Added 3/2/2007 -- see comment above.  
  
  return renderCount;
}



int MmsTtsVoiceware::renderTtsText(MmsVoicewareWavData* data, const int i, const int logOnOK)
{
  // Render a single TTS string as a WAV file, returning 0 or -1.

  int result = 0;
  MmsSession::Op* op = (MmsSession::Op*)data->pparamA;  
  MmsStringArray* ttsStrings = op->ttsData->ttsStrings;
  MmsStringArray* ttsFiles   = op->ttsData->ttsFiles;

  const int sessionID = op->sessionID();
  char namebuf[MMS_RECORDFILENAMESIZE + 1];
  Tts* tts = Tts::instance();

  // Unfortunately we cannot pass TTSRequestFile a full path, nor does it return
  // us the path it writes to. The effective path is the concatenation of the
  // directory specified in ttssrv.ini key "NfsDir", and parameter 5 (directory)
  // of TTSRequestFile, which we hard code on the following line for now.
  // If our calculated path, and NeoSpeech's calculated path, were to be
  // not the same, we would not be able to clean up the TTS wav files after use.
  const static char *bit16 = "16bit", *bit8 = "8bit";  

  if (data->cparamA == NULL)                // Voice not yet selected?   
      if (0 != (int)this->setVoice(data)) return -1;    
   
  char* wavFilePath = tts->makeWavFilePath(data);   
  ttsFiles->add(wavFilePath);               // Create WAV path and save with session   

  data->wavFilePath = wavFilePath;          // Write companion file
  Tts::instance()->writeWavFileDescriptor(data);   

  memset(namebuf, 0, sizeof(namebuf));   
  Tts::instance()->getNameFromPath(wavFilePath, namebuf, MMS_RECORDFILENAMESIZE);
  tts->removeNameFromPath(wavFilePath, TRUE); 

  char  vwSubdir[MAXPATHLEN];
  this->buildVoicewareOutdir(data, vwSubdir);

  char* ttsText = ttsStrings->getAt(i);
  this->fixupTextMarkup(ttsText);
  const int textlen  = ACE_OS::strlen(ttsText); 

  // If the text contains SSML markup, mark it for SSML parsing
  const int isSsml   = this->isSsmlMarkup(ttsText, textlen);
  const int textType = isSsml? TEXT_VXML: TEXT_NORMAL;
 
  const int wavFormat = data->iparamA == 8? FORMAT_8BITWAV: FORMAT_WAV; 
  int volume = data->volume;
  if (volume < 1  || volume > 500) volume = 100;
  int speed = data->rate;
  if (speed  < 25 || speed  > 400) speed  = 100; 

  result = TTSRequestFileEx(tts->m_ttsServerIP, tts->m_ttsServerPort, 
           ttsText,     // Text to be rendered
           textlen,     // Length of text to be rendered
           vwSubdir,    // Output directory relative to ttssrv.ini nsfdir
           namebuf,     // Output file name, no extension
           TTS_KATE_DB, // SpeakerID: TTS_KATE_DB, TTS_PAUL_DB, or asian voices
           wavFormat,   // Object format: FORMAT_WAV/PCM/MULAW/ALAW/ADPCM/8BITWAV/8BITPCM
           textType,    // Input text file type: TEXT_NORMAL or TEXT_VXML
           volume,      // Volume: 0 to 500 (%); default -1 or 100
           speed,       // Speed: 25 to 400 (%)  default -1 or 100
           -1,          // Pitch: 50 to 200 (%)  default -1 or 100
           0);          // Userdict index: index 0 specifies default userdict_eng.csv    


  // We altered the path string to terminate after directory, so change it back
  char* p = wavFilePath + strlen(wavFilePath);
  *p = ACE_DIRECTORY_SEPARATOR_CHAR_A;  


  if  (result < 0)
  {    MMSLOG((LM_ERROR,"TTSX session %d TTS error %d '%s' - text not rendered\n", 
               sessionID, result, this->errtext(result)));  

       switch(result)
       { case -1: case -2: case -3: // No connection to TTS server 
              tts->raiseTtsConxFailureAlarm();
       }

       result = -1;
  }           
  else 
  if  (logOnOK)
       MMSLOG((LM_DEBUG,"TTSX session %d TTS to %s %s.wav\n", 
               sessionID, wavFormat == FORMAT_WAV? bit16: bit8, namebuf)); 

  return result;
}



void MmsTtsVoiceware::buildVoicewareOutdir(MmsVoicewareWavData* data, char* buf) 
{
  // Construct the name of the NeoSpeech TTSRequestFileEx "outdir" parameter.
  // This will be "audio" plus, if we are using locale directories, appname
  // plus locale, for example, "audio\myAppName\en-US". 
  // However, note that we now *do not* append appname/locale to the path.

  // Note that if this directory string is incorrect, in that it does not match
  // a specified wav file path, NeoSpeech will cause the stack to become hosed, 
  // and media server will subsequently crash at some unrelated spot. 

  ACE_OS::strcpy(buf,mmsAudioDir);

  #if(0)  // we write TTS wavs to audio root, not to locale directories

  MmsConfig* config = Tts::instance()->config();

  if (!config->serverParams.disregardLocaleDirectories && !data->rdata.ldata.isEmpty())
  {
      char separator[2] = { ACE_DIRECTORY_SEPARATOR_CHAR_A, '\0' };
      ACE_OS::strcat(buf, separator);
      ACE_OS::strcat(buf, data->rdata.ldata.appname);
      ACE_OS::strcat(buf, separator);
      ACE_OS::strcat(buf, data->rdata.ldata.locale);
  } 

  #endif
}



int MmsTtsVoiceware::isSsmlMarkup(char* text, const int textlen)
{
  // Determine if the text to be rendered is SSML markup. 
  char* p = text;                 // Skip whitespace
  while(*p && (*p == ' ' || *p == '\t')) p++;    
  if   (*p != '<') return FALSE;  // First nonblank must be <

  const static char* ssmlsig = "<prompt";
  const static int   ssmlsiglen = 7;

  const int currentLen  = p - text;
  const int expectedLen = currentLen + ssmlsiglen;
  if (expectedLen > textlen) return FALSE;

  char* savePoint = text + expectedLen;
  char  savedChar = *savePoint;
  *savePoint = 0;                 // Temporarily terminate text here
                                  // Compare 7 characters
  const int isSsml = 0 == stricmp(p, ssmlsig);

  *savePoint = savedChar;         // Restore character from above

  return isSsml;
}



long MmsTtsVoiceware::setVoiceFormat(MmsTtsWavData* data)
{
  // Set current voice quality to indicated quality, which if not 8 or 16 bit,
  // defaults to 16 bit.
  int wavFormat = 0;

  switch(data->rdata.quality)                            
  {
    case 8:
    case 16: break;
    default: data->rdata.quality = Tts::instance()->config()->media.ttsQualityBits;
  }

  switch(data->rdata.quality)
  {
    case 8:  wavFormat = 8; break;
    default: wavFormat = 16;
  }

  return wavFormat;
}



long MmsTtsVoiceware::setVoice(MmsTtsWavData* data)  
{
  // Point wavData->cParamA to a text string naming the voice 
  // corresponding to the voice ordinal specified by caller

  // The NeoSpeech engine does not use string representations of voice files
  // however eventually we may want to specify friendly config strings such as
  // "eng:am:f:1", and map those to specific vendor's voices.

  // if (data->voice < 1) data->voice = 0;
  // int sessionID = ((MmsSession*)data->pparamA)->sessionID();

  // We currently have no reason to configure voice, since we have only the one
  // voice installed, so we hard-code the voice so as not to risk a config file 
  // typo causing TTS to fail. 

  data->cparamA = "eng:am:f:1";             // Unused except to test non-null
  data->rdata.voice = TTS_KATE_DB;   

  return 0;
}



long MmsTtsVoiceware::setVoiceParams(unsigned int params, MmsTtsWavData* data)  
{      
  short volumeDelta = (short) params & 0xffff; 
  short speedDelta  = (short)(params >> 16);

  volumeDelta -= TTS_PARAM_OFFSET;               // De-normalize
  speedDelta  -= TTS_PARAM_OFFSET;               // to -10 <= n <= +10

  static const int normalVolume = 100;           // Neospeech normal is 100%
  static const int normalSpeed  = 100;  

  static const int minVolume = 1;                // Percentage of normal
  static const int maxVolume = 500;

  static const int minSpeed  = 25;               // Percentage of normal
  static const int maxSpeed  = 400;

  if  (volumeDelta < 0)
       data->volume = normalVolume + (10 * volumeDelta);
  else data->volume = normalVolume + (40 * volumeDelta);

  if  (speedDelta < 0)
       data->rate = normalSpeed + (10 * speedDelta);
  else data->rate = normalSpeed + (30 * speedDelta);

  return 0;
}



void MmsTtsVoiceware::cleanup() 
{
  // Housekeeping prior to media server shutdown
  #ifdef MMS_WINPLATFORM
  WSACleanup(); 
  #endif
}



int MmsTtsVoiceware::isTtsServerAvailable()
{
  // TODO: determine how to ping the service in a portable manner  
  return 1;
}



char* MmsTtsVoiceware::errtext(const int n)
{
  static const char* ss = "success";      //  0 or 1
  static const char* hn = "hostname";     // -1 
  static const char* sk = "socket";       // -2
  static const char* cn = "connect";      // -3
  static const char* rw = "read/write";   // -4
  static const char* mm = "memory";       // -5
  static const char* tc = "text content"; // -6
  static const char* vf = "voice format"; // -7
  static const char* pr = "parameter";    // -8
  static const char* rs = "result";       // -9
  static const char* sp = "speaker";      // -10
  static const char* dm = "disk media";   // -11
  static const char* uk = "unknown";      // other

  char* s = 0;

  switch(n)
  {
    case  0: 
    case  1:  s = (char*)ss; break;
    case -1:  s = (char*)hn; break;
    case -2:  s = (char*)sk; break;
    case -3:  s = (char*)cn; break;
    case -4:  s = (char*)rw; break;
    case -5:  s = (char*)mm; break;
    case -6:  s = (char*)tc; break;
    case -7:  s = (char*)vf; break;
    case -8:  s = (char*)pr; break;
    case -9:  s = (char*)rs; break;
    case -10: s = (char*)sp; break;
    case -11: s = (char*)dm; break;
    default:  s = (char*)uk; break;  
  }

  return s;
}



int MmsTtsVoiceware::fixupTextMarkup(char* s)
{
  // Client supplies neospeech angle-bracket markup using square brackets
  // since angle brackets get substituted out for &lt, &gt in the app 
  // packaging process. Here we substitute these artificial delimiters 
  // for angle brackets, returning a count of characters replaced.

  if (!s) return 0;
  int count = 0;
  char* p = s, c;

  while(*p)
  {
    switch(c = *p)
    { case '[': *p = '<'; count++; break;
      case ']': *p = '>'; count++;
    }
    p++;
  }

  return count;
}



int MmsTtsVoiceware::validateNeospeechConfigFile(const int isLog)
{
  // Reads Neospeech ini file and validates wav output base directory path therein

  #ifdef MMS_WINPLATFORM
  const static char* inipath = "C:\\ttssrv.ini";
  #else
  const static char* inipath = "C:/ttssrv.ini";
  #endif

  struct stat statinfo; memset(&statinfo, 0, sizeof(struct stat));
  stat(inipath, &statinfo);                 // Get file system info for "ttssrv.ini" 
  const int iniFileSize = statinfo.st_size;
  if (iniFileSize == 0) return -1;          // File not found

  FILE *f = NULL; int result = -1;
  char *buf = NULL, *mmsAudioBasePath = NULL;

  do 
  { f = fopen(inipath, "r");                // Open file  
    if (!f) break;
    buf = new char[iniFileSize];            // Read file contents into buffer
    const int len = fread(buf, 1, iniFileSize, f); 
    fclose(f); f = NULL;                    // Close file
    if (len == 0) break;

    const char* eof = buf + len;
    const char* nfsdirkey = "NfsDir"; 
    const int   nfsdirkeylen = strlen(nfsdirkey);    
    char* p = strstr(buf, nfsdirkey);       // Find "NfsDir" key in file
    if (!p) break; 

    p += (nfsdirkeylen);                    // Find value of NfsDir key/value pair
    while((p < eof) && (ISWHITESPACE(p))) p++;          
    if (p == eof) break;
    const char* ttsWavOutputBasePath = p;
                                            
    char* q = p;                            // Find end of line or start of comment  
    while((q < eof) && (*q != 0x0a) && (*q != 0x3b)) q++;   
    if (q == eof) break;
    if (*--q == 0x0d) q--; 
                                     
    while((q > p) && (ISWHITESPACE(q))) q--;// Back up to end of value string ...
    if (q == p) break;

    const int valuelen = ++q - p;           // ... and terminate string
    *q = '\0';

    MmsConfig* config = Tts::instance()->config();
    if (!config) break;                     // Get media server audio dir path
    char* pmmsAudioBasePath   = config->serverParams.audioBasePath;
    int   mmsAudioBasepathLen = strlen(pmmsAudioBasePath);
    mmsAudioBasePath = new char[mmsAudioBasepathLen+1];
    strcpy(mmsAudioBasePath, pmmsAudioBasePath);

    p = strrchr(mmsAudioBasePath, '\\');    // Chop off "\audio" part
    if (!p) p = strrchr(mmsAudioBasePath, '/');
    if (!p) break;
    *p = 0;
    mmsAudioBasepathLen = strlen(mmsAudioBasePath);
    
    p = mmsAudioBasePath;                   // Convert backslashes to forward
    for(int i=0; i < mmsAudioBasepathLen; i++, p++)
    {   if (*p == '\\') 
            *p  = '/';
    }
                                            // Finally compare the two paths
    if (stricmp(ttsWavOutputBasePath, mmsAudioBasePath) == 0)     
        result = 0;    
    else
    if (isLog)
    {
        MMSLOG((LM_ERROR,"TTSX C:/ttssrv.ini NfsDir does not match CUME audio path\n")); 
        MMSLOG((LM_ERROR,"TTSX NfsDir: %s\n", ttsWavOutputBasePath));
        MMSLOG((LM_ERROR,"TTSX CUME path: %s\n", mmsAudioBasePath));  
    }
  } while(0);

  if (mmsAudioBasePath) delete[] mmsAudioBasePath;
  if (buf) delete[] buf;

  return result;
}
 

#endif // TTS_ENGINE_IN_USE == TTS_ENGINE_NEOSPEECH