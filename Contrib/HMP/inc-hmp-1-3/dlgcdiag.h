/************************************************************************************
 *      Copyright (c) 2001-2002 Intel Corporation
 *
 *      THIS IS UNPUBLISHED PROPRIETARY SOURCE CODE OF Intel Corporation
 *      The copyright notice above does not evidence any actual or
 *      intended publication of such source code.
 ************************************************************************************/

/************************************************************************************
 * FILE: dlgcdiag.h
 *
 * DESCRIPTION: This header file provides typedefs and variable definitions for the
 *              APIs that comprise the dlgcdiag library.
 *
 * **********************************************************************************
 * REVISION HISTORY:
 *
 * 01/15/01 BHals       Original Version
 * 04/19/01 BHals       Modified following code review
 * 04/26/02 RCasson     Added definitions for 16 bit SRAM, werewolf
 *                      and 16 bit werewolf. Added argument in executePost 
 *                      prototype.
 * 05/16/02 RCasson     Changed POST_LAST_TEST from 1C to 1B (28) which is the 
 *                      last test.
 * 05/20/02 RCasson     Added DM3POST_QDRVSETPOSTSTATE.
 * 05/30/02 RCasson     Added enumerated test types 
 *
 ************************************************************************************/

 /*
 * Flag that this file has been included
 */
#ifndef  DLGCDIAG_H
#define  DLGCDIAG_H

