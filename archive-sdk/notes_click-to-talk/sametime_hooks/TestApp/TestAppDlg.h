// TestAppDlg.h : header file
//

#if !defined(AFX_TESTAPPDLG_H__6CB35514_3B0C_4754_8179_F2E0AA1B8F96__INCLUDED_)
#define AFX_TESTAPPDLG_H__6CB35514_3B0C_4754_8179_F2E0AA1B8F96__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

/////////////////////////////////////////////////////////////////////////////
// CTestAppDlg dialog

class CTestAppDlg : public CDialog
{
// Construction
public:
	CTestAppDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
	//{{AFX_DATA(CTestAppDlg)
	enum { IDD = IDD_TESTAPP_DIALOG };
		// NOTE: the ClassWizard will add data members here
	//}}AFX_DATA

	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CTestAppDlg)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	//{{AFX_MSG(CTestAppDlg)
	virtual BOOL OnInitDialog();
	afx_msg void OnSysCommand(UINT nID, LPARAM lParam);
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	afx_msg void OnButtonInstall();
	afx_msg void OnButtonUninstall();
	afx_msg void OnClose();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_TESTAPPDLG_H__6CB35514_3B0C_4754_8179_F2E0AA1B8F96__INCLUDED_)
