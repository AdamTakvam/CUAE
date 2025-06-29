//
// mmsDeviceVoice.cpp
//
#include "StdAfx.h"
#include "mms.h"
#ifdef  MMS_WINPLATFORM
#pragma warning(disable:4786)
#endif

#include "mmsAsr.h"
#include "mmsDeviceVoice.h" 
#include "mmsParameterMap.h"
#include "mmsMsgTypes.h"
#include <eclib.h>
#include <minmax.h>

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION



int MmsDeviceVoice::playMultiple(MMS_DV_TPT_LIST* tptlist, 
  MMS_PLAYRECINFO* recinfo, unsigned int mode)
{
  // Plays any combination of data file, memory, or custom device.
  // If there are to be termination conditions such as timeouts or digits, 
  // they must have been previously set (this->setTerminationCondition)
  // on the supplied termination table list. The play input file(s) 
  // must be open via some method such as openfilePlay, on the supplied 
  // DX_IOTT entry/ies.

  // We clear the channel digit buffer prior to play, and also prior
  // to play tone. Therefore, if playing an announcement prior to playing
  // a tone, and if terminating a subsequent voice media operation on digits, 
  // user must be aware that digits entered prior to play of tone are discarded.

  m_deviceState = INUSE;   

  const unsigned int smode = mode & MmsMediaDevice::ASYNC? ASYNC: SYNC;

  if  (mode & MmsMediaDevice::NOCLEAR); 
  else clearDigitBuffer();
                    
  setIoXferBlock(recinfo);
  if  (recinfo->flags & MMS_PLAYRECINFO::LOGPLAYREC) logIoXferBlockData();
  if ((recinfo->flags & MMS_PLAYRECINFO::LOGTPT) && tptlist) 
       showTerminationParameterList(*tptlist);
                       
  this->elapsedTimeReset();                 // Mark start of play time  

  DV_TPT* tpt = tptlist? tptlist->head: NULL; 
  this->isChannelBusy = TRUE;
                                            // Start playing     
  int  result = dx_playiottdata(m_handle, m_iott, tpt, &m_xpb, smode);

  if  (result == -1)
  {
       MMSLOG((LM_ERROR,"%s %s dx_playiottdata %s\n",devname,em,ATDV_ERRMSGP(m_handle))); 
       this->isChannelBusy = FALSE;    
       return -1;                     
  } 

  if  (smode & ASYNC) 
       m_mediaState = MEDIAWAITING;             
  else this->isChannelBusy = FALSE;

  return 0;
}



int MmsDeviceVoice::record
( MMS_DV_TPT_LIST* tptlist, MMS_PLAYRECINFO* recinfo, char* path, unsigned int mode)
{
  // Records voice data a single disk file.
  // If there are to be termination conditions such as timeouts or digits, 
  // they must have been previously set (this->setTerminationCondition)
  // on the supplied termination table list. The record output file(s) 
  // must be open via some method such as openfileRecord, on the supplied 
  // DX_IOTT entry/ies.
  int result = 0;

  unsigned int smode = mode & MmsMediaDevice::ASYNC? ASYNC: SYNC;
  if  (mode & MmsMediaDevice::NOCLEAR); 
  else clearDigitBuffer();

  m_deviceState = INUSE;
  if  ((recinfo->flags & MMS_PLAYRECINFO::LOGTPT) && tptlist) 
        showTerminationParameterList(*tptlist);

  this->elapsedTimeReset();                 // Mark start of record time 
  this->isChannelBusy = TRUE;                            

  DV_TPT* tpt = tptlist? tptlist->head: NULL;

  this->setPlayRecordMode(recinfo, smode);

  do 
  {   if (-1 == (result = dx_rec(m_handle, m_iott, tpt, smode)))
      {
          MMSLOG((LM_ERROR,"%s %s dx_rec %s\n",devname, em, ATDV_ERRMSGP(m_handle)));  
          break;                     
      } 

      if (recinfo->filetype == MmsDeviceVoice::WAV)
      {                                     // Convert dx_rec raw pcm to wav file
          if (-1 == (result = this->pcmToWav(path, recinfo)))      
          {
              MMSLOG((LM_ERROR,"%s could not write WAV headers to recorded PCM\n", devname));  
              break;   
          } 
      }
  } while(0);

  if  (result == -1)
       this->isChannelBusy = FALSE;
  else
  if  (smode & ASYNC) 
       m_mediaState = MEDIAWAITING; 
  else this->isChannelBusy = FALSE;
                 
  return result;
}



