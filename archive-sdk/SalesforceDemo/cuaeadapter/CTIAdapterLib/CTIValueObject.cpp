#include "StdAfx.h"
#include ".\ctivalueobject.h"
#include ".\include\ctivalueobject.h"
#include "winreg.h"

CCTIValueObject::CCTIValueObject(std::wstring sId, std::wstring sLabel)
:CCTIIdLabelObject(sId,sLabel)
{
}

CCTIValueObject::~CCTIValueObject(void)
{
}

void CCTIValueObject::PopulateValueFromMap(PARAM_MAP& mapValues)
{
	PARAM_MAP::iterator itValue = mapValues.find(GetId());
	SetValue(itValue!=mapValues.end()?itValue->second:L"");
}
