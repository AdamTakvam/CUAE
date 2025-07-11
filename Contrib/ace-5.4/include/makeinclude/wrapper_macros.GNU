# -*- Makefile -*-

#----------------------------------------------------------------------------
#  wrapper_macros.GNU,v 4.159 2004/01/12 23:40:47 dhinton Exp
#----------------------------------------------------------------------------
#
#  ACE supports the following flags.  They can be enabled either on
#  the command line, e.g., "make purify=1", or added to your
#  platform_macros.GNU.  To disable the option, set the flag to null,
#  e.g., "make debug=".  Some flags support setting to 0 disable,
#  e.g., "make debug=0".  debug=1 is enabled in the platform files
#  that are released with ACE.
#
#  Please note that the effects of a flag may be platform specific.
#  Also, combinations of certain flags may or may not be allowed on
#  specific platforms, e.g., debug=1 opt=1 is supported by g++ but
#  not all other C++ compilers.
#
#  Flag             Description
#  ----             -----------
#  debug            Enable debugging; see DCFLAGS and DCCFLAGS.
#  exceptions       Enable exception handling (not supported by all platforms).
#  include_env      Enable usage of CORBA::Environment parameters that is
#                   compatible with TAO versions before 1.2.2.  Default:
#                   disabled.  Has no effect when building with exceptions=0
#  minimum_corba    Enable minimumCORBA support.  (Documented here, but
#                   implemented in TAO/rules.TAO.GNU.)
#  fast             Enable -fast option, e.g., with Sun C++.
#  inline           Enable ACE inlining.  Some platforms enable inlining by
#                   default, others do not. If platform_macros.GNU does not
#                   set this, inline is enabled.
#  templates	    Type of template instantiation to perform. Can be one of
# 		    automatic, used, explicit. Defaults to automatic if the
#                   platform/compiler combination supports it, otherwise is
#                   set to explicit.
#  optimize         Enable optimization; see OCFLAGS and OCCFLAGS.
#  probe            Enable ACE_Timeprobes.
#  profile          Enable profiling; see PCFLAGS and PCCFLAGS.
#  purify           Purify all executables.  purify must be in user's PATH!
#  quantify         Quantify all executables.  quantify must be in user's PATH!
#  repo             Use GNU template repository (g++ with repo patches and
#                   egcs only).
#  rtti             Enable run-time type identification.  On some platforms,
#                   it is enabled by default, so this is ignored.
#  shared_libs      Build shared libraries. Ignored if static_libs_only is set.
#  static_libs      Build static libraries. Ignored if shared_libs_only is set.
#  shared_libs_only Only build shared libraries.  Ignored if no SHLIBs are
#                   specified by the Makefile, as in performance-tests/Misc.
#  static_libs_only Only build static libraries.
#  static_link      Link only static libs to executables, e.g., uses "-static"
#                   flag with the GNU ld linker.
#  threads          Build with thread support.
#  xt_reactor       Build the XtReactor.
#  fl_reactor       Build the FlReactor.
#  tk_reactor       Build the TkReactor.
#  qt_reactor       Build the QtReactor.
#  gtk_reactor      Build the GtkReactor.
#  ssl              Build with OpenSSL support.
#  rapi             Build with RAPI
#  rwho             Build with rwho, this results in building apps/drwho
#  pipes            Build with pipe support, resulting in adding -pipe to the
#                   compiler options.
#  split            Build the library by first splitting up the ACE source
#                   to several files, with one object code entity for each
#                   source file. This allows an application that is linked
#                   with ACE to extract _exactly_ what it needs from the
#                   library, resulting in a smaller executable.
#                   Setting this to 1 overrides debug to 0.
#  sctp             Build support for TAO SCIOP and for ACE SCTP
#                   wrapper-facades. The sctp macro should be set to a string
#                   value representing a particular SCTP implementation.
#                   Recognized values include: openss7
#
#  versioned_so     Add versioning to libraries.  Defaults to 1 (true). If 0,
#                   no version number is appended to shared library names.
#                   If versioned_so is 1 (enabled), the SOVERSION make
#                   variable can also be set to specify the version info
#                   to append to the name - it should have a leading '.' -
#                   else it defaults to .ACE_MAJOR.ACE_MINOR.ACE_BETA.
#  wfmo             Build with wfmo support (Win32 only)
#
#  Usually, users do not need to be concerned with make targets.
#  Just enter "make" on the command line to build.  A few notable
#  targets are listed below.
#
#  Target             Description
#  ------             -----------
#  show_statics       Lists all static objects in object files built for
#                       current directory.  Only supported for g++.
#  show_uninit        Lists all uninitialized in object files built for
#                       current directory.  Only supported for g++.
#
#  The following describes the ACE wrapper macros:
#
#  Variable     Description
#  --------     -----------
#  ACE_HAS_GNUG_PRE_2_8 CXX
#               includes "g++", and the g++ version is prior to 2.8.0.  This is
#               automatically determined below, but can be overridden in the
#               user's platform_macros.GNU or environment.  Three values are
#               supported: 1 for g++ < 2.8.0, 0 for g++ >= 2.8.0 or egcs,
#               undefined for non-g++ compilers.
#  ARFLAGS      Flags for the archive utility (ar)
#  CC           C compiler command
#  CXX          C++ compiler command
#  RC           Resource compiler command
#  COMPILE.c    Flags for compiling with C
#  COMPILE.cc   Flags for compiling with C++
#  COMPILE.rc   Flags for compiling with the resource compiler
#  CPPFLAGS     C pre-processor flags
#  CFLAGS       C compilation flags
#  CCFLAGS      C++ compilation flags
#  DCFLAGS      C compilation flags for debugging
#  DCCFLAGS     C++ compilation flags for debugging
#  DEFFLAGS     C++ preprocessor flag for defining symbols
#  DLD          Name of dynamic linker
#  LD           Name of linker
#  IDL          Name of the CORBA IDL compiler
#  TEMPLATES_FLAG
#               Flag to pass to the compiler to control the instantiation of
# 		templates.
#  INSBIN       Binary (executable) installation directory
#  INSINC       Include file installation directory
#  INSMAN       Manpage installation directory
#  INSLIB       Library installation directory
#  LDFLAGS      ld linker flags
#  LINK.c       Flags for linking with C
#  LINK.cc      Flags for linking with C++
#  MAKEFLAGS    Flags that are passed into the compilation from the commandline
#  OCFLAGS      Optimizing C compilation flags
#  OCCFLAGS     Optimizing C++ compilation flags
#  PCFLAGS      C compilation flags for profiling
#  PCCFLAGS     C++ compilation flags for profiling
#  PLATFORM_XT_CPPFLAGS
#               Platform CPP options for X11 (some require -I...)
#  PLATFORM_XT_LDFLAGS
#               Platform LD options for X11 (some require -L...)
#  PLATFORM_XT_LIBS
#               Platform libraries require with X11
#  PLATFORM_SSL_CPPFLAGS
#               Platform CPP options for OpenSSL (may need -I...)
#  PLATFORM_SSL_LDFLAGS
#               Platform LD options for OpenSSL (may need -L...)
#  PLATFORM_SSL_LIBS
#               Platform libraries required with OpenSSL
#  PLATFORM_RAPI_CPPFLAGS
#               Platform CPP options for RAPI
#  PLATFORM_RAPI_LDFLAGS
#               Platform LD options for RAPI
#  PLATFORM_RAPI_LIBS
#               Platform libraries required with RAPI
#  PLATFORM_SCTP_CPPFLAGS
#               Platform CPP options required for SCTP
#  PLATFORM_SCTP_LDFLAGS
#               Platform LD options required for SCTP
#  PLATFORM_SCTP_LIBS
#               Platform libraries required for SCTP
#  PRELINK      Executable to precede linking, such as quantify
#  PURELINK     If set, contains purify executable as well as all options
#  PURE_CACHE_BASE_DIR
#               The base directory in which Pure products will produce
#               instrumented libraries.  If you don't want that to be below
#               /tmp, set this variable in your environment or on the make
#               command line.
#  PURE_CACHE_DIR
#               The full name of the directory in which Pure produces will
#               produce instrumented libraries.
#  PTDIRS       Pathnames of directories containing template code
#  RM           Name of program to use to remove files
#  SOFLAGS      Flags used to build a shared library
#  SOLINK.cc    Link line necessary to build a share library
#  VAR          Variant identifier suffix
#  VDIR         Directory for object code
#  VSHDIR       Directory for shared object code
#  ACE_ROOT     Pathname for the root of the build tree
#
#----------------------------------------------------------------------------
#      Platform-dependent macro definitions
#      (link to the appropriate platform-specific config file).
#----------------------------------------------------------------------------

