#include "StdAfx.h"
#include "ctiuserinterface.h"
#include "CTIAdapter.h"
#include "CTIAgent.h"
#include "CTILine.h"
#include "CTIReasonCodeSet.h"
#include "CTIAppExchange.h"
#include "CTIForm.h"
#include "CTIButton.h"
#include <fstream>
#include "CTIUtils.h"

CCTIUserInterface::CCTIUserInterface(CCTIAdapter* pAdapter, int nNumberOfFixedLines)
:m_bConnected(false),
 m_bLoggedIn(false),
 m_pAdapter(pAdapter),
 m_pCTIAgent(NULL),
 m_nNumberOfFixedLines(nNumberOfFixedLines),
 m_sAgentState(AGENTSTATE_LOGOUT),
 m_bEnableOneStepTransfer(false),
 m_bEnableOneStepConference(false),
 m_pReasonCodeSet(NULL),
 m_bNotReadyReasonRequired(false),
 m_bLogoutReasonRequired(false),
 m_pLoginForm(NULL),
 m_bAutoReconnect(true),
 m_pAppExchange(NULL),
 m_bQueuedLogout(false),
 m_bAutoLogin(false),
 m_bQueuedLogin(false),
 m_bShowWrapupSaveButton(false),
 m_bUseAsynchronousSearch(true),
 m_bWrapupCodeRequired(true)
{
	CCTILogger::CreateLogger(LOGTYPE_CTI_CONNECTOR);
	m_statusBar.SetVisible(false);
	SetLoggedIn(false);
}

CCTIUserInterface::~CCTIUserInterface(void)
{
	DestroyChildren();

	for (AniPartyMap::iterator itMap=m_mapANIToParties.begin();itMap!=m_mapANIToParties.end();itMap++)
	{
		CCTIParty* pParty = itMap->second;
		delete pParty;
	}
	m_mapANIToParties.clear();

	for (CallLogList::iterator itLog=m_listCallLogs.begin();itLog!=m_listCallLogs.end();itLog++)
	{
		CCTICallLog* pLog = *itLog;
		delete pLog;
	}
	m_listCallLogs.clear();

	delete m_pPreviousCalls;
}

void CCTIUserInterface::Initialize()
{
	//Synchronize this method
	AutoLock autoLock(this);

	m_pLoginForm = CreateLoginForm();
	m_pLoginForm->SetVisible(false);

	m_pCTIAgent = new CCTIAgent();
	m_pCTIAgent->SetVisible(false);

	//Create all the fixed lines
	for (int nLine = 1;nLine<=GetNumberOfFixedLines();nLine++) {
		CCTILine* pLine = new CCTILine(this,nLine);
		pLine->SetVisible(false);
		m_listLines.push_back(pLine);
	}

	m_pReasonCodeSet = new CCTIReasonCodeSet();
	m_pReasonCodeSet->SetVisible(false);

	m_pPreviousCalls = new CCTIPreviousCalls();
}

bool CCTIUserInterface::CTIIsOccupiedAgentState(std::wstring sAgentState)
{
	return (sAgentState==AGENTSTATE_BUSY || sAgentState == AGENTSTATE_WRAPUP);
} 

void CCTIUserInterface::UIClearCallLogs()
{
	for (CallLogList::iterator itLog=m_listCallLogs.begin();itLog!=m_listCallLogs.end();itLog++)
	{
		CCTICallLog* pLog = *itLog;
		delete pLog;
	}
	m_listCallLogs.clear();

	m_pPreviousCalls->Reset();
}

CCTIAppExchange* CCTIUserInterface::GetAppExchange() {
	if (m_pAppExchange==NULL) {
		m_pAppExchange=new CCTIAppExchange(this);
	}
	
	return m_pAppExchange;
}

void CCTIUserInterface::UIParseIncomingXMLMessage(BSTR xmlMessage) 
{
	MSXML2::IXMLDOMDocumentPtr pXMLDoc;
	HRESULT hr = pXMLDoc.CreateInstance(__uuidof(MSXML2::DOMDocument60));
	if (FAILED(hr)) 
	{
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIUserInterface::UIParseIncomingXMLMessage: Failed to CreateInstance on an XML DOM");
		return;
	}
	
	if (pXMLDoc->loadXML(xmlMessage) == VARIANT_TRUE)
	{
		MSXML2::IXMLDOMNodePtr pNode;
		// Query the xmlMessage node.
		pNode = pXMLDoc->selectSingleNode(L"//MESSAGE");
		if (pNode!=NULL)
		{
			PARAM_MAP parameters;

			MSXML2::IXMLDOMElementPtr pElement = static_cast<MSXML2::IXMLDOMElementPtr>(pNode);
			_variant_t vMessageId = pElement->getAttribute(KEY_ID);
			std::wstring sMessageId = CCTIUtils::VariantToString(vMessageId);

			// Query a node-set.
			MSXML2::IXMLDOMNodeListPtr pParamNodeList = pXMLDoc->selectNodes(L"//PARAMETER");
			for (int i=0; i<pParamNodeList->length; i++)
			{
				pNode = pParamNodeList->item[i];
				pElement = static_cast<MSXML2::IXMLDOMElementPtr>(pNode);
				_variant_t vParamName = pElement->getAttribute(KEY_NAME);
				std::wstring sParamName = CCTIUtils::VariantToString(vParamName);
				_variant_t vParamValue = pElement->getAttribute(KEY_VALUE);
				std::wstring sParamValue = CCTIUtils::VariantToString(vParamValue);

				parameters[sParamName]=sParamValue;
			}

			UIHandleMessage(sMessageId,parameters);

			pParamNodeList.Release();
			pElement.Release();
		}

		pNode.Release();
	}

	pXMLDoc.Release();
}

void CCTIUserInterface::UIRefresh()
{
	AutoLock autoLock(this);

	MSXML2::IXMLDOMDocumentPtr pXMLDoc;
	HRESULT hr = pXMLDoc.CreateInstance(__uuidof(MSXML2::DOMDocument60));
	if (FAILED(hr)) 
	{
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIUserInterface::UIRefresh: Failed to CreateInstance on an XML DOM");
		return;
	}
	//This call to SerializeToXML will cascade down to all the children of this object.
	MSXML2::IXMLDOMDocumentFragmentPtr xmlUi = SerializeToXML(pXMLDoc);
	pXMLDoc->appendChild(xmlUi);
	xmlUi.Release();

	m_pAdapter->SendUIRefreshEvent(pXMLDoc->xml);
	pXMLDoc.Release();
}

