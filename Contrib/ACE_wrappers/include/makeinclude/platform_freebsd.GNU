# platform_freebsd.GNU,v 4.18 2002/06/12 23:34:18 kitty Exp

# platform_freebsd.GNU

VERSION := $(shell uname -r | sed -e "s/`uname -r | sed -e 's/.//'`//")
ifeq ($(VERSION),2)
versioned_so = 1
endif

ifndef exceptions
  exceptions = 1
endif

ifeq (,$(debug))
  debug = 1
endif

ifeq (,$(optimize))
  optimize = 1
endif

CC              = gcc
CXX             = g++

# Test for template instantiation.
#
include $(ACE_ROOT)/include/makeinclude/platform_g++_common.GNU


CFLAGS          += -W -Wall -Wpointer-arith -pipe
CCFLAGS         += $(CFLAGS) $(TEMPLATES_FLAG)
DCFLAGS         += -g
ifneq ($(VERSION),3)
  LDFLAGS       += -Wl,-rpath $(ACE_ROOT)/ace
endif
DLD             = $(CXX)
LD              = $(CXX)
LIBS            += -lstdc++
OCFLAGS         += -O2
PIC             = -fpic
AR              = ar
ARFLAGS         = ruv
RANLIB          = ranlib

SOFLAGS += $(CPPFLAGS) -shared
SOBUILD = $(COMPILE.cc) $(PIC) -o $(VSHDIR)$*.so $<
PRELIB = @echo
