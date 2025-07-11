// Configuration.cpp,v 4.69 2002/07/31 05:46:38 jwillemsen Exp
#include "ace/Configuration.h"
#include "ace/Auto_Ptr.h"

// Can remove this when import_config and export_config are removed from
// ACE_Configuration. They're deprecated at ACE 5.2.
#include "ace/Configuration_Import_Export.h"

#if defined (ACE_HAS_EXPLICIT_TEMPLATE_INSTANTIATION)

#if defined (ACE_HAS_THREADS)
// ACE_SYNCH_MUTEX should not be used in the template instantiations
// because the resulting template instantiation for the
// single-threaded case already exists in ACE.
template class ACE_Allocator_Adapter<ACE_Malloc<ACE_MMAP_MEMORY_POOL, ACE_Thread_Mutex> >;
template class ACE_Malloc<ACE_MMAP_MEMORY_POOL, ACE_Thread_Mutex>;
template class ACE_Malloc_T<ACE_MMAP_MEMORY_POOL, ACE_Thread_Mutex, ACE_Control_Block>;
#endif /* ACE_HAS_THREADS */
template class ACE_Hash_Map_Entry<ACE_Configuration_ExtId, ACE_Configuration_Section_IntId>;
template class ACE_Hash_Map_Entry<ACE_Configuration_ExtId, ACE_Configuration_Value_IntId>;
template class ACE_Hash_Map_Entry<ACE_Configuration_ExtId, int>;
template class ACE_Hash_Map_Iterator_Base_Ex<ACE_Configuration_ExtId, ACE_Configuration_Section_IntId, ACE_Hash<ACE_Configuration_ExtId>, ACE_Equal_To<ACE_Configuration_ExtId>, ACE_Null_Mutex>;
template class ACE_Hash_Map_Iterator_Base_Ex<ACE_Configuration_ExtId, ACE_Configuration_Value_IntId, ACE_Hash<ACE_Configuration_ExtId>, ACE_Equal_To<ACE_Configuration_ExtId>, ACE_Null_Mutex>;
template class ACE_Hash_Map_Iterator_Base_Ex<ACE_Configuration_ExtId, int, ACE_Hash<ACE_Configuration_ExtId>, ACE_Equal_To<ACE_Configuration_ExtId>, ACE_Null_Mutex>;


// Added to fix problems in SunOS CC5.0
template class ACE_Hash_Map_Reverse_Iterator_Ex<ACE_Configuration_ExtId,ACE_Configuration_Value_IntId,ACE_Hash<ACE_Configuration_ExtId>,ACE_Equal_To<ACE_Configuration_ExtId>,ACE_Null_Mutex>;
template class ACE_Hash_Map_Iterator_Ex<ACE_Configuration_ExtId,ACE_Configuration_Value_IntId,ACE_Hash<ACE_Configuration_ExtId>,ACE_Equal_To<ACE_Configuration_ExtId>,ACE_Null_Mutex>;
template class ACE_Hash_Map_Iterator_Ex<ACE_Configuration_ExtId,int,ACE_Hash<ACE_Configuration_ExtId>,ACE_Equal_To<ACE_Configuration_ExtId>,ACE_Null_Mutex>;
template class ACE_Hash_Map_Iterator_Ex<ACE_Configuration_ExtId,ACE_Configuration_Section_IntId,ACE_Hash<ACE_Configuration_ExtId>,ACE_Equal_To<ACE_Configuration_ExtId>,ACE_Null_Mutex>;
template class ACE_Equal_To<ACE_Configuration_ExtId>;
template class ACE_Hash_Map_Reverse_Iterator_Ex<ACE_Configuration_ExtId,ACE_Configuration_Section_IntId,ACE_Hash<ACE_Configuration_ExtId>,ACE_Equal_To<ACE_Configuration_ExtId>,ACE_Null_Mutex>;
template class ACE_Hash_Map_Reverse_Iterator_Ex<ACE_Configuration_ExtId,int,ACE_Hash<ACE_Configuration_ExtId>,ACE_Equal_To<ACE_Configuration_ExtId>,ACE_Null_Mutex>;
template class ACE_Hash<ACE_Configuration_ExtId>;

template class ACE_Hash_Map_Manager_Ex<ACE_Configuration_ExtId, ACE_Configuration_Section_IntId, ACE_Hash<ACE_Configuration_ExtId>, ACE_Equal_To<ACE_Configuration_ExtId>, ACE_Null_Mutex>;
template class ACE_Hash_Map_Manager_Ex<ACE_Configuration_ExtId, ACE_Configuration_Value_IntId, ACE_Hash<ACE_Configuration_ExtId>, ACE_Equal_To<ACE_Configuration_ExtId>, ACE_Null_Mutex>;
template class ACE_Hash_Map_Manager_Ex<ACE_Configuration_ExtId, int, ACE_Hash<ACE_Configuration_ExtId>, ACE_Equal_To<ACE_Configuration_ExtId>, ACE_Null_Mutex>;
template class ACE_Hash_Map_Manager<ACE_Configuration_ExtId, ACE_Configuration_Section_IntId, ACE_Null_Mutex>;
template class ACE_Hash_Map_Manager<ACE_Configuration_ExtId, ACE_Configuration_Value_IntId, ACE_Null_Mutex>;
template class ACE_Hash_Map_Manager<ACE_Configuration_ExtId, int, ACE_Null_Mutex>;
template class ACE_Hash_Map_With_Allocator<ACE_Configuration_ExtId, ACE_Configuration_Section_IntId>;
template class ACE_Hash_Map_With_Allocator<ACE_Configuration_ExtId, ACE_Configuration_Value_IntId>;
template class ACE_Hash_Map_With_Allocator<ACE_Configuration_ExtId, int>;

#elif defined (ACE_HAS_TEMPLATE_INSTANTIATION_PRAGMA)

#if defined (ACE_HAS_THREADS)
// ACE_SYNCH_MUTEX should not be used in the template instantiations
// because the resulting template instantiation for the
// single-threaded case already exists in ACE.
#pragma instantiate ACE_Allocator_Adapter<ACE_Malloc<ACE_MMAP_MEMORY_POOL, ACE_Thread_Mutex> >
#pragma instantiate ACE_Malloc<ACE_MMAP_MEMORY_POOL, ACE_Thread_Mutex>
#pragma instantiate ACE_Malloc_T<ACE_MMAP_MEMORY_POOL, ACE_Thread_Mutex, ACE_Control_Block>
#endif /* ACE_HAS_THREADS */
#pragma instantiate ACE_Hash_Map_Entry<ACE_Configuration_ExtId, ACE_Configuration_Section_IntId>
#pragma instantiate ACE_Hash_Map_Entry<ACE_Configuration_ExtId, ACE_Configuration_Value_IntId>
#pragma instantiate ACE_Hash_Map_Entry<ACE_Configuration_ExtId, int>
#pragma instantiate ACE_Hash_Map_Iterator_Base_Ex<ACE_Configuration_ExtId, ACE_Configuration_Section_IntId, ACE_Hash<ACE_Configuration_ExtId>, ACE_Equal_To<ACE_Configuration_ExtId>, ACE_Null_Mutex>
#pragma instantiate ACE_Hash_Map_Iterator_Base_Ex<ACE_Configuration_ExtId, ACE_Configuration_Value_IntId, ACE_Hash<ACE_Configuration_ExtId>, ACE_Equal_To<ACE_Configuration_ExtId>, ACE_Null_Mutex>
#pragma instantiate ACE_Hash_Map_Iterator_Base_Ex<ACE_Configuration_ExtId, int, ACE_Hash<ACE_Configuration_ExtId>, ACE_Equal_To<ACE_Configuration_ExtId>, ACE_Null_Mutex>

#pragma instantiate ACE_Hash_Map_Reverse_Iterator_Ex<ACE_Configuration_ExtId,ACE_Configuration_Value_IntId,ACE_Hash<ACE_Configuration_ExtId>,ACE_Equal_To<ACE_Configuration_ExtId>,ACE_Null_Mutex>
#pragma instantiate ACE_Hash_Map_Iterator_Ex<ACE_Configuration_ExtId,ACE_Configuration_Value_IntId,ACE_Hash<ACE_Configuration_ExtId>,ACE_Equal_To<ACE_Configuration_ExtId>,ACE_Null_Mutex>
#pragma instantiate ACE_Hash_Map_Iterator_Ex<ACE_Configuration_ExtId,int,ACE_Hash<ACE_Configuration_ExtId>,ACE_Equal_To<ACE_Configuration_ExtId>,ACE_Null_Mutex>
#pragma instantiate ACE_Hash_Map_Iterator_Ex<ACE_Configuration_ExtId,ACE_Configuration_Section_IntId,ACE_Hash<ACE_Configuration_ExtId>,ACE_Equal_To<ACE_Configuration_ExtId>,ACE_Null_Mutex>
#pragma instantiate ACE_Equal_To<ACE_Configuration_ExtId>
#pragma instantiate ACE_Hash_Map_Reverse_Iterator_Ex<ACE_Configuration_ExtId,ACE_Configuration_Section_IntId,ACE_Hash<ACE_Configuration_ExtId>,ACE_Equal_To<ACE_Configuration_ExtId>,ACE_Null_Mutex>
#pragma instantiate ACE_Hash_Map_Reverse_Iterator_Ex<ACE_Configuration_ExtId,int,ACE_Hash<ACE_Configuration_ExtId>,ACE_Equal_To<ACE_Configuration_ExtId>,ACE_Null_Mutex>
#pragma instantiate ACE_Hash<ACE_Configuration_ExtId>

