#include "StdAfx.h"
#include "ctiline.h"
#include "ctibutton.h"
#include "ctiline.h"
#include "CTITimer.h"
#include <algorithm>
#include "CTIUtils.h"
#include "CTIConstants.h"
#include <algorithm>
#include "CTIUserInterface.h"

CCTILine::CCTILine(CCTIUserInterface* pUI,int nLineNumber, bool bFixed)
:m_pUI(pUI),
m_nLineNumber(nLineNumber),
m_bFixed(bFixed),
m_pCallDuration(NULL),
m_pHoldDuration(NULL)
{
	SetVisible(true);
	//We'll set this attribute for XML generation later on
	SetAttribute(KEY_LINE_NUMBER,nLineNumber);
	SetState(LINE_OPEN);

	SetAllowAlternate(false);
	SetAllowDialpad(false);

	m_dialpad.SetVisible(false);
	AddDefaultButtons();

	m_pCallDuration = new CCTITimer(KEY_CALL_DURATION);
	m_pHoldDuration = new CCTITimer(KEY_HOLD_DURATION);
}

CCTILine::~CCTILine(void)
{
	for (ButtonMap::iterator itButton = m_mapButtons.begin();itButton!=m_mapButtons.end();itButton++) {
		CCTIButton* pButton = itButton->second;
		delete pButton;
	}
}

MSXML2::IXMLDOMDocumentFragmentPtr CCTILine::SerializeToXML(MSXML2::IXMLDOMDocumentPtr pXMLDoc) 
{
	MSXML2::IXMLDOMDocumentFragmentPtr pFragment = pXMLDoc->createDocumentFragment();
	if (GetVisible()) {
		MSXML2::IXMLDOMElementPtr pXmlLine= pXMLDoc->createElement("CTILine");

		AddAttributesToElement(pXMLDoc,pXmlLine);

		AddChildIfVisible(pXMLDoc,pXmlLine,&m_lineStatus);

		for (PartyList::iterator it=m_listParties.begin();it!=m_listParties.end();it++) {
			CCTIParty* pParty = *it;
			AddChildIfVisible(pXMLDoc, pXmlLine, pParty);
		}

		m_pCallDuration->Update();
		AddChildIfVisible(pXMLDoc, pXmlLine, m_pCallDuration);
		m_pHoldDuration->Update();
		AddChildIfVisible(pXMLDoc, pXmlLine, m_pHoldDuration);

		//We've got to output the buttons in the proper order; thankfully, std::map is sorted by key (it's actually a tree, not a hashmap).
		for (ButtonMap::iterator itButton = m_mapButtons.begin();itButton!=m_mapButtons.end();itButton++) {
			CCTIButton* pButton = itButton->second;
			AddChildIfVisible(pXMLDoc, pXmlLine, pButton);
		}

		AddChildIfVisible(pXMLDoc, pXmlLine, &m_dialpad);

		pFragment->appendChild(pXmlLine);
		pXmlLine.Release();
	}
	return pFragment;
}

void CCTILine::SetState(int nLineState)
{
	m_nLineState = nLineState;
	SetAttribute(KEY_STATE,GetStringFromState(nLineState));
	if (nLineState!=LINE_ON_HOLD) {
		EndHoldTimer();
	}
}

int CCTILine::GetState(void)
{
	return m_nLineState;
}

std::wstring CCTILine::GetStringFromState(int nLineState)
{
	switch (nLineState) 
	{
		case LINE_OPEN:
			return KEY_OPEN;
		case LINE_RINGING:
			return KEY_RINGING;
		case LINE_ON_CALL:
			return KEY_ON_CALL;
		case LINE_DIALING:
			return KEY_DIALING;
		case LINE_ON_HOLD:
			return KEY_ON_HOLD;
	}

	return L"";
}

void CCTILine::AddDefaultButtons()
{
	AddLongButton(BUTTON_ANSWER,KEY_ANSWER,COLOR_GREEN);	
	AddLongButton(BUTTON_REJECT,KEY_REJECT,COLOR_RED);
	AddLongButton(BUTTON_RELEASE,KEY_RELEASE,COLOR_RED);	
	AddLongButton(BUTTON_ACCEPT_TRANSFER,KEY_ACCEPT_TRANSFER,COLOR_GREEN);	
	AddLongButton(BUTTON_COMPLETE_TRANSFER,KEY_COMPLETE_TRANSFER,COLOR_GREEN);	
	AddLongButton(BUTTON_ACCEPT_CONFERENCE,KEY_ACCEPT_CONFERENCE,COLOR_GREEN);
	AddLongButton(BUTTON_COMPLETE_CONFERENCE,KEY_COMPLETE_CONFERENCE,COLOR_GREEN);
	AddLongButton(BUTTON_RETRIEVE,KEY_RETRIEVE,COLOR_BEIGE);
	AddShortButton(BUTTON_HOLD,KEY_HOLD,L"/img/btn_hold.gif");		
	AddShortButton(BUTTON_TRANSFER,KEY_TRANSFER,L"/img/btn_transfer.gif");	
	AddShortButton(BUTTON_CONFERENCE,KEY_CONFERENCE,L"/img/btn_conference.gif");
	AddShortButton(BUTTON_NEW_LINE,KEY_NEW_LINE,L"/img/btn_newline.gif");
}

void CCTILine::EnableButtons(std::list<int>& listButtonIds)
{
	//Set only the buttons in the list to visible, and all others to invisible
	for (ButtonMap::iterator itButton = m_mapButtons.begin();itButton!=m_mapButtons.end();itButton++) {
		int nButtonId = itButton->first;
		CCTIButton* pButton = itButton->second;
		if (pButton) {
			bool bVisible = find(listButtonIds.begin(),listButtonIds.end(),nButtonId)!=listButtonIds.end();
			pButton->SetVisible(bVisible);
		}
	}
}

