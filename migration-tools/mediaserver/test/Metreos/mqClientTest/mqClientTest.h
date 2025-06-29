//
// mqClientTest.h
//
#ifndef MQCLIENTTEST_H
#define MQCLIENTTEST_H

#include "ace/OS.h"   
#include "ace/ACE.h"
#include "mms.h"
#include "mmsConfig.h"
#include "time.h"
#include <conio.h>
 
#include "mmsMqListener.h"
#include "mmsMqWriter.h"
#include "mmsAppMessage.h"

#define MMSTEST_LOGFILE_DIR  "C:\\Documents and Settings\\Administrator\\Desktop\\"
#define MMSTEST_LOGFILE_NAME "mqClientTest"
#define MMSTEST_LOGFILE_EXT  ".log"
#define MMSTEST_MAXLOGSIZE 4000

#define CONSOLE_TITLE_PREFIX "mmsMqClientTest"
#define INSTANCE_QUEUE_MASK  "mqClientTestQ%d"
#define MMS_MESSAGEID_NAME   "messageId"

#define TEST_APP_NAME   "MyApplication"
#define TEST_LOCALE_DIR "en-US"

#define BAIL_AFTER_N_TIMEOUTS   20
#define BAIL_AFTER_N_HEARTBEATS  5
#define DEFAULT_HEARTBEAT_INTERVAL 60

#define CLIENTTEST_INTERCOMMAND_DELAYSECONDS 1
#define USE_DEFAULT_INTERCOMMAND_DELAY     (-1)

// Numeric coder type spec "coder n", "remoteCoderType n", "localCoderType n"
// These values are those as supplied by media server provider.
// Values can be combined to indicate that if a low bitrate coder is unavailable
// the specified g711 coder should be used instead. For example 0x9 says to
// use G.729 if available, otherwise use G.711 uLaw.
#define MMS_CODERTYPE_G711ULAW 0x1
#define MMS_CODERTYPE_G711ALAW 0x2
#define MMS_CODERTYPE_G723     0x4
#define MMS_CODERTYPE_G729     0x8 

static enum testcoder               {  G711U,   G711A,   G723,   G729  }; 
static const char* coderstrings[] = { "g711u", "g711a", "g723", "g729" };
static const int   mmscoders[]    
 = { MMS_CODERTYPE_G711ULAW, MMS_CODERTYPE_G711ALAW, MMS_CODERTYPE_G723, MMS_CODERTYPE_G729 }; 

#define waitForChar() do{char c=0; printf("any key ..."); while(!c) c=_getch();} while(0)

