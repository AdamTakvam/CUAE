# platform_irix5.3_g++.GNU,v 4.13 2003/08/09 04:43:51 dhinton Exp

# Irix 5.3 with GNU C++

debug = 1
optimize = 1

CC              = gcc
CXX             = g++

DCFLAGS		+= -g
DLD             = $(CXX)
LD		= $(CXX)
LDFLAGS         += -L$(ACE_ROOT)/ace
OCFLAGS		+= -O2
PIC             = -fpic
RANLIB          = echo
SOFLAGS         += -shared $(CPPFLAGS)
SOBUILD         = $(COMPILE.cc) $(PIC) -o $(VSHDIR)$*.o $<; \
                  $(SOLINK.cc) -o $@ $(LDFLAGS) $(VSHDIR)$*.o

# Test for template instantiation, add to SOFLAGS if SONAME set, 
# add -E to LDFLAGS if using GNU ld
#
include $(ACE_ROOT)/include/makeinclude/platform_g++_common.GNU

CCFLAGS		+= $(CFLAGS) $(TEMPLATES_FLAG)
