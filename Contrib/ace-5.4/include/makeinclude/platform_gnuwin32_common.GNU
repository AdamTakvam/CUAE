# platform_gnuwin32_common.GNU,v 4.13 2003/10/29 08:08:59 jwillemsen Exp

ifneq ($(mingw32),1)
  ifneq ($(cygwin32),1)
    fail := \
      $(shell \
          echo "You should not use this file but one of"; \
          echo "platform_mingw32.GNU or platform_cygwin32.GNU"; \
          exit 1)
  endif
endif

# debug = 1
# optimize = 1
# exceptions = 0

GNUWIN32_DLL_ALLOW_EXECP = 0

GNUWIN32_CXX_VERSION := $(shell $(CXX) -dumpversion)
#exceptions across dll boundries are ok in gcc 3+
ifeq (2.95,$(findstring 2.95,$(GNUWIN32_CXX_VERSION)))
  GNUWIN32_DLL_ALLOW_EXECP = 0
else
  ifeq (3.,$(findstring 3.,$(GNUWIN32_CXX_VERSION)))
    GNUWIN32_DLL_ALLOW_EXECP = 1
  else
    GNUWIN32_DLL_ALLOW_EXECP = 0
  endif
endif

ifndef winsock2
  winsock2 = 1
endif

ifndef exceptions
  exceptions = 0
endif
ifeq (,$(debug))
  debug = 1
endif
ifeq (,$(optimize))
  optimize = 1
endif
ifeq (,$(threads))
  threads = 1
endif
ifeq (,$(pipes))
  pipes = 1
endif
ifeq ($(debug),0)
CFLAGS += -DNDEBUG
endif

CC      = gcc
CXX     = g++
RC      = windres

MODEL_FLAGS += -mcpu=$(TCPU)

CFLAGS  += -W -Wall -Wpointer-arith
ifeq ($(threads),1)
        MODEL_FLAGS += -mthreads
endif # threads

CFLAGS += $(MODEL_FLAGS)

EXEEXT = .exe

DCFLAGS += -g
DLD     = $(CXX)
LD      = $(CXX)

ifeq ($(mingw32),1)
ifeq ($(winsock2),1)
LIBS    += -lws2_32 -lmswsock
endif
LIBS    += -lwsock32 -lnetapi32
endif

OCFLAGS += -O3
PIC        =
AR         = ar
ARFLAGS    = rsuv
RANLIB     = ranlib
SOEXT      = dll
SOFLAGS    += $(MODEL_FLAGS) $(CPPFLAGS)
SOBUILD    = $(COMPILE.cc) $(PIC) -o $(VSHDIR)$*.so $<

# The sed below is to strip comments on the .def file,
# to workaround to a bug in ld.
SHLIBBUILD = \
  $(RM) $(VSHDIR)/$@.def.old $(VSHDIR)/$@.def; \
  dlltool --export-all --output-def $(VSHDIR)/$@.def --dllname $@ \
    $(VSHOBJS1) \
  && mv $(VSHDIR)/$@.def $(VSHDIR)/$@.def.old \
  && sed 's/;.*$$//g' < $(VSHDIR)/$@.def.old > $(VSHDIR)/$@.def \
  && $(SOLINK.cc) -Wl,--enable-auto-image-base -Wl,--out-implib,$@.a \
    -shared -o $@ $(LDFLAGS) $(VSHDIR)/$@.def \
    $(VSHOBJS1) $(ACE_SHLIBS) $(LIBS)

ifneq ($(GNUWIN32_DLL_ALLOW_EXECP), 1)
  ifeq ($(exceptions),1)
    SHLIBBUILD += \
     ;echo "Warning: exceptions fail when mixing DLL and non DLL calls."
  endif
endif

PRELIB  = @true

# Test for template instantiation, add to SOFLAGS if SONAME set, 
# add -E to LDFLAGS if using GNU ld
#
include $(ACE_ROOT)/include/makeinclude/platform_g++_common.GNU

CCFLAGS += $(CFLAGS) $(TEMPLATES_FLAG)
