//  
// mmsAppMessage.h
//
// IPC XML static data
//
#ifndef MMS_APPMESSAGEX_H
#define MMS_APPMESSAGEX_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif


class MmsAppMessageX 
{
  public:                                             
  static MmsAppMessageX* instance();

  virtual ~MmsAppMessageX() { }

  static void destroy();

  private:
  static MmsAppMessageX* singleton;
  MmsAppMessageX() {}
  void init();

  public:

  enum messageIDs
  { MMSMSG_CONNECT, 
    MMSMSG_DISCONNECT, 
    MMSMSG_PLAY, 
    MMSMSG_PLAYTONE,  
    MMSMSG_GETDIGITS,  
    MMSMSG_STOPMEDIA,
    MMSMSG_RECORD,
    MMSMSG_CONFEREESETATTR,
    MMSMSG_MONITOR_CALL_STATE,
    MMSMSG_SENDDIGITS,
    MMSMSG_SERVER, 
    MMSMSG_HEARTBEAT,
    MMSMSG_ADJUST_PLAY,
    MMSMSG_VOICEREC,
     
    // MMSMSG_RECORDTRANSACTION,
    // MMSMSG_CONFRESREMAINING,
    // MMSMSG_CONFENABLEVOLCONTROL,
    // MMSMSG_ASSIGNVOLDIGIT,
    // MMSMSG_ASSIGNSPEEDDIGIT,
    // MMSMSG_ADJUSTVOL,
    // MMSMSG_ADJUSTSPEED,
    // MMSMSG_CLEARVOLSPEED, 
                        
    MESSAGE_COUNT
  };

  enum paramIDs
  { TRANSACTION_ID,
    CONNECTION_ID,
    CONFERENCE_ID,
    CLIENT_ID,
    SERVER_ID,
    OPERATION_ID,
    COMMAND_TIMEOUT,
    SESSION_TIMEOUT,
    IP_ADDRESS,     
    PORT,
    CONNECTION_ATTRIBUTE,
    CONFERENCE_ATTRIBUTE,
    CONFEREE_ATTRIBUTE,
    AUDIO_FILE_ATTRIBUTE,
    AUDIO_TONE_ATTRIBUTE,
    TERMINATION_CONDITION,
    MEDIA_ELAPSED_TIME,
    FILE_NAME,
    HEARTBEAT_INTERVAL,
    HEARTBEAT_ID,
    HEARTBEAT_PAYLOAD,
    REXMIT_CONNECTION,   
    MACHINE_NAME,
    QUEUE_NAME,
    CALL_STATE,
    USER_TOKEN,
    HAIRPIN,
    HAIRPIN_PROMOTE,
    CANCELED_TRANSID,
    VOLUME,
    SPEED,
    ADJUST_TYPE,
    TOGGLE_TYPE,
    EXPIRES,
    DIGITS,
    QUERY,
    BLOCK,
    MODIFY,
    APP_NAME,
    LOCALE,
    CONCURRENT,
    GRAMMAR_NAME,
    VR_MEANING,
    VR_SCORE,
    VOICE_BARGEIN,
    ROUTING_GUID,
    CANCEL_ON_DIGIT,
    PARAM_COUNT
  };
 
  enum termcondIDs
  { MAXTIME,
    DIGIT,
    DIGITLIST,
    MAXDIGITS,
    SILENCE,
    NONSILENCE,
    DIGITDELAY,
    DIGITPATTERN,
    TERMCOND_COUNT
  };

  enum conferenceAttrIDs
  { SOUND_TONE,
    NO_TONE,
    SOUND_TONE_WHEN_RECEIVE_ONLY,
    NO_TONE_WHEN_RECEIVE_ONLY,
    CONFERENCE_ATTR_COUNT
  };

  enum confereeAttrIDs
  { MONITOR,
    RECEIVE_ONLY,
    TARIFF_TONE,
    COACH,
    PUPIL,
    CONFEREE_ATTR_COUNT
  };

