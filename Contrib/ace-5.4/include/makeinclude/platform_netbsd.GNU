# platform_netbsd.GNU,v 4.10 2003/08/09 04:43:51 dhinton Exp

# platform_netbsd.GNU

# ***** Notice: This file is still under testing. *****
# To be used on netbsd platform without pthread support.

debug = 1
optimize = 1
versioned_so = 1

CC              = gcc
CXX		= g++

CFLAGS          += -w -fno-strict-prototypes
DCFLAGS         += -g
DLD             = ld
LD		= $(CXX)
LIBS            += -lstdc++  #-lcompat -lresolv
##OCFLAGS         += -O2
PIC             = -fpic -DPIC
AR              = ar
ARFLAGS         = ruv
RANLIB          = ranlib

SOFLAGS += -Bshareable -x /usr/lib/c++rt0.o
SOBUILD = $(COMPILE.cc) $(PIC) -o $(VSHDIR)$*.so  $<
PRELIB = @echo

# Test for template instantiation, add to SOFLAGS if SONAME set, 
# add -E to LDFLAGS if using GNU ld
#
include $(ACE_ROOT)/include/makeinclude/platform_g++_common.GNU
CCFLAGS         += $(CFLAGS) $(TEMPLATES_FLAG)
