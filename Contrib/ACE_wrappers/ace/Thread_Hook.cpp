// Thread_Hook.cpp,v 4.3 2002/07/02 19:56:49 shuston Exp

#include "ace/Thread_Hook.h"
#include "ace/OS.h"

ACE_RCSID(ace, Thread_Hook, "Thread_Hook.cpp,v 4.3 2002/07/02 19:56:49 shuston Exp")

ACE_THR_FUNC_RETURN
ACE_Thread_Hook::start (ACE_THR_FUNC func,
                        void *arg)
{
  return (func) (arg);
}

ACE_Thread_Hook *
ACE_Thread_Hook::thread_hook (ACE_Thread_Hook *hook)
{
  return ACE_OS_Object_Manager::thread_hook (hook);
}

ACE_Thread_Hook *
ACE_Thread_Hook::thread_hook (void)
{
  return ACE_OS_Object_Manager::thread_hook ();
}
