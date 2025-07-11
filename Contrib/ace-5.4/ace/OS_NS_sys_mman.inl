// -*- C++ -*-
// OS_NS_sys_mman.inl,v 1.4 2003/11/18 16:05:00 dhinton Exp

#include "ace/OS_NS_fcntl.h"
#include "ace/OS_NS_unistd.h"
#include "ace/OS_NS_stdio.h"
#include "ace/OS_NS_macros.h"
#include "ace/OS_NS_errno.h"
#include "ace/os_include/sys/os_mman.h"

#if defined (__Lynx__)
#  include "ace/OS_NS_sys_stat.h"
#endif /* __Lynx__ */

#if defined (ACE_HAS_VOIDPTR_MMAP)
// Needed for some odd OS's (e.g., SGI).
typedef void *ACE_MMAP_TYPE;
#else
typedef char *ACE_MMAP_TYPE;
#endif /* ACE_HAS_VOIDPTR_MMAP */

ACE_INLINE int
ACE_OS::madvise (caddr_t addr, size_t len, int map_advice)
{
  ACE_OS_TRACE ("ACE_OS::madvise");
#if defined (ACE_WIN32)
  ACE_UNUSED_ARG (addr);
  ACE_UNUSED_ARG (len);
  ACE_UNUSED_ARG (map_advice);

  ACE_NOTSUP_RETURN (-1);
#elif !defined (ACE_LACKS_MADVISE)
  ACE_OSCALL_RETURN (::madvise (addr, len, map_advice), int, -1);
#else
  ACE_UNUSED_ARG (addr);
  ACE_UNUSED_ARG (len);
  ACE_UNUSED_ARG (map_advice);
  ACE_NOTSUP_RETURN (-1);
#endif /* ACE_WIN32 */
}

