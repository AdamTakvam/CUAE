//
// mmsConfig.cpp
//
// Media server configuration object initialization and config file reader 
//
#include "StdAfx.h"
#pragma warning(disable:4786)
#include "mmsConfig.h"

#ifdef MMS_ENABLE_MEMORY_LEAK_DETECTION
#define new MMS_MEMLEAK_NEW
#endif // MMS_ENABLE_MEMORY_LEAK_DETECTION

  
                                         
int MmsConfig::readLocalConfigFile(char* path)
{
  char configFilePath[MAXPATHLEN];
  this->getConfigFilePath(configFilePath, MAXPATHLEN);

  f = fopen(configFilePath, "r");
  if (!f) return -1;

  keytables keys; 

  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  // ndx tables map symbolic constants (mmsConfig.h) to config file item names   
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  ndx nonDottedKeys[] =                     // Non-dotted items
  { {NONDOTTED_FOOBAR,  "foobar"},
    {NONDOTTED_IPARAMA, "iparamA"},
    {NONDOTTED_IPARAMB, "iparamB"},
    {NONDOTTED_IPARAMC, "iparamC"},
    {NONDOTTED_IPARAMD, "iparamD"},
    {NONDOTTED_CPARAMA, "cparamA"},
  };

  keys.nonDottedKeys  = nonDottedKeys;
  keys.nNonDottedKeys = sizeof(nonDottedKeys) / sizeof(ndx);


  ndx topLevelKeys[] =                      // Object names (object.item)
  { {LOGGER,          "Logger"},
    {SERVERLOGGER,    "ServerLogger"},
    {CLIENTLOGGER,    "ClientLogger"},
    {DIAGNOSTICS,     "Diagnostics"},
    {IPCADAPTER,      "IpcAdapter"},
    {REPORTS,         "Reports"},
    {CLIENT,          "Client"},
    {SERVER,          "Server"},
    {MEDIA,           "Media"},
    {HMP,             "Hmp"},
  };

  keys.topLevelKeys  = topLevelKeys;
  keys.nTopLevelKeys = sizeof(topLevelKeys) / sizeof(ndx);


  ndx loggerKeys[] =                        // Logger object item names
  { {LOGGER_DEFAULTMESSAGELEVEL,   "defaultMessageLevel"},
    {LOGGER_GLOBALMESSAGELEVEL,    "globalMessageLevel"},
    {LOGGER_NUMTHREADS,            "numThreads"},
    {LOGGER_TIMESTAMP,             "timestamp"},
    {LOGGER_DESTSTDOUT,            "destStdout"},
    {LOGGER_DESTDEBUG,             "destDebug"},
    {LOGGER_DESTFILE,              "destFile"},
    {LOGGER_DESTSOCKET,            "destSocket"},
    {LOGGER_DESTOTHER,             "destOther"},
    {LOGGER_ISFULLPATH,            "isFullpath"},
    {LOGGER_BACKUP,                "backup"},
    {LOGGER_FLUSH,                 "flush"},
    {LOGGER_MAXLINES,              "maxlines"},
    {LOGGER_FILEPATH,              "filepath"},
    {LOGGER_SOCKET_PORT,           "port"},
    {LOGGER_SOCKET_CONNECTINTERVAL,"socketConnectIntervalSeconds"},
	  {LOGGER_DESTLOGSERVER,         "destLogServer"},
   };

  keys.loggerKeys  = loggerKeys;
  keys.nLoggerKeys = sizeof(loggerKeys) / sizeof(ndx);


  ndx mediaKeys[] =                         // Media object item names
  { 
    {MEDIA_CODER,                      "coder"},
    {MEDIA_FRAMESIZE,                  "framesize"},
    {MEDIA_VAD_ENABLE,                 "vadEnable"},   
    {MEDIA_REMOTE_CODER_TYPE,          "remoteCoderType"},
    {MEDIA_REMOTE_CODER_FRAMESIZE,     "remoteCoderFramesize"},
    {MEDIA_REMOTE_CODER_VAD_ENABLE,    "remoteCoderVadEnable"},   
    {MEDIA_LOCAL_CODER_TYPE,           "localCoderType"},
    {MEDIA_LOCAL_CODER_FRAMESIZE,      "localCoderFramesize"},
    {MEDIA_LOCAL_CODER_VAD_ENABLE,     "localCoderVadEnable"},
    {MEDIA_CONFERENCE_NOTIFY_ON_JOIN,  "conferenceNotifyOnJoin"},
    {MEDIA_CONFERENCE_NOTONE_RCVONLY,  "conferenceNoToneReceiveOnly"},
    {MEDIA_RECORD_FILE_EXPIRATION_DAYS,"recordFileExpirationDays"},
    {MEDIA_UTILITY_POOLSIZE_FACTOR,    "utilityPoolSizeFactor"},
    {MEDIA_ACTIVE_TALKERS_ENABLED,     "conferenceActiveTalkersEnabled"},
    {MEDIA_AGC_DISABLE_IP,             "agcDisableIP"},
    {MEDIA_AGC_DISABLE_VOX,            "agcDisableVox"},
    {MEDIA_AGC_DISABLE_CONFEREE,       "agcDisableConferee"},
    {MEDIA_TONECLAMP_DISABLE,          "toneClampDisableIP"},
    {MEDIA_RFC2833_ENABLE,             "rfc2833Enable"},
    {MEDIA_VERIFY_RTCP_MOD2,           "verifyRemotePortMod2"},
    {MEDIA_TTS_ENABLE,                 "ttsEnable"},
    {MEDIA_TTS_ENGINE,                 "ttsEngine"},
    {MEDIA_TTS_SERVER_IP,              "ttsServerIP"},
    {MEDIA_TTS_SERVER_PORT,            "ttsServerPort"},
    {MEDIA_TTS_QUALITY_KHZ,            "ttsQualityKHz"},
    {MEDIA_TTS_QUALITY_BITS,           "ttsQualityBits"},
    {MEDIA_TTS_VOICE_RATE,             "ttsVoiceRate"},
    {MEDIA_TTS_VOICE,                  "ttsVoice"},
    {MEDIA_TTS_VOLUME,                 "ttsVolume"},
    {MEDIA_TTS_FILE_EXPIRE_DAYS,       "ttsExpireDays"},
    {MEDIA_TTS_ISPATH_STRATEGY,        "ttsIsPathStrategy"},
    {MEDIA_TTS_VALIDATE_VENDOR_CONFIG, "ttsValidateVendorConfig"},
    {MEDIA_DEFAULT_TONE_FREQ,          "defaultToneFrequency"},
    {MEDIA_G723_KBPS,                  "g723kbps"},
    {MEDIA_G729_TYPE,                  "g729type"},
    {MEDIA_ENHANCED_RTP_OVERRIDE,      "enhancedRTP"},
    {MEDIA_ASR_ENABLE,                 "asrEnable"},
    {MEDIA_ASR_ENGINE,                 "asrEngine"},
    {MEDIA_PCM_SAMPLE_SIZE,            "pcmSampleSize"},
    {MEDIA_VOICE_BARGEIN_THRESHOLD,    "voiceBargeinThreshold"},
  }; 
   
  keys.mediaKeys  = mediaKeys;
  keys.nMediaKeys = sizeof(mediaKeys) / sizeof(ndx);


  ndx diagnosticsKeys[] =                   // Diagnostics object item names
  { {DIAGNOSTICS_FLAGS,   "flags"},
    {DIAGNOSTICS_FLAGS_B, "flagsB"},
    {DIAGNOSTICS_LOG_RESOURCE_STATUS,   "logResourceStatus"},
    {DIAGNOSTICS_SHUTDOWN_AT_HEARTBEAT, "shutdownAtHearbeat"},
    {DIAGNOSTICS_BREAK_AT_ALLOC,        "debugBreakAtAllocation"},
    {DIAGNOSTICS_LOG_OUTBOUND_MESSAGES, "logOutboundMessages"},
  };  
     
  keys.diagnosticsKeys  = diagnosticsKeys;
  keys.nDiagnosticsKeys = sizeof(diagnosticsKeys) / sizeof(ndx);


  ndx clientKeys[] =                        // Client object item names
  { {CLIENT_PROTOCOLS,                 "ipcAdapters"},
    {CLIENT_MSMQ_APP_QUEUENAME,        "msmqAppQueueName"},
    {CLIENT_MSMQ_MMS_QUEUENAME,        "msmqMmsQueueName"},
    {CLIENT_MSMQ_APP_MACHINENAME,      "msmqAppMachineName"},
    {CLIENT_MSMQ_MMS_MACHINENAME,      "msmqMmsMachineName"},
    {CLIENT_MSMQ_MAX_MESSAGE_AGE_SECS, "maxMqMessageAgeSecs"},
    {CLIENT_MSMQ_TIMEOUT_MSECS,        "msmqTimeoutMsecs"},
    {CLIENT_MSMQ_HEARTBEAT_INTERVAL,   "heartbeatIntervalSecs"},
    {CLIENT_RESPOND_ON_ASYNC_EXECUTE,  "respondOnAsyncExecute"},
    {CLIENT_TEST_ADAPTER_ITERATIONS,   "serverTestAdapterIterations"},
    {CLIENT_PERMIT_DEFAULT_APP_QUEUE,  "permitDefaultAppQueue"},
    {CLIENT_HEARTBEAT_ACK_EXPECTED,    "heartbeatAckExpected"},
    {CLIENT_HEARTBEAT_ACKS_MISSABLE,   "heartbeatAcksMissable"},
  };     
       
  keys.clientKeys  = clientKeys;
  keys.nClientKeys = sizeof(clientKeys) / sizeof(ndx);


  ndx serverKeys[] =                        // Server object item names
  { {SERVER_THREADPOOL_SIZE_FACTOR,        "serviceThreadPoolSizeFactor"},
    {SERVER_AUDIO_BASEPATH,                "audioBasePath"},
    {SERVER_REASSIGN_IDLE_VOX,             "reassignIdleVoiceResources"},
    {SERVER_WAIT_FOR_VOX_SECS,             "waitForVoiceResourceSeconds"},
    {SERVER_WAIT_FOR_VOX_MSECS,            "waitForVoiceResourceMsecs"},
    {SERVER_EVENT_WAIT_FOR_DEPENDENCY_MS,  "eventWaitForDependencyMsecs"},
    {SERVER_IDLE_SELECTION_STRATEGY,       "idleDeviceSelectionStrategy"},
    {SERVER_SESSION_TIMEOUT_SECS_DEFAULT,  "sessionTimeoutSecondsDefault"},
    {SERVER_CMD_TIMEOUT_MSECS_DEFAULT,     "commandTimeoutMsecsDefault"},
    {SERVER_CMD_TIMEOUT_MSECS_CONNECT,     "commandTimeoutMsecsConnect"},
    {SERVER_CMD_TIMEOUT_MSECS_PLAY,        "commandTimeoutMsecsPlay"},
    {SERVER_CMD_TIMEOUT_MSECS_RECORD,      "commandTimeoutMsecsRecord"},
    {SERVER_CMD_TIMEOUT_MSECS_RECORDTRAN,  "commandTimeoutMsecsRecordTrans"},
    {SERVER_CMD_TIMEOUT_MSECS_GETDIGITS,   "commandTimeoutMsecsGetDigits"},
    {SERVER_CMD_TIMEOUT_MSECS_MONCALLSTATE,"commandTimeoutMsecsMonitorCallState"},
    {SERVER_CMD_TIMEOUT_MSECS_VOICEREC,    "commandTimeoutMsecsVoiceRec"},
    {SERVER_DEFAULT_BITRATE_PLAY,          "defaultBitRatePlay"},
    {SERVER_DEFAULT_SAMPLESIZE_PLAY,       "defaultSampleSizePlay"},
    {SERVER_DEFAULT_FORMAT_PLAY,           "defaultFormatPlay"},
    {SERVER_DEFAULT_FILETYPE_PLAY,         "defaultFileTypePlay"},
    {SERVER_DEFAULT_BITRATE_RECORD,        "defaultBitRateRecord"},
    {SERVER_DEFAULT_SAMPLESIZE_RECORD,     "defaultSampleSizeRecord"},
    {SERVER_DEFAULT_FORMAT_RECORD,         "defaultFormatRecord"},
    {SERVER_DEFAULT_FILETYPE_RECORD,       "defaultFileTypeRecord"},
    {SERVER_DEFAULT_MAXSIL_SECS_PLAY,      "defaultMaxSilenceSecondsPlay"},
    {SERVER_DEFAULT_MAXSIL_SECS_RECORD,    "defaultMaxSilenceSecondsRecord"},
    {SERVER_DEFAULT_MAXDELAY_SECS_DIGITS,  "defaultMaxDelaySecondsReceive"},
    {SERVER_DEFAULT_MAX_SECS_TONE,         "defaultMaxSecsTone"},
    {SERVER_CONNECT_ASYNC,                 "connectAsync"},
    {SERVER_SESSION_MON_INTERVAL_SECS,     "sessionMonitorIntervalSeconds"},
    {SERVER_POST_PROVISIONAL_RESULT,       "postProvisionalResult"},
    {SERVER_CLEAN_LOGS_AFTER_DAYS,         "cleanLogsAfterDays"},
    {SERVER_CLEAN_DIRS_AFTER_MINUTES,      "cleanFilesIntervalMinutes"},
    {SERVER_CONFEREE_TIMEOUT_MINUTES,      "inactiveConfereeTimeoutMinutes"},
    {SERVER_CONFEREE_TIMEOUT_STRATEGY,     "inactiveConfereeTimeoutStrategy"},
    {SERVER_DIGIT_SEQUENCE_SECONDS,        "digitSequenceIntervalSeconds"},  
    {SERVER_CALL_STATE_DURATION,           "callStateDurationMsecs"}, 
    {SERVER_TIMEOUT_SECONDS,               "serverTimeoutSeconds"}, 
    {SERVER_THREAD_PING_INTERVAL_SECONDS,  "threadMonitorIntervalSeconds"},
    {SERVER_SET_UNHANDLED_EXCEPTION_TRAP,  "setUnhandledExceptionTrap"}, 
    {SERVER_CRASH_ON_VOICE_EVENT,          "crashServerOnVoiceEvent"}, 
    {SERVER_OVERWRITE_DUMP_FILE,           "overwriteDumpFile"}, 
    {SERVER_CACHE_BUSY_DISCONNECT,         "cacheBusyDisconnect"}, 
    {SERVER_POINTER_VALIDATION_LEVEL,      "pointerValidationLevel"},
    {SERVER_DUMP_BASEPATH,                 "dumpBasePath"},
    {SERVER_RESX_DETAIL_LOG_INTERVAL_MS,   "resourceDetailLogIntervalMsecs"},
    {SERVER_SYSTEM_STATS_LOG_INTERVAL_MS,  "systemStatsLogIntervalMsecs"},
    {SERVER_MAX_DIRECTORY_SCAN_COUNT,      "maxDirectoryScanCount"},
    {SERVER_FLATMAP_IPC_PORT,              "defaultFlatmapIpcPort"}, 
    {SERVER_HAIRPIN_OPTS,                  "hairpinOpts"}, 
    {SERVER_HAIRPIN_PROMOTION_OPTS,        "hairpinPromotionOpts"}, 
    {SERVER_PENDING_COMMAND_STRATEGY,      "pendingCommandStrategy"},
    {SERVER_REQUIRE_CONCURRENT_OP_CONFIRM, "requireConcurrentOpConfirmation"},
    {SERVER_DRIVE_MAPPING_LIST,            "driveMappingList"},
    {SERVER_DISREGARD_LOCALE_DIRECTORIES,  "disregardLocaleDirectories"},
    {SERVER_DEFAULT_APPNAME,               "defaultAppName"},
    {SERVER_DEFAULT_LOCALE,                "defaultLocale"},
  }; 

  keys.serverKeys  = serverKeys;
  keys.nServerKeys = sizeof(serverKeys) / sizeof(ndx);


  ndx ipcAdapterKeys[] =                    // Adapter object item names
  { {IPCADAPTER_MAXMAPSIZE, "maxMapSize"},
  }; 

  keys.ipcAdapterKeys  = ipcAdapterKeys;
  keys.nIpcAdapterKeys = sizeof(ipcAdapterKeys) / sizeof(ndx);


  ndx hmpKeys[] =                           // HMP object item names
  { {HMP_MAXCONNECTIONS,            "maxConnections"},
    {HMP_MAXINITIALIPRESOURCES,     "maxInitialResourcesIP"},
    {HMP_MAXINITIALVOICERESOURCES,  "maxInitialResourcesVoice"},
    {HMP_MAXINITIALCONFRESOURCES,   "maxInitialResourcesConference"},
    {HMP_MAXWAITSECSSTARTSERVICE,   "startServiceMaxWaitSeconds"},
    {HMP_VOLUME,                    "volume"},
    {HMP_SPEED,                     "speed"},
    {HMP_RTP_PORT_BASE,             "rtpPortBase"},
    {HMP_INTERNAL_LICENSING_MODE,   "internalLicensingMode"},
    {HMP_CISCO_DEVKEY,              "ciscoDevKey"},
    {HMP_LICENSE_HEADROOM_PERCENT,  "licenseHeadroomPercent"},
    {HMP_DISALLOW_LICENSE_HEADROOM, "disallowLicenseHeadroom"},
    {HMP_ASSUME_SDK_ON_LICENSE_FAIL,"assumeSdkOnLicenseFailure"},
    {HMP_DISABLE_LICENSE_MODE,      "disableCeiling"},
    {HMP_LOG_MAX_RESOURCES,         "logMaxResources"},
    {HMP_SET_DSCP_EXEDITE_FORWARD,  "setDscpExpediteForward"},
  }; 

  keys.hmpKeys  = hmpKeys;
  keys.nHmpKeys = sizeof(hmpKeys) / sizeof(ndx);


  ndx reportsKeys[] =                       // Reports object item names
  { {REPORTS_HIWATER_G711,          "hiwaterG711Resx"},
    {REPORTS_LOWATER_G711,          "lowaterG711Resx"},
    {REPORTS_HIWATER_G729_ETC,      "hiwaterG729EtcResx"},
    {REPORTS_LOWATER_G729_ETC,      "lowaterG729EtcResx"},
    {REPORTS_HIWATER_VOICE,         "hiwaterVoiceResx"},
    {REPORTS_LOWATER_VOICE,         "lowaterVoiceResx"},
    {REPORTS_HIWATER_CONFERENCE,    "hiwaterConferenceResx"},
    {REPORTS_LOWATER_CONFERENCE,    "lowaterConferenceResx"},
    {REPORTS_HIWATER_CONFERENCES,   "hiwaterConferences"},
    {REPORTS_LOWATER_CONFERENCES,   "lowaterConferences"},
    {REPORTS_HIWATER_TTS,           "hiwaterTtsResx"},
    {REPORTS_LOWATER_TTS,           "lowaterTtsResx"},
    {REPORTS_HIWATER_SPEECHREC,     "hiwaterSpeechRecResx"},
    {REPORTS_LOWATER_SPEECHREC,     "lowaterSpeechRecResx"},
    {REPORTS_HIWATER_SPEECHINT,     "hiwaterSpeechIntResx"},
    {REPORTS_LOWATER_SPEECHINT,     "lowaterSpeechIntResx"},
    {REPORTS_DISABLE_ALARMS,        "disableAlarms"},
    {REPORTS_DISABLE_STATS,         "disableStats"},
    {REPORTS_ALARM_THRESHOLD,       "alarmThreshold"},
    {REPORTS_STAT_SERVER_PORT,      "statServerPort"},
    {REPORTS_STAT_SERVER_IP,        "statServerIP"},
    {REPORTS_MONITOR_CONX_SECONDS,  "monitorConnectionIntervalSeconds"},
  }; 

  keys.reportsKeys  = reportsKeys;
  keys.nReportsKeys = sizeof(reportsKeys) / sizeof(ndx);

  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
  // end ndx structs     
  // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

  int  linenum  = 0, items = 0;
  char line [MAXCONFIGLINESIZE+1];          // Config file line buffer


  while(1)                                  // While not eof on config file ...
  {
    if  (NULL == ACE_OS::fgets(line, MAXCONFIGLINESIZE, f)) break;
    linenum++;
    m_object = 0;
    m_item   = 0;

    int  result  = this->identifyObject(line, &keys);
    if  (result == MMSCONFIG_LINE_IS_BLANK_OR_COMMENT) continue; 

    if  (result == -1 || this->identifyItem(&keys) == -1) 
    {
         MMSLOG((LM_ERROR,"CFIG line %d: name not recognized\n", linenum));
         continue;
    } 

    if ((this->identifyValue() == -1) || (this->assignValue() == -1))
    {
         MMSLOG((LM_ERROR,"CFIG line %d: invalid item value\n", linenum));
         continue;
    }

    items++;
  }
       
  isInitialLoad = 0;
  return items;
}


                                            // Parse out object (obj.item) token
