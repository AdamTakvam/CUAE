#
#  Copyright (c) 2002, 2003, 2004 by Cisco Systems, Inc. All Rights Reserved.
#
# Don't use this makefile - use sappwin32.mak
CFG = release
PLATFORM = WIN32
#PLATFORM = UNIXWIN32
IMAG = sappwin
IMAGE = $(IMAG).exe
IMAGEMAP = $(IMAG).map

!IF "$(PLATFORM)"=="WIN32"
# compile sccptest for win32
COMP_DEFS = -D SAPP_SAPP_SCCPTEST -D SAPP_PLATFORM_WIN32 -D SCCP_PLATFORM_WINDOWS
!ENDIF
!IF "$(PLATFORM)"=="UNIXWIN32"
# compile sccptest for win32, but using unix stuff
#COMP_DEFS = -D SAPP_SAPP_SCCPTEST -D SAPP_PLATFORM_WIN32 -D SAPP_PLATFORM_UNIX_WIN -D SCCP_PLATFORM_UNIX 
COMP_DEFS = -D SAPP_SAPP_SCCPTEST -D SAPP_PLATFORM_UNIX -D SAPP_PLATFORM_UNIX_WIN -D SCCP_PLATFORM_UNIX 
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


!IF "$(PLATFORM)"=="WIN32"
SAPP_SRCS = cli_menu.cpp debug_tools.cpp \
			platform_win32.cpp sccptest.cpp

#SAPP_OBJS = $(SAPP_SRCS:%.cpp=%.obj)

SAPP_OBJS = cli_menu.obj debug_tools.obj \
			platform_win32.obj sccptest.obj
!ENDIF
!IF "$(PLATFORM)"=="UNIXWIN32"
SAPP_SRCS = cli_menu.cpp debug_tools.cpp \
			platform_unix.cpp sccptest.cpp

#SAPP_OBJS = $(SAPP_SRCS:%.cpp=%.obj)

SAPP_OBJS = cli_menu.obj debug_tools.obj \
			platform_unix.obj sccptest.obj
!ENDIF


WIN_SRCS = sapp.c sapp_win79xx.c timer.c \
			timer_platform_windows.c

#WIN_OBJS = $(WIN_SRCS:%.c=%.obj)

WIN_OBJS = sapp.obj sapp_win79xx.obj timer.obj \
			timer_platform_windows.obj

OBJECTS = $(SCCP_OBJS) $(SAPP_OBJS) $(WIN_OBJS)

SAPPWINOBJS = $(SAPP_OBJS) $(WIN_OBJS)

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

$(IMAGE): $(OBJDIR) $(SAPPWINOBJS) sccp.lib
	$(LINK) $(LFLAGS) /out:$(OBJDIR)\$@ $(SAPPWINOBJS)

sccp.lib: $(OBJDIR) $(SCCP_OBJS)
	$(LINK) -lib /LIBPATH:$(OBJDIR) /out:$(OBJDIR)\$@ $(SCCP_OBJS)

.c.obj:; $(CC) $(CFLAGS) -Fo$(OBJDIR)\ $<
.cpp.obj:; $(CC) $(CPPFLAGS) -Fo$(OBJDIR)\ $<


#
#  Copyright (c) 2002, 2003, 2004 by Cisco Systems, Inc. All Rights Reserved.
#

CFG = release
PLATFORM = WIN32
IMAGE = sccptestwin32

#COMP_DEFS = -D

CC = cl
SOURCEDIR = .
OBJDI = obj-$(CC)-$(PLATFORM)
OBJDIR = .\$(OBJDI)\$(CFG)

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
DBFLAGS = -Zi -GZ -Gm -D DEBUG
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

RM = -@erase

LINCDIR = "C:\Program Files\Microsoft Visual Studio\VC98\Lib"
LINCLUDES = $(LINCDIR)\ws2_32.lib $(LINCDIR)\kernel32.lib $(LINCDIR)\$(MTLIB)
LFLAGS = $(LINCLUDES) /map:"$(OBJDIR)\$(IMAGE).map" /machine:I386


SCCP_OBJS = "$(OBJDIR)\am.obj" \
			"$(OBJDIR)\gapi.obj" \
			"$(OBJDIR)\llist.obj" \
			"$(OBJDIR)\sccp.obj" \
			"$(OBJDIR)\sccp_debug.obj" \
			"$(OBJDIR)\sccp_platform.obj" \
			"$(OBJDIR)\sccpcc.obj" \
			"$(OBJDIR)\sccpcm.obj" \
			"$(OBJDIR)\sccpmsg.obj" \
			"$(OBJDIR)\sccprec.obj" \
			"$(OBJDIR)\sccpreg.obj" \
			"$(OBJDIR)\sem.obj" \
			"$(OBJDIR)\sllist.obj" \
			"$(OBJDIR)\ssapi.obj"


SAPP_OBJS = "$(OBJDIR)\cli_menu.obj" \
			"$(OBJDIR)\debug_tools.obj" \
			"$(OBJDIR)\platform_win32.obj" \
			"$(OBJDIR)\sapp.obj" \
			"$(OBJDIR)\sapp_win79xx.obj" \
			"$(OBJDIR)\sccptest.obj" \
			"$(OBJDIR)\timer.obj" \
			"$(OBJDIR)\timer_platform_windows.obj"

OBJECTS =   $(SCCP_OBJS) \
            $(SAPP_OBJS)