void CCTIUserInterface::UIHandleMessage(std::wstring& message, PARAM_MAP& parameters)
{
	if (CCTILogger::GetLogLevel()>=LOGLEVEL_HIGH) {
		std::wstring sParams = L"CCTIUserInterface::UIHandleMessage: Message received: %s.  Parameters: ";
		//Log the params also
		for (PARAM_MAP::iterator it=parameters.begin();it!=parameters.end();it++) {
			sParams+=it->first;
			sParams+=L"=";
			sParams+=it->second;
			sParams+=L"; ";
		}
		CCTILogger::Log(LOGLEVEL_HIGH,sParams.c_str(),message);
	} else {
		CCTILogger::Log(LOGLEVEL_MED,L"CCTIUserInterface::UIHandleMessage: Message received: %s.",message);
	}

	if (message==KEY_EXIT) {
		CTIDisconnect(parameters);
	} else if (message==KEY_UPDATE_SID) {
		UIUpdateSid(parameters);
	} else if (message==KEY_CONNECT) {
		CTIConnect(parameters);
	}  else if (message==KEY_LOGIN) {
		CTILogin(parameters);
	} else if (message==KEY_DIAL) {
		QueueCallInitiate(parameters);
	} else if (message==KEY_RELEASE) {
		CallRelease(parameters);
	} else if (message==KEY_ANSWER) {
		CallAnswer(parameters);
	} else if (message==KEY_HOLD) {
		CallHold(parameters);
	} else if (message==KEY_RETRIEVE) {
		CallRetrieve(parameters);
	} else if (message==KEY_ALTERNATE) {
		CallAlternate(parameters);
	} else if (message==KEY_AGENT_STATE) {
		QueueCTIChangeAgentState(parameters);
	} else if (message==KEY_TRANSFER) {
		//Open the transfer dialpad
		std::wstring sLineNumber = parameters[KEY_LINE_NUMBER];
		int nLineNumber = CCTIUtils::StringToInt(sLineNumber);
		UIShowDialpad(nLineNumber, DIALPAD_TRANSFER,true);
	} else if (message==KEY_CONFERENCE) {
		//Open the conference dialpad
		std::wstring sLineNumber = parameters[KEY_LINE_NUMBER];
		int nLineNumber = CCTIUtils::StringToInt(sLineNumber);
		UIShowDialpad(nLineNumber, DIALPAD_CONFERENCE,true);
	} else if (message==KEY_TOGGLE_DIALPAD || message==KEY_NEW_LINE) {
		//Open the dial dialpad
		std::wstring sLineNumber = parameters[KEY_LINE_NUMBER];
		int nLineNumber = CCTIUtils::StringToInt(sLineNumber);
		CCTILine* pLine = GetLine(nLineNumber);
		if (pLine->GetDialpad() && pLine->GetDialpad()->GetVisible()) {
			UIHideDialpad(nLineNumber, message==KEY_TOGGLE_DIALPAD ? DIALPAD_DIAL : DIALPAD_NEW_LINE,true);
		} else {
			UIShowDialpad(nLineNumber, message==KEY_TOGGLE_DIALPAD ? DIALPAD_DIAL : DIALPAD_NEW_LINE,true);
		}
	} else if (message==KEY_INIT_TRANSFER) {
		CallInitiateTransfer(parameters);
	} else if (message==KEY_INIT_CONFERENCE) {
		CallInitiateConference(parameters);
	} else if (message==KEY_SS_TRANSFER) {
		CallOneStepTransfer(parameters);
	} else if (message==KEY_SS_CONFERENCE) {
		CallOneStepConference(parameters);
	} else if (message==KEY_COMPLETE_TRANSFER) {
		CallCompleteTransfer(parameters);
	} else if (message==KEY_COMPLETE_CONFERENCE) {
		CallCompleteConference(parameters);
	} else if (message==KEY_CANCEL_TRANSFER) {
		//Open the transfer dialpad
		std::wstring sLineNumber = parameters[KEY_LINE_NUMBER];
		int nLineNumber = CCTIUtils::StringToInt(sLineNumber);
		UIHideDialpad(nLineNumber, DIALPAD_TRANSFER,true);
	} else if (message==KEY_CANCEL_CONFERENCE) {
		//Open the conference dialpad
		std::wstring sLineNumber = parameters[KEY_LINE_NUMBER];
		int nLineNumber = CCTIUtils::StringToInt(sLineNumber);
		UIHideDialpad(nLineNumber, DIALPAD_CONFERENCE,true);
	} else if (message==KEY_CANCEL_DIAL) {
		//Open the conference dialpad
		std::wstring sLineNumber = parameters[KEY_LINE_NUMBER];
		int nLineNumber = CCTIUtils::StringToInt(sLineNumber);
		UIHideDialpad(nLineNumber, DIALPAD_DIAL,true);
	} else if (message==KEY_CANCEL_NEW_LINE) {
		//Open the conference dialpad
		std::wstring sLineNumber = parameters[KEY_LINE_NUMBER];
		int nLineNumber = CCTIUtils::StringToInt(sLineNumber);
		UIHideDialpad(nLineNumber, DIALPAD_NEW_LINE,true);
	} else if (message==KEY_REASON_CODE_WRAPUP) {
		CallSetWrapupCode(parameters);
	} else if (message==KEY_REASON_CODE_NOT_READY) {
		PARAM_MAP reasonParams;
		reasonParams[KEY_ID]=KEY_NOT_READY;
		reasonParams[KEY_REASON_CODE]=parameters[KEY_ID];
		CTIChangeAgentState(reasonParams);
	} else if (message==KEY_REASON_CODE_LOGOUT) {
		PARAM_MAP reasonParams;
		reasonParams[KEY_ID]=KEY_LOGOUT;
		reasonParams[KEY_REASON_CODE]=parameters[KEY_ID];
		CTIChangeAgentState(reasonParams);
	} else if (message==KEY_UPDATE_XML) {
		//This message is to test the XML generator in action
		UIRefresh();
	} else if (message==KEY_UPDATE_LOG_SETTINGS) {
		std::wstring sLogLevel = parameters[KEY_LOGLEVEL];
		int nLogLevel = CCTIUtils::StringToInt(sLogLevel);
		
		CCTILogger::SetLogLevel(nLogLevel);
		std::wstring sBrowserConnectorFile = parameters[KEY_BROWSER_CONNECTOR_FILE];
		std::wstring sCtiConnectorFile = parameters[KEY_CTI_CONNECTOR_FILE];

		CCTILogger::SetLogFilePaths(sBrowserConnectorFile, sCtiConnectorFile);
	} else if (message==KEY_UPDATE_COMMENTS) {
		CallUpdateComments(parameters);
	} else if (message==KEY_CLICK_TO_DIAL) {
		CallClickToDial(parameters);
	} else if (message==KEY_ADD_LOG_OBJECT) {
		UIAddLogObject(parameters);
	} else if (message==KEY_SELECT_LOG_OBJECT) {
		CallSelectLogObject(parameters);
	} else if (message==KEY_TOGGLE_TWISTY) {
		UIToggleTwisty(parameters);
	} else if (message==KEY_SAVE_WRAPUP) {
		CallSaveWrapup();
	}
}

void CCTIUserInterface::CallClickToDial(PARAM_MAP& parameters)
{
	std::wstring sDN = parameters[KEY_DN];
	if (sDN.empty()) {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIUserInterface::CallClickToDial: Error: empty DN.");
		return;
	}

	QueueCallInitiate(parameters);
}

void CCTIUserInterface::OnCTIConnection()
{
	CCTILogger::Log(LOGLEVEL_MED,L"OnCTIConnection called.");
	m_bConnected = true;
	m_pLoginForm->PopulateFormData(m_mapUserParams);

	UIHideProgressBar();
	UIHideStatusBar();

	if (m_bQueuedLogin) {
		m_bQueuedLogin = false;
		CTILogin(m_mapUserParams);
	} else if (GetAutoLogin() && !m_mapUserParams.empty()) {
		CCTILogger::Log(LOGLEVEL_MED,L"CCTIUserInterface::OnCTIConnection: Autologin is true.  Automatically logging in.");
		CTILogin(m_mapUserParams);
	} else {
		m_pLoginForm->SetVisible(true);
	}

	UIRefresh();
}

void CCTIUserInterface::OnButtonEnablementChange(std::list<int>& listEnabledButtons, bool bSendUpdatedXML)
{
	//Synchronize this method
	AutoLock autoLock(this);

	for(LineList::iterator it = m_listLines.begin();it!=m_listLines.end();it++) {
		CCTILine* pLine = *it;
		if (pLine->GetFixed()) {
			pLine->EnableButtons(listEnabledButtons);
			break;
		}
	}

	if (bSendUpdatedXML) UIRefresh();
}

void CCTIUserInterface::OnButtonEnablementChange(int nLine, std::list<int>& listEnabledButtons, bool bSendUpdatedXML)
{
	CCTILine* pLine = GetLine(nLine);
	if (pLine) {
		pLine->EnableButtons(listEnabledButtons);
	}
	
	if (bSendUpdatedXML) UIRefresh();
}

void CCTIUserInterface::OnAgentStateEnablementChange(std::list<std::wstring>& listEnabledAgentStates, std::wstring& sAgentState) {
	m_pCTIAgent->SetAgentState(sAgentState);
	m_pCTIAgent->EnableAgentStates(listEnabledAgentStates);
}

void CCTIUserInterface::OnCTIConnectionFailed(std::wstring sErrorMessage)
{
	CCTILogger::Log(LOGLEVEL_LOW,L"CCTIUserInterface::OnCTIConnectionFailed: Connection failure.");
	SetChildrenVisible(false,false,false,false);
	//If sErrorMessage is empty then it'll just use the standard error message, which corresponds to the "CONNECTION_FAILED" ID
	UIShowStatusBar(true,KEY_CONNECTION_FAILED,sErrorMessage);
	UIRefresh();

	//Sleep for a couple of seconds, then try to reconnect
	Sleep(2000);
	UIShowProgressBar(KEY_RECONNECTING);
	m_bConnected = false;
	if (GetAutoReconnect()) {
		CTIConnect(m_mapConnectionParams);
	}
	UIRefresh();
}

void CCTIUserInterface::OnCTIConnectionClosed()
{
	CCTILogger::Log(LOGLEVEL_MED,L"CCTIUserInterface::OnCTIConnectionClosed: Connection closed.");
	m_bConnected = false;
	/*
	if (GetAutoReconnect()) {
		CTIConnect(m_mapConnectionParams);
	}
	*/
	UIRefresh();
}

void CCTIUserInterface::OnCallTransferred(std::wstring sCallObjectId)
{
	CCTILine* pLine = GetLineByCallId(sCallObjectId);
	if (pLine) {
		for (int nParty = 1; nParty<=pLine->GetNumberOfParties();nParty++) 
		{
			CCTIParty* pParty = pLine->GetParty(nParty);
			if (pParty->GetType()==PARTYTYPE_TRANSFERRED_FROM) {
				OnCallPartyDropped(sCallObjectId,pParty->GetANI());
			}
		}

		pLine->ShowLineStatusBar(false,KEY_CALL_TRANSFERRED);
	} else {
		UIShowStatusBar(false,KEY_CALL_TRANSFERRED);
	}
	UIRefresh();
}

void CCTIUserInterface::OnCallConferenced(std::wstring sCallObjectId,StringList listParties)
{
	CCTILine* pLine = GetLineByCallId(sCallObjectId);
	if (pLine) {
		pLine->RemoveAllParties();
		for (StringList::iterator it=listParties.begin();it!=listParties.end();it++) {
			std::wstring sPartyANI = *it;
			AniPartyMap::iterator itParty = m_mapANIToParties.find(sPartyANI);
			if (itParty!=m_mapANIToParties.end()) {
				CCTIParty* pParty = itParty->second;
				pParty->SetType(PARTYTYPE_CONFERENCED_PARTY);
				pLine->AddParty(pParty);
			} else {
				//We don't know who this is -- create a party for the ANI
				CCTIParty* pParty = CreateParty(sPartyANI,PARTYTYPE_CONFERENCED_PARTY);
				pLine->AddParty(pParty);
			}
		}
		//pLine->SetState(LINE_ON_CALL);
		pLine->ShowLineStatusBar(false,KEY_CALL_CONFERENCED);

		UIRefresh();
	}
}

