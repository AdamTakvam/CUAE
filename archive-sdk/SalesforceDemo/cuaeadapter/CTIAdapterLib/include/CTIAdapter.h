#include "stdafx.h"
#pragma once

class CCTIAdapter
/**
 * @version 1.0
 * 
 * This class should be one of the base classes of the main ATL class that comprises the adapter's COM interface.
 * Its function is to provide an interface between the CCTIUserInterface base class and the COM class.
 * Specifically, it provides a common means by which methods in CCTIUserInterface can cause the COM class to fire an UIRefresh event.
 */
{
public:

	/**
	 * A placeholder method that takes in an XML string and generates a COM UIRefresh event with it.
	 * This pure virtual class must be subclassed (and this method implemented) by every CTI adapter.
	 *
	 * @param xml The XML to include with the event.
	 */
	virtual void SendUIRefreshEvent(_bstr_t xml) = 0;

	/**
	 * A placeholder method that takes in an XML string and generates a COM UpdateTrayMenu event with it.
	 * Unlike SendUIRefreshEvent, this method is optional; adapters need not implement it unless they choose to do so.
	 *
	 * The XML should be of the following format:
	 *
	 * <MENU>
	 *		<ITEM ID=KEY_MENUITEM1 LABEL="Menu Item 1" CHECKED="false" ENABLED="true"/>
	 *		<ITEM ID=KEY_MENUITEM2 LABEL="Menu Item 2" CHECKED="false" ENABLED="true"/>
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
	virtual void SendUpdateTrayMenuEvent(_bstr_t bsMenu) { };
};