#pragma once
#include "ctiidlabelobject.h"

/**
 * @version 1.0
 * 
 * This class represents a set of reason codes in the Salesforce.com CTI user interface.  Note that no distinction is
 * made here between wrap-up, not ready, and logout reason codes; for the purposes of this class, they're all just reason codes.
 */
class CCTIReasonCodeSet :
	public CCTIIdLabelObject
{
protected:
	PARAM_MAP m_mapReasonCodes; /**< The map of reason codes (where the keys of the map are the reason code IDs and the values are the code labels). */
	std::wstring m_sSelectedReasonCode; /**< The ID of the currently selected reason code in this set. */
public:
	CCTIReasonCodeSet(void);
	virtual ~CCTIReasonCodeSet(void);
	CCTIReasonCodeSet(std::wstring sId, std::wstring sLabel=L"");

	/**
	 * Serializes this object to XML.
	 */
	virtual MSXML2::IXMLDOMDocumentFragmentPtr SerializeToXML(MSXML2::IXMLDOMDocumentPtr pXMLDoc);

	/**
	 * Sets the map of reason codes (where the keys of the map are the reason code IDs and the values are the code labels).
	 *
	 * @param mapReasonCodes The map of reason codes.
	 */
	virtual void SetReasonCodes(PARAM_MAP& mapReasonCodes);

	/**
	 * Sets the call object ID of the call we're wrapping up.  Should only be used if this object contains wrap-up codes.
	 *
	 * @param sCallObjectId The call object ID.
	 */
	virtual void SetCallObjectId(std::wstring& sCallObjectId) { SetAttribute(KEY_CALL_OBJECT_ID,sCallObjectId); };
	/**
	 * Gets the call object ID of the call we're wrapping up.  Should only be used if this object contains wrap-up codes.
	 *
	 * @return The call object ID of the call we're wrapping up.
	 */
	virtual std::wstring GetCallObjectId() { return GetAttribute(KEY_CALL_OBJECT_ID); };

	/**
	 * Sets the selected reason code.
	 *
	 * @param sId The ID of the reason code to select.
	 */
	virtual void SetSelectedReasonCode(std::wstring sId) { 
		m_sSelectedReasonCode = sId!=KEY_REASON_CODE_NONE?sId:L""; 
	};

	/**
	 * Gets the selected reason code.
	 *
	 * @return The ID of the selected reason code, or the empty string if no reason code is selected.
	 */
	virtual std::wstring GetSelectedReasonCode() { return m_sSelectedReasonCode ; };

	/**
	 * Selects the first reason code in the list.
	 */
	virtual void SelectFirstReasonCode();

	/**
	 * Defines whether a Save button should be shown with these reason codes.
	 *
	 * @param bShowSave True if a Save button should be shown with these reason codes.
	 */
	virtual void ShowSaveButton(bool bShowSave)
	{
		if (bShowSave) {
			SetAttribute(KEY_SHOW_SAVE,bShowSave);
		} else {
			RemoveAttribute(KEY_SHOW_SAVE);
		}
	}

	/**
	 * Defines whether a None option should be shown with these reason codes.
	 *
	 * @param bShowSave True if a None option should be shown with these reason codes.
	 */
	virtual void ShowNone(bool bShowNone)
	{
		if (bShowNone) {
			SetAttribute(KEY_SHOW_NONE,bShowNone);
		} else {
			RemoveAttribute(KEY_SHOW_NONE);
		}
	}

};
