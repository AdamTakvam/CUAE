#ifndef __METREOS_SETTINGS_BASE_H__
#define __METREOS_SETTINGS_BASE_H__

#pragma once

#include "Resource.h"
#include "Addin.h"

#define STRING_PROPERTY_SIZE 256

class MetreosSettingsBase
{
public:
    MetreosSettingsBase();
    virtual ~MetreosSettingsBase();

    static bool     AlwaysRecordCalls();

    virtual LRESULT OnBnClickedButtonValidate(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/) = 0;
    virtual LRESULT OnEnChangeCmUsername(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/) = 0;
    virtual LRESULT OnEnChangeCmPassword(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/) = 0;
    virtual LRESULT OnEnChangeAppserver(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/) = 0;
    virtual LRESULT OnBnClickedAlwaysRecord(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/) = 0;
    virtual LRESULT OnEnChangeUseremailaddr(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/) = 0;

protected:
    bool ValidateUserSettings(char* appServerStr, char* usernameStr, char* passwordStr, UINT appServerPort);

    void LoadPropertyValuesFromRegistry();
    void SavePropertyValuesToRegistry();

    void UpdatePropertyValue(int key, char value[256]);
    void UpdatePropertyValue(int key, DWORD value);

protected:
    char    m_emailAddrStr[STRING_PROPERTY_SIZE];
    char    m_usernameStr[STRING_PROPERTY_SIZE];
    char    m_passwordStr[STRING_PROPERTY_SIZE];
    char    m_appServerStr[STRING_PROPERTY_SIZE];
    DWORD   m_appServerPort;
    bool    m_alwaysRecord;

    CFont   m_validatedFont;
    CFont   m_standardFont;
};


#endif // __METREOS_SETTINGS_BASE_H__