int CCTIUserInterface::OnCallDialing(std::wstring sCallObjectId, PARAM_MAP& mapInfoFields, bool bPerformSearch, bool bLogCall, int nLineNumber)
{
	if (mapInfoFields[KEY_DNIS].empty()) {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIUserInterface::OnCallDialing: Warning: OnCallDialing was called with no DNIS info field.");
	}

	CCTILine* pLine = NULL;
	if (nLineNumber!=NULL) {
		pLine = GetLine(nLineNumber);
	} else {
		if (nLineNumber!=0) CCTILogger::Log(LOGLEVEL_MED,L"CCTIUserInterface::OnCallDialing: Specified line %d does not exist.  Creating virtual line.  Call ID %s.",nLineNumber,sCallObjectId);
	}

	if (!pLine) {
		CCTILogger::Log(LOGLEVEL_MED,L"CCTIUserInterface::OnCallDialing: Putting call on next available fixed line.");
		//Get the next open fixed line
		pLine = GetNextAvailableFixedLine();

		if (!pLine) {
			CCTILogger::Log(LOGLEVEL_MED,L"CCTIUserInterface::OnCallDialing: No available fixed lines, creating virtual line.");
			//No open fixed lines -- create a virtual line for it.
			pLine = CreateVirtualLine(sCallObjectId);
		}
	}


	if (pLine) {
		pLine->HideDialpad();
		pLine->HideLineStatusBar();
		pLine->SetCallObjectId(sCallObjectId);
		pLine->SetState(LINE_DIALING);
		//Since we're dialing, it's obviously outbound
		pLine->SetCallType(CALLTYPE_OUTBOUND);
		pLine->SetAllowDialpad(false);

		CCTIParty* pParty = CreateParty(mapInfoFields[KEY_DNIS]);

		StringList* pListLayoutInfoFields = GetAppExchange()->GetInfoFieldsForLayout(CALLTYPE_OUTBOUND);
		pParty->SetInfoFields(mapInfoFields,pListLayoutInfoFields);

		CCTICallLog* pLog = NULL;

		if (bLogCall && GetAppExchange()->CanCreateCallLogs()) 
		{
			pLog = CreateCallLog(pLine->GetLineNumber(),sCallObjectId,CALLTYPE_OUTBOUND);
		}

		if (bPerformSearch) {
			CCTILogger::Log(LOGLEVEL_MED,L"CCTIUserInterface::OnCallDialing: Performing search.");
			RelObjSetList listRelatedObjects;
			PARAM_MAP mapAttachedData;

			// if we got an entity id from this outbound call, use it for search
			if  (!m_sId.empty() && CCTIUtils::StringEndsWith(m_sLastDN, mapInfoFields[KEY_DNIS])) {
				mapAttachedData[m_sEntityName.append(L".Id")]=m_sId;
			}

			//Dialing is always the outbound layout
			if (GetUseAsynchronousSearch()) {
				GetAppExchange()->AsyncSearch(sCallObjectId,LAYOUT_OUTBOUND,mapInfoFields[KEY_DNIS],mapAttachedData);
			} else {
				GetAppExchange()->Search(LAYOUT_OUTBOUND,mapInfoFields[KEY_DNIS],mapAttachedData,pLog,listRelatedObjects);
				
				CCTILogger::Log(LOGLEVEL_MED,L"CCTIUserInterface::OnCallDialing: Found %d Related Object Sets.",(long)(listRelatedObjects.size()));
				pParty->AddRelatedObjectSets(listRelatedObjects);

				if (pLog)
				{
					pLog->AddRelatedObjects(&listRelatedObjects);
				}
			}
		}

		pLine->AddParty(pParty);

		UIRefresh();

		return pLine->GetLineNumber();
	} else {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIUserInterface::OnCallDialing: Could not find line to put call on!  Line %d, Call ID %s.",nLineNumber,sCallObjectId);
	}

	return 0;
}

int CCTIUserInterface::OnCallRinging(std::wstring sCallObjectId, int nCallType, bool bPerformSearch, bool bLogCall, PARAM_MAP& mapInfoFields, PARAM_MAP& mapAttachedData, int nLineNumber)
{
	if (mapInfoFields[KEY_ANI].empty() && mapAttachedData[KEY_ORIGINAL_ANI].empty()) {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIUserInterface::OnCallRinging: Warning: OnCallRinging was called with no ANI info field.");
	}

	CCTILogger::Log(LOGLEVEL_MED,L"CCTIUserInterface::OnCallRinging: Call ID %s, line %d.",sCallObjectId,nLineNumber);

	CCTILine* pLine = NULL;
	if (nLineNumber!=NULL) {
		pLine = GetLine(nLineNumber);
	} else {
		if (nLineNumber!=0) CCTILogger::Log(LOGLEVEL_MED,L"CCTIUserInterface::OnCallRinging: Specified line %d does not exist.  Creating virtual line.  Call ID %s.",nLineNumber,sCallObjectId);
	}

	if (!pLine) {
		CCTILogger::Log(LOGLEVEL_MED,L"CCTIUserInterface::OnCallRinging: Putting call on next available fixed line.");
		//Get the next open fixed line
		pLine = GetNextAvailableFixedLine();

		if (!pLine) {
			CCTILogger::Log(LOGLEVEL_MED,L"CCTIUserInterface::OnCallRinging: No available fixed lines, creating virtual line.");
			//No open fixed lines -- create a virtual line for it.
			pLine = CreateVirtualLine(sCallObjectId);
		}
	}

	if (pLine) {
		pLine->HideLineStatusBar();
		pLine->SetCallObjectId(sCallObjectId);
		pLine->SetState(LINE_RINGING);
		
		std::wstring sANI;
		std::wstring sTransferrerANI;
		if (mapAttachedData.find(KEY_ORIGINAL_ANI)!=mapAttachedData.end()) {
			//The original ANI may have been specified in the attached data if this is a transfer or conference
			//This ANI takes precedence.
			sANI = mapAttachedData[KEY_ORIGINAL_ANI];
			sTransferrerANI = mapInfoFields[KEY_ANI];
			//Reset the info field so it shows the original caller's ANI
			mapInfoFields[KEY_ANI] = sANI;
		} else if (mapInfoFields.find(KEY_ANI)!=mapInfoFields.end()) {
			sANI = mapInfoFields[KEY_ANI];
		}

		int nLayout = LAYOUT_INCOMING_INTERNAL;
		if (nCallType == CALLTYPE_INBOUND) {
			nLayout = LAYOUT_INCOMING_EXTERNAL;
		}

		CCTIParty* pParty = CreateParty(sANI);

		StringList* pListLayoutInfoFields = GetAppExchange()->GetInfoFieldsForLayout(nLayout);
		pParty->SetInfoFields(mapInfoFields,pListLayoutInfoFields);

		pLine->SetCallType(nCallType);
		pLine->SetAllowDialpad(false);
		
		CCTICallLog* pLog = NULL;
		if (bLogCall && GetAppExchange()->CanCreateCallLogs()) 
		{
			pLog = CreateCallLog(pLine->GetLineNumber(),sCallObjectId,nCallType);
		}

		if (bPerformSearch) {
			CCTILogger::Log(LOGLEVEL_MED,L"CCTIUserInterface::OnCallRinging: Performing search.");
			RelObjSetList listRelatedObjects;

			if (GetUseAsynchronousSearch()) {
				GetAppExchange()->AsyncSearch(sCallObjectId,nLayout,sANI,mapAttachedData);
			} else {
				GetAppExchange()->Search(nLayout,mapInfoFields[KEY_ANI],mapAttachedData,pLog,listRelatedObjects);
				
				CCTILogger::Log(LOGLEVEL_MED,L"CCTIUserInterface::OnCallRinging: Found %d Related Object Sets.",(long)(listRelatedObjects.size()));
				pParty->AddRelatedObjectSets(listRelatedObjects);

				if (pLog)
				{
					pLog->AddRelatedObjects(&listRelatedObjects);
				}
			}
		}

		pLine->AddParty(pParty);

		//Check if there's any transfers or conferences
		if (bPerformSearch && (!mapAttachedData[KEY_TRANSFERRED_FROM].empty() || !mapAttachedData[KEY_CONFERENCED_FROM].empty()))
		{
			CCTIParty* pTransferringParty = CreateParty(sTransferrerANI,mapAttachedData[KEY_TRANSFERRED_FROM].empty()?PARTYTYPE_CONFERENCED_FROM:PARTYTYPE_TRANSFERRED_FROM);
			
			CCTIInfoField* pInfoField = new CCTIInfoField(KEY_ANI);
			pInfoField->SetValue(sTransferrerANI);
			pTransferringParty->AddInfoField(pInfoField);

			//The transferror doesn't get the data attached to the call, just this map with the User ID in it...
			PARAM_MAP mapTransferAttachedData;
			mapTransferAttachedData[L"User.Id"] = mapAttachedData[KEY_TRANSFERRED_FROM].empty()?mapAttachedData[KEY_CONFERENCED_FROM]:mapAttachedData[KEY_TRANSFERRED_FROM];

			RelObjSetList listTransferRelatedObjects;
			//We assume an internal layout for transfers & conferences
			if (GetUseAsynchronousSearch()) {
				GetAppExchange()->AsyncSearch(sCallObjectId,LAYOUT_INCOMING_INTERNAL,sTransferrerANI,mapTransferAttachedData,false);
			} else {
				GetAppExchange()->Search(LAYOUT_INCOMING_INTERNAL,sTransferrerANI,mapTransferAttachedData,NULL,listTransferRelatedObjects,false);
				
				pTransferringParty->AddRelatedObjectSets(listTransferRelatedObjects);
			}

			pLine->AddParty(pTransferringParty);
		}

		UIRefresh();

		return pLine->GetLineNumber();
	} else {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIUserInterface::OnCallRinging: Could not find line to put call on!  Line %d, Call ID %s.",nLineNumber,sCallObjectId);
	}

	return 0;
}

