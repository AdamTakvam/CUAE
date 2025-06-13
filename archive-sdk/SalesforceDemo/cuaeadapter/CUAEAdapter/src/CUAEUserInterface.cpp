#include "StdAfx.h"
#include "CUAEUserInterface.h"
#include "CUAEEventSink.h"
#include "FlatmapImpl.h"
#include "CTIForm.h"
#include "CTIEditBox.h"
#include "CTIButton.h"
#include "CTIAdapter.h"
#include "CTIAppExchange.h"
#include "Psapi.h"

//#include <afxsock.h> 

#define DEFAULT_FLATMAP_PORT			10000                    /* Port number for IPC communication between Apache module and Application Server */
/*******************************************************************/
/* The callback function to notify Salesforce module about an incoming call */
int incomingCallNotifier(const char* to, const char* from, const char* originalTo, const char* callId, void* userInterface)
{
	USES_CONVERSION;

	PARAM_MAP pMap;
	PARAM_MAP pMapAttachedData;

	/* convert char* to unicode */

	LPWSTR L_to = A2W(to);
	LPWSTR L_from = A2W(from);
	LPWSTR L_callId = A2W(callId);

	pMap[KEY_ANI] = L_from;
	pMap[KEY_DNIS] = L_to;
	
	CCTILogger::Log(LOGLEVEL_HIGH,L"incomingCallNotifier: Verbose: IncomingCall message received from CUAE, CallID: %s", L_callId);

	CUAEUserInterface* cuaeUserInterface = (CUAEUserInterface *) userInterface;
	cuaeUserInterface->OnCallRinging(L_callId, CALLTYPE_INBOUND, true, true, pMap, pMapAttachedData);

	return 1;
}

/*******************************************************************/
/* The callback function to notify Salesforce module about a call going active */
int callActiveNotifier(const char* to, const char* callId, void* userInterface)
{
	USES_CONVERSION;

	/* convert char* to unicode */
	LPWSTR L_callId = A2W(callId);

	CCTILogger::Log(LOGLEVEL_HIGH,L"callActiveNotifier: Verbose: CallActive message received from CUAE, CallID: %s", L_callId);

	CUAEUserInterface* cuaeUserInterface = (CUAEUserInterface *) userInterface;

	CCTILine* line = cuaeUserInterface->GetLineByCallId(L_callId);
	int nLine = line->GetLineNumber();

	std::list<int> listEnabledButtons;

	listEnabledButtons.push_back(BUTTON_RELEASE);
	//listEnabledButtons.push_back(BUTTON_HOLD);
	listEnabledButtons.push_back(BUTTON_TRANSFER);
	//listEnabledButtons.push_back(BUTTON_CONFERENCE);
	listEnabledButtons.push_back(BUTTON_NEW_LINE);

	cuaeUserInterface->OnButtonEnablementChange(nLine,listEnabledButtons,false);

	cuaeUserInterface->OnCallEstablished(L_callId);
	

	return 1;
}

/*******************************************************************/
/* The callback function to notify Salesforce module about a call going inactive */
int callInactiveNotifier(int inUse, const char* callId, void* userInterface)
{
	USES_CONVERSION;

	/* convert char* to unicode */
	LPWSTR L_callId = A2W(callId);

	CCTILogger::Log(LOGLEVEL_HIGH,L"callInactiveNotifier: Verbose: CallInactive message received from CUAE, CallID: %s", L_callId);

	CUAEUserInterface* cuaeUserInterface = (CUAEUserInterface *) userInterface;
	cuaeUserInterface->OnCallHeld(L_callId);

	return 1;
}

/*******************************************************************/
/* The callback function to notify Salesforce module about a call being hungup*/
int hangupNotifier(const char* cause, const char* callId, void* userInterface)
{
	USES_CONVERSION;

	/* convert char* to unicode */
	LPWSTR L_callId = A2W(callId);

	CCTILogger::Log(LOGLEVEL_HIGH,L"hangupNotifier: Verbose: Hangup message received from CUAE, CallID: %s", L_callId);

	CUAEUserInterface* cuaeUserInterface = (CUAEUserInterface *) userInterface;
	cuaeUserInterface->OnCallEnd(L_callId, true);

	return 1;
}

