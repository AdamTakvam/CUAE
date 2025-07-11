// Registry_Name_Space.cpp,v 4.15 2002/03/13 08:43:43 jwillemsen Exp

#include "ace/Registry_Name_Space.h"

ACE_RCSID(ace, Registry_Name_Space, "Registry_Name_Space.cpp,v 4.15 2002/03/13 08:43:43 jwillemsen Exp")

#if (defined (ACE_WIN32) && defined (UNICODE))
// This only works on Win32 platforms when UNICODE is turned on

ACE_Registry_Name_Space::ACE_Registry_Name_Space (void)
{
}

ACE_Registry_Name_Space::ACE_Registry_Name_Space (ACE_Name_Options *name_options)
{
  if (this->open (name_options) != 0)
    ACE_ERROR ((LM_ERROR,  ACE_LIB_TEXT ("%p\n"),
                ACE_LIB_TEXT ("ACE_Registry_Name_Space::open")));
}


ACE_Registry_Name_Space::~ACE_Registry_Name_Space (void)
{
}


int
ACE_Registry_Name_Space::open (ACE_Name_Options *name_options)
{
  const ACE_TCHAR *host = name_options->nameserver_host ();
  ACE_Registry::Naming_Context predefined;

  int result = ACE_Predefined_Naming_Contexts::connect (predefined,
                                                        HKEY_LOCAL_MACHINE,
                                                        host);
  if (result != 0)
    ACE_ERROR_RETURN ((LM_ERROR,  ACE_LIB_TEXT ("%p\n"),
                       ACE_LIB_TEXT ("ACE_Predefined_Naming_Context::connect")),
                      result);
  else
    {
      // Directory
      ACE_TString name = name_options->namespace_dir ();
      // Separator
      name += ACE_Registry::STRING_SEPARATOR;
      // Filename
      name += name_options->database ();

      // Create new context or bind to existing one
      result = predefined.bind_context (name,
                                        this->context_);
      if (result != 0)
        ACE_ERROR_RETURN ((LM_ERROR,  ACE_LIB_TEXT ("%p\n"),  ACE_LIB_TEXT ("ACE_Registry::Naming_Context::bind_context")), result);
    }
  return 0;
}


int
ACE_Registry_Name_Space::bind (const ACE_NS_WString &name,
                               const ACE_NS_WString &value,
                               const char *type)
{
  ACE_UNUSED_ARG(type);

  // Pointer to data
  const ACE_WSTRING_TYPE *data = value.fast_rep ();

  // Size
  u_long size = value.length () * sizeof (ACE_WSTRING_TYPE);

  // Represent value as an ACE_Registry::Object
  ACE_Registry::Object object ((void *) data,
                               size,
                               REG_SZ);
  // Add new <key>/<value> pair
#if defined ACE_USES_WCHAR
  return this->context_.bind (name.fast_rep(),
                              object);
#else
  return this->context_.bind (name.char_rep(),
                              object);
#endif /* ACE_HAS_WCHAR */
}


int
ACE_Registry_Name_Space::rebind (const ACE_NS_WString &name,
                                 const ACE_NS_WString &value,
                                 const char *type)
{
  ACE_UNUSED_ARG(type);

  // Pointer to data
  const ACE_WSTRING_TYPE *data = value.fast_rep ();

  // Size
  u_long size = value.length () * sizeof (ACE_WSTRING_TYPE);

  // Represent value as an ACE_Registry::Object
  ACE_Registry::Object object ((void *) data,
                               size,
                               REG_SZ);
  // Add new <key>/<value> pair
#if defined (ACE_USES_WCHAR)
  return this->context_.rebind (name.fast_rep (),
                                object);
#else
  return this->context_.rebind (name.char_rep (),
                                object);
#endif /* ACE_USES_WCHAR */
}


int
ACE_Registry_Name_Space::unbind (const ACE_NS_WString &name)
{
#if defined (ACE_USES_WCHAR)
  return this->context_.unbind (name.fast_rep ());
#else
  return this->context_.unbind (name.char_rep ());
#endif /* ACE_USES_WCHAR */
}


