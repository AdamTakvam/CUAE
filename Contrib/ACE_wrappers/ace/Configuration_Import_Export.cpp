// Configuration_Import_Export.cpp,v 4.22 2002/06/14 01:31:03 shuston Exp

#include "ace/Configuration_Import_Export.h"

ACE_Config_ImpExp_Base::ACE_Config_ImpExp_Base (ACE_Configuration& config)
  : config_ (config)
{
}

ACE_Config_ImpExp_Base::~ACE_Config_ImpExp_Base (void)
{
}

ACE_Registry_ImpExp::ACE_Registry_ImpExp (ACE_Configuration& config)
    : ACE_Config_ImpExp_Base (config)
{
}

ACE_Registry_ImpExp::~ACE_Registry_ImpExp (void)
{
}

// Imports the configuration database from filename.
// No existing data is removed.
int
ACE_Registry_ImpExp::import_config (const ACE_TCHAR* filename)
{
  if (0 == filename)
    {
      errno = EINVAL;
      return -1;
    }
  FILE* in = ACE_OS::fopen (filename, ACE_LIB_TEXT ("r"));
  if (!in)
    return -1;

  // @@ XXX - change this to a dynamic buffer
  ACE_TCHAR buffer[4096];
  ACE_Configuration_Section_Key section;
  while (ACE_OS::fgets (buffer, 4096, in))
    {
      // Check for a comment
      if (buffer[0] == ACE_LIB_TEXT (';') || buffer[0] == ACE_LIB_TEXT ('#'))
        continue;

      if (buffer[0] == ACE_LIB_TEXT ('['))
        {
          // We have a new section here, strip out the section name
          ACE_TCHAR* end = ACE_OS::strrchr (buffer, ACE_LIB_TEXT (']'));
          if (!end)
            {
              ACE_OS::fclose (in);
              return -3;
            }
          *end = 0;

          if (config_.expand_path (config_.root_section (), buffer + 1, section, 1))
            {
              ACE_OS::fclose (in);
              return -3;
            }
          continue;
        }              // end if firs char is a [

      if (buffer[0] == ACE_LIB_TEXT ('"'))
        {
          // we have a value
          ACE_TCHAR* end = ACE_OS::strchr (buffer+1, '"');
          if (!end)  // no closing quote, not a value so just skip it
            continue;

          // null terminate the name
          *end = 0;
          ACE_TCHAR* name = buffer + 1;
          end+=2;
          // determine the type
          if (*end == '\"')
            {
              // string type
              // truncate trailing "
              ++end;
              ACE_TCHAR* trailing = ACE_OS::strrchr (end, '"');
              if (trailing)
                *trailing = 0;
              if (config_.set_string_value (section, name, end))
                {
                  ACE_OS::fclose (in);
                  return -4;
                }
            }
          else if (ACE_OS::strncmp (end, ACE_LIB_TEXT ("dword:"), 6) == 0)
            {
              // number type
              ACE_TCHAR* endptr = 0;
              u_int value = ACE_OS::strtoul (end + 6, &endptr, 16);
              if (config_.set_integer_value (section, name, value))
                {
                  ACE_OS::fclose (in);
                  return -4;
                }
            }
          else if (ACE_OS::strncmp (end, ACE_LIB_TEXT ("hex:"), 4) == 0)
            {
              // binary type
              u_int string_length = ACE_OS::strlen (end + 4);
              // divide by 3 to get the actual buffer length
              u_int length = string_length / 3;
              u_int remaining = length;
              u_char* data = 0;
              ACE_NEW_RETURN (data,
                              u_char[length],
                              -1);
              u_char* out = data;
              ACE_TCHAR* inb = end + 4;
              ACE_TCHAR* endptr = 0;
              while (remaining)
                {
                  u_char charin = (u_char) ACE_OS::strtoul (inb, &endptr, 16);
                  *out = charin;
                  ++out;
                  --remaining;
                  inb += 3;
                }
              if (config_.set_binary_value (section, name, data, length))
                {
                  ACE_OS::fclose (in);
                  return -4;
                }
            }
          else
            {
              // invalid type, ignore
              continue;
            }
        }// end if first char is a "
      else
        {
          // if the first character is not a ", [, ;, or # we may be
          // processing a file in the old format.
          // Try and process the line as such and if it fails,
          // return an error
          int rc;
          if ((rc = process_previous_line_format (buffer, section)) != 0)
            {
              ACE_OS::fclose (in);
              return rc;
            }
        }             // end if maybe old format
    }                 // end while fgets

  if (ferror (in))
    {
      ACE_OS::fclose (in);
      return -1;
    }

  ACE_OS::fclose (in);
  return 0;
}

