// 
// mmsDeviceVoice.h
//
#ifndef MMS_DEVICEVOICE_H
#define MMS_DEVICEVOICE_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include "mmsMediaDevice.h"

static int CspStreamCallback(int ec_dev, char *buffer, UINT length);

struct MMS_DV_TPT_LIST;                      



class MmsDeviceVoice: public MmsMediaDevice       
{
  public:
  MmsDeviceVoice(int ordinal, MmsConfig*);  // Ctor
  virtual ~MmsDeviceVoice(); 
                                            // Open voice resource
  virtual mmsDeviceHandle open(OPENINFO& openinfo, unsigned short mode);

  virtual void close();                     // Close voice resource
                                            // Get transmit timeslot
  virtual int timeslot(SC_TSINFO* slotinfo); 
                                            // Listen on a timeslot
  virtual int listen  (SC_TSINFO* slotinfo);

  virtual int unlisten(); 

  virtual int isListening(); 
                                            // Halt any voice media operation
  int  stopMediaOperation(const int mode = ASYNC);  

  struct MMS_PLAYRECINFO                    // Play/record parameters
  { int  rate;                              // RATE_6KHZ,RATE_8KHZ,RATE_11KHZ
    int  samplesize;                        // SIZE_4BIT, SIZE_8IT, SIZE_16BIT
    int  mode;                              // MULAW,ALAW,PCM,ADPCM,TEXT
    int  filetype;                          // VOX,WAV,UIO
    int  nogain;                            // Do not use AGC (boolean)
    int  tone;                              // Play tone prior
    int  flags;
    enum bitflags {LOGPLAYREC=2, LOGTPT=4};
    MMS_PLAYRECINFO() {rate=RATE_8KHZ; mode=ADPCM; filetype=VOX; samplesize=SIZE_4BIT, nogain=0; tone=0; flags=0;}
    MMS_PLAYRECINFO(int r,int m,int t, int s) {rate=r; mode=m; filetype=t; samplesize=s; nogain=0; tone=0; flags=0;}
  };
                                            // Play from n files (data|mem)
  int  playMultiple(MMS_DV_TPT_LIST* tptlist,  
       MMS_PLAYRECINFO*, unsigned int mode=ASYNC);
                                            // Record voice to disk
  int  record(MMS_DV_TPT_LIST* tptlist, MMS_PLAYRECINFO*, char* path, unsigned int mode=ASYNC);
                                            // Record voice to n files (data|mem)
  int  recordMultiple(MMS_DV_TPT_LIST* tptlist,  
       MMS_PLAYRECINFO*, unsigned int mode=ASYNC);
                                            // Record 2 voice sources to n files 
  int  recordTransaction(MmsMediaDevice* party1, MmsMediaDevice* party2, 
       MMS_DV_TPT_LIST* tptlist, MMS_PLAYRECINFO*, unsigned int mode);
                                            // Play a 1 or 2 frequency tone
  int  playtone(int f1=600, int f2=0, int dB1=(-10), int dB2=(-10), 
       int duration=50, MMS_DV_TPT_LIST* tptlist=0, int mode=ASYNC);
                                            // Collect DTMF 
  int  receiveDigits(MMS_DV_TPT_LIST* tptlist, int mode=ASYNC, int* outcond=0);

  int  pcmToWav(char* path, MMS_PLAYRECINFO* info);
                                            // Specify playback adj digits
  int  assignVolumeAdjustmentDigit(char digit, int adjval);
  int  assignSpeedAdjustmentDigit (char digit, int adjval);
  int  clearVolumeSpeedAdjustments();
                                            // Adjust playback speed/volume
  int  adjustVolume(int action, int adjsize, int toggletype=0);
  int  adjustSpeed (int action, int adjsize, int toggletype=0);
  int  checkUserDigitBuffer();   
  int  checkDigitBuffer();
  void clearDigitBuffer();
  void logDigitBuffer();  

