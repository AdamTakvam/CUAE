#include "stdafx.h"
#include <CriticalSection.h>

CCriticalSection::CCriticalSection()
{
	ZeroMemory(&m_CS, sizeof m_CS);
	InitializeCriticalSection(&m_CS);
}

CCriticalSection::~CCriticalSection()
{
	DeleteCriticalSection(&m_CS);
	ZeroMemory(&m_CS, sizeof m_CS);
}

void CCriticalSection::Enter() const
{
	EnterCriticalSection(const_cast<CRITICAL_SECTION*>(&m_CS));
}

void CCriticalSection::Leave() const
{
	LeaveCriticalSection(const_cast<CRITICAL_SECTION*>(&m_CS));
}


CMutex::CMutex(const wchar_t* name, BOOL initialOwner) :
	m_Mutex(0)
{
#if !defined(_WIN32_WCE)
	m_Mutex = OpenMutexW(MUTEX_ALL_ACCESS, FALSE, name);
#endif

	if (m_Mutex == 0)
	{
		// Failed to open, let's try create
		//
		m_Mutex = CreateMutexW(0, initialOwner, name);

		if (m_Mutex == 0 || m_Mutex == INVALID_HANDLE_VALUE)
		{
			m_Mutex = 0;
		}
	}
}

CMutex::~CMutex()
{
	if (m_Mutex)
	{
		CloseHandle(m_Mutex);
		m_Mutex = 0;
	}
}

void CMutex::Enter() const
{
	if (m_Mutex)
	{
		WaitForSingleObject(m_Mutex, INFINITE);
	}
}

void CMutex::Leave() const
{
	if (m_Mutex)
		ReleaseMutex(m_Mutex);
}