ACE_INLINE void *
ACE_OS::mmap (void *addr,
              size_t len,
              int prot,
              int flags,
              ACE_HANDLE file_handle,
              off_t off,
              ACE_HANDLE *file_mapping,
              LPSECURITY_ATTRIBUTES sa,
              const ACE_TCHAR *file_mapping_name)
{
  ACE_OS_TRACE ("ACE_OS::mmap");
#if !defined (ACE_WIN32) || defined (ACE_HAS_PHARLAP)
  ACE_UNUSED_ARG (file_mapping_name);
#endif /* !defined (ACE_WIN32) || defined (ACE_HAS_PHARLAP) */

#if defined (ACE_WIN32) && !defined (ACE_HAS_PHARLAP)

#  if defined(ACE_HAS_WINCE)
  ACE_UNUSED_ARG (addr);
  if (ACE_BIT_ENABLED (flags, MAP_FIXED))     // not supported
  {
    errno = EINVAL;
    return MAP_FAILED;
  }
#  else
  if (!ACE_BIT_ENABLED (flags, MAP_FIXED))
    addr = 0;
  else if (addr == 0)   // can not map to address 0
  {
    errno = EINVAL;
    return MAP_FAILED;
  }
#  endif

  int nt_flags = 0;
  ACE_HANDLE local_handle = ACE_INVALID_HANDLE;

  // Ensure that file_mapping is non-zero.
  if (file_mapping == 0)
    file_mapping = &local_handle;

  if (ACE_BIT_ENABLED (flags, MAP_PRIVATE))
    {
#  if !defined(ACE_HAS_WINCE)
      prot = PAGE_WRITECOPY;
#  endif  // ACE_HAS_WINCE
      nt_flags = FILE_MAP_COPY;
    }
  else if (ACE_BIT_ENABLED (flags, MAP_SHARED))
    {
      if (ACE_BIT_ENABLED (prot, PAGE_READONLY))
        nt_flags = FILE_MAP_READ;
      if (ACE_BIT_ENABLED (prot, PAGE_READWRITE))
        nt_flags = FILE_MAP_WRITE;
    }

  // Only create a new handle if we didn't have a valid one passed in.
  if (*file_mapping == ACE_INVALID_HANDLE)
    {
#  if !defined(ACE_HAS_WINCE) && (!defined (ACE_HAS_WINNT4) || (ACE_HAS_WINNT4 == 0))
      int try_create = 1;
      if ((file_mapping_name != 0) && (*file_mapping_name != 0))
        {
          // On Win9x, we first try to OpenFileMapping to
          // file_mapping_name. Only if there is no mapping object
          // with that name, and the desired name is valid, do we try
          // CreateFileMapping.

          *file_mapping = ACE_TEXT_OpenFileMapping (nt_flags,
                                                    0,
                                                    file_mapping_name);
          if (*file_mapping != 0
              || (::GetLastError () == ERROR_INVALID_NAME
                  && ::GetLastError () == ERROR_FILE_NOT_FOUND))
            try_create = 0;
        }

      if (try_create)
#  endif /* !ACE_HAS_WINCE && (ACE_HAS_WINNT4 || ACE_HAS_WINNT4 == 0) */
        {
          const LPSECURITY_ATTRIBUTES attr =
            ACE_OS::default_win32_security_attributes (sa);

          *file_mapping = ACE_TEXT_CreateFileMapping (file_handle,
                                                      attr,
                                                      prot,
                                                      0,
                                                      0,
                                                      file_mapping_name);
        }
    }

  if (*file_mapping == 0)
    ACE_FAIL_RETURN (MAP_FAILED);

#  if defined (ACE_OS_EXTRA_MMAP_FLAGS)
  nt_flags |= ACE_OS_EXTRA_MMAP_FLAGS;
#  endif /* ACE_OS_EXTRA_MMAP_FLAGS */

#  if !defined (ACE_HAS_WINCE)
  void *addr_mapping = ::MapViewOfFileEx (*file_mapping,
                                          nt_flags,
                                          0,
                                          off,
                                          len,
                                          addr);
#  else
  void *addr_mapping = ::MapViewOfFile (*file_mapping,
                                        nt_flags,
                                        0,
                                        off,
                                        len);
#  endif /* ! ACE_HAS_WINCE */

  // Only close this down if we used the temporary.
  if (file_mapping == &local_handle)
    ::CloseHandle (*file_mapping);

  if (addr_mapping == 0)
    ACE_FAIL_RETURN (MAP_FAILED);
  else
    return addr_mapping;
#elif defined (__Lynx__)
  // The LynxOS 2.5.0 mmap doesn't allow operations on plain
  // file descriptors.  So, create a shm object and use that.
  ACE_UNUSED_ARG (sa);

  char name [128];
  sprintf (name, "%d", file_handle);

  // Assumes that this was called by ACE_Mem_Map, so &file_mapping !=
  // 0.  Otherwise, we don't support the incomplete LynxOS mmap
  // implementation.  We do support it by creating a hidden shared
  // memory object, and using that for the mapping.
  int shm_handle;
  if (! file_mapping)
    file_mapping = &shm_handle;
  if ((*file_mapping = ::shm_open (name,
                                   O_RDWR | O_CREAT | O_TRUNC,
                                   ACE_DEFAULT_FILE_PERMS)) == -1)
    return MAP_FAILED;
  else
    {
      // The size of the shared memory object must be explicitly set on LynxOS.
      const off_t filesize = ACE_OS::filesize (file_handle);
      if (::ftruncate (*file_mapping, filesize) == -1)
        return MAP_FAILED;
      else
        {
#  if defined (ACE_OS_EXTRA_MMAP_FLAGS)
          flags |= ACE_OS_EXTRA_MMAP_FLAGS;
#  endif /* ACE_OS_EXTRA_MMAP_FLAGS */
          char *map = (char *) ::mmap ((ACE_MMAP_TYPE) addr,
                                       len,
                                       prot,
                                       flags,
                                       *file_mapping,
                                       off);
          if (map == MAP_FAILED)
            return MAP_FAILED;
          else
            // Finally, copy the file contents to the shared memory object.
            return ::read (file_handle, map, (int) filesize) == filesize
              ? map
              : MAP_FAILED;
        }
    }
#elif !defined (ACE_LACKS_MMAP)
  ACE_UNUSED_ARG (sa);

#  if defined (ACE_OS_EXTRA_MMAP_FLAGS)
  flags |= ACE_OS_EXTRA_MMAP_FLAGS;
#  endif /* ACE_OS_EXTRA_MMAP_FLAGS */
  ACE_UNUSED_ARG (file_mapping);
  ACE_OSCALL_RETURN ((void *) ::mmap ((ACE_MMAP_TYPE) addr,
                                      len,
                                      prot,
                                      flags,
                                      file_handle,
                                      off),
                     void *, MAP_FAILED);
#else
  ACE_UNUSED_ARG (addr);
  ACE_UNUSED_ARG (len);
  ACE_UNUSED_ARG (prot);
  ACE_UNUSED_ARG (flags);
  ACE_UNUSED_ARG (file_handle);
  ACE_UNUSED_ARG (off);
  ACE_UNUSED_ARG (file_mapping);
  ACE_UNUSED_ARG (sa);
  ACE_NOTSUP_RETURN (MAP_FAILED);
#endif /* ACE_WIN32 && !ACE_HAS_PHARLAP */
}

