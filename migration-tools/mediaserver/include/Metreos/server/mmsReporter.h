//
// mmsReporter.h 
//
// Alarms and Statistics interface 
//
#ifndef MMS_REPORTER_H
#define MMS_REPORTER_H

#ifdef  MMS_WINPLATFORM
#pragma once
#endif
#pragma warning(disable:4786)

#include "mms.h"
#include "mmsTask.h"
#include "mmsConfig.h"
#include "mmsFlatMap.h"
#include "mmsServerCmdHeader.h"

#include "Flatmap.h"
#include "ipc/headerExtension.h"
#include "ipc/FlatmapIpcClient.h"
#define MMSM_REPORTER_LOG_SERVERDISCO MMSM_USER+0x20
#define MMSM_REPORTER_LOG_SOCKETERR   MMSM_USER+0x21

class MmsReporterIpcClient;



class MmsReporter: public MmsBasicTask 
{
  protected:
  MmsConfig* config;
  MmsTask*   logger;
  MmsReporterIpcClient* ipcClient;
  void onStartReporter();
  void onStopReporter();
  int  isConnected();
  int  isInitialized() { return this->isinitialized; }
  int  connectAsync();
  int  connectSync(const int always=0);

  virtual int handleMessage(MmsMsg*);

  virtual int onThreadStarted();            // Thread startup hook

  virtual int close(unsigned long);         // Thread exit hook
                     
  int onAlarmRequest(MmsAlarmParams*);      // Publish alarm                       
  int onAlarmRequest(MmsMsg*);              // Publish alarm
  int onStatsRequest(MmsMsg*);              // Publish stat
  int onCommand     (MmsMsg*);              // Handle other external commands
  int clearAlarm(MmsAlarmParams*);          // Clear alarm if set
  int isAlarmSet(const int index);          // Query if set
  int isStartupStatsPublished;

  int initializeAlarmState();
  int handleAlarmStateOnResourceChange(MmsStatsParams* params);

  int monitorAlarmStateForResource(MmsStatsParams*, const int hiwater, const int lowater, 
      const int hiwaterAlarmIndex, const int lowaterAlarmIndex, const int noMoreAlarmIndex);

  int generateAlarm(const int msgID, const int alarmIndex, 
      const bool isBurningResource, MmsStatsParams*);
                                            // Callback from foreign code
  static void reporterCallback(void* datastruct);
  static MmsConfig*   configx;
  static MmsReporter* singleton;            // To do: formalize singleton
  static MmsTask*     thisx;                // Static copy of this

  struct alarmState                         // An entry in the alarm instance table
  {    
    int  alarmType;          // from enum alarmTypes
    int  statCategory;       // from enum statCategories
    unsigned instanceID;     // stat server "guid": nonzero indicates alarm active
    ACE_Thread_Mutex* lock;  // table entry lock
    int  isAlarmActive() { return instanceID != 0; }
    void clearAlarm()    { instanceID = 0; }
    void setAlarm()      { instanceID = Mms::getTickCount(); }
    void clear() { memset(this,0,sizeof(struct alarmState)); }
  };

  int   publishStats (MmsStatsParams*);
  int   raiseAlarm   (MmsAlarmParams*);
  int   transmitAlarm(Metreos::FlatMapWriter&, const int isClearingAlarm=FALSE);
  int   transmitStats(Metreos::FlatMapWriter&);
  void  buildAlarmMap(MmsAlarmParams*, struct alarmState*, Metreos::FlatMapWriter&);
  void  buildStatsMap(MmsStatsParams*, Metreos::FlatMapWriter&);
  int   timestampText(char* buf, time_t stamp); 
  char* mediaTypeText(const int);

  public:
                                             
