#include "StdAfx.h"
#include "ctiidlabelobject.h"

CCTIIdLabelObject::CCTIIdLabelObject(void)
{
}

CCTIIdLabelObject::CCTIIdLabelObject(std::wstring sId, std::wstring sLabel)
{
	SetId(sId);
	if (!sLabel.empty()) SetLabel(sLabel);
}

CCTIIdLabelObject::~CCTIIdLabelObject(void)
{

}
std::wstring CCTIIdLabelObject::GetId(void)
{
	return GetAttribute(KEY_ID);
}

void CCTIIdLabelObject::SetId(std::wstring sId)
{
	SetAttribute(KEY_ID,sId);
}

std::wstring CCTIIdLabelObject::GetLabel(void)
{
	return GetAttribute(KEY_LABEL);
}

void CCTIIdLabelObject::SetLabel(std::wstring sLabel)
{
	SetAttribute(KEY_LABEL,sLabel);
}
