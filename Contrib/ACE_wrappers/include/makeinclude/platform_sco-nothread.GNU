# platform_sco-nothread.GNU,v 4.17 2002/06/12 23:34:18 kitty Exp

debug = 1
optimize = 1

CC              = gcc
CXX		= g++
CFLAGS		+= -w
CCFLAGS		+= $(CFLAGS) -fno-implicit-templates -fno-strict-prototypes
DCFLAGS		+= -g #### should this be -gstabs?
DLD             = $(CXX)
LD		= $(CXX)
LIBS            = -lsocket
OCFLAGS		= -O2
PIC             =
AR              = ar
ARFLAGS         = ruv
RANLIB          = /bin/true

SOFLAGS += $(CPPFLAGS)
SOBUILD = $(COMPILE.cc) $(PIC) -o $(VSHDIR)$*.so $<
PRELIB  = @true
