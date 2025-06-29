//
// mmsLeakTest.h
//
// memory leak test support   
// facilitates linking of debug heap and modifying of operator new  
//
#ifndef MMSLEAKTEST_H
#define MMSLEAKTEST_H

#ifdef  MMS_WINPLATFORM
#pragma once

// Compile switch to enable memory leak detection via linking the debug heap and
// appropriate crt calls in various spots in the code, likewise compiled in or
// out depending on whether MMS_ENABLE_MEMORY_LEAK_DETECTION is defined.  

// #define MMS_ENABLE_MEMORY_LEAK_DETECTION  // Comment out when not leak testing

#ifdef  MMS_ENABLE_MEMORY_LEAK_DETECTION

#define _CRTDBG_MAP_ALLOC
#include <stdlib.h>
#include <crtdbg.h>
#pragma warning(disable:4291)
#pragma warning(disable:4786)

#define MMS_MEMLEAK_NEW  new(_NORMAL_BLOCK, __FILE__, __LINE__)
       
#else   // MMS_ENABLE_MEMORY_LEAK_DETECTION
#define MMS_MEMLEAK_NEW

#endif  // MMS_ENABLE_MEMORY_LEAK_DETECTION

#endif  // MMS_WINPLATFORM

#endif  // MMSLEAKTEST_H