  enum connectionAttrIDs
  { CODER,
    FRAMESIZE,
    VADENABLE,
    CODER_TYPE_REMOTE,
    FRAMESIZE_REMOTE,
    VADENABLE_REMOTE,
    DATAFLOW_DIRECTION,
    CODER_TYPE_LOCAL,
    FRAMESIZE_LOCAL,
    VADENABLE_LOCAL,
    CONNECTION_ATTR_COUNT,
  };

  enum dataFlowDirectionIDs
  { IP_BIDIRECTIONAL, 
    IP_RECEIVEONLY, 
    IP_SENDONLY, 
    MULTICAST_SERVER, 
    MULTICAST_CLIENT,
    DATAFLOW_DIRECTION_COUNT
  };

  enum audioFileAttrIDs
  { FORMAT,
    ENCODING,
    BITRATE,
    SAMPLESIZE,
    AUDIO_FILE_ATTR_COUNT
  };

  enum audioToneAttrIDs
  { FREQUENCY1,
    AMPLITUDE1,
    FREQUENCY2,
    AMPLITUDE2,
    DURATION,
    AUDIO_TONE_ATTR_COUNT
  };

  enum serverQueryIDs
  { MEDIA_RESOURCES,
    SERVER_QUERY_COUNT
  };

  enum callStateIDs
  { CS_SILENCE,
    CS_NONSILENCE,
    CS_DIGITS,
     
    // CS_LCOFF:      // loop current off 
    // CS_LCON:       // loop current on 
    // CS_LCREV:      // loop current reversal 
    // CS_RINGS:      // rings received 
    // CS_RNGOFF:     // caller hang up event    
    // CS_TONEOFF:    // tone off event 
    // CS_TONEON:     // tone on event 
    // CS_WINK:       // received a wink 
    // CS_OFFHOOK:    // offhook event 
    // CS_ONHOOK:     // onhook event 
     
    CALL_STATE_COUNT
  };

  enum mediaResourceIDs
  { IP_RESOURCES_INSTALLED,
    IP_RESOURCES_AVAILABLE,
    VOICE_RESOURCES_INSTALLED,
    VOICE_RESOURCES_AVAILABLE,
    CONF_RESOURCES_INSTALLED,
    CONF_RESOURCES_AVAILABLE,
    LOBIT_RESOURCES_INSTALLED,
    LOBIT_RESOURCES_AVAILABLE,
    TTS_RESOURCES_INSTALLED,
    TTS_RESOURCES_AVAILABLE,
    ASR_RESOURCES_INSTALLED,
    ASR_RESOURCES_AVAILABLE,
    CSP_RESOURCES_INSTALLED,
    CSP_RESOURCES_AVAILABLE,    
    MEDIA_RESOURCE_COUNT
  };

  public:

  static char** messagenames;
  static char** paramnames;
  static char** termcondnames;
  static char** callStateNames;
  static char** conferenceAttrNames;
  static char** confereeAtrrNames;
  static char** connectionAttrNames; 
  static char** audioFileAttrNames;
  static char** audioToneAttrNames;
  static char** mediaResourceNames;
  static char** serverQueryNames;
  static char** dataflowDirectionNames; 

  protected:

  char* messagenamesX       [MESSAGE_COUNT];
  char* paramnamesX         [PARAM_COUNT];
  char* termcondnamesX      [TERMCOND_COUNT];
  char* callStateNamesX     [CALL_STATE_COUNT];
  char* conferenceAttrNamesX[CONFERENCE_ATTR_COUNT];
  char* confereeAtrrNamesX  [CONFEREE_ATTR_COUNT];
  char* connectionAttrNamesX[CONNECTION_ATTR_COUNT]; 
  char* audioFileAttrNamesX [AUDIO_FILE_ATTR_COUNT];
  char* audioToneAttrNamesX [AUDIO_TONE_ATTR_COUNT];
  char* mediaResourceNamesX [MEDIA_RESOURCE_COUNT];
  char* serverQueryNamesX   [SERVER_QUERY_COUNT];
  char* dataflowDirectionNamesX[DATAFLOW_DIRECTION_COUNT];                           
};


#endif // MMS_APPMESSAGEX_H