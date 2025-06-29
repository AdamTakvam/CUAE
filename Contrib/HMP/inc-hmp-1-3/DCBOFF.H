/********************************************************************************
 * Copyright (c) 1990-2002 Intel Corporation
 * All Rights Reserved.  All names, products, and services mentioned herein 
 * are the trademarks or registered trademarks of their respective organizations 
 * and are the sole property of their respective owners
 ********************************************************************************/

/******************************************************************************** 
 * FILENAME:    DCBOFF.H
 * DESCRIPTION:  
 ********************************************************************************/

#define DCB_DSP1                    1
#define DCB_DSP2                    2

#define DCB_DSP1_ATI_OFFSET     0x1480
#define DCB_DSP2_ATI_OFFSET     0x1484
#define DCB_DSP1_RESTBL_OFFSET  0x1500
#define DCB_DSP2_RESTBL_OFFSET  0x15c0

#define DCB_RESCOUNT            32 

#define DCB_ATI_SIZE            0x4
#define DCB_RESTBL_SIZE         DCB_RESCOUNT*6
#define DCB_APPRESTBL_SIZE      DCB_RESCOUNT*12

#define NAMELENGTH               64

typedef struct _DCBEXTRA {
   CHAR DBName[NAMELENGTH];
   CHAR ResTbl[DCB_RESTBL_SIZE];
} DCBEXTRA, *LPDCBEXTRA;
