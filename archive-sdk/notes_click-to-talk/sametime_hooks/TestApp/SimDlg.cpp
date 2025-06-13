// SimDlg.cpp : implementation file
//

#include "stdafx.h"
#include "TestApp.h"
#include "SimDlg.h"


// CSimDlg dialog

IMPLEMENT_DYNAMIC(CSimDlg, CDialog)
CSimDlg::CSimDlg(CWnd* pParent /*=NULL*/)
	: CDialog(CSimDlg::IDD, pParent)
{
}

CSimDlg::~CSimDlg()
{
}

void CSimDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}


BEGIN_MESSAGE_MAP(CSimDlg, CDialog)
END_MESSAGE_MAP()


// CSimDlg message handlers