int MmsConfig::identifyObject(char* line, keytables* keys)  
{
  char*  p = line;                           
  while(*p && ISWHITESPACE(p)) p++;         // Find first noblank character
  if   (ISENDOFLINE(p)) return MMSCONFIG_LINE_IS_BLANK_OR_COMMENT;

  while(*p && *p != '#' && *p != '=') p++;  // Find equal sign              
  if   (*p == '#')      return MMSCONFIG_LINE_IS_BLANK_OR_COMMENT;
  if   (ISENDOFLINE(p)) return -1;

  this->equal = p;                          // Mark equal sign

  char*  q = this->equal; q--;
  while(ISWHITESPACE(q))  q--;              // Strip trailing whitespace 

  p = line;
  while(ISWHITESPACE(p))  p++;              // Strip leading whitespace

  this->beg = p;
  this->end = q;

  while(p  < this->end && *p != '.') p++;   // Find dot (object.item)
  if   (p == this->end) 
  {     m_object = TOPLEVEL;                // No dot -- assume top-level item
        this->begkey = this->beg;           // (e.g. thisitem = 1, as opposed
        return MMSCONFIG_TOKEN_FOUND;       // to thisobject.thisitem = 1)
  }

  q = p; q--;
  while(*q == ' ') q--;                     // Strip spaces

  this->endobj = q;                         // Mark end of object name

  p++;
  while(*p == ' ') p++;                     // Strip spaces
  this->begkey = p;                         // Mark start of item name 

  q  = endobj; q++;                         // Point after object name
  char save  = *q;                          // Save that character
  *q = '\0';                                // Terminate objname string
  this->keyi = keys->topLevelKeys;

  for(int i = 0; i < keys->nTopLevelKeys; i++, keyi++)
  {
      if  (stricmp(keyi->key, this->beg) == 0)
      {    m_object = keyi->id;
           break;
      }
  }
 
  #ifdef MMSCONFIGTEST
  if  (m_object) MMSLOG((LM_DEBUG,"CFIG '%s'\n", this->beg));
  #endif
  *q = save;
  return m_object? MMSCONFIG_TOKEN_FOUND: -1;
}



