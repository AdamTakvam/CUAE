/* -*- C++ -*- */
// Singleton.i,v 4.6 2002/05/28 11:08:38 dhinton Exp

// Default constructors.
//
// Note: don't explicitly initialize "instance_", because TYPE may not
// have a default constructor.  Let the compiler figure it out . . .

template <class TYPE, class ACE_LOCK> ACE_INLINE
ACE_Singleton<TYPE, ACE_LOCK>::ACE_Singleton (void)
{
}

template <class TYPE, class ACE_LOCK> ACE_INLINE
ACE_Unmanaged_Singleton<TYPE, ACE_LOCK>::ACE_Unmanaged_Singleton (void)
{
}

template <class TYPE, class ACE_LOCK> ACE_INLINE
ACE_TSS_Singleton<TYPE, ACE_LOCK>::ACE_TSS_Singleton (void)
{
}

template <class TYPE, class ACE_LOCK> ACE_INLINE
ACE_Unmanaged_TSS_Singleton<TYPE, ACE_LOCK>::ACE_Unmanaged_TSS_Singleton (void)
{
}

template <class TYPE, class ACE_LOCK> ACE_INLINE
ACE_DLL_Singleton_T<TYPE, ACE_LOCK>::ACE_DLL_Singleton_T (void)
{
}

template <class TYPE, class ACE_LOCK>
ACE_DLL_Singleton_T<TYPE, ACE_LOCK>::~ACE_DLL_Singleton_T (void)
{
}