// This method exports the entire configuration database to <filename>.
// Once the file is opened this method calls 'export_section' passing
// the root section.
int
ACE_Registry_ImpExp::export_config (const ACE_TCHAR* filename)
{
  if (0 == filename)
    {
      errno = EINVAL;
      return -1;
    }
  int result = -1;

  FILE* out = ACE_OS::fopen (filename, ACE_LIB_TEXT ("w"));
  if (out)
    {
      result = this->export_section (config_.root_section (),
                                     ACE_LIB_TEXT (""),
                                     out);
      ACE_OS::fclose (out);
    }
  return result;
}

// Method provided by derived classes in order to write one section
// to the file specified.  Called by export_config when exporting
// the entire configuration object.

int
ACE_Registry_ImpExp::export_section (const ACE_Configuration_Section_Key& section,
                                     const ACE_TString& path,
                                     FILE* out)
{
  // don't export the root
  if (path.length ())
    {
      // Write out the section header
      ACE_TString header = ACE_LIB_TEXT ("[");
      header += path;
      header += ACE_LIB_TEXT ("]");
      header += ACE_LIB_TEXT (" \n");
      if (ACE_OS::fputs (header.fast_rep (), out) < 0)
        return -1;
      // Write out each value
      int index = 0;
      ACE_TString name;
      ACE_Configuration::VALUETYPE type;
      ACE_TString line;
      ACE_TCHAR int_value[32];
      ACE_TCHAR bin_value[3];
      void* binary_data;
      u_int binary_length;
      ACE_TString string_value;
      while (!config_.enumerate_values (section, index, name, type))
        {
          line = ACE_LIB_TEXT ("\"") + name + ACE_LIB_TEXT ("\"=");
          switch (type)
            {
            case ACE_Configuration::INTEGER:
              {
                u_int value;
                if (config_.get_integer_value (section, name.fast_rep (), value))
                  return -2;
                ACE_OS::sprintf (int_value, ACE_LIB_TEXT ("%08x"), value);
                line += ACE_LIB_TEXT ("dword:");
                line += int_value;
                break;
              }
            case ACE_Configuration::STRING:
              {
                if (config_.get_string_value (section,
                                              name.fast_rep (),
                                              string_value))
                  return -2;
                line += ACE_LIB_TEXT ("\"");
                line += string_value + ACE_LIB_TEXT ("\"");
                break;
              }
#ifdef _WIN32
            case ACE_Configuration::INVALID:
              break;  // JDO added break.  Otherwise INVALID is processed
              // like BINARY. If that's correct, please remove the
              // break and these comments
#endif
            case ACE_Configuration::BINARY:
              {
                // not supported yet - maybe use BASE64 codeing?
                if (config_.get_binary_value (section,
                                              name.fast_rep (),
                                              binary_data,
                                              binary_length))
                  return -2;
                line += ACE_LIB_TEXT ("hex:");
                unsigned char* ptr = (unsigned char*)binary_data;
                while (binary_length)
                  {
                    if (ptr != binary_data)
                      {
                        line += ACE_LIB_TEXT (",");
                      }
                    ACE_OS::sprintf (bin_value, ACE_LIB_TEXT ("%02x"), *ptr);
                    line += bin_value;
                    --binary_length;
                    ++ptr;
                  }
                delete [] (char*) binary_data;
                break;
              }
            default:
              return -3;
            }
          line += ACE_LIB_TEXT ("\n");
          if (ACE_OS::fputs (line.fast_rep (), out) < 0)
            return -4;
          index++;
        }
    }
  // Export all sub sections
  int index = 0;
  ACE_TString name;
  ACE_Configuration_Section_Key sub_key;
  ACE_TString sub_section;
  while (!config_.enumerate_sections (section, index, name))
    {
      ACE_TString sub_section (path);
      if (path.length ())
        sub_section += ACE_LIB_TEXT ("\\");
      sub_section += name;
      if (config_.open_section (section, name.fast_rep (), 0, sub_key))
        return -5;
      if (export_section (sub_key, sub_section.fast_rep (), out))
        return -6;
      index++;
    }
  return 0;
}