/*******************************************************************/
/* The callback function to notify Salesforce module about a call being initiated by the user*/
int initiateCallNotifier(const char* callId, const char* to, const char* from,  void* userInterface)
{
	USES_CONVERSION;

	/* convert char* to unicode */
	LPWSTR L_callId = A2W(callId);
	LPWSTR L_to = A2W(to);

	CCTILogger::Log(LOGLEVEL_HIGH,L"initiateCallNotifier: Verbose: InitiateCall message received from CUAE, CallID: %s", L_callId);

	CUAEUserInterface* cuaeUserInterface = (CUAEUserInterface *) userInterface;

	PARAM_MAP mapInfoFields;
	mapInfoFields[KEY_DNIS]=L_to;

	cuaeUserInterface->OnCallDialing(L_callId,mapInfoFields,true,true);

	return 1;
}

/*******************************************************************/
/* The callback function to notify Salesforce module about a login response*/
int loginNotifier(const char* deviceName, int lineDnCount, char lineDns[5][25],  void* userInterface)
{
	USES_CONVERSION;

	/* convert char* to unicode */
	LPWSTR L_deviceName = A2W(deviceName);

	CCTILogger::Log(LOGLEVEL_HIGH,L"loginNotifier: Verbose: LoginACK message received from CUAE, DeviceName: %s, LineCount: %d", L_deviceName, lineDnCount);

	CUAEUserInterface* cuaeUserInterface = (CUAEUserInterface *) userInterface;

	for(int i = 0; i < lineDnCount; i++)
	{
		LPWSTR L_currLineDn = A2W(lineDns[i]);

		//cuaeUserInterface->SetExtension("Hello");

		CCTILogger::Log(LOGLEVEL_HIGH,L"loginNotifier: Verbose: LoginACK message has DN %s", L_currLineDn);
	}

	

	//cuaeUserInterface->>OnCallDialing(L_callId,mapInfoFields,true,true);

	return 1;
}


/*******************************************************************/
/* The callback function to allow IPC to log*/
int logCallback(const char* log)
{
	USES_CONVERSION;

	/* convert char* to unicode */
	LPWSTR L_log = A2W(log);

	CCTILogger::Log(LOGLEVEL_HIGH,L"LogCallback: Verbose: %s", L_log);

	return 1;
}




CUAEUserInterface::CUAEUserInterface(CCTIAdapter* pAdapter)
:CCTIUserInterface(pAdapter,1)
{
}

CUAEUserInterface::~CUAEUserInterface()
{
	delete this->m_pEventSink;
}

void CUAEUserInterface::Initialize()
{
	cuaeIP = "";
	deviceName = "";
	connected = 0;

	// This should initialize CUAE IP and Device, if the user's
	// configured the two fields
	this->m_pEventSink = new CUAEEventSink(this);

	createFlatmapIpcClient((void*) this); // Create the flatmap client

	/* Assign callback functions to get telephony events */
	assignIncomingCallNotifier(incomingCallNotifier);
	assignCallActiveNotifier(callActiveNotifier);
	assignCallInactiveNotifier(callInactiveNotifier);
	assignHangupNotifier(hangupNotifier);
	assignInitiateCallNotifier(initiateCallNotifier);
	assignLoginNotifier(loginNotifier);
	assignLogCallback(logCallback);

	ConnectToServer();

	CCTIUserInterface::Initialize();

	// NEW LOGO
	SetLogoImageUrl(L"https://na1.salesforce.com/servlet/servlet.ImageServer?oid=00D000000000062&id=015300000007Emu");
	
	SetOneStepConferenceEnabled(false);
	SetOneStepTransferEnabled(true);
	SetAllowDialpadForAllLines(true);

	SetChildrenVisible(false, true, false, true);
}

int CUAEUserInterface::ConnectToServer()
{
	if(connected)
	{
		disconnectFromFlatmapIpcServer();
		connected = 0;
		SetLoggedIn(false);
		SetConnected(false);
	}

	if(!connected && cuaeIP != "" && deviceName != "")
	{
		connected = connectToFlatmapIpcServer(cuaeIP, DEFAULT_FLATMAP_PORT);

		if(connected == 0)
		{
			CCTILogger::Log(LOGLEVEL_LOW,L"CCTIUserInterface::Initialize: Warning: Unable to connect to CUAE!");
			// TODO update UI
		}
		else
		{
			/*sendLogin(deviceName);

			SetLoggedIn(true);

			OnCTIConnection();

			SetConnected(true);*/
		}
	}

	return connected;
}