#pragma instantiate ACE_Hash_Map_Manager_Ex<ACE_Configuration_ExtId, ACE_Configuration_Section_IntId, ACE_Hash<ACE_Configuration_ExtId>, ACE_Equal_To<ACE_Configuration_ExtId>, ACE_Null_Mutex>
#pragma instantiate ACE_Hash_Map_Manager_Ex<ACE_Configuration_ExtId, ACE_Configuration_Value_IntId, ACE_Hash<ACE_Configuration_ExtId>, ACE_Equal_To<ACE_Configuration_ExtId>, ACE_Null_Mutex>
#pragma instantiate ACE_Hash_Map_Manager_Ex<ACE_Configuration_ExtId, int, ACE_Hash<ACE_Configuration_ExtId>, ACE_Equal_To<ACE_Configuration_ExtId>, ACE_Null_Mutex>
#pragma instantiate ACE_Hash_Map_Manager<ACE_Configuration_ExtId, ACE_Configuration_Section_IntId, ACE_Null_Mutex>
#pragma instantiate ACE_Hash_Map_Manager<ACE_Configuration_ExtId, ACE_Configuration_Value_IntId, ACE_Null_Mutex>
#pragma instantiate ACE_Hash_Map_Manager<ACE_Configuration_ExtId, int, ACE_Null_Mutex>
#pragma instantiate ACE_Hash_Map_With_Allocator<ACE_Configuration_ExtId, ACE_Configuration_Section_IntId>
#pragma instantiate ACE_Hash_Map_With_Allocator<ACE_Configuration_ExtId, ACE_Configuration_Value_IntId>
#pragma instantiate ACE_Hash_Map_With_Allocator<ACE_Configuration_ExtId, int>

#endif /* ACE_HAS_EXPLICIT_TEMPLATE_INSTANTIATION */

ACE_Section_Key_Internal::ACE_Section_Key_Internal (void)
  : ref_count_ (0)
{
}

ACE_Section_Key_Internal::~ACE_Section_Key_Internal (void)
{
}

int
ACE_Section_Key_Internal::add_ref (void)
{
  ++ref_count_;
  return 0;
}

int
ACE_Section_Key_Internal::dec_ref (void)
{
  if (!--ref_count_)
    delete this;
  return 0;
}

ACE_Configuration_Section_Key::ACE_Configuration_Section_Key (void)
  : key_ (0)
{
}

ACE_Configuration_Section_Key::~ACE_Configuration_Section_Key (void)
{
  if (key_)
    key_->dec_ref ();
}

ACE_Configuration_Section_Key::ACE_Configuration_Section_Key (ACE_Section_Key_Internal* key)
  : key_ (key)
{
  if (key_)
    key_->add_ref ();
}

ACE_Configuration_Section_Key::ACE_Configuration_Section_Key (const ACE_Configuration_Section_Key& rhs)
  : key_ (rhs.key_)
{
  if (key_)
    key_->add_ref ();
}

ACE_Configuration_Section_Key&
ACE_Configuration_Section_Key::operator= (const ACE_Configuration_Section_Key& rhs)
{
  if (this != &rhs)
    {
      if (key_)
        key_->dec_ref ();

      key_ = rhs.key_;

      if (key_)
        key_->add_ref ();
    }
  return *this;
}

//////////////////////////////////////////////////////////////////////////////

ACE_Configuration::ACE_Configuration (void)
  : root_ ()
{
}

ACE_Configuration::~ACE_Configuration (void)
{
}

ACE_Section_Key_Internal*
ACE_Configuration::get_internal_key (const ACE_Configuration_Section_Key& key)
{
  return key.key_;
}

int
ACE_Configuration::expand_path (const ACE_Configuration_Section_Key& key,
                                const ACE_TString& path_in,
                                ACE_Configuration_Section_Key& key_out,
                                int create)
{
  // Make a copy of key
  ACE_Configuration_Section_Key current_section = key;
  ACE_Auto_Basic_Array_Ptr<ACE_TCHAR> pData (path_in.rep ());
  ACE_Tokenizer parser (pData.get ());
  parser.delimiter_replace ('\\', '\0');
  parser.delimiter_replace ('/', '\0');

  for (ACE_TCHAR *temp = parser.next ();
       temp != 0;
       temp = parser.next ())
    {
      // Open the section
      if (open_section (current_section,
                        temp,
                        create,
                        key_out))
        return -1;

      current_section = key_out;
    }

  return 0;

}

// import_config and export_config are here for backward compatibility,
// and have been deprecated.
int
ACE_Configuration::export_config (const ACE_TCHAR* filename)
{
  ACE_Registry_ImpExp exporter (*this);
  return exporter.export_config (filename);
}

int
ACE_Configuration::import_config (const ACE_TCHAR* filename)
{
  ACE_Registry_ImpExp importer (*this);
  return importer.import_config (filename);
}

int
ACE_Configuration::validate_name (const ACE_TCHAR* name, int allow_path)
{
  // Invalid character set
  const ACE_TCHAR* reject =
    allow_path ? ACE_LIB_TEXT ("][") : ACE_LIB_TEXT ("\\][");

  // Position of the first invalid character or terminating null.
  size_t pos = ACE_OS_String::strcspn (name, reject);

  // Check if it is an invalid character.
  if (name[pos] != ACE_LIB_TEXT ('\0'))
    {
      errno = EINVAL;
      return -1;
    }

  // The first character can never be a path separator.
  if (name[0] == ACE_LIB_TEXT ('\\'))
    {
      errno = EINVAL;
      return -1;
    }

  // Validate length.
  if (pos == 0 || pos > 255)
    {
      errno = ENAMETOOLONG;
      return -1;
    }

  return 0;
}


const ACE_Configuration_Section_Key&
ACE_Configuration::root_section (void) const
{
  return root_;
}

/**
 * Determine if the contents of this object is the same as the
 * contents of the object on the right hand side.
 * Returns 1 (True) if they are equal and 0 (False) if they are not equal
 */
