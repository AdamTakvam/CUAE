#include "StdAfx.h"
#include "DialSingleDialog.h"
#include "ControlMessage.h"
#include "RegistryHelpers.h"
#include "HttpHelpers.h"
#include "MetreosSettingsBase.h"
#include ".\dialsingledialog.h"

CDialSingleDialog::CDialSingleDialog()
{}

CDialSingleDialog::~CDialSingleDialog()
{
    m_contactName.Detach();
    m_dialNumCombo.Detach();
    m_statusBar.Detach();
    m_stopRecordButton.Detach();
    m_dialButton.Detach();
}

LRESULT CDialSingleDialog::OnInitDialog(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	CAxDialogImpl<CDialSingleDialog>::OnInitDialog(uMsg, wParam, lParam, bHandled);

    m_contactName.Attach(GetDlgItem(IDC_EDIT_CONTACTNAME));
    m_dialNumCombo.Attach(GetDlgItem(IDC_COMBO_NUMBERS));
    m_statusBar.Attach(GetDlgItem(IDC_STATUSBAR_DSD));
    m_stopRecordButton.Attach(GetDlgItem(IDC_BTN_STOP_RECORD));
    m_dialButton.Attach(GetDlgItem(IDOK));

    m_stopRecordButton.EnableWindow(0);

    InitDialogData();

	return 1;  // Let the system set the focus
}

LRESULT CDialSingleDialog::OnDestroy(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
    DialogDestroy();
    m_dialNumCombo.Clear();

	return 1;  // Let the system set the focus
}

LRESULT CDialSingleDialog::OnClickedStartCall(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled)
{
    USES_CONVERSION;

    if (m_dialNumCombo.GetWindowTextLength() == 0)
		return 0;  

    if(HandleStartCall() == false)
    {
        EndDialog(wID);
    }

	return 0;
}

LRESULT CDialSingleDialog::OnClickedCancel(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled)
{
	EndDialog(wID);
	return 0;
}

LRESULT CDialSingleDialog::OnBnClickedBtnRecord(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled)
{
    USES_CONVERSION;

    HandleStopRecord();

    return 0;
}

void CDialSingleDialog::SetDialInformation(const DialParticipant_list& numbers)
{
    DialParticipant* srcParticipant = *(numbers.begin());

    m_pParticipant.Name = srcParticipant->Name;
    m_pParticipant.BusNum = srcParticipant->BusNum;
    m_pParticipant.Bus2Num = srcParticipant->Bus2Num;
    m_pParticipant.HomeNum = srcParticipant->HomeNum;
    m_pParticipant.Home2Num = srcParticipant->Home2Num;
    m_pParticipant.MobileNum = srcParticipant->MobileNum;
}

void CDialSingleDialog::InitDialogData()
{
    USES_CONVERSION;

    SetDlgItemText(IDC_EDIT_CONTACTNAME, OLE2CT(m_pParticipant.Name));

    UINT checkRecordButton = MetreosSettingsBase::AlwaysRecordCalls() ? 1 : 0;
    CheckDlgButton(IDC_CHECK_RECORDCALL, checkRecordButton);
    
    m_dialNumCombo.ResetContent();

    PopulateNumbersCombo(m_pParticipant.Home2Num, "Home 2");
    PopulateNumbersCombo(m_pParticipant.HomeNum, "Home");
    PopulateNumbersCombo(m_pParticipant.MobileNum, "Mobile");
    PopulateNumbersCombo(m_pParticipant.Bus2Num, "Business 2");
    PopulateNumbersCombo(m_pParticipant.BusNum, "Business");
    
    m_dialNumCombo.SetCurSel(0);
    m_dialNumCombo.SetEditSel(0, 100);
    m_dialNumCombo.SetFocus();
}

void CDialSingleDialog::PopulateNumbersCombo(CComBSTR number, CComBSTR desc)
{
    USES_CONVERSION;

    if((number != 0) && (number != ""))
    {
        CComBSTR textToAdd = desc;
        textToAdd += ": ";
        textToAdd += number;

        m_dialNumCombo.AddString(OLE2CT(textToAdd));
    }
}

void CDialSingleDialog::BuildControlMessage(ControlMessage& controlMsg)
{
    CString contactStr;
    CString numStr;
    CString numDigitOnlyStr;

    m_contactName.GetWindowText(contactStr);
    m_dialNumCombo.GetWindowText(numStr);

    // Iterate over the text array removing those characters
    // that are not digits.
    for(int i = 0; i < numStr.GetLength(); i++)
    {
        if(isdigit((unsigned char)numStr[i]) != 0)
        {
            numDigitOnlyStr += numStr[i];
        }
    }

    controlMsg.AddCallee(contactStr.GetBuffer(), numDigitOnlyStr.GetBuffer());
}