int MmsConfig::assignValue()                 // Assign config value to object.item 
{
  int     result = 0;
  Logger* whichlogger = 0;


  switch(m_object)
  { 
    case TOPLEVEL: 
         switch(m_item)
         {
           case NONDOTTED_FOOBAR:           // This is an example item
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else this->foobar = m_value;
                break;

           case NONDOTTED_IPARAMA:          // Utility entries
                if  (m_datatype   != MMSCONFIG_DATATYPE_INT) result = -1; 
                else this->iparamA = m_value;
                break;

           case NONDOTTED_IPARAMB:    
                if  (m_datatype   != MMSCONFIG_DATATYPE_INT) result = -1; 
                else this->iparamB = m_value;
                break;

           case NONDOTTED_IPARAMC:    
                if  (m_datatype   != MMSCONFIG_DATATYPE_INT) result = -1; 
                else this->iparamC = m_value;
                break;

          case NONDOTTED_IPARAMD:    
                if  (m_datatype   != MMSCONFIG_DATATYPE_INT) result = -1; 
                else this->iparamD = m_value;
                break;

           case NONDOTTED_CPARAMA:    
                if  (m_datatype   != MMSCONFIG_DATATYPE_CHAR) result = -1; 
                else ACE_OS::strncpy(cparamA, this->begval, sizeof(cparamA)-1); 

                break;

           default: result = -1;
         }
         break;
    

    case MEDIA:
         switch(m_item)
         {
           case MEDIA_CODER:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else media.coder  = media.remoteCoder = media.localCoder = m_value;
                break;

           case MEDIA_FRAMESIZE:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else media.framesize = media.remoteCoderFramesize 
                                     = media.localCoderFramesize = m_value;
                break;

           case MEDIA_VAD_ENABLE:
                if  (!m_isbool) result = -1;  
                else media.vadEnable = media.remoteCoderVadEnable 
                                     = media.localCoderVadEnable = m_value;
                break;

           case MEDIA_REMOTE_CODER_TYPE:
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) result = -1; 
                else ACE_OS::strcpy(media.remoteCoderType, this->begval); 
                break;

           case MEDIA_REMOTE_CODER_FRAMESIZE:            
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else media.remoteCoderFramesize = m_value;
                break;

           case MEDIA_REMOTE_CODER_VAD_ENABLE:            
                if  (!m_isbool) result = -1; 
                else media.remoteCoderVadEnable = m_value;
                break;

           case MEDIA_LOCAL_CODER_TYPE:
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) result = -1; 
                else ACE_OS::strcpy(media.localCoderType, this->begval); 
                break;

           case MEDIA_LOCAL_CODER_FRAMESIZE:            
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else media.localCoderFramesize = m_value;
                break;

           case MEDIA_LOCAL_CODER_VAD_ENABLE:            
                if  (!m_isbool) result = -1; 
                else media.localCoderVadEnable = m_value;
                break;

           case MEDIA_CONFERENCE_NOTIFY_ON_JOIN: 
                if  (!m_isbool) result = -1;  
                else media.conferenceNotifyOnJoin = m_value;
                break;

           case MEDIA_CONFERENCE_NOTONE_RCVONLY:
                if  (!m_isbool) result = -1; 
                else media.conferenceNoToneReceiveOnly = m_value;
                break;

           case MEDIA_RECORD_FILE_EXPIRATION_DAYS:            
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else media.recordFileExpirationDays = m_value;
                break;

           case MEDIA_UTILITY_POOLSIZE_FACTOR:            
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else media.utilityPoolSizeFactor = m_value;
                break;

           case MEDIA_ACTIVE_TALKERS_ENABLED:            
                if  (!m_isbool) result = -1; 
                else media.conferenceActiveTalkersEnabled = m_value;
                break;

           case MEDIA_AGC_DISABLE_IP:            
                if  (!m_isbool) result = -1; 
                else media.agcDisableIP = m_value;
                break;

           case MEDIA_AGC_DISABLE_VOX:            
                if  (!m_isbool) result = -1; 
                else media.agcDisableVox = m_value;
                break;

           case MEDIA_AGC_DISABLE_CONFEREE:            
                if  (!m_isbool) result = -1; 
                else media.agcDisableConferee = m_value;
                break;

           case MEDIA_TONECLAMP_DISABLE:            
                if  (!m_isbool) result = -1; 
                else media.toneClampDisable = m_value;
                break;

           case MEDIA_RFC2833_ENABLE:            
                if  (!m_isbool) result = -1; 
                else media.rfc2833Enable = m_value;
                break;

           case MEDIA_VERIFY_RTCP_MOD2:            
                if  (!m_isbool) result = -1; 
                else media.verifyRemotePortMod2 = m_value;
                break;

           case MEDIA_TTS_ENABLE:            
                if  (!m_isbool) result = -1; 
                else media.ttsEnable = m_value;
                break;

           case MEDIA_TTS_ENGINE:
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) result = -1; 
                else ACE_OS::strncpy(media.ttsEngine, this->begval, 
                                     sizeof(media.ttsEngine)-1); 
                break;

           case MEDIA_TTS_SERVER_IP:
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) result = -1; 
                else ACE_OS::strncpy(media.ttsServerIP, this->begval,
                                     sizeof(media.ttsServerIP)-1); 
                break;

           case MEDIA_TTS_SERVER_PORT:            
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else media.ttsServerPort = m_value;
                break;

           case MEDIA_TTS_VOLUME:            
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else media.ttsVolume = m_value;
                break;
 
           case MEDIA_TTS_QUALITY_KHZ:            
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1;                 
                else media.ttsQualityKHz = m_value;
                break;

           case MEDIA_TTS_QUALITY_BITS:            
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else 
                if  (m_value != 8 && m_value != 16) result = -1;
                else media.ttsQualityBits = m_value;
                break;

           case MEDIA_TTS_VOICE_RATE:            
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else media.ttsVoiceRate = m_value;
                break;

           case MEDIA_TTS_VOICE:            
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else media.ttsVoice = m_value;
                break;

           case MEDIA_TTS_FILE_EXPIRE_DAYS:            
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else media.ttsExpireDays = m_value;
                break;

           case MEDIA_TTS_ISPATH_STRATEGY:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else media.ttsIsPathStrategy = m_value;
                break;

           case MEDIA_TTS_VALIDATE_VENDOR_CONFIG:
                if  (!m_isbool) result = -1; 
                else media.ttsValidateVendorConfig = m_value;
                break;

           case MEDIA_DEFAULT_TONE_FREQ:            
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else media.defaultToneFrequency = m_value;
                break;

           case MEDIA_G723_KBPS:
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) result = -1; 
                else 
                {    ACE_OS::strncpy(media.g723kbps, this->begval, sizeof(media.g723kbps)-1); 
                     media.setLowBitrateCoderTypes();
                }
                break;

           case MEDIA_G729_TYPE:
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) result = -1; 
                else 
                {    ACE_OS::strncpy(media.g729type, this->begval, sizeof(media.g729type)-1); 
                     media.setLowBitrateCoderTypes();
                }
                break;

           case MEDIA_ENHANCED_RTP_OVERRIDE:             
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else media.enhancedRTP = m_value;
                break;

           case MEDIA_ASR_ENABLE:            
                if  (!m_isbool) result = -1; 
                else media.asrEnable = m_value;
                break;

           case MEDIA_ASR_ENGINE:
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) result = -1; 
                else ACE_OS::strncpy(media.asrEngine, this->begval, 
                                     sizeof(media.asrEngine)-1); 
                break;

           case MEDIA_PCM_SAMPLE_SIZE:             
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else media.pcmSampleSize = m_value;
                break;

           case MEDIA_VOICE_BARGEIN_THRESHOLD:             
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else media.voiceBargeinThreshold = m_value;
                break;

           default: result = -1; 
         }
         break;


    case SERVERLOGGER: 
         whichlogger = &serverLogger;       // Fall thru ...
    case CLIENTLOGGER: 
         if  (whichlogger == 0)             // FYI there is no longer a
              whichlogger  = &clientLogger; // client logger to configure                                

         switch(m_item)
         {
           case LOGGER_DEFAULTMESSAGELEVEL: // External to internal priority
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else whichlogger->defaultMessageLevel = msgPriorityToLm(m_value);
                break;

           case LOGGER_GLOBALMESSAGELEVEL:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else whichlogger->globalMessageLevel = msgPriorityToLm(m_value);
                break;

           case LOGGER_NUMTHREADS:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else 
                if  (isInitialLoad) 
                     whichlogger->numThreads  = m_value;
                else
                if  (whichlogger->numThreads != m_value) logImmutable();
                break;

           case LOGGER_TIMESTAMP:
                if  (!m_isbool) result = -1; 
                else whichlogger->timestamp = m_value;
                break;

           case LOGGER_DESTSTDOUT:
                if  (!m_isbool) result = -1; 
                else whichlogger->destStdout = m_value;
                break;

           case LOGGER_DESTDEBUG:
                if  (!m_isbool) result = -1; 
                else whichlogger->destDebug = m_value;
                break;

           case LOGGER_DESTFILE:
                if  (!m_isbool) result = -1; 
                else whichlogger->destFile = m_value;
                break;

           case LOGGER_DESTSOCKET:
                if  (!m_isbool) result = -1; 
                else whichlogger->destSocket = m_value;
                break;

           case LOGGER_DESTOTHER:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else whichlogger->destOther = m_value;
                break;

           case LOGGER_MAXLINES:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else whichlogger->maxlines = m_value;
                break;

           case LOGGER_BACKUP:
                if  (!m_isbool) result = -1; 
                else whichlogger->backup = m_value;
                break;

           case LOGGER_FLUSH:
                if  (!m_isbool) result = -1; 
                else whichlogger->flush = m_value;
                break;

           case LOGGER_ISFULLPATH:
                if  (!m_isbool) result = -1;
                else whichlogger->isFullpath = m_value;
                break;

           case LOGGER_FILEPATH:
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) result = -1;
                else ACE_OS::strcpy(whichlogger->filepath, this->begval);
                break;

           case LOGGER_SOCKET_PORT:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else whichlogger->port = m_value;
                break;

           case LOGGER_SOCKET_CONNECTINTERVAL:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else whichlogger->socketConnectIntervalSeconds = m_value;
                break;

           case LOGGER_DESTLOGSERVER:
                if  (!m_isbool) result = -1; 
                else whichlogger->destLogServer = m_value;
                break;



           default: result = -1; 
         }     
         break;

       
    case IPCADAPTER:    
         switch(m_item)
         {
           case IPCADAPTER_MAXMAPSIZE: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else ipcAdapter.maxMapSize = m_value;
                break;
           default: result = -1; 
         }
         break;


    case DIAGNOSTICS:    
         switch(m_item)
         {
           case DIAGNOSTICS_FLAGS:          // External hex string               
                diagnostics.flags  = (unsigned int)strtol(this->begval, &m_p, 16);
                MmsAs::isLoggingResxUsage = (diagnostics.flags & MMS_DIAG_LOG_RESXUSE) != 0;
                break;

           case DIAGNOSTICS_FLAGS_B:        // External hex string
                diagnostics.flagsB = (unsigned int)strtol(this->begval, &m_p, 16);
                break;

           case DIAGNOSTICS_LOG_RESOURCE_STATUS: 
                if  (!m_isbool) result = -1; 
                else diagnostics.logResourceStatus = m_value;
                break;

           case DIAGNOSTICS_SHUTDOWN_AT_HEARTBEAT: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else diagnostics.shutdownAtHearbeat = m_value;
                break;

           case DIAGNOSTICS_BREAK_AT_ALLOC: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else diagnostics.debugBreakAtAllocation = m_value;
                break;

           case DIAGNOSTICS_LOG_OUTBOUND_MESSAGES:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else diagnostics.logOutboundMessages = m_value;
                break;

           default: result = -1; 
         }
         break;


    case CLIENT:       
         switch(m_item)
         { 
           case CLIENT_PROTOCOLS:           // External hex string              
                if  (isInitialLoad) 
                     clientParams.ipcAdapters  = (unsigned int)strtol(this->begval, &m_p, 16);
                else
                if  (clientParams.ipcAdapters != (unsigned int)strtol(this->begval, &m_p, 16))
                     logImmutable();
                break;
                 
           case CLIENT_MSMQ_APP_QUEUENAME:
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) result = -1;
                else ACE_OS::strcpy(clientParams.msmqAppQueueName, this->begval); 
                break;
        
           case CLIENT_MSMQ_MMS_QUEUENAME: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) result = -1; 
                else ACE_OS::strcpy(clientParams.msmqMmsQueueName, this->begval); 
                break;
       
           case CLIENT_MSMQ_APP_MACHINENAME: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) result = -1; 
                else ACE_OS::strcpy(clientParams.msmqAppMachineName, this->begval); 
                break;
      
           case CLIENT_MSMQ_MMS_MACHINENAME: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) result = -1; 
                else ACE_OS::strcpy(clientParams.msmqMmsMachineName, this->begval); 
                break;
     
           case CLIENT_MSMQ_MAX_MESSAGE_AGE_SECS: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT)  result = -1; 
                else clientParams.maxMqMessageAgeSecs = m_value;
                break;

           case CLIENT_MSMQ_TIMEOUT_MSECS: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT)  result = -1; 
                else clientParams.msmqTimeoutMsecs = m_value;
                break;
        
           case CLIENT_MSMQ_HEARTBEAT_INTERVAL:   
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT)  result = -1; 
                else clientParams.heartbeatIntervalSecs = m_value;
                break;  

           case CLIENT_RESPOND_ON_ASYNC_EXECUTE:   
                if  (!m_isbool) result = -1; 
                else clientParams.respondOnAsyncExecute = m_value;
                break;

           case CLIENT_TEST_ADAPTER_ITERATIONS:   
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT)  result = -1; 
                else clientParams.serverTestAdapterIterations = m_value;
                break;

           case CLIENT_PERMIT_DEFAULT_APP_QUEUE:   
                if  (!m_isbool) result = -1; 
                else clientParams.permitDefaultAppQueue = m_value;
                break;

           case CLIENT_HEARTBEAT_ACK_EXPECTED:   
                if  (!m_isbool) result = -1; 
                else clientParams.heartbeatAckExpected = m_value;
                break;

           case CLIENT_HEARTBEAT_ACKS_MISSABLE:   
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT)  result = -1; 
                else clientParams.heartbeatAcksMissable = m_value;
                break;

           default: result = -1; 
         }
         break;   


    case SERVER:
         switch(m_item)
         {             
           case SERVER_THREADPOOL_SIZE_FACTOR:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.serviceThreadPoolSizeFactor = m_value;
                break;
    
           case SERVER_AUDIO_BASEPATH:
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) 
                     result = -1;
                else 
                {    ACE_OS::strcpy(serverParams.audioBasePath, this->begval);
                     result = this->editPath(serverParams.audioBasePath);
                     MMSLOG((LM_INFO,"CFIG audio base path is %s\n", 
                             serverParams.audioBasePath));
                }
                break;

           case SERVER_CMD_TIMEOUT_MSECS_DEFAULT:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.commandTimeoutMsecsDefault = m_value;
                break;

           case SERVER_CMD_TIMEOUT_MSECS_CONNECT:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.commandTimeoutMsecsConnect = m_value;
                break;

           case SERVER_CMD_TIMEOUT_MSECS_PLAY:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.commandTimeoutMsecsPlay = m_value;
                break;

           case SERVER_CMD_TIMEOUT_MSECS_RECORD:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.commandTimeoutMsecsRecord = m_value;
                break;

           case SERVER_CMD_TIMEOUT_MSECS_RECORDTRAN:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.commandTimeoutMsecsRecordTransaction = m_value;
                break;

           case SERVER_CMD_TIMEOUT_MSECS_GETDIGITS:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.commandTimeoutMsecsGetDigits = m_value;
                break;

           case SERVER_CMD_TIMEOUT_MSECS_VOICEREC:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.commandTimeoutMsecsVoiceRec = m_value;
                break;

           case SERVER_DEFAULT_MAXSIL_SECS_PLAY: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.defaultMaxSilenceSecondsPlay = m_value;
                break;

           case SERVER_DEFAULT_MAXSIL_SECS_RECORD: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.defaultMaxSilenceSecondsRecord = m_value;
                break;

           case SERVER_DEFAULT_MAXDELAY_SECS_DIGITS: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.defaultMaxDelaySecondsReceive = m_value;
                break;

           case SERVER_DEFAULT_MAX_SECS_TONE: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.defaultMaxSecsTone = m_value;
                break;

           case SERVER_EVENT_WAIT_FOR_DEPENDENCY_MS: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.eventWaitForDependencyMsecs = m_value;
                break;

           case SERVER_CONNECT_ASYNC: 
                if  (!m_isbool) result = -1; 
                else serverParams.connectAsync = m_value;
                break;

           case SERVER_SESSION_MON_INTERVAL_SECS: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.sessionMonitorIntervalSeconds = m_value;
                break;

           case SERVER_DEFAULT_BITRATE_PLAY: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else
                if  (0 == (m_n = identifyBitrate(m_value))) result = -1; 
                else serverParams.defaultBitRatePlay = m_n;
                break;

           case SERVER_DEFAULT_SAMPLESIZE_PLAY: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else
                if  (0 == (m_n = identifySamplesize(m_value))) result = -1; 
                else serverParams.defaultSampleSizePlay = m_n;
                break;

           case SERVER_DEFAULT_FORMAT_PLAY: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) result = -1; 
                else
                if  (0 == (m_n = identifyFileFormat(this->begval))) result = -1; 
                else serverParams.defaultFormatPlay = m_n;
                break;

           case SERVER_DEFAULT_FILETYPE_PLAY: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) result = -1; 
                else
                if  (0 == (m_n = identifyFiletype(this->begval))) result = -1; 
                else serverParams.defaultFileTypePlay = m_n;
                break;

           case SERVER_DEFAULT_BITRATE_RECORD: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else
                if  (0 == (m_n = identifyBitrate(m_value))) result = -1; 
                else serverParams.defaultBitRateRecord = m_n;
                break;

           case SERVER_DEFAULT_SAMPLESIZE_RECORD: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else
                if  (0 == (m_n = identifySamplesize(m_value))) result = -1; 
                else serverParams.defaultSampleSizeRecord = m_n;
                break;

           case SERVER_DEFAULT_FORMAT_RECORD: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) result = -1; 
                else
                if  (0 == (m_n = identifyFileFormat(this->begval))) result = -1; 
                else serverParams.defaultFormatRecord = m_n;
                break;

           case SERVER_DEFAULT_FILETYPE_RECORD: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) result = -1; 
                else
                if  (0 == (m_n = identifyFiletype(this->begval))) result = -1; 
                else serverParams.defaultFileTypeRecord = m_n;
                break;

           case SERVER_REASSIGN_IDLE_VOX:
                if  (!m_isbool) result = -1; 
                else serverParams.reassignIdleVoiceResources = m_value;
                break;

           case SERVER_WAIT_FOR_VOX_SECS:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.waitForVoiceResourceSeconds = m_value;
                break;

           case SERVER_WAIT_FOR_VOX_MSECS:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.waitForVoiceResourceMsecs = m_value;
                break;

           case SERVER_IDLE_SELECTION_STRATEGY:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.idleDeviceSelectionStrategy = m_value;
                // MMS_STRATEGY_IDLED_MOST_RECENTLY 0, MMS_STRATEGY_IDLELONGEST 1
                break;

           case SERVER_POST_PROVISIONAL_RESULT:
                if  (!m_isbool) result = -1; 
                else serverParams.postProvisionalResult = m_value;
                break;

           case SERVER_CLEAN_LOGS_AFTER_DAYS:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.cleanLogsAfterDays = m_value;
                break;

           case SERVER_CLEAN_DIRS_AFTER_MINUTES:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.cleanDirsIntervalMinutes = m_value;
                break;

           case SERVER_CONFEREE_TIMEOUT_MINUTES:  // 0 = no timeout
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.inactiveConfereeTimeoutMinutes = m_value;
                break;

           case SERVER_CONFEREE_TIMEOUT_STRATEGY: // 0 = no timeout
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.inactiveConfereeTimeoutStrategy = m_value;
                break;

           case SERVER_DIGIT_SEQUENCE_SECONDS:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.digitSequenceIntervalSeconds = m_value;
                break;

           case SERVER_CALL_STATE_DURATION:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.defaultMonitorCallStateDuration = m_value;
                break;

           case SERVER_SESSION_TIMEOUT_SECS_DEFAULT:      
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.sessionTimeoutSecondsDefault = m_value;
                break;

           case SERVER_TIMEOUT_SECONDS:     // 0 = no timeout
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.serverTimeoutSeconds = m_value;
                break;

           case SERVER_THREAD_PING_INTERVAL_SECONDS:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.threadMonitorIntervalSeconds = m_value;
                break;

           case SERVER_SET_UNHANDLED_EXCEPTION_TRAP:
                if  (!m_isbool) result = -1; 
                else serverParams.setUnhandledExceptionTrap = m_value;
                break;

           case SERVER_CRASH_ON_VOICE_EVENT:
                if  (!m_isbool) result = -1; 
                else serverParams.crashServerOnVoiceEvent = m_value;
                break;

           case SERVER_OVERWRITE_DUMP_FILE:
                if  (!m_isbool) result = -1; 
                else serverParams.overwriteDumpFile = m_value;
                break;

           case SERVER_POINTER_VALIDATION_LEVEL:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.pointerValidationLevel = m_value;
                break;

           case SERVER_DUMP_BASEPATH:
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) 
                     result = -1;
                else 
                {    ACE_OS::strcpy(serverParams.dumpBasePath, this->begval);
                     result = this->editPath(serverParams.dumpBasePath);                     
                }
                break;

           case SERVER_RESX_DETAIL_LOG_INTERVAL_MS:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.resourceDetailLogIntervalMsecs = m_value;
                break;

           case SERVER_SYSTEM_STATS_LOG_INTERVAL_MS:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.systemStatsLogIntervalMsecs = m_value;
                break;

           case SERVER_MAX_DIRECTORY_SCAN_COUNT:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.maxDirectoryScanCount = m_value;
                break;

           case SERVER_FLATMAP_IPC_PORT:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.defaultFlatmapIpcPort = m_value;
                break;

           case SERVER_HAIRPIN_OPTS:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.hairpinOpts = m_value;
                break;

           case SERVER_HAIRPIN_PROMOTION_OPTS:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.hairpinPromotionOpts = m_value;
                break;

           case SERVER_PENDING_COMMAND_STRATEGY:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else serverParams.pendingCommandStrategy = m_value;
                break;
 
           case SERVER_REQUIRE_CONCURRENT_OP_CONFIRM:
                if  (!m_isbool) result = -1; 
                else serverParams.requireConcurrentOpConfirmation = m_value;
                break;

           case SERVER_CACHE_BUSY_DISCONNECT:
                if  (!m_isbool) result = -1; 
                else serverParams.cacheBusyDisconnect = m_value;
                break;

           case SERVER_DRIVE_MAPPING_LIST:
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) 
                     result = -1;
                else 
                {    
                  ACE_OS::strcpy(serverParams.driveMappingList, this->begval);
                  parseDriveMappings();
                }
                break;

           case SERVER_DISREGARD_LOCALE_DIRECTORIES:
                if  (!m_isbool) result = -1; 
                else serverParams.disregardLocaleDirectories = m_value;
                break;

           case SERVER_DEFAULT_APPNAME:
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) 
                     result = -1;
                else ACE_OS::strncpy(serverParams.defaultAppName, this->begval, MMS_MAX_APPNAMSIZE);
                break;

           case SERVER_DEFAULT_LOCALE:
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) 
                     result = -1;
                else ACE_OS::strncpy(serverParams.defaultLocale, this->begval, MMS_MAX_LOCALESIZE);
                break;

           default: result = -1; 
         }

         break;


    case HMP:
         switch(m_item)
         {
           case HMP_MAXCONNECTIONS: 
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else 
                if  (isInitialLoad) 
                     hmp.maxConnections  = m_value;
                else
                if  (hmp.maxConnections != m_value) logImmutable();
                break;

           case HMP_MAXINITIALIPRESOURCES:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else hmp.maxInitialResourcesIP = m_value;
                break;

           case HMP_MAXINITIALVOICERESOURCES:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else hmp.maxInitialResourcesVoice = m_value;
                break;

           case HMP_MAXINITIALCONFRESOURCES:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else hmp.maxInitialResourcesConf = m_value;
                break;

           case HMP_MAXWAITSECSSTARTSERVICE:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else hmp.startSvcMaxWaitSecs = m_value;
                break;

           case HMP_VOLUME:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else hmp.volume = m_value;
                break;

           case HMP_SPEED:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else hmp.speed = m_value;
                break;

           case HMP_RTP_PORT_BASE:             
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else hmp.rtpPortBase = m_value;
                break;

           case HMP_INTERNAL_LICENSING_MODE:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else hmp.internalLicensingMode = m_value;
                break;

           case HMP_CISCO_DEVKEY:
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) 
                     result = -1;
                else result = this->validateDevKey(this->begval);
                break;

           case HMP_LICENSE_HEADROOM_PERCENT:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) 
                     result = -1; 
                else
                if  (m_value < 0 || m_value > MMS_MAX_LICENSE_HEADROOM_PERCENT)
                {
                     hmp.licenseHeadroomPercent = m_value < 1? 0: MMS_MAX_LICENSE_HEADROOM_PERCENT;
                     MMSLOG((LM_DEBUG,"CFIG licenseHeadroomPercent invalid - making %s\n", 
                             hmp.licenseHeadroomPercent));
                }
                else hmp.licenseHeadroomPercent = m_value;  
                break;

           case HMP_DISALLOW_LICENSE_HEADROOM:
                if  (!m_isbool) result = -1; 
                else hmp.disallowLicenseHeadroom = m_value;
                break;

           case HMP_ASSUME_SDK_ON_LICENSE_FAIL:
                if  (!m_isbool) result = -1; 
                else hmp.assumeSdkOnLicenseFailure = m_value;
                break;

           case HMP_DISABLE_LICENSE_MODE:
                if  (!m_isbool) result = -1; 
                else hmp.disableCeiling = m_value;
                break;

           case HMP_LOG_MAX_RESOURCES:
                if  (!m_isbool) result = -1; 
                else hmp.logMaxResources = m_value;
                break; 
 
           case HMP_SET_DSCP_EXEDITE_FORWARD:
                if  (!m_isbool) result = -1; 
                else hmp.setDscpExpediteForward = m_value;
                break;  

           default: result = -1; 
         }
         break;


    case REPORTS:
         switch(m_item)
         {
           case REPORTS_HIWATER_G711:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else reports.hiwaterG711Resx = m_value;
                break;

           case REPORTS_LOWATER_G711:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else reports.lowaterG711Resx = m_value;
                break;

           case REPORTS_HIWATER_G729_ETC:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else reports.hiwaterG729EtcResx = m_value;
                break;

           case REPORTS_LOWATER_G729_ETC:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else reports.lowaterG729EtcResx = m_value;
                break;

           case REPORTS_HIWATER_VOICE:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else reports.hiwaterVoiceResx = m_value;
                break;

           case REPORTS_LOWATER_VOICE:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else reports.lowaterVoiceResx = m_value;
                break;

           case REPORTS_HIWATER_CONFERENCE:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else reports.hiwaterConferenceResx = m_value;
                break;

           case REPORTS_LOWATER_CONFERENCE:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else reports.lowaterConferenceResx = m_value;
                break;

           case REPORTS_HIWATER_CONFERENCES:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else reports.hiwaterConferences = m_value;
                break;

           case REPORTS_LOWATER_CONFERENCES:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else reports.lowaterConferences = m_value;
                break;

           case REPORTS_HIWATER_TTS:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else reports.hiwaterTtsResx = m_value;
                break;

           case REPORTS_LOWATER_TTS:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else reports.lowaterTtsResx = m_value;
                break;

           case REPORTS_HIWATER_SPEECHREC:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else reports.hiwaterSpeechRecResx = m_value;
                break;

           case REPORTS_LOWATER_SPEECHREC:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else reports.lowaterSpeechRecResx = m_value;
                break;

           case REPORTS_HIWATER_SPEECHINT:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else reports.hiwaterSpeechIntResx = m_value;
                break;

           case REPORTS_LOWATER_SPEECHINT:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else reports.lowaterSpeechIntResx = m_value;
                break;

           case REPORTS_DISABLE_ALARMS:
                if  (!m_isbool) result = -1; 
                else reports.disableAlarms = m_value;
                break;

           case REPORTS_DISABLE_STATS:
                if  (!m_isbool) result = -1; 
                else reports.disableStats = m_value;
                break;

           case REPORTS_ALARM_THRESHOLD:
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else reports.alarmThreshold = m_value;
                break;

          case REPORTS_STAT_SERVER_PORT:            
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else reports.statServerPort = m_value;
                break;

           case REPORTS_STAT_SERVER_IP:
                if  (m_datatype  != MMSCONFIG_DATATYPE_CHAR) result = -1; 
                else ACE_OS::strncpy(reports.statServerIP, this->begval,
                                     sizeof(reports.statServerIP)-1); 
                break;

           case REPORTS_MONITOR_CONX_SECONDS:            
                if  (m_datatype  != MMSCONFIG_DATATYPE_INT) result = -1; 
                else reports.monitorConnectionIntervalSeconds = m_value;
                break;

           default: result = -1; 
         }
         break;
  }

  return result;
}