int MmsDeviceVoice::recordMultiple(MMS_DV_TPT_LIST* tptlist, 
  MMS_PLAYRECINFO* recinfo, unsigned int mode)
{
  // Records voice data to some combination of data file or memory..
  // If there are to be termination conditions such as timeouts or digits, 
  // they must have been previously set (this->setTerminationCondition)
  // on the supplied termination table list. The record output file(s) 
  // must be open via some method such as openfileRecord, on the supplied 
  // DX_IOTT entry/ies.
  int result = 0;

  unsigned int smode = mode & MmsMediaDevice::ASYNC? ASYNC: SYNC;
  if  (mode & MmsMediaDevice::NOCLEAR); 
  else clearDigitBuffer();

  m_deviceState = INUSE;
  if  ((recinfo->flags & MMS_PLAYRECINFO::LOGTPT) && tptlist) 
        showTerminationParameterList(*tptlist);

  this->elapsedTimeReset();                 // Mark start of record time 
  this->isChannelBusy = TRUE;                            

  DV_TPT* tpt = tptlist? tptlist->head: NULL;

  const int isUsingRecIottData = recinfo->filetype == MmsDeviceVoice::WAV;

  if  (isUsingRecIottData)
  {
       this->setIoXferBlock(recinfo);

       if  (recinfo->flags & MMS_PLAYRECINFO::LOGPLAYREC) logIoXferBlockData();

       if  (recinfo->tone) smode |= RM_TONE; 
            // vox AGC is not currently per command but per server 
       if  (m_config->media.agcDisableVox) mode |= MD_NOGAIN;
                                           
       result = dx_reciottdata(m_handle, m_iott, tpt, &m_xpb, smode);
  }
  else
  {    this->setPlayRecordMode(recinfo, smode);

       result = dx_rec(m_handle, m_iott, tpt, smode);
  }

  
  if  (result == -1)
  {
       MMSLOG((LM_ERROR,"%s %s dx_rec %s\n",devname,em,ATDV_ERRMSGP(m_handle)));  
       this->isChannelBusy = FALSE;   
       return -1;                     
  } 

  if  (smode & ASYNC) 
       m_mediaState = MEDIAWAITING; 
  else this->isChannelBusy = FALSE;
                 
  return 0;
}



int MmsDeviceVoice::recordTransaction(MmsMediaDevice* partyIP1, MmsMediaDevice* partyIP2,
  MMS_DV_TPT_LIST* tptlist, MMS_PLAYRECINFO* recinfo, unsigned int mode)
{
  // Records voice from two sources to any combination of file or memory. 
  // If there are to be termination conditions such as timeouts or digits, 
  // they must have been previously set (this->setTerminationCondition)
  // on the supplied termination table list. The record output file(s) 
  // must be open via some method such as openfileRecord, on the supplied 
  // DX_IOTT entry/ies.
  // If mode includes RM_TONE, each source timeslot must be listening to
  // the transmit timeslot of the record channel; the alert tone can only
  // be transmitted on the record channel's timeslot.

  m_deviceState = INUSE;

  const unsigned int smode = mode & MmsMediaDevice::ASYNC? ASYNC: SYNC;
  if  (mode & MmsMediaDevice::NOCLEAR); 
  else clearDigitBuffer();

  setIoXferBlock(recinfo);
  if  (recinfo->flags & MMS_PLAYRECINFO::LOGTPT) logIoXferBlockData();
  if  (recinfo->tone) mode |= RM_TONE;
  if  ((recinfo->flags & MMS_PLAYRECINFO::LOGTPT) && tptlist) 
        showTerminationParameterList(*tptlist);

  this->elapsedTimeReset();                 // Mark start of record time      

  DV_TPT* tpt = tptlist? tptlist->head: NULL;

  mmsTimeslotHandle parties[2];             // Extract timeslots from 
  parties[0] = partyIP1->timeslotNumber();  // IP resource of each party
  parties[1] = partyIP2->timeslotNumber();

  SC_TSINFO slotinfo;                       // Construct 2-party timeslot info
  slotinfo.sc_numts = 2;
  slotinfo.sc_tsarrayp = parties;
  this->isChannelBusy = TRUE;
                                            // Start recording     
  int  result = dx_mreciottdata(m_handle, m_iott, tpt, &m_xpb, smode, &slotinfo);
  
  if  (result == -1)
  {
       MMSLOG((LM_ERROR,"%s %s dx_mreciottdata\n",devname,em));    
       this->isChannelBusy = FALSE; 
       return -1;                     
  } 

  if  (smode & ASYNC) 
       m_mediaState = MEDIAWAITING;                  
  else this->isChannelBusy = FALSE;

  return 0;
}



int MmsDeviceVoice:: playtone(int f1, int f2, int dB1, int dB2, int dur, 
  MMS_DV_TPT_LIST* tptlist, int mode) 
{
  // Play a tone defined by frequencies f1,f2, amplitudes db1,db2, and duration
  // Frequency range 200 to 3000 hz. To play a single tone, specify f2 zero.
  // Frequency out of range results in f1=600, f2=0.
  // If amplitude out of range 0 to -40dB. default of -10dB is used.
  // Duration is specified in 10ms units - zero results in 500ms duration 
  // Duration -1 is infinite, plays until term condition met or stopped

  // We clear the channel digit buffer prior to play, and also prior
  // to play tone. Therefore, if playing an announcement prior to playing
  // a tone, and if terminating a subsequent voice media operation on digits, 
  // user must be aware that digits entered prior to play of tone are discarded.

  m_deviceState = INUSE;

  const unsigned int smode = mode & MmsMediaDevice::ASYNC? ASYNC: SYNC;
  if  (mode & MmsMediaDevice::NOCLEAR); 
  else clearDigitBuffer();

  if  (tptlist && (m_config->diagnostics.flags & MMS_DIAG_LOG_TERMCONDITIONS))
       showTerminationParameterList(*tptlist);

  this->elapsedTimeReset();                 // Mark start of play time    

  DV_TPT* tpt = tptlist? tptlist->head: NULL;
                                            // Convert duration from ms to 10ms units
  int  duration = dur < 1? (-1): (dur / 10) + ((dur % 10) >= 5);         
                                            // Require a terminating condition                                            
  if  (duration == (-1) && tpt == NULL) return -1;
  int  freq1 = (f1  >= 200 && f1  <= 3000)?  f1: m_config->media.defaultToneFrequency;
  int  freq2 = (f2  >= 200 && f2  <= 3000)?  f2: 0;
  int  ampl1 = (dB1 <= 0   && dB1 >= (-40))? dB1:(-10);
  int  ampl2 = (dB2 <= 0   && dB2 >= (-40))? dB2:(-10);
  
  TN_GEN tngen;
  dx_bldtngen(&tngen, freq1, freq2, ampl1, ampl2, duration);
  this->isChannelBusy = TRUE;

  if  (-1 == dx_playtone(m_handle, &tngen, tpt, smode))
  {
       MMSLOG((LM_ERROR,"%s %s dx_playtone\n",devname,em));
       this->isChannelBusy = FALSE;
       return -1;                     
  } 

  if  (smode & ASYNC) 
       m_mediaState = MEDIAWAITING;                  
  else this->isChannelBusy = FALSE;

  return 0;
}



