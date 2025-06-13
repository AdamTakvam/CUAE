#pragma once
#include "CTIObject.h"
#include "CTIRelatedObjectSet.h"
#include "CTIForm.h"
#include "CTILine.h"
#include "CTILogo.h"
#include "CTIStatic.h"
#include <map>
#include <string>
#include <list>
#include "CTIProgressBar.h"
#include "CTICallLog.h"
#include "CTIPreviousCalls.h"

class CCTIAgent;
class CCTIAdapter;
class CCTIReasonCodeSet;
class CCTIAppExchange;
class CCTIButton;
class CCTIParty;

typedef std::map<std::wstring,CCTIParty*> AniPartyMap;

/**
 * @version 1.0
 * 
 * This class represents the base class for the "glue" between a CTI server and the Salesforce.com user interface.
 * All CTI adapters should subclass this class, overriding the methods in the Commands section to actually perform the actions described.
 * CTI adapters should also create event sinks that call the corresponding events in this class, optionally overriding the event methods here to perform additional processing.
 */
class CCTIUserInterface :
	public CCTIObject
{
protected:
	CCTIForm* m_pLoginForm; /**< The form containing the controls (edit boxes, etc.) in which the user enters his login data. */
	CCTIAgent* m_pCTIAgent; /**< The agent object used to render the . */
	LineList m_listLines; /**< The list of lines. */
	CCTIReasonCodeSet* m_pReasonCodeSet; /**< The reason codes object used to render wrap-up and reason codes when necessary. */
	CCTIAppExchange* m_pAppExchange; /**< The default AppExchange API communication object. */

	CCTIStatic m_statusBar; /**< The status bar to be shown and hidden via UIShowStatusBar and UIHideStatusBar.  The status bar will be rendered above any other items in the softphone. */
	CCTIProgressBar m_progressBar; /**< The progress bar to be shown and hidden via UIShowProgressBar and UIHideProgressBar. */
	CCTILogo m_logo; /**< The object containing the logo to be shown at the bottom of the softphone. */

	bool m_bConnected;	/**< Indicates whether the user is connected. */
	bool m_bLoggedIn; /**< Indicates whether an agent is logged in. */
	bool m_bAutoLogin; /**< Indicates whether the user is set to automatically log in. */

	std::wstring m_sCurrentUserName; /**< The name of the current user, populated by the CCTIAppExchange object when the SID is updated. **/

	int m_nNumberOfFixedLines; /**< The number of lines this user interface will support (set at object creation). */
	std::wstring m_sAgentState; /**< The current agent state. */
	bool m_bQueuedLogin; /**< Flag indicating whether a login has been queued for the next OnCTIConnection event. */

	CCTIAdapter* m_pAdapter; /**< The "adapter" object that mediates between this CCTIUserInterface and the COM interface. */

	bool m_bEnableOneStepTransfer; /**< Flag indicating whether to allow one-step transfer. */
	bool m_bEnableOneStepConference; /**< Flag indicating whether to allow one-step conference. */

	bool m_bNotReadyReasonRequired; /**< Flag indicating whether a reason code is required when going not ready. */
	bool m_bLogoutReasonRequired; /**< Flag indicating whether a reason code is required when logging out. */
	bool m_bShowWrapupSaveButton; /**< Flag indicating whether a save button is rendered with wrapup codes. */
	bool m_bWrapupCodeRequired; /**< Flag indicating whether a wrapup code is required.  If it's not required, then a "None" option will be rendered when wrapup codes are shown. */

	PARAM_MAP m_mapWrapupReasonCodes; /**< Map of not ready reason code IDs to labels. */
	PARAM_MAP m_mapNotReadyReasonCodes; /**< Map of not ready reason code IDs to labels. */
	PARAM_MAP m_mapLogoutReasonCodes;  /**< Map of logout reason code IDs to labels. */

	bool m_bAutoReconnect; /**< Flag indicating whether we should automatically reconnect on connection closed or failure. */
	PARAM_MAP m_mapConnectionParams; /**< Map of the most recent connection parameters that were used.  Will be used to attempt a reconnect in case of connection failure. */

	PARAM_MAP m_mapUserParams; /**< Map of user parameters (e.g. login info) that have been associated with this user in the CustomSetup table. */

	PARAM_MAP m_mapQueuedMakeCall; /**< The parameter map for a queued CallInitiate; that is, a CallInitiate that was issues when the user was in a ready state, so we forced him to not ready. */
	bool m_bQueuedLogout; /**< Flag indicating whether a logout was queued, i.e. the user tried to log out from a ready state, so we force him to not ready first and then log out. */

	PARAM_MAP m_mapInfoFieldLabels; /**< A map of custom info field IDs to their labels. */
	AniPartyMap m_mapANIToParties; /**< A map of ANIs to the party objects associated with them. */

	std::wstring m_sDialOutCode; /**< The sequence of digits that allows the phone to dial outside the local switch.*/
	std::wstring m_sDomesticDialCode; /**< The sequence of digits that allows the phone to dial a domestic long distance number.*/
	std::wstring m_sInternationalDialCode; /**< The sequence of digits that allows the phone to dial a international long distance number.*/

	CCTIPreviousCalls* m_pPreviousCalls; /**< The object that holds call logs of previous calls. */
	CallLogList m_listCallLogs; /**< A list of call logs for current calls. */
	std::wstring m_sLastEndedCallId; /**< The call object ID of the last ended call. */
	std::wstring m_sLastTransferredCallId; /**< The call object ID of the last transferred call. */

	std::wstring m_sExtension; /**< The extension associated with this agent. */

	// Last call parameters
	std::wstring m_sId; /**< The Salesforce Id associated with the most recently dialed call. */
	std::wstring m_sEntityName; /**< If applicable, the entity name associated with the most recently dialed call. */
	std::wstring m_sLastDN; /**< The most recently dialed number. */

	bool m_bUseAsynchronousSearch; /**< Flag indicating whether searches should be performed asynchronously. */

	/**
	 * Creates a CCTIParty object and manages its lifetime.
	 *
	 * @param sANI The ANI associated with the party.
	 * @param nPartyType (optional) The type of the party.  See the documentation for CCTIParty for more information.
	 * @return The newly created CCTIParty object
	 */
	virtual CCTIParty* CreateParty(std::wstring sANI, int nPartyType=0);

	/**
	 * Creates a call log for the specified line with a localized subject like "Call 6/7/2006 12:51PM."
	 * Override this method to change aspects of call logs (such as the subject).
	 *
	 * @param nLineNumber The line number for which to create the log.
	 * @param sCallObjectId The call object ID of the call for which this call log is being created.
	 * @param nCallType The call type of the call for which this call log is being created.
	 * @return The newly created CCTICallLog object.
	 */
	virtual CCTICallLog* CreateCallLog(int nLineNumber,std::wstring& sCallObjectId,int nCallType);

	/**
	 * Deletes the specified party.  This should only be called just before the party is removed from the line, lest the line
	 * maintain an invalid reference.
	 *
	 * @param pParty The party to destroy.
	 */
	void DestroyParty(CCTIParty* pParty);

	/**
	 * Checks if all lines are fallow; if they are, deletes all the currently stored party objects.
	 */
	virtual void DestroyParties();
public:
	/**
	 * Creates a new CCTIUserInterface object and assigns it the number of fixed lines to display.
	 *
	 * @param pAdapter The CCTIAdapter object that this CCTIUserInterface will attach to.
	 * @param nNumberOfFixedLines The number of fixed lines that this CCTIUserInterface will support.
	 */
	CCTIUserInterface(CCTIAdapter* pAdapter,int nNumberOfFixedLines);
	~CCTIUserInterface(void);

	/**
	 * Serializes this user interface object and all its children to XML and sends it off to the adapter
	 * for inclusion in a COM event.
	 */
	virtual void UIRefresh();

	/**
	 * Parses XML-encoded input messages from the browser, retrieves the parameters therein, and routes the message
	 * to the proper command handler using the UIHandleMessage method.
	 * 
	 * @param xmlMessage The BString-encoded XML message.
	 */
	virtual void UIParseIncomingXMLMessage(BSTR xmlMessage);

	/**
	 * This method should be called immediately after the CCTIUserInterface object is constructed.
	 * By default, creates all the children of this user interface, including the login form, agent state area,
	 * all lines, and the reason code object.
	 *
	 * If this method is overridden, the subclasser must be sure to call the base class implementation first.
	 */
	virtual void Initialize();
	/**
	 * This method is called when the CCTIUserInterface object is destructed.  It can be overridden to destroy additional objects that the subclass creates.
	 * If this method is overridden, the subclasser must be sure to call the base class implementation first.
	 */
	virtual void DestroyChildren();

	/**
	 * Serializes this user interface and all its visible children to XML.
	 *
	 * @param pXMLDoc The XML document 
	 * @return 
	 */
	virtual MSXML2::IXMLDOMDocumentFragmentPtr SerializeToXML(MSXML2::IXMLDOMDocumentPtr pXMLDoc);

	/**
	 * Sets the flag indicating whether a wrapup code is required.
	 *
	 * If a wrapup code is not required and the agent state is set to AGENTSTATE_WRAPUP,
	 * then the wrapup codes will be rendered with an extra code, "None", which will not set
	 * any wrapup data.
	 *
	 * @param bRequired True if a wrapup code is required.
	 */
	virtual void SetWrapupCodeRequired(bool bRequired) { m_bWrapupCodeRequired=bRequired; };
	/**
	 * Sets the flag indicating whether a wrapup code is required.
	 *
	 * @return True if a wrapup code is required.
	 */
	virtual bool GetWrapupCodeRequired() { return m_bWrapupCodeRequired; };

	/**
	 * Sets the flag indicating whether not ready reason codes are required.
	 *
	 * @param bRequired True if a reason code is required to go not ready.
	 */
	virtual void SetNotReadyReasonRequired(bool bRequired) { m_bNotReadyReasonRequired=bRequired; };
	/**
	 * Gets the flag indicating whether not ready reason codes are required.
	 *
	 * @return True if a reason code is required to go not ready.
	 */
	virtual bool GetNotReadyReasonRequired() { return m_bNotReadyReasonRequired; };

	/**
	 * Sets the flag indicating whether logout reason codes are required.
	 *
	 * @param bRequired True if a reason code is required to log out.
	 */
	virtual void SetLogoutReasonRequired(bool bRequired) { m_bLogoutReasonRequired=bRequired; };
	/**
	 * Gets the flag indicating whether logout reason codes are required.
	 *
	 * @return True if a reason code is required to log out.
	 */
	virtual bool GetLogoutReasonRequired() { return m_bLogoutReasonRequired; };

	/**
	 * Sets the flag indicating whether a save button is rendered with wrapup codes.
	 *
	 * @param bRequired True if a save button is rendered with wrapup codes.
	 */
	virtual void SetShowWrapupSaveButton(bool bShowWrapupSaveButton) { m_bShowWrapupSaveButton=bShowWrapupSaveButton; };
	/**
	 * Gets the flag indicating whether a save button is rendered with wrapup codes.
	 *
	 * @return True if a save button is rendered with wrapup codes.
	 */
	virtual bool GetShowWrapupSaveButton() { return m_bShowWrapupSaveButton; };

	/**
	 * Shows the reason codes that are supplied in the input map.  A CTIReasonCodeSet with the codes 
	 * from that map will be generated in the XML as a result of this method call.
	 *
	 * @param mapReasonCodes Map containing the reason codes.  Keys of the map are the code keys, and values are the code descriptions.
	 * @param sReasonCodeId The reason code type ID.  Must be one of REASON_CODE_WRAPUP, REASON_CODE_NOT_READY, or REASON_CODE_LOGOUT.
	 */
	virtual void UIShowReasonCodes(PARAM_MAP& mapReasonCodes, std::wstring sReasonCodeId);

	/**
	 * Shows the wrapup codes.
	 */
	virtual void UIShowWrapupCodes();
	/**
	 * Sets the codes that will be shown when the wrapup codes are visible.
	 *
	 * @param mapReasonCodes Map containing the wrapup codes.  Keys of the map are the code keys, and values are the code descriptions.
	 */
	virtual void SetWrapupReasonCodes(PARAM_MAP& mapReasonCodes);
	/**
	 * Gets the map of wrapup codes.
	 *
	 * @return The map containing the wrapup codes.
	 */
	virtual PARAM_MAP* GetWrapupReasonCodes();

	/**
	 * Shows the not ready reason codes.
	 */
	virtual void UIShowNotReadyReasonCodes();
	/**
	 * Sets the codes that will be shown when the not ready reason codes are visible.
	 *
	 * @param mapReasonCodes Map containing the not ready reason codes.  Keys of the map are the code keys, and values are the code descriptions.
	 */
	virtual void SetNotReadyReasonCodes(PARAM_MAP& mapReasonCodes);
	/**
	 * Gets the map of not ready reason codes.
	 *
	 * @return The map containing the not ready reason codes.
	 */
	virtual PARAM_MAP* GetNotReadyReasonCodes();

	/**
	 * Shows the logout reason codes.
	 */
	virtual void UIShowLogoutReasonCodes();
	/**
	 * Sets the codes that will be shown when the logout reason codes are visible.
	 *
	 * @param mapReasonCodes Map containing the logout reason codes.  Keys of the map are the code keys, and values are the code descriptions.
	 */
	virtual void SetLogoutReasonCodes(PARAM_MAP& mapReasonCodes);
	/**
	 * Gets the map of logout reason codes.
	 *
	 * @return The map containing the logout reason codes.
	 */
	virtual PARAM_MAP* GetLogoutReasonCodes();

	/**
	 * Makes this UI's map of user parameters identical to the input source map.
	 *
	 * @param mapSource The source map.
	 */
	void SetUserParametersMap(PARAM_MAP& mapSource) { m_mapUserParams = mapSource; };

	/**
	 * Gets the next available fixed line (i.e. the next line with state LINE_OPEN), or NULL if there are no available fixed lines.
	 *
	 * @return The next available fixed line, or NULL if there are none.
	 */
	CCTILine* GetNextAvailableFixedLine();

	/**
	 * Finds whether the adapter is connected to the CTI server.
	 * The base method does a simple check against a boolean variable it keeps.
	 * Subclasses could optionally override this method to do a more thorough connected check.
	 *
	 * @return true if the adapter is connected to the CTI server.
	 */
	virtual bool GetConnected() { return m_bConnected; } ;

	/**
	 * Sets the flag that indicates whether the adapter is connected to the CTI server.
	 *
	 * @param bConnected True if the adapter is connected to the CTI server.
	 */
	virtual void SetConnected(bool bConnected) { m_bConnected=bConnected; } ;

	/**
	 * Finds whether the agent is logged in with this adapter.
	 * The base method does a simple check against a boolean variable it keeps.
	 * Subclasses could optionally override this method to do a more thorough logged-in check.
	 *
	 * @return true if the agent is logged in with this adapter.
	 */
	virtual bool GetLoggedIn() { return m_bLoggedIn; } ;

	/**
	 * Sets the flag that indicates whether the agent is logged in to the CTI server.
	 *
	 * @param bLoggedIn True if the agent is logged in to the CTI server.
	 */
	virtual void SetLoggedIn(bool bLoggedIn);

	/**
	 * @return True if searches should be performed asynchronously.
	 */
	virtual bool GetUseAsynchronousSearch() { return m_bUseAsynchronousSearch; } ;

	/**
	 * Sets the flag that indicates whether searches should be performed asynchronously.
	 *
	 * @param bUseAsynchronousSearch True if searches should be performed asynchronously.
	 */
	virtual void SetUseAsynchronousSearch(bool bUseAsynchronousSearch)
	{
		m_bUseAsynchronousSearch = bUseAsynchronousSearch;
	};

	/**
	 * Finds whether the current user is set to autologin.
	 *
	 * @return true if the user is set to autologin.
	 */
	virtual bool GetAutoLogin() { return m_bAutoLogin; } ;

	/**
	 * Sets whether the current user will autologin.
	 *
	 * @param bAutoLogin true if the user should autologin.
	 */
	virtual void SetAutoLogin(bool bAutoLogin) { m_bAutoLogin=bAutoLogin; } ;

	/*****************************************
	 *   Events                              *
	 *                                       *
	 *****************************************/

	/**
	 * This event should be called when a connection is made with the CTI server.
	 */
	virtual void OnCTIConnection();
	/**
	 * This event should be called when a connection failure with the CTI server occurs.
	 *
	 * @param sErrorMessage (optional) The error message to show.  If this parameter is not specified then the default "connection failed" error message will be shown.
	 */
	virtual void OnCTIConnectionFailed(std::wstring sErrorMessage=L"");
	/**
	 * This event should be called when a connection with the CTI server is closed (but not necessarily due to failure).
	 */
	virtual void OnCTIConnectionClosed();

	/**
	 * This version of the OnButtonEnablementChange event enables all the buttons specified in the list (by numeric button ID) for _all_ fixed lines in this UI.
	 * All buttons not in the list will be disabled.
	 *
	 * @param listEnabledButtons The list of buttons that should be enabled.
	 * @param bSendUpdatedXML True if this event should immediately update the XML (should be set to false if the button enablement change is a subset of another event which will itself update the XML) 
	 */
	virtual void OnButtonEnablementChange(std::list<int>& listEnabledButtons, bool bSendUpdatedXML=true);

	/**
	 * This version of the OnButtonEnablementChange event enables all the buttons specified in the list (by numeric button ID) _only_ for the line specified.
	 * All buttons not in the list will be disabled.
	 *
	 * @param nLine The line for which the buttons should be enabled. 
	 * @param listEnabledButtons The list of buttons that should be enabled.
	 * @param bSendUpdatedXML True if this event should immediately update the XML (should be set to false if the button enablement change is a subset of another event which will itself update the XML) 
	 */
	virtual void OnButtonEnablementChange(int nLine, std::list<int>& listEnabledButtons, bool bSendUpdatedXML=true);

	/**
	 * This event enables all agent states specified in the list and also sets the current agent state as specified.
	 *
	 * @param listEnabledAgentStates A list of agent states to enable.
     * @param sAgentState The current agent state
	 */
	virtual void OnAgentStateEnablementChange(std::list<std::wstring>& listEnabledAgentStates, std::wstring& sAgentState);

	/**
	 * This event indicates that the agent's state has changed, and updates the UI to reflect the new state.
	 *
	 * @param sAgentState The new agent state.
	 */
	virtual void OnAgentStateChange(std::wstring& sAgentState);

	/**
	 * This event should be called when a line begins ringing (or is about to ring).  The map of info fields
	 * should contain static data about the call; at the very least, it should have an entry called KEY_ANI, but it
	 * can also contain KEY_DNIS, data about the queue and skill group, and so on.  The map of attached data
	 * can be empty; if it is filled, the keys must be of the form "Object.Field", where "Object" corresponds
	 * to the developer name of an object defined in Salesforce.com, and "Field" corresponds to the developer name
	 * of a field in the same.  So "Case.CaseNumber" would be a valid key.  The values corresponding these keys
	 * should be the values that, for example, an IVR script has passed in; so if there's an entry that is
	 * "Case.CaseNumber"->KEY_1001, then this event will automatically go and do a search on cases where Case.CaseNumber=1001.
	 *
	 * @param sCallObjectId The call object ID.  This should be the unique ID that the CTI server assigns to the call.
	 * @param nCallType The call type.  Should be one of CALLTYPE_INTERNAL or CALLTYPE_INBOUND.
	 * @param bPerformSearch Flag indicating whether this event should perform a search in the AppExchange API using the supplied parameter maps.
	 * @param bLogCall Flag indicating whether a call log should be created for this call.  If true, a Current Call Log area will appear in the UI for this call.
	 * @param mapInfoFields A map of info fields to show about this call.
	 * @param mapAttachedData A map of data attached to this call.
	 * @param nLineNumber The line number on which the call has begun.  If this parameter is unspecified or set to NULL, then a new "virtual" line will be created for this call.
	 *
	 * @return The line number onto which the call was placed (or 0 if the call could not be successfully placed).
	 */
	virtual int OnCallRinging(std::wstring sCallObjectId, int nCallType, bool bPerformSearch, bool bLogCall, PARAM_MAP& mapInfoFields, PARAM_MAP& mapAttachedData, int nLineNumber=NULL);

	/**
	 * This event should be called when an agent dials out from a line.  The map of info fields
	 * should contain static data about the call; at the very least, it should have an entry called KEY_DNIS
	 * indicating the number that was dialed.
	 *
	 * @param nLineNumber The line number from which the outgoing call was made.  If this parameter is unspecified or set to NULL, then a new "virtual" line will be created for this call.
	 * @param sCallObjectId The call object ID.  This should be the unique ID that the CTI server assigns to the call.
	 * @param mapInfoFields A map of info fields to show about this call.
	 *
	 * @return The line number onto which the call was placed (or 0 if the call could not be successfully placed).
	 */
	virtual int OnCallDialing(std::wstring sCallObjectId, PARAM_MAP& mapInfoFields,bool bPerformSearch,bool bLogCall,int nLineNumber=NULL);

	/**
	 * This event should be called when a call with a particular ID has been established.
	 *
	 * @param sCallObjectId The call ID of the call that is established.
	 */
	virtual void OnCallEstablished(std::wstring sCallObjectId);

	/**
	 * This event should be called when a call with a particular ID has ended.
	 * This event will clear out that call's line; if the line was temporary (i.e. created just for that call, as in a transfer or conference),
	 * then the line will be deleted and all other lines renumbered.
	 *
	 * This event will also save the call log for the call and move that log to the previous calls area, unless otherwise specified
	 * in bMoveCallLogToPrevious.
	 *
	 * @param sCallObjectId The call ID of the call that has ended.
	 * @param bMoveCallLogToPrevious (optional) True if this method should move the call log for this call to the previous calls section, false if it should not (i.e. in anticipation of wrapup mode).  Default is true.
	 */
	virtual void OnCallEnd(std::wstring sCallObjectId, bool bMoveCallLogToPrevious=true);

	/**
	 * This event should be called when a call with a particular ID has been placed on hold.
	 *
	 * @param sCallObjectId The call ID of the call that is placed on hold.
	 */
	virtual void OnCallHeld(std::wstring sCallObjectId);

	/**
	 * This event should be called when a call with a particular ID has been retrieved.
	 *
	 * @param sCallObjectId The call ID of the call that is retrieved.
	 */
	virtual void OnCallRetrieved(std::wstring sCallObjectId);

	/**
	 * This event should be called when a call has been transferred.
	 *
	 * @param sCallObjectId The call object ID of the call that was transferred.
	 */
	virtual void OnCallTransferred(std::wstring sCallObjectId);

	/**
	 * This event should be called when a call has been conferenced.
	 *
	 * @param sCallObjectId The call object ID of the call that was conferenced.
	 * @param listParties A list of ANIs that correspond to the parties of the conference.
	 */
	virtual void OnCallConferenced(std::wstring sCallObjectId,StringList listParties);

	/**
	 * This event should be called when a party drops from a conference, but the conference
	 * has not yet ended.
	 *
	 * @param sCallObjectId The call object ID of the call from which the party has dropped.
	 * @param sANI The ANI of the party that has dropped.
	 */
	virtual void OnCallPartyDropped(std::wstring& sCallObjectId, std::wstring& sANI);

	/**
	 * This event should be thrown if an outgoing call attempt failed, e.g. the line was busy
	 * or the number does not exist.  It displays the input error message on the line's status bar as an error.
	 * If the line is unspecified, then the error message gets displayed in the main CCTIUserInterface
	 * status bar.  
	 *
	 * If the call had already been created, i.e. it is already shown in the UI, then
	 * the adapter should follow a call to this event with one to OnCallEnd to clear the call off the line.
	 *
	 * @param sErrorMessageId The ID of the error message to display, if the error message is stored in Salesforce.com.  This can be the empty string if sErrorMessage is specified.
	 * @param sErrorMessage The error message to display if the error message is generated by the CTI back end.  This can be left blank if sErrorMessageId was specified.
	 * @param sCallObjectId (optional) The call object ID of the call that failed.
	 * @param nLine (optional) The line on which to display the error message; preferably the line from which the action was initiated.
	 */
	virtual void OnCallAttemptFailed(std::wstring sErrorMessageId, std::wstring sErrorMessage, std::wstring sCallObjectId=L"", int nLine=0);

	/***********************************************************************************
	 *   Beginning of commands section                                                 *
	 *                                                                                 *
	 ***********************************************************************************/

	/***********************************************************************************
	 *   Commands that generally don't need to be overridden                           *
	 *   (see below for commands that must be overridden)                              *
	 ***********************************************************************************/
	
	/**
	 * This method is called every time the browser refreshes its UI, either by a page refresh or due to an event.
	 * It updates the instance and Salesforce.com session ID information that is stored internally so that searches
	 * and other such AppExchange API accesses can be performed.
	 *
	 * Subclasses probably do not need to override this command; but if they do, they should be sure to call this base class method first.
	 *
	 * @param parameters A map that should contain at least two key-value pairs, one for KEY_SID and one for KEY_INSTANCE.
	 */
	virtual void UIUpdateSid(PARAM_MAP& parameters);

	/**
	 * This method is called whenever the user updates the comments on an active call log.
	 *
	 * Subclasses probably do not need to override this command; but if they do, they should be sure to call this base class method first.
	 *
	 * @param parameters A map that should contain at least two key-value pairs, one for KEY_LINE_NUMBER and one for KEY_VALUE 
	 * (where VALUE corresponds to the comments that the user entered).
	 */
	virtual void CallUpdateComments(PARAM_MAP& parameters);

	/**
	 * This command handles click-to-dial links that were clicked in Salesforce.com.  
	 * It interprets the input phone number, adding dialing codes if necessary, and calls CallInitiate.
	 * Its parameters will include at least KEY_DN (the number to dial, complete with punctuation)
	 * Generally, subclasses will not need to implement this method.
	 *
	 * @param parameters 
	 */
	virtual void CallClickToDial(PARAM_MAP& parameters);

	/**
	 * This command shows the dialpad of the input type on a line, which can then be used to perform the operation
	 * specified in the dialpad type.  Subclasses generally will not need to override this method, as it does not perform any actual CTI actions.
	 *
	 * @param nLineNumber The line number on which to show the dialpad.
	 * @param nDialpadType Must be one of DIALPAD_DIAL (for the "make a call" dialpad), DIALPAD_TRANSFER (for the transfer dialpad), or DIALPAD_CONFERENCE (for the conference dialpad)
	 * @param bUpdateXML Should be true if this command should immediately update the user interface.
	 */
	virtual void UIShowDialpad(int nLineNumber, int nDialpadType, bool bUpdateXML);

	/**
	 * This command hides the dialpad of the input type on a line.  Subclasses generally will not need to override this method, as it does not perform any actual CTI actions.
	 *
	 * @param nLineNumber The line number on which to show the dialpad.
	 * @param nDialpadType Must be one of DIALPAD_ALL (to hide any dialpad currently showing), DIALPAD_DIAL, DIALPAD_TRANSFER, or DIALPAD_CONFERENCE.
	 * @param bUpdateXML Should be true if this command should immediately update the user interface.
	 */
	virtual void UIHideDialpad(int nLineNumber, int nDialpadType, bool bUpdateXML);

	/**
	 * This method uses the map version of CallAttachData to attach a single item to the call.  This method
	 * generally does not need to be overridden by subclasses.
	 *
	 * @param sCallObjectId The call object ID of the active call to attach to.
	 * @param sKey The key of the data to attach.
	 * @param sValue The value of the data to attach.
	 */
	virtual void CallAttachSingleItem(std::wstring sCallObjectId, std::wstring sKey, std::wstring sValue) 
	{ 
		PARAM_MAP mapAttachedData;
		mapAttachedData[sKey] = sValue;
		CallAttachData(sCallObjectId,mapAttachedData);
	};

	/**
	 * Adds the object to all current call logs.  This method will usually be called when a new Salesforce.com
	 * page is navigated to.
	 *
	 * The parameters of this command should include at least KEY_ID, KEY_TYPE_NAME, and KEY_OBJECT_NAME, where
	 * KEY_ID is the ID of the object to be added, KEY_TYPE_NAME is the type name (like "Contact"), and KEY_OBJECT_NAME
	 * is the name of the object itself (like "Harry Williams").
	 *
	 * @param parameters The parameters of the command.
	 */
	virtual void UIAddLogObject(PARAM_MAP& parameters);

	/**
	 * Selects the object specified in the parameters' KEY_ID value in the call log specified by the parameters' KEY_LINE_NUMBER value.
	 *
	 * The parameters of this command should include at least KEY_ID and KEY_LINE_NUMBER.
	 *
	 * @param parameters The parameters of the command.
	 */
	virtual void CallSelectLogObject(PARAM_MAP& parameters);

	/**
	 * Toggles the twisty to open or close a current call log or the previous calls area
	 *
	 * The parameters of this command should include at least KEY_ID, which should be either a line number or KEY_PREVIOUS for the previous calls area.
	 *
	 * @param parameters The parameters of the command.
	 */
	virtual void UIToggleTwisty(PARAM_MAP& parameters);

	/**
	 * Moves the current call log to the previous call logs section, first upserting its contents to Salesforce.com.
	 *
	 * @param pLog The call log to move to previous calls.
	 * @param bRemoveFromCurrentList (optional) True if this method should remove the call log from the list of current logs, false if it should not.  Default is true.
	 */
	virtual void UIMoveCallLogToPreviousCalls(CCTICallLog* pLog, bool bRemoveFromCurrentList=true);

	/**
	 * Takes the specified call log out of previous calls and restores it to current status.
	 *
	 * @param pLog The call log to make current.
	 */
	virtual void UIMakePreviousCallLogCurrent(CCTICallLog* pLog);

	/**
	 * This method returns true if the input agent state is an "occupied" agent state.  An "occupied" agent state is defined as an agent state
	 * wherein it is possible that there may be active calls; by default, this means that the agent is either in a call or in wrapup mode.  
	 * This method should be overridden if more "occupied" agent states are added.  For instance, someone may choose to add an “At Lunch” 
	 * agent state, and that would be considered unoccupied (because there won’t be any calls on the line when you’re at lunch) – but if 
	 * someone chose to add a “Dialing” agent state, that would be considered an “occupied” agent state, because it’s likely that calls 
	 * will be on a line (or at least the line will be engaged) while the agent is dialing.
	 *
	 * @param sAgentState The agent state.
	 * @return True if the input agent state is an "occupied" agent state.
	 */
	virtual bool CTIIsOccupiedAgentState(std::wstring sAgentState);

	/**
	 * Updates a newly saved call log by setting its ID and refreshing the UI to reflect the fact that the log has been saved.
	 *
	 * @param sCallObjectId The call object ID of the call log to update.
	 * @param sId The ID of the Task object created for the call log.
	 */
	virtual void UIUpdateSavedCallLog(std::wstring sCallObjectId, std::wstring sId);

	/***********************************************************************************
	 *   Commands that should always be overridden                                     *
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
	 * There is no default set of parameters for this command; it is entirely dependent on the data that the platform requires for disconnection.
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CTIDisconnect(PARAM_MAP& parameters) { };

	/**
	 * This command should log an agent into the CTI server.  
	 * There is no default set of parameters for this command; it is entirely dependent on the data that the platform requires for login.
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CTILogin(PARAM_MAP& parameters);
	/**
	 * This command should log an agent out of the CTI server.  
	 * Its parameters may include KEY_REASON_CODE (the ID of the logout reason code) in the event that GetLogoutReasonRequired is true.
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CTILogout(PARAM_MAP& parameters) { };
	/**
	 * This command should make a call.  
	 * Its parameters will include at least KEY_DN (the number to dial) and KEY_LINE_NUMBER (the line from which to make the call).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallInitiate(PARAM_MAP& parameters);

	/**
	 * This command should release the active call on a line.  
	 * Its parameters will include at least KEY_LINE_NUMBER (the line from which to release the call).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallRelease(PARAM_MAP& parameters) { };
	/**
	 * This command should answer the active call on a line.  
	 * Its parameters will include at least KEY_LINE_NUMBER (the line on which to answer the call).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallAnswer(PARAM_MAP& parameters) { };
	/**
	 * This command should place the active call on hold on a line.  
	 * Its parameters will include at least KEY_LINE_NUMBER (the line on which to hold the call).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallHold(PARAM_MAP& parameters) { };
	/**
	 * This command should alternate between a call on hold and an active call.  
	 * Its parameters will include at least KEY_LINE_NUMBER (the line on which alternate was clicked).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallAlternate(PARAM_MAP& parameters) { };
	/**
	 * This command should retrieve the held call on a line.  
	 * Its parameters will include at least KEY_LINE_NUMBER (the line from which to retrieve the call).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallRetrieve(PARAM_MAP& parameters) { };
	/**
	 * This command should initiate a consult transfer for the active call on a line.
	 * The default implementation of this command attaches data to the call; subclasses should be sure to call the base class implementation when overriding this method.
	 *
	 * Its parameters will include at least KEY_DN (the number to transfer to) and KEY_LINE_NUMBER (the line from which to make the transfer).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallInitiateTransfer(PARAM_MAP& parameters);
	/**
	 * This command should initiate a consult conference for the active call on a line.  
	 * The default implementation of this command attaches data to the call; subclasses should be sure to call the base class implementation when overriding this method.
	 *
	 * Its parameters will include at least KEY_DN (the number to conference to) and KEY_LINE_NUMBER (the line from which to make the conference).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallInitiateConference(PARAM_MAP& parameters);
	/**
	 * This command should initiate a one step transfer for the active call on a line.  
	 * The default implementation of this command attaches data to the call; subclasses should be sure to call the base class implementation when overriding this method.
	 *
	 * Its parameters will include at least KEY_DN (the number to transfer to) and KEY_LINE_NUMBER (the line from which to make the transfer).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallOneStepTransfer(PARAM_MAP& parameters);
	/**
	 * This command should initiate a one step conference for the active call on a line. 
	 * The default implementation of this command attaches data to the call; subclasses should be sure to call the base class implementation when overriding this method.
	 * 
	 * Its parameters will include at least KEY_DN (the number to conference to) and KEY_LINE_NUMBER (the line from which to make the conference).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallOneStepConference(PARAM_MAP& parameters);
	/**
	 * This command should complete a consult transfer for the active call on a line.  
	 * Its parameters will include at least KEY_DN (the number to transfer to) and KEY_LINE_NUMBER (the line from which to make the transfer).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallCompleteTransfer(PARAM_MAP& parameters) { };
	/**
	 * This command should complete a consult conference for the active call on a line.  
	 * Its parameters will include at least KEY_DN (the number to conference to) and KEY_LINE_NUMBER (the line from which to make the conference).
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallCompleteConference(PARAM_MAP& parameters) { };
	/**
	 * This command should set the wrapup code for a call that was completed.  
	 * Its parameters will include at least KEY_CALL_OBJECT_ID (the call object ID of the call being wrapped up) 
	 * and KEY_ID (the ID of the wrapup code that was selected).
	 *
	 * Subclasses should be sure to call the base class method prior to acting upon the wrapup code.
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CallSetWrapupCode(PARAM_MAP& parameters);

	/**
	 * This command should save the wrapup code to the CTI system and set the agent back to a ready state.
	 * It does not have any parameters.
	 */
	virtual void CallSaveWrapup();

	/**
	 * Subclasses should override this method to actually change to the desired agent state.  Any override of this method
	 * should be sure to call the base class method first.
	 * The input parameter map will contain a parameter called KEY_ID which will map to the agent state that the user specified.
	 * Possible values of the ID parameter are KEY_LOGOUT, KEY_NOT_READY, and KEY_READY, and other possible codes that subclasses might define.
	 *
	 * @param parameters The map of parameters.
	 */
	virtual void CTIChangeAgentState(PARAM_MAP& parameters) 
	{
		//The user is explicitly logging out, so turn off autologin for now
		if (parameters[KEY_ID]==KEY_LOGOUT) {
			SetAutoLogin(false);
		}
	};

	/**
	 * This method should be overridden by subclasses to attach data to the active call on the specified line.  If a name collision
	 * exists -- i.e. there exists a piece of data attached to the call with the same key as one specified here -- then it is
	 * expected that this method will overwrite that piece of data.
	 *
	 * Note that the attached data is passed as a map rather than a reference.  
	 * This creates a local copy of the map, allowing subclasses to modify the map as they see fit without affecting the caller's map.
	 *
	 * @param sCallObjectId The call object ID of the active call to attach to.
	 * @param mapAttachedData A map of key-value pairs of the data to attach.
	 */
	virtual void CallAttachData(std::wstring sCallObjectId, PARAM_MAP mapAttachedData) { };

	/**
	 * This method should be overridden by subclasses to unattach the piece of data with the specified key from the active call.
	 * If the key does not exist, this method should return silently.
	 *
	 * @param sCallObjectId The call object ID of the active call to attach to.
	 * @param sKey The key of the data to unattach.
	 */
	virtual void CallUnattachData(std::wstring sCallObjectId, std::wstring sKey) { };

	/***********************************************************************************
	 *   End of commands section                                                       *
	 *                                                                                 *
	 ***********************************************************************************/
	
	/**
	 * This method handles messages that are incoming from the browser controller.  
	 * It should be overridden by subclasses to handle messages that don't correspond to default commands.
	 *
	 * @param message The specific message that was called (DIAL, LOGIN, etc.)
	 * @param parameters The parameters that were specified in the call (say, on the query string)
	 */
	virtual void UIHandleMessage(std::wstring& message, PARAM_MAP& parameters);

	/**
	 * Returns the number of fixed lines that this adapter supports (set in the constructor of this object).
	 *
	 * @return The number of fixed lines that this user interface includes.
	 */
	virtual int GetNumberOfFixedLines();

	/**
	 * Returns the total number of lines, both fixed and virtual, that this user interface currently includes.
	 *
	 * @return The total number of lines, both fixed and virtual, that this user interface currently includes.
	 */
	virtual int GetNumberOfLines();

	/**
	 * Returns the CCTILine object representing the specified line, or NULL if no such object exists.
	 *
	 * @param nLineNumber The line number to return.
	 * @return A CCTILine object representing the specified line.
	 */
	virtual CCTILine* GetLine(int nLineNumber);

	/**
	 * Returns the CCTILine object representing line handling the given call ID, or NULL if no such object exists.
	 *
	 * @param sCallObjectId An active call object ID.
	 * @return A CCTILine object representing the line handling the given call ID.
	 */
	virtual CCTILine* CCTIUserInterface::GetLineByCallId(const std::wstring& sCallObjectId);

	/**
	 * Sets the visibility of the children of this user interface.
	 *
	 * @param bAgentsVisible True if the agent state should be visible, false otherwise.
	 * @param bLinesVisible True if the lines should be visible, false otherwise.
	 * @param bReasonCodesVisible True if the reason codes should be visible, false otherwise.
	 * @param bCallLogsVisible True if the call logs should be visible, false otherwise.
	 */
	virtual void SetChildrenVisible(bool bAgentsVisible, bool bLinesVisible, bool bReasonCodesVisible,bool bCallLogsVisible);
	
	/**
	 * Shows or hides the "One-Step Transfer" button when the user begins a transfer operation.  This method
	 * can be set at any time and will be "remembered" for all future dialpads during this agent's session.
	 *
	 * @param bEnable True if "One-Step Transfer" should be available.
	 */
	virtual void SetOneStepTransferEnabled(bool bEnable) { m_bEnableOneStepTransfer = bEnable; };

	/**
	 * Gets the current visibility of the One-Step Transfer button in the transfer dialpad.
	 *
	 * @return The current visibility of the One-Step Transfer button in the transfer dialpad.
	 */
	virtual bool GetOneStepTransferEnabled() { return m_bEnableOneStepTransfer; };

	/**
	 * Shows or hides the "One-Step Conference" button when the user begins a conference operation.  This method
	 * can be set at any time and will be "remembered" for all future dialpads during this agent's session.
	 *
	 * @param bEnable True if "One-Step Conference" should be available.
	 */
	virtual void SetOneStepConferenceEnabled(bool bEnable) { m_bEnableOneStepConference = bEnable; };

	/**
	 * Gets the current visibility of the One-Step Conference button in the conference dialpad.
	 *
	 * @return The current visibility of the One-Step Conference button in the conference dialpad.
	 */
	virtual bool GetOneStepConferenceEnabled() { return m_bEnableOneStepConference; };

	/**
	 * Sets whether we should automatically reconnect on connection closed or failure.
	 *
	 * @param bAutoReconnect True if we should automatically reconnect on connection closed or failure.
	 */
	virtual void SetAutoReconnect(bool bAutoReconnect) { m_bAutoReconnect = bAutoReconnect; };

	/**
	 * Gets whether we automatically reconnect on connection closed or failure.
	 *
	 * @return True if we automatically reconnect on connection closed or failure.
	 */
	virtual bool GetAutoReconnect() { return m_bAutoReconnect; };

	/**
	 * This method should be subclassed by all adapters to provide an appropriate form for logging 
	 * an agent into this adapter.  This method should create a new instance of CCTIForm; the caller
	 * will take responsibility for disposing of the object.
	 *
	 * @return A CCTILoginForm object whose children are all the visual elements necessary to log an agent into this adapter.
	 */
	virtual CCTIForm* CreateLoginForm();

	/**
	 * This method uses the CallAttachData method to attach the current user's name and extension to the call (useful for transfers and conferences).
	 * Subclasses generally do not need to override this method, except if they wish to attach additional user information when it's called.
	 *
	 * @param nLineNumber The line containing the call to attach the data to.
	 * @param nUserInfoType The type of user info to attach.  Should be one of USERINFO_TRANSFER or USERINFO_CONFERENCE.
	 */
	virtual void CallAttachUserInfo(int nLineNumber, int nUserInfoType);

	/**
	 * Sets the name (as recorded in Salesforce.com) of the currently logged-in user.
	 *
	 * @param sUserName The name of the currently logged-in user.
	 */
	virtual void SetCurrentUserName(std::wstring sUserName) { m_sCurrentUserName = sUserName; };
	/**
	 * Gets the name (as recorded in Salesforce.com) of the currently logged-in user.
	 */
	virtual std::wstring GetCurrentUserName() { return m_sCurrentUserName; };

	/**
	 * Sets the call object ID of the last ended call.  This should be called prior to going to wrap-up mode.
	 *
	 * @param sCallId The name of the currently logged-in user.
	 */
	virtual void SetLastEndedCallId(std::wstring sCallId) { m_sLastEndedCallId = sCallId; };
	/**
	 * Gets the the call object ID of the last ended call.
	 */
	virtual std::wstring GetLastEndedCallId() { return m_sLastEndedCallId; };

	/**
	 * Sets the call object ID of the last transferred call.  If a transfer occurs, this should be called prior to going to wrap-up mode.
	 *
	 * @param sCallId 
	 */
	virtual void SetLastTransferredCallId(std::wstring sCallId) { m_sLastTransferredCallId = sCallId; };
	/**
	 * Gets the the call object ID of the last transferred call.
	 */
	virtual std::wstring GetLastTransferredCallId() { return m_sLastTransferredCallId; };

	/**
	 * Takes the input button and makes copies of it for all lines.  Note that the button object passed here is not disposed
	 * of by CCTIUserInterface; it is the responsibility of the caller to dispose of the button object input here.
	 *
	 * @param nButtonId The numeric ID of the button.  Note that the button's ID also defines its position when shown.  Buttons are shown according to ascending order of their IDs.
	 * @param pButton The button object.  Note that the caller of this method should be sure to dispose of this button object after this call.
	 */
	virtual void UIAddButtonToAllLines(int nButtonId, CCTIButton* pButton);

	/**
	 * Removes the button with the specified numeric ID from all lines.
	 *
	 * @param nButtonId The numeric ID of the buttons to remove.
	 */
	virtual void UIRemoveButtonFromAllLines(int nButtonId);

	/**
	 * Adds an agent state to the CTIAgent object and immediately makes it invisible.  It can be made visible later.
	 *
	 * @param sId The ID of the agent state.
	 * @param nOrder The order in which the agent state should be displayed.
	 * @param sLabel The label of the agent state.
	 */
	virtual void UIAddAgentState(std::wstring sId, int nOrder, std::wstring sLabel);

	/**
	 * Gets the one and only search object that this CCTIUserInterface contains, creating it first if necessary.
	 * Subclasses should override this method to provide their own subclass of CCTIAppExchange if they wish to override
	 * the default search behavior.
	 *
	 * @return A reference to the one and only search object that this CCTIUserInterface contains.
	 */
	virtual CCTIAppExchange* GetAppExchange();

	/**
	 * Sets the URL of the image that should be shown in the logo section of the UI.
	 * The image should be exactly 116x31 (WxH) pixels.
	 *
	 * @param sImageUrl The URL of the image.
	 */
	virtual void SetLogoImageUrl(std::wstring sImageUrl) {
		m_logo.SetImageUrl(sImageUrl);
	}

	/**
	 * Creates a virtual line to handle the call with the specified call ID.
	 *
	 * @param sCallObjectId The call ID that will be handled by the virtual line.
	 * @return A CCTILine object representing a virtual line.
	 */
	virtual CCTILine* CreateVirtualLine(std::wstring sCallObjectId);
	/**
	 * Destroys the virtual line associated with the input call ID.
	 *
	 * @param pLine The virtual line to destroy.
	 */
	virtual void DestroyVirtualLine(CCTILine* pLine);

	/**
	 * Gets the call object ID from the specified line, or the empty string if the line has no call object id.
	 *
	 * @param nLineNumber The line from which to get the call object ID
	 * @return The call object ID, or the empty string if the line has no call object id.
	 */
	virtual std::wstring GetCallObjectIdFromLine(int nLineNumber);

	/**
	 * Sets the dialpad to be allowed or disallowed for all lines.
	 *
	 * @param bAllowDialpad True if the dialpad should be allowed for all lines.
	 */
	virtual void SetAllowDialpadForAllLines(bool bAllowDialpad);

	/**
	 * Queues a CallInitiate command so that the agent can first be set to Not Ready.
	 *
	 * @param parameters The parameters of the CallInitiate command
	 */
	virtual void QueueCallInitiate(PARAM_MAP& parameters);

	/**
	 * Queues a CTIChangeAgentState command (generally LOGOUT) so that the agent can first be set to Not Ready.
	 *
	 * @param parameters The parameters of the CTIChangeAgentState command
	 */
	virtual void QueueCTIChangeAgentState(PARAM_MAP& parameters);

	/**
	 * Hides the progress bar.
	 */
	virtual void UIHideProgressBar();
	/**
	 * Shows a progress bar with the text associated with given ID or with the optional input label.
	 *
	 * @param sId The ID of the text to show with the progress bar.
	 * @param sLabel (optional) The text to show with the progress bar.
	 */
	virtual void UIShowProgressBar(std::wstring sId, std::wstring sLabel=L"");

	/**
	 * Hides the status bar.
	 */
	virtual void UIHideStatusBar();
	/**
	 * Shows a status bar with the text associated with given ID or with the optional input label.
	 *
	 * @param bError True if the status bar should display itself as an error message; false if it should be only informational.
	 * @param sId The ID of the text to show with the status bar.
	 * @param sLabel (optional) The text to show with the status bar.
	 */
	virtual void UIShowStatusBar(bool bError, std::wstring sId, std::wstring sLabel=L"");

	/**
	 * Queues a login request until after an OnCTIConnection has been received.
	 *
	 * @param parameters The login parameters.
	 */
	virtual void QueueCTILogin(PARAM_MAP& parameters);

	/**
	 * Hides the line status bar on all lines.
	 */
	virtual void UIHideLineStatusBars();

	/**
	 * Sets a label associated with the input info field ID.
	 *
	 * @param sId The info field ID.
	 * @param sLabel The info field label.
	 */
	virtual void SetInfoFieldLabel(std::wstring sId, std::wstring sLabel)
	{
		m_mapInfoFieldLabels[sId]=sLabel;
	}

	/**
	 * Returns the info field label corresponding to the input ID, if one exists.
	 *
	 * @param sId The info field ID.
	 * @return The info field corresponding to the ID, or the empty string if none exist.
	 */
	virtual std::wstring GetInfoFieldLabel(std::wstring sId)
	{
		PARAM_MAP::iterator it = m_mapInfoFieldLabels.find(sId);
		if (it!=m_mapInfoFieldLabels.end()) {
			return it->second;
		}
		return L"";
	}

	/**
	 * Sets the ANI of a party and updates it in the ANI-to-party map.  This method should be called
	 * if a party's ANI changes after the OnCallRinging message (if, for example, the ANI was not available
	 * at the time OnCallRinging was called).
	 *
	 * @param pParty The party to change.
	 * @param sANI The ANI to set in the party.
	 */
	virtual void SetPartyANI(CCTIParty* pParty, std::wstring sANI);

	/**
	 * Finds the line with the old call object ID and changes it to the new call object ID.  This may
	 * occur, for example, with switches that create a new connection (and a new corresponding ID) when 
	 * a conference or transfer is completed.
	 *
	 * @param sOldCallObjectId The old call object ID.
	 * @param sNewCallObjectId The new call object ID.
	 * @return True if a line with the old call object ID was found and its ID changed.
	 */
	virtual bool CallChangeCallObjectId(const std::wstring& sOldCallObjectId,const std::wstring& sNewCallObjectId);

	/**
	 * Gets the call log object associated with the specified line number.
	 *
	 * @param nLineNumber The line number.
	 * @return The call log object associated with the specified line, or NULL if no such log exists.
	 */
	virtual CCTICallLog* GetCallLogForLine(int nLineNumber); 

	/**
	 * Gets the call log object associated with the specified call object ID.
	 *
	 * @param sCallObjectId The call object ID.
	 * @return The call log object associated with the specified call object ID, or NULL if no such log exists.
	 */
	CCTICallLog* GetCallLogForCallId(const std::wstring& sCallObjectId);

	/**
	 * Sets the dial out code, which is the sequence of digits that allows the phone to dial outside the local switch.
	 *
	 * @param sDialOutCode The dial out code (like KEY_9).
	 */
	virtual void SetDialOutCode(std::wstring sDialOutCode) {
		m_sDialOutCode=sDialOutCode;
	}
	/**
	 * Gets the dial out code, which is the sequence of digits that allows the phone to dial outside the local switch.
	 */
	virtual std::wstring GetDialOutCode() { return m_sDialOutCode; };

	/**
	 * Sets the domestic dial code, which is the sequence of digits that allows the phone to dial a domestic long distance number.
	 *
	 * @param sDomesticDialCode The domestic dial code (like KEY_9).
	 */
	virtual void SetDomesticDialCode(std::wstring sDomesticDialCode) {
		m_sDomesticDialCode=sDomesticDialCode;
	}
	/**
	 * Gets the domestic dial code, which is the sequence of digits that allows the phone to dial a domestic long distance number.
	 */
	virtual std::wstring GetDomesticDialCode() { return m_sDomesticDialCode; };

	/**
	 * Sets the international dial code, which is the sequence of digits that allows the phone to dial a international long distance number.
	 *
	 * @param sInternationalDialCode The International dial code (like KEY_9).
	 */
	virtual void SetInternationalDialCode(std::wstring sInternationalDialCode) {
		m_sInternationalDialCode=sInternationalDialCode;
	}
	/**
	 * Gets the international dial code, which is the sequence of digits that allows the phone to dial a international long distance number.
	 */
	virtual std::wstring GetInternationalDialCode() { return m_sInternationalDialCode; };

	/**
	 * Adds the object to the call log referred to by the input call object ID (and selects it in the call log).  This method is synchronized 
	 * in such a way that multiple threads can use it to add objects to call logs without having to worry whether the call log 
	 * has been deprecated or disposed of.
	 *
	 * @param sCallObjectId The call object ID.
	 * @param sId The ID of the Salesforce.com object to add to the call log.
	 * @param sObjectLabel The object label of the Salesforce.com object (like "Contact")
	 * @param sObjectName The object name of the Salesforce.com object (like "Harry Smith")
	 * @param sEntityName The name of the type in the API (like "Contact", or "Issue__c" for a custom object called "Issue")
	 * @param bAttachToCall True if this object should be attached to the call, false otherwise.
	 */
	virtual void UIAddLogObjectByCallId(std::wstring& sCallObjectId, std::wstring& sId, std::wstring& sObjectLabel, std::wstring& sObjectName, std::wstring& sEntityName, bool bAttachToCall);

	/**
	 * Clears all current and previous call log objects, freeing their memory.
	 */
	virtual void UIClearCallLogs();

	/**
	 * Sanitizes the dialed number by stripping non-numeric characters from it and adding dial-out and dialing codes if necessary.
	 *
	 * @param parameters A PARAM_MAP that should contain a parameter called KEY_DN.
	 */
	virtual void SanitizeDN(PARAM_MAP& parameters);

	
	/**
	 * Sets the extension number associated with this agent.
	 *
	 * @param sExtension The extension number associated with this line.
	 */
	virtual void SetExtension(std::wstring& sExtension);

	/**
	 * Gets the extension number associated with this agent.
	 *
	 * @return The extension number associated with this agent.
	 */
	virtual std::wstring GetExtension() { return m_sExtension; };

	/**
	 * @return A pointer to the previous call logs object.
	 */
	virtual CCTIPreviousCalls* GetPreviousCallLogs() { return m_pPreviousCalls; };

	/**
	 * @return True if all lines are currently open, false otherwise.
	 */
	virtual bool GetAllLinesOpen();

	/**
	 * Gets the current agent state.
	 *
	 * @return The ID of the current agent state.
	 */
	virtual std::wstring GetCurrentAgentState();
};