void CUAEUserInterface::Uninitialize()
{
	destroyFlatmapIpcClient();
}

void CUAEUserInterface::SetMonitoredDevice(char* setDeviceName)
{	
	char *deviceNameArray = new char [strlen(setDeviceName) + 1];
	strcpy(deviceNameArray, setDeviceName);
	deviceName = deviceNameArray;
}

void CUAEUserInterface::SetCuaeIP(char* setCuaeIP)
{
	char *cuaeIPArray = new char [strlen(setCuaeIP) + 1];
	strcpy(cuaeIPArray, setCuaeIP);
	cuaeIP = cuaeIPArray;
}

void CUAEUserInterface::CTIConnect(PARAM_MAP& parameters)
{
	CCTIUserInterface::CTIConnect(parameters);
	
	//Send the tray menu
	std::wstring sMenu = L"<MENU>";
	sMenu+=L"<ITEM ID=\"MENU_CONFIGURE_CUAE\" LABEL=\"Configure...\" CHECKED=\"false\"/>";
	sMenu+=L"</MENU>";

	m_pAdapter->SendUpdateTrayMenuEvent(CCTIUtils::StringToBSTR(sMenu));

	sendLogin(deviceName);

	SetLoggedIn(true);

	OnCTIConnection();

	SetConnected(true);
}


CCTIForm* CUAEUserInterface::CreateLoginForm()
{
	CCTIForm* pForm = new CCTIForm();

	/*
	pForm->AddStatic(KEY_ENTER_LOGIN);

	
	pForm->AddEditBox(L"MY_ID", L"My Login Id", L"blah");
	CCTIEditBox* pAgentId = pForm->AddEditBox(KEY_AGENT_ID);

	CCTIEditBox* pPassword = pForm->AddEditBox(KEY_PASSWORD);
	pPassword->SetPassword(true);

	CCTIEditBox* pInstrument = pForm->AddEditBox(KEY_EXTENSION);


	CCTIButton* pLogin = pForm->AddButton(KEY_LOGIN);
	pLogin->SetColor(COLOR_GREEN);
	pLogin->SetLongStyle(true);

	*/
	return pForm;
}


bool CUAEUserInterface::GetAutoLogin()
{
	return m_bAutoLogin;
}

void CUAEUserInterface::CTILogin(PARAM_MAP& parameters)
{
	CCTIUserInterface::CTILogin(parameters);
	UIHideProgressBar();

	sendLogin(deviceName);
	
	/*
	//Any login info is fine for us.
	PARAM_MAP agentStateParams;
	OnAgentStateChange(std::wstring(AGENTSTATE_NOT_READY));
	*/
}

void CUAEUserInterface::CTILogout(PARAM_MAP& parameters)
{
	sendLogout(deviceName);

	OnAgentStateChange(std::wstring(AGENTSTATE_LOGOUT));
}

void CUAEUserInterface::CallInitiate(PARAM_MAP& parameters)
{
	USES_CONVERSION;

	CCTIUserInterface::CallInitiate(parameters);
	//Makes it seem as though we just connected and went straight through

	std:wstring to = parameters[KEY_DN];
	const char* A_to= W2CA(to.c_str());

	sendMakeCall(deviceName, A_to);
}

