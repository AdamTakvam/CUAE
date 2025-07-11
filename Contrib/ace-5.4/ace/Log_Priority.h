/* -*- C++ -*- */

//=============================================================================
/**
 *  @file    Log_Priority.h
 *
 *  Log_Priority.h,v 4.18 2003/07/19 19:04:11 dhinton Exp
 *
 *  @author Douglas C. Schmidt <schmidt@cs.wustl.edu>
 */
//=============================================================================

#ifndef ACE_LOG_PRIORITY_H
#define ACE_LOG_PRIORITY_H
#include /**/ "ace/pre.h"

/**
 * @brief This data type indicates the relative priorities of the
 *    logging messages, from lowest to highest priority. 
 *
 * These values are defined using powers of two so that it's
 * possible to form a mask to turn them on or off dynamically.
 * We only use 12 bits, however, so users are free to use the
 * remaining 19 bits to define their own priority masks.  
 */
enum ACE_Log_Priority
{
  // = Note, this first argument *must* start at 1!

  /// Shutdown the logger (decimal 1).
  LM_SHUTDOWN = 01,

  /// Messages indicating function-calling sequence (decimal 2).
  LM_TRACE = 02,

  /// Messages that contain information normally of use only when
  /// debugging a program (decimal 4).
  LM_DEBUG = 04,

  /// Informational messages (decimal 8).
  LM_INFO = 010,

  /// Conditions that are not error conditions, but that may require
  /// special handling (decimal 16).
  LM_NOTICE = 020,

  /// Warning messages (decimal 32).
  LM_WARNING = 040,

  /// Initialize the logger (decimal 64).
  LM_STARTUP = 0100,

  /// Error messages (decimal 128).
  LM_ERROR = 0200,

  /// Critical conditions, such as hard device errors (decimal 256).
  LM_CRITICAL = 0400,

  /// A condition that should be corrected immediately, such as a
  /// corrupted system database (decimal 512).
  LM_ALERT = 01000,

  /// A panic condition.  This is normally broadcast to all users
  /// (decimal 1024).
  LM_EMERGENCY = 02000,

  /// The maximum logging priority.
  LM_MAX = LM_EMERGENCY,

  /// Do not use!!  This enum value ensures that the underlying
  /// integral type for this enum is at least 32 bits.
  LM_ENSURE_32_BITS = 0x7FFFFFFF
};

#include /**/ "ace/post.h"
#endif /* ACE_LOG_PRIORITY_H */
