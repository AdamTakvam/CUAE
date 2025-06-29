#include "StdAfx.h"
#include "COMAddinUtil.h"
#include "MetreosToolbar.h"
#include "DialSingleDialog.h"
#include "DialMultipleDialog.h"
#include "MetreosPropPageDialog.h"
#include ".\metreostoolbar.h"

CMetreosToolbar::CMetreosToolbar()
{
}

CMetreosToolbar::CMetreosToolbar(CComPtr<Outlook::_Application> olApp)
{
    ATLASSERT(olApp);
    m_spOutlookApp = olApp;
}

CMetreosToolbar::~CMetreosToolbar()
{        
    // Deregister the event callbacks.
    DialButtonEvents::DispEventUnadvise(m_spDialButton);
    SettingsButtonEvents::DispEventUnadvise(m_spSettingsButton);
}

void CMetreosToolbar::SetOutlookApplicationObj(CComPtr<Outlook::_Application> olApp)
{
    ATLASSERT(olApp);
    m_spOutlookApp = olApp;
}

HRESULT CMetreosToolbar::BuildToolbar()
{
    CComPtr<Outlook::_Explorer> spExplorer;
    CComPtr<Office::_CommandBars> spCmdBars;
    HRESULT hr;

    // Get the active explorer.
    hr = m_spOutlookApp->ActiveExplorer(&spExplorer);
    if (FAILED(hr)) { return hr; }
    ATLASSERT(spExplorer);

    // Get the CommandBars interface that represents Outlook's 		
    // toolbars and menu items.
    hr = spExplorer->get_CommandBars(&spCmdBars);
    if (FAILED(hr)) { return hr; }
    ATLASSERT(spCmdBars);

    return BuildToolbar(spCmdBars);
}


HRESULT CMetreosToolbar::BuildToolbar(CComPtr<Outlook::_Inspector> spInspector)
{
    CComPtr<Office::_CommandBars> spCmdBars;
    HRESULT hr;

    hr = spInspector->get_CommandBars(&spCmdBars);
    if (FAILED(hr)) { return hr; }
    ATLASSERT(spCmdBars);

    return BuildToolbar(spCmdBars);
}

HRESULT CMetreosToolbar::ToggleToolbar(CComPtr<Outlook::_Inspector> spInspector, bool bShow)
{
    CComPtr<Office::_CommandBars> spCmdBars;
    HRESULT hr;

    hr = spInspector->get_CommandBars(&spCmdBars);
    if (FAILED(hr)) { return hr; }
    ATLASSERT(spCmdBars);

    return ToggleToolbar(spCmdBars, bShow);
}


HRESULT CMetreosToolbar::BuildToolbar(CComPtr<Office::_CommandBars> spCmdBars)
{
    CComPtr<Office::CommandBar> spCmdBar;
    HRESULT hr;

    // Before we add the new command bar, search the current
    // command bar collection for our command bar's name.
    // If the command bar is already there, then we will use
    // the already existing one.
    int foundAtIndex = FindCommandBar(spCmdBars);

    m_spCommandBar = 0;
    if(foundAtIndex == -1)
    {
        // Command bar not present, create a new one.
        CComVariant vName(COMMAND_BAR_NAME);    // The bar's name.
        CComVariant vPos(Office::msoBarTop);    // Bar is on top.            
        CComVariant vTemp(VARIANT_FALSE);       // Bar is not temporary.	
        CComVariant vEmpty(DISP_E_PARAMNOTFOUND, VT_ERROR);	

        // Add the new command bar to the control collection.
        hr = spCmdBars->Add(vName, vPos, vEmpty, vTemp, &m_spCommandBar);
        if (FAILED(hr)) { return hr; }
    }
    else
    {
        // Command bar already exists, pull it out of the command bars collection.
        hr = spCmdBars->get_Item(CComVariant(foundAtIndex), &m_spCommandBar);
        if (FAILED(hr)) { return hr; }
    }

    ATLASSERT(m_spCommandBar);

    hr = AddMetreosButton();
    if (FAILED(hr)) { return hr; }

    hr = AddDialButton();
    if (FAILED(hr)) { return hr; }

    // Show the toolbar.
    m_spCommandBar->put_Enabled(VARIANT_TRUE);
    m_spCommandBar->put_Visible(VARIANT_TRUE); 

    // Register the callback event for the dial button.
    DialButtonEvents::DispEventAdvise(m_spDialButton, &DIID__CommandBarButtonEvents);
    SettingsButtonEvents::DispEventAdvise(m_spSettingsButton, &DIID__CommandBarButtonEvents);

    return S_OK;
}

