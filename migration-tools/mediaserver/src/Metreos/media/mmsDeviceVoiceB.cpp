//
// mmsDeviceVoice.cpp
//
#include "StdAfx.h"
#include "mms.h"
#ifdef  MMS_WINPLATFORM
#pragma warning(disable:4786)
#endif

#include "mmsDeviceVoice.h" 
#include "mmsParameterMap.h"
#include "mmsAsr.h"
#include "mmsMsgTypes.h"
#include "mmsWavHeader.h"    
#include <eclib.h>
#include <sys/stat.h>   

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION



int MmsDeviceVoice::pcmToWav(char* wavpath, MMS_PLAYRECINFO* recinfo)
{
  // Adds a wav header to raw PCM file passed as wavpath
  struct stat statinfo; memset(&statinfo, 0, sizeof(struct stat));
  stat(wavpath, &statinfo);                 // Get size of raw pcm file
  const int pcmFileSize = statinfo.st_size;
  if (pcmFileSize == 0) return -1;

  int isError = TRUE, renresult = -1, c = 0, written = 0;
  MmsWavHeader* wavHeader = NULL;
  FILE* fWav = NULL, *fPcm = NULL;
                                            // Change pcm path extension
  char pcmpath[MAXPATHLEN]; strcpy(pcmpath, wavpath);
  MmsDirectoryRecursor::PathInfo  pathinfo; 
  MmsDirectoryRecursor::parsepath(pathinfo, pcmpath);
  strcpy(pathinfo.ext, ".pcm"); 
  
  do {   
                                            // Rename raw pcm file
  renresult = ACE_OS::rename(wavpath, pcmpath);
  if (renresult == -1) break;                   
                                            // Open raw pcm file
  fPcm = fopen(pcmpath, "r"); if (!fPcm) break;
                                            // Open new wav file 
  fWav = fopen(wavpath,"wb"); if (!fWav) break; 
                                            // Create wav header data
  wavHeader = new MmsWavHeader(recinfo->rate, pcmFileSize);
                                            // Write wav header to wav file
  if (0 == fwrite(wavHeader, 1, sizeof(MmsWavHeader), fWav)) break;

  while(EOF != (c = fgetc(fPcm)))           // Copy raw pcm data to wav file
        written += (EOF != fputc(c, fWav));

  } while(0);

  if (fPcm) fclose(fPcm);                   
  if (fWav) fclose(fWav);                       
  if (wavHeader) delete wavHeader;           
  if (renresult != -1)                       
      ACE_OS::unlink(pcmpath);              // Erase pcm file
  if (written == pcmFileSize) 
      isError = FALSE;

  return isError? -1: 0;
}



unsigned int MmsDeviceVoice::terminationReason() 
{
  return this->isStreaming()? ATEC_TERMMSK(m_handle): ATDX_TERMMSK(m_handle);
}



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Diagnostics
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


void MmsDeviceVoice::showTerminationParameterList(MMS_DV_TPT_LIST& tptlist)
{
  // Diagnostic tool to walk a termination parameter list and interpret
  // and display each list node.  
   
  MMSLOG((LM_DEBUG,"%s %d entries in TPT:\n",devname,tptlist.size));     
  int entry = 0;
  DV_TPT* p = tptlist.head;

  while(p) 
  { 
    char buf[16]; ACE_OS::sprintf(buf,"%1d ",entry);
    std::string s = buf;

    switch(p->tp_type) 
    {
      case IO_CONT: s += "CONT "; break;
      case IO_EOT:  s += "EOT  "; break;
      case IO_LINK: s += "LINK "; break;
      default:      s += "???? "; 
    }

    switch(p->tp_termno)
    {
      case DX_DIGMASK:  s+= "DIGMASK  "; break;
      case DX_DIGTYPE:  s+= "DIGTYPE  "; break;
      case DX_IDDTIME:  s+= "IDDTIME  "; break;
      case DX_MAXDATA:  s+= "MAXDATA  "; break;
      case DX_MAXDTMF:  s+= "MAXDTMF  "; break;
      case DX_MAXNOSIL: s+= "MAXNOSIL "; break;
      case DX_MAXSIL:   s+= "MAXSIL   "; break;
      case DX_MAXTIME:  s+= "MAXTIME  "; break;
      case DX_TONE:     s+= "TONE     "; break;
      default:          s+= "???????? ";  
    }
     
    ACE_OS::sprintf(buf,"%08x ", p->tp_length); s += buf; 
    ACE_OS::sprintf(buf,"%08x ", p->tp_flags);  s += buf; 
    ACE_OS::sprintf(buf,"%08x ", p->tp_data);   s += buf; 

    MMSLOG((LM_DEBUG,"%s %s\n",devname,s.c_str()));     
    entry++;
    p = p->tp_nextp; 
  }
}



