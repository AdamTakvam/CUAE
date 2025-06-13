#pragma once
#include "ctiobject.h"
#include <string>

/**
 * @version 1.0
 * 
 * This class encapsulates a CCTIObject that contains an ID and label.  It is mostly a convenience class, but it's
 * subclassed by most of the visual elements, as most of them do indeed contain an ID and a label.  At runtime, the
 * Browser Controller will attempt to use the ID to translate any text on the visual elements to the proper language.
 * If the BC has no mapping for the ID of that object, then it will use the label instead to show that text.
 */
class CCTIIdLabelObject :
	public CCTIObject
{
public:
	CCTIIdLabelObject(void);
	CCTIIdLabelObject(std::wstring sId, std::wstring sLabel=L"");

	virtual ~CCTIIdLabelObject(void);
	std::wstring GetId(void);
	virtual void SetId(std::wstring sId);

	virtual std::wstring GetLabel(void);
	virtual void SetLabel(std::wstring sLabel);
};

typedef std::map<std::wstring,CCTIIdLabelObject*> IdLabelObjectMap;
typedef std::list<CCTIIdLabelObject*> IdLabelObjectList;