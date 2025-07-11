// -*- C++ -*-  NamespaceSupport.cpp,v 1.7 2002/08/02 17:44:44 kitty Exp

#include "ACEXML/common/NamespaceSupport.h"

static const ACEXML_Char ACEXML_XMLNS_PREFIX_name[] = {'x', 'm', 'l', 'n', 's', 0};
const ACEXML_Char *ACEXML_NamespaceSupport::XMLNS_PREFIX = ACEXML_XMLNS_PREFIX_name;

static const ACEXML_Char ACEXML_DEFAULT_NS_PREFIX[] = {0};

static const ACEXML_Char ACEXML_TABOO_NS_PREFIX[] = {'x', 'm', 'l', 0};

static const ACEXML_Char ACEXML_XMLNS_URI_name[] = {
  'h', 't', 't', 'p', ':', '/', '/',
  'w', 'w', 'w', '.', 'w', '3', '.', 'o', 'r', 'g', '/',
  'X', 'M', 'L', '/', '1', '9', '9', '8', '/',
  'n', 'a', 'm', 'e', 's', 'p', 'a', 'c', 'e', 0};
const ACEXML_Char *ACEXML_NamespaceSupport::XMLNS = ACEXML_XMLNS_URI_name;

#if !defined (__ACEXML_INLINE__)
# include "ACEXML/common/NamespaceSupport.i"
#endif /* __ACEXML_INLINE__ */

ACEXML_Namespace_Context_Stack::ACEXML_Namespace_Context_Stack (void)
  : head_ (0)
{
}

ACEXML_Namespace_Context_Stack::~ACEXML_Namespace_Context_Stack (void)
{
  // Clean up stuff.
}

int
ACEXML_Namespace_Context_Stack::push (ACEXML_NS_CONTEXT *nsc)
{
  struct NS_Node_T *temp = 0;
  ACE_NEW_RETURN (temp, struct NS_Node_T, -1);

  temp->item_ = nsc;
  temp->next_ = this->head_;

  this->head_ = temp;
  return 0;
}

ACEXML_NS_CONTEXT *
ACEXML_Namespace_Context_Stack::pop (void)
{
  if (this->head_ != 0)
    {
      struct NS_Node_T *temp = this->head_;
      this->head_ = temp->next_;

      ACEXML_NS_CONTEXT* retv = temp->item_;
      delete temp;
      return retv;
    }
  return 0;
}


ACEXML_NamespaceSupport::ACEXML_NamespaceSupport (void)
  : ns_stack_ (),
    effective_context_ (0)
{
  // @@ No way to tell if the new fails.
  ACE_NEW (effective_context_,
           ACEXML_NS_CONTEXT ());

  ACEXML_String prefix (ACEXML_TABOO_NS_PREFIX, 0, 0);
  ACEXML_String uri (ACEXML_XMLNS_URI_name, 0, 0);
  this->effective_context_->bind (prefix, uri);
}

ACEXML_NamespaceSupport::~ACEXML_NamespaceSupport (void)
{
  while (this->popContext () == 0)
    ;
}

int
ACEXML_NamespaceSupport::declarePrefix (const ACEXML_Char *prefix,
                                        const ACEXML_Char *uri)
{
  // Unless predefined by w3.org(?) NS prefix can never start with
  // "xml".
  if (ACE_OS_String::strcmp (ACEXML_TABOO_NS_PREFIX, prefix) == 0)
    return -1;

  ACEXML_String ns_prefix (prefix, 0, 0);
  ACEXML_String ns_uri (uri, 0, 0);

  return this->effective_context_->bind (ns_prefix,
                                         ns_uri);
}

int
ACEXML_NamespaceSupport::getDeclaredPrefixes (ACEXML_STR_LIST &prefixes) const
{
  ACEXML_NS_CONTEXT_ENTRY *entry;

  // The prefix for default namespace (empty string) is included in
  // the return list.
  for (ACEXML_NS_CONTEXT_ITER iter (*this->effective_context_);
       iter.next (entry) != 0;
       iter.advance ())
    prefixes.enqueue_tail (entry->ext_id_.c_str ());

  return 0;
}

const ACEXML_Char *
ACEXML_NamespaceSupport::getPrefix (const ACEXML_Char *uri) const
{
  ACEXML_NS_CONTEXT_ENTRY *entry;

  for (ACEXML_NS_CONTEXT_ITER iter (*this->effective_context_);
       iter.next (entry) != 0;
       iter.advance ())
    if (entry->int_id_ == ACEXML_String (uri, 0, 0) &&
        entry->ext_id_ != ACEXML_String (ACEXML_DEFAULT_NS_PREFIX, 0, 0))
      return entry->ext_id_.c_str ();

  return 0;                     // Nothing found.
}

int
ACEXML_NamespaceSupport::getPrefixes (ACEXML_STR_LIST &prefixes) const
{
  ACEXML_NS_CONTEXT_ENTRY *entry;

  // The prefix for default namespace (empty string) is not included
  // in the return list.
  for (ACEXML_NS_CONTEXT_ITER iter (*this->effective_context_);
       iter.next (entry) != 0;
       iter.advance ())
    if (entry->ext_id_ != ACEXML_String(ACEXML_DEFAULT_NS_PREFIX, 0, 0))
      prefixes.enqueue_tail (entry->ext_id_.c_str ());
    else
      continue;

  return 0;
}

