//
// mmsParameterMap.h  
// parameter flatmap declarations
//
#ifndef MMS_PARAMETERMAP_H
#define MMS_PARAMETERMAP_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif


static void initMapParameter(char* p, int size, char fill=0, int term=0)
{
  memset(p, fill, size);
  if  (term)
     *(p + (size - 1)) = '\0';
}

#define isFlatmapOutputParameter(n) (n<512)


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Server command parameter IDs (flatmap keys): these must be unique values
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                                                      // Output parameters
#define MMSP_TIMEOUT_REMAINING_MS                14   // Ticks left on timer 
#define MMSP_TERMINATION_REASON_BITMASK          15   // Termination reason(s)
#define MMSP_RECEIVE_DIGITS_RETURN_BUFFER        16   // Returned DTMF digits

                                                      // Begin input parameters
#define MMSP_CONNECTION_ID                      512   // Connection ID  
#define MMSP_OPERATION_ID                       513   // Operation ID  
#define MMSP_CALL_ID                            514   // Call ID
#define MMSP_REMOTE_CONX_ATTRIBUTES             515   // bitmap
#define MMSP_LOCAL_CONX_ATTRIBUTES              516   // bitmap
#define MMSP_PORT_NUMBER                        517   // int
#define MMSP_IP_ADDRESS                         518   // string
#define MMSP_CONFERENCE_ID                      519   // int
#define MMSP_ALTERNATE_CODER                    520   // int
#define MMSP_SESSION_TIMEOUT_SECS               521   // int
#define MMSP_COMMAND_TIMEOUT_MS                 522   // int
#define MMSP_CONFERENCE_ATTRIBUTES              530   // int (bitmap)
#define MMSP_CONFEREE_ATTRIBUTES                531   // int (bitmap)
#define MMSP_HAIRPIN                            532   // int
#define MMSP_HAIRPIN_PROMOTE                    533   // int
#define MMSP_PLAY_RECORD_ATTRIBUTES             539   // int (bitmap)
#define MMSP_TERMINATION_CONDITIONS             540   // flatmap
#define MMSP_TERMINATION_CONDITION              541   // flatmap
#define MMSP_TERMINATION_CONDITION_TYPE         542   // int 
#define MMSP_TERMINATION_CONDITION_LENGTH       543   // int   
#define MMSP_TERMINATION_CONDITION_DATA1        544   // int   
#define MMSP_TERMINATION_CONDITION_DATA2        545   // int 
#define MMSP_EXPIRES                            548   // int 
#define MMSP_DIGITLIST                          549   // string
#define MMSP_FILELIST                           550   // flatmap
#define MMSP_FILECOUNT                          551   // int
#define MMSP_FILESPEC                           552   // flatmap
#define MMSP_FILENAME                           553   // string
#define MMSP_FILEOFFSET                         554   // int
#define MMSP_FILELENGTH                         555   // int
#define MMSP_FILENAME_IS_TTSTEXT                556   // int   
#define MMSP_FILENAME_IS_ASSIGNED               557   // int   
#define MMSP_FREQUENCY_AMPLITUDE                560   // flatmap
#define MMSP_FREQUENCY                          561   // int
#define MMSP_AMPLITUDE                          562   // int
#define MMSP_DURATION                           563   // int
#define MMSP_VOLSPEED_DIGIT                     564   // int
#define MMSP_VOLSPEED_ADJUSTMENT                565   // int
#define MMSP_CONFERENCE_VOLCONTROL              566   // int
#define MMSP_CALL_STATE                         570   // int   
#define MMSP_CALL_STATE_DURATION                571   // int
#define MMSP_VOLUME                             575   // int
#define MMSP_SPEED                              576   // int
#define MMSP_ADJUSTMENT_TYPE                    577   // int
#define MMSP_TOGGLE_TYPE                        578   // int
#define MMSP_BLOCK                              579   // int
#define MMSP_MODIFY                             580   // int
#define MMSP_GRAMMARNAME                        581   // string
#define MMSP_GRAMMARSPEC                        582   // flatmap
#define MMSP_GRAMMARLIST                        583   // flatmap
#define MMSP_VR_MEANING                         584   // string
#define MMSP_VR_SCORE                           585   // int
#define MMSP_VOICE_BARGEIN                      586   // int
#define MMSP_ROUTING_GUID                       587   // string
#define MMSP_CANCEL_ON_DIGIT                    588   // int
#define MMSP_APP_NAME                           591   // string
#define MMSP_LOCALE                             592   // string

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Termination condition types
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
#define TERMCONDITION_TIMEOUT       1       // len = u100ms; flag = TF_MAXTIME
#define TERMCONDITION_MAXDIGITS     2       // l=n, f=TF_MAXDTMF
#define TERMCONDITION_DIGIT         3       // l=DG_DTMF<<8|digit;f=TF_DIGTYPE
#define TERMCONDITION_MAXSILENCE    4       // l=100ms,f=TF_MAXSIL
#define TERMCONDITION_MAXNONSILENCE 5       // f=TF_MAXNOSIL


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Lengths of certain entries inserted into a flatmap to hold return values
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

