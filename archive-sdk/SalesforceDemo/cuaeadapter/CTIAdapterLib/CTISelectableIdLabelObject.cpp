#include "stdafx.h"
#include "CTISelectableIdLabelObject.h"

CCTISelectableIdLabelObject::CCTISelectableIdLabelObject(std::wstring sId, std::wstring sLabel, bool bSelected)
:CCTIIdLabelObject(sId,sLabel),m_bSelected(bSelected)
{

}

CCTISelectableIdLabelObject::CCTISelectableIdLabelObject(bool bSelected)
:m_bSelected(bSelected)
{
	
}

void CCTISelectableIdLabelObject::SetSelected(bool bSelected)
{
	m_bSelected = bSelected;
	SetAttribute(KEY_SELECTED,m_bSelected);
}

bool CCTISelectableIdLabelObject::GetSelected(void)
{
	return m_bSelected;
}