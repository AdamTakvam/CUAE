/**************************************************************
    Copyright (C) 2004.  Intel Corporation.

    All Rights Reserved.  All names, products,
    and services mentioned herein are the trademarks
    or registered trademarks of their respective organizations
    and are the sole property of their respective owners.
 **************************************************************/
#ifndef NCMAPI_H
#define NCMAPI_H

/***************************************************************

  %name:           NCMApi.H %
  %created_by:     gadrer %
  %date_created:   Mon Jan 31 14:22:37 2005 %
  Description:  Contains API calls needed for NCM clients.
  %version:        HMP35 %
  %derived_by:     gadrer %
  %date_modified:  Mon Jan 31 14:22:38 2005 %


***************************************************************/

//
#include "NCMTypes.h"
#include <winsvc.h>

#ifdef __cplusplus
extern "C" {
#endif

#ifndef lint
static char    *_hsw_1_NCMApi_H = "@(#) %filespec: NCMApi.H-HMP35 %  (%full_filespec: NCMApi.H-HMP35:incl:hsw#5 %)";
#endif


#ifdef  _NCMAPI_
#   if (!defined(DLG_STATICALLY_LINKED) && defined(WIN32))
#      define NCM_API __declspec(dllexport)
#   else
#      define NCM_API
#   endif
#else
#   if (!defined(DLG_STATICALLY_LINKED) && defined(WIN32))
#      define NCM_API __declspec(dllimport) extern
#   else
#      define NCM_API extern
#   endif
#endif

// --------------------------------------------------------------------------------
//  Read
// --------------------------------------------------------------------------------

NCM_API NCMRetCode      NCM_GetAllFamilies( NCMFamily ** ppncmFamily );
NCM_API NCMRetCode      NCM_GetInstalledFamilies( NCMFamily ** ppncmFamily );

NCM_API NCMRetCode      NCM_GetAllDevices( NCMFamily *, NCMDevice ** );
NCM_API NCMRetCode      NCM_GetInstalledDevices( NCMFamily *,
                                                 NCMDevice ** ppncmDevice );

NCM_API NCMRetCode      NCM_GetProperties( NCMFamily *, NCMDevice *,
                                           NCMProperty ** ppncmProp );
NCM_API NCMRetCode      NCM_GetVariables( NCMFamily *, NCMDevice *,
                                          NCMProperty *, NCMVariable ** );
NCM_API NCMRetCode      NCM_GetValue( NCMFamily *, NCMDevice *,
                                      NCMProperty *, NCMVariable *,
                                      NCMValue ** );
NCM_API NCMRetCode      NCM_GetValueRange( NCMFamily *, NCMDevice *,
                                           NCMProperty *, NCMVariable *,
                                           NCMValue ** );

NCM_API NCMRetCode      NCM_IsEditable( NCMFamily *, NCMDevice *,
                                        NCMProperty *, NCMVariable *,
                                        BOOL * );

NCM_API NCMRetCode      NCM_GetErrorMsg( NCMRetCode ncmRcIn, NCMErrorMsg** );

NCM_API NCMRetCode      NCM_IsBoardEnabled( NCMFamily *,
                                               NCMDevice *,
                                               BOOL * );


// --------------------------------------------------------------------------------
//  Create, Update, Delete
// --------------------------------------------------------------------------------

NCM_API NCMRetCode      NCM_SetValue( NCMFamily *, NCMDevice *, NCMProperty *, NCMVariable *, NCMValue * );

NCM_API NCMRetCode      NCM_AddDevice( NCMFamily *family, NCMDevice *modelName, NCMDevice *uniqueName );
NCM_API NCMRetCode      NCM_DeleteEntry( NCMFamily *, NCMDevice * );

NCM_API NCMRetCode      NCM_EnableBoard( NCMFamily *,
                                            NCMDevice *,
                                            BOOL );

NCM_API NCMRetCode      NCM_DetectBoards( GL_PROG_FUNC*, int * );

//Added the function to synchronize software versions. Greg Perlovsky 08-08-00
NCM_API NCMRetCode      NCM_GetVersionInfo(NCMSysVersion *psysver);
////////////////////////// Third Party Configuration, RChen  11/17/99////////////////////////
// --------------------------------------------------------------------------------
//  Intel® Dialogic® product service
// --------------------------------------------------------------------------------

NCM_API NCMRetCode      NCM_GetDlgSrvStartupMode( NCMDlgSrvStartupMode *);
NCM_API NCMRetCode      NCM_SetDlgSrvStartupMode( NCMDlgSrvStartupMode );

NCM_API NCMRetCode      NCM_GetDlgSrvState( NCMDlgSrvState * );

// Support semi-automatic mode since SR5.11 SP1
NCM_API	NCMRetCode  	NCM_StopSystem();
NCM_API	NCMRetCode  	NCM_StartSystem();
NCM_API	NCMRetCode		NCM_GetSystemState (NCMSystemState *);

//M.Chia 2-3-99 added the function below from J.Xu
NCM_API NCMRetCode      NCM_GetDlgSrvStateEx( SERVICE_STATUS * );

NCM_API NCMRetCode      NCM_StartDlgSrv();
NCM_API NCMRetCode      NCM_StopDlgSrv();


// --------------------------------------------------------------------------------
//  Administration
// --------------------------------------------------------------------------------
NCM_API NCMRetCode  NCM_Dealloc( NCMString * pncmStr );

NCM_API NCMRetCode  NCM_GetValueEx(      IN     NCMFamily   *,
                                         IN     NCMDevice   *,
                                         IN     NCMVariable *,
                                         OUT    NCMValueEx  ** );

NCM_API NCMRetCode  NCM_GetValueRangeEx( IN     NCMFamily   *,
                                         IN     NCMDevice   *,
                                         IN     NCMVariable *,
                                         OUT    NCMValueEx  ** );
NCM_API NCMRetCode  NCM_SetValueEx(      IN     NCMFamily   *,
                                         IN     NCMDevice   *,
                                         IN     NCMVariable *,
                                         IN     NCMValueEx  * );

NCM_API NCMRetCode  NCM_DeallocValue(    IN     NCMValueEx  *  );
NCM_API NCMRetCode  NCM_DetectBoardsEx(  IN     NCM_DETECTION_INFO   *,
                                         INOUT  NCM_DETECTION_RESULT * );

// --------------------------------------------------------------------------------
//  Attributes
// --------------------------------------------------------------------------------
NCM_API NCMRetCode  NCM_GetPropertyAttributes( IN     NCMFamily   *,
                                               IN     NCMDevice   *,
                                               IN     NCMProperty *,
                                               OUT    NCMPropertyAttributes * );

NCM_API NCMRetCode  NCM_GetVariableAttributes( IN     NCMFamily   *,
                                               IN     NCMDevice   *,
                                               IN     NCMVariable *,
                                               OUT    NCMVariableAttributes * );

//Jack Xu 7/23/98 add 2 new API functions calling start and stop boards.

NCM_API NCMRetCode NCM_StartBoard(IN NCMFamily*, IN NCMDevice*);

NCM_API NCMRetCode NCM_StopBoard(IN NCMFamily*, IN NCMDevice*);

// ----------------------------------------------------------------------------
//  Clock Master Fallback List - Pulsepoint     4/23/99 M.Chia
// ----------------------------------------------------------------------------

NCM_API NCMRetCode  NCM_GetClockMasterFallbackList( IN      NCMDevice   *,
                                                    OUT     int         * ,
                                                    OUT     NCMDevice   ** );

NCM_API NCMRetCode  NCM_SetClockMasterFallbackList( IN      NCMDevice   *,
                                                    IN      NCMDevice   * );


NCM_API NCMRetCode  NCM_SetTDMBusValue(             IN      NCMDevice   *,
                                                    IN      NCMVariable *,
                                                    IN      NCMValue    *);

NCM_API NCMRetCode  NCM_GetTDMBusValue(             IN      NCMDevice   *,
                                                    IN      NCMVariable *,
                                                    IN      NCMValue    ** );

////// OpenSwitch ////////////////////// 5/24/99 ///// ///////// ///////
////////////////////////////////////////////////////////////////////////
NCM_API NCMRetCode  NCM_GetDialogicDir( IN     char *,
                                        INOUT  int  *,
                                        OUT    char * );

// Device Mapper 12/8/00
NCM_API NCMRetCode NCM_GetAUID (IN NCMFamily*,
                                IN  NCMDevice*,
                                OUT int *,
                                OUT long *);

NCM_API NCMRetCode NCM_GetFamilyDeviceByAUID (IN long,
                                              OUT NCMFamily**,
                                              OUT NCMDevice** );

NCM_API NCMRetCode NCM_ReconfigBoard( IN NCMFamily *pncmFamily,
                                      IN NCMDevice *pncmDevice,
                                      NCM_DETECTION_INFO * pdetectInfo,
                                      OUT NCMDevice ** ppncmDevice);


// --------------------------------------------------------------------------------
//  Country Specific Parameters (CSP)
// --------------------------------------------------------------------------------

NCM_API NCMRetCode  NCM_GetCspCountries( NCMValue ** ncmCountries );
NCM_API NCMRetCode  NCM_GetCspCountryCode( char *countryName,
                                               NCMValue ** ppncmCountryCode
                                         );
NCM_API NCMRetCode  NCM_GetCspCountryName( char *countryCode,
                                               NCMValue ** ppncmCountryName
                                         );
NCM_API NCMRetCode  NCM_GetCspFeaturesVariables( char *country,
                                                 NCMVariable ** ppncmCspFeatures
                                               );
NCM_API NCMRetCode  NCM_GetCspFeaturesValue( char *country, char *features,
                                             NCMVariable *,
                                             NCMValue ** ppncmFeatValues
                                           );
NCM_API NCMRetCode  NCM_GetCspFeaturesValueRange( char *country, NCMVariable *,
                                                  NCMValue ** ppncmCspValRange
                                                );

// --------------------------------------------------------------------------------
//  High Availability
// --------------------------------------------------------------------------------
NCM_API NCMRetCode NCM_IsHotSwapSystem (OUT BOOL *);
NCM_API NCMRetCode NCM_GetHotSwapBoardCount (OUT unsigned long *);

//--following new function for Trunk Configuration. Task#18618
NCM_API NCMRetCode NCM_ApplyTrunkConfiguration(NCMFamily Family,
                                       NCMDevice Device,
                                       NCMTrunkConfig*  pTrunkConfig,
                                       NCMFeatureType* pMediaLoad, BYTE* pErrorMsg);



// --------------------------------------------------------------------------------
//  Advance IP Management
// --------------------------------------------------------------------------------
NCM_API NCMRetCode NCM_GetDefaultIPAddress(IN	NCMFamily *pncmFamily,
					  IN	NCMDevice *pncmModel,
					  IN	NCMDevice *pncmUniqueDevice,
					  INOUT	char* pIPAddress);

NCM_API	NCMRetCode NCM_GetMacIPAddressList(IN	NCMFamily *pncmFamily,
					  IN	NCMDevice *pncmDevice,
					  IN	DWORD *pdwSize,
					  IN	DWORD *pdwCount,
					  INOUT	BYTE  *pBuf);


////// 3rd PARTY API////////////// 12/06/2002 ///// Santosh Deshpande ///
////////////////////////////////////////////////////////////////////////
NCM_API NCMRetCode	NCM_GetThirdPartyTechName(		 IN		NCMFamily **ppncmFamily);


NCM_API NCMRetCode NCM_GetBridgeDeviceHMPClockMasterFallbackList( OUT NCMFamily **ppFamily,
                                                                  OUT NCMDevice **ppDevice );

#ifdef __cplusplus
}
#endif

#endif
