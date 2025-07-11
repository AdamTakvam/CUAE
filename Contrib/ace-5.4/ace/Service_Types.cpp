// Service_Types.cpp,v 4.28 2003/11/07 20:27:28 shuston Exp

#include "ace/Service_Types.h"

#if !defined (__ACE_INLINE__)
#include "ace/Service_Types.i"
#endif /* __ACE_INLINE__ */

#include "ace/Stream_Modules.h"
#include "ace/Stream.h"
#include "ace/OS_NS_stdio.h"
#include "ace/OS_NS_string.h"

ACE_RCSID(ace, Service_Types, "Service_Types.cpp,v 4.28 2003/11/07 20:27:28 shuston Exp")

typedef ACE_Stream<ACE_SYNCH> MT_Stream;
typedef ACE_Module<ACE_SYNCH> MT_Module;
typedef ACE_Task<ACE_SYNCH> MT_Task;

ACE_ALLOC_HOOK_DEFINE(ACE_Service_Type_Impl)

void
ACE_Service_Type_Impl::dump (void) const
{
#if defined (ACE_HAS_DUMP)
  ACE_TRACE ("ACE_Service_Type_Impl::dump");
#endif /* ACE_HAS_DUMP */
}

ACE_Service_Type_Impl::ACE_Service_Type_Impl (void *so,
                                              const ACE_TCHAR *s_name,
                                              u_int f,
                                              ACE_Service_Object_Exterminator gobbler)
  : name_ (0),
    obj_ (so),
    gobbler_ (gobbler),
    flags_ (f)
{
  ACE_TRACE ("ACE_Service_Type_Impl::ACE_Service_Type_Impl");
  this->name (s_name);
}

ACE_Service_Type_Impl::~ACE_Service_Type_Impl (void)
{
  ACE_TRACE ("ACE_Service_Type_Impl::~ACE_Service_Type_Impl");

  // It's ok to call this, even though we may have already deleted it
  // in the fini() method since it would then be NULL.
  delete [] (ACE_TCHAR *) this->name_;
}

int
ACE_Service_Type_Impl::fini (void) const
{
  ACE_TRACE ("ACE_Service_Type_Impl::fini");
  if (ACE::debug ())
    ACE_DEBUG ((LM_DEBUG,
                ACE_LIB_TEXT ("destroying %s, flags = %d\n"),
                this->name_,
                this->flags_));

  delete [] (ACE_TCHAR *) this->name_;
  ((ACE_Service_Type_Impl *) this)->name_ = 0;

  if (ACE_BIT_ENABLED (this->flags_,
                       ACE_Service_Type::DELETE_OBJ))
    {
      if (gobbler_ != 0)
        gobbler_ (this->object ());
      else
        // Cast to remove const-ness.
        operator delete ((void *) this->object ());
    }

  if (ACE_BIT_ENABLED (this->flags_,
                       ACE_Service_Type::DELETE_THIS))
    delete (ACE_Service_Type_Impl *) this;

  return 0;
}

ACE_Service_Object_Type::ACE_Service_Object_Type (void *so,
                                                  const ACE_TCHAR *s_name,
                                                  u_int f,
                                                  ACE_Service_Object_Exterminator gobbler)
  : ACE_Service_Type_Impl (so, s_name, f, gobbler)
{
  ACE_TRACE ("ACE_Service_Object_Type::ACE_Service_Object_Type");
}

int
ACE_Service_Object_Type::init (int argc, ACE_TCHAR *argv[]) const
{
  ACE_TRACE ("ACE_Service_Object_Type::init");

  void *obj = this->object ();
  ACE_Service_Object *so = (ACE_Service_Object *) obj;

  if (so == 0)
    return -1;
  else
    return so->init (argc, argv);
}

ACE_ALLOC_HOOK_DEFINE(ACE_Module_Type)

void
ACE_Module_Type::dump (void) const
{
#if defined (ACE_HAS_DUMP)
  ACE_TRACE ("ACE_Module_Type::dump");
#endif /* ACE_HAS_DUMP */
}

