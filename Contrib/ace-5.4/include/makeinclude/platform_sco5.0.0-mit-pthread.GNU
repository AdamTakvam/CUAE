# platform_sco5.0.0-mit-pthread.GNU,v 4.16 2003/08/09 04:43:51 dhinton Exp

# Requires MIT pthreads and gcc 2.7.2 installed in /usr/progressive
# to produce libACE.so correctly changed the specs file for gcc 2.7.2
# for not link -lc and -lgcc and other not shared libraries

debug = 1
optimize = 1

CC              = gcc
# for threads
# for no threads
#CXX		= gcc -fno-implicit-templates -w -fno-strict-prototypes
# LDFLAGS	+= -L $(ACE_ROOT)/ace -L ./ ${MIT_PTHREADS_DIR}/init/init.o # -z muldefs
# LDLIBS	+= -L $(ACE_ROOT)/ace -L ./ ${MIT_PTHREADS_DIR}/init/init.o -lACE # -z muldefs
LDLIBS	+= -L $(ACE_ROOT)/ace -L ./ -lACE # -z muldefs
CXX		= g++
CFLAGS		+= -melf -w

DCFLAGS		+= -gstabs
DCCFLAGS	+= $(DCFLAGS)
DLD             = $(CXX)
LD              = $(CXX)
LIBS            = -L${MIT_PTHREADS_DIR} -lnsl -lpthread
OCFLAGS		+= -O2
OCCFLAGS        += $(OCFLAGS)
PIC             = -fPIC
AR              = ar
ARFLAGS         = ruv
RANLIB          = /bin/true

SOFLAGS += $(CPPFLAGS) -melf -fPIC -G
SOBUILD = $(COMPILE.cc) $(PIC) -o $(VSHDIR)$*.so $<
# SOLINK	= $(SOLINK.cc) -o $@ $(LDFLAGS) $<
PRELIB  = @true

# Test for template instantiation, add to SOFLAGS if SONAME set, 
# add -E to LDFLAGS if using GNU ld
#
include $(ACE_ROOT)/include/makeinclude/platform_g++_common.GNU

CCFLAGS		+= -melf -fPIC -Xpg4plus $(TEMPLATES_FLAG) -I ${MIT_PTHREADS_DIR}/include -I /usr/progressive/lib/g++-include