# Set the cleanup targets.  These targets are used by realclean even if
# the component isn't being built, i.e., the decision to build it was based
# on components built in another makefile and written a file (that might no
# longer be available) and accessed by $(ACE_ROOT)/bin/ace_components.
#
# If a component should always be built, just use the normal BIN, LIB, or
# SHLIB variables.  If it's dependent on a component, use the BIN2 or
# BIN_UNCHECKED variety, then assign that to BIN if the required component
# is available.
#
# If you want to include multiple targets in a single Makefile, you must
# create your own targets, e.g., server and client.  In this case, assign
# them to BIN, or BIN_UNCHECKED if they depend on other components, do not
# include rules.bin.GNU.  That way they will still get cleaned up.
BIN_UNCHECKED ?=
CLEANUP_BIN ?=
ifdef BIN
  CLEANUP_BIN = $(BIN)
endif # !BIN
ifdef BIN2
  CLEANUP_BIN += $(BIN2)
endif # !BIN2
ifneq ($(BIN_UNCHECKED),)
  CLEANUP_BIN += $(BIN_UNCHECKED)
endif # !BIN_UNCHECKED

LIB_UNCHECKED ?=
CLEANUP_LIB ?=
ifdef LIB
  CLEANUP_LIB = $(LIB)
endif # !LIB
ifdef LIB2
  CLEANUP_LIB += $(LIB2)
