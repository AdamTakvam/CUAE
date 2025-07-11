# platform_openbsd.GNU,v 1.4 2003/10/17 21:48:05 shuston Exp

# platform_openbsd.GNU

versioned_so = 1

ifndef exceptions
  exceptions = 1
endif

ifeq (,$(debug))
  debug = 1
endif

ifeq (,$(optimize))
  optimize = 1
endif

ifeq (,$(threads))
  threads = 1
endif

CC              = gcc
CXX		= g++

CFLAGS          += -W -Wall -Wpointer-arith -pipe
DCFLAGS         += -g
LDFLAGS		+= -Wl,-rpath $(ACE_ROOT)/ace
DLD             = $(CXX)
LD		= $(CXX)
LIBS            +=
OCFLAGS         += -O2
PIC             = -fpic
AR              = ar
ARFLAGS         = ruv
RANLIB          = ranlib

#SOFLAGS += -Bshareable -x /usr/lib/c++rt0.o
SOFLAGS += $(CPPFLAGS) -shared
SOBUILD = $(COMPILE.cc) $(PIC) -o $(VSHDIR)$*.so  $<
PRELIB = @echo

ifeq ($(threads),1)
LDFLAGS         += -pthread
CFLAGS          += -D_THREAD_SAFE -DACE_HAS_THREADS
endif # threads

# Setting the internal name and version of shared libraries isn't supported
# by OpenBSD.  In fact, the -h option gives an error and interestingly
# enough, the -soname option doesn't but just ignores its parameter.
# This tells platform_g++_common.GNU not to add these link options.
with_ld = openbsd

# Test for template instantiation, add to SOFLAGS if versioned_so set, 
# add -E to LDFLAGS if using GNU ld
#
include $(ACE_ROOT)/include/makeinclude/platform_g++_common.GNU

CCFLAGS         += $(CFLAGS) $(TEMPLATES_FLAG)