void CUAEUserInterface::CallRelease(PARAM_MAP& parameters)
{
	USES_CONVERSION;

	CCTIUserInterface::CallRelease(parameters);
	int nLine = CCTIUtils::StringToInt(parameters[KEY_LINE_NUMBER]);
	CCTILine* pLine = GetLine(nLine);
	if (pLine)
	{
		// Inform IPC/CUAE mechanism of hangup request

		std:wstring callId = pLine->GetCallObjectId();
					
		CCTILogger::Log(LOGLEVEL_HIGH,L"CCTIUserInterface::Initialize: Verbose: Sending hangup for callId %s", callId);

		const char* A_callId= W2CA(callId.c_str());

		sendHangup(deviceName, A_callId);

		// rather than wait on hangup event, let's just act like it succeded and update the UI accordingly

		OnCallEnd(callId);
	}
	else
	{
		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIUserInterface::CallRelease: Warning: Unable to find line to hangup");
	}


	/*
	CCTIUserInterface::CallRelease(parameters);
	int nLine = CCTIUtils::StringToInt(parameters[KEY_LINE_NUMBER]);
	CCTILine* pLine = GetLine(nLine);
	if (pLine) {
		//Drop the call
		std::list<int> listEnabledButtons;
		OnButtonEnablementChange(pLine->GetLineNumber(),listEnabledButtons,false);
		OnCallEnd(pLine->GetCallObjectId());

		bool bSetToReady = true;
		for (int nCurrentLine=1;nCurrentLine<GetNumberOfLines();nCurrentLine++) {
			CCTILine* pCurrentLine = GetLine(nCurrentLine);
			if (pCurrentLine->GetNumberOfParties()>0) {
				bSetToReady=false;
			}
		}
		if (bSetToReady) OnAgentStateChange(std::wstring(AGENTSTATE_READY));
	}
	*/
}


void CUAEUserInterface::OnCallEnd(std::wstring sCallObjectId, bool bMoveCallLogToPrevious)
{
	CCTILine* pLine = GetLineByCallId(sCallObjectId);
	if (pLine) {
		int callId = CCTIUtils::StringToInt(pLine->GetCallObjectId());
		callsBeingTransferred.erase(callId);

		std::list<int> listEnabledButtons;
		OnButtonEnablementChange(pLine->GetLineNumber(),listEnabledButtons,true);

		/*
		bool bSetToReady = true;
		for (int nCurrentLine=1;nCurrentLine<GetNumberOfLines();nCurrentLine++) {
			CCTILine* pCurrentLine = GetLine(nCurrentLine);
			if (pCurrentLine->GetNumberOfParties()>0) {
				bSetToReady=false;
			}
		}
		if (bSetToReady) OnAgentStateChange(std::wstring(AGENTSTATE_READY));
		*/
	}
	CCTIUserInterface::OnCallEnd(sCallObjectId, bMoveCallLogToPrevious);
}


void CUAEUserInterface::CallAnswer(PARAM_MAP& parameters)
{
	CCTIUserInterface::CallAnswer(parameters);
	int nLine = CCTIUtils::StringToInt(parameters[KEY_LINE_NUMBER]);
	CCTILine* pLine = GetLine(nLine);
	if (pLine) {
		std::list<int> listEnabledButtons;

		listEnabledButtons.push_back(BUTTON_RELEASE);
		//listEnabledButtons.push_back(BUTTON_HOLD); // Issue of 3rd-party hold not supported by CUAE at the moment
		listEnabledButtons.push_back(BUTTON_TRANSFER);
		//listEnabledButtons.push_back(BUTTON_CONFERENCE);

		OnButtonEnablementChange(pLine->GetLineNumber(),listEnabledButtons,false);

		OnCallEstablished(pLine->GetCallObjectId());

		if (pLine->GetLineCookie(KEY_CONFERENCED)==KEY_TRUE) {
			pLine->ClearLineCookie(KEY_CONFERENCED);
			std::wstring sParty1 = pLine->GetLineCookie(KEY_PARTY1);
			std::wstring sParty2 = pLine->GetLineCookie(KEY_PARTY2);
			pLine->ClearLineCookie(KEY_PARTY1);
			pLine->ClearLineCookie(KEY_PARTY2);

			Sleep(2000);

			//Now the "conference" has been accepted.
			StringList listParties;
			listParties.push_back(sParty1);
			listParties.push_back(sParty2);

			OnCallConferenced(pLine->GetCallObjectId(),listParties);
		}
	}
}

void CUAEUserInterface::CallHold(PARAM_MAP& parameters)
{
	CCTIUserInterface::CallHold(parameters);
	int nLine = CCTIUtils::StringToInt(parameters[KEY_LINE_NUMBER]);

	CCTILine* pLine = GetLine(nLine);
	std::wstring sCallObjId = pLine->GetCallObjectId();
	int callId = CCTIUtils::StringToInt(sCallObjId);
	
	// no 3rd-party hold ability in CUAE today

	/*
	if (pLine) {
		std::list<int> listEnabledButtons;
		listEnabledButtons.push_back(BUTTON_RETRIEVE);

		OnButtonEnablementChange(pLine->GetLineNumber(),listEnabledButtons,false);

		OnCallHeld(pLine->GetCallObjectId());
	}
	*/
}