int
ACEXML_NamespaceSupport::getPrefixes (const ACEXML_Char *uri,
                                      ACEXML_STR_LIST &prefixes) const
{
  ACEXML_NS_CONTEXT_ENTRY *entry;

  for (ACEXML_NS_CONTEXT_ITER iter (*this->effective_context_);
       iter.next (entry) != 0;
       iter.advance ())
    if (entry->int_id_ == ACEXML_String (uri, 0, 0) &&
        entry->ext_id_ != ACEXML_String (ACEXML_DEFAULT_NS_PREFIX, 0, 0))
      prefixes.enqueue_tail (entry->ext_id_.c_str ());
    else
      continue;

  return 0;                     // Nothing found.
}

const ACEXML_Char *
ACEXML_NamespaceSupport::getURI (const ACEXML_Char *prefix) const
{
  ACEXML_NS_CONTEXT_ENTRY *entry;

  if (this->effective_context_->find (ACEXML_String (prefix, 0, 0),
                                      entry) == 0)
    return entry->int_id_.c_str ();
  return 0;
}

int
ACEXML_NamespaceSupport::popContext (void)
{
  delete this->effective_context_;

  if ((this->effective_context_ = this->ns_stack_.pop ()) == 0)
    return -1;
  return 0;
}

int
ACEXML_NamespaceSupport::pushContext (void)
{
  ACEXML_NS_CONTEXT *temp = this->effective_context_;
  ACE_NEW_RETURN (this->effective_context_,
                  ACEXML_NS_CONTEXT (),
                  -1);

  // @@ Copy everything from the old context to the new one.
  ACEXML_NS_CONTEXT_ENTRY *entry;

  for (ACEXML_NS_CONTEXT_ITER iter (*temp);
       iter.next (entry) != 0;
       iter.advance ())
    this->effective_context_->bind (entry->ext_id_,
                                    entry->int_id_);
  this->ns_stack_.push (temp);
  return 0;
}


int
ACEXML_NamespaceSupport::processName (const ACEXML_Char *qName,
                                      const ACEXML_Char *&uri,
                                      const ACEXML_Char *&name,
                                      int is_attribute) const
{
  int qlen = ACE_OS_String::strlen (qName);
  int len = -1;
  for (int i = 0; i < qlen; ++i)
    if (qName [i] == ':')
      {
        len = i;
        break;
      }

  ACEXML_String prefix;

  if (len == -1)
    {
      name = qName;
    }
  else
    {
      prefix.set (qName, len, 1);
      name = qName + len + 1;
    }

  if (is_attribute && len == -1) {
    uri = ACEXML_DEFAULT_NS_PREFIX;
    return 0;
  }

  ACEXML_NS_CONTEXT_ENTRY *entry;

  if (this->effective_context_->find (prefix, entry) == 0)
    uri = entry->int_id_.c_str ();
  else
    {
      uri = ACEXML_DEFAULT_NS_PREFIX;
      return -1;
    }

  return 0;
}

int
ACEXML_NamespaceSupport::reset (void)
{
  // Not implemented.
  return 0;
}

#if defined (ACE_HAS_EXPLICIT_TEMPLATE_INSTANTIATION)
template class ACE_Hash_Map_Entry<ACEXML_String, ACEXML_String>;
template class ACE_Hash_Map_Manager_Ex<ACEXML_String, ACEXML_String, ACE_Hash<ACEXML_String>, ACE_Equal_To<ACEXML_String>, ACE_Null_Mutex>;
template class ACE_Hash_Map_Iterator_Base_Ex<ACEXML_String, ACEXML_String, ACE_Hash<ACEXML_String>, ACE_Equal_To<ACEXML_String>, ACE_Null_Mutex>;
template class ACE_Hash_Map_Iterator_Ex<ACEXML_String, ACEXML_String, ACE_Hash<ACEXML_String>, ACE_Equal_To<ACEXML_String>, ACE_Null_Mutex>;
template class ACE_Hash_Map_Reverse_Iterator_Ex<ACEXML_String, ACEXML_String, ACE_Hash<ACEXML_String>, ACE_Equal_To<ACEXML_String>, ACE_Null_Mutex>;
template class ACE_Unbounded_Queue<const ACEXML_Char *>;
template class ACE_Unbounded_Queue_Iterator<const ACEXML_Char *>;
template class ACE_Node<const ACEXML_Char *>;
#elif defined (ACE_HAS_TEMPLATE_INSTANTIATION_PRAGMA)
#pragma instantiate ACE_Hash_Map_Entry<ACEXML_String, ACEXML_String>
#pragma instantiate ACE_Hash_Map_Manager_Ex<ACEXML_String, ACEXML_String, ACE_Hash<ACEXML_String>, ACE_Equal_To<ACEXML_String>, ACE_Null_Mutex>
#pragma instantiate ACE_Hash_Map_Iterator_Base_Ex<ACEXML_String, ACEXML_String, ACE_Hash<ACEXML_String>, ACE_Equal_To<ACEXML_String>, ACE_Null_Mutex>
#pragma instantiate ACE_Hash_Map_Iterator_Ex<ACEXML_String, ACEXML_String, ACE_Hash<ACEXML_String>, ACE_Equal_To<ACEXML_String>, ACE_Null_Mutex>
#pragma instantiate ACE_Hash_Map_Reverse_Iterator_Ex<ACEXML_String, ACEXML_String, ACE_Hash<ACEXML_String>, ACE_Equal_To<ACEXML_String>, ACE_Null_Mutex>
#pragma instantiate ACE_Unbounded_Queue<const ACEXML_Char *>
#pragma instantiate ACE_Unbounded_Queue_Iterator<const ACEXML_Char *>
#pragma instantiate ACE_Node<const ACEXML_Char *>
#endif /* ACE_HAS_EXPLICIT_TEMPLATE_INSTANTIATION */
