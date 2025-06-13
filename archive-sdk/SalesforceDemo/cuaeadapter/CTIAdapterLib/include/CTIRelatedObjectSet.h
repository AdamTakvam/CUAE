#pragma once
#include "ctiidlabelobject.h"
#include "CTIRelatedObject.h"

/**
 * @version 1.0
 * 
 * This class represents a set of like related objects (like Contacts or Accounts) in the Salesforce.com CTI user interface.
 */
class CCTIRelatedObjectSet :
	public CCTIIdLabelObject
{
protected:
	RelObjectList m_listRelatedObjects; /**< The list of related objects (kept for the purposes of maintaining their order) */
	RelObjectMap m_mapRelatedObjects; /**< The id-to-pointer hashmap of related objects (kept for the purposes of the Merge method) */

	std::wstring m_sSingularLabel; /**< The singular label of this set's object type. */
	std::wstring m_sPluralLabel; /**< The plural label of this set's object type. */
public:
	CCTIRelatedObjectSet();
	CCTIRelatedObjectSet(std::wstring sId, std::wstring sSingularLabel, std::wstring sPluralLabel);
	virtual ~CCTIRelatedObjectSet(void);

	/**
	 * Sets the singular label of this set's object type.
	 *
	 * @param sSingularLabel The singular label of this set's object type.
	 */
	virtual void SetSingularLabel(std::wstring& sSingularLabel) { m_sSingularLabel = sSingularLabel; };
	/**
	 * Gets the singular label of this set's object type.
	 *
	 * @return The singular label of this set's object type.
	 */
	virtual std::wstring GetSingularLabel() { return m_sSingularLabel; };

	/**
	 * Sets the plural label of this set's object type.
	 *
	 * @param sPluralLabel The plural label of this set's object type.
	 */
	virtual void SetPluralLabel(std::wstring& sPluralLabel) { m_sPluralLabel = sPluralLabel; };
	/**
	 * Gets the plural label of this set's object type.
	 *
	 * @return The plural label of this set's object type.
	 */
	virtual std::wstring GetPluralLabel() { return m_sPluralLabel; };

	/**
	 * Gets the entity name of this set's object type.
	 *
	 * @return The entity name of this set's object type.
	 */
	virtual std::wstring GetObjectName() { return GetId(); };

	/**
	 * Gets the XML tag associated with this object.
	 *
	 * @return The XML tag associated with this object.
	 */
	virtual _bstr_t GetTag() { return "CTIRelatedObjectSet"; };

	/**
	 * This method takes this object and serializes it to XML.
	 *
	 * @param pXMLDoc The document object (used to create all elements, attributes, etc.)
	 * @return A document fragment that represents this object serialized to XML.
	 */
	virtual MSXML2::IXMLDOMDocumentFragmentPtr SerializeToXML(MSXML2::IXMLDOMDocumentPtr pXMLDoc);

	/**
	 * Returns the list of related objects that this set contains.
	 *
	 * @return The list of related objects that this set contains.
	 */
	virtual RelObjectList* GetRelatedObjects() { return &m_listRelatedObjects; };

	/**
	 * Returns the number of related objects that this set contains.
	 *
	 * @return The number of related objects that this set contains.
	 */
	virtual int GetRelatedObjectCount() { return (int)m_listRelatedObjects.size(); };

	/**
	 * Adds a related object to this set.
	 *
	 * @param pRelatedObject The related object to add.
	 */
	virtual void AddRelatedObject(CCTIRelatedObject* pRelatedObject);

	/**
	 * Returns the first object in this set, or NULL if this set is empty.
	 * This method is handy for reporting purposes, when we can only attach an activity to one member of the set.
	 *
	 * @return The first object in this set, or the empty string if this set is empty.
	 */
	virtual CCTIRelatedObject* GetFirstObject();

	/**
	 * Returns true if this set contains the related object referred to by the specified ID, false otherwise.
	 *
	 * @param sId The ID of the related object to search for.
	 * @return True if this set contains the related object referred to by the specified ID, false otherwise.
	 */
	virtual bool ContainsRelatedObject(std::wstring sId);

	/**
	 * Merges the contents of another related object set of the same type into this one, eliminating any duplicates.
	 * Note that this method call will "steal" any non-duplicate related objects from the input parameter set, leaving
	 * only duplicate items in it.  It is expected that this object will be the primary object after this method call,
	 * and the input parameter set will be disposed of.
	 *
	 * @param pSet The related object set to merge into this one.
	 */
	virtual void Merge(CCTIRelatedObjectSet* pSet);
};

typedef std::list<CCTIRelatedObjectSet*> RelObjSetList;