void CUAEUserInterface::CallAlternate(PARAM_MAP& parameters) 
{ 
	//Nothing to see here...
}

void CUAEUserInterface::CTIChangeAgentState(PARAM_MAP& parameters)
{
	CCTIUserInterface::OnAgentStateChange(parameters[KEY_ID]);
}

void CUAEUserInterface::CallRetrieve(PARAM_MAP& parameters)
{
	CCTIUserInterface::CallRetrieve(parameters);
	int nLine = CCTIUtils::StringToInt(parameters[KEY_LINE_NUMBER]);
	CCTILine* pLine = GetLine(nLine);

	std::wstring sCallObjId = pLine->GetCallObjectId();
	int callId = CCTIUtils::StringToInt(sCallObjId);
}

void CUAEUserInterface::OnCallRetrieved(std::wstring sCallObjectId)
{
	CCTILine* pLine = GetLineByCallId(sCallObjectId);

	if (pLine) {
		std::list<int> listEnabledButtons;
		listEnabledButtons.push_back(BUTTON_RELEASE);
		//listEnabledButtons.push_back(BUTTON_HOLD);
		listEnabledButtons.push_back(BUTTON_TRANSFER);
		//listEnabledButtons.push_back(BUTTON_CONFERENCE);

		OnButtonEnablementChange(pLine->GetLineNumber(),listEnabledButtons,false);

		CCTIUserInterface::OnCallRetrieved(pLine->GetCallObjectId());
	}
}

void CUAEUserInterface::CallInitiateTransfer(PARAM_MAP& parameters)
{
	CallOneStepTransfer(parameters);
	//CCTIUserInterface::CallInitiateTransfer(parameters);

	/*
	//Set a cookie on the transfer line so we know this is the one we transferred from
	int nSourceLine = CCTIUtils::StringToInt(parameters[KEY_LINE_NUMBER]);
	CCTILine* pLine = GetLine(nSourceLine);
	pLine->SetLineCookie(KEY_TRANSFERRED,KEY_TRUE);

	//Makes it seem as though we just connected and went straight through

	//Create a call object ID
	std::wstring sCallObjectId=CreateCallObjectId();

	PARAM_MAP mapInfoFields;
	mapInfoFields[KEY_DNIS]=parameters[KEY_DN];

	//This will automatically stick the "call" on the next line
	int nTransferLine = OnCallDialing(sCallObjectId,mapInfoFields,true,false);

	//Now sleep for a bit, and establish the call afterwards
	Sleep(2000);

	std::list<int> listEnabledButtons;

	listEnabledButtons.push_back(BUTTON_RELEASE);
	listEnabledButtons.push_back(BUTTON_COMPLETE_TRANSFER);

	OnButtonEnablementChange(nTransferLine,listEnabledButtons,false);

	OnCallEstablished(sCallObjectId);
	*/
}

void CUAEUserInterface::CallOneStepTransfer(PARAM_MAP& parameters)
{
	//CCTIUserInterface::CallOneStepTransfer(parameters);
	int nSourceLine = CCTIUtils::StringToInt(parameters[KEY_LINE_NUMBER]);
	CCTILine* pLine = GetLine(nSourceLine);
	int callId = CCTIUtils::StringToInt(pLine->GetCallObjectId());
	std::wstring dialNum = parameters[KEY_DN];
	//callsBeingTransferred.insert(callId);
	
	//m_pEventSink->TransferCall(callId, dialNum); // TODO TRANSFER CALL

	/*
	//Set a cookie on the transfer line so we know this is the one we transferred from
	int nSourceLine = CCTIUtils::StringToInt(parameters[KEY_LINE_NUMBER]);
	CCTILine* pLine = GetLine(nSourceLine);
	pLine->SetLineCookie(KEY_TRANSFERRED,KEY_TRUE);

	//Makes it seem as though we just connected and went straight through

	//Create a call object ID
	std::wstring sCallObjectId=CreateCallObjectId();

	PARAM_MAP mapInfoFields;
	mapInfoFields[KEY_DNIS]=parameters[KEY_DN];

	//This will automatically stick the "call" on the next line
	int nTransferLine = OnCallDialing(sCallObjectId,mapInfoFields,true,false);

	//Now sleep for a bit, and establish the call afterwards
	Sleep(2000);

	PARAM_MAP completeParams;
	completeParams[KEY_LINE_NUMBER]=CCTIUtils::IntToString(nTransferLine);
	CallCompleteTransfer(completeParams);
	*/
}

