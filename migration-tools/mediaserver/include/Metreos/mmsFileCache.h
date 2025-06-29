#ifndef MMS_FILECACHE_H
#define MMS_FILECACHE_H
#ifdef MMS_WINPLATFORM
#pragma once
#endif
#include <map>
#include <set>
#include <list>
#include "mms.h"



class MmsFileCache                          //  
{  
  
  struct FCB
  { 
    char  URL[MAXPATHLEN];
    char  localpath[MAXPATHLEN];
    FILE* f;
  };

};



 
#endif