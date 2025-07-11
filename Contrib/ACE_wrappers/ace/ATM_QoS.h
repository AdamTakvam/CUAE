// -*- C++ -*-

//==========================================================================
/**
 *  @file    ATM_QoS.h
 *
 *  ATM_QoS.h,v 4.10 2002/05/02 04:08:13 ossama Exp
 *
 *  @author Joe Hoffert
 */
//==========================================================================


#ifndef ACE_ATM_QoS_H
#define ACE_ATM_QoS_H
#include "ace/pre.h"

#include "ace/config-all.h"

#if !defined(ACE_LACKS_PRAGMA_ONCE)
#pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#if defined (ACE_HAS_ATM)

#if defined (ACE_HAS_FORE_ATM_WS2)
// just map to WS2 GQOS struct
typedef ACE_QoS ATM_QoS;
#elif defined (ACE_HAS_FORE_ATM_XTI)
typedef struct netbuf ATM_QoS;
#elif defined (ACE_HAS_LINUX_ATM)
#include "atm.h"
#include "ATM_Params.h"
typedef struct atm_qos ATM_QoS;
#else
typedef int ATM_QoS;
#endif /* ACE_HAS_FORE_ATM_WS2 || ACE_HAS_FORE_ATM_XTI || ACE_HAS_LINUX_ATM */

/**
 * @class ACE_ATM_QoS
 *
 * @brief Define the QoS parameters for ATM
 *
 * This class wraps up QoS parameters for both ATM/XTI and
 * ATM/WinSock2 to make the mechanism for the ATM protocol
 * transparent.
 */
class ACE_Export ACE_ATM_QoS
{
public:
  // Constants used for ATM options
  static const long LINE_RATE;
  static const int OPT_FLAGS_CPID;
  static const int OPT_FLAGS_PMP;
  static const int DEFAULT_SELECTOR;
  static const int DEFAULT_PKT_SIZE;

  // = Initializattion and termination methods.
  /// Default constructor.
  ACE_ATM_QoS(int = DEFAULT_PKT_SIZE);

  /// Constructor with a CBR rate.
  ACE_ATM_QoS(int,
              int = DEFAULT_PKT_SIZE);

  ~ACE_ATM_QoS ();

  /// Set the rate.
  void set_rate (ACE_HANDLE,
                 int,
                 int);

  /// Set CBR rate in cells per second.
  void set_cbr_rate (int,
                     int = DEFAULT_PKT_SIZE);

  /// Get ATM_QoS struct.
  ATM_QoS get_qos (void);

  /// Dump the state of an object.
  void dump (void) const;

  /// Declare the dynamic allocation hooks.
  ACE_ALLOC_HOOK_DECLARE;

protected:
  /// Construct QoS options.
  char* construct_options(ACE_HANDLE,
                          int,
                          int,
                          long*);

private:
  ATM_QoS qos_;
};

#if defined (__ACE_INLINE__)
#include "ace/ATM_QoS.i"
#endif /* __ACE_INLINE__ */

#endif /* ACE_HAS_ATM */
#include "ace/post.h"
#endif /* ACE_ATM_QoS_H */
