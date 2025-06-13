#pragma once
#include "CTIUserInterface.h"
#include "CUAEEventSink.h"
#include "Flatmap.h"
#include <set>

class CUAEUserInterface :
	public CCTIUserInterface
{
protected:
	std::set<int> callsBeingTransferred;
	CUAEEventSink* m_pEventSink;
	char* deviceName;
	char* cuaeIP;
	int connected;


public:
	CUAEUserInterface(CCTIAdapter* pAdapter);
	virtual ~CUAEUserInterface();
	virtual void Initialize();
	virtual void Uninitialize();
	virtual bool GetAutoLogin();
	virtual void OnAgentStateChange(std::wstring& sAgentState);
	virtual void SetCuaeIP(char* deviceName);
	virtual void SetMonitoredDevice(char* deviceName);
	virtual int ConnectToServer();


	CCTIForm* CreateLoginForm();

	/***********************************************************************************
	 *   Commands that always need to be overridden                                    *
	 *                                                                                 *
	 ***********************************************************************************/

	/**
	 * This command should perform a connection to the CTI server.  
	 * There is no default set of parameters for this command; it is entirely dependent on the data that the platform requires for connection.
	 * Subclasses that override this method should ensure that they always call the base class method first.
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CTIConnect(PARAM_MAP& parameters);

	/**
	 * This command should disconnect from the CTI server.  
	 * In this case, there is no need for a Disconnect command, so it does nothing.
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CTIDisconnect(PARAM_MAP& parameters) { };


	virtual void CallUpdateComments(PARAM_MAP& parameters);

	/**
	 * This command should log an agent into the CTI server.  
	 * There is no default set of parameters for this command; it is entirely dependent on the data that the platform requires for login.
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CTILogin(PARAM_MAP& parameters);
	/**
	 * This command should log an agent out of the CTI server.  
	 * Its parameters may include "REASON_CODE" (the ID of the logout reason code) in the event that GetLogoutReasonRequired is true.
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CTILogout(PARAM_MAP& parameters);
	/**
	 * This command should make a call.  
	 * Its parameters will include at least "DN" (the number to dial) and "LINE_NUMBER" (the line from which to make the call).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallInitiate(PARAM_MAP& parameters);

	/**
	 * This command should release the active call on a line.  
	 * Its parameters will include at least "LINE_NUMBER" (the line from which to release the call).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallRelease(PARAM_MAP& parameters);
	/**
	 * This command should answer the active call on a line.  
	 * Its parameters will include at least "LINE_NUMBER" (the line on which to answer the call).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallAnswer(PARAM_MAP& parameters);
	/**
	 * This command should place the active call on hold on a line.  
	 * Its parameters will include at least "LINE_NUMBER" (the line on which to hold the call).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallHold(PARAM_MAP& parameters);
	/**
	 * This command should CallAlternate between a call on hold and an active call.  
	 * Its parameters will include at least "LINE_NUMBER" (the line on which CallAlternate was clicked).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallAlternate(PARAM_MAP& parameters);
	/**
	 * This command should retrieve the held call on a line.  
	 * Its parameters will include at least "LINE_NUMBER" (the line from which to retrieve the call).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallRetrieve(PARAM_MAP& parameters);
	/**
	 * This command should initiate a consult transfer for the active call on a line.
	 * The default implementation of this command attaches data to the call; subclasses should be sure to call the base class implementation when overriding this method.
	 *
	 * Its parameters will include at least "DN" (the number to transfer to) and "LINE_NUMBER" (the line from which to make the transfer).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallInitiateTransfer(PARAM_MAP& parameters);
	/**
	 * This command should initiate a consult conference for the active call on a line.  
	 * The default implementation of this command attaches data to the call; subclasses should be sure to call the base class implementation when overriding this method.
	 *
	 * Its parameters will include at least "DN" (the number to conference to) and "LINE_NUMBER" (the line from which to make the conference).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallInitiateConference(PARAM_MAP& parameters);
	/**
	 * This command should initiate a one step transfer for the active call on a line.  
	 * The default implementation of this command attaches data to the call; subclasses should be sure to call the base class implementation when overriding this method.
	 *
	 * Its parameters will include at least "DN" (the number to transfer to) and "LINE_NUMBER" (the line from which to make the transfer).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallOneStepTransfer(PARAM_MAP& parameters);
	/**
	 * This command should initiate a one step conference for the active call on a line. 
	 * The default implementation of this command attaches data to the call; subclasses should be sure to call the base class implementation when overriding this method.
	 * 
	 * Its parameters will include at least "DN" (the number to conference to) and "LINE_NUMBER" (the line from which to make the conference).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallOneStepConference(PARAM_MAP& parameters);
	/**
	 * This command should complete a consult transfer for the active call on a line.  
	 * Its parameters will include at least "DN" (the number to transfer to) and "LINE_NUMBER" (the line from which to make the transfer).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallCompleteTransfer(PARAM_MAP& parameters);
	/**
	 * This command should complete a consult conference for the active call on a line.  
	 * Its parameters will include at least "DN" (the number to conference to) and "LINE_NUMBER" (the line from which to make the conference).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallCompleteConference(PARAM_MAP& parameters);
	/**
	 * This command should set the wrapup code for a call that was completed.  
	 * Its parameters will include at least "LINE_NUMBER" (the line on which the code was set) 
	 * and "ID" (the ID of the wrapup code that was selected).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallSetWrapupCode(PARAM_MAP& parameters);

	/**
	 * Subclasses should override this method to actually change to the desired agent state.  Any override of this method
	 * should be sure to call the base class method first.
	 * The input parameter map will contain a parameter called "ID" which will map to the agent state that the user specified.
	 * Possible values of the ID parameter are "LOGOUT", "NOT_READY", and "READY", and other possible codes that subclasses might define.
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CTIChangeAgentState(PARAM_MAP& parameters);

	/**
	 * Creates a unique call object ID for a call (well, relatively unique -- it's based on the current tick count).
	 *
	 * @return A new call object id
	 */
	std::wstring CreateCallObjectId()
	{
		wchar_t tickCount[33];
		_ltow(GetTickCount(),tickCount,10);
		std::wstring sCallObjectId=L"call.";
		sCallObjectId+=tickCount;
		return sCallObjectId;
	}

	/**
	 * Does some custom message processing above that of CCTIUserInterface.
	 *
	 * @param message The message.
	 * @param parameters The message parameters.
	 */
	virtual void UIHandleMessage(std::wstring& message, PARAM_MAP& parameters);

	/**
	 * Enables agent states according to the specified active agent state
	 * @param sAgentState 
	 */
	virtual void SetAgentStateEnablement(std::wstring& sAgentState);

	virtual int OnCallRinging(std::wstring sCallObjectId, int nCallType, bool bPerformSearch, bool bLogCall, PARAM_MAP& mapInfoFields, PARAM_MAP& mapAttachedData, int nLineNumber = 0);
	virtual void OnCallRetrieved(std::wstring sCallObjectId);
	virtual void OnCallEnd(std::wstring sCallObjectId, bool bMoveCallLogToPrevious=true);
};