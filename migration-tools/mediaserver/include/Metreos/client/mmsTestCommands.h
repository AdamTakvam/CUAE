// mmsTestCommands.h  
#ifndef MMS_TESTCOMMANDS_H
#define MMS_TESTCOMMANDS_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif

// These classes represent incoming protocol data for each server test 
// command, and the subsequent parsing of the command data. That is,
// they present some "pre-parsed" test data for media server testing.
// There is no need to go through the exercise of actually parsing
// command data in the protocol converter tests, since the format in
// which data is presented is vendor-specific, and thus that logic will 
// be different for each protocol supported by the media server. 

#define SERVERTEST_BOGUS_REMOTE_IP          "127.0.0.1"
#define SERVERTEST_BOGUS_REMOTE_PORT        5002

#define BOGUSNAME_CONNECT                   "connect"
#define BOGUSNAME_DISCONNECT                "disconnect"
#define BOGUSNAME_PLAY                      "play"
#define BOGUSNAME_RECORD                    "record"
#define BOGUSNAME_RECORD_TRANSACTION        "recordtrans"
#define BOGUSNAME_PLAYTONE                  "playtone"
#define BOGUSNAME_RECEIVE_DIGITS            "receivedigits"
#define BOGUSNAME_STOP_MEDIA_OPERATION      "stopmedia"
#define BOGUSNAME_ASSIGN_VOLADJ_DIGIT       "assignvoladjdigit"
#define BOGUSNAME_ASSIGN_SPDADJ_DIGIT       "assignspdadjdigit"
#define BOGUSNAME_ADJUST_VOLUME             "adjustvol"
#define BOGUSNAME_ADJUST_SPEED              "adjustspd"
#define BOGUSNAME_CLEAR_ADJUSTMENTS         "clearadjustments"
#define BOGUSNAME_CONFERENCE_RESOURCES      "confresources"
#define BOGUSNAME_SET_ATTRIBUTES            "setattrs"
#define BOGUSNAME_ENABLE_VOLCONTROL         "enablevolctrl"
#define BOGUSNAME_VOICEREC                  "voiceRec"



class MmsBogusProtocolData                  // Base class for each command
{
  public:
  char* getCommandName()  { return (char*)commandName.c_str(); }
  int   getConnectionID() { return connectionID; }
  int   getConxIdLength() { return sizeof(connectionID); }

  virtual int getCommandTimeoutMs() { return commandTimeoutMs; }

  std::string commandName;
  int  connectionID;
  int  commandTimeoutMs;         // Connection timeout 0=default

  MmsBogusProtocolData(char* name, int conxID=0)
  {
    commandName  = name;
    connectionID = conxID;
    commandTimeoutMs = 0;
  }
};



struct MmsBogusConferenceInfo               // Represents parsed conference data
{
  int conferenceID;
  int isSoundTone;
  int isNoToneForPassive;
  MmsBogusConferenceInfo() {conferenceID = 0; isSoundTone = 1; isNoToneForPassive = 1;}; 

  int getConferenceID()       { return conferenceID; }   
  int getIsSoundTone()        { return isSoundTone; } 
  int getIsNoToneForPassive() { return isNoToneForPassive; } 
};



struct MmsBogusConfereeInfo                 // Represents parsed conferee 
{                                           // identification data
  int isKibitzer;
  int isReadOnly;  
  int isTariffTone;
  int isCoach;
  int isPupil;
  MmsBogusConfereeInfo() { isKibitzer = isReadOnly = isTariffTone = isCoach = isPupil = 0; }

