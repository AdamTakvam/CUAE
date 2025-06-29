//
// mmsAudioFileDescriptor.h  
//
// Companion file written with each mms recording
//
#ifndef MMS_AFD_H
#define MMS_AFD_H
#ifdef  MMS_WINPLATFORM
#pragma once 
#endif

#include "mmsCommon.h"
#include "mmsDeviceVoice.h"

#define MMSAFD_ID1 "MMS "
#define MMSAFD_ID2 "AFD "

#define MMSAFC_CURRENT_VERSION " 01 "
#define MMSAFD_TIMESTAMPMASK "%04d%02d%02d %02d%02d%02d " 

#define MMSAFD_VOX   "vox "
#define MMSAFD_WAV   "wav "
#define MMSAFD_ULAW  "ulaw"
#define MMSAFD_ALAW  "alaw"
#define MMSAFD_PCM   "pcm "
#define MMSAFD_ADPCM "apcm"
#define MMSAFD_6KHZ  "   6"
#define MMSAFD_8KHZ  "   8"
#define MMSAFD_11KHZ "  11"
#define MMSAFD_4BIT  "   4"
#define MMSAFD_8BIT  "   8"
#define MMSAFD_16BIT "  16"



struct  MMSPLAYFILEINFO                     // Object to pass back a playfile
{ char* path;                               // path, and in which to build a
  int   pathlength;                         // path to a mms properties file
  int   isPlay;
  int   isTtsText;
  char  fullpath[MAXPATHLEN];
  char  propfilepath[MAXPATHLEN];
  MmsLocaleParams ldata;
  void  clear()     { memset(this,0,sizeof(MMSPLAYFILEINFO)); }
  MMSPLAYFILEINFO() { clear(); }
  MMSPLAYFILEINFO(MmsLocaleParams& lp) { clear(); ldata.set(lp); }
};



class MmsAudioFileDescriptor
{
  // Properties companion file written with each file recorded by mms

  public:                                   // File layout:
  char id1       [4];                       // MMS  
  char id2       [4];                       // AFD
  char type      [4];                       // vox, wav
  char mode      [4];                       // ulaw, alaw, pcm, acpm
  char rate      [4];                       // 6, 8, 11
  char version   [4];                       // bNNb schema version
  char timestamp[16];                       // yyyymmdd hhmmss 
  char duration  [4];                       // nnnn (total days file lives) 
  char samplesize[4];                       // 4, 8, 16
  char unused   [36];          
                                            // We write/read this many bytes
  int  calculateRecordSize() { return (unused + sizeof(unused)) - id1; }

  int  write(MmsDeviceVoice::MMS_PLAYRECINFO& info, 
             MMSPLAYFILEINFO* pathinfo, int days=0);

  int  read(MmsDeviceVoice::MMS_PLAYRECINFO& info, 
            MMSPLAYFILEINFO* pathinfo);

  int  write(MMSPLAYFILEINFO* pathinfo);

  int  read(MMSPLAYFILEINFO* pathinfo);

  int  read(char* path);

  void set(MmsDeviceVoice::MMS_PLAYRECINFO& info);

  void get(MmsDeviceVoice::MMS_PLAYRECINFO& info);

  static int erase(MMSPLAYFILEINFO* pathinfo);

  void stamp();

  void setExpiration(int days);

  void stampToBinary(struct tm* t);

  int  isValid();
  int  isVox() { return memcmp(type, MMSAFD_VOX, 4) == 0; }
  int  isWav() { return memcmp(type, MMSAFD_WAV, 4) == 0; }

  int  daysToExpiration();

  static int toDays(const struct tm* t);

  MmsAudioFileDescriptor(); 

  private:
  ACE_Thread_Mutex recordlock;
};


#endif
