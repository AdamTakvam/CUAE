#pragma once

#include "CTIObject.h"
#include <map>
#include "ctidialpad.h"
#include "CTIRelatedObjectSet.h"
#include "CTIInfoField.h"
#include "CTIStatic.h"
#include "CTIParty.h"

class CCTIUserInterface;
class CCTITransferConferenceInfo;
class CCTITimer;
class CCTIButton;

typedef std::map<int,CCTIButton*> ButtonMap;
/**
 * @version 1.0
 * 
 * This class represents a single line and all its children in the Salesforce.com CTI user interface.
 */
class CCTILine :
	public CCTIObject
{
protected:
	bool m_bFixed; /**< Flag indicating whether this line is fixed or virtual. */
	int m_nLineNumber; /**< This line's line number. */
	bool m_bAllowAlternate; /**< Flag indicating whether this line's info area will show a link allowing it to be alternated to. */
	bool m_bAllowDialpad; /**< Flag indicating whether this line's info area will show a link allowing a dialpad to be shown. */
	int m_nLineState; /**< This line's current state.  Should be one of LINE_OPEN, LINE_RINGING, LINE_ON_CALL, or LINE_DIALING.	*/
	int m_nCallType; /**< This line's current call type.  Should be one of CALLTYPE_INTERNAL, CALLTYPE_INBOUND, or CALLTYPE_OUTBOUND.	*/
	std::wstring m_sCallObjectId; /**< This line's current call object ID. */

	PartyList m_listParties; /**< A list of parties currently participating in this call. */

	CCTITimer* m_pCallDuration; /**< The call duration timer. */
	CCTITimer* m_pHoldDuration; /**< The hold time duration timer. */

	ButtonMap m_mapButtons; /**< A map of this line's buttons. */
	CCTIDialpad m_dialpad; /**< This line's dialpad object. */

	CCTIStatic m_lineStatus;

	std::wstring m_sANI; /**< This line's current ANI (should be empty if there's no call on the line). */

	PARAM_MAP m_mapLineCookies; /**< A map that holds this line's "cookies" -- arbitrary pieces of data which developers can set on lines which do not get saved to the CTI system or Salesforce.com.*/

	CCTIUserInterface* m_pUI; /**< The CCTIUserInterface object that owns this line. */
public:
	/**
	 * Creates a representation of a line with the specified line number.
	 *
	 * @param pUI The CCTIUserInterface object that owns this line.
	 * @param nLineNumber The line number of this line.
	 * @param bFixed True if this line is a fixed line; false if it is an virtual line.
	 */
	CCTILine(CCTIUserInterface* pUI, int nLineNumber, bool bFixed=true);
	virtual ~CCTILine(void);

	/**
	 * Serializes this object to XML.
	 *
	 * @param pXMLDoc The document object (used to create all elements, attributes, etc.).
	 * @return A document fragment that represents this object serialized to XML.
	 */
	virtual MSXML2::IXMLDOMDocumentFragmentPtr SerializeToXML(MSXML2::IXMLDOMDocumentPtr pXMLDoc);

	/**
	 * Gets this line's line number.
	 *
	 * @return This line's line number.
	 */
	virtual int GetLineNumber() { return m_nLineNumber; };

	/**
	 * Sets this line's line number.
	 *
	 * @return This line's line number.
	 */
	virtual void SetLineNumber(int nLineNumber) { m_nLineNumber=nLineNumber; };

	/**
	 * Sets the state of this line.
	 *
	 * @param nLineState The state of this line.  Should be one of LINE_OPEN, LINE_RINGING, LINE_ON_CALL, or LINE_DIALING.	
	 */														
	virtual void SetState(int nLineState);					
	/**
	 * Gets the state of this line.
	 *
	 * @return The state of this line.
	 */
	virtual int GetState();								
	/**
	 * Gets a line state ID string (as used in the XML serialization of this line) from the input numeric state.
	 *
	 * @param nLineState The state for which to get the ID string.  Should be one of LINE_OPEN, LINE_RINGING, LINE_ON_CALL, or LINE_DIALING.	
	 * @return A line state ID string corresponding to the input numeric line state.
	 */
	virtual std::wstring GetStringFromState(int nLineState);

	/**
	 * Returns true if this is a fixed line, false if this is an virtual line.
	 *
	 * @return True if this is a fixed line, false if this is an virtual line.
	 */
	virtual bool GetFixed() { return m_bFixed;} ;

	/**
	 * Sets whether this line's CallAlternate link is available.
	 *
	 * @param bAllowAlternate Should be true if this line's CallAlternate link is available, false if not.
	 */
	virtual void SetAllowAlternate(bool bAllowAlternate)
	{
		m_bAllowAlternate = bAllowAlternate;
		SetAttribute(KEY_ALLOW_ALTERNATE,m_bAllowAlternate);
	};
	/**
	 * Gets whether this line's CallAlternate link is available.
	 *
	 * @return True if this line's CallAlternate link is available.
	 */
	virtual bool GetAllowAlternate()
	{
		return m_bAllowAlternate;
	};

	/**
	 * Sets whether this line's dialpad link is available.  If the dialpad becomes unavailable and is currently shown,
	 * this method will automatically hide it.
	 *
	 * @param bAllowDialpad Should be true if this line's dialpad link is available, false if not.
	 */
	virtual void SetAllowDialpad(bool bAllowDialpad)
	{
		m_bAllowDialpad = bAllowDialpad;
		if (!m_bAllowDialpad) {
			m_dialpad.SetVisible(false);
		}
		SetAttribute(KEY_ALLOW_DIALPAD,m_bAllowDialpad);
	};
	/**
	 * Gets whether this line's dialpad link is available.
	 *
	 * @return True if this line's dialpad link is available.
	 */
	virtual bool GetAllowDialpad()
	{
		return m_bAllowDialpad;
	};

	/**
	 * Gets this line's dialpad object.
	 *
	 * @return This line's dialpad.
	 */
	virtual CCTIDialpad* GetDialpad() { return &m_dialpad; };

	/**
	 * Sets this line's current call type.  Should be one of CALLTYPE_INTERNAL, CALLTYPE_INBOUND, or CALLTYPE_OUTBOUND.
	 *
	 * @param nCallType Should be one of CALLTYPE_INTERNAL, CALLTYPE_INBOUND, or CALLTYPE_OUTBOUND.
	 */
	virtual void SetCallType(int nCallType) { m_nCallType = nCallType; };
	/**
	 * Gets this line's current call type.  
	 *
	 * @return One of CALLTYPE_INTERNAL, CALLTYPE_INBOUND, or CALLTYPE_OUTBOUND.
	 */
	virtual int GetCallType() { return m_nCallType; };

	/**
	 * Sets this line's call object ID.  This should be the unique ID that the CTI server assigns to the call.
	 *
	 * @param sCallObjectId The unique ID that the CTI server assigns to the active call.
	 */
	virtual void SetCallObjectId(std::wstring sCallObjectId);
	/**
	 * Gets this line's current call object ID.
	 */
	virtual std::wstring GetCallObjectId() { return m_sCallObjectId; };
	/**
	 * Adds the default buttons to this line.
	 * Note that all buttons are invisible until a call to EnableButtons is made to make them visible.
	 */
	virtual void AddDefaultButtons();

	/**
	 * Makes all buttons with IDs in the input list visible, and makes any buttons not in the input list invisible.
	 *
	 * @param listButtonIds The list of buttons to make visible.
	 */
	virtual void EnableButtons(std::list<int>& listButtonIds);

	/**
	 * Shows the dialpad for this line.
	 *
	 * @param nDialpadType Must be one of DIALPAD_DIAL, DIALPAD_TRANSFER, or DIALPAD_CONFERENCE.
	 * @param bAllowOneStep True to allow one-step transfer or conference.  Has no effect if type is DIALPAD_DIAL.
	 */
	virtual void ShowDialpad(int nDialpadType, bool bAllowOneStep=false);

	/**
	 * Hides the dialpad for this line.
	 */
	virtual void HideDialpad();

	/**
	 * This method adds a long-style button (which is invisible until a call to EnableButtons makes it visible) to this line.
	 *
	 * @param nId A numeric ID that defines how this adapter refers to the buttons, and also in what position the button will be displayed (so if you specify 15, it'll display between "Answer" and "Hang up", which are 10 and 30 respectively.
	 * @param sMessage The string ID that defines the specific message that is sent when this button is clicked.  There should be a corresponding handler for this command defined in your implementation of CCTIUserInterface::UIHandleMessage.
	 * @param sColor (optional) This button's color.  Should be a 6-letter hex string.
	 * @param sLabel (optional) This button's label.  Should be specified if the label isn't covered by Salesforce.com.
	 *
	 * @return The CCTIButton object that this method created.  Callers can use this return value to further customize the button.
	 */
	virtual CCTIButton* AddLongButton(int nId, std::wstring sMessage, std::wstring sColor, std::wstring sLabel=L"");

	/**
	 * This method adds a short-style button (which is invisible until a call to EnableButtons makes it visible) to this line.
	 *
	 * @param nId A numeric ID that defines how this adapter refers to the buttons, and also in what position the button will be displayed (so if you specify 15, it'll display between "Answer" and "Hang up", which are 10 and 30 respectively.
	 * @param sMessage The string ID that defines the specific message that is sent when this button is clicked.  There should be a corresponding handler for this command defined in your implementation of CCTIUserInterface::UIHandleMessage.
	 * @param sIconURL This button's icon.  Should be an URL to a valid 51x17 (WxH) image, either relative to Salesforce.com (as in /img/btn_hold.gif) or absolute.
	 * @param sLabel (optional) This button's label.  Should be specified if the label isn't covered by salesforce.com.
	 *
	 * @return The CCTIButton object that this method created.  Callers can use this return value to further customize the button.
	 */
	virtual CCTIButton* AddShortButton(int nId, std::wstring sMessage, std::wstring sIconURL, std::wstring sLabel=L"");

	/**
	 * Adds a button object to this line.  Note that when this method is called, CCTILine will take ownership of the CCTIButton
	 * object and take responsibility for destroying it; therefore, the caller should never destroy the CCTIButton object passed
	 * in here.
	 *
	 * @param nId A numeric ID that defines how this adapter refers to the buttons, and also in what position the button will be displayed (so if you specify 15, it'll display between "Answer" and "Hang up", which are 10 and 30 respectively.
	 * @param pButton The button object of the button to add.
	 */
	virtual void AddButton(int nId, CCTIButton* pButton);

	/**
	 * Removes a button from this line.
	 *
	 * @param nId The numeric ID of the button to remove.
	 */
	virtual void RemoveButton(int nId);
	/**
	 * Starts the call duration timer and makes it visible.
	 */
	virtual void StartCallDurationTimer();
	/**
	 * Starts the hold time duration timer makes it visible.
	 */
	virtual void StartHoldTimer();
	/**
	 * Ends the call duration timer and hides it.
	 */
	virtual void EndCallDurationTimer();
	/**
	 * Ends the hold time duration timer and hides it.
	 */
	virtual void EndHoldTimer();

	/**
	 * Resets the line to an open state with no active call.
	 */
	virtual void Reset();
	/**
	 * Returns the total number of seconds that have elapsed during this call, or 0 if no call is currently active.
	 *
	 * @return The total number of seconds that have elapsed during this call, or 0 if no call is currently active.
	 */
	virtual int GetCallDurationSeconds();

	/**
	 * Sets the text of the status bar for the line and makes it visible.
	 *
	 * @param bError True if the status text should be shown as an error, false otherwise.
	 * @param sLineStatusId The ID of the status text for the line.
	 * @param sLineStatusLabel The label of the status text for the line.  Should be specified if Salesforce.com has no translation for the input ID.
	 */
	virtual void ShowLineStatusBar(bool bError, std::wstring sLineStatusId, std::wstring sLineStatusLabel=L"");

	/**
	 * Clears the status bar for the line.
	 */
	virtual void HideLineStatusBar();

	/**
	 * Sets a line cookie, which is an arbitrary piece of data that developers can set on lines for their own use.
	 * Line cookies do not get saved to the CTI system or Salesforce.com, but they will exist for the life of the line.
	 *
	 * @param sKey The key of the cookie to set.
	 * @param sValue The value of the cookie to set.
	 */
	virtual void SetLineCookie(std::wstring sKey, std::wstring sValue)
	{
		m_mapLineCookies[sKey]=sValue;
	};

	/**
	 * Gets the value of a line cookie, which is an arbitrary piece of data that developers can set on lines for their own use.
	 * Line cookies do not get saved to the CTI system or Salesforce.com, but they will exist for the life of the line.
	 *
	 * @param sKey The key of the cookie to get.
	 *
	 * @return The value of the cookie that's associated with the input key, or the empty string if no value has been set for that key.
	 */
	virtual std::wstring GetLineCookie(std::wstring sKey)
	{
		PARAM_MAP::iterator it = m_mapLineCookies.find(sKey);
		if (it!=m_mapLineCookies.end()) return it->second;

		return L"";
	}

	/**
	 * Clears a line cookie.
	 *
	 * @param sKey The key of the line cookie to clear.
	 */
	virtual void ClearLineCookie(std::wstring sKey)
	{
		m_mapLineCookies.erase(sKey);
	}

	/**
	 * Indicates whether this line contains the cookie with the specified key.
	 *
	 * @param sKey The key of the line cookie to clear.
	 *
	 * @return True if this line contains the cookie with the specified key, false otherwise.
	 */
	virtual bool HasLineCookie(std::wstring sKey)
	{
		return (m_mapLineCookies.find(sKey)!=m_mapLineCookies.end());
	}

	/**
	 * Adds a party to this line.
	 *
	 * @param pParty The party to add to this line
	 * @param nLocation (optional) The 1-indexed location that this party should occupy in the line.  If unspecified, the party becomes the last party on the line.
	 */
	virtual void AddParty(CCTIParty* pParty, int nLocation=0);

	/**
	 * Removes a party from this line.
	 *
	 * @param pParty The party to remove.
	 */
	virtual void RemoveParty(CCTIParty* pParty);

	/**
	 * Removes the party associated with the specified ANI, if one exists, from this line.
	 *
	 * @param sANI The ANI associated with the party to remove.
	 */
	virtual void RemovePartyByANI(std::wstring& sANI);

	/**
	 * Removes all the parties from this line.
	 */
	virtual void RemoveAllParties();

	/**
	 * Gets the first party on this line.
	 *
	 * @return The first party on this line, or NULL if this line contains no parties.
	 */
	virtual CCTIParty* GetFirstParty();

	/**
	 * Gets the party with the index nParty from this line.
	 *
	 * @param nParty The 1-indexed number of the party to retrieve.
	 * @return The party at that index, or NULL if there is none there.
	 */
	CCTIParty* GetParty(int nParty);

	/**
	 * Gets the party specified by the given ANI.
	 *
	 * @param sANI The ANI associated with the party to retrieve.
	 * @return The party associated with that ANI, or NULL if there is none.
	 */
	CCTIParty* GetPartyByANI(std::wstring sANI);

	/**
	 * Gets the number of parties currently associated with this line.
	 *
	 * @return The number of parties currently associated with this line.
	 */
	virtual int GetNumberOfParties() { return (int)m_listParties.size(); };
};

typedef std::list<CCTILine*> LineList;