endif # !LIB2
ifneq ($(LIB_UNCHECKED),)
  CLEANUP_LIB = $(LIB_UNCHECKED)
endif # !LIB_UNCHECKED

SHLIB_UNCHECKED ?=
CLEANUP_SHLIB ?=
ifdef SHLIB
  CLEANUP_SHLIB = $(SHLIB)
endif # !SHLIB
ifdef SHLIB2
  CLEANUP_SHLIB += $(SHLIB2)
endif # SHLIB2
ifneq ($(SHLIB_UNCHECKED),)
  CLEANUP_SHLIB += $(SHLIB_UNCHECKED)
endif # ! SHLIB_UNCHECKED

SHLIBA_UNCHECKED ?=
CLEANUP_SHLIBA ?=
ifdef SHLIBA
  CLEANUP_SHLIBA = $(SHLIBA)
endif # SHLIBA
ifdef SHLIBA2
  CLEANUP_SHLIBA = $(SHLIBA2)
endif # SHLIB2A
ifneq ($(SHLIBA_UNCHECKED),)
  CLEANUP_SHLIBA = $(SHLIBA_UNCHECKED)
endif # SHLIBA_UNCHECKED

#### Default to building shared libraries only.  Set it here to give the
#### platform a chance to override.
ifndef shared_libs
  shared_libs = 1
endif # shared_libs
ifndef static_libs
  static_libs = 0
endif # static_libs

#### The system has X Windows and we want to include the XtReactor in
#### the library
xt_reactor = 0

#### Set up the build ACE version in case the platform_macros.GNU wants it.
ACE_MAJOR_VERSION := $(shell awk '/ACE_MAJOR_VERSION/ { print $$3}' ${ACE_ROOT}/ace/Version.h)
ACE_MINOR_VERSION := $(shell awk '/ACE_MINOR_VERSION/ { print $$3}' ${ACE_ROOT}/ace/Version.h)
ACE_BETA_VERSION := $(shell awk '/ACE_BETA_VERSION/ { print $$3}' ${ACE_ROOT}/ace/Version.h)