int ACE_Configuration::operator== (const ACE_Configuration& rhs) const
{
  int rc = 1;
  int sectionIndex = 0;
  ACE_TString sectionName;
  ACE_Configuration *nonconst_this = ACE_const_cast (ACE_Configuration*, this);
  ACE_Configuration &nonconst_rhs  = ACE_const_cast (ACE_Configuration&, rhs);

  const ACE_Configuration_Section_Key& rhsRoot = rhs.root_section ();
  ACE_Configuration_Section_Key rhsSection;
  ACE_Configuration_Section_Key thisSection;

  // loop through each section in this object
  while ((rc) && (!nonconst_this->enumerate_sections (this->root_,
                                                      sectionIndex,
                                                      sectionName)))
    {
      // find that section in the rhs object
      if (nonconst_rhs.open_section (rhsRoot,
                                     sectionName.c_str (),
                                     0,
                                     rhsSection) != 0)
        {
          // If the rhs object does not contain the section then we are
          // not equal.
          rc = 0;
        }
      else if (nonconst_this->open_section (this->root_,
                                            sectionName.c_str (),
                                            0,
                                            thisSection) != 0)
        {
          // if there is some error opening the section in this object
          rc = 0;
        }
      else
        {
          // Well the sections match
          int         valueIndex = 0;
          ACE_TString valueName;
          VALUETYPE   valueType;
          VALUETYPE   rhsType;

          // Enumerate each value in this section
          while ((rc) && nonconst_this->enumerate_values (thisSection,
                                                          valueIndex,
                                                          valueName,
                                                          valueType))
            {
              // look for the same value in the rhs section
              if (nonconst_rhs.find_value (rhsSection,
                                           valueName.c_str (),
                                           rhsType) != 0)
                {
                  // We're not equal if the same value cannot
                  // be found in the rhs object.
                  rc = 0;
                }
              else if (valueType != rhsType)
                {
                  // we're not equal if the types do not match.
                  rc = 0;
                }
              else
                {
                  // finally compare values.
                  if (valueType == STRING)
                    {
                      ACE_TString thisString, rhsString;
                      if (nonconst_this->get_string_value (thisSection,
                                                           valueName.c_str (),
                                                           thisString) != 0)
                        {
                          // we're not equal if we cannot get this string
                          rc = 0;
                        }
                      else if (nonconst_rhs.get_string_value (rhsSection,
                                                              valueName.c_str (),
                                                              rhsString) != 0)
                        {
                          // we're not equal if we cannot get rhs string
                          rc = 0;
                        }
                      rc = thisString == rhsString;
                    }
                  else if (valueType == INTEGER)
                    {
                      u_int thisInt, rhsInt;
                      if (nonconst_this->get_integer_value (thisSection,
                                                            valueName.c_str (),
                                                            thisInt) != 0)
                        {
                          // we're not equal if we cannot get this int
                          rc = 0;
                        }
                      else if (nonconst_rhs.get_integer_value (rhsSection,
                                                               valueName.c_str (),
                                                               rhsInt) != 0)
                        {
                          // we're not equal if we cannot get rhs int
                          rc = 0;
                        }
                      rc = thisInt == rhsInt;
                    }
                  else if (valueType == BINARY)
                    {
                      void* thisData = 0;
                      void* rhsData = 0;
                      u_int thisLength, rhsLength;
                      if (nonconst_this->get_binary_value (thisSection,
                                                           valueName.c_str (),
                                                           thisData,
                                                           thisLength) != 0)
                        {
                          // we're not equal if we cannot get this data
                          rc = 0;
                        }
                      else if (nonconst_rhs.get_binary_value (rhsSection,
                                                              valueName.c_str (),
                                                              rhsData,
                                                              rhsLength) != 0)
                        {
                          // we're not equal if we cannot get this data
                          rc = 0;
                        }

                      rc = thisLength == rhsLength;
                      // are the length's the same?

                      if (rc)
                        {
                          unsigned char* thisCharData = (unsigned char*)thisData;
                          unsigned char* rhsCharData = (unsigned char*)rhsData;
                          // yes, then check each element
                          for (u_int count = 0;
 (rc) && (count < thisLength);
                               count++)
                            {
                              rc = (* (thisCharData + count) == * (rhsCharData + count));
                            }

                          delete [] thisCharData;
                          delete [] rhsCharData;
                        }// end if the length's match


                    }
                  // We should never have valueTypes of INVALID, therefore
                  // we're not comparing them.  How would we since we have
                  // no get operation for invalid types.
                  // So, if we have them, we guess they are equal.

                }// end else if values match.

              valueIndex++;

            }// end value while loop

          // look in the rhs for values not in this
          valueIndex = 0;
          while ((rc) &&
 (!nonconst_rhs.enumerate_values (rhsSection,
                                                  valueIndex,
                                                  valueName,
                                                  rhsType)))
            {
              // look for the same value in this section
              if (nonconst_this->find_value (thisSection,
                                             valueName.c_str (),
                                             valueType) != 0)
                {
                  // We're not equal if the same value cannot
                  // be found in the rhs object.
                  rc = 0;
                }
              valueIndex++;
            }// end while for rhs values not in this.

        }// end else if sections match.

      sectionIndex++;

    }// end section while loop

  // Finally, make sure that there are no sections in rhs that do not
  // exist in this
  sectionIndex = 0;
  while ((rc)
         && (!nonconst_rhs.enumerate_sections (rhsRoot,
                                               sectionIndex,
                                               sectionName)))
    {
      // find the section in this
      if (nonconst_this->open_section (this->root_,
                                       sectionName.c_str (),
                                       0,
                                       thisSection) != 0)
        {
          // if there is some error opening the section in this object
          rc = 0;
        }
      else if (nonconst_rhs.open_section (rhsRoot,
                                          sectionName.c_str (),
                                          0,
                                          rhsSection) != 0)
        {
          // If the rhs object does not contain the section then we
          // are not equal.
          rc = 0;
        }
      sectionIndex++;
    }
  return rc;
}


//////////////////////////////////////////////////////////////////////////////

#if defined (WIN32)

static const int ACE_DEFAULT_BUFSIZE = 256;

ACE_Section_Key_Win32::ACE_Section_Key_Win32 (HKEY hKey)
  : hKey_ (hKey)
{
}

ACE_Section_Key_Win32::~ACE_Section_Key_Win32 (void)
{
  ::RegCloseKey (hKey_);
}

//////////////////////////////////////////////////////////////////////////////

int
ACE_Configuration_Win32Registry::operator== (const ACE_Configuration_Win32Registry &rhs) const
{
  ACE_UNUSED_ARG (rhs);
  return 1;
}

int
ACE_Configuration_Win32Registry::operator!= (const ACE_Configuration_Win32Registry &rhs) const
{
  ACE_UNUSED_ARG (rhs);
  return 1;
}

ACE_Configuration_Win32Registry::ACE_Configuration_Win32Registry (HKEY hKey)
{
  ACE_Section_Key_Win32 *temp;

  ACE_NEW (temp, ACE_Section_Key_Win32 (hKey));

  root_ = ACE_Configuration_Section_Key (temp);
}


ACE_Configuration_Win32Registry::~ACE_Configuration_Win32Registry (void)
{
}

int
ACE_Configuration_Win32Registry::open_section (const ACE_Configuration_Section_Key& base,
                                               const ACE_TCHAR* sub_section,
                                               int create,
                                               ACE_Configuration_Section_Key& result)
{
  if (validate_name (sub_section, 1))
    return -1;

  HKEY base_key;
  if (load_key (base, base_key))
    return -1;

  HKEY result_key;
  if (ACE_TEXT_RegOpenKeyEx (base_key,
                             sub_section,
                             0,
                             KEY_ALL_ACCESS,
                             &result_key) != ERROR_SUCCESS)
    {
      if (!create)
        return -1;

      if (ACE_TEXT_RegCreateKeyEx (base_key,
                                   sub_section,
                                   0,
                                   0,
                                   REG_OPTION_NON_VOLATILE,
                                   KEY_ALL_ACCESS,
                                   0,
                                   &result_key,
#if defined (__MINGW32__)
 (PDWORD) 0
#else
                                   0
#endif /* __MINGW32__ */
                                   ) != ERROR_SUCCESS)
        return -1;
    }

  ACE_Section_Key_Win32 *temp;

  ACE_NEW_RETURN (temp, ACE_Section_Key_Win32 (result_key), -1);
  result = ACE_Configuration_Section_Key (temp);
  return 0;
}

int
ACE_Configuration_Win32Registry::remove_section (const ACE_Configuration_Section_Key& key,
                                                 const ACE_TCHAR* sub_section,
                                                 int recursive)
{
  if (validate_name (sub_section))
    return -1;

  HKEY base_key;
  if (load_key (key, base_key))
    return -1;

  if (recursive)
    {
      ACE_Configuration_Section_Key section;
      if (open_section (key, sub_section, 0, section))
        return -1;

      HKEY sub_key;
      if (load_key (section, sub_key))
        return -1;

      ACE_TCHAR name_buffer[ACE_DEFAULT_BUFSIZE];
      DWORD buffer_size = ACE_DEFAULT_BUFSIZE;
      // Note we don't increment the index because the
      // enumeration becomes invalid if we change the
      // subkey, which we do when we delete it.  By leaving
      // it 0, we always delete the top entry
      while (ACE_TEXT_RegEnumKeyEx (sub_key,
                                    0,
                                    name_buffer,
                                    &buffer_size,
                                    0,
                                    0,
                                    0,
                                    0) == ERROR_SUCCESS)
        {
          remove_section (section, name_buffer, 1);
          buffer_size = ACE_DEFAULT_BUFSIZE;
        }
    }

#if (ACE_HAS_WINNT4 != 0)
  if (ACE_TEXT_RegDeleteKey (base_key, sub_section) != ERROR_SUCCESS)
    return -1;
#else
  if (!recursive)
    {
      ACE_Configuration_Section_Key section;
      if (open_section (key, sub_section, 0, section))
        return -1;

      HKEY sub_key;
      if (load_key (section, sub_key))
        return -1;

      ACE_TCHAR name_buffer[ACE_DEFAULT_BUFSIZE];
      DWORD buffer_size = ACE_DEFAULT_BUFSIZE;
      // Check for a an entry under the sub_key
      if (ACE_TEXT_RegEnumKeyEx (sub_key,
                                 0,
                                 name_buffer,
                                 &buffer_size,
                                 0,
                                 0,
                                 0,
                                 0) == ERROR_SUCCESS)
        return -1;
    }
  else if (ACE_TEXT_RegDeleteKey (base_key, sub_section) != ERROR_SUCCESS)
    return -1;
#endif

  return 0;
}

