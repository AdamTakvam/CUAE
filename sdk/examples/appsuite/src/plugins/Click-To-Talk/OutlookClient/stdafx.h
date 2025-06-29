// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently,
// but are changed infrequently

#pragma once

#ifndef STRICT
#define STRICT
#endif

// Modify the following defines if you have to target a platform prior to the ones specified below.
// Refer to MSDN for the latest info on corresponding values for different platforms.
#ifndef WINVER				// Allow use of features specific to Windows 95 and Windows NT 4 or later.
#define WINVER 0x0400		// Change this to the appropriate value to target Windows 98 and Windows 2000 or later.
#endif

#ifndef _WIN32_WINNT		// Allow use of features specific to Windows NT 4 or later.
#define _WIN32_WINNT 0x0400	// Change this to the appropriate value to target Windows 2000 or later.
#endif						

#ifndef _WIN32_WINDOWS		// Allow use of features specific to Windows 98 or later.
#define _WIN32_WINDOWS 0x0410 // Change this to the appropriate value to target Windows Me or later.
#endif

#ifndef _WIN32_IE			// Allow use of features specific to IE 4.0 or later.
#define _WIN32_IE 0x0400	// Change this to the appropriate value to target IE 5.0 or later.
#endif

#define _ATL_APARTMENT_THREADED
#define _ATL_NO_AUTOMATIC_NAMESPACE

#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS	// some CString constructors will be explicit

// turns off ATL's hiding of some common and often safely ignored warning messages
#define _ATL_ALL_WARNINGS

#include "resource.h"
#include <afxwin.h>
#include <afxcmn.h>
#include <afxext.h>
#include <atlbase.h>
#include <atlcom.h>
#include <atlctl.h>
#include <atlhttp.h>
#include <atlhost.h>

#pragma warning( disable : 4278 )
#pragma warning( disable : 4146 )

// Import the IDTExtensibility2 interface based on its LIBID.
#import "libid:AC0714F2-3D04-11D1-AE7D-00A0C90F26F4" version("1.0") lcid("0")  raw_interfaces_only named_guids

// Import the Microsoft Office type library based on its LIBID.
//#import "libid:2DF8D04C-5BFA-101B-BDE5-00AA0044DE52" version("2.2") lcid("0") raw_interfaces_only named_guids

// Import the Microsoft Outlook type library based on its LIBID. Version 9.0.
//#import "libid:00062FFF-0000-0000-C000-000000000046" version("9.0") raw_interfaces_only named_guids

#ifdef SUPPORT_OL9
#import "x:\contrib\microsoft\Office\9\mso9.dll" rename_namespace("Office"), named_guids, raw_interfaces_only
#import "x:\contrib\microsoft\Office\9\MSOUTL9.olb" rename_namespace("Outlook"), named_guids, raw_interfaces_only 
#else
#import "x:\contrib\microsoft\Office\10\mso.dll" rename_namespace("Office"), named_guids, raw_interfaces_only
#import "x:\contrib\microsoft\Office\10\MSOUTL.olb" rename_namespace("Outlook"), named_guids, raw_interfaces_only 
#endif

#pragma warning( default : 4146 )
#pragma warning( default : 4278 )

class DECLSPEC_UUID("38774F1C-924E-46BB-880D-49E232904D9B") OutlookClientLib;

using namespace ATL;
using namespace Office;

class CAddInModule : public CAtlDllModuleT< CAddInModule >
{
public:
	CAddInModule()
	{
		m_hInstance = NULL;
	}

	DECLARE_LIBID(__uuidof(OutlookClientLib))

	inline HINSTANCE GetResourceInstance()
	{
		return m_hInstance;
	}

    inline HINSTANCE GetModuleInstance()
    {
        return GetResourceInstance();
    }

	inline void SetResourceInstance(HINSTANCE hInstance)
	{
		m_hInstance = hInstance;
	}

private:
	HINSTANCE m_hInstance;
};

#define _Module _AtlModule
extern CAddInModule _AtlModule;