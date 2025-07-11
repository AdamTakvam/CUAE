# platform_m88k.GNU,v 4.19 2003/08/09 04:43:51 dhinton Exp

# SunOS 5.x (Solaris 2.x) with SunC++ 4.x

debug = 1
optimize = 1

CC		= gcc
CXX             = g++


CFLAGS		+= -w
DCFLAGS		+= -g
DLD		= $(CXX)
LD		= $(CXX)
INCLDIRS	+= -I$(ACE_ROOT) -I.
INCLDIRS	+= -I/devel/rmm/v1.0/local/src/threads/include
LDFLAGS		+= -L $(ACE_ROOT)/ace -L ./
LDFLAGS		+= -L/devel/rmm/v1.0/local/src/threads/lib
LIBS		+= -lsocket -ldl -lnsl -lgen -lstdc++ -lg++
LIBS		+= -lgthreads -lgmalloc
OCFLAGS		+= -O2
PIC		= -fpic
AR		= ar
ARFLAGS		= ruv
RANLIB		= /bin/true
SOFLAGS		+= -G $(CPPFLAGS)
SOBUILD		= $(COMPILE.cc) $(PIC) -o $(VSHDIR)$*.o $<; \
		  $(SOLINK.cc) -o $@ -h $@ $(LDFLAGS) $(VSHDIR)$*.o
PRELIB	= @true

# Test for template instantiation, add to SOFLAGS if SONAME set, 
# add -E to LDFLAGS if using GNU ld
#
include $(ACE_ROOT)/include/makeinclude/platform_g++_common.GNU

CCFLAGS		+= $(CFLAGS) -D__m88k__ $(TEMPLATES_FLAG) -fno-strict-prototypes
