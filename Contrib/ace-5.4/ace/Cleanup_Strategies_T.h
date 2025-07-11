// -*- C++ -*-

//=============================================================================
/**
 *  @file    Cleanup_Strategies_T.h
 *
 *  Cleanup_Strategies_T.h,v 4.13 2003/07/19 19:04:11 dhinton Exp
 *
 *  @author Kirthika Parameswaran <kirthika@cs.wustl.edu>
 */
//=============================================================================


#ifndef CLEANUP_STRATEGIES_H
#define CLEANUP_STRATEGIES_H
#include /**/ "ace/pre.h"

#include "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

// For linkers that cant grok long names.
#define ACE_Cleanup_Strategy ACLE

/**
 * @class ACE_Cleanup_Strategy
 *
 * @brief Defines a default strategy to be followed for cleaning up
 * entries from a map which is the container.
 *
 * By default the entry to be cleaned up is removed from the
 * container.
 */
template <class KEY, class VALUE, class CONTAINER>
class ACE_Cleanup_Strategy
{

public:

  /// The method which will do the cleanup of the entry in the container.
  virtual int cleanup (CONTAINER &container, KEY *key, VALUE *value);
};

//////////////////////////////////////////////////////////////////////
#define ACE_Recyclable_Handler_Cleanup_Strategy ARHCLE

/**
 * @class ACE_Recyclable_Handler_Cleanup_Strategy
 *
 * @brief Defines a strategy to be followed for cleaning up
 * entries which are svc_handlers from a container.
 *
 * The entry to be cleaned up is removed from the container.
 * Here, since we are dealing with svc_handlers specifically, we
 * perform a couple of extra operations. Note: To be used when
 * the handler is recyclable.
 */
template <class KEY, class VALUE, class CONTAINER>
class ACE_Recyclable_Handler_Cleanup_Strategy : public ACE_Cleanup_Strategy<KEY, VALUE, CONTAINER>
{

public:

  /// The method which will do the cleanup of the entry in the container.
  virtual int cleanup (CONTAINER &container, KEY *key, VALUE *value);
};

//////////////////////////////////////////////////////////////////////
#define ACE_Refcounted_Recyclable_Handler_Cleanup_Strategy ARRHCLE

/**
 * @class ACE_Refcounted_Recyclable_Handler_Cleanup_Strategy
 *
 * @brief Defines a strategy to be followed for cleaning up
 * entries which are svc_handlers from a container.
 *
 * The entry to be cleaned up is removed from the container.
 * Here, since we are dealing with recyclable svc_handlers with
 * addresses which are refcountable specifically, we perform a
 * couple of extra operations and do so without any locking.
 */
template <class KEY, class VALUE, class CONTAINER>
class ACE_Refcounted_Recyclable_Handler_Cleanup_Strategy : public ACE_Cleanup_Strategy<KEY, VALUE, CONTAINER>
{

public:

  /// The method which will do the cleanup of the entry in the container.
  virtual int cleanup (CONTAINER &container, KEY *key, VALUE *value);
};

//////////////////////////////////////////////////////////////////////

/**
 * @class ACE_Handler_Cleanup_Strategy
 *
 * @brief Defines a strategy to be followed for cleaning up
 * entries which are svc_handlers from a container.
 *
 * The entry to be cleaned up is removed from the container.
 * Here, since we are dealing with svc_handlers specifically, we
 * perform a couple of extra operations. Note: This cleanup strategy
 * should be used in the case when the handler has the caching
 * attributes.
 */
template <class KEY, class VALUE, class CONTAINER>
class ACE_Handler_Cleanup_Strategy : public ACE_Cleanup_Strategy<KEY, VALUE, CONTAINER>
{

public:

  /// The method which will do the cleanup of the entry in the container.
  virtual int cleanup (CONTAINER &container, KEY *key, VALUE *value);
};

//////////////////////////////////////////////////////////////////////
#define ACE_Null_Cleanup_Strategy ANCLE

/**
 * @class ACE_Null_Cleanup_Strategy
 *
 * @brief Defines a do-nothing implementation of the cleanup strategy.
 *
 * This class simply does nothing at all! Can be used to nullify
 * the effect of the Cleanup Strategy.
 */
template <class KEY, class VALUE, class CONTAINER>
class ACE_Null_Cleanup_Strategy : public ACE_Cleanup_Strategy<KEY, VALUE, CONTAINER>
{

public:

  /// The dummy cleanup method.
  virtual int cleanup (CONTAINER &container, KEY *key, VALUE *value);
};

#if defined (ACE_TEMPLATES_REQUIRE_SOURCE)
#include "ace/Cleanup_Strategies_T.cpp"
#endif /* ACE_TEMPLATES_REQUIRE_SOURCE */

#if defined (ACE_TEMPLATES_REQUIRE_PRAGMA)
#pragma implementation ("Cleanup_Strategies_T.cpp")
#endif /* ACE_TEMPLATES_REQUIRE_PRAGMA */

#include /**/ "ace/post.h"
#endif /* CLEANUP_STRATEGIES_H */
