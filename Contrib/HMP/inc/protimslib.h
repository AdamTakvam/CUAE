/****************************************************************************
 *                     Copyright (C) 1998 Dialogic Corp.
 *                           All Rights Reserved
 ****************************************************************************
 *
 ****************************************************************************
 * 
 *                                 TITLE
 *
 * FILE: PROTOMSLIB.H
 *
 * REVISION: 1.0
 *
 * DATE: January 01, 2000
 *
 * PURPOSE: Define Information Elements, Driver Command,and Structures
 *          for the PROTIMS library.
 *
 * INTERFACE: None
 *
 * Note: None
 *
 * REVISION HISTORY:
 *
 *     Date         Description
 *
 ****************************************************************************/


#ifndef __PROTIMSLIB_H__
#define __PROTIMSLIB_H__


#include "srllib.h"
#include "ccerr.h"
#include "isdnlib.h"

typedef struct 
{
	int total;
	int success;
	int error;
}PRTMS_LBACK,*PRTMS_LBACK_PTR;

#endif   /*   __PROTIMSLIB_H__    */