int
ACE_Configuration_Win32Registry::enumerate_values (const ACE_Configuration_Section_Key& key,
                                                   int index,
                                                   ACE_TString& name,
                                                   VALUETYPE& type)
{
  HKEY base_key;
  if (load_key (key, base_key))
    return -1;

  ACE_TCHAR name_buffer[ACE_DEFAULT_BUFSIZE];
  DWORD buffer_size = ACE_DEFAULT_BUFSIZE;
  DWORD value_type;

  int rc = ACE_TEXT_RegEnumValue (base_key,
                                  index,
                                  name_buffer,
                                  &buffer_size,
                                  0,
                                  &value_type,
                                  0,
                                  0);
  if (rc == ERROR_NO_MORE_ITEMS)
    return 1;
  else if (rc != ERROR_SUCCESS)
    return -1;

  name = name_buffer;

  switch (value_type)
    {
    case REG_BINARY:
      type = BINARY;
      break;
    case REG_SZ:
      type = STRING;
      break;
    case REG_DWORD:
      type = INTEGER;
      break;
    default:
      type = INVALID;
    }

  return 0;
}

int
ACE_Configuration_Win32Registry::enumerate_sections (const ACE_Configuration_Section_Key& key,
                                                     int index,
                                                     ACE_TString& name)
{
  HKEY base_key;
  if (load_key (key, base_key))
    return -1;

  ACE_TCHAR name_buffer[ACE_DEFAULT_BUFSIZE];
  DWORD buffer_size = ACE_DEFAULT_BUFSIZE;
  int rc = ACE_TEXT_RegEnumKeyEx (base_key,
                                  index,
                                  name_buffer,
                                  &buffer_size,
                                  0,
                                  0,
                                  0,
                                  0);
  if (rc == ERROR_NO_MORE_ITEMS)
    return 1;
  else if (rc != ERROR_MORE_DATA && rc != ERROR_SUCCESS)
    return -1;

  name = name_buffer;

  return 0;
}

int
ACE_Configuration_Win32Registry::set_string_value (const ACE_Configuration_Section_Key& key,
                                                   const ACE_TCHAR* name,
                                                   const ACE_TString& value)
{
  if (validate_name (name))
    return -1;

  HKEY base_key;
  if (load_key (key, base_key))
    return -1;

  if (ACE_TEXT_RegSetValueEx (base_key,
                              name,
                              0,
                              REG_SZ,
                              (BYTE *) value.fast_rep (),
                              (value.length () + 1) * sizeof (ACE_TCHAR))
      != ERROR_SUCCESS)
    return -1;

  return 0;
}

int
ACE_Configuration_Win32Registry::set_integer_value (const ACE_Configuration_Section_Key& key,
                                                    const ACE_TCHAR* name,
                                                    u_int value)
{
  if (validate_name (name))
    return -1;

  HKEY base_key;
  if (load_key (key, base_key))
    return -1;

  if (ACE_TEXT_RegSetValueEx (base_key,
                              name,
                              0,
                              REG_DWORD,
                              (BYTE *) &value,
                              sizeof (value)) != ERROR_SUCCESS)
    return -1;

  return 0;
}

int
ACE_Configuration_Win32Registry::set_binary_value (const ACE_Configuration_Section_Key& key,
                                                   const ACE_TCHAR* name,
                                                   const void* data,
                                                   u_int length)
{
  if (validate_name (name))
    return -1;

  HKEY base_key;
  if (load_key (key, base_key))
    return -1;

  if (ACE_TEXT_RegSetValueEx (base_key,
                              name,
                              0,
                              REG_BINARY,
                              (BYTE *) data,
                              length) != ERROR_SUCCESS)
    return -1;

  return 0;
}

int
ACE_Configuration_Win32Registry::get_string_value (const ACE_Configuration_Section_Key& key,
                                                   const ACE_TCHAR* name,
                                                   ACE_TString& value)
{
  if (validate_name (name))
    return -1;

  HKEY base_key;
  if (load_key (key, base_key))
    return -1;

  // Get the size of the binary data from windows
  DWORD buffer_length = 0;
  DWORD type;
  if (ACE_TEXT_RegQueryValueEx (base_key,
                                name,
                                0,
                                &type,
                                (BYTE *) 0,
                                &buffer_length) != ERROR_SUCCESS)
    return -1;

  if (type != REG_SZ)
    {
      errno = 0; ACE_OS::last_error (ERROR_INVALID_DATATYPE);
      return -1;
    }

  ACE_TCHAR *temp = 0;
  ACE_NEW_RETURN (temp,
                  ACE_TCHAR[buffer_length],
                  -1);

  ACE_Auto_Basic_Array_Ptr<ACE_TCHAR> buffer (temp);

  if (ACE_TEXT_RegQueryValueEx (base_key,
                                name,
                                0,
                                &type,
                                (BYTE *) buffer.get (),
                                &buffer_length) != ERROR_SUCCESS)
  {
    return -1;
  }

  value = buffer.get ();
  return 0;
}

int
ACE_Configuration_Win32Registry::get_integer_value (const ACE_Configuration_Section_Key& key,
                                                    const ACE_TCHAR* name,
                                                    u_int& value)
{
  if (validate_name (name))
    return -1;

  HKEY base_key;
  if (load_key (key, base_key))
    return -1;

  DWORD length = sizeof (value);
  DWORD type;
  if (ACE_TEXT_RegQueryValueEx (base_key,
                                name,
                                0,
                                &type,
                                (BYTE *) &value,
                                &length) != ERROR_SUCCESS)
    return -1;

  if (type != REG_DWORD)
    {
      errno = 0; ACE_OS::last_error (ERROR_INVALID_DATATYPE);
      return -1;
    }

  return 0;
}

int
ACE_Configuration_Win32Registry::get_binary_value (const ACE_Configuration_Section_Key &key,
                                                   const ACE_TCHAR *name,
                                                   void *&data,
                                                   u_int &length)
{
  if (validate_name (name))
    return -1;

  HKEY base_key;
  if (load_key (key, base_key))
    return -1;

  // Get the size of the binary data from windows
  DWORD buffer_length = 0;
  DWORD type;
  if (ACE_TEXT_RegQueryValueEx (base_key,
                                name,
                                0,
                                &type,
                                (BYTE *) 0,
                                &buffer_length) != ERROR_SUCCESS)
    return -1;

  if (type != REG_BINARY)
    {
      errno = 0; ACE_OS::last_error (ERROR_INVALID_DATATYPE);
      return -1;
    }

  length = buffer_length;

  ACE_NEW_RETURN (data, BYTE[length], -1);

  if (ACE_TEXT_RegQueryValueEx (base_key,
                                name,
                                0,
                                &type,
                                (BYTE *) data,
                                &buffer_length) != ERROR_SUCCESS)
    {
      delete [] (BYTE *) data;
      data = 0;
      return -1;
    }

  return 0;
}

int
ACE_Configuration_Win32Registry::find_value (const ACE_Configuration_Section_Key& key,
                                             const ACE_TCHAR* name,
                                             VALUETYPE& type_out)
{
  if (validate_name (name))
    return -1;

  HKEY base_key;
  if (load_key (key, base_key))
    return -1;

  DWORD buffer_length=0;
  DWORD type;
  int result=ACE_TEXT_RegQueryValueEx (base_key,
                                       name,
                                       0,
                                       &type,
                                       0,
                                       &buffer_length);
  if (result != ERROR_SUCCESS)
    return -1;

  switch (type)
    {
    case REG_SZ:
      type_out = STRING;
      break;
    case REG_DWORD:
      type_out = INTEGER;
      break;
    case REG_BINARY:
      type_out = BINARY;
      break;
    default:
      return -1; // unknown type
    }

  return 0;
}

int
ACE_Configuration_Win32Registry::remove_value (const ACE_Configuration_Section_Key& key,
                                               const ACE_TCHAR* name)
{
  if (validate_name (name))
    return -1;

  HKEY base_key;
  if (load_key (key, base_key))
    return -1;

  if (ACE_TEXT_RegDeleteValue (base_key, name) != ERROR_SUCCESS)
    return -1;

  return 0;
}


int
ACE_Configuration_Win32Registry::load_key (const ACE_Configuration_Section_Key& key,
                                           HKEY& hKey)
{
  ACE_Section_Key_Win32* pKey = ACE_dynamic_cast (ACE_Section_Key_Win32*,
                                                  get_internal_key (key));
  if (!pKey)
    return -1;

  hKey = pKey->hKey_;
  return 0;
}

