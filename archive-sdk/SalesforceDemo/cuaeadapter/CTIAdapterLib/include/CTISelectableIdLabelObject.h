#pragma once
#include "CTIIdLabelObject.h"

/**
 * @version 1.0
 * 
 * This class represents an object with an ID, LABEL, and SELECTED parameter in the Salesforce.com CTI user interface.
 * This is not a top-level object in the user interface, but a base class for other objects.
 */
class CCTISelectableIdLabelObject :
	public CCTIIdLabelObject
{
public:
	CCTISelectableIdLabelObject(bool bSelected=false);
	CCTISelectableIdLabelObject(std::wstring sId, std::wstring sLabel, bool bSelected=false);
	virtual ~CCTISelectableIdLabelObject(void) { };
	/**
	 * Sets this object's selected state
	 *
	 * @param bSelected True if this object should become selected, false otherwise
	 */
	virtual void SetSelected(bool bSelected);

	/**
	 * Gets this object's selected state
	 *
	 * @return True if this object is selected, false otherwise
	 */
	virtual bool GetSelected(void);

	/**
	 * Gets the XML tag associated with this object.
	 *
	 * @return The XML tag associated with this object.
	 */
	virtual _bstr_t GetTag() { return "CTISelectableIdLabelObject"; };
protected:
	bool m_bSelected;
};