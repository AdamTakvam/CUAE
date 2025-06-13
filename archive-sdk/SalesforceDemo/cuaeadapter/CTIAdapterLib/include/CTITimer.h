#pragma once
#include "ctiidlabelobject.h"
#include "ctiutils.h"

/**
 * @version 1.0
 * 
 * This class encapsulates a timer which will tick in real time in the user interface.  
 * A timer can only be attached to a line.
 */
class CCTITimer :
	public CCTIIdLabelObject
{
public:
	CCTITimer(std::wstring sId, std::wstring sLabel=L"");
	virtual ~CCTITimer(void);

	virtual int GetMinutes() { return m_nMinutes; };
	virtual void SetMinutes(int nMinutes) { 
		m_nMinutes = nMinutes; 
		SetAttribute(KEY_MINUTES,nMinutes);
	};

	virtual int GetSeconds() { return m_nSeconds; };
	virtual void SetSeconds(int nSeconds) { 
		m_nSeconds = nSeconds; 
		SetAttribute(KEY_SECONDS,nSeconds);
	};

	/**
	 * Gets the total number of seconds that have elapsed since this timer was started.
	 *
	 * @return The total number of seconds that have elapsed since this timer was started.
	 */
	virtual int GetTotalSeconds();

	/**
	 * Reset the timer to 0.  Note that Start() must be called again in order to start the timer after a reset.
	 */
	virtual void Reset();

	/**
	 * Starts the timer at this instant.  
	 * Note that the timer will only update itself with the current time when Update() is called,
	 * so one should be sure to call Update() before serializing this timer to XML.
	 */
	virtual void Start() {
		m_nStartTicks = GetTickCount();
	};

	/**
	 * Updates this timer to contain the amount of time since the last time Start() was called.
	 * Note that if Start() was never called, then this method will simply show a time of 0.  So be sure to always call Start()
	 * before calling Update().
	 */
	virtual void Update();

	/**
	 * Gets the XML tag associated with this object.
	 *
	 * @return The XML tag associated with this object.
	 */
	virtual _bstr_t GetTag() { return "CTITimer"; };

protected:
	int m_nMinutes;
	int m_nSeconds;
	DWORD m_nStartTicks;
};
