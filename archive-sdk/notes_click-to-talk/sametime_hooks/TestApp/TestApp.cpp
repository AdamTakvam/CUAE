// TestApp.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include "TestApp.h"
#include "TestAppDlg.h"
#include "Selector.h"
#include "SimDlg.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CTestAppApp

BEGIN_MESSAGE_MAP(CTestAppApp, CWinApp)
	//{{AFX_MSG_MAP(CTestAppApp)
		// NOTE - the ClassWizard will add and remove mapping macros here.
		//    DO NOT EDIT what you see in these blocks of generated code!
	//}}AFX_MSG
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CTestAppApp construction

CTestAppApp::CTestAppApp()
{
	// TODO: add construction code here,
	// Place all significant initialization in InitInstance
}

/////////////////////////////////////////////////////////////////////////////
// The one and only CTestAppApp object

CTestAppApp theApp;

/////////////////////////////////////////////////////////////////////////////
// CTestAppApp initialization

BOOL CTestAppApp::InitInstance()
{
	AfxEnableControlContainer();

	// Standard initialization
	// If you are not using these features and wish to reduce the size
	//  of your final executable, you should remove from the following
	//  the specific initialization routines you do not need.

#ifdef _AFXDLL
	Enable3dControls();			// Call this when using MFC in a shared DLL
#else
	Enable3dControlsStatic();	// Call this when linking to MFC statically
#endif

	int nResponse;
	CSelector sdlg;
	nResponse = sdlg.DoModal();
	if (IDCANCEL == nResponse)
		return FALSE;
	if (IDOK == nResponse)
	{
		if (sdlg.m_bAsInstaller)
		{
			CTestAppDlg dlg;
			m_pMainWnd = &dlg;
			nResponse = dlg.DoModal();
			if (nResponse == IDOK)
			{
			}
			else if (nResponse == IDCANCEL)
			{
			}
		}
		else
		{
			CSimDlg dlg;
			m_pMainWnd = &dlg;
			nResponse = dlg.DoModal();
			if (nResponse == IDOK)
			{
			}
			else if (nResponse == IDCANCEL)
			{
			}
		}
	}
	

	// Since the dialog has been closed, return FALSE so that we exit the
	//  application, rather than start the application's message pump.
	return FALSE;
}