#define MMS_SIZEOF_TIMEOUT_REMAINING_MS                8   // asciiz
#define MMS_SIZEOF_TERMINATION_REASON_BITMASK          4   // 32 bits
#define MMS_SIZEOF_RECEIVE_DIGITS_RETURN_BUFFER       32   // 31 asciiz digits
#define MMS_SIZEOF_UUIDID                             37   // 36 + 1
#define MMS_SIZEOF_IPADDRESS                          16   // 15 + 1
#define MMS_SIZEOF_VR_MEANING                         512  // 512 bytes character array

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Constants for parameter values, bit flags, etc.
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

// Numeric coder type spec "coder n", "remoteCoderType n", "localCoderType n"
// These can be combined to indicate that if a low bitrate coder is unavailable
// the specified g711 coder should be used instead. For example 0x9 says to
// uer G.729 if available, otherwise use G.711 uLaw.
#define MMS_CODERTYPE_G711ULAW 0x1
#define MMS_CODERTYPE_G711ALAW 0x2
#define MMS_CODERTYPE_G723     0x4
#define MMS_CODERTYPE_G729     0x8 

// MMSP_REMOTE_CONX_ATTRIBUTES and MMSP_LOCAL_CONX_ATTRIBUTES
// 32 bits specifying coder type, frame size, frames per packet, 
// coder payload type, redundancy payload type, etc.

// MMSP_xxxxxx_CONX_ATTRIBUTES: codec type
// ---------------------------------------
#define MMS_CODER_G711ALAW64K              0x1
#define MMS_CODER_G711ULAW64K              0x2
#define MMS_CODER_G7231_5_3K               0x4
#define MMS_CODER_G7231_6_3K               0x8
#define MMS_CODER_G726_32K                0x10
#define MMS_CODER_G729                    0x20
#define MMS_CODER_G729ANNEXA              0x40
#define MMS_CODER_G729ANNEXB              0x80
#define MMS_CODER_G729ANNEXAB            0x100
#define MMS_CODER_GSMFULLRATE            0x200
#define MMS_GET_CODER_BITS(m)   (m & 0x3ff)
#define MMS_CLEAR_CODER_BITS(m) (m &= ~(0x3ff))

// MMSP_xxxxxx_CONX_ATTRIBUTES: frame size
// ---------------------------------------
// G.711 coders will specify framesize as 10, 20, or 30. with frames per packet
// fixed at 1 in HMP. G.723 and G.729 have framesize fixed at 30 in HMP, with
// frames per packet varying; however media server API will continue to use
// framesize as the variable on the front end, and translate this to frames
// per packet for HMP. We will use the Cisco CallManager expectations in this
// regard; G.723 framesizes 30 and 60 become fpp fpp 2 and 3; G.729 framesizes
// 20, 30, 40 become fpp 2, 3, 4. Since we use 3 bits for framesize, we will 
// define their meanings differently for each coder. 
#define MMS_CODER_FRAMESIZE_20          0x800
#define MMS_CODER_FRAMESIZE_30          0x1000
#define MMS_CODER_OTHER                 0x400

#define MMS_CODER_G711_FRAMESIZE_10     MMS_CODER_OTHER    
#define MMS_CODER_G711_FRAMESIZE_20     MMS_CODER_FRAMESIZE_20
#define MMS_CODER_G711_FRAMESIZE_30     MMS_CODER_FRAMESIZE_30

#define MMS_CODER_G723_FRAMESIZE_30     MMS_CODER_FRAMESIZE_30  
#define MMS_CODER_G723_FRAMESIZE_60     MMS_CODER_OTHER

#define MMS_CODER_G729_FRAMESIZE_20     MMS_CODER_FRAMESIZE_20    
#define MMS_CODER_G729_FRAMESIZE_30     MMS_CODER_FRAMESIZE_30
#define MMS_CODER_G729_FRAMESIZE_40     MMS_CODER_OTHER

