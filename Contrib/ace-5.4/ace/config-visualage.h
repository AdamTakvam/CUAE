/* -*- C++ -*- */
// config-visualage.h,v 4.5 2003/07/19 19:04:15 dhinton Exp

// This configuration file automatically includes the proper
// configurations for IBM's VisualAge C++ compiler on Win32 and AIX.

#ifndef CONFIG_VISUALAGE_H
#define CONFIG_VISUALAGE_H
#include /**/ "ace/pre.h"

#ifdef __TOS_WIN__
   #include "ace/config-win32.h"
#elif __TOS_AIX__
   #include "ace/config-aix-4.x.h"
#else
   #include "PLATFORM NOT SPECIFIED"
#endif /* __TOS_WIN__ */

#include /**/ "ace/post.h"
#endif //CONFIG_VISUALAGE_H