ACE_Module_Type::ACE_Module_Type (void *m,
                                  const ACE_TCHAR *m_name,
                                  u_int f)
  : ACE_Service_Type_Impl (m, m_name, f)
{
  ACE_TRACE ("ACE_Module_Type::ACE_Module_Type");
}

int
ACE_Module_Type::init (int argc, ACE_TCHAR *argv[]) const
{
  ACE_TRACE ("ACE_Module_Type::init");
  void *obj = this->object ();
  MT_Module *mod = (MT_Module *) obj;
  MT_Task *reader = mod->reader ();
  MT_Task *writer = mod->writer ();

  if (reader->init (argc, argv) == -1
      || writer->init (argc, argv) == -1)
    return -1;
  else
    return 0;
}

int
ACE_Module_Type::suspend (void) const
{
  ACE_TRACE ("ACE_Module_Type::suspend");
  void *obj = this->object ();
  MT_Module *mod = (MT_Module *) obj;
  MT_Task *reader = mod->reader ();
  MT_Task *writer = mod->writer ();

  if (reader->suspend () == -1
      || writer->suspend () == -1)
    return -1;
  else
    return 0;
}

int
ACE_Module_Type::resume (void) const
{
  ACE_TRACE ("ACE_Module_Type::resume");
  void *obj = this->object ();
  MT_Module *mod = (MT_Module *) obj;
  MT_Task *reader = mod->reader ();
  MT_Task *writer = mod->writer ();

  if (reader->resume () == -1
      || writer->resume () == -1)
    return -1;
  else
    return 0;
}

// Note, these operations are somewhat too familiar with the
// implementation of ACE_Module and ACE_Module::close...

int
ACE_Module_Type::fini (void) const
{
  ACE_TRACE ("ACE_Module_Type::fini");

  void *obj = this->object ();
  MT_Module *mod = (MT_Module *) obj;
  MT_Task *reader = mod->reader ();
  MT_Task *writer = mod->writer ();

  if (reader != 0)
    reader->fini ();

  if (writer != 0)
    writer->fini ();

  // Close the module and delete the memory.
  mod->close (MT_Module::M_DELETE);
  return ACE_Service_Type_Impl::fini ();
}

int
ACE_Module_Type::info (ACE_TCHAR **str, size_t len) const
{
  ACE_TRACE ("ACE_Module_Type::info");
  ACE_TCHAR buf[BUFSIZ];

  ACE_OS::sprintf (buf,
                   ACE_LIB_TEXT ("%s\t %s"),
                   this->name (),
                   ACE_LIB_TEXT ("# ACE_Module\n"));

  if (*str == 0 && (*str = ACE_OS::strdup (buf)) == 0)
    return -1;
  else
    ACE_OS::strsncpy (*str, buf, len);
  return ACE_static_cast (int, ACE_OS::strlen (buf));
}

void
ACE_Module_Type::link (ACE_Module_Type *n)
{
  ACE_TRACE ("ACE_Module_Type::link");
  this->link_ = n;
}

ACE_Module_Type *
ACE_Module_Type::link (void) const
{
  ACE_TRACE ("ACE_Module_Type::link");
  return this->link_;
}

ACE_ALLOC_HOOK_DEFINE(ACE_Stream_Type)

void
ACE_Stream_Type::dump (void) const
{
#if defined (ACE_HAS_DUMP)
  ACE_TRACE ("ACE_Stream_Type::dump");
#endif /* ACE_HAS_DUMP */
}

int
ACE_Stream_Type::init (int, ACE_TCHAR *[]) const
{
  ACE_TRACE ("ACE_Stream_Type::init");
  return 0;
}

int
ACE_Stream_Type::suspend (void) const
{
  ACE_TRACE ("ACE_Stream_Type::suspend");

  for (ACE_Module_Type *m = this->head_;
       m != 0;
       m = m->link ())
    m->suspend ();

  return 0;
}

