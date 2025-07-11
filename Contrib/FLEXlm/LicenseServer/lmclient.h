/*****************************************************************************

    Copyright (c) 1988-2006 Macrovision Europe Ltd. and/or Macrovision Corporation. All Rights Reserved.
	This software has been provided pursuant to a License Agreement
	containing restrictions on its use.  This software contains
	valuable trade secrets and proprietary information of
	Macrovision Corporation and is protected by law.  It may
	not be copied or distributed in any form or medium, disclosed
	to third parties, reverse engineered or used in any manner not
	provided for in said License Agreement except with the prior
	written authorization from Macrovision Corporation.

 *****************************************************************************/

/**   @file lmclient.h
 *    @brief Client side FLEXlm definitions

 *
  ************************************************************/

#ifndef _LM_CLIENT_H_
#define _LM_CLIENT_H_

#include <stdio.h>
#include <stdlib.h>

#ifndef FLEXLM_ULTRALITE
#include "lm_trl.h"
#include "time.h"
#endif
#include "lmhostid.h"

/*
 *	Macros to make c++ and ansi-c happy
 */
#if defined(c_plusplus) || defined(__cplusplus)
#define  lm_extern  extern "C"
#define  lm_noargs  void
#define  lm_args(args) args
#else	/* ! c++ */
#define  lm_extern  extern
#if (!defined(sun) && !defined(mips)) || defined(__STDC__)
#ifndef ANSI
#define ANSI
#endif
#define  lm_noargs  void
#define  lm_args(args) args
#else	/* ! stdc || ansi */
#define  lm_noargs  /* void */
#define  lm_args(args) ()
#define const
#endif	/* stdc || ansi */
#endif	/* c++ */

#if defined( _WIN32) && !defined(WINNT)
#define WINNT
#endif

#if defined ( _MSC_VER) && ! defined (PC)
#define PC
#endif

#if defined(WINNT) && !defined(PC)
#define PC
#endif

#ifdef PC
#if defined(LM_INTERNAL) && (!defined(OS2) || (defined(OS2) && defined(_LMGR_WINDLL)))
#include <windows.h>
#endif /* LM_INTERNAL */
#if !defined(OS2) && !defined(WINNT)
#define LM_CALLBACK_TYPE _far _pascal
#endif /* pc16 */
#ifdef OS2
#define FAR
#define LM_CALLBACK_TYPE _System
#endif /* OS2 */
#ifdef WINNT
#define LM_CALLBACK_TYPE _cdecl
#endif
#ifndef FAR
#ifdef WINNT
#define FAR
#else /* WINNT */
#define FAR far
#define LM_CALLBACK_TYPE _far _pascal
#endif /* WINNT */
#endif /* FAR */
#else /* defined(PC) */
#ifndef FAR
#define FAR
#endif /* FAR */
#define LM_CALLBACK_TYPE
#endif /* PC */


typedef char * LM_A_VAL_TYPE; /* so that it will be big enough for */
			      /* any data type on any system */
typedef void  FAR * LM_VOID_PTR;
typedef void  FAR * FAR * LM_VOID_PTR_PTR;
typedef int   FAR * LM_INT_PTR;
typedef short FAR * LM_SHORT_PTR;
typedef long  FAR * LM_LONG_PTR;
typedef unsigned long  FAR * LM_U_LONG_PTR;
typedef char  FAR * LM_CHAR_PTR;
typedef char  FAR * FAR * LM_CHAR_PTR_PTR;
typedef unsigned char  FAR * LM_U_CHAR_PTR;

/*
 *	FLEXlm version
 *
 * For windows when changing the version or copyright string
 * the file h/flexwinver.h must also be changed
 */

#define FLEXLM_VERSION 10
#define FLEXLM_REVISION 8
#define FLEXLM_SUB_MINOR_REVISION 5
#define FLEXLM_PATCH_NUMBER       0 

#define FLEXLM_PATCH ""	/* no longer used */
#define MAX_PATCH_SIZE 1	/* Max length of a patch letter */

#define FLEXLM_VERSION_REVISION_STRING "10.8.5.0"

#define FLEXLM_COPYRIGHT_YEAR "2006"
#define FLEXLM_BORROW_REVISION	1
#define COPYRIGHT_STRING(firstYear)	"Copyright (c) " #firstYear "-"FLEXLM_COPYRIGHT_YEAR " Macrovision Europe Ltd. and/or Macrovision Corporation. All Rights Reserved."

/*
 *	Codes returned from all client library routines
 */

/** The error code returned by most of the functions. it will be one of
 * the LM_xxx defines below */
typedef int FLEX_ERROR ;

/* FLEXlm error codes.
 * If a new error is added here it must also be added to:
 * 		lm_lerr.h
 * 		lmerrors.h
 *		lcontext.h	*/
