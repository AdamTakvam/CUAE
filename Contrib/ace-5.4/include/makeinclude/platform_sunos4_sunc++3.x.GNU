# platform_sunos4_sunc++3.x.GNU,v 4.6 2002/06/12 23:34:18 kitty Exp

# SunOS 4.x (Solaris 1.x) with SunC++ 3.x (note that this is a very old C++
# compiler, based on cfront.  In paricular, it's not the same thing as
# SunC++ 4.x (which is also known as SPARCWorks 3.x).

debug = 1

CC		= cc
CXX		= CC
CCFLAGS		+= $(CFLAGS)
DCFLAGS		+= -g
DLD		= /bin/ld
LD		= $(CXX)
LIBS		+=
PIC		= -pic
AR		= ar
ARFLAGS		= ruv
RANLIB		= ranlib
SOFLAGS		+= -assert pure-text
SOBUILD		= $(COMPILE.cc) $(PIC) -o $(VSHDIR)$*.o $<; \
		  $(RM) -rf $(VSHDIR)pt$*; mkdir $(VSHDIR)pt$*; \
		  YYZ="`$(CXX) $(CCFLAGS) $(CPPFLAGS) \
		  -ptr$(VSHDIR)pt$* -ptr./ptrepository $<`"; \
		  $(RM) -rf a.out; \
		  YYZ="`echo $(VSHDIR)pt$*/*.o`"; \
		  $(SOLINK.cc) -o $@ ./$(VSHDIR)$*.o $(YYZ)
