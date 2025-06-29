// 
// mmsConfig.h 
//
// Configuration objects, reader and parser
// 
// Both server and client have a MmsConfig instance, a reference to which is 
// given to various objects in each. The MmsConfig value object initializes
// itself with default values, and reads its config file on command. Various
// client listener interfaces may send a command to reload the config file.
// Client will recognize the command first, and reload its config value object.
// Client then messages server commanding server to reload its config object.
//
// MmsConfig represents single-valued properties as members of objects, or
// as standalone items. MmsConfig intitializes all items with default values,
// and on demand, reads a standard properties file to set any of those values,
// which could look like:
//
// # This is an example MMS properties file
// Server.maxServicePoolThreads = 256    
// ServerLogger.numThreads = 2
// foobar = "who cares"      # standalone item
//
// Note that properties file interpretation is case-insensitive
//     
#ifndef MMS_CONFIG_H
#define MMS_CONFIG_H
#ifdef  MMS_WINPLATFORM
#pragma once
#endif
#include "mms.h"
#include "ace/Log_Priority.h"
#include "mmsParameterMap.h"
#include <map>
#include <string>

#define MAXCONFIGLINESIZE 255
#define MMS_CONFIG_FILE_PATH "c:/Program Files/Cisco System/Unified Communications Environment/MediaEngine/mmsconfig.properties"
#define MMS_REGCONFIGPATH "ConfigPath"
#define MMS_HMP_LICENSED_RESOURCES_LOC  \
  "Software\\Dialogic\\Installed Boards\\DM3\\HMP_Software #0 in slot 0/65535"
#define MMS_HMP_LICENSED_RESOURCES_SUBKEY "PCDFileDesc"
#define MMS_NO_DEVKEY  ""                   // Value specifying empty devkey 
#define MMS_MAX_DEVKEYSIZE         15       // Max size of a developer override key
#define MMS_MAX_PLAYLIST_SIZE       6
#define IPCADAPTER_MAXMAPSIZE       1 
#define CISCO_LICENSING_MODE_NORMAL 0
#define MMS_MAX_LICENSE_HEADROOM_PERCENT 25 // Maximum permitted license overage


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Configuration value symbolic constants
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
#define MMS_USE_HMP_MAXCONNECTIONS    65535 // Means default to max conx
#define MMS_USE_MAX_IP_RESOURCES          0 // Means default to IP resx count 
#define MMS_USE_MAX_VOICE_RESOURCES       0 // Means default to voice resx count
#define MMS_USE_MAX_CONF_RESOURCES        0 // Means default to conf resx count 
#define MMS_USE_THREADPOOL_DEFAULT        0 // Means to use default formula 
#define MMS_STRATEGY_IDLED_MOST_RECENTLY  0 // Select device idled most recent
#define MMS_STRATEGY_IDLE_LONGEST         1 // Select device idled the longest
#define MMS_STRATEGY_PENDING_CMD_DISABLE  0 // Do not permit pending commands
#define MMS_STRATEGY_PENDING_CMD_ENABLE   1 // Handle pending commands


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Media defaults
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
#define MMS_RECORD_FILE_EXPIRATION_DAYS  10 // How long to keep recording
#define MMS_CONFERENCE_NOTIFY_ON_JOIN     1 // Sound tone on party join
#define MMS_CONFERENCE_NOTONE_RECEIVEONLY 1 // Do not sound tone if party r/o 
#define MMS_ACTIVE_TALKERS_ENABLED        0 // Enable active talkers monitoring
#define MMS_UTILITY_POOLSIZE_FACTOR       2 // Index to percentage of max conx 
#define MMS_MEDIA_CODER                   MMS_CODERTYPE_G711ULAW 
#define MMS_MEDIA_REMOTE_CODER            MMS_MEDIA_CODER  
#define MMS_MEDIA_REMOTE_CODER_TYPE      "g711ulaw64k"
#define MMS_MEDIA_REMOTE_CODER_FRAMESIZE  0
#define MMS_MEDIA_REMOTE_CODER_VAD_ENABLE 0
#define MMS_MEDIA_LOCAL_CODER_TYPE       "g711ulaw64k"
#define MMS_MEDIA_LOCAL_CODER_FRAMESIZE  30
#define MMS_MEDIA_LOCAL_CODER_VAD_ENABLE  0
#define MMS_MEDIA_G723_KBPS               "5.3"
#define MMS_MEDIA_G729_TYPE               "a"
#define MMS_MEDIA_AGC_DISABLE_IP          0 // Disable gain control at IP
#define MMS_MEDIA_AGC_DISABLE_VOX         0 // Disable gain control at vox
#define MMS_MEDIA_AGC_DISABLE_CONFEREE    0 // Disable gain control at conf
#define MMS_MEDIA_RFC2833_ENABLE          0 // Enable RFC2833 digit xfer mode
#define MMS_MEDIA_TONECLAMP_DISABLE       0 // Disable tone clamping
#define MMS_MEDIA_VERIFY_RTCP_MOD2        0 // Verify RTCP port modulo 2
#define MMS_MEDIA_DEFAULT_TONE_FREQ     600 // Default frequency for playtone

#define MMS_MEDIA_TTS_ENABLE              1 // Enable TTS 1/0
#define MMS_MEDIA_TTS_ENGINE             "neospeech"
#define MMS_MEDIA_TTS_SERVER_IP          "localhost"
#define MMS_MEDIA_TTS_SERVER_PORT         1314
#define MMS_MEDIA_TTS_VOICE               1 // Default voice
#define MMS_MEDIA_TTS_VOLUME              0 // Volume, zero default
#define MMS_MEDIA_TTS_VOICE_RATE          0 // Speak speed -10 <= rate <= +10
#define MMS_MEDIA_TTS_QUALITY_KHZ         0 // Voice quality KHz  
#define MMS_MEDIA_TTS_QUALITY_BITS        8 // Voice quality bit resolution 16/8  
#define MMS_MEDIA_TTS_ISPATH_STRATEGY     0 // 0 = use file system; 1 = dot + n
#define MMS_MEDIA_TTS_FILE_EXPIRE_DAYS    0 // File exp: 0 = delete after play
#define MMS_MEDIA_TTS_VALIDATE_VNDCONFIG  1 // Validate vendor config file 0/1

#define MMS_MEDIA_ASR_ENABLE              0 // Enable ASR 1/0
#define MMS_MEDIA_ASR_ENGINE             "speechworks"

#define MMS_MEDIA_PCM_SAMPLE_SIZE         8 // PCM sample size 8 (default) or 16
#define MMS_MEDIA_VOICE_BARGEIN_THRESHOLD -30 // CSP channel voice bargein threshold

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// HMP defaults
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// READ THIS: max connections etc. for an HMP license are specified in the 
// config file, not here in the code. These are fail-safe defaults only.

#define MMS_HMP_MAXCONNECTIONS_DEFAULT      MMS_USE_HMP_MAXCONNECTIONS
#define MMS_HMP_MAX_INITIAL_CONF_RESOURCES  MMS_USE_MAX_CONF_RESOURCES
#define MMS_HMP_MAX_INITIAL_IP_RESOURCES    MMS_USE_MAX_IP_RESOURCES 
#define MMS_HMP_MAX_INITIAL_VOICE_RESOURCES MMS_USE_MAX_IP_RESOURCES 
#define MMS_HMP_START_SVC_MAXWAITSECS 60    // Max wait to start HMP service
#define MMS_HMP_DEFAULT_VOLUME 0            // Zero = HMP default - do not change 
#define MMS_HMP_DEFAULT_SPEED  0            // Zero = HMP default - do not change 
#define MMS_HMP_RTP_PORT_BASE  20480        // HMP RTP port base, 0 to 65536, HMP default is 41952
#define MMS_HMP_INTERNAL_LICENSING_MODE 0   // License mode -- 0 = normal
#define MMS_HMP_CISCO_DEVKEY MMS_NO_DEVKEY  // Developer key for overrides  
#define MMS_HMP_LICENSE_HEADROOM_PERCENT 0  // Headroom over and above license limit
#define MMS_HMP_DISABLE_LICENSE_MODE 0      // Switch to turn off license mode limits 
#define MMS_HMP_DISALLOW_LICENSE_HEADROOM 1 // Switch to turn off headroom  
#define MMS_HMP_ASSUME_SDK_ON_LICENSE_FAIL 0// Assume SDK mode if no license server   
#define MMS_HMP_LOG_MAX_RESOURCES 1         // Log resource counts at startup 
#define MMS_HMP_SET_DSCP_EXPEDITE_FORWARD 1 // Set EF value (0xb8) in TCP dscp/tos byte 1/0 
       
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Server configuration defaults
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Constant MMS_USE_HMP_MAXCONNECTIONS used for MMS_MAX_CONNECTION_THREADS
// means that the server should build a connection pool whose number of 
// threads is equal to the number of HMP connections licensed, as specified 
// in the config file. MMS_SERVICE_THREADPOOL_SIZE_FACTOR range is 1-4, and
// represents the percentage of max connections determining the size of the
// service thread pool in 1/8 increments, where 1 represents 2/8, and 4 
// represents 5/8. The default is 1 (2/8 or 25% of max connections). Pool 
// size is always at least 4 threads.

