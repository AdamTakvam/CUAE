# platform_unixware_g++.GNU,v 4.23 2003/08/09 04:43:51 dhinton Exp

# UnixWare V2.2.1.2 with g++ version 2.7.2.2

#ifndef debug
debug = 0
#endif

#ifndef exceptions
exceptions = 1
#endif

#ifndef threads
threads = 1
#endif


CC              = gcc
CXX             = g++

ACE_GNUG_ON_UNIXWARE = 1

#ifeq ($(exceptions),1)
  CCFLAGS += -fexceptions
#endif

DCFLAGS         += -g
#DCFLAGS         += -g

OCCFLAGS         += -O2

DLD             = $(CXX) -g
LD              = $(CXX)
LIBS            += -lsocket -lnsl -lgen -lthread
PIC             = -fpic
AR              = ar
ARFLAGS         = ruv
RANLIB          = /bin/true
#SOFLAGS        = -assert pure-text
SOFLAGS         += -shared
# SOBUILD - compile into .so directly
SOBUILD         = $(COMPILE.cc) $(PIC) -o $(VSHDIR)$*.so $<
# SOLINK        = $(SOLINK.cc) -o $@ $(LDFLAGS) $<
PRELIB          = @true

# Test for template instantiation, add to SOFLAGS if SONAME set, 
# add -E to LDFLAGS if using GNU ld
#
include $(ACE_ROOT)/include/makeinclude/platform_g++_common.GNU

CFLAGS          += -W -Wall -Wpointer-arith -pipe #### -Winline
CCFLAGS         += $(CFLAGS) $(TEMPLATES_FLAG)