void MmsDeviceVoice::logIoXferBlockData()
{
  static char mask[] = "%s session %d i/o %s %s %dkhz %dbps\n"; 
  char* ffmt = "?", *dfmt = "?";
  int   khz=0;
   
  if  (m_xpb.wFileFormat == FILE_FORMAT_VOX)  ffmt = "vox"; else
  if  (m_xpb.wFileFormat == FILE_FORMAT_WAVE) ffmt = "wav"; 

  if  (m_xpb.wDataFormat == DATA_FORMAT_MULAW) dfmt = "ulaw"; else
  if  (m_xpb.wDataFormat == DATA_FORMAT_ALAW)  dfmt = "alaw"; else
  if  (m_xpb.wDataFormat == DATA_FORMAT_PCM)   dfmt = "pcm";  else
  if  (m_xpb.wDataFormat == DATA_FORMAT_DIALOGIC_ADPCM) dfmt = "adpcm"; 

  if  (m_xpb.nSamplesPerSec == DRT_6KHZ)  khz = 6; else
  if  (m_xpb.nSamplesPerSec == DRT_8KHZ)  khz = 8; else
  if  (m_xpb.nSamplesPerSec == DRT_11KHZ) khz = 11;  

  MMSLOG((LM_DEBUG,mask, devname,m_owner,ffmt,dfmt,khz,m_xpb.wBitsPerSample));     
}



void MmsDeviceVoice::logPlayRecordMode(unsigned int& mode)
{
  static char mask[] = "%s %s %dkhz agc %s tone %s\n"; 
  char* ffmt = "?", *dfmt = "?", *gain="on", *tone="off";
  int   khz=0;

  if  (mode & MD_PCM)   dfmt = "ulaw/pcm"; else
  if  (mode & RM_ALAW)  dfmt = "alaw";     else
  if  (mode & MD_ADPCM) dfmt = "adpcm";  

  if  (mode & RM_SR6)  khz = 6; else
  if  (mode & RM_SR8)  khz = 8; 

  if  (mode & MD_NOGAIN) gain = "off";
  if  (mode & RM_TONE)   tone = "on";

  MMSLOG((LM_DEBUG,mask, devname,dfmt,khz,gain,tone));     
}


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// MmsVolumeSpeedEncoder
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -


void MmsVolumeSpeedEncoder::copy(const whichSetting setting, MmsVolumeSpeedEncoder& other)
{
  if (setting & VSE_VOLVAL)
      this->vol = other.vol;
    
  if (setting & VSE_SPEEDVAL)
      this->spd = other.spd;

  if (setting & VSE_VADJTYPE)
  {
      this->vatype = other.vatype;
      this->vttype = other.vttype;
  }
    
  if (setting & VSE_SADJTYPE)
  {
      this->satype = other.satype;
      this->sttype = other.sttype;
  }
}



unsigned int MmsVolumeSpeedEncoder::pack()
{
  unsigned int packed = 0;
  packed |= ((this->vol + MMS_VSOFF) & 0x1f);
  packed |=(((this->spd + MMS_VSOFF) & 0x1f) << 5);
  packed |= ((this->sttype & 0x3) << 10);
  packed |= ((this->vttype & 0x3) << 12);
  packed |= ((this->satype & 0x3) << 14);
  packed |= ((this->vatype & 0x3) << 16);
  return packed;
}



void MmsVolumeSpeedEncoder::unpack(const unsigned int packed)
{
  this->vol = (packed &  0x1f) - MMS_VSOFF;
  this->spd =((packed & (0x1f << 5)) >> 5) - MMS_VSOFF;
  this->sttype = (packed & (0x3 << 10)) >> 10;
  this->vttype = (packed & (0x3 << 12)) >> 12;
  this->satype = (packed & (0x3 << 14)) >> 14;
  this->vatype = (packed & (0x3 << 16)) >> 16;
}


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// CSP
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

