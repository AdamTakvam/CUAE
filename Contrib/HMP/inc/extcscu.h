
/******************************************************************************
 *		Copyright (c) 1998-1999 Dialogic Corporation.
 *		All Rights Reserved. 
 * 
 *		THIS IS UNPUBLISHED PROPRIETARY SOURCE CODE OF Dialogic Corporation 
 *		The copyright notice above does not evidence any actual or 
 *		intended publication of such source code. 
 ******************************************************************************
 * 
 ******************************************************************************
 * FILE:         %name: extcscu.h % 
 * VERSION:      %version: 3 %  
 * FULLNAME:     %full_name: 1/incl/extcscu.h/3 % 
 * 
 * AUTHOR:       %created_by: klotzw % 
 *               %derived_by: klotzw %   
 * 
 * PUPPOSE: 
 * 
 * History: 
 * 
 *     Date       Who            Description 
 *    _______________________________________ 
 *    03/26/1999  WRK            Modified file to support both Windows NT and DOS
 ******************************************************************************/


#ifdef WIN32

#ifndef EKV_NOTIMP
#define EKV_NOTIMP         0x0500  /* Function not implemented */
#endif

#ifdef __USING_DEF_FILE__

#ifndef DllExport
#define DllExport	__declspec( dllexport )
#endif	/*	Dllexport	*/

#ifndef DllImport
#define DllImport __declspec( dllimport )
#endif	/*	Dllimport	*/

#ifdef _KVCSCU_DLL
#define	DllLinkage	DllExport
#else
#define DllLinkage	DllImport
#endif

#endif	/* __USING_DEF_FILE__	*/

#ifdef __CROSS_COMPAT_LIB__
#undef DllLinkage
#define DllLinkage    
#endif

#endif /* end ifdef WIN32 */

#if (defined (__BORLANDC__) || defined (__cplusplus) || defined( __STDC__ ))

    extern "C" {int __cdecl dx_verifyCS(int channel);}  // Returns whether board passes security check.

#else

    int __cdecl dx_verifyCS(int channel);

#endif /* end if __BORLANDC__ || __cplusplus || __STDC__ */