int MmsConfig::identifyItem(keytables* ks)  // Validate item (obj.item) token 
{
  this->keys = NULL;
  int   size = 0;

  switch(m_object)                          // Assign itemname table
  { 
    case TOPLEVEL:     keys = ks->nonDottedKeys;  size = ks->nNonDottedKeys;  break;
    case SERVERLOGGER: keys = ks->loggerKeys;     size = ks->nLoggerKeys;     break;
    case CLIENTLOGGER: keys = ks->loggerKeys;     size = ks->nLoggerKeys;     break;
    case CLIENT:       keys = ks->clientKeys;     size = ks->nClientKeys;     break;
    case SERVER:       keys = ks->serverKeys;     size = ks->nServerKeys;     break;
    case HMP:          keys = ks->hmpKeys;        size = ks->nHmpKeys;        break;
    case MEDIA:        keys = ks->mediaKeys;      size = ks->nMediaKeys;      break;
    case REPORTS:      keys = ks->reportsKeys;    size = ks->nReportsKeys;    break;
    case LOGGER:       keys = ks->loggerKeys;     size = ks->nLoggerKeys;     break;
    case DIAGNOSTICS:  keys = ks->diagnosticsKeys;size = ks->nDiagnosticsKeys;break;
    case IPCADAPTER: 
         keys = ks->ipcAdapterKeys; size = ks->nIpcAdapterKeys; 
         break;
  }

  if  (keys == NULL) return -1; 

  char* q = this->end; q++;
  char  save = *q;
  *q = '\0';                                // Terminate itemname string
                                            // Identify item
  for(int i = 0; i < size; i++, keys++)     
  {
    if  (stricmp(keys->key, this->begkey) == 0)
    {    m_item = keys->id;
         break;
    }
  }

  #ifdef MMSCONFIGTEST
  if  (m_item) MMSLOG((LM_DEBUG,"CFIG '%s'\n", this->begkey));
  #endif
  *q = save;
  return m_item? MMSCONFIG_TOKEN_FOUND: -1; 
}


                                             
int MmsConfig::identifyValue()              // Parse out and type item value
{
  m_length = m_datatype = m_isbool = 0;
  this->begval = this->endval = 0; 

  char*  p = this->equal; p++;              // Start after equal sign
  while(*p && ISWHITESPACE(p)) p++;         // Find first noblank character
  if   (ISENDOFLINE(p) || *p == '#') return -1;

  char*  q = p;
  while(NOTENDOFLINE(q) && *q != '#') q++;  // Find end of line or comment

  q--;
  while(ISWHITESPACE(q)) q--;               // Strip trailing whitespace
  if  (*p == '\"')                          // If quoted string ...
  {    if  (*q != '\"') return -1;          // Ensure close quote
       p++; q--;                            // Remove quotes
       m_datatype = MMSCONFIG_DATATYPE_CHAR;
  }

  m_length = q - p + 1;                     // Determine length of value
  if  (m_length < 1) return -1; 

  this->begval = p;                         // Set value boundaries
  this->endval = q;
  *++q = '\0';                              // Add terminator for logging
                                            // Determine data type of value
  int digits = 0, dots = 0, isneg = 0, expecteddigits = m_length; 
              
  for(int i=0; i < m_length; i++, p++)
  {
    if  (isdigit(*p))
         digits++;
    else
    if  (*p == '-' && i == 0)               // Negative sign?
         isneg = TRUE;
    else                                    // Decimal point?
    if  (*p == '.')
         dots++;      
  }

  if  (isneg && digits)
       expecteddigits--;
  if  ((dots == 1) && digits)
       expecteddigits--;

  if  (m_datatype);                         // When quoted, always character
  else
  if ((digits == expecteddigits) && !dots)
  {
       m_datatype = MMSCONFIG_DATATYPE_INT;
       m_value    = ACE_OS::atoi(this->begval);
       if  (m_value == 0 || m_value == 1)
            m_isbool = TRUE;
  }
  else
  if  ((dots == 1) && (digits == expecteddigits))
  {
       m_datatype = MMSCONFIG_DATATYPE_FIXED;
       m_fixedval = ACE_OS::strtod(this->begval, &q);
  }
  else m_datatype = MMSCONFIG_DATATYPE_CHAR;

  #ifdef MMSCONFIGTEST
  MMSLOG((LM_DEBUG,"CFIG '%s'\n",begval));
  #endif
  return MMSCONFIG_TOKEN_FOUND;
}