#define LM_NOERROR				 0  /** The was no error. @ref FLEX_ERROR */
#define	LM_NOCONFFILE			-1	/** Can't find license file */
#define LM_BADFILE				-2	/** License file corrupted */
#define LM_NOSERVER	  			-3	/** Cannot connect to a license server */
#define LM_MAXUSERS	  			-4	/** Maximum number of users reached */
#define LM_NOFEATURE 			-5	/** No such feature exists */
#define LM_NOSERVICE			-6	/** No TCP/IP service "FLEXlm" */
#define LM_NOSOCKET				-7	/** No socket to talk to server on */
#define LM_BADCODE				-8	/** Bad encryption code */
#define	LM_NOTTHISHOST			-9	/** Hostid doesn't match license */
#define	LM_LONGGONE				-10	/** Software Expired */
#define	LM_BADDATE				-11	/** Bad date in license file */
#define	LM_BADCOMM				-12	/** Bad return from server */
#define LM_NO_SERVER_IN_FILE	-13  /** No servers specified in license file */
#define LM_BADHOST				-14	/** Bad SERVER hostname in license file */
#define LM_CANTCONNECT			-15	/** Cannot connect to server */
#define LM_CANTREAD				-16	/** Cannot read from server */
#define LM_CANTWRITE			-17	/** Cannot write to server */
#define LM_NOSERVSUPP			-18	/** Server does not support this feature */
#define LM_SELECTERR			-19	/* Error in select system call */
#define LM_SERVBUSY				-20	/** Application server "busy" (connecting) */
#define LM_OLDVER				-21	/** Config file doesn't support this version */
#define LM_CHECKINBAD	 		-22	/** Feature checkin failed at daemon end */
#define LM_BUSYNEWSERV	 		-23	/** Server busy/new server connecting */
#define LM_USERSQUEUED	 		-24	/** Users already in queue for this feature */
#define	LM_SERVLONGGONE	 		-25	/** Version not supported at server end */
#define	LM_TOOMANY	 			-26	/** Request for more licenses than supported */
#define LM_CANTREADKMEM	 		-27	/** Cannot read /dev/kmem */
#define LM_CANTREADVMUNIX		-28	/** Cannot read /vmunix */
#define LM_CANTFINDETHER 		-29	/** Cannot find ethernet device */
#define LM_NOREADLIC	 		-30	/** Cannot read license file */
#define	LM_TOOEARLY				-31	/** Start date for feature not reached */
#define	LM_NOSUCHATTR	 		-32	/** No such attr for lm_set_attr/ls_get_attr */
#define	LM_BADHANDSHAKE	 		-33	/** Bad encryption handshake with server */
#define LM_CLOCKBAD	 			-34	/** Clock difference too large between client/server */
#define LM_FEATQUEUE	 		-35	/** We are in the queue for this feature */
#define LM_FEATCORRUPT	 		-36	/** Feature database corrupted in daemon */
#define LM_BADFEATPARAM	 		-37	/** dup_select mismatch for this feature */
#define LM_FEATEXCLUDE	 		-38	/** User/host on EXCLUDE list for feature */
#define LM_FEATNOTINCLUDE		-39	/** User/host not in INCLUDE list for feature */
#define LM_CANTMALLOC	 		-40	/** Cannot allocate dynamic memory */
#define LM_NEVERCHECKOUT 		-41	/** Feature never checked out (lm_status()) */
#define LM_BADPARAM		 		-42	/** Invalid parameter */
#define LM_NOKEYDATA	 		-43	/** No FLEXlm key data */
#define LM_BADKEYDATA	 		-44	/** Invalid FLEXlm key data */
#define LM_FUNCNOTAVAIL	 		-45	/** FLEXlm function not available */
#define LM_DEMOKIT	     		-46	/** FLEXlm software is demonstration version */
#define LM_NOCLOCKCHECK	 		-47	/** Clock check not available in daemon */
#define LM_BADPLATFORM	 		-48	/** FLEXlm platform not enabled */
#define LM_DATE_TOOBIG	 		-49	/** Date too late for binary format */
#define LM_EXPIREDKEYS	 		-50	/** FLEXlm key data has expired */
#define LM_NOFLEXLMINIT	 		-51	/** FLEXlm not initialized */
#define LM_NOSERVRESP	 		-52	/** Server did not respond to message */
#define LM_CHECKOUTFILTERED 	-53	/** Request rejected by vendor-defined filter */
#define LM_NOFEATSET 	 		-54	/** No FEATURESET line present in license file */
#define LM_BADFEATSET 	 		-55	/** Incorrect FEATURESET line in license file */
#define LM_CANTCOMPUTEFEATSET 	-56	/** Cannot compute FEATURESET line */
#define LM_SOCKETFAIL	 		-57	/** socket() call failed */
#define LM_SETSOCKFAIL	 		-58	/** setsockopt() failed */
#define LM_BADCHECKSUM	 		-59	/** message checksum failure */
#define LM_SERVBADCHECKSUM 		-60	/** server message checksum failure */
#define LM_SERVNOREADLIC 		-61	/** Cannot read license file from server */
#define LM_NONETWORK	 		-62	/** Network software (tcp/ip) not available */
#define LM_NOTLICADMIN	 		-63	/** Not a license administrator */
#define LM_REMOVETOOSOON 		-64	/** lmremove request too soon */
#define LM_BADVENDORDATA 		-65	/** Bad VENDORCODE struct passed to lm_init() */
#define LM_LIBRARYMISMATCH 		-66	/** FLEXlm include file/library mismatch */
#define LM_NONETOBORROW	 		-67	/** No licenses to borrow */
#define LM_NOBORROWSUPP	 		-68	/** License BORROW support not enabled */
#define LM_NOTONSERVER 	 		-69	/** FLOAT_OK can't run standalone on SERVER */
#define LM_BORROWLOCKED  		-70	/** Meter already being updated for another counter */
#define LM_BAD_TZ	 	 		-71	/** Invalid TZ environment variable */
#define LM_OLDVENDORDATA 		-72	/** "Old-style" vendor keys (3-word) */
#define LM_LOCALFILTER   		-73	/** Local checkout filter requested request */
#define LM_ENDPATH	     		-74	/** Attempt to read beyond the end of LF path */
#define LM_VMS_SETIMR_FAILED	-75 /** VMS SYS$SETIMR call failed */
#define LM_INTERNAL_ERROR		-76	/** Internal FLEXlm error -- Please report */
#define LM_BAD_VERSION   		-77	/** Version number must be string of dec float */
#define LM_NOADMINAPI    		-78	/** FLEXadmin API functions not available */
#define LM_NOFILEOPS     		-79 /** FLEXlm internal error -79 */
#define LM_NODATAFILE    		-80 /** FLEXlm internal error -80 */
#define LM_NOFILEVSEND   		-81 /** FLEXlm internal error -81 */
#define LM_BADPKG	 	 		-82	/** Invalid PACKAGE line in license file */
#define LM_SERVOLDVER	 		-83	/** Server FLEXlm version older than client's */
#define LM_USER_BASED	 		-84	/** Incorrect number of USERS/HOSTS INCLUDED in options file -- see server log */
#define LM_NOSERVCAP	 		-85	/** Server doesn't support this request */
#define LM_OBJECTUSED	 		-86	/** This license object already in use (Java only) */
#define LM_MAXLIMIT	     		-87	/** Checkout exceeds MAX specified in options file */
#define LM_BADSYSDATE	 		-88	/** System clock has been set back */
#define LM_PLATNOTLIC	 		-89	/** This platform not authorized by license */
#define LM_FUTURE_FILE	 		-90	/** "Future license file format or misspelling in license file" */
#define LM_DEFAULT_SEEDS 		-91	/** "ENCRYPTION_SEEDs are non-unique" */
#define LM_SERVER_REMOVED  		-92	/** "Server removed during reread, or server hostid mismatch with license" */
#define LM_POOL 				-93	/** "This feature is available in a different license pool" */
#define LM_LGEN_VER 			-94	/** "Attempt to generate license with incompatible attributes" */
#define LM_NOT_THIS_HOST 		-95 /** "Network connect to THIS_HOST failed" */
#define LM_HOSTDOWN 			-96 /** "Server node is down or not responding" */
#define LM_VENDOR_DOWN 			-97 /** "The desired vendor daemon is down" */
#define LM_CANT_DECIMAL 		-98 /** "The FEATURE line can't be converted to decimal format" */
#define LM_BADDECFILE 			-99  /** "The decimal format license is typed incorrectly" */
#define LM_REMOVE_LINGER 		-100 /** "Cannot remove a lingering license" */
#define LM_RESVFOROTHERS 		-101 /** "All licenses are reserved for others" */
#define LM_BORROW_ERROR 		-102 /** "A FLEXid borrow error occurred" */
#define LM_TSOK_ERR 			-103 /** Terminal Server remote client not allowed */
#define LM_BORROW_TOOLONG 		-104 /** "Cannot borrow that long" */
#define LM_UNBORROWED_ALREADY 	-105 /** "Feature already returned to server" */
#define LM_SERVER_MAXED_OUT 	-106 /** "License server out of network connections" */
#define LM_NOBORROWCOMP 		-107 /** "Can't borrow a PACKAGE component" */
#define LM_BORROW_METEREMPTY 	-108 /** "Licenses all borrowed or meter empty" */
#define LM_NOBORROWMETER 		-109 /** "No Borrow Meter Found" */
#define LM_NODONGLE 			-110 /** "Dongle not attached, or can't read dongle" */
#define LM_NORESLINK 			-111 /** "lmgr.res, Windows Resource file, not linked" */
#define LM_NODONGLEDRIVER 		-112 /** "Missing Dongle Driver" */
#define LM_FLEXLOCK2CKOUT		-113 /** "2 FLEXlock checkouts attempted" */
#define LM_SIGN_REQ 			-114 /**"SIGN= attribute required, but missing from license"*/
#define LM_PUBKEY_ERR 			-115 /** "Error in Public Key package" */
#define LM_NOTRLSUPPORT 		-116 /** "TRL not supported for this platform" */
#define LM_NOCROSUPPORT			LM_NOTRLSUPPORT
#define LM_BORROW_LINGER_ERR 	-117 /** "BORROW failed" */
#define LM_BORROW_EXPIRED 		-118 /** "BORROW period has expired" */
#define LM_MUST_BE_LOCAL 		-119 /** "lmdown and lmreread must be run on license server node" */
#define LM_BORROW_DOWN 			-120 /** "Cannot lmdown the server when licenses are borrowed" */
#define LM_FLOATOK_ONEHOSTID 	-121 /** "FLOAT_OK license must have exactly one dongle hostid" */
#define LM_BORROW_DELETE_ERR 	-122 /** Unable to delete local borrow info */
#define LM_BORROW_RETURN_EARLY_ERR		-123 /** Support for returning a borrowed license early is not enabled */
#define LM_BORROW_RETURN_SERVER_ERR		-124 /** Error returning borrowed license on server */
#define LM_CANT_CHECKOUT_JUST_PACKAGE	-125 /** Error when trying to just checkout a PACKAGE(BUNDLE) */
#define LM_COMPOSITEID_INIT_ERR 		-126 /** "Composite Hostid not initialized" */
#define LM_COMPOSITEID_ITEM_ERR 		-127 /** "An item needed for Composite Hostid missing or invalid" */
#define LM_BORROW_MATCH_ERR		-128 /** "Error, borrowed license doesn't match any known server license." */
#define LM_NULLPOINTER			-129 /** A null pointer was detected. @ref FLEX_ERROR */
#define LM_BADHANDLE			-130 /** A bad handle was used. @ref FLEX_ERROR */
#define LM_EMPTYSTRING			-131 /** An emptstring was detected. @ref FLEX_ERROR */
#define LM_BADMEMORYACCESS		-132 /** Tried to asscess memory that we shouldn't have. @ref FLEX_ERROR */
#define LM_NOTSUPPORTED			-133 /** Operation is not supported yet. @ref FLEX_ERROR */
#define LM_NULLJOBHANDLE		-134 /** The job handle was NULL. @ref FLEX_ERROR */
#define LM_EVENTLOG_INIT_ERR	-135 /** Error enabling event log */
#define LM_EVENTLOG_DISABLED	-136 /** Event logging is disabled */
#define LM_EVENTLOG_WRITE_ERR	-137 /** Error writing to event log */
#define LM_BADINDEX				-138 /** Internal error. An invalid array index was used. */
#define LM_TIMEOUT				-139 /** a timeout has occured */
#define LM_BADCOMMAND			-140 /** A bad command was found in a message */
#define LM_SOCKET_BROKEN_PIPE	-141 /** Error writing to socket.  Peer has closed socket */
#define LM_INVALID_SIGNATURE	-142 /** Attempting to generate version specific license tied to a single hostid, which is composite. */
#define LM_UNCOUNTED_NOT_SUPPORTED	-143 /** Version specific signatures are not supported for uncounted licenses. */
#define LM_REDUDANT_SIGNATURES	-144 /** License template contains redudant signature specifiers. */
#define LM_BADCODE_V71_LK		-145 /** Bad V71_LK signature. */
#define LM_BADCODE_V71_SIGN		-146 /** Bad V71_SIGN signature */
#define LM_BADCODE_V80_LK		-147 /** Bad V80_LK signature */
#define LM_BADCODE_V80_SIGN		-148 /** Bad V80_SIGN signature */
#define LM_BADCODE_V81_LK		-149 /** Bad V81_LK signature */
#define LM_BADCODE_V81_SIGN		-150 /** Bad V81_SIGN signature */
#define LM_BADCODE_V81_SIGN2	-151 /** Bad V81_SIGN2 signature */
#define LM_BADCODE_V84_LK		-152 /** Bad V84_LK signature */
#define LM_BADCODE_V84_SIGN		-153 /** Bad V84_SIGN signature */
#define LM_BADCODE_V84_SIGN2	-154 /** Bad V84_SIGN2 signature */
#define LM_LK_REQ				-155 /** License key required but missing from the license certificate. */
#define LM_BADAUTH				-156 /** Bad AUTH={} signature */
#define LM_REPAIR_NEEDED		-157 /** TS record invalid */
#define LM_TS_OPEN				-158 /** Cannot open TS */
#define LM_BAD_FULFILLMENT		-159 /** Invalid Fulfillment record */
#define LM_BAD_ACTREQ			-160 /** Invalid activation request received */
#define LM_TS_NO_FULFILL_MATCH	-161 /** Invalid activation request received */
#define LM_BAD_ACT_RESP			-162	/** Invalid activation response received */
#define LM_CANTRETURN			-163	/** Can't return the fulfillment */
#define LM_RETURNEXCEEDSMAX		-164	/** Return would exceed max count(s) */
#define LM_NO_REPAIRS_LEFT		-165	/** Return would exceed max count(s) */
#define LM_NOT_ALLOWED			-166 /** Specified operation is not allowed */
#define LM_ENTLEXCLUDE	 		-167 /** User/host on EXCLUDE list for entitlement */
#define LM_ENTLNOTINCLUDE		-168 /** User/host not in INCLUDE list for entitlement */
#define	LM_ACTIVATION			-169	/** Activation error */
#define LM_TS_BADDATE			-170	/** Invalid date format in trusted storage */
#define LM_ENCRYPTION_FAILED	-171 /** Encryption failed */
#define LM_DECRYPTION_FAILED	-172 /** Encryption failed */
#define LM_BADCONTEXT			-173 /** Bad filter context */
#define LM_LAST_ERRNO			-173  /** The last valid error number */
/*  WARNING
 * When adding new error numbers add them to the main branch first to make 
 * sure we do not add conflicting error numbers on different branches. */


/*
 *	Old error code names - Don't use these anymore
 *	(These error code names may BE REMOVED in a future version )
 */

#define	NOCONFFILE	LM_NOCONFFILE
#define BADFILE		LM_BADFILE
#define NOSERVER	LM_NOSERVER
#define MAXUSERS	LM_MAXUSERS
#define NOFEATURE	LM_NOFEATURE
#define NOSOCKET	LM_NOSOCKET
#define BADCODE		LM_BADCODE
#define	NOTTHISHOST	LM_NOTTHISHOST
#define	LONGGONE	LM_LONGGONE
#define	BADDATE		LM_BADDATE
#define	BADCOMM		LM_BADCOMM
#define NO_SERVER_IN_FILE LM_NO_SERVER_IN_FILE
#define BADHOST		LM_BADHOST
#define CANTCONNECT	LM_CANTCONNECT
#define CANTREAD	LM_CANTREAD
#define CANTWRITE	LM_CANTWRITE
#define NOSERVSUPP	LM_NOSERVSUPP
#define SELECTERR	LM_SELECTERR
#define SERVBUSY	LM_SERVBUSY
#define OLDVER		LM_OLDVER
#define CHECKINBAD	LM_CHECKINBAD
#define BUSYNEWSERV	LM_BUSYNEWSERV
#define USERSQUEUED	LM_USERSQUEUED
#define	SERVLONGGONE	LM_SERVLONGGONE
#define	TOOMANY		LM_TOOMANY
#define CANTREADKMEM	LM_CANTREADKMEM
#define CANTREADVMUNIX	LM_CANTREADVMUNIX
#define CANTFINDETHER	LM_CANTFINDETHER
#define NOREADLIC	LM_NOREADLIC
#define	TOOEARLY	LM_TOOEARLY
#define	NOSUCHATTR	LM_NOSUCHATTR
#define	BADHANDSHAKE	LM_BADHANDSHAKE
#define CLOCKBAD	LM_CLOCKBAD
#define FEATQUEUE	LM_FEATQUEUE
#define FEATCORRUPT	LM_FEATCORRUPT
#define BADFEATPARAM	LM_BADFEATPARAM
#define FEATEXCLUDE	LM_FEATEXCLUDE
#define FEATNOTINCLUDE	LM_FEATNOTINCLUDE
#define CANTMALLOC	LM_CANTMALLOC
#define NEVERCHECKOUT	LM_NEVERCHECKOUT
#define BADPARAM	LM_BADPARAM
#define NOKEYDATA	LM_NOKEYDATA
#define BADKEYDATA	LM_BADKEYDATA
#define FUNCNOTAVAIL	LM_FUNCNOTAVAIL
#define DEMOKIT		LM_DEMOKIT
#define NOCLOCKCHECK	LM_NOCLOCKCHECK
#define BADPLATFORM	LM_BADPLATFORM
#define DATE_TOOBIG	LM_DATE_TOOBIG
#define EXPIREDKEYS	LM_EXPIREDKEYS
#define NOFLEXLMINIT	LM_NOFLEXLMINIT
#define NOSERVRESP	LM_NOSERVRESP
#define CHECKOUTFILTERED LM_CHECKOUTFILTERED
#define NOFEATSET 	LM_NOFEATSET
#define BADFEATSET 	LM_BADFEATSET
#define CANTCOMPUTEFEATSET LM_CANTCOMPUTEFEATSET
#define SOCKETFAIL	LM_SOCKETFAIL
#define SETSOCKFAIL	LM_SETSOCKFAIL
#define BADCHECKSUM	LM_BADCHECKSUM
#define SERVBADCHECKSUM	LM_SERVBADCHECKSUM
#define SERVNOREADLIC	LM_SERVNOREADLIC
#define NONETWORK	LM_NONETWORK
#define NOTLICADMIN	LM_NOTLICADMIN
#define REMOVETOOSOON	LM_REMOVETOOSOON
#define BADVENDORDATA	LM_BADVENDORDATA
#define LIBRARYMISMATCH	LM_LIBRARYMISMATCH
#define NONETOBORROW	LM_NONETOBORROW
#define NOBORROWSUPP	LM_NOBORROWSUPP
#define BORROWCORRUPT	LM_BORROWCORRUPT
#define BORROWDUPLICATE	LM_BORROWDUPLICATE
#define LMBAD_TZ	LM_BAD_TZ
#define BADAUTH	    LM_BADAUTH