void CCTIUserInterface::OnCallEstablished(std::wstring sCallObjectId)
{
	CCTILogger::Log(LOGLEVEL_MED,L"CCTIUserInterface::OnCallEstablished: Call ID %s.",sCallObjectId);
	CCTILine* pLine = GetLineByCallId(sCallObjectId);
	if (pLine) {
		pLine->HideLineStatusBar();
		pLine->SetState(LINE_ON_CALL);
		pLine->StartCallDurationTimer();

		CCTICallLog* pLog = GetCallLogForCallId(sCallObjectId);
		if (pLog) {
			//Enable the log for saving when the call is ended
			pLog->SetSaveOnEnd(true);
			pLog->SetVisible(true);
		}

		UIRefresh();
	} else {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIUserInterface::OnCallEstablished: Could not find line that call is on!  Call ID %s.",sCallObjectId);
	}
}

void CCTIUserInterface::OnCallHeld(std::wstring sCallObjectId)
{
	CCTILogger::Log(LOGLEVEL_MED,L"OnCallHeld for call ID %s.",sCallObjectId);
	CCTILine* pLine = GetLineByCallId(sCallObjectId);
	if (pLine) {
		pLine->HideLineStatusBar();
		pLine->SetState(LINE_ON_HOLD);
		pLine->StartHoldTimer();

		UIRefresh();
	} else {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIUserInterface::OnCallHeld: Could not find line that call is on!  Call ID %s.",sCallObjectId);
	}
}

void CCTIUserInterface::OnCallRetrieved(std::wstring sCallObjectId)
{
	CCTILogger::Log(LOGLEVEL_MED,L"OnCallRetrieved for call ID %s.",sCallObjectId);
	CCTILine* pLine = GetLineByCallId(sCallObjectId);
	if (pLine) {
		pLine->HideLineStatusBar();
		pLine->SetState(LINE_ON_CALL);
		pLine->EndHoldTimer();

		UIRefresh();
	} else {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIUserInterface::OnCallRetrieved: Could not find line that call is on!  Call ID %s.",sCallObjectId);
	}
}

void CCTIUserInterface::OnCallEnd(std::wstring sCallObjectId, bool bMoveCallLogToPrevious)
{
	//Synchronize this method
	AutoLock autoLock(this);

	m_sLastEndedCallId = sCallObjectId;
	CCTILogger::Log(LOGLEVEL_MED,L"CCTIUserInterface::OnCallEnd: Call ID %s.",sCallObjectId);
	CCTILine* pLine = GetLineByCallId(sCallObjectId);

	if (pLine) {
		pLine->HideLineStatusBar();

		CCTICallLog* pLog = GetCallLogForCallId(sCallObjectId);
		if (pLog) {
			if (pLog->GetSaveOnEnd()) {
				pLog->SetCallIsActive(false);
				pLog->SetCallObjectId(sCallObjectId);
				pLog->SetCallDuration(pLine->GetCallDurationSeconds());
				pLog->SetCallType(pLine->GetCallType());
				
				//If we're in wrapup mode, let the return from wrapup do the saving
				if (bMoveCallLogToPrevious && m_pCTIAgent->GetAgentState()!=AGENTSTATE_WRAPUP) {
					UIMoveCallLogToPreviousCalls(pLog);
				}

				m_pPreviousCalls->SetVisible(true);
			} else {
				//No save on end, so perhaps the call was never actually established.  Ditch the call log.
				m_listCallLogs.remove(pLog);
				delete pLog;
				pLog=NULL;
			}
		}

		pLine->Reset();

		if (pLine->GetFixed()) {
			//Hide any transfer or conference dialpads
			UIHideDialpad(pLine->GetLineNumber(),DIALPAD_TRANSFER,false);
			UIHideDialpad(pLine->GetLineNumber(),DIALPAD_CONFERENCE,false);
			pLine->SetAllowDialpad(true);
		} else {
			//It was a virtual line just for this call -- destroy it
			DestroyVirtualLine(pLine);
		}

		UIRefresh();
	} else {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIUserInterface::OnCallEnd: Could not find line that call is on!  Call ID %s.",sCallObjectId);
	}

	DestroyParties();
}

void CCTIUserInterface::OnCallPartyDropped(std::wstring& sCallObjectId, std::wstring& sANI)
{
	CCTILogger::Log(LOGLEVEL_MED,L"CCTIUserInterface::OnCallPartyDropped: %s.",sANI);
	CCTILine* pLine = GetLineByCallId(sCallObjectId);
	//We won't drop the last party this way -- that's for OnCallEnd
	if (pLine && pLine->GetNumberOfParties()>1) {
		pLine->RemovePartyByANI(sANI);

		if (pLine->GetNumberOfParties()<2) {
			//Remove the "conference established" status bar if there are only 2 parties left
			pLine->HideLineStatusBar();
		}
	}
	UIRefresh();
}

void CCTIUserInterface::OnCallAttemptFailed(std::wstring sErrorMessageId, std::wstring sErrorMessage, std::wstring sCallObjectId, int nLine)
{
	CCTILine* pLine = GetLineByCallId(sCallObjectId);

	//We'll try to show the call failure on a line, but if we can't find any line for the call, we'll just show the error in the main UI
	//status bar.
	if (!pLine && nLine!=0) 
	{
		pLine = GetLine(nLine);
	}

	if (pLine) {
		pLine->ShowLineStatusBar(true,sErrorMessageId,sErrorMessage);
	} else {
		UIShowStatusBar(true,sErrorMessageId,sErrorMessage);
	}

	UIRefresh();
}

void CCTIUserInterface::OnAgentStateChange(std::wstring& sAgentState)
{
	//Synchronize this method
	AutoLock autoLock(this);

	CCTILogger::Log(LOGLEVEL_MED,L"CCTIUserInterface::OnAgentStateChange: %s.",sAgentState);
	if (GetConnected()) {
		UIHideProgressBar();
		UIHideStatusBar();
	}
	UIHideLineStatusBars();

	if (!CTIIsOccupiedAgentState(sAgentState))
	{
		//If there are any remaining current call logs at this point, move them to the previous calls area
		for (CallLogList::iterator it = m_listCallLogs.begin();it!=m_listCallLogs.end();it++) {
			CCTICallLog* pLog = *it;

			if (pLog->GetSaveOnEnd()) {
				//We don't want the method below to modify the list while we're iterating through it (hence the false); we'll take care of that in a moment
				UIMoveCallLogToPreviousCalls(pLog,false);
			} else {
				//No save on end, so perhaps the call was never actually established.  Ditch the call log.
				delete pLog;
			}
		}
		//Now clear the list of current logs
		m_listCallLogs.clear();

		//Clear out last transferred call Id.  By this point, we shouldn't need it anymore, 
		//and we don't want it staying set or it will screw up future wrapups
		SetLastTransferredCallId(L"");
	}

	// if we not creating call logs in Salesforce.com, we want to skip wrapup mode, no matter what the settings are
	if (sAgentState==AGENTSTATE_WRAPUP && m_listCallLogs.empty()) {
		PARAM_MAP mapAgentState;
		mapAgentState[KEY_ID] = AGENTSTATE_READY;
		CTIChangeAgentState(mapAgentState);
		return;
	}

	if (sAgentState==AGENTSTATE_LOGOUT) {
		SetAllowDialpadForAllLines(false);
		//The agent has logged out.  Clean everything up and return to the login screen.
		SetLoggedIn(false);
		//Make sure the login screen has the latest data
		m_pLoginForm->PopulateFormData(m_mapUserParams);
		if (GetConnected()) {
			m_pLoginForm->SetVisible(true);
		}
		SetChildrenVisible(false,false,false,false);
		UIClearCallLogs();
		
	} else if (sAgentState==AGENTSTATE_WRAPUP) {
		SetAllowDialpadForAllLines(false);
		m_pLoginForm->SetVisible(false);

		//If there are any current call logs at this point, set their call durations
		for (CallLogList::iterator it = m_listCallLogs.begin();it!=m_listCallLogs.end();it++) {
			CCTICallLog* pLog = *it;
			CCTILine* pLine = GetLine(pLog->GetLineNumber());
			if (pLine && pLine->GetState()==LINE_ON_CALL) {
				pLog->SetCallDuration(pLine->GetCallDurationSeconds());
				//Also, set the last ended call to this one so the wrapup codes will refer to it properly
				m_sLastEndedCallId = pLine->GetCallObjectId();
			}
		}

		UIShowWrapupCodes();
	} else {
		if (sAgentState==AGENTSTATE_READY) {
			SetAllowDialpadForAllLines(true);
			SetChildrenVisible(true,true,false,true);
			for (int i=1;i<=GetNumberOfFixedLines();i++) {
				UIHideDialpad(i,DIALPAD_DIAL,false);
			}
		} else if (sAgentState==AGENTSTATE_NOT_READY) {
			SetAllowDialpadForAllLines(true);
			//Check to see if any actions were queued
			if (m_bQueuedLogout) {
				m_bQueuedLogout = false;
				PARAM_MAP mapAgentState;
				mapAgentState[KEY_ID] = AGENTSTATE_LOGOUT;
				CTIChangeAgentState(mapAgentState);
			} else {
				SetChildrenVisible(true,true,false,true);
				if (!m_mapQueuedMakeCall.empty()) {
					CallInitiate(m_mapQueuedMakeCall);
					m_mapQueuedMakeCall.clear();
				}
			}
		} else {
			SetChildrenVisible(true,true,false,true);
		}

		m_pLoginForm->SetVisible(false);
		SetLoggedIn(true);
	}

	UIRefresh();
}

