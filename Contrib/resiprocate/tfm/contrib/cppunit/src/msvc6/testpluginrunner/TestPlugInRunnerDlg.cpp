// TestPlugInRunnerDlg.cpp : implementation file
//

#include "stdafx.h"
#include "TestPlugInRunnerDlg.h"
#include "TestPlugIn.h"
#include "TestPlugInException.h"

#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// TestPlugInRunnerDlg dialog

TestPlugInRunnerDlg::TestPlugInRunnerDlg( TestPlugInRunnerModel *model,
                                          CWnd* pParent ) :
	  TestRunnerDlg( model, IDD_TEST_PLUG_IN_RUNNER, pParent )
{
	//{{AFX_DATA_INIT(TestPlugInRunnerDlg)
		// NOTE: the ClassWizard will add member initialization here
	//}}AFX_DATA_INIT
	// Note that LoadIcon does not require a subsequent DestroyIcon in Win32
	m_hIcon = AfxGetApp()->LoadIcon(IDR_TEST_PLUGIN_RUNNER);
}

void TestPlugInRunnerDlg::DoDataExchange(CDataExchange* pDX)
{
	TestRunnerDlg::DoDataExchange(pDX);
	//{{AFX_DATA_MAP(TestPlugInRunnerDlg)
		// NOTE: the ClassWizard will add DDX and DDV calls here
	//}}AFX_DATA_MAP
}

BEGIN_MESSAGE_MAP(TestPlugInRunnerDlg, TestRunnerDlg)
	//{{AFX_MSG_MAP(TestPlugInRunnerDlg)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_CHOOSE_DLL, OnChooseDll)
	ON_BN_CLICKED(IDC_RELOAD_DLL, OnReloadDll)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// TestPlugInRunnerDlg message handlers

BOOL TestPlugInRunnerDlg::OnInitDialog()
{
	TestRunnerDlg::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon
	
	// TODO: Add extra initialization here
	
	return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void TestPlugInRunnerDlg::OnPaint() 
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, (WPARAM) dc.GetSafeHdc(), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		TestRunnerDlg::OnPaint();
	}
}

// The system calls this to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR TestPlugInRunnerDlg::OnQueryDragIcon()
{
	return (HCURSOR) m_hIcon;
}


TestPlugInRunnerModel &
TestPlugInRunnerDlg::plugInModel()
{
  return *static_cast<TestPlugInRunnerModel*>( m_model );
}


void 
TestPlugInRunnerDlg::OnChooseDll() 
{
  CFileDialog dlg( TRUE, "*.dll", "", 0, 
                   "Test Plug-in (*.dll)|*.dll|All Files (*.*)|*.*||",
                   this );
  if ( dlg.DoModal() != IDOK )
    return;

  try
  {
    TestPlugIn *plugIn = new TestPlugIn( std::string( dlg.GetPathName() ) );
    plugInModel().setPlugIn( plugIn );
    updateHistoryCombo();
  }
  catch ( TestPlugInException &e )
  {
    AfxMessageBox( e.what() );
  }
}


void 
TestPlugInRunnerDlg::OnReloadDll() 
{
  plugInModel().reloadPlugIn();
}