int MmsConfig::identifyBitrate(const int configval)
{
  // Converts bitrate as specified in config file to bitrate  as stored
  // in config object
  switch(configval) 
  { case 6: return MMS_RATE_KHZ_6;
    case 8: return MMS_RATE_KHZ_8;
    case 11:return MMS_RATE_KHZ_11; 
  }
  return 0;
}



int MmsConfig::identifySamplesize(const int configval)
{
  // Converts samplesize as specified in config file to samplesize  as stored
  // in config object
  switch(configval) 
  { case 4: return MMS_SAMPLESIZE_BIT_4;
    case 8: return MMS_SAMPLESIZE_BIT_8;
    case 16:return MMS_SAMPLESIZE_BIT_16; 
  }
  return 0;
}



int MmsConfig::identifyFileFormat(const char* configval)
{
  // Converts format as specified in config file to format as stored
  // in config object
  int  format = 0;
  if  (stricmp(configval, "ulaw")  == 0) format = MMS_FORMAT_MULAW; else
  if  (stricmp(configval, "mulaw") == 0) format = MMS_FORMAT_MULAW; else
  if  (stricmp(configval, "alaw")  == 0) format = MMS_FORMAT_ALAW;  else
  if  (stricmp(configval, "adpcm") == 0) format = MMS_FORMAT_ADPCM; else
  if  (stricmp(configval, "pcm")   == 0) format = MMS_FORMAT_PCM;  
  return format;
}