int MmsDeviceVoice::receiveDigits(MMS_DV_TPT_LIST* tptlist, int mode, int* outcond)
{
  // Begin receiving DTMF into local digit buffer. If there are to be 
  // termination conditions such as timeouts or number of digits, 
  // set them first via this->setTerminationCondition() on the supplied 
  // termination table list. 

  // When operation is async, return value is zero or -1, and outcond is ignored.
  // When operation is synchronous, return value is -1 if error, otherwise the
  // number of digits collected. If outcond* was supplied, caller's outcond
  // is populated with the terminating condition.

  m_deviceState = INUSE;

  const unsigned int smode = mode & MmsMediaDevice::ASYNC? ASYNC: SYNC;

  if  (tptlist && (m_config->diagnostics.flags & MMS_DIAG_LOG_TERMCONDITIONS))
       showTerminationParameterList(*tptlist);

  DV_TPT* tpt = tptlist? tptlist->head: NULL;
  this->elapsedTimeReset();                 
  this->isChannelBusy = TRUE;

  const int result = dx_getdig(m_handle, tpt, &m_digitBuf, smode);
				 
  if  (result < 0)
  {
       MMSLOG((LM_ERROR,"%s %s dx_getdig %s\n",devname,em,ATDV_ERRMSGP(m_handle))); 
       this->isChannelBusy = FALSE;    
       return -1;                     
  } 
				
  if  (smode & ASYNC)                       // Asynchronous operation
       m_mediaState = MEDIAWAITING;   
  else                                      // Synchronous operation
  {    this->isChannelBusy = FALSE;         // Return value includes null term
       const int numDigitsReceived = max(result-1,0);
       const int terminatingCondition = this->terminationReason();
       if (outcond) *outcond = terminatingCondition; 
       return numDigitsReceived;
  }
   
  return 0;                   
}



int MmsDeviceVoice::stopMediaOperation(const int mode)
{
  // Stops any onging async I/O operation such as play, record, receive digits

  if  (m_mediaState < MEDIAWAITING) return -1;
  static const char *dx = "dx_stopch", *ec = "ec_stopch";

  // Note that EV_SYNC is zero, EV_ASYNC 0x8000
  const unsigned int smode = mode & MmsMediaDevice::ASYNC? ASYNC: SYNC;

  int result = 0;

  if (this->isStreaming())                  // Voice rec 
  {
       result = ec_stopch(m_handle, FULLDUPLEX, EV_SYNC);
       flags &= ~DEVICEFLAGS_IS_STREAMING;
  }
  else result = dx_stopch(m_handle, smode); // Everything else
  
  if  (result == -1)
  {
       MMSLOG((LM_ERROR,"DEVV %s %s\n", em, isStreaming()? ec: dx));     
       return -1;                     
  } 

  if  (smode & ASYNC);  
  else 
  {   iottReset();
      m_deviceState = INUSE;
      m_mediaState  = IDLE;
      this->isChannelBusy = FALSE;
  }

  return 0;
}



int MmsDeviceVoice::assignVolumeAdjustmentDigit(char digit, int adjval)
{
  // Assigns a digit to adjust volume up or down a multiple of 2dB
  // Digits: '0' thru '9', '*', '#'
  // Each increment of adjval is +/- 2dB, 1 <= abs(adjval) <= 4
  // Specify adjval of SV_NORMAL (0xff) to assign a reset digit
  // (to begin playback volume at origin, specify ('\0', SV_NORMAL);)
  // To clear adjustment digits, this->clearVolumeSpeedAdjustments();
  m_deviceState = INUSE;

  if  (-1 == dx_addvoldig(m_handle, digit, adjval))
  {
       MMSLOG((LM_ERROR,"%s %s dx_addvoldig\n",devname,em));     
       return -1;                     
  }

  return 0;
}