#define FLEXLM_PORT 744		/* FLEXlm assigned port number */
#define FLEXLM_FINDER_HOST	   "flexlm_license_finder"
#define FLEXLM_FINDER_HOST_BACKUP  "flexlm_license_finder2"
#define FLEXLM_FINDER_HOST_BACKUP2 "flexlm_license_finder3"
#define MAX_FINDER_TYPE MAX_LONGNAME_SIZE	/* Maximum length of a finder "type" string */

/*
 *	Values for the "flag" parameter in the lm_checkout() call
 */

#define LM_CO_NOWAIT	0	/* Don't wait, report status */
#define LM_CO_WAIT	1	/* Don't return until license is available */
#define LM_CO_QUEUE	2	/* Put me in the queue, return immediately */
#define LM_CO_LOCALTEST	3	/* Perform local checks, no checkout */

/*
 *	lc_checkin flag
 */
#define LM_CI_ALL_FEATURES 0	/* used instead of a feature name */


/*
 *	Parameter values for the checkout "group_duplicates" parameter
 *	In order to specify what constitutes a duplicate, 'or' together
 *	from the set { LM_DUP_USER LM_DUP_HOST LM_DUP_DISP LM_DUP_VENDOR},
 *	or use:
 *		LM_DUP_NONE or LM_DUP_SITE.
 */
#define LM_DUP_NONE 0x4000	/* Don't allow any duplicates */
#define LM_DUP_SITE   0x0	/* Nothing to match => everything matches */
#define LM_DUP_USER   0x1	/* Allow dup if user matches */
#define LM_DUP_HOST   0x2	/* Allow dup if host matches */
#define LM_DUP_DISP   0x4	/* Allow dup if display matches */
#define LM_DUP_VENDOR 0x8	/* Allow dup if vendor-defined matches */
#define LM_DUP_BUNDLE 0x10	/* Allow dup if vendor-defined matches */
#define LM_COUNT_DUP_STRING "16384"	/* For ls_vendor.c: LM_DUP_NONE */
#define LM_NO_COUNT_DUP_STRING "3"	/* For ls_vendor.c: _USER | _HOST */
/*
 *	Flags for 'flag' field of lc_cryptstr()
 */
#define LM_CRYPT_ONLY 			0x1
					/* return code for first FEATURE only */
#define LM_CRYPT_FORCE			0x2 /* default */
					/* crypt lines with codes already set */
#define LM_CRYPT_IGNORE_FEATNAME_ERRS	0x4 	/* don't report these errs */
#define LM_CRYPT_NO_LINE_NUMBERS	0x8 	/* Don't print line numbers */
						/* in error messages */
#define LM_CRYPT_FOR_CKSUM_ONLY         0x10
#define LM_CRYPT_DECIMAL_FMT 	        0x20	/* output is compressed dec */
#define LM_CRYPT_KEY2_ONLY 		0x80 	/* return only SIGN= */

/*
 *	lc_convert flags
 */
#define LM_CONVERT_TO_DECIMAL		0x1
#define LM_CONVERT_TO_READABLE		0x2

#define LM_LICENSE_FILE_SUFFIX "lic" 	/* This is for informational purposes,
					   and cannot be altered */

#ifndef LM_LOG_MAX_LEN
#define LM_LOG_MAX_LEN 145
#else
#if LM_LOG_MAX_LEN != 145
#error LM_LOG_MAX_LEN bad value
#endif
#endif

#define LM_MAXPATHLEN	512		/* Maximum file path length */
#define MAX_FEATURE_LEN 30		/* Longest featurename string */
#define DATE_LEN	11		/* dd-mmm-yyyy */
#define BINARY_DATE_LEN	4		/* binary date length */
#if defined (EMBEDDED_FLEXLM) || \
	(defined( PC) && !defined (WINNT )  && !defined(OS2))

#define MAX_CONFIG_LINE 512		/* Max length of a license file line */
#else
#define MAX_CONFIG_LINE	4096		/* Max length of a license file line */
#endif
#define	OLD_MAX_SERVER_NAME	32		/* Maximum FLEXlm length of hostname */
#define	OLD_MAX_DOMAIN_NAME	34		/* Maximum FLEXlm length of domain */
#define	OLD_MAX_HOSTNAME	64		/* Maximum length of a hostname */
#define	OLD_MAX_DISPLAY_NAME 32		/* Maximum length of a display name */
#define OLD_MAX_USER_NAME 20		/* Maximum length of a user name */
#define OLD_MAX_VENDOR_CHECKOUT_DATA 32	/* Maximum length of vendor-defined */
#define OLD_MAX_FINDER_TYPE 20	/* Maximum length of a finder string */

#define MAX_LONGNAME_SIZE	1024
#define	MAX_SERVER_NAME			MAX_LONGNAME_SIZE		/* Maximum FLEXlm length of hostname */
#define	MAX_DOMAIN_NAME			MAX_LONGNAME_SIZE		/* Maximum FLEXlm length of domain */
#if defined(MAX_HOSTNAME)
#if MAX_HOSTNAME !=MAX_LONGNAME_SIZE
#error MAX_HOSTNAME conflict
#endif
#else
#define	MAX_HOSTNAME			MAX_LONGNAME_SIZE		/* Maximum length of a hostname */
#endif
#define	MAX_DISPLAY_NAME 		MAX_LONGNAME_SIZE		/* Maximum length of a display name */
#define MAX_USER_NAME			MAX_LONGNAME_SIZE		/* Maximum length of a user name */
#define MAX_VENDOR_CHECKOUT_DATA MAX_LONGNAME_SIZE	/* Maximum length of vendor-defined */
					/*		checkout data       */
#define MAX_PLATFORM_NAME 12		/* e.g., "sun4_u4" */
#define MAX_DAEMON_NAME 10		/* Max length of DAEMON string */
#define MAX_VENDOR_NAME MAX_DAEMON_NAME	/* Synomym for MAX_DAEMON_NAME */
#define MAX_SERVERS	5		/* Maximum number of servers */
#define MAX_USER_DEFINED 64		/* Max size of vendor-defined string */
#define MAX_VER_LEN 10			/* Maximum length of a version string */
#define MAX_LONG_LEN 10			/* Length of a long after sprintf */
#define MAX_64BIT_HEX_LEN 16		/* Length of 64-bit long, printed hex */
#define MAX_SHORT_LEN 5			/* Length of a short after sprintf */
#define MAX_INET 48			/* Maximum length of INET addr string */
#define MAX_BINDATE_YEAR 2027		/* Binary date has 7-bit year */
#define MAX_PROJECT_LEN 30		/* for $LM_PROJECT */

/*
 *	-------------------------------------------------------------
 *	RESERVED strings
 */

#define LM_RESERVED_BORROW 	"BORROW"
#define LM_RESERVED_FEATURE 	"FEATURE"
#define LM_RESERVED_FEATURESET 	"FEATURESET"
#define LM_RESERVED_INCREMENT 	"INCREMENT"
#define LM_RESERVED_PACKAGE 	"PACKAGE"
#define LM_RESERVED_PROG 	"DAEMON"
#define LM_RESERVED_PROG_ALIAS 	"VENDOR"
#define LM_RESERVED_REDIRECT 	"REDIRECT"
#define LM_RESERVED_SERVER 	"SERVER"
#define LM_RESERVED_UPGRADE 	"UPGRADE"
#define LM_RESERVED_USE_SERVER 	"USE_SERVER"
#define LM_RESERVED_THIS_HOST 	"this_host"
#define LM_RESERVED_UNCOUNTED 	"uncounted"
#define LM_RESERVED_UNEXPIRING 	"permanent"

/*
 *	License file location
 */

#define LM_DEFAULT_ENV_SPEC "LM_LICENSE_FILE"	/* How a user can specify */


#if defined(PC)||defined(_WINDOWS)||defined(_WINDLL)||defined(WINNT)||defined(OS2)

#define LM_DEFAULT_LICENSE_FILE "C:\\flexlm\\license.dat"
#else  /* defined(_WINDOWS)||defined(_WINDLL)||defined(PC)||defined(WINNT) */
#define LM_DEFAULT_LICENSE_FILE "/usr/local/flexlm/licenses/license.dat"
#endif /* PC */


/*
 *	V1/V2/V3 compatibility macros
 */

/*
 *	Define the data required for the new lm_xxx -> lc_xxx call macros
 *	Use of these macros will ensure that you won't need to change
 *	your code if we need to add new variables to the list.
 *
 *	These can be used 3 ways:
 *
 *		1. If all your lm_xxx calls are in one module, use
 *			the LM_DATA_STATIC macro in this module, otherwise
 *
 *		2. Use LM_DATA macro in one module, and LM_DATA_REF in
 *			other modules which call lm_xxx functions.
 *			This produces global symbol(s), or
 *
 *		3. Use LM_DATA_STATIC macro in one module, and pass
 *			parameters to other modules using LM_DATA_PARAM.
 *			Declare parameters in called function using
 *			LM_DATA_DECL.
 *		  e.g.:
 *
 *			LM_DATA_STATIC;
 *
 *			.
 *			.
 *			.
 *			my_function(LM_DATA_PARAM, p1, p2, p3);
 *
 *				void my_function(LM_DATA_PARAM, p1, p2, p3)
 *				LM_DATA_DECL;
 *				int p1;
 *				char *p2.
 *				int p3;
 *
 *
 */
#define LM_DATA LM_HANDLE_PTR lm_job = (LM_HANDLE_PTR) 0
#define LM_DATA_REF lm_extern LM_HANDLE_PTR lm_job
#define LM_DATA_STATIC static LM_HANDLE_PTR lm_job = (LM_HANDLE_PTR) 0
#define LM_DATA_PARAM lm_job
#define LM_DATA_DECL LM_HANDLE_PTR lm_job

#define _lm_errno lc_get_errno(lm_job)
#define lm_errno err_info.maj_errno
#define errno_minor err_info.min_errno
#define u_errno err_info.sys_errno

/*
 *	Communications constants
 */
#define MASTER_WAIT 20                  /* # seconds to wait for a connection */

/*
 *	Structure types
 */

#define VENDORCODE_BIT64	1	/* 64-bit code */
#define VENDORCODE_BIT64_CODED	2	/* 64-bit code with feature data */
#define VENDORCODE_3	3		/* VENDORCODE2 with version data */
#define VENDORCODE_4	4		/* VENDORCODE3 with new vendor keys */
#define VENDORCODE_5	4
#define VENDORCODE_6	4
#define VENDORCODE_7	4
#define LM_DAEMON_INFO_TYPE	101	/* DAEMON_INFO data structure */
#define LM_JOB_HANDLE_TYPE	102	/* Job handle */
#define LM_JOB_HANDLE_DSPECIAL_TYPE 103	/* used by lmgrd and daemons */
#define LM_VD_GENINFO_HANDLE_TYPE	105	/* lm_get_attr struct handle */
#define LM_VD_FEATINFO_HANDLE_TYPE	106	/* lm_get_attr struct handle */

/*
 *	Host identification data structure
 */
#define OLD_MAX_HOSTID_LEN (OLD_MAX_SERVER_NAME + 9)
#define MAX_HOSTID_LEN (MAX_LONGNAME_SIZE + 9)
					/* hostname + strlen("HOSTNAME=") */
#define MAX_SHORTHOSTID_LEN (ETHER_LEN * 2)

typedef struct hostid {			/* Host ID data */
			short override;	/* Hostid checking override type */
#define NO_EXTENDED 1			/* Turn off extended hostid */
			short type;	/* Type of HOST ID */
			short oldtype;

			short representation; /* Normal or other */
#define HOSTID_REP_NORMAL 0
#define HOSTID_REP_DECIMAL 1
			union {
				unsigned long data;
#define ETHER_LEN 6			/* Length of an ethernet address */
				unsigned char e[ETHER_LEN];
/*
 *                              int had better be 32-bits for these intel ids
 */
				unsigned int intel32[32/32];
				unsigned int intel64[64/32];
				unsigned int intel96[96/32];
				char user[MAX_LONGNAME_SIZE + 1];
				char display[MAX_LONGNAME_SIZE + 1];
				char host[MAX_LONGNAME_SIZE + 1];
				char vendor[MAX_HOSTID_LEN+1];
				char string[MAX_HOSTID_LEN+1];
#define HOSTID_INTERNET_LEN	4
				short internet[HOSTID_INTERNET_LEN];
			      } id;

				/* private data for hostid. size will change across
				 * releases */
				char internalData[104];

			char *vendor_id_prefix; /* also METER_BORROW devname */
			struct hostid *next;

#define hostid_value id.data
#define hostid_eth id.e
#define hostid_user id.user
#define hostid_display id.display
#define hostid_hostname id.host
#define hostid_string id.string
#define hostid_internet id.internet
#define hostid_flexid id.string
#define hostid_domain id.string
#define hostid_composite id.string
} HOSTID, FAR *HOSTID_PTR, FAR **HOSTID_PTR_PTR;


