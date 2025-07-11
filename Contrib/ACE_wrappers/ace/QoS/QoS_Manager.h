/* -*- C++ -*- */

//=============================================================================
/**
 *  @file    QoS_Manager.h
 *
 *  QoS_Manager.h,v 1.3 2002/04/23 05:37:39 jwillemsen Exp
 *
 *  @author Vishal Kachroo
 */
//=============================================================================


#ifndef ACE_QOS_MANAGER_H
#define ACE_QOS_MANAGER_H
#include "ace/pre.h"

#include "ace/Addr.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
#define  ACE_LACKS_PRAGMA_ONCE
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/IPC_SAP.h"
#include "ace/Containers_T.h"
#include "ACE_QoS_Export.h"
#include "QoS_Session.h"

/**
 * @class ACE_QoS_Manager
 *
 * @brief This class manages the QoS sessions associated with ACE_SOCK.
 *
 * This class provides functions to manage the QoS
 * associated with a socket.  The idea is to keep the management of
 * QoS for a socket separate from the socket itself. Currently, the
 * manager is used to manage the QoS session set. It will handle more
 * responsibilities in the future.
 */
class ACE_QoS_Export ACE_QoS_Manager
{

public:
  /// Default constructor.
  ACE_QoS_Manager (void);

  /// Default destructor.
  ~ACE_QoS_Manager (void);

  /**
   * Join the given QoS session. A socket can join multiple QoS
   * sessions.  This call adds the given QoS session to the list of
   * QoS sessions that the socket has already joined.
   */
  int join_qos_session (ACE_QoS_Session *qos_session);

  typedef ACE_Unbounded_Set <ACE_QoS_Session *> ACE_QOS_SESSION_SET;

  /// Get the QoS session set.
  ACE_QOS_SESSION_SET qos_session_set (void);

private:

  /// Set of QoS sessions that this socket has joined.
  ACE_QOS_SESSION_SET qos_session_set_;
};

#include "ace/post.h"
#endif /* ACE_QOS_MANAGER_H */
