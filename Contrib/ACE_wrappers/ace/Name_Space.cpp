// Name_Space.cpp
// Name_Space.cpp,v 4.6 2002/01/18 02:20:08 schmidt Exp

#include "ace/Name_Space.h"

ACE_RCSID(ace, Name_Space, "Name_Space.cpp,v 4.6 2002/01/18 02:20:08 schmidt Exp")

ACE_Name_Binding::ACE_Name_Binding (void)
  : type_ (ACE_OS::strdup (""))
{
  ACE_TRACE ("ACE_Name_Binding::ACE_Name_Binding");
}


ACE_Name_Binding::~ACE_Name_Binding (void)
{
  ACE_TRACE ("ACE_Name_Binding::~ACE_Name_Binding");
  ACE_OS::free ((void *) this->type_);
}

ACE_Name_Binding::ACE_Name_Binding (const ACE_NS_WString &name,
                                    const ACE_NS_WString &value,
                                    const char *type)
  : name_ (name),
    value_ (value),
    type_ (type == 0 ? ACE_OS::strdup ("") : ACE_OS::strdup (type))
{
  ACE_TRACE ("ACE_Name_Binding::ACE_Name_Binding");
}

ACE_Name_Binding::ACE_Name_Binding (const ACE_Name_Binding &s)
  : name_ (s.name_),
    value_ (s.value_),
    type_ (ACE_OS::strdup (s.type_))
{
  ACE_TRACE ("ACE_Name_Binding::ACE_Name_Binding");
}

void
ACE_Name_Binding::operator = (const ACE_Name_Binding &s)
{
  ACE_TRACE ("ACE_Name_Binding::operator =");

  if (this != &s)
    {
      ACE_OS::free ((void *) this->type_);
      this->name_ = s.name_;
      this->value_ = s.value_;
      this->type_ = ACE_OS::strdup (s.type_);
    }
}

int
ACE_Name_Binding::operator == (const ACE_Name_Binding &s) const
{
  ACE_TRACE ("ACE_Name_Binding::operator ==");
  return this->name_ == s.name_
    && this->value_ == s.value_
    && ACE_OS::strcmp (this->type_, s.type_) == 0;
}

ACE_Name_Space::~ACE_Name_Space (void)
{
  ACE_TRACE ("ACE_Name_Space::~ACE_Name_Space");
}