#define MMS_SERVICE_THREADPOOL_SIZE_FACTOR     1      // Range 1-4, 25% to 62.5%
#define MMS_SESSION_MONITOR_INTERVAL_SECONDS   5
#define MMS_THREAD_MONITOR_INTERVAL_SECONDS   10 
#define MMS_CONNECT_ASYNC                      1
#define MMS_WAIT_FOR_VOICE_RESOURCE_SECONDS    3  
#define MMS_WAIT_FOR_VOICE_RESOURCE_MSECS      0  
#define MMS_REASSIGN_IDLE_VOICE_RESOURCES      1
#define MMS_POST_PROVISIONAL_RESULT            1
#define MMS_SET_UNHANDLED_EXCEPTION_TRAP       1
#define MMS_CRASH_SERVER_ON_VOICE_EVENT        0
#define MMS_SUPPRESS_LOG_MSG_RECEIPT           1
#define MMS_OVERWRITE_DUMP_FILE                1
#define MMS_REQUIRE_CONCURRENT_OP_CONFIRMATION 1      // Concurrent op must be explicit
#define MMS_CLEAN_LOGS_AFTER_DAYS              3 
#define MMS_POINTER_VALIDATION_LEVEL           1      // Pointer dereference check
#define MMS_CACHE_BUSY_DISCONNECT              1      // Cache disco when session busy
#define MMS_MAX_SECONDS_THREAD_NONRESPONSE     60     // 0 = no server timeout
#define MMS_DEFAULT_MONCALLSTATE_DURATION_MS   2000   // 2 secs  
#define MMS_EVENT_WAIT_FOR_DEPENDENCY_MS       2000   
#define MMS_HEARTBEAT_INTERVAL_MSECS           1000
#define MMS_RESX_DETAIL_LOG_INTERVAL_MSECS     3000   // How often to log resx detail
#define MMS_SYS_DETAIL_LOG_INTERVAL_MSECS      60000  // How often to log system stats
#define MMS_MAX_DIRECTORY_SCAN_COUNT           50     // How many dirs to scan for audio
#define MMS_SESSION_TIMEOUT_SECONDS_DEFAULT    180    // 3 mins inactivity
#define MMS_COMMAND_TIMEOUT_MSECS_DEFAULT      60000  // 1 min
#define MMS_COMMAND_TIMEOUT_MSECS_CONNECT      10000  // 10 secs
#define MMS_COMMAND_TIMEOUT_MSECS_PLAY         60000   
#define MMS_COMMAND_TIMEOUT_MSECS_RECORD       60000   
#define MMS_COMMAND_TIMEOUT_MSECS_RECORD_TRANS 300000 // 5 mins
#define MMS_COMMAND_TIMEOUT_MSECS_MONCALLSTATE 30000  // 30 secs
#define MMS_COMMAND_TIMEOUT_MSECS_GET_DIGITS   90000  // 90 secs 
#define MMS_COMMAND_TIMEOUT_MSECS_VOICEREC     90000  // 90 secs
#define MMS_DEFAULT_MAX_SILENCE_SECS_PLAY      5
#define MMS_DEFAULT_MAX_SILENCE_SECS_RECORD    10
#define MMS_DEFAULT_MAX_DELAY_SECS_RECEIVE     15
#define MMS_DEFAULT_MAX_SECS_TONE              10
#define MMS_CLEAN_DIRECTORIES_INTERVAL_MINUTES 240
#define MMS_DIGIT_SEQUENCE_INTERVAL_SECONDS    2
#define MMS_IDLE_DEVICE_SELECTION_STRATEGY     MMS_STRATEGY_IDLE_LONGEST
#define MMS_PENDING_COMMAND_STRATEGY           MMS_STRATEGY_PENDING_CMD_ENABLE 
#define MMS_INACTIVE_CONFEREE_TIMEOUT_MINUTES  0
#define MMS_INACTIVE_CONFEREE_TIMEOUT_STRATEGY 0
#define MMS_SESSION_AVAILABLE_AGE_THRESHOLD_MS 1000   // Session available if free for n ms
#define MMS_DEFAULT_FLATMAP_IPC_PORT           9530 

#define MMS_DEFAULT_HAIRPIN_OPTS               0
#define MMS_HAIRPIN_OFF_UNLESS_OVERRIDE        0
#define MMS_HAIRPIN_ON_UNLESS_OVERRIDE         1
#define MMS_HAIRPIN_NEVER                      2

#define MMS_DEFAULT_HAIRPIN_PROMOTE            1
#define MMS_HPIN_PROMOTE_OFF_UNLESS_OVERRIDE   0
#define MMS_HPIN_PROMOTE_ON_UNLESS_OVERRIDE    1  
#define MMS_HPIN_DEMOTE_ON_UNLESS_OVERRIDE     2 
#define MMS_HPIN_PROMOTE_NEVER                 4
#define MMS_HPIN_DEMOTE_NEVER                  8 

#define MMS_DISREGARD_LOCALE_DIRECTORIES       0 
#define MMS_DEFAULT_APPNAME                   ""
#define MMS_DEFAULT_LOCALE                    ""                             
 
#define MMS_AUDIO_BASEPATH        "c:\\Program Files\\Cisco Systems\\Unified Application Environment\\MediaServer\\audio"
#define MMS_DUMP_BASEPATH         "c:\\Program Files\\Cisco Systems\\Unified Application Environment\\MediaServer\\dump"

#define MMS_DRIVE_MAPPING_LIST    ""

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Bitflags indicating available IPC adapters
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

#define MGCP_START                        0x00000001
#define MGCP_METREOS                      0x00000001
#define MGCP_END                          0x00000010
#define SIP_START                         0x00000010
#define SIP_END                           0x00000100
#define VOICEXML_START                    0x00000100
#define VOICEXML_END                      0x00000200
#define SALT_START                        0x00000200
#define SALT_END                          0x00000400
#define H248_START                        0x00000400
#define H248_END                          0x00000800
#define MEGACO_START                      0x00000800
#define MEGACO_END                        0x00001000
#define HTTP_START                        0x00100000
#define HTTP_METREOS_SERVER_CONTROL       0x00100000
#define HTTP_END                          0x00400000
#define SNMP_START                        0x00400000
#define SNMP_METREOS_SERVER_CONTROL       0x00400000
#define SNMP_END                          0x00800000
#define MSMQ_START                        0x00800000
#define MSMQ_METREOS                      0x00800000
#define MSMQ_END                          0x01000000
#define GUI_START                         0x01000000
#define GUI_METREOS_SERVERCONTROL_WINDOWS 0x01000000
#define GUI_END                           0x08000000
#define FLATMAP_IPC_START                 0x08000000
#define FLATMAP_IPC                       0x08000000
#define FLATMAP_IPC_END                   0x10000000
#define SERVER_TEST_ADAPTER_A             0x10000000

// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Logger configuration default values
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

#define MMS_LOGGER_DEFAULT_MSGLEVEL       LM_DEBUG
#define MMS_LOGGER_DEFAULT_NUMTHREADS     1

#define MMS_SERVER_LOGGER_NUMTHREADS      2 // How many threads handle logging
#define MMS_SERVER_LOGGER_TIMESTAMP       1 // Timestamp the file log output?
#define MMS_SERVER_LOGGER_DEST_STDOUT     1 // Send log output to stdout?
#define MMS_SERVER_LOGGER_DEST_DEBUG      0 // Send log output to debugger
#define MMS_SERVER_LOGGER_DEST_LOCALFILE  1 // Send log output to file?
#define MMS_SERVER_LOGGER_DEST_SOCKET     0 // Send log output over socket
#define MMS_SERVER_LOGGER_LOGFILEPATH    "c:\\Program Files\\Cisco Systems\\Unified Application Environment\\MediaServer\\logs\\mms.log"
#define MMS_SERVER_LOGGER_ISFULLPATH      1 
#define MMS_SERVER_LOGGER_BACKUP          1 // Backup log on boot?
#define MMS_SERVER_LOGGER_FLUSH           0 // Flush after write?
#define MMS_SERVER_LOGGER_MAXLINES     4000 // Max size of log; 0=no maximum
#define MMS_SERVER_LOGGER_SOCKCON_SECS   30 // Attempt socket connect every n
#define MMS_SERVER_LOGGER_DEST_LOGSERVER	0	// Send log to log server


#define MMS_CLIENT_LOGGER_NUMTHREADS      1
#define MMS_CLIENT_LOGGER_TIMESTAMP       1
#define MMS_CLIENT_LOGGER_DEST_STDOUT     1
#define MMS_CLIENT_LOGGER_DEST_DEBUG      1
#define MMS_CLIENT_LOGGER_DEST_LOCALFILE  1
#define MMS_CLIENT_LOGGER_LOGFILEPATH     "c:\\mmsclient.log"
#define MMS_CLIENT_LOGGER_ISFULLPATH      1


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Client configuration default values
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                                           
#ifdef  MMS_LINK_WITH_MSMQ                  // Bitwise-OR of adapters to be loaded
#define MMS_INITIAL_ADAPTERS_MASK  MSMQ_METREOS | GUI_METREOS_SERVERCONTROL_WINDOWS | FLATMAP_IPC 
#else
#define MMS_INITIAL_ADAPTERS_MASK  FLATMAP_IPC  | GUI_METREOS_SERVERCONTROL_WINDOWS
#endif