#ifdef __cplusplus
extern "C" {
#endif

/*
 * function prototype for running the POST diagnostic tests
 */


#ifdef WIN32
#ifndef DLLExport
#define DLLExport _declspec(dllexport)
#endif
#endif

#if defined (UNIX) || defined (linux)
#ifndef DLLExport
#define DLLExport
#endif
#endif

extern int DLLExport executePost(unsigned int, unsigned int, int, int,
                                 unsigned char *,
                                 unsigned char *, char *);

#define DIAGMSG_MIN_SIZE    512

/*
 * Values for DM3 board slot and bus numbers
 */
#define SLOT_RANGE_START          0
#define SLOT_RANGE_END           31
#define BUS_RANGE_START           0

#ifdef WIN32
#define BUS_RANGE_END           0xFE
#endif

#ifdef UNIX
#define BUS_RANGE_END           0xFE
#endif

#define BUS_NUM_UNDEFINED       0xFF
#define SLOT_NUM_UNDEFINED      0xFF


/*
 * POST status definitions
 */
#define POST_IN_PROGRESS       0x01    /* POST is still in progress */
#define POST_PASS_BYTE1_MERC   0x03    /* POST testing completed successfully */
#define POST_PASS_BYTE1_WW     0x02    /* werewolf POST testing completed 
                                          successfully */
#define POST_FIRST_TEST_MERC   0x04    /* first test number */
#define POST_LAST_TEST_MERC    0x1B    /* last test number */
#define POST_PASS_BYTE2_MERC   0xFC    /* information value associated with 
                                          successful SRAM POST TEST*/
#define POST_FIRST_TEST_WW     0x11    /* first werewolf test number */
#define POST_LAST_TEST_WW      0x41    /* last werewolf test number */

#define POST_PASS_BYTE2_MERC16 0x7C    /* information value associated with
                                          successful SRAM 16 bit POST test */
#define POST_PASS_BYTE2_WW     0xFD    /* information value associated with 
                                          successful werewolf POST TEST*/
#define POST_PASS_BYTE2_WW16   0x7D    /* information value associated with 
                                          successful werewolf 16 bit POST TEST*/

/*
 * dm3post-defined diagnostic codes. These codes are not returned by the FW's POST
 * operation. They are defined here to represent error conditions that occur while
 * dm3post is operating. These codes will be returned to the calling function.
 */
#define DM3POST_ERROR	0xFF    /* Diagnostic byte 1 for dm3post error conditions */

/* 
 * if the dm3post tool returns 0xFF in the first diagnostic byte, the second
 * diagnostic byte will contain one of the following error codes
 */

/*
 * values 0x01 thru 0x2F apply to error messages that can occur on all platforms
 */
#define DM3POST_INVALIDSLOTENTRY            0x01
#define DM3POST_BOARDNOTFOUND               0x02
#define DM3POST_SLOTNUMNOTENTERED           0x03
#define DM3POST_MULTIPLESLOTNUMSENTERED     0x04
#define DM3POST_MULTIPLEBUSNUMSENTERED      0x05
#define DM3POST_AMBIGUOUSSLOTNUMBER         0x06
#define DM3POST_QMSGVARFIELDPUT             0x07
#define DM3POST_INVALIDBOARDSTATE           0x08

/*
 * values 0x30 thru 0x4F apply to error messages that can occur on WINDOWS platforms
 */
#ifdef WIN32
#define DM3POST_MNTENUMMPATHDEVICE          0x30
#define DM3POST_CREATEFILE                  0x31
#define DM3POST_MNTALLOCATEMMB              0x32
#define DM3POST_MNTSENDPOSTMESSAGE          0x33
#define DM3POST_MNTRESETBOARD               0x34
#define DM3POST_NCMGETVALUEEX               0x35
#define DM3POST_NCMGETINSTALLEDDEVICES      0x36
#define DM3POST_MNTGETPOSTLOCATIONCONTENT   0x37
#define DM3POST_INVALIDBUSENTRY             0x38
#define DM3POST_DEVICEIOCONTROL             0x39
#define DM3POST_MCCONFIGURECOMPONENT        0x3A
#endif

/*
 * values 0x50 thru 0x6F apply to error messages that can occur on Unix platforms
 */
#ifdef UNIX
#define DM3POST_INVALIDBUSENTRY             0x50
#define DM3POST_QDRVSETINTERFACE            0x51
#define DM3POST_QQUEUEOPEN                  0x52
#define DM3POST_QQUEUEBIND                  0x53
#define DM3POST_QDRVBRDMAP                  0x54
#define DM3POST_QDRVREPORTBOARDS            0x55
#define DM3POST_QDRVREPORTBRDSTATES         0x56
#define DM3POST_QDRVBRDSHUTDOWN             0x57
#define DM3POST_QDRVBRDGETDIAGSTATE         0x58
#define DM3POST_QDRVBRDSTART                0x59
#define DM3POST_QDRVPROTSTART               0x5A
#define DM3POST_NOBOARDSTATE                0x5B
#define DM3POST_QMSGALLOCATE                0x5C
#define DM3POST_QMSGWRITE                   0x5D
#define DM3POST_QDRVBRDCONFIG               0x5E
#define DM3POST_QMSGREAD                    0x5F
#define DM3POST_QDRVBOARDGETROM             0x60
#define DM3POST_QDRVSETPOSTSTATE            0x61
#define DM3POST_QDRVSTART                   0x62
#define DM3POST_QDRVBRDSTARTTIMEOUT         0x63
#define DM3POST_QWWSETPARM                  0x64
#endif

/* 
*  DTI16 test numbers  
*/
typedef int TEST_TYPE;
#define NULLTEST                        (TEST_TYPE) 0xFFFF
#define INVALIDTEST                     (TEST_TYPE)  0 
#define INSTRUCTION_SET                 (TEST_TYPE) 0x11
#define YAVAPAI                         (TEST_TYPE) 0x12
#define TIMER_INTERRUPT                 (TEST_TYPE) 0x13
#define CPUPCIBUS                       (TEST_TYPE) 0x14
#define HARDWARECONFIGCHECKSUM          (TEST_TYPE) 0x21
#define CODECHECKSUM                    (TEST_TYPE) 0x22
#define CONFIGROMCHECKSUM               (TEST_TYPE) 0x23
#define CHECKERBOARD                    (TEST_TYPE) 0x24
#define REVERSECHECKERBOARD             (TEST_TYPE) 0x25
#define ADDRESSTEST                     (TEST_TYPE) 0x26
#define TESTBYTEWORDACCESS              (TEST_TYPE) 0x27
#define FRONTPANELLEDS                  (TEST_TYPE) 0x31
#define HDLCCONTROLLER                  (TEST_TYPE) 0x32
#define CTBUS                           (TEST_TYPE) 0x33
#define PSTNDIGITALNETWORKINTERFACES    (TEST_TYPE) 0x34
#define PERIPHERALPCIBUS                (TEST_TYPE) 0x35
#define EIA_232SERIAL                   (TEST_TYPE) 0x36
#define IEEE802_3                       (TEST_TYPE) 0x37
#define TIMERSERVICES                   (TEST_TYPE) 0x38
#define SPSELF_TEST                     (TEST_TYPE) 0x41


#ifdef __cplusplus
}
#endif

#endif      /* end of DLGCDIAG_H */
