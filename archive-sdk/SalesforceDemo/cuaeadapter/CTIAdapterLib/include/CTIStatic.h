#pragma once
#include "ctiidlabelobject.h"

/**
 * @version 1.0
 * 
 *  This class represents a static text element in the Salesforce.com CTI user interface.
 */
class CCTIStatic :
	public CCTIIdLabelObject
{
public:
	CCTIStatic(std::wstring sId=L"", std::wstring sLabel=L"");
	~CCTIStatic(void);

	/**
	 * Gets the XML tag associated with this object.
	 *
	 * @return The XML tag associated with this object.
	 */
	virtual _bstr_t GetTag() { return "CTIStaticText"; };

	/**
	 * Sets whether this static is an error message.
	 *
	 * @param bError Should be true if this static is an error message, false if not.
	 */
	virtual void SetError(bool bError) {
		m_bError = bError;
		SetAttribute(KEY_ERROR,bError);
	};

	/**
	 * Gets whether this static is an error message.
	 *
	 * @return The error state of this error message.
	 */
	virtual bool GetError() { return m_bError; };
protected:
	bool m_bError;
};
