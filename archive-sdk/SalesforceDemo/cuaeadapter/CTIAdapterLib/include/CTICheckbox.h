#pragma once
#include "ctiidlabelobject.h"

/**
 * @version 1.0
 * 
 * This class represents a checkbox in the Salesforce.com CTI user interface.
 */
class CCTICheckbox :
	public CCTIIdLabelObject
{
public:
	CCTICheckbox(std::wstring sId, std::wstring sLabel=L"");
	CCTICheckbox(void);
	virtual ~CCTICheckbox(void);
	/**
	 * Gets the XML tag associated with this object.
	 *
	 * @return The XML tag associated with this object.
	 */
	virtual _bstr_t GetTag() { return "CTICheckbox"; };

	virtual void SetChecked(bool bChecked)
	{
		m_bChecked = bChecked;
		SetAttribute(KEY_CHECKED,m_bChecked);
	};

	virtual bool GetChecked() { return m_bChecked; };

protected:
	bool m_bChecked;
};
