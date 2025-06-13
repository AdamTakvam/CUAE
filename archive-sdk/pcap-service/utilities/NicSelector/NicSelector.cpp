// NicSelector.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include "NicSelector.h"
#include "MasterDlg.h"
#include "SetupPage.h"
#include "VerifyPage.h"

#include "pcap.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CNicSelectorApp

BEGIN_MESSAGE_MAP(CNicSelectorApp, CWinApp)
	ON_COMMAND(ID_HELP, CWinApp::OnHelp)
END_MESSAGE_MAP()


// CNicSelectorApp construction

CNicSelectorApp::CNicSelectorApp()
{
	// TODO: add construction code here,
	// Place all significant initialization in InitInstance
}


// The one and only CNicSelectorApp object

CNicSelectorApp theApp;


// CNicSelectorApp initialization

BOOL CNicSelectorApp::InitInstance()
{
	// InitCommonControls() is required on Windows XP if an application
	// manifest specifies use of ComCtl32.dll version 6 or later to enable
	// visual styles.  Otherwise, any window creation will fail.
	InitCommonControls();

	CWinApp::InitInstance();

	if (!AfxSocketInit())
	{
		AfxMessageBox(IDP_SOCKETS_INIT_FAILED);
		return FALSE;
	}

	AfxEnableControlContainer();

	// Standard initialization
	// If you are not using these features and wish to reduce the size
	// of your final executable, you should remove from the following
	// the specific initialization routines you do not need
	// Change the registry key under which our settings are stored
	// TODO: You should modify this string to be something appropriate
	// such as the name of your company or organization
	SetRegistryKey(_T("Local AppWizard-Generated Applications"));

    // The reason why we need to do this every time is because WinPCap fails to 
    // detect any interfaces in worker thread on some machines.
    int index = 0;
    pcap_if_t *alldevs;
    char errbuf[PCAP_ERRBUF_SIZE];
    if (pcap_findalldevs(&alldevs, errbuf) != -1)
    {
        pcap_if_t *d;
        for(d=alldevs; d; d=d->next)
            index++;
        // Free all devices
	    pcap_freealldevs(alldevs);
    }

	CMasterDlg Dlg;	
    m_pMainWnd = &Dlg;
	CSetupPage SetupPage;
	CVerifyPage VerifyPage;
	
	Dlg.AddPage(&SetupPage, CSetupPage::IDD);
	Dlg.AddPage(&VerifyPage, CVerifyPage::IDD);

	if (Dlg.DoModal() == ID_WIZFINISH)
	{
		AfxMessageBox("Finished Wizard");
	}
	else
	{
		AfxMessageBox("Cancelled Wizard");
	}

    return FALSE;
}