PACE_ROOT ?= $(ACE_ROOT)/PACE

# Define some variables to silence warnings
SHR_FILTER ?=

# Include this before the below variables to make it possible for
# platform_macros.GNU to set default values for them.
include $(ACE_ROOT)/include/makeinclude/platform_macros.GNU

# Define some variables to silence warnings
GHS ?=
CHORUS ?=
AIX_TEMPLATE_HACK ?=

# $(ACE_PLATFORM_CONFIG) is used in dependency rules and corresponds to the
# platform dependent config-*.h file included in config.h, and can be set
# in your platform_*.GNU file or platform_macros.GNU.
ACE_PLATFORM_CONFIG ?= config.h

ifeq ($(threads),0)
  ifneq ($(findstring -DACE_MT_SAFE=0,$(CPPFLAGS)),-DACE_MT_SAFE=0)
    CPPFLAGS += -DACE_MT_SAFE=0
  endif # -DACE_MT_SAFE=0
endif # threads

####
#### Defaults are exceptions off (0), fast off (0), rtti off (0), and
#### versioned_so on (1).
####
exceptions ?= 0

fast ?= 0

# @@@ Should this really be the default?  Or should we let the
# compiler choose?
rtti ?= 0

ifeq (,$(versioned_so))
  versioned_so = 1
endif
SOVERSION ?=
SONAME ?=

#----------------------------------------------------------------------------
#      Platform-independent macro definitions
#----------------------------------------------------------------------------

INCLDIRS ?=
DEFFLAGS ?=

CPPFLAGS        += $(DEFFLAGS) $(INCLDIRS)

# Define default extentions for IDL-generated files
IDL_CLIENT_HDR_EXT ?= C.h
IDL_CLIENT_INL_EXT ?= C.i
IDL_CLIENT_SRC_EXT ?= C.cpp
IDL_SERVER_HDR_EXT ?= S.h
IDL_SERVER_INL_EXT ?= S.i
IDL_SERVER_SRC_EXT ?= S.cpp
IDL_SERVER_THDR_EXT ?= S_T.h
IDL_SERVER_TINL_EXT ?= S_T.i
IDL_SERVER_TSRC_EXT ?= S_T.cpp

ifeq (,$(findstring -I$(ACE_ROOT),$(INCLDIRS)))
  INCLDIRS        += -I$(ACE_ROOT)
endif
INSBIN          ?= $(ACE_ROOT)/bin
INSINC          ?= $(ACE_ROOT)/ace
INSLIB          ?= $(ACE_ROOT)/ace
INSMAN          ?= $(ACE_ROOT)/man

ifeq (,$(findstring -L$(ACE_ROOT)/ace,$(LDFLAGS)))
  LDFLAGS         += -L$(ACE_ROOT)/ace -L./
endif
LEX             ?= flex

EXEEXT ?=

ifndef COMSPEC
  ifdef ComSpec
    #### ACE+TAO use COMSPEC, but ComSpec is defined.
    COMSPEC = $(ComSpec)
  endif # ComSpec
endif # ! COMPSPEC

ifdef COMSPEC
  #### Assume we're on a WIN32 host.
  ACE_NUL = nul
else  # ! WIN32
  ACE_NUL = /dev/null
endif # ! WIN32

#### Redirect stderr to nul device, and mask exit status.
ACE_NUL_STDERR = 2>$(ACE_NUL) || true
PCFLAGS         += #### set in platform_macros.GNU file, e.g., -p
PCCFLAGS        += #### set in platform_macros.GNU file, e.g., -p
VAR             =

# Let users override the default VDIR directories
VDIR          ?= .obj/
VSHDIR        ?= .shobj/

ACELIB ?= -lACE