int
ACE_Registry_Name_Space::resolve (const ACE_NS_WString &name,
                                  ACE_NS_WString &value,
                                  char *&type)
{
  ACE_UNUSED_ARG(type);

  // This object will be used to query the size of the data.
  // Note: The query_object.data will be null for this invocation.
  ACE_Registry::Object query_object;
  int result =
#if defined (ACE_USES_WCHAR)
    this->context_.resolve (name.fast_rep (), query_object);
#else
    this->context_.resolve (name.char_rep (), query_object);
#endif /* ACE_USES_WCHAR */
  if (result != 0)
    return result;

  // Resize the value passed by the user
  // Note: -1 is used because the size includes the null terminator
  value.resize ((query_object.size () - 1) / sizeof (ACE_WSTRING_TYPE));

  // Represent new space as an ACE_Registry::Object
  ACE_Registry::Object object ((void *) value.fast_rep (),
                               query_object.size (),
                               REG_SZ);

#if defined (ACE_USES_WCHAR)
  result = this->context_.resolve (name.fast_rep (), object);
#else
  result = this->context_.resolve (name.char_rep (), object);
#endif /* ACE_USES_WCHAR */
  if (object.size () != query_object.size ())
    return -1;
  if (result != 0)
    return result;

  return 0;
}


int
ACE_Registry_Name_Space:: list_names (ACE_WSTRING_SET &set,
                                      const ACE_NS_WString &pattern)
{
  ACE_BINDING_SET binding_set;
  int result = this->list_name_entries (binding_set,
                                        pattern);
  if (result != 0)
    return result;

  ACE_BINDING_ITERATOR iterator (binding_set);

  for (ACE_Name_Binding *entry = 0;
       iterator.next (entry) !=0;
       iterator.advance())
    {
      set.insert (entry->name_);
    }
  return 0;
}


int
ACE_Registry_Name_Space::list_values (ACE_WSTRING_SET &set,
                                      const ACE_NS_WString &pattern)
{
  ACE_BINDING_SET binding_set;
  int result = this->list_name_entries (binding_set,
                                        pattern);
  if (result != 0)
    return result;

  ACE_BINDING_ITERATOR iterator (binding_set);

  for (ACE_Name_Binding *entry = 0;
       iterator.next (entry) !=0;
       iterator.advance())
    {
      set.insert (entry->value_);
    }
  return 0;
}


int
ACE_Registry_Name_Space::list_types (ACE_WSTRING_SET &set,
                                     const ACE_NS_WString &pattern)
{
  ACE_UNUSED_ARG(set);
  ACE_UNUSED_ARG(pattern);

  return 0;
}


int
ACE_Registry_Name_Space::list_name_entries (ACE_BINDING_SET &set,
                                            const ACE_NS_WString &pattern)
{
  ACE_UNUSED_ARG(pattern);

  ACE_Registry::Binding_List list;
  int result = this->context_.list (list);
  if (result != 0)
    return result;

  // Iterator through all entries
  for (ACE_Registry::Binding_List::iterator i = list.begin ();
       i != list.end ();
       i++)
    {
      // Yeeesss! STL rules!
      ACE_Registry::Binding &binding = *i;

      if (binding.type () == ACE_Registry::OBJECT)
        {
          // Key
          ACE_TString string = binding.name ();
          ACE_NS_WString key (string.c_str ());

          // Value
          ACE_NS_WString value;
          char *type = 0;
          result = this->resolve (key,
                                  value,
                                  type);
          if (result != 0)
            ACE_ERROR_RETURN ((LM_ERROR,  ACE_LIB_TEXT ("%p\n"),  ACE_LIB_TEXT ("ACE_Registry::Naming_Context::resolve")), result);

          // Complete binding
          ACE_Name_Binding binding (key, value, type);
          set.insert (binding);
        }
    }
  return 0;
}


int
ACE_Registry_Name_Space::list_value_entries (ACE_BINDING_SET &set,
                                             const ACE_NS_WString &pattern)
{
  return this->list_name_entries (set, pattern);
}


int
ACE_Registry_Name_Space::list_type_entries (ACE_BINDING_SET &set,
                                            const ACE_NS_WString &pattern)
{
  return this->list_name_entries (set, pattern);
}


void
ACE_Registry_Name_Space::dump (void) const
{
}


#endif /* ACE_WIN32 && UNICODE */
