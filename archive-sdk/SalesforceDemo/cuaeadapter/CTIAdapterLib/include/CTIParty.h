#pragma once
#include "ctiobject.h"
#include "ctiinfofield.h"
#include "CtiRelatedObjectSet.h"

class CCTIUserInterface;

class CCTIParty :
	public CCTIObject
{
protected:
	CCTIUserInterface* m_pUI; /**< The user interface object that created and maintains this party. */
	InfoFieldList m_listInfoFields; /**< A list of the info fields associated with this party. */
	RelObjSetList m_listRelatedObjectSets; /**< A list of the related object sets associated with this party. */

	int m_nType; /**< The type of this party. */
	std::wstring m_sANI; /**< The phone number that this party object is associated with. */
public:
	CCTIParty(CCTIUserInterface* pUI,std::wstring sANI,int nPartyType=0);
	virtual ~CCTIParty(void);

	/**
	 * Serializes this object to an XML document fragment.
	 *
	 * @param pXMLDoc The master XML document
	 * @return An XML document fragment representing this object.
	 */
	MSXML2::IXMLDOMDocumentFragmentPtr SerializeToXML(MSXML2::IXMLDOMDocumentPtr pXMLDoc);

	/**
	 * Adds an info field to this party.
	 * Note that once the info field is attached to this party, the party object will own the object from that point forward and
	 * handle deletion of it.  The caller of this method should never delete the CCTIInfoField object that is passed in here.
	 *
	 * @param pInfoField An info field to attach to this party.
	 */
	virtual void AddInfoField(CCTIInfoField* pInfoField);

	/**
	 * Gets the value of the info field with the specified ID.
	 *
	 * @param sId The ID of the info field.
	 * @return The value of the info field, or the empty string if no info field with that ID exists.
	 */
	std::wstring GetInfoFieldValue(std::wstring sId);

	/**
	 * Sets the value of the existing info field with the specified ID.
	 *
	 * @param sId The ID of the info field to set.
	 * @param sValue The info field's new value.
	 *
	 * @return True if the info field exists and its value was successfully set.
	 */
	virtual bool SetInfoFieldValue(std::wstring sId, std::wstring sValue);

	/**
	 * Adds info fields to this party in the order specified by the input info field list.  
	 * One info field will be added for each key in the input map, with its ID being the 
	 * key in the map and its VALUE being the value in the map.
	 *
	 * Note that the keys should correspond exactly with IDs defined in the Salesforce.com XSL servlet.
	 *
	 * @param mapInfoFields A map of key-value pairs to serve as info fields.
	 * @param pListLayoutInfoFields The list of info fields that are specified in the layout.
	 */
	virtual void SetInfoFields(PARAM_MAP& mapInfoFields, StringList* pListLayoutInfoFields);

	/**
	 * Clears all the info fields attached to this party and deletes the corresponding objects.
	 */
	virtual void ClearInfoFields();

	/**
	 * Attaches related object sets from the input list to this party.
	 *
	 * @param listRelatedObjectSets A list of related object sets to attach.
	 */
	virtual void AddRelatedObjectSets(RelObjSetList& listRelatedObjectSets);

	/**
	 * Adds a single related object set to this party.
	 *
	 * @param pSet The related object set to attach to this party.
	 */
	void AddRelatedObjectSet(CCTIRelatedObjectSet* pSet);
	/**
	 * Clears all the related object sets attached to this party and deletes the corresponding objects.
	 */
	virtual void ClearRelatedObjectSets();

	/**
	 * Gets the related object set attached to this party that contains the specified type of object.
	 *
	 * @param sObjectName The name of the object for which to get the set (like "Contact")
	 *
	 * @return The related object set for that object type, or NULL if none exist
	 */
	CCTIRelatedObjectSet* GetRelatedObjectSet(std::wstring& sObjectName);

	/**
	 * Gets the related object sets currently associated with this party.
	 *
	 * @return The related object sets currently associated with this line.
	 */
	virtual RelObjSetList* GetRelatedObjectSets() { return &m_listRelatedObjectSets; };

	/**
	 * Sets the type of this party.  The type should be one of:
	 *
	 * PARTYTYPE_NONE: No party type specified.  This should only occur when this party is the only party on a call.
	 * PARTYTYPE_ORIGINAL_CALLER: Indicates that this party is the original caller that is being transferred or conferenced.
	 * PARTYTYPE_TRANSFERRED_FROM: Indicates that the party is the transferrer of a call.
	 * PARTYTYPE_CONFERENCED_FROM: Indicates that the party is the conferencer of a call.
	 * PARTYTYPE_CONFERENCED_PARTY: Indicates that the party is a member of a conference call currently in progress.
	 *
	 * @param nType The type of this party.
	 */
	virtual void SetType(int nType);

	/**
	 * @return The type of this party.
	 */
	virtual int GetType() { return m_nType; };

	/**
	 * Set this party's current ANI.
	 *
	 * @param sANI This party's current ANI.
	 */
	virtual void SetANI(std::wstring sANI) { 
		m_sANI = sANI; 
		SetAttribute(KEY_ANI,sANI);
	};
	/**
	 * Get this party's current ANI.
	 *
	 * @return This party's current ANI.
	 */
	virtual std::wstring GetANI() { return m_sANI; };
};

typedef std::list<CCTIParty*> PartyList;