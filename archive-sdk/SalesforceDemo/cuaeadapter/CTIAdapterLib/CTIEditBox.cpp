#include "StdAfx.h"
#include "ctieditbox.h"

CCTIEditBox::~CCTIEditBox(void)
{
}

CCTIEditBox::CCTIEditBox(std::wstring sId, std::wstring sLabel)
:CCTIValueObject(sId,sLabel),
m_bPassword(false)
{

}