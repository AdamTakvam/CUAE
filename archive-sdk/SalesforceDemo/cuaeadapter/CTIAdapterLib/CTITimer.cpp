#include "StdAfx.h"
#include "ctitimer.h"
#include "math.h"

CCTITimer::CCTITimer(std::wstring sId, std::wstring sLabel)
:CCTIIdLabelObject(sId,sLabel),
m_nMinutes(0),
m_nSeconds(0),
m_nStartTicks(0)
{
	SetVisible(false);
}

CCTITimer::~CCTITimer(void)
{
}

void CCTITimer::Update() {
	//No sense updating a timer that was never started.
	if (m_nStartTicks>0) {
		DWORD nCurrentTicks = GetTickCount() - m_nStartTicks;
		//Ticks are in milliseconds
		SetMinutes((int)floor((float)nCurrentTicks/60000));
		SetSeconds((nCurrentTicks/1000)%60);
	}
}

void CCTITimer::Reset() {
	SetSeconds(0);
	SetMinutes(0);
	m_nStartTicks = 0;
}

int CCTITimer::GetTotalSeconds() 
{ 
	Update();
	return m_nMinutes*60+m_nSeconds; 
}