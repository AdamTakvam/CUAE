// SString.cpp,v 4.86 2003/11/10 01:48:02 dhinton Exp

#include "ace/Malloc.h"
#if !defined (ACE_HAS_WINCE)
//# include "ace/Service_Config.h"
#endif /* !ACE_HAS_WINCE */
#include "ace/SString.h"
#include "ace/Auto_Ptr.h"
#include "ace/OS_NS_string.h"

#if !defined (ACE_LACKS_IOSTREAM_TOTALLY)
// FUZZ: disable check_for_streams_include
# include "ace/streams.h"
#endif /* ! ACE_LACKS_IOSTREAM_TOTALLY */

#if !defined (__ACE_INLINE__)
#include "ace/SString.i"
#endif /* __ACE_INLINE__ */

ACE_RCSID (ace,
           SString, 
           "SString.cpp,v 4.61 2001/03/04 00:55:30 brunsch Exp")


// ************************************************************

#if !defined (ACE_LACKS_IOSTREAM_TOTALLY)
ACE_OSTREAM_TYPE &
operator<< (ACE_OSTREAM_TYPE &os, const ACE_CString &cs)
{
  if (cs.fast_rep () != 0)
    os << cs.fast_rep ();
  return os;
}

ACE_OSTREAM_TYPE &
operator<< (ACE_OSTREAM_TYPE &os, const ACE_WString &ws)
{
  // @@ Need to figure out how to print the "wide" string
  //    on platforms that don't support "wide" strings.
#if defined (ACE_HAS_WCHAR)
  os << ACE_Wide_To_Ascii (ws.fast_rep ()).char_rep ();
#else
  ACE_UNUSED_ARG (ws);
  os << "(*non-printable string*)";
#endif
  return os;
}

ACE_OSTREAM_TYPE &
operator<< (ACE_OSTREAM_TYPE &os, const ACE_SString &ss)
{
  if (ss.fast_rep () != 0)
    os << ss.fast_rep ();
  return os;
}
#endif /* !ACE_LACKS_IOSTREAM_TOTALLY */

// *****************************************************************

char *
ACE_NS_WString::char_rep (void) const
{
  ACE_TRACE ("ACE_NS_WString::char_rep");
  if (this->len_ <= 0)
    return 0;
  else
    {
      char *t;

      ACE_NEW_RETURN (t,
                      char[this->len_ + 1],
                      0);

      for (size_t i = 0; i < this->len_; i++)
        // Note that this cast may lose data if wide chars are
        // actually used!
        t[i] = char (this->rep_[i]);

      t[this->len_] = '\0';
      return t;
    }
}

ACE_USHORT16 *
ACE_NS_WString::ushort_rep (void) const
{
  ACE_TRACE ("ACE_NS_WString::ushort_rep");
  if (this->len_ <= 0)
    return 0;
  else
    {
      ACE_USHORT16 *t;

      ACE_NEW_RETURN (t,
                      ACE_USHORT16[this->len_ + 1],
                      0);

      for (size_t i = 0; i < this->len_; i++)
        // Note that this cast may lose data if wide chars are
        // actually used!
        t[i] = (ACE_USHORT16)this->rep_[i];

      t[this->len_] = 0;
      return t;
    }
}

ACE_NS_WString::ACE_NS_WString (const char *s,
                                ACE_Allocator *alloc)
  : ACE_WString (alloc)
{
  if (s == 0)
    return;

  this->len_ = this->buf_len_ = ACE_OS::strlen (s);

  if (this->buf_len_ == 0)
    return;

  ACE_ALLOCATOR (this->rep_,
                 (ACE_WSTRING_TYPE *)
                 this->allocator_->malloc ((this->buf_len_ + 1) *
                                           sizeof (ACE_WSTRING_TYPE)));
  this->release_ = 1;
  for (size_t i = 0; i <= this->buf_len_; i++)
    this->rep_[i] = s[i];
}

