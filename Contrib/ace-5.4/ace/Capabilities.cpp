#include "ace/Capabilities.h"
#include "ace/os_include/os_ctype.h"
#include "ace/OS_Memory.h"
#include "ace/OS_NS_string.h"

#if !defined (__ACE_INLINE__)
#include "ace/Capabilities.i"
#endif /* !__ACE_INLINE__ */

#include "ace/OS_NS_stdio.h"

ACE_RCSID (ace,
           Capabilities,
           "Capabilities.cpp,v 4.15 2003/11/05 23:30:46 shuston Exp")


#define ACE_ESC ((ACE_TCHAR)0x1b)


ACE_CapEntry::~ACE_CapEntry (void)
{
}

ACE_Capabilities::ACE_Capabilities (void)
  : caps_ ()
{
}

ACE_Capabilities::~ACE_Capabilities (void)
{
  this->resetcaps ();
}

const ACE_TCHAR *
ACE_Capabilities::parse (const ACE_TCHAR *buf, ACE_TString &cap)
{
  while (*buf != ACE_LIB_TEXT ('\0') && *buf != ACE_LIB_TEXT (','))
    {
      if (*buf == ACE_LIB_TEXT ('\\'))
        {
          buf++;
          if (*buf == ACE_LIB_TEXT ('E') || *buf == ACE_LIB_TEXT ('e'))
            {
              cap += ACE_ESC;
              buf++;
              continue;
            }
          else if (*buf == ACE_LIB_TEXT ('r'))
            {
              cap += ACE_LIB_TEXT ('\r');
              buf++;
              continue;
            }
          else if (*buf == ACE_LIB_TEXT ('n'))
            {
              cap += ACE_LIB_TEXT ('\n');
              buf++;
              continue;
            }
          else if (*buf == ACE_LIB_TEXT ('t'))
            {
              cap += ACE_LIB_TEXT ('\t');
              buf++;
              continue;
            }
          else if (*buf == ACE_LIB_TEXT ('\\'))
            {
              cap += *buf++;
              continue;
            }
          if (isdigit(*buf))
            {
              // @@ UNICODE Does this work with unicode?
              int oc = 0;
              for (int i = 0;
                   i < 3 && *buf && isdigit (*buf);
                   i++)
                oc = oc * 8 + (*buf++ - ACE_LIB_TEXT ('0'));

              cap += (ACE_TCHAR) oc;
              continue;
            }
        }
      cap += *buf++;
    }
  return buf;
}

const ACE_TCHAR *
ACE_Capabilities::parse (const ACE_TCHAR *buf, int &cap)
{
  int n = 0;

  while (*buf && isdigit (*buf))
    n = n * 10 + (*buf++ - ACE_LIB_TEXT ('0'));

  cap = n;

  return buf;
}

void
ACE_Capabilities::resetcaps (void)
{
  for (MAP::ITERATOR iter (this->caps_);
       !iter.done ();
       iter.advance ())
    {
      MAP::ENTRY *entry;
      iter.next (entry);
      delete entry->int_id_;
    }

  this->caps_.close ();
  this->caps_.open ();
}