int MmsDeviceVoice::assignSpeedAdjustmentDigit(char digit, int adjval)
{
  // Assigns a digit to adjust speed up or down a multiple of 10%
  // Digits: '0' thru '9', '*', '#'
  // Each increment of adjval is +/- 10%, 1 <= abs(adjval) <= 4
  // Specify adjval of SV_NORMAL (0xff) to assign a reset digit
  // To clear adjustment digits, this->clearVolumeSpeedAdjustments();
  m_deviceState = INUSE;

  if  (-1 == dx_addspddig(m_handle, digit, adjval))
  {
       MMSLOG((LM_ERROR,"%s %s dx_addspddig\n",devname,em));     
       return -1;                     
  }

  return 0;
}



int MmsDeviceVoice::adjustVolume(int action, int adjval, int toggletype) 
{
  return this->adjustVolumeOrSpeed(SV_VOLUMETBL, action, adjval, toggletype);
}



int MmsDeviceVoice::adjustSpeed(int action, int adjval, int toggletype)
{
  return this->adjustVolumeOrSpeed(SV_SPEEDTBL, action, adjval, toggletype);
}


 
int MmsDeviceVoice::adjustVolumeOrSpeed(int which, int action, int adjval, int toggletype)
{
  // Each virtual volume or speed slider has 21 ticks, -10 to +10
  // Each tick on a virtual volume slider is 2dB.
  // Each tick on a virtual speed slider is delta 10% of normal
  // action: SV_ABSPOS:    Absolute slider tick (-10 <= adjval <= +10)
  // action: SV_RELCURPOS: Adjust slider up or down by (adjval) ticks
  // action: SV_TOGGLE:    
  //         adjval: SV_TOGORIGIN:  toggle between origin & last adjustment
  //         adjval: SV_CURORIGIN:  reset to origin (normal speed or volume)
  //         adjval: SV_CURLASTMOD: set to last modified level
  //         adjval: SV_RESETORIG:  set both current and last mod to origin
 
  const static char *xvol = "volume", *xspd = "speed"; 
  m_deviceState = INUSE;       

  int hmpaction = SV_ABSPOS;
  int hmpadjamt = 0;

  switch(action)   
  {
    case MMS_ADJTYPE_ABSOLUTE:
    case MMS_ADJTYPE_NONE:
         hmpaction = SV_ABSPOS;
         hmpadjamt = adjval;
         break;

    case MMS_ADJTYPE_RELATIVE:
         hmpaction = SV_RELCURPOS;
         hmpadjamt = adjval;
         break;

    case MMS_ADJTYPE_TOGGLE:
         hmpaction = SV_TOGGLE;
         hmpadjamt = toggletype;
         break;

    default: return -1;
  }

  if (m_config->diagnostics.flags & MMS_DIAG_LOG_VOLSPEED)   
      MMSLOG((LM_DEBUG,"%s adjusting %s adjtype %d amt %d\n", devname,
              which == SV_VOLUMETBL? xvol:xspd, hmpaction, hmpadjamt));

  // Note that mms and hmp values for the adjval parameter are the same

  if  (-1 == dx_adjsv(m_handle, which, hmpaction, hmpadjamt))
  {
       MMSLOG((LM_ERROR,"%s %s dx_adjsv\n",devname,em));     
       return -1;                     
  } 
  else 
  if  (m_config->diagnostics.flags & MMS_DIAG_LOG_VOLSPEED)   
  {
       int curSpeed, curVol;
       if (dx_getcursv(m_handle, &curVol, &curSpeed) != -1) 
           MMSLOG((LM_DEBUG,"%s current volume is %d, speed is %d\n",devname,curVol, curSpeed));  

       DX_SVMT svmt;
       if (dx_getsvmt(m_handle, SV_VOLUMETBL, &svmt ) != -1 )
       {  
            int index;
            MMSLOG((LM_DEBUG,"%s ---------- Volume Modification Table ----------\n",devname));  
            for ( index = 0; index < 10; index++ )
                MMSLOG((LM_DEBUG,"%s decrease[%d] = %d\n", devname, index, svmt.decrease[index]));  

            MMSLOG((LM_DEBUG,"%s origin is %d\n", devname, svmt.origin));  

            for ( index = 0; index < 10; index++ )
                MMSLOG((LM_DEBUG,"%s increase[%d] = %d\n", devname, index, svmt.increase[index]));  
       }  

       if (dx_getsvmt(m_handle, SV_SPEEDTBL, &svmt ) != -1 )
       {  
            int index;
            MMSLOG((LM_DEBUG,"%s ---------- Speed Modification Table ----------\n",devname));  
            for ( index = 0; index < 10; index++ )
                MMSLOG((LM_DEBUG,"%s decrease[%d] = %d\n", devname, index, svmt.decrease[index]));  

            MMSLOG((LM_DEBUG,"%s origin is %d\n", devname, svmt.origin));  

            for ( index = 0; index < 10; index++ )
                MMSLOG((LM_DEBUG,"%s increase[%d] = %d\n", devname, index, svmt.increase[index]));  
        }  
  }

  return 0;
}
 


