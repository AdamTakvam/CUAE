#pragma once
#include "CTIValueObject.h"

class CCTIInfoField :
	public CCTIValueObject
{
public:
	CCTIInfoField(std::wstring sId, std::wstring sLabel=L"");
	~CCTIInfoField(void);

	/**
	 * Gets the XML tag associated with this object.
	 *
	 * @return The XML tag associated with this object.
	 */
	virtual _bstr_t GetTag() { return "CTIInfoField"; };

	/**
	 * Sets whether this field should be rendered as a checkbox.  If this is set to true, the field will be 
	 * rendered as a checked checkbox if the value is the string "true", an an unchecked checkbox otherwise.
	 *
	 * @param bCheckbox True if this field should be rendered as a checkbox.
	 */
	virtual void SetCheckbox(bool bCheckbox) {
		SetAttribute(KEY_CHECKBOX,bCheckbox);
	}

	/**
	 * Sets the HREF of this info field.  If this URL is set to a value, then the info field
	 * will be rendered as a link to the desired URL.  If the URL is set to empty then the info field
	 * will be rendered as plain text.
	 *
	 * @param sUrl The URL to set the info field link to, or empty if the info field should be rendered as plain text.
	 */
	virtual void SetHREF(std::wstring sUrl) {
		if (!sUrl.empty()) {
			SetAttribute(KEY_HREF,sUrl);
		} else {
			RemoveAttribute(KEY_HREF);
		}
	}
};

typedef std::list<CCTIInfoField*> InfoFieldList;