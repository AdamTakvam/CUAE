# platform_macosx_panther.GNU,v 1.4 2003/12/07 02:08:52 jmzorko Exp
# platform_macosx.GNU
# support for Mac OS X 10.3 (Panther)

ifndef exceptions
  exceptions = 1
endif
ifeq (,$(debug))
  debug = 1
endif
ifeq (,$(optimize))
  optimize = 0
endif
ifeq (,$(versioned_so))
  versioned_so = 0
endif
with_ld = macosx

CC              = gcc
CXX             = g++
CFLAGS          += -Wall -Wpointer-arith -Wno-long-double -pipe  

DCFLAGS         += -g
DLD             = libtool
LD              = $(CXX)
LIBS            += -lcc_dynamic -lstdc++ -lSystem

OCFLAGS         += -O3
RANLIB          = ranlib
SOEXT		= dylib
SOFLAGS += -dynamic
SOBUILD = -o $(VSHDIR)$*.dylib $<

# Test for template instantiation, add to SOFLAGS if versioned_so set, 
# add -E to LDFLAGS if using GNU ld
#
include $(ACE_ROOT)/include/makeinclude/platform_g++_common.GNU

LDFLAGS         += -flat_namespace -undefined warning
CCFLAGS         += $(CFLAGS) $(TEMPLATES_FLAG)
