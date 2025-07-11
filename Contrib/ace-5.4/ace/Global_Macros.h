// -*- C++ -*-

//=============================================================================
/**
 *  @file   Global_Macros.h
 *
 *  Global_Macros.h,v 4.23 2003/11/06 18:19:38 dhinton Exp
 *
 *  @author Douglas C. Schmidt <schmidt@cs.wustl.edu>
 *  @author Jesper S. M|ller<stophph@diku.dk>
 *  @author and a cast of thousands...
 *  @This one is split from the famous OS.h
 */
//=============================================================================

#ifndef ACE_GLOBAL_MACROS_H
#define ACE_GLOBAL_MACROS_H

#include /**/ "ace/pre.h"

// Included just keep compilers that see #pragma dierctive first
// happy.
#include "ace/ACE_export.h"

#if !defined (ACE_LACKS_PRAGMA_ONCE)
# pragma once
#endif /* ACE_LACKS_PRAGMA_ONCE */

// Start Global Macros
# define ACE_BEGIN_DUMP ACE_LIB_TEXT ("\n====\n(%P|%t|%x)\n")
# define ACE_END_DUMP ACE_LIB_TEXT ("====\n")

# if defined (ACE_NDEBUG)
#   define ACE_DB(X)
# else
#   define ACE_DB(X) X
# endif /* ACE_NDEBUG */

// ACE_NO_HEAP_CHECK macro can be used to suppress false report of
// memory leaks. It turns off the built-in heap checking until the
// block is left. The old state will then be restored Only used for
// Win32 (in the moment).
# if defined (ACE_WIN32)

#   if defined (_DEBUG) && !defined (ACE_HAS_WINCE) && !defined (__BORLANDC__)
# include /**/ <crtdbg.h>

class ACE_Export ACE_No_Heap_Check
{
public:
  ACE_No_Heap_Check (void)
    : old_state (_CrtSetDbgFlag (_CRTDBG_REPORT_FLAG))
  { _CrtSetDbgFlag (old_state & ~_CRTDBG_ALLOC_MEM_DF);}
  ~ACE_No_Heap_Check (void) { _CrtSetDbgFlag (old_state);}
private:
  int old_state;
};
#     define ACE_NO_HEAP_CHECK ACE_No_Heap_Check ____no_heap;
#   else /* !_DEBUG */
#     define ACE_NO_HEAP_CHECK
#   endif /* _DEBUG */
# else /* !ACE_WIN32 */
#   define ACE_NO_HEAP_CHECK
# endif /* ACE_WIN32 */

// Turn a number into a string.
# define ACE_ITOA(X) #X

// Create a string of a server address with a "host:port" format.
# define ACE_SERVER_ADDRESS(H,P) H":"P

// A couple useful inline functions for checking whether bits are
// enabled or disabled.

// Efficiently returns the least power of two >= X...
# define ACE_POW(X) (((X) == 0)?1:(X-=1,X|=X>>1,X|=X>>2,X|=X>>4,X|=X>>8,X|=X>>16,(++X)))
# define ACE_EVEN(NUM) (((NUM) & 1) == 0)
# define ACE_ODD(NUM) (((NUM) & 1) == 1)
# define ACE_BIT_ENABLED(WORD, BIT) (((WORD) & (BIT)) != 0)
# define ACE_BIT_DISABLED(WORD, BIT) (((WORD) & (BIT)) == 0)
# define ACE_BIT_CMP_MASK(WORD, BIT, MASK) (((WORD) & (BIT)) == MASK)
# define ACE_SET_BITS(WORD, BITS) (WORD |= (BITS))
# define ACE_CLR_BITS(WORD, BITS) (WORD &= ~(BITS))

# if !defined (ACE_ENDLESS_LOOP)
#  define ACE_ENDLESS_LOOP
# endif /* ! ACE_ENDLESS_LOOP */

# if defined (ACE_NEEDS_FUNC_DEFINITIONS)
    // It just evaporated ;-)  Not pleasant.
#   define ACE_UNIMPLEMENTED_FUNC(f)
# else
#   define ACE_UNIMPLEMENTED_FUNC(f) f;
# endif /* ACE_NEEDS_FUNC_DEFINITIONS */

// Easy way to designate that a class is used as a pseudo-namespace.
// Insures that g++ "friendship" anamolies are properly handled.
# define ACE_CLASS_IS_NAMESPACE(CLASSNAME) \
private: \
CLASSNAME (void); \
CLASSNAME (const CLASSNAME&); \
friend class ace_dewarn_gplusplus

// ----------------------------------------------------------------

#if defined (ACE_HAS_NO_THROW_SPEC)
#   define ACE_THROW_SPEC(X)
#else
# if defined (ACE_HAS_EXCEPTIONS)
#   define ACE_THROW_SPEC(X) throw X
#   if defined (ACE_WIN32) && defined(_MSC_VER) && !defined (ghs)
// @@ MSVC "supports" the keyword but doesn't implement it (Huh?).
//    Therefore, we simply supress the warning for now.
#     pragma warning( disable : 4290 )
#   endif /* ACE_WIN32 */
# else  /* ! ACE_HAS_EXCEPTIONS */
#   define ACE_THROW_SPEC(X)
# endif /* ! ACE_HAS_EXCEPTIONS */
#endif /*ACE_HAS_NO_THROW_SPEC*/

// ----------------------------------------------------------------

// Deal with MSVC++ 6 (or less) insanity for CORBA...
# if defined (ACE_HAS_BROKEN_NAMESPACES)
#   define ACE_CORBA_1(NAME) CORBA_##NAME
#   define ACE_CORBA_2(TYPE, NAME) CORBA_##TYPE##_##NAME
#   define ACE_CORBA_3(TYPE, NAME) CORBA_##TYPE::NAME
#   if !defined (ACE_NESTED_CLASS)
#     define ACE_NESTED_CLASS(TYPE, NAME) NAME
#   endif  /* !ACE_NESTED_CLASS */
# else  /* ! ACE_HAS_BROKEN_NAMESPACES */
#   define ACE_CORBA_1(NAME) CORBA::NAME
#   define ACE_CORBA_2(TYPE, NAME) CORBA::TYPE::NAME
#   define ACE_CORBA_3(TYPE, NAME) CORBA::TYPE::NAME
#   if !defined (ACE_NESTED_CLASS)
#     define ACE_NESTED_CLASS(TYPE, NAME) TYPE::NAME
#   endif  /* !ACE_NESTED_CLASS */
# endif /* ! ACE_HAS_BROKEN_NAMESPACES */

