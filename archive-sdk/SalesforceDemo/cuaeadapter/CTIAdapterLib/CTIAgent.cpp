#include "StdAfx.h"
#include "ctiagent.h"
#include "CTIUserInterface.h"

CCTIAgent::CCTIAgent(void)
{
	//Create an Agent State object for all standard agent states
	AddAgentState(AGENTSTATE_READY,0);
	AddAgentState(AGENTSTATE_NOT_READY,1);
	AddAgentState(AGENTSTATE_WRAPUP,2);
	AddAgentState(AGENTSTATE_BUSY,3);
	AddAgentState(AGENTSTATE_LOGOUT,4);
}

CCTIAgent::~CCTIAgent(void)
{
}

MSXML2::IXMLDOMDocumentFragmentPtr CCTIAgent::SerializeToXML(MSXML2::IXMLDOMDocumentPtr pXMLDoc) 
{
	MSXML2::IXMLDOMDocumentFragmentPtr pFragment = pXMLDoc->createDocumentFragment();
	if (GetVisible()) {
		MSXML2::IXMLDOMElementPtr pXmlAgent= pXMLDoc->createElement("CTIAgent");
		AddAttributesToElement(pXMLDoc,pXmlAgent);

		for (std::map<std::wstring,CCTIAgentState*>::iterator it=m_mapAgentStates.begin();it!=m_mapAgentStates.end();it++) {
			CCTIAgentState* pAgentState = it->second;
			AddChildIfVisible(pXMLDoc, pXmlAgent, pAgentState);
		}

		pFragment->appendChild(pXmlAgent);
		pXmlAgent.Release();
	}
	return pFragment;
}

void CCTIAgent::SetAgentState(std::wstring& sAgentState) {
	m_sAgentState = sAgentState;
	m_mapAgentStates[sAgentState]->SetVisible(true);	
	m_mapAgentStates[sAgentState]->SetSelected(true);
}

void CCTIAgent::EnableAgentStates(std::list<std::wstring>& listEnabledAgentStates)
{
	for (std::map<std::wstring,CCTIAgentState*>::iterator it=m_mapAgentStates.begin();it!=m_mapAgentStates.end();it++) {
		CCTIAgentState* pAgentState = it->second;
		pAgentState->SetVisible(find(listEnabledAgentStates.begin(),listEnabledAgentStates.end(),it->first)!=listEnabledAgentStates.end());
		pAgentState->SetSelected(it->first==m_sAgentState);
	}
	// the active agent state should be in the list, if not, enable and select it specifically
	if (find(listEnabledAgentStates.begin(),listEnabledAgentStates.end(),m_sAgentState)==listEnabledAgentStates.end()) {
		m_mapAgentStates[m_sAgentState]->SetVisible(true);
		m_mapAgentStates[m_sAgentState]->SetSelected(true);

		CCTILogger::Log(LOGLEVEL_MED,L"CCTIAgent::EnableAgentStates: Current agent state %s not provided in list.", m_sAgentState);		
	}
}

std::wstring CCTIAgent::GetAgentState()
{
	return m_sAgentState;
}

void CCTIAgent::AddAgentState(std::wstring sId, int nOrder, std::wstring sLabel)
{
	// remove duplicates
	if (m_mapAgentStates.find(sId)!=m_mapAgentStates.end()) {
		CCTIAgentState* pAgentState = m_mapAgentStates[sId];
		delete pAgentState;
	}

	CCTIAgentState* pNewAgentState = new CCTIAgentState(sId,sLabel);
	pNewAgentState->SetVisible(false);
	pNewAgentState->SetOrder(nOrder);
	m_mapAgentStates[sId]=pNewAgentState;
}