int MmsDeviceVoice::clearVolumeSpeedAdjustments()
{
  // Resets any adjustments to normal, and clears DTMF digit associations
  m_deviceState = INUSE;
  
  if  (-1 == dx_adjsv(m_handle, SV_VOLUMETBL, SV_ABSPOS, 0))
  {                                         // Set volume back to 0
      MMSLOG((LM_ERROR,"%s %s dx_adjsv (vol) \n",devname,em));    
      return -1;
  }
  
  if  (-1 == dx_adjsv(m_handle, SV_SPEEDTBL, SV_ABSPOS, 0))
  {                                         // Set speed back to 0
      MMSLOG((LM_ERROR,"%s %s dx_adjsv (speed)\n",devname,em));    
      return -1;
  }

  return 0;
}



int MmsDeviceVoice::setIoXferBlock(MMS_PLAYRECINFO* recinfo)
{
  // Specifies file format, data format, sampling rate, and resolution
  // for the extended play and record functions. The XPB must remain in
  // scope for the duration of playback.   
                                           
  switch(recinfo->filetype)
  { case VOX:   m_xpb.wFileFormat = FILE_FORMAT_VOX;   break;
    case WAV:   m_xpb.wFileFormat = FILE_FORMAT_WAVE;  break;
    default:    m_xpb.wFileFormat = FILE_FORMAT_VOX;   
  }

  switch(recinfo->mode)
  { case MULAW: m_xpb.wDataFormat = DATA_FORMAT_MULAW; break; 
    case ALAW:  m_xpb.wDataFormat = DATA_FORMAT_ALAW;  break;
    case PCM:   m_xpb.wDataFormat = DATA_FORMAT_PCM;   break;
    case ADPCM: m_xpb.wDataFormat = DATA_FORMAT_DIALOGIC_ADPCM; 
         //m_xpb.wBitsPerSample = 4;
         break;
    default:    m_xpb.wDataFormat = DATA_FORMAT_MULAW;
  }

  switch(recinfo->rate)
  { case RATE_6KHZ:  m_xpb.nSamplesPerSec = DRT_6KHZ;  break; 
    case RATE_8KHZ:  m_xpb.nSamplesPerSec = DRT_8KHZ;  break; 
    case RATE_11KHZ: m_xpb.nSamplesPerSec = DRT_11KHZ; break; 
    default:         m_xpb.nSamplesPerSec = DRT_8KHZ;   
  }

  // Overwrite sample size for WAV + PCM
  // if (m_xpb.wFileFormat == FILE_FORMAT_WAVE && m_xpb.wDataFormat == DATA_FORMAT_PCM)
  //     m_xpb.wBitsPerSample = m_config->media.pcmSampleSize;

  switch(recinfo->samplesize)
  { case SIZE_4BIT:  m_xpb.wBitsPerSample = 4;  break; 
    case SIZE_8BIT:  m_xpb.wBitsPerSample = 8;  break; 
    case SIZE_16BIT: m_xpb.wBitsPerSample = 16; break; 
    default:         m_xpb.wBitsPerSample = 8;   
  }

  return 0;
}



int MmsDeviceVoice::setPlayRecordMode(MMS_PLAYRECINFO* recinfo, unsigned int& mode)
{
  // Specifies data format, sampling rate, and resolution for the standard
  // play and record functions. Options are masked into the play/rec mode 
  // doubleword, which also includes sync/async bit. 

  // Note that this method is not now executed on play, since we use dx_playiottdata 
  // for all plays, which uses the xpb rather than a mode parameter (setIoXferBlock)

  const int isVox = recinfo->filetype == MmsDeviceVoice::VOX;

  mode |= MD_PCM; // MD_PCM is "ulaw PCM" according to dialogic header. 8 bps.

  if  (recinfo->rate == RATE_6KHZ && !isVox)
       mode |= RM_SR6;  // 48kbps
  else mode |= RM_SR8;  // 64kbps


  #if(0)  // old code
  switch(recinfo->mode)
  {  
    case MULAW: mode |= MD_PCM;   break;  // uLaw   
    case ALAW:  mode |= RM_ALAW;  break;
    case PCM:   mode |= MD_PCM;   break;     
    case ADPCM: mode |= MD_ADPCM; break;
    default:    mode |= MD_PCM;              
  }

  switch(recinfo->rate)
  { case RATE_6KHZ:  mode |= RM_SR6; break; 
    case RATE_8KHZ:  mode |= RM_SR8; break; 
    default:         mode |= RM_SR8;   
  }
  #endif

  if  (recinfo->nogain) mode |= MD_NOGAIN;  // AGC default is on
  if  (recinfo->tone)   mode |= RM_TONE;    // Tone prior record default off 

  if  (recinfo->flags & MMS_PLAYRECINFO::LOGPLAYREC) logPlayRecordMode(mode);

  return 0;
}



