/* ****************************License******************************** 
** Copyright 1994-2003.  SpeechWorks International, Inc.  All rights
** reserved.
**  
** Use of this software is subject to certain restrictions and limitations
** set forth in a license agreement entered into between SpeechWorks
** International Inc. and the licensee of this software.  Please refer
** to the license agreement for license use rights and restrictions.
**
** Portions of the OpenSpeech Recognizer Software are subject to
** copyrights of AT&T Corp., E-Speech Corporation, Bell Communications
** Research, Inc., European Telecommunications Standards Institute and
** GlobeTrotter Software, Inc.
**      
** SpeechWorks and OpenSpeech are  registered trademarks, and
** SpeechWorks Here, OpenSpeech DialogModules, DialogModules and the
** SpeechWorks logo are trademarks of SpeechWorks International, Inc.
** in the United States and other countries
*/

#ifndef _SWItrcCleanup_H
#define _SWItrcCleanup_H

/* Ensure that code that calls our functions is compiled correctly by telling
   the compiler about our calling convention and structure packing alignment. */

#if defined(_MSC_VER)		 /* Microsoft Visual C++ */
  #if !defined(ALTAPI)
    #define ALTAPI __stdcall
  #endif
  #pragma pack(push, 8)
#elif defined(__BORLANDC__)	 /* Borland C++ */
  #if !defined(ALTAPI)
    #define ALTAPI __pascal
  #endif
  #pragma option -a8
#elif defined(__WATCOMC__)	 /* Watcom C++ */
  #if !defined(ALTAPI)
    #define ALTAPI __stdcall
  #endif
  #pragma pack(push, 8)
#else				 /* Any other includng Unix */
  #if !defined(ALTAPI)
    #define ALTAPI
  #endif
#endif

#ifdef __cplusplus
extern "C" {
#endif

extern int ALTAPI SWItrcThreadCleanup();


#ifdef __cplusplus
}
#endif


/* Reset the structure packing alignments for different compilers. */

#if defined(_MSC_VER)            /* Microsoft Visual C++ */
  #pragma pack(pop)
#elif defined(__BORLANDC__)      /* Borland C++ */
  #pragma option -a.
#elif defined(__WATCOMC__)       /* Watcom C++ */
  #pragma pack(pop)
#endif

#endif // include guard
