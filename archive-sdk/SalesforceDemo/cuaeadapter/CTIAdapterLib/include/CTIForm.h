#pragma once
#include "ctiobject.h"
#include "ctiidlabelobject.h"
#include <map>
#include <string>
#include <typeinfo>

class CCTIEditBox;
class CCTICheckbox;
class CCTIButton;
class CCTIStatic;
class CCTIInfoField;

/** This class represents a CTIForm object, which contains a set of logically related children.
 * @version 1.0
 *
 * This class represents a CTIForm object, which contains a set of logically related children.
 * These children can be combo boxes, edit boxes, checkboxes, buttons, etc.
 * Ultimately, when this form is rendered in the context of the Salesforce.com page, it will behave such that
 * when a user presses a button, the values of all the other elements of the form will be sent along with it.
 */
class CCTIForm :
	public CCTIObject
{
public:
	CCTIForm(void);
	virtual ~CCTIForm(void);

	/**
	 * Serializes this form and all its children to XML.  This form's children will be serialized in exactly the order
	 * in which they were added.
	 *
	 * @param pXMLDoc The parent XML DOM document in whose context this is being serialized.
	 * @return A document fragment containing this form and its children.
	 */
	MSXML2::IXMLDOMDocumentFragmentPtr SerializeToXML(MSXML2::IXMLDOMDocumentPtr pXMLDoc);

	/**
	 * Adds a child CTIObject to this form.  Bear in mind that the children of the form will ultimately be serialized in exactly
	 * the order in which they were added.
	 *
	 * @param pChild The child object to add to this form.
	 */
	virtual void AddChild(CCTIIdLabelObject* pChild);
	
	/**
	 * Gets the XML tag associated with this object.
	 *
	 * @return The XML tag associated with this object.
	 */
	virtual _bstr_t GetTag() { return "CTIForm"; };

	/**
	 * Creates a new static text object and adds it as a child of this form. 
	 *
	 * @param sId The ID of the static.
	 * @param sLabel (optional) The label of the static in case its ID cannot be translated to a string.
	 *
	 * @return The static object that was created.
	 */
	virtual CCTIStatic* AddStatic(std::wstring sId, std::wstring sLabel=L"");

	/**
	 * Creates a new info field and adds it as a child of this form. 
	 *
	 * @param sId The ID of the info field.
	 * @param sLabel (optional) The label of the info field in case its ID cannot be translated to a string.
	 * @param sValue (optional) The initial value displayed in the info field.
	 *
	 * @return The info field object that was created.
	 */
	virtual CCTIInfoField* AddInfoField(std::wstring sId, std::wstring sLabel=L"", std::wstring sValue=L"");

	/**
	 * Creates a new edit box and adds it as a child of this form. 
	 *
	 * @param sId The ID of the edit box.
	 * @param sLabel (optional) The label of the edit box in case its ID cannot be translated to a string.
	 * @param sValue (optional) The initial value displayed in the edit box.
	 *
	 * @return The edit box object that was created.
	 */
	virtual CCTIEditBox* AddEditBox(std::wstring sId, std::wstring sLabel=L"", std::wstring sValue=L"");

	/**
	 * Creates a new checkbox and adds it as a child of this form.
	 *
	 * @param sId The ID of the checkbox.
	 * @param sLabel (optional) The label of the checkbox in case its ID cannot be translated to a string.
	 * @param bChecked (optional) The checked state of the checkbox.  Defaults to false.
	 *
	 * @return The checkbox object that was created.
	 */
	virtual CCTICheckbox* AddCheckbox(std::wstring sId, std::wstring sLabel=L"", bool bChecked=false); 

	/**
	 * Creates a new button and adds it as a child of this form.
	 *
	 * @param sId The ID of the button.
	 * @param sLabel (optional) The label of the button in case its ID cannot be translated to a string.
	 *
	 * @return The button object that was created.
	 */
	virtual CCTIButton* AddButton(std::wstring sId, std::wstring sLabel=L""); 

	/**
	 * Populates the data for the children of this form.  All children should have first been added to this form; items in the map with
	 * IDs that don't refer to children of this form will be ignored (although a TRACE message will be generated).
	 * The input map should be of the form:
	 *
	 * key = object ID
	 * value = object value
	 *
	 * So, for instance, if you want the edit box called KEY_PERIPHERAL_ID to contain the value KEY_1000, then
	 * mapFormData[KEY_PERIPHERAL_ID] should be set to KEY_1000.
	 *
	 * The use of this method differs a bit depending on the 
	 *
	 * Any IDs in the input map that are not actually children of this form will be ignored.
	 *
	 * @param mapFormData A map containing IDs of the pertinent objects as keys and values for those objects as values.
	 */
	virtual void PopulateFormData(PARAM_MAP& mapFormData);

	/**
	 * Gets the child object of this form corresponding to the input ID.
	 *
	 * @param sId The id of the child object to find.
	 * @return The child object corresponding to the input ID, or NULL if none is found.
	 */
	virtual CCTIIdLabelObject* GetChildById(std::wstring& sId);
protected:
	IdLabelObjectList m_listChildren; /**< A list of the child objects. */
};