// ----------------------------------------------------------------

# define ACE_TRACE_IMPL(X) ACE_Trace ____ (ACE_LIB_TEXT (X), __LINE__, ACE_LIB_TEXT (__FILE__))

# if (ACE_NTRACE == 1)
#   define ACE_TRACE(X)
# else
#   if !defined (ACE_HAS_TRACE)
#     define ACE_HAS_TRACE
#   endif /* ACE_HAS_TRACE */
#   define ACE_TRACE(X) ACE_TRACE_IMPL(X)
#   include "ace/Trace.h"
# endif /* ACE_NTRACE */

// ----------------------------------------------------------------

// Convenient macro for testing for deadlock, as well as for detecting
// when mutexes fail.
#define ACE_GUARD_ACTION(MUTEX, OBJ, LOCK, ACTION, REACTION) \
   ACE_Guard< MUTEX > OBJ (LOCK); \
   if (OBJ.locked () != 0) { ACTION; } \
   else { REACTION; }
#define ACE_GUARD_REACTION(MUTEX, OBJ, LOCK, REACTION) \
  ACE_GUARD_ACTION(MUTEX, OBJ, LOCK, ;, REACTION)
#define ACE_GUARD(MUTEX, OBJ, LOCK) \
  ACE_GUARD_REACTION(MUTEX, OBJ, LOCK, return)
#define ACE_GUARD_RETURN(MUTEX, OBJ, LOCK, RETURN) \
  ACE_GUARD_REACTION(MUTEX, OBJ, LOCK, return RETURN)
# define ACE_WRITE_GUARD(MUTEX,OBJ,LOCK) \
  ACE_Write_Guard< MUTEX > OBJ (LOCK); \
    if (OBJ.locked () == 0) return;
# define ACE_WRITE_GUARD_RETURN(MUTEX,OBJ,LOCK,RETURN) \
  ACE_Write_Guard< MUTEX > OBJ (LOCK); \
    if (OBJ.locked () == 0) return RETURN;
# define ACE_READ_GUARD(MUTEX,OBJ,LOCK) \
  ACE_Read_Guard< MUTEX > OBJ (LOCK); \
    if (OBJ.locked () == 0) return;
# define ACE_READ_GUARD_RETURN(MUTEX,OBJ,LOCK,RETURN) \
  ACE_Read_Guard< MUTEX > OBJ (LOCK); \
    if (OBJ.locked () == 0) return RETURN;

// ----------------------------------------------------------------

# define ACE_DES_NOFREE(POINTER,CLASS) \
   do { \
        if (POINTER) \
          { \
            (POINTER)->~CLASS (); \
          } \
      } \
   while (0)

# define ACE_DES_ARRAY_NOFREE(POINTER,SIZE,CLASS) \
   do { \
        if (POINTER) \
          { \
            for (size_t i = 0; \
                 i < SIZE; \
                 ++i) \
            { \
              (&(POINTER)[i])->~CLASS (); \
            } \
          } \
      } \
   while (0)

# define ACE_DES_FREE(POINTER,DEALLOCATOR,CLASS) \
   do { \
        if (POINTER) \
          { \
            (POINTER)->~CLASS (); \
            DEALLOCATOR (POINTER); \
          } \
      } \
   while (0)

# define ACE_DES_ARRAY_FREE(POINTER,SIZE,DEALLOCATOR,CLASS) \
   do { \
        if (POINTER) \
          { \
            for (size_t i = 0; \
                 i < SIZE; \
                 ++i) \
            { \
              (&(POINTER)[i])->~CLASS (); \
            } \
            DEALLOCATOR (POINTER); \
          } \
      } \
   while (0)

# if defined (ACE_HAS_WORKING_EXPLICIT_TEMPLATE_DESTRUCTOR)
#   define ACE_DES_NOFREE_TEMPLATE(POINTER,T_CLASS,T_PARAMETER) \
     do { \
          if (POINTER) \
            { \
              (POINTER)->~T_CLASS (); \
            } \
        } \
     while (0)
#   define ACE_DES_ARRAY_NOFREE_TEMPLATE(POINTER,SIZE,T_CLASS,T_PARAMETER) \
     do { \
          if (POINTER) \
            { \
              for (size_t i = 0; \
                   i < SIZE; \
                   ++i) \
              { \
                (&(POINTER)[i])->~T_CLASS (); \
              } \
            } \
        } \
     while (0)
#if defined(__IBMCPP__) && (__IBMCPP__ >= 400)
#   define ACE_DES_FREE_TEMPLATE(POINTER,DEALLOCATOR,T_CLASS,T_PARAMETER) \
     do { \
          if (POINTER) \
            { \
              (POINTER)->~T_CLASS T_PARAMETER (); \
              DEALLOCATOR (POINTER); \
            } \
        } \
     while (0)
#else
#   define ACE_DES_FREE_TEMPLATE(POINTER,DEALLOCATOR,T_CLASS,T_PARAMETER) \
     do { \
          if (POINTER) \
            { \
              (POINTER)->~T_CLASS (); \
              DEALLOCATOR (POINTER); \
            } \
        } \
     while (0)
#endif /* defined(__IBMCPP__) && (__IBMCPP__ >= 400) */
#   define ACE_DES_ARRAY_FREE_TEMPLATE(POINTER,SIZE,DEALLOCATOR,T_CLASS,T_PARAMETER) \
     do { \
          if (POINTER) \
            { \
              for (size_t i = 0; \
                   i < SIZE; \
                   ++i) \
              { \
                (&(POINTER)[i])->~T_CLASS (); \
              } \
              DEALLOCATOR (POINTER); \
            } \
        } \
     while (0)
#if defined(__IBMCPP__) && (__IBMCPP__ >= 400)
#   define ACE_DES_FREE_TEMPLATE2(POINTER,DEALLOCATOR,T_CLASS,T_PARAM1,T_PARAM2) \
     do { \
          if (POINTER) \
            { \
              (POINTER)->~T_CLASS <T_PARAM1, T_PARAM2> (); \
              DEALLOCATOR (POINTER); \
            } \
        } \
     while (0)
#else
#   define ACE_DES_FREE_TEMPLATE2(POINTER,DEALLOCATOR,T_CLASS,T_PARAM1,T_PARAM2) \
     do { \
          if (POINTER) \
            { \
              (POINTER)->~T_CLASS (); \
              DEALLOCATOR (POINTER); \
            } \
        } \
     while (0)
