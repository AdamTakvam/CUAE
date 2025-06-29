#ifndef __METREOS_TOOLBAR_H__
#define __METREOS_TOOLBAR_H__

#pragma once

#include "Resource.h"
#include "AddIn.h"
#include "EventCommon.h"

#include <list>


const LPWSTR METREOS_TEXT               = L"Metreos";
const LPWSTR METREOS_TOOL_TIP           = L"ClickToTalk menu";
const LPWSTR COMMAND_BAR_NAME           = L"Metreos ClickToTalk";
const LPWSTR DIAL_BUTTON_TEXT           = L"Call";
const LPWSTR DIAL_BUTTON_TOOL_TIP       = L"Call selected contacts";
const LPWSTR SETTINGS_BUTTON_TEXT       = L"Settings...";


struct DialParticipant
{
    CComBSTR Name;
    CComBSTR BusNum;
    CComBSTR Bus2Num;
    CComBSTR HomeNum;
    CComBSTR Home2Num;
    CComBSTR MobileNum;
};


typedef std::list<DialParticipant*>             DialParticipant_list;
typedef DialParticipant_list::iterator          DialParticipant_list_iterator;
typedef DialParticipant_list::const_iterator    DialParticipant_list_const_iterator;


class ATL_NO_VTABLE CMetreosToolbar :
    public CComObjectRootEx<CComSingleThreadModel>,
    public CComCoClass<CMetreosToolbar, &CLSID_MetreosToolbar>,
    public IDispatchImpl<IMetreosToolbar, &IID_IMetreosToolbar, &LIBID_OutlookClientLib, /*wMajor =*/ 1, /*wMinor =*/ 0>,
    public IDispEventSimpleImpl<1, CMetreosToolbar, &__uuidof(Office::_CommandBarButtonEvents)>,
    public IDispEventSimpleImpl<2, CMetreosToolbar, &__uuidof(Office::_CommandBarButtonEvents)>
{
public:
                        CMetreosToolbar();
                        CMetreosToolbar(CComPtr<Outlook::_Application> olApp);
    virtual             ~CMetreosToolbar();

    void                SetOutlookApplicationObj(CComPtr<Outlook::_Application> olApp);

    HRESULT             BuildToolbar(CComPtr<Outlook::_Inspector> inspector);
    HRESULT             ToggleToolbar(CComPtr<Outlook::_Inspector> inspector, bool bShow);
    HRESULT             BuildToolbar();

    void                Enable();
    void                Disable();

    void                SpawnDialDialog();
    void                PopulateDialButton(CComPtr<Outlook::_ContactItem> contactItem);
    void                PopulateDialButton(CComPtr<Outlook::Selection> selection);
    void                ClearDialParticipants();

    CComPtr<IDispatch>  GetDialButtonObj() const;
    CComPtr<IDispatch>  GetSettingsButtonObj() const;


    typedef IDispEventSimpleImpl<1, CMetreosToolbar, &__uuidof(Office::_CommandBarButtonEvents)> DialButtonEvents;
    typedef IDispEventSimpleImpl<2, CMetreosToolbar, &__uuidof(Office::_CommandBarButtonEvents)> SettingsButtonEvents;


    void __stdcall OnClickDialButton(IDispatch* /* Office::_CommandBarButton* */ Ctrl, VARIANT_BOOL* CancelDefault);
    void __stdcall OnClickSettingsButton(IDispatch* /* Office::_CommandBarButton* */ Ctrl, VARIANT_BOOL* CancelDefault);


    DECLARE_REGISTRY_RESOURCEID(IDR_METREOSTOOLBAR)

    BEGIN_COM_MAP(CMetreosToolbar)
	    COM_INTERFACE_ENTRY(IMetreosToolbar)
	    COM_INTERFACE_ENTRY(IDispatch)
    END_COM_MAP()

    ////////////////////////////////////////////////////////////
	// Event Sink Map
    ////////////////////
    BEGIN_SINK_MAP(CMetreosToolbar)
        SINK_ENTRY_INFO(1,  __uuidof(Office::_CommandBarButtonEvents),  BTN_CLICK,  OnClickDialButton,      &OnClickButtonInfo)
        SINK_ENTRY_INFO(2,  __uuidof(Office::_CommandBarButtonEvents),  BTN_CLICK,  OnClickSettingsButton,  &OnClickButtonInfo) 
    END_SINK_MAP()

	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{ return S_OK; }
	
	void FinalRelease() 
	{}

protected:
    HRESULT             BuildToolbar(CComPtr<Office::_CommandBars> spCmdBars);
    HRESULT             ToggleToolbar(CComPtr<Office::_CommandBars> spCmdBars, bool bShow);
	HRESULT				ToggleCallButton(CComPtr<Office::_CommandBars> spCmdBars, bool bEnable);

    HRESULT             AddMetreosButton();
    HRESULT             AddDialButton();
    int                 FindCommandBar(const CComPtr<Office::_CommandBars>& spCmdBars) const;
	void				EmptyClipbook();

protected:
    CComPtr<Outlook::_Application>          m_spOutlookApp;
    CComPtr<Office::_CommandBarButton>      m_spMetreosCircleButton;
    CComPtr<Office::_CommandBarButton>      m_spDialButton;
    CComPtr<Office::_CommandBarButton>      m_spSettingsButton;
    CComPtr<Office::CommandBarPopup>        m_spMetreosPopupButton;
    CComPtr<Office::CommandBar>             m_spCommandBar;

    DialParticipant_list                    m_dialParticipants;
};

OBJECT_ENTRY_AUTO(__uuidof(MetreosToolbar), CMetreosToolbar)

#endif // __METREOS_TOOLBAR_H__