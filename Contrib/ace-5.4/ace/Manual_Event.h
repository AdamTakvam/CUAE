// -*- C++ -*-

//==========================================================================
/**
 *  @file    Manual_Event.h
 *
 *  Manual_Event.h,v 4.1 2003/08/04 03:53:51 dhinton Exp
 *
 *   Moved from Synch.h.
 *
 *  @author Douglas C. Schmidt <schmidt@cs.wustl.edu>
 */
//==========================================================================

#ifndef ACE_MANUAL_EVENT_H
#define ACE_MANUAL_EVENT_H
#include /**/ "ace/pre.h"

#include "ace/ACE_export.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/Event.h"

/**
 * @class ACE_Manual_Event
 *
 * @brief Manual Events.
 *
 * Specialization of Event mechanism which wakes up all waiting
 * thread on <signal>.  All platforms support process-scope locking
 * support.  However, only Win32 platforms support global naming and
 * system-scope locking support.
 */
class ACE_Export ACE_Manual_Event : public ACE_Event
{
public:
  /// constructor which will create manual event
  ACE_Manual_Event (int initial_state = 0,
                    int type = USYNC_THREAD,
                    const char *name = 0,
                    void *arg = 0);

#if defined (ACE_HAS_WCHAR)
  /// constructor which will create manual event (wchar_t version)
  ACE_Manual_Event (int initial_state,
                    int type,
                    const wchar_t *name,
                    void *arg = 0);
#endif /* ACE_HAS_WCHAR */

  /// Default dtor.
  ~ACE_Manual_Event (void);

  /// Dump the state of an object.
  void dump (void) const;

  /// Declare the dynamic allocation hooks
  ACE_ALLOC_HOOK_DECLARE;
};

#if defined (__ACE_INLINE__)
#include "ace/Manual_Event.inl"
#endif /* __ACE_INLINE__ */


#include /**/ "ace/post.h"
#endif /* ACE_MANUAL_EVENT_H */
