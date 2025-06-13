#ifdef WIN32

#pragma once

#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers
#define _WIN32_WINNT 0x0400     // Required for ACE

#include <windows.h>

#endif // WIN32