HKEY
ACE_Configuration_Win32Registry::resolve_key (HKEY hKey,
                                              const ACE_TCHAR* path,
                                              int create)
{
  HKEY result = 0;
  // Make a copy of hKey
#if defined (ACE_HAS_WINCE)
  if (::RegOpenKeyEx (hKey, 0, 0, 0, &result) != ERROR_SUCCESS)
#else
  if (::RegOpenKey (hKey, 0, &result) != ERROR_SUCCESS)
#endif  // ACE_HAS_WINCE
    return 0;

  // recurse through the path
  ACE_TCHAR *temp_path = 0;
  ACE_NEW_RETURN (temp_path,
                  ACE_TCHAR[ACE_OS::strlen (path) + 1],
                  0);
  ACE_Auto_Basic_Array_Ptr<ACE_TCHAR> pData (temp_path);
  ACE_OS::strcpy (pData.get (), path);
  ACE_Tokenizer parser (pData.get ());
  parser.delimiter_replace ('\\', '\0');
  parser.delimiter_replace ('/', '\0');

  for (ACE_TCHAR *temp = parser.next ();
       temp != 0;
       temp = parser.next ())
    {
      // Open the key
      HKEY subkey;

#if defined (ACE_HAS_WINCE)
      if (ACE_TEXT_RegOpenKeyEx (result,
                                 temp,
                                 0,
                                 0,
                                 &subkey) != ERROR_SUCCESS)
#else
      if (ACE_TEXT_RegOpenKey (result,
                               temp,
                               &subkey) != ERROR_SUCCESS)
#endif  // ACE_HAS_WINCE
        {
          // try creating it
          if (!create || ACE_TEXT_RegCreateKeyEx (result,
                                                  temp,
                                                  0,
                                                  0,
                                                  0,
                                                  KEY_ALL_ACCESS,
                                                  0,
                                                  &subkey,
#if defined (__MINGW32__)
 (PDWORD) 0
#else
                                                  0
#endif /* __MINGW32__ */
                                                  ) != ERROR_SUCCESS)
            {
              // error
              ::RegCloseKey (result);
              return 0;
            }
        }
      // release our open key handle
      ::RegCloseKey (result);
      result = subkey;
    }

  return result;
}

#endif /* WIN_32 */

///////////////////////////////////////////////////////////////

ACE_Configuration_Value_IntId::ACE_Configuration_Value_IntId (void)
  : type_ (ACE_Configuration::INVALID),
    length_ (0)
{
  this->data_.ptr_ = 0;
}

ACE_Configuration_Value_IntId::ACE_Configuration_Value_IntId (ACE_TCHAR* string)
  : type_ (ACE_Configuration::STRING),
    length_ (0)
{
  this->data_.ptr_ = string;
}

ACE_Configuration_Value_IntId::ACE_Configuration_Value_IntId (u_int integer)
  : type_ (ACE_Configuration::INTEGER),
    length_ (0)
{
  this->data_.int_ = integer;
}

ACE_Configuration_Value_IntId::ACE_Configuration_Value_IntId (void* data, size_t length)
  : type_ (ACE_Configuration::BINARY),
    length_ (length)
{
  this->data_.ptr_ = data;
}

ACE_Configuration_Value_IntId::ACE_Configuration_Value_IntId (const ACE_Configuration_Value_IntId& rhs)
  : type_ (rhs.type_),
    data_ (rhs.data_),
    length_ (rhs.length_)
{
}

ACE_Configuration_Value_IntId::~ACE_Configuration_Value_IntId (void)
{
}

ACE_Configuration_Value_IntId& ACE_Configuration_Value_IntId::operator= (const ACE_Configuration_Value_IntId& rhs)
{
  if (this != &rhs)
    {
      type_ = rhs.type_;
      data_ = rhs.data_;
      length_ = rhs.length_;
    }
  return *this;
}

void
ACE_Configuration_Value_IntId::free (ACE_Allocator *alloc)
{
  if (this->type_ == ACE_Configuration::STRING
      || this->type_ == ACE_Configuration::BINARY)
    alloc->free (data_.ptr_);
  // Do nothing in other cases...
}

ACE_Configuration_ExtId::ACE_Configuration_ExtId (void)
  : name_ (0)
{
}

ACE_Configuration_ExtId::ACE_Configuration_ExtId (const ACE_TCHAR* name)
  : name_ (name)
{
}

ACE_Configuration_ExtId::ACE_Configuration_ExtId (const ACE_Configuration_ExtId& rhs)
  : name_ (rhs.name_)
{
}

ACE_Configuration_ExtId::~ACE_Configuration_ExtId (void)
{
}

ACE_Configuration_ExtId& ACE_Configuration_ExtId::operator= (const ACE_Configuration_ExtId& rhs)
{
  if (this != &rhs)
    name_ = rhs.name_;

  return *this;
}

int
ACE_Configuration_ExtId::operator == (const ACE_Configuration_ExtId& rhs) const
{
  return (ACE_OS::strcmp (name_, rhs.name_) == 0);
}

int
ACE_Configuration_ExtId::operator != (const ACE_Configuration_ExtId& rhs) const
{
  return (ACE_OS::strcmp (name_, rhs.name_) != 0);
}

u_long
ACE_Configuration_ExtId::hash (void) const
{
  ACE_TString temp (name_);
  return temp.hash ();
}

const ACE_TCHAR*
ACE_Configuration_ExtId::name (void)
{
  return name_;
}

void
ACE_Configuration_ExtId::free (ACE_Allocator *alloc)
{
  alloc->free ((void *) (name_));
}

///////////////////////////////////////////////////////////////////////

ACE_Configuration_Section_IntId::ACE_Configuration_Section_IntId (void)
  : value_hash_map_ (0),
    section_hash_map_ (0)
{
}

ACE_Configuration_Section_IntId::ACE_Configuration_Section_IntId (VALUE_MAP* value_hash_map, SUBSECTION_MAP* section_hash_map)
  : value_hash_map_ (value_hash_map),
    section_hash_map_ (section_hash_map)
{
}

ACE_Configuration_Section_IntId::ACE_Configuration_Section_IntId (const ACE_Configuration_Section_IntId& rhs)
  : value_hash_map_ (rhs.value_hash_map_),
    section_hash_map_ (rhs.section_hash_map_)
{

}

ACE_Configuration_Section_IntId::~ACE_Configuration_Section_IntId ()
{
}

ACE_Configuration_Section_IntId&
ACE_Configuration_Section_IntId::operator= (const ACE_Configuration_Section_IntId& rhs)
{
  if (this != &rhs)
    {
      value_hash_map_ = rhs.value_hash_map_;
      section_hash_map_ = rhs.section_hash_map_;
    }
  return *this;
}

void
ACE_Configuration_Section_IntId::free (ACE_Allocator *alloc)
{
  alloc->free ((void *) (value_hash_map_));
  alloc->free ((void *) (section_hash_map_));
}

ACE_Configuration_Section_Key_Heap::ACE_Configuration_Section_Key_Heap (const ACE_TCHAR* path)
  : path_ (0),
    value_iter_ (0),
    section_iter_ (0)
{
  path_ = ACE_OS::strdup (path);
}

ACE_Configuration_Section_Key_Heap::~ACE_Configuration_Section_Key_Heap ()
{
  delete value_iter_;
  delete section_iter_;
  ACE_OS::free (path_);
}

//////////////////////////////////////////////////////////////////////////////

ACE_Configuration_Heap::ACE_Configuration_Heap (void)
  : allocator_ (0),
    index_ (0),
    default_map_size_ (0)
{
  ACE_Configuration_Section_Key_Heap *temp = 0;

  ACE_NEW (temp, ACE_Configuration_Section_Key_Heap (ACE_LIB_TEXT ("")));
  root_ = ACE_Configuration_Section_Key (temp);
}

ACE_Configuration_Heap::~ACE_Configuration_Heap (void)
{
  if (allocator_)
    allocator_->sync ();

  delete allocator_;
}

int
ACE_Configuration_Heap::open (int default_map_size)
{
  default_map_size_ = default_map_size;
  // Create the allocator with the appropriate options.
  // The name used for  the lock is the same as one used
  // for the file.
  ACE_NEW_RETURN (this->allocator_,
                  HEAP_ALLOCATOR (),
                  -1);
  return create_index ();
}


int
ACE_Configuration_Heap::open (const ACE_TCHAR* file_name,
                              void* base_address,
                              int default_map_size)
{
  default_map_size_ = default_map_size;

  // Make sure that the file name is of the legal length.
  if (ACE_OS::strlen (file_name) >= MAXNAMELEN + MAXPATHLEN)
    {
      errno = ENAMETOOLONG;
      return -1;
    }

#if !defined (CHORUS)
  ACE_MMAP_Memory_Pool::OPTIONS options (base_address);
#else
  // Use base address == 0, don't use a fixed address.
  ACE_MMAP_Memory_Pool::OPTIONS options (0,
                                         0,
                                         0,
                                         ACE_CHORUS_LOCAL_NAME_SPACE_T_SIZE);
#endif /* CHORUS */

  // Create the allocator with the appropriate options.  The name used
  // for  the lock is the same as one used for the file.
  ACE_NEW_RETURN (this->allocator_,
                  PERSISTENT_ALLOCATOR (file_name,
                                        file_name,
                                        &options),
                  -1);

#if !defined (ACE_LACKS_ACCESS)
  // Now check if the backing store has been created successfully.
  if (ACE_OS::access (file_name, F_OK) != 0)
    ACE_ERROR_RETURN ((LM_ERROR,
                       ACE_LIB_TEXT ("create_index\n")),
                      -1);
#endif /* ACE_LACKS_ACCESS */

  return create_index ();
}

