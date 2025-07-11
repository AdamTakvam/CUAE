// -*- C++ -*-

//=============================================================================
/**
 *  @file    Dirent.h
 *
 *  Dirent.h,v 4.20 2002/04/10 17:44:15 ossama Exp
 *
 *  Define a portable C++ interface to <ACE_OS_Dirent> directory-entry manipulation.
 *
 *  @author Douglas C. Schmidt <schmidt@cs.wustl.edu>
 */
//=============================================================================

#ifndef ACE_DIRENT_H
#define ACE_DIRENT_H
#include "ace/pre.h"

#include "ace/ACE_export.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/OS_Dirent.h"

/**
 * @class ACE_Dirent
 *
 * @brief Define a portable C++ directory-entry iterator based on the POSIX API.
 */
class ACE_Export ACE_Dirent
{
public:
  // = Initialization and termination methods.
  /// Default constructor.
  ACE_Dirent (void);

  /// Constructor calls <opendir>
  ACE_EXPLICIT ACE_Dirent (const ACE_TCHAR *dirname);

  /// Opens the directory named by filename and associates a directory
  /// stream with it.
  int open (const ACE_TCHAR *filename);

  /// Destructor calls <closedir>.
  ~ACE_Dirent (void);

  /// Closes the directory stream and frees the <ACE_DIR> structure.
  void close (void);

  // = Iterator methods.
  /**
   * Returns a pointer to a structure representing the directory entry
   * at the current position in the directory stream to which dirp
   * refers, and positions the directory stream at the next entry,
   * except on read-only filesystems.  It returns a NULL pointer upon
   * reaching the end of the directory stream, or upon detecting an
   * invalid location in the directory.  <readdir> shall not return
   * directory entries containing empty names.  It is unspecified
   * whether entries are returned for dot or dot-dot.  The pointer
   * returned by <readdir> points to data that may be overwritten by
   * another call to <readdir> on the same directory stream.  This
   * data shall not be overwritten by another call to <readdir> on a
   * different directory stream.  <readdir> may buffer several
   * directory entries per actual read operation; <readdir> marks for
   * update the st_atime field of the directory each time the
   * directory is actually read.
   */
  dirent *read (void);

  /**
   * Has the equivalent functionality as <readdir> except that an
   * <entry> and <result> buffer must be supplied by the caller to
   * store the result.
   */
  int read (struct dirent *entry,
            struct dirent **result);

  // = Manipulators.
  /// Returns the current location associated with the directory
  /// stream.
  long tell (void);

  /**
   * Sets the position of the next <readdir> operation on the
   * directory stream.  The new position reverts to the position
   * associated with the directory stream at the time the <telldir>
   * operation that provides loc was performed.  Values returned by
   * <telldir> are good only for the lifetime of the <ACE_DIR> pointer from
   * which they are derived.  If the directory is closed and then
   * reopened, the <telldir> value may be invalidated due to
   * undetected directory compaction.  It is safe to use a previous
   * <telldir> value immediately after a call to <opendir> and before
   * any calls to readdir.
   */
  void seek (long loc);

  /**
   * Resets the position of the directory stream to the beginning of
   * the directory.  It also causes the directory stream to refer to
   * the current state of the corresponding directory, as a call to
   * <opendir> would.
   */
  void rewind (void);

private:
  /// Pointer to the directory stream.
  ACE_DIR *dirp_;
};

#if defined (__ACE_INLINE__)
#include "ace/Dirent.i"
#endif /* __ACE_INLINE__ */

#include "ace/post.h"
#endif /* ACE_DIRENT_H */