int
ACE_Capabilities::fillent (const ACE_TCHAR *buf)
{
  this->resetcaps ();
  while (*buf)
    {
      ACE_TString s;
      int n;
      ACE_TString name;
      ACE_CapEntry *ce;

      // Skip blanks
      while (*buf && isspace(*buf)) buf++;
      // If we get end of line return

      if (*buf == ACE_LIB_TEXT ('\0'))
        break;

      if (*buf == ACE_LIB_TEXT ('#'))
        {
          while (*buf && *buf != ACE_LIB_TEXT ('\n'))
            buf++;
          if (*buf == ACE_LIB_TEXT ('\n'))
            buf++;
          continue;
        }
      while(*buf && *buf != ACE_LIB_TEXT ('=')
            && *buf!= ACE_LIB_TEXT ('#')
            && *buf != ACE_LIB_TEXT (','))
        name += *buf++;

      // If name is null.
      switch (*buf)
        {
        case ACE_LIB_TEXT ('='):
          // String property
          buf = this->parse (buf + 1, s);
          ACE_NEW_RETURN (ce,
                          ACE_StringCapEntry (s),
                          -1);
          if (this->caps_.bind (name, ce) == -1)
            {
              delete ce;
              return -1;
            }
          break;
        case ACE_LIB_TEXT ('#'):
          // Integer property
          buf = this->parse (buf + 1, n);
          ACE_NEW_RETURN (ce,
                          ACE_IntCapEntry (n),
                          -1);
          if (this->caps_.bind (name, ce) == -1)
            {
              delete ce;
              return -1;
            }
          break;
        case ACE_LIB_TEXT (','):
          // Boolean
          ACE_NEW_RETURN (ce,
                          ACE_BoolCapEntry (1),
                          -1);
          if (this->caps_.bind (name, ce) == -1)
            {
              delete ce;
              return -1;
            }
          break;
        default:
          return 0;
        }

      if (*buf++ != ACE_LIB_TEXT (','))
        return -1;
    }

  return 0;
}

int
ACE_Capabilities::is_entry (const ACE_TCHAR *name, const ACE_TCHAR *line)
{
  for (;;)
    {
      // Skip blanks or irrelevant characters
      while (*line && isspace(*line))
        line++;

      // End of line reached
      if (*line == ACE_LIB_TEXT ('\0'))
        break;

      // Build the entry name
      ACE_TString nextname;
      while (*line && *line != ACE_LIB_TEXT ('|') && *line != ACE_LIB_TEXT (','))
        nextname += *line++;

      // We have found the required entry?
      if (ACE_OS::strcmp (nextname.c_str (), name) == 0)
        return 1;

      // Skip puntuaction char if neccesary.
      if (*line == ACE_LIB_TEXT ('|') || *line == ACE_LIB_TEXT (','))
        line++;
      else
        {
          ACE_DEBUG ((LM_DEBUG,
                      ACE_LIB_TEXT ("Invalid entry\n")));
          break;
        }
    }
  return 0;
}

int
ACE_Capabilities::getline (FILE *fp, ACE_TString &line)
{
  int ch;

  line.set (0, 0);

  while ((ch = fgetc (fp)) != EOF && ch != ACE_LIB_TEXT ('\n'))
    line += (ACE_TCHAR) ch;

  if (ch == EOF && line.length () == 0)
    return -1;
  else
    return 0;
}

int
ACE_Capabilities::getval (const ACE_TCHAR *keyname, ACE_TString &val)
{
  ACE_CapEntry* cap;
  if (this->caps_.find (keyname, cap) == -1)
    return -1;

  ACE_StringCapEntry *scap =
    ACE_dynamic_cast (ACE_StringCapEntry *, cap);
  if (scap == 0)
    return -1;

  val = scap->getval ();
  return 0;
}

int
ACE_Capabilities::getval (const ACE_TCHAR *keyname, int &val)
{
  ACE_CapEntry *cap;
  if (this->caps_.find (keyname, cap) == -1)
    return -1;

  ACE_IntCapEntry *icap =
    ACE_dynamic_cast (ACE_IntCapEntry *, cap);
  if (icap != 0)
    {
      val = icap->getval ();
      return 0;
    }

  ACE_BoolCapEntry *bcap =
    ACE_dynamic_cast (ACE_BoolCapEntry *, cap);

  if (bcap == 0)
    return -1;

  val = bcap->getval ();
  return 0;
}

#if !defined (ACE_IS_SPLITTING)
static int
is_empty (const ACE_TCHAR *line)
{
  while (*line && isspace (*line))
    line++;

  return *line == ACE_LIB_TEXT ('\0') || *line == ACE_LIB_TEXT ('#');
}