int MmsConfig::identifyFiletype  (const char* configval)
{
  // Converts filetype as specified in config file to filetype as stored
  // in config object
  int  type = 0;
  if  (stricmp(configval, "vox") == 0) type = MMS_FILETYPE_VOX; else
  if  (stricmp(configval, "wav") == 0) type = MMS_FILETYPE_WAV;   
  return type;
}



int MmsConfig::editPath(char* path)
{
  // Remove any trailing slash and whitespace from path
  int result = 0;

  while(1)
  {
    int pathLen = ACE_OS::strlen(path);
    if (pathLen < 2) 
    {   result = -1;
        break;
    }
    
    char* lastChar = path + (pathLen - 1);

    if (*lastChar == '\\' || *lastChar == '/' || *lastChar <= ' ')
        *lastChar  = '\0';
    else break;    
  }

  return result;   
}



int MmsConfig::validateDevKey(char* key)
{
  // Validates the developer override key config item.
  // This key enables overiding of various functionality such as licensing.
  // Valid key is of the form "$/nnnnnnn/", where nnnnnn = some multiple of 2^17+1
  // The mechanism is not robust (duh) -- don't advertise the valid keys obviously.
   
  int result = -1;
  static char* errmask = "CFIG bogus internal key specified: %s\n";
  static char* okmask  = "CFIG internal override key configured\n";

  do {

  const int len = key? strlen(key): 0;
  if (len < 1) 
  {   result = 0;  // No override key
      break;
  }

  if (len > MMS_MAX_DEVKEYSIZE || key[0] != '$' || key[1] != '/' || key[len-1] != '/')
  {   MMSLOG((LM_ERROR,errmask,key));
      break;
  }

  char *p = key+2, *q = &key[len-1]; 
  char  c = *q; *q = 0;
  const int n = atoi(p); 
  *q = c;
  int div = 1; div <<= 17;  
 
  if (n == 0 || n % ++div != 0)  // not divisible by 2^17+1
  {   MMSLOG((LM_ERROR,errmask,key));
      break;
  }
 
  ACE_OS::strcpy(hmp.ciscoDevKey, key);
  calculated.isValidDevkey = 1;
  MMSLOG((LM_ALERT,okmask));
  result = 0;
  
  } while(0);

  return result;
}