#if defined (ACE_WSTRING_HAS_USHORT_SUPPORT)
ACE_NS_WString::ACE_NS_WString (const ACE_USHORT16 *s,
                                size_t len,
                                ACE_Allocator *alloc)
  : ACE_WString (alloc)
{
  if (s == 0)
    return;

  this->buf_len_ = len;

  if (this->buf_len_ == 0)
    return;

  ACE_ALLOCATOR (this->rep_,
                 (ACE_WSTRING_TYPE *)
                 this->allocator_->malloc ((this->buf_len_) *
                                           sizeof (ACE_WSTRING_TYPE)));
  this->release_ = 1;
  for (size_t i = 0; i < this->buf_len_; i++)
    this->rep_[i] = s[i];
}
#endif /* ACE_WSTRING_HAS_USHORT_SUPPORT */

// *****************************************************************

const int ACE_SString::npos = -1;

ACE_ALLOC_HOOK_DEFINE(ACE_SString)

void
ACE_SString::dump (void) const
{
#if defined (ACE_HAS_DUMP)
  ACE_TRACE ("ACE_SString::dump");
#endif /* ACE_HAS_DUMP */
}

// Copy constructor.

ACE_SString::ACE_SString (const ACE_SString &s)
  : allocator_ (s.allocator_),
    len_ (s.len_)
{
  ACE_TRACE ("ACE_SString::ACE_SString");

  if (this->allocator_ == 0)
    this->allocator_ = ACE_Allocator::instance ();

  this->rep_ = (char *) this->allocator_->malloc (s.len_ + 1);
  ACE_OS::memcpy ((void *) this->rep_,
                  (const void *) s.rep_,
                  this->len_);
  this->rep_[this->len_] = '\0';
}

// Default constructor.

ACE_SString::ACE_SString (ACE_Allocator *alloc)
  : allocator_ (alloc),
    len_ (0),
    rep_ (0)

{
  ACE_TRACE ("ACE_SString::ACE_SString");

  if (this->allocator_ == 0)
    this->allocator_ = ACE_Allocator::instance ();

  this->len_ = 0;
  this->rep_ = (char *) this->allocator_->malloc (this->len_ + 1);
  this->rep_[this->len_] = '\0';
}

// Set the underlying pointer (does not copy memory).

void
ACE_SString::rep (char *s)
{
  ACE_TRACE ("ACE_SString::rep");

  this->rep_ = s;

  if (s == 0)
    this->len_ = 0;
  else
    this->len_ = ACE_OS::strlen (s);
}

// Constructor that actually copies memory.

ACE_SString::ACE_SString (const char *s,
                          ACE_Allocator *alloc)
  : allocator_ (alloc)
{
  ACE_TRACE ("ACE_SString::ACE_SString");

  if (this->allocator_ == 0)
    this->allocator_ = ACE_Allocator::instance ();

  if (s == 0)
    {
      this->len_ = 0;
      this->rep_ = (char *) this->allocator_->malloc (this->len_ + 1);
      this->rep_[this->len_] = '\0';
    }
  else
    {
      this->len_ = ACE_OS::strlen (s);
      this->rep_ = (char *) this->allocator_->malloc (this->len_ + 1);
      ACE_OS::strcpy (this->rep_, s);
    }
}

ACE_SString::ACE_SString (char c,
                          ACE_Allocator *alloc)
  : allocator_ (alloc)
{
  ACE_TRACE ("ACE_SString::ACE_SString");

  if (this->allocator_ == 0)
    this->allocator_ = ACE_Allocator::instance ();

  this->len_ = 1;
  this->rep_ = (char *) this->allocator_->malloc (this->len_ + 1);
  this->rep_[0] = c;
  this->rep_[this->len_] = '\0';
}

// Constructor that actually copies memory.

ACE_SString::ACE_SString (const char *s,
                          size_t len,
                          ACE_Allocator *alloc)
  : allocator_ (alloc)
{
  ACE_TRACE ("ACE_SString::ACE_SString");

  if (this->allocator_ == 0)
    this->allocator_ = ACE_Allocator::instance ();

  if (s == 0)
    {
      this->len_ = 0;
      this->rep_ = (char *) this->allocator_->malloc (this->len_ + 1);
      this->rep_[this->len_] = '\0';
    }
  else
    {
      this->len_ = len;
      this->rep_ = (char *) this->allocator_->malloc (this->len_ + 1);
      ACE_OS::memcpy (this->rep_, s, len);
      this->rep_[len] = '\0'; // Make sure to NUL terminate this!
    }
}

// Assignment operator (does copy memory).

