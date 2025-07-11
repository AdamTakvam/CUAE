// -*- C++ -*- SAXExceptions.cpp,v 1.5 2002/05/23 19:14:25 kitty Exp

#include "ACEXML/common/SAXExceptions.h"
#include "ace/Log_Msg.h"
#include "ace/ACE.h"

static const ACEXML_Char ACEXML_SAXException_name[] = {
  'A', 'C', 'E', 'X', 'M', 'L',
  'S', 'A', 'X',
  '_', 'E', 'x', 'c', 'e', 'p', 't', 'i', 'o', 'n', 0};
const ACEXML_Char *ACEXML_SAXException::exception_name_ = ACEXML_SAXException_name;

static const ACEXML_Char ACEXML_SAXNotSupportedException_name[] = {
  'A', 'C', 'E', 'X', 'M', 'L',
  'S', 'A', 'X',
  'N', 'o', 't',
  'S', 'u', 'p', 'p', 'o', 'r', 't', 'e', 'd',
  '_', 'E', 'x', 'c', 'e', 'p', 't', 'i', 'o', 'n', 0};
const ACEXML_Char *ACEXML_SAXNotSupportedException::exception_name_ = ACEXML_SAXNotSupportedException_name;

static const ACEXML_Char ACEXML_SAXNotRecognizedException_name[] = {
  'A', 'C', 'E', 'X', 'M', 'L',
  'S', 'A', 'X',
  'N', 'o', 't',
  'R', 'e', 'c', 'o', 'g', 'n', 'i', 'z', 'e', 'd',
  '_', 'E', 'x', 'c', 'e', 'p', 't', 'i', 'o', 'n', 0};
const ACEXML_Char *ACEXML_SAXNotRecognizedException::exception_name_ = ACEXML_SAXNotRecognizedException_name;

static const ACEXML_Char ACEXML_SAXParseException_name[] = {
  'A', 'C', 'E', 'X', 'M', 'L',
  'S', 'A', 'X',
  'P', 'a', 'r', 's', 'e',
  '_', 'E', 'x', 'c', 'e', 'p', 't', 'i', 'o', 'n', 0};
const ACEXML_Char *ACEXML_SAXParseException::exception_name_ = ACEXML_SAXParseException_name;

#if !defined (__ACEXML_INLINE__)
# include "ACEXML/common/SAXExceptions.i"
#endif /* __ACEXML_INLINE__ */

ACEXML_SAXException::ACEXML_SAXException (void)
  : message_ (0)
{
}

ACEXML_SAXException::ACEXML_SAXException (const ACEXML_Char *msg)
  : message_ (ACE::strnew (msg))
{
}

ACEXML_SAXException::ACEXML_SAXException (const ACEXML_SAXException &ex)
  : ACEXML_Exception (ex),
    message_ (ACE::strnew (ex.message_))

{
}


ACEXML_SAXException::~ACEXML_SAXException (void)
{
  delete[] this->message_;
}

const ACEXML_Char *
ACEXML_SAXException::id (void)
{
  return ACEXML_SAXException::exception_name_;
}

ACEXML_Exception *
ACEXML_SAXException::duplicate (void)
{
  ACEXML_Exception *tmp;
  ACE_NEW_RETURN (tmp,
                  ACEXML_SAXException (*this),
                  // Replace ACEXML_Exception with appropriate type.
                  0);
  return tmp;
}

int
ACEXML_SAXException::is_a (const ACEXML_Char *name)
{
  if (name == ACEXML_SAXException::exception_name_
      || ACE_OS::strcmp (ACEXML_Exception::exception_name_,
                         name) == 0)
    return 1;
  else
    return this->ACEXML_Exception::is_a (name);

  ACE_NOTREACHED (return 0;)
}

void
ACEXML_SAXException::print (void)
{
  // @@ Nanbor, I don't know how to handle the case
  //    when we define ACEXML_UTF16 as ACEXML_Char
  ACE_DEBUG ((LM_DEBUG,
              "Exception: ACEXML_SAXException -- %s\n",
              this->message_));
}

ACEXML_SAXNotSupportedException::ACEXML_SAXNotSupportedException (void)
{
}

ACEXML_SAXNotSupportedException::ACEXML_SAXNotSupportedException (const ACEXML_SAXNotSupportedException &ex)
  : ACEXML_SAXException (ex)
{
}


ACEXML_SAXNotSupportedException::~ACEXML_SAXNotSupportedException (void)
{
  delete[] this->message_;
}