int MmsDeviceVoice::setTerminationCondition
( MMS_DV_TPT_LIST& tptlist, int condition, int val, int flagz, int xtradata)
{
  // Creates a new entry in the MMS_DV_TPT_LIST termination table list, 
  // using the parameters supplied. 

  DV_TPT* tptEntry = tptlist.addnode(); 
  tptEntry->tp_termno  = (unsigned short)condition;
  unsigned short value = (unsigned short)val;
  unsigned short flags = (unsigned short)flagz;

  switch(condition)  
  {
    case DX_MAXDTMF:                        // Max # of digits received
         tptEntry->tp_length = value;
         tptEntry->tp_flags  = flagz == 0? TF_MAXDTMF: flags;
         break;
    case DX_DIGTYPE:                        // Terminate on specified digit
         tptEntry->tp_length = value;       // Length IS the ascii digit
         tptEntry->tp_flags  = flagz == 0? TF_DIGTYPE: flags;
         break;         
    case DX_DIGMASK:                        // Terminate on one or more digits
         tptEntry->tp_length = value;       // Digit mask (see addToDigitMask)
         tptEntry->tp_flags  = flagz == 0? TF_MAXNOSIL: flags;
         break;
    case DX_MAXTIME:                        // Max func time (10 or 100ms units)
         tptEntry->tp_length = value;       //(100ms unless flags & TF_10MS)
         tptEntry->tp_flags  = flagz == 0? TF_MAXTIME: flags;
         break;
    case DX_MAXSIL:                         // Max length of silence (10/100ms)
         tptEntry->tp_length = value;
         tptEntry->tp_flags  = flagz == 0? TF_MAXSIL: flags;
                                            // Initial length of term silence 
         tptEntry->tp_data   = (unsigned short)xtradata;  
         if (xtradata) tptEntry->tp_flags |= TF_SETINIT;
         break;
    case DX_MAXNOSIL:                       // Max length of non-silence 
         tptEntry->tp_length = value;
         tptEntry->tp_flags  = flagz == 0? TF_MAXNOSIL: flags;
         break;
    case DX_IDDTIME:                       // Inter-digit delay 
         tptEntry->tp_length = value;
         tptEntry->tp_flags  = flagz == 0? TF_IDDTIME | TF_FIRST: flags;
         break;

    default: return -1;
  }   
  
  return 0;
}
                                            


unsigned short MmsDeviceVoice::addToDigitMask(unsigned char digit, unsigned short mask)
{
  // I/O termination condition can be specified as one or more specific digits, 
  // which are specified as a bit string. To create the mask invoke this 
  // method once for each digit, passing the digit and the current mask.
  // The new bit mask is returned. Example: mask = addToDigitMask('#', mask);

  static unsigned short masks[]={DM_0,DM_1,DM_2,DM_3,DM_4,DM_5,DM_6,DM_7,DM_8,DM_9};
  unsigned short maskbit = 0;

  if  (digit >= '0' && digit <= '9')
       maskbit = masks[digit  - '0'];
   
  else switch(digit)
  {    case '*': maskbit = DM_S; break;
       case '#': maskbit = DM_P; break;
       case 'a': maskbit = DM_A; break;
       case 'b': maskbit = DM_B; break;
       case 'c': maskbit = DM_C; break;
       case 'd': maskbit = DM_D; break;
  }

  return mask | maskbit;
}



int MmsDeviceVoice::openfilePlay(char* path, int offset, int length)
{
  int  handle = dx_fileopen(path, O_BINARY|O_RDONLY); // ,0666); unix
  if  (handle == -1)
  {
      MMSLOG((LM_ERROR,"%s %s dx_fileopen %s\n",devname,em,path));     
      return -1;                     
  } 

  DX_IOTT* iottentry = this->iottNext();
  iottentry->io_fhandle = handle;
  iottentry->io_type    = IO_DEV |  IO_EOT;  
  if (offset) iottentry->io_type |= IO_USEOFFSET;  
  iottentry->io_offset  = offset;           
  iottentry->io_length  = length? length:-1;// -1 = play to eof or a term cond
  return 0;
}


                                             
int MmsDeviceVoice::openfileRecord(char* path)
{                                           // ,0666); linux
  int handle = dx_fileopen(path, O_CREAT|O_BINARY|O_RDWR, _S_IREAD|_S_IWRITE); 

  if (handle == -1)
  {
      MMSLOG((LM_ERROR,"%s %s dx_fileopen %s\n",devname,em,path));     
      return -1;                     
  } 

  DX_IOTT* iottentry = this->iottNext();
  iottentry->io_fhandle = handle;
  iottentry->io_type    = IO_DEV | IO_EOT;  // | IO_USEOFFSET  
  iottentry->io_offset  = 0;
  iottentry->io_length  = -1;               // -1 = record until a term cond
  return 0;
}


                                            // Ctor
MmsDeviceVoice::MmsDeviceVoice(int ordinal, MmsConfig* config): 
  MmsMediaDevice(DEVICETYPE::VOICE, ordinal, config)
{ 
  ACE_OS::strcpy(devname,"DEVV"); 
  this->config = config;
  m_isEchoCancellationEnabled = FALSE;      // Get from config
  m_iottSize = m_iottCount = volspeed = 0;  
  m_iott = NULL;	 
}



mmsDeviceHandle MmsDeviceVoice::open(OPENINFO& openinfo, unsigned short mode) 
{  
  static char *maskError = "%s %s dx_open %s\n";
  static char *maskOpen  = "%s opened %s as %d\n";
  static char *maskCsp   = "%s opened %s as %d (CSP)\n";
  const int isLoggingOpenOk = config->serverLogger.globalMessageLevel >= LM_INFO;

  HMPCARDID(deviceID,"dxxx", openinfo.key.board, openinfo.key.card);

  if (-1 == (m_handle = dx_open(deviceID, 0))) 
  {
      ACE_OS::printf  (maskError,devname,em,deviceID);   
      MMSLOG((LM_ERROR,maskError,devname,em,deviceID));  
      return -1;                     
  }

  MmsAs::openVox++;  
  char* mask = maskOpen;
  int   parmval = 0;  
                                             
  if (ec_getparm(m_handle, DXCH_BARGEIN, (void *)&parmval) != -1) 
  {   
      this->flags |= DEVICEFLAGS_IS_CSP;    // This device is CSP-capable
      MmsAs::openCSP++;
      mask = maskCsp;
  }

  if (isLoggingOpenOk) ACE_OS::printf(mask,devname,deviceID,m_handle); 
  this->init();
  m_deviceState = AVAILABLE;
  m_key = openinfo.key;
  return m_handle;
}