       // HMP ECR (Echo Cancellation Resource)
       // Whether or not echo cancellation is enabled is a licensing issue. 
       // We will detect echo cancellation enabled/disabled via config file.
       // When echo cancellation is enabled, different functions are used to 
       // get timeslot, listen, and unlisten. 
       // In addition, if echo cancellation is enabled, non-linear processing 
       // (NLP) may be enabled or disabled during the listen (dx_listenecrex 
       // in the HMP Voice Software Reference Programmers' Guide for details). 
       // NLP will be enabled/disabled via both user config file tuning, and 
       // by the application; for example, if NLP is enabled, it should be   
       // turned off during voice recognition.
  int  enableEchoCancellation(int onOrOff);
  int  enableNlp(int onOrOff);
  int  isEchoCancellationEnabled() { return m_isEchoCancellationEnabled; }
  int  isNlpEnabled() { return m_ecrct.isNlpEnabled(); }

  DX_IOTT* iott()     { return m_iott; }
  void iottReset();  // needs to be called from a play/rec init
  int  iottClose();  // needs to be called from a play/rec cleanup
                                            // Add a termination cond to list
  int  setTerminationCondition(MMS_DV_TPT_LIST& tptlist, int condition, 
          int value, int flags=(-1), int extradata=0);
                                            // Add a digit to a termination mask
  unsigned short addToDigitMask(unsigned char digit, unsigned short mask);

  int  openfileRecord(char* path);
  int  openfilePlay  (char* path, int offset=0, int length=0);
  int  setIoXferBlock   (MMS_PLAYRECINFO* recinfo);
  int  setPlayRecordMode(MMS_PLAYRECINFO* recinfo, unsigned int& mode);  

  int  registerCallStatusSilence();
  int  clearCallStatus();

  unsigned int terminationReason();
  char* digitBuffer() { return m_digitBuf.dg_value; }
  ACE_Thread_Mutex dataLock;                // Digit buffer, listen mutex
  MmsTime startTime;                        // Play duration timer 
 
  enum SAMPLESIZES{SIZE_4BIT=1, SIZE_8BIT, SIZE_16BIT}; 
  enum RATES{RATE_6KHZ=1, RATE_8KHZ, RATE_11KHZ}; 
  enum MODES{MULAW=1, ALAW, PCM, ADPCM, TEXT};
  enum TYPES{VOX=1, WAV, UIO};

  virtual void showTerminationParameterList(MMS_DV_TPT_LIST&);
  void elapsedTimeReset() { startTime = ACE_OS::gettimeofday(); }
  long elapsedTimeMs();
  int  isChannelBusy;
  int  islistened;
  unsigned int volspeed;
  int  isCspDevice() { return (flags & DEVICEFLAGS_IS_CSP) != 0; }
  int  isStreaming() { return (flags & DEVICEFLAGS_IS_STREAMING) != 0; }
  int  streamCsp(const int voiceBargein = 0);
  int  startStreaming();

  enum deviceCaps { DEVICECAP_CSP = 1 };    // Desired device capabilities
                                                                                                
  int reset();

  protected:
  enum{IOTT_SIZE_DELTA = MMS_MAX_INITIAL_IOTTS}; 
  int      m_iottSize;
  int      m_iottCount;
  DX_XPB   m_xpb;  	 
  DX_IOTT* m_iott;
  DV_DIGIT m_digitBuf;
  MmsConfig* config; 

  struct ECRCTEX: public DX_ECRCT           // HMP echo cancellation resource
  { ECRCTEX()                                
    { ct_length  = SIZE_OF_ECR_CT;          // Defined in dxxxlib.h
      ct_NLPflag = ECR_CT_DISABLE;          // Default to NLP off
    } 
    int enableNlp(int enable=TRUE)          // Turn NLP on or off
    { int  wasNlpEnabled = (ct_NLPflag == ECR_CT_ENABLE);
      ct_NLPflag = enable? ECR_CT_ENABLE: ECR_CT_DISABLE;  
      return wasNlpEnabled;                 // Note that ECR_CT_ENABLE is zero  
    }
    int isNlpEnabled() { return ct_NLPflag == ECR_CT_ENABLE; }
  };

  ECRCTEX  m_ecrct;                         // HMP echo cancellation resource
  int  m_isEchoCancellationEnabled;         // Configured item