# NOTE: VLDLIBS is defined below, in case it is overridden for static-
#       or shared-only builds.
YACC          ?= bison



#----------------------------------------------------------------------------
#      Platform-dependent macros that require platform_macros.GNU.
#----------------------------------------------------------------------------

ifeq ($(findstring g++,$(CXX)),g++)
  ifndef ACE_HAS_GNUG_PRE_2_8
      ACE_HAS_GNUG_PRE_2_8 := \
        $(shell \
          if $(CXX) -dumpversion 2>/dev/null | egrep '^(cygnus-)?2\.[0-7]' \
	  >/dev/null; then echo 1; else echo 0; fi)
  endif # ! ACE_HAS_GNUG_PRE_2_8
endif # g++

OBJEXT ?= o
SOEXT ?= so
LIBEXT ?= a

CC_OUTPUT_FLAG ?= -o
SO_OUTPUT_FLAG ?= -o
LINK_OUTPUT_FLAG ?= -o

#----------------------------------------------------------------------------
#      make flags
#----------------------------------------------------------------------------

ifeq ($(optimize),0)
  override optimize =
endif # optimize

ifneq ($(optimize),)
  CFLAGS  += $(OCFLAGS)
  CCFLAGS += $(OCCFLAGS)
endif # optimize

split ?= 0
ifeq ($(split),0)
  override split =
endif # split

ifneq ($(split),)
  override split_target = SPLIT
  override debug = 0
  CPPFLAGS += -DACE_IS_SPLITTING
else
  override split_target =
endif # split

ifeq ($(debug),0)
  override debug =
endif # debug

ifeq ($(debug),)
  CPPFLAGS += -DACE_NDEBUG
# Disable the RCSID for release/non-debug builds.
  ifeq (,$(findstring ACE_USE_RCSID,$(CPPFLAGS)))
    CPPFLAGS += -DACE_USE_RCSID=0
  endif
else
  CFLAGS  += $(DCFLAGS)
  CCFLAGS += $(DCCFLAGS)
endif # debug

ifeq ($(threads),0)
  override threads =
endif # threads

ifeq ($(xt_reactor),0)
  override xt_reactor =
endif # xt_reactor

ifneq ($(xt_reactor),)
  override ace_with_x11=1
  CPPFLAGS += -DACE_HAS_XT $(PLATFORM_XT_CPPFLAGS)
  LIBS += $(PLATFORM_XT_LIBS)
  LDFLAGS += $(PLATFORM_XT_LDFLAGS)
  ACE_MAKE_OPTIONS += xt_reactor
endif # xt_reactor

fl_reactor ?=
ifeq ($(fl_reactor),0)
  override fl_reactor =
endif # fl_reactor

ifneq ($(fl_reactor),)
  override ace_with_x11=1
  override ace_with_gl=1
  CPPFLAGS += -DACE_HAS_FL $(PLATFORM_FL_CPPFLAGS)
  LIBS += $(PLATFORM_FL_LIBS)
  LDFLAGS += $(PLATFORM_FL_LDFLAGS)
  ACE_MAKE_OPTIONS += rl_reactor
endif # fl_reactor

tk_reactor ?=
ifeq ($(tk_reactor),0)
  override tk_reactor =
endif # tk_reactor

ifneq ($(tk_reactor),)
  override ace_with_tk=1
  CPPFLAGS += -DACE_HAS_TK $(PLATFORM_TK_CPPFLAGS)
  LIBS += $(PLATFORM_TK_LIBS)
  LDFLAGS += $(PLATFORM_TK_LDFLAGS)
  ACE_MAKE_OPTIONS += tk_reactor
endif # tk_reactor

qt_reactor ?=
ifeq ($(qt_reactor),0)
  override qt_reactor =
endif # qt_reactor

ifneq ($(qt_reactor),)
  override ace_with_qt=1
  CPPFLAGS += -DACE_HAS_QT $(PLATFORM_QT_CPPFLAGS)
  LIBS += $(PLATFORM_QT_LIBS)
  LDFLAGS += $(PLATFORM_QT_LDFLAGS)
  ACE_MAKE_OPTIONS += qt_reactor