int MmsDeviceVoice::timeslot(SC_TSINFO* slotinfo) 
{
  slotinfo->sc_numts = 1;
  slotinfo->sc_tsarrayp = &m_timeslotNumber;

  int result = isEchoCancellationEnabled()?
      dx_getxmitslotecr(m_handle, slotinfo):         
      dx_getxmitslot   (m_handle, slotinfo);

  if (result == -1)
      MMSLOG((LM_ERROR, isEchoCancellationEnabled()?
      "%s dt_getxmitslot %d\n": "%s %s dx_getxmitslotecr %d\n",devname,em,m_handle));     
    
  return result;
} 



int MmsDeviceVoice::listen(SC_TSINFO* slotinfo)
{
  int result = isEchoCancellationEnabled()?
      dx_listenecrex(m_handle, slotinfo, &m_ecrct):
      dx_listen     (m_handle, slotinfo);

  if (result == -1)
      MMSLOG((LM_ERROR, isEchoCancellationEnabled()?
      "%s %s dx_listenecrex %d\n": "%s %s dx_listen %d\n",devname,em,m_handle));                                    // 0517
  else 
  {    m_mediaState = MEDIAWAITING;
       islistened = TRUE;

       if  (m_config->diagnostics.flags & MMS_DIAG_LOG_LISTENS)
            this->logListening(slotinfo);
  }

  return result;
}



int MmsDeviceVoice::unlisten()
{
  int result = isEchoCancellationEnabled()?
      dx_unlistenecr(m_handle):
      dx_unlisten   (m_handle);

  if (result == -1)
      MMSLOG((LM_ERROR, isEchoCancellationEnabled()?
      "%s dx_unlistenecr %d\n": "%s %s dx_unlisten %d\n", devname,em,m_handle)); 
  else
  if ((m_config->diagnostics.flags & MMS_DIAG_LOG_LISTENS)
    && m_mediaState == MEDIAWAITING)
       this->logUnlistening();

  islistened = FALSE;  
  m_mediaState = MEDIAIDLE;
  return result;
}



int MmsDeviceVoice::isListening()
{
  return islistened;
}

  

void MmsDeviceVoice::close() 
{  
  if  (m_handle > 0)                  
       dx_close(m_handle);

  m_mediaState  = MEDIAIDLE;
  m_deviceState = CLOSED;
  m_handle = 0; 
}



MmsDeviceVoice::~MmsDeviceVoice()           // Dtor
{  
  this->close();
  if  (m_iott) delete[] m_iott;
}



void MmsDeviceVoice::clearDigitBuffer()   
{  
  ACE_Guard<ACE_Thread_Mutex> x(this->dataLock); 
   
  memset(m_digitBuf.dg_value, 0, DG_MAXDIGS + 1);

  if  (-1 == dx_clrdigbuf(m_handle))        // Clear firmware buffer
       MMSLOG((LM_ERROR,"%s %s dx_clrdigbuf: %s\n",
               devname, em, ATDV_ERRMSGP(m_handle))); 
  else
  if  (m_config->diagnostics.flags & MMS_DIAG_LOG_DIGBUF)
       MMSLOG((LM_DEBUG,"%s vox%d clear digbuf\n", devname, m_handle)); 
}



void MmsDeviceVoice::logDigitBuffer()   
{  
  if (!(m_config->diagnostics.flags & MMS_DIAG_LOG_DIGBUF)) return;

  #if(0)                                    // involves extra processor overhead 
                                             
  int n = checkDigitBuffer();

  if (n >= 0)
      MMSLOG((LM_DEBUG,"%s vox%d digits buffered %d\n", devname, m_handle, n)); 

  #endif
}



int MmsDeviceVoice::checkDigitBuffer()   
{  
  int  uncollectedDigitsCount = 0;          // ATDX_BUFDIGS is not implemented
                                            // as of HMP 1.1 sr1
  if  (-1 == (uncollectedDigitsCount = ATDX_BUFDIGS(m_handle)))
       MMSLOG((LM_ERROR,"%s %s ATDX_BUFDIGS %s\n", devname, em, 
               ATDV_ERRMSGP(m_handle))); 
  
  return uncollectedDigitsCount;
}



int MmsDeviceVoice::checkUserDigitBuffer()   
{  
  // Ensure that digit buffer is a terminated string, returning -1 or length
  char *digbuf, *p; digbuf = p = m_digitBuf.dg_value;
  int zeros = 0; 

  for(int i=0; i < (DG_MAXDIGS +1); i++, p++)
      if (*p == 0) zeros++;

  return zeros == 0? -1: ACE_OS::strlen(digbuf); 
}