  int getIsKibitzer()   { return isKibitzer; }
  int getIsReadOnly()   { return isReadOnly; }
  int getIsTariffTone() { return isTariffTone; }
  int getIsCoach()      { return isCoach; }
  int getIsPupil()      { return isPupil; }

};


                                            
class MmsBogusProtocolDataCONNECT: public MmsBogusProtocolData, 
  public MmsBogusConferenceInfo, public MmsBogusConfereeInfo  
{
  // Represents data for a parsed connection request
  public:
  int  isConference;             // boolean
  int  port;                     // even
  int  rFrameSize, lFrameSize;   // 0,10,20,30: 20
  int  rFpp, lFpp;               // 0-4 not g.711: 1
  int  rVadEnable, lVadEnable;   // boolean: 0
  int  rCoderPayloadType;        // 0-127: 0
  int  lCoderPayloadType;        // 0-127: 0
  int  rRedundancyPayloadType;   // 0-127: 0
  int  lRedundancyPayloadType;   // 0-127: 0
  int  sessionTimeoutSeconds;    // Session timeout 0=default
  std::string ip;                // dotted IP address
  std::string remoteCoderType;   // "G711ALAW64K"
  std::string localCoderType;     
  
  MmsBogusProtocolDataCONNECT(): MmsBogusProtocolData(BOGUSNAME_CONNECT, 0)
  {
    port = SERVERTEST_BOGUS_REMOTE_PORT;
    ip   = SERVERTEST_BOGUS_REMOTE_IP;

    isConference = 0;
    rCoderPayloadType = lCoderPayloadType = 0;
    rRedundancyPayloadType = lRedundancyPayloadType = 0;
    sessionTimeoutSeconds  = 0;
    rFrameSize = 20;  rFpp = 1; rVadEnable = 0;
    remoteCoderType = "G711ALAW64K";
    lFrameSize = 0;  lFpp = 0; lVadEnable = 0;
    localCoderType = "";
  }

  int getIsConference()     { return isConference; }
  int getPort()             { return port; }
  int getRemoteFrameSize()  { return rFrameSize; }
  int getRemoteFpp()        { return rFpp; }
  int getRemoteVadEnable()  { return rVadEnable; }
  int getRemoteCoderPayloadType()      { return rCoderPayloadType; }
  int getRemoteRedundancyPayloadType() { return rRedundancyPayloadType; }
  int getLocalFrameSize()   { return lFrameSize; }
  int getLocalFpp()         { return lFpp; }
  int getLocalVadEnable()   { return lVadEnable; }
  int getLocalCoderPayloadType()      { return lCoderPayloadType; }
  int getLocalRedundancyPayloadType() { return lRedundancyPayloadType; }

  int getSessionTimeoutSecs() { return sessionTimeoutSeconds; }

  char* getIP()             { return (char*)ip.c_str(); }
  char* getRemoteCoderType(){ return (char*)remoteCoderType.c_str(); }
  char* getLocalCoderType() { return (char*)localCoderType.c_str(); }

  int getIpLength()         { return ip.size(); }
  int getRemoteCoderLength(){ return remoteCoderType.size(); }
  int getLocalCoderLength() { return remoteCoderType.size(); }
};



class MmsBogusProtocolDataDISCONNECT: public MmsBogusProtocolData 
{
  public:
  MmsBogusProtocolDataDISCONNECT(int conxID):
  MmsBogusProtocolData(BOGUSNAME_DISCONNECT, conxID) { }
};



struct MMS_BOGUS_FILELIST                 // Represents a list of file paths
{
  enum{MAXFILES=5};
  int  numfiles, currfile;              
  std::string filepath[5];          
  MMS_BOGUS_FILELIST() { numfiles = currfile = 0; }

  char* nextfile() 
  { return currfile < numfiles? (char*)(filepath[currfile++].c_str()): NULL;
  }

  int put(char* path) 
  { if (numfiles >= MAXFILES) return -1;
    filepath[numfiles++] = path;
    return 0;
  }

  int count() { return numfiles; }
};



                                            
struct MMS_BOGUS_TERMINATION_CONDITION      // Represents data for an i/o 
{                                           // termination condition
  enum{CONDEOF, TIMEOUT, STOPPED, DIGIT, DIGITDELAY, MAXDIGITS, NONSILENCE, SILENCE};
  int  condition;
  int  length;
  int  value1;
  int  value2;
  MMS_BOGUS_TERMINATION_CONDITION() {memset(this,0,sizeof(MMS_BOGUS_TERMINATION_CONDITION));}

  int  isConditionEOF()        { return condition == CONDEOF; }  
  int  isConditionTimeout()    { return condition == CONDEOF; }
  int  isConditionStopped()    { return condition == STOPPED; }
  int  isConditionDigit()      { return condition == DIGIT; }
  int  isConditionDigitDelay() { return condition == DIGITDELAY; }
  int  isConditionMaxDigits()  { return condition == MAXDIGITS; }
  int  isConditionNonSilence() { return condition == NONSILENCE; }
  int  isConditionSilence()    { return condition == SILENCE; }
};


                                            // Represents a list of
struct MMS_BOGUS_TERMINATION_CONDITIONS     // termination conditions
{
  enum{MAXCONDITIONS=8};
  int  numconditions; 
  MMS_BOGUS_TERMINATION_CONDITION termCondition[MAXCONDITIONS];

