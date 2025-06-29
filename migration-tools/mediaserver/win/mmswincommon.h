#if !defined(MMSWINCOMMON_H)
#define MMSWINCOMMON_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif

#define MMS_REGKEY     "Software\\Metreos\\MediaServer"
#define MMS_REGMMSPATH "CommandLine"


#define MMSCONTITLE "Cisco Unified Media Engine Console"
#define MMSHMPTITLE "Intel Dialogic Configuration Manager"

#define MMSWIN_APPTITLE  "Cisco Unified Media Engine"
#define MMSWIN_WINCLASS  "mmswin"

// Note that WM_USER is 0x400 = decimal 1024
#define WM_FROMSERVER        (WM_USER+256) 
#define WM_MMSWIN_TRAYMSG    (WM_USER+257)
#define WM_MMS_REFRESHCONFIG (WM_USER+258)
#define WM_MMS_REFRESHCONFIG (WM_USER+258)
#define WM_MMS_PING          (WM_USER+259)  // 617
#define WM_MMS_PINGBACK      (WM_USER+260)

// Note that WM_NOTIFY is 0x4e = decimal 78
#define NOTIFY_HIDE      1
#define NOTIFY_SHOW      2
#define NOTIFY_CONFIG    3
#define NOTIFY_START     4
#define NOTIFY_STOP      5
#define NOTIFY_QUIT      6
#define NOTIFY_CYCLE     7

#define SW_LIMBO 0xdead 
                                            // Default if not found in registry
#define DEFAULT_MMS_CMDLINE "c:\\Program Files\\Cisco Systems\\Unified Application Environment\\MediaServer\\bin\\mmsserver.exe"



#endif