#define TEST_ANNOUNCE_AND_GET_DIGITS      1
#define TEST_ANNOUNCE_AND_SEND_DIGITS     2
#define TEST_HALF_CONNECT                 3  
#define TEST_RECORD_PLAYBACK              4  
#define TEST_CONFERENCING_A               5 
#define TEST_CONFERENCING_B               6 
#define TEST_CONFERENCING_C               7    
#define TEST_CONFERENCING_D               8  
#define TEST_CONFERENCING_E               9  
#define TEST_CONFERENCING_F              10
#define TEST_CONFERENCING_G              11
#define TEST_CONCURRENT_COMMANDS         12
#define TEST_SERVER_QUERY_A              13
#define TEST_AUDIO_DESCRIPTOR_FILE       14
#define TEST_MULT_CONCURRENT_CONF        15
#define TEST_SETH_A                      16
#define TEST_2PARTY_NOANNOUNCE           17 
#define TEST_2PARTY_WITHANNOUNCE         18 
#define TEST_HEARTBEAT_A                 19
#define TEST_CONFERENCE_DISCO_A          20   
#define TEST_CONFEREE_SET_ATTR           21
#define TEST_STOP_MEDIA_OPERATION        22
#define TEST_PLAY_TO_CONFEREES           23
#define TEST_PLAY_TO_CONFEREES_B         24
#define TEST_PLAY_TO_CONFERENCE          25
#define TEST_RECORD_CONFERENCE           26
#define TEST_RECORD_CONFERENCE_BARGEIN   27
#define TEST_PLAY_TO_CONFERENCE_TIMEOUT  28
#define TEST_DISCONNECT_WITH_BARGEIN     29
#define TEST_PLAY_ON_HALF_CONNECT        30   
#define TEST_RECONNECT                   31
#define TEST_RECONNECT_TO_CONFERENCE     32
#define TEST_CONNECT_PARAMETER_STATE     33
#define TEST_CONFERENCE_ABANDON_RECORD   34
#define TEST_CONFERENCE_RECORD_CANCEL    35
#define TEST_CONFERENCE_RECORD_TERMINATE 36 
#define TEST_RECORD_TERMINATE_ON_DIGIT   37
#define TEST_GET_CONFEREE_DIGITS         38
#define TEST_DIGITPATTERN                39
#define TEST_CONF_REC_PROMPT_DISCO       40
#define CONSECUTIVEPLAYWITHDIGITTERM     41
#define CANCEL_DIGITPATTERN              42
#define TEST_DIGITLIST                   43
#define TEST_CALLSTATE                   44
#define TEST_DISCO_CONFEREE_WITH_BARGEIN 45
#define CONFEREE_STOPMEDIA_AND_REPLAY    46
#define FLOOD_DIGITS                     47
#define VOICE_TUNNEL_LOOP                48
#define SCHEDULED_CONFERENCE_SIM         49
#define RECORD_CONFEREE_CMD_TIMEOUT      50
#define DIGIT_PATTERN_IMMEDIATE          51
#define TEST_PLAYTONE                    52
#define TEST_PLAY_2_CONFEREE_GETDIGITS   53
#define TEST_RETRANSMIT_CONNECTION       54
#define TEST_PLAY_AND_GETDIGITS          55
#define SWITCH_IPC_ADAPTER               56
#define TEST_TTS_A                       57
#define ABANDON_PLAY_TO_LONE_CONFEREE    58
#define RECONNECT_OVER_MEDIA             59
#define CONFRECORD_WHILE_PLAYCONFEREE    60
#define TEST_MAX_CONNECTIONS             61
#define TEST_HAIRPIN_A                   62
#define TEST_SIMPLE_LBR                  63
#define TEST_DIGIT_PATTERN_C             65
#define TEST_VOLSPEED                    66
#define TEST_VOICEREC                    67
#define TEST_CONCURRENT_PLAY_GET_DIGITS  68
#define TEST_CONCURRENT_VR_GET_DIGITS    69
#define TEST_CONCURRENT_STOPMEDIA        70
#define TEST_COACH_PUPIL                 71
#define TEST_CONFSTOPMEDIA_UTILSESSION   72
#define TEST_CONF_DELETE_UTILSESSION     73
#define TEST_CONF_EMPTY_PLAY_AND_RECORD  74
#define TEST_HALF_CONNECT_LOOP           75
#define TEST_CONCURRENT_TTS              76

#define SERVER_CONNECT 1
#define SERVER_DISCO   2                                
#define CONNECT_DISCO  SERVER_CONNECT | SERVER_DISCO
#define NO_CONNECT_NO_DISCO 0

#define MMS_MQ_MAX_XMLMESSAGESIZE  2048
#define MMS_MQ_RECEIVE_MAX_PROPERTIES 4 

#define IPC_ADAPTER_MSMQ        0
#define IPC_ADAPTER_METREOS     1