  int put(int cond, int len, int v1, int v2)
  {
    if  (numconditions >= MAXCONDITIONS) return -1;

    MMS_BOGUS_TERMINATION_CONDITION* tc = &termCondition[numconditions++];
    tc->condition = cond;
    tc->length = len;
    tc->value1 = v1;
    tc->value2 = v2;
    return 0;
  }

  MMS_BOGUS_TERMINATION_CONDITION* get(int i) { return &termCondition[i]; }

  MMS_BOGUS_TERMINATION_CONDITIONS() { numconditions = 0; }
  int count() { return numconditions; }
};


                                            
class MmsBogusProtocolDataPLAY: public MmsBogusProtocolData 
{
  // Represents data for a parsed play request
  // Note that for DIGIT this is actually a DX_DIGMASK condition and we should
  // write the MMSP_DIGITLIST parameter to the condition, with a digit string
  public:
  MMS_BOGUS_FILELIST files;

  MMS_BOGUS_TERMINATION_CONDITIONS terminations;

  enum{MULAW,ALAW,PCM,ADPCM};
  enum{KHZ6,KHZ8,KHZ11};

  int  format;
  int  rate;
  
  MmsBogusProtocolDataPLAY(int conxID): 
  MmsBogusProtocolData(BOGUSNAME_PLAY, conxID)
  { 
    format = ADPCM; rate = KHZ8;

    files.put("thankyou.vox");
    files.put("thankyou.vox");

    terminations.put(MMS_BOGUS_TERMINATION_CONDITION::DIGIT,   '#',  0, 0);
    terminations.put(MMS_BOGUS_TERMINATION_CONDITION::TIMEOUT, 1200, 0, 0);
  }
};


                                            
class MmsBogusProtocolDataRECORD: public MmsBogusProtocolData 
{
  // Represents data for a parsed record request
  public:
  MMS_BOGUS_FILELIST files;

  MMS_BOGUS_TERMINATION_CONDITIONS terminations;

  enum{MULAW,ALAW,PCM,ADPCM};
  enum{KHZ6,KHZ8,KHZ11};

  int  format;
  int  rate;
  
  MmsBogusProtocolDataRECORD(int conxID): 
  MmsBogusProtocolData(BOGUSNAME_RECORD, conxID)
  { 
    format = ADPCM; rate = KHZ8;

    files.put("recorded.vox");

    terminations.put(MMS_BOGUS_TERMINATION_CONDITION::SILENCE, 200, 0, 0);
    terminations.put(MMS_BOGUS_TERMINATION_CONDITION::DIGIT,   '#', 0, 0);
    terminations.put(MMS_BOGUS_TERMINATION_CONDITION::TIMEOUT, 400, 0, 0);
  }
};



class MmsBogusProtocolDataRECORDTRANSACTION: public MmsBogusProtocolData 
{
  public:
  MMS_BOGUS_FILELIST files;

  MMS_BOGUS_TERMINATION_CONDITIONS terminations;

  enum{MULAW,ALAW,PCM,ADPCM};
  enum{KHZ6,KHZ8,KHZ11};
                                            // Second connection is passed
  int connectionID2;                        // in the constructor
  int format;
  int rate;
  
  MmsBogusProtocolDataRECORDTRANSACTION(int conxID, int conxID2): 
  MmsBogusProtocolData(BOGUSNAME_RECORD_TRANSACTION, conxID)
  { 
    connectionID2 = conxID2;
    format = PCM; rate = KHZ8;
  }
};



class MmsBogusProtocolDataPLAYTONE: public MmsBogusProtocolData 
{
  public:
  MMS_BOGUS_TERMINATION_CONDITIONS terminations;

  int f1, f2, a1, a2, duration;
  
  MmsBogusProtocolDataPLAYTONE(int conxID): 
  MmsBogusProtocolData(BOGUSNAME_PLAYTONE, conxID)
  { 
    f1 = 1000; f2 = 500; a1 = -10; a2 = -10; duration = 100;

    terminations.put(MMS_BOGUS_TERMINATION_CONDITION::DIGIT,   '#', 0, 0);
    terminations.put(MMS_BOGUS_TERMINATION_CONDITION::TIMEOUT, duration, 0, 0);
  }
};



class MmsBogusProtocolDataRECEIVEDIGITS: public MmsBogusProtocolData 
{
  public:
  MMS_BOGUS_TERMINATION_CONDITIONS terminations;

  char digits[36];
  
