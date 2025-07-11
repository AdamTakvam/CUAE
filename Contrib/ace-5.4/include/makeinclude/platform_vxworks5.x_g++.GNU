# platform_vxworks5.x_g++.GNU,v 4.100 2003/12/27 10:30:25 jwillemsen Exp
#
# VxWorks 5.5.1 with GNU g++ that is distributed by Wind River.

#### Notes:
#### 1) This file requires that the WIND_BASE and WIND_HOST_TYPE environment
####    variables be set.  If the target CPU is not a PPC604, then your CPU
####    environment variable must be set.  If perl is not on your path,
####    you'll also need to set your PERL_PATH environment variable to
####    the full path to perl.
#### 2) If you have problems with munch output not being compilable
####    because it contains identifiers with ".", e.g., ".cpp", in them:
####    add a global variable or function to that .cpp file.  See
####    ace/IOStream_T.cpp for an explanation and example.
#### 3) If you're building on a WIN32 host, be sure that $(CC) is on
####    your PATH (or is a full pathname).

VXWORKS = 1
CROSS-COMPILE = 1

ifndef exceptions
  exceptions = 0
endif # ! exceptions
ifndef debug
  debug = 1
endif # ! debug
ifndef optimize
  optimize = 1
endif # ! optimize
ifndef rtti
  rtti = 1
endif # ! rtti
ifndef templates
  templates = explicit
endif # ! templates
ifndef versioned_so
  versioned_so = 0
endif # ! versioned_so

shared_libs =
static_libs = 1
# VxWorks doesn't have rwho
rwho = 0

ifeq (,$(WIND_BASE))
  default:
	@ERROR: you must set your WIND_BASE environment variable
endif # WIND_BASE

ifeq (,$(WIND_HOST_TYPE))
  default:
	@ERROR: you must set your WIND_HOST_TYPE environment variable
endif # WIND_HOST_TYPE

HOST_DIR = $(WIND_BASE)/host/$(WIND_HOST_TYPE)

#### Unconditionally add the path to the VxWorks installation
#### to ensure that we pick up the right applications.
#### We should be able to do this with explicit paths on the apps,
#### but there is at least one app (munch) which references
#### another (wtxtcl) without a path.
#### Use := instead of += because += inserts a space.
PATH := $(HOST_DIR)/bin:$(PATH)

ifeq (,$(PERL_PATH))
  PERL_PATH = perl
endif # ! PERL_PATH


#### BEGIN target CPU-specific settings

#### This is kind of messy so that it can support multiple targets.
#### You'll need to set CPU to a supported CPU using one of these
#### methods (unless your CPU is a PowerPC 604):
#### 1) create a $ACE_ROOT/include/makeinclude/platform_macros.GNU
####    that contains something like:
####
####  WIND_BASE = /project/doc/pkg/wind
####  WIND_HOST_TYPE = sun4-solaris2
####  CPU = I80486
####  GCC_VERSION = gcc-2.96
####
####  include $(ACE_ROOT)/include/makeinclude/platform_vxworks5.x_g++.GNU
####
####  NOTE: The Wind River release versioning is confusing.
####  Tornado appears to have the following combinations shipped:
####        Tornado 2.0 has VxWorks 5.4 and cygnus-2.7.2-960126
####        Tornado 2.0.1 has VxWorks 5.4.1 and cygnus-2.7.2-960126
####        Tornado 2.0.2 has VxWorks 5.4.2 and cygnus-2.7.2-960126
####        Tornado 2.1.x has VxWorks 5.4.2 and gcc-2.96
####        Tornado 2.2 has VxWorks 5.5.1 and gcc-2.96
####        Tornado 2.2.1 has VxWorks 5.5.1 and gcc-2.96
####  Since these combinations are not uniquely distinguishable,
####  we will use the following conventions in ACE for ACE_VXWORKS
####  (per J. Willemsen on the ACE mailing list):
####        0x540 indicates Tornado 2.0
####        0x541 indicates Tornado 2.0.1
####        0x542 indicates Tornado 2.1.x (note inconsistancy with WRS)
####        0x551 indicates Tornado 2.2.x
####
#### 2) modify the code below to set CPU, or
#### 3) set CPU on the command line, e.g., "make CPU=I80486"
ifeq ($(CPU),)
  CPU = PPC604
endif # default CPU

ifeq ($(GCC_VERSION),)
  GCC_VERSION = gcc-2.96
endif # default GCC Version

