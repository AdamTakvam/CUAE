#include "StdAfx.h"
#include "Connect.h"
#include "MetreosToolbar.h"
#include "ControlMessage.h"
#include "MetreosPropPageDialog.h"

extern CAddInModule _AtlModule;

CConnect::CConnect() :
    m_haveRegisteredInspectorEvents(false)
{}

////////////////////////////////////////////////////////////
// IDTExtensibility2 Implementation
////////////////////
HRESULT __stdcall CConnect::OnConnection(IDispatch* pApplication, AddInDesignerObjects::ext_ConnectMode ConnectMode, IDispatch* pAddInInst, LPSAFEARRAY* custom)
{
    CComQIPtr<Outlook::_Application> olApp(pApplication);
    if(olApp == 0) { return S_FALSE; }

    m_spOutlookApp = olApp;
    m_spAddInInstance = pAddInInst;

    CComPtr<Outlook::_Explorer> explorer;
    CComPtr<Outlook::_Inspectors> inspectors;

    HRESULT hr = m_spOutlookApp->ActiveExplorer(&explorer);
    if(FAILED(hr)) { return S_FALSE; }
    
    hr = m_spOutlookApp->get_Inspectors(&inspectors);
    if(FAILED(hr)) { return S_FALSE; }

    // Check to see if we have a null explorer. If we do 
    // that probably means Outlook has been launched because
    // someone clicked a "mailto:" link.
    //
    // If this is the case, do not initialize or load the
    // MetreosToolbar and do not register any events.
    //
    if(explorer != 0)
    {
        // We have a valid explorer, so lets finish intializing
        // everything.    
        hr = CoCreateInstance(CLSID_MetreosToolbar, 0, CLSCTX_ALL, IID_IMetreosToolbar, (void**)&m_spMetreosToolbar);
        if(FAILED(hr)) { return S_FALSE; }

        m_spMetreosToolbar->SetOutlookApplicationObj(m_spOutlookApp);

        hr = AppEvents::DispEventAdvise(m_spOutlookApp, &__uuidof(Outlook::ApplicationEvents));
        hr = ExplorerEvents::DispEventAdvise(explorer, &__uuidof(Outlook::ExplorerEvents));
        hr = InspectorsEvents::DispEventAdvise(inspectors, &__uuidof(Outlook::InspectorsEvents));

        if (ConnectMode != AddInDesignerObjects::ext_cm_Startup)
        {
            HRESULT hr;
            hr = OnStartupComplete(custom);
            if(FAILED(hr)) { return hr; }
        }
    }

    return S_OK;
}

HRESULT __stdcall CConnect::OnDisconnection(AddInDesignerObjects::ext_DisconnectMode /*RemoveMode*/, SAFEARRAY** /*custom*/ )
{
    CComPtr<Outlook::_Explorer> explorer;
    HRESULT hr = m_spOutlookApp->ActiveExplorer(&explorer);
    if(FAILED(hr)) { return S_FALSE; }

    ExplorerEvents::DispEventUnadvise(explorer);
    AppEvents::DispEventUnadvise(m_spOutlookApp);

    m_spMetreosToolbar = 0;
    m_spOutlookApp = 0;

    return S_OK;
}

HRESULT __stdcall CConnect::OnAddInsUpdate (SAFEARRAY** /*custom*/ )
{ return S_OK; }

HRESULT __stdcall CConnect::OnStartupComplete (SAFEARRAY** /*custom*/ )
{
    if(m_spMetreosToolbar != 0)
    {
        // Build the Metreos toolbar.
        HRESULT hr = m_spMetreosToolbar->BuildToolbar();
        if(FAILED(hr)) { return hr; }

		m_spMetreosToolbar->Disable();
    }

    return S_OK;
}

HRESULT __stdcall CConnect::OnBeginShutdown (SAFEARRAY** /*custom*/ )
{ return S_OK; }


////////////////////////////////////////////////////////////
// Event Handlers
////////////////////
void __stdcall CConnect::OnOptionsAddPages(IDispatch* Ctrl)
{
	CComQIPtr<Outlook::PropertyPages> spPages(Ctrl);
	ATLASSERT(spPages);

	CComVariant varProgId(OLESTR("OutlookClient.MetreosPropPage"));
	CComBSTR bstrTitle(OLESTR("Metreos ClickToTalk"));

	HRESULT hr = spPages->Add((_variant_t)varProgId,(_bstr_t)bstrTitle);
	if(FAILED(hr))
    {
		ATLTRACE("\nFailed addin propertypage");

		TCHAR msg[512], title[256];
		HINSTANCE hInst = _AtlModule.GetResourceInstance();
		::LoadString(hInst, IDS_ERROR_ADDIN_PP, msg, sizeof(msg)/sizeof(TCHAR)); 		
		::LoadString(hInst, IDS_ERROR_ADDIN_PP_TITLE, title, sizeof(title)/sizeof(TCHAR)); 		
        MessageBox(::GetActiveWindow(), msg /*"Failed addin propertypage"*/, title /*"OnOptionsAddPages"*/, MB_OK);
    }
}

