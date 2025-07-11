# platform_mvs.GNU,v 4.11 2002/06/12 23:34:18 kitty Exp

# For MVS OpenEdition platform

debug = 0

CXX           = cxx
DCFLAGS	      += -g
DCCFLAGS      += -g
CPPFLAGS      += -+
DEFFLAGS      = -D_ALL_SOURCE -DNDEBUG
CC            = $(CXX)
LD            = $(CXX)
DLD           = $(CXX)
AR            = ar
ARFLAGS       = ruv
RANLIB        = echo
#LDFLAGS      = -W l,p,map

# MVCMD needed because cxx does not use the -o option to place objects
MVCMD         = @test ! -s $(@F) || mv $(@F) $(@D)/$(@F)

# Used to build static executables
ACELIB_STATIC = -lACE

# Used to build shared executables (much smaller in size)
ACELIB_DLL    = $(ACE_ROOT)/ace/libACE.x

# set accordingly to build either static or shared executables
ACELIB        = $(ACELIB_DLL)

SOFLAGS       += -W l,dll
PIC           = -W c,exportall
static_libs   = 1
