#pragma once

#include "resource.h"       // main symbols
#include "stdafx.h"
#include "CTIAdapter.h"
#include "ISalesforceCTIAdapter.h"
#include <tapi.h>

class CUAEUserInterface;

[
	coclass,
	threading("free"),
	event_source("com"),
	vi_progid("CUAEAdapter.CUAEAdapter"),
	progid("CUAEAdapter.CUAEAdapter.1"),
	version(1.0),
	uuid("028a4096-2701-4c10-b529-27c3229415e9"),
	helpstring("CUAEAdapterBase Class")
]
class ATL_NO_VTABLE CUAEAdapterBase : 
	public ISalesforceCTIAdapter,
	public CCTIAdapter
{
protected:
	CUAEUserInterface* m_pUI; /**< The subclass of CCTIUserInterface that this adapter will use. */
	HLINE hline;

public:
	/**
	 * Receives an inbound XML-formatted message via COM from the browser controller.
	 * It should be formatted as (with as many parameters as needed):
	 * <MESSAGE ID="MESSAGE_ID">
	 *		<PARAMETER NAME="PARAM1" VALUE="VALUE1"/>
	 * </MESSAGE>
	 *
	 * @param message The XML-formatted message to handle
	 * @return An HRESULT indicating whether the message was successfully received and parsed
	 */
	STDMETHOD(UIAction)(BSTR message);

	/**
	 * Returns the name and author of the adapter.  Should be like "Salesforce.com CTI Adapter For Cisco IPCC Enterprise"
	 *
	 * @param bsName Contains the return value.
	 */
	STDMETHOD(GetAdapterName)(BSTR* bsName);

	/**
	 * Returns the version of the adapter, which can contain any character (like "1.01b").
	 *
	 * @param bsName Contains the return value.
	 */
	STDMETHOD(GetAdapterVersion)(BSTR* bsVersion);

	/**
	 * Generates an UpdateTrayMenu event containing the input menu xml.
	 *
	 * The XML should be of the following format:
	 *
	 * <MENU>
	 *		<ITEM ID="MENUITEM1" LABEL="Menu Item 1" CHECKED="false" ENABLED="true"/>
	 *		<ITEM ID="MENUITEM2" LABEL="Menu Item 2" CHECKED="false" ENABLED="true"/>
	 * </MENU>
	 *
	 * The value specified in the ID parameter will be the command that is sent to CCTIUserInterface when that menu item
	 * is clicked.  The LABEL parameter is the label that will be shown in the menu.  The CHECKED parameter, if specified
	 * as "true", will cause the menu item to appear checked (it's just a normal menu item if this remains unchecked).  
	 * The ENABLED item, if specified as "false", will cause the menu item to appear disabled and grayed (if unspecified
	 * the menu item will default to enabled).
	 *
	 * @param bsMenu The XML describing the menu.
	 */
	virtual void SendUpdateTrayMenuEvent(_bstr_t bsMenu);

	/**
	 * A method that takes in an XML string and generates a COM UIRefresh event with it.
	 *
	 * @param xml The XML to include with the event.
	 */
	virtual void SendUIRefreshEvent(_bstr_t xml);

	CUAEAdapterBase()
	{
	}

	__event __interface _ISalesforceCTIAdapterEvents;

	DECLARE_PROTECT_FINAL_CONSTRUCT()

	HRESULT FinalConstruct();
	
	void FinalRelease();

public:

};