HRESULT CMetreosToolbar::ToggleToolbar(CComPtr<Office::_CommandBars> spCmdBars, bool bShow)
{
    CComPtr<Office::CommandBar> spCmdBar;
    HRESULT hr;

    // Before we add the new command bar, search the current
    // command bar collection for our command bar's name.
    // If the command bar is already there, then we will use
    // the already existing one.
    int foundAtIndex = FindCommandBar(spCmdBars);

    m_spCommandBar = 0;
    if(foundAtIndex != -1)
    {
        // Command bar already exists, hide it.
        hr = spCmdBars->get_Item(CComVariant(foundAtIndex), &m_spCommandBar);
        if (FAILED(hr)) { return hr; }

		if (bShow)
		{
			m_spCommandBar->put_Enabled(VARIANT_TRUE);
			m_spCommandBar->put_Visible(VARIANT_TRUE); 
		}
		else
		{
			m_spCommandBar->put_Enabled(VARIANT_FALSE);
			m_spCommandBar->put_Visible(VARIANT_FALSE); 
		}
    }

    return S_OK;
}

HRESULT CMetreosToolbar::ToggleCallButton(CComPtr<Office::_CommandBars> spCmdBars, bool bEnable)
{
    CComPtr<Office::CommandBar> spCmdBar;
    HRESULT hr;

    // Before we add the new command bar, search the current
    // command bar collection for our command bar's name.
    // If the command bar is already there, then we will use
    // the already existing one.
    int foundAtIndex = FindCommandBar(spCmdBars);

    m_spCommandBar = 0;
    if(foundAtIndex != -1)
    {
        hr = spCmdBars->get_Item(CComVariant(foundAtIndex), &m_spCommandBar);
        if (FAILED(hr)) { return hr; }

		m_spCommandBar->put_Enabled(VARIANT_TRUE);
		m_spCommandBar->put_Visible(VARIANT_TRUE);
		if (bEnable)
		{
			m_spDialButton->put_Enabled(VARIANT_TRUE);
		}
		else
		{
			m_spDialButton->put_Enabled(VARIANT_FALSE);
		}
    }

    return S_OK;
}


void CMetreosToolbar::Enable()
{
    // Currently does nothing because whenever we try to
    // set Visible = true/false the other command bars 
    // that Outlook has get screwed up.

    CComPtr<Outlook::_Explorer> spExplorer;

    HRESULT hr = m_spOutlookApp->ActiveExplorer(&spExplorer);
    if(FAILED(hr)) { return; }

    CComPtr<Office::_CommandBars> spCmdBars;
    hr = spExplorer->get_CommandBars(&spCmdBars);
    if (FAILED(hr)) { return; }

    ATLASSERT(spCmdBars);

    ToggleCallButton(spCmdBars, true);
}

void CMetreosToolbar::Disable()
{
    // Currently does nothing because whenever we try to
    // set Visible = true/false the other command bars 
    // that Outlook has get screwed up.

    // Only add the MetreosToolbar if we are manipulating a 
    // contact item.

    CComPtr<Outlook::_Explorer> spExplorer;

    HRESULT hr = m_spOutlookApp->ActiveExplorer(&spExplorer);
    if(FAILED(hr)) { return; }

    CComPtr<Office::_CommandBars> spCmdBars;
    hr = spExplorer->get_CommandBars(&spCmdBars);
    if (FAILED(hr)) { return; }

    ATLASSERT(spCmdBars);

    ToggleCallButton(spCmdBars, false);
}

void CMetreosToolbar::SpawnDialDialog()
{
    if(m_dialParticipants.size() == 0)
    {
        // No dial participants were selected.
        return;
    }

    if(m_dialParticipants.size() == 1)
    {
        CDialSingleDialog dlg;
        dlg.SetDialInformation(m_dialParticipants);
        dlg.DoModal();
    }
    else
    {
        CDialMultipleDialog dlg;
        dlg.SetDialInformation(m_dialParticipants);
        dlg.DoModal();
    }
}