  void iottRealloc();
  DX_IOTT* iottNext();

  int  init();

  int  adjustVolumeOrSpeed(int which, int action, int adjvalue, int toggletype=0);
  void logIoXferBlockData();
  void logPlayRecordMode(unsigned int& mode);
};                                         



struct MMS_DV_TPT_LIST                      // List of termination conditions 
{                                           // implemented as a linked list
  DV_TPT* addnode();                        // Add and return a new list node
  void clear();                             // Walk and clear the list
  DV_TPT* head, *tail;
  int     size;
  virtual ~MMS_DV_TPT_LIST() { clear(); }
  MMS_DV_TPT_LIST(): head(0) { clear(); }
};



class MmsVolumeSpeedEncoder
{ 
  // Encapsulates editing, packing, unpacking of volume and speed settings  
  // Volume/speed settings pack into 18 bits as follows:
  //
  //          bits    
  //    111111110000000000   V: volume 5 bits -- value 0-21 mapped to -10 thru +10
  //    765432109876543210   S: speed  5 bits -- ditto
  //    ------------------   A: vol adjtype 2 bits ---  C: vol togtype 2 bits
  //    AABBCCDDSSSSSVVVVV   B: spd adjtype 2 bits ---  D: spd togtype 2 bits  

  public:
  MmsVolumeSpeedEncoder() { clear(); }

  MmsVolumeSpeedEncoder(const unsigned packed) 
  { clear(); 
    unpack(packed); 
    isvol = vol != 0; isspd = spd != 0;
  }

  MmsVolumeSpeedEncoder
  ( unsigned v, unsigned s, unsigned va, unsigned sa, unsigned vt, unsigned st) 
  { clear(); 
    volume(v); speed(s); vadjtype(va); sadjtype(sa); vtogtype(vt); stogtype(st); 
  }

  void clear() { isvol = isspd = vol = spd = vatype = satype = vttype = sttype = 0; } 

  // Normalization offset for volume or speed setting. HMP value is -10 thru +10
  // so we add 11 to this value when we pack so we don't have to allow for a sign
  #define MMS_VSOFF 11   
 
  unsigned int pack();

  void unpack(const unsigned int packed);
   
  enum whichSetting 
  { VSE_VOLVAL=1, VSE_SPEEDVAL=2, VSE_VADJTYPE=4, 
    VSE_VOLUME=5, VSE_SADJTYPE=8, VSE_SPEED=10 
  };

  void copy(const whichSetting, MmsVolumeSpeedEncoder&);

  static int isDefaultVolumeSpeed(const unsigned packed)
  {
    return (packed & 0x3ff) == 0; // 10 bits
  }

  // Setters
  void isVolumeSet(const int b) { this->isvol = b; }
  void isSpeedSet (const int b) { this->isspd = b; }

  void volume(const int v)
  {
    this->vol = v < -10? -10: v > 10? 10: v;
  }

  void speed(const int s)
  {
    this->spd = s < -10? -10: s > 10? 10: s;
  }

  void vadjtype(const unsigned vat)
  {    
    this->vatype = vat < 0? 0: vat > 3? 3: vat;
  }

  void sadjtype(const unsigned sat)
  {    
    this->satype = sat < 0? 0: sat > 3? 3: sat;
  }

  void vtogtype(const unsigned vtt)
  {    
    this->vttype = vtt < 0? 0: vtt > 3? 3: vtt;
  }

  void stogtype(const unsigned stt)
  {    
    this->sttype = stt < 0? 0: stt > 3? 3: stt;
  }

  // Getters
  int isParamSet()   { return isvol|isspd; }
  int isVolumeSet()  { return isvol;  }
  int isSpeedSet ()  { return isspd;  }
  unsigned volume()  { return vol;    }
  unsigned speed()   { return spd;    }
  unsigned vadjtype(){ return vatype; }
  unsigned sadjtype(){ return satype; }
  unsigned vtogtype(){ return vttype; }
  unsigned stogtype(){ return sttype; }

  private:
  int isvol, isspd, vol, spd;
  unsigned int vatype, satype, vttype, sttype;
};


#endif