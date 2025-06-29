//
// MmsProtocolAdapter.h 
//
// Base class for media server adapters
// 
#ifndef MMS_PROTOCOLADAPTER_H
#define MMS_PROTOCOLADAPTER_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif

#include "mmsCommon.h"
#include "mmsFlatMap.h"
#include "mmsConfig.h"
#include "mmsServerCmdHeader.h"
#include "mmsServerManager.h"

// Base class for media server adapters
// Implementations should handle network events as follows:
// 1. Invoke base class onProtocolDataReceived(data*) as data is received.
// 2. Handle preparseCommand(data*), to pre-parse the raw data, returning
//    the server command ID (mmsCommandTypes.h) represented by the raw data.
// 3. Handle setCommandHeader to fill in the command header object
// 4. Handle onCommandXXXXX(data*) to parse the raw data, create an MmsFlatMap
//    packaging the command and parameters, invoke mapCommit(mapwriter), and 
//    invoke postServerCommand(map*);
// 5. Handle onReturnXXXXX(map*) to inspect the return package from the server,
//    inspect return code, inspect termination conditions, retrieve returned 
//    data such as connection ID, DTMF, etc, and transmit this information on
//    to client.
//
// Adapters should be prepared for the following events:
//
// onStartAdapter()
// Invoked once the adapter has registered itself with server manager.
// Indicates server is now ready to begin receiving commands from adapter.
// 
// onStopAdapter()
// Indicates server is not accepting media commands
//
// onShutdown()
// Indicates the adapter will be shut down upon return from this event.
//
// onHeartbeat()
// A one-second pulse
//
// onCommandxxxxx, etc., e.g. onCommandConnect
// onReturnxxxxx,  etc., e.g. onReturnConnect 
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

#define MMSCLIENT_ADJTYPE_ABSOLUTE "abs"
#define MMSCLIENT_ADJTYPE_RELATIVE "rel"
#define MMSCLIENT_ADJTYPE_TOGGLE   "tog"
struct MmsLocaleParams;


class MmsIpcAdapter: public MmsBasicTask
{
  protected:

  int     onProtocolDataReceived(void* protocolData); 
  virtual int   preparseCommand (void* protocolData)=0; 
  virtual void  setCommandHeader(MmsServerCmdHeader* cmdHdr, int cmd)=0;
  virtual void  onCommandComplete(char* map); 

  virtual void  onServerCommandReturn(MmsMsg* msg);
  virtual char* mapComplete(MmsFlatMapWriter&, int commandType);
  virtual char* MmsIpcAdapter::mapComplete
      (MmsFlatMapWriter&, MmsServerCmdHeader&, int commandType);

  virtual void  onCommandConnect(void* data); 
  virtual void  onCommandDisconnect(void* data);
  virtual void  onCommandServer(void* data);
  virtual void  onCommandPlay(void* data);
  virtual void  onCommandRecord(void* data);
  virtual void  onCommandRecordTransaction(void* data);
  virtual void  onCommandPlaytone(void* data);
  virtual void  onCommandReceiveDigits(void* data);
  virtual void  onCommandSendDigits(void* data);
  virtual void  onCommandStopMediaOperation(void* data);
  virtual void  onCommandAdjustPlay(void* data);
  virtual void  onCommandAssignVolumeAdjustmentDigit(void* data);
  virtual void  onCommandAssignSpeedAdjustmentDigit(void* data);
  virtual void  onCommandAdjustVolume(void* data);  
  virtual void  onCommandAdjustSpeed(void* data);
  virtual void  onCommandClearVolumeSpeedAdjustments(void* data);
  virtual void  onCommandConferenceResourcesRemaining(void* data);
  virtual void  onCommandConfereeSetAttribute(void* data);
  virtual void  onCommandConfereeEnableVolumeControl(void* data);
  virtual void  onCommandHeartbeatAck(void* data);
  virtual void  onCommandMonitorCallState(void* data);
  virtual void  onCommandVoiceRec(void* data);

