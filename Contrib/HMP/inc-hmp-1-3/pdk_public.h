/**********************************************************************
*
*   C Header:       pdk_public.h
*   Instance:       1
*   Description:    
*   %created_by:    parikhn %
*   %date_created:  Tue Apr 09 13:23:23 2002 %
*
**********************************************************************/
/**********************************************************************
*  Copyright (C) 1998-2002 Intel Corporation.
*  All Rights Reserved
*
*  All names, products, and services mentioned herein are the 
*  trademarks or registered trademarks of their respective 
*  organizations and are the sole property of their respective owners.
**********************************************************************/

#ifndef _1_pdk_public_h_H
#define _1_pdk_public_h_H

#ifndef lint
static 
#ifdef __cplusplus          
const             /* C++ needs const */     
#endif
char    *_1_pdk_public_h = "@(#) %filespec: pdk_public.h-9 %  (%full_filespec: pdk_public.h-9:incl:gc#1 %)";
#endif

/* Define the set ID for PDKRT protocol parameters */
#define PDKSET_PSI_VAR              0xA000
#define PDKSET_SERVICE_VAR          0xA001

/* Define the parm ID for PDKRT PSI parameters */
#define PDKPARM_EXT_SIG_INT1        0x0   /* Protocol Extension Integer 1 */
#define PDKPARM_EXT_SIG_INT2        0x1   /* Protocol Extension Integer 2 */
#define PDKPARM_EXT_SIG_INT3        0x2   /* Protocol Extension Integer 3 */
#define PDKPARM_EXT_SIG_STR1        0x3   /* Protocol Extension String 1  */
#define PDKPARM_EXT_SIG_STR2        0x4   /* Protocol Extension String 2  */

/*
   Define the extension function ID for PDKRT extension functions
   supported by gc_Extension( )
*/
#define PDK_EXT_ID_PROT_EXTENSION   0x0


                    /********************************/
                    /* PDK MakeCall block structure */
                    /********************************/

/*
-- Note: this is set up to be exactly the same as IC_MAKECALL_BLK for ICAPI
-- thus allowing applications currently using ICAPI to run using the PDK
-- without additional changes for gc_MakeCall()
*/

#ifndef NO_CALL_PROGRESS                        /* In case app also included icapi.h    */
#define     NO_CALL_PROGRESS    0x1             /* Normally we would not use a          */
                                                /* double negative, but do so herehere  */
                                                /* to maintain backward compatability   */
                                                /* with ICAPI                           */
#endif

#define MEDIA_TYPE_DETECT  0x2


typedef struct pdk_makecall_blk {
    unsigned long       flags;                  
    void                *v_rfu_ptr;             /* RFU */
    unsigned long       ul_rfu[4];              /* RFU */
} PDK_MAKECALL_BLK;


#endif
