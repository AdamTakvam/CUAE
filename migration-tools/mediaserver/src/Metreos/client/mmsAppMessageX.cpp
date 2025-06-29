//
// MmsAppMessageX.cpp  
//
// IPC XML static data initialization 
//
#include "StdAfx.h"
#include "stdlib.h"
#pragma warning(disable:4786)
#include "mmsAppMessageX.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

// static instance data initialization
MmsAppMessageX* MmsAppMessageX::singleton = NULL;

char** MmsAppMessageX::messagenames           = NULL;
char** MmsAppMessageX::paramnames             = NULL;
char** MmsAppMessageX::termcondnames          = NULL;
char** MmsAppMessageX::callStateNames         = NULL;
char** MmsAppMessageX::conferenceAttrNames    = NULL;
char** MmsAppMessageX::confereeAtrrNames      = NULL;
char** MmsAppMessageX::connectionAttrNames    = NULL;
char** MmsAppMessageX::audioFileAttrNames     = NULL;
char** MmsAppMessageX::audioToneAttrNames     = NULL;
char** MmsAppMessageX::mediaResourceNames     = NULL;
char** MmsAppMessageX::serverQueryNames       = NULL;
char** MmsAppMessageX::dataflowDirectionNames = NULL;



MmsAppMessageX* MmsAppMessageX::instance()
{
  if (!singleton) 
  {      
       singleton = new MmsAppMessageX; 

       singleton->init();
  }
 
  return singleton;
}



void MmsAppMessageX::destroy()
{
   if (singleton) 
       delete singleton;
   singleton = 0;
}



