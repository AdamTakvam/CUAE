/* -*- C++ -*- */

//=============================================================================
/**
 *  @file Intrusive_List_Node.h
 *
 *  Intrusive_List_Node.h,v 4.2 2002/05/02 04:08:13 ossama Exp
 *
 *  @author Carlos O'Ryan <coryan@uci.edu>
 */
//=============================================================================

#ifndef ACE_INTRUSIVE_LIST_NODE_H
#define ACE_INTRUSIVE_LIST_NODE_H
#include "ace/pre.h"

#include "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

/**
 * @class ACE_Intrusive_List_Node
 *
 * @brief Implement the requirements for ACE_Intrusive_List
 *
 * The class should be used as follows:
 *
 * class My_Object : public ACE_Intrusive_List_Node<My_Object> {<BR>
 * ....<BR>
 * };<BR>
 *
 * However, ACE is supported on platforms that would surely get
 * confused using such templates, the class is provided as a helper
 * for our lucky users that only need portability to modern C++
 * compilers.
 *
 */
template <class T>
class ACE_Intrusive_List_Node
{
public:
  /** @name Accesors and modifiers to the next and previous pointers
   *
   */
  //@{
  T *prev (void) const;
  void prev (T *);
  T *next (void) const;
  void next (T *);
  //@}

protected:
  /// Constructor
  /**
   * The constructor is protected, because only derived classes should
   * be instantiated.
   */
  ACE_Intrusive_List_Node (void);

private:
  /// Head and tail of the list
  T *prev_;
  T *next_;
};

#if defined (__ACE_INLINE__)
#include "ace/Intrusive_List_Node.inl"
#endif /* __ACE_INLINE__ */

#if defined (ACE_TEMPLATES_REQUIRE_SOURCE)
#include "ace/Intrusive_List_Node.cpp"
#endif /* ACE_TEMPLATES_REQUIRE_SOURCE */

#if defined (ACE_TEMPLATES_REQUIRE_PRAGMA)
#pragma implementation ("Intrusive_List_Node.cpp")
#endif /* ACE_TEMPLATES_REQUIRE_PRAGMA */

#include "ace/post.h"
#endif /* ACE_INTRUSIVE_LIST_NODE_H */