void CUAEUserInterface::CallInitiateConference(PARAM_MAP& parameters)
{
	CCTIUserInterface::CallInitiateConference(parameters);

	/*
	//Set a cookie on the transfer line so we know this is the one we transferred from
	int nTransferLine = CCTIUtils::StringToInt(parameters[KEY_LINE_NUMBER]);
	CCTILine* pLine = GetLine(nTransferLine);
	pLine->SetLineCookie(KEY_TRANSFERRED,KEY_TRUE);

	//Makes it seem as though we just connected and went straight through

	//Create a call object ID
	std::wstring sCallObjectId=CreateCallObjectId();

	PARAM_MAP mapInfoFields;
	mapInfoFields[KEY_DNIS]=parameters[KEY_DN];

	//This will automatically stick the "call" on the next line
	int nLine = OnCallDialing(sCallObjectId,mapInfoFields,true,false);

	//Now sleep for a bit, and establish the call afterwards
	Sleep(2000);

	std::list<int> listEnabledButtons;

	listEnabledButtons.push_back(BUTTON_RELEASE);
	listEnabledButtons.push_back(BUTTON_COMPLETE_TRANSFER);

	OnButtonEnablementChange(nLine,listEnabledButtons,false);

	OnCallEstablished(sCallObjectId);
	*/
}

void CUAEUserInterface::CallOneStepConference(PARAM_MAP& parameters)
{
	CCTIUserInterface::CallOneStepConference(parameters);
}

void CUAEUserInterface::CallCompleteTransfer(PARAM_MAP& parameters)
{
	CCTIUserInterface::CallCompleteTransfer(parameters);

	/*
	int nTransferLine = CCTIUtils::StringToInt(parameters[KEY_LINE_NUMBER]);
	CCTILine* pTransferLine = GetLine(nTransferLine);

	//End the call on any existing line
	for (int nCurrentLine=1;nCurrentLine<GetNumberOfLines();nCurrentLine++) {
		if (nCurrentLine!=nTransferLine) {
			CCTILine* pLine = GetLine(nCurrentLine);
			if (pLine->HasLineCookie(KEY_TRANSFERRED)) {
				pLine->ClearLineCookie(KEY_TRANSFERRED);

				std::list<int> listEnabledButtons;
				OnButtonEnablementChange(pLine->GetLineNumber(),listEnabledButtons,false);

				OnCallEnd(pLine->GetCallObjectId());
			}
		}
	}
	if (pTransferLine) OnCallEnd(pTransferLine->GetCallObjectId());
	
	OnAgentStateChange(std::wstring(AGENTSTATE_READY));
	*/
}

void CUAEUserInterface::CallCompleteConference(PARAM_MAP& parameters)
{
	CCTIUserInterface::CallCompleteConference(parameters);
}

void CUAEUserInterface::CallSetWrapupCode(PARAM_MAP& parameters)
{
	CCTIUserInterface::CallSetWrapupCode(parameters);
}

void CUAEUserInterface::UIHandleMessage(std::wstring& message, PARAM_MAP& parameters)
{
	CCTIUserInterface::UIHandleMessage(message,parameters);

	//If it begins with KEY_MENU then it's one of our menu messages.  Send it to the event sink for processing.
	if (CCTIUtils::StringBeginsWith(message,std::wstring(KEY_MENU))) 
	{
		this->m_pEventSink->HandleMenuMessage(message);
	}
}

void CUAEUserInterface::CallUpdateComments(PARAM_MAP& parameters) 
{
	CCTIUserInterface::CallUpdateComments(parameters);
	
	int lineNumber = CCTIUtils::StringToInt(parameters[KEY_LINE_NUMBER]);
	CCTICallLog* pLog = GetCallLogForLine(lineNumber);
	if (pLog) pLog->SetComments(parameters[KEY_VALUE]);
}