#define MAX_CRYPT_LEN 20	/* use 8 bytes of encrypted return string to
				   produce a 16 char HEX representation  + 4 */




/*
 *	Vendor encryption seed
 */

typedef struct vendorcode {
			    short type;	    /* Type of structure */
			    long data[2];   /* 64-bit code */
			  } VENDORCODE1;

typedef struct vendorcode2 {
			    short type;	   /* Type of structure */
			    long data[2];  /* 64-bit code */
			    long keys[3];


			  } VENDORCODE2;

typedef struct vendorcode3 {
			    short type;	   /* Type of structure */
			    long data[2];  /* 64-bit code */
			    long keys[3];


			    short flexlm_version;
			    short flexlm_revision;
			  } VENDORCODE3;

typedef struct vendorcode4 {
			    short type;	   /* Type of structure */
			    unsigned long data[2]; /* 64-bit code */
			    unsigned long keys[4];




			    short flexlm_version;
			    short flexlm_revision;
			  } VENDORCODE4;

/*
 *	new with Version 6
 */
typedef struct vendorcode5 {
			    short type;	   /* Type of structure */
			    unsigned long data[2]; /* 64-bit code */
			    unsigned long keys[4];




			    short flexlm_version;
			    short flexlm_revision;
			    char flexlm_patch[2];
#define LM_MAX_BEH_VER 4
			    char behavior_ver[LM_MAX_BEH_VER + 1];
			  } VENDORCODE5;

typedef struct vendorcode6 {
                            short type;    /* Type of structure */
                            unsigned long data[2]; /* 64-bit code */
                            unsigned long keys[4];
#define LM_PUBKEYS      3
#define LM_MAXPUBKEYSIZ 40
/*
 *                          pubkey is for both the public and private keys
 *                          The public key goes here when authenticating
 *                          the private key when generating licenses
 */
                            int pubkeysize[LM_PUBKEYS];
                            unsigned char pubkey[LM_PUBKEYS][LM_MAXPUBKEYSIZ];
                            int pubkeyinfo1;
                            int (*pubkey_fptr)(void);
                            short flexlm_version;
                            short flexlm_revision;
                            char flexlm_patch[2];
#define LM_MAX_BEH_VER 4
                            char behavior_ver[LM_MAX_BEH_VER + 1];
                            unsigned long trlkeys[2];
                          } VENDORCODE6;

/*
 *			    pubkey is for both the public and private keys
 *			    The public key goes here when authenticating
 *			    the private key when generating licenses
 *			    Internal use only
 */
#define LM_PUBKEYS 	3
#define LM_MAXPUBKEYSIZ 40

typedef struct _pubkeyinfo {
			    int pubkeysize[LM_PUBKEYS];
			    unsigned char pubkey[LM_PUBKEYS][LM_MAXPUBKEYSIZ];
			    int (*pubkey_fptr)();
			    int strength;
			    int sign_level;
} LM_VENDORCODE_PUBKEYINFO;



#define LM_MAX_BEH_VER 4
#define LM_MAXSIGNS 	4   /* SIGN=, SIGN2=, SIGN3=, SIGN4= */

typedef struct vendorcode7 {
			    short type;	   /* Type of structure */
			    unsigned long data[2]; /* 64-bit code */
			    unsigned long keys[4];
			    short flexlm_version;
			    short flexlm_revision;
			    char flexlm_patch[2];
			    char behavior_ver[LM_MAX_BEH_VER + 1];
			    unsigned long trlkeys[2];
			    int signs; /* number of entries in pubkeyinfo */
			    int strength;
			    int sign_level;
			    LM_VENDORCODE_PUBKEYINFO pubkeyinfo[LM_MAXSIGNS];
			  } VENDORCODE7, *VENDORCODE_PTR;

#define LM_BEHAVIOR_V2 		"02.0"
#define LM_BEHAVIOR_V3 		"03.0"
#define LM_BEHAVIOR_V4 		"04.0"
#define LM_BEHAVIOR_V5 		"05.0"
#define LM_BEHAVIOR_V5_1 	"05.1"
#define LM_BEHAVIOR_V6 		"06.0"
#define LM_BEHAVIOR_V7 		"07.0"
#define LM_BEHAVIOR_V7_1	"07.1"
#define LM_BEHAVIOR_V8		"08.0"
#define LM_BEHAVIOR_V8_1	"08.1"
#define LM_BEHAVIOR_V8_2	"08.2"
#define LM_BEHAVIOR_V8_3	"08.3"
#define LM_BEHAVIOR_V9		"09.0"
#define LM_BEHAVIOR_V9_3	"09.3"
#define LM_BEHAVIOR_V10		"10.0"

#define LM_BEHAVIOR_CURRENT	LM_BEHAVIOR_V10

/*
 *	The current default VENDORCODE
 */

#define VENDORCODE VENDORCODE7


#define LM_CODE_GLOBAL(name, x, y, k1, k2, k3, k4, k5)  \
					VENDORCODE name = \
						{ VENDORCODE_6, \
						  { (x)^(k5), (y)^(k5) }, \
						  { (k1), (k2), (k3), (k4) }, \
						  FLEXLM_VERSION, \
						  FLEXLM_REVISION, \
						  FLEXLM_PATCH, \
						  LM_VER_BEHAVIOR, \
						  { TRL_KEY1, \
						  TRL_KEY2 }, \
						  0, \
						  LM_STRENGTH, \
						  0 \
						  }

#define LM_CODE(name, x, y, k1, k2, k3, k4, k5)  \
					static VENDORCODE name = \
						{ VENDORCODE_6, \
						  { (x)^(k5), (y)^(k5) }, \
						  { (k1), (k2), (k3), (k4) }, \
						  FLEXLM_VERSION, \
						  FLEXLM_REVISION, \
						  FLEXLM_PATCH, \
						  LM_VER_BEHAVIOR, \
						  { TRL_KEY1, \
						  TRL_KEY2 }, \
						  0, \
						  LM_STRENGTH, \
						  0 \
						  }

/* version of LM_CODE that does not use globals, but has everything
 * passed in.
 */
#define LM_CODE_NEW(name, x, y, k1, k2, k3, k4, k5, ver, rev, pat, beh, \
        t1, t2, str)  \
    static VENDORCODE (name) = \
    { VENDORCODE_6, \
            { (x)^(k5), (y)^(k5) }, \
            { (k1), (k2), (k3), (k4) }, \
            (ver), \
            (rev), \
            pat, \
            beh, \
            { (t1), \
            (t2) }, \
            0, \
            (str), \
            0 \
    }

/* version of LM_CODE_GEN_INIT that does not use globals, but has everything
 * passed in.
 */
#define LM_CODE_GEN_INIT_NEW(code, x, y, cnt, key, siz) \
    (code)->data[0] = x; \
    (code)->data[1] = y; \
    { \
        int pcnt;	 \
        int sign_level = (cnt); \
        for (pcnt = 0; pcnt <= (cnt); pcnt++) \
        { \
            memcpy(&(code)->pubkeyinfo[pcnt].pubkeysize, (siz)[pcnt], \
                sizeof((siz)[pcnt])); \
            (code)->pubkeyinfo[pcnt].pubkey_fptr = (int (*)())l_prikey_sign; \
            (code)->pubkeyinfo[pcnt].sign_level = sign_level--; \
            { \
                int i; \
                for (i = 0; i < LM_PUBKEYS; i++) \
                { \
                    memcpy((code)->pubkeyinfo[pcnt].pubkey[i], \
                    (key)[pcnt][i], (siz)[pcnt][i]); \
                } \
            } \
        } \
    }

#define LM_CODE_GEN_INIT(code) \
	(code)->data[0] = ENCRYPTION_SEED1; \
	(code)->data[1] = ENCRYPTION_SEED2; \
	{ \
	  int pcnt;	 \
	  int sign_level = l_priseedcnt; \
	  	for (pcnt = 0; pcnt <= l_priseedcnt; pcnt++) \
		{ \
			memcpy(&(code)->pubkeyinfo[pcnt].pubkeysize, lm_prisize[pcnt], \
						sizeof(lm_prisize[pcnt])); \
			(code)->pubkeyinfo[pcnt].pubkey_fptr = (int (*)())l_prikey_sign; \
			(code)->pubkeyinfo[pcnt].sign_level = sign_level--; \
			{ \
			  int i; \
				for (i = 0; i < LM_PUBKEYS; i++) \
				{ \
					memcpy((code)->pubkeyinfo[pcnt].pubkey[i], \
						lm_prikey[pcnt][i], lm_prisize[pcnt][i]); \
				} \
			} \
		} \
	}


#define LM_KEY_API \
	int l_key_type; \
	void (*l_key_gen_callback)(); \
	void (*l_key_callback)() = 0;

#ifndef LM_SOCKET
#define LM_SOCKET int
#endif	/* !defined LM_SOCKET */

/*
 *	Communications protocols
 */

#define LM_NO_TRANSPORT_SPECIFIED 0
#define LM_TCP			  1
#define LM_TCP_PREFIX 		  "TCP:"
#define LM_TCP_PREFIX_LEN 	  4
#define LM_TCP_PREFIX_ALT	  "PORT="
#define LM_TCP_PREFIX_ALT_LEN	  5

#define LM_TYPE_PREFIX	  	"TYPE="
#define LM_TYPE_PREFIX_LEN	  5
#define LM_OPTIONS_PREFIX	"OPTIONS="
#define LM_OPTIONS_PREFIX_LEN	  8

#ifdef FIFO
#define LM_LOCAL		  3
#define LM_LOCAL_PREFIX 	  "LOCAL:"
#define LM_LOCAL_PREFIX_LEN	  6
#endif

#define LM_FILE_COMM		  4
#define LM_FILE_PREFIX 		  "FILE:"
#define LM_FILE_PREFIX_LEN 	  5

/*
 *	Server data from the license file FEATURE file
 */
typedef struct lm_server {		/* License servers */
	char name[MAX_LONGNAME_SIZE + 1];	/* Hostname */
	HOSTID *idptr;		/* hostid */
	struct lm_server FAR *next;	/* NULL =none */
	int commtype; 		/* TCP/FIFO/FILE */
	int port;			/* port in native
					byte order*/
	LM_CHAR_PTR filename;		/* FILE base path */
	char sflags;
#define L_SFLAG_US 	  	0x1
#define L_SFLAG_THIS_HOST 	0x2
#define L_SFLAG_PRINTED_DEC	0x4
#define L_SFLAG_PARSED_DEC   	0x8
#define L_SFLAG_WRONG_HOST   	0x10
#define L_SFLAG_REPORT_LOG_ENABLED	0x20

	int SAMsuiteFlag;	/* internal use for SAMsuite */

	struct _commBuffer *commBufRead;
} LM_SERVER, FAR *LM_SERVER_PTR;

typedef struct _keylist {
	char *key;
	int sign_level;
	struct _keylist *next;
} LM_KEYLIST;

typedef struct lm_server_list {
	struct lm_server_list *next;
	LM_SERVER_PTR s;
}	LM_SERVER_LIST, FAR *LM_SERVER_LIST_PTR;

/*
 *	Feature data from the license file FEATURE file
 */