endif # qt_reactor

QTDIR ?=
ifneq ($(QTDIR),)
  MOC = ${QTDIR}/bin/moc
else
  MOC = moc
endif

gtk_reactor ?=
ifeq ($(gtk_reactor),0)
  override gtk_reactor =
endif # gtk_reactor

ifneq ($(gtk_reactor),)
  override ace_with_gtk=1
  CPPFLAGS += -DACE_HAS_GTK $(PLATFORM_GTK_CPPFLAGS)
  LIBS += $(PLATFORM_GTK_LIBS)
  LDFLAGS += $(PLATFORM_GTK_LDFLAGS)
  ACE_MAKE_OPTIONS += gtk_reactor
endif # gtk_reactor

rapi ?=
ifeq ($(rapi),0)
     override rapi =
endif #rapi

ifneq ($(rapi),)
  CPPFLAGS += -DACE_HAS_RAPI $(PLATFORM_RAPI_CPPFLAGS)
  LIBS += $(PLATFORM_RAPI_LIBS)
  LDFLAGS += $(PLATFORM_RAPI_LDFLAGS)
  ACE_MAKE_OPTIONS += rapi
endif # rapi

sctp ?=
ifeq ($(sctp),0)
     override sctp =
endif #rapi

# if any sctp implementation is present then add
ifneq ($(sctp),)
 CPPFLAGS += -DACE_HAS_SCTP $(PLATFORM_SCTP_CPPFLAGS)
 LDFLAGS  += $(PLATFORM_SCTP_LDFLAGS)
 LIBS     += $(PLATFORM_SCTP_LIBS)
  ACE_MAKE_OPTIONS += sctp
endif

ace_with_gl ?=
ifeq ($(ace_with_gl),0)
  override ace_with_gl =
endif # ace_with_gl

ifneq ($(ace_with_gl),)
  override ace_with_x11 = 1
  CPPFLAGS += $(PLATFORM_GL_CPPFLAGS)
  LIBS += $(PLATFORM_GL_LIBS)
  LDFLAGS += $(PLATFORM_GL_LDFLAGS)
  ACE_MAKE_OPTIONS += ace_with_gl
endif # ace_with_gl

ace_with_x11 ?=
ifeq ($(ace_with_x11),0)
  override ace_with_x11 =
endif # ace_with_x11

ifneq ($(ace_with_x11),)
  CPPFLAGS += $(PLATFORM_X11_CPPFLAGS)
  LIBS += $(PLATFORM_X11_LIBS)
  LDFLAGS += $(PLATFORM_X11_LDFLAGS)
  ACE_MAKE_OPTIONS += ace_with_x11
endif # ace_with_x11

ssl ?=
ifeq ($(ssl),0)
  override ssl =
else
  ACE_MAKE_OPTIONS += ssl
endif

ifeq ($(exceptions),0)
  ifeq ($(ACE_HAS_GNUG_PRE_2_8),0)
    #### Disable the default exception handling of g++ >= 2.8.0.
    ifneq ($(ACE_GNUG_ON_UNIXWARE),)
      CCFLAGS += -fcheck-new
    else
      CCFLAGS += -fno-exceptions -fcheck-new
    endif # ! ACE_GNUG_ON_UNIXWARE
  endif # ! ACE_HAS_GNUG_PRE_2_8
else  # exceptions
  CPPFLAGS += -DACE_HAS_EXCEPTIONS
  ifndef include_env
    include_env ?=
  endif
  ifeq ($(include_env),1)
    CPPFLAGS += -DACE_ENV_BKWD_COMPAT
  endif
  ifeq ($(ACE_HAS_GNUG_PRE_2_8),1)
    #### Enable exception handling with g++ < 2.8.0.  Not recommended,
    #### but provided here for completeness.
    CCFLAGS += -fhandle-exceptions