# These variables may be set on the command line if a different ACE_VXWORKS
# flag is desired.
ifeq ($(VXWORKS_VERSION_FLAG),)
   ifeq ($(GCC_VERSION),gcc-2.96)
       VXWORKS_VERSION_FLAG = -DACE_VXWORKS=0x551
   else
       VXWORKS_VERSION_FLAG = -DACE_VXWORKS=0x540
       VXWORKS_STDINC_FLAG = -nostdinc
   endif # GCC_VERSION
endif # VXWORKS_VERSION_FLAG

ifeq ($(CPU),SH7700)
  BINXSYM_NAME = xsym
  CFLAGS += -B$(GCCLIB_DIR)/ -DCPU=SH7700 -m3
  GCCLIB_DIR = $(HOST_DIR)/lib/gcc-lib/sh-wrs-vxworks/$(GCC_VERSION)
  TOOLENV = sh
else
ifeq ($(CPU),SH7700LE)
  BINXSYM_NAME = xsym
  CFLAGS += -B$(GCCLIB_DIR)/ -DCPU=SH7700 -m3 -ml
  GCCLIB_DIR = $(HOST_DIR)/lib/gcc-lib/sh-wrs-vxworks/$(GCC_VERSION)
  TOOLENV = sh
else
ifeq ($(CPU),SH7750)
  BINXSYM_NAME = xsym
  CFLAGS += -B$(GCCLIB_DIR)/ -DCPU=SH7750 -m4
  GCCLIB_DIR = $(HOST_DIR)/lib/gcc-lib/sh-wrs-vxworks/$(GCC_VERSION)
  TOOLENV = sh
else
ifeq ($(CPU),SH7750LE)
  BINXSYM_NAME = xsym
  CFLAGS += -B$(GCCLIB_DIR)/ -DCPU=SH7750 -m4 -ml -mspace
  GCCLIB_DIR = $(HOST_DIR)/lib/gcc-lib/sh-wrs-vxworks/$(GCC_VERSION)
  TOOLENV = sh
else
ifeq ($(CPU),PPC403)
  BINXSYM_NAME = xsym
  CFLAGS += -B$(GCCLIB_DIR)/ -DCPU=PPC403 -mcpu=403
  GCCLIB_DIR = $(HOST_DIR)/lib/gcc-lib/powerpc-wrs-vxworks/$(GCC_VERSION)
  TOOLENV = ppc
else
ifeq ($(CPU),PPC604)
  BINXSYM_NAME = xsym
  CFLAGS += -B$(GCCLIB_DIR)/ -DCPU=PPC604
  CCFLAGS += -mlongcall
  GCCLIB_DIR = $(HOST_DIR)/lib/gcc-lib/powerpc-wrs-vxworks/$(GCC_VERSION)
  INCLDIRS += -I$(HOST_DIR)/powerpc-wrs-vxworks/include
  INCLDIRS += -I$(HOST_DIR)/lib/gcc-lib/powerpc-wrs-vxworks/include
  TOOLENV = ppc
else
ifeq ($(CPU),PPC603)
  BINXSYM_NAME = xsym
  CFLAGS += -B$(GCCLIB_DIR)/ -DCPU=PPC603
  CCFLAGS += -mlongcall
  GCCLIB_DIR = $(HOST_DIR)/lib/gcc-lib/powerpc-wrs-vxworks/$(GCC_VERSION)
  TOOLENV = ppc
else
ifeq ($(CPU),PPC860)
  BINXSYM_NAME = xsym
  CFLAGS += -B$(GCCLIB_DIR)/ -DCPU=PPC860 -mcpu=860
  CCFLAGS += -mlongcall
  GCCLIB_DIR = $(HOST_DIR)/lib/gcc-lib/powerpc-wrs-vxworks/$(GCC_VERSION)
  TOOLENV = ppc
else
ifeq ($(CPU),SIMNT)
  BINXSYM_NAME = xsym
  CFLAGS += -U__WINNT -UWIN32 -U__WINNT__ -UWINNT -U__MINGW32__ -U_WIN32 \
            -U__WIN32 -U__WIN32__ -U__WIN32 -B$(GCCLIB_DIR) -DCPU=SIMNT
  GCCLIB_DIR = $(HOST_DIR)/lib/gcc-lib/powerpc-wrs-vxworks/$(GCC_VERSION) 
  TOOLENV = simpc
else
ifeq ($(CPU),PENTIUMPRO)
  BINXSYM_NAME = xsymDec
  CFLAGS += -DCPU=PENTIUMPRO
  TOOLENV = pentium
else
ifeq ($(CPU),PENTIUM)
  BINXSYM_NAME = xsymDec
  CFLAGS += -DCPU=PENTIUM
  TOOLENV = pentium
