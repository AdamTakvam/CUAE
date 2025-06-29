//
// MmsMqAppAdapter.h   
//
// xml based protocol adapter 
//
#ifndef MMS_MQAPPADAPTER_H
#define MMS_MQAPPADAPTER_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif

#include "mmsProtocolAdapter.h"
#include "mmsParameterMap.h"
#include "mmsCommandTypes.h"

#include "mmsAppMessage.h"
#include "mmsMqListener.h"
#include "mmsMqWriter.h"
  

class MmsMqAppAdapter: public MmsIpcAdapter
{
  public:
                                            // Ctor
  MmsMqAppAdapter(MmsTask::InitialParams* params);
  virtual ~MmsMqAppAdapter();
                        
  virtual int  turnMqMessageAround(const int rc, const int reason, const int isdeletexml=TRUE) = 0;  
  virtual void turnServerConnectMessageAround(const int, const int,const int isdeletexml=TRUE) = 0;

  virtual void postClientsHeartbeat() = 0;      // Send pulse to all clients
  virtual void pushServerData(MmsMsg* msg) = 0; // Push server data to client

  protected:
  unsigned int heartbeats;                  // Heartbeat count
  unsigned int isExpectingClientToken;      // Client ID token passing off/on
  MmsAppMessage* xmlmsg;                    // Inbound xml

  int getHeartbeatIntervalParam(MmsAppMessage*);
  int getHeartbeatPayloadParam (MmsAppMessage*);

  virtual void onHeartbeat(); 
  virtual void onServerPush(MmsMsg* msg);
  virtual void onData(void* data);
  virtual int  preparseCommand(void* protocolData);
  virtual void setCommandHeader(MmsServerCmdHeader* cmdHdr, int cmd);

  virtual void onCommandConnect(void* data);
  virtual void onCommandDisconnect(void* data);
  virtual void onCommandPlay(void* data);
  virtual void onCommandPlaytone(void* data);
  virtual void onCommandRecord(void* data);
  virtual void onCommandRecordTransaction(void* data);
  virtual void onCommandReceiveDigits(void* data);
  virtual void onCommandSendDigits(void* protocolData);
  virtual void onCommandStopMediaOperation(void* data);
  virtual void onCommandMonitorCallState(void* data);
  virtual void onCommandAdjustPlay(void* data);
  virtual void onCommandConferenceResourcesRemaining(void* data); 
  virtual void onCommandAssignVolumeAdjustmentDigit(void* data);
  virtual void onCommandAssignSpeedAdjustmentDigit(void* data);
  virtual void onCommandAdjustVolume(void* data); 
  virtual void onCommandAdjustSpeed(void* data);
  virtual void onCommandClearVolumeSpeedAdjustments(void* data); 
  virtual void onCommandConfereeSetAttribute(void* data);
  virtual void onCommandConfereeEnableVolumeControl(void* data);
  virtual void onCommandVoiceRec(void* data);

  virtual void onReturnConnect(char* map); 
  virtual void onReturnDisconnect(char* map);
  virtual void onReturnPlay(char* map);
  virtual void onReturnPlaytone(char* map);
  virtual void onReturnRecord(char* map);
  virtual void onReturnRecordTransaction(char* map);
  virtual void onReturnReceiveDigits(char* map);
  virtual void onReturnSendDigits(char* map);
  virtual void onReturnStopMediaOperation(char* map);
  virtual void onReturnConferenceResourcesRemaining(char* map);
  virtual void onReturnAdjustments(char* map);
  virtual void onReturnConfereeSetAttribute(char* map);
  virtual void onReturnConfereeEnableVolumeControl(char* map);
  virtual void onReturnMonitorCallState(char* map);
  virtual void onReturnAdjustPlay(char* map);
  virtual void onReturnVoiceRec(char* map);

  void  onGenericCommand(void* data, int command);
  void  onGenericReturn (char* map,  int command);

  void  shutdownSelf();
  void  shutdownMediaServer();

  virtual int registerClient(int which, MmsAppMessage* xmlmsg=0) = 0;

  virtual int getServerIdFromClientId(const int serverID) = 0;

  virtual int onError(int whicherror);

  MmsMqAppAdapter() { }

  MmsAppMessage* writeStandardClientMessageContent
      (char* flatmap, const int commandno);

  virtual int postClientReturnMessage(MmsAppMessage**, char* flatmap) = 0;

  void writeTerminationReasons(MmsAppMessage* xmlmsg, char* flatmap);
  void assignFilename(MmsFlatMapWriter& filespecMap);
  void insertFilename(MmsAppMessage* outxml, char* flatmap);
  int  isTtsRequest(char* path, char* ext, const int extlen, MmsLocaleParams&);
  void formatQueryMediaResources(MmsAppMessage* outxml);
  void formatAvailableMediaResources(MmsAppMessage* outxml);

  int  buildFileListParameters            (MmsFlatMapWriter&, MmsAppMessage*, MmsLocaleParams&, int cmd);
  int  buildAdjustPlayParameters          (MmsFlatMapWriter&, MmsAppMessage*, const int reqd=0);
  int  buildConnectionParameters          (MmsFlatMapWriter&, MmsAppMessage*);
  int  buildTerminationConditionParameters(MmsFlatMapWriter&, MmsAppMessage*);
  int  buildConferenceParameters          (MmsFlatMapWriter&, MmsAppMessage*);
  int  buildCallStateParameters           (MmsFlatMapWriter&, MmsAppMessage*);
  int  buildConferenceAttributeParameters (MmsFlatMapWriter&, MmsAppMessage*,
       unsigned int* flags=NULL); 
  int  buildConfereeAttributeParameters   (MmsFlatMapWriter&, MmsAppMessage*, 
       unsigned int* flags=NULL);
  int  buildConfereeAttributeParameter    (MmsFlatMapWriter&, MmsAppMessage*); 
  int  buildAudioFileAttributeParameters  (MmsFlatMapWriter&, MmsAppMessage*);
  int  buildAudioToneAttributeParameters  (MmsFlatMapWriter&, MmsAppMessage*);
  int  buildGrammarListParameters         (MmsFlatMapWriter&, MmsAppMessage*);
  int  buildLocaleDirectoryParameters     (MmsFlatMapWriter&, MmsAppMessage*, MmsLocaleParams&);
  virtual int buildCommonParameters       (MmsFlatMapWriter&, MmsAppMessage*, 
       MmsServerCmdHeader& cmdHeader, unsigned int* flags=NULL) = 0;
  int  editVolumeSpeedParameter(MmsAppMessage* xmlmsg, char* bufpos, int* paramval);
  int  isExistingMediaFile(char* fn, MmsLocaleParams&);
  int  editNumericCoderType(char* coder, int& codertype, unsigned int& altcoder, const int isLocal=0);
  int  stripServerID (const int id, MmsServerCmdHeader& commandHeader);
  int  insertServerID(const int id, const int serverID);
  int  insertServerIdExcludeZero(const int id, const int serverID);
  int  insertServerID(char* flatmap);
  int  logCommand(const int command, const char* cmd, const ACE_Log_Priority pri=LM_DEBUG);

  static int isZeros(char* sz);

  enum parsebitflags{EDIT_ERROR = 1, CONFERENCEID_PRESENT = 2}; 
};

#define RECORDED_FILENAME_RETURN_LENGTH (8 + 1 + 3 + 1)

#endif // #ifndef MMS_MQAPPADAPTER_H