  MmsReporter(MmsTask::InitialParams* params);
  virtual ~MmsReporter();
  static MmsReporter* instance() { return singleton; }
  static void destroy();
  static void waitOnConnectLock();
  static int isConnecting, isDisconnecting;
  unsigned getUniqueTick();
  void publishStartupShutdownStats();
  int  isShutdownRequest;
  int  isinitialized;
  int  isEnabled;
  ACE_thread_t listenerThread;
  static ACE_Thread_Mutex* connectLock;

  enum statsServerIpcMessageIDs
  {
    // Correspond to app server IStats.StatsListener.Messages constants
    MMSM_SET_ALARM     = 1000,
    MMSM_CLEAR_ALARM   = 1001,
    MMSM_SET_STATISTIC = 1100,
  };

  enum alarmParamIDs    
  {
    // Correspond to app server IStats.StatsListener.Messages.Fields constants
    MMSP_ALARM_TYPE= 2001,
    MMSP_SEVERITY  = 2002,
    MMSP_TEXT      = 2003,
    MMSP_TIMESTAMP = 2004,
    MMSP_GUID      = 2005,
  };

  enum statsParamIDs
  {
    // Correspond to app server IStats.StatsListener.Messages.Fields constants
    MMSP_STATS_TYPE  = 2100,
    MMSP_STATS_VALUE = 2101
  };

 enum severityTypeValues 
  { // These correspond to app server red, yellow, green
    severityTypeSevere = 1, 
    severityTypeCaution= 2,
    severityTypeNormal = 3,  
  };


  enum alarmTypes
  {
    MMS_SERVER_COMPROMISED       = 200,
    MMS_UNEXPECTED_CONDITION     = 201,
    MMS_UNSCHEDULED_SHUTDOWN     = 202,
    MMS_RESX_NOT_DEPLOYED        = 203,     // Not on this server, e.g. no voicerec

    MMS_NO_MORE_CONNECTIONS      = 210,     // Out of G711
    MMS_HIWATER_CONNECTIONS      = 211,
    MMS_LOWATER_CONNECTIONS      = 212,
     
    MMS_NO_MORE_VOX              = 220,     // Out of voice resources
    MMS_HIWATER_VOX              = 221,
    MMS_LOWATER_VOX              = 222,

    MMS_NO_MORE_G729_ETC         = 230,     // Out of low bitrate resources
    MMS_HIWATER_G729_ETC         = 231,
    MMS_LOWATER_G729_ETC         = 232, 

    MMS_NO_MORE_CONFRESX_IN_SVC  = 240,     // Out of conference resources on server
    MMS_HIWATER_CONFRESX_IN_SVC  = 241,
    MMS_LOWATER_CONFRESX_IN_SVC  = 242, 

    MMS_NO_MORE_SLOTS_IN_CONF    = 243,     // Out of conferee slots in conference
    MMS_HIWATER_SLOTS_IN_CONF    = 244,
    MMS_LOWATER_SLOTS_IN_CONF    = 245,  

    MMS_NO_MORE_CONFERENCES      = 246,     // Out of conferences on this server
    MMS_HIWATER_CONFERENCES      = 247,
    MMS_LOWATER_CONFERENCES      = 248,

    MMS_NO_MORE_TTS_PORTS_FAILS  = 250,     // Out of TTS ports - request rejected
    MMS_NO_MORE_TTS_PORTS_QUEUES = 251,     // Out of TTS ports - request queued on TTS server
    MMS_HIWATER_TTS_PORTS        = 252,
    MMS_LOWATER_TTS_PORTS        = 253, 

    MMS_NO_MORE_ASR_RESX         = 260,     // Out of ASR ports - request rejected
    MMS_HIWATER_ASR_RESX         = 261,
    MMS_LOWATER_ASR_RESX         = 262,  
  };


  enum alarmTypesIndexes
  {
    NDX_SERVER_COMPROMISED       = 0,
    NDX_UNEXPECTED_CONDITION     = 1,
    NDX_UNSCHEDULED_SHUTDOWN     = 2,
    NDX_RESX_NOT_DEPLOYED        = 3,       // Not on this server, e.g. no voicerec

