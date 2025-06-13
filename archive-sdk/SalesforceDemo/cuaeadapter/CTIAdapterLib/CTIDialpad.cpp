#include "StdAfx.h"
#include "ctidialpad.h"

CCTIDialpad::CCTIDialpad(void)
:m_nDialpadType(DIALPAD_DIAL),
 m_bAllowOneStep(false)
{
}

CCTIDialpad::~CCTIDialpad(void)
{
}

void CCTIDialpad::SetType(int nDialpadType)
{ 
	m_nDialpadType=nDialpadType;
	std::wstring sDialpadType = KEY_DIAL;
	switch (nDialpadType) {
		case DIALPAD_TRANSFER:
			{
				sDialpadType=KEY_TRANSFER;
				break;
			}
		case DIALPAD_CONFERENCE:
			{
				sDialpadType=KEY_CONFERENCE;
				break;
			}
		case DIALPAD_NEW_LINE:
			{
				sDialpadType=KEY_NEW_LINE;
			}
	}
	SetAttribute(KEY_TYPE,sDialpadType); 
}