CCTILine* CCTIUserInterface::GetNextAvailableFixedLine() {
	//Synchronize this method
	AutoLock autoLock(this);

	CCTILine* pReturnLine = NULL;

	for(LineList::iterator it = m_listLines.begin();it!=m_listLines.end();it++) {
		CCTILine* pLine = *it;
		if (pLine->GetFixed() && pLine->GetState()==LINE_OPEN) {
			pReturnLine = pLine;
			break;
		}
	}

	return pReturnLine;
}

MSXML2::IXMLDOMDocumentFragmentPtr CCTIUserInterface::SerializeToXML(MSXML2::IXMLDOMDocumentPtr pXMLDoc) 
{
	//Synchronize this method
	AutoLock autoLock(this);

	MSXML2::IXMLDOMDocumentFragmentPtr pFragment = pXMLDoc->createDocumentFragment();
	MSXML2::IXMLDOMElementPtr pXmlUi= pXMLDoc->createElement("CTIUserInterface");

	AddAttributesToElement(pXMLDoc, pXmlUi);

	//Outputs the status bar if it's visible
	AddChildIfVisible(pXMLDoc, pXmlUi, &m_statusBar);

	//Outputs the progress bar if it's visible
	AddChildIfVisible(pXMLDoc, pXmlUi, &m_progressBar);

	//Output the login form (which will only serialize itself if it's visible, of course, otherwise it'll just return an empty fragment)
	AddChildIfVisible(pXMLDoc, pXmlUi, m_pLoginForm);

	//Output the agent state area
	AddChildIfVisible(pXMLDoc, pXmlUi, m_pCTIAgent);

	//Output the reason codes
	AddChildIfVisible(pXMLDoc, pXmlUi, m_pReasonCodeSet);
	
	//Output the individual lines
	for(LineList::iterator it = m_listLines.begin();it!=m_listLines.end();it++) {
		CCTILine* pLine = *it;
		AddChildIfVisible(pXMLDoc, pXmlUi, pLine);
	}

	//Output the current and previous call logs
	for(CallLogList::iterator itLog = m_listCallLogs.begin();itLog!=m_listCallLogs.end();itLog++) {
		CCTICallLog* pLog = *itLog;
		AddChildIfVisible(pXMLDoc, pXmlUi, pLog);
	}
	AddChildIfVisible(pXMLDoc, pXmlUi, m_pPreviousCalls);

	//Outputs the logo section if it's visible
	AddChildIfVisible(pXMLDoc, pXmlUi, &m_logo);

	pFragment->appendChild(pXmlUi);
	pXmlUi.Release();

	//This IXMLDOMDocumentFragmentPtr must be released by the caller after it's appended to the larger document or fragment!
	return pFragment;
}

void CCTIUserInterface::DestroyChildren() 
{
	//Synchronize this method
	AutoLock autoLock(this);

	if (m_pCTIAgent) delete m_pCTIAgent;
	for(LineList::iterator it = m_listLines.begin();it!=m_listLines.end();it++) {
		CCTILine* pLine = *it;
		delete pLine;
	}
	m_listLines.clear();

	if (m_pReasonCodeSet) {
		delete m_pReasonCodeSet;
		m_pReasonCodeSet = NULL;
	}
	if (m_pAppExchange) {
		delete m_pAppExchange;
		m_pAppExchange = NULL;
	}
}

int CCTIUserInterface::GetNumberOfFixedLines()
{
	return m_nNumberOfFixedLines;
}

int CCTIUserInterface::GetNumberOfLines()
{
	//Synchronize this method
	AutoLock autoLock(this);

	int nNumberOfLines = (int)m_listLines.size();

	return nNumberOfLines;
}


CCTILine* CCTIUserInterface::GetLine(int nLineNumber)
{
	//Synchronize this method
	AutoLock autoLock(this);

	CCTILine* pReturnLine = NULL;

	for(LineList::iterator it = m_listLines.begin();it!=m_listLines.end();it++) {
		CCTILine* pLine = *it;
		if (pLine->GetLineNumber()==nLineNumber) {
			pReturnLine=pLine;
			break;
		}
	}

	if (!pReturnLine) CCTILogger::Log(LOGLEVEL_MED,L"CCTIUserInterface::GetLine: Line %d not found.",nLineNumber);
	return pReturnLine;
}

bool CCTIUserInterface::CallChangeCallObjectId(const std::wstring& sOldCallObjectId,const std::wstring& sNewCallObjectId)
{
	//Synchronize this method
	AutoLock autoLock(this);

	CCTILine* pLine = GetLineByCallId(sOldCallObjectId);
	if (pLine) {
		pLine->SetCallObjectId(sNewCallObjectId);
		//Change the ID in the log too...
		CCTICallLog* pLog = GetCallLogForCallId(sOldCallObjectId);
		if (pLog) {
			pLog->SetCallObjectId(sNewCallObjectId);
		}
		return true;
	} else return false;
}

CCTILine* CCTIUserInterface::GetLineByCallId(const std::wstring& sCallObjectId) 
{
	//Synchronize this method
	AutoLock autoLock(this);

	CCTILine* pReturnLine = NULL;

	if (!sCallObjectId.empty()) {
		for(LineList::iterator it = m_listLines.begin();it!=m_listLines.end();it++) {
			CCTILine* pLine = *it;
			if (pLine->GetCallObjectId()==sCallObjectId) {
				pReturnLine=pLine;
				break;
			}
		}
	}

	if (pReturnLine) {
		CCTILogger::Log(LOGLEVEL_MED,L"CCTIUserInterface::GetLineByCallId: Call ID %s is on line %d.",sCallObjectId,pReturnLine->GetLineNumber());
	} else {
		CCTILogger::Log(LOGLEVEL_MED,L"CCTIUserInterface::GetLineByCallId: No line found for call ID %s.",sCallObjectId);
	}
	return pReturnLine;
}

void CCTIUserInterface::SetChildrenVisible(bool bAgentsVisible, bool bLinesVisible, bool bReasonCodesVisible, bool bCallLogsVisible)
{
	//Synchronize this method
	AutoLock autoLock(this);

	m_pCTIAgent->SetVisible(bAgentsVisible);

	for(LineList::iterator it = m_listLines.begin();it!=m_listLines.end();it++) {
		CCTILine* pLine = *it;
		pLine->SetVisible(bLinesVisible);
	}

	m_pReasonCodeSet->SetVisible(bReasonCodesVisible);

	for(CallLogList::iterator itLog = m_listCallLogs.begin();itLog!=m_listCallLogs.end();itLog++) {
		CCTICallLog* pLog = *itLog;
		pLog->SetVisible(bCallLogsVisible && pLog->GetSaveOnEnd());
	}
	m_pPreviousCalls->SetVisible(bCallLogsVisible);
}

void CCTIUserInterface::SetPartyANI(CCTIParty* pParty, std::wstring sANI)
{
	//Remove the old ANI from the map
	m_mapANIToParties.erase(pParty->GetANI());
	pParty->SetANI(sANI);
	//Add the new ANI to the map
	m_mapANIToParties[sANI]=pParty;
}

void CCTIUserInterface::UIShowDialpad(int nLineNumber, int nDialpadType, bool bUpdateXML)
{
	CCTILine* pLine = GetLine(nLineNumber);
	if (pLine) {
		bool bAllowOneStep = ((nDialpadType==DIALPAD_TRANSFER && GetOneStepTransferEnabled()) ||
							  (nDialpadType==DIALPAD_CONFERENCE && GetOneStepConferenceEnabled()));
		pLine->ShowDialpad(nDialpadType,bAllowOneStep);
		if (bUpdateXML) UIRefresh();
	}
}

void CCTIUserInterface::UIHideDialpad(int nLineNumber, int nDialpadType, bool bUpdateXML)
{
	CCTILine* pLine = GetLine(nLineNumber);
	if (pLine && (pLine->GetDialpad()->GetType()==nDialpadType || nDialpadType==DIALPAD_ALL)) {
		pLine->HideDialpad();
		if (bUpdateXML) UIRefresh();
	}
}

void CCTIUserInterface::UIHideLineStatusBars()
{
	for(LineList::iterator it = m_listLines.begin();it!=m_listLines.end();it++) {
		CCTILine* pLine = *it;
		pLine->HideLineStatusBar();
	}
}

