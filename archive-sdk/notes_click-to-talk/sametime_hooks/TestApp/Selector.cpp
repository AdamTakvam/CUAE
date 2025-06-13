// Selector.cpp : implementation file
//

#include "stdafx.h"
#include "TestApp.h"
#include "Selector.h"
#include ".\selector.h"


// CSelector dialog

IMPLEMENT_DYNAMIC(CSelector, CDialog)
CSelector::CSelector(CWnd* pParent /*=NULL*/)
	: CDialog(CSelector::IDD, pParent)
{
	m_bAsInstaller = FALSE;
}

CSelector::~CSelector()
{
}

void CSelector::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CSelector, CDialog)
	ON_BN_CLICKED(IDC_RADIO_INSTALLER, OnBnClickedRadioInstaller)
	ON_BN_CLICKED(IDC_RADIO_SIMULATOR, OnBnClickedRadioSimulator)
END_MESSAGE_MAP()


// CSelector message handlers

void CSelector::OnBnClickedRadioInstaller()
{
	m_bAsInstaller = TRUE;
	OnOK();
}

void CSelector::OnBnClickedRadioSimulator()
{
	m_bAsInstaller = FALSE;
	OnOK();
}