    NDX_NO_MORE_CONNECTIONS      = 4,       // Out of G711
    NDX_HIWATER_CONNECTIONS      = 5,
    NDX_LOWATER_CONNECTIONS      = 6,
     
    NDX_NO_MORE_VOX              = 7,       // Out of voice resources
    NDX_HIWATER_VOX              = 8,
    NDX_LOWATER_VOX              = 9,

    NDX_NO_MORE_G729_ETC         = 10,      // Out of low bitrate resources
    NDX_HIWATER_G729_ETC         = 11,
    NDX_LOWATER_G729_ETC         = 12, 

    NDX_NO_MORE_CONFRESX_IN_SVC  = 13,      // Out of conference resources on server
    NDX_HIWATER_CONFRESX_IN_SVC  = 14,
    NDX_LOWATER_CONFRESX_IN_SVC  = 15, 

    NDX_NO_MORE_SLOTS_IN_CONF    = 16,      // Out of conferee slots in conference
    NDX_HIWATER_SLOTS_IN_CONF    = 17,
    NDX_LOWATER_SLOTS_IN_CONF    = 18, 

    NDX_NO_MORE_CONFERENCES      = 19,      // Out of conferences on this server
    NDX_HIWATER_CONFERENCES      = 20,
    NDX_LOWATER_CONFERENCES      = 21, 

    NDX_NO_MORE_TTS_PORTS_FAILS  = 22,      // Out of TTS ports - request rejected
    NDX_NO_MORE_TTS_PORTS_QUEUES = 23,      // Out of TTS ports - request queued on TTS server
    NDX_HIWATER_TTS_PORTS        = 24,
    NDX_LOWATER_TTS_PORTS        = 25, 

    NDX_NO_MORE_ASR_RESX         = 26,      // Out of ASR ports - request rejected
    NDX_HIWATER_ASR_RESX         = 27,
    NDX_LOWATER_ASR_RESX         = 28,  

    NDX_ALARM_TYPES_COUNT        = 29       // Number of entries in this enumeration
  };

  static void raiseServerAlarm(const int alarmType, char* msg=0);
  void setShutdown();

  protected:
  struct alarmState  alarmStates[NDX_ALARM_TYPES_COUNT];
  ACE_Thread_Mutex   alarmStatesTableLock;
  void cleanup();

  struct alarmState* findAlarmByAlarmTypeIndex(const int index);
}; 



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
// MmsReporterIpcClient: stat server IPC client
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 

class MmsReporterIpcClient: public Metreos::IPC::FlatMapIpcClient
{
  public:
  MmsReporterIpcClient(MmsReporter*);
  virtual void OnIncomingFlatMapMessage(const int msgType, const Metreos::FlatMapReader&);
  virtual void OnDisconnected();
	virtual void OnFailure();

  int   initiateConnectToStatServer(const int port, char* ip);
  int   blockingConnectToStatServer(const int port, char* ip);
  int   postIpcMessage(const int msg, Metreos::FlatMapWriter&);
  int   disconnectFromStatServer(); 
  int   isshutdown, isconnected, isreconnect, consecutiveConnectLocks, consecutiveConnectFailures;
  int   statServerPort;
  char* statServerIP;
  MmsReporter* reporter;

  enum 
  { INITIAL_CONNECT_ATTEMPTS = 3,      // Attempts on startup
    WAITMS_BETWEEN_CONNECTS  = 1000, 
    WAITMS_AFTER_CONNECT     = 750,          
    WAITMS_AFTER_CONNECTFAIL = 1500,
    MAX_CONNECT_ATTEMPTS     = 3,      // Attempts on incoming transactions
    MAX_CONNECT_LOCKOUTS     = 16,
  };

  private:
  static ACE_THR_FUNC_RETURN ConnectThread(void*);
};


#endif  // #ifndef MMS_REPORTER_H
