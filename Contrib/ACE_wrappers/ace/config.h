#ifndef ACE_METREOS_CONFIG_H
#define ACE_METREOS_CONFIG_H
#pragma once

//
// Metreos ACE Windows configuration header
// This must go into c:/ACE_Wrappers/ace directory prior to building ACE
//

#define ACE_HAS_STANDARD_CPP_LIBRARY 1

// By default, tracing code is not compiled.  To compile it in, cause
// ACE_NTRACE to not be defined, and rebuild ACE.
// #define ACE_NTRACE 0  // This ENABLES function tracing. Don't do it, it sucks.

#include "ace/config-win32.h"

#endif ACE_METREOS_CONFIG_H