void CCTIUserInterface::SetNotReadyReasonCodes(PARAM_MAP& mapReasonCodes)
{
	m_mapNotReadyReasonCodes = mapReasonCodes;
}

PARAM_MAP* CCTIUserInterface::GetNotReadyReasonCodes()
{
	return &m_mapNotReadyReasonCodes;
}

void CCTIUserInterface::SetLogoutReasonCodes(PARAM_MAP& mapReasonCodes)
{
	m_mapLogoutReasonCodes = mapReasonCodes;
}

PARAM_MAP* CCTIUserInterface::GetLogoutReasonCodes()
{
	return &m_mapLogoutReasonCodes;
}

void CCTIUserInterface::SetWrapupReasonCodes(PARAM_MAP& mapReasonCodes)
{
	m_mapWrapupReasonCodes = mapReasonCodes;
}

PARAM_MAP* CCTIUserInterface::GetWrapupReasonCodes()
{
	return &m_mapWrapupReasonCodes;
}

void CCTIUserInterface::UIShowReasonCodes(PARAM_MAP& mapReasonCodes, std::wstring sReasonCodeId)
{
	m_pReasonCodeSet->SetId(sReasonCodeId);
	m_pReasonCodeSet->SetReasonCodes(mapReasonCodes);
	// if a call was transferred, we want to associate the wrapup to that call log, not necessarily the last ended call	
	std::wstring sTransferredCallId = GetLastTransferredCallId();
	m_pReasonCodeSet->SetCallObjectId(!sTransferredCallId.empty() ? sTransferredCallId : m_sLastEndedCallId);
	m_pReasonCodeSet->ShowSaveButton(sReasonCodeId==REASON_CODE_WRAPUP && GetShowWrapupSaveButton());
	m_pReasonCodeSet->ShowNone(sReasonCodeId==REASON_CODE_WRAPUP && !GetWrapupCodeRequired());

	//In wrapup mode we'll show the call logs, but for other modes we don't
	SetChildrenVisible(false,false,true,sReasonCodeId==REASON_CODE_WRAPUP);

	UIRefresh();
}

void CCTIUserInterface::UIShowWrapupCodes()
{
	UIShowReasonCodes(m_mapWrapupReasonCodes,REASON_CODE_WRAPUP);
}

void CCTIUserInterface::UIShowNotReadyReasonCodes()
{
	UIShowReasonCodes(m_mapNotReadyReasonCodes,REASON_CODE_NOT_READY);
}

void CCTIUserInterface::UIShowLogoutReasonCodes()
{
	UIShowReasonCodes(m_mapLogoutReasonCodes,REASON_CODE_LOGOUT);
}

void CCTIUserInterface::UIUpdateSid(PARAM_MAP& parameters) 
{
	GetAppExchange()->UpdateSid(parameters[KEY_INSTANCE],parameters[KEY_SID]);
}

void CCTIUserInterface::UIMoveCallLogToPreviousCalls(CCTICallLog* pLog,bool bRemoveFromCurrentList)
{
	GetAppExchange()->UpsertCallLog(pLog);
	if (bRemoveFromCurrentList) m_listCallLogs.remove(pLog);
	m_pPreviousCalls->AddCallLog(pLog);
}

void CCTIUserInterface::UIMakePreviousCallLogCurrent(CCTICallLog* pLog)
{
	//Pull the log out of the previous calls and put it back in the current calls
	m_pPreviousCalls->RemoveCallLog(pLog);
	m_listCallLogs.push_back(pLog);
}

void CCTIUserInterface::CallUpdateComments(PARAM_MAP& parameters) 
{
	if (!parameters[KEY_CALL_OBJECT_ID].empty()) {		
		//Attach the comments to the call
		CCTICallLog* pLog = GetCallLogForCallId(parameters[KEY_CALL_OBJECT_ID]);
		if (pLog) pLog->SetComments(parameters[KEY_VALUE]);
	}
}

CCTIForm* CCTIUserInterface::CreateLoginForm()
{
	return NULL;
}

void CCTIUserInterface::CallInitiateTransfer(PARAM_MAP& parameters)
{
	int nLineNumber = NULL;
	CCTILine* pLine = NULL;
	if (!parameters[KEY_LINE_NUMBER].empty()) {
		nLineNumber = CCTIUtils::StringToInt(parameters[KEY_LINE_NUMBER]);
		
		//Attach the original ANI to the call, in case the underlying platform does not send the original ANI.
		pLine = GetLine(nLineNumber);
			
		if (pLine) {
			//Hide the dialpad
			pLine->HideDialpad();
			CallAttachUserInfo(nLineNumber, USERINFO_TRANSFER);
		}			
	}

	if (parameters[KEY_DN].empty()) {
		if (pLine) pLine->ShowDialpad(DIALPAD_TRANSFER);
		OnCallAttemptFailed(KEY_ENTER_DIAL_NUMBER,L"",L"",nLineNumber);
	} else {		
		SanitizeDN(parameters);	

		if (parameters[KEY_DN].empty()) {
			if (pLine) pLine->ShowDialpad(DIALPAD_TRANSFER);
			OnCallAttemptFailed(KEY_ENTER_VALID_NUMBER,L"",L"",nLineNumber);
		}
	}	
}

void CCTIUserInterface::CallInitiateConference(PARAM_MAP& parameters)
{
	int nLineNumber = NULL;
	CCTILine* pLine = NULL;
	if (!parameters[KEY_LINE_NUMBER].empty()) {
		nLineNumber = CCTIUtils::StringToInt(parameters[KEY_LINE_NUMBER]);
		
		//Attach the original ANI to the call, in case the underlying platform does not send the original ANI.
		pLine = GetLine(nLineNumber);
		if (pLine) {
			//Hide the dialpad
			pLine->HideDialpad();

			CallAttachUserInfo(nLineNumber, USERINFO_CONFERENCE);
		}
	}

	if (parameters[KEY_DN].empty()) {
			if (pLine) pLine->ShowDialpad(DIALPAD_TRANSFER);
			OnCallAttemptFailed(KEY_ENTER_DIAL_NUMBER,L"",L"",nLineNumber);
		} else {
			SanitizeDN(parameters);
			if (parameters[KEY_DN].empty()) {
				if (pLine) pLine->ShowDialpad(DIALPAD_TRANSFER);
				OnCallAttemptFailed(KEY_ENTER_VALID_NUMBER,L"",L"",nLineNumber);
			}
		}
}

void CCTIUserInterface::SanitizeDN(PARAM_MAP& parameters)
{
	if (parameters.find(KEY_DN)==parameters.end()) return;
	std::wstring sDN = parameters[KEY_DN];
	std::wstring sStrippedDN = CCTIUtils::GetStrippedPhoneNumber(sDN);

	std::wstring sDomesticPrefix = GetDialOutCode() + GetDomesticDialCode();
	std::wstring sIntlPrefix = GetDialOutCode() + GetInternationalDialCode();

	//A short number indicates an extension
	//A + in front of the number indicates an international number
	if (sStrippedDN.length()<7) {
		//Probably an extension
		parameters[KEY_DN]=sStrippedDN;
	} else if (sDN.at(0)=='+') {
		//Probably an international number
		parameters[KEY_DN]=GetDialOutCode()+GetInternationalDialCode()+sStrippedDN;
	} else if (CCTIUtils::StringBeginsWith(sStrippedDN,GetDomesticDialCode()) || CCTIUtils::StringBeginsWith(sStrippedDN,GetInternationalDialCode())) {
		//If it's got the domestic or int'l code in there but it's just missing the dial out code, then we'll add that.
		parameters[KEY_DN]=GetDialOutCode()+sStrippedDN;
	} else if ((!CCTIUtils::StringBeginsWith(sStrippedDN,sDomesticPrefix) && !CCTIUtils::StringBeginsWith(sStrippedDN,sIntlPrefix))
				|| (CCTIUtils::StringBeginsWith(sStrippedDN,sDomesticPrefix) && sStrippedDN.length()<=11)) {
			/* It doesn't begin with a domestic or int'l prefix
			* (or it does _appear_ to begin with a domestic prefix, but it's too short a number to really be true)
			* The latter can occur with certain area codes; like if your outside dialing code is 9, then the area code
			* for Roseville, (916), will appear as if it were prefixed by a 9 and a 1, but really that's just the area code,
			* so we still have to append 91 to it.
			*
			* The best we can do in this case is assume it's a domestic call and prefix the codes accordingly.
			*/
			if (sStrippedDN.length()<10) {
				parameters[KEY_DN]=GetDialOutCode()+sStrippedDN;
			} else {
				//Only put the domestic dial code in there if there's also an area code
				parameters[KEY_DN]=GetDialOutCode()+GetDomesticDialCode()+sStrippedDN;
			}
	} else {
		parameters[KEY_DN]=sStrippedDN;
	}
}

void CCTIUserInterface::CallInitiate(PARAM_MAP& parameters)
{
	int nLineNumber = NULL;
	CCTILine* pLine = NULL;
	if (!parameters[KEY_LINE_NUMBER].empty()) {
		//There may not be a line number in the message, particularly in the case of click to dial
		nLineNumber = CCTIUtils::StringToInt(parameters[KEY_LINE_NUMBER]);
		
		pLine = GetLine(nLineNumber);
		if (pLine) {
			//Hide the dialpad
			pLine->HideDialpad();
		}
	} 

	SanitizeDN(parameters);
	
	if (parameters[KEY_DN].empty()) {
		if (pLine) pLine->ShowDialpad(DIALPAD_DIAL);
		OnCallAttemptFailed(KEY_ENTER_VALID_NUMBER,L"",L"",nLineNumber);
	}

	// store call variables in case this is a click-to-dial
	m_sId = parameters[KEY_ID];
	m_sEntityName = parameters[KEY_ENTITY_NAME];
	m_sLastDN = parameters[KEY_DN];
}

