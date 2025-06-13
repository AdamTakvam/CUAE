#pragma once
#include "CTIIdLabelObject.h"
#include "CTIRelatedObjectSet.h"
#include "CTIRelatedObject.h"
#include "CTIUtils.h"

class CCTILine;
class CCTIUserInterface;

typedef std::map<std::wstring,StringPair> StringToPairMap;

/**
 * This is a convenience class that serves to hold data about a who or what entry in a call log.
 */
class CCTIWhoWhat : public CCTIObject
{
protected:
	bool m_bSelected; /**< Flag indicating whether this who/what is currently selected. */
	int m_nWhoOrWhat;  /**< Flag indicating whether this is a who or a what.  Must be one of LOG_OBJECTTYPE_WHO or LOG_OBJECTTYPE_WHAT.*/
public:
	/**
	 * Constructor for CCTIWhoWhat.  All the string parameters are stored as attributes using SetAttribute.
	 *
	 * @param nWhoOrWhat Flag that indicates whether this object is a who or a what.  Must be one of LOG_OBJECTTYPE_WHO or LOG_OBJECTTYPE_WHAT.
	 * @param sId The ID of the object.
	 * @param sObjectLabel The label of the object (like "Contact"). 
	 * @param sObjectName The name of the object (like "Harry Smith").
	 * @param sEntityName The name of the entity in the API (like "Contact", or "Issue__c" for a custom object called "Issue").
	 */
	CCTIWhoWhat(int nWhoOrWhat,std::wstring sId,std::wstring sObjectLabel,std::wstring sObjectName,std::wstring sEntityName);

	/**
	 * Sets whether this who/what is currently selected.
	 *
	 * @param bSelected True if this who/what is currently selected.
	 */
	virtual void SetSelected(bool bSelected);
	/**
	 * Gets whether this who/what is currently selected.
	 *
	 * @return True if this who/what is currently selected.
	 */
	virtual bool GetSelected() { return m_bSelected; };

	/**
	 * Gets the entity name of this who/what.
	 *
	 * @return The entity name of this who/what.
	 */
	virtual std::wstring GetObjectType() { return GetAttribute(KEY_ENTITY_NAME); };

	/**
	 * Returns the who tag if this object is a who, and the what tag if it's a what.
	 *
	 * @return This object's tag name when it's serialized to XML.
	 */
	virtual _bstr_t GetTag() { return m_nWhoOrWhat==LOG_OBJECTTYPE_WHO?"CTIWho":"CTIWhat"; };
};

 typedef std::map<std::wstring,CCTIWhoWhat*> WhoWhatMap;

/**
 * @version 1.0
 * 
 * This class represents a call log for a current or previous call in the Salesforce.com CTI user interface.  The ID field of this
 * class (inherited from CCTIIdLabelObject) is the ID of the task that's created for this log (or the empty string if none has been created); 
 * the LABEL field of this class is the Subject field of the task that was created.
 */