void CMetreosToolbar::PopulateDialButton(CComPtr<Outlook::_ContactItem> contactItem)
{
    DialParticipant* participant = new DialParticipant;
    m_dialParticipants.push_back(participant);

    CComBSTR name;
    CComBSTR firstName;
    CComBSTR lastName;

    contactItem->get_FirstName(&firstName);
    contactItem->get_LastName(&lastName);

    name = firstName;
    name += " ";
    name += lastName;

    participant->Name = name;

    contactItem->get_BusinessTelephoneNumber(&participant->BusNum);
    contactItem->get_Business2TelephoneNumber(&participant->Bus2Num);
    contactItem->get_HomeTelephoneNumber(&participant->HomeNum);
    contactItem->get_Home2TelephoneNumber(&participant->Home2Num);
    contactItem->get_MobileTelephoneNumber(&participant->MobileNum);
}

void CMetreosToolbar::PopulateDialButton(CComPtr<Outlook::Selection> selection)
{
    long selectionCount = 0;

    ClearDialParticipants();

    selection->get_Count(&selectionCount);

    if(selectionCount > 4)
    {
        // Error, maximum of 4 conference participants.
    }

    for(int i = 1; i <= selectionCount; i++)
    {
        CComPtr<IDispatch> item;
        selection->Item(CComVariant(i), &item);

        CComQIPtr<Outlook::_ContactItem> contactItem(item);

        if(contactItem != 0)
        {
            PopulateDialButton(contactItem);
        }
    }
}