void CCTIUserInterface::CallOneStepTransfer(PARAM_MAP& parameters)
{
	if (!parameters[KEY_LINE_NUMBER].empty()) {
		int nLineNumber = CCTIUtils::StringToInt(parameters[KEY_LINE_NUMBER]);
		
		CCTILine* pLine = GetLine(nLineNumber);
		if (pLine) {
			//Hide the dialpad
			pLine->HideDialpad();
		}

		CallAttachUserInfo(nLineNumber, USERINFO_TRANSFER);
	}

	SanitizeDN(parameters);
}

void CCTIUserInterface::CallOneStepConference(PARAM_MAP& parameters)
{
	if (!parameters[KEY_LINE_NUMBER].empty()) {
		int nLineNumber = CCTIUtils::StringToInt(parameters[KEY_LINE_NUMBER]);
		
		CCTILine* pLine = GetLine(nLineNumber);
		if (pLine) {
			//Hide the dialpad
			pLine->HideDialpad();
		}

		CallAttachUserInfo(nLineNumber, USERINFO_CONFERENCE);
	}

	SanitizeDN(parameters);
}

void CCTIUserInterface::CallAttachUserInfo(int nLineNumber, int nUserInfoType)
{
	if (nUserInfoType==USERINFO_CONFERENCE || nUserInfoType==USERINFO_TRANSFER) {
		CCTILine* pLine = GetLine(nLineNumber);
		std::wstring sCallObjectId = pLine->GetCallObjectId();
		CallUnattachData(sCallObjectId, KEY_TRANSFERRED_FROM);
		CallUnattachData(sCallObjectId, KEY_CONFERENCED_FROM);
		CallAttachSingleItem(sCallObjectId,nUserInfoType==USERINFO_CONFERENCE?KEY_CONFERENCED_FROM:KEY_TRANSFERRED_FROM,GetAppExchange()->GetUserId());
	}
}

void CCTIUserInterface::CTIConnect(PARAM_MAP& parameters) 
{
	UIShowProgressBar(KEY_CONNECTING);
	//Set the map of connection parameters for auto-reconnect
	m_mapConnectionParams = parameters;

	SetDialOutCode(parameters[L"/reqDialingOptions/reqOutsidePrefix"]);
	SetDomesticDialCode(parameters[L"/reqDialingOptions/reqLongDistPrefix"]);
	SetInternationalDialCode(parameters[L"/reqDialingOptions/reqInternationalPrefix"]);

	if (CCTILogger::GetLogLevel()>=LOGLEVEL_MED) {
		for (PARAM_MAP::iterator it=parameters.begin();it!=parameters.end();it++) {
			std::wstring sKey = it->first;
			std::wstring sValue = it->second;
			CCTILogger::Log(LOGLEVEL_MED,L"Found connection parameter %s=%s.",sKey,sValue);
		}
	}
}

void CCTIUserInterface::UIAddButtonToAllLines(int nButtonId, CCTIButton* pButton)
{
	//Synchronize this method
	AutoLock autoLock(this);

	for (LineList::iterator itLine=m_listLines.begin();itLine!=m_listLines.end();itLine++) {
		CCTILine* pLine = *itLine;
		pLine->AddButton(nButtonId,pButton->Clone());
	}
}

void CCTIUserInterface::UIRemoveButtonFromAllLines(int nButtonId) {
	//Synchronize this method
	AutoLock autoLock(this);

	for (LineList::iterator itLine=m_listLines.begin();itLine!=m_listLines.end();itLine++) {
		CCTILine* pLine = *itLine;
		pLine->RemoveButton(nButtonId);
	}
}

void CCTIUserInterface::UIAddAgentState(std::wstring sId, int nOrder, std::wstring sLabel) {
	if (m_pCTIAgent) {
		m_pCTIAgent->AddAgentState(sId, nOrder, sLabel);
	}
}

void CCTIUserInterface::CTILogin(PARAM_MAP& parameters)
{
	m_pLoginForm->SetVisible(false);
	UIHideStatusBar();
	UIShowProgressBar(KEY_LOGGING_IN);
	UIRefresh();

	//Save the values in the custom setup table
	if (!GetAutoLogin() || m_mapUserParams.size() == 0) {
		//Check to see if anything's changed...
		bool bChanged = m_mapUserParams.size()!=parameters.size();
		if (!bChanged) {
			for (PARAM_MAP::iterator it=m_mapUserParams.begin();it!=m_mapUserParams.end();it++) {
				if (parameters[it->first]!=it->second) {
					bChanged = true;
					break;
				}
			}
		}

		if (bChanged) {
			//The params have changed -- save the new ones
			m_mapUserParams.clear();
			m_mapUserParams = parameters;
			GetAppExchange()->SaveUserParams(parameters);
		}
	}
}

CCTICallLog* CCTIUserInterface::CreateCallLog(int nLineNumber,std::wstring& sCallObjectId,int nCallType)
{
	//Synchronize this method
	AutoLock autoLock(this);

	std::wstring sSubject;

	//This should be a localized version of a string with two replacement parameters, like "Call {0} {1}"
	sSubject = GetAppExchange()->GetTaskSubject();

	wchar_t pszDate[64];
	std::wstring sDate;

	wchar_t pszTime[64];
	std::wstring sTime;

	if (GetDateFormatW(NULL,0,NULL,NULL,pszDate,64)!=0 &&
		GetTimeFormatW(LOCALE_USER_DEFAULT,TIME_NOSECONDS,NULL,NULL,pszTime,64)!=0) 
	{
		sDate = pszDate;
		sTime = pszTime;
	}

	sSubject = CCTIUtils::SearchAndReplace(sSubject,std::wstring(L"{0}"),sDate);
	sSubject = CCTIUtils::SearchAndReplace(sSubject,std::wstring(L"{1}"),sTime);

	CCTICallLog* pLog = new CCTICallLog(this,sSubject,nLineNumber,true);
	
	pLog->SetCallObjectId(sCallObjectId);
	pLog->SetCallType(nCallType);

	m_listCallLogs.push_back(pLog);

	return pLog;
}

CCTILine* CCTIUserInterface::CreateVirtualLine(std::wstring sCallObjectId)
{
	//Synchronize this method
	AutoLock autoLock(this);

	int nLines = GetNumberOfLines();

	//Add the virtual line to the list
	CCTILine* pLine = new CCTILine(this,nLines+1,false);
	m_listLines.push_back(pLine);

	pLine->SetCallObjectId(sCallObjectId);

	return pLine;
}

void CCTIUserInterface::DestroyVirtualLine(CCTILine* pLine)
{
	//Synchronize this method
	AutoLock autoLock(this);

	if (!pLine->GetFixed()) {
		//We can only destroy a virtual line, not a fixed one
		m_listLines.remove(pLine);
		//Renumber any remaining lines
		int nLineNumber = 1;
		for(LineList::iterator it = m_listLines.begin();it!=m_listLines.end();it++) {
			CCTILine* pLine = *it;
			if (!pLine->GetFixed()) {
				pLine->SetLineNumber(nLineNumber);
			}
			nLineNumber++;
		}

		delete pLine;
	} else {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIUserInterface::DestroyVirtualLine: Cannot destroy a fixed line.");
	}
}

std::wstring CCTIUserInterface::GetCallObjectIdFromLine(int nLineNumber)
{
	CCTILine* pLine = GetLine(nLineNumber);
	if (pLine) return pLine->GetCallObjectId();
	return L"";
}

void CCTIUserInterface::SetAllowDialpadForAllLines(bool bAllowDialpad)
{
	for(LineList::iterator it = m_listLines.begin();it!=m_listLines.end();it++) {
		CCTILine* pLine = *it;
		if (pLine->GetFixed()) {
			pLine->SetAllowDialpad(bAllowDialpad);
		}
	}
}

void CCTIUserInterface::QueueCallInitiate(PARAM_MAP& parameters)
{
	std::wstring sDN = parameters[KEY_DN];
	CCTILine* pLine = NULL;

	if (!parameters[KEY_LINE_NUMBER].empty()) {
		int nLineNumber = CCTIUtils::StringToInt(parameters[KEY_LINE_NUMBER]);
		pLine = GetLine(nLineNumber);
	}

	if (!sDN.empty()) {
		if (pLine) {
			pLine->HideLineStatusBar();
		} else {
			UIHideStatusBar();
		}
		//Force the agent to not ready before we make the call
		if (m_pCTIAgent->GetAgentState()==AGENTSTATE_READY) {
			m_mapQueuedMakeCall.insert(parameters.begin(),parameters.end());
			PARAM_MAP mapAgentState;
			mapAgentState[KEY_ID] = AGENTSTATE_NOT_READY;
			CTIChangeAgentState(mapAgentState);
		} else CallInitiate(parameters);
	} else {
		if (pLine) {
			pLine->ShowLineStatusBar(true,KEY_ENTER_DIAL_NUMBER);
		} else {
			UIShowStatusBar(true,KEY_ENTER_DIAL_NUMBER);
		}
		UIRefresh();
	}
}