typedef struct config {			/* Feature data line */
/*
 *			First, the required fields
 */
			short type;			/* Type */

#define CONFIG_FEATURE 0				/*  FEATURE line */
#define CONFIG_INCREMENT 1				/*  INCREMENT line */
#define CONFIG_UPGRADE 2				/*  UPGRADE line */
#define CONFIG_BORROW 3					/*  BORROW line */
#define CONFIG_PACKAGE 4				/*  PACKAGE line */
#define CONFIG_PORT_HOST_PLUS 100			/*  Marker to use
							    Server for
							    checkout */
#define CONFIG_UNKNOWN 9999				/*  Unknown line */

			/* Ascii feature name.
			 * the size of this array has been increased by 500 bytes for
			 * internal use. the length of a feature name has not changed from
			 * MAX_FEATURE_LEN (30). DO NOT use this extra space in client
			 * applications. */
#define EXTRA_INTERNAL_FEATURE_NAME_LEN	500
			char feature[MAX_FEATURE_LEN+1+EXTRA_INTERNAL_FEATURE_NAME_LEN];
			char version[MAX_VER_LEN+1];/* Feat's version */
#define LM_PKG_ANY_VERSION "ANYVER"
			char daemon[MAX_DAEMON_NAME+1];	/* DAEMON to serve */
			char date[DATE_LEN+1];		/* Expiration date */
			char startdate[DATE_LEN+1];	/* start date */
			int users;			/* Licensed # users */
			char code[MAX_CRYPT_LEN+1];	/* encryption code */
#define CONFIG_PORT_HOST_PLUS_CODE "PORT_AT_HOST_PLUS   " /* +port@host marker*/
			LM_SERVER_PTR server;		/* License server(s) */
			int lf;				/* License file index */
/*
 *			Optional stuff below here ...
 */

			LM_CHAR_PTR lc_vendor_def; /* Vendor-defined string */
			HOSTID 	*idptr;		/* Licensed host --
						   can be a list as of v5 */
			char  fromversion[MAX_VER_LEN + 1];
							/* Upgrade from ver. */

			int reg1;
			unsigned short lc_got_options;	/* Bitmap of options,
							   for int-type opts */
#define LM_LICENSE_LINGER_PRESENT        0x1
#define LM_LICENSE_DUP_PRESENT           0x2
#define LM_LICENSE_WQUEUE_PRESENT        0x4
#define LM_LICENSE_WTERMS_PRESENT        0x8
#define LM_LICENSE_WLOSS_PRESENT        0x10
#define LM_LICENSE_OVERDRAFT_PRESENT    0x20
#define LM_LICENSE_CKSUM_PRESENT        0x40
#define LM_LICENSE_OPTIONS_PRESENT      0x80
#define LM_LICENSE_TYPE_PRESENT        0x100
#define LM_LICENSE_SUITE_DUP_PRESENT   0x200
#define LM_LICENSE_SORT_PRESENT        0x400


/*
 *		NOTE: lc_linger, lc_dup_group, lc_prereq, lc_sublic, and
 *		      lc_dist_constraint are for future use by Macrovision Corporation.
 *		      DO NOT USE these fields.
 */
			int lc_linger;		/* internal use */
			int lc_dup_group;	/* dup_group -override client */
			int lc_overdraft;	/* # of overdraft licenses */
			int lc_cksum;		/* Line checksum */
			unsigned char lc_options_mask; /* these can pool */
#define LM_OPT_SUITE 		0x1		/* PACKAGE is a SUITE (bit 1)*/
#define LM_OPT_SUPERSEDE 	0x2		/* Invalidates features of
						   same name with previous
						   date */
#define LM_OPT_ISFEAT		0x4		/* used by server */
#define LM_OPT_BUNDLE 	       0x10		/* PACKAGE is a BUNDLE */
#define LM_OPT_SUITEBUNDLE 	       (LM_OPT_SUITE|LM_OPT_BUNDLE)
			unsigned int lc_type_mask; /* these don't pool */
#define LM_TYPE_CAPACITY 	0x1
#define LM_TYPE_METER 		0x2
#define LM_TYPE_HOST_BASED	0x4
#define LM_TYPE_USER_BASED	0x8
#define LM_TYPE_MINIMUM	0x10
#define LM_TYPE_PLATFORMS      0x20
#define LM_TYPE_FLOAT_OK       0x40	/* Node-locked to dongle,
					uncounted, but can float 1
					use if on server */
#define LM_TYPE_TS_OK		0x80	/* Terminal Server support bit */
#define LM_TYPE_BORROW         0x100	/* BORROW[=n] */

			unsigned char	lc_from_server;	/* if non zero, came from the license server. */

			int lc_suite_dup;	/* parent dup group */
			LM_CHAR_PTR lc_vendor_info;/* (Unencrypted) vendor info */
			LM_CHAR_PTR lc_dist_info;	/* (not authenticated)*/
			LM_CHAR_PTR lc_user_info;	/* (not authenticated)*/
			LM_CHAR_PTR lc_asset_info;	/* (not authenticated)*/
			LM_CHAR_PTR lc_issuer;	/* Who issued the license */
			LM_CHAR_PTR lc_notice;	/* Intellectual prop.notice */
			LM_CHAR_PTR_PTR lc_platforms; /* List of platforms */
			LM_CHAR_PTR lc_prereq;	/* Prerequesite products */
			LM_CHAR_PTR lc_sublic;	/* Sub-licensed products */
			LM_CHAR_PTR lc_dist_constraint; /* extra distributor
								constraints */
			LM_CHAR_PTR lc_serial;		/* Serial Number */
			LM_CHAR_PTR lc_issued;		/* Serial Number */
			int lc_user_based;		/* Number of users */
			short lc_minimum;		/* min ckout # */
			int lc_host_based;		/* Number of hosts */
			int lc_max_borrow_hours;
			LM_CHAR_PTR_PTR lc_supersede_list; /* list of features
							   this supersedes */
			unsigned char lc_sort;		/* 0 to 255 */


/*
 *			Internal GSI use only (DO NOT USE)
 */
			LM_CHAR_PTR lc_w_binary;
			LM_CHAR_PTR lc_w_argv0;
#define PHP_NEXT_CONF_SIZE	17
			char php_next_conf_pos[PHP_NEXT_CONF_SIZE]; 	/* for +port@host */
			int lc_w_queue;
			int lc_w_termsig;
			int lc_w_loss;
			int lc_future_minor;
/*
 *		Package info
 */
			unsigned char package_mask;
#define LM_LICENSE_PKG_ENABLE 	0x1	/* Enabling FEATURE for package */
					/* If PKG_SUITE or PKG_BUNDLE is NOT set, this
					   should never be checked out
					   or listed by lmstat */
#define LM_LICENSE_PKG_SUITE 	0x2	/* This enabling FEATURE is for a
					   SUITE (implies ENABLE is set) */
#define LM_LICENSE_PKG_COMPONENT 0x4	/* a component from a PACKAGE */
#define LM_LICENSE_PKG_BUNDLE 	0x8	/* Similar to PKG_SUITE */
#define LM_LICENSE_PKG_SUITEBUNDLE (LM_LICENSE_PKG_SUITE | \
				LM_LICENSE_PKG_BUNDLE)

			struct config FAR *components;   /* If a PACKAGE */
			struct config FAR *parent_feat;  /* If a component --
							    assoc. FEATURE
							    line */
			struct config FAR *parent_pkg;	/* If a component,
							   points to associated
							   pkg */
			unsigned char conf_state; 	/* for PORT@HOST PLUS */
#define LM_CONF_COMPLETE 0 /* completely correct -- not +port@host */
#define LM_CONF_MARKER   1 /* contains nothing at all */
#define LM_CONF_CODE	 2 /* Only feature name and license-key are correct */
#define LM_CONF_BASIC  	 3 /* Missing optional information & pointers */
#define LM_CONF_OPTIONS  4 /* Everything but pointers -- servers, next, etc. */
#define LM_CONF_REMOVED  0xff
			char conf_featdata; /* exists only in FEATDATA
					       (l_check.c) and should only
					       be free'd there */

			unsigned char decimal_fmt;
#define LM_CONF_DECIMAL_FMT_V6 	0x1
#define LM_CONF_DECIMAL_FMT 	LM_CONF_DECIMAL_FMT_V6
			unsigned char borrow_flags;
#define LM_CONF_BORROWED 1
#define LM_CONF_FLOAT_OK 2			/* indicates checkout from */
						/* license-server, not dongle */
			int file_order;
			char *sort_str;		/* for internal use */
			char *date_sort_str;	/* for internal use */

            /* points to keys for current vendor needed by client. This will
             * usually be the vendor whose name matches the daemon name in
             * this struct.
             */
			LM_KEYLIST *lc_keylist;
			/*int *lc_key_number;	 SIGNn= */
			char *lc_sign;		/* SIGN= */
			int strength_override;	/* lic gen only */
 			char hash[30];
			HOSTID *floatid; 	/* license-server node:
						 * the one node where the
						 * node-locked checkout will
						 * fail, to prevent an extra
						 * use.   If ANY, we don't
						 * do that check.
						 */
			int	upgrade_count;		/* for internal use */
			int	upgrade_remaining;	/* for internal use */
/*
 *			Links
 */
			struct config FAR *next;	/* Ptr to next one */
			struct config FAR *last;	/* Ptr to previous */
			struct _priv_config_data * pPrivData;
		      } CONFIG, FAR *CONFIG_PTR, FAR * FAR *CONFIG_PTR_PTR;

/*
 *	LM_VENDOR_HOSTID -- for vendor-defined hostids
 *	arg to LM_A_VENDOR_ID_DECLARE
 */
typedef struct lm_vendor_hostid {
				char *label;		/* string to uniquely
							   identify your
							   hostid */
				int  hostid_num;	/* first should be
							   HOSTID_VENDOR,
							   then
							   HOSTID_VENDOR+n */
				char case_sensitive;	/* if false, will
							   compare hostids
							   ignore case */
#ifdef PC
				HOSTID_PTR (LM_CALLBACK_TYPE *get_vendor_id)
					lm_args((short idtype));
#else
				HOSTID_PTR (*get_vendor_id)
						lm_args((short idtype));
#endif /* Unix */

							/* routine to
							   get all
							   vendor-defined
							   hostids */
#ifdef LM_INTERNAL
				struct lm_vendor_hostid *next;
#endif /* LM_INTERNAL */
			} LM_VENDOR_HOSTID, FAR * LM_VENDOR_HOSTID_PTR;

#ifdef LM_INTERNAL
/*
 *	User customization - CLIENT LIBRARY use only
 */

struct lm_handle;

typedef void (FAR *LM_PFV) ();

