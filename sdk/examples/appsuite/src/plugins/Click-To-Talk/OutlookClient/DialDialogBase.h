#ifndef __DIAL_DIALOG_BASE_H__
#define __DIAL_DIALOG_BASE_H__

#pragma once

#include "Resource.h"
#include "Addin.h"
#include "MetreosToolbar.h"
#include "ControlMessage.h"

class DialDialogBase
{
public:
    DialDialogBase();
    virtual ~DialDialogBase() {}

    void DialogDestroy();

    virtual void SetDialInformation(const DialParticipant_list& numbers) = 0;
    virtual void BuildControlMessage(ControlMessage& controlMsg) = 0;

    bool MakeCall(char* appServer, DWORD appServerPort, bool record, ControlMessage& controlMessage);
    
    bool HandleStartCall();
    void HandleStopRecord();

protected:
    void UpdateStatusBar(LPCSTR statusText);
    void UpdateStatusBar(UINT nID);

    char*   m_conferenceId;    

    CEdit   m_statusBar;
    CButton m_stopRecordButton;
    CButton m_dialButton;
};

#endif // __DIAL_DIALOG_BASE_H__