#endif /* defined(__IBMCPP__) && (__IBMCPP__ >= 400) */
#   define ACE_DES_FREE_TEMPLATE3(POINTER,DEALLOCATOR,T_CLASS,T_PARAM1,T_PARAM2,T_PARAM3) \
     do { \
          if (POINTER) \
            { \
              (POINTER)->~T_CLASS (); \
              DEALLOCATOR (POINTER); \
            } \
        } \
     while (0)
#   define ACE_DES_FREE_TEMPLATE4(POINTER,DEALLOCATOR,T_CLASS,T_PARAM1,T_PARAM2,T_PARAM3, T_PARAM4) \
     do { \
          if (POINTER) \
            { \
              (POINTER)->~T_CLASS (); \
              DEALLOCATOR (POINTER); \
            } \
        } \
     while (0)
#   define ACE_DES_ARRAY_FREE_TEMPLATE2(POINTER,SIZE,DEALLOCATOR,T_CLASS,T_PARAM1,T_PARAM2) \
     do { \
          if (POINTER) \
            { \
              for (size_t i = 0; \
                   i < SIZE; \
                   ++i) \
              { \
                (&(POINTER)[i])->~T_CLASS (); \
              } \
              DEALLOCATOR (POINTER); \
            } \
        } \
     while (0)
# else /* ! ACE_HAS_WORKING_EXPLICIT_TEMPLATE_DESTRUCTOR */
#   define ACE_DES_NOFREE_TEMPLATE(POINTER,T_CLASS,T_PARAMETER) \
     do { \
          if (POINTER) \
            { \
              (POINTER)->T_CLASS T_PARAMETER::~T_CLASS (); \
            } \
        } \
     while (0)
#   define ACE_DES_ARRAY_NOFREE_TEMPLATE(POINTER,SIZE,T_CLASS,T_PARAMETER) \
     do { \
          if (POINTER) \
            { \
              for (size_t i = 0; \
                   i < SIZE; \
                   ++i) \
              { \
                (POINTER)[i].T_CLASS T_PARAMETER::~T_CLASS (); \
              } \
            } \
        } \
     while (0)
#   if defined (__Lynx__) && __LYNXOS_SDK_VERSION == 199701L
  // LynxOS 3.0.0's g++ has trouble with the real versions of these.
#     define ACE_DES_FREE_TEMPLATE(POINTER,DEALLOCATOR,T_CLASS,T_PARAMETER)
#     define ACE_DES_ARRAY_FREE_TEMPLATE(POINTER,DEALLOCATOR,T_CLASS,T_PARAMETER)
#     define ACE_DES_FREE_TEMPLATE2(POINTER,DEALLOCATOR,T_CLASS,T_PARAM1,T_PARAM2)
#     define ACE_DES_FREE_TEMPLATE3(POINTER,DEALLOCATOR,T_CLASS,T_PARAM1,T_PARAM2)
#     define ACE_DES_FREE_TEMPLATE4(POINTER,DEALLOCATOR,T_CLASS,T_PARAM1,T_PARAM2)
#     define ACE_DES_ARRAY_FREE_TEMPLATE2(POINTER,DEALLOCATOR,T_CLASS,T_PARAM1,T_PARAM2)
#   else
#     define ACE_DES_FREE_TEMPLATE(POINTER,DEALLOCATOR,T_CLASS,T_PARAMETER) \
       do { \
            if (POINTER) \
              { \
                POINTER->T_CLASS T_PARAMETER::~T_CLASS (); \
                DEALLOCATOR (POINTER); \
              } \
          } \
       while (0)
#     define ACE_DES_ARRAY_FREE_TEMPLATE(POINTER,SIZE,DEALLOCATOR,T_CLASS,T_PARAMETER) \
       do { \
            if (POINTER) \
              { \
                for (size_t i = 0; \
                     i < SIZE; \
                     ++i) \
                { \
                  POINTER[i].T_CLASS T_PARAMETER::~T_CLASS (); \
                } \
                DEALLOCATOR (POINTER); \
              } \
          } \
       while (0)
#     define ACE_DES_FREE_TEMPLATE2(POINTER,DEALLOCATOR,T_CLASS,T_PARAM1,T_PARAM2) \
       do { \
            if (POINTER) \
              { \
                POINTER->T_CLASS <T_PARAM1, T_PARAM2>::~T_CLASS (); \
                DEALLOCATOR (POINTER); \
              } \
          } \
       while (0)
#     define ACE_DES_FREE_TEMPLATE3(POINTER,DEALLOCATOR,T_CLASS,T_PARAM1,T_PARAM2,T_PARAM3) \
       do { \
            if (POINTER) \
              { \
                POINTER->T_CLASS <T_PARAM1, T_PARAM2, T_PARAM3>::~T_CLASS (); \
                DEALLOCATOR (POINTER); \
              } \
          } \
       while (0)
#     define ACE_DES_FREE_TEMPLATE4(POINTER,DEALLOCATOR,T_CLASS,T_PARAM1,T_PARAM2,T_PARAM3,T_PARAM4) \
       do { \
            if (POINTER) \
              { \
                POINTER->T_CLASS <T_PARAM1, T_PARAM2, T_PARAM3, T_PARAM4>::~T_CLASS (); \
                DEALLOCATOR (POINTER); \
              } \
          } \
       while (0)
#     define ACE_DES_ARRAY_FREE_TEMPLATE2(POINTER,SIZE,DEALLOCATOR,T_CLASS,T_PARAM1,T_PARAM2) \
       do { \
            if (POINTER) \
              { \
                for (size_t i = 0; \
                     i < SIZE; \
                     ++i) \
                { \
                  POINTER[i].T_CLASS <T_PARAM1, T_PARAM2>::~T_CLASS (); \
                } \
                DEALLOCATOR (POINTER); \
              } \
          } \
       while (0)
#   endif /* defined (__Lynx__) && __LYNXOS_SDK_VERSION == 199701L */
# endif /* defined ! ACE_HAS_WORKING_EXPLICIT_TEMPLATE_DESTRUCTOR */


/*******************************************************************/

/// Service Objects, i.e., objects dynamically loaded via the service
/// configurator, must provide a destructor function with the
/// following prototype to perform object cleanup.
typedef void (*ACE_Service_Object_Exterminator)(void *);