//
// This method read the line format origionally used in ACE 5.1
//
int
ACE_Registry_ImpExp::process_previous_line_format (ACE_TCHAR* buffer,
                                                   ACE_Configuration_Section_Key& section)
{
  // Chop any cr/lf at the end of the line.
  ACE_TCHAR *endp = ACE_OS_String::strpbrk (buffer, ACE_LIB_TEXT ("\r\n"));
  if (endp != 0)
    *endp = '\0';

  // assume this is a value, read in the value name
  ACE_TCHAR* end = ACE_OS::strchr (buffer, '=');
  if (end)  // no =, not a value so just skip it
    {
      // null terminate the name
      *end = 0;
      end++;
      // determine the type
      if (*end == '\"')
        {
          // string type
          if(config_.set_string_value (section, buffer, end + 1))
            return -4;
        }
      else if (*end == '#')
        {
          // number type
          u_int value = ACE_OS::atoi (end + 1);
          if (config_.set_integer_value (section, buffer, value))
            return -4;
        }
    }
  return 0;
}                // end read_previous_line_format


ACE_Ini_ImpExp::ACE_Ini_ImpExp (ACE_Configuration& config)
    : ACE_Config_ImpExp_Base (config)
{
}

ACE_Ini_ImpExp::~ACE_Ini_ImpExp (void)
{
}

// Method to read file and populate object.
int
ACE_Ini_ImpExp::import_config (const ACE_TCHAR* filename)
{
  if (0 == filename)
    {
      errno = EINVAL;
      return -1;
    }
  FILE* in = ACE_OS::fopen (filename, ACE_LIB_TEXT ("r"));
  if (!in)
    return -1;

  // @@ Make this a dynamic size!
  ACE_TCHAR buffer[4096];
  ACE_Configuration_Section_Key section;
  while (ACE_OS::fgets (buffer, sizeof buffer, in))
    {
      ACE_TCHAR *line = this->squish (buffer);
      // Check for a comment and blank line
      if (line[0] == ACE_LIB_TEXT (';')  || 
          line[0] == ACE_LIB_TEXT ('#')  || 
          line[0] == '\0')
        continue;

      if (line[0] == ACE_LIB_TEXT ('['))
        {
          // We have a new section here, strip out the section name
          ACE_TCHAR* end = ACE_OS::strrchr (line, ACE_LIB_TEXT (']'));
          if (!end)
            {
              ACE_OS::fclose (in);
              return -3;
            }
          *end = 0;

          if (config_.expand_path (config_.root_section (),
                                   line + 1,
                                   section,
                                   1))
            {
              ACE_OS::fclose (in);
              return -3;
            }

          continue;
        }

      // We have a line; name ends at equal sign.
      ACE_TCHAR *end = ACE_OS::strchr (line, ACE_LIB_TEXT ('='));
      if (end == 0)                            // No '='
        {
          ACE_OS::fclose (in);
          return -3;
        }
      *end++ = '\0';
      ACE_TCHAR *name = this->squish (line);
      if (ACE_OS::strlen (name) == 0)          // No name; just an '='
        {
          ACE_OS::fclose (in);
          return -3;
        }

      // Now find the start of the value
      ACE_TCHAR *value = this->squish (end);
      size_t value_len = ACE_OS::strlen (value);
      if (value_len > 0)
        {
          // ACE 5.2 (and maybe earlier) exported strings may be enclosed
          // in quotes. If string is quote-delimited, strip the quotes.
          // Newer exported files don't have quote delimiters.
          if (value[0] == ACE_LIB_TEXT ('"') &&
              value[value_len - 1] == ACE_LIB_TEXT ('"'))
            {
              // Strip quotes off both ends.
              value[value_len - 1] = '\0';
              value++;
            }
        }

      if (config_.set_string_value (section, name, value))
        {
          ACE_OS::fclose (in);
          return -4;
        }
    }             // end while fgets

  if (ferror (in))
    {
      ACE_OS::fclose (in);
      return -1;
    }

  ACE_OS::fclose (in);
  return 0;
}

// This method exports the entire configuration database to <filename>.
// Once the file is opened this method calls 'export_section' passing
// the root section.
int
ACE_Ini_ImpExp::export_config (const ACE_TCHAR* filename)
{
  if (0 == filename)
    {
      errno = EINVAL;
      return -1;
    }
  int result = -1;

  FILE* out = ACE_OS::fopen (filename, ACE_LIB_TEXT ("w"));
  if (out)
    {
      result = this->export_section (config_.root_section (),
                                     ACE_LIB_TEXT (""),
                                     out);
      ACE_OS::fclose (out);
    }
  return result;
}

