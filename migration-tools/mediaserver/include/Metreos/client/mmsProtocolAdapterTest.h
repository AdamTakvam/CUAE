// MmsProtocolAdapterTest.h  
#ifndef MMS_PROTOCOLADAPTERTEST_H
#define MMS_PROTOCOLADAPTERTEST_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include "mmsProtocolAdapter.h"
#include "mmsParameterMap.h"
#include "mmsCommandTypes.h"
#include "mmsTestCommands.h"
#define MMS_SERVERTEST_INTERCOMMAND_PAUSE_SECONDS 1

  

class MmsTestProtocolAdapter: public MmsIpcAdapter
{
  public:
                                            // Ctor
  MmsTestProtocolAdapter(MmsTask::InitialParams* params): MmsIpcAdapter(params)
  { m_iterations = 0;
  }

  virtual ~MmsTestProtocolAdapter() {}

  protected:

  virtual void onStartAdapter();  
  virtual void onStopAdapter() { }
  virtual void onHeartbeat();
  virtual void onServerPush(MmsMsg* msg);
  virtual void onData(void*) { }
  virtual int  onShutdown()  { return 0; }
  virtual int  preparseCommand(void* protocolData);
  virtual void setCommandHeader(MmsServerCmdHeader* cmdHdr, int cmd);

  virtual void onCommandConnect(void* data);
  virtual void onCommandDisconnect(void* data);
  virtual void onCommandPlay(void* data);
  virtual void onCommandPlaytone(void* data);
  virtual void onCommandRecord(void* data);
  virtual void onCommandRecordTransaction(void* data);
  virtual void onCommandReceiveDigits(void* data);
  virtual void onCommandStopMediaOperation(void* data);
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
  virtual void onReturnStopMediaOperation(char* map);
  virtual void onReturnConferenceResourcesRemaining(char* map);
  virtual void onReturnAdjustments(char* map);
  virtual void onReturnConfereeSetAttribute(char* map);
  virtual void onReturnConfereeEnableVolumeControl(char* map);
  virtual void onReturnVoiceRec(char* map);

  void  onGenericCommand(MmsBogusProtocolData* data, int command);
  void  onGenericReturn(char* map, char* text);

  void  onTestConnect(MmsMsg* msg);
  void  onTestDisconnect(MmsMsg* msg);
  void  mapInit(MmsFlatMapWriter& map, MmsBogusProtocolData*);

  void  shutdownMediaServer();

  virtual void onCommandComplete(char* map);

  MmsTestProtocolAdapter() { }

  void buildTerminationConditionParameters
      (MmsFlatMapWriter&, MMS_BOGUS_TERMINATION_CONDITIONS&);

  void buildFileListParameters
      (MmsFlatMapWriter& map, MMS_BOGUS_FILELIST& filelist);

  unsigned int buildPlayRecordParameters
      (const int dataformat, const int datarate);

  int m_iterations;
};



#endif
