#
#  Copyright (c) 2002, 2003, 2004 by Cisco Systems, Inc. All Rights Reserved.
#

CFG = release
PLATFORM = win32
#PLATFORM = unixwin32
#PLATFORM = sccpmain
IMAG = sappwin
IMAGE = $(IMAG).exe
IMAGEMAP = $(IMAG).map

!IF "$(SCCPMAIN)"=="sccpmain"
#IMAGE = $(SCCPMAIN).exe
IMAGEMAP = $(SCCPMAIN).map
SCCPMAIN_DEFS = -D SCCPMAIN
!ENDIF

!IF "$(PLATFORM)"=="win32"
# compile sccptest for win32
COMP_DEFS = -D SAPP_SAPP_SCCPTEST -D SAPP_PLATFORM_WIN32 -D SCCP_PLATFORM_WINDOWS $(SCCPMAIN_DEFS)
!ENDIF
!IF "$(PLATFORM)"=="unixwin32"
# compile sccptest for win32, but using unix stuff
#COMP_DEFS = -D SAPP_SAPP_SCCPTEST -D SAPP_PLATFORM_WIN32 -D SAPP_PLATFORM_UNIX_WIN -D SCCP_PLATFORM_UNIX 
COMP_DEFS = -D SAPP_SAPP_SCCPTEST -D SAPP_PLATFORM_UNIX -D SAPP_PLATFORM_UNIX_WIN -D SCCP_PLATFORM_UNIX $(SCCPMAIN_DEFS)
!ENDIF

CC = cl
LINK = link
SOURCEDIR = .
OBJDI = obj-$(CC)-$(PLATFORM)
OBJDIR = .\$(OBJDI)\$(CFG)

#SAPP flags- WIN32,_DEBUG,_CONSOLE,_MBCS,SAPP_PLATFORM_WIN32,SAPP_SAPP_SCCPTEST
#SCCP flags- _DEBUG,_LIB,WIN32,_MBCS,SCCP_PLATFORM_WINDOWS,SAPP_PLATFORM_WIN32

INCLUDES = -Ih
!IF "$(CFG)"=="release"
MTFLAGS = -MT
MTLIB = libcmt.lib
#-O1 space
#-O2 speed
OPTFLAGS = -O1
DBFLAGS = -D NDEBUG
!ELSE
MTFLAGS = -MTd
MTLIB = libcmtd.lib
OPTFLAGS = -Od
DBFLAGS = -Zi -GZ -Gm -D _DEBUG
!ENDIF

#CPP_PROJ=/W3 /GX /O2 /D "NDEBUG" /D "_LIB" /D "WIN32" /D "_MBCS" /FR"$(INTDIR)\\" /Fp"$(INTDIR)\sccp.pch" /YX /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /c 
#CPP_PROJ=/nologo /MTd /W3 /Gm /GX /ZI /Od /I "..\h" /D "_DEBUG" /D "_LIB" /D "WIN32" /D "_MBCS" /FR"$(INTDIR)\\" /Fp"$(INTDIR)\sccp.pch" /YX /Fo"$(INTDIR)\\" /Fd"$(INTDIR)\\" /FD /GZ /c 
#-MT link with libcmt.lib
#-MTs link with libcmts.lib
#-Gm enable minimal rebuild
#-Zi enable debugging information
#-Od disable optimizations
#-GZ enable runtime debug checks
CFLAGS = $(COMP_DEFS) -nologo -c $(OPTFLAGS) $(DBFLAGS) $(MTFLAGS) $(INCLUDES)
CPPFLAGS = $(COMP_DEFS) -nologo -c $(OPTFLAGS) $(DBFLAGS) $(MTFLAGS) $(INCLUDES)

RM = -@erase

LINCDIR = "C:\Program Files\Microsoft Visual Studio\VC98\Lib"
#LINCLUDES = $(LINCDIR)\ws2_32.lib $(LINCDIR)\kernel32.lib $(LINCDIR)\$(MTLIB)
LINCLUDES = /LIBPATH:$(OBJDIR) /LIBPATH:$(LINCDIR) ws2_32.lib kernel32.lib sccp.lib $(MTLIB)
LFLAGS = $(LINCLUDES) /map:$(OBJDIR)\$(IMAGEMAP) /machine:I386
SCCPMAIN_LFLAGS = /LIBPATH:$(OBJDIR) /map:$(OBJDIR)\$(IMAGEMAP) /machine:I386 /SUBSYSTEM:CONSOLE


SCCP_SRCS = am.c gapi.c sccp.c \
			sccp_debug.c sccp_platform.c \
			sccpcc.c sccpcm.c sccpmsg.c \
			sccprec.c sccpreg.c sem.c \
			sllist.c ssapi.c 

#SAPP_OBJS = $(SCCP_SRCS:%.cpp=%.obj)
SCCP_OBJS = am.obj gapi.obj sccp.obj \
			sccp_debug.obj sccp_platform.obj \
			sccpcc.obj sccpcm.obj sccpmsg.obj \
			sccprec.obj sccpreg.obj sem.obj \
			sllist.obj ssapi.obj 


!IF "$(PLATFORM)"=="win32"
TEST_SRCS = cli_menu.cpp debug_tools.cpp \
			platform_win32.cpp sccptest.cpp

#TEST_OBJS = $(SAPP_SRCS:%.cpp=%.obj)

TEST_OBJS = cli_menu.obj debug_tools.obj \
			platform_win32.obj sccptest.obj
!ENDIF
!IF "$(PLATFORM)"=="unixwin32"
TEST_SRCS = cli_menu.cpp debug_tools.cpp \
			platform_unix.cpp sccptest.cpp

#TEST_OBJS = $(SAPP_SRCS:%.cpp=%.obj)

TEST_OBJS = cli_menu.obj debug_tools.obj \
			platform_unix.obj sccptest.obj
!ENDIF


SAPP_SRCS = sapp.c sapp_win79xx.c timer.c \
			timer_platform_windows.c

#SAPP_OBJS = $(WIN_SRCS:%.c=%.obj)

SAPP_OBJS = sapp.obj sapp_win79xx.obj timer.obj \
			timer_platform_windows.obj

OBJECTS = $(SCCP_OBJS) $(TEST_OBJS) $(SAPP_OBJS)

SCCPTESTOBJS = $(TEST_OBJS) $(SAPP_OBJS)

TARGETS = $(IMAGE)

all: $(TARGETS)

default: all

clean:
	cd $(OBJDIR)
	$(RM) *.obj"
	$(RM) *.lib"
	$(RM) *.exe"
	$(RM) *.map"

"$(OBJDIR)" :
	if not exist "$(OBJDIR)/$(NULL)" mkdir "$(OBJDIR)"

$(IMAGE): $(OBJDIR) $(SCCPTESTOBJS) sccp.lib
	$(LINK) $(LFLAGS) /out:$(OBJDIR)\$@ $(SCCPTESTOBJS)

$(SCCPMAIN).exe: $(OBJDIR) $(SCCP_OBJS)
	$(LINK) $(SCCPMAIN_LFLAGS) /out:$(OBJDIR)\$@ $(SCCP_OBJS)

sccp.lib: $(OBJDIR) $(SCCP_OBJS)
	$(LINK) -lib /LIBPATH:$(OBJDIR) /out:$(OBJDIR)\$@ $(SCCP_OBJS)

.c.obj:; $(CC) $(CFLAGS) -Fo$(OBJDIR)\ $<
.cpp.obj:; $(CC) $(CPPFLAGS) -Fo$(OBJDIR)\ $<
