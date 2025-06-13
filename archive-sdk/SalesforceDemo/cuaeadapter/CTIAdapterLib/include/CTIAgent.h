#pragma once
#include "ctiobject.h"
#include "CtiAgentState.h"
#include <map>

/**
 * @version 1.0
 * 
 * This class represents the agent state selector in the Salesforce.com CTI user interface.
 */
class CCTIAgent :
	public CCTIObject
{
public:
	CCTIAgent(void);
	virtual ~CCTIAgent(void);

	virtual MSXML2::IXMLDOMDocumentFragmentPtr SerializeToXML(MSXML2::IXMLDOMDocumentPtr pXMLDoc);
protected:
	 std::map<std::wstring,CCTIAgentState*> m_mapAgentStates;
	 std::wstring m_sAgentState; /**< A string storing the current agent state, for the sake of convenience.  Should always be in sync with the active state in m_mapAgentStates. */
public:
	
	/**
	 * Makes agents states visible as provided in the list.  The current agent state should be in this list, 
     * or it will be added automatically.
	 * 
	 * @param listEnabledAgentStates The list of agent states to enable.
	 */
	virtual void EnableAgentStates(std::list<std::wstring>& listEnabledAgentStates);
	virtual void SetAgentState(std::wstring& sAgentState);
	virtual std::wstring GetAgentState();
	/**
	 * Adds an agent state to the set and immediately makes it invisible.  It can be made visible later.
	 *
	 * @param sId The ID of the agent state.
	 * @param nOrder The order in which the agent state should be displayed.
	 * @param sLabel (optional) The label of the agent state, if there is no default label that corresponds to the ID.
	 */
	void AddAgentState(std::wstring sId, int nOrder, std::wstring sLabel=L"");
};
