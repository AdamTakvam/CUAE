#pragma once
#include "CTIValueObject.h"

/**
 * @version 1.0
 * 
 * This class represents a single field of a CTIRelatedObject in the Salesforce.com CTI user interface.
 */
class CCTIField :
	public CCTIValueObject
{
public:
	CCTIField(std::wstring sId, std::wstring sLabel,std::wstring sValue);
	virtual ~CCTIField(void);

	/**
	 * Gets the XML tag associated with this object.
	 *
	 * @return The XML tag associated with this object.
	 */
	virtual _bstr_t GetTag() { return "CTIField"; };

	/**
	 * Sets whether this field should be rendered as a checkbox.  If this is set to true, the field will be 
	 * rendered as a checked checkbox if the value is the string "true", an an unchecked checkbox otherwise.
	 *
	 * @param bCheckbox True if this field should be rendered as a checkbox.
	 */
	virtual void SetCheckbox(bool bCheckbox) {
		SetAttribute(KEY_CHECKBOX,bCheckbox);
	}
};

typedef std::list<CCTIField*> FieldList;