typedef struct lm_options {

	short max_license_len;
	short disable_env;		/* for creating licenses def:70*/
	LM_CHAR_PTR config_file;	/* The license file */
	short got_config_file;	/* Flag to indicate whether config_file
							is filled in */
	int check_interval;	/* Check interval (sec) (- implies no check) */
	int retry_interval;	/* Reconnection retry interval */
	int timer_type;
	int retry_count;	/* Number of reconnection retrys */
	int conn_timeout;	/* How long to wait for connect to complete */
	short normal_hostid;	/* 0 for extended, <> 0 for normal checking */
	int (FAR *user_exitcall) lm_args((LM_CHAR_PTR feature));
				/* Pointer to (user-supplied) exit handler */
	int (FAR *user_reconnect) lm_args((LM_CHAR_PTR feature,
				       int pass, int max, int interval));
				/* Pointer to (user) reconnection handler */
	int (FAR *user_reconnect_done) lm_args((LM_CHAR_PTR feature,
					    int tries, int max, int interval));

				/* Pointer to (user-supplied) exit handler */
	int (FAR *user_exitcall_ex) lm_args((struct lm_handle * pJob, LM_CHAR_PTR feature, void * pUserData));
				/* Pointer to (user) reconnection handler */
	int (FAR *user_reconnect_ex) lm_args((struct lm_handle * pJob, LM_CHAR_PTR feature,
				       int pass, int max, int interval, void * pUserData));
				/* Pointer to reconnection-complete handler */
	int (FAR *user_reconnect_done_ex) lm_args((struct lm_handle * pJob, LM_CHAR_PTR feature,
					    int tries, int max, int interval, void * pUserData));
				/* pointer to checkout filter called before local verification */
 	int (FAR *outfilter_ex) lm_args((struct lm_handle * pJob, CONFIG_PTR conf, void * pUserData));
				/* pointer to checkout filter called after local verification */
  	int (FAR *outfilter_last_ex) lm_args((struct lm_handle * pJob, CONFIG_PTR conf, void * pUserData));

	char user_override[MAX_LONGNAME_SIZE+1];	/* Override username */
	char host_override[MAX_LONGNAME_SIZE+1];	/* Override hostname */
	char display_override[MAX_LONGNAME_SIZE+1];	/* Override display */
	char platform_override[MAX_PLATFORM_NAME+1];	/* Override platform */
	char vendor_checkout_data[MAX_VENDOR_CHECKOUT_DATA+1];
				/* vendor-defined checkout data */
	int (FAR *periodic_call) lm_args((lm_noargs));
				/* User-supplied call every few times
							thru lm_timer() */
	int periodic_count;	/* # of lm_timer() per periodic_call() */
	int periodic_counter;	/* to keep track of periodic_count */
	short no_traffic_encrypt;	/* Do not encrypt traffic */
	int max_timediff;	/* Maximum time diff: client/server (minutes) */
	LM_CHAR_PTR_PTR ethernet_boards; /* User-supplied Ethernet device table */
				/*  list of string ptrs, ending with a
						NULL pointer */
	long linger_interval;	/* How long license lingers after program exit
						or checkin (seconds) */
	void (FAR *setitimer)();	/* Substitute for setitimer() */
	LM_PFV (FAR *sighandler)();	/* Substitute for signal() */
	short try_old_comm;	/* Does l_connect() try old comm version code */
				/* Get hostid by vendor-defined type */
	int (FAR *check_vendor_id) lm_args((HOSTID_PTR id1, HOSTID_PTR id2));
			/* Compare 2 hostids */
	LM_CHAR_PTR (FAR *print_vendor_id) lm_args((HOSTID_PTR id));
				/* Print vendor hostid */
	int (FAR *outfilter) lm_args((CONFIG_PTR conf));
				/* checkout filter */
	int commtype;			/* user-requested commtype
					 * LM_TCP, LM_LOCAL */
	int allow_set_transport; /* disallow users setting commtype */
				 /* default = 1 -- allow users to reset
				  *or TCP */
	int transport_reset;	 /* init 0 */
#define LM_RESET_BY_USER 1
#define LM_RESET_BY_APPL 2
	short cache_file;	/* Does l_init_file() cache the LF data --
						lmgrd ONLY */
	short disable_finder;	/* Disable finder - daemons and lm_set_attr */
	LM_CHAR_PTR finder_path;	/* Cached finder path name */
	short capacity;

	long flags;			/* 32 boolean flags */
#define LM_OPTFLAG_GENERIC_SERVER	0x1 /* True if ok, default False */
#define LM_OPTFLAG_PORT_HOST_PLUS	0x2 /* True if ok, default True */
#define LM_OPTFLAG_DIAGS_ENABLED	0x4 /* True if ok, default True */
#define LM_OPTFLAG_TRY_OLD_COMM		0x8 /* True if ok, default False */
#define LM_OPTFLAG_RETRY_CHECKOUT      0x10 /* True if retry, default False */
#define LM_OPTFLAG_SUPPORT_HP_IDMODULE 0x20 /* default False */
#define LM_OPTFLAG_CHECK_BADDATE       0x40 /* default False */
#define LM_OPTFLAG_NO_HEARTBEAT       0x200 /* default False */
#define LM_OPTFLAG_LONG_ERRMSG        0x400 /* default True */

#define LM_OPTFLAG_PC_PROMPT_FOR_FILE 0x2000  /* default true */
#define LM_OPTFLAG_INTERNAL1 	      0x4000
#define LM_OPTFLAG_CKOUT_INSTALL_LIC 0x10000  /* default true */
#define LM_OPTFLAG_PERROR_MSGBOX     0x20000  /* default true */
#define LM_OPTFLAG_INTERNAL3         0x40000  /* default true */
#define LM_OPTFLAG_JAVA_LIC_FMT     0x200000  /* default is false */
#define LM_OPTFLAG_BORROW_RETURN			0x10000000	/* default is false */
#define LM_OPTFLAG_FQDN						0x20000000	/* default is false */

#ifdef PC
	long windows_module_handle;
#endif /* PC */
  	void * pUserData;

 	} LM_OPTIONS, FAR *LM_OPTIONS_PTR;
#endif /* LM_INTERNAL */

/*
 *	Data associated with a VENDOR (connection info, license file
 *		data pointers, etc.) - CLIENT LIBRARY use only
 */

typedef struct lm_daemon_info {
	short type;			/* Structure ID */
	struct lm_daemon_info FAR *next;/* Forward ptr */
	int commtype;			/* Actual Communications type
					 * LM_TCP,  LM_LOCAL */
	LM_SOCKET sock;		/* Socket file descriptor */
	int usecount;			/* Socket use count */
	int serialno;			/* Socket "serial #" */
	LM_SERVER_PTR server;		/* servers associated with socket */
	char daemon[MAX_DAEMON_NAME+1]; /* Which daemon socket refers to */
	struct _FlexCrypt *encryption; /* Handshake encryption code */
	int comm_version;		/* Communications version of server */
	int comm_revision;		/* Communications rev of server */
	int our_comm_version;		/* Our current comm version */
	int our_comm_revision;		/* Our current comm rev */
	unsigned short patch;			/* patch_level of server */
	int tcp_timeout;	/* similar for TCP, but ONLY applies
				 * when a client node crashes or is
				 * disconnected from net
				 */
#define LM_TCP_TIMEOUT_INCREMENT	60*1	/* 1-minute increments */
	unsigned short ver;
	unsigned short rev;

		       } LM_DAEMON_INFO, FAR *LM_DAEMON_INFO_PTR;

typedef struct lm_err_info {
			int maj_errno;
			int min_errno;
			int sys_errno;
			LM_CHAR_PTR_PTR lic_files;
			char feature[MAX_FEATURE_LEN + 1];
			LM_CHAR_PTR context;
#ifdef LM_INTERNAL
			char const *short_err_descr;
			char const *long_err_descr;
			char const *sys_err_descr;
			LM_CHAR_PTR errstring;
			int warn;
			unsigned short mask;
			char flags;
#define LM_EIFLAG_NOVENDOR_FEATS 0x1
#endif /* LM_INTERNAL */
} LM_ERR_INFO, FAR *LM_ERR_INFO_PTR;

/*
 *	-------------------------------------------------------------
 *	LMGRD_STAT
 *	As of V6, we need a struct to deal with each lmgrd we talk
 *	to.  Each lmgrd may support any number of license files.
 *	Therefore, we ask lmgrd for a list of it's files (if it's
 *	a v6 lmgrd), and download the license from the server.
 */

typedef struct _lmgrd_stat {
	struct _lmgrd_stat *next;
	int up;		/* if true, server is running and responding */
	char *license_paths;	/* server sends the full path it's using */
	LM_SERVER *server;	/* contains hostname and port number */
	int flexlm_ver;		/* version and rev of lmgrd */
	int flexlm_rev;
	char *license_file;	/* this is the path we used to talk to server */
	char *vendor_daemons;	/* space separated list of daemons supported */
	char port_at_host[MAX_HOSTNAME + MAX_SHORT_LEN + 5];
	LM_ERR_INFO e;		/* any errors confronted connecting to server */
} LMGRD_STAT;

typedef LMGRD_STAT *LMGRD_STAT_PTR, **LMGRD_STAT_PTR_PTR;

#ifdef LM_INTERNAL
#include <stdio.h>
#include <setjmp.h>

typedef FILE FAR *LM_FILE_PTR;
typedef char * LM_TIMER;


/*************************************************************/
/** message queue to hold the heartbeat response and
 * feature available messages if they are received before we are
 * ready for them
 **************************************************************/
typedef struct lm_hostid_redirect {
				HOSTID *from;
				HOSTID *to;
				struct lm_hostid_redirect FAR *next;
				} LM_HOSTID_REDIRECT,
				  FAR *LM_HOSTID_REDIRECT_PTR;

#endif /* LM_INTERNAL */

typedef struct _lm_borrow_stat {
	struct _lm_borrow_stat *next;
	char feature[MAX_FEATURE_LEN+1];
	char vendor[MAX_VENDOR_NAME + 1];
	time_t start;
	time_t end;
	char code[MAX_CRYPT_LEN + 1];
	int borrow_binary_id;
} LM_BORROW_STAT;

/** version struct for major and minor numbers */
typedef struct _m_version
{
	unsigned short	major;		/** version */
	unsigned short	minor;		/** revision */
} M_VERSION;

/** flexlm version */
#define MAX_VERSION_STRING	100
typedef struct _flex_version
{
	M_VERSION		version;	/** major and minor version */
	unsigned short	subMinor;	/** sub minor version */
	unsigned short	patch;		/** patch number */
	unsigned long	build;		/** build number */
	unsigned short	beta;		/** only set if this is a beta release */
	char			patchStr[2];	/* the patch char */
	char			verString[MAX_VERSION_STRING];
} FLEX_VERSION;

/*
 *  COMPOSITE HostID definition
 */

struct _composit_id_info;
typedef struct license_file *LF_POINTER;
/*
 *	Handles returned by FLEXlm
 */

typedef struct lm_handle {
	int type;		/* Type of struct */
#ifdef LM_INTERNAL
	FLEX_VERSION			version;	/* the flexlm version from the vendor code */
	struct lm_handle FAR *	first_job; /* First job in list */
	struct lm_handle FAR *	next;/* Next job in list */
	LM_ERR_INFO err_info;
	LM_DAEMON_INFO_PTR daemon; /* Daemon data */
    LM_OPTIONS_PTR options;    /* Options for this job */
	LM_HOSTID_REDIRECT_PTR redirect; /* Hostid redirection */
	CONFIG_PTR line;	/* Pointer to list of license file lines & components*/
	CONFIG_PTR packages;	/* list of packages */
	LM_CHAR_PTR_PTR lic_files; /* Array of license file names*/
	int lfptr;		/* Current license file ptr */
	int lm_numlf;	/* Number of license files */
	LF_POINTER license_file_pointers; /* LF data pointers */
	LM_CHAR_PTR lic_file_strings; /* buffer for lic file names */
#define LFPTR_INIT -1
#define LFPTR_FILE1 0
   char vendor[MAX_VENDOR_NAME+1];	/* This is read only now */
   char alt_vendor[MAX_VENDOR_NAME+1]; /* This is read only now */
					      /* Vendor name from lm_init() */
   CONFIG_PTR conf;	/* Current FEATURE line from "line" list above */
   LM_CHAR_PTR_PTR feat_ptrs;	/* lm_feat_list */
   LM_CHAR_PTR features;	/* lm_feat_list */
   HOSTID_PTR idptr;		/* lm_getid_type */
   long last_idptr_time;	/* lm_getid_type */
#define CHECK_HOSTID_INTERVAL 30 /* at most, check hostid every 30  seconds */
	LM_TIMER timer; 	/* gets cast to LM_TIMER */
	int feat_count;	/* signals are turned off when feat_count == 0*/
	int num_retries;	/* used for reconnecting */
	int idle;		/* job is idle */
	LM_CHAR_PTR featdata; /* used for heartbeats */

	unsigned long flags;
	unsigned char heartbeat_seq;
	unsigned char heartbeat_time;
	short num_minutes;	/* used by lc_heartbeat */
	long last_failed_reconnect;	/* lc_heartbeat */
	long last_heartbeat;		/* lc_heartbeat */
	long *recent_reconnects;	/* lc_heartbeat */

	LM_SERVER_PTR servers; /* used for lc_master_list */
	LM_SERVER_LIST_PTR conf_servers;
#ifdef FIFO
	LM_LOCAL_DATA_PTR localcomm; /* allocated if LM_LOCAL */
#endif /* FIFO */
#ifdef PC
	unsigned char dongle_ports[4]; /* [0] = FLEXID1 [1] = FLEXID2, etc */
#endif /* PC */
	char *asc_hostid_buf;
	char *perror_buf;
	char *saved_hostname;
	char *display;
	char *hostname;
	char *username;

	unsigned short last_ckout_sernum;
	unsigned long group_id;
	int config_number; 	/* for ordering feats */
	char *curr_registry;
#ifndef PC
	char *rcfile;
	time_t rcfile_t;
	long rcfile_s;
	char *borrfile;
	time_t borrfile_t;
	long borrfile_s;
#endif /* Unix */
	char *mt_info;
	char *path_env;
	char *vd_path_env;
	int port_end;
	unsigned int borrow_linger_minutes;
	LM_BORROW_STAT *borrow_stat;
	void *userlist;
	struct _private_data *internalData;

#endif /* LM_INTERNAL */
} LM_HANDLE,	/* Handle returned by certain calls */
		FAR *LM_HANDLE_PTR,
		FAR * FAR *LM_HANDLE_PTR_PTR;

typedef int (*LM_DS_AUTH) lm_args((char *, 	/* job context pointer */
				unsigned char *,/* binary data to auth*/
				int, 		/* length of text */
				unsigned char *,/* binary signature */
				int,		/* signature length */
				int *,		/* pubkeysize */
				unsigned char pubkey[LM_PUBKEYS][LM_MAXPUBKEYSIZ],/* pubkey */
				int,		/* pubkeyinfo1 */
				int));		/* sign_level */
						/* returns 0 == success,
							<>0 == failure */

