// stdafx.h : include file for standard system include files,
//  or project specific include files that are used frequently, but
//      are changed infrequently
//

#if !defined(AFX_STDAFX_H__E94F78C3_5DE8_4525_BDC4_FD3D514E0D67__INCLUDED_)
#define AFX_STDAFX_H__E94F78C3_5DE8_4525_BDC4_FD3D514E0D67__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


// Insert your headers here
#define WIN32_LEAN_AND_MEAN		// Exclude rarely-used stuff from Windows headers

#include <windows.h>


#ifdef _DEBUG
#define DEBUG(str)	OutputDebugString(str)
#else
#define DEBUG(str)
#endif

// TODO: reference additional headers your program requires here

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_STDAFX_H__E94F78C3_5DE8_4525_BDC4_FD3D514E0D67__INCLUDED_)
