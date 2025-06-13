#if !defined(AFX_HARDWAREPAGE_H__C7A07F0F_2EF0_11D4_9FA9_0030DB0011C6__INCLUDED_)
#define AFX_HARDWAREPAGE_H__C7A07F0F_2EF0_11D4_9FA9_0030DB0011C6__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
// HardwarePage.h : header file
//

#include "NewWizPage.h"
#include "afxwin.h"
#include "HListBox.h"

/////////////////////////////////////////////////////////////////////////////
// CVerifyPage dialog

class CVerifyPage : public CNewWizPage
{
// Construction
public:
	CVerifyPage(CWnd* pParent = NULL);   // standard constructor

// Dialog Data
	//{{AFX_DATA(CVerifyPage)
	enum { IDD = IDD_VERIFY };
	CStatic	m_CaptionCtrl;
	//}}AFX_DATA

	CFont m_Font;

// Overrides
	// ClassWizard generated virtual function overrides
	//{{AFX_VIRTUAL(CVerifyPage)
	protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	//}}AFX_VIRTUAL

// Implementation
protected:
	BOOL OnWizardFinish();
	void OnSetActive();
	BOOL OnKillActive();
    BOOL OnQueryCancel();
    void DoTest(BOOL bNew);
    void UpdateConfig();

	// Generated message map functions
	//{{AFX_MSG(CVerifyPage)
	virtual BOOL OnInitDialog();
	//}}AFX_MSG
	DECLARE_MESSAGE_MAP()

public:
    afx_msg void OnSkipTestClicked();
    afx_msg void OnBnClickedButtonTest();
    void SkinnyFound();
    void SkinnyNotFound(BOOL bFinal);
    BOOL ProcessSkinnyPacket(const char *pkt_data, int packet_len, int offset, int ih_len, int th_len);
    BOOL ProcessSkinnyMessage(const char *pptr);
    void AddMessage(CString sMag);
    void RemoveLogs();

    BOOL m_bVerified;
    int m_iTesingNic;
    BOOL m_bVerifying;

private:
    CHListBox m_ListLogs;
};

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.

#endif // !defined(AFX_HARDWAREPAGE_H__C7A07F0F_2EF0_11D4_9FA9_0030DB0011C6__INCLUDED_)
