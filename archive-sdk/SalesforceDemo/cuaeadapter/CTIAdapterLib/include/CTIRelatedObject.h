#pragma once
#include "ctiidlabelobject.h"
#include "CTIField.h"
#include <map>

/**
 * @version 1.0
 * 
 * This class represents a single related object (like a Contact or Account) in the Salesforce.com CTI user interface.
 */
class CCTIRelatedObject :
	public CCTIIdLabelObject
{
public:
	CCTIRelatedObject(std::wstring sId, std::wstring sLabel);
	~CCTIRelatedObject(void);

	/**
	 * Gets the XML tag associated with this object.
	 *
	 * @return The XML tag associated with this object.
	 */
	virtual _bstr_t GetTag() { return "CTIRelatedObject"; };

	/**
	 * Adds a field to this related object.
	 *
	 * @param pField The field to add.
	 */
	virtual void AddField(CCTIField* pField);

	/**
	 * Gets the value of the field bearing the Id of the input parameter.
	 *
	 * @param sFieldId The Id of the field whose value to retrieve.
	 * @return The value of the field, or the empty string if the field does not exist.
	 */
	virtual std::wstring GetFieldValue(std::wstring sFieldId);

	/**
	 * This method takes this object and serializes it to XML.
	 *
	 * @param pXMLDoc The document object (used to create all elements, attributes, etc.)
	 * @return A document fragment that represents this object serialized to XML.
	 */
	virtual MSXML2::IXMLDOMDocumentFragmentPtr SerializeToXML(MSXML2::IXMLDOMDocumentPtr pXMLDoc);
protected:
	std::wstring m_sName; /**< The name of this related object (like "Harry Smith") */
	FieldList m_listFields; /**< The fields in this related object. */
};

typedef std::list<CCTIRelatedObject*> RelObjectList;
typedef std::map<std::wstring,CCTIRelatedObject*> RelObjectMap;