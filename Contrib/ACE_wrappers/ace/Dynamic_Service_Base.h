/* -*- C++ -*- */

//=============================================================================
/**
 *  @file Dynamic_Service_Base.h
 *
 *  Dynamic_Service_Base.h,v 4.3 2002/04/10 17:44:15 ossama Exp
 *
 *  @author Prashant Jain <pjain@cs.wustl.edu>
 *  @author Douglas C. Schmidt <schmidt@cs.wustl.edu>
 */
//=============================================================================

#ifndef ACE_DYNAMIC_SERVICE_BASE_H
#define ACE_DYNAMIC_SERVICE_BASE_H

#include "ace/pre.h"

#include "ace/ACE_export.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */


/**
 * @class ACE_Dynamic_Service_Base
 *
 * @brief Base class for all ACE_Dynamic_Service instantiations.
 *
 * Factors out common code shared by all ACE_Dynamic_Service
 * instantiations, this avoid code bloat.
 */
class ACE_Export ACE_Dynamic_Service_Base
{
public:
  /// Dump the current static of the object
  void dump (void) const;

protected:
  /// Return instance using <name> to search the Service_Repository.
  static void* instance (const ACE_TCHAR *name);
};

#include "ace/post.h"
#endif /* ACE_DYNAMIC_SERVICE_BASE_H */