#define MMS_MAX_MQMESSAGE_AGE_SECS       60 // Age after which msg is discarded
#define MMS_MSMQ_TIMEOUT_MSECS         3000 // Wakeup interval (3 secs)
#define MMS_CLIENT_HEARTBEAT_SECS        10 // How often to send client heartbeat
#define MMS_PROTOCOL_MAX_MAPSIZE       4096 // Max size of a parameter flatmap  
#define MMS_RESPOND_ON_ASYNC_EXECUTE      0 // Provisional return msg desired 1/0 
#define MMS_PERMIT_DEFAULT_APP_QUEUE      1 // If not specified on connect
#define MMS_ACKNOWLEDGE_HEARTBEAT         1 // Server expects heartbeat ack 1/0
#define MMS_MAX_MISSING_HEARTBEAT_ACKS    0 // Consecutive missed acks before disco
#define MMS_MSMQ_APP_MACHINE_NAME  "jld"  
#define MMS_MSMQ_MMS_MACHINE_NAME  "jld"
#define MMS_MSMQ_APP_QUEUE_NAME    "mmsTestQueue"
#define MMS_MSMQ_MMS_QUEUE_NAME    "mmsLocalQueue"


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Reports (alarms and stats) configuration default values
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

#define MMS_REPORTS_DISABLE_ALARMS        0 // Set to 1 to default to alarms disabled
#define MMS_REPORTS_DISABLE_STATS         0 // Set to 1 to default to stats disabled
#define MMS_REPORTS_ALARM_THRESHOLD       0
#define MMS_REPORTS_HIWATER_G711          0
#define MMS_REPORTS_LOWATER_G711          0
#define MMS_REPORTS_HIWATER_G729_ETC      0
#define MMS_REPORTS_LOWATER_G729_ETC      0
#define MMS_REPORTS_HIWATER_VOICE         0
#define MMS_REPORTS_LOWATER_VOICE         0
#define MMS_REPORTS_HIWATER_CONFERENCE    0
#define MMS_REPORTS_LOWATER_CONFERENCE    0
#define MMS_REPORTS_HIWATER_CONFERENCES   0
#define MMS_REPORTS_LOWATER_CONFERENCES   0
#define MMS_REPORTS_HIWATER_TTS           0
#define MMS_REPORTS_LOWATER_TTS           0
#define MMS_REPORTS_HIWATER_SPEECHREC     0
#define MMS_REPORTS_LOWATER_SPEECHREC     0
#define MMS_REPORTS_HIWATER_SPEECHINT     0
#define MMS_REPORTS_LOWATER_SPEECHINT     0
#define MMS_REPORTS_MONITOR_CONX_SECONDS  180  

#define MMS_REPORTS_STAT_SERVER_IP       "localhost"
#define MMS_REPORTS_STAT_SERVER_PORT      9201


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Diagnostics bitflags
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// diagnostics.flags and diagnostics.flagsB entries are used for engineering
// troubleshooting, and are interpreted as hexadecimal representations of 
// 32-bit doublewords. Following are the individual bits comprising these 
// flags, each of which which must be a unique log2 value. The config file 
// entry value will be an external hex string representing the sum of the  
// flag values to be set. For example if the values of flags to be set are  
// 0x100, 0x8, and 0x2, the config entry would be as follows:
// diagnostics.flags = 10A
 
#define MMS_DIAG_LOG_CONNECT_ATTRS      0x1 // Log coder attributes   
#define MMS_DIAG_LOG_PLAYREC_ATTRS      0x2 // Log file/data formats
#define MMS_DIAG_LOG_TERMCONDITIONS     0x4 // Log effective term conds
#define MMS_DIAG_LOG_EVENT_TABLE        0x8 // Log registered events
#define MMS_DIAG_LOG_DEVICE_DETAIL     0x10 // Log state of each media device  
#define MMS_DIAG_LOG_SESSION_DETAIL    0x20 // Log state of each session
#define MMS_DIAG_LOG_TIMESLOTS         0x40 // Log bus timeslot assignments
#define MMS_DIAG_LOG_LISTENS           0x80 // Log listen/unlisten
#define MMS_DIAG_LOG_CDT              0x100 // Log conference descriptor table
#define MMS_DIAG_LOG_CST              0x200 // Log call state transitions 
#define MMS_DIAG_LOG_DIGBUF           0x400 // Log digit buffer state 
#define MMS_DIAG_LOG_MAP_LIFETIME     0x800 // Log param map assign and free 
#define MMS_DIAG_LOG_CONFX_ATTRS     0x1000 // Log conference/conferee attrs
#define MMS_DIAG_LOG_VOLSPEED        0x2000 // Log volume speed adjustments
#define MMS_DIAG_LOG_OPTABLE         0x4000 // Log op table in the session
#define MMS_DIAG_LOG_RESXUSE         0x8000 // Log resource usage
#define MMS_DIAG_LOG_FILELIST       0x10000 // Log play/record files


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// MMS configuration value object
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
                                    
class MmsConfig                              
{ 
  public:
  enum g723types { MMS_G723_53, MMS_G723_63 }; // HMP G.723 varieties
  enum g729types { MMS_G729_A,  MMS_G729_AB }; // HMP G.729 varieties

  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  // Object descriptions -- for entries with "object.item = value" signature
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

  struct Logger
  {
    int  defaultMessageLevel;               // Message priority assumed as default
    int  globalMessageLevel;                // Current message priority level
    int  numThreads;                        // Number of threads for log print
    int  timestamp;                         // Timestamp file logrecs boolean
    int  destStdout;                        // Boolean log to stdout
    int  destDebug;                         // Boolean log to debugger
    int  destFile;                          // Boolean log to localfile  
    int  destSocket;                        // Boolean log to socket
    int  destLogServer;                     // Boolean log to log server
    int  destOther;                         // Other MmsLogDestination::flags
    int  isFullpath;                        // Boolean: is specified path complete
    int  backup;                            // Boolean: back up log at boot
    int  flush;                             // Boolean: flush after each write
    int  maxlines;                          // Maximum lines before cycling
    int  port;                              // Required when destSocket set
    int  socketConnectIntervalSeconds;      // Connect attempt every n seconds
    char filepath[MAXPATHLEN];              // Local file path or partial path

    Logger()
    {
      memset(this, 0, sizeof(Logger));
      defaultMessageLevel = MMS_LOGGER_DEFAULT_MSGLEVEL;
      globalMessageLevel  = MMS_LOGGER_DEFAULT_MSGLEVEL;
      numThreads          = MMS_LOGGER_DEFAULT_NUMTHREADS;
    }
  };


  struct ServerLogger: public Logger        // Media server logger
  {
    ServerLogger(): Logger() 
    {
      memset(this, 0, sizeof(ServerLogger));
      numThreads     = MMS_SERVER_LOGGER_NUMTHREADS;
      timestamp      = MMS_SERVER_LOGGER_TIMESTAMP;
      destStdout     = MMS_SERVER_LOGGER_DEST_STDOUT;
      destDebug      = MMS_SERVER_LOGGER_DEST_DEBUG;
      destFile       = MMS_SERVER_LOGGER_DEST_LOCALFILE;
      destSocket     = MMS_SERVER_LOGGER_DEST_SOCKET;
	    destLogServer	 = MMS_SERVER_LOGGER_DEST_LOGSERVER;
      isFullpath     = MMS_SERVER_LOGGER_ISFULLPATH;
      backup         = MMS_SERVER_LOGGER_BACKUP;
      flush          = MMS_SERVER_LOGGER_FLUSH;
      maxlines       = MMS_SERVER_LOGGER_MAXLINES;
      socketConnectIntervalSeconds = MMS_SERVER_LOGGER_SOCKCON_SECS;
      strcpy(filepath, MMS_SERVER_LOGGER_LOGFILEPATH); 
    }
  };


  struct ClientLogger: public Logger        // Media server client process logger
  {
    ClientLogger(): Logger() 
    {
      numThreads     = MMS_CLIENT_LOGGER_NUMTHREADS;
      timestamp      = MMS_CLIENT_LOGGER_TIMESTAMP;
      destStdout     = MMS_CLIENT_LOGGER_DEST_STDOUT;
      destDebug      = MMS_CLIENT_LOGGER_DEST_DEBUG;
      destFile       = MMS_CLIENT_LOGGER_DEST_LOCALFILE;
      isFullpath     = MMS_CLIENT_LOGGER_ISFULLPATH;
      strcpy(filepath, MMS_CLIENT_LOGGER_LOGFILEPATH);
    }
  };



  struct Diagnostics                        // Diagnostics configuration
  {
    int logResourceStatus;                  // Verbose post-command status
    int shutdownAtHearbeat;                 // If > 0 server runs for n beats
    int debugBreakAtAllocation;             // Crt mem alloc number to break at
    int logOutboundMessages;                // Write outbound messages to n files 
    unsigned int flags;                     // Bitflags
    unsigned int flagsB;                    // Bitflags

