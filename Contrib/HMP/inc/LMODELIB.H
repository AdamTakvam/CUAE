/*########@@@SOFT@@@WARE@@@COPY@@@RIGHT@@@###################################
# INTEL CONFIDENTIAL
#
# Copyright (C) 2000-2006 Intel Corporation. All Rights Reserved.
# The source code contained or described herein and all documents related
# to the source code ("Material") are owned by Intel Corporation or its
# suppliers or licensors. Title to the Material remains with Intel Corporation
# or its suppliers and licensors. The Material contains trade secrets and
# proprietary and confidential information of Intel or its suppliers and
# licensors. The Material is protected by worldwide copyright and trade secret
# laws and treaty provisions. No part of the Material may be used, copied,
# reproduced, modified, published, uploaded, posted, transmitted, distributed,
# or disclosed in any way without Intel's prior express written permission.
#
# No license under any patent, copyright, trade secret or other intellectual
# property right is granted to or conferred upon you by disclosure or delivery
# of the Materials, either expressly, by implication, inducement, estoppel or
# otherwise. Any license under such intellectual property rights must be
# express and approved by Intel in writing.
#
###################################@@@SOFT@@@WARE@@@COPY@@@RIGHT@@@########*/


/*****************************************************************************
 * Filename:    lmodelib.h 
 * DESCRIPTION: Learn Mode application header file
 *                 
 ****************************************************************************/


// *
// * Tone frequency structure: Specifies frequency information for a tone
// * and is a member of the TN_DESC structure.
// */
typedef struct {
   unsigned short int tn_freq1;    // 1st tone frequency 
   unsigned short int tn_fq1dev;   // 1st tone deviation 
   unsigned short int tn_freq2;    // 2nd tone frequency 
   unsigned short int tn_fq2dev;   // 2nd tone deviation 
} TN_FREQ;


// *
// * Tone duration structure: Specifies cadence information for a tone
// * and is a member of the TN_DESC structure.
// *
typedef struct {
   short int          tn_on;       // Cadence on-time (10msec)    
   short int          tn_ondev;    // on-time deviation (10msec)  
   short int          tn_off;      // Cadence off-time (10msec)   
   short int          tn_offdev;   // off-time deviation (10msec) 
} TN_DUR;


// *
// * Tone description structure: Specifies complete characteristics of a tone.
// *
typedef struct {
   TN_FREQ            tn_freq;     // input/output frequency structure 
   TN_DUR             tn_dur;      // input/output duration structure  
   unsigned short int tn_qualid;   // output qualification ID          
} TN_DESC;


// *
// * Tone amplitude structure: Specifies amplitude information used to
// * learn a tone.
// *
typedef struct {
   short int        tn_fq1_minamp; // 1st tone min. amplitude (dB) 
   short int        tn_fq1_maxamp; // 1st tone max. amplitude (dB) 
   short int        tn_fq2_minamp; // 2nd tone min. amplitude (dB) 
   short int        tn_fq2_maxamp; // 2nd tone max. amplitude (dB) 
} TN_AMP;


// *
// * Learn Mode Parameters structure: Specifies the parameters and options
// * for learning a tone.
// *
typedef struct {
   unsigned short int lm_cadflag;   // cadence/continuous tone flag    
   unsigned short int lm_cnt_min;   // min on-time for continuous tone 
   unsigned short int lm_frames;    // number of tone on/off samples   
   unsigned short int lm_qualid;    // qualification ID used           
   unsigned short int lm_method;    // method for freq/dur tolerances  
   float              lm_frq_tol;   // frequency parm for tolerance    
   float              lm_dur_tol;   // duration parm for tolerance     
} LM_PARM;

/*
 * Tone Information Structure List
 */
typedef struct {
   TN_INFO *tn_infop;
   unsigned short size;        /* size of the array being passed  */
} TN_INFOLIST;

/*
 * Events generated by Learn Mode.
 */
#define TDX_LEARNTONE   0x91        /* Learn Tone completed */
#define TDX_CLASSIFY    0x92        /* Tone classification complete */
#define TDX_LMERROR     0x93        /* Error performing Learn Mode */


/*
 * Learn Mode qualification templates (lm_parmp->lm_qualid)
 */
#define QT_LMDEF        0           /* default qualification template */
#define QT_LMLOW        7           /* low qualification template */
#define QT_LMMID        8           /* middle qualification template */
#define QT_LMHIGH       9           /* high qualification template */