typedef int (*LM_DS_GEN) lm_args((char *, 	/* job context pointer */
				unsigned char *,/* binary data to sign */
				int, 		/* length of data */
				char *,		/* LM_VENDORCODE_PUBKEYINFO * */
				unsigned char *,/* return signature */
				int *));	/* input: length of buffer
						   output: length of signature
						   or if length not large
						   enough, needed length
						   */
						/* returns 0 == success,
						   <>0 == failure */

/*
 *	User data returned from the license server
 */

typedef int LM_LICENSE_HANDLE;
typedef struct lm_users {
			   struct lm_users FAR *next;
			   char name[MAX_LONGNAME_SIZE + 1];
			   char node[MAX_LONGNAME_SIZE + 1];
			   char display[MAX_LONGNAME_SIZE + 1];
			   char vendor_def[MAX_VENDOR_CHECKOUT_DATA + 1];
			   int nlic;	/* Number of licenses */
			   short opts;	/* options flag */
#define INQUEUE		0x1	/* User is in queue */
#define HOSTRES		0x2	/* Reservation for a host "node" */
#define USERRES		0x4	/* Reservation for user "name" */
#define DISPLAYRES	0x8	/* Reservation for display "name" */
#define GROUPRES	0x10	/* Reservation for group "name" */
#define INTERNETRES	0x20	/* Reservation for internet "name" */
#define BORROWEDLIC	0x40	/* Unused */
#define UNKNOWNRES	0x80	/* Unknown reservation type from daemon */
#define HOSTGROUPRES	0x100   /* Reservation for group "node" */
#define PROJECTRES      0x200	/* Reservation for $LM_PROJECT */
#define LM_USERS_BORROW 0x400	/* LINGER is really a borrow */
#define BUNDLERES	0x800	/* BUNDLED reservation */
#define lm_isres(x) ((x) & (HOSTGROUPRES | HOSTRES | USERRES | DISPLAYRES | GROUPRES | INTERNETRES | PROJECTRES | BORROWEDLIC | UNKNOWNRES | BUNDLERES))
						/* This is a reservation */
			   long time;		/* Seconds value from timeval */
			   char version[MAX_VER_LEN+1];
						/* Version of software */
			   long linger;		/* Linger interval */
			   CONFIG_PTR ul_conf;	/* CONFIG associated */
			   LM_LICENSE_HANDLE  ul_license_handle;
						/* Server's license handle */
			   char project[MAX_PROJECT_LEN + 1];
			   int processID;
			 } LM_USERS, FAR *LM_USERS_PTR;

/*
 *	struct definition for lm_get_attr(LM_A_VD_GENERIC_INFO, ...)
 *			Matches flags available in ls_vendor.c.
 */
typedef struct _lm_vd_generic_info {
	int type; 		/* LM_VD_GENINFO_HANDLE_TYPE */
	CONFIG_PTR feat;	/* pointer to FEATURE line for daemon */
	char user_init1;	/* flag on/off */
	char user_init2;	/* flag on/off */
	char outfilter;		/* flag on/off */
	char infilter;		/* flag on/off */
	char callback;		/* flag on/off */
	char vendor_msg;	/* flag on/off */
	char vendor_challenge;	/* flag on/off */
	char lockfile;		/* flag std/non-std */
	int read_wait;		/* How long to wait for solicited reads */
	char dump_send_data;	/* flag on/off */
	char normal_hostid;	/* flag/unused */
	int conn_timeout;	/* How long to wait for a connection */
	char enforce_startdate; /* flag on/off */
	char tell_startdate; 	/* flag on/off */
	int minimum_user_timeout; /* Minimum user inactivity timeout (seconds)
					<= 0 -> activity timeout disabled */
	int min_lmremove;	/* Minimum amount of time (seconds) that a
				   client must be connected to the daemon before
				   an "lmremove" command will work. */
	char use_featset;	/* flag on/off */
	int dup_sel;		/* V1 compatability */
	char use_all_feature_lines; /* flag on/off */
	char do_checkroot; 	/* flag on/off */
	char show_vendor_def;	/* flag on/off */
	char allow_borrow;	/* unused */
	char redirect_verify;	/* flag on/off */
	char periodic_call;	/* flag on/off */
	char compare_on_increment; /* flag on/off */
	char compare_on_upgrade; /* flag on/off */
	int  version;		/* FLEXLM_VERSION */
	int  revision;		/* FLEXLM_REVISION */
	int  lite;
	time_t lmgrd_start;		 /* these need to be cast to time_t */
	time_t vd_start;			/* before being used */
	time_t server_current_time;
			} LM_VD_GENERIC_INFO;

/*
 *	struct definition for lm_get_attr(LM_A_VD_FEATURE_INFO, ...)
 *		user sets type to
 */
typedef struct _lm_vd_feature_info_1 {
			int type; 		/* LM_VD_FEATINFO_HANDLE_TYPE */
			CONFIG_PTR feat;	/* pointer to FEATURE line */
			int rev;		/* rev of this struct
						   (was lowwater) */
			int timeout;		/* User inactivity timeout */
			int linger;		/* User LINGER specification */
			short dup_select;	/* duplicate mask */
			int res;		/* # reserved licenses */
			int tot_lic_in_use;	/* total res && non-res in
						   use (was maxborrow)*/
			int float_in_use;	/* non-reserved licenses
						   in use */
			int user_cnt;		/* # of processes with this
						   feature checked out */
			int num_lic;		/* max num of lic avail in this
						   vd-pool for this feature */
			int queue_cnt;		/* number of users queued
						   -- there's no way to predict
						   how many licenses these
						   users will use until they
						   actually use a license */
			int overdraft;		/* # overdraft licenses */
			char code[MAX_CRYPT_LEN+1]; /* currently unused */
			unsigned long borrowed;	/* The number of licenses
						 * in use that are
						 * borrowed
						 */
			int maxborrowhours;	/* Max hours a license can be borrowed. */
		} LM_VD_FEATURE_INFO;

/*
 *	structure definition used with lc_set_attr_vendor(LM_SA_COMPOSITE_ID_INIT) to set
 *	composite hostid info for common vendor daemon.
 */
typedef struct
{
	int		numIDs;		/* Number of IDs in list */
	int *	pIDList;	/* List of ID types */
} COMPOSITE_HOSTID_INFO;



#define LM_REAL_TIMER    1234
#ifndef LM_FLEXLM_DIR
#ifdef PC
#define LM_FLEXLM_DIR	"C:\\flexlm"
#else
#ifdef MAC10
#define LM_FLEXLM_DIR	"/var/tmp/.flexlm"
#else
#ifdef MONTAVISTA
#define LM_FLEXLM_DIR	"/tmp/.flexlm"
#else
#define LM_FLEXLM_DIR	"/usr/tmp/.flexlm"
#endif /* MONTAVISTA */
#endif /* MAC10 */
#endif /* PC */
#endif

#ifndef CFCFLEX
#ifdef WINNT
#include <windows.h>
#include <io.h>
#include <direct.h>

typedef HRESULT(WINAPI *L_SHGETFOLDERPATHA)(HWND, int, HANDLE, DWORD, LPSTR);
#define CSIDL_FLAG_CREATE               0x8000
#define CSIDL_PROGRAM_FILES_COMMON      0x002b
#define CSIDL_COMMON_APPDATA			0x0023
#define OS_NT_WIN2K_XP                  1	/* This one also used for Windows Vista */
#define OS_95_98_ME                     2
#define OS_WIN32S                       3
#define DEFAULT_FLEXLM_DIR				"c:\\flexlm"
#define	DIRECTORY_SEPARATOR				"\\"
#else /* !WINNT */
#define	DIRECTORY_SEPARATOR				"/"
#define MAX_PATH						4096
#if defined(PATH_MAX) && PATH_MAX > MAX_PATH
#undef MAX_PATH
#define MAX_PATH						PATH_MAX
#endif
#endif
#endif
/*
 *	FLEXlm library prototypes
 */

#if (defined(_WINDLL) || defined(_WINDOWS)) && !defined(WINNT) && !defined(OS2)
#define API_ENTRY	_far _pascal  /* Windows DLL API function entry type */
#else
#define API_ENTRY
#endif
lm_extern CONFIG_PTR API_ENTRY lc_auth_data lm_args((LM_HANDLE_PTR job,
						  LM_CHAR_PTR feature));
lm_extern int API_ENTRY lc_baddate 	lm_args((LM_HANDLE_PTR job));
lm_extern int API_ENTRY lc_borrow_return lm_args((LM_HANDLE *, char *, char *));
lm_extern int API_ENTRY lc_check_key lm_args((LM_HANDLE_PTR, CONFIG_PTR,
					const VENDORCODE_PTR));
lm_extern void API_ENTRY lc_checkin lm_args((LM_HANDLE_PTR, const LM_CHAR_PTR,
							int ));
lm_extern int API_ENTRY lc_checkout lm_args((LM_HANDLE_PTR , const LM_CHAR_PTR ,
				       const LM_CHAR_PTR  , int , int ,
				       const VENDORCODE_PTR , int ));
lm_extern LM_CHAR_PTR API_ENTRY lc_chk_conf lm_args(( LM_HANDLE *, CONFIG *,
									int));
lm_extern int API_ENTRY lc_ck_feats lm_args((LM_HANDLE_PTR job, LM_CHAR_PTR vendor));
lm_extern void API_ENTRY lc_cleanup lm_args((lm_noargs));
lm_extern int API_ENTRY lc_convert lm_args(( LM_HANDLE *, char *, char **,
						char **, int));
lm_extern HOSTID * API_ENTRY lc_copy_hostid lm_args((LM_HANDLE_PTR, HOSTID *));
lm_extern LM_CHAR_PTR API_ENTRY lc_crypt lm_args((LM_HANDLE_PTR job,
				      CONFIG_PTR conf, LM_CHAR_PTR sdate,
					      VENDORCODE_PTR code));
lm_extern int API_ENTRY lc_cryptstr lm_args((LM_HANDLE_PTR, LM_CHAR_PTR,
					LM_CHAR_PTR_PTR, VENDORCODE_PTR,
					int, LM_CHAR_PTR, LM_CHAR_PTR_PTR));
lm_extern  LM_CHAR_PTR API_ENTRY lc_curr_date lm_args((LM_HANDLE_PTR));
lm_extern LM_CHAR_PTR API_ENTRY lc_daemon lm_args((LM_HANDLE_PTR job,
					LM_CHAR_PTR daemon, LM_CHAR_PTR options,
					LM_INT_PTR port));
lm_extern int API_ENTRY lc_disconn lm_args((LM_HANDLE_PTR job, int force));
lm_extern LM_CHAR_PTR API_ENTRY lc_display lm_args((LM_HANDLE_PTR job, int flag));
lm_extern LM_ERR_INFO_PTR API_ENTRY lc_err_info lm_args((LM_HANDLE_PTR job));
lm_extern LM_CHAR_PTR API_ENTRY lc_errstring lm_args((LM_HANDLE_PTR job));
lm_extern LM_CHAR_PTR API_ENTRY lc_errtext lm_args((LM_HANDLE_PTR,int ));
lm_extern void API_ENTRY lc_get_version(FLEX_VERSION *ver);
lm_extern long API_ENTRY lc_expire_days lm_args((LM_HANDLE_PTR , CONFIG_PTR ));
#define LM_FOREVER 3650000 /* a date beyond 31-dec-9999 */
lm_extern LM_CHAR_PTR_PTR API_ENTRY lc_feat_list lm_args((LM_HANDLE_PTR job,
						       int flags,
						void (FAR *dupaction)(lm_noargs)));
#define LM_FLIST_ONLY_FLOATING 0x1 /* xor into flags */
#define LM_FLIST_ALL_FILES 0x2

lm_extern LM_CHAR_PTR API_ENTRY lc_feat_set lm_args((LM_HANDLE_PTR job,
						  LM_CHAR_PTR daemon,
						  VENDORCODE_PTR code,
						  LM_CHAR_PTR_PTR codes));
