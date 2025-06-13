#include "StdAfx.h"
#include "ctiagentstate.h"

CCTIAgentState::CCTIAgentState()
:CCTISelectableIdLabelObject(false)
{
	SetVisible(false);
}

CCTIAgentState::CCTIAgentState(std::wstring sId, std::wstring sLabel)
:CCTISelectableIdLabelObject(sId,sLabel)
{

}

CCTIAgentState::~CCTIAgentState()
{
}