/** @name Service Configurator macros
 *
 * The following macros are used to define helper objects used in
 * ACE's Service Configurator framework, which is described in
 * Chapter 5 of C++NPv2 <www.cs.wustl.edu/~schmidt/ACE/book2/>.  This
 * framework implements the Component Configurator pattern, which is
 * described in Chapter 2 of POSA2 <www.cs.wustl.edu/~schmidt/POSA/>.
 * The intent of this pattern is to allow developers to dynamically
 * load and configure services into a system.  With a little help from
 * this macros statically linked services can also be dynamically
 * configured.
 *
 * More details about this component are available in the documentation
 * of the ACE_Service_Configurator class and also
 * ACE_Dynamic_Service.
 *
 * Notice that in all the macros the SERVICE_CLASS parameter must be
 * the name of a class derived from ACE_Service_Object.
 */
//@{
/// Declare a the data structure required to register a statically
/// linked service into the service configurator.
/**
 * The macro should be used in the header file where the service is
 * declared, its only argument is usually the name of the class that
 * implements the service.
 *
 * @param SERVICE_CLASS The name of the class implementing the
 *   service.
 */
# define ACE_STATIC_SVC_DECLARE(SERVICE_CLASS) \
extern ACE_Static_Svc_Descriptor ace_svc_desc_##SERVICE_CLASS ;

/// As ACE_STATIC_SVC_DECLARE, but using an export macro for NT
/// compilers.
/**
 * NT compilers require the use of explicit directives to export and
 * import symbols from a DLL.  If you need to define a service in a
 * dynamic library you should use this version instead.
 * Normally ACE uses a macro to inject the correct export/import
 * directives on NT.  Naturally it also the macro expands to a blank
 * on platforms that do not require such directives.
 * The first argument (EXPORT_NAME) is the prefix for this export
 * macro, the full name is formed by appending _Export.
 * ACE provides tools to generate header files that define the macro
 * correctly on all platforms, please see
 * $ACE_ROOT/bin/generate_export_file.pl
 *
 * @param EXPORT_NAME The export macro name prefix.
 * @param SERVICE_CLASS The name of the class implementing the service.
 */
#define ACE_STATIC_SVC_DECLARE_EXPORT(EXPORT_NAME,SERVICE_CLASS) \
extern EXPORT_NAME##_Export ACE_Static_Svc_Descriptor ace_svc_desc_##SERVICE_CLASS;

/// Define the data structure used to register a statically linked
/// service into the Service Configurator.
/**
 * The service configurator requires several arguments to build and
 * control an statically linked service, including its name, the
 * factory function used to construct the service, and some flags.
 * All those parameters are configured in a single structure, an
 * instance of this structure is statically initialized using the
 * following macro.
 *
 * @param SERVICE_CLASS The name of the class that implements the
 *    service, must be derived (directly or indirectly) from
 *    ACE_Service_Object.
 * @param NAME The name for this service, this name is used by the
 *    service configurator to match configuration options provided in
 *    the svc.conf file.
 * @param TYPE The type of object.  Objects can be streams or service
 *    objects.  Please read the ACE_Service_Configurator and ASX
 *    documentation for more details.
 * @param FN The name of the factory function, usually the
 *    ACE_SVC_NAME macro can be used to generate the name.  The
 *    factory function is often defined using ACE_FACTORY_DECLARE and
 *    ACE_FACTORY_DEFINE.
 * @param FLAGS Flags to control the ownership and lifecycle of the
 *    object. Please read the ACE_Service_Configurator documentation
 *    for more details.
 * @param ACTIVE If not zero then a thread will be dedicate to the
 *    service. Please read the ACE_Service_Configurator documentation
 *    for more details.
 */
#define ACE_STATIC_SVC_DEFINE(SERVICE_CLASS, NAME, TYPE, FN, FLAGS, ACTIVE) \
ACE_Static_Svc_Descriptor ace_svc_desc_##SERVICE_CLASS = { NAME, TYPE, FN, FLAGS, ACTIVE };

/// Automatically register a service with the service configurator
/**
 * In some applications the services must be automatically registered
 * with the service configurator, before main() starts.
 * The ACE_STATIC_SVC_REQUIRE macro defines a class whose constructor
 * register the service, it also defines a static instance of that
 * class to ensure that the service is registered before main.
 *
 * On platforms that lack adequate support for static C++ objects the
 * macro ACE_STATIC_SVC_REGISTER can be used to explicitly register
 * the service.
 *
 * @todo One class per-Service_Object seems wasteful.  It should be
 *   possible to define a single class and re-use it for all the
 *   service objects, just by passing the Service_Descriptor as an
 *   argument to the constructor.
 */
#if defined(ACE_LACKS_STATIC_CONSTRUCTORS)
# define ACE_STATIC_SVC_REQUIRE(SERVICE_CLASS)\
class ACE_Static_Svc_##SERVICE_CLASS {\
public:\
  ACE_Static_Svc_##SERVICE_CLASS() { \
    ACE_Service_Config::static_svcs ()->insert (\
         &ace_svc_desc_##SERVICE_CLASS); \
  } \
};
#define ACE_STATIC_SVC_REGISTER(SERVICE_CLASS)\
ACE_Static_Svc_##SERVICE_CLASS ace_static_svc_##SERVICE_CLASS

#else /* !ACE_LACKS_STATIC_CONSTRUCTORS */

# define ACE_STATIC_SVC_REQUIRE(SERVICE_CLASS)\
class ACE_Static_Svc_##SERVICE_CLASS {\
public:\
  ACE_Static_Svc_##SERVICE_CLASS() { \
    ACE_Service_Config::static_svcs ()->insert (\
         &ace_svc_desc_##SERVICE_CLASS); \
    } \
};\
static ACE_Static_Svc_##SERVICE_CLASS ace_static_svc_##SERVICE_CLASS;
#define ACE_STATIC_SVC_REGISTER(SERVICE_CLASS) do {} while (0)

#endif /* !ACE_LACKS_STATIC_CONSTRUCTORS */

/// Declare the factory method used to create dynamically loadable
/// services.
/**
 * Once the service implementation is dynamically loaded the Service
 * Configurator uses a factory method to create the object.
 * This macro declares such a factory function with the proper
 * interface and export macros.
 * Normally used in the header file that declares the service
 * implementation.
 *
 * @param CLS must match the prefix of the export macro used for this
 *        service.
 * @param SERVICE_CLASS must match the name of the class that
 *        implements the service.
 *
 */