const ACEXML_Char *
ACEXML_SAXNotSupportedException::id (void)
{
  return ACEXML_SAXNotSupportedException::exception_name_;
}

ACEXML_Exception *
ACEXML_SAXNotSupportedException::duplicate (void)
{
  ACEXML_Exception *tmp;
  ACE_NEW_RETURN (tmp,
                  ACEXML_SAXNotSupportedException (*this),
                  // Replace ACEXML_Exception with appropriate type.
                  0);
  return tmp;
}

int
ACEXML_SAXNotSupportedException::is_a (const ACEXML_Char *name)
{
  if (name == ACEXML_SAXNotSupportedException::exception_name_
      || ACE_OS::strcmp (ACEXML_Exception::exception_name_,
                         name) == 0)
    return 1;
  else
    return this->ACEXML_SAXException::is_a (name);

  ACE_NOTREACHED (return 0;)
}

void
ACEXML_SAXNotSupportedException::print (void)
{
  ACE_DEBUG ((LM_DEBUG,
              "Exception: ACEXML_SAXNotSupportedException -- %s\n",
              this->message_));
}

ACEXML_SAXNotRecognizedException::ACEXML_SAXNotRecognizedException (void)
{
}

ACEXML_SAXNotRecognizedException::ACEXML_SAXNotRecognizedException (const ACEXML_Char *msg)
  : ACEXML_SAXException (msg)
{
}

ACEXML_SAXNotRecognizedException::ACEXML_SAXNotRecognizedException (const ACEXML_SAXNotRecognizedException &ex)
  : ACEXML_SAXException (ex)
{
}


ACEXML_SAXNotRecognizedException::~ACEXML_SAXNotRecognizedException (void)
{
  delete[] this->message_;
}

const ACEXML_Char *
ACEXML_SAXNotRecognizedException::id (void)
{
  return ACEXML_SAXNotRecognizedException::exception_name_;
}

ACEXML_Exception *
ACEXML_SAXNotRecognizedException::duplicate (void)
{
  ACEXML_Exception *tmp;
  ACE_NEW_RETURN (tmp,
                  ACEXML_SAXNotRecognizedException (*this),
                  // Replace ACEXML_Exception with appropriate type.
                  0);
  return tmp;
}

int
ACEXML_SAXNotRecognizedException::is_a (const ACEXML_Char *name)
{
  if (name == ACEXML_SAXNotRecognizedException::exception_name_
      || ACE_OS::strcmp (ACEXML_Exception::exception_name_,
                         name) == 0)
    return 1;
  else
    return this->ACEXML_SAXException::is_a (name);

  ACE_NOTREACHED (return 0;)
}

void
ACEXML_SAXNotRecognizedException::print (void)
{
  ACE_DEBUG ((LM_DEBUG,
              "Exception: ACEXML_SAXNotRecognizedException -- %s\n",
              this->message_));
}

ACEXML_SAXParseException::ACEXML_SAXParseException (void)
{
}

ACEXML_SAXParseException::ACEXML_SAXParseException (const ACEXML_Char *msg)
  : ACEXML_SAXException (msg)
{
}

ACEXML_SAXParseException::ACEXML_SAXParseException (const ACEXML_SAXParseException &ex)
  : ACEXML_SAXException (ex)
{
}


ACEXML_SAXParseException::~ACEXML_SAXParseException (void)
{
}

const ACEXML_Char *
ACEXML_SAXParseException::id (void)
{
  return ACEXML_SAXParseException::exception_name_;
}

ACEXML_Exception *
ACEXML_SAXParseException::duplicate (void)
{
  ACEXML_Exception *tmp;
  ACE_NEW_RETURN (tmp,
                  ACEXML_SAXParseException (*this),
                  // Replace ACEXML_Exception with appropriate type.
                  0);
  return tmp;
}

int
ACEXML_SAXParseException::is_a (const ACEXML_Char *name)
{
  if (name == ACEXML_SAXParseException::exception_name_
      || ACE_OS::strcmp (ACEXML_Exception::exception_name_,
                         name) == 0)
    return 1;
  else
    return this->ACEXML_SAXException::is_a (name);

  ACE_NOTREACHED (return 0;)
}

void
ACEXML_SAXParseException::print (void)
{
  ACE_DEBUG ((LM_DEBUG,
              "Exception: ACEXML_SAXParseException -- %s\n",
              this->message_));
}
