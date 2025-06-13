#pragma once


// CSimDlg dialog

class CSimDlg : public CDialog
{
	DECLARE_DYNAMIC(CSimDlg)

public:
	CSimDlg(CWnd* pParent = NULL);   // standard constructor
	virtual ~CSimDlg();

// Dialog Data
	enum { IDD = IDD_SIM_DIALOG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
};
