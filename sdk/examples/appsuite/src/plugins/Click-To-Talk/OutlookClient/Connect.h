#ifndef __CONNECT_H__
#define __CONNECT_H__

#pragma once

#include "Resource.h"
#include "AddIn.h"
#include "MetreosToolbar.h"
#include "EventCommon.h"


////////////////////////////////////////////////////////////
// class CConnect
//
// Primary Outlook Add-In class. Implements all required
// interfaces for connecting with Outlook and handling
// various Outlook events.
//
////////////////////
class ATL_NO_VTABLE CConnect : 
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CConnect, &CLSID_Connect>,
    public IDispatchImpl<IConnect, &IID_IConnect, &LIBID_OutlookClientLib>,
    public IDispatchImpl<AddInDesignerObjects::_IDTExtensibility2, &AddInDesignerObjects::IID__IDTExtensibility2, &AddInDesignerObjects::LIBID_AddInDesignerObjects, 1, 0>,
    public IDispEventSimpleImpl<10, CConnect, &__uuidof(Outlook::ApplicationEvents)>,
    public IDispEventSimpleImpl<20, CConnect, &__uuidof(Outlook::ExplorerEvents)>,
    public IDispEventSimpleImpl<30, CConnect, &__uuidof(Outlook::InspectorEvents)>,
    public IDispEventSimpleImpl<40, CConnect, &__uuidof(Outlook::InspectorsEvents)>
{
public:
	CConnect();


    ////////////////////////////////////////////////////////////
	// Event Types
    ////////////////////
    typedef IDispEventSimpleImpl<10, CConnect, &__uuidof(Outlook::ApplicationEvents)> AppEvents;
    typedef IDispEventSimpleImpl<20, CConnect, &__uuidof(Outlook::ExplorerEvents)> ExplorerEvents;
    typedef IDispEventSimpleImpl<30, CConnect, &__uuidof(Outlook::InspectorEvents)> InspectorEvents;
    typedef IDispEventSimpleImpl<40, CConnect, &__uuidof(Outlook::InspectorsEvents)> InspectorsEvents;


    ////////////////////////////////////////////////////////////
	// Event Handlers
    ////////////////////
	void __stdcall OnOptionsAddPages(IDispatch* /* PropertyPages* */ Ctrl);
    void __stdcall OnSelectionChange();
    void __stdcall OnFolderSwitch();
    void __stdcall OnCloseInspector();
    void __stdcall OnNewInspector(IDispatch* newInspector);


    ////////////////////////////////////////////////////////////
	// IDTExtensibility2 Implementation
    ////////////////////
	virtual HRESULT __stdcall   OnConnection(IDispatch* Application, AddInDesignerObjects::ext_ConnectMode ConnectMode, IDispatch* AddInInst, SAFEARRAY** custom);
	virtual HRESULT __stdcall   OnDisconnection(AddInDesignerObjects::ext_DisconnectMode RemoveMode, SAFEARRAY** custom );
	virtual HRESULT __stdcall   OnAddInsUpdate(SAFEARRAY** custom );
	virtual HRESULT __stdcall   OnStartupComplete(SAFEARRAY** custom );
	virtual HRESULT __stdcall   OnBeginShutdown(SAFEARRAY** custom );


    DECLARE_REGISTRY_RESOURCEID(IDR_ADDIN)
    DECLARE_NOT_AGGREGATABLE(CConnect)

    BEGIN_COM_MAP(CConnect)
        COM_INTERFACE_ENTRY(IConnect)
	    COM_INTERFACE_ENTRY2(IDispatch, IConnect)
	    COM_INTERFACE_ENTRY(AddInDesignerObjects::IDTExtensibility2)
    END_COM_MAP()

    ////////////////////////////////////////////////////////////
	// Event Sink Map
    ////////////////////
    BEGIN_SINK_MAP(CConnect)
        SINK_ENTRY_INFO(10, __uuidof(Outlook::ApplicationEvents),       APP_OPTIONS_PAGES_ADD,      OnOptionsAddPages,      &OnOptionsAddPagesInfo)
        SINK_ENTRY_INFO(20, __uuidof(Outlook::ExplorerEvents),          EXP_SELECTION_CHANGE,       OnSelectionChange,      &OnSelectionChangeInfo)
        SINK_ENTRY_INFO(20, __uuidof(Outlook::ExplorerEvents),          EXP_FOLDER_SWITCH,          OnFolderSwitch,         &OnFolderSwitchInfo)
        SINK_ENTRY_INFO(30, __uuidof(Outlook::InspectorEvents),         INS_CLOSE,                  OnCloseInspector,       &OnCloseInspectorInfo)
        SINK_ENTRY_INFO(40, __uuidof(Outlook::InspectorsEvents),        INSS_NEW_INSPECTOR,         OnNewInspector,         &OnNewInspectorInfo)
    END_SINK_MAP()

	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct()
	{ return S_OK; }
	
	void FinalRelease() 
	{}

protected:
    ////////////////////////////////////////////////////////////
	// Convenience Methods
    ////////////////////
    void GetToolsOptionsControl();

    CComPtr<Outlook::_Application>  m_spOutlookApp;
	CComPtr<IDispatch>              m_spAddInInstance;

    CComPtr<CMetreosToolbar>        m_spMetreosToolbar;

    bool                            m_haveRegisteredInspectorEvents;
};

OBJECT_ENTRY_AUTO(__uuidof(Connect), CConnect)

#endif // __CONNECT_H__
