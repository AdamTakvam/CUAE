#include "StdAfx.h"
#include "ctifield.h"

CCTIField::CCTIField(std::wstring sId, std::wstring sLabel, std::wstring sValue)
:CCTIValueObject(sId,sLabel)
{
	SetVisible(true);
	SetValue(sValue);
}

CCTIField::~CCTIField(void)
{
}
