#define ISalesforceCTIAdapter_IID "38AEB740-2427-4B69-ABD2-5F114666AAE8"

#define DISPID_UIRefresh 1

// ISalesforceCTIAdapter
[
	object,
	uuid("38AEB740-2427-4B69-ABD2-5F114666AAE8"),
	dual,	helpstring("ISalesforceCTIAdapter Interface"),
	pointer_default(unique)
]
__interface ISalesforceCTIAdapter : IDispatch
{
	[id(1), helpstring("Sends an action to the CTI adapter")] HRESULT UIAction([in] BSTR message);
	[id(2), helpstring("Returns the author and name of the current adapter")] HRESULT GetAdapterName([out, retval] BSTR* Value);
	[id(3), helpstring("Returns a string representing the version of the current adapter")] HRESULT GetAdapterVersion([out, retval] BSTR* Value);
};


// _ISalesforceCTIAdapterEvents
[
	dispinterface,
	uuid("7385BF7C-4440-44CC-BB33-B29D8759AD88"),
	helpstring("_ISalesforceCTIAdapterEvents Interface")
]
__interface _ISalesforceCTIAdapterEvents : public IDispatch
{
	[id(DISPID_UIRefresh), helpstring("method UIRefresh")] HRESULT UIRefresh(BSTR pXMLUI);
	[id(0x2), helpstring("Notifies the Browser Connector that the adapter tray menu has changed.")] HRESULT UpdateTrayMenu(BSTR xmlMenu);
};