lm_extern LM_HANDLE_PTR API_ENTRY lc_first_job lm_args((LM_HANDLE_PTR ));
lm_extern int API_ENTRY l_flush_config lm_args((LM_HANDLE_PTR job));
lm_extern void API_ENTRY lc_free_config(LM_HANDLE * job, CONFIG *	pConfig);
lm_extern void API_ENTRY lc_free_hostid lm_args((LM_HANDLE_PTR, HOSTID *));
lm_extern void API_ENTRY lc_free_job lm_args((LM_HANDLE_PTR job));
lm_extern void API_ENTRY lc_free_lmgrd_stat lm_args(( LM_HANDLE_PTR,
					LMGRD_STAT_PTR));
lm_extern void API_ENTRY lc_free_mem lm_args((LM_HANDLE_PTR, LM_CHAR_PTR));
lm_extern int API_ENTRY lc_get_attr lm_args((LM_HANDLE_PTR job, int key,
					 LM_SHORT_PTR val));
lm_extern CONFIG_PTR API_ENTRY lc_get_config lm_args((LM_HANDLE_PTR job,
						   LM_CHAR_PTR feature));

lm_extern int API_ENTRY lc_get_errno lm_args((LM_HANDLE_PTR job));
lm_extern LM_CHAR_PTR API_ENTRY lc_get_feats lm_args((LM_HANDLE_PTR job,
						LM_CHAR_PTR daemon));
lm_extern int API_ENTRY  lc_get_registry	lm_args(( LM_HANDLE_PTR, char *, char **));
lm_extern HOSTID_PTR API_ENTRY lc_gethostid lm_args((LM_HANDLE_PTR job));
lm_extern HOSTID_PTR API_ENTRY lc_getid_type lm_args((LM_HANDLE_PTR job,
						   int idtype));

lm_extern int API_ENTRY lc_heartbeat lm_args((LM_HANDLE_PTR job,
			LM_INT_PTR ret_num_reconnects, int minutes ));
lm_extern int API_ENTRY lc_hostid lm_args((LM_HANDLE_PTR job, int type, char *buf));
lm_extern LM_CHAR_PTR API_ENTRY lc_hostname lm_args((LM_HANDLE_PTR job, int flag));
lm_extern void API_ENTRY lc_idle lm_args((LM_HANDLE_PTR job, int flag));
lm_extern int API_ENTRY lc_init lm_args((LM_HANDLE_PTR job, LM_CHAR_PTR vendor_id,
				     VENDORCODE_PTR vendor_key,
				     LM_HANDLE_PTR_PTR job_id));
lm_extern int API_ENTRY lc_install_license lm_args((LM_HANDLE_PTR, char *, char **,
						int *));
lm_extern int API_ENTRY lc_install_license_path lm_args(( LM_HANDLE_PTR, char *,
						char *));
lm_extern int API_ENTRY lc_isadmin lm_args((LM_HANDLE_PTR job, LM_CHAR_PTR user));
lm_extern LM_CHAR_PTR API_ENTRY lc_lic_where lm_args((LM_HANDLE_PTR job));
lm_extern void API_ENTRY lc_log lm_args((LM_HANDLE_PTR job, LM_CHAR_PTR msg));
lm_extern LM_SERVER_PTR API_ENTRY lc_master_list lm_args((LM_HANDLE_PTR job));
lm_extern int API_ENTRY lc_nap lm_args((LM_HANDLE_PTR, int));
#define L_NEW_JOB l_n36_buf
typedef int (*lc_new_job_cb)(char *, VENDORCODE *, int, int, int, int *, char * );
lm_extern lc_new_job_cb L_NEW_JOB;

#if ! defined( FLEXLM_DLL) && (defined(PC) && !defined(FLEX_STATIC))
#define lc_new_job_arg2 L_NEW_JOB
#else
#define lc_new_job_arg2 0
#define LM_DLL_SIGN
#endif
lm_extern int API_ENTRY lc_new_job lm_args((LM_HANDLE_PTR, lc_new_job_cb,
					VENDORCODE_PTR, LM_HANDLE_PTR_PTR ));

lm_extern CONFIG_PTR API_ENTRY lc_next_conf lm_args((LM_HANDLE_PTR job,
						     LM_CHAR_PTR feature,
						     CONFIG_PTR_PTR pos));
lm_extern LM_HANDLE_PTR API_ENTRY lc_next_job lm_args((LM_HANDLE_PTR ));
lm_extern void API_ENTRY lc_perror lm_args((LM_HANDLE_PTR job, LM_CHAR_PTR msg));
lm_extern int API_ENTRY lc_remove lm_args((LM_HANDLE_PTR job, LM_CHAR_PTR feature,
				       LM_CHAR_PTR user, LM_CHAR_PTR host,
				       LM_CHAR_PTR display));
lm_extern int API_ENTRY lc_removeh lm_args((LM_HANDLE_PTR job, LM_CHAR_PTR feature,
					LM_CHAR_PTR server, int port,
					LM_CHAR_PTR handle));
lm_extern int API_ENTRY lc_set_attr lm_args((LM_HANDLE_PTR job, int key,
					 LM_A_VAL_TYPE value));
lm_extern int API_ENTRY lc_set_attr_vendor lm_args((LM_HANDLE_PTR job,
                     LM_CHAR_PTR vendorName, int attribute, LM_A_VAL_TYPE value));
lm_extern void API_ENTRY lc_set_errno lm_args((LM_HANDLE_PTR , int));
lm_extern int API_ENTRY lc_set_registry	lm_args(( LM_HANDLE_PTR, char *, char *));
lm_extern int API_ENTRY lc_shutdown lm_args((LM_HANDLE_PTR job, int prompt,
					 int print));
lm_extern int API_ENTRY l_shutdown lm_args((LM_HANDLE_PTR job, int prompt,
					 int print, LM_CHAR_PTR, LM_CHAR_PTR,
					 LMGRD_STAT_PTR, LMGRD_STAT_PTR_PTR));
lm_extern int API_ENTRY lc_startup lm_args((LM_HANDLE_PTR, char *, char *, char *));
lm_extern int API_ENTRY lc_status lm_args((LM_HANDLE_PTR job, LM_CHAR_PTR prompt));
lm_extern CONFIG_PTR API_ENTRY lc_test_conf lm_args((LM_HANDLE_PTR job));
lm_extern int API_ENTRY lc_timer lm_args((LM_HANDLE_PTR ));
lm_extern LM_USERS_PTR API_ENTRY lc_userlist lm_args((LM_HANDLE_PTR job,
						   LM_CHAR_PTR feature));
lm_extern LM_CHAR_PTR API_ENTRY lc_username lm_args((LM_HANDLE_PTR job, int flag));
lm_extern LM_CHAR_PTR API_ENTRY lc_vsend lm_args((LM_HANDLE_PTR job,
					       LM_CHAR_PTR msg));
lm_extern void API_ENTRY  lc_set_vendor(LM_HANDLE * job, char * vendor);
lm_extern void API_ENTRY  lc_set_alt_vendor(LM_HANDLE * job, char * vendor);
lm_extern void API_ENTRY  lc_set_vendor_alias(LM_HANDLE * job, char * vendor);

lm_extern unsigned char API_ENTRY l_onebyte lm_args((unsigned x));
lm_extern unsigned API_ENTRY l_sum lm_args((char *str));
lm_extern unsigned API_ENTRY l_cksum lm_args((LM_HANDLE *lm_job,
					      CONFIG *conf, int *bad,
					      VENDORCODE *dummycode));
lm_extern HOSTID_PTR API_ENTRY l_new_hostid lm_args((lm_noargs));

typedef struct hosttype {
			  int code; 	/* machine type (see lm_hosttype.h) */
			  LM_CHAR_PTR name;  /* Machine name, eg. sun 3/50 */
#define MAX_HOSTTYPE_NAME 50		/* Longest hosttype name length */
			  int flexlm_speed; /* Speed determined at run time */
			  int vendor_speed; /* Speed claim by vendor */
			} HOSTTYPE, FAR *HOSTTYPE_PTR;

lm_extern HOSTTYPE_PTR API_ENTRY lc_hosttype lm_args((LM_HANDLE_PTR job,
						  int run_benchmark));

lm_extern int lc_init_simple_composite lm_args((LM_HANDLE_PTR, int *, int));

/* This function is listed for historical reasons, but should not be used */

lm_extern LM_CHAR_PTR API_ENTRY l_extract_date 	lm_args((LM_HANDLE_PTR,
						LM_CHAR_PTR ));
lm_extern int API_ENTRY  lc_bin_date            lm_args((LM_CHAR_PTR date, 
                                                         LM_CHAR_PTR return_date));
lm_extern int la_init(LM_HANDLE *job);


/*
 *	Old (pre v4.0) FLEXlm API calls
 */

#define lm_auth_data(f) 	lc_auth_data(lm_job, f)
#define lm_baddate()		lc_baddate(lm_job)
#define lm_check()		lc_check(lm_job)
#define lm_chkdir(dir)		lc_chkdir(lm_job, dir)
#define lm_checkin(f, k)	lc_checkin(lm_job, f, k)
#define lm_checkout(f, v, n, fl, k, d) lc_checkout(lm_job, f, v, n, fl, k, d)
#define lm_chk_conf(s)		lc_ck_feats(lm_job, s)
#define lm_ck_feats(v)		lc_ck_feats(lm_job, v)
#define lm_copy_hostid(h)	lc_copy_hostid(lm_job, h)
#define lm_curr_date()		lc_curr_date(lm_job)
#define lm_daemon(d, o, p) 	lc_daemon(lm_job, d, o, p)
#define lm_disconn(force) 	lc_disconn(lm_job, force)
#define lm_display(flag) 	lc_display(lm_job, flag)
#define lm_errstring(x)		lc_errstring(lm_job)
#define lm_errtext(e)		lc_errtext(lm_job, e)
#define lm_feat_list(f, d)	lc_feat_list(lm_job, f, d)
#define lm_feat_list_lfp(l, f, d) lc_feat_list_lfp(lm_job, l, f, d)
#define lm_feat_set_lfp(l, d)	lc_feat_set_lfp(lm_job, l, d)
#define lm_feat_set(d, c, c2)	lc_feat_set(lm_job, d, c, c2)
#define lm_free_daemon_list(d)	lc_free_daemon_list(lm_job, d)
#define lm_free_job(j)		lc_free_job(j)
#define lm_free_mem(p)		lc_free_mem(j,p)
#define lm_free_hostid(h)	lc_free_hostid(lm_job, h)
#define lm_get_attr(a, v)	lc_get_attr(lm_job, a, v)
#define lm_get_config(f) 	lc_get_config(lm_job, f)
#define lm_get_feats(d)		lc_get_feats(lm_job, d)
#define lm_get_redir()		lc_get_redir(lm_job)
#define lm_gethostid()		lc_gethostid(lm_job)
#define lm_getid_type(d)	lc_getid_type(lm_job, d)
#define lm_hostname(f)		lc_hostname(lm_job, f)
#define lm_hosttype(r)		lc_hosttype(lm_job, r)
#define lm_idle(flag)		lc_idle(lm_job, flag)
#define lm_init(v, k, j)	lc_init(lm_job, v, k, j)
#define lm_isadmin(u)		lc_isadmin(lm_job, u)
#define lm_lic_where()		lc_lic_where(lm_job)
#define lm_license_dump(vendor)	lc_license_dump(lm_job, vendor)
#define lm_log(m)		lc_log(lm_job, m)
#define lm_master_list()	lc_master_list(lm_job)
#define lm_nap(ms)		lc_nap(lm_job, ms)
#define lm_next_conf(f, p) 	lc_next_conf(lm_job, f, p)
#define lm_node_lock(f, v, n, fl, k, d) lc_node_lock(lm_job, v, n, fl, k, d)
#define lm_perror(s)		lc_perror(lm_job, s)
#define lm_redir_ver(f, t, c, s) lc_redir_ver(lm_job, f, t, c, s)
#define lm_remove(f, u, h, d)	lc_remove(lm_job, f, u, h, d)
#define lm_removeh(f, s, p, h)	lc_removeh(lm_job, f, s, p, h)
#define lm_set_attr(a, v)	lc_set_attr(lm_job, a, v)
#define lm_status(f)		lc_status(lm_job, f)
#define lm_shutdown(p, print)	lc_shutdown(lm_job, p, print)
#define lm_startup(p, l, f)	lc_startup(lm_job, p, l, f)
#define lm_timer()		lc_timer(lm_job)
#define lm_userlist(f)		lc_userlist(lm_job, f)
#define lm_username(f)		lc_username(lm_job, f)
#define lm_vsend(m)		lc_vsend(lm_job, m)


#endif /* _LM_CLIENT_H_ */
