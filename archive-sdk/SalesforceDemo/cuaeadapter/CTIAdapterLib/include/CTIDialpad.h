#pragma once
#include "ctiobject.h"

/**
 * @version 1.0
 * 
 * This class represents a dialpad (a visual element through which a user can dial a number or transfer or conference a call) in the Salesforce.com CTI user interface.
 */
class CCTIDialpad :
	public CCTIObject
{
public:
	CCTIDialpad(void);
	virtual ~CCTIDialpad(void);

	virtual void SetType(int nDialpadType);
	virtual int GetType() {return m_nDialpadType;};

	virtual void SetAllowOneStep(bool bAllowOneStep) 
	{ 
		m_bAllowOneStep=bAllowOneStep;
		SetAttribute(KEY_ALLOW_SINGLE_STEP,bAllowOneStep); 
	};
	virtual bool GetAllowOneStep() { return m_bAllowOneStep; };

	/**
	 * Sets the number that is shown in this dialpad's edit box.
	 *
	 * @param sNumber The number.
	 */
	virtual void SetNumber(std::wstring sNumber) { SetAttribute(KEY_NUMBER,sNumber); };
	/**
	 * Gets the number that is shown in this dialpad's edit box.
	 *
	 */
	virtual std::wstring GetNumber() { return GetAttribute(KEY_NUMBER); };
	/**
	 * Gets the XML tag associated with this object.
	 *
	 * @return The XML tag associated with this object.
	 */
	virtual _bstr_t GetTag() { return "CTIDialpad"; };
protected:
	int m_nDialpadType;
	bool m_bAllowOneStep;
};