default: all

clean:
	$(RM) "$(OBJDIR)\*.obj"
	$(RM) "$(OBJDIR)\*.exe"
	$(RM) "$(OBJDIR)\*.map"

all: sccp2 sapp2 sccptest2

sccptest2: $(OBJDIR) $(OBJDIR)\$(IMAGE).exe

sccp2: $(OBJDIR) $(SCCP_OBJS)

sapp2: $(OBJDIR) $(SAPP_OBJS)

"$(OBJDIR)" :
	if not exist "$(OBJDIR)/$(NULL)" mkdir "$(OBJDIR)"


$(OBJDIR)\$(IMAGE).exe: $(OBJECTS)
	link $(LFLAGS) /out:"$(OBJDIR)\$(IMAGE).exe" $(OBJECTS)

SOURCEDIR = .

.c{$(OBJDIR)}.obj::
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\" $(SOURCEDIR)\am.c

SOURCE = am
"$(OBJDIR)\am.obj": $(SOURCEDIR)\am.c
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\am.obj" $(SOURCEDIR)\am.c

SOURCE = gapi
"$(OBJDIR)\gapi.obj": $(SOURCEDIR)\gapi.c
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\gapi.obj" $(SOURCEDIR)\gapi.c

SOURCE = llist
"$(OBJDIR)\llist.obj": $(SOURCEDIR)\llist.c
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\llist.obj" $(SOURCEDIR)\llist.c

SOURCE = sccp
"$(OBJDIR)\sccp.obj": $(SOURCEDIR)\sccp.c
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\sccp.obj" $(SOURCEDIR)\sccp.c

SOURCE = sccp_debug
"$(OBJDIR)\sccp_debug.obj": $(SOURCEDIR)\sccp_debug.c
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\sccp_debug.obj" $(SOURCEDIR)\sccp_debug.c

SOURCE = sccp_platform
"$(OBJDIR)\sccp_platform.obj": $(SOURCEDIR)\sccp_platform.c
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\sccp_platform.obj" $(SOURCEDIR)\sccp_platform.c

SOURCE = sccpcc
"$(OBJDIR)\sccpcc.obj": $(SOURCEDIR)\sccpcc.c
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\sccpcc.obj" $(SOURCEDIR)\sccpcc.c

SOURCE = sccpcm
"$(OBJDIR)\sccpcm.obj": $(SOURCEDIR)\sccpcm.c
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\sccpcm.obj" $(SOURCEDIR)\sccpcm.c

SOURCE = sccpmsg
"$(OBJDIR)\sccpmsg.obj": $(SOURCEDIR)\sccpmsg.c
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\sccpmsg.obj" $(SOURCEDIR)\sccpmsg.c

SOURCE = sccprec
"$(OBJDIR)\sccprec.obj": $(SOURCEDIR)\sccprec.c
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\sccprec.obj" $(SOURCEDIR)\sccprec.c

SOURCE = sccpreg
"$(OBJDIR)\sccpreg.obj": $(SOURCEDIR)\sccpreg.c
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\sccpreg.obj" $(SOURCEDIR)\sccpreg.c

SOURCE = ssapi
"$(OBJDIR)\ssapi.obj": $(SOURCEDIR)\ssapi.c
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\ssapi.obj" $(SOURCEDIR)\ssapi.c

SOURCE = sem
"$(OBJDIR)\sem.obj": $(SOURCEDIR)\sem.c
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\sem.obj" $(SOURCEDIR)\sem.c

SOURCE = sllist
"$(OBJDIR)\sllist.obj": $(SOURCEDIR)\sllist.c
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\sllist.obj" $(SOURCEDIR)\sllist.c

SOURCE = cli_menu
"$(OBJDIR)\cli_menu.obj": $(SOURCEDIR)\cli_menu.cpp
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\cli_menu.obj" $(SOURCEDIR)\cli_menu.cpp

SOURCE = debug_tools
"$(OBJDIR)\debug_tools.obj": $(SOURCEDIR)\debug_tools.cpp
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\debug_tools.obj" $(SOURCEDIR)\debug_tools.cpp

SOURCE = platform_win32
"$(OBJDIR)\platform_win32.obj": $(SOURCEDIR)\platform_win32.cpp
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\platform_win32.obj" $(SOURCEDIR)\platform_win32.cpp

SOURCE = sapp
"$(OBJDIR)\sapp.obj": $(SOURCEDIR)\sapp.c
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\sapp.obj" $(SOURCEDIR)\sapp.c

SOURCE = sapp_win79xx
"$(OBJDIR)\sapp_win79xx.obj": $(SOURCEDIR)\sapp_win79xx.c
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\sapp_win79xx.obj" $(SOURCEDIR)\sapp_win79xx.c

SOURCE = sccptest
"$(OBJDIR)\sccptest.obj": $(SOURCEDIR)\sccptest.cpp
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\sccptest.obj" $(SOURCEDIR)\sccptest.cpp

SOURCE = timer
"$(OBJDIR)\timer.obj": $(SOURCEDIR)\timer.c
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\timer.obj" $(SOURCEDIR)\timer.c

SOURCE = timer_platform_windows
"$(OBJDIR)\timer_platform_windows.obj": $(SOURCEDIR)\timer_platform_windows.c
	$(CC) $(CFLAGS) -Fo"$(OBJDIR)\timer_platform_windows.obj" $(SOURCEDIR)\timer_platform_windows.c