    Diagnostics()
    {
      shutdownAtHearbeat = 0;               // Remove in production  
      logOutboundMessages = 0;              // Off 
      logResourceStatus  = 0;               // Off
      debugBreakAtAllocation = 0;           // Off
      flags = flagsB = 0;                   // Set via config only 
    }
  };



  struct Client                             // Media server client adapters 
  {
    int  heartbeatIntervalSecs;             // How often to send heartbeat
    unsigned int ipcAdapters;               // Adapters to be loaded at boot

    int  serverTestAdapterIterations;       // # of x to loop tests; 0 = forever   
    int  maxMqMessageAgeSecs;               // Discard messages elder than this
    int  msmqTimeoutMsecs;                  // Wakeup interval
    int  respondOnAsyncExecute;             // Return msg on async execute desired 1/0
    int  permitDefaultAppQueue;             // If not specified on connect 1/0
    int  heartbeatAckExpected;              // Server expects heartbeat ack 1/0
    int  heartbeatAcksMissable;             // Beats missed before client disconnect

    char msmqAppMachineName[64];
    char msmqMmsMachineName[64];
    char msmqAppQueueName[64];
    char msmqMmsQueueName[64];
  
    Client()
    {          
      memset(this, 0, sizeof(Client));         
      heartbeatIntervalSecs   = MMS_CLIENT_HEARTBEAT_SECS;
      ipcAdapters             = MMS_INITIAL_ADAPTERS_MASK;

      serverTestAdapterIterations = 1;  
      maxMqMessageAgeSecs     = MMS_MAX_MQMESSAGE_AGE_SECS; 
      msmqTimeoutMsecs        = MMS_MSMQ_TIMEOUT_MSECS;  
      respondOnAsyncExecute   = MMS_RESPOND_ON_ASYNC_EXECUTE; 
      permitDefaultAppQueue   = MMS_PERMIT_DEFAULT_APP_QUEUE; 

      heartbeatAckExpected    = MMS_ACKNOWLEDGE_HEARTBEAT;
      heartbeatAcksMissable   = MMS_MAX_MISSING_HEARTBEAT_ACKS;

      ACE_OS::strcpy(msmqAppMachineName, MMS_MSMQ_APP_MACHINE_NAME);
      ACE_OS::strcpy(msmqMmsMachineName, MMS_MSMQ_MMS_MACHINE_NAME);
      ACE_OS::strcpy(msmqAppQueueName,   MMS_MSMQ_APP_QUEUE_NAME);
      ACE_OS::strcpy(msmqMmsQueueName,   MMS_MSMQ_MMS_QUEUE_NAME);
    }
  };


  struct Media                              // Media properties
  {
    int  recordFileExpirationDays;          // How long to keep a recording
    int  conferenceNotifyOnJoin;            // Default booleans - tone on join
    int  conferenceNoToneReceiveOnly;       // No tone for receive only parties
    int  conferenceActiveTalkersEnabled;    // Active talkers monitored 1/0
    int  utilityPoolSizeFactor;             // Index of percentage of max conx
    int  enhancedRTP;                       // Low bitrate avail count override
    int  g723kbps_n;                        // Enumeration of g723 types
    int  g729type_n;                        // Enumeration of g729 types
    char g723kbps[8];                       // G.723 kbps "5.3" or "6.3"
    char g729type[4];                       // G.729 annex "", "a", or "ab"
             
    int  coder;
    int  framesize;
    int  vadEnable;
    int  remoteCoder;
    int  remoteCoderFramesize;
    int  remoteCoderVadEnable;
    char remoteCoderType[32];
    char localCoderType [32];
    int  localCoder;
    int  localCoderFramesize;
    int  localCoderVadEnable;
    int  agcDisableIP;                      // Disable gain control at IP 
    int  agcDisableVox;                     // Disable gain control on record
    int  agcDisableConferee;                // Disable gain control at conferee
    int  toneClampDisable;
    int  rfc2833Enable;
    int  verifyRemotePortMod2;
    int  defaultToneFrequency;
    int  pcmSampleSize;                      

    int  ttsEnable;                         // Enable TTS 1/0 
    char ttsEngine[16];
    char ttsServerIP[16];
    int  ttsServerPort;    
    int  ttsQualityKHz;                      
    int  ttsQualityBits;                    // 16 or 8
    int  ttsVoiceRate;                      // Speak rate, determining wpm
    int  ttsVolume;                         // Gain of wav output
    int  ttsVoice;                          // Which voice to speak 
    int  ttsExpireDays;                     // How long to retain wave file
    int  ttsIsPathStrategy;                 // How we determine if file path  
    int  ttsValidateVendorConfig;           // Validate vendor config file boolean

    int  voiceBargeinThreshold;             // CSP channel voice bargein threshold
    int  asrEnable;                         // Enable ASR 1/0
    char asrEngine[16];                     // ASR engine name

    int setLowBitrateCoderTypes()
    {
      // Converts G.723 and G.729 subtypes as specified in config file 
      // to internal enumerations
      this->g723kbps_n = strcmp(this->g723kbps,"6.3") == 0? MMS_G723_63: MMS_G723_53;
      this->g729type_n = strcmp(this->g729type, "ab") == 0? MMS_G729_AB: MMS_G729_A;
      return 0;
    }


    Media()
    {
      memset(this, 0, sizeof(Media));
      recordFileExpirationDays     = MMS_RECORD_FILE_EXPIRATION_DAYS;
      conferenceNotifyOnJoin       = MMS_CONFERENCE_NOTIFY_ON_JOIN;              
      conferenceNoToneReceiveOnly  = MMS_CONFERENCE_NOTONE_RECEIVEONLY; 
      conferenceActiveTalkersEnabled = MMS_ACTIVE_TALKERS_ENABLED;
      utilityPoolSizeFactor        = MMS_UTILITY_POOLSIZE_FACTOR;
      enhancedRTP                  = 0;

      ACE_OS::strcpy(remoteCoderType,  MMS_MEDIA_REMOTE_CODER_TYPE);
      remoteCoderFramesize = MMS_MEDIA_REMOTE_CODER_FRAMESIZE ;
      remoteCoderVadEnable = MMS_MEDIA_REMOTE_CODER_VAD_ENABLE; 
      defaultToneFrequency = MMS_MEDIA_DEFAULT_TONE_FREQ;
      ACE_OS::strcpy(g723kbps, MMS_MEDIA_G723_KBPS);
      ACE_OS::strcpy(g729type, MMS_MEDIA_G729_TYPE);
      this->setLowBitrateCoderTypes();
 
      #if 0
      ACE_OS::strcpy(localCoderType,   MMS_MEDIA_LOCAL_CODER_TYPE);
      localCoderFramesize  = MMS_MEDIA_LOCAL_CODER_FRAMESIZE;
      localCoderVadEnable  = MMS_MEDIA_LOCAL_CODER_VAD_ENABLE;
      #else
      *localCoderType = '\0';               // When we do not configure these
      localCoderFramesize  = 0;             // they default to remote values
      localCoderVadEnable  = MMS_MEDIA_LOCAL_CODER_VAD_ENABLE;
      #endif   

      agcDisableIP         = MMS_MEDIA_AGC_DISABLE_IP;
      agcDisableVox        = MMS_MEDIA_AGC_DISABLE_VOX;
      agcDisableConferee   = MMS_MEDIA_AGC_DISABLE_CONFEREE;
      toneClampDisable     = MMS_MEDIA_TONECLAMP_DISABLE;
      rfc2833Enable        = MMS_MEDIA_RFC2833_ENABLE; 
      verifyRemotePortMod2 = MMS_MEDIA_VERIFY_RTCP_MOD2;

      ttsEnable      = MMS_MEDIA_TTS_ENABLE;
      ACE_OS::strncpy(ttsEngine,  MMS_MEDIA_TTS_ENGINE,   sizeof(ttsEngine)-1);
      ACE_OS::strncpy(ttsServerIP,MMS_MEDIA_TTS_SERVER_IP,sizeof(ttsServerIP)-1); 
      ttsServerPort  = MMS_MEDIA_TTS_SERVER_PORT;
      ttsQualityBits = MMS_MEDIA_TTS_QUALITY_BITS;
      ttsVoiceRate   = MMS_MEDIA_TTS_VOICE_RATE;
      ttsVolume      = MMS_MEDIA_TTS_VOLUME;
      ttsVoice       = MMS_MEDIA_TTS_VOICE;
      ttsExpireDays  = MMS_MEDIA_TTS_FILE_EXPIRE_DAYS;
      ttsIsPathStrategy = MMS_MEDIA_TTS_ISPATH_STRATEGY;
      ttsValidateVendorConfig = MMS_MEDIA_TTS_VALIDATE_VNDCONFIG;

      asrEnable = MMS_MEDIA_ASR_ENABLE;
      ACE_OS::strncpy(asrEngine,  MMS_MEDIA_ASR_ENGINE,   sizeof(asrEngine)-1);

      pcmSampleSize = MMS_MEDIA_PCM_SAMPLE_SIZE;
      voiceBargeinThreshold = MMS_MEDIA_VOICE_BARGEIN_THRESHOLD;
    }
  };


  struct Server                             // Media server process 
  {
    int  serviceThreadPoolSizeFactor;       // 1-4 = 25% to 62.5% of max connections