void MmsConfig::logImmutable()
{
  static char* mask = "CFIG cannot reset %s.%s\n";
  if  (this->keyi == NULL || this->keys == NULL) return; 
  char* objname  = keyi->key? keyi->key: "";
  char* itemname = keys->key? keys->key: "";
  MMSLOG((LM_WARNING,mask,objname,itemname));
}



int MmsConfig::getConfigFilePath(char* outpath, const int outpathlength)
{         
  #ifdef MMS_WINPLATFORM 

  HKEY  hkey;
  DWORD dwResult, dwDataSize, dwDataType; 
                                            
  dwResult = RegOpenKeyEx(HKEY_LOCAL_MACHINE, MMS_REGKEY, 0, KEY_ALL_ACCESS, &hkey);
  dwResult = RegQueryValueEx(hkey, MMS_REGCONFIGPATH, NULL, NULL, NULL, &dwDataSize);
  if  (dwDataSize)
       dwResult = RegQueryValueEx
      (hkey, MMS_REGCONFIGPATH, NULL, &dwDataType, (BYTE*)outpath, &dwDataSize);
  RegCloseKey(hkey);

  if  (dwResult != ERROR_SUCCESS)           // If not in registry use default
       ACE_OS::strncpy(outpath, MMS_CONFIG_FILE_PATH, outpathlength-1);

  #else

  ACE_OS::strncpy(outpath, MMS_CONFIG_FILE_PATH, outpathlength-1);

  #endif

  return 0;
}



