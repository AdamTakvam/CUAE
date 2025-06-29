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

#ifndef _SWItrcErrors_H
#define _SWItrcErrors_H

#ifdef __cplusplus
extern "C" {
#endif

  /*
  ** Error severity
  **    OOS - out of service - aka CRITICAL
  **    SA  - service affecting - aka SEVERE
  **    NSA - not service affecting - aka WARNING
  **    
  */
  typedef enum SWItrcSeverity {
    SWITRC_ERROR_CRITICAL_OOS = 0,	/* CRITICAL - out of service*/
    SWITRC_ERROR_SEVERE_SA,		/* SEVERE - service affecting */
    SWITRC_WARNING_NSA,		        /* WARNING - not service affecting*/
  } SWItrcSeverity;

  /*
  ** return codes
  */
#define SWITRC_SUCCESS 0
#define SWITRC_BAD_TAG -1
#define SWITRC_INTERNAL_ERROR -2
#define SWITRC_IO_ERROR -3
#define SWITRC_LISTENER_ERROR -4
#define SWITRC_INVALID_ARGUMENT -5
#define SWITRC_SYSTEM_ERROR -6

/*
** ERROR CODES
*/
#define SWI_ERROR_GENERIC               -11             /* generic error */
#define SWI_ERROR_SYSTEM                -10             /* system error */
#define SWI_ERROR_INVALID_ARG           -9              /* invalid arguments */
#define SWI_CONFIG_ERROR                -8              /* error opening/reading file */
#define SWI_ERROR_BUFFER_OVERFLOW       -7              /* buffer overflow */
#define SWI_ERROR_MUTEX                 -6              /* error using mutex */
#define SWI_ERROR_FILE                  -5              /* error opening/reading file */
#define SWI_ERROR_NOMEM                 -4              /* memory allocation failed */
#define SWI_FATAL                       -3              /* fatal error */
#define SWI_ERROR                       -2              /* recoverable error */
#define SWI_FAIL                        -1              /* non-error failure */
#define SWI_SUCCESS                     0               /* success */                           

/*
** transaction errors
*/
#define SWI_TRANSACTION_ESCAPE		50
#define SWI_TRANSACTION_FAILURE		51

/*
** dictionary errors
*/
#define SWI_DICT_VALID_PHONEME		100
#define SWI_DICT_INVALID_PHONEME	101
#define SWI_DICT_ERROR_NO_DICT_FILE	102
#define SWI_DICT_ERROR_NOMEM		103
#define SWI_DICT_ERROR_INVALID_LOOKUP	104
#define SWI_DICT_ERROR_INVALID_DICT	105
#define SWI_DICT_ERROR_DICT_EXISTS	106
#define SWI_DICT_ERROR_WORD_NOT_FOUND	107
#define SWI_DICT_ERROR_PRON_NOT_FOUND	108
#define SWI_DICT_ERROR_WORD_EXISTS	109
#define SWI_DICT_ERROR_PRON_EXISTS	110
#define SWI_DICT_ERROR_PRON_TOOMANY	111
#define SWI_DICT_ERROR_FILTER_ERROR	112

/*
** trc errors
*/
#define SWI_TRC_EOPEN	                1000	/* Log file already open */
#define SWI_TRC_EBADARG	                1001	/* Bad argument */
#define SWI_TRC_ECANTOPEN	        1002	/* Can't open log file */
#define SWI_TRC_ENOWIN	                1003	/* Can't open log window */
#define SWI_TRC_ENOTOPEN	        1004	/* Log file not open */
#define SWI_TRC_ENOTCONF	        1005	/* Logging not configured */
#define SWI_TRC_ERROR_BAD_TAG		1006
#define SWI_TRC_INTERNAL_ERROR		1007
#define SWI_TRC_IO_ERROR		1008
#define SWI_TRC_LISTENER_ERROR		1009

/*
** ialloc errors
*/
  
#ifdef __cplusplus
}
#endif

#endif
/*
** ----------------- end of file SWItrcErrors.h -----------------
*/

