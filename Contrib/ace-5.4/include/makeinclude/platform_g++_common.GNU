# -*- Makefile -*-
# platform_g++_common.GNU,v 4.22 2004/01/09 20:22:31 dhinton Exp
#
# Common file help turn on/off explicit template instantiation

ifeq ($(CXX),insure)
  # insure does not pass through the -dumpversion option.
  CXX_FOR_VERSION_TEST = g++
else
  CXX_FOR_VERSION_TEST = $(CXX)
endif

CXX_VERSION := $(shell $(CXX_FOR_VERSION_TEST) -dumpversion)

# If no option has been specified, try to enable templates based on the
# version of the compiler.
#
ifndef templates
  ifeq (2.95,$(findstring 2.95,$(CXX_VERSION)))
    templates = automatic
  else
    ifeq (3.,$(findstring 3.,$(CXX_VERSION)))
      templates = automatic
    else
      ifeq (2.96,$(findstring 2.96,$(CXX_VERSION)))
        templates = automatic
      else
        ifeq (egcs, $(findstring egcs, $(CXX_VERSION)))
          templates = explicit
        else
          templates = explicit
        endif
      endif
    endif
  endif
endif

# Turn on the proper flags for explicit template instantiation.
#
ifeq ($(templates),explicit)
  ifeq ($(TEMPLATES_FLAG),) # Turn on flags if none is speficied.
    TEMPLATES_FLAG=-fno-implicit-templates
  endif
  CPPFLAGS += -DACE_HAS_EXPLICIT_TEMPLATE_INSTANTIATION
endif

# The correct flags to pass to the linker for ELF dynamic shared library
# versioning. None of this works on OpenBSD's ld.
# If the platform file didn't already set versioned_so, default to 1.
versioned_so ?= 1
with_ld ?=
ifeq ($(versioned_so),1)
  ifeq ($(with_ld),hpux)
    SOFLAGS += -Wl,+h -Wl,$(SONAME)
  else
    ifeq ($(with_ld),openbsd)
    else
      ifeq ($(with_ld),aix)
      # What to do here???
      else
        SOFLAGS += -Wl,-h -Wl,$(SONAME)
      endif
    endif
  endif
endif

static_libs_only ?=

# Only modify LDFLAGS if DLD has been set.
ifneq ($(DLD),)
  ifeq ($(DLD),$(CXX_FOR_VERSION_TEST)) # only try this is we are using ld through gcc
    LD_FOR_VERSION_TEST = $(shell $(CXX_FOR_VERSION_TEST) -print-prog-name=ld)
  else
    LD_FOR_VERSION_TEST = $(DLD)
  endif # DLD = CXX_FOR_VERSION_TEST
  # The -E option is GNU ld specific 
  GNU_LD := $(shell sh -c '$(LD_FOR_VERSION_TEST) -v 2>&1 | grep -c "GNU ld"')
endif # DLD

ifeq ($(GNU_LD),1)
  STATIC_LINK_FLAG ?= -static
endif # GNU_LD

# Add all symbols to the dynamic symbol table.  Needed to enable dynamic_cast
# for shared libraries. (see http://gcc.gnu.org/faq.html#dso)
ifeq ($(shared_libs), 1)
  ifneq ($(static_libs_only), 1)
    ifeq ($(GNU_LD),1)
      # Make sure this version of ld supports the -E option.
      LD_EXPORT_DEFINED := $(shell sh -c '$(LD_FOR_VERSION_TEST) -E 2>&1 | grep -i -e "(option|flag)" /dev/null; echo $$?')
      ifeq ($(LD_EXPORT_DEFINED),1)
        LDFLAGS += -Wl,-E
      endif # LD_EXPORT_DEFINED = 1
    endif # GNU ld
  endif # static_libs_only
endif # shared_libs
