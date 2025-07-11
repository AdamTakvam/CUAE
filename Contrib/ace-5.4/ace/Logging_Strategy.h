/* -*- C++ -*- */

//=============================================================================
/**
 *  @file    Logging_Strategy.h
 *
 *  Logging_Strategy.h,v 4.13 2002/09/26 09:17:23 jwillemsen Exp
 *
 *  @author Prashant Jain <pjain@cs.wustl.edu>
 */
//=============================================================================

#ifndef ACE_LOGGING_STRATEGY_H
#define ACE_LOGGING_STRATEGY_H

#include "ace/Service_Object.h"
#include "ace/Log_Msg.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#if !defined (ACE_DEFAULT_LOGFILE_POLL_INTERVAL)
#define ACE_DEFAULT_LOGFILE_POLL_INTERVAL 600 /* Seconds */
#endif /* ACE_DEFAULT_LOGFILE_POLL_INTERVAL */

/**
 * @class ACE_Logging_Strategy
 *
 * @brief This class provides the hooks to control the output produced
 * by any of the network services.
 *
 * Depending upon when this service is invoked and with what
 * flags, the output of other network services can be
 * controlled. The output can be streamed to stderr, to a file,
 * to a logging daemon, or it can be set to be "silent".
 * If logging records are output to a file, the file can be set
 * to a maximum size and repeatedly split into new files.  The
 * log file size can be limited at any logging point (i.e.,
 * application, client logging daemon, or server logging daemon)
 * by specifying the -i <sample_interval_in_secs> and -m
 * <max_size_in_KB> options for the Logging_Strategy class in a
 * svc.conf file.
 *
 * By default, two logfiles are generated.  It's possible, however, to
 * generate as many logfiles as necessary to store all the
 * information.  To achieve this, it is only necessary to indicate the
 * maximum size of the logfiles via the -m option and the process will
 * generate automatically the logfiles.  You can control the total
 * number of logfiles created via the -n option.
 *
 * By using the -o option we can also choose the mode of organization
 * of the files, e.g., the first one is the normal used in Unix
 * systems (when cron rotates the logs it keeps the lowest number the
 * most recent one), the second is for increasing speed (we only
 * create a new log file, and don't rotate the others (fewer accesses
 * to disk)).
 */
class ACE_Export ACE_Logging_Strategy : public ACE_Service_Object
{
public:
  /// Constructor.
  ACE_Logging_Strategy (void);

  /// Dynamic linking initialization hook.
  virtual int init (int argc, ACE_TCHAR *argv[]);

  /// Dynamic linking termination hook.
  virtual int fini (void);

  /**
   * Timeout handler which tests logfile size.  If the current logfile
   * size exceeds <max_size_>, the current logfile is closed, saved to
   * logfile.old, and a new logfile is reopened.
   */
  virtual int handle_timeout (const ACE_Time_Value& tv,
                              const void* arg);

  /**
   * Parse arguments provided in svc.conf file.
   * @arg '-f' Pass in the flags (such as OSTREAM, STDERR, LOGGER, VERBOSE,
   *           SILENT, VERBOSE_LITE) used to control logging.
   * @arg '-i' The interval (in seconds) at which the logfile size is sampled
   *           (default is 0, i.e., do not sample by default).
   * @arg '-k' Set the logging key.
   * @arg '-m' Maximum logfile size in Kbytes.
   * @arg '-n' Set the program name for the %n format specifier.
   * @arg '-N' The maximum number of logfiles that we want created.
   * @arg '-o' Specifies that we want the no standard logfiles ordering
   *           (fastest processing in <handle_timeout>).  Default is not to
   *           order logfiles.
   * @arg '-p' Pass in the process-wide priorities to either enable (e.g.,
   *           DEBUG, INFO, WARNING, NOTICE, ERROR, CRITICAL, ALERT,
   *           EMERGENCY) or to disable (e.g., ~DEBUG, ~INFO, ~WARNING,
   *           ~NOTICE, ~ERROR, ~CRITICAL, ~ALERT, ~EMERGENCY).
   * @arg '-s' Ensure that the OSTREAM flag is set.
   * @arg '-t' Pass in the thread-wide priorities to either enable (e.g.,
   *           DEBUG, INFO, WARNING, NOTICE, ERROR, CRITICAL, ALERT,
   *           EMERGENCY) or to disable (e.g., ~DEBUG, ~INFO, ~WARNING,
   *           ~NOTICE, ~ERROR, ~CRITICAL, ~ALERT, ~EMERGENCY).
   * @arg '-w' Cause the logfile to be wiped out, both on startup and on
   *           reconfiguration.
   */
  int parse_args (int argc, ACE_TCHAR *argv[]);

  void log_msg (ACE_Log_Msg *log_msg);

protected:
  /// Tokenize to set all the flags
  void tokenize (ACE_TCHAR *flag_string);

  /// Tokenize to set priorities (either process or thread one).
  void priorities (ACE_TCHAR *priority_string,
                   ACE_Log_Msg::MASK_TYPE mask);

  /// Current thread's priority mask set by <priorities>
  u_long thread_priority_mask_;

  /// Process-wide priority mask set by <priorities>
  u_long process_priority_mask_;

  /// Flags we keep track of.
  u_long flags_;

  /// File name we're logging to.
  ACE_TCHAR *filename_;

  /// Logger key for distributed logging.
  ACE_TCHAR *logger_key_;

  /// Program name to be used for %n format specifier.
  ACE_TCHAR *program_name_;

  /// If non-0 then wipeout the logfile, otherwise append to it.
  /// Default value is 0.
  int wipeout_logfile_;

  /// This tells us in what file we last wrote. It will be increased
  /// to enable multiple log files
  int count_;

  /// If non-0 we have a maximum number of log files we can write.
  /// Default value is 0, i.e., no maximum number.
  int fixed_number_;

  /// If non-0 we order the files as we rotate them.  Default value
  /// is 0, i.e., we do not rotate files by default.
  int order_files_;

  /// Tells us what is the maximum log file to write. We will write
  /// <max_file_number_> + 1 files (includes the current log file).
  /// Default value is 1, i.e., 2 files by default.
  int max_file_number_;

  /// If non-zero, sampling interval (in secs) at which maximum logfile
  /// size is checked, otherwise logfile size can grow indefinitely.
  /// Default value is 0.
  u_long interval_;

  /// Maximum logfile size (in KB).  Default value is
  /// <ACE_DEFAULT_MAX_LOGFILE_SIZE>.
  u_long max_size_;

  /// ACE_Log_Msg instance to work with
  ACE_Log_Msg *log_msg_;
};

ACE_FACTORY_DECLARE (ACE, ACE_Logging_Strategy)

#endif /* ACE_LOGGING_STRATEGY_H */