else
ifeq ($(CPU),I80486)
  #### Note: -fno-implicit-templates doesn't seem to work well with g++
  #### cygnus-2.7.2-960126 for pc486 target.  Some member functions don't
  #### get instantiated.  Wind River knows about this problem (SPR 8483).
  #### -fno-implicit-templates should be supported on Tornado II, so
  #### it is added for that platform below.
  BINXSYM_NAME = xsymDec
  CFLAGS += -DCPU=I80486 -m486
  TOOLENV = 386
else
ifeq ($(CPU),I80386)
  BINXSYM_NAME = xsymDec
  CFLAGS += -DCPU=I80386
  TOOLENV = 386
else
ifeq ($(CPU),SIMSPARCSOLARIS)
  BINXSYM_NAME = xsym
  CFLAGS += -B$(GCCLIB_DIR)/ -DCPU=SIMSPARCSOLARIS
  GCCLIB_DIR = \
    $(HOST_DIR)/lib/gcc-lib/sparc-sun-solaris2.5.1/$(GCC_VERSION)
  TOOLENV = simso
else
ifeq ($(CPU),SPARC)
  BINXSYM_NAME = xsym
  CFLAGS += -B$(GCCLIB_DIR)/ -DCPU=SPARC
  GCCLIB_DIR = $(HOST_DIR)/lib/gcc-lib/sparc-wrs-vxworks/$(GCC_VERSION)
  TOOLENV = sparc
else
ifeq ($(CPU),MC68020)
  CFLAGS += -B$(GCCLIB_DIR)/ -DCPU=MC68020 -m68020 -m68881
  GCCLIB_DIR = $(HOST_DIR)/lib/gcc-lib/m68k-wrs-vxworks/$(GCC_VERSION)
  TOOLENV = 68k
else
ifeq ($(CPU),MC68030)
  BINXSYM_NAME = xsym
  CFLAGS += -B$(GCCLIB_DIR)/ -DCPU=MC68030 -m68030 -m68881
  GCCLIB_DIR = $(HOST_DIR)/lib/gcc-lib/m68k-wrs-vxworks/$(GCC_VERSION)
  TOOLENV = 68k
else
ifeq ($(CPU),MC68040)
  BINXSYM_NAME = xsym
  CFLAGS += -B$(GCCLIB_DIR)/ -DCPU=MC68040 -m68040 -m68881
  GCCLIB_DIR = $(HOST_DIR)/lib/gcc-lib/m68k-wrs-vxworks/$(GCC_VERSION)
  TOOLENV = 68k
else
ifeq ($(CPU),MC68060)
  BINXSYM_NAME = xsym
  CFLAGS += -B$(GCCLIB_DIR)/ -DCPU=MC68060 -m68040 -m68881
  GCCLIB_DIR = $(HOST_DIR)/lib/gcc-lib/m68k-wrs-vxworks/$(GCC_VERSION)
  TOOLENV = 68k
else
  BINXSYM_NAME = xsym
endif # MC68060
endif # MC68040
endif # MC68030
endif # MC68020
endif # SPARC
endif # SIMSPARCSOLARIS
endif # I80386
endif # I80486
endif # PENTIUM
endif # PENTIUMPRO
endif # SIMNT
endif # PPC860
endif # PPC603
endif # PPC604
endif # PPC403
endif # SH7750LE
endif # SH7750
endif # SH7700LE
endif # SH7700

#### END target CPU-specific settings

#### Set up Win32 vs. Unix host specific macros.
ifeq ($(WIND_HOST_TYPE),x86-win32)
  CXX      = $(HOST_DIR)/bin/cc$(TOOLENV)
  ifeq (,$(pipes))
    pipes = 1
  endif # pipes
else
ifeq ($(TOOLENV),simso)
  #### vxsim needs to use ccsimso, not g++simso.
  CXX      = $(HOST_DIR)/bin/cc$(TOOLENV)
else
  CXX      = $(HOST_DIR)/bin/g++$(TOOLENV)
  ifeq (,$(pipes))
    pipes = 1
  endif # pipes
endif # simso
endif # x86-win32

ifeq ($(shared_libs),1)
  SHARED_LIBS = 1
endif
ifeq ($(shared_libs_only),1)
  SHARED_LIBS = 1
endif

ifeq ($(SHARED_LIBS),1)
  ACELIB = -L./
  #### There's no difference between non-shared and shared object code.
  VSHDIR = $(VDIR)
endif # shared_libs

