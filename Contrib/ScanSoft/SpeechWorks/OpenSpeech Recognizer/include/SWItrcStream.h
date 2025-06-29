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

#ifndef _SWItrcStream_H
#define _SWItrcStream_H

#include <stdio.h>

#ifdef __cplusplus
#include <cwchar>
extern "C" {
#else
#include <wchar.h>
#endif

#if defined(_MSC_VER)            /* Microsoft Visual C++ */
  #if !defined(ALTAPI)
    #define ALTAPI __stdcall
  #endif
  #pragma pack(push, 8)
#elif defined(__BORLANDC__)      /* Borland C++ */
  #if !defined(ALTAPI)
    #define ALTAPI __pascal
  #endif
  #pragma option -a8
#elif defined(__WATCOMC__)       /* Watcom C++ */
  #if !defined(ALTAPI)
    #define ALTAPI __stdcall
  #endif
  #pragma pack(push, 8)
#else                            /* Any other includng Unix */
  #if !defined(ALTAPI)
    #define ALTAPI
  #endif
#endif

typedef void* SWItrcStream;

/** @name SWItrcStream defs
 ** @memo defines stream operations for use with SWItrc
 **/


  /** @name SWItrcStreamOpen()
   ** @memo opens the given stream
   **
   ** @doc The Stream is opened for append mode, if it already exists,
   ** or write mode, if the stream doesn't exist.
   **
   ** @param	path 	[*]in []out []modify    <br>
   **		path of stream
   **
   ** @return   the stream, if a succesful open occurred
   ** @return   NULL if the open failed
   **/
extern SWItrcStream ALTAPI SWItrcStreamOpen(
  wchar_t* path);                /* in */

  /** @name SWItrcStreamClose()
   ** @memo closes the given stream
   **
   ** @param	stream	[]in []out [*]modify    <br>
   **		stream on which to operate
   **
   ** @return   SWITRC_SUCCESS
   **/
extern int ALTAPI SWItrcStreamClose(
  SWItrcStream* stream); /* modify */

  /** @name SWItrcStreamArchive()
   ** @memo archives the given stream.
   ** @doc  Archiving will close the current open
   ** file, rename it, appending the input arg append
   ** to the filename. A new file is then opened under
   ** the current stream's path.
   ** <p>
   ** For example, if the current stream is a file named 'switrc.log',
   ** the SWItrcStreamArchive(stream, '_prv') operation will rename
   ** 'switrc.log' to 'switrc.log_prv', and open a new 'switrc.log'
   ** <p>
   ** @param	stream	[]in []out [*]modify    <br>
   **		stream on which to operate
   ** @param	suffix	[*]in []out []modify    <br>
   **		string to append to the original path name
   **            of the input stream. The result of the
   **            archive operation will be the renaming of
   **            input stream's name_suffix
   **
   ** @return SWITRC_SUCCESS    archive succeeded
   ** @return SWITRC_IO_ERROR   close, rename, or open failed
   **/
extern int ALTAPI SWItrcStreamArchive(
    SWItrcStream* stream,               /* modify */
    wchar_t*      suffix);              /* in */

  /** @name SWItrcStreamGetFile()
   ** @memo returns the file handle for the given stream
   **
   ** @param	stream	[*]in []out []modify    <br>
   **		stream on which to operate
   **
   ** @return   the file handle
   **
   **/
extern FILE* ALTAPI SWItrcStreamGetFileHandle(
  SWItrcStream stream); /* in*/

  /** @name SWItrcStreamGetPath()
   ** @memo returns the path given when the stream was opened.
   **
   ** @param	stream	[*]in []out []modify    <br>
   **		stream on which to operate
   ** @return    pointer to path string
   **
   **/
extern wchar_t* ALTAPI SWItrcStreamGetPath(
  SWItrcStream stream);


  /** @name SWItrcStreamInitialize
   ** @memo initializes the SWItrcStream facility
   **
   ** @doc Note: this is usually called by the DLL/DSO loading/initialization
   ** code
   **
   **/
extern void ALTAPI SWItrcStreamInitialize( void );



#ifdef __cplusplus
}
#endif

#endif
/*
** ----------------- end of file SWItrcStream.h -----------------
*/
