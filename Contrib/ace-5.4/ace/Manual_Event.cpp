// Manual_Event.cpp,v 4.3 2003/11/01 11:15:13 dhinton Exp

#include "ace/Manual_Event.h"

#if !defined (__ACE_INLINE__)
#include "ace/Manual_Event.inl"
#endif /* __ACE_INLINE__ */


ACE_RCSID(ace, Manual_Event, "Manual_Event.cpp,v 4.3 2003/11/01 11:15:13 dhinton Exp")

ACE_Manual_Event::ACE_Manual_Event (int initial_state,
                                    int type,
                                    const char *name,
                                    void *arg)
  : ACE_Event (1,
               initial_state,
               type,
               ACE_TEXT_CHAR_TO_TCHAR (name),
               arg)
{
}

#if defined (ACE_HAS_WCHAR)
ACE_Manual_Event::ACE_Manual_Event (int initial_state,
                                    int type,
                                    const wchar_t *name,
                                    void *arg)
  : ACE_Event (1,
               initial_state,
               type,
               ACE_TEXT_WCHAR_TO_TCHAR (name),
               arg)
{
}
#endif /* ACE_HAS_WCHAR */

void
ACE_Manual_Event::dump (void) const
{
#if defined (ACE_HAS_DUMP)
  ACE_Event::dump ();
#endif /* ACE_HAS_DUMP */
}