ifeq ($(static_libs),1)
  AR              = $(HOST_DIR)/bin/ar$(TOOLENV)
  ARFLAGS         = rv
else  # ! static_libs
  AR              = echo
  ARFLAGS         = "build shared lib only"
endif # ! static_libs

CC            = $(HOST_DIR)/bin/cc$(TOOLENV)

#### Tornado II 2.2.x uses gcc-2.96
#### Linux cross-compiler uses full egcs
#### Anything older is no longer supported by TAO
ifndef ACE_CC
  ifndef COMSPEC
    ifdef ComSpec
      #### ACE+TAO use COMSPEC, but ComSpec is defined on some WIN32's.
      COMSPEC = $(ComSpec)
    endif # ComSpec
  endif # ! COMPSPEC

  ifdef COMSPEC
    #### Assume we're on a WIN32 host.  Further, assume that $(CC)
    #### is on the user's PATH.
    ACE_CC := $(CC)
  else  # ! COMSPEC
    #### Don't use this on WIN32 host, because the shell there seems
    #### to have trouble exec'ing sed.
    ACE_CC := $(shell type $(CC) | sed -e 's/.* is //')
  endif # ! COMSPEC
endif # ! ACE_CC
ifndef ACE_CC_VERSION
  ACE_CC_VERSION := $(shell $(ACE_CC) --version)
endif # ! ACE_CC_VERSION


# explicit template flags are handled by platform_g++_common.GNU
ifneq ($(templates),explicit)
  TEMPLATES_FLAG += -fimplicit-templates -fmerge-templates
endif

ACE_HAS_GNUG_PRE_2_8 := 0
CFLAGS +=  $(VXWORKS_VERSION_FLAG)
ifneq (1,$(exceptions))
  #### With exceptions, this is applied in wrapper_macros.GNU.
  #### Without, we do it here.
  CFLAGS += -Wno-uninitialized
endif # ! exceptions

CFLAGS          += -D_REENTRANT -ansi -fno-builtin -fno-defer-pop \
                   -fvolatile $(VXWORKS_STDINC_FLAG) -nostdlib -W -Wall \
                   $(VXWORKS_VERSION_FLAG)
CCFLAGS         += $(CFLAGS)
ifeq (0,$(rtti))
  #### Don't use RTTI with Tornado 1.0.x since the Tornado libraries
  #### don't support it.
  CCFLAGS += -fno-rtti -DACE_LACKS_RTTI
endif # rtti
DCFLAGS         += -g
DLD             = $(LD)
INCLDIRS        += -I$(WIND_BASE)/target/h
INCLDIRS        += -I$(HOST_DIR)/include/g++-3
LD              = $(PERL_PATH) $(ACE_ROOT)/bin/ace_ld \
                  -o $(VDIR) -C $(COMPILE.c) -- \
                  -m $(HOST_DIR)/bin/munch -n $(HOST_DIR)/bin/nm$(TOOLENV) $(HOST_DIR)/bin/ld$(TOOLENV)
LDFLAGS         += -X -r
#### WindRiver only recommends -O, not -O2, with some CPUs, including
#### the i386 family.  And -O2 causes some compilation failures.
OCFLAGS         += -O
PIC             =

# Test for template instantiation, add to SOFLAGS if SONAME set, 
# add -E to LDFLAGS if using GNU ld
#
include $(ACE_ROOT)/include/makeinclude/platform_g++_common.GNU

# TEMPLATES_FLAG is set by platform_g++_common.GNU
ifneq ($(TEMPLATES_FLAG),) # If specified, add it to the CCFLAGS
  CCFLAGS += $(TEMPLATES_FLAG)
endif


#### ld can't handle INCLDIRS, so override LINK definitions.
LINK.c           = override
LINK.c.override  = $(LD) $(LDFLAGS) $(LDLIBS) $(LIBS)

LINK.cc          = override
LINK.cc.override = $(LD)


#### To extract the symbol table from each executable, uncomment the POSTLINK
#### definition below.
## POSTLINK         = ; VX_CPU_FAMILY=$(TOOLENV) $(BINXSYM_NAME) < $@ > $@.sym

ifeq (1,$(exceptions))
  #### The VxWorks 1.0 libraries don't support exceptions.
  #### And, exceptions give the Tornado 2.0 compiler fits, e.g., with
  #### ACE_wrappers/TAO/tao/IORManipulation.cpp.
  #### But, Tornado 2.2.x handles exceptions, but with a memory bloat
  #### of around a factor of 4.5. So beware of enabling them unless you
  #### have lots of memory!
endif # exceptions