class CCTICallLog :
	public CCTIIdLabelObject
{
protected:
	bool m_bOpen; /**< Indicates whether this call log is open or "rolled up."   Only applicable to current call logs, not previous ones. */  
	bool m_bSaveOnEnd; /**< The call log gets created when the call begins, but it should only get saved after the call is established.  This flag should be set after the call is established to indicate that the call should indeed be saved. */  
	int m_nLineNumber; /**< The line number that this call log is associated with. */

	WhoWhatMap m_mapWhos; /**< A map of IDs to the names of the available whos (which comprise Contacts or Leads) for this log. */
	WhoWhatMap m_mapWhats; /**< A map of IDs to the names of the available whats (which comprise every object other than Contacts or Leads) for this log. */

	CCTIWhoWhat m_whoNone; /**< A placeholder object for the "blank who." */
	CCTIWhoWhat m_whatNone; /**< A placeholder object for the "blank what." */

	std::wstring m_sSelectedWhoId; /**< The ID of the who which is presently selected. */
	std::wstring m_sSelectedWhatId; /**< The ID of the what which is presently selected. */
	std::wstring m_sComments; /**< The comments associated with this log. */
	std::wstring m_sCallDisposition; /**< The call disposition (aka wrapup code) of the call that this log is associated with. */

	bool m_bDirty; /**< Flag indicating whether this call log has changed since it was last upserted. */
	bool m_bCallIsActive; /**< Flag indicating whether this call log is associated with a currently active call (that data can be attached to). */

	/**
	 * Attaches the whoId and whatId to the call object itself.  This only works if the underlying CCTIUserInterface subclass
	 * has implemented attachments.
	 */
	virtual void AttachLogObjectsToCall();

	int m_nCallDuration; /**< The duration of a call.  This should only be set for calls that have completed.*/
	std::wstring m_sCallObjectId; /**< The call object ID that this log pertains to. */
	int m_nCallType; /**< The call type of the call that this log pertains to. */

	CCTIUserInterface* m_pUI; /**< The CCTIUserInterface that created this log. */
public:
	/**
	 * @param sLabel The label of the task that will be created for this call log
	 * @param nLineNumber The line number of this call log
	 * @param bOpen True if this call log should be open
	 */
	CCTICallLog(CCTIUserInterface* pUI, std::wstring sLabel, int nLineNumber, bool bOpen);
	virtual ~CCTICallLog();
	/**
	 * Sets the open state of this call log.  Only applicable to current call logs, not previous ones.
	 *
	 * @param bOpen True if the call log should be open, false if it's "rolled up."
	 */
	virtual void SetOpen(bool bOpen);
	/**
	 * Gets the open state of this call log.  Only applicable to current call logs, not previous ones.
	 *
	 * @return True if the call log is open, false if it's "rolled up."
	 */
	virtual bool GetOpen();

	/**
	 * Sets the SaveOnEnd state of this call log.  This flag should be set once the call has been established to indicate that
	 * the log should be saved when the call is ended.  Only applicable to current call logs, not previous ones.
	 *
	 * @param bSaveOnEnd True if the call log should save itself to the AppExchange API when the call is ended.
	 */
	virtual void SetSaveOnEnd(bool bSaveOnEnd)
	{
		m_bSaveOnEnd = bSaveOnEnd;	
	};
	/**
	 * Gets the SaveOnEnd state of this call log.  Only applicable to current call logs, not previous ones.
	 *
	 * @return True if the call log should save itself to the AppExchange API when the call is ended.
	 */
	virtual bool GetSaveOnEnd() { return m_bSaveOnEnd; };

	/**
	 * Sets the line number that this call log is associated with.  Only applicable to current call logs, not previous ones.
	 *
	 * @param pLine The line number this log should be associated with.
	 */
	virtual void SetLineNumber(int nLineNumber);
	/**
	 * Gets the line number that this call log is associated with.
	 *
	 * @return The line number that this log is associated with, or 0 if no such line exists or this call log is unassociated with a line.
	 */
	virtual int GetLineNumber();

	/**
	 * Gets the XML tag associated with this object.
	 *
	 * @return The XML tag associated with this object.
	 */
	virtual _bstr_t GetTag() { return "CTICallLog"; };
	
	/**
	 * Serializes this object to XML.
	 *
	 * @param pXMLDoc The document object (used to create all elements, attributes, etc.).
	 * @return A document fragment that represents this object serialized to XML.
	 */
	virtual MSXML2::IXMLDOMDocumentFragmentPtr SerializeToXML(MSXML2::IXMLDOMDocumentPtr pXMLDoc);

	/**
	 * Returns true if the specified ID refers to a "who" object (one with an ID starting with 003 or 00Q),
	 * and false otherwise.  An object that is not a "who" is, by definition, a "what."
	 *
	 * @param sId The ID of the object.
	 * @return True if the specified ID refers to a "who" object.
	 */
	static bool IsAWho(std::wstring& sId);

	/**
	 * Returns true if the specified ID refers to a Lead object (one with an ID starting with 00Q),
	 * and false otherwise.
	 *
	 * @param sId The ID of the object.
	 * @return True if the specified ID refers to a Lead object.
	 */
	static bool IsALead(std::wstring& sId);

	/**
	 * Returns true if the specified ID refers to a User object (one with an ID starting with 005),
	 * and false otherwise.
	 *
	 * @param sId The ID of the object.
	 * @return True if the specified ID refers to a User object.
	 */
	static bool IsAUser(std::wstring& sId);

	/**
	 * Adds an object to the list of available whos or whats and makes it selected in that dropdown.
	 * The object is a "who" if it is a Contact or Lead (so its ID starts with 003 or 00Q), otherwise it's a what.
	 *
	 * @param sId The ID of the object to add.
	 * @param sObjectLabel The label of the object to add (like "Contact").
	 * @param sObjectName The object name of the object to add (like "Harry Williams")
	 * @param sEntityName The name of the entity in the API (like "Contact", or "Issue__c" for a custom object called "Issue").
	 * @param bAttachToCall (optional) True if this object should be attached to the call after it's added to the log.
	 * @param bAutoSelect (optional) True if this object should be auto selected after it's added to the log. 
	 *
	 * @return LOG_OBJECTTYPE_WHO if the added object is a who, LOG_OBJECTTYPE_WHAT if it is a what, or NULL if it could not be added 
	 */
	virtual int AddObject(std::wstring& sId, std::wstring& sObjectLabel, std::wstring& sObjectName, std::wstring& sEntityName, bool bAttachToCall=true, bool bAutoSelect=true);

	/**
	 * Adds all the related objects in the input list of related object sets to the dropdowns of this
	 * call log, to a maximum of LOG_MAXIMUM_RELATED_OBJECTS per dropdown.
	 *
	 * @param listRelObjSets A list of related object sets
	 */
	void AddRelatedObjects(RelObjSetList* listRelObjSets);

	/**
	 * Selects the object with the given ID in the appropriate box (either who or what).
	 * The object selected is a "who" if it is a Contact or Lead (so its ID starts with 003 or 00Q), 
	 * otherwise it's a what.  The object to be selected must already be an option in 
	 *
	 * @param sId The ID of the object to be selected.
	 * @param bAttachToCall (optional) True if the newly selected value should be attached to the call.
	 */
	virtual void SelectObject(std::wstring sId, bool bAttachToCall=true);

	/**
	 * Deselects whatever object is selected in the who or what dropdown.
	 *
	 * @param nWhoOrWhat Determines which dropdown to deselect.  Must be one of LOG_OBJECTTYPE_WHO or LOG_OBJECTTYPE_WHAT.
	 */
	virtual void DeselectObject(int nWhoOrWhat);

	/**
	 * Sets the duration of the call that this log is associated with.  This method should be called after the phone
	 * call completes, but prior to the creation of the Task object for this call log.
	 *
	 * @param nCallDuration The call duration in seconds.
	 */
	virtual void SetCallDuration(int nCallDuration) { m_nCallDuration = nCallDuration; };
	
	/**
	 * @return The duration of the call that this log is associated with.
	 */
	virtual int GetCallDuration() { return m_nCallDuration; };

	/**
	 * Sets the call object ID of the call that this log is associated with.  This method should be called
	 * prior to the creation of the Task object for this call log.
	 *
	 * @param sCallObjectId The call object ID.
	 */
	virtual void SetCallObjectId(std::wstring sCallObjectId);
	
	/**
	 * @return The call object ID of the call that this log is associated with.
	 */
	virtual std::wstring GetCallObjectId();

	/**
	 * Sets the call disposition (aka wrapup code) of the call that this log is associated with.  
	 * This method should be called prior to the creation of the Task object for this call log.
	 *
	 * @param sCallDisposition The call object ID.
	 */
	virtual void SetCallDisposition(std::wstring sCallDisposition) { 
		m_sCallDisposition = sCallDisposition; 
	};
	
	/**
	 * @return The call disposition (aka wrapup code) of the call that this log is associated with.
	 */
	virtual std::wstring GetCallDisposition() { return m_sCallDisposition; };

	/**
	 * Sets whether this log has changed since it was last saved to the AppExchange API.
	 *
	 * @param bDirty True if this log has changed since it was last saved.
	 */
	void SetDirty(bool bDirty);

	/**
	 * Gets whether this log has changed since it was last saved to the AppExchange API.
	 *
	 * @return True if this log has changed since it was last saved.
	 */
	bool GetDirty();

	/**
	 * Sets whether this log is associated with a currently active call (to which data can be attached).
	 *
	 * @param bCallIsActive True if this log is associated with a currently active call.
	 */
	void SetCallIsActive(bool bCallIsActive);

	/**
	 * Gets whether this log is associated with a currently active call (to which data can be attached).
	 *
	 * @return True if this log is associated with a currently active call.
	 */
	bool GetCallIsActive();

	/**
	 * Sets the comments of this log.
	 *
	 * @param sComments The comments.
	 */
	virtual void SetComments(std::wstring sComments);
	
	/**
	 * @return The comments of this log.
	 */
	virtual std::wstring GetComments() { return m_sComments; };

	/**
	 * @return The selected who ID of this log.
	 */
	virtual std::wstring GetSelectedWhoId() { return m_sSelectedWhoId; };

	/**
	 * @return The selected what ID of this log.
	 */
	virtual std::wstring GetSelectedWhatId() { return m_sSelectedWhatId; };

	/**
	 * Sets this call type of the call this log pertains to.  
	 * Should be one of CALLTYPE_INTERNAL, CALLTYPE_INBOUND, or CALLTYPE_OUTBOUND.
	 *
	 * @param nCallType Should be one of CALLTYPE_INTERNAL, CALLTYPE_INBOUND, or CALLTYPE_OUTBOUND.
	 */
	virtual void SetCallType(int nCallType) { m_nCallType = nCallType; };

	/**
	 * Gets the call type of the call this log pertains to.  
	 *
	 * @return One of CALLTYPE_INTERNAL, CALLTYPE_INBOUND, or CALLTYPE_OUTBOUND.
	 */
	virtual int GetCallType() { return m_nCallType; };
};

typedef std::list<CCTICallLog*> CallLogList;