#define MMS_GET_FRAMESIZE_BITS(m)   (m & 0x1c00)
#define MMS_CLEAR_FRAMESIZE_BITS(m) (m &= ~(0x1c00))

// MMSP_xxxxxx_CONX_ATTRIBUTES: VAD
// --------------------------------
#define MMS_CODER_VAD_ENABLE            0x2000
#define MMS_CLEAR_VAD_ENABLE(m)  (m &= ~0x2000)

// MMSP_xxxxxx_CONX_ATTRIBUTES: frames/pkt
// ---------------------------------------
// Not applicable to G.711 coders. We recognize 4 frames/packet values. 
#define MMS_CODER_FPP_1                 0x4000
#define MMS_CODER_FPP_2                 0x8000
#define MMS_CODER_FPP_3                0x10000
#define MMS_CODER_FPP_4                0x20000
#define MMS_GET_FPP_BITS(m)   (m & 0x3c000)
#define MMS_CLEAR_FPP_BITS(m) (m &= ~(0x3c000))

// MMSP_xxxxxx_CONX_ATTRIBUTES: Coder payload type
// -----------------------------------------------
// Notes: 7 bits masked into 01FC0000
#define MMS_CODER_PAYLOAD_TYPE_PUT(m,v) (m |= ((v & 0x7f) << 18))
#define MMS_CODER_PAYLOAD_TYPE_GET(m) ((m & 0x1fc0000) >> 18)

// MMSP_xxxxxx_CONX_ATTRIBUTES: Redundancy payload type 
// ----------------------------------------------------
// Notes: 7 bits masked into FE000000 
// Alert: Do not use this parameter yet - see notes following
#define MMS_REDUNDANCY_PAYLOAD_TYPE_PUT(m,v) (m |= ((v & 0x7f) << 25))
#define MMS_REDUNDANCY_PAYLOAD_TYPE_GET(m) ((m & 0xfe000000) >> 25)

// MMSP_xxxxxx_CONX_ATTRIBUTES: Data flow direction 
// ----------------------------------------------------
// Notes: 3 bits masked into 0E000000
// Alert: We initially had only one data flow direction. Now we have 5.
// Rather than define and code an extra parameter map entry, we're
// using these bits previously reserved for redundancy payload type,
// since we currently have no use for that. If at some point we have a
// use for redundancy payload type, we should probably define an extra 
// parameter in which to pass data flow direction.
// 0111 changed constants to begin at 1 rather than zero, in order that
// we can identify the case where data flow direction is not specified
#define MMS_DATAFLOW_DIRECTION_PUT(m,v) (m |= ((v & 0x7) << 25))
#define MMS_DATAFLOW_DIRECTION_GET(m)  ((m & 0xe000000) >> 25)
#define MMS_DATAFLOW_DIRECTION_CLEAR(m) (m &= ~(0xe000000))

   
#define MMS_DIRECTION_IPBI 1
#define MMS_DIRECTION_IPRO 2
#define MMS_DIRECTION_IPSO 3
#define MMS_DIRECTION_MCS  4
#define MMS_DIRECTION_MCC  5



// MMSP_PLAY_RECORD_ATTRIBUTES: data format and bit rate
// -----------------------------------------------------
#define MMS_FORMAT_MULAW             0x1   
#define MMS_FORMAT_ALAW              0x2   
#define MMS_FORMAT_PCM               0x4   
#define MMS_FORMAT_ADPCM             0x8 
#define MMS_SAMPLESIZE_BIT_4         0x10  
#define MMS_SAMPLESIZE_BIT_8         0x20  
#define MMS_SAMPLESIZE_BIT_16        0x40  
#define MMS_SAMPLESIZE_UNDEF         0x80  
#define MMS_RATE_KHZ_6               0x100
#define MMS_RATE_KHZ_8               0x200
#define MMS_RATE_KHZ_11              0x400
#define MMS_FILETYPE_VOX             0x1000
#define MMS_FILETYPE_WAV             0x2000
#define MMS_PLAYTONE                 0x4000
#define MMS_GAINCONTROL_NONE         0x8000
#define MMS_GET_FORMAT_BITS(m)       (m & 0xf)  
#define MMS_GET_SAMPLESIZE_BITS(m)   (m & 0xf0)
#define MMS_GET_RATE_BITS(m)         (m & 0x700)

#define MMS_PLAY_DEFAULT_TONE    0x10000
#define MMS_PLAY_USER_TONE       0x20000


