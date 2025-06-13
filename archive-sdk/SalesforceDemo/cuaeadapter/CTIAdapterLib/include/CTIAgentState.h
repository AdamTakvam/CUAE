#pragma once
#include "CTISelectableIdLabelObject.h"

/**
 * @version 1.0
 * 
 * This class represents a single agent state in the Salesforce.com CTI user interface.
 */
class CCTIAgentState :
	public CCTISelectableIdLabelObject
{
public:
	CCTIAgentState(void);
	CCTIAgentState(std::wstring sId, std::wstring sLabel);
	virtual ~CCTIAgentState(void);
	/**
	 * Gets the XML tag associated with this object.
	 *
	 * @return The XML tag associated with this object.
	 */
	virtual _bstr_t GetTag() { return "CTIAgentState"; };

	/**
	 * Sets the parameter that governs the order in which this agent state will be displayed
	 * relative to the others.
	 *
	 * @param nOrder The numeric position at which this agent state should appear.
	 */
	virtual void SetOrder(int nOrder) { SetAttribute(KEY_ORDER,nOrder); };
};
