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

#ifndef _SWItrc_H
#define _SWItrc_H
#include <stdarg.h>
#include "SWItrcErrors.h"
#include "SWItrcStream.h"

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

#define SWITRC_ERROR_DETAIL   4

#define SWITRC_TAG_ENABLED    1
#define SWITRC_TAG_DISABLED   0
#define SWITRC_MAX_NAMESIZE 128

#ifdef __cplusplus
extern "C" {
#endif

/** @name SWItrc
 ** @memo is the SpeechWorks logging engine
 **
 ** @doc  Copyright 1996, 1997, 1998, 1999, 2000 SpeechWorks International, Inc.
 **  All Rights Reserved.
 **/



  int ALTAPI SWItrcInitProc(void);

  /*
  ** tag related methods
  */

  /**
  ** @name SWItrcEnable()
  ** @memo turns on all output and notification for the given tag
  **
  ** @doc This is the api call that will enable all code instrumented
  ** for the given tag (typically calls to SWItrcPrintln()) to
  ** generate output, both to all SWITrcStreams, as well as
  ** notifications to SWItrcListeners which have been registered.
  **
  ** @param tag		[*]in []out []modify    <br>
  **			the tag of interest
  ** @return	SWITRC_SUCCESS		success
  ** @return	SWITRC_INTERNAL_ERROR	internal snafu
  ** @return	SWITRC_BAD_TAG		invalid tag
  ** @return	SWITRC_IO_ERROR 	an error occurred writing to the output stream
  ** */
  extern int ALTAPI SWItrcEnable(unsigned int tag);

  /**
  ** @name SWItrcDisable()
  ** @memo turns off all output and notification for the given tag.
  **
  ** @doc disabling a tag disable both default output as well as
  ** listener notification for the given tag.
  **
  ** @param tag		[*]in []out []modify    <br>
  **			the tag of interest
  **
  ** @return	SWITRC_SUCCESS		success
  ** @return	SWITRC_INTERNAL_ERROR	internal snafu
  ** @return	SWITRC_BAD_TAG		invalid tag
  ** @return	SWITRC_IO_ERROR	        an error occurred writing to the output stream
  **
  ** @see SWItrcEnable()
  ** */
  extern int ALTAPI SWItrcDisable(unsigned int tag);

  /**
  ** @name SWItrcIsEnabled()
  ** @memo returns whether or not the given tag is enabled
  **
  ** @doc .
  **
  ** @param             [*]in []out []modify    <br>
  **                    tag, tag of interest
  **
  ** @return	SWITRC_TAG_ENABLED	tag is enabled
  ** @return	SWITRC_TAG_DISABLED	tag is not enabled
  **
  ** @see SWItrcEnable()
  ** @see SWItrcDisable()
  */
  extern unsigned char ALTAPI SWItrcIsEnabled(unsigned int tag);

  /**
   ** @name SWItrcEnableTimeStamp()
   **
   ** @memo turns on timestamp portion of date/time
   **		in thread/global output stream
   **
   ** @doc  this does not affect time stamp delivery to TRC listeners
   **
   ** @return	SWITRC_SUCCESS:		success
   ** @return	SWITRC_INTERNAL_ERROR:	internal snafu
   */
  extern int ALTAPI SWItrcEnableTimeStamp(void);

  /**
  ** @name SWItrcDisableTimeStamp()
  ** @memo turns off timestamp portion of date/time in output logging
  ** @doc Note this doesn't affect time stamp information in listener notification
  **
  ** @return	SWITRC_SUCCESS:		success
  **		SWITRC_INTERNAL_ERROR:	internal snafu
  */
  extern int ALTAPI SWItrcDisableTimeStamp(void);

  /**
  ** @name SWIGetTimeStampEnabled()
  ** @memo returns whether or not timestamping has been enabled
  **
  ** @doc Time stamp enablement only affect default output logging,
  ** not listener notification
  **
  **
  ** @return SWITRC_TAG_ENABLED if timestamping is enabled
  ** @return SWITRC_TAG_DISABLED if timestamping is disabled
  **
  ** @see SWItrcEnableTimeStamp()
  ** @see SWItrcDisableTimeStamp() */
  extern unsigned char ALTAPI SWItrcGetTimeStampEnabled(void);

  /**
  ** @name SWItrcSetTagName()
  ** @memo enables a string to be associad with a tag.
  **
  ** @doc The registered string will be output as
  ** part of the print operation, as well as in SWItrcListener
  ** notification.
  **
  ** @param tag	[*]in []out []modify    <br>
  **		tag of interest
  ** @param tagName
  **		[*]in(set) [*]out(get) []modify    <br>
  **		name of the tag used for output
  **		 - will be limited to SWITRC_MAX_NAMESIZE bytes
  **
  ** @return	SWITRC_SUCCESS:		success
  **		SWITRC_INTERNAL_ERROR:	internal snafu
  **		SWITRC_BAD_TAG:		invalid tag
  **
  */
  extern int ALTAPI SWItrcSetTagName(
    unsigned int tag,		/* in */
    const wchar_t* tagname	/* in */
  );

  /**
  ** @name SWItrcGetTagName()
  **
  ** @memo enables a string to be associad with a tag.
  **
  ** @doc The registered string will be output as
  ** part of the print operation, as well as in SWItrcListener
  ** notification.
  **<p>
  ** Note Tag map files are generally used for setting tag names and enabling/disabling tags.
  **
  ** @param tag	[*]in []out []modify    <br>
  **		tag of interest
  ** @param tagName
  **		[*]in(set) [*]out(get) []modify    <br>
  **		name of the tag used for output
  **		 - will be limited to SWITRC_MAX_NAMESIZE bytes
  **
  ** @return	SWITRC_SUCCESS:		success
  **		SWITRC_INTERNAL_ERROR:	internal snafu
  **		SWITRC_BAD_TAG:		invalid tag
  **
  ** @see
  */
  extern int ALTAPI SWItrcGetTagName(
    unsigned int tag,		/* in */
    wchar_t* tagname		/* out */
  );


/**
** @name SWItrcParseTagMapFile()
** @param wpath	[*]in []out []modify    <br>
**		widechar path for file to parse
**
*/
extern int ALTAPI SWItrcParseTagMapFile( const wchar_t* wpath );

/**
** @name SWItrcParseErrorMessageFile()
** @param wpath	[*]in []out []modify    <br>
**		widechar path for file to parse
**
*/
extern int ALTAPI SWItrcParseErrorMessageFile( const wchar_t* wpath );

/**
** @name SWItrcConfigureFromEnvironment()
** @memo parses the registry/environment for configuration information.

** @doc All relevant files which are referenced in the configuration
** variables are also reparsed.
** <p>
** the current variables/registry keys examined are:
**
** <TABLE border=1>
**
** <THEAD>
**   <tr>
**      <TH>#define name </TH><TH>    Key name	</TH><TH>Description	</th> <th>sample value</th>
**
**   </tr>
**  </THEAD>
** <TBODY>
** <TR><TD>SWITRC_TIMESTAMP_SUPPRESS_KEYNAME
**      </TD><TD> DiagSuppressTimestamp
**      </TD><td> True turns off timestamp in output log. This is
**      useful for regression testing
**      </td>
** </tr>
** <TR><TD>SWITRC_ERROR_MAP_CONFIG_KEYNAME
**      </TD><TD> DiagErrorMap
**      </TD><td>Language specific error definition file, where error
**      codes are given names and canonical messages. There may be one
**      of these per system
**      </TD><TD> C:\\projects\\rel7-0-0\\core_Rel70\\slee\\src\\trc\\SWIErrors.en.us.txt
**      </td>
** </tr>
** <TR><TD>SWITRC_STDOUT_FILE_KEYNAME
**      </TD><TD> DiagFileName
**      </TD><td>Default output logging file, where all default
**      logging goes, unless overridden using the API
**      </TD><TD>c:\\switrc.log
**      </td>
** </tr>
** <TR><TD>SWITRC_STDOUT_MAXFILESIZEKB_KEYNAME
**      </TD><TD> DiagMaxFileSizeKB
**      </TD><td>threshold size of, in kilobytes, that any given ouput
**      stream will reach. When this threshold is exceeded, the file
**      is renamed, with 'prv' appened to the DiagFileName.  Any
**      previous 'prv' file is deleted. A new DiagFileName is then
**      opened. This ensures only 2 files for any given ouput stream
**      will exist - the current one and the previous one.
**      </TD><TD>10
**      </td>
** </tr>
** <TR><TD>SWITRC_OUTPUT_TYPE_KEYNAME
**      </TD><TD> DiagOutputStreamTypes
**      </TD><td> 'file' indicates that the defined ouput stream (s)
**      (e.g. DiagFileName or explicitly set using the API) will be
**      actively written.'debug' indicates that log output is written
**      to stdout.  If both 'debug' and 'file' are given, then both
**      are active.
**      </TD><TD>debug,file
**      </td>
** </tr>
** <TR><TD>SWITRC_CONFIG_POLL_SECS_KEYNAME
**      </TD><TD> DiagConfigPollSecs
**      </TD><td>Number of seconds between checking the registry, and
**      corresponding re parsing of all tag map files.This allows tags
**      to be enabled and disabled and other settings to be changed
**      while an application is running, without having to restart
**      it.TRC will never let this value be less than 1 second.
**      </TD><TD> 10  (60 or more for an in production system)
**      </td>
** </tr>
** <TR><TD>SWITRC_BASELINE_TAG_MAP_CONFIG_KEYNAME
**      </TD><TD> DiagTagMapsBaseline
**      </TD><td>Semi colon separated full paths of tag map files,
**      pertaining to the SpeechWorks product. Note that no spaces
**      should appear before or after the ';'
**      </TD><TD>  C:\\projects\\rel7-0-0\\core_Rel70\\slee\\src\\trc\\defaultTagmap.txt;C:\\projects\\rel7-0-0\\core_Rel70\\slee\\src\\trc\\bwcompatTagmap.txt
**      </td>
** </tr>
** <TR><TD>SWITRC_PLATFORM_TAG_MAP_CONFIG_KEYNAME
**      </TD><TD> DiagTagMapsPlatform
**      </TD><td>Semi colon separated full paths of tag map files,
**      pertaining to the platform.  This is intended to allow an
**      integrator to set up tagging for a given platform.Note that no
**      spaces should appear before or after the ';'
**      </TD><TD>  </td>
** </tr>
** <TR><TD>SWITRC_USER_TAG_MAP_CONFIG_KEYNAME
**      </TD><TD> DiagTagMapsUser
**      </td><td>emi colon separated full paths of tag map files, used
**      by application developers. This is intiended for app
**      developers to manage their own tag maps independently of the
**      Speechworks product and the platform.Note that no spaces
**      should appear before or after the ';'
**      </TD><TD>  C:\\projects\\rel7-0-0\\core_Rel70\\slee\\src\\trc\\testTagMap.txt
**      </td>
** </tr>
** <TR><TD align=middle>(not public)
**      </TD><TD> DiagBaseFileName
**      </TD><td>se for bootstrap logging only.  This is the bootstrap
**      logging file used by SWItrcBase, which performs very limited
**      logging and error generation prior to SWItrc's initialization.
**      </TD><TD> c:\\switrcBase.log
**      </td>
** </tr>
** </TBODY>
** </TABLE>
**
** @param wpath	[*]in []out []modify    <br>
** widechar path for file to parse */
extern int ALTAPI SWItrcConfigureFromEnvironment(void);

  /*
  ** output methods
  */

  /**
  ** @name SWItrcPrintln()
  **
  ** @memo This is the key function used to instrument code.
  **
  ** @doc SWItrcPrintln() uses wide chars, SWItrcNPrintln()
  ** uses narrow (traditional ASCII) chars.
  ** <p>
  ** Example:
  ** <p>
  ** <pre>
  ** #define TRC_TAG 50006
  ** int foo( void )
  ** {
  **   static const wchar_t stag[] = L"foo()";
  **
  **   SWItrcPrintln(TRC_TAG, stag, L"entered.");
  **   SWItrcPrintln(TRC_TAG, stag, L"here's an int (%d), and a string (%s).",
  **                 5, L"hi there");
  **   SWItrcPrintln(TRC_TAG, stag, L"return.");
  **
  **   return (0);
  ** }
  ** </pre>
  ** Assuming the tag has been enabled, and named "TRC_SAMPLE", the output would look like:
  **
<pre>
Dec 21 14:36:49.33|||| TRC_SAMPLE| foo()| entered.
Dec 21 14:36:49.33|||| TRC_SAMPLE| foo()| here's an int (5), and a string (hi there).
Dec 21 14:36:49.33|||| TRC_SAMPLE| foo()| return.
</pre>
  **
  ** @param tag	        [*]in []out []modify    <br>
  **			tag of interest
  ** @param subtag	[*]in []out []modify    <br>
  **			any arbitrary string, used to
  **			further refine categorization of
  **			the tagged message (if desired)
  ** @param fmt	        [*]in []out []modify    <br>
  **			format string for formatting the
  **			message, similar to a printf()
  **			format in C.
  ** @param ...	        [*]in []out []modify    <br>
  **			varargs list against which the
  **			fmt will be appied, similar to a
  **			printf() list in C.
  **
  ** @return	SWITRC_SUCCESS		success
  ** @return	SWITRC_INTERNAL_ERROR	internal snafu
  ** @return	SWITRC_BAD_TAG		invalid tag
  ** @return	SWITRC_IO_ERROR 	an error occurred writing to the output stream
  **
  */
  extern int ALTAPI SWItrcPrintln(unsigned int tag,		/* in */
				  const wchar_t* subtag,	/* in */
				  const wchar_t* fmt,		/* in */
				  ...);				/* in */
  extern int ALTAPI SWItrcVPrintln(unsigned int tag,	/* in */
				  const wchar_t* subtag,	/* in */
				  const wchar_t* fmt,		/* in */
				  va_list vArgs);			/* in */

  /**
  ** @name SWItrcNPrintln()
  **
  ** @memo is functionally equivalent to SWItrcPrintln(), but provides
  ** for standard ASCII chars as input, rather than wide chars
  **
  ** @doc SWItrcPrintln() uses wide chars, SWItrcNPrintln()
  ** uses narrow (traditional ASCII) chars. They are identical
  ** functionally.
  **
  ** @param tag	        [*]in []out []modify    <br>
  **			tag of interest
  ** @param subtag	[*]in []out []modify    <br>
  **			any arbitrary string, used to
  **			further refine categorization of
  **			the tagged message (if desired)
  ** @param fmt	        [*]in []out []modify    <br>
  **			format string for formatting the
  **			message, similar to a printf()
  **			format in C. <p>
  ** @param ...	        [*]in []out []modify    <br>
  **			varargs list against which the
  **			fmt will be appied, similar to a
  **			printf() list in C.
  **
  ** @return	SWITRC_SUCCESS		success
  ** @return	SWITRC_INTERNAL_ERROR	internal snafu
  ** @return	SWITRC_BAD_TAG		invalid tag
  ** @return	SWITRC_IO_ERROR 	an error occurred writing to the output stream
  **
  */
  extern int ALTAPI SWItrcNPrintln(unsigned int tag,		/* in */
				  const char* subtag,		/* in */
				  const char* fmt,		/* In */
				  ...);				/* in */

  /** @name SWItrc Error Map file
   ** @memo shows the format of the error map file, referred to by the DiagErrorMap registry key/ environment variable
   ** @doc the format of the error map file is as follows:
   **
<pre>
#
# error codes
#
# format
#   name			: error val	: error message
#   ----			  ---------	 ------------------
SWI_CONFIG_ERROR		: -8	:	error opening/reading file
SWI_ERROR_BUFFER_OVERFLOW	: -7	:	buffer overflow
SWI_ERROR_MUTEX			: -6	:	error using mutex
SWI_ERROR_FILE			: -5	:	error opening/reading file
SWI_ERROR_NOMEM			: -4	:	memory allocation failed
SWI_FATAL			: -3	:	fatal error
SWI_ERROR			: -2	:	recoverable error
SWI_FAIL			: -1	:	non-error failure
SWI_SUCCESS			: 0	:	success
...
</pre>
   **
   ** @see SWItrcLogError()
   ** @see SWItrcNLogError()
   **/

  /**
   ** @name SWItrcLogError()
   ** @memo  logs error messages,  uses wide char strings as input param
   **
   ** @doc LogError functions use an error number to indicate the
   ** actual error. This number is correlated with the error map file
   ** given in the system registry/environment, "DiagErrorMap". The
   ** file indicated by the DiagErrorMap key is intended to allow
   ** canonical error messages to be generated for the language where
   ** the system is deployed
   **
   ** @param errortag	[*]in []out []modify            <br>
   **			one of the SWItrcErrorTags, indicating severity
   ** @param errorNum	[*]in []out []modify            <br>
   **			error number - used to lookup appropriate error message
   ** @param subtag	[*]in []out []modify            <br>
   **			arbitrary string - typically used to identify
   **			facility/class, etc.
   ** @param fmt		[*]in []out []modify            <br>
   **			addtional message information format - follows
   **			basic printf formatting.
   ** @param ...		[*]in []out []modify            <br>
   **			varargs list for addtional error messaging
   **
   ** @return	SWITRC_SUCCESS:		success
   ** @return	SWITRC_INTERNAL_ERROR:	internal snafu
   ** @return	SWITRC_IO_ERROR:	an error occurred writing to the output stream
   ** @see SWItrcNlogError()
   ** @see TRC Error Map file
   **/
  extern int ALTAPI SWItrcLogError(SWItrcSeverity severity,	/* in */
			    int errorNum,			/* in */
			    const wchar_t* subtag,		/* in */
			    const wchar_t* fmt, ...);		/* in */

  extern int ALTAPI SWItrcVLogError(SWItrcSeverity severity,	/* in */
			    int errorNum,			/* in */
			    const wchar_t* subtag,		/* in */
			    const wchar_t* fmt, /* in */
				va_list vArgs);		/* in */

  /**
   ** @name SWItrcNLogError()
   ** logs error messages,  uses narrow (traditional ASCII) chars
   **
   ** @param errortag	[*]in []out []modify                <br>
   **			one of the SWItrcErrorTags, indicating severity
   ** @param errorNum	[*]in []out []modify                <br>
   **			error number - used to lookup appropriate error message
   ** @param subtag	[*]in []out []modify                <br>
   **			arbitrary string - typically used to identify
   **			facility/class, etc.
   ** @param fmt		[*]in []out []modify                <br>
   **			addtional message information format - follows
   **			basic printf formatting.
   ** @param ...		[*]in []out []modify                <br>
   **			varargs list for addtional error messaging
   **
   ** @return	SWITRC_SUCCESS:		success
   ** @return	SWITRC_INTERNAL_ERROR:	internal snafu
   ** @return	SWITRC_IO_ERROR:	an error occurred writing to the output stream
   **
   ** @see       SWItrcLogError()
   ** @see TRC Error Map file
   **/
  extern int ALTAPI SWItrcNLogError(SWItrcSeverity severity,	/* in */
			    int errorNum,			/* in */
			    const char* subtag,			/* in */
			    const char* fmt, ...);		/* in */

  /*
  ** Thread specific properties
  */

  /** @name SWItrc thread specific variables
   **
   ** @memo These variables enable certain bits of information to be
   ** maintained for each thread.
   **
   ** @doc These variables allow a platform or
   ** application to set up information which will be included in TRC
   ** output and forwarded to each listener. This allows all output to
   ** have application, and channel information to be included in the output.
   **<p>
   ** In addition, the default ouput stream for a thread may be
   ** redirected, enabling default logging to be thread-based (e.g. each
   ** channel could be logged to it's own file).
   **
   ** <table border=1>
   ** <tr><th align=middle>  Variable       </th><th align=middle> Description </th></tr>
   ** <tr><td align=middle> appName     </td><td> application name       </td></tr>
   ** <tr><td align=middle> channelNum  </td><td> channel number, usually staring at 0</td></tr>
   ** <tr><td align=middle> channelId   </td><td> channel id, can be anything: phone number,ANI,DNIS,customer accout,...</td></tr>
   ** <tr><td align=middle> printStream </td><td> an SWItrcStream, this directs the calling thread's default log output<p> It does not affect listener notification</td></tr>
   ** </table
   **
   ** @see SWItrcSetThreadAppName()
   ** @see SWItrcSetThreadChannelNum()
   ** @see SWItrcSetThreadChannelId()
   ** @see SWItrcSetThreadPrintStream()
   **
   ** @see SWItrcGetThreadAppName()
   ** @see SWItrcGetThreadChannelNum()
   ** @see SWItrcGetThreadChannelId()
   ** @see SWItrcGetThreadPrintStream()
   **/

  /**
   ** @name SWItrcGetThreadAppName()
   **
   ** @memo returns current value of the app name, registered for the
   **		thread.
   **
   ** @doc The app name is any arbitray string, maintained on a
   ** perthread basis and is written to the output message to aid in
   ** identification.
   **
   ** @param answerbuff  []in [*]out []modify    <br>
   **
   **/
  extern int ALTAPI SWItrcGetThreadAppName(wchar_t* answerbuff);        /* out */

  /**
   ** @name SWItrcGetThreadAppName()
   **
   ** @memo returns current value of the app name, registered for the
   **		thread.
   **
   ** @doc The app name is any arbitray string, maintained on a
   ** perthread basis and is written to the output message to aid in
   ** identification.
   **
   ** @param answerbuff  [*]in []out []modify    <br>
   **
   **/
  extern int ALTAPI SWItrcSetThreadAppName(const wchar_t* val);	      /* in */

  /**
  ** @name SWItrcGetThreadChannelNum()
  **
  ** @memo retrieves the channel number for the calling thread.
  **
  ** @doc Note that the channelNum is a thread safe variable.  Each
  ** new thread must explicitly set this, or it will revert to the
  ** default value.  The default value is -1.
  **
  ** @return the thread safe setting of channel num
  **/
  extern int ALTAPI SWItrcGetThreadChannelNum(void);

  /**
  ** @name SWItrcSetThreadChannelNum()
  **
  ** @memo sets the channel num for the calling thread.
  **
  ** @doc Note that the channelNum is a thread safe
  **		variable.  Each new thread must explicitly
  **		set this, or it will revert to the default value.
  **		The default value is -1.
  ** @param	val	[*]in []out []modify    <br>
  **			the new channel number
  ** @return	the thread safe setting of channel num
  */
  extern int ALTAPI SWItrcSetThreadChannelNum(int val);

  /**
  ** @name SWItrcGetThreadChannelId()
  ** @memo      returns the channel id for the calling thread.
  **
  ** @doc	The channelId is any arbitrary string that
  **		an application may use to symbolically reference
  **		the channel (e.g a phone number, ANI, or DNIS)
  **		Like the appName, and channelNum, the
  **		channelId is written to the output stream message,
  **		as well as the output notification to all
  **		subscribed SWItrcListeners.
  **<p>
  **		Note that the channelId is a thread safe
  **		variable.  Each new thread must explicitly
  **		set this, or it will revert to the default value.
  **		The default value is "".
  **
  ** @param     valbuff []in [*]out []modify    <br>
  **            location where the resultant value will be placed.
  */
  extern int ALTAPI SWItrcGetThreadChannelId(wchar_t* valbuff);
  /**
  ** @name SWItrcGetThreadChannelId()
  ** @memo      sets the channel id for the calling thread.
  **
  ** @doc	The channelId is any arbitrary string that
  **		an application may use to symbolically reference
  **		the channel (e.g a phone number, ANI, or DNIS)
  **		Like the appName, and channelNum, the
  **		channelId is written to the output stream message,
  **		as well as the output notification to all
  **		subscribed SWItrcListeners.
  **<p>
  **		Note that the channelId is a thread safe
  **		variable.  Each new thread must explicitly
  **		set this, or it will revert to the default value.
  **		The default value is "".
  **
  ** @param     chanid []in [*]out []modify    <br>
  **            new channel id value
  */
  extern int ALTAPI SWItrcSetThreadChannelId(const wchar_t* chanid);

  /** @name SWItrcGetGlobalHostName()
   **/
  extern int ALTAPI SWItrcGetGlobalHostName(wchar_t* outbuff);
  /** @name SWItrcSetGlobalHostName()
   **/
  extern int ALTAPI SWItrcSetGlobalHostName(wchar_t* val);

  /**
  ** @name SWItrcSetGlobalPrintStream()
  **
  ** @memo sets the default printStream where default logging will be
  **		written.
  **
  ** @doc       The default for the global printstream is
  **		an SWItrcStream created for stdout.
  **<p>
  **            Calls to SWItrcSetGlobalPrintStream will enable any
  **            threads spawned AFTER this call to inherit the global
  **            setting. You should use SWITrcSetThreadPrintStream to
  **            explicitly set a streams output stream other than the
  **            global print stream.
  **<p>
  **		Additionally, the caller may elect to set the print
  **		stream to NULL. NULL turns print message printing off
  **		for any subsequently spawned threads (but doesn't
  **		affect active threads).  This is useful if you want
  **		all message processing to be handled exclusively by
  **		SWItrcListeners.
  **
  ** @param val	[*]in []out []modify    <br>
  **			the new file handle
  **
  ** @return SWITRC_SUCCESS;
  */
  extern int   ALTAPI SWItrcSetGlobalPrintStream(SWItrcStream printStream);

  /**
  ** @name SWItrcGetGlobalPrintStream()
  **
  ** @memo returns the default printStream where default logging will be
  **		written.
  **
  ** @return the printstream
  **/
  extern SWItrcStream ALTAPI SWItrcGetGlobalPrintStream(void);


  extern int   ALTAPI SWItrcGetGlobalPrintStreamPath(wchar_t* outbuff);

  /**
  ** @name SWItrcSetThreadPrintStream()
  **
  ** @memo sets the default printStream where default logging will be
  **		written.
  **
  ** @doc       The default for the global printstream is
  **		an SWItrcStream created for stdout.
  **<p>
  **            Calls to SWItrcSetThreadPrintStream will enable the calling
  **            thread to explicitly set the output stream.
  **<p>
  **		Additionally, the caller may elect to set the print
  **		stream to NULL. NULL turns print message printing off
  **		for the calling  threads.  This is useful if you want
  **		all message processing to be handled exclusively by
  **		SWItrcListeners.
  **
  ** @param val	[*]in []out []modify    <br>
  **			the new printstream
  ** @return    a pointer to the current print output stream
  **
  */
  extern int   ALTAPI SWItrcSetThreadPrintStream(SWItrcStream printStream);

  /**
  ** @name SWItrcGetThreadPrintStream()
  **
  ** @memo returns the calling thread's printStream where default logging will be
  **		written.
  ** @doc .
  ** @return the printstream
  **/
  extern SWItrcStream ALTAPI SWItrcGetThreadPrintStream(void);

  extern int   ALTAPI SWItrcGetThreadPrintStreamPath(wchar_t* outbuff);



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
** ----------------- end of file SWItrc.h -----------------
*/