void MmsAppMessageX::init()              
{                
  // Intialize static pointers to singleton instance arrays            
  MmsAppMessageX::messagenames           = messagenamesX;
  MmsAppMessageX::paramnames             = paramnamesX;
  MmsAppMessageX::termcondnames          = termcondnamesX;
  MmsAppMessageX::callStateNames         = callStateNamesX;
  MmsAppMessageX::conferenceAttrNames    = conferenceAttrNamesX;
  MmsAppMessageX::confereeAtrrNames      = confereeAtrrNamesX;
  MmsAppMessageX::connectionAttrNames    = connectionAttrNamesX;
  MmsAppMessageX::audioFileAttrNames     = audioFileAttrNamesX;
  MmsAppMessageX::audioToneAttrNames     = audioToneAttrNamesX;
  MmsAppMessageX::mediaResourceNames     = mediaResourceNamesX;
  MmsAppMessageX::serverQueryNames       = serverQueryNamesX;
  MmsAppMessageX::dataflowDirectionNames = dataflowDirectionNamesX;
               
  // Initialize parse table data
  messagenamesX[MMSMSG_CONNECT]              = "connect";
  messagenamesX[MMSMSG_DISCONNECT]           = "disconnect";
  messagenamesX[MMSMSG_PLAY]                 = "play";
  messagenamesX[MMSMSG_PLAYTONE]             = "playTone";
  messagenamesX[MMSMSG_GETDIGITS]            = "receiveDigits";  
  messagenamesX[MMSMSG_STOPMEDIA]            = "stopMediaOperation"; 
  messagenamesX[MMSMSG_RECORD]               = "record";
  messagenamesX[MMSMSG_SENDDIGITS]           = "sendDigits";
  messagenamesX[MMSMSG_MONITOR_CALL_STATE]   = "monitorCallState";
  messagenamesX[MMSMSG_CONFEREESETATTR]      = "confereeSetAttribute";
  messagenamesX[MMSMSG_SERVER]               = "server";
  messagenamesX[MMSMSG_HEARTBEAT]            = "heartbeat";
  messagenamesX[MMSMSG_ADJUST_PLAY]          = "adjustPlay";
  messagenamesX[MMSMSG_VOICEREC]             = "voiceRec";

  #if 0                                     // Commands not yet implemented
  messagenamesX[MMSMSG_RECORDTRANSACTION]    = "recordTransaction";
  messagenamesX[MMSMSG_CONFRESREMAINING]     = "conferenceResourcesRemaining";
  messagenamesX[MMSMSG_CONFENABLEVOLCONTROL] = "confereeEnableVolumeControl";
  messagenamesX[MMSMSG_ASSIGNVOLDIGIT]       = "assignVolumeAdjustmentDigit";
  messagenamesX[MMSMSG_ASSIGNSPEEDDIGIT]     = "assignSpeedAdjustmentDigit";
  messagenamesX[MMSMSG_ADJUSTVOL]            = "adjustVolume";
  messagenamesX[MMSMSG_ADJUSTSPEED]          = "adjustSpeed";
  messagenamesX[MMSMSG_CLEARVOLSPEED]        = "clearVolSpeedAdjustments";
  #endif 

  paramnamesX[TRANSACTION_ID]       = "transactionId";
  paramnamesX[CONNECTION_ID]        = "connectionId";
  paramnamesX[CONFERENCE_ID]        = "conferenceId";
  paramnamesX[CLIENT_ID]            = "clientId";
  paramnamesX[SERVER_ID]            = "serverId";
  paramnamesX[OPERATION_ID]         = "operationId";
  paramnamesX[COMMAND_TIMEOUT]      = "commandTimeout";
  paramnamesX[SESSION_TIMEOUT]      = "sessionTimeout";
  paramnamesX[IP_ADDRESS]           = "ipAddress";
  paramnamesX[PORT]                 = "port";
  paramnamesX[CONNECTION_ATTRIBUTE] = "connectionAttribute";
  paramnamesX[CONFERENCE_ATTRIBUTE] = "conferenceAttribute";
  paramnamesX[CONFEREE_ATTRIBUTE]   = "confereeAttribute";
  paramnamesX[AUDIO_FILE_ATTRIBUTE] = "audioFileAttribute";
  paramnamesX[AUDIO_TONE_ATTRIBUTE] = "audioToneAttribute";
  paramnamesX[TERMINATION_CONDITION]= "terminationCondition";
  paramnamesX[MEDIA_ELAPSED_TIME]   = "mediaElapsedTime";
  paramnamesX[FILE_NAME]            = "filename";
  paramnamesX[HEARTBEAT_INTERVAL]   = "heartbeatInterval";
  paramnamesX[HEARTBEAT_ID]         = "heartbeatId";
  paramnamesX[HEARTBEAT_PAYLOAD]    = "heartbeatPayload";
  paramnamesX[REXMIT_CONNECTION]    = "retransmit";  
  paramnamesX[MACHINE_NAME]         = "machineName";
  paramnamesX[QUEUE_NAME]           = "queueName";
  paramnamesX[CALL_STATE]           = "callState";
  paramnamesX[USER_TOKEN]           = "userToken";
  paramnamesX[HAIRPIN]              = "hairpin";
  paramnamesX[HAIRPIN_PROMOTE]      = "hairpinPromote";
  paramnamesX[CANCELED_TRANSID]     = "canceledTransID";
  paramnamesX[VOLUME]               = "volume";
  paramnamesX[SPEED]                = "speed";
  paramnamesX[ADJUST_TYPE]          = "adjustmentType";
  paramnamesX[TOGGLE_TYPE]          = "toggleType";
  paramnamesX[DIGITS]               = "digits";
  paramnamesX[EXPIRES]              = "expires";
  paramnamesX[QUERY]                = "query";
  paramnamesX[BLOCK]                = "block";
  paramnamesX[MODIFY]               = "modify";
  paramnamesX[APP_NAME]             = "appName";
  paramnamesX[LOCALE]               = "locale";
  paramnamesX[CONCURRENT]           = "concurrent";
  paramnamesX[GRAMMAR_NAME]         = "grammarName";
  paramnamesX[VR_MEANING]           = "vrMeaning";
  paramnamesX[VR_SCORE]             = "vrScore";
  paramnamesX[VOICE_BARGEIN]        = "voiceBargeIn";
  paramnamesX[ROUTING_GUID]         = "RoutingGuid";
  paramnamesX[CANCEL_ON_DIGIT]      = "cancelOnDigit";

  termcondnamesX[MAXTIME]      = "maxtime";
  termcondnamesX[DIGIT]        = "digit";
  termcondnamesX[DIGITLIST]    = "digitlist";
  termcondnamesX[MAXDIGITS]    = "maxdigits";
  termcondnamesX[SILENCE]      = "silence";
  termcondnamesX[NONSILENCE]   = "nonsilence"; 
  termcondnamesX[DIGITDELAY]   = "digitdelay"; 
  termcondnamesX[DIGITPATTERN] = "digitpattern";

  connectionAttrNamesX[CODER]               = "coder";
  connectionAttrNamesX[FRAMESIZE]           = "framesize";
  connectionAttrNamesX[VADENABLE]           = "vadenable";
  connectionAttrNamesX[CODER_TYPE_REMOTE]   = "coderTypeRemote";
  connectionAttrNamesX[FRAMESIZE_REMOTE]    = "framesizeRemote";
  connectionAttrNamesX[VADENABLE_REMOTE]    = "vadEnableRemote";
  connectionAttrNamesX[DATAFLOW_DIRECTION]  = "dataflowDirection";
  connectionAttrNamesX[CODER_TYPE_LOCAL]    = "coderTypeLocal";
  connectionAttrNamesX[FRAMESIZE_LOCAL]     = "frameSizeLocal";
  connectionAttrNamesX[VADENABLE_LOCAL]     = "vadEnableLocal";

  dataflowDirectionNamesX[IP_BIDIRECTIONAL] = "ipBidirectional";
  dataflowDirectionNamesX[IP_RECEIVEONLY]   = "ipReceiveOnly";
  dataflowDirectionNamesX[IP_SENDONLY]      = "ipSendOnly";
  dataflowDirectionNamesX[MULTICAST_SERVER] = "multicastServer";
  dataflowDirectionNamesX[MULTICAST_CLIENT] = "multicastClient";

  mediaResourceNamesX[IP_RESOURCES_INSTALLED]    = "ipResourcesInstalled";
  mediaResourceNamesX[IP_RESOURCES_AVAILABLE]    = "ipResourcesAvailable";
  mediaResourceNamesX[VOICE_RESOURCES_INSTALLED] = "voiceResourcesInstalled";
  mediaResourceNamesX[VOICE_RESOURCES_AVAILABLE] = "voiceResourcesAvailable";
  mediaResourceNamesX[CONF_RESOURCES_INSTALLED]  = "conferenceResourcesInstalled";
  mediaResourceNamesX[CONF_RESOURCES_AVAILABLE]  = "conferenceResourcesAvailable";
  mediaResourceNamesX[LOBIT_RESOURCES_INSTALLED] = "lowBitrateResourcesInstalled";
  mediaResourceNamesX[LOBIT_RESOURCES_AVAILABLE] = "lowBitrateResourcesAvailable";
  mediaResourceNamesX[TTS_RESOURCES_INSTALLED]   = "ttsResourcesInstalled";
  mediaResourceNamesX[TTS_RESOURCES_AVAILABLE]   = "ttsBitrateResourcesAvailable";
  mediaResourceNamesX[ASR_RESOURCES_INSTALLED]   = "vendorSpeechResourcesInstalled";
  mediaResourceNamesX[ASR_RESOURCES_AVAILABLE]   = "vendorSpeechResourcesAvailable";
  mediaResourceNamesX[CSP_RESOURCES_INSTALLED]   = "hmpSpeechResourcesInstalled";
  mediaResourceNamesX[CSP_RESOURCES_AVAILABLE]   = "hmpSpeechResourcesAvailable";

  serverQueryNamesX[MEDIA_RESOURCES] = "mediaResources";

  audioFileAttrNamesX[FORMAT]      = "format";
  audioFileAttrNamesX[ENCODING]    = "encoding";
  audioFileAttrNamesX[BITRATE]     = "bitrate";
  audioFileAttrNamesX[SAMPLESIZE]  = "samplesize";

  audioToneAttrNamesX[FREQUENCY1]  = "frequency1";
  audioToneAttrNamesX[AMPLITUDE1]  = "amplitude1";
  audioToneAttrNamesX[FREQUENCY2]  = "frequency2";
  audioToneAttrNamesX[AMPLITUDE2]  = "amplitude2";
  audioToneAttrNamesX[DURATION]    = "duration";

  callStateNamesX[CS_SILENCE]      = "silence";  
  callStateNamesX[CS_NONSILENCE]   = "nonsilence";
  callStateNamesX[CS_DIGITS]       = "digits";

  conferenceAttrNamesX[SOUND_TONE] = "soundTone";
  conferenceAttrNamesX[NO_TONE]    = "noTone";
  conferenceAttrNamesX[SOUND_TONE_WHEN_RECEIVE_ONLY] = "soundToneWhenReceiveOnly";
  conferenceAttrNamesX[NO_TONE_WHEN_RECEIVE_ONLY] = "noToneWhenReceiveOnly";

  confereeAtrrNamesX[MONITOR]      = "monitor";
  confereeAtrrNamesX[RECEIVE_ONLY] = "receiveOnly";
  confereeAtrrNamesX[TARIFF_TONE]  = "tariffTone";
  confereeAtrrNamesX[COACH]        = "coach";
  confereeAtrrNamesX[PUPIL]        = "pupil";
}