void __stdcall CConnect::OnSelectionChange()
{
    CComPtr<Outlook::_Explorer> explorer;
    CComPtr<IDispatch> item;
    CComPtr<Outlook::Selection> selection;

    HRESULT hr = m_spOutlookApp->ActiveExplorer(&explorer);
    if(FAILED(hr)) { return; }
    ATLASSERT(explorer);

    explorer->get_Selection(&selection);

    long selectionCount = 0;
    selection->get_Count(&selectionCount);

    if(selectionCount > 0)
    {
        // Grab the first item.
        selection->Item(CComVariant(1), &item);
    }

    CComQIPtr<Outlook::_ContactItem> contactItem(item);
    if(contactItem != 0)
    {
        // We have a contact item. Pass it to the MetreosToolbar
        // so it can populate its drop-down list.
        m_spMetreosToolbar->PopulateDialButton(selection);
    }
}

void __stdcall CConnect::OnFolderSwitch()
{    
    CComPtr<Outlook::_Explorer> spExplorer;
    CComPtr<Outlook::MAPIFolder> spNewFolder;

    HRESULT hr = m_spOutlookApp->ActiveExplorer(&spExplorer);
    if(FAILED(hr)) { return; }
    
    if(spExplorer != 0)
    {
        hr = spExplorer->get_CurrentFolder(&spNewFolder);
        if(FAILED(hr)) { return; }
        ATLASSERT(spNewFolder);

        CComBSTR folderName;
        spNewFolder->get_Name(&folderName);

        if(folderName == "Contacts")
        {
            m_spMetreosToolbar->Enable();      
        }
        else
        {
            m_spMetreosToolbar->Disable();
        }
    }
}

void __stdcall CConnect::OnNewInspector(IDispatch* newInspector)
{
    CComQIPtr<Outlook::_Inspector> inspector(newInspector);

    CComPtr<IDispatch> item;
    inspector->get_CurrentItem(&item);
	
    CComQIPtr<Outlook::_ContactItem> contactItem(item);

    // Only add the MetreosToolbar if we are manipulating a 
    // contact item.
    HRESULT hr;
    CComPtr<CMetreosToolbar> spMtb;
    hr = CoCreateInstance(CLSID_MetreosToolbar, 0, CLSCTX_ALL, IID_IMetreosToolbar, (void**)&spMtb);
    if(FAILED(hr)) { return; }

    if(contactItem != 0)
    {
        spMtb->SetOutlookApplicationObj(m_spOutlookApp);
        spMtb->BuildToolbar(inspector);
        spMtb->ClearDialParticipants();
        spMtb->PopulateDialButton(contactItem);
    }
	else
	{
        spMtb->ToggleToolbar(inspector, false);
	}
}

void __stdcall CConnect::OnCloseInspector()
{}

////////////////////////////////////////////////////////////
// Convenience Methods
////////////////////
void CConnect::GetToolsOptionsControl()
{
    CComPtr<Outlook::_Explorer> explorer;
    CComPtr<Office::_CommandBars> cmdBars;
    CComPtr<Office::CommandBar> menuBar;
    CComPtr<Office::CommandBarControls> menuBarControls;
    CComPtr<Office::CommandBarControl> toolsControl;

    HRESULT hr;

    m_spOutlookApp->ActiveExplorer(&explorer);

    explorer->get_CommandBars(&cmdBars);
    cmdBars->get_ActiveMenuBar(&menuBar);
    menuBar->get_Controls(&menuBarControls);

    hr = menuBarControls->get_Item(CComVariant(5), &toolsControl);
    if(FAILED(hr)) { return; }
    ATLASSERT(toolsControl);

    IDispatch* iDisp;

    hr = toolsControl->get_Control(&iDisp);
    if(FAILED(hr)) { return; }
    ATLASSERT(iDisp);

    CComQIPtr<Office::CommandBarPopup> toolsMenu(iDisp);
    ATLASSERT(toolsMenu);

    menuBarControls = 0;
    hr = toolsMenu->get_Controls(&menuBarControls);
    if(FAILED(hr)) { return; }
    ATLASSERT(menuBarControls);

    int count = 0;
    menuBarControls->get_Count(&count);

    CComBSTR caption;
    for(int i = 1; i <= count; i++)
    {
        CComPtr<Office::CommandBarControl> optionsControl;
        if(menuBarControls->get_Item(CComVariant(i), &optionsControl) == S_OK)
        {
            ATLASSERT(optionsControl);

            optionsControl->get_Caption(&caption);

            if(caption == L"&Options...")
            {
                optionsControl->Execute();
                break;
            }
        }
    }
}
