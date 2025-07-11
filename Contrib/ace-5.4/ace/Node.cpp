// Node.cpp,v 4.4 2003/04/01 10:59:08 okellogg Exp

#ifndef ACE_NODE_C
#define ACE_NODE_C

#include "ace/Node.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

ACE_RCSID(ace, Node, "Node.cpp,v 4.4 2003/04/01 10:59:08 okellogg Exp")

ACE_ALLOC_HOOK_DEFINE(ACE_Node)

# if ! defined (ACE_HAS_BROKEN_NOOP_DTORS)
  template <class T>
ACE_Node<T>::~ACE_Node (void)
{
}
# endif /* ! defined (ACE_HAS_BROKEN_NOOP_DTORS) */

template <class T>
ACE_Node<T>::ACE_Node (const T &i, ACE_Node<T> *n)
  : next_ (n),
    item_ (i),
    deleted_ (false)
{
  // ACE_TRACE ("ACE_Node<T>::ACE_Node");
}

template <class T>
ACE_Node<T>::ACE_Node (ACE_Node<T> *n, int)
  : next_ (n),
    deleted_ (false)
{
  // ACE_TRACE ("ACE_Node<T>::ACE_Node");
}

template <class T>
ACE_Node<T>::ACE_Node (const ACE_Node<T> &s)
  : next_ (s.next_),
    item_ (s.item_),
    deleted_ (false)
{
  // ACE_TRACE ("ACE_Node<T>::ACE_Node");
}

#endif /* ACE_NODE_C */
