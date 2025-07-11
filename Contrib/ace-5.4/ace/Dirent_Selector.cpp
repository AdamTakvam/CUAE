// Dirent_Selector.cpp,v 4.4 2003/11/01 11:15:12 dhinton Exp

#include "ace/Dirent_Selector.h"

#if !defined (__ACE_INLINE__)
#include "ace/Dirent_Selector.inl"
#endif /* __ACE_INLINE__ */

#include "ace/OS_NS_dirent.h"
#include "ace/OS_NS_stdlib.h"

ACE_RCSID (ace,
           Dirent_Selector,
           "Dirent_Selector.cpp,v 4.4 2003/11/01 11:15:12 dhinton Exp")

// Construction/Destruction

ACE_Dirent_Selector::ACE_Dirent_Selector (void)
  : namelist_ (0),
    n_ (0)
{
}

ACE_Dirent_Selector::~ACE_Dirent_Selector (void)
{
}

int
ACE_Dirent_Selector::open (const ACE_TCHAR *dir,
                           int (*sel)(const dirent *d),
                           int (*cmp) (const dirent **d1,
                                       const dirent **d2))
{
  n_ = ACE_OS::scandir (dir, &this->namelist_, sel, cmp);
  return n_;
}

int
ACE_Dirent_Selector::close (void)
{
  for (--n_; n_>=0; --n_)
    ACE_OS::free (this->namelist_[n_]);

  ACE_OS::free (this->namelist_);
  return 0;
}