int
ACE_Configuration_Heap::create_index (void)
{
  void *section_index = 0;

  // This is the easy case since if we find hash table in the
  // memory-mapped file we know it's already initialized.
  if (this->allocator_->find (ACE_CONFIG_SECTION_INDEX, section_index) == 0)
    this->index_ = (SECTION_MAP *) section_index;

  // Create a new <index_> (because we've just created a new
  // memory-mapped file).
  else
    {
      size_t index_size = sizeof (SECTION_MAP);
      section_index = this->allocator_->malloc (index_size);

      if (section_index == 0
          || create_index_helper (section_index) == -1
          || this->allocator_->bind (ACE_CONFIG_SECTION_INDEX,
                                     section_index) == -1)
        {
          // Attempt to clean up.
          ACE_ERROR ((LM_ERROR,
                      ACE_LIB_TEXT ("create_index\n")));
          this->allocator_->remove ();
          return -1;
        }
      // Add the root section
      return new_section (ACE_LIB_TEXT (""), root_);
    }
  return 0;
}

int
ACE_Configuration_Heap::create_index_helper (void *buffer)
{
  ACE_ASSERT (this->allocator_);
  this->index_ = new (buffer) SECTION_MAP (this->allocator_);
  return 0;
}

int
ACE_Configuration_Heap::load_key (const ACE_Configuration_Section_Key& key,
                                  ACE_TString& name)
{
  ACE_ASSERT (this->allocator_);
  ACE_Configuration_Section_Key_Heap* pKey =
    ACE_dynamic_cast (ACE_Configuration_Section_Key_Heap*,
                      get_internal_key (key));
  if (!pKey)
    return -1;

  name = pKey->path_;
  return 0;
}


int
ACE_Configuration_Heap::add_section (const ACE_Configuration_Section_Key& base,
                                     const ACE_TCHAR* sub_section,
                                     ACE_Configuration_Section_Key& result)
{
  ACE_ASSERT (this->allocator_);
  ACE_TString section;
  if (load_key (base, section))
    return -1;

  // Find the base section
  ACE_Configuration_ExtId ExtId (section.fast_rep ());
  ACE_Configuration_Section_IntId IntId;
  if (index_->find (ExtId, IntId, allocator_))
    return -1;

  // See if this section already exists
  ACE_Configuration_ExtId SubSectionExtId (sub_section);
  int ignored = 0;

  if (!IntId.section_hash_map_->find (SubSectionExtId, ignored, allocator_))
    {
      // already exists!
      errno = EEXIST;
      return -1;
    }

  // Create the new section name
  // only prepend a separater if were not at the root
  if (section.length ())
    section += ACE_LIB_TEXT ("\\");

  section += sub_section;

  // Add it to the base section
  ACE_TCHAR* pers_name = (ACE_TCHAR *) allocator_->malloc ((ACE_OS::strlen (sub_section) + 1) * sizeof (ACE_TCHAR));
  ACE_OS::strcpy (pers_name, sub_section);
  ACE_Configuration_ExtId SSExtId (pers_name);
  if (IntId.section_hash_map_->bind (SSExtId, ignored, allocator_))
    {
      allocator_->free (pers_name);
      return -1;
    }
  return (new_section (section, result));
}

int
ACE_Configuration_Heap::new_section (const ACE_TString& section,
                                     ACE_Configuration_Section_Key& result)
{
  ACE_ASSERT (this->allocator_);
  // Create a new section and add it to the global list

  // Allocate memory for items to be stored in the table.
  size_t section_len = section.length () + 1;
  ACE_TCHAR *ptr = (ACE_TCHAR*) this->allocator_->malloc (section_len * sizeof (ACE_TCHAR));

  int return_value = -1;

  if (ptr == 0)
    return -1;
  else
    {
      // Populate memory with data.
      ACE_OS::strcpy (ptr, section.fast_rep ());

      void *value_hash_map = 0;
      size_t map_size = sizeof (VALUE_MAP);
      value_hash_map = this->allocator_->malloc (map_size);

      // If allocation failed ...
      if (value_hash_map == 0)
        return -1;

      // Initialize allocated hash map through placement new.
      if (value_open_helper (default_map_size_, value_hash_map ) == -1)
        {
          this->allocator_->free (value_hash_map );
          return -1;
        }

      // create the section map
      void* section_hash_map = 0;
      map_size = sizeof (SUBSECTION_MAP);
      section_hash_map = this->allocator_->malloc (map_size);

      // If allocation failed
      if (section_hash_map == 0)
        return -1;

      // initialize allocated hash map through placement new
      if (section_open_helper (default_map_size_, section_hash_map) == -1)
        {
          this->allocator_->free (value_hash_map );
          this->allocator_->free (section_hash_map);
          return -1;
        }

      ACE_Configuration_ExtId name (ptr);
      ACE_Configuration_Section_IntId entry ((VALUE_MAP*) value_hash_map ,
 (SUBSECTION_MAP*) section_hash_map);

      // Do a normal bind.  This will fail if there's already an
      // entry with the same name.
      return_value = this->index_->bind (name, entry, this->allocator_);

      if (return_value == 1)
        {
          // Entry already existed so bind failed. Free our dynamically
          // allocated memory.
          this->allocator_->free ((void *) ptr);
          return return_value;
        }

      if (return_value == -1)
        // Free our dynamically allocated memory.
        this->allocator_->free ((void *) ptr);
      else
        // If bind () succeed, it will automatically sync
        // up the map manager entry.  However, we must sync up our
        // name/value memory.
        this->allocator_->sync (ptr, section_len);

    }

  // set the result
  ACE_Configuration_Section_Key_Heap *temp;
  ACE_NEW_RETURN (temp,
                  ACE_Configuration_Section_Key_Heap (section.fast_rep ()),
                  -1);
  result = ACE_Configuration_Section_Key (temp);
  return return_value;
}

int
ACE_Configuration_Heap::value_open_helper (size_t hash_table_size,
                                          void *buffer)
{
  ACE_ASSERT (this->allocator_);
  new (buffer) VALUE_MAP (hash_table_size, this->allocator_);
  return 0;
}

int
ACE_Configuration_Heap::section_open_helper (size_t hash_table_size,
                                             void *buffer)
{
  ACE_ASSERT (this->allocator_);
  new (buffer) SUBSECTION_MAP (hash_table_size, this->allocator_);
  return 0;
}

int
ACE_Configuration_Heap::open_section (const ACE_Configuration_Section_Key& base,
                                      const ACE_TCHAR* sub_section,
                                      int create,
                                      ACE_Configuration_Section_Key& result)
{
  ACE_ASSERT (this->allocator_);
  if (validate_name (sub_section, 1))    // 1 == allow_path
    return -1;

  result = base;

  for (const ACE_TCHAR* separator;
       (separator = ACE_OS_String::strchr (sub_section, ACE_TEXT ('\\'))) != 0;
       )
    {
      ACE_TString simple_section (sub_section, separator - sub_section);
      int ret_val =
        open_simple_section (result, simple_section.c_str (), create, result);
      if (ret_val)
        return ret_val;
      sub_section = separator + 1;
    }

  return open_simple_section (result, sub_section, create, result);
}

int
ACE_Configuration_Heap::open_simple_section (const ACE_Configuration_Section_Key& base,
                                             const ACE_TCHAR* sub_section,
                                             int create,
                                             ACE_Configuration_Section_Key& result)
{
  ACE_TString section;
  if (load_key (base, section))
    return -1;

  // Only add the \\ if were not at the root
  if (section.length ())
    section += ACE_LIB_TEXT ("\\");

  section += sub_section;

  // resolve the section
  ACE_Configuration_ExtId ExtId (section.fast_rep ());
  ACE_Configuration_Section_IntId IntId;
  if (index_->find (ExtId, IntId, allocator_))
    {
      if (!create)
        {
          errno = ENOENT;
          return -1;
        }
      return add_section (base, sub_section, result);
    }

  ACE_Configuration_Section_Key_Heap *temp;

  ACE_NEW_RETURN (temp,
                  ACE_Configuration_Section_Key_Heap (section.fast_rep ()),
                  -1);
  result = ACE_Configuration_Section_Key (temp);

  return 0;
}

