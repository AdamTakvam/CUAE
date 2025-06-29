// 
// mmsCommon.h 
//
// shared items not project-specific and without dependencies
//     
#ifndef MMS_COMMON_H
#define MMS_COMMON_H
#ifdef  MMS_WINPLATFORM
#pragma once
#endif

#include <stdio.h>
#include <string.h>

#define MMS_MAX_APPNAMSIZE        127       // Max characters in an application name
#define MMS_MAX_LOCALESIZE         11       // Max characters in a locale directory name    


enum statCategories
{    
  MMS_STAT_CATEGORY_SERVER      = 2111,  
  MMS_STAT_CATEGORY_RTP         = 2101, 
  MMS_STAT_CATEGORY_VOX         = 2100,  
  MMS_STAT_CATEGORY_G729        = 2102,
  MMS_STAT_CATEGORY_CONFRESX    = 2103,
  MMS_STAT_CATEGORY_TTS         = 2105,
  MMS_STAT_CATEGORY_CSP         = 2104,
  MMS_STAT_CATEGORY_CONFSLOTS   = 2106,
  MMS_STAT_CATEGORY_CONFERENCES = 2107,
  MMS_STAT_CATEGORY_ASR         = 2108,     

  MMS_STAT_CATEGORY_COUNT       = 10,       // Number of entries in this enumeration
};



struct MmsAlarmStatParams    // Alarm and Stats objects common parent
{
  unsigned int signature;    // Identifies inheritor's object type
  int  paramType;            // Alarm type or stats type
  int  resourceType;         // MMS_STAT_CATEGORY_XXX
  int  isStaticallyAllocated;// If nonzero, memory is not freed
  int  severity;             // 1 = red, 2 = yellow, 3 = green
};



struct MmsAlarmParams: public MmsAlarmStatParams
{
  // Allocated and populated at point of alarm, pointer passed as msg.param()
  // Note that parmType will be alarm type *index* from alarmTypesIndexes enumeration,
  // and resourceType will be a MMS_STAT_CATEGORY_XXX from statCategories enumeration,
  enum { sig=0xc7d6e5f4, maxTextLength=79 };

  int  isClearingAlarm;
  char text[MmsAlarmParams::maxTextLength+1];
  void settext(char* p) { if (p) strncpy(text, p, maxTextLength); }

  MmsAlarmParams() { }

  MmsAlarmParams(const int alarmType, const int resxType=0, const int sev=0) 
  { 
    clear();
    signature = sig;  
    paramType = alarmType; 
    resourceType = resxType;
    severity  = sev; 
  }

  void clear() { memset(this, 0, sizeof(MmsAlarmParams)); }
};



struct MmsStatsParams: public MmsAlarmStatParams
{
  // Allocated and populated at point of statistic event 
  // Note that paramType will be the same as resource type, MMS_STAT_CATEGORY_XXX
  enum { sig=0xa7b6c5d4 };

  int oldValue;
  int newValue;

  MmsStatsParams() { }

  MmsStatsParams(const int rt, const int ov, const int nv) 
  { 
    clear();
    signature    = sig; 
    paramType    = rt;
    resourceType = rt; 
    oldValue     = ov;
    newValue     = nv;
  }

  void clear() { memset(this, 0, sizeof(MmsStatsParams)); }
};



struct MmsLocaleParams 
{
  char appname[MMS_MAX_APPNAMSIZE+1];
  char locale [MMS_MAX_LOCALESIZE+1];

  void set(char* a, char* l) { setAppName(a); setLocale(l); }

  void set(MmsLocaleParams& p) 
  { memcpy(appname, p.appname, sizeof(appname));
    memcpy(locale,  p.locale,  sizeof(locale)); 
  }

  void setAppName(char* p) { if (p) strncpy(appname, p, MMS_MAX_APPNAMSIZE); } 
  void setLocale (char* p) { if (p) strncpy(locale,  p, MMS_MAX_LOCALESIZE); } 

  int  isEmpty() { return *appname == 0 || *locale == 0; }
  void clear()   { memset(this,0,sizeof(MmsLocaleParams)); }
  MmsLocaleParams() { clear(); }
};



struct mmsHmpRegistryResourceCounts
{
  int voiceResources;
  int lobitrateResources;
  int cspResources;
  int conferenceResources;
  int ipResources;

  mmsHmpRegistryResourceCounts() { memset(this,0,sizeof(mmsHmpRegistryResourceCounts)); }
};



#endif // #ifndef MMS_COMMON_H