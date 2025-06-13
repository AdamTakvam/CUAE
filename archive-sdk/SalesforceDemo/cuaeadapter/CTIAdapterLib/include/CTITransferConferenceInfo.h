#pragma once
#include "ctiform.h"

class CCTIStatic;
class CCTIInfoField;
/** 
 * @version 1.0
 * 
 * A class that holds the visual elements for displaying transfer and conference information when one arrives.
 * Unlike the majority of the "CTI" classes, this class does not correspond to a single visual element.  Rather,
 * it is a placeholder for a few different visual elements that display information about a transfer or conference.
 * It derives from CCTIForm because it is, in fact, a special case of a form, albeit one that doesn't have any buttons
 * to submit the data, or indeed even any data to submit.  Rather, this class derives from CCTIForm because CCTIForm
 * is a logical container of visual elements.
 */
class CCTITransferConferenceInfo :
	public CCTIForm
{
public:
	CCTITransferConferenceInfo();
	~CCTITransferConferenceInfo();

	/**
	 * Sets whether this object is showing transfer info or conference info.
	 *
	 * @param nTransferOrConference Should be one of USERINFO_TRANSFER or USERINFO_CONFERENCE to indicate whether this is showing transfer or conference info, respectively.
	 */
	virtual void SetTransferOrConference(int nTransferOrConference);

	/**
	 * Gets whether this object is showing transfer info or conference info.
	 *
	 * @return The transfer/conference state of this object.
	 */
	virtual int GetTransferOrConference() { return m_nTransferOrConference; };
	
	/**
	 * Gets the XML tag associated with this object.
	 *
	 * @return The XML tag associated with this object.
	 */
	virtual _bstr_t GetTag() { return "CTITransferConferenceInfo"; };

	/**
	 * This method acts as an override to the base class to ensure that only non-interactive controls
	 * ever get added to a CCTITransferConferenceInfo object.  This method will do nothing and return NULL.
	 *
	 * @param sId The ID of the edit box.
	 * @param sLabel (optional) The label of the edit box in case its ID cannot be translated to a string.
	 * @param sValue (optional) The initial value displayed in the edit box.
	 *
	 * @return NULL.
	 */
	virtual CCTIEditBox* AddEditBox(std::string sId, std::string sLabel="", std::string sValue="") { return NULL; };

	/**
	 * This method acts as an override to the base class to ensure that only non-interactive controls
	 * ever get added to a CCTITransferConferenceInfo object.  This method will do nothing and return NULL.
	 *
	 * @param sId The ID of the checkbox.
	 * @param sLabel (optional) The label of the checkbox in case its ID cannot be translated to a string.
	 * @param bChecked (optional) The checked state of the checkbox.  Defaults to false.
	 *
	 * @return NULL.
	 */
	virtual CCTICheckbox* AddCheckbox(std::string sId, std::string sLabel="", bool bChecked=false) { return NULL; }; 

	/**
	 * This method acts as an override to the base class to ensure that only non-interactive controls
	 * ever get added to a CCTITransferConferenceInfo object.  This method will do nothing and return NULL.
	 *
	 * @param sId The ID of the button.
	 * @param sLabel (optional) The label of the button in case its ID cannot be translated to a string.
	 *
	 * @return NULL.
	 */
	virtual CCTIButton* AddButton(std::string sId, std::string sLabel="") { return NULL; }; 
protected:
	int m_nTransferOrConference; /**< The transfer/conference state of this object. */
	CCTIStatic* m_pTransferredBy;
	CCTIStatic* m_pConferencedBy;
	CCTIInfoField* m_pPartyName;
	CCTIInfoField* m_pPartyExtension;
public:
	virtual void CreateChildren(void);
	virtual void SetPartyName(std::string sPartyName);
	virtual void SetPartyExtension(std::string sPartyExtension);
};