int
ACE_Configuration_Heap::remove_section (const ACE_Configuration_Section_Key& key,
                                        const ACE_TCHAR* sub_section,
                                        int recursive)
{
  ACE_ASSERT (this->allocator_);
  if (validate_name (sub_section))
    return -1;

  ACE_TString section;
  if (load_key (key, section))
    return -1;

  // Find this key
  ACE_Configuration_ExtId ParentExtId (section.fast_rep ());
  ACE_Configuration_Section_IntId ParentIntId;
  if (index_->find (ParentExtId, ParentIntId, allocator_))
    return -1;// no parent key

  // Find this subkey
  if (section.length ())
    section += ACE_LIB_TEXT ("\\");

  section += sub_section;
  ACE_Configuration_ExtId SectionExtId (section.fast_rep ());
  SECTION_ENTRY* section_entry;
  SECTION_HASH* hashmap = index_;
  if (hashmap->find (SectionExtId, section_entry))
    return -1;

  if (recursive)
    {
      ACE_Configuration_Section_Key section;
      if (open_section (key, sub_section, 0, section))
        return -1;

      int index = 0;
      ACE_TString name;
      while (!enumerate_sections (section, index, name))
        {
          if (remove_section (section, name.fast_rep (), 1))
            return -1;

          index++;
        }
    }

  // Now make sure we dont have any subkeys
  if (section_entry->int_id_.section_hash_map_->current_size ())
    {
      errno = ENOTEMPTY;
      return -1;
    }

  // Now remove subkey from parent key
  ACE_Configuration_ExtId SubSExtId (sub_section);
  SUBSECTION_ENTRY* subsection_entry;
  if (((SUBSECTION_HASH*)ParentIntId.section_hash_map_)->
      find (SubSExtId, subsection_entry))
    return -1;

  if (ParentIntId.section_hash_map_->unbind (SubSExtId, allocator_))
    return -1;

  subsection_entry->ext_id_.free (allocator_);

  // Remember the pointers so we can free them after we unbind
  ACE_Configuration_ExtId ExtIdToFree (section_entry->ext_id_);
  ACE_Configuration_Section_IntId IntIdToFree (section_entry->int_id_);

  // iterate over all values and free memory
  VALUE_HASH* value_hash_map = section_entry->int_id_.value_hash_map_;
  VALUE_HASH::ITERATOR value_iter = value_hash_map->begin ();
  while (!value_iter.done ())
    {
      VALUE_ENTRY* value_entry;
      if (!value_iter.next (value_entry))
        return 1;

      value_entry->ext_id_.free (allocator_);
      value_entry->int_id_.free (allocator_);

      value_iter.advance ();
    }

  // remove it
  if (index_->unbind (SectionExtId, allocator_))
    return -1;

  // Free the memory
  ExtIdToFree.free (allocator_);
  IntIdToFree.free (allocator_);

  return 0;
}

int
ACE_Configuration_Heap::enumerate_values (const ACE_Configuration_Section_Key& key,
                                          int index,
                                          ACE_TString& name,
                                          VALUETYPE& type)
{
  ACE_ASSERT (this->allocator_);
  ACE_Configuration_Section_Key_Heap* pKey =
    ACE_dynamic_cast (ACE_Configuration_Section_Key_Heap*,
                      get_internal_key (key));
  if (!pKey)
    return -1;

  name = pKey->path_;

  // resolve the section
  ACE_Configuration_ExtId ExtId (pKey->path_);
  ACE_Configuration_Section_IntId IntId;
  if (index_->find (ExtId, IntId, allocator_))
    return -1;

  // Handle iterator resets
  if (index == 0)
    {
      ACE_Hash_Map_Manager_Ex<ACE_Configuration_ExtId ,
                              ACE_Configuration_Value_IntId,
                              ACE_Hash<ACE_Configuration_ExtId>,
                              ACE_Equal_To<ACE_Configuration_ExtId>,
                              ACE_Null_Mutex>* hash_map = IntId.value_hash_map_;
      delete pKey->value_iter_;

      ACE_NEW_RETURN (pKey->value_iter_,
                      VALUE_HASH::ITERATOR (hash_map->begin ()),
                      -1);
    }

  // Get the next entry
  ACE_Hash_Map_Entry<ACE_Configuration_ExtId, ACE_Configuration_Value_IntId>* entry;

  if (!pKey->value_iter_->next (entry))
    return 1;

  // Return the value of the iterator and advance it
  name = entry->ext_id_.name_;
  type = entry->int_id_.type_;
  pKey->value_iter_->advance ();

  return 0;
}

int
ACE_Configuration_Heap::enumerate_sections (const ACE_Configuration_Section_Key& key,
                                            int index,
                                            ACE_TString& name)
{
  ACE_ASSERT (this->allocator_);
  // cast to a heap section key
  ACE_Configuration_Section_Key_Heap* pKey =
    ACE_dynamic_cast (ACE_Configuration_Section_Key_Heap*,
                      get_internal_key (key));
  if (!pKey)
    return -1;  // not a heap key!

  // resolve the section
  ACE_Configuration_ExtId ExtId (pKey->path_);
  ACE_Configuration_Section_IntId IntId;
  if (index_->find (ExtId, IntId, allocator_))
    return -1; // unknown section

  // Handle iterator resets
  if (index == 0)
    {
      if (pKey->section_iter_)
        delete pKey->section_iter_;

      ACE_NEW_RETURN (pKey->section_iter_,
                      SUBSECTION_HASH::ITERATOR (IntId.section_hash_map_->begin ()),
                      -1);
    }

  // Get the next entry
  ACE_Hash_Map_Entry<ACE_Configuration_ExtId, int>* entry;
  if (!pKey->section_iter_->next (entry))
    return 1;

  // Return the value of the iterator and advance it
  pKey->section_iter_->advance ();
  name = entry->ext_id_.name_;

  return 0;
}

int
ACE_Configuration_Heap::set_string_value (const ACE_Configuration_Section_Key& key,
                                          const ACE_TCHAR* name,
                                          const ACE_TString& value)
{
  ACE_ASSERT (this->allocator_);
  if (validate_name (name))
    return -1;

  ACE_TString section;
  if (load_key (key, section))
    return -1;

  ACE_Configuration_ExtId section_ext (section.fast_rep ());
  ACE_Configuration_Section_IntId section_int;
  if (index_->find (section_ext, section_int, allocator_))
    return -1;

  // Get the entry for this item (if it exists)
  VALUE_ENTRY* entry;
  ACE_Configuration_ExtId item_name (name);
  if (section_int.value_hash_map_->VALUE_HASH::find (item_name, entry) == 0)
    {
      // found item, replace it
      // Free the old value
      entry->int_id_.free (allocator_);
      // Allocate the new value in this heap
      ACE_TCHAR* pers_value =
 (ACE_TCHAR *) allocator_->malloc ((value.length () + 1) * sizeof (ACE_TCHAR));
      ACE_OS::strcpy (pers_value, value.fast_rep ());
      ACE_Configuration_Value_IntId new_value_int (pers_value);
      entry->int_id_ = new_value_int;
    }
  else
    {
      // it doesn't exist, bind it
      ACE_TCHAR* pers_name =
 (ACE_TCHAR *) allocator_->malloc ((ACE_OS::strlen (name) + 1) * sizeof (ACE_TCHAR));
      ACE_OS::strcpy (pers_name, name);
      ACE_TCHAR* pers_value =
 (ACE_TCHAR *) allocator_->malloc ((value.length () + 1) * sizeof (ACE_TCHAR));
      ACE_OS::strcpy (pers_value, value.fast_rep ());
      ACE_Configuration_ExtId item_name (pers_name);
      ACE_Configuration_Value_IntId item_value (pers_value);
      if (section_int.value_hash_map_->bind (item_name, item_value, allocator_))
        {
          allocator_->free (pers_value);
          allocator_->free (pers_name);
          return -1;
        }
      return 0;
    }

  return 0;
}

int
ACE_Configuration_Heap::set_integer_value (const ACE_Configuration_Section_Key& key,
                                           const ACE_TCHAR* name,
                                           u_int value)
{
  ACE_ASSERT (this->allocator_);
  if (validate_name (name))
    return -1;

  // Get the section name from the key
  ACE_TString section;
  if (load_key (key, section))
    return -1;

  // Find this section
  ACE_Configuration_ExtId section_ext (section.fast_rep ());
  ACE_Configuration_Section_IntId section_int;
  if (index_->find (section_ext, section_int, allocator_))
    return -1;  // section does not exist

  // Get the entry for this item (if it exists)
  VALUE_ENTRY* entry;
  ACE_Configuration_ExtId item_name (name);
  if (section_int.value_hash_map_->VALUE_HASH::find (item_name, entry) == 0)
    {
      // found item, replace it
      ACE_Configuration_Value_IntId new_value_int (value);
      entry->int_id_ = new_value_int;
    }
  else
    {
      // it doesn't exist, bind it
      ACE_TCHAR* pers_name =
 (ACE_TCHAR *) allocator_->malloc ((ACE_OS::strlen (name) + 1) * sizeof (ACE_TCHAR));
      ACE_OS::strcpy (pers_name, name);
      ACE_Configuration_ExtId item_name (pers_name);
      ACE_Configuration_Value_IntId item_value (value);
      if (section_int.value_hash_map_->bind (item_name, item_value, allocator_))
        {
          allocator_->free (pers_name);
          return -1;
        }
      return 0;
    }

  return 0;
}