// Method provided by derived classes in order to write one section to the
// file specified.  Called by export_config when exporting the entire
// configuration objet

int
ACE_Ini_ImpExp::export_section (const ACE_Configuration_Section_Key& section,
                                const ACE_TString& path,
                                FILE* out)
{
  // don't export the root
  if (path.length ())
    {
      // Write out the section header
      ACE_TString header = ACE_LIB_TEXT ("[");
      header += path;
      header += ACE_LIB_TEXT ("]\n");
      if (ACE_OS::fputs (header.fast_rep (), out) < 0)
        return -1;
      // Write out each value
      int index = 0;
      ACE_TString name;
      ACE_Configuration::VALUETYPE type;
      ACE_TString line;
      ACE_TCHAR int_value[32];
      ACE_TCHAR bin_value[3];
      void* binary_data;
      u_int binary_length;
      ACE_TString string_value;
      while (!config_.enumerate_values (section, index, name, type))
        {
          line = name + ACE_LIB_TEXT ("=");
          switch (type)
            {
            case ACE_Configuration::INTEGER:
              {
                u_int value;
                if (config_.get_integer_value (section, name.fast_rep (), value))
                  return -2;
                ACE_OS::sprintf (int_value, ACE_LIB_TEXT ("%08x"), value);
                line += int_value;
                break;
              }
            case ACE_Configuration::STRING:
              {
                if (config_.get_string_value (section,
                                              name.fast_rep (),
                                              string_value))
                  return -2;
                line += string_value;
                break;
              }
#ifdef _WIN32
            case ACE_Configuration::INVALID:
              break;  // JDO added break.  Otherwise INVALID is processed
              // like BINARY. If that's correct, please remove the
              // break and these comments
#endif
            case ACE_Configuration::BINARY:
              {
                // not supported yet - maybe use BASE64 codeing?
                if (config_.get_binary_value (section,
                                              name.fast_rep (),
                                              binary_data,
                                              binary_length))
                  return -2;
                line += ACE_LIB_TEXT ("\"");
                unsigned char* ptr = (unsigned char*)binary_data;
                while (binary_length)
                  {
                    if (ptr != binary_data)
                      {
                        line += ACE_LIB_TEXT (",");
                      }
                    ACE_OS::sprintf (bin_value, ACE_LIB_TEXT ("%02x"), *ptr);
                    line += bin_value;
                    --binary_length;
                    ++ptr;
                  }
                line += ACE_LIB_TEXT ("\"");
                delete [] (char *) binary_data;
                break;
              }
            default:
              return -3;

            }// end switch on type

          line += ACE_LIB_TEXT ("\n");
          if (ACE_OS::fputs (line.fast_rep (), out) < 0)
            return -4;
          index++;
        }// end while enumerating values
    }
  // Export all sub sections
  int index = 0;
  ACE_TString name;
  ACE_Configuration_Section_Key sub_key;
  ACE_TString sub_section;
  while (!config_.enumerate_sections (section, index, name))
    {
      ACE_TString sub_section (path);
      if (path.length ())
        sub_section += ACE_LIB_TEXT ("\\");
      sub_section += name;
      if (config_.open_section (section, name.fast_rep (), 0, sub_key))
        return -5;
      if (export_section (sub_key, sub_section.fast_rep (), out))
        return -6;
      index++;
    }
  return 0;

}

// Method to squish leading and trailing whitespaces from a string.
// Whitespace is defined as: spaces (' '), tabs ('\t') or end-of-line
// (cr/lf).  The terminating nul is moved up to expunge trailing
// whitespace and the returned pointer points at the first
// non-whitespace character in the string, which may be the nul
// terminator if the string is all whitespace.

ACE_TCHAR *
ACE_Ini_ImpExp::squish (ACE_TCHAR *src)
{
  ACE_TCHAR *cp;

  if (src == 0)
    return 0;

  // Start at the end and work backwards over all whitespace.
  for (cp = src + ACE_OS_String::strlen (src) - 1;
       cp != src;
       --cp)
    if (!ACE_OS_String::ace_isspace (*cp))
      break;
  cp[1] = '\0';          // Chop trailing whitespace

  // Now start at the beginning and move over all whitespace.
  for (cp = src; ACE_OS_String::ace_isspace (*cp); ++cp)
    continue;

  return cp;
}