  virtual void  onReturnConnect(char* map); 
  virtual void  onReturnDisconnect(char* map); 
  virtual void  onReturnPlay(char* map);
  virtual void  onReturnRecord(char* map);
  virtual void  onReturnRecordTransaction(char* map);
  virtual void  onReturnPlaytone(char* map);
  virtual void  onReturnReceiveDigits(char* map);
  virtual void  onReturnSendDigits(char* map);
  virtual void  onReturnStopMediaOperation(char* map);
  virtual void  onReturnAdjustments(char* map);
  virtual void  onReturnConferenceResourcesRemaining(char* map);
  virtual void  onReturnConfereeSetAttribute(char* map);
  virtual void  onReturnConfereeEnableVolumeControl(char* map);
  virtual void  onReturnMonitorCallState(char* map);
  virtual void  onReturnAdjustPlay(char* map);
  virtual void  onReturnVoiceRec(char* map);

  virtual void  onInitTask();
  virtual void  onStartAdapter()=0; 
  virtual void  onStopAdapter()=0;
  virtual int   onShutdown()=0;  
  virtual void  onHeartbeat()=0;
  virtual void  onServerPush(MmsMsg* msg)=0;
  virtual void  onData(void* data)=0;
  virtual void  onAck(MmsMsg* msg);
  virtual int   onThreadStarted();
  virtual int   onError(int which);
  virtual int   close(unsigned long);

  virtual int handleMessage(MmsMsg* msg);    
                                            // Finalize and flatten map
  char* mapCommit(MmsFlatMapWriter* map, MmsServerCmdHeader* hdr); 
                                            
  int   postServerCommand(char* flatmap);   // Send command map to server

  int   postServerMessage(unsigned int type, long param=0);
                                            
  MmsServerManager* serverManager;

  void freeMapMemory(void* heapmem);

  int  getMediaFullpath(char* fullpath, char* subpath, char* basepath, 
       MmsLocaleParams&, const int isIgnoreLocale);

  void onServerCommandReturn (char* flatmap);
  void showTerminationReasons(char* flatmap);
  void showTerminationReason (char* reason);

  MmsIpcAdapter() {}
  int  ackState;
  int  m_count;
  int  m_isStarted;                         // Adapter started
  int  m_isActive;                          // Server handshaking complete
  int  m_isClientConnected;                 // Client handshaking complete 
  virtual int isConnected()  { return m_isActive && m_isClientConnected; }
  enum{ACK_INITIAL,ACK_ETC};
  MmsLogger* logger;
  MmsConfig* config; 
  MmsTask*   reporter;  

  public:                                          
  MmsIpcAdapter(MmsTask::InitialParams* params);

  virtual ~MmsIpcAdapter() {}
};


#define MAX_PARAM_VALUEPARTS     3
#define MAX_PARAM_VALUE_LENGTH 255


class MmsParameterValue
{
  // Parameter value part parse object (614)
  // Multiple values may exist in the raw value, delimited by colon characters
  
  public:
  int   nval[MAX_PARAM_VALUEPARTS];
  char* cval[MAX_PARAM_VALUEPARTS];
  int   valueCount;
  char  rawValue[MAX_PARAM_VALUE_LENGTH+1];

  MmsParameterValue(char* v) 
  { memset(this, 0, sizeof(MmsParameterValue)); 
    int n = ACE_OS::strlen(v);
    if (n > MAX_PARAM_VALUE_LENGTH) n = MAX_PARAM_VALUE_LENGTH;
    memcpy(rawValue, v, n);
    parse();
  }


  int ival(int n) 
  { 
    char*  p  = n >=0 && n < MAX_PARAM_VALUEPARTS? cval[n]: NULL;
    return p == NULL? 0: ACE_OS::atoi(p);
  }


  int dsecs(int msecs)
  {
    return (msecs / 100) + (msecs % 100 != 0); 
  }


  protected:

  void parse()
  {
    // Separate value parts delimited by colons
    char* p = rawValue, *pstart = rawValue;

    while(*p++)
    {
      if (*p == ':')
      {
          *p = '\0';              
          cval[valueCount++] = pstart;
          if  (valueCount == MAX_PARAM_VALUEPARTS) return;
          pstart  = p + 1;
      }
    }  

    cval[valueCount++] = pstart;    // Final or only value part
  }
};  



#endif