// Implements simple read/write control for pages.  Affects a page if
// part of the page is referenced.  Currently PROT_READ, PROT_WRITE,
// and PROT_RDWR has been mapped in OS.h.  This needn't have anything
// to do with a mmap region.

ACE_INLINE int
ACE_OS::mprotect (void *addr, size_t len, int prot)
{
  ACE_OS_TRACE ("ACE_OS::mprotect");
#if defined (ACE_WIN32) && !defined (ACE_HAS_PHARLAP)
  DWORD dummy; // Sigh!
  return ::VirtualProtect(addr, len, prot, &dummy) ? 0 : -1;
#elif !defined (ACE_LACKS_MPROTECT)
  ACE_OSCALL_RETURN (::mprotect ((ACE_MMAP_TYPE) addr, len, prot), int, -1);
#else
  ACE_UNUSED_ARG (addr);
  ACE_UNUSED_ARG (len);
  ACE_UNUSED_ARG (prot);
  ACE_NOTSUP_RETURN (-1);
#endif /* ACE_WIN32 && !ACE_HAS_PHARLAP */
}

ACE_INLINE int
ACE_OS::msync (void *addr, size_t len, int sync)
{
  ACE_OS_TRACE ("ACE_OS::msync");
#if defined (ACE_WIN32) && !defined (ACE_HAS_PHARLAP)
  ACE_UNUSED_ARG (sync);

  ACE_WIN32CALL_RETURN (ACE_ADAPT_RETVAL (::FlushViewOfFile (addr, len), ace_result_), int, -1);
#elif !defined (ACE_LACKS_MSYNC)
# if !defined (ACE_HAS_BROKEN_NETBSD_MSYNC)
  ACE_OSCALL_RETURN (::msync ((ACE_MMAP_TYPE) addr, len, sync), int, -1);
# else
  ACE_OSCALL_RETURN (::msync ((ACE_MMAP_TYPE) addr, len), int, -1);
  ACE_UNUSED_ARG (sync);
# endif /* ACE_HAS_BROKEN_NETBSD_MSYNC */
#else
  ACE_UNUSED_ARG (addr);
  ACE_UNUSED_ARG (len);
  ACE_UNUSED_ARG (sync);
  ACE_NOTSUP_RETURN (-1);
#endif /* ACE_WIN32 && !ACE_HAS_PHARLAP */
}

ACE_INLINE int
ACE_OS::munmap (void *addr, size_t len)
{
  ACE_OS_TRACE ("ACE_OS::munmap");
#if defined (ACE_WIN32)
  ACE_UNUSED_ARG (len);

  ACE_WIN32CALL_RETURN (ACE_ADAPT_RETVAL (::UnmapViewOfFile (addr), ace_result_), int, -1);
#elif !defined (ACE_LACKS_MMAP)
  ACE_OSCALL_RETURN (::munmap ((ACE_MMAP_TYPE) addr, len), int, -1);
#else
  ACE_UNUSED_ARG (addr);
  ACE_UNUSED_ARG (len);
  ACE_NOTSUP_RETURN (-1);
#endif /* ACE_WIN32 */
}

ACE_INLINE ACE_HANDLE
ACE_OS::shm_open (const ACE_TCHAR *filename,
                  int mode,
                  int perms,
                  LPSECURITY_ATTRIBUTES sa)
{
  ACE_OS_TRACE ("ACE_OS::shm_open");
# if defined (ACE_HAS_SHM_OPEN)
  ACE_UNUSED_ARG (sa);
  ACE_OSCALL_RETURN (::shm_open (filename, mode, perms), ACE_HANDLE, -1);
# else  /* ! ACE_HAS_SHM_OPEN */
  // Just use ::open.
  return ACE_OS::open (filename, mode, perms, sa);
# endif /* ACE_HAS_SHM_OPEN */
}

ACE_INLINE int
ACE_OS::shm_unlink (const ACE_TCHAR *path)
{
  ACE_OS_TRACE ("ACE_OS::shm_unlink");
# if defined (ACE_HAS_SHM_OPEN)
  ACE_OSCALL_RETURN (::shm_unlink (path), int, -1);
# else  /* ! ACE_HAS_SHM_OPEN */
  // Just use ::unlink.
  return ACE_OS::unlink (path);
# endif /* ACE_HAS_SHM_OPEN */
}