static int
is_line (const ACE_TCHAR *line)
{
  while (*line && isspace (*line))
    line++;

  return *line != ACE_LIB_TEXT ('\0');
}
#endif /* !ACE_IS_SPLITTING */

int
ACE_Capabilities::getent (const ACE_TCHAR *fname, const ACE_TCHAR *name)
{
  FILE *fp = ACE_OS::fopen (fname, ACE_LIB_TEXT ("r"));

  if (fp == 0)
    ACE_ERROR_RETURN ((LM_ERROR,
                       ACE_LIB_TEXT ("Can't open %s file\n"),
                       fname),
                      -1);

  int done;
  ACE_TString line;

  while (!(done = (this->getline (fp, line) == -1))
         && is_empty (line.c_str ()))
    continue;

  while (!done)
    {
      ACE_TString newline;
      ACE_TString description;

      while (!(done = (this->getline (fp, newline) == -1)))
        if (is_line (newline.c_str ()))
          description += newline;
        else
          break;

      if (this->is_entry (name, line.c_str()))
        {
          ACE_OS::fclose (fp);
          return this->fillent (description.c_str ());
        }

      line = newline;
      while (!done && is_empty (line.c_str ()))
        done = this->getline (fp, line) == -1;
    }

  ACE_OS::fclose (fp);
  return -1;
}

#if defined (ACE_HAS_EXPLICIT_TEMPLATE_INSTANTIATION)
template class ACE_Hash_Map_Entry<ACE_TString,ACE_CapEntry*>;
template class ACE_Hash_Map_Manager_Ex<ACE_TString,ACE_CapEntry*,ACE_Hash<ACE_TString>,ACE_Equal_To<ACE_TString>,ACE_Null_Mutex>;
template class ACE_Hash_Map_Iterator_Base_Ex<ACE_TString,ACE_CapEntry*,ACE_Hash<ACE_TString>,ACE_Equal_To<ACE_TString>,ACE_Null_Mutex>;
template class ACE_Hash_Map_Iterator_Ex<ACE_TString,ACE_CapEntry*,ACE_Hash<ACE_TString>,ACE_Equal_To<ACE_TString>,ACE_Null_Mutex>;
template class ACE_Hash_Map_Reverse_Iterator_Ex<ACE_TString,ACE_CapEntry*,ACE_Hash<ACE_TString>,ACE_Equal_To<ACE_TString>,ACE_Null_Mutex>;
template class ACE_Hash<ACE_TString>;
template class ACE_Equal_To<ACE_TString>;
#elif defined (ACE_HAS_TEMPLATE_INSTANTIATION_PRAGMA)
#pragma instantiate ACE_Hash_Map_Entry<ACE_TString,ACE_CapEntry*>
#pragma instantiate ACE_Hash_Map_Manager_Ex<ACE_TString,ACE_CapEntry*,ACE_Hash<ACE_TString>,ACE_Equal_To<ACE_TString>,ACE_Null_Mutex>
#pragma instantiate ACE_Hash_Map_Iterator_Base_Ex<ACE_TString,ACE_CapEntry*,ACE_Hash<ACE_TString>,ACE_Equal_To<ACE_TString>,ACE_Null_Mutex>
#pragma instantiate ACE_Hash_Map_Iterator_Ex<ACE_TString,ACE_CapEntry*,ACE_Hash<ACE_TString>,ACE_Equal_To<ACE_TString>,ACE_Null_Mutex>
#pragma instantiate ACE_Hash_Map_Reverse_Iterator_Ex<ACE_TString,ACE_CapEntry*,ACE_Hash<ACE_TString>,ACE_Equal_To<ACE_TString>,ACE_Null_Mutex>
#pragma instantiate ACE_Hash<ACE_TString>
#pragma instantiate ACE_Equal_To<ACE_TString>
#endif /* ACE_HAS_EXPLICIT_TEMPLATE_INSTANTIATION */