void MmsConfig::parseDriveMappings()
{
  char mappingToken[] = ";\t\n\0";

  char* mapping = ACE_OS::strtok(serverParams.driveMappingList, mappingToken);

  while(mapping)
  {
    char buff[MAXPATHLEN];
    ACE_OS::memset(&buff, 0, sizeof(buff));
    ACE_OS::strcpy(buff, mapping);
    addDriveMapping(buff);
    mapping = ACE_OS::strtok(NULL, mappingToken);
  }
}



void MmsConfig::addDriveMapping(char* mapping)
{
  char* p = ACE_OS::strchr(mapping, '=');

  if (p && (p+1))
  {
    std::string value = std::string(p+1);
    *p = 0;
    std::string key = std::string(mapping);
    driveMappings[key] = value;
    MMSLOG((LM_NOTICE,"CFIG drive mapping %s=%s\n", key.c_str(), value.c_str()));
  }
}



int MmsConfig::getDriveMappingFullPath(std::string& path)
{
  driveMappingsTable::iterator i = driveMappings.begin();

  for(; i != driveMappings.end(); i++)
  {
    std::string key = i->first;
    std::string value = i->second;

    std::string::size_type start = 0; 
    std::string::size_type offset = 0; 

    if((start = path.find(key, offset)) != std::string::npos) 
    { 
        path.replace(start, key.length(), value); 
        return 1;   // success
    } 
  }  

  return 0;         // failure
}