int
ACE_Configuration_Heap::set_binary_value (const ACE_Configuration_Section_Key& key,
                                          const ACE_TCHAR* name,
                                          const void* data,
                                          u_int length)
{
  ACE_ASSERT (this->allocator_);
  if (validate_name (name))
    return -1;

  // Get the section name from the key
  ACE_TString section;
  if (load_key (key, section))
    return -1;

  // Find this section
  ACE_Configuration_ExtId section_ext (section.fast_rep ());
  ACE_Configuration_Section_IntId section_int;
  if (index_->find (section_ext, section_int, allocator_))
    return -1;    // section does not exist

  // Get the entry for this item (if it exists)
  VALUE_ENTRY* entry;
  ACE_Configuration_ExtId item_name (name);
  if (section_int.value_hash_map_->VALUE_HASH::find (item_name, entry) == 0)
    {
      // found item, replace it
      // Free the old value
      entry->int_id_.free (allocator_);
      // Allocate the new value in this heap
      ACE_TCHAR* pers_value = (ACE_TCHAR *) allocator_->malloc (length);
      ACE_OS::memcpy (pers_value, data, length);
      ACE_Configuration_Value_IntId new_value_int (pers_value, length);
      entry->int_id_ = new_value_int;
    }
  else
    {
      // it doesn't exist, bind it
      ACE_TCHAR* pers_name =
 (ACE_TCHAR *) allocator_->malloc ((ACE_OS::strlen (name) + 1) * sizeof (ACE_TCHAR));
      ACE_OS::strcpy (pers_name, name);
      ACE_TCHAR* pers_value = (ACE_TCHAR *) allocator_->malloc (length);
      ACE_OS::memcpy (pers_value, data, length);
      ACE_Configuration_ExtId item_name (pers_name);
      ACE_Configuration_Value_IntId item_value (pers_value, length);
      if (section_int.value_hash_map_->bind (item_name, item_value, allocator_))
        {
          allocator_->free (pers_value);
          allocator_->free (pers_name);
          return -1;
        }
      return 0;
    }

/*
  // Find this section
  ACE_Configuration_ExtId ExtId (section.fast_rep ());
  ACE_Configuration_Section_IntId IntId;
  if (index_->find (ExtId, IntId, allocator_))
    return -1;    // section does not exist

  // See if the value exists first
  ACE_Configuration_ExtId VExtIdFind (name);
  ACE_Configuration_Value_IntId VIntIdFind;
  if (IntId.value_hash_map_->find (VExtIdFind, VIntIdFind, allocator_))
    {
      // it doesn't exist, bind it
      ACE_TCHAR* pers_name =
 (ACE_TCHAR *) allocator_->malloc ((ACE_OS::strlen (name) + 1) * sizeof (ACE_TCHAR));
      ACE_OS::strcpy (pers_name, name);
      ACE_TCHAR* pers_value =
 (ACE_TCHAR *) allocator_->malloc (length);
      ACE_OS::memcpy (pers_value, data, length);
      ACE_Configuration_ExtId VExtId (pers_name);
      ACE_Configuration_Value_IntId VIntId (pers_value, length);
      if (IntId.value_hash_map_->bind (VExtId, VIntId, allocator_))
        {
          allocator_->free (pers_value);
          allocator_->free (pers_name);
          return -1;
        }
      return 0;
    }
  else
    {
      // it does exist, free the old value memory
      VIntIdFind.free (allocator_);
      // Assign a new value
      ACE_TCHAR* pers_value = (ACE_TCHAR *) allocator_->malloc (length);
      ACE_OS::memcpy (pers_value, data, length);
      VIntIdFind = ACE_Configuration_Value_IntId (pers_value, length);
    }
*/
  return 0;
}

int
ACE_Configuration_Heap::get_string_value (const ACE_Configuration_Section_Key& key,
                                          const ACE_TCHAR* name,
                                          ACE_TString& value)
{
  ACE_ASSERT (this->allocator_);
  if (validate_name (name))
    return -1;

  // Get the section name from the key
  ACE_TString section;
  if (load_key (key, section))
    return -1;

  // Find this section
  ACE_Configuration_ExtId ExtId (section.fast_rep ());
  ACE_Configuration_Section_IntId IntId;
  if (index_->find (ExtId, IntId, allocator_))
    return -1;    // section does not exist

  // See if it exists first
  ACE_Configuration_ExtId VExtId (name);
  ACE_Configuration_Value_IntId VIntId;
  if (IntId.value_hash_map_->find (VExtId, VIntId, allocator_))
    return -1;    // unknown value

  // Check type
  if (VIntId.type_ != ACE_Configuration::STRING)
    {
      errno = ENOENT;
      return -1;
    }

  // everythings ok, return the data
  value = ACE_static_cast (ACE_TCHAR*, VIntId.data_.ptr_);
  return 0;
}

int
ACE_Configuration_Heap::get_integer_value (const ACE_Configuration_Section_Key& key,
                                           const ACE_TCHAR* name,
                                           u_int& value)
{
  ACE_ASSERT (this->allocator_);
  if (validate_name (name))
    return -1;

  // Get the section name from the key
  ACE_TString section;
  if (load_key (key, section))
    return -1;

  // Find this section
  ACE_Configuration_ExtId ExtId (section.fast_rep ());
  ACE_Configuration_Section_IntId IntId;
  if (index_->find (ExtId, IntId, allocator_))
    return -1;    // section does not exist


  // See if it exists first
  ACE_Configuration_ExtId VExtId (name);
  ACE_Configuration_Value_IntId VIntId;
  if (IntId.value_hash_map_->find (VExtId, VIntId, allocator_))
    return -1;    // unknown value

  // Check type
  if (VIntId.type_ != ACE_Configuration::INTEGER)
    {
      errno = ENOENT;
      return -1;
    }

  // Everythings ok, return the data
  value = VIntId.data_.int_;
  return 0;
}

int
ACE_Configuration_Heap::get_binary_value (const ACE_Configuration_Section_Key& key,
                                          const ACE_TCHAR* name,
                                          void*& data,
                                          u_int& length)
{
  ACE_ASSERT (this->allocator_);
  if (validate_name (name))
    return -1;

  // Get the section name from the key
  ACE_TString section;
  if (load_key (key, section))
    return -1;

  // Find this section
  ACE_Configuration_ExtId ExtId (section.fast_rep ());
  ACE_Configuration_Section_IntId IntId;
  if (index_->find (ExtId, IntId, allocator_))
    return -1;    // section does not exist

  ACE_Configuration_ExtId VExtId (name);
  ACE_Configuration_Value_IntId VIntId;
  // See if it exists first
  if (IntId.value_hash_map_->find (VExtId, VIntId, allocator_))
    return -1;    // unknown value

  // Check type
  if (VIntId.type_ != ACE_Configuration::BINARY)
    {
      errno = ENOENT;
      return -1;
    }

  // Make a copy
  ACE_NEW_RETURN (data, char[VIntId.length_], -1);
  ACE_OS::memcpy (data, VIntId.data_.ptr_, VIntId.length_);
  length = VIntId.length_;
  return 0;
}

int
ACE_Configuration_Heap::find_value (const ACE_Configuration_Section_Key& key,
                                    const ACE_TCHAR* name,
                                    VALUETYPE& type_out)
{
  ACE_ASSERT (this->allocator_);
  if (validate_name (name))
    return -1;

  // Get the section name from the key
  ACE_TString section;
  if (load_key (key, section))
    return -1;

  // Find this section
  ACE_Configuration_ExtId ExtId (section.fast_rep ());
  ACE_Configuration_Section_IntId IntId;
  if (index_->find (ExtId, IntId, allocator_))
    return -1;    // section does not exist

  // Find it
  ACE_Configuration_ExtId ValueExtId (name);
  VALUE_ENTRY* value_entry;
  if (((VALUE_HASH *) IntId.value_hash_map_)->find (ValueExtId, value_entry))
    return -1;  // value does not exist

  type_out = value_entry->int_id_.type_;
  return 0;
}

int
ACE_Configuration_Heap::remove_value (const ACE_Configuration_Section_Key& key,
                                      const ACE_TCHAR* name)
{
  ACE_ASSERT (this->allocator_);
  if (validate_name (name))
    return -1;

  // Get the section name from the key
  ACE_TString section;
  if (load_key (key, section))
    return -1;

  // Find this section
  ACE_Configuration_ExtId ExtId (section.fast_rep ());
  ACE_Configuration_Section_IntId IntId;
  if (index_->find (ExtId, IntId, allocator_))
    return -1;    // section does not exist

  // Find it
  ACE_Configuration_ExtId ValueExtId (name);
  VALUE_ENTRY* value_entry;
  if (((VALUE_HASH *) IntId.value_hash_map_)->find (ValueExtId, value_entry))
    return -1;

  // free it
  value_entry->ext_id_.free (allocator_);
  value_entry->int_id_.free (allocator_);

  // Unbind it
  if (IntId.value_hash_map_->unbind (ValueExtId, allocator_))
    return -1;

  return 0;
}
