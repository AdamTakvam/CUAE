#pragma once
#include "CTICallLog.h"
#include "CTIObject.h"


/**
 * @version 1.0
 * 
 * This class represents a set of previous call logs in the Salesforce.com CTI user interface.
 * It contains a queue of previous calls, up to 3 calls; when a call is added to the queue beyond
 * 3, the oldest call gets removed from the queue, thus ensuring that only the 3 most recent calls
 * are represented.
 */
class CCTIPreviousCalls : public CCTIObject
{
protected:
	bool m_bOpen; /**< Indicates whether the previous calls area is open or "rolled up." */ 
	CallLogList m_listCallLogs; /**< Holds the queue of call logs. */
public:
	CCTIPreviousCalls();
	virtual ~CCTIPreviousCalls();

	/**
	 * Resets the previous calls area and deletes all the call log objects it holds.
	 */
	virtual void Reset();

	/**
	 * Sets the open state of the previous calls area.
	 *
	 * @param bOpen True if the previous calls area should be open, false if it's "rolled up."
	 */
	virtual void SetOpen(bool bOpen);
	/**
	 * Gets the open state of the previous calls area.
	 *
	 * @return True if the previous calls area is open, false if it's "rolled up."
	 */
	virtual bool GetOpen();

	/**
	 * Adds a call log to the previous calls list.
	 *
	 * @param pLine The CCTICallLog object to add to the previous calls list.
	 */
	virtual void AddCallLog(CCTICallLog* pLog);

	/**
	 * Removes the specified call log from the previous calls list, but does not delete the call log object.
	 *
	 * @param pLog The CCTICallLog object to remove from the previous calls list.
	 */
	virtual void RemoveCallLog(CCTICallLog* pLog);

	/**
	 * Gets the XML tag associated with this object.
	 *
	 * @return The XML tag associated with this object.
	 */
	virtual _bstr_t GetTag() { return "CTIPreviousCalls"; };

	/**
	 * Finds the call log in the previous calls by call object ID.
	 *
	 * @param sCallObjectId The call object ID of the call log to return.
	 * @return The call log corresponding to the input call object ID, or NULL if none exist in the previous calls.
	 */
	virtual CCTICallLog* GetCallLogByCallId(std::wstring& sCallObjectId);

	/**
	 * Serializes this object to XML.
	 *
	 * @param pXMLDoc The document object (used to create all elements, attributes, etc.).
	 * @return A document fragment that represents this object serialized to XML.
	 */
	virtual MSXML2::IXMLDOMDocumentFragmentPtr SerializeToXML(MSXML2::IXMLDOMDocumentPtr pXMLDoc);

	/**
	 * Sets the text that shows in the report link section above the logo, which by default is "My Calls Today."
	 *
	 * @param sText The text to show in the report link section above the logo
	 */
	virtual void SetReportText(std::wstring sText)
	{
		SetAttribute(KEY_REPORT_TEXT,sText);
	}

	/**
	 * Sets the URL that the report link section above the logo points to.  This URL should generally be a relative URL
	 * such as the default URL, which is:
	 *
	 * /00O?pc0=CALLTYPE&pv0=&type=te&sort=CD&sd=8%2F30%2F2006&c=CS&c=OP&c=CO&c=LD&c=AT&c=CD_FMT&c=CL_DISP&c=CALLTYPE&c=SU&c=AS&details=yes&retURL=%2F00O%2Fo&pn0=ne&rt=24&scope=user&t=title_MyCallsToday&closed=closed
	 *
	 * Like the default URL, the URL specified here should have no protocol or host information.
	 *
	 * @param sUrl The relative URL that the report link should point to.
	 */
	virtual void SetReportURL(std::wstring sUrl) {
		SetAttribute(KEY_REPORT_URL,sUrl);
	}

	/**
	 * Sets whether the report link is shown above the logo.  By default it will be shown.
	 *
	 * @param bReportLinkVisible True if the report link above the logo should be visible, false otherwise. 
	 */
	virtual void SetReportLinkVisible(bool bReportLinkVisible)
	{
		SetAttribute(KEY_REPORT_VISIBLE,bReportLinkVisible);
	}

	virtual void SetReportDate(int year, int month, int day) {
		SetAttribute(KEY_REPORT_YEAR, year);
		SetAttribute(KEY_REPORT_MONTH, month);
		SetAttribute(KEY_REPORT_DAY, day);
	}
};