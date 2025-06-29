/**************************************************************
 *   Copyright © 2000-2002,2005 Intel Corporation. All rights reserved. 
 * 
 *   Intel is a trademark or registered trademark of Intel 
 *   Corporation or its subsidiaries in the United States and 
 *   other countries. Other names and brands may be claimed as 
 *   the property of others.
 **********************************************************************/

#ifdef __cplusplus
extern "C" {
#endif

/*#ifdef DLG_WIN32_OS
    #ifdef OEMLICENSE_EXPORTS
        #define OEMLICENSE_API __declspec(dllexport)
    #else
        #undef OEMLICENSE_API
        #define OEMLICENSE_API __declspec(dllimport)
    #endif
#else
    #define OEMLICENSE_API
#endif //of os

enum OEMLicenseRetCode
{
	OEMLicenseRetCode_Success = 0

};*/

OEMLICENSE_API OEMLicenseRetCode OEMLicense_RegisterOEMLib( const char *szLibraryName, const char *szFunctionName);

#ifdef __cplusplus
}
#endif
