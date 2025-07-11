// FILE.cpp
// FILE.cpp,v 4.16 2003/11/01 11:15:12 dhinton Exp

/* Defines the member functions for the base class of the ACE_IO_SAP
   ACE_FILE abstraction. */

#include "ace/FILE.h"

#if defined (ACE_LACKS_INLINE_FUNCTIONS)
#include "ace/FILE.i"
#endif

#include "ace/OS_NS_unistd.h"
#include "ace/OS_NS_sys_stat.h"

ACE_RCSID(ace, FILE, "FILE.cpp,v 4.16 2003/11/01 11:15:12 dhinton Exp")

ACE_ALLOC_HOOK_DEFINE(ACE_FILE)

void
ACE_FILE::dump (void) const
{
#if defined (ACE_HAS_DUMP)
  ACE_TRACE ("ACE_FILE::dump");
  ACE_IO_SAP::dump ();
#endif /* ACE_HAS_DUMP */
}

// This is the do-nothing constructor.

ACE_FILE::ACE_FILE (void)
{
  ACE_TRACE ("ACE_FILE::ACE_FILE");
}

// Close the file

int
ACE_FILE::close (void)
{
  ACE_TRACE ("ACE_FILE::close");
  int result = 0;

  if (this->get_handle () != ACE_INVALID_HANDLE)
    {
      result = ACE_OS::close (this->get_handle ());
      this->set_handle (ACE_INVALID_HANDLE);
    }
  return result;
}

int
ACE_FILE::get_info (ACE_FILE_Info *finfo)
{
  ACE_TRACE ("ACE_FILE::get_info");
  ACE_stat filestatus;

  int result = ACE_OS::fstat (this->get_handle (),
                              &filestatus);

  if (result == 0)
    {
      finfo->mode_ = filestatus.st_mode;
      finfo->nlink_ = filestatus.st_nlink;
      finfo->size_ = filestatus.st_size;
    }

  return result;
}

int
ACE_FILE::get_info (ACE_FILE_Info &finfo)
{
  ACE_TRACE ("ACE_FILE::get_info");

  return this->get_info (&finfo);
}

int
ACE_FILE::truncate (off_t length)
{
  ACE_TRACE ("ACE_FILE::truncate");
  return ACE_OS::ftruncate (this->get_handle(), length);
}

off_t
ACE_FILE::seek (off_t offset, int startpos)
{
  return ACE_OS::lseek (this->get_handle (),
                        offset,
                        startpos);
}

off_t
ACE_FILE::position (long offset, int startpos)
{
  ACE_TRACE ("ACE_FILE::position");
  return this->seek (offset, startpos);
}

off_t
ACE_FILE::tell (void)
{
  ACE_TRACE ("ACE_FILE::tell");
  return ACE_OS::lseek (this->get_handle (), 0, SEEK_CUR);
}

off_t
ACE_FILE::position (void)
{
  ACE_TRACE ("ACE_FILE::position");
  return this->tell ();
}

// Return the local endpoint address.

int
ACE_FILE::get_local_addr (ACE_Addr &addr) const
{
  ACE_TRACE ("ACE_FILE::get_local_addr");

  // Perform the downcast since <addr> had better be an
  // <ACE_FILE_Addr>.
  ACE_FILE_Addr *file_addr =
    ACE_dynamic_cast (ACE_FILE_Addr *, &addr);

  if (file_addr == 0)
    return -1;
  else
    {
      *file_addr = this->addr_;
      return 0;
    }
}

// Return the same result as <get_local_addr>.

int
ACE_FILE::get_remote_addr (ACE_Addr &addr) const
{
  ACE_TRACE ("ACE_FILE::get_remote_addr");

  return this->get_local_addr (addr);
}

int
ACE_FILE::remove (void)
{
  ACE_TRACE ("ACE_FILE::remove");

  this->close ();
  return ACE_OS::unlink (this->addr_.get_path_name ());
}

int
ACE_FILE::unlink (void)
{
  ACE_TRACE ("ACE_FILE::unlink");

  return ACE_OS::unlink (this->addr_.get_path_name ());
}