  MmsBogusProtocolDataRECEIVEDIGITS(int conxID): 
  MmsBogusProtocolData(BOGUSNAME_RECEIVE_DIGITS, conxID)
  { 
    memset(digits,0,sizeof(digits));

    terminations.put(MMS_BOGUS_TERMINATION_CONDITION::TIMEOUT, 100, 0, 0);
    terminations.put(MMS_BOGUS_TERMINATION_CONDITION::DIGIT,   '#', 0, 0);
  }
};



struct MMS_BOGUS_ADJUSTMENT_DIGIT
{
  enum values{DIGIT_UP=1, DIGIT_DOWN, DIGIT_RESET};
  int  value;
  unsigned char digit;

  unsigned char getDigit()  { return digit; }
  int isDigitUp()    { return value == DIGIT_UP; }
  int isDigitDown()  { return value == DIGIT_DOWN; }
  int isDigitReset() { return value == DIGIT_RESET; }


  MMS_BOGUS_ADJUSTMENT_DIGIT() { value = 0; digit = 0; }
};



class MmsBogusProtocolDataASSIGNVOLDIGIT: public MmsBogusProtocolData 
{
  public:
  MMS_BOGUS_ADJUSTMENT_DIGIT adjustment;
  
  MmsBogusProtocolDataASSIGNVOLDIGIT(int conxID): 
  MmsBogusProtocolData(BOGUSNAME_ASSIGN_VOLADJ_DIGIT, conxID)
  { 
  }
};



class MmsBogusProtocolDataASSIGNSPEEDDIGIT: public MmsBogusProtocolData 
{
  public:
  MMS_BOGUS_ADJUSTMENT_DIGIT adjustment;
  
  MmsBogusProtocolDataASSIGNSPEEDDIGIT(int conxID): 
  MmsBogusProtocolData(BOGUSNAME_ASSIGN_SPDADJ_DIGIT, conxID)
  { 
  }
};



struct MMS_BOGUS_VOLSPEED_ADJUSTMENT
{
  enum  values{DIGIT_UP=1, DIGIT_DOWN, DIGIT_RESET};
  int   value;

  enum  adjustments{ADJABSOLUTE=1, ADJRELATIVE, ADJTOGGLE};
  short adjustment;

  enum  adjustmenttypes{TOGGLEORG2PRIOR,RESETORG,SETPRIOR,RESETALL};
  short adjustmenttype;

  MMS_BOGUS_VOLSPEED_ADJUSTMENT() { memset(this,0,sizeof(MMS_BOGUS_VOLSPEED_ADJUSTMENT));}
};



class MmsBogusProtocolDataADJUSTVOLUME: public MmsBogusProtocolData 
{
  public:
  MMS_BOGUS_VOLSPEED_ADJUSTMENT adjustment;
  
  MmsBogusProtocolDataADJUSTVOLUME(int conxID): 
  MmsBogusProtocolData(BOGUSNAME_ADJUST_VOLUME, conxID)
  { 
  }
};



class MmsBogusProtocolDataADJUSTSPEED: public MmsBogusProtocolData 
{
  public:
  MMS_BOGUS_VOLSPEED_ADJUSTMENT adjustment;
  
  MmsBogusProtocolDataADJUSTSPEED(int conxID): 
  MmsBogusProtocolData(BOGUSNAME_ADJUST_SPEED, conxID)
  { 
  }
};



class MmsBogusProtocolDataCLEARADJUSTMENTS: public MmsBogusProtocolData 
{
  public:
  MmsBogusProtocolDataCLEARADJUSTMENTS(int conxID): 
  MmsBogusProtocolData(BOGUSNAME_CLEAR_ADJUSTMENTS, conxID)
  { 
  }
};



class MmsBogusProtocolDataCONFRESOURCES: public MmsBogusProtocolData 
{
  public:
  int returnvalue;
  MmsBogusProtocolDataCONFRESOURCES(int conxID): 
  MmsBogusProtocolData(BOGUSNAME_CONFERENCE_RESOURCES, conxID)
  { 
  }
};



class MmsBogusProtocolDataCONFEREEATTRS: public MmsBogusProtocolData 
{
  public:
  unsigned int attrs;
  MmsBogusProtocolDataCONFEREEATTRS(int conxID): 
  MmsBogusProtocolData(BOGUSNAME_SET_ATTRIBUTES, conxID)
  { attrs = 0;
  }
};



class MmsBogusProtocolDataCONFEREEVOLENABLE: public MmsBogusProtocolData 
{
  public:
  int onoff;
  MmsBogusProtocolDataCONFEREEVOLENABLE(int conxID): 
  MmsBogusProtocolData(BOGUSNAME_ENABLE_VOLCONTROL, conxID)
  { onoff=1;
  }
};



#endif
