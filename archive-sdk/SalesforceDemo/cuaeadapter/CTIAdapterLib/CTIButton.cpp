#include "StdAfx.h"
#include "ctibutton.h"

CCTIButton::CCTIButton(void)
{
	SetVisible(false);
}

CCTIButton::~CCTIButton(void)
{
}

CCTIButton::CCTIButton(std::wstring sId, std::wstring sLabel)
:CCTIIdLabelObject(sId,sLabel)
{
	SetVisible(false);
	SetLongStyle(false);
}

CCTIButton* CCTIButton::Clone()
{
	CCTIButton* pButton = new CCTIButton(GetId(),GetLabel());
	//Copy the map of attributes
	pButton->m_mapAttributes.insert(m_mapAttributes.begin(),m_mapAttributes.end());
	pButton->SetLongStyle(GetLongStyle());
	pButton->SetVisible(GetVisible());
	return pButton;
}