    int  connectAsync;                      // Start remote media async (boolean)

    int  reassignIdleVoiceResources;        // Permit voice resx usurp (1/0)
    int  idleDeviceSelectionStrategy;       // Stategy used to select idle device
    int  waitForVoiceResourceSeconds;       // Timeout for voice resource acquire  
    int  waitForVoiceResourceMsecs;         // in two parts: seconds and millisecs
    int  heartbeatIntervalMsecs;            // Heartbeat (1000 = 1 second)
    int  eventWaitForDependencyMsecs;       // Term event wait for command complete
    int  postProvisionalResult;             // Return result of async execute (1/0)
    int  cleanLogsAfterDays;                // # days after which to delete logs
    int  sessionMonitorIntervalSeconds;     // Session/cmd timeout check interval
    int  threadMonitorIntervalSeconds;      // Threaded task status check interval
    int  sessionTimeoutSecondsDefault;      // Timeout for disconnecting session
    int  serverTimeoutSeconds;              // Thread nonresponse before server halts
    int  cleanDirsIntervalMinutes;          // How often to police log/audio dirs
    int  inactiveConfereeTimeoutMinutes;    // Time after which conferee forced out
    int  inactiveConfereeTimeoutStrategy;   // Strategy used to time out conferee
    int  pendingCommandStrategy;            // Strategy used to handle pending command
    int  setUnhandledExceptionTrap;         // Trap top level excps/force server (1/0)
    int  overwriteDumpFile;                 // Dumps use same file name (1/0)
    int  cacheBusyDisconnect;               // Cache session disco when session busy
    int  suppressLogMessageReceipt;         // Suppress logging of "msg received"(1/0) 
    int  pointerValidationLevel;            // Level of pointer dereference checking
    int  maxDirectoryScanCount;             // Number of directories to scan for files
    int  defaultFlatmapIpcPort;             // Flatmap IPC port
    int  hairpinOpts;                       // Conferencing options eg hairpinning
    int  hairpinPromotionOpts;              // Hairpinning promotion/demotion options
    int  requireConcurrentOpConfirmation;   // Concurrent op must be explicit (1/0)

    int  commandTimeoutMsecsDefault;        // Default timeouts for async commands
    int  commandTimeoutMsecsPlay;  
    int  commandTimeoutMsecsRecord ; 
    int  commandTimeoutMsecsRecordTransaction; 
    int  commandTimeoutMsecsMonitorCallState; 
    int  commandTimeoutMsecsGetDigits; 
    int  commandTimeoutMsecsConnect; 
    int  commandTimeoutMsecsVoiceRec;

    int  resourceDetailLogIntervalMsecs;    // How often to log resource detail
    int  systemStatsLogIntervalMsecs ;      // How often to log system stats

    int  defaultBitRatePlay;                // Uses mmsParameterMap.h constants
    int  defaultFormatPlay;                 // ditto
    int  defaultFileTypePlay;               // ditto
    int  defaultBitRateRecord;              // ditto
    int  defaultFormatRecord;               // ditto
    int  defaultFileTypeRecord;             // ditto
    int  defaultSampleSizePlay;             // ditto
    int  defaultSampleSizeRecord;           // ditto

    int  defaultMaxSilenceSecondsPlay;
    int  defaultMaxSilenceSecondsRecord;
    int  defaultMaxDelaySecondsReceive;
    int  digitSequenceIntervalSeconds;
    int  defaultMaxSecsTone;
    int  defaultMonitorCallStateDuration;

    int  crashServerOnVoiceEvent;           // Force a top level exception
    int  disregardLocaleDirectories;        // When 1, do not use locale paths

    char audioBasePath[MAXPATHLEN];         // Audio files directory 
		char dumpBasePath [MAXPATHLEN];         // Dump files archive directory
    char driveMappingList[MAXPATHLEN];      // Drive mapping list
    char defaultAppName[MMS_MAX_APPNAMSIZE+1]; // Default app name for debugging
    char defaultLocale [MMS_MAX_LOCALESIZE+1]; // Default locale directory for debugging

    
    Server()
    {
      memset(this, 0, sizeof(Server));
      serviceThreadPoolSizeFactor     = MMS_SERVICE_THREADPOOL_SIZE_FACTOR;
      connectAsync                    = MMS_CONNECT_ASYNC;
      sessionMonitorIntervalSeconds   = MMS_SESSION_MONITOR_INTERVAL_SECONDS;
      threadMonitorIntervalSeconds    = MMS_THREAD_MONITOR_INTERVAL_SECONDS;     
      reassignIdleVoiceResources      = MMS_REASSIGN_IDLE_VOICE_RESOURCES;
      idleDeviceSelectionStrategy     = MMS_IDLE_DEVICE_SELECTION_STRATEGY;
      waitForVoiceResourceSeconds     = MMS_WAIT_FOR_VOICE_RESOURCE_SECONDS;
      waitForVoiceResourceMsecs       = MMS_WAIT_FOR_VOICE_RESOURCE_MSECS;
      heartbeatIntervalMsecs          = MMS_HEARTBEAT_INTERVAL_MSECS;
      eventWaitForDependencyMsecs     = MMS_EVENT_WAIT_FOR_DEPENDENCY_MS;
      postProvisionalResult           = MMS_POST_PROVISIONAL_RESULT;
      cleanLogsAfterDays              = MMS_CLEAN_LOGS_AFTER_DAYS;
      suppressLogMessageReceipt       = MMS_SUPPRESS_LOG_MSG_RECEIPT;
      setUnhandledExceptionTrap       = MMS_SET_UNHANDLED_EXCEPTION_TRAP;
      crashServerOnVoiceEvent         = MMS_CRASH_SERVER_ON_VOICE_EVENT;
      maxDirectoryScanCount           = MMS_MAX_DIRECTORY_SCAN_COUNT;
      overwriteDumpFile               = MMS_OVERWRITE_DUMP_FILE;
      cacheBusyDisconnect             = MMS_CACHE_BUSY_DISCONNECT;
      pointerValidationLevel          = MMS_POINTER_VALIDATION_LEVEL;
      serverTimeoutSeconds            = MMS_MAX_SECONDS_THREAD_NONRESPONSE;
      sessionTimeoutSecondsDefault    = MMS_SESSION_TIMEOUT_SECONDS_DEFAULT;
      commandTimeoutMsecsDefault      = MMS_COMMAND_TIMEOUT_MSECS_DEFAULT;
      commandTimeoutMsecsPlay         = MMS_COMMAND_TIMEOUT_MSECS_PLAY;
      commandTimeoutMsecsRecord       = MMS_COMMAND_TIMEOUT_MSECS_RECORD;
      commandTimeoutMsecsRecordTransaction = MMS_COMMAND_TIMEOUT_MSECS_RECORD_TRANS; 
      commandTimeoutMsecsMonitorCallState  = MMS_COMMAND_TIMEOUT_MSECS_MONCALLSTATE;
      commandTimeoutMsecsGetDigits    = MMS_COMMAND_TIMEOUT_MSECS_GET_DIGITS; 
      commandTimeoutMsecsConnect      = MMS_COMMAND_TIMEOUT_MSECS_CONNECT;
      commandTimeoutMsecsVoiceRec     = MMS_COMMAND_TIMEOUT_MSECS_VOICEREC;
      cleanDirsIntervalMinutes        = MMS_CLEAN_DIRECTORIES_INTERVAL_MINUTES; 
      inactiveConfereeTimeoutMinutes  = MMS_INACTIVE_CONFEREE_TIMEOUT_MINUTES;
      inactiveConfereeTimeoutStrategy = MMS_INACTIVE_CONFEREE_TIMEOUT_STRATEGY;
      pendingCommandStrategy          = MMS_PENDING_COMMAND_STRATEGY;
      digitSequenceIntervalSeconds    = MMS_DIGIT_SEQUENCE_INTERVAL_SECONDS;
      hairpinOpts                     = MMS_DEFAULT_HAIRPIN_OPTS;
      hairpinPromotionOpts            = MMS_DEFAULT_HAIRPIN_PROMOTE;
      requireConcurrentOpConfirmation = MMS_REQUIRE_CONCURRENT_OP_CONFIRMATION;
      disregardLocaleDirectories      = MMS_DISREGARD_LOCALE_DIRECTORIES;

      resourceDetailLogIntervalMsecs  = MMS_RESX_DETAIL_LOG_INTERVAL_MSECS;
      systemStatsLogIntervalMsecs     = MMS_SYS_DETAIL_LOG_INTERVAL_MSECS;

      ACE_OS::strcpy(audioBasePath,MMS_AUDIO_BASEPATH);
      ACE_OS::strcpy(dumpBasePath, MMS_DUMP_BASEPATH);
      ACE_OS::strcpy(driveMappingList, MMS_DRIVE_MAPPING_LIST);
      ACE_OS::strcpy(defaultAppName, MMS_DEFAULT_APPNAME);
      ACE_OS::strcpy(defaultLocale,  MMS_DEFAULT_LOCALE);

                                            // Encoding and bitrate defaults
      defaultBitRatePlay     = MMS_RATE_KHZ_8;   
      defaultSampleSizePlay  = MMS_SAMPLESIZE_BIT_8;
      defaultFormatPlay      = MMS_FORMAT_MULAW;                 
      defaultFileTypePlay    = MMS_FILETYPE_VOX;                
      defaultBitRateRecord   = MMS_RATE_KHZ_8;               
      defaultSampleSizeRecord= MMS_SAMPLESIZE_BIT_8;
      defaultFormatRecord    = MMS_FORMAT_MULAW;                
      defaultFileTypeRecord  = MMS_FILETYPE_VOX;  

      defaultMaxSilenceSecondsPlay    = MMS_DEFAULT_MAX_SILENCE_SECS_PLAY;
      defaultMaxSilenceSecondsRecord  = MMS_DEFAULT_MAX_SILENCE_SECS_RECORD; 
      defaultMaxDelaySecondsReceive   = MMS_DEFAULT_MAX_DELAY_SECS_RECEIVE; 
      defaultMaxSecsTone              = MMS_DEFAULT_MAX_SECS_TONE; 
      defaultMonitorCallStateDuration = MMS_DEFAULT_MONCALLSTATE_DURATION_MS;

      defaultFlatmapIpcPort           = MMS_DEFAULT_FLATMAP_IPC_PORT;
    }
  };