int   putMessage(char* body, const int length);
void  sendServerConnect(MmsAppMessage*, int serverID=-1);
void  sendServerDisconnect(MmsAppMessage*);
void  putConnectParam(MmsAppMessage*, char* param);
void  putParam(MmsAppMessage*, const int nameindex, const char* val);
void  putParam(MmsAppMessage*, const int nameindex, const int val);
void  putDefaultLocaleParameters(MmsAppMessage*);
void  putConnectCommand(MmsAppMessage*);
void  putDiscoCommand  (MmsAppMessage*);
void  log();
void  log(char*);
char* getbuf();
FILE* openLog();
void  closeLog();
int   getMessageResultCode();
int   getMessageTransactionID();
void* getMessageClientID();
int   getMessageServerID(char** outpsid=0);
int   getMessageConnectionID(char** outpconxid=0);
int   showResultCode();
int   checkResultCode(int code1=0, int code2=0);
int   getExpectedConnectionID(char** outid=0);
int   getExpectedConferenceID(char** outid=0);
int   getExpectedOperationID (char** outid=0);
int   showReturnedPortAndIP();
int   showHeartbeatContent(const int idonly = 0);
int   showVoiceRecognitionResult();
int   sendHeartbeatAck(const int heartbeatID);
int   isHeartbeatMessage(MmsAppMessage*);
int   isProvisionalServerResponse();
char* getReturnedRecordPath();
int   getHighestConsoleNumber();  
BOOL  CALLBACK findOurMostRecentConsole(HWND, LPARAM);
DWORD getMediaServerThreadID();

int   switchIpcAdapter();  
int   listenForFlatmapIpcMessage();
                
