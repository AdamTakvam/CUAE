//
// mmsCommandTypes.h  
//
// Media server command identifier constants
//
#ifndef MMS_COMMANDTYPES_H
#define MMS_COMMANDTYPES_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif

#define COMMANDTYPE_CONNECT               1
#define COMMANDTYPE_DISCONNECT            2
#define COMMANDTYPE_SERVER                4   
#define COMMANDTYPE_PLAY                  5  
#define COMMANDTYPE_RECORD                6  // See IS_VOICE_MEDIA_COMMAND macro 
#define COMMANDTYPE_RECORD_TRANSACTION    7  // prior to adding commands here --
#define COMMANDTYPE_PLAYTONE              8  // 5-12 must be async voice media  
#define COMMANDTYPE_VOICEREC              9                                          
#define COMMANDTYPE_MONITOR_CALL_STATE    14  
#define COMMANDTYPE_STOP_OPERATION        15
#define COMMANDTYPE_RECEIVE_DIGITS        16 
#define COMMANDTYPE_SEND_DIGITS           17
#define COMMANDTYPE_ADJUST_PLAY           20
#define COMMANDTYPE_ASSIGN_VOLADJ_DIGIT   21
#define COMMANDTYPE_ASSIGN_SPEEDADJ_DIGIT 22
#define COMMANDTYPE_ADJUST_VOLUME         23
#define COMMANDTYPE_ADJUST_SPEED          24 
#define COMMANDTYPE_CLEAR_VS_ADJUSTMENTS  25 
#define COMMANDTYPE_CONFERENCE_RESOURCES  26 
#define COMMANDTYPE_CONFEREE_SETATTRIBUTE 27
#define COMMANDTYPE_CONFEREE_ENABLE_VOL   28
#define COMMANDTYPE_HEARTBEAT             29


#define IS_VOICE_MEDIA_COMMAND(n) \
 ((n) >= COMMANDTYPE_PLAY && (n) < COMMANDTYPE_STOP_OPERATION)

#define IS_LISTENONLY_VOICE_COMMAND(n) \
 ((n) == COMMANDTYPE_MONITOR_CALL_STATE)

#define IS_ASYNC_VOICE_COMMAND(n) \
 (IS_VOICE_MEDIA_COMMAND(n) || (n) == COMMANDTYPE_RECEIVE_DIGITS)

#define IS_INLINE_COMMAND(n) \
 (((n) == COMMANDTYPE_STOP_OPERATION) || (n) == COMMANDTYPE_SEND_DIGITS || \
  ((n) == COMMANDTYPE_ADJUST_PLAY))

#define IS_CONNECTION_COMMAND(n) \
 ((n) >= COMMANDTYPE_CONNECT && (n) < COMMANDTYPE_SERVER)

// A metreos-defined voice termination condition. These are defined here  
// only because the header is included on both adapater and server sides.
#define DX_METREOS_DIGPATTERN 15
#define TM_METREOS_AUTOSTOP   0x00400  // see dialogic/inc/dxtables.h
#define TM_METREOS_DIGPATTERN 0x00800  // see dialogic/inc/dxtables.h
#define TM_METREOS_TIMEOUT    0x01000  // see dialogic/inc/dxtables.h

#endif
