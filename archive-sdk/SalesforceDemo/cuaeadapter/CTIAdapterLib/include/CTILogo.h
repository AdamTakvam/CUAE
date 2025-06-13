#pragma once
#include "ctiobject.h"
#include "ctiutils.h"

/**
 * @version 1.0
 * 
 * Represents the logo section of the user interface.  This element is always visible.
 */
class CCTILogo :
	public CCTIObject
{
public:
	CCTILogo(void);
	virtual ~CCTILogo(void);

	/**
	 * Gets the XML tag associated with this object.
	 *
	 * @return The XML tag associated with this object.
	 */
	virtual _bstr_t GetTag() { return "CTILogo"; };

	/**
	 * Sets the image url for this logo.  The image should be exactly 116x31 (WxH).
	 *
	 * @param sImageUrl The image url for this logo.
	 */
	virtual void SetImageUrl(std::wstring sImageUrl) { SetAttribute(KEY_IMAGE_URL,sImageUrl); };
	/**
	 * Gets the image url for this logo.
	 *
	 * @return The image url for this logo.
	 */
	virtual std::wstring GetImageUrl() { return GetAttribute(KEY_IMAGE_URL); };

	/**
	 * Sets the link url for this logo.
	 *
	 * @param sLinkUrl The link url for this logo.
	 */
	virtual void SetLinkUrl(std::wstring sLinkUrl) { SetAttribute(KEY_LINK_URL,sLinkUrl); };
	/**
	 * Gets the link url for this logo.
	 *
	 * @return The link url for this logo.
	 */
	virtual std::wstring GetLinkUrl() { return GetAttribute(KEY_LINK_URL); };

	/**
	 * Sets the link text for this logo.
	 *
	 * @param sLinkText The link text for this logo.
	 */
	virtual void SetLinkText(std::wstring sLinkText) { SetAttribute(KEY_LINK_TEXT,sLinkText); };
	/**
	 * Gets the link Text for this logo.
	 *
	 * @return The link text for this logo.
	 */
	virtual std::wstring GetLinkText() { return GetAttribute(KEY_LINK_TEXT); };

	/**
	 * Sets the popup window width for the logo link -- the width of the window that is generated when the link is clicked.
	 *
	 * @param nWindowWidth The window width for this logo.
	 */
	virtual void SetWindowWidth(int nWindowWidth) { SetAttribute(KEY_WINDOW_WIDTH,nWindowWidth); };
	/**
	 * Gets the popup window width for the logo link -- the width of the window that is generated when the link is clicked.
	 *
	 * @return The window width for this logo.
	 */
	virtual int GetWindowWidth() { return CCTIUtils::StringToInt(GetAttribute(KEY_WINDOW_WIDTH)); };

	/**
	 * Sets the popup window height for the logo link -- the height of the window that is generated when the link is clicked.
	 *
	 * @param nWindowHeight The window height for this logo.
	 */
	virtual void SetWindowHeight(int nWindowHeight) { SetAttribute(KEY_WINDOW_HEIGHT,nWindowHeight); };
	/**
	 * Gets the popup window height for the logo link -- the height of the window that is generated when the link is clicked.
	 *
	 * @return The window height for this logo.
	 */
	virtual int GetWindowHeight() { return CCTIUtils::StringToInt(GetAttribute(KEY_WINDOW_HEIGHT)); };


};