  struct IpcAdapter                         // Client IPC converter
  {
    int maxMapSize;                         // Max bytes in a parameter flatmap
                                             
    IpcAdapter()
    {
        maxMapSize = MMS_PROTOCOL_MAX_MAPSIZE;
    }
  };



  struct Reports                            // Alarms, statistics, etc.
  {
    int hiwaterG711Resx;                    // Hi water mark connections
    int lowaterG711Resx;                    // Lo water mark connections
    int hiwaterG729EtcResx;                 // Hi water mark lo bit rate resources
    int lowaterG729EtcResx;                 // Lo water mark lo bit rate resources
    int hiwaterVoiceResx;                   // Hi water mark voice resources
    int lowaterVoiceResx;                   // Lo water mark voice resources
    int hiwaterConferenceResx;              // Hi water mark conference resources
    int lowaterConferenceResx;              // Lo water mark conference resources
    int hiwaterConferences;                 // Hi water mark concurrent conferences
    int lowaterConferences;                 // Lo water mark concurrent conferences
    int hiwaterTtsResx;                     // Hi water mark TTS resources (ports)
    int lowaterTtsResx;                     // Lo water mark TTS resources (ports)
    int hiwaterSpeechRecResx;               // Hi water mark speech rec ports
    int lowaterSpeechRecResx;               // Lo water mark speech rec ports
    int hiwaterSpeechIntResx;               // Hi water mark speech enabled voice resx
    int lowaterSpeechIntResx;               // Lo water mark speech enabled voice resx

    int disableAlarms;                      // Boolean to disable alarm calls
    int disableStats;                       // Boolean to disable stats calls
    int alarmThreshold;                     // Alarm level: 0=all;1=alert+;2=error+
    int monitorConnectionIntervalSeconds;   // How often server manager pings us

    int  statServerPort;                    // Port to connect to alarm/stat server
    char statServerIP[16];                  // Address of alarm/stat server

    Reports()
    {
        memset(this, 0, sizeof(Reports));
        hiwaterG711Resx       = MMS_REPORTS_HIWATER_G711;
        lowaterG711Resx       = MMS_REPORTS_LOWATER_G711;
        hiwaterG729EtcResx    = MMS_REPORTS_LOWATER_G729_ETC;
        lowaterG729EtcResx    = MMS_REPORTS_LOWATER_G729_ETC;
        hiwaterVoiceResx      = MMS_REPORTS_HIWATER_VOICE;
        lowaterVoiceResx      = MMS_REPORTS_LOWATER_VOICE;
        hiwaterConferenceResx = MMS_REPORTS_HIWATER_CONFERENCE;
        lowaterConferenceResx = MMS_REPORTS_LOWATER_CONFERENCE;
        hiwaterConferences    = MMS_REPORTS_HIWATER_CONFERENCES;
        lowaterConferences    = MMS_REPORTS_LOWATER_CONFERENCES;
        hiwaterTtsResx        = MMS_REPORTS_HIWATER_TTS;
        lowaterTtsResx        = MMS_REPORTS_LOWATER_TTS;
        hiwaterSpeechRecResx  = MMS_REPORTS_HIWATER_SPEECHREC;
        lowaterSpeechRecResx  = MMS_REPORTS_LOWATER_SPEECHREC;
        hiwaterSpeechIntResx  = MMS_REPORTS_HIWATER_SPEECHINT;
        lowaterSpeechIntResx  = MMS_REPORTS_LOWATER_SPEECHINT;

        disableAlarms   = MMS_REPORTS_DISABLE_ALARMS;
        disableStats    = MMS_REPORTS_DISABLE_STATS;
        alarmThreshold  = MMS_REPORTS_ALARM_THRESHOLD;
        monitorConnectionIntervalSeconds = MMS_REPORTS_MONITOR_CONX_SECONDS;

        statServerPort  = MMS_REPORTS_STAT_SERVER_PORT;
        ACE_OS::strncpy(statServerIP,MMS_REPORTS_STAT_SERVER_IP,sizeof(statServerIP)-1); 
    } 
  };


  struct HMP
  {
    int  maxConnections;                    // Ceiling on max concurrent conx
    int  maxInitialResourcesIP;             // Number to load at startup
    int  maxInitialResourcesVoice;          // Number to load at startup
    int  maxInitialResourcesConf;           // Number to load at startup
    int  startSvcMaxWaitSecs;               // Max wait to start hmp service
    int  volume;                            // -10 <= v <= +10; default 0
    int  speed;                             // -10 <= s <= +10; default 0
    int  rtpPortBase;                       // HMP RTP port base
    int  internalLicensingMode;             // Licensing mode: 0=normal 1=overridable
    int  disableCeiling;                    // When 1, do not use license limit ceiling
    int  licenseHeadroomPercent;            // Headroom above license if any
    int  disallowLicenseHeadroom;           // When 1, don't permit any headroom
    int  assumeSdkOnLicenseFailure;         // When 1, revert to SDK if no license
    int  logMaxResources;                   // When 1, dump resource counts to log
    int  setDscpExpediteForward;            // When 1, set EF as TCP DSCP byte

    char ciscoDevKey[MMS_MAX_DEVKEYSIZE+1]; // Key to enable developer mode

    HMP()                                   // Remember, these values are over-
    { memset(this, 0, sizeof(HMP));         // ridden by config file values
      maxConnections           = MMS_HMP_MAXCONNECTIONS_DEFAULT;
      maxInitialResourcesIP    = MMS_HMP_MAX_INITIAL_IP_RESOURCES;          
      maxInitialResourcesVoice = MMS_HMP_MAX_INITIAL_VOICE_RESOURCES;          
      maxInitialResourcesConf  = MMS_HMP_MAX_INITIAL_CONF_RESOURCES; 
      startSvcMaxWaitSecs      = MMS_HMP_START_SVC_MAXWAITSECS; 
      volume                   = MMS_HMP_DEFAULT_VOLUME;
      speed                    = MMS_HMP_DEFAULT_SPEED;
      rtpPortBase              = MMS_HMP_RTP_PORT_BASE;

      disableCeiling           = MMS_HMP_DISABLE_LICENSE_MODE;
      internalLicensingMode    = MMS_HMP_INTERNAL_LICENSING_MODE;
      ACE_OS::strcpy(ciscoDevKey,MMS_HMP_CISCO_DEVKEY);
      licenseHeadroomPercent   = MMS_HMP_LICENSE_HEADROOM_PERCENT;
      disallowLicenseHeadroom  = MMS_HMP_DISALLOW_LICENSE_HEADROOM;
      assumeSdkOnLicenseFailure= MMS_HMP_ASSUME_SDK_ON_LICENSE_FAIL;
      logMaxResources          = MMS_HMP_LOG_MAX_RESOURCES;
      setDscpExpediteForward   = MMS_HMP_SET_DSCP_EXPEDITE_FORWARD;
    }
  };


  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  // Entries calculated from other entries, perhaps also using other sources
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

  struct Calculated
  {
    int maxConnections;                     // Final word on server instance max
    int maxMediaResourcesIP;                 
    int maxMediaResourcesVoice;              
    int maxMediaResourcesConf; 
    int threadPoolSize; 
    int utilityPoolSize;

    int isConferencingEnabled;              // Does server have conf resources
    int isVoiceEnabled;                     // Does server have voice resources

    int isValidDevkey;                      // Was a valid developer key supplied

    int heartbeatsPerSecond;

    Calculated()                                    
    {                                        
      memset(this,0,sizeof(Calculated));
    }
  };


  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  // Config data objects 
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

  ServerLogger    serverLogger;
  ClientLogger    clientLogger;
  Client          clientParams;
  IpcAdapter      ipcAdapter;
  Server          serverParams;
  Media           media;
  HMP             hmp;
  Reports         reports;
  Diagnostics     diagnostics;
  Calculated      calculated; 


  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  // Standalone (non-dotted) config entries 
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                                            
  int  foobar;                              // Test entry
  int  iparamA;                             // Utility entries
  int  iparamB;
  int  iparamC;
  int  iparamD;
  char cparamA[64];