#  else  # ! ACE_HAS_GNUG_PRE_2_8
#  ifeq ($(ACE_HAS_GNUG_PRE_2_8),0)
#    #### Disable this warning on g++ >= 2.8.0 if exceptions are enabled,
#    #### it gets confused and reports too many bogus warnings.
#    CCFLAGS += -Wno-uninitialized
#  endif # g++ > 2.8.0
  endif # ! ACE_HAS_GNUG_PRE_2_8
  ACE_MAKE_OPTIONS += exceptions
endif # exceptions

profile ?=
ifeq ($(profile),0)
  override profile =
endif # profile

ifneq ($(profile),)
  CFLAGS  += $(PCFLAGS)
  CCFLAGS += $(PCCFLAGS)
endif # profile

inline ?= 1
ifeq ($(inline),0)
  CPPFLAGS += -DACE_NO_INLINE
else
ifeq ($(inline),1)
  CPPFLAGS += -D__ACE_INLINE__
endif # inline eq 1
endif # inline eq 0

ifeq ($(shared_libs),0)
  override shared_libs =
endif # shared_libs
ifeq ($(static_libs),0)
  override static_libs =
endif # static_libs

shared_libs_only ?=
ifeq ($(shared_libs_only),0)
  override shared_libs_only =
endif # shared_libs_only

static_libs_only ?=
ifeq ($(static_libs_only),0)
  override static_libs_only =
endif # static_libs_only

ifdef shared_libs_only
  override shared_libs = 1
  override static_libs =
endif # shared_libs_only
ifdef static_libs_only
  override shared_libs =
  override static_libs = 1
endif # static_libs_only

ifdef shared_libs
  ifndef static_libs
    override shared_libs_only = 1
  endif
  ACE_MAKE_OPTIONS += shared_libs
endif

ifdef static_libs
  ifndef shared_libs
    override static_libs_only = 1
  endif
  ACE_MAKE_OPTIONS += static_libs
endif

ACE_SHLIBS ?=

ifdef shared_libs
  ifdef SHLIBA
    LDLIBS := $(LDLIBS:-l%=-l%shr)
    ACE_SHLIBS := $(ACE_SHLIBS:-l%=-l%shr)
  endif # SHLIBA
else # ! shared_libs
  ifdef static_libs
    #### Don't build shared libs.
    PIC             =
    SHLIB           =
    SOEXT           = o
    VSHDIR          = $(VDIR)
    VLDLIBS         = $(LDLIBS:%.so=%.o)
  endif # static_libs
endif # shared_libs

ifdef kylix
  LDLIBS := $(LDLIBS:-l%=lib%.$(SOEXT))
  LIBS := $(LIBS:-l%=lib%.$(SOEXT))
  ACELIB := $(ACELIB:-l%=lib%.$(SOEXT))
  ACE_SHLIBS := $(ACE_SHLIBS:-l%=lib%.$(SOEXT))
endif # kylix

VLDLIBS = $(LDLIBS) $(ACELIB) $(LIBS)

probe ?=
ifeq ($(probe),0)
  override probe =
endif # probe

ifeq ($(probe),1)
    CPPFLAGS += -DACE_COMPILE_TIMEPROBES
endif # probe

purify ?=
ifeq ($(purify),0)
  override purify =
endif # purify

ifdef purify
PURE_CACHE_BASE_DIR = /tmp/purifycache
PURE_CACHE_DIR = $(PURE_CACHE_BASE_DIR)-$(LOGNAME)-$(notdir $(CXX) )

  #### Pick up Purify directory from the users PATH.
  ACE_PURIFY_DIR := \
    $(shell type purify | sed -e 's/.* is //' -e 's%/purify$$%%')

  #### You might want to adjust the Purify options below.
  #### -best-effort is undocumented but supported, and seems
  #### to help avoid occasional link failure.
  PURELINK += purify -best-effort -cache-dir=$(PURE_CACHE_DIR) \
              -chain-length=20 -fds-inuse-at-exit=no \
              -inuse-at-exit -max_threads=100
  CPPFLAGS += -DACE_HAS_PURIFY -I$(ACE_PURIFY_DIR)
