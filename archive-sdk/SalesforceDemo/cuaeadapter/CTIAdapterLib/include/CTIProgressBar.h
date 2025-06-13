#pragma once
#include "ctiidlabelobject.h"

/**
 * This class represents a progress bar control that can exist within the context of a CCTIUserInterface or CCTILine.
 * The progress bar does not give an indication of specific progress, but merely indicates that processing is occurring.
 */
class CCTIProgressBar :
	public CCTIIdLabelObject
{
public:
	CCTIProgressBar();
	virtual ~CCTIProgressBar();

	/**
	 * Gets the XML tag associated with this object.
	 *
	 * @return The XML tag associated with this object.
	 */
	virtual _bstr_t GetTag() { return "CTIProgressBar"; };
};