  MmsConfig(): f(0), isInitialLoad(1), keyi(0), keys(0)       
  {                                         // Initialize non-dotted entries:
    foobar   = 0;                           // To test top-level config entry
    iparamA  = iparamB = iparamC = iparamD = 0;       
    *cparamA = 0;
  } 

  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  // End config data - begin internals
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

  int readLocalConfigFile(char* path=0);

  int getNumDriveMappings() { return driveMappings.size(); }
  int getDriveMappingFullPath(std::string& path);

  ~MmsConfig() { if (f) fclose(f); } 

  protected:
  struct ndx {int id; char* key;}; 

  struct keytables
  {
    ndx* nonDottedKeys;
    ndx* topLevelKeys;
    ndx* loggerKeys;
    ndx* diagnosticsKeys;
    ndx* clientKeys;
    ndx* serverKeys;
    ndx* ipcAdapterKeys;
    ndx* hmpKeys;
    ndx* mediaKeys;
    ndx* reportsKeys;

    int  nNonDottedKeys;
    int  nTopLevelKeys;
    int  nLoggerKeys;
    int  nDiagnosticsKeys;
    int  nClientKeys;
    int  nServerKeys;
    int  nIpcAdapterKeys;
    int  nHmpKeys;
    int  nMediaKeys;
    int  nReportsKeys;

    keytables() { memset(this, 0, sizeof(keytables));}
  };

  FILE*  f;
  double m_fixedval;
  int    isInitialLoad; 
  int    m_object, m_item, m_datatype, m_length, m_value, m_isbool, m_n;
  ndx*   keyi,*keys;
  char*  beg, *end, *endobj, *begkey, *equal, *begval, *endval, *m_p;  

  int  identifyObject(char* line, keytables*);  
  int  identifyItem(keytables*);
  int  identifyValue();                                                   
  int  assignValue(); 

  int  getConfigFilePath (char* outpath, const int outpathlength);
  int  identifyBitrate   (const int   configval);
  int  identifySamplesize(const int   configval);
  int  identifyFileFormat(const char* configval);
  int  identifyFiletype  (const char* configval);
  int  editPath(char* path);
  int  validateDevKey(char* key);
  void logImmutable();

  typedef std::map<std::string, std::string> driveMappingsTable;
  driveMappingsTable driveMappings;
  void parseDriveMappings();
  void addDriveMapping(char* mapping);
};


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Miscellaneous constants
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

#define MMSCONFIG_TOKEN_FOUND              0
#define MMSCONFIG_LINE_IS_BLANK_OR_COMMENT 1

#define MMSCONFIG_DATATYPE_INT    1
#define MMSCONFIG_DATATYPE_FIXED  2
#define MMSCONFIG_DATATYPE_CHAR   3

#define NOTWHITESPACE(p) (*p != ' '  && *p != '\t')
#define ISWHITESPACE(p)  (*p == ' '  || *p == '\t')
#define NOTENDOFLINE(p)  (*p != '\0' && *p != '\n' && *p != '\r')
#define ISENDOFLINE(p)   (*p == '\0' || *p == '\n' || *p == '\r')



// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Maintenance instructions
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
//
// To add a new object: 
// 1. Create an identifier for the object, under "Object identifiers " below
// 2. Declare a similarly-named struct for the object, under MmsConfig public
// 3. Create appropriate entries in struct keytables, above
// 4. Add the name of the object, paired with the identifier from (1),
//    to ndx struct topLevelKeys in MmsConfig.cpp.
// 5. Determine object items (data members) and add to struct from (2)
// 6. Create identifiers for each item under "Item identifiers" below.
// 7. Create default symbolic constants for each item, at the top of this
//    file, and set each object item to its default constant, in the 
//    constructor of the object from (2).
// 8. Create an ndx structure in MmsConfig.cpp readLocalConfigFile(),
//  . pairing each item identifier from (6), with a string representation of 
//    the item name, from (5)
// 9. Add a switch entry in MmsConfig.cpp MmsConfig::identifyItem(), for the
//    identifier from (1), assigning your values from the keytables struct 
//    from (3), to local variables, using the other switch entries as a guide.
// 10.To MmsConfig.cpp MmsConfig::assignValue(), add a high-level case to
//    the data typing switch for the new object identifier from (1), and under 
//    that, add a data value editing case for each item identifier from (6).
// 11.Add entries to the properties file, if needed, using the item names 
//    from (5), overriding the default values set in (7).
//
// To add a data item to an existing object:
// 1. Determine item name, and add to the existing object struct in MmsConfig.
// 2. Create an identifier for the item, under "Item identifiers" below.
// 3. Create a default symbolic constant for the item, at the top of this
//    file, and set the object item to its default constant, in the 
//    constructor of the object from (2).
// 4. Add an entry to the object's ndx struct in MmsConfig.cpp 
//    MmsConfig::readLocalConfigFile(), pairing the item identifier from (2), 
//    with a string representation of the item name, from (1)
// 5. To MmsConfig.cpp MmsConfig::assignValue(), add a data value editing case
//    for the item identifier from (2), under the object's data typing switch.
// 6. Add an entry to the properties file, if needed, using the item name 
//    from (1), overriding the default values set in (3).
//
// To add a data item which is not a member of an object (non-dotted item):
// 1. Determine item name, and add the data member MmsConfig next to the 
//    other non-dotted entries.
// 2. Create a NONDOTTED_ identifer under "Item identifiers" below.
// 3. Create a default symbolic constant for the item, at the top of this
//    file, and set the object item to its default constant, in the 
//    MmsConfig constructor.above.
// 4. Add an entry to the nonDottedKeys ndx struct in MmsConfig.cpp 
//    MmsConfig::readLocalConfigFile(), pairing the item identifier from (2), 
//    with a string representation of the item name, from (1).
// 5. To MmsConfig.cpp MmsConfig::assignValue(), add a data value editing case
//    for the item identifier from (2), under the object's data typing switch.
// 6. Add an entry to the properties file, if needed, using the item name 
//    from (1), overriding the default values set in (3).


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Object identifiers (object.item = value)
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
#define TOPLEVEL        0 
#define LOGGER          1 
#define SERVERLOGGER    2 
#define CLIENTLOGGER    3 
#define DIAGNOSTICS     4
#define IPCADAPTER      5 
#define CLIENT          6 
#define SERVER          7 
#define HMP             8 
#define MEDIA           9 
#define REPORTS        10


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// Item identifiers (object.item = value)
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// These must be unique within category, e.g. SERVER

#define LOGGER_DEFAULTMESSAGELEVEL             1 
#define LOGGER_GLOBALMESSAGELEVEL              2 
#define LOGGER_NUMTHREADS                      3 
#define LOGGER_TIMESTAMP                       4 
#define LOGGER_DESTSTDOUT                      5 
#define LOGGER_DESTDEBUG                       6 
#define LOGGER_DESTFILE                        7 
#define LOGGER_DESTSOCKET                      8
#define LOGGER_DESTOTHER                       9 
#define LOGGER_ISFULLPATH                     10 
#define LOGGER_BACKUP                         11
#define LOGGER_FLUSH                          12
#define LOGGER_MAXLINES                       13
#define LOGGER_FILEPATH                       14 
#define LOGGER_SOCKET_PORT                    15
#define LOGGER_SOCKET_CONNECTINTERVAL         16
#define LOGGER_DESTLOGSERVER									17

#define SHAREDMEMORY_PACLIENTQUEUESLOTS        1 
#define SHAREDMEMORY_PASERVERQUEUESLOTS        2 
            
#define CLIENT_PROTOCOLS                       1 
#define CLIENT_MSMQ_APP_QUEUENAME              2
#define CLIENT_MSMQ_MMS_QUEUENAME              3
#define CLIENT_MSMQ_APP_MACHINENAME            4
#define CLIENT_MSMQ_MMS_MACHINENAME            5
#define CLIENT_MSMQ_MAX_MESSAGE_AGE_SECS       6
#define CLIENT_MSMQ_TIMEOUT_MSECS              7
#define CLIENT_MSMQ_HEARTBEAT_INTERVAL         8
#define CLIENT_TEST_ADAPTER_ITERATIONS         9
#define CLIENT_RESPOND_ON_ASYNC_EXECUTE       10
#define CLIENT_PERMIT_DEFAULT_APP_QUEUE       11 
#define CLIENT_HEARTBEAT_ACK_EXPECTED         12
#define CLIENT_HEARTBEAT_ACKS_MISSABLE        13

