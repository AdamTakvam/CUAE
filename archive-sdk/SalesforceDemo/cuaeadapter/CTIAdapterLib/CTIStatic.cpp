#include "StdAfx.h"
#include ".\ctistatic.h"

CCTIStatic::CCTIStatic(std::wstring sId, std::wstring sLabel)
:CCTIIdLabelObject(sId,sLabel),
m_bError(false)
{

}

CCTIStatic::~CCTIStatic(void)
{
}
