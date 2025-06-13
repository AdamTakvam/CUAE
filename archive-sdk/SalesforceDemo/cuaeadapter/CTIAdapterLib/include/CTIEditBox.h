#pragma once
#include "CTIValueObject.h"

/**
 * @version 1.0
 * 
 *  This class represents an edit box in the Salesforce.com CTI user interface.
 */
class CCTIEditBox :
	public CCTIValueObject
{
public:
	CCTIEditBox(std::wstring sId, std::wstring sLabel=L"");
	virtual ~CCTIEditBox(void);

	/**
	 * Gets the XML tag associated with this object.
	 *
	 * @return The XML tag associated with this object.
	 */
	virtual _bstr_t GetTag() { return "CTIEditBox"; };

	/**
	 * Sets the PASSWORD attribute of this edit box.
	 * If the PASSWORD attribute is set to KEY_TRUE then the edit box will render itself as a password input, masking characters as they are entered.
	 *
	 * @param bPassword The value to set the PASSWORD attribute to.
	 */
	virtual void SetPassword(bool bPassword)
	{
		m_bPassword = bPassword;
		SetAttribute(KEY_PASSWORD,m_bPassword);
	};

	/**
	 * Gets the PASSWORD attribute of this edit box.
	 */
	virtual bool GetPassword() { return m_bPassword; };
protected:
	bool m_bPassword; /**< The current state of the PASSWORD attribute. */
};