HRESULT CMetreosToolbar::AddMetreosButton()
{
    HRESULT hr;

    // Get the toolbar's CommandBarControls
    CComPtr <Office::CommandBarControls> spBarControls;
    hr = m_spCommandBar->get_Controls(&spBarControls);
    if (FAILED(hr)) { return hr; }
    ATLASSERT(spBarControls);

    CComVariant vTemp(VARIANT_TRUE);	
    CComVariant vEmpty(DISP_E_PARAMNOTFOUND, VT_ERROR);	
    CComVariant vToolBarType(Office::msoControlButton);
    CComVariant vShow(VARIANT_TRUE);                        // Show the toolbar?

    CComPtr<Office::CommandBarControl> spCmdBarControl; 

    // Add a control for the first button to the toolbar.
    hr = spBarControls->Add(vToolBarType, vEmpty, vEmpty, vEmpty, vShow, &spCmdBarControl);
    if (FAILED(hr)) { return hr; }
    ATLASSERT(spCmdBarControl);

#ifdef SUPPORT_OL9
    CComQIPtr <Office::_CommandBarButton> spCmdButton(spCmdBarControl);
    ATLASSERT(spCmdButton);

    // To set a bitmap to a button, load a 32x32 bitmap
    // and copy it to clipboard. Call CommandBarButton's PasteFace()
    // to copy the bitmap to the button face. To Use Outlook's
    // set of predefined bitmap, set button's FaceId to the
    // button whose bitmap you want to use.
    HBITMAP hBmp = (HBITMAP)::LoadImage(
        _AtlModule.GetResourceInstance(),
        MAKEINTRESOURCE(IDB_BMP_METREOSCIRCLE),
        IMAGE_BITMAP,
        0,
        0,
        LR_LOADMAP3DCOLORS);

    // Copy bitmap and transparency mask to clipboard.
    ::OpenClipboard(NULL);
    ::EmptyClipboard();
    CCOMAddinUtil::CopyTransBitmap(hBmp);
    ::CloseClipboard();
    ::DeleteObject(hBmp);

    spCmdButton->put_Style(Office::msoButtonIcon);

    // Paste the bitmap from the clipboard onto the button.
    hr = spCmdButton->PasteFace();
    if (FAILED(hr)) 
	{
		EmptyClipbook();
		return hr; 
	}

    spCmdButton->put_Visible(VARIANT_TRUE);

    m_spMetreosCircleButton = spCmdButton;

    spCmdBarControl = 0;
    vToolBarType = Office::msoControlPopup;
    // Add a control for the first button to the toolbar.
    hr = spBarControls->Add(vToolBarType, vEmpty, vEmpty, vEmpty, vShow, &spCmdBarControl);
    if (FAILED(hr)) 
	{
		EmptyClipbook();
		return hr; 
	}
    ATLASSERT(spCmdBarControl);

    CComPtr<Office::CommandBarPopup> spCmdBarPopup;
    spCmdBarPopup = spCmdBarControl;
    ATLASSERT(spCmdBarPopup);

    spCmdBarPopup->put_Caption(METREOS_TEXT);
    spCmdBarPopup->put_TooltipText(METREOS_TOOL_TIP);

    m_spMetreosPopupButton = spCmdBarPopup;

    spBarControls = 0;
    hr = spCmdBarPopup->get_Controls(&spBarControls);
    if (FAILED(hr)) 
	{
		EmptyClipbook();
		return hr; 
	}

    ATLASSERT(spBarControls);

    vToolBarType = Office::msoControlButton;

    spCmdBarControl = 0;
    hr = spBarControls->Add(vToolBarType, vEmpty, vEmpty, vEmpty, vShow, &spCmdBarControl);
    if (FAILED(hr)) 
	{
		EmptyClipbook();
		return hr; 
	}

    ATLASSERT(spCmdBarControl);

    spCmdButton = spCmdBarControl;
    ATLASSERT(spCmdButton);

    spCmdButton->put_Enabled(VARIANT_TRUE);
    spCmdButton->put_Visible(VARIANT_TRUE);
    spCmdButton->put_Style(Office::msoButtonCaption);
    spCmdButton->put_Caption(SETTINGS_BUTTON_TEXT);
    m_spSettingsButton = spCmdButton;

	EmptyClipbook();
#else
    CComQIPtr<Office::_CommandBarButton> spCmdButton(spCmdBarControl); 
    spCmdButton->put_Style(Office::msoButtonIcon);

    HBITMAP hBmp = ::LoadBitmap(
        _AtlModule.GetResourceInstance(),
        MAKEINTRESOURCE(IDB_BMP_METREOSCIRCLE));

    PICTDESC pdBmp; 
    pdBmp.cbSizeofstruct = sizeof(PICTDESC); 
    pdBmp.picType = PICTYPE_BITMAP; 
    pdBmp.bmp.hbitmap = hBmp; 
    pdBmp.bmp.hpal = 0; 

    IPictureDisp* pPic;    
    hr = OleCreatePictureIndirect(&pdBmp, IID_IPictureDisp, TRUE, (void**)&pPic);  
    spCmdButton->put_Picture(pPic); 
    pPic->Release();  

    spCmdButton->put_Visible(VARIANT_TRUE);

    m_spMetreosCircleButton = spCmdButton;

    spCmdBarControl = 0;
    vToolBarType = Office::msoControlPopup;
    // Add a control for the first button to the toolbar.
    hr = spBarControls->Add(vToolBarType, vEmpty, vEmpty, vEmpty, vShow, &spCmdBarControl);
    if (FAILED(hr)) 
		return hr; 
    ATLASSERT(spCmdBarControl);

    CComPtr<Office::CommandBarPopup> spCmdBarPopup;
    spCmdBarPopup = spCmdBarControl;
    ATLASSERT(spCmdBarPopup);

    spCmdBarPopup->put_Caption(METREOS_TEXT);
    spCmdBarPopup->put_TooltipText(METREOS_TOOL_TIP);

    m_spMetreosPopupButton = spCmdBarPopup;

    spBarControls = 0;
    hr = spCmdBarPopup->get_Controls(&spBarControls);
    if (FAILED(hr)) 
		return hr; 

    ATLASSERT(spBarControls);

    vToolBarType = Office::msoControlButton;

    spCmdBarControl = 0;
    hr = spBarControls->Add(vToolBarType, vEmpty, vEmpty, vEmpty, vShow, &spCmdBarControl);
    if (FAILED(hr)) 
		return hr; 

    ATLASSERT(spCmdBarControl);

    spCmdButton = spCmdBarControl;
    ATLASSERT(spCmdButton);

    spCmdButton->put_Enabled(VARIANT_TRUE);
    spCmdButton->put_Visible(VARIANT_TRUE);
    spCmdButton->put_Style(Office::msoButtonCaption);
    spCmdButton->put_Caption(SETTINGS_BUTTON_TEXT);
    m_spSettingsButton = spCmdButton;
#endif
    return S_OK;
}

