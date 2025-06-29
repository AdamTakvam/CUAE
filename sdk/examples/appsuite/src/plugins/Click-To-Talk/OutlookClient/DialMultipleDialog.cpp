#include "StdAfx.h"
#include "DialMultipleDialog.h"
#include "ControlMessage.h"
#include "HttpHelpers.h"
#include "RegistryHelpers.h"
#include "MetreosSettingsBase.h"
#include ".\dialmultipledialog.h"

CDialMultipleDialog::CDialMultipleDialog() : 
    m_numParticipants(0)
{}

CDialMultipleDialog::~CDialMultipleDialog()
{
    m_dialNumCombo[0].Detach();
    m_dialNumCombo[1].Detach();
    m_dialNumCombo[2].Detach();
    m_dialNumCombo[3].Detach();

    m_editBoxes[0].Detach();
    m_editBoxes[1].Detach();
    m_editBoxes[2].Detach();
    m_editBoxes[3].Detach();

    m_statusBar.Detach();
    m_stopRecordButton.Detach();
    m_dialButton.Detach();
}

LRESULT CDialMultipleDialog::OnInitDialog(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
    CAxDialogImpl<CDialMultipleDialog>::OnInitDialog(uMsg, wParam, lParam, bHandled);

    m_dialNumCombo[0].Attach(GetDlgItem(IDC_COMBO_NUMBERS1));
    m_dialNumCombo[1].Attach(GetDlgItem(IDC_COMBO_NUMBERS2));
    m_dialNumCombo[2].Attach(GetDlgItem(IDC_COMBO_NUMBERS3));
    m_dialNumCombo[3].Attach(GetDlgItem(IDC_COMBO_NUMBERS4));

    m_editBoxes[0].Attach(GetDlgItem(IDC_EDIT_CONTACTNAME1));
    m_editBoxes[1].Attach(GetDlgItem(IDC_EDIT_CONTACTNAME2));
    m_editBoxes[2].Attach(GetDlgItem(IDC_EDIT_CONTACTNAME3));
    m_editBoxes[3].Attach(GetDlgItem(IDC_EDIT_CONTACTNAME4));

    m_statusBar.Attach(GetDlgItem(IDC_STATUSBAR_DSD));
    m_stopRecordButton.Attach(GetDlgItem(IDC_BTN_STOP_RECORD));
    m_dialButton.Attach(GetDlgItem(IDOK));

    m_stopRecordButton.EnableWindow(0);

    InitDialogData();

    return 1;  // Let the system set the focus
}

LRESULT CDialMultipleDialog::OnDestroy(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
    DialogDestroy();
    
    m_dialNumCombo[0].Clear();
    m_dialNumCombo[1].Clear();
    m_dialNumCombo[2].Clear();
    m_dialNumCombo[3].Clear();

    m_numParticipants = 0;

	return 1;  // Let the system set the focus
}

LRESULT CDialMultipleDialog::OnClickedStartCall(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled)
{
    USES_CONVERSION;

	bool go = false;
    for(int j = 0; j < m_numParticipants; j++)
    {
        if (m_dialNumCombo[j].GetWindowTextLength() > 0)
		{
			go = true;
			break;
		}
    }

	if (!go)
		return 0;

    if(HandleStartCall() == false)
    {
        EndDialog(wID);
    }

    return 0;
}

LRESULT CDialMultipleDialog::OnClickedCancel(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled)
{
	EndDialog(wID);
	return 0;
}

LRESULT CDialMultipleDialog::OnBnClickedBtnRecord(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled)
{
    USES_CONVERSION;

    HandleStopRecord();

    return 0;
}

void CDialMultipleDialog::SetDialInformation(const DialParticipant_list& numbers)
{
    DialParticipant_list_const_iterator i = numbers.begin();

    int j = 0;
    while((i != numbers.end()) && (j < 4))
    {
        DialParticipant* srcParticipant = *i;

        if( (srcParticipant->BusNum != 0) ||
            (srcParticipant->Bus2Num != 0) ||
            (srcParticipant->HomeNum != 0) ||
            (srcParticipant->Home2Num != 0) ||
            (srcParticipant->MobileNum != 0))
        {
            m_pParticipants[j].Name = srcParticipant->Name;
            m_pParticipants[j].BusNum = srcParticipant->BusNum;
            m_pParticipants[j].Bus2Num = srcParticipant->Bus2Num;
            m_pParticipants[j].HomeNum = srcParticipant->HomeNum;
            m_pParticipants[j].Home2Num = srcParticipant->Home2Num;
            m_pParticipants[j].MobileNum = srcParticipant->MobileNum;
            j++;
        }

        i++;
    }

    m_numParticipants = j;
}

void CDialMultipleDialog::InitDialogData()
{
    USES_CONVERSION;

    UINT checkRecordButton = MetreosSettingsBase::AlwaysRecordCalls() ? 1 : 0;
    CheckDlgButton(IDC_CHECK_RECORDCALL, checkRecordButton);

    for(int i = 0; i < m_numParticipants; i++)
    {
        m_editBoxes[i].Clear();
        m_editBoxes[i].SetWindowText(OLE2CT(m_pParticipants[i].Name));

        m_dialNumCombo[i].ResetContent();
        PopulateNumbersCombo(i, m_pParticipants[i].Home2Num, "Home 2");
        PopulateNumbersCombo(i, m_pParticipants[i].HomeNum, "Home");
        PopulateNumbersCombo(i, m_pParticipants[i].MobileNum, "Mobile");
        PopulateNumbersCombo(i, m_pParticipants[i].Bus2Num, "Business 2");
        PopulateNumbersCombo(i, m_pParticipants[i].BusNum, "Business");

        m_dialNumCombo[i].SetCurSel(0);
        m_dialNumCombo[i].SetEditSel(0, 100);

        m_editBoxes[i].EnableWindow();
        m_dialNumCombo[i].EnableWindow();
    }
}

void CDialMultipleDialog::PopulateNumbersCombo(int index, CComBSTR number, CComBSTR desc)
{
    USES_CONVERSION;

    if((number != 0) && (number != ""))
    {
        CComBSTR textToAdd = desc;
        textToAdd += ": ";
        textToAdd += number;

        m_dialNumCombo[index].AddString(OLE2CT(textToAdd));
    }
}

void CDialMultipleDialog::BuildControlMessage(ControlMessage& controlMsg)
{
    CString contactStr[4];
    CString numStr[4];
    CString numDigitOnlyStr[4];

    for(int j = 0; j < m_numParticipants; j++)
    {
        m_editBoxes[j].GetWindowText(contactStr[j]);
        m_dialNumCombo[j].GetWindowText(numStr[j]);

        // Iterate over the text array removing those characters
        // that are not digits.
        for(int i = 0; i < numStr[j].GetLength(); i++)
        {
            if(isdigit((unsigned char)numStr[j][i]) != 0)
            {
                numDigitOnlyStr[j] += numStr[j][i];
            }
        }
    }

    for(int k = 0; k < m_numParticipants; k++)
    {
        controlMsg.AddCallee(contactStr[k].GetBuffer(), numDigitOnlyStr[k].GetBuffer());
    }
}
