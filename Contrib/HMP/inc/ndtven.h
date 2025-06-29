/*****************************************************************************
 * Copyright © 1993-95, Intel Corporation. All rights reserved.  Intel is a 
 * trademark or registered trademark of Intel Corporation or its subsidiaries 
 * in the United States and other countries. Other names and brands may be 
 * claimed as the property of others. 
 *****************************************************************************/
 /*****************************************************************************
 *        FILE: ndtven.h
 *     VERSION: 
 * DESCRIPTION: Module header file for the BRI/DTI veneer library.
 *
 *****************************************************************************/

#ifndef __NDTVEN_H__
#define __NDTVEN_H__

#ifdef VME_SPAN
#else
#pragma pack(1)
#endif

/*
 * Structure to hold pointers to library functions for all supported devices.
 */
typedef struct {

#ifdef __cplusplus
extern "C" {   // C++ func bindings to enable C funcs to be called from C++
#define extern
#endif

#if (defined(__STDC__) || defined(__cplusplus) || defined(_BORLANDC))
             int    (*dt_getctinfo)(int, CT_DEVINFO *);
             int    (*dt_assignxmitslot)(int, SC_TSINFO *, USHORT);
             int    (*dt_getxmitslot)(int, SC_TSINFO *);
             int    (*dt_unassignxmitslot)(int, USHORT);
             int    (*dt_getparm)(int, ULONG, void *);
             int    (*dt_setparm)(int, ULONG, void *);
             int    (*dt_listen)(int, SC_TSINFO *);
             int    (*dt_unlisten)(int);
             int    (*dt_tstcom)(int, int);
             int    (*dt_tstdat)(int, int);
#else
             int    (*dt_getctinfo)();
             int    (*dt_assignxmitslot)();
             int    (*dt_getxmitslot)();
             int    (*dt_unassignxmitslot)();
             int    (*dt_getparm)();
             int    (*dt_setparm)();
             int    (*dt_listen)();
             int    (*dt_unlisten)();
             int    (*dt_tstcom)();
             int    (*dt_tstdat)();
#endif

#ifdef __cplusplus
}
#undef extern
#endif

} LIBBRI_FUNC, *LIBBRI_FUNC_PTR;


/*
 * Prototypes for BRI card library functions.
 */
#ifdef __cplusplus
extern "C" {   // C++ func bindings to enable C funcs to be called from C++
#define extern
#endif

#if (defined(__STDC__) || defined(__cplusplus))
DllLinkage int   bri_getctinfo(int, CT_DEVINFO *),
                 bri_assignxmitslot(int, SC_TSINFO *, USHORT),
                 bri_getxmitslot(int, SC_TSINFO *),
                 bri_unassignxmitslot(int, USHORT),
                 bri_getparm(int, ULONG, void *),
                 bri_setparm(int, ULONG, void *),
                 bri_listen(int, SC_TSINFO *),
                 bri_unlisten(int),
                 bri_tstcom(int, int),
                 bri_tstdat(int, int);
#else
DllLinkage       bri_getctinfo(),
                 bri_assignxmitslot(),
                 bri_getxmitslot(),
                 bri_unassignxmitslot(),
                 bri_getparm(),
                 bri_setparm(),
                 bri_listen(),
                 bri_unlisten(),
                 bri_tstcom(),
                 bri_tstdat();
#endif

/*
 * Prototypes for SPAN-DTI card library functions.
 */
#if (defined(__STDC__) || defined(__cplusplus))
int   dti_getctinfo(int, CT_DEVINFO *),
      dti_assignxmitslot(int, SC_TSINFO *, USHORT),
      dti_getxmitslot(int, SC_TSINFO *),
      dti_unassignxmitslot(int, USHORT),
      dti_getparm(int, ULONG, void *),
      dti_setparm(int, ULONG, void *),
      dti_listen(int, SC_TSINFO *),
      dti_unlisten(int);
      dti_tstcom(int, int),
      dti_tstdat(int, int);
#else
int   dti_getctinfo(),
      dti_assignxmitslot(),
      dti_getxmitslot(),
      dti_unassignxmitslot(),
      dti_getparm(),
      dti_setparm(),
      dti_listen(),
      dti_unlisten(),
      dti_tstcom(),
      dti_tstdat();
#endif

#ifdef __cplusplus
}
#undef extern
#endif

#ifdef VME_SPAN
#else
#pragma pack()
#endif

#endif