int
ACE_Stream_Type::resume (void) const
{
  ACE_TRACE ("ACE_Stream_Type::resume");

  for (ACE_Module_Type *m = this->head_;
       m != 0;
       m = m->link ())
    m->resume ();

  return 0;
}

ACE_Stream_Type::ACE_Stream_Type (void *s,
                                  const ACE_TCHAR *s_name,
                                  u_int f)
  : ACE_Service_Type_Impl (s, s_name, f),
    head_ (0)
{
  ACE_TRACE ("ACE_Stream_Type::ACE_Stream_Type");
}

int
ACE_Stream_Type::info (ACE_TCHAR **str, size_t len) const
{
  ACE_TRACE ("ACE_Stream_Type::info");
  ACE_TCHAR buf[BUFSIZ];

  ACE_OS::sprintf (buf,
                   ACE_LIB_TEXT ("%s\t %s"),
                   this->name (),
                   ACE_LIB_TEXT ("# STREAM\n"));

  if (*str == 0 && (*str = ACE_OS::strdup (buf)) == 0)
    return -1;
  else
    ACE_OS::strsncpy (*str, buf, len);
  return ACE_static_cast (int, ACE_OS::strlen (buf));
}

int
ACE_Stream_Type::fini (void) const
{
  ACE_TRACE ("ACE_Stream_Type::fini");
  void *obj = this->object ();
  MT_Stream *str = (MT_Stream *) obj;

  for (ACE_Module_Type *m = this->head_; m != 0; )
    {
      ACE_Module_Type *t = m->link ();

      // Final arg is an indication to *not* delete the Module.
      str->remove (m->name (),
                   MT_Module::M_DELETE_NONE);

      // Finalize the Module (this may delete it, but we don't really
      // care since we don't access it again).
      m->fini ();
      m = t;
    }

  str->close ();
  return ACE_Service_Type_Impl::fini ();
}

// Locate and remove <mod_name> from the ACE_Stream.

int
ACE_Stream_Type::remove (ACE_Module_Type *mod)
{
  ACE_TRACE ("ACE_Stream_Type::remove");

  ACE_Module_Type *prev = 0;
  void *obj = this->object ();
  MT_Stream *str = (MT_Stream *) obj;
  int result = 0;

  for (ACE_Module_Type *m = this->head_; m != 0; )
    {
      // We need to do this first so we don't bomb out if we delete m!
      ACE_Module_Type *link = m->link ();

      if (m == mod)
        {
          if (prev == 0)
            this->head_ = link;
          else
            prev->link (link);

          // Final arg is an indication to *not* delete the Module.
          if (str->remove (m->name (),
                           MT_Module::M_DELETE_NONE) == -1)
            result = -1;

          // This call may end up deleting m, which is ok since we
          // don't access it again!
          m->fini ();
        }
      else
        prev = m;

      m = link;
    }

  return result;
}

int
ACE_Stream_Type::push (ACE_Module_Type *new_module)
{
  ACE_TRACE ("ACE_Stream_Type::push");
  void *obj = this->object ();
  MT_Stream *str = (MT_Stream *) obj;

  new_module->link (this->head_);
  this->head_ = new_module;
  obj = new_module->object ();
  return str->push ((MT_Module *) obj);
}

ACE_Module_Type *
ACE_Stream_Type::find (const ACE_TCHAR *mod_name) const
{
  ACE_TRACE ("ACE_Stream_Type::find");

  for (ACE_Module_Type *m = this->head_;
       m != 0;
       m = m->link ())
    if (ACE_OS::strcmp (m->name (), mod_name) == 0)
      return m;

  return 0;
}

