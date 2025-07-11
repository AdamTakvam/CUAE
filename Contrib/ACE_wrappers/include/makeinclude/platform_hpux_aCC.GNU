# platform_hpux_aCC.GNU,v 4.36 2002/06/12 23:34:18 kitty Exp
#
# This is for the HP-UX 10.x and 11.x using the aC++ compiler.
#
# For 10.x, most of the difference for threaded vs. non-threaded is
# contained in ace/config-hpux-10.x.h.  However, to build without threads,
# you should run the make with "threads=0" since the default is to build
# with threads.
#
# HP-UX 11 supports both 32-bit and 64-bit builds, regardless of which
# architecture the build is done on.  This file is set up to build native
# to the machine it's running on.  To cross-build for another platform, add
# "buildbits=32" to make a 32-bit build, and "buildbits=64" to do a 64-bit
# build.  Note that if you explicitly specify a 64-bit build, the option
#  -Wl,+vnocompatwarnings is added to shut up the "At least one PA 2.0 object
# file detected..." messages.

ifeq (,$(buildbits))
  buildbits = 0
endif
ifeq (,$(debug))
  debug = 1
endif
ifeq (,$(distrib))
  distrib = 0
endif
ifeq (,$(exceptions))
  exceptions = 1
endif
ifeq (,$(optimize))
  optimize = 0
endif
ifeq (,$(stdcpplib))
  stdcpplib = 1
endif
ifeq (,$(threads))
  threads = 1
endif

# First, extract the OS version number.
HPVERS_WORDS := $(subst ., ,$(shell uname -r))
HPUX_VERS	:= $(join $(word 2,$(HPVERS_WORDS)),$(word 3,$(HPVERS_WORDS)))
#
CC		= /bin/cc
CXX		= aCC

# Extract the compiler version number. If it's A.03.27 (on HP-UX 11) or
# A.01.27 (on HP-UX 10.20) (or higher) then we can turn on the standard
# C++ library usage, else we need to stick with the old one.
CXXVERS_WORDS := $(subst ., , $(shell $(CXX) -V 2>&1))
CXXVERS := $(word 8, $(CXXVERS_WORDS))
ifneq (27,$(CXXVERS))
ifneq (30,$(CXXVERS))
  stdcpplib = 0
endif
endif

ifeq (1,$(stdcpplib))
  CCFLAGS += -AA -DACE_HAS_STANDARD_CPP_LIBRARY=1
ifeq (1, $(threads))
  CCFLAGS += -D_RWSTD_MULTI_THREAD -D_REENTRANT
endif
# Compile errors using -AA and +noeh together - HP problem report
# JAGad12608, fix slated for Dec 2001 Release -- Steve Huston, April 10, 2001.
  exceptions = 1
endif

# Set the appropriate preprocessor defs for threading based on OS version.
# If the user has requested building without threads, then don't set any
# for 10.20, but for 11.00, the default is to use threads, so explicitly
# turn them off if the user requested build without threads.
# On HP-UX 11, even without building for threads, the *_r reentrant
# functions are available, and _REENTRANT enables them.
ifeq ($(threads),1)
ifeq ($(word 2,$(HPVERS_WORDS)), 11)
THR_DEFS	= -DACE_HAS_THREADS -D_POSIX_C_SOURCE=199506L
else
THR_DEFS	= -DACE_HAS_THREADS -D_REENTRANT
endif
else
ifeq ($(word 2,$(HPVERS_WORDS)), 11)
THR_DEFS	= -DACE_HAS_THREADS=0 -D_REENTRANT
endif
endif

# Suppress warning 302 ((...) parameter list is a non-portable feature)
CCFLAGS		+= $(THR_DEFS) -DHPUX_VERS=$(HPUX_VERS) -DACE_LACKS_PRAGMA_ONCE +W302

# If exception support is explicitly disabled, tell the compiler.
# This is not recommended since the run-time library can throw exceptions.
ifneq ($(exceptions),1)
CCFLAGS  += +noeh
endif

# Additionally, on HP-UX 10.20, suppress 495 to shut up the warnings from
# the system header files.  667 is also suppressed, but the compiler still
# tells you there was a future error, but at least you can pick out any real
# errors by quickly scanning the output. 829 is suppressed because the
# system headers have offending string literals assigned to char *.
ifeq ($(word 2,$(HPVERS_WORDS)), 10)
CCFLAGS         += +W495,667,829,908
endif
DCCFLAGS	+= -g
DLD		= $(CXX) $(CCFLAGS)
LD		= $(CXX)
OCCFLAGS	= -O
PIC		= +Z

AR		= $(COMPILE.cc) +inst_close $^; /bin/ar
ARFLAGS		= ruv
RANLIB		= echo
LDFLAGS		= -Wl,+s -z
SOFLAGS		+= -b

ifeq ($(distrib),1)

ifeq ($(word 2,$(HPVERS_WORDS)), 10)
SOFLAGS		+= -Wl,-x -Wl,+h/usr/lib/$(@F)
else
ifeq ($(buildbits),32)
SOFLAGS		+= -Wl,-x -Wl,+h/usr/lib/$(@F)
endif
ifeq ($(buildbits),64)
SOFLAGS		+= -Wl,-x -Wl,+h/usr/lib/pa20_64/$(@F)
endif
endif

endif

SOEXT		= sl
SONAME		=
SOVERSION	=

ifeq ($(buildbits),32)
CCFLAGS		+= +DA1.1 +DS1.1
else
ifeq ($(buildbits),64)
  ifeq ($(word 2,$(HPVERS_WORDS)), 11)
    ## Without the W, it isn't really a 64-bit build
    CCFLAGS     += +DA2.0W +DS2.0W
  else
    CCFLAGS     += +DA2.0 +DS2.0
  endif
LDFLAGS		+= -Wl,+vnocompatwarnings
else
CCFLAGS         += +DAportable
endif
endif

#
# Grab the appropriate libraries based on the version of HP-UX we're building
# on and whether or not thread support is being used.
#
ifeq ($(word 2,$(HPVERS_WORDS)), 11)

#11.x:
ifeq ($(threads),1)
## -L./ must directly preceed the system libraries for 64-bit builds
## with aCC 3.27.  Without this change, libpthread.sl can not be found.
LIBS		+= -L./ -lxti -lpthread -lrt -ldld
else
LIBS		+= -lxti -lrt -ldld
endif

else

# 10.x:
LIBS		+= -lxti -ldld
ifeq ($(threads),1)
LIBS		+= -lcma
endif

endif
