#
#  Copyright (c) 2002, 2003, 2004 by Cisco Systems, Inc. All Rights Reserved.
#

CFG = release
#CFG = debug

#CC = gcc
CC = /router/bin/gcc.c2.95.3-p4
CPP = $(CC)
#COMPILE.cpp = $(CC)

IMAGE = sappunix
INCLUDES = -Ih
OBJDIR =

ifeq ($(SCCPMAIN),sccpmain)
#IMAGE = $(SCCPMAIN).exe
#IMAGEMAP = $(SCCPMAIN).map
SCCPMAIN_DEFS = -DSCCPMAIN
endif

COMP_DEFS = -DSAPP_SAPP_SCCPTEST -DSAPP_PLATFORM_UNIX -DSCCP_PLATFORM_UNIX -DSAPP_HAS_RECURSION -D_REENTRANT $(SCCPMAIN_DEFS)

#WARNFLAGS = -Wall
ifeq ($(CFG),release)
CFLAGS = $(COMP_DEFS) -c $(WARNFLAGS) $(INCLUDES)
CPPFLAGS = $(COMP_DEFS) -c $(WARNFLAGS) $(INCLUDES)
else
# use -E during static analysis
CFLAGS = $(COMP_DEFS) -c -g $(WARNFLAGS) $(INCLUDES)
CPPFLAGS = $(COMP_DEFS) -c -g $(WARNFLAGS) $(INCLUDES)
endif

RM = rm -f

LINCDIR = 
LINCLUDES = -lsocket -lpthread
LFLAGS = $(LINCLUDES) -o -Wl,-Map "$(OBJDIR)$(IMAGE)"
SCCPMAIN_LFLAGS = -o -Wl,-Map $(SCCPMAIN)

SCCP_SRCS = am.c gapi.c sccp.c \
			sccp_debug.c sccp_platform.c \
			sccpcc.c sccpcm.c sccpmsg.c \
			sccprec.c sccpreg.c sem.c \
			sllist.c ssapi.c

SCCP_OBJS = $(SCCP_SRCS:%.c=%.o)

SAPP_SRCS = cli_menu.cpp debug_tools.cpp \
			platform_unix.cpp sccptest.cpp


SAPP_OBJS = $(SAPP_SRCS:%.cpp=%.o)

WIN_SRCS = sapp.c sapp_win79xx.c timer.c \
			timer_platform_unix.c

WIN_OBJS = $(WIN_SRCS:%.c=%.o)

TARGETS = $(IMAGE)

all : $(TARGETS)

default: all

clean:
	$(RM) *.o
	$(RM) *.lib
	$(RM) $(IMAGE)

#$(IMAGE): sccp.lib $(SAPP_OBJS) $(WIN_OBJS)
$(IMAGE): $(SCCP_OBJS) $(SAPP_OBJS) $(WIN_OBJS)
	$(CC) -o $@ $(SCCP_OBJS) $(SAPP_OBJS) $(WIN_OBJS) $(LINCLUDES)

$(SCCPMAIN): $(SCCP_OBJS)
	$(CC) -Wl,-Map -o $@ $(SCCP_OBJS)

sccp.lib: $(SCCP_OBJS)
	$(CC) -shared -o $@ $(SCCP_OBJS)

.c.o:; $(CC) $(CFLAGS) -o $@ $<
.cpp.o:; $(CC) $(CPPFLAGS) -o $@ $<