int
ACE_Service_Object_Type::fini (void) const
{
  ACE_TRACE ("ACE_Service_Object_Type::fini");

  void *obj = this->object ();

  ACE_Service_Object *so = (ACE_Service_Object *) obj;

  if (so)
    {
      so->fini ();

#if 0
      if (ACE_BIT_ENABLED (this->flags_,
                           ACE_Service_Type::DELETE_OBJ))
        delete so;
#endif /* 1 */
    }

  return ACE_Service_Type_Impl::fini ();
}
/*
#if defined (ACE_HAS_EXPLICIT_TEMPLATE_INSTANTIATION)
template class ACE_Message_Queue<ACE_SYNCH>;
template class ACE_Message_Queue_Iterator<ACE_SYNCH>;
template class ACE_Message_Queue_Reverse_Iterator<ACE_SYNCH>;
template class ACE_Message_Queue_Factory<ACE_SYNCH>;
template class ACE_Dynamic_Message_Queue<ACE_SYNCH>;
template class ACE_Module<ACE_SYNCH>;
template class ACE_Stream<ACE_SYNCH>;
template class ACE_Stream_Head<ACE_SYNCH>;
template class ACE_Stream_Tail<ACE_SYNCH>;
template class ACE_Task<ACE_SYNCH>;
template class ACE_Thru_Task<ACE_SYNCH>;

// Even with threads, these ACE_NULL_SYNCH specializations are necessary.
#if defined (ACE_HAS_THREADS)
  template class ACE_Message_Queue<ACE_NULL_SYNCH>;
  template class ACE_Message_Queue_Iterator<ACE_NULL_SYNCH>;
  template class ACE_Message_Queue_Reverse_Iterator<ACE_NULL_SYNCH>;
  template class ACE_Message_Queue_Factory<ACE_NULL_SYNCH>;
  template class ACE_Dynamic_Message_Queue<ACE_NULL_SYNCH>;
  template class ACE_Module<ACE_NULL_SYNCH>;
  template class ACE_Task<ACE_NULL_SYNCH>;
  template class ACE_Thru_Task<ACE_NULL_SYNCH>;
  #endif *//* ACE_HAS_THREADS */
/*
#elif defined (ACE_HAS_TEMPLATE_INSTANTIATION_PRAGMA)
#pragma instantiate ACE_Message_Queue<ACE_SYNCH>
#pragma instantiate ACE_Message_Queue_Iterator<ACE_SYNCH>
#pragma instantiate ACE_Message_Queue_Reverse_Iterator<ACE_SYNCH>
#pragma instantiate ACE_Message_Queue_Factory<ACE_SYNCH>
#pragma instantiate ACE_Dynamic_Message_Queue<ACE_SYNCH>
#pragma instantiate ACE_Module<ACE_SYNCH>
#pragma instantiate ACE_Stream<ACE_SYNCH>
#pragma instantiate ACE_Stream_Head<ACE_SYNCH>
#pragma instantiate ACE_Stream_Tail<ACE_SYNCH>
#pragma instantiate ACE_Task<ACE_SYNCH>
#pragma instantiate ACE_Thru_Task<ACE_SYNCH>
// Even with threads, these ACE_NULL_SYNCH specializations are necessary.
#if defined (ACE_HAS_THREADS)
  #pragma instantiate ACE_Message_Queue<ACE_NULL_SYNCH>
  #pragma instantiate ACE_Message_Queue_Iterator<ACE_NULL_SYNCH>
  #pragma instantiate ACE_Message_Queue_Reverse_Iterator<ACE_NULL_SYNCH>
  #pragma instantiate ACE_Message_Queue_Factory<ACE_NULL_SYNCH>
  #pragma instantiate ACE_Dynamic_Message_Queue<ACE_NULL_SYNCH>
  #pragma instantiate ACE_Module<ACE_NULL_SYNCH>
  #pragma instantiate ACE_Task<ACE_NULL_SYNCH>
  #pragma instantiate ACE_Thru_Task<ACE_NULL_SYNCH>
  #endif *//* ACE_HAS_THREADS */
//#else
//#endif /* ACE_HAS_EXPLICIT_TEMPLATE_INSTANTIATION */