HRESULT CMetreosToolbar::AddDialButton()
{
    HRESULT hr;

    // Get the toolbar's CommandBarControls
    CComPtr <Office::CommandBarControls> spBarControls;
    hr = m_spCommandBar->get_Controls(&spBarControls);
    if (FAILED(hr)) { return hr; }
    ATLASSERT(spBarControls);

    CComVariant vTemp(VARIANT_TRUE);	
    CComVariant vEmpty(DISP_E_PARAMNOTFOUND, VT_ERROR);	
    CComVariant vToolBarType(Office::msoControlButton); // Toolbar control is a button.
    CComVariant vShow(VARIANT_TRUE);                    // Show the toolbar?

    CComPtr <Office::CommandBarControl> spCmdBarControl; 

    // Add a control for the first button to the toolbar.
    hr = spBarControls->Add(vToolBarType, vEmpty, vEmpty, vEmpty, vShow, &spCmdBarControl);
    if (FAILED(hr)) { return hr; }
    ATLASSERT(spCmdBarControl);

    // Get a _CommandBarButton interface for each toolbar button
    // so we can specify button styles and other properties.
    // Each button displays a bitmap with text next to it.
    CComQIPtr <Office::_CommandBarButton> spCmdButton(spCmdBarControl);
    ATLASSERT(spCmdButton);

    // Set the button style before giving it the bitmap.
    spCmdButton->put_Style(Office::msoButtonIconAndCaption);
    spCmdButton->put_FaceId(568);
    spCmdButton->put_Visible(VARIANT_TRUE); 
    spCmdButton->put_Caption(DIAL_BUTTON_TEXT);
    spCmdButton->put_Enabled(VARIANT_TRUE);
    spCmdButton->put_TooltipText(DIAL_BUTTON_TOOL_TIP); 

    // Save the button as a member variable.
    m_spDialButton = spCmdButton;

    return S_OK;
}

int CMetreosToolbar::FindCommandBar(const CComPtr<Office::_CommandBars>& spCmdBars) const
{
    CComBSTR bstrName;
    HRESULT hr;
    int cmdBarCount;

    spCmdBars->get_Count(&cmdBarCount);

    for(int i = 1; i <= cmdBarCount; i++)
    {   
        CComPtr<Office::CommandBar> cmdBar;

        hr = spCmdBars->get_Item(CComVariant(i), &cmdBar);
        ATLASSERT(cmdBar);

        cmdBar->get_Name(&bstrName);

        if(bstrName == COMMAND_BAR_NAME)
        {
            return i;
        }
    }

    return -1;
}

void CMetreosToolbar::ClearDialParticipants()
{
    DialParticipant_list_iterator i;
    for(i = m_dialParticipants.begin(); i != m_dialParticipants.end(); i++)
    {
        if(*i != 0)
        {
            delete *i;
        }
    }

    m_dialParticipants.clear();
}

CComPtr<IDispatch> CMetreosToolbar::GetDialButtonObj() const
{
    return CComQIPtr<IDispatch>(m_spDialButton);
}

CComPtr<IDispatch> CMetreosToolbar::GetSettingsButtonObj() const
{
    return CComQIPtr<IDispatch>(m_spSettingsButton);
}

void CMetreosToolbar::EmptyClipbook()
{
    ::OpenClipboard(NULL);
    ::EmptyClipboard();
    ::CloseClipboard();
}

////////////////////////////////////////////////////////////
// Event Handlers
////////////////////
void __stdcall CMetreosToolbar::OnClickDialButton(IDispatch* Ctrl, VARIANT_BOOL* CancelDefault)
{
    USES_CONVERSION;
    CComQIPtr<Office::_CommandBarButton> pCommandBarButton(Ctrl);

    SpawnDialDialog();
}

void __stdcall CMetreosToolbar::OnClickSettingsButton(IDispatch* Ctrl, VARIANT_BOOL* CancelDefault)
{
    USES_CONVERSION;
    CComQIPtr<Office::_CommandBarButton> pCommandBarButton(Ctrl);

    CMetreosPropPageDialog dlg;
    dlg.DoModal();
}