void CCTIUserInterface::QueueCTIChangeAgentState(PARAM_MAP& parameters)
{
	//Force the agent to not ready before we make the call
	if (m_pCTIAgent->GetAgentState()==AGENTSTATE_READY && parameters[KEY_ID]==AGENTSTATE_LOGOUT) {
		m_bQueuedLogout = true;
		PARAM_MAP mapAgentState;
		mapAgentState[KEY_ID] = AGENTSTATE_NOT_READY;
		CTIChangeAgentState(mapAgentState);
	} else CTIChangeAgentState(parameters);
}

void CCTIUserInterface::UIShowProgressBar(std::wstring sId, std::wstring sLabel)
{
	m_progressBar.SetId(sId);
	m_progressBar.SetLabel(sLabel);
	m_progressBar.SetVisible(true);
	//The progress bar & status bar should never be shown at the same time
	UIHideStatusBar();
}

void CCTIUserInterface::UIHideProgressBar()
{
	m_progressBar.SetVisible(false);
}

void CCTIUserInterface::UIShowStatusBar(bool bError, std::wstring sId, std::wstring sLabel)
{
	m_statusBar.SetId(sId);
	m_statusBar.SetLabel(sLabel);
	m_statusBar.SetError(bError);
	m_statusBar.SetVisible(true);
	//If the status bar is shown, the progress bar shouldn't be there
	UIHideProgressBar();
}

void CCTIUserInterface::UIHideStatusBar()
{
	m_statusBar.SetVisible(false);
}

void CCTIUserInterface::QueueCTILogin(PARAM_MAP& parameters)
{
	m_mapUserParams.clear();
	m_mapUserParams = parameters;
	m_bQueuedLogin = true;
}

CCTIParty* CCTIUserInterface::CreateParty(std::wstring sANI, int nPartyType)
{
	//Synchronize this method
	AutoLock autoLock(this);

	CCTIParty* pParty = NULL;

	AniPartyMap::iterator it = m_mapANIToParties.find(sANI);
	//The ANI should not be in the map
	if (it==m_mapANIToParties.end()) {
		pParty = new CCTIParty(this,sANI,nPartyType);
		m_mapANIToParties[sANI]=pParty;
	} else {
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIUserInterface::CreateParty: Party already exists.  Using existing party.");
		pParty = it->second;
	}

	return pParty;
}

void CCTIUserInterface::DestroyParties()
{
	//Synchronize this method
	AutoLock autoLock(this);

	if (GetAllLinesOpen())
	{
		for (AniPartyMap::iterator itMap=m_mapANIToParties.begin();itMap!=m_mapANIToParties.end();itMap++)
		{
			CCTIParty* pParty = itMap->second;
			delete pParty;
		}
		m_mapANIToParties.clear();
	}
}

void CCTIUserInterface::DestroyParty(CCTIParty* pParty)
{
	//Synchronize this method
	AutoLock autoLock(this);

	m_mapANIToParties.erase(pParty->GetANI());
	delete pParty;
}

bool CCTIUserInterface::GetAllLinesOpen() {
	//Synchronize this method
	AutoLock autoLock(this);

	bool bAllLinesOpen = true;
	for (LineList::iterator it=m_listLines.begin();it!=m_listLines.end();it++) 
	{
		CCTILine* pLine = *it;
		if (pLine->GetNumberOfParties()>0) {
			bAllLinesOpen=false;
			break;
		}
	}
	return bAllLinesOpen;
}

CCTICallLog* CCTIUserInterface::GetCallLogForCallId(const std::wstring& sCallObjectId)
{
	//Synchronize this method
	AutoLock autoLock(this);

	CCTICallLog* pLog = NULL;
	for (CallLogList::iterator it=m_listCallLogs.begin();it!=m_listCallLogs.end();it++) {
		CCTICallLog* pCurrentLog = *it;
		if (pCurrentLog->GetCallObjectId()==sCallObjectId) {
			pLog = pCurrentLog;
			break;
		}
	}
	return pLog;
}

CCTICallLog* CCTIUserInterface::GetCallLogForLine(int nLineNumber) 
{
	//Synchronize this method
	AutoLock autoLock(this);

	CCTICallLog* pLog = NULL;
	for (CallLogList::iterator it=m_listCallLogs.begin();it!=m_listCallLogs.end();it++) {
		CCTICallLog* pCurrentLog = *it;
		if (pCurrentLog->GetLineNumber()==nLineNumber) {
			pLog = pCurrentLog;
			break;
		}
	}
	return pLog;
}

void CCTIUserInterface::UIAddLogObject(PARAM_MAP& parameters)
{
	//Synchronize this method
	AutoLock autoLock(this);
	int nLines = GetNumberOfLines();

	for (CallLogList::iterator it=m_listCallLogs.begin();it!=m_listCallLogs.end();it++) {
		CCTICallLog* pCurrentLog = *it;
		
		std::wstring sCallObjectId = pCurrentLog->GetCallObjectId();
		CCTILine* pLine = GetLineByCallId(sCallObjectId);
		if (pLine == NULL || 
			(pLine != NULL && pLine->GetState() == LINE_ON_HOLD && nLines != 1)) {
			bool bAutoSelect = GetCurrentAgentState()==AGENTSTATE_WRAPUP && m_sLastEndedCallId==sCallObjectId;
			pCurrentLog->AddObject(parameters[KEY_ID],parameters[KEY_OBJECT_LABEL],parameters[KEY_OBJECT_NAME],parameters[KEY_ENTITY_NAME], true, bAutoSelect);
		} else {
			pCurrentLog->AddObject(parameters[KEY_ID],parameters[KEY_OBJECT_LABEL],parameters[KEY_OBJECT_NAME],parameters[KEY_ENTITY_NAME]);
		}
	}
	UIRefresh();
}

void CCTIUserInterface::UIAddLogObjectByCallId(std::wstring& sCallObjectId, std::wstring& sId, std::wstring& sObjectLabel, std::wstring& sObjectName, std::wstring& sEntityName, bool bAttachToCall)
{
	//Synchronize this method
	AutoLock autoLock(this);

	CCTILine* pLine = GetLineByCallId(sCallObjectId);
	if (pLine) {
		for (CallLogList::iterator it=m_listCallLogs.begin();it!=m_listCallLogs.end();it++) {
			CCTICallLog* pCurrentLog = *it;
			if (pCurrentLog->GetLineNumber()==pLine->GetLineNumber()) {
				pCurrentLog->AddObject(sId,sObjectLabel,sObjectName,sEntityName,bAttachToCall);
			}
		}
	}
}

void CCTIUserInterface::CallSelectLogObject(PARAM_MAP& parameters)
{
	//Synchronize this method
	AutoLock autoLock(this);

	if (!parameters[KEY_CALL_OBJECT_ID].empty()) {		
		//Find the log in question and select the object on it
		CCTICallLog* pLog = GetCallLogForCallId(parameters[KEY_CALL_OBJECT_ID]);
		if (pLog) pLog->SelectObject(parameters[KEY_ID]);
	}

	UIRefresh();
}

void CCTIUserInterface::UIToggleTwisty(PARAM_MAP& parameters)
{
	if (parameters[KEY_ID]==KEY_PREVIOUS) {
		m_pPreviousCalls->SetOpen(!m_pPreviousCalls->GetOpen());
	} else {
		//Should be the line number of a current call log
		int nLineNumber = CCTIUtils::StringToInt(parameters[KEY_ID]);
		CCTICallLog* pLog = GetCallLogForLine(nLineNumber);
		if (pLog) pLog->SetOpen(!pLog->GetOpen());
	}
	UIRefresh();
}

void CCTIUserInterface::CallSetWrapupCode(PARAM_MAP& parameters)
{
	std::wstring sCodeId = parameters[KEY_ID];
	std::wstring sWrapupCode = sCodeId!=KEY_REASON_CODE_NONE?sCodeId:L"";
	parameters[KEY_ID] = sWrapupCode;
	m_pReasonCodeSet->SetSelectedReasonCode(sWrapupCode);

	CCTICallLog* pLog = GetCallLogForCallId(parameters[KEY_CALL_OBJECT_ID]);
	if (pLog) pLog->SetCallDisposition(sWrapupCode);
}

void CCTIUserInterface::CallSaveWrapup()
{
}

void CCTIUserInterface::SetExtension(std::wstring& sExtension) 
{ 
	m_sExtension = sExtension;
	if (GetAppExchange()) GetAppExchange()->SaveUserExtension(sExtension);
}

void CCTIUserInterface::SetLoggedIn(bool bLoggedIn) { 
	m_bLoggedIn=bLoggedIn; 
	SetAttribute(KEY_LOGGED_IN, bLoggedIn);
}

std::wstring CCTIUserInterface::GetCurrentAgentState()
{
	if (m_pCTIAgent)
	{
		return m_pCTIAgent->GetAgentState();
	}

	return L"";
}

void CCTIUserInterface::UIUpdateSavedCallLog(std::wstring sCallObjectId, std::wstring sId)
{
	AutoLock autoLock(this);
	CCTICallLog* pLog = m_pPreviousCalls->GetCallLogByCallId(sCallObjectId);
	if (pLog) {
		pLog->SetId(sId);
	}

	UIRefresh();
}