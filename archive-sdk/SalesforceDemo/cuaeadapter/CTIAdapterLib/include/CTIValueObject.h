#pragma once
#include "ctiidlabelobject.h"

/**
 * @version 1.0
 * 
 * This class encapsulates a CCTIObject that contains an ID, label, and value.  It is a convenience class to allow
 * for reuse of code across its subclasses.
 */
class CCTIValueObject :
	public CCTIIdLabelObject
{
public:
	CCTIValueObject(std::wstring sId, std::wstring sLabel=L"");
	~CCTIValueObject(void);
	
	/**
	 * Gets the VALUE attribute of this object.
	 */
	virtual std::wstring GetValue() { return GetAttribute(KEY_VALUE); };
	
	/**
	 * Sets the VALUE attribute of this object.
	 *
	 * @param sValue The value to set the VALUE attribute to.
	 */
	virtual void SetValue(std::wstring sValue) { SetAttribute(KEY_VALUE,sValue); };

	/**
	 * Populates the value of this control from the input PARAM_MAP.  The value of this control will be set to the corresponding
	 * value in the map where the map key matches this control's ID.  If the map does not contain an entry for this control's ID,
	 * then the value of this control will be set to the empty string.
	 *
	 * @param mapValues Map from which this control's value should be drawn.
	 */
	virtual void PopulateValueFromMap(PARAM_MAP& mapValues);
};
