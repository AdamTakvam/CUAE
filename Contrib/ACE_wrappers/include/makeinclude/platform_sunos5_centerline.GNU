# platform_sunos5_centerline.GNU,v 4.6 2002/06/12 23:34:18 kitty Exp

# SunOS 5.x (Solaris 2.x) with Centerline C++

debug = 1

CC		= cc
CXX		= CC
CFLAGS          += -mt
CCFLAGS         += $(CFLAGS)
DCFLAGS		+= -g
DLD		= $(CXX) -mt
LD		= $(CXX) -mt
LIBS		+= -lsocket -ldl -lnsl -lgen
PIC		= -PIC
#AR		= CC
#ARFLAGS	= -xar -o
ARFLAGS		= ruv
RANLIB		= echo
SOFLAGS		+= -G $(CPPFLAGS) $(PTDIRS)
SOBUILD		= $(COMPILE.cc) $(PIC) -o $(VSHDIR)$*.o $<; \
		  $(SOLINK.cc) $(PIC) -o $@ $(LDFLAGS) $(VSHDIR)$*.o
