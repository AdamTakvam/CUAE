/* -*- C++ -*- */

//=============================================================================
/**
 *  @file    Capabilities.h
 *
 *  Capabilities.h,v 4.11 2002/04/11 06:14:54 jwillemsen Exp
 *
 *  @author Arturo Montes <mitosys@colomsat.net.co>
 */
//=============================================================================


#ifndef ACE_CAPABILITIES_H
#define ACE_CAPABILITIES_H
#include "ace/pre.h"

#include "ace/config-all.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

#include "ace/Synch.h"
#include "ace/Hash_Map_Manager.h"
#include "ace/Containers.h"
#include "ace/SString.h"

/**
 * @class ACE_CapEntry
 *
 * @brief This class is the base class for all ACE Capabilities entry
 * subclasses.
 *
 * This class is not instantiable and does not provide accessors
 * or methods.  If you want to add a new kind of attribute you
 * subclasses of this class and dynamic cast to proper subclass.
 */
class ACE_Export ACE_CapEntry
{
public:
   virtual ~ACE_CapEntry (void);

protected:
  enum
  {
    ACE_INTCAP = 0,
    ACE_STRINGCAP = 1,
    ACE_BOOLCAP = 2
  };

  ACE_CapEntry (int captype);

  int captype_;
};

/**
 * @class ACE_IntCapEntry
 *
 * @brief This class implement the ACE Integer Capability subclass.
 *
 * This is a container class for ACE Capabilities integer container
 * values.
 */
class ACE_Export ACE_IntCapEntry : public ACE_CapEntry
{
public:
  ACE_IntCapEntry (int val);
  int getval (void) const;

protected:
  int val_;
};

/**
 * @class ACE_StringCapEntry
 *
 * @brief This class implement the ACE String Capability subclass.
 *
 * This is a container class for ACE Capabilities String container
 * values.
 */
class ACE_Export ACE_StringCapEntry : public ACE_CapEntry
{
public:
  ACE_StringCapEntry (const ACE_TString &val);
  ACE_TString getval (void) const;

protected:
  ACE_TString val_;
};

/**
 * @class ACE_BoolCapEntry
 *
 * @brief This class implement the ACE Bool Capability subclass.
 *
 * This is a container class for ACE Capabilities bool container
 * values.
 */
class ACE_Export ACE_BoolCapEntry : public ACE_CapEntry
{
public:
  ACE_BoolCapEntry (int val);
  int getval (void) const;

protected:
  int val_;
};

/**
 * @class ACE_Capabilities
 *
 * @brief This class implement the ACE Capabilities.
 *
 * This is a container class for ACE Capabilities
 * values. Currently exist three different capability values:
 * <ACE_IntCapEntry> (integer), <ACE_BoolCapEntry> (bool) and
 * <ACE_StringCapEntry> (String).  An <ACE_Capabilities> is a
 * unordered set of pair = (<String>, <ACE_CapEntry> *).  Where
 * the first component is the name of capability and the second
 * component is a pointer to the capability value container.  A
 * <FILE> is a container for <ACE_Capabilities>, the
 * <ACE_Capabilities> has a name in the file, as a termcap file.
 */
class ACE_Export ACE_Capabilities
{
public:
  /// The Constructor
  ACE_Capabilities (void);

  /// The Destructor
  ~ACE_Capabilities(void);

public:
  /// Get a string entry.
  int getval (const ACE_TCHAR *ent, ACE_TString &val);

  /// Get an integer entry.
  int getval (const ACE_TCHAR *ent, int &val);

  /// Get the ACE_Capabilities name from FILE fname and load the
  /// associated capabitily entries in map.
  int getent (const ACE_TCHAR *fname, const ACE_TCHAR *name);

protected:
  /// Parse an integer property
  const ACE_TCHAR *parse (const ACE_TCHAR *buf, int &cap);

  /// Parse a string property
  const ACE_TCHAR *parse (const ACE_TCHAR *buf, ACE_TString &cap);

  /// Fill the ACE_Capabilities with description in ent.
  int fillent(const ACE_TCHAR *ent);

  /// Parse a cap entry
  int parseent (const ACE_TCHAR *name, ACE_TCHAR *line);

  /// Get a line from FILE input stream
  int getline (FILE* fp,
               ACE_TString &line);

  /// Is a valid entry
  int is_entry (const ACE_TCHAR *name, const ACE_TCHAR *line);

  /// Reset the set of capabilities
  void resetcaps (void);

  /// Attributes
private:
  /// This is the set of ACE_CapEntry.
  ACE_Hash_Map_Manager<ACE_TString, ACE_CapEntry *, ACE_Null_Mutex> caps_;
};

#if defined (ACE_IS_SPLITTING)
int
is_empty (const ACE_TCHAR *line)
{
  while (*line && isspace (*line))
    line++;

  return *line == ACE_LIB_TEXT ('\0') || *line == ACE_LIB_TEXT ('#');
}

int
is_line (const ACE_TCHAR *line)
{
  while (*line && isspace (*line))
    line++;

  return *line != ACE_LIB_TEXT ('\0');
}
#endif /* ACE_IS_SPLITTING */

#if defined (__ACE_INLINE__)
#include "ace/Capabilities.i"
#endif /* __ACE_INLINE__ */

#include "ace/post.h"
#endif /* __ACE_CAPABILITIES_H__ */
