#pragma once
#import <msxml6.dll>

#include <map>
#include <string>
#include <list>
#include <strstream>

#include "CTIUtils.h"
#include "CTILogger.h"
#include "CriticalSection.h"
#include "CTIConstants.h"

/**
 * @version 1.0
 * 
 * This class is the base class for all visual objects in the Salesforce.com CTI user interface.  It provides such common functionality
 * as visibility, attribute handling, and simple serialization.
 */
class CCTIObject : public CCriticalSection
{
public:
	CCTIObject();
	virtual ~CCTIObject();
	
	/**
	 * Sets an attribute on this object.  Setting an attribute will cause it to be emitted as an XML attribute of this object
	 * when SerializeToXML is called.
	 *
	 * @param sName The name of the attribute.
	 * @param sValue The value of the attribute.
	 */
	virtual void SetAttribute(std::wstring sName, std::wstring sValue);

	/**
	 * Sets a wchar_t* attribute on this object.  Setting an attribute will cause it to be emitted as an XML attribute of this object
	 * when SerializeToXML is called.
	 *
	 * @param sName The name of the attribute.
	 * @param sValue The value of the attribute.
	 */
	virtual void SetAttribute(std::wstring sName, const wchar_t* sValue);

	/**
	 * Sets a boolean attribute on this object.  Setting an attribute will cause it to be emitted as an XML attribute of this object
	 * when SerializeToXML is called.
	 *
	 * @param sName The name of the attribute.
	 * @param bValue The value of the attribute.
	 */
	virtual void SetAttribute(std::wstring sName, bool bValue);

	/**
	 * Sets a integer attribute on this object.  Setting an attribute will cause it to be emitted as an XML attribute of this object
	 * when SerializeToXML is called.
	 *
	 * @param sName The name of the attribute.
	 * @param nValue The value of the attribute.
	 */
	virtual void SetAttribute(std::wstring sName, int nValue);

	/**
	 * Removes an attribute from this object.
	 *
	 * @param sName The name of the attribute to remove.
	 */
	virtual void RemoveAttribute(std::wstring sName);

	/**
	 * Gets the value of an attribute on this object.
	 *
	 * @param sName The name of the attribute.
	 * @return The value of the input attribute, or the empty string if that attribute is not present.
	 */
	virtual std::wstring GetAttribute(std::wstring sName);

	/**
	 * Sets the visiblility of this object.
	 * If this object is not visible, then it will not be emitted in XML.
	 *
	 * @param bVisible The visibility of this object.
	 */
	virtual void SetVisible(bool bVisible);
	/**
	 * Gets the visibility of this object.
	 *
	 * @return The visibility of this object.
	 */
	virtual bool GetVisible();

	/**
	 * This method takes this object and serializes it to XML.  By default, it just generates a document fragment
	 * containing a single element with the tag defined in GetTag() and the attributes that have been attached to this
	 * object.  Any objects with more complicated features (like subtags) should override this method to serialize themselves
	 * as they see fit.
	 *
	 * @param pXMLDoc The document object (used to create all elements, attributes, etc.).
	 * @return A document fragment that represents this object serialized to XML.
	 */
	virtual MSXML2::IXMLDOMDocumentFragmentPtr SerializeToXML(MSXML2::IXMLDOMDocumentPtr pXMLDoc);

	/**
	 * Adds the attributes of this object as XML attributes attached to the input XML element.
	 *
	 * @param pXMLDoc The document object (used to create all elements, attributes, etc.)
	 * @param pElement The element on which to attach the attributes.
	 */
	virtual void AddAttributesToElement(MSXML2::IXMLDOMDocumentPtr pXMLDoc, MSXML2::IXMLDOMElementPtr pElement);

	/**
	 * Appends the XML of the child object (if the child object is visible) to the element passed in.
	 *
	 * @param pXMLDoc The document object (used to create all elements, attributes, etc.).
	 * @param pXmlElement The element to which the child object's XML should be added.
	 * @param pChild The child object to process.
	 */
	virtual void AddChildIfVisible(MSXML2::IXMLDOMDocumentPtr pXMLDoc, MSXML2::IXMLDOMElementPtr pXmlElement, CCTIObject* pChild);

	/**
	 * This method should be overridden by any subclass which intends to use the default SerializeToXML method.
	 *
	 * @return This object's tag name when it's serialized to XML.
	 */
	virtual _bstr_t GetTag() { return "CTIObject"; };
protected:
	PARAM_MAP m_mapAttributes; /**< The map containing this object's attributes. */

	bool m_bVisible; /**< Flag indicating whether this object is visible (i.e. emitted in XML) */
};

typedef std::list<CCTIObject*> ObjectList;