#define ACE_FACTORY_DECLARE(CLS,SERVICE_CLASS) \
extern "C" CLS##_Export ACE_Service_Object *\
_make_##SERVICE_CLASS (ACE_Service_Object_Exterminator *);

/// Define the factory method (and destructor) for a dynamically
/// loadable service.
/**
 * Use with arguments matching ACE_FACTORY_DECLARE.
 * Normally used in the .cpp file that defines the service
 * implementation.
 *
 * This macro defines both the factory method and the function used to
 * cleanup the service object.
 *
 * If this macro is used to define a factory function that need not be
 * exported (for example, in a static service situation), CLS can be
 * specified as ACE_Local_Service.
 */
# define ACE_Local_Service_Export

# define ACE_FACTORY_DEFINE(CLS,SERVICE_CLASS) \
void _gobble_##SERVICE_CLASS (void *p) { \
  ACE_Service_Object *_p = ACE_static_cast (ACE_Service_Object *, p); \
  ACE_ASSERT (_p != 0); \
  delete _p; } \
extern "C" CLS##_Export ACE_Service_Object *\
_make_##SERVICE_CLASS (ACE_Service_Object_Exterminator *gobbler) \
{ \
  ACE_TRACE (#SERVICE_CLASS); \
  if (gobbler != 0) \
    *gobbler = (ACE_Service_Object_Exterminator) _gobble_##SERVICE_CLASS; \
  return new SERVICE_CLASS; \
}

/// The canonical name for a service factory method
#define ACE_SVC_NAME(SERVICE_CLASS) _make_##SERVICE_CLASS

/// The canonical way to invoke (i.e. construct) a service factory
/// method.
#define ACE_SVC_INVOKE(SERVICE_CLASS) _make_##SERVICE_CLASS (0)

//@}

/** @name Helper macros for services defined in the netsvcs library.
 *
 * The ACE services defined in netsvcs use this helper macros for
 * simplicity.
 *
 */
//@{
# define ACE_SVC_FACTORY_DECLARE(X) ACE_FACTORY_DECLARE (ACE_Svc, X)
# define ACE_SVC_FACTORY_DEFINE(X) ACE_FACTORY_DEFINE (ACE_Svc, X)
//@}

#if defined (ACE_WIN32)
// These are used in SPIPE_Acceptor/Connector, but are ignored at runtime.
#   if defined (ACE_HAS_WINCE)
#     if !defined (PIPE_TYPE_MESSAGE)
#       define PIPE_TYPE_MESSAGE  0
#     endif
#     if !defined (PIPE_READMODE_MESSAGE)
#       define PIPE_READMODE_MESSAGE  0
#     endif
#     if !defined (PIPE_WAIT)
#       define PIPE_WAIT  0
#     endif
#   endif /* ACE_HAS_WINCE */
#else /* !ACE_WIN32 */
// Add some typedefs and macros to enhance Win32 conformance...
#   if !defined (LPSECURITY_ATTRIBUTES)
#     define LPSECURITY_ATTRIBUTES int
#   endif /* !defined LPSECURITY_ATTRIBUTES */
#   if !defined (GENERIC_READ)
#     define GENERIC_READ 0
#   endif /* !defined GENERIC_READ */
#   if !defined (FILE_SHARE_READ)
#     define FILE_SHARE_READ 0
#   endif /* !defined FILE_SHARE_READ */
#   if !defined (OPEN_EXISTING)
#     define OPEN_EXISTING 0
#   endif /* !defined OPEN_EXISTING */
#   if !defined (FILE_ATTRIBUTE_NORMAL)
#     define FILE_ATTRIBUTE_NORMAL 0
#   endif /* !defined FILE_ATTRIBUTE_NORMAL */
#   if !defined (MAXIMUM_WAIT_OBJECTS)
#     define MAXIMUM_WAIT_OBJECTS 0
#   endif /* !defined MAXIMUM_WAIT_OBJECTS */
#   if !defined (FILE_FLAG_OVERLAPPED)
#     define FILE_FLAG_OVERLAPPED 0
#   endif /* !defined FILE_FLAG_OVERLAPPED */
#   if !defined (FILE_FLAG_SEQUENTIAL_SCAN)
#     define FILE_FLAG_SEQUENTIAL_SCAN 0
#   endif   /* FILE_FLAG_SEQUENTIAL_SCAN */
#   if !defined(FILE_FLAG_WRITE_THROUGH)
#     define FILE_FLAG_WRITE_THROUGH 0
#   endif /* !defined FILE_FLAG_WRITE_THROUGH */
#   if !defined(PIPE_WAIT)
#     define PIPE_WAIT 0
#   endif /* !defined PIPE_WAIT */
#   if !defined(PIPE_NOWAIT)
#     define PIPE_NOWAIT 0
#   endif /* !defined PIPE_WAIT */
#   if !defined(PIPE_READMODE_BYTE)
#     define PIPE_READMODE_BYTE 0
#   endif /* !defined PIPE_READMODE_BYTE */
#   if !defined(PIPE_READMODE_MESSAGE)
#     define PIPE_READMODE_MESSAGE 0
#   endif /* !defined PIPE_READMODE_MESSAGE */
#   if !defined(PIPE_TYPE_BYTE)
#     define PIPE_TYPE_BYTE 0
#   endif /* !defined PIPE_TYPE_BYTE */
#   if !defined(PIPE_TYPE_MESSAGE)
#     define PIPE_TYPE_MESSAGE 0
#   endif /* !defined PIPE_TYPE_MESSAGE */
#endif /* ACE_WIN32 */


// Some useful abstrations for expressions involving
// ACE_Allocator.malloc ().  The difference between ACE_NEW_MALLOC*
// with ACE_ALLOCATOR* is that they call constructors also.

#include "ace/OS_Errno.h"    /* Need errno and ENOMEM */

# define ACE_ALLOCATOR_RETURN(POINTER,ALLOCATOR,RET_VAL) \
   do { POINTER = ALLOCATOR; \
     if (POINTER == 0) { errno = ENOMEM; return RET_VAL; } \
   } while (0)
# define ACE_ALLOCATOR(POINTER,ALLOCATOR) \
   do { POINTER = ALLOCATOR; \
     if (POINTER == 0) { errno = ENOMEM; return; } \
   } while (0)
# define ACE_ALLOCATOR_NORETURN(POINTER,ALLOCATOR) \
   do { POINTER = ALLOCATOR; \
     if (POINTER == 0) { errno = ENOMEM; } \
   } while (0)

# define ACE_NEW_MALLOC_RETURN(POINTER,ALLOCATOR,CONSTRUCTOR,RET_VAL) \
   do { POINTER = ALLOCATOR; \
     if (POINTER == 0) { errno = ENOMEM; return RET_VAL;} \
     else { new (POINTER) CONSTRUCTOR; } \
   } while (0)
# define ACE_NEW_MALLOC(POINTER,ALLOCATOR,CONSTRUCTOR) \
   do { POINTER = ALLOCATOR; \
     if (POINTER == 0) { errno = ENOMEM; return;} \
     else { new (POINTER) CONSTRUCTOR; } \
   } while (0)
# define ACE_NEW_MALLOC_NORETURN(POINTER,ALLOCATOR,CONSTRUCTOR) \
   do { POINTER = ALLOCATOR; \
     if (POINTER == 0) { errno = ENOMEM;} \
     else { new (POINTER) CONSTRUCTOR; } \
   } while (0)

/* ACE_Metrics */
#if defined ACE_LACKS_ARRAY_PLACEMENT_NEW
# define ACE_NEW_MALLOC_ARRAY_RETURN(POINTER,ALLOCATOR,CONSTRUCTOR,COUNT,RET_VAL) \
   do { POINTER = ALLOCATOR; \
     if (POINTER == 0) { errno = ENOMEM; return RET_VAL;} \
     else { for (u_int i = 0; i < COUNT; ++i) \
              {new (POINTER) CONSTRUCTOR; ++POINTER;} \
            POINTER -= COUNT;} \
   } while (0)
# define ACE_NEW_MALLOC_ARRAY(POINTER,ALLOCATOR,CONSTRUCTOR,COUNT) \
   do { POINTER = ALLOCATOR; \
     if (POINTER == 0) { errno = ENOMEM; return;} \
     else { for (u_int i = 0; i < COUNT; ++i) \
              {new (POINTER) CONSTRUCTOR; ++POINTER;} \
            POINTER -= COUNT;} \
   } while (0)
#else /* ! defined ACE_LACKS_ARRAY_PLACEMENT_NEW */
# define ACE_NEW_MALLOC_ARRAY_RETURN(POINTER,ALLOCATOR,CONSTRUCTOR,COUNT,RET_VAL) \
   do { POINTER = ALLOCATOR; \
     if (POINTER == 0) { errno = ENOMEM; return RET_VAL;} \
     else { new (POINTER) CONSTRUCTOR [COUNT]; } \
   } while (0)
# define ACE_NEW_MALLOC_ARRAY(POINTER,ALLOCATOR,CONSTRUCTOR,COUNT) \
   do { POINTER = ALLOCATOR; \
     if (POINTER == 0) { errno = ENOMEM; return;} \
     else { new (POINTER) CONSTRUCTOR [COUNT]; } \
   } while (0)
#endif /* defined ACE_LACKS_ARRAY_PLACEMENT_NEW */

// This is being placed here temporarily to help stablelize the builds, but will
// be moved out along with the above macros as part of the subsetting.  dhinton
# if !defined (ACE_HAS_WINCE)
#   if !defined (ACE_LACKS_NEW_H)
#     if defined (ACE_USES_STD_NAMESPACE_FOR_STDCPP_LIB)
#       include /**/ <new>
#     else
#       include /**/ <new.h>
#     endif /* ACE_USES_STD_NAMESPACE_FOR_STDCPP_LIB */
#   endif /* ! ACE_LACKS_NEW_H */
# endif /* !ACE_HAS_WINCE */

# define ACE_NOOP(x)

#if defined (ACE_PSOS)
#   define ACE_SEH_TRY if (1)
#   define ACE_SEH_EXCEPT(X) while (0)
#   define ACE_SEH_FINALLY if (1)
#elif defined (ACE_WIN32)
#   if !defined (ACE_HAS_WIN32_STRUCTURAL_EXCEPTIONS)
#     define ACE_SEH_TRY if (1)
#     define ACE_SEH_EXCEPT(X) while (0)
#     define ACE_SEH_FINALLY if (1)
#   elif defined(__BORLANDC__)
#     if (__BORLANDC__ >= 0x0530) /* Borland C++ Builder 3.0 */
#       define ACE_SEH_TRY try
#       define ACE_SEH_EXCEPT(X) __except(X)
#       define ACE_SEH_FINALLY __finally
#     else
#       define ACE_SEH_TRY if (1)
#       define ACE_SEH_EXCEPT(X) while (0)
#       define ACE_SEH_FINALLY if (1)
#     endif
#   elif defined (__IBMCPP__) && (__IBMCPP__ >= 400)
#     define ACE_SEH_TRY if (1)
#     define ACE_SEH_EXCEPT(X) while (0)
#     define ACE_SEH_FINALLY if (1)
#   else
#     define ACE_SEH_TRY __try
#     define ACE_SEH_EXCEPT(X) __except(X)
#     define ACE_SEH_FINALLY __finally
#   endif /* ACE_HAS_WIN32_STRUCTURAL_EXCEPTIONS */
# else /* !ACE_WIN32 && ACE_PSOS */
#   define ACE_SEH_TRY if (1)
#   define ACE_SEH_EXCEPT(X) while (0)
#   define ACE_SEH_FINALLY if (1)
#endif /* ACE_WIN32 && ACE_PSOS */


// These should probably be put into a seperate header.

// The following is necessary since many C++ compilers don't support
// typedef'd types inside of classes used as formal template
// arguments... ;-(.  Luckily, using the C++ preprocessor I can hide
// most of this nastiness!

# if defined (ACE_HAS_TEMPLATE_TYPEDEFS)

// Handle ACE_Message_Queue.
#   define ACE_SYNCH_DECL class _ACE_SYNCH
#   define ACE_SYNCH_USE _ACE_SYNCH
#   define ACE_SYNCH_MUTEX_T ACE_TYPENAME _ACE_SYNCH::MUTEX
#   define ACE_SYNCH_CONDITION_T ACE_TYPENAME _ACE_SYNCH::CONDITION
#   define ACE_SYNCH_SEMAPHORE_T ACE_TYPENAME _ACE_SYNCH::SEMAPHORE

// Handle ACE_Malloc*
#   define ACE_MEM_POOL_1 class _ACE_MEM_POOL
#   define ACE_MEM_POOL_2 _ACE_MEM_POOL
#   define ACE_MEM_POOL _ACE_MEM_POOL
#   define ACE_MEM_POOL_OPTIONS ACE_TYPENAME _ACE_MEM_POOL::OPTIONS

// Handle ACE_Svc_Handler
#   define ACE_PEER_STREAM_1 class _ACE_PEER_STREAM
#   define ACE_PEER_STREAM_2 _ACE_PEER_STREAM
#   define ACE_PEER_STREAM _ACE_PEER_STREAM
#   define ACE_PEER_STREAM_ADDR ACE_TYPENAME _ACE_PEER_STREAM::PEER_ADDR

// Handle ACE_Acceptor
#   define ACE_PEER_ACCEPTOR_1 class _ACE_PEER_ACCEPTOR
#   define ACE_PEER_ACCEPTOR_2 _ACE_PEER_ACCEPTOR
#   define ACE_PEER_ACCEPTOR _ACE_PEER_ACCEPTOR
#   define ACE_PEER_ACCEPTOR_ADDR ACE_TYPENAME _ACE_PEER_ACCEPTOR::PEER_ADDR

// Handle ACE_Connector
#   define ACE_PEER_CONNECTOR_1 class _ACE_PEER_CONNECTOR
#   define ACE_PEER_CONNECTOR_2 _ACE_PEER_CONNECTOR
#   define ACE_PEER_CONNECTOR _ACE_PEER_CONNECTOR
#   define ACE_PEER_CONNECTOR_ADDR ACE_TYPENAME _ACE_PEER_CONNECTOR::PEER_ADDR
#   if !defined(ACE_HAS_TYPENAME_KEYWORD)
#     define ACE_PEER_CONNECTOR_ADDR_ANY ACE_PEER_CONNECTOR_ADDR::sap_any
#   else
    //
    // If the compiler supports 'typename' we cannot use
    //
    // PEER_CONNECTOR::PEER_ADDR::sap_any
    //
    // because PEER_CONNECTOR::PEER_ADDR is not considered a type. But:
    //
    // typename PEER_CONNECTOR::PEER_ADDR::sap_any
    //
    // will not work either, because now we are declaring sap_any a
    // type, further:
    //
    // (typename PEER_CONNECTOR::PEER_ADDR)::sap_any
    //
    // is considered a casting expression. All I can think of is using a
    // typedef, I tried PEER_ADDR but that was a source of trouble on
    // some platforms. I will try:
    //
#     define ACE_PEER_CONNECTOR_ADDR_ANY ACE_PEER_ADDR_TYPEDEF::sap_any
#   endif /* ACE_HAS_TYPENAME_KEYWORD */

// Handle ACE_SOCK_*
#   define ACE_SOCK_ACCEPTOR ACE_SOCK_Acceptor
#   define ACE_SOCK_CONNECTOR ACE_SOCK_Connector
#   define ACE_SOCK_STREAM ACE_SOCK_Stream

// Handle ACE_SOCK_SEQPACK_*
#   define ACE_SOCK_SEQPACK_ACCEPTOR ACE_SOCK_SEQPACK_Acceptor
#   define ACE_SOCK_SEQPACK_CONNECTOR ACE_SOCK_SEQPACK_Connector
#   define ACE_SOCK_SEQPACK_ASSOCIATION ACE_SOCK_SEQPACK_Association

// Handle ACE_MEM_*
#   define ACE_MEM_ACCEPTOR ACE_MEM_Acceptor
#   define ACE_MEM_CONNECTOR ACE_MEM_Connector
#   define ACE_MEM_STREAM ACE_MEM_Stream

// Handle ACE_LSOCK_*
#   define ACE_LSOCK_ACCEPTOR ACE_LSOCK_Acceptor
#   define ACE_LSOCK_CONNECTOR ACE_LSOCK_Connector
#   define ACE_LSOCK_STREAM ACE_LSOCK_Stream

// Handle ACE_TLI_*
#   define ACE_TLI_ACCEPTOR ACE_TLI_Acceptor
#   define ACE_TLI_CONNECTOR ACE_TLI_Connector
#   define ACE_TLI_STREAM ACE_TLI_Stream

// Handle ACE_SPIPE_*
#   define ACE_SPIPE_ACCEPTOR ACE_SPIPE_Acceptor
#   define ACE_SPIPE_CONNECTOR ACE_SPIPE_Connector
#   define ACE_SPIPE_STREAM ACE_SPIPE_Stream

// Handle ACE_UPIPE_*
#   define ACE_UPIPE_ACCEPTOR ACE_UPIPE_Acceptor
#   define ACE_UPIPE_CONNECTOR ACE_UPIPE_Connector
#   define ACE_UPIPE_STREAM ACE_UPIPE_Stream

// Handle ACE_FILE_*
#   define ACE_FILE_CONNECTOR ACE_FILE_Connector
#   define ACE_FILE_STREAM ACE_FILE_IO

// Handle ACE_*_Memory_Pool.
#   define ACE_MMAP_MEMORY_POOL ACE_MMAP_Memory_Pool
#   define ACE_LITE_MMAP_MEMORY_POOL ACE_Lite_MMAP_Memory_Pool
#   define ACE_SBRK_MEMORY_POOL ACE_Sbrk_Memory_Pool
#   define ACE_SHARED_MEMORY_POOL ACE_Shared_Memory_Pool
#   define ACE_LOCAL_MEMORY_POOL ACE_Local_Memory_Pool
#   define ACE_PAGEFILE_MEMORY_POOL ACE_Pagefile_Memory_Pool

# else /* TEMPLATES are broken in some form or another (i.e., most C++ compilers) */

// Handle ACE_Message_Queue.
#   if defined (ACE_HAS_OPTIMIZED_MESSAGE_QUEUE)
#     define ACE_SYNCH_DECL class _ACE_SYNCH_MUTEX_T, class _ACE_SYNCH_CONDITION_T, class _ACE_SYNCH_SEMAPHORE_T
#     define ACE_SYNCH_USE _ACE_SYNCH_MUTEX_T, _ACE_SYNCH_CONDITION_T, _ACE_SYNCH_SEMAPHORE_T
#   else
#     define ACE_SYNCH_DECL class _ACE_SYNCH_MUTEX_T, class _ACE_SYNCH_CONDITION_T
#     define ACE_SYNCH_USE _ACE_SYNCH_MUTEX_T, _ACE_SYNCH_CONDITION_T
#   endif /* ACE_HAS_OPTIMIZED_MESSAGE_QUEUE */
#   define ACE_SYNCH_MUTEX_T _ACE_SYNCH_MUTEX_T
#   define ACE_SYNCH_CONDITION_T _ACE_SYNCH_CONDITION_T
#   define ACE_SYNCH_SEMAPHORE_T _ACE_SYNCH_SEMAPHORE_T

// Handle ACE_Malloc*
#   define ACE_MEM_POOL_1 class _ACE_MEM_POOL, class _ACE_MEM_POOL_OPTIONS
#   define ACE_MEM_POOL_2 _ACE_MEM_POOL, _ACE_MEM_POOL_OPTIONS
#   define ACE_MEM_POOL _ACE_MEM_POOL
#   define ACE_MEM_POOL_OPTIONS _ACE_MEM_POOL_OPTIONS

// Handle ACE_Svc_Handler
#   define ACE_PEER_STREAM_1 class _ACE_PEER_STREAM, class _ACE_PEER_ADDR
#   define ACE_PEER_STREAM_2 _ACE_PEER_STREAM, _ACE_PEER_ADDR
#   define ACE_PEER_STREAM _ACE_PEER_STREAM
#   define ACE_PEER_STREAM_ADDR _ACE_PEER_ADDR

// Handle ACE_Acceptor
#   define ACE_PEER_ACCEPTOR_1 class _ACE_PEER_ACCEPTOR, class _ACE_PEER_ADDR
#   define ACE_PEER_ACCEPTOR_2 _ACE_PEER_ACCEPTOR, _ACE_PEER_ADDR
#   define ACE_PEER_ACCEPTOR _ACE_PEER_ACCEPTOR
#   define ACE_PEER_ACCEPTOR_ADDR _ACE_PEER_ADDR

// Handle ACE_Connector
#   define ACE_PEER_CONNECTOR_1 class _ACE_PEER_CONNECTOR, class _ACE_PEER_ADDR
#   define ACE_PEER_CONNECTOR_2 _ACE_PEER_CONNECTOR, _ACE_PEER_ADDR
#   define ACE_PEER_CONNECTOR _ACE_PEER_CONNECTOR
#   define ACE_PEER_CONNECTOR_ADDR _ACE_PEER_ADDR
#   define ACE_PEER_CONNECTOR_ADDR_ANY ACE_PEER_CONNECTOR_ADDR::sap_any

// Handle ACE_SOCK_*
#   define ACE_SOCK_ACCEPTOR ACE_SOCK_Acceptor, ACE_INET_Addr
#   define ACE_SOCK_CONNECTOR ACE_SOCK_Connector, ACE_INET_Addr
#   define ACE_SOCK_STREAM ACE_SOCK_Stream, ACE_INET_Addr

// Handle ACE_SOCK_SEQPACK_*
#   define ACE_SOCK_SEQPACK_ACCEPTOR ACE_SOCK_SEQPACK_Acceptor, ACE_Multihomed_INET_Addr
#   define ACE_SOCK_SEQPACK_CONNECTOR ACE_SOCK_SEQPACK_Connector, ACE_Multihomed_INET_Addr
#   define ACE_SOCK_SEQPACK_ASSOCIATION ACE_SOCK_SEQPACK_Association, ACE_Multihomed_INET_Addr

// Handle ACE_MEM_*
#   define ACE_MEM_ACCEPTOR ACE_MEM_Acceptor, ACE_MEM_Addr
#   define ACE_MEM_CONNECTOR ACE_MEM_Connector, ACE_INET_Addr
#   define ACE_MEM_STREAM ACE_MEM_Stream, ACE_INET_Addr

// Handle ACE_LSOCK_*
#   define ACE_LSOCK_ACCEPTOR ACE_LSOCK_Acceptor, ACE_UNIX_Addr
#   define ACE_LSOCK_CONNECTOR ACE_LSOCK_Connector, ACE_UNIX_Addr
#   define ACE_LSOCK_STREAM ACE_LSOCK_Stream, ACE_UNIX_Addr

// Handle ACE_TLI_*
#   define ACE_TLI_ACCEPTOR ACE_TLI_Acceptor, ACE_INET_Addr
#   define ACE_TLI_CONNECTOR ACE_TLI_Connector, ACE_INET_Addr
#   define ACE_TLI_STREAM ACE_TLI_Stream, ACE_INET_Addr

// Handle ACE_SPIPE_*
#   define ACE_SPIPE_ACCEPTOR ACE_SPIPE_Acceptor, ACE_SPIPE_Addr
#   define ACE_SPIPE_CONNECTOR ACE_SPIPE_Connector, ACE_SPIPE_Addr
#   define ACE_SPIPE_STREAM ACE_SPIPE_Stream, ACE_SPIPE_Addr

// Handle ACE_UPIPE_*
#   define ACE_UPIPE_ACCEPTOR ACE_UPIPE_Acceptor, ACE_SPIPE_Addr
#   define ACE_UPIPE_CONNECTOR ACE_UPIPE_Connector, ACE_SPIPE_Addr
#   define ACE_UPIPE_STREAM ACE_UPIPE_Stream, ACE_SPIPE_Addr

// Handle ACE_FILE_*
#   define ACE_FILE_CONNECTOR ACE_FILE_Connector, ACE_FILE_Addr
#   define ACE_FILE_STREAM ACE_FILE_IO, ACE_FILE_Addr

// Handle ACE_*_Memory_Pool.
#   define ACE_MMAP_MEMORY_POOL ACE_MMAP_Memory_Pool, ACE_MMAP_Memory_Pool_Options
#   define ACE_LITE_MMAP_MEMORY_POOL ACE_Lite_MMAP_Memory_Pool, ACE_MMAP_Memory_Pool_Options
#   define ACE_SBRK_MEMORY_POOL ACE_Sbrk_Memory_Pool, ACE_Sbrk_Memory_Pool_Options
#   define ACE_SHARED_MEMORY_POOL ACE_Shared_Memory_Pool, ACE_Shared_Memory_Pool_Options
#   define ACE_LOCAL_MEMORY_POOL ACE_Local_Memory_Pool, ACE_Local_Memory_Pool_Options
#   define ACE_PAGEFILE_MEMORY_POOL ACE_Pagefile_Memory_Pool, ACE_Pagefile_Memory_Pool_Options
# endif /* ACE_HAS_TEMPLATE_TYPEDEFS */

#include /**/ "ace/post.h"

#endif /*ACE_GLOBAL_MACROS_H*/