void CCTILine::ShowDialpad(int nDialpadType, bool bAllowOneStep)
{
	m_dialpad.SetType(nDialpadType);
	m_dialpad.SetAllowOneStep(bAllowOneStep);
	m_dialpad.SetVisible(true);
	HideLineStatusBar();
}

void CCTILine::HideDialpad()
{
	m_dialpad.SetVisible(false);
	HideLineStatusBar();
}

void CCTILine::AddButton(int nId, CCTIButton* pButton)
{
	// remove duplicates
	RemoveButton(nId);
	m_mapButtons[nId] = pButton;
}

void CCTILine::RemoveButton(int nId) {
	if (m_mapButtons.find(nId)!=m_mapButtons.end()) {
		CCTIButton* pButton = m_mapButtons[nId];
		m_mapButtons.erase(nId);
		delete pButton;
	}
}

CCTIButton* CCTILine::AddShortButton(int nId, std::wstring sMessage, std::wstring sIconURL, std::wstring sLabel)
{
	CCTIButton* pButton = new CCTIButton(sMessage,sLabel);
	pButton->SetLongStyle(false);
	pButton->SetIconURL(sIconURL);
	m_mapButtons[nId] = pButton;

	return pButton;
}

CCTIButton* CCTILine::AddLongButton(int nId, std::wstring sMessage, std::wstring sColor, std::wstring sLabel)
{
	CCTIButton* pButton = new CCTIButton(sMessage,sLabel);
	pButton->SetLongStyle(true);
	pButton->SetColor(sColor);
	m_mapButtons[nId] = pButton;

	return pButton;
}

void CCTILine::StartCallDurationTimer()
{
	if (m_pCallDuration) {
		m_pCallDuration->SetVisible(true);
		m_pCallDuration->Start();
	}
}

void CCTILine::StartHoldTimer()
{
	if (m_pHoldDuration) {
		m_pHoldDuration->SetVisible(true);
		m_pHoldDuration->Start();
	}
}

void CCTILine::EndCallDurationTimer()
{
	if (m_pCallDuration) {
		m_pCallDuration->Reset();
		m_pCallDuration->SetVisible(false);
	}
}

void CCTILine::EndHoldTimer()
{
	if (m_pHoldDuration) {
		m_pHoldDuration->Reset();
		m_pHoldDuration->SetVisible(false);
	}
}

void CCTILine::Reset()
{
	SetState(LINE_OPEN);
	m_listParties.clear();
	EndCallDurationTimer();
	//Just in case...
	EndHoldTimer();
	SetAllowDialpad(false);
	SetCallObjectId(L"");
}

int CCTILine::GetCallDurationSeconds(void)
{
	return m_pCallDuration->GetTotalSeconds();
}

void CCTILine::SetCallObjectId(std::wstring sCallObjectId) {
	m_sCallObjectId = sCallObjectId;
	if (!m_sCallObjectId.empty()) {
		SetAttribute(KEY_CALL_OBJECT_ID,sCallObjectId);
	} else {
		RemoveAttribute(KEY_CALL_OBJECT_ID);
	}
}

void CCTILine::ShowLineStatusBar(bool bError, std::wstring sLineStatusId, std::wstring sLineStatusLabel)
{
	m_lineStatus.SetId(sLineStatusId);
	m_lineStatus.SetLabel(sLineStatusLabel);
	m_lineStatus.SetError(bError);
	m_lineStatus.SetVisible(true);

	m_pUI->UIHideStatusBar();	
}

void CCTILine::HideLineStatusBar()
{
	m_lineStatus.SetVisible(false);
}

void CCTILine::AddParty(CCTIParty* pParty, int nLocation)
{
	if (nLocation<1 || (UINT)nLocation>=m_listParties.size()) {
		m_listParties.push_back(pParty);
	} else {
		int nPosition = 1;
		for (PartyList::iterator it=m_listParties.begin();it!=m_listParties.end();it++) {
			if (nLocation==nPosition) {
				m_listParties.insert(it,pParty);
				break;
			} else nPosition++;
		}
	}
}

void CCTILine::RemoveAllParties()
{
	m_listParties.clear();
}


void CCTILine::RemoveParty(CCTIParty* pParty)
{
	PartyList::iterator it = find(m_listParties.begin(),m_listParties.end(),pParty);
	if (it!=m_listParties.end()) m_listParties.erase(it);
}

void CCTILine::RemovePartyByANI(std::wstring& sANI)
{
	CCTIParty* pRemoveParty = NULL;
	for (PartyList::iterator it=m_listParties.begin();it!=m_listParties.end();it++) 
	{
		CCTIParty* pParty = *it;
		if (pParty->GetANI()==sANI) {
			pRemoveParty = pParty;
			break;
		}
	}
	
	if (pRemoveParty!=NULL) RemoveParty(pRemoveParty);
}

CCTIParty* CCTILine::GetFirstParty()
{
	return GetParty(1);
}

CCTIParty* CCTILine::GetParty(int nParty)
{
	int nIndex = 0;
	for (PartyList::iterator it=m_listParties.begin();it!=m_listParties.end();it++) 
	{
		nIndex++;
		if (nIndex==nParty) {
			return *it;
		}
	}
	return NULL;
}

CCTIParty* CCTILine::GetPartyByANI(std::wstring sANI)
{
	for (PartyList::iterator it=m_listParties.begin();it!=m_listParties.end();it++) 
	{
		CCTIParty* pParty = *it;
		if (pParty->GetANI()==sANI) {
			return pParty;
		}
	}
	return NULL;
}