/*
 * Learn mode methods (lm_parmp->lm_method)
 */
#define DEF_METHOD      2           /* default method used to compute the tolerance */
#define LM_ABSOLUTE     1           /* method #1 for tolerance computation */
#define LM_RATIO        2           /* method #2 for tolerance computation */
#define LM_STATISTICAL  3           /* method #3 for tolerance computation */


/*
 * Miscellaneous learn mode min/max values
 */
#define MAX_FRAME       127         /* max # of frames to do the training */
#define MIN_FRAME       5           /* min # of frames to do the training */
#define FRQ_MIN         200         /* min frequency valid for tone template */
#define FRQ_MAX         4000        /* max frequency valid for tone template */
#define DUR_MIN         0           /* min on/off time valid for tone template */
#define DUR_MAX         1000        /* max on/off time valid for tone template */
#define MIN_AMP         -60         /* min amplitude for GTD */
#define MAX_AMP         0           /* max amplitude for GTD */
#define MIN_CNT_TIME    1           /* min on-time for continuous tone */
#define MAX_CNT_TIME    1000        /* max on-time for continuous tone */
#define FRQTOL_MIN1     0           /* min frequency tolerance for method #1 */
#define FRQTOL_MAX1     300         /* max frequency tolerance for method #1 */
#define DURTOL_MIN1     0           /* min duration tolerance for method #1 */
#define DURTOL_MAX1     100         /* max duration tolerance for method #1 */
#define FRQTOL_MIN2     0           /* min frequency tolerance for method #2 */
#define FRQTOL_MAX2     1           /* max frequency tolerance for method #2 */
#define DURTOL_MIN2     0           /* min duration tolerance for method #2 */
#define DURTOL_MAX2     1           /* max duration tolerance for method #2 */
#define FRQTOL_MIN3     0           /* min frequency tolerance for method #3 */
#define FRQTOL_MAX3     0.2         /* max frequency tolerance for method #3 */
#define DURTOL_MIN3     0           /* min duration tolerance for method #3 */
#define DURTOL_MAX3     0.2         /* max duration tolerance for method #3 */


/*
 * DEFAULT values for Learn Mode parameters.
 */
#define FRAMES          10            /* no. of frames to do the training */
#define CON_MIN         400           /* min. on time for continuous tone */
#define FRQ_TOLE1       (float)30.0   /* tolerance used for method1 */
#define FRQ_TOLE2       (float)0.06   /* tolerance used for method2 */
#define FRQ_TOLE3       (float)2.17   /* confidence interval is 97% */
#define DUR_TOLE1       (float)3.0    /* tolerance used for method1 */
#define DUR_TOLE2       (float)0.08   /* tolerance used for method2 */
#define DUR_TOLE3       (float)2.17   /* confidence interval is 97% */

#ifndef LINUX
#ifdef LMODE_DEFINE
#define LMODEAPI __declspec(dllexport)
#else
#define LMODEAPI __declspec(dllimport)
#endif
#else
#define LMODEAPI
#endif

typedef enum _lrnret
{
	LRNFAIL = -1,
	LRNSUCCESS
}LRNRET;

#ifdef __cplusplus
extern "C" {               // C Plus Plus function bindings
#define extern
#endif

/*
 * Learn Mode Library functions.
 */
#if (defined(__STDC__) || defined(__cplusplus) || defined(WIN32))
LMODEAPI void lm_clramp(TN_AMP *);
LMODEAPI void lm_clrdesc(TN_DESC *);
LMODEAPI void lm_clrparm(LM_PARM *);
LMODEAPI LRNRET  lm_learntone(int, LM_PARM *, TN_AMP *, short *, TN_DESC *, TN_DESC *, TN_INFO *, int );
LMODEAPI LRNRET  lm_LearnTone(int , LM_PARM *, TN_AMP *, short *, TN_DESC *, TN_DESC *, TN_INFOLIST *, int);
#else
LMODEAPI void lm_clramp();
LMODEAPI void lm_clrdesc();
LMODEAPI void lm_clrparm();
LMODEAPI LRNRET  lm_learntone();
LMODEAPI LRNRET  lm_LearnTone();
#endif

#ifdef __cplusplus
}                          // C Plus Plus function bindings
#undef extern
#endif