int   heartbeatTestA(const unsigned int n = CONNECT_DISCO);
int   sethsTestA();
int   serverQuerySequenceA   (const unsigned int n = CONNECT_DISCO);
int   sendHalfConnectSequence(const unsigned int n = CONNECT_DISCO);
int   recordConfereeTestA    (const unsigned int n = CONNECT_DISCO); 
int   testAnnounceAndGetDigitsSequence (const unsigned n=CONNECT_DISCO);
int   testAnnounceAndSendDigitsSequence(const unsigned n=CONNECT_DISCO);
int   recordAudioAndPlaybackSequence   (const unsigned int n = CONNECT_DISCO);
int   sendConcurrentCommandSequence    (const unsigned int n = CONNECT_DISCO);
int   testAudioDescriptorFileSequence  (const unsigned int n = CONNECT_DISCO);
int   disconnectBargeInTestA           (const unsigned int n = CONNECT_DISCO);
int   disconnectBargeInTestB           (const unsigned int n = CONNECT_DISCO);
int   disconnectBargeInTestC           (const unsigned int n = CONNECT_DISCO);
int   announceAndJoinTwoPartyConference(); 
int   doNotAnnounceTwoPartyConference  (); 
int   stopMediaOperationTestA(const unsigned int n = CONNECT_DISCO);
int   stopMediaOperationTestC(const unsigned int n = CONNECT_DISCO);
int   confereeSetAttrA   (const unsigned int n = CONNECT_DISCO);  
int   conferenceSequenceA(const unsigned int n = CONNECT_DISCO);
int   conferenceSequenceB(const unsigned int n = CONNECT_DISCO);
int   conferenceSequenceC(const unsigned int n = CONNECT_DISCO);
int   conferenceSequenceD(const unsigned int n = CONNECT_DISCO);
int   conferenceSequenceE(const unsigned int n = CONNECT_DISCO);
int   conferenceSequenceF(const unsigned int n = CONNECT_DISCO);
int   conferenceSequenceG(const unsigned int n = CONNECT_DISCO);
int   confereeRecordWhileBusy   (const unsigned int n = CONNECT_DISCO);
int   conferenceDisconnectTestA (const unsigned int n = CONNECT_DISCO);
int   conferencePlayToConfereesA(const unsigned int n = CONNECT_DISCO);
int   conferencePlayToConfereesB(const unsigned int n = CONNECT_DISCO); 
int   conferencePlayToAll       (const unsigned int n = CONNECT_DISCO);
int   conferenceRecordAll       (const unsigned int n = CONNECT_DISCO);
int   conferenceRecordBargeIn   (const unsigned int n = CONNECT_DISCO);
int   conferenceRecordBargeInEx (const unsigned int n = CONNECT_DISCO);
int   conferenceTestPlayTimeout (const unsigned int n = CONNECT_DISCO);
int   reconnectRemoteIP         (const unsigned int n = CONNECT_DISCO);     
int   testConnectParameterState (const unsigned int n = CONNECT_DISCO);
int   reconnectRemoteIPToConference(const unsigned int n = CONNECT_DISCO);
int   attemptCommandOnHalfConnectedSession(const unsigned int n = CONNECT_DISCO);
int   conferenceAbandonRecording(const unsigned int n = CONNECT_DISCO);                                                                        
int   conferenceRecordAndCancel (const unsigned int n = CONNECT_DISCO);
int   conferenceRecordTerminationTest(const unsigned int n = CONNECT_DISCO);
int   testRecordTerminationOnDigit(const unsigned int n = CONNECT_DISCO);
int   confereeGetDigitsTest(const unsigned int n = CONNECT_DISCO); 
int   digitPatternTest(const unsigned int n = CONNECT_DISCO);
int   conferenceRecordPromptAndDisco(const unsigned int n = CONNECT_DISCO);
int   twoPlaysWithGetDigitPlusDelay (const unsigned int n = CONNECT_DISCO);
int   cancelDigitPattern(const unsigned int n = CONNECT_DISCO);
int   monitorCallState(const unsigned int n = CONNECT_DISCO);
int   digitListTest(const unsigned int n = CONNECT_DISCO);
int   confereeStopMediaAndReplay(const unsigned int n = CONNECT_DISCO);
int   digitsOverManyConnections(const unsigned int n = CONNECT_DISCO);
int   voiceTunnelLoop(const unsigned int n = CONNECT_DISCO);
int   voiceTunnelNoConference(const unsigned int n = CONNECT_DISCO);
int   scheduledConferenceSim (const unsigned int n = CONNECT_DISCO);
int   digitPatternTestB(const unsigned int n = CONNECT_DISCO);
int   digitPatternTestC(const unsigned int n = CONNECT_DISCO);
int   testPlaytone(const unsigned int n = CONNECT_DISCO);
int   playTwoToConfereeAndGetDigits(const unsigned int n = CONNECT_DISCO);
int   testRetransmitConnection(const unsigned int n = CONNECT_DISCO); 
int   playAndGetDigitsTest(const unsigned int n = CONNECT_DISCO); 
int   ttsTestA(const unsigned int n = CONNECT_DISCO);
int   abandonPlayToLoneConferee(const unsigned int n = CONNECT_DISCO);
int   reconnectOverMedia(const unsigned int n = CONNECT_DISCO);
int   conferenceRecordWhilePlayToConferee(const unsigned int n = CONNECT_DISCO);
int   testMaxConnections  (const unsigned int n = CONNECT_DISCO);
int   testMaxConnectionsB (const unsigned int n = CONNECT_DISCO);
int   testMaxConnectionsD (const unsigned int n = CONNECT_DISCO);
int   hairpinTestA(const unsigned int n = CONNECT_DISCO);
int   getDigitsTimeout(const unsigned int n = CONNECT_DISCO);
int   testVolumeSpeedControl(const unsigned int n = CONNECT_DISCO);
int   voiceRecTest(const unsigned int n = CONNECT_DISCO);
int   concurrentPlayGetDigits(const unsigned int n = CONNECT_DISCO);
int   concurrentVoiceRecGetDigits(const unsigned int n = CONNECT_DISCO);
int   stopMediaOnConcurrentOperations(const unsigned int n = CONNECT_DISCO); 
int   coachPupilTest (const unsigned int n = CONNECT_DISCO);
int   coachPupilTestB(const unsigned int n = CONNECT_DISCO);  
int   conferenceStopMediaOnUtilitySession(const unsigned int n = CONNECT_DISCO);
int   conferenceDeleteUtilitySession(const unsigned int n = CONNECT_DISCO); 
int   conferenceEmptyPlayAndRecord(const unsigned int n = CONNECT_DISCO);
int   halfConnectLoop(const unsigned int n = CONNECT_DISCO); 
int   testConcurrentTTS(const unsigned int n = CONNECT_DISCO); 

// LBR codec tests
int   simpleLBRTest(const unsigned int n = CONNECT_DISCO);
 
// int   invalidLBRParametersTest();
// int   localLBRCoderTypeMissingTest();
// int   remoteLBRCoderTypeMissingTest();
// int   bothLBRCoderTypesMissingTest();
 
    
#endif