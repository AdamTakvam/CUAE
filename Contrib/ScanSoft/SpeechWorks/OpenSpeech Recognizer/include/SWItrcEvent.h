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

#ifndef _SWItrcEvent_H
#define _SWItrcEvent_H

#include <wchar.h>
#include "SWItrcErrors.h"

/* Ensure that code that calls our functions is compiled correctly by
   telling the compiler about our calling convention and structure
   packing alignment. */

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

#ifdef __cplusplus
struct SWItrcEventStream;
struct SWItrcEventHandle;
struct SWItrcContentHandle;
#else
typedef struct SWItrcEventStream { void * dummy; } SWItrcEventStream;
typedef struct SWItrcEventHandle { void * dummy; } SWItrcEventHandle;
typedef struct SWItrcContentHandle { void * dummy; } SWItrcContentHandle;
#endif

#define EVENT_LOG_TRC 113

#define SWITRC_MEDIA_ULAW        L"audio/basic"
#define SWITRC_MEDIA_AURORA      L"audio/x-aurora"
#define SWITRC_MEDIA_WAV         L"audio/x-wav"
#define SWITRC_MEDIA_MEASUREMENT L"application/x-swi-measurement"
#define SWITRC_MEDIA_TEXT        L"text/plain"

/** @name SWItrc 
 ** @memo is the SpeechWorks logging engine
 **
 ** @doc  Copyright 1996-2002 SpeechWorks International, Inc.
 **  All Rights Reserved.
 **/

  int ALTAPI SWItrcEventInitProc(void);

  /*
  ** Event log stream creation, typically one per telephony channel
  */

  int ALTAPI SWItrcEventStreamCreate(SWItrcEventStream **stream);
  int ALTAPI SWItrcEventStreamDestroy(SWItrcEventStream **stream);

  /*
  ** Event log stream specific properties
  */

  int ALTAPI SWItrcGetEventStreamAppName(SWItrcEventStream *stream,
					 wchar_t *appName);
  int ALTAPI SWItrcSetEventStreamAppName(SWItrcEventStream *stream,
					 const wchar_t *appName);

  int ALTAPI SWItrcGetEventStreamChannelId(SWItrcEventStream *stream,
					   wchar_t *channelId);
  int ALTAPI SWItrcSetEventStreamChannelId(SWItrcEventStream *stream,
					   const wchar_t *channelId);
  int ALTAPI SWItrcResetEventStreamCPU(SWItrcEventStream *stream);

  /*
  ** Character logging methods
  */

  int ALTAPI SWItrcLogEventStart(SWItrcEventStream *stream, 
				 const wchar_t *event,
				 SWItrcEventHandle **handle);
  int ALTAPI SWItrcNLogEventStart(SWItrcEventStream *stream, 
				  const char *event,
				  SWItrcEventHandle **handle);
  
  int ALTAPI SWItrcLogEventToken(SWItrcEventHandle *handle, 
				 const wchar_t *token,
				 const wchar_t *value);
  int ALTAPI SWItrcNLogEventToken(SWItrcEventHandle *handle, 
				  const char *token,
				  const char *value);

  int ALTAPI SWItrcLogEventTokenI(SWItrcEventHandle *handle, 
				  const wchar_t *token,
				  int value);
  int ALTAPI SWItrcNLogEventTokenI(SWItrcEventHandle *handle, 
				   const char *token,
				   int value);
  
  int ALTAPI SWItrcLogEventEnd(SWItrcEventHandle **handle);

  /*
  ** Binary content logging methods
  */
  
  /**
   * @name SWItrcLogEventContentOpen
   * @memo Open a handle to log (potentially large or binary) content
   *
   * @doc In situations where large blocks of data need to be logged
   *      and/or the data is binary, this method should be used to
   *      open a content logging handle. Data is written via
   *      SWItrcLogEventContentWrite( ), and the handle is then closed
   *      via SWItrcLogEventContentClose( ). The key/value pair
   *      returned by this method indicates the location of the logged
   *      data, and should be used to reference this content within
   *      the event log.
   *
   * @param  contentType  [IN] MEDIA content type for the data
   * @param  logKey       [OUT] Key name to cross-reference this content
   *                      in an event log entry, valid until
   *                      SWItrcLogEventContentClose( ) is called
   * @param  logValue     [OUT] Value to cross-reference this content
   *                      in an event log entry, valid until
   *                      SWItrcLogEventContentClose( ) is called
   * @param  handle       [OUT] Handle for writing the content via 
   *                      SWItrcLogEventContentWrite( ) and closing it via
   *                      SWItrcLogEventContentClose( )
   *
   * @return SWITRC_SUCCESS on success
   * @return SWITRC_INTERNAL_ERROR on normal failure (binary content
   *           dumps are disabled at the system level)
   */
  int ALTAPI SWItrcLogEventContentOpen(SWItrcEventStream*     stream,
				       const wchar_t*         contentType,
				       const wchar_t**        logKey,
				       const wchar_t**        logValue,
				       SWItrcContentHandle**  handle);

  /**
   * @name SWItrcLogEventContentClose
   * @memo Close a handle for logging (potentially large or binary) content
   *
   * @doc Close a content handle that was previously opened. Closing
   *      a NULL or previously closed handle will result in an error.
   *
   * @param  handle   [IN/OUT] Handle to close, will be
   *                  set to NULL on success
   *
   * @return SWITRC_SUCCESS on success
   */
  int ALTAPI SWItrcLogEventContentClose(SWItrcContentHandle**  handle);

  /**
   * @name SWItrcLogEventContentWrite
   * @memo Write (potentially large or binary) content to a logging handle
   *
   * @doc Write data to a content handle that was previously opened.
   *
   * @param buffer   [OUT] Buffer of data to write to the handle
   * @param buflen   [IN]  Number of bytes to write
   * @param nwritten [OUT] Number of bytes actual written, may be less then
   *                 buflen if an error is returned
   * @param handle   [IN] Handle to write to
   *
   * @return SWITRC_SUCCESS on success
   */
  int ALTAPI SWItrcLogEventContentWrite(const unsigned char*  buffer,
					unsigned long         buflen,
					unsigned long*        nwritten,
					SWItrcContentHandle*  handle);

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

#endif
/*
** ----------------- end of file SWItrcEvent.h -----------------
*/
