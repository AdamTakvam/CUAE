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

#ifndef _SWItrcListener_H
#define _SWItrcListener_H

#include <time.h>

#ifdef __cplusplus
extern "C" {
#endif


/** @name SWItrc Listener
 ** @memo SpeechWorks logging engine's listener interface.
 **
  @doc 
  A TRC listener is a callback which when registered will receiving
  logging events that it has enabled.  To write a listener, defined a
  function which matches the SWItrcPrintListener interface and then
  register it SWItrcRegisterPrintListener or with SWItrcRegisterPrintListenerForTag.  

  TRC uses a tagging
  facility to control what is logged and which listeners received
  different log messages. Every log message is sent with a tag and a
  subtag.  If these tag are enabled in the TAG MAP file, then they
  will be sent to the registered listeners for that Tag.  
 **/

//@{

  /**
  ** @name SWItrcPrintListener
  ** @memo   Prototype for listener notification
  **
  ** @doc All printlistener registrants must conform to this signature
  **
  ** @param timet	[*]in []out []modify            <br>
  **			time structure 
  ** @param millitm	[*]in []out []modify            <br>
  **			integer millisecs portion of time
  ** @param appname	[*]in []out []modify            <br>
  **			name of application, as set by 
  **                    SWItrcSetThreadAppName()
  ** @param channelid	[*]in []out []modify            <br>
  **			channel id as set by
  **                    SWItrcSetThreadChannelId()
  ** @param channelnum	[*]in []out []modify            <br>
  **			channel number as set by
  **                    SWItrcSetThreadChannelNum()
  ** @param tag 	[*]in []out []modify            <br>
  **			tag for which print was enabled
  ** @param tagName    	[*]in []out []modify            <br>
  **			name of tag, set by 
  **                    SWItrcSetTagName(), or as set in tag map file
  ** @param subtag 	[*]in []out []modify            <br>
  **			subtag passed to SWItrcPrintln()
  ** @param printmsg 	[*]in []out []modify            <br>
  **			resultant printf-lie string passed to
  **                    SWItrcPrintln()
  ** @param userdata 	[*]in []out []modify            <br>
  **			userdata registerd when call to 
  **                    SWItrcRegigerPrintListener() was called
  **
  */
  typedef void ALTAPI SWItrcPrintListener(
    time_t		timet,		/* in */
    unsigned short      millitm,        /* in */
    const wchar_t*	appname,	/* in */
    const wchar_t*	channelid,	/* in */
    unsigned int	channelnum,	/* in */
    unsigned int	tag,		/* in */
    const wchar_t*	tagName,	/* in */
    const wchar_t*	subtag,		/* in */
    const wchar_t*	printmsg,	/* in */
    void*		userdata	/* in */
    );

  /**
  ** @name SWItrcRegisterPrintListener
  **
  ** @memo Subscribes the given TRCListener, enabling notification for
  ** all and output calls (via SWItrcPrintln()) for all tags.
  **
  ** @doc As each print()/println() is processed by TRC. Output
  ** notification only occurs if the the given tag has been enabled.
  **
  ** @param alistener	[*]in []out []modify            <br>
  **				the subscribing TRCListener 
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
  ** @see SWItrcPrintListener()
  **/
  extern int  ALTAPI SWItrcRegisterPrintListener(
    SWItrcPrintListener* alistener,	/* in */
    void* userdata);			/* in */

  /**
  ** @name SWItrcUnregisterPrintListener
  **
  ** @memo Unsubscribes the given TRCListener/userdata combination.
  **
  ** @param alistener	[*]in []out []modify            <br>
  **				the subscribing TRCListener 
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
  ** @see SWItrcPrintListener()
  ** @see SWItrcRegisterPrintListener()
  */
  extern int  ALTAPI SWItrcUnregisterPrintListener(
    SWItrcPrintListener* alistener,	/* in */
    void* userdata);			/* in */

  /**
  ** @name SWItrcRegisterPrintListenerForTag
  ** @memo	Subscribes the given TRCListener for a specific tag.
  **
  ** @doc The given listener will be notified for all output (via the
  ** SWItrcPrintln() calls), for the given tag, as each
  ** print()/println() is processed by TRC.  Output notification only
  ** occurs if the the given tag has been enabled.
  **
  ** @param alistener	[*]in []out []modify            <br>
  **				the subscribing TRCListener 
  ** @param userdata	[*]in []out []modify            <br>
  **                            userdata that will be returned to the 
  **                            when notification occurs listener.
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
  ** @see       SWItrcPrintListener()
  */
  extern int  ALTAPI SWItrcRegisterPrintListenerForTag(
    unsigned int	 tag,		/* in */
    SWItrcPrintListener* alistener,	/* in */
    void*		 userdata);	/* in */

  /**
  ** @name SWItrcUnregisterPrintListenerForTag
  ** @memo Unsubscribes the given TRCListener for the given tag.
  ** @doc  Unsubscribes the given TRCListener/userdata combination.
  ** @param alistener	[*]in []out []modify            <br>
  **				the subscribing TRCListener 
  ** @param userdata	[*]in []out []modify            <br>
  **                            userdata that will be returned to the 
  **                            when notification occurs listener.
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
  ** @see SWItrcPrintListener()
  ** @see SWItrcRegisterPrintListenerForTag()
  */
  extern int  ALTAPI SWItrcUnregisterPrintListenerForTag(
    unsigned int	 tag,		/* in */
    SWItrcPrintListener* alistener,	/* in */
    void*		 userdata);	/* in */

//@}
#ifdef __cplusplus
}
#endif

#endif
/*
** ----------------- end of file SWItrcListener.h -----------------
*/

