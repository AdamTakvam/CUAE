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

#ifndef _SWItrcEventListener_H
#define _SWItrcEventListener_H

#include <wchar.h>
#include <time.h>
#include "SWItrcEvent.h"
#include "VXIvalue.h"

#ifdef __cplusplus
extern "C" {
#endif


/** @name SWItrc Event Listener
 ** @memo SpeechWorks logging engine's event listener interface.
 **
  @doc 
  A TRC listener is a callback which when registered will receiving
  logging events that it has enabled.  To write a listener, defined a
  function which matches the SWItrcEventListener interface and then
  register it with SWItrcRegisterEventListener.
 **/

//@{

  /**
   ** @name SWItrcContentListenerStream
   ** @memo Event content stream required for content listeners, which
   **       contains methods for writing data and closing the stream.
   */
  typedef struct SWItrcContentListenerStream
  {
    /**
     ** @name Close
     ** @memo Close the stream
     **
     ** @param stream 	[]in []out [*]modify            <br>
     **                 Handle to the stream to close, will be set
     **                 to NULL on success
     **
     ** @return SWITRC_SUCCESS on success
     */
    int (*Close)(struct SWItrcContentListenerStream  **stream);
    
    /**
     ** @name Write
     ** @memo Write content to the stream
     **
     ** @param buffer   [*]in []out []modify            <br>
     **                 Buffer of data to write to the stream
     ** @param buflen   [*]in []out []modify            <br>
     **                 Number of bytes to write
     ** @param nwritten [*]in []out []modify            <br>
     **                 Number of bytes actual written, may be less then
     **                 buflen if an error is returned
     ** @param stream   [*]in []out []modify            <br>
     **                 Handle to the stream to write to
     **
     ** @return SWITRC_SUCCESS on success
     */
    int (*Write)(struct SWItrcContentListenerStream  *stream,
		 const unsigned char                 *buffer,
		 unsigned long                        buflen,
		 unsigned long                       *nwritten);
    
  } SWItrcContentListenerStream;

  /**
  ** @name SWItrcEventListener
  ** @memo   Prototype for event listener notification
  **
  ** @doc All event listener registrants must conform to this signature
  **
  ** @param timet	[*]in []out []modify            <br>
  **			time structure 
  ** @param millitm	[*]in []out []modify            <br>
  **			integer millisecs portion of time
  ** @param appname	[*]in []out []modify            <br>
  **			name of application, as set by 
  **                    SWItrcSetEventStreamAppName()
  ** @param channelid	[*]in []out []modify            <br>
  **			channel id as set by
  **                    SWItrcSetEventStreamChannelId()
  ** @param reserved	[*]in []out []modify            <br>
  **			reserved for future use
  ** @param event 	[*]in []out []modify            <br>
  **			name of the event
  ** @param keys        [*]in []out []modify            <br>
  **                    VXIVector of key names, all a VXIString, where
  **                    keys[n] corresponds to values[n]. The event
  **                    name key (EVNT), CPU times (UCPU, SCPU), and
  **                    event specific tokens are already present, but
  **                    the timestamp (TIME) and channel (CHAN) are
  **                    not.
  ** @param values      [*]in []out []modify            <br>
  **                    VXIVector of values, all a VXIString, where
  **                    keys[n] corresponds to values[n]
  ** @param userdata 	[*]in []out 
  **			userdata registered when
  **                    SWItrcRegisterEventListener() was called
  **
  */
  typedef void ALTAPI SWItrcEventListener(
    time_t		timet,		/* in */
    unsigned short      millitm,        /* in */
    const wchar_t*	appname,	/* in */
    const wchar_t*	channelid,	/* in */
    unsigned int	reserved,	/* in */
    const wchar_t*	event,          /* in */
    const VXIVector*	keys,           /* in */
    const VXIVector*	values,         /* in */
    void*		userdata	/* in */
    );

  /**
  ** @name SWItrcRegisterEventListener
  **
  ** @memo Subscribes the given TRC listener, enabling notification for
  ** all output calls (via SWItrcLogEvent[...]()).
  **
  ** @doc As each SWItrcLogEventEnd( ) is processed by TRC.
  **
  ** @param stream      [*]in []out []modify            <br>
  **                            event logging stream to listen to
  ** @param alistener	[*]in []out []modify            <br>
  **				the subscribing TRC listener 
  ** @param userdata	[*]in []out []modify            <br>
  **                            userdata that will be returned to the 
  **                            when notification occurs listener.
  **                            Note. the same listener may be 
  **                            registered multiple times, as long
  **                            as unique userdata is passed with each
  **                            registration. In this case, the listener
  **                            will be called once for each unique userdata.
  ** 
  ** @return	SWITRC_SUCCESS:		success
  ** @return	SWITRC_INTERNAL_ERROR:	internal error
  ** @return	SWITRC_BAD_TAG:		invalid tag
  **
  ** @see SWItrcEventListener()
  **/
  extern int  ALTAPI SWItrcRegisterEventListener(
    SWItrcEventStream* stream,          /* in */
    SWItrcEventListener* alistener,	/* in */
    void* userdata);			/* in */

  /**
  ** @name SWItrcUnregisterEventListener
  **
  ** @memo Unsubscribes the given TRC listener/userdata combination.
  **
  ** @param stream      [*]in []out []modify            <br>
  **                            event logging stream to listen to
  ** @param alistener	[*]in []out []modify            <br>
  **				the subscribing TRC listener 
  ** @param userdata	[*]in []out []modify            <br>
  **                            userdata that will be returned to the 
  **                            when notification occurs listener.
  **<p>
  **                            Note. the same listener may be 
  **                            registered multiple times, as long
  **                            as unique userdata is passed. In this
  **                            case, the listener will be called once
  **                            for each unique userdata.
  ** 
  ** @return	SWITRC_SUCCESS:		success
  ** @return	SWITRC_INTERNAL_ERROR:	internal error
  ** @return	SWITRC_BAD_TAG:		invalid tag
  **
  ** @see SWItrcEventListener()
  ** @see SWItrcRegisterEventListener()
  */
  extern int  ALTAPI SWItrcUnregisterEventListener(
    SWItrcEventStream* stream,          /* in */
    SWItrcEventListener* alistener,	/* in */
    void* userdata);			/* in */

  /**
  ** @name SWItrcEventContentListener
  ** @memo   Prototype for event content listener notification
  **
  ** @doc All event content listener registrants must conform to this signature
  **
  ** @param appname	[*]in []out []modify            <br>
  **			name of application, as set by 
  **                    SWItrcSetEventStreamAppName()
  ** @param channelid	[*]in []out []modify            <br>
  **			channel id as set by
  **                    SWItrcSetEventStreamChannelId()
  ** @param reserved	[*]in []out []modify            <br>
  **			reserved for future use
  ** @param contentType [*]in []out []modify            <br>
  **                    MEDIA content type for the event content
  ** @param userdata 	[*]in []out 
  **			userdata registered when 
  **                    SWItrcRegisterEventContentListener() was called
  ** @param logKey      []in [*]out []modify            <br>
  **                    Key name to cross-reference this content
  **                    in an event log entry, must be valid until
  **                    stream->Close( ) is called
  ** @param logValue    []in [*]out []modify            <br>
  **                    Value to cross-reference this content
  **                    in an event log entry
  ** @param stream      []in [*]out []modify            <br>
  **                    Handle for writing the content and closing the
  **                    stream, must be valid until stream->Close( )
  **                    is called
  **
  ** @return SWITRC_SUCCESS on success (valid stream returned),
  **         SWITRC_LISTENER_ERROR otherwise (SWItrc will not
  **         log an error, the listener should do so itself if
  **         that is appropriate)
  */
  typedef int ALTAPI SWItrcEventContentListener(
    const wchar_t*	           appname,	   /* in */
    const wchar_t*	           channelid,	   /* in */
    unsigned int	           reserved,	   /* in */
    const wchar_t*                 contentType,    /* in */
    void*		           userdata,	   /* in */
    const wchar_t**                logKey,         /* out */
    const wchar_t**                logValue,       /* out */
    SWItrcContentListenerStream**  contentStream   /* out */
    );

  /**
  ** @name SWItrcRegisterEventContentListener
  **
  ** @memo Subscribes the given TRC listener, enabling notification for
  ** all content output calls (via SWItrcSWItrcLogEventContentOpen[...]()).
  **
  ** @doc As each SWItrcLogEventContentOpen( ) is processed by TRC.
  **
  ** @param stream      [*]in []out []modify            <br>
  **                            event logging stream to listen to
  ** @param alistener	[*]in []out []modify            <br>
  **				the subscribing TRC listener 
  ** @param userdata	[*]in []out []modify            <br>
  **                            userdata that will be returned to the 
  **                            when notification occurs listener.
  **                            Note. the same listener may be 
  **                            registered multiple times, as long
  **                            as unique userdata is passed with each
  **                            registration. In this case, the listener
  **                            will be called once for each unique userdata.
  ** 
  ** @return	SWITRC_SUCCESS:		success
  ** @return	SWITRC_INTERNAL_ERROR:	internal error
  ** @return	SWITRC_BAD_TAG:		invalid tag
  **
  ** @see SWItrcEventContentListener()
  **/
  extern int  ALTAPI SWItrcRegisterEventContentListener(
    SWItrcEventStream* stream,              /* in */
    SWItrcEventContentListener* alistener,  /* in */
    void* userdata);			    /* in */

  /**
  ** @name SWItrcUnregisterEventContentListener
  **
  ** @memo Unsubscribes the given TRC listener/userdata combination.
  **
  ** @param stream      [*]in []out []modify            <br>
  **                            event logging stream to listen to
  ** @param alistener	[*]in []out []modify            <br>
  **				the subscribing TRC listener 
  ** @param userdata	[*]in []out []modify            <br>
  **                            userdata that will be returned to the 
  **                            when notification occurs listener.
  **<p>
  **                            Note. the same listener may be 
  **                            registered multiple times, as long
  **                            as unique userdata is passed. In this
  **                            case, the listener will be called once
  **                            for each unique userdata.
  ** 
  ** @return	SWITRC_SUCCESS:		success
  ** @return	SWITRC_INTERNAL_ERROR:	internal error
  ** @return	SWITRC_BAD_TAG:		invalid tag
  **
  ** @see SWItrcEventContentListener()
  ** @see SWItrcRegisterEventContentListener()
  */
  extern int  ALTAPI SWItrcUnregisterEventContentListener(
    SWItrcEventStream* stream,              /* in */
    SWItrcEventContentListener* alistener,  /* in */
    void* userdata);			    /* in */

//@}
#ifdef __cplusplus
}
#endif

#endif
/*
** ----------------- end of file SWItrcEventListener.h -----------------
*/

