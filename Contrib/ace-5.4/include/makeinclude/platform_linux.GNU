# -*- Makefile -*-
# platform_linux.GNU,v 4.60 2004/01/07 23:28:11 dhinton Exp

# According to Bryon G. Rigg <bgrigg@opus.bcbnet.com>, this file
# should allow ACE to be built on Linux.

# debug = 1
# optimize = 1

# We always include config-linux.h on Linux platforms.
ACE_PLATFORM_CONFIG ?= config-linux.h

exceptions ?= 1

ifeq (,$(debug))
  debug = 1
endif
ifeq (,$(optimize))
  optimize = 1
endif

threads ?= 1

PLATFORM_XT_CPPFLAGS=
PLATFORM_XT_LIBS=-lXt
PLATFORM_XT_LDFLAGS=

PLATFORM_FL_CPPFLAGS=
PLATFORM_FL_LIBS=-lfltk
PLATFORM_FL_LDFLAGS=

PLATFORM_X11_CPPFLAGS=-I/usr/X11R6/include
PLATFORM_X11_LIBS=-lX11
PLATFORM_X11_LDFLAGS=-L/usr/X11R6/lib

PLATFORM_GL_CPPFLAGS=-I/usr/X11R6/include
PLATFORM_GL_LIBS    =-lGL
PLATFORM_GL_LDFLAGS =-L/usr/X11R6/lib

PLATFORM_GTK_CPPFLAGS=$(shell gtk-config --cflags)
PLATFORM_GTK_LIBS    =$(shell gtk-config --libs)
PLATFORM_GTK_LDFLAGS =

PLATFORM_WX_CPPFLAGS= $(shell wx-config --cflags) $(PLATFORM_GTK_CPPFLAGS)
PLATFORM_WX_LIBS    = $(shell wx-config --libs)   $(PLATFORM_GTK_LIBS)
PLATFORM_WX_LDFLAGS =

PLATFORM_AIO_SUPPORT := \
       $(shell test -e /usr/lib/librt.so && echo -DACE_HAS_AIO_CALLS)

ssl ?= 0
ifeq ($(ssl),1)
  # Some Linux OpenSSL installations compile in Kerberos support.  Add
  # the Kerberos include path to preprocessor include path.
  #
  # We should probably also add the Kerberos libraries to
  # PLATFORM_SSL_LIBS but we can't be sure if they are needed without
  # a more sophisticated check.  This will only be a problem when
  # statically linking the OpenSSL library.  The majority of
  # installations use shared OpenSSL libraries so we should be okay,
  # at least until we migrate to Autoconf.
  PLATFORM_SSL_CPPFLAGS += -I/usr/kerberos/include
endif # ssl

insure ?= 0
ifeq ($(insure),1)
  CC  = insure
  CXX = insure
else
  CC  = gcc
  CXX = g++
endif

CFLAGS  += -W -Wall -Wpointer-arith -pipe
ifeq ($(threads),1)
    CPPFLAGS  += -D_REENTRANT $(PLATFORM_AIO_SUPPORT)
endif # threads

# 
# Rely on _GNU_SOURCE to set these defaults defined in /usr/include/features.h 
# instead of setting them directly here (older versions of gcc don't set it
# for you): _SVID_SOURCE _BSD_SOURCE _POSIX_SOURCE _POSIX_C_SOURCE=199506L, etc...
CPPFLAGS += -D_GNU_SOURCE

DCFLAGS += -g
DLD     = $(CXX)
LD      = $(CXX)
LIBS    += -ldl

ifeq ($(threads),1)
  LIBS += -lpthread
endif
LIBS += $(shell test -e /usr/lib/librt.so && echo -lrt)

PLATFORM_QT_CPPFLAGS=-I$(QTDIR)/include
PLATFORM_QT_LIBS=-lqt
PLATFORM_QT_LDFLAGS=-L$(QTDIR)/lib

OCFLAGS += -O3
PIC     = -fPIC
AR      = ar
ARFLAGS = rsuv
RANLIB  = @true
SOFLAGS += $(CPPFLAGS) -shared
SOBUILD = $(COMPILE.cc) $(PIC) -o $(VSHDIR)$*.so $<; \
          $(SOLINK.cc) -o $@ $(LDFLAGS) $(VSHDIR)$*.o
PRELIB  = @true

# Test for template instantiation, add to SOFLAGS if SONAME set, 
# add -E to LDFLAGS if using GNU ld
#
include $(ACE_ROOT)/include/makeinclude/platform_g++_common.GNU

CCFLAGS += $(CFLAGS) $(TEMPLATES_FLAG)

# Added line below to support "Executable Shared Object" files (as
# needed by the service configurator).
# Marius Kjeldahl <mariusk@sn.no, marius@funcom.com>
ifeq ($(threads),1)
    ESOBUILD = $(COMPILEESO.cc) $(PIC) -shared -o $(VSHDIR)$*.so $<
    ifndef PRELIB
       PRELIB = @true
    endif # ! PRELIB
endif

sctp ?=
# support for OpenSS7 SCTP
ifeq ($(sctp),openss7)
  PLATFORM_SCTP_CPPFLAGS= -DACE_HAS_OPENSS7_SCTP
  PLATFORM_SCTP_LDFLAGS=
  PLATFORM_SCTP_LIBS= 
endif

# support for LKSCTP (Linux Kernel 2.5)
ifeq ($(sctp),lksctp)
  PLATFORM_SCTP_CPPFLAGS= -DACE_HAS_LKSCTP
  PLATFORM_SCTP_LDFLAGS=
  PLATFORM_SCTP_LIBS= /usr/local/lib/libsctp.a
endif

#### GNU gas has a string limit of 4096 characters.  On Alphas,
#### builds will fail due to running over that limit.  There are
#### at least two workarounds:
#### 1) Change the limit to 8192 characters and rebuild gas.  See
####    ACE-INSTALL.html for more information.
#### 2) Don't use -g when compiling those files.
#### If you're building on an Alpha and you haven't hacked and
#### rebuilt gas, you might need to uncomment the following.
#### ifeq ($(debug),1)
####   SUPPRESS_DASH_G = 1
#### endif # debug