#define SERVER_THREADPOOL_SIZE_FACTOR          3
#define SERVER_UNUSED_AVAILABLE_1              4
#define SERVER_UNUSED_AVAILABLE_2              5
#define SERVER_AUDIO_BASEPATH                  6
#define SERVER_REASSIGN_IDLE_VOX               7
#define SERVER_WAIT_FOR_VOX_SECS               8
#define SERVER_WAIT_FOR_VOX_MSECS              9
#define SERVER_IDLE_SELECTION_STRATEGY        10
#define SERVER_SESSION_TIMEOUT_SECS_DEFAULT   11
#define SERVER_CMD_TIMEOUT_MSECS_DEFAULT      12
#define SERVER_CMD_TIMEOUT_MSECS_CONNECT      13
#define SERVER_CMD_TIMEOUT_MSECS_PLAY         14
#define SERVER_CMD_TIMEOUT_MSECS_RECORD       15
#define SERVER_CMD_TIMEOUT_MSECS_RECORDTRAN   16
#define SERVER_CMD_TIMEOUT_MSECS_GETDIGITS    17
#define SERVER_CMD_TIMEOUT_MSECS_MONCALLSTATE 18
#define SERVER_CMD_TIMEOUT_MSECS_VOICEREC     19
#define SERVER_DEFAULT_BITRATE_PLAY           20
#define SERVER_DEFAULT_FORMAT_PLAY            21
#define SERVER_DEFAULT_FILETYPE_PLAY          22
#define SERVER_DEFAULT_BITRATE_RECORD         23
#define SERVER_DEFAULT_FORMAT_RECORD          24
#define SERVER_DEFAULT_FILETYPE_RECORD        25
#define SERVER_DEFAULT_MAXSIL_SECS_PLAY       30
#define SERVER_DEFAULT_MAXSIL_SECS_RECORD     31
#define SERVER_DEFAULT_MAXDELAY_SECS_DIGITS   32
#define SERVER_DEFAULT_MAX_SECS_TONE          33
#define SERVER_CONNECT_ASYNC                  36
#define SERVER_SESSION_MON_INTERVAL_SECS      37
#define SERVER_POST_PROVISIONAL_RESULT        38
#define SERVER_UNUSED_1                       39
#define SERVER_CLEAN_LOGS_AFTER_DAYS          40
#define SERVER_CLEAN_DIRS_AFTER_MINUTES       41
#define SERVER_DIGIT_SEQUENCE_SECONDS         42
#define SERVER_CALL_STATE_DURATION            43
#define SERVER_CALL_STATE_DURATION            43
#define SERVER_TIMEOUT_SECONDS                44
#define SERVER_THREAD_PING_INTERVAL_SECONDS   45
#define SERVER_SET_UNHANDLED_EXCEPTION_TRAP   46
#define SERVER_CRASH_ON_VOICE_EVENT           47
#define SERVER_OVERWRITE_DUMP_FILE            48
#define SERVER_POINTER_VALIDATION_LEVEL       49
#define SERVER_EVENT_WAIT_FOR_DEPENDENCY_MS   50
#define SERVER_DUMP_BASEPATH                  51
#define SERVER_RESX_DETAIL_LOG_INTERVAL_MS    52
#define SERVER_SYSTEM_STATS_LOG_INTERVAL_MS   53
#define SERVER_MAX_DIRECTORY_SCAN_COUNT       54
#define SERVER_FLATMAP_IPC_PORT               55
#define SERVER_HAIRPIN_OPTS                   56
#define SERVER_HAIRPIN_PROMOTION_OPTS         57
#define SERVER_CONFEREE_TIMEOUT_MINUTES       58
#define SERVER_CONFEREE_TIMEOUT_STRATEGY      59
#define SERVER_PENDING_COMMAND_STRATEGY       60
#define SERVER_REQUIRE_CONCURRENT_OP_CONFIRM  61
#define SERVER_CACHE_BUSY_DISCONNECT          62
#define SERVER_DRIVE_MAPPING_LIST             63
#define SERVER_DEFAULT_SAMPLESIZE_PLAY        64
#define SERVER_DEFAULT_SAMPLESIZE_RECORD      65
#define SERVER_DISREGARD_LOCALE_DIRECTORIES   66
#define SERVER_DEFAULT_APPNAME                67
#define SERVER_DEFAULT_LOCALE                 68

#define HMP_MAXCONNECTIONS                     1  
#define HMP_MAXINITIALIPRESOURCES              2
#define HMP_MAXINITIALVOICERESOURCES           3
#define HMP_MAXINITIALCONFRESOURCES            4 
#define HMP_MAXWAITSECSSTARTSERVICE            5 
#define HMP_VOLUME                             6 
#define HMP_SPEED                              7 
#define HMP_RTP_PORT_BASE                      8
#define HMP_INTERNAL_LICENSING_MODE            9
#define HMP_CISCO_DEVKEY                      10  
#define HMP_LICENSE_HEADROOM_PERCENT          11
#define HMP_DISALLOW_LICENSE_HEADROOM         12 
#define HMP_ASSUME_SDK_ON_LICENSE_FAIL        13 
#define HMP_DISABLE_LICENSE_MODE              14 
#define HMP_LOG_MAX_RESOURCES                 15
#define HMP_SET_DSCP_EXEDITE_FORWARD          16

#define MEDIA_CONFERENCE_NOTIFY_ON_JOIN        1 
#define MEDIA_CONFERENCE_NOTONE_RCVONLY        2
#define MEDIA_CODER                            3
#define MEDIA_FRAMESIZE                        4
#define MEDIA_VAD_ENABLE                       5
#define MEDIA_REMOTE_CODER                     6
#define MEDIA_REMOTE_CODER_TYPE                7
#define MEDIA_REMOTE_CODER_FRAMESIZE           8
#define MEDIA_REMOTE_CODER_VAD_ENABLE          9 
#define MEDIA_LOCAL_CODER                     10   
#define MEDIA_LOCAL_CODER_TYPE                11
#define MEDIA_LOCAL_CODER_FRAMESIZE           12
#define MEDIA_LOCAL_CODER_VAD_ENABLE          13
#define MEDIA_RECORD_FILE_EXPIRATION_DAYS     14
#define MEDIA_UTILITY_POOLSIZE_FACTOR         15
#define MEDIA_ACTIVE_TALKERS_ENABLED          16
#define MEDIA_AGC_DISABLE_IP                  17
#define MEDIA_AGC_DISABLE_VOX                 18
#define MEDIA_AGC_DISABLE_CONFEREE            19
#define MEDIA_TONECLAMP_DISABLE               20
#define MEDIA_VERIFY_RTCP_MOD2                21
#define MEDIA_TTS_ENABLE                      22
#define MEDIA_TTS_ENGINE                      23
#define MEDIA_TTS_VOLUME                      24
#define MEDIA_TTS_SERVER_IP                   25
#define MEDIA_TTS_SERVER_PORT                 26 
#define MEDIA_TTS_QUALITY_KHZ                 27
#define MEDIA_TTS_QUALITY_BITS                28
#define MEDIA_TTS_VOICE_RATE                  29
#define MEDIA_TTS_VOICE                       30
#define MEDIA_TTS_FILE_EXPIRE_DAYS            31
#define MEDIA_TTS_ISPATH_STRATEGY             32
#define MEDIA_TTS_VALIDATE_VENDOR_CONFIG      33
#define MEDIA_DEFAULT_TONE_FREQ               34
#define MEDIA_G723_KBPS                       35
#define MEDIA_G729_TYPE                       36
#define MEDIA_ENHANCED_RTP_OVERRIDE           37
#define MEDIA_ASR_ENABLE                      38
#define MEDIA_ASR_ENGINE                      39
#define MEDIA_PCM_SAMPLE_SIZE                 40
#define MEDIA_RFC2833_ENABLE                  41
#define MEDIA_VOICE_BARGEIN_THRESHOLD         42

#define DIAGNOSTICS_FLAGS                      1
#define DIAGNOSTICS_FLAGS_B                    2
#define DIAGNOSTICS_LOG_RESOURCE_STATUS        3 
#define DIAGNOSTICS_SHUTDOWN_AT_HEARTBEAT      4
#define DIAGNOSTICS_BREAK_AT_ALLOC             5
#define DIAGNOSTICS_LOG_OUTBOUND_MESSAGES      6

#define REPORTS_HIWATER_G711                   1
#define REPORTS_LOWATER_G711                   2
#define REPORTS_HIWATER_G729_ETC               3
#define REPORTS_LOWATER_G729_ETC               4
#define REPORTS_HIWATER_VOICE                  5
#define REPORTS_LOWATER_VOICE                  6
#define REPORTS_HIWATER_CONFERENCE             7
#define REPORTS_LOWATER_CONFERENCE             8
#define REPORTS_HIWATER_CONFERENCES            9
#define REPORTS_LOWATER_CONFERENCES           10
#define REPORTS_HIWATER_TTS                   11
#define REPORTS_LOWATER_TTS                   12
#define REPORTS_HIWATER_SPEECHREC             13
#define REPORTS_LOWATER_SPEECHREC             14
#define REPORTS_HIWATER_SPEECHINT             15
#define REPORTS_LOWATER_SPEECHINT             16
#define REPORTS_DISABLE_ALARMS                17
#define REPORTS_DISABLE_STATS                 18
#define REPORTS_ALARM_THRESHOLD               19
#define REPORTS_STAT_SERVER_PORT              20
#define REPORTS_STAT_SERVER_IP                21
#define REPORTS_MONITOR_CONX_SECONDS          22

#define NONDOTTED_FOOBAR  1
#define NONDOTTED_IPARAMA 2
#define NONDOTTED_IPARAMB 3
#define NONDOTTED_IPARAMC 4
#define NONDOTTED_IPARAMD 5
#define NONDOTTED_CPARAMA 6


// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
// End maintenance 
// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

#endif