void CUAEUserInterface::SetAgentStateEnablement(std::wstring& sAgentState) {
	SetChildrenVisible(false, true, false, false);
	SetAllowDialpadForAllLines(true);

	/*
	std::list<std::wstring> listEnabledAgentStates;

	if (sAgentState == AGENTSTATE_LOGOUT) {
		// do nothing
	} else if (sAgentState == AGENTSTATE_NOT_READY || sAgentState == AGENTSTATE_READY) {
		listEnabledAgentStates.push_back(AGENTSTATE_READY);
		listEnabledAgentStates.push_back(AGENTSTATE_NOT_READY);
		listEnabledAgentStates.push_back(AGENTSTATE_LOGOUT);
	} else if (sAgentState == AGENTSTATE_BUSY) {
		// if an agent is busy (on a call), show busy status and wrapup (allow agent to queue wrapup)
		listEnabledAgentStates.push_back(AGENTSTATE_BUSY);
		//listEnabledAgentStates.push_back(AGENTSTATE_WRAPUP);
	} else if (sAgentState == AGENTSTATE_WRAPUP) {
		listEnabledAgentStates.push_back(AGENTSTATE_WRAPUP);
	} else if (sAgentState == AGENTSTATE_LOGGED_IN) {
		// do nothing
	}

	OnAgentStateEnablementChange(listEnabledAgentStates, sAgentState);
	*/
}

void CUAEUserInterface::OnAgentStateChange(std::wstring& sAgentState) {
	SetChildrenVisible(false, true, false, false);
	UIHideProgressBar();
	SetAllowDialpadForAllLines(true);
}

//int CUAEUserInterface::OnCallDialing(std::wstring sCallObjectId, PARAM_MAP& mapInfoFields, bool bPerformSearch, bool bLogCall, int nLineNumber)
//{
//	if (mapInfoFields[KEY_DNIS].empty()) {
//		CCTILogger::Log(LOGLEVEL_LOW,L"CCTIUserInterface::OnCallDialing: Warning: OnCallDialing was called with no DNIS info field.");
//	}
//	else
//	{
//		// Strip the '9' and '1' from the number
//		std::wstring dn = mapInfoFields[KEY_DNIS];
//		if (dn.length() > 7)
//		{
//			if (CCTIUtils::StringBeginsWith(dn, GetDialOutCode()))
//			{
//				int l = (int)GetDialOutCode().length();
//				dn = dn.substr(l, dn.length() - l);
//
//				if (CCTIUtils::StringBeginsWith(dn, GetDomesticDialCode()))
//				{
//					int l2 = (int)GetDomesticDialCode().length();
//					dn = dn.substr(l2, dn.length() - l2);
//				}
//			}
//			mapInfoFields[KEY_DNIS] = dn;
//		}
//	}
//
//	int nLine = CCTIUserInterface::OnCallDialing(sCallObjectId, mapInfoFields, bPerformSearch, bLogCall, nLineNumber);
//
//	std::list<int> listEnabledButtons;
//
//	listEnabledButtons.push_back(BUTTON_RELEASE);
//	//listEnabledButtons.push_back(BUTTON_HOLD);
//	listEnabledButtons.push_back(BUTTON_TRANSFER);
//	//listEnabledButtons.push_back(BUTTON_CONFERENCE);
//
//	OnButtonEnablementChange(nLine,listEnabledButtons);
//
//	return nLine;
//
//}

int CUAEUserInterface::OnCallRinging(std::wstring sCallObjectId, int nCallType, bool bPerformSearch, bool bLogCall, PARAM_MAP& mapInfoFields, PARAM_MAP& mapAttachedData, int nLineNumber)
{
	int nLine = CCTIUserInterface::OnCallRinging(sCallObjectId, nCallType, bPerformSearch, bLogCall, mapInfoFields, mapAttachedData, nLineNumber);

	if (nLine != 0)
	{
		std::list<int> listEnabledButtons;

		listEnabledButtons.push_back(BUTTON_RELEASE);
		//listEnabledButtons.push_back(BUTTON_HOLD);
		listEnabledButtons.push_back(BUTTON_TRANSFER);
		//listEnabledButtons.push_back(BUTTON_CONFERENCE);

		OnButtonEnablementChange(nLine,listEnabledButtons);
	}
	return nLine;
}


#include "IPC/FlatmapIpcclient.h" // TODO resolve location of this