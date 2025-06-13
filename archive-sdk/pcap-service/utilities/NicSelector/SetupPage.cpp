// SetupPage.cpp : implementation file
//

#include "stdafx.h"
#include "NicSelector.h"
#include "SetupPage.h"
#include "NewWizDialog.h"
#include "MasterDlg.h"

#include "pcap.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#undef THIS_FILE
static char THIS_FILE[] = __FILE__;
#endif

/////////////////////////////////////////////////////////////////////////////
// CSetupPage dialog


CSetupPage::CSetupPage(CWnd* pParent /*=NULL*/)
	: CNewWizPage(CSetupPage::IDD, pParent)
{
	//{{AFX_DATA_INIT(CSetupPage)
	//}}AFX_DATA_INIT
}


void CSetupPage::DoDataExchange(CDataExchange* pDX)
{
    CDialog::DoDataExchange(pDX);
    //{{AFX_DATA_MAP(CSetupPage)
    DDX_Control(pDX, ST_CAPTION, m_CaptionCtrl);
    //}}AFX_DATA_MAP
    DDX_Control(pDX, IDC_LIST_NICS, m_ListNics);
}


BEGIN_MESSAGE_MAP(CSetupPage, CNewWizPage)
	//{{AFX_MSG_MAP(CSetupPage)
	//}}AFX_MSG_MAP
END_MESSAGE_MAP()

/////////////////////////////////////////////////////////////////////////////
// CSetupPage message handlers

BOOL CSetupPage::OnInitDialog() 
{
	CNewWizPage::OnInitDialog();
	
    // Make title description font different from others
	m_Font.CreateFont(-14, 0, 0, 0, 
		FW_BOLD, FALSE, FALSE, 0, DEFAULT_CHARSET, OUT_DEFAULT_PRECIS, 
		CLIP_DEFAULT_PRECIS, DEFAULT_QUALITY, DEFAULT_PITCH, _T("MS Sans Serif"));
	m_CaptionCtrl.SetFont(&m_Font, TRUE);

    // List all devices
    int index = 0;
    pcap_if_t *alldevs;
    char errbuf[PCAP_ERRBUF_SIZE];
    if (pcap_findalldevs(&alldevs, errbuf) != -1)
    {
        pcap_if_t *d;
        for(d=alldevs; d; d=d->next)
        {
            if (d->description)
                m_ListNics.InsertString(index, d->description);
            else
                m_ListNics.InsertString(index, d->name);

            index++;
        }
        // Free all devices
	    pcap_freealldevs(alldevs);
    }

    if (m_ListNics.GetCount() == 0)
    {
        GetDlgItem(RB_KNOWN)->EnableWindow(0);
        GetDlgItem(RB_UNKNOWN)->EnableWindow(0);
        m_pParent->EnableNext(false);
    }
    else
    {
        m_ListNics.SetCurSel(index-1);
	    CheckRadioButton(RB_KNOWN, RB_UNKNOWN, RB_UNKNOWN);
    }
    	
	return TRUE;  // return TRUE unless you set the focus to a control
	              // EXCEPTION: OCX Property Pages should return FALSE
}


LRESULT CSetupPage::OnWizardNext()
{
    int index = m_ListNics.GetCurSel();
    if (index == LB_ERR)
        return -1;

    if (m_pParent)
    {
        ((CMasterDlg*)m_pParent)->m_iTotalNics = m_ListNics.GetCount();
        ((CMasterDlg*)m_pParent)->m_bAutoMode = IsDlgButtonChecked(RB_UNKNOWN) ? TRUE : FALSE;
        if (((CMasterDlg*)m_pParent)->m_bAutoMode)
            ((CMasterDlg*)m_pParent)->m_iNicIndex = ((CMasterDlg*)m_pParent)->m_iTotalNics;
        else
            ((CMasterDlg*)m_pParent)->m_iNicIndex = index+1;
    }

	return 0;
}

void CSetupPage::OnSetActive()
{
    if (m_ListNics.GetCount() == 0 || m_ListNics.GetCurSel() == LB_ERR)
    {
        if (m_pParent)
            m_pParent->GetDlgItem(ID_WIZNEXT)->EnableWindow(0);
    }
}

BOOL CSetupPage::OnKillActive()
{
	return TRUE;
}