endif # purify

quantify ?=
ifeq ($(quantify),0)
  override quantify =
endif # quantify

ifdef quantify
  #### Pick up Quantify directory from the users PATH.
  ACE_QUANTIFY_DIR := \
    $(shell type quantify | sed -e 's/.* is //' -e 's%/quantify$$%%')

  #### You might want to adjust the Quantify options below.
  #### -best-effort is undocumented but supported, and seems
  #### to help avoid occasional link failure.
  PRELINK += quantify -best-effort -cache-dir=$(PURE_CACHE_DIR) \
             -max_threads=100
  CPPFLAGS += -DACE_HAS_QUANTIFY -I$(ACE_QUANTIFY_DIR)

  ifeq ($(shell uname -p),sparc)
    PRELINK += -record-register-window-traps
  endif # sparc
endif # quantify

repo ?=
ifeq ($(repo),0)
  override repo =
endif # repo

ifdef repo
  #### Remove -fno-implicit-templates from, and add -frepo to, CCFLAGS.
  CCFLAGS := $(strip $(subst -fno-implicit-templates,,$(CCFLAGS)))
  CCFLAGS += -frepo
  CPPFLAGS += -DACE_HAS_GNU_REPO
#  ace_lib_prelink := 1
endif # repo

# If pipes is not set, we default to 0
pipes ?= 0

# If pipes is set to 1 then add the compiler flag -pipe. This uses pipe instead
# of intermediate files. This can be disabled by setting pipes to 0 in the
# platform_macros.GNU file.
ifeq ($(pipes),1)
   CFLAGS += -pipe
endif # pipes

# RMCast is turned on by default
rmcast ?= 1
ifeq ($(rmcast),1)
  ACE_MAKE_OPTIONS += rmcast
endif

# If not explicitly is set that we don't have rwho, assume we have it.
rwho ?= 1

# If not explicitly is set that we don't have stlport, assume we 
# don't have it.
stlport ?= 0
ifeq ($(stlport),1)
  CPPFLAGS += -DACE_HAS_STLPORT $(PLATFORM_STLPORT_CPPFLAGS)
  LDFLAGS  += $(PLATFORM_STLPORT_LDFLAGS)
  LIBS     += $(PLATFORM_STLPORT_LIBS)
endif


# If not explicitly is set that we have wfmo, assume we don't have it.
ifndef wfmo
  wfmo = 0
endif

#----------------------------------------------------------------------------
#      Conditional macro definitions
#----------------------------------------------------------------------------

PTDIRS ?=
PRELINK ?=
POSTLINK ?=
PURELINK ?=
TEMPLATES_FLAG ?=
MVCMD ?=

COMPILE.c       = $(CC) $(CFLAGS) $(CPPFLAGS) -c
COMPILE.cc      = $(CXX) $(CCFLAGS) $(CPPFLAGS) $(PTDIRS) -c
RC ?=

COMPILE.rc      = $(RC)
COMPILE-NO_DASH_G.cc = `echo $(COMPILE.cc) | sed 's/-g //'`
# 960905 Marius Kjeldahl <marius@funcom.com>
# Added the line below to be used for compiling executable shared objects
COMPILEESO.cc   = $(CXX) $(CCFLAGS) $(CPPFLAGS) $(PTDIRS)

ifeq ($(LINK.c),override)
  LINK.c  = $(LINK.c.override)
else
  LINK.c  = $(PURELINK) $(PRELINK) $(LD) $(CFLAGS) $(CPPFLAGS) $(LDFLAGS) \
            $(LDLIBS) $(LIBS)
endif # LINK.c.override

ifeq ($(LINK.cc),override)
  LINK.cc = $(LINK.cc.override)
else
  LINK.cc = $(PURELINK) $(PRELINK) $(LD) $(CCFLAGS) $(CPPFLAGS) $(PTDIRS)
endif # LINK.cc.override

SOLINK.cc = $(PRELINK) $(DLD) $(SOFLAGS)