int MmsDeviceVoice::registerCallStatusSilence()
{
  return dx_setevtmsk(m_handle, DM_SILON|DM_SILOF);
}



int MmsDeviceVoice::clearCallStatus()
{
  return dx_setevtmsk(m_handle, 0);
}



int MmsDeviceVoice::enableEchoCancellation(int onOrOff)
{ 
  int wasEchoCancellationEnabled = m_isEchoCancellationEnabled;
  m_isEchoCancellationEnabled = onOrOff? 1: 0;
  return wasEchoCancellationEnabled;
}



int MmsDeviceVoice::enableNlp(int onOrOff)
{ 
  return (int)m_ecrct.enableNlp((unsigned char)onOrOff);
} 



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Support methods
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

void MmsDeviceVoice::iottReset()            // Reset IOTT to beginning -- this
{                                           // must be done at start of each  
  if  (m_iottCount > 0)                     // play or record operation.
       this->iottClose();

  m_iottCount  = 0;
  memset(m_iott, 0, sizeof(DX_IOTT) * m_iottSize);
}



DX_IOTT* MmsDeviceVoice::iottNext()         // Return next available IOTT entry
{                                           
  if  (m_iottCount == m_iottSize)           // If IOTT at capacity ...
       this->iottRealloc();                 // ... increase size

  DX_IOTT* iottEntry = NULL;

  if  (m_iottCount > 0)                     // If not initial entry ...
  {                                         // make current entry not terminal  
       iottEntry = &m_iott[m_iottCount-1];                                          
       iottEntry->io_type &= ~IO_EOT;            
       iottEntry->io_type |=  IO_CONT;
  }

  iottEntry = &m_iott[m_iottCount++];       // Point at to new entry
  iottEntry->io_type = IO_EOT;              // Default new entry to terminal
  return iottEntry;                         // Return new entry
}



int MmsDeviceVoice::iottClose()              
{  
  // Cleans up all files opened for a play or record operation. This must
  // be done after each such operation. Note that may need to deallocate 
  // memory for in-memory files (io_type & IO_MEM) files here, if such files
  // are one-shots and not cached.
 
  int  result = 0;
  if  (m_iottCount == 0) return result;

  DX_IOTT* iottEntry = m_iott;

  while(1)                                  // Walk the IOTT list
  {
    if  (iottEntry->io_fhandle && iottEntry->io_type)
         if  (-1 == dx_fileclose(iottEntry->io_fhandle))
         {
              MMSLOG((LM_ERROR,"%s %s dx_fileclose\n",devname,em));     
              result = -1; 
         } 

    if  ((iottEntry->io_type & IO_EOT) || !(iottEntry->io_type & IO_CONT))                   
          break;
  
    iottEntry++;     
  } 

  return result;
}
  
                                          

void MmsDeviceVoice::iottRealloc()
{   
  // Increases size of iott array by IOTT_SIZE_DELTA entries                                        
  const int saveLength = sizeof(DX_IOTT) * m_iottSize;
  unsigned char* saveBuf = new unsigned char[saveLength];
  memcpy(saveBuf, m_iott, saveLength);
  delete[] m_iott;
  
  m_iott = new DX_IOTT[m_iottSize += IOTT_SIZE_DELTA]; 
  memset(m_iott, 0, sizeof(DX_IOTT) * m_iottSize);
  memcpy(m_iott, saveBuf, saveLength);
  delete[] saveBuf;
  MMSLOG((LM_DEBUG,"%s IOTT reallocated to %d\n",devname,m_iottSize));
}



int MmsDeviceVoice::init()
{
  if  (m_iott)
       delete[] m_iott;

  m_iottSize = IOTT_SIZE_DELTA;             // IOTT array starts out with 
  m_iott = new DX_IOTT[IOTT_SIZE_DELTA];    // IOTT_SIZE_DELTA entries
  this->iottReset();

  return 0;
}


int MmsDeviceVoice::reset()
{
  int result = 0;

  clearDigitBuffer();
  result = dx_stopch(m_handle, EV_ASYNC);

  if  (result == -1)
  {
    MMSLOG((LM_ERROR,"%s dx_stopch failed on vox%d\n", devname, m_handle));     
    return result;                     
  } 

  return result;
}


DV_TPT* MMS_DV_TPT_LIST::addnode()          // Add and return a new list node                     
{ 
  DV_TPT* newnode = new DV_TPT;
  memset (newnode, 0, sizeof(DV_TPT));
  newnode->tp_type = IO_EOT;                 
  if  (tail)                               
  {    tail->tp_nextp = newnode;            // Link prior node to new node
       tail->tp_type  = IO_LINK;
  }
  else head = newnode;
  tail = newnode;
  size++;
  return newnode;
}



void MMS_DV_TPT_LIST::clear()               // Walk and clear the list
{ 
  DV_TPT* thisnode = head;

  while(thisnode) 
  { DV_TPT* nextnode = thisnode->tp_nextp; 
    delete  thisnode; 
    thisnode = nextnode; 
  }
  size = 0;
  head = tail = NULL;
}



long MmsDeviceVoice::elapsedTimeMs()        // Get play/rec time in ms
{
  MmsTime elapsed = ACE_OS::gettimeofday();
  elapsed -= this->startTime;
  const long ms = elapsed.msec();
  return ms > 0? ms: 0;
}