// MMSP_TERMINATION_CONDITION_TYPE 
// -------------------------------
#define MMS_TERM_TYPE_MAX_DIGITS       1
#define MMS_TERM_SPECIFIC_DIGIT        2
#define MMS_TERM_TYPE_TIMEOUT          3
#define MMS_TERM_TYPE_MAX_SILENCE      4
#define MMS_TERM_TYPE_MAX_NONSILENCE   5


// MMSP_VOLSPEED_DIGIT 
// -------------------
// Note that the signed 16 bit value is maintained in the low 16 bits
// of this 32-bit value, and the sign extension is cleared to make way
// for the adjustment digit. 
#define MMS_PUT_ADJUSTMENT_VALUE(m,v) do{m |=(short)v;m &= 0xffff;}while(0)
#define MMS_PUT_ADJUSTMENT_DIGIT(m,v) (m |= (v << 16))
#define MMS_GET_ADJUSTMENT_VALUE(m)   (m & 0xffff)
#define MMS_GET_ADJUSTMENT_DIGIT(m)   (char)((m & 0xff0000) >> 16)
#define MMS_GET_ADJUSTMENT_ACTION(m)  MMS_GET_ADJUSTMENT_DIGIT(m)


// MMSP_VOLSPEED_ADJUSTMENT 
// ------------------------
// Note that we'll use MMS_PUT_ADJUSTMENT_VALUE, MMS_GET_ADJUSTMENT_VALUE
// to operate on the low 16 bits of on this 32-bit value as well. 
// These bitflags are in the high 16 bits.
#define MMS_ADJUST_TO_ABSOLUTE_POSITION 0x10000  
#define MMS_ADJUST_TO_RELATIVE_POSITION 0x20000  
#define MMS_ADJUST_TO_TOGGLE_POSITION   0x40000  
#define MMS_TOGGLE_ORIGIN_TO_PRIOR     0x100000
#define MMS_RESET_TO_ORIGIN            0x200000
#define MMS_SET_TO_PRIOR               0x400000
#define MMS_RESET_ALL_TO_ORIGIN        0x800000


// MMSP_CONFERENCE_ATTRIBUTES
// --------------------------
#define MMS_CONFERENCE_SOUNDTONE            0x1
#define MMS_CONFERENCE_RCVONLY_NOTONE       0x2


// MMSP_CONFEREE_ATTRIBUTES
// ------------------------
#define MMS_CONFEREE_RECEIVE_ONLY           0x1
#define MMS_CONFEREE_TARIFF_TONE            0x2
#define MMS_CONFEREE_COACH                  0x4
#define MMS_CONFEREE_PUPIL                  0x8
#define MMS_CONFEREE_MONITOR               0x10
#define MMS_CONFEREE_ATTRIBUTE_OFF       0x1000


// MMSP_CONFERENCE_VOLCONTROL 
// --------------------------
#define MMS_SET_ONOROFF(m,v)        (m |=  (v & 0xff))
#define MMS_SET_VOLUP_DIGIT(m,v)    (m |= ((v & 0xff) << 8))
#define MMS_SET_VOLRESET_DIGIT(m,v) (m |= ((v & 0xff) << 16))
#define MMS_SET_VOLDOWN_DIGIT(m,v)  (m |= ((v & 0xff) << 24))
#define MMS_GET_ONOROFF(m)          (m & 0xff)
#define MMS_GET_VOLUP_DIGIT(m)     ((m & 0xff00) >> 8)
#define MMS_GET_VOLRESET_DIGIT(m)  ((m & 0xff0000) >> 16)
#define MMS_GET_VOLDOWN_DIGIT(m)   ((m & 0xff000000) >> 24)


// MMSP_CALL_STATE     
// ---------------
#define MMS_CALL_STATE_SILENCE    1
#define MMS_CALL_STATE_NONSILENCE 2
#define MMS_CALL_STATE_DIGITS     3

#define MMS_OMIT_EXTENSION (-1)

    
// MMSP_ADJUSTMENT_TYPE     
// --------------------
#define MMS_ADJTYPE_NONE     0
#define MMS_ADJTYPE_ABSOLUTE 1
#define MMS_ADJTYPE_RELATIVE 2
#define MMS_ADJTYPE_TOGGLE   3


// MMSP_TOGGLE_TYPE     
// --------------------
#define MMS_TOGTYPE_ORG_CUR   0
#define MMS_TOGTYPE_TO_CURORG 1
#define MMS_TOGTYPE_LASTMOD   2
#define MMS_TOGTYPE_RESETORG  3

#endif
