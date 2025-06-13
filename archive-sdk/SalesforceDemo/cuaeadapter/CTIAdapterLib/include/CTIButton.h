#pragma once
#include "ctiidlabelobject.h"

/**
 * @version 1.0
 * 
 * This class represents a button in the Salesforce.com CTI user interface.
 */
class CCTIButton :
	public CCTIIdLabelObject
{
public:
	CCTIButton(std::wstring sId, std::wstring sLabel=L"");
	CCTIButton(void);
	virtual ~CCTIButton(void);

	virtual std::wstring GetIconURL() { return GetAttribute(KEY_ICON_URL); };
	virtual void SetIconURL(std::wstring sUrl) { SetAttribute(KEY_ICON_URL,sUrl); };
	
	virtual std::wstring GetPressedIconURL() { return GetAttribute(KEY_PRESSED_ICON_URL); };
	virtual void SetPressedIconURL(std::wstring sUrl) { SetAttribute(KEY_PRESSED_ICON_URL,sUrl); };

	/**
	 * Gets this button's LINK_URL parameter.
	 *
	 * @return The URL to pop up when this button is pressed.
	 */
	virtual std::wstring GetLinkURL() { return GetAttribute(KEY_LINK_URL); };
	/**
	 * Sets this button's LINK_URL parameter.  If this is set, then the button will pop up a new window to
	 * this link rather than sending a message to the CTI client.
	 *
	 * @param sUrl The URL to pop up when this button is pressed.
	 */
	virtual void SetLinkURL(std::wstring sUrl) { SetAttribute(KEY_LINK_URL,sUrl); };

	/**
	 * 
	 *
	 * @return The long-style button color.
	 */
	virtual std::wstring GetColor() { return GetAttribute(KEY_COLOR); };
	/**
	 * Sets the button color of a long-style button.
	 * For a long-style button, should be one of KEY_RED, KEY_GREEN, or "BEIGE."
	 * Ignored for short-style buttons.
	 *
	 * @param sColor The long-style button color.
	 */
	virtual void SetColor(std::wstring sColor) { SetAttribute(KEY_COLOR,sColor); };

	virtual bool GetIsToggleButton() { return GetAttribute(KEY_TOGGLE_BUTTON)==KEY_TRUE?true:false; };
	virtual void SetIsToggleButton(bool bToggle) { SetAttribute(KEY_TOGGLE_BUTTON,bToggle); };

	/**
	 * Gets whether this button is long-style or not.
	 * A long-style button encompasses the full width of the softphone.
	 *
	 * @return 
	 */
	virtual bool GetLongStyle() { return GetAttribute(KEY_LONG_STYLE)==KEY_TRUE?true:false; };
	virtual void SetLongStyle(bool bLongStyle) { SetAttribute(KEY_LONG_STYLE,bLongStyle); };

	virtual bool GetToggleState() { return GetAttribute(KEY_TOGGLE_STATE)==KEY_TRUE?true:false; };
	virtual void GetToggleState(bool bToggleState) { SetAttribute(KEY_TOGGLE_STATE,bToggleState); };
	/**
	 * Gets the XML tag associated with this object.
	 *
	 * @return The XML tag associated with this object.
	 */
	virtual _bstr_t GetTag() { return "CTIButton"; };

	/**
	 * Creates a new button object with the very same attributes as the original.  Note that is it the caller's
	 * responsibility to dispose of the new object created here.
	 *
	 * @return A new button object with the very same attributes as the original
	 */
	virtual CCTIButton* Clone();
};
