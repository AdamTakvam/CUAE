#pragma once


// CSelector dialog

class CSelector : public CDialog
{
	DECLARE_DYNAMIC(CSelector)

public:
	CSelector(CWnd* pParent = NULL);   // standard constructor
	virtual ~CSelector();

// Dialog Data
	enum { IDD = IDD_SELECT_DIALOG };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()

public:
	BOOL m_bAsInstaller;
	afx_msg void OnBnClickedRadioInstaller();
	afx_msg void OnBnClickedRadioSimulator();
};