ACE_SString &
ACE_SString::operator= (const ACE_SString &s)
{
  ACE_TRACE ("ACE_SString::operator=");
  // Check for identify.

  if (this != &s)
    {
      // Only reallocate if we don't have enough space...
      if (this->len_ < s.len_)
        {
          this->allocator_->free (this->rep_);
          this->rep_ = (char *) this->allocator_->malloc (s.len_ + 1);
        }
      this->len_ = s.len_;
      ACE_OS::strcpy (this->rep_, s.rep_);
    }

  return *this;
}

// Return substring.
ACE_SString
ACE_SString::substring (size_t offset,
                        ssize_t length) const
{
  ACE_SString nill;
  size_t count = length;

  // case 1. empty string
  if (len_ == 0)
    return nill;

  // case 2. start pos l
  if (offset >= len_)
    return nill;

  // get all remaining bytes
  if (length == -1)
    count = len_ - offset;

  return ACE_SString (&rep_[offset], count, this->allocator_);
}

// ************************************************************

ACE_Tokenizer::ACE_Tokenizer (ACE_TCHAR *buffer)
  : buffer_ (buffer),
    index_ (0),
    preserves_index_ (0),
    delimiter_index_ (0)
{
}

int
ACE_Tokenizer::delimiter (ACE_TCHAR d)
{
  if (delimiter_index_ == MAX_DELIMITERS)
    return -1;

  delimiters_[delimiter_index_].delimiter_ = d;
  delimiters_[delimiter_index_].replace_ = 0;
  delimiter_index_++;
  return 0;
}

int
ACE_Tokenizer::delimiter_replace (ACE_TCHAR d,
                                  ACE_TCHAR replacement)
{
  // Make it possible to replace delimiters on-the-fly, e.g., parse
  // string until certain token count and then copy rest of the
  // original string.
  for (int i = 0; i < delimiter_index_; i++)
    if (delimiters_[i].delimiter_ == d)
      {
        delimiters_[i].replacement_ = replacement;
        delimiters_[i].replace_ = 1;
        return 0;
      }

  if (delimiter_index_ >= MAX_DELIMITERS)
    return -1;

  delimiters_[delimiter_index_].delimiter_ = d;
  delimiters_[delimiter_index_].replacement_ = replacement;
  delimiters_[delimiter_index_].replace_ = 1;
  delimiter_index_++;
  return 0;
}

int
ACE_Tokenizer::preserve_designators (ACE_TCHAR start,
                                     ACE_TCHAR stop,
                                     int strip)
{
  if (preserves_index_ == MAX_PRESERVES)
    return -1;

  preserves_[preserves_index_].start_ = start;
  preserves_[preserves_index_].stop_ = stop;
  preserves_[preserves_index_].strip_ = strip;
  preserves_index_++;
  return 0;
}

int
ACE_Tokenizer::is_delimiter (ACE_TCHAR d,
                             int &replace,
                             ACE_TCHAR &r)
{
  replace = 0;

  for (int x = 0; x < delimiter_index_; x++)
    if (delimiters_[x].delimiter_ == d)
      {
        if (delimiters_[x].replace_)
          {
            r = delimiters_[x].replacement_;
            replace = 1;
          }
        return 1;
      }

  return 0;
}

int
ACE_Tokenizer::is_preserve_designator (ACE_TCHAR start,
                                       ACE_TCHAR &stop,
                                       int &strip)
{
  for (int x = 0; x < preserves_index_; x++)
    if (preserves_[x].start_ == start)
      {
        stop = preserves_[x].stop_;
        strip = preserves_[x].strip_;
        return 1;
      }

  return 0;
}

