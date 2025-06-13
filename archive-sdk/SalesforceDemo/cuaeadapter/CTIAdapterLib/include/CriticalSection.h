//
// Copyright (c) 2004 salesforce.com.  All rights reserved.
//

#if !defined(UTILS_CRITICAL_SECTION_H)
#define UTILS_CRITICAL_SECTION_H

/**
 * This class is the base interface for CCriticalSection and CMutex, the thread synchronization 
 * classes used across the Salesforce.com CTI library.  Its methods are generally not called
 * directly, but rather accessed via the "autolock" concept through LockImpl.
 */
class ILockable
{
public:
	virtual ~ILockable() {}
	/**
	 * Enters a synchronized section of code.
	 */
	virtual void Enter() const = 0;
	/**
	 * Leaves a synchronized section of code.
	 */
	virtual void Leave() const = 0;
};


/**
 * This is a resource grabber class that automatically locks the
 * resource on construction and releases the resource on destruction.
 *
 * To define the scope of the lock, use braces { }.
 */
template <class T>
class LockImpl
{
public:
	/**
	 * Acquire the resource.
	 */
	LockImpl(const T* obj) :
		m_Obj(obj)
	{
		if (m_Obj)
			m_Obj->Enter();
	}

	/**
	 * Release the resource.
	 */
	~LockImpl()
	{
		if (m_Obj)
			m_Obj->Leave();
	}

private:
	const T* m_Obj;
};

/**
 * This class is a wrapper for the Win32 CRITICAL_SECTION object.
 * Objects that want to be thread-safe should derive from this class.
 * To lock the resource, they can call Enter().
 * To release the resource, call Leave().
 */
class CCriticalSection : public ILockable
{
public:
	//This LockImpl will lock the input CCriticalSection upon its creation, and will unlock the critical section when it goes out of scope.
    typedef LockImpl<CCriticalSection> AutoLock;

    /**
     * Creates and initializes the CRITICAL_SECTION object.
     */
    CCriticalSection();

    /**
     * Destroys the CRITICAL_SECTION object.
     */
    ~CCriticalSection();

    /**
     * Attempt to enter the critical section.
     * If another thread is using the object, this function blocks until no other thread has the resource.
     */
    void Enter() const;

    /**
     * Leave the critical section. Releases the shared resource.
     */
    void Leave() const;

protected:
    /** The real CRITICAL_SECTION object. */
    CRITICAL_SECTION m_CS;
};

/**
 * This class is a wrapper for the Win32 MUTEX object.
 */
class CMutex : public ILockable
{
public:
    CMutex(const wchar_t* name, BOOL initialOwner = FALSE);
    ~CMutex();

    /**
     * Attempt to enter the critical section.
     * If another thread is using the object, this function blocks until no other thread has the resource.
     */
    void Enter() const;

    /**
     * Leave the critical section. Releases the shared resource.
     */
    void Leave() const;

private:
    HANDLE m_Mutex;
};

#endif
