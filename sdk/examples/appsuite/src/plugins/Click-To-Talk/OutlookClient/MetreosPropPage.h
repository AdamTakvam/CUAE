// MetreosPropPage.h : Declaration of the CMetreosPropPage
#pragma once
#include "Resource.h"       // main symbols
#include <atlctl.h>

#include "Addin.h"
#include "MetreosSettingsBase.h"

// CMetreosPropPage
class ATL_NO_VTABLE CMetreosPropPage : 
    public CComObjectRootEx<CComSingleThreadModel>,
    public IDispatchImpl<IMetreosPropPage, &IID_IMetreosPropPage, &LIBID_OutlookClientLib, /*wMajor =*/ 1, /*wMinor =*/ 0>,
    public CComCompositeControl<CMetreosPropPage>,
    public IPersistStreamInitImpl<CMetreosPropPage>,
    public IOleControlImpl<CMetreosPropPage>,
    public IOleObjectImpl<CMetreosPropPage>,
    public IOleInPlaceActiveObjectImpl<CMetreosPropPage>,
    public IViewObjectExImpl<CMetreosPropPage>,
    public IOleInPlaceObjectWindowlessImpl<CMetreosPropPage>,
    public IPersistStorageImpl<CMetreosPropPage>,
    public ISpecifyPropertyPagesImpl<CMetreosPropPage>,
    public IQuickActivateImpl<CMetreosPropPage>,
    public IDataObjectImpl<CMetreosPropPage>,
    public IProvideClassInfo2Impl<&CLSID_MetreosPropPage, NULL, &LIBID_OutlookClientLib>,
    public CComCoClass<CMetreosPropPage, &CLSID_MetreosPropPage>,
    public IPersistPropertyBagImpl<CMetreosPropPage>,
    public IDispatchImpl<Outlook::PropertyPage, &__uuidof(Outlook::PropertyPage), &Outlook::LIBID_Outlook, /* wMajor = */ 9, /* wMinor = */ 1>,
    public MetreosSettingsBase
{
public:

    CMetreosPropPage()
    {
        m_bWindowOnly = TRUE;
        CalcExtent(m_sizeExtent);
    }

    DECLARE_OLEMISC_STATUS(OLEMISC_RECOMPOSEONRESIZE | 
        OLEMISC_CANTLINKINSIDE | 
        OLEMISC_INSIDEOUT | 
        OLEMISC_ACTIVATEWHENVISIBLE | 
        OLEMISC_SETCLIENTSITEFIRST
        )

    DECLARE_REGISTRY_RESOURCEID(IDR_METREOSPROPPAGE)

    BEGIN_COM_MAP(CMetreosPropPage)
        COM_INTERFACE_ENTRY(IMetreosPropPage)
        COM_INTERFACE_ENTRY2(IDispatch, PropertyPage)
        COM_INTERFACE_ENTRY(IViewObjectEx)
        COM_INTERFACE_ENTRY(IViewObject2)
        COM_INTERFACE_ENTRY(IViewObject)
        COM_INTERFACE_ENTRY(IOleInPlaceObjectWindowless)
        COM_INTERFACE_ENTRY(IOleInPlaceObject)
        COM_INTERFACE_ENTRY2(IOleWindow, IOleInPlaceObjectWindowless)
        COM_INTERFACE_ENTRY(IOleInPlaceActiveObject)
        COM_INTERFACE_ENTRY(IOleControl)
        COM_INTERFACE_ENTRY(IOleObject)
        COM_INTERFACE_ENTRY(IPersistStreamInit)
        COM_INTERFACE_ENTRY2(IPersist, IPersistStreamInit)
        COM_INTERFACE_ENTRY(ISpecifyPropertyPages)
        COM_INTERFACE_ENTRY(IQuickActivate)
        COM_INTERFACE_ENTRY(IPersistStorage)
        COM_INTERFACE_ENTRY(IDataObject)
        COM_INTERFACE_ENTRY(IProvideClassInfo)
        COM_INTERFACE_ENTRY(IProvideClassInfo2)
        COM_INTERFACE_ENTRY(IPersistPropertyBag)
        COM_INTERFACE_ENTRY(Outlook::PropertyPage)
    END_COM_MAP()

    BEGIN_PROP_MAP(CMetreosPropPage)
        PROP_DATA_ENTRY("_cx", m_sizeExtent.cx, VT_UI4)
        PROP_DATA_ENTRY("_cy", m_sizeExtent.cy, VT_UI4)
    END_PROP_MAP()

    BEGIN_MSG_MAP(CMetreosPropPage)
        MESSAGE_HANDLER(WM_INITDIALOG, OnInitDialog)
        COMMAND_HANDLER(IDC_BUTTON_VALIDATE, BN_CLICKED, OnBnClickedButtonValidate)
        COMMAND_HANDLER(IDC_CM_USERNAME, EN_CHANGE, OnEnChangeCmUsername)
        COMMAND_HANDLER(IDC_CM_PASSWORD, EN_CHANGE, OnEnChangeCmPassword)
        COMMAND_HANDLER(IDC_APPSERVER, EN_CHANGE, OnEnChangeAppserver)
        COMMAND_HANDLER(IDC_ALWAYS_RECORD, BN_CLICKED, OnBnClickedAlwaysRecord)
        COMMAND_HANDLER(IDC_USEREMAILADDR, EN_CHANGE, OnEnChangeUseremailaddr)
        CHAIN_MSG_MAP(CComCompositeControl<CMetreosPropPage>)
    END_MSG_MAP()

    // Handler prototypes:
    //  LRESULT MessageHandler(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
    //  LRESULT CommandHandler(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);
    //  LRESULT NotifyHandler(int idCtrl, LPNMHDR pnmh, BOOL& bHandled);

    BEGIN_SINK_MAP(CMetreosPropPage)
        // Make sure the Event Handlers have __stdcall calling convention
    END_SINK_MAP()
    
    // IViewObjectEx
    DECLARE_VIEW_STATUS(0)

public:
    enum { IDD = IDD_METREOSPROPPAGE };

    DECLARE_PROTECT_FINAL_CONSTRUCT()

    HRESULT FinalConstruct() { return S_OK;  }
    void FinalRelease() 
    {
        m_appServerEdit.Detach();
        m_appServerPortEdit.Detach();
    }

    HRESULT __stdcall   SetClientSite(IOleClientSite* pClientSite);
    HRESULT __stdcall   Apply();
    HRESULT __stdcall   GetPageInfo(BSTR* HelpFile, long* HelpContext);
    HRESULT __stdcall   get_Dirty(VARIANT_BOOL* Dirty);

protected:
    CComVariant                         m_fDirty;
    CComPtr<Outlook::PropertyPageSite>  m_pPropPageSite;

    CEdit   m_appServerEdit;
    CEdit   m_appServerPortEdit;

public:
    LRESULT OnInitDialog(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
    LRESULT OnBnClickedButtonValidate(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);
    LRESULT OnEnChangeCmUsername(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);
    LRESULT OnEnChangeCmPassword(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);
    LRESULT OnEnChangeAppserver(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);
    LRESULT OnBnClickedAlwaysRecord(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);
    LRESULT OnEnChangeUseremailaddr(WORD /*wNotifyCode*/, WORD /*wID*/, HWND /*hWndCtl*/, BOOL& /*bHandled*/);
};

OBJECT_ENTRY_AUTO(__uuidof(MetreosPropPage), CMetreosPropPage)