int MmsDeviceVoice::streamCsp(const int voiceBargein)
{
  int result = 0;
  int error = 0;

  // Set DXCH_EC_TAP_LENGTH to 128. This setting takes effect only for
  // SpringWare boards; for DM3 boards, the setting will be ignored (and 128 is
  // the default anyway for DM3 boards).  This should be the first CSP parameter
  // set, since it will reset all other parameters to their default settings.
  int parmval = 128;
  result = ec_setparm(m_handle, DXCH_EC_TAP_LENGTH, (void*)&parmval);
  if (result == -1) 
  {
    MMSLOG((LM_ERROR,"%s could not set csp param DXCH_EC_TAP_LENGTH\n", devname));
    error = 1;
  }

  // Set ECCH_XFERBUFFERSIZE to 512 (1/16th of a second, 8192 per second).  
  // The default is 16 KB which is way too large for an ASR application -- barge-in 
  // could not take place until the first buffer is delivered, meaning up to 2 seconds
  // latency. The smaller the buffer, the faster the barge-in response will be.   
  // But set it too small and you could overload the board and/or drivers.
  parmval = 512;
  result = ec_setparm(m_handle, ECCH_XFERBUFFERSIZE, (void*)&parmval);
  if (result == -1) 
  {
    MMSLOG((LM_ERROR,"%s could not set csp param ECCH_XFERBUFFERSIZE\n", devname));
    error = 1;
  }

  // Turn off non-linear processing(comfort noise). Dialogic documentation
  // indicates that ASR applications should turn this off.
  parmval = 1;    // 0 = ON, 1 = OFF
  result = ec_setparm(m_handle, ECCH_NLP, (void*)&parmval);
  if (result == -1) 
  {
    MMSLOG((LM_ERROR,"%s could not set csp param ECCH_NLP\n", devname));
    error = 1;
  }

  // Enable barge in for prompt termination
  if (voiceBargein)
  {
    // If DXCH_BARGEIN is enabled, speaker phone operation seems to stop playing
    // when noise is detected.  We may want to expose this parameter to designer.
    parmval = 1;
    result = ec_setparm(m_handle, DXCH_BARGEIN, (void*)&parmval);
    if (result == -1) 
    {
      MMSLOG((LM_ERROR,"%s could not set csp param DXCH_BARGEIN\n", devname));
      error = 1;
    }
    else 
    {
        MMSLOG((LM_ERROR,"%s voice bargein option is enabled\n", devname));

        parmval = 1; // use energy-only mode
        result = ec_setparm(m_handle, ECCH_SVAD , (void*)&parmval);
        if (result == -1) 
        {
            MMSLOG((LM_ERROR,"%s could not set csp param ECCH_SVAD\n", devname));
            error = 1;
        }

        // Set threshold value for barge-in during a prompt
        parmval = m_config->media.voiceBargeinThreshold; // default is -40
        result = ec_setparm(m_handle, DXCH_SPEECHPLAYTHRESH, (void*)&parmval);
        if (result == -1) 
        {
            MMSLOG((LM_ERROR,"%s could not set csp param DXCH_SPEECHPLAYTHRESH\n", devname));
            error = 1;
        }

        // Set DXCH_SPEECHPLAYTRIGG to adjust sensitivity
        parmval = 10;  // 10 is default
        result = ec_setparm(m_handle, DXCH_SPEECHPLAYTRIGG, (void*)&parmval);
        if (result == -1) 
        {
            MMSLOG((LM_ERROR,"%s could not set csp param DXCH_SPEECHPLAYTRIGG\n", devname));
            error = 1;
        }

        // Set DXCH_SPEECHPLAYWINDOW,  to adjust sensitivity
        parmval = 10;  // 10 is default
        result = ec_setparm(m_handle, DXCH_SPEECHPLAYWINDOW, (void*)&parmval);
        if (result == -1) 
        {
            MMSLOG((LM_ERROR,"%s could not set csp param DXCH_SPEECHPLAYWINDOW, \n", devname));
            error = 1;
        }
    }
  }

  // Enable both TDX_BARGEIN and TDX_PLAY
  parmval = 0;
  result = ec_setparm(m_handle, DXCH_BARGEINONLY, (void*)&parmval);
  if (result == -1) 
  {
    MMSLOG((LM_ERROR,"%s could not set csp param DXCH_BARGEINONLY\n", devname));
    error = 1;
  }

  result = error == 1? -1 : 0;

  if (result == 0)
      flags |= DEVICEFLAGS_IS_STREAMING;

  return result;
}



int MmsDeviceVoice::startStreaming()
{
    int result = 0;    
    #define REC_LENGTH 900 // Recording time in 0.1 second for CSP streaming

    // Setting up DV_TPT (terminating conditions) for record
    DV_TPT tpt;
    dx_clrtpt(&tpt, 1);
    tpt.tp_type   = IO_EOT;
    tpt.tp_termno = DX_MAXTIME;
    tpt.tp_length = REC_LENGTH;
    tpt.tp_flags  = TF_MAXTIME;

    // Record data format set to 8 bit PCM MuLaw, 8k samples/s
    DX_XPB xpb;
    xpb.wFileFormat = FILE_FORMAT_VOX;
    xpb.wDataFormat = DATA_FORMAT_MULAW;
    xpb.nSamplesPerSec = DRT_8KHZ;
    xpb.wBitsPerSample = 8;

    // Start recording audio
    result = ec_stream(m_handle, &tpt, &xpb, CspStreamCallback, EV_ASYNC|MD_NOGAIN);

    if  (result == -1) 
         MMSLOG((LM_ERROR,"%s could not start csp streaming\n", devname));
    else MMSLOG((LM_INFO, "%s start csp streaming\n", devname));

    return result;
}



static int CspStreamCallback(int ec_dev, char *buffer, UINT length)
{
  // Post CSP data chunk to session manager and write it to VR engine
  if (buffer != NULL && length > 0)
      Asr::instance()->m_task->postMessage(MMSM_CSPDATAREADY, ec_dev, buffer, length);  
  return length;
}