ACE_TCHAR *
ACE_Tokenizer::next (void)
{
  // Check if the previous pass was the last one in the buffer.
  if (index_ == -1)
    {
      index_ = 0;
      return 0;
    }

  ACE_TCHAR replacement;
  int replace;
  ACE_TCHAR *next_token;

  // Skip all leading delimiters.
  for (;;)
    {
      // Check for end of string.
      if (buffer_[index_] == '\0')
        {
          // If we hit EOS at the start, return 0.
          index_ = 0;
          return 0;
        }

      if (this->is_delimiter (buffer_[index_],
                              replace,
                              replacement))
        index_++;
      else
        break;
    }

  // When we reach this point, buffer_[index_] is a non-delimiter and
  // not EOS - the start of our next_token.
  next_token = buffer_ + index_;

  // A preserved region is it's own token.
  ACE_TCHAR stop;
  int strip;
  if (this->is_preserve_designator (buffer_[index_],
                                    stop,
                                    strip))
    {
      while (++index_)
        {
          if (buffer_[index_] == '\0')
            {
              index_ = -1;
              goto EXIT_LABEL;
            }

          if (buffer_[index_] == stop)
            break;
        }

      if (strip)
        {
          // Skip start preserve designator.
          next_token += 1;
          // Zap the stop preserve designator.
          buffer_[index_] = '\0';
          // Increment to the next token.
          index_++;
        }

      goto EXIT_LABEL;
    }

  // Step through finding the next delimiter or EOS.
  for (;;)
    {
      // Advance pointer.
      index_++;

      // Check for delimiter.
      if (this->is_delimiter (buffer_[index_],
                              replace,
                              replacement))
        {
          // Replace the delimiter.
          if (replace != 0)
            buffer_[index_] = replacement;

          // Move the pointer up and return.
          index_++;
          goto EXIT_LABEL;
        }

      // A preserve designator signifies the end of this token.
      if (this->is_preserve_designator (buffer_[index_],
                                        stop,
                                        strip))
        goto EXIT_LABEL;

      // Check for end of string.
      if (buffer_[index_] == '\0')
        {
          index_ = -1;
          goto EXIT_LABEL;
        }
    }

EXIT_LABEL:
  return next_token;
}

// *************************************************************

#if defined (ACE_HAS_EXPLICIT_TEMPLATE_INSTANTIATION)
template class ACE_String_Base<char>;
template ACE_String_Base<char> operator + (const ACE_String_Base<char> &,
                                           const ACE_String_Base<char> &);
template ACE_String_Base<char> operator + (const ACE_String_Base<char> &,
                                           const char *);
template ACE_String_Base<char> operator + (const char *,
                                           const ACE_String_Base<char> &);
template class ACE_String_Base<ACE_WSTRING_TYPE>;
template ACE_String_Base<ACE_WSTRING_TYPE> operator + (const ACE_String_Base<ACE_WSTRING_TYPE> &,
                                                       const ACE_String_Base<ACE_WSTRING_TYPE> &);
template ACE_String_Base<ACE_WSTRING_TYPE> operator + (const ACE_String_Base<ACE_WSTRING_TYPE> &,
                                                       const ACE_WSTRING_TYPE *);
template ACE_String_Base<ACE_WSTRING_TYPE> operator + (const ACE_WSTRING_TYPE *,
                                                       const ACE_String_Base<ACE_WSTRING_TYPE> &);
#elif defined (ACE_HAS_TEMPLATE_INSTANTIATION_PRAGMA)
#pragma instantiate ACE_String_Base<char>
#pragma instantiate ACE_String_Base<char> operator + (const ACE_String_Base<char> &, const ACE_String_Base<char> &)
#pragma instantiate ACE_String_Base<char> operator + (const ACE_String_Base<char> &, const char *)
#pragma instantiate ACE_String_Base<char> operator + (const char *,const ACE_String_Base<char> &)
#pragma instantiate ACE_String_Base<ACE_WSTRING_TYPE>
#pragma instantiate ACE_String_Base<ACE_WSTRING_TYPE> operator + (const ACE_String_Base<ACE_WSTRING_TYPE> &, const ACE_String_Base<ACE_WSTRING_TYPE> &)
#pragma instantiate ACE_String_Base<ACE_WSTRING_TYPE> operator + (const ACE_String_Base<ACE_WSTRING_TYPE> &, const ACE_WSTRING_TYPE *)
#pragma instantiate ACE_String_Base<ACE_WSTRING_TYPE> operator + (const ACE_WSTRING_TYPE *,const ACE_String_Base<ACE_WSTRING_TYPE> &)
#elif defined (__GNUC__) && (defined (_AIX) || defined (__hpux) || defined (VXWORKS) || defined (__Lynx__))
template char ACE_String_Base<char>::NULL_String_;
template ACE_WSTRING_TYPE ACE_String_Base<ACE_WSTRING_TYPE>::NULL_String_;
#endif /* ACE_HAS_EXPLICIT_TEMPLATE_INSTANTIATION */
