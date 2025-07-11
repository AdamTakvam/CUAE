# -*- Makefile -*-
#----------------------------------------------------------------------------
#       rules.lib.GNU,v 4.54 2004/01/07 23:28:11 dhinton Exp
#
#       Build libraries (i.e., contain no binary executables)
#       GNU version
#       Requires GNU make
#----------------------------------------------------------------------------

#---------------------------------------------------------------------------
#       Library versioning
#---------------------------------------------------------------------------

ifeq ($(versioned_so),1)
  # Turn on symbol versioning. The scheme that we follow is to allow
  # applications dependent on libraries, with same version numbers (major,
  # minor and beta) to run, but applications with dependencies on libraries
  # with different minor or major or beta versions to fail.
  #
  # Version number of the libraries
  #
  ifeq ($(SOVERSION),)
    SOVERSION = .$(ACE_MAJOR_VERSION).$(ACE_MINOR_VERSION).$(ACE_BETA_VERSION)
  endif # SOVERSION

  # Name that will be written into the dynamic library
  #
  ifeq ($(SONAME),)
    SONAME = $(SHLIB)$(SOVERSION)
  endif # SONAME
endif # versioned_so


#----------------------------------------------------------------------------
# The following targets arrange to build both unshared and shared libraries
#----------------------------------------------------------------------------

LIB ?=
VLIB = $(LIB:%.$(LIBEXT)=%$(VAR).$(LIBEXT))
CLEANUP_VLIB = $(CLEANUP_LIB:%.$(LIBEXT)=%$(VAR).$(LIBEXT))

# Add VLIB to VLIBS if building static_libs.
ifdef static_libs 
  VLIBS += $(VLIB)
endif

SHLIB ?=
# Setup shared library variables, including versions.
VSHLIB_NO_VER = $(SHLIB:%.$(SOEXT)=%$(VAR).$(SOEXT))
CLEANUP_VSHLIB_NO_VER = $(CLEANUP_SHLIB:%.$(SOEXT)=%$(VAR).$(SOEXT))
VSHLIB = $(VSHLIB_NO_VER:%=%$(SOVERSION))
CLEANUP_VSHLIB = $(CLEANUP_VSHLIB_NO_VER:%=%$(SOVERSION))

SHLIBA ?=
VSHLIBA = $(SHLIBA:%.$(LIBEXT)=%$(VAR).$(LIBEXT))

# Add VSHLIB if we're building shared_libs.
ifdef shared_libs
  VLIBS += $(VSHLIB) $(VSHLIBA)
endif

# Warn if no libs found
ifeq ($(VLIBS),)
  LIB_WARNING = 0
  ifndef LIB_UNCHECKED
    LIB_WARNING = 1
  endif # !LIB_CHECKED
  ifndef SHLIB_UNCHECKED
    LIB_WARNING = 1
  endif # !SHLIB_UNCHECKED
  ifeq ($(LIB_WARNING),1)
    @echo Warning: rules.lib.GNU included, but no library targets detected.
  endif # LIB_WARNING
else # VLIBS
  # Libraries always depend on idl_stubs, if they exist.
  IDL_SRC ?=
  ifeq ($(IDL_SRC),)
    $(VLIBS): $(IDL_SRC)
  endif # IDL_SRC
endif # !VLIBS

VLOBJS   += $(addsuffix .$(OBJEXT),$(addprefix $(VDIR),$(basename $(notdir $(LSRC)))))
ifdef LSRC2
  LSRC += $(LSRC2)
endif

VSHOBJS = $(addsuffix .$(OBJEXT),$(addprefix $(VSHDIR),$(basename $(notdir $(LSRC)))))
VSHOBJS1 = $(addsuffix .$(OBJEXT),$(addprefix $(VSHDIR),$(basename $(notdir $(LSRC)))))

ifdef static_libs
  LIB_INSTALL += $(VLIB:%=$(INSLIB)/%)
  CLEANUP_INSTALL += $(CLEANUP_VLIB:%=$(INSLIB)/%)
endif # static_libs

ifdef shared_libs
  LIB_INSTALL += $(VSHLIB:%=$(INSLIB)/%)
  CLEANUP_INSTALL += $(CLEANUP_VSHLIB:%=$(INSLIB)/%)
  CLEANUP_INSTALL += $(CLEANUP_VSHLIB_NO_VER:%=$(INSLIB)/%*)
  ifdef SHLIBA
    LIB_INSTALL += $(VSHLIBA:%=$(INSLIB)/%)
    CLEANUP_INSTALL += $(CLEANUP_SHLIBA:%=$(INSLIB)/%)
  endif
endif # shared_libs

ace_lib_prelink ?= 0
aix_build ?= 0

$(VLIB): $(VLOBJS)

ifeq (1,$(repo))
  ifneq (,$(ACELIB))
    #### With repo, link in all of the ace object files to the lib.
    #### This avoids multiple instantiations.  We haven't found it to
    #### be necessary with shared libs, so just do it for static libs.
    vpath %.$(OBJEXT) $(ACE_ROOT)/ace/$(VDIR)

    $(VLIB): $(ACE_ROOT)/ace/$(VDIR)*.$(OBJEXT)
  endif # ! ACELIB
endif # repo

ifdef PRELIB
	$(PRELIB)
endif
ifeq (1,$(ace_lib_prelink))
        #### Attempt a link (which will fail), to add template instantiations
        #### to lib files, for example.
	-$(LINK.cc) $(LINK_OUTPUT_FLAG) ace-templ_inst $? $(LDFLAGS) $(ACE_NUL_STDERR)
endif # ace_lib_prelink
	echo $(TMPINCDIR:%=%/*.o) $(filter %.o, $?) | xargs $(AR) $(ARFLAGS) $@ $(AREXTRA);
ifdef ibmcxx_build
# This is required to get AIX IBM C/C++ to instantiate and compile the needed
# templates.
	if test -s ./$(TEMPINCDIR)/*.C; \
	then \
		$(LINK.cc) $(LINK_OUTPUT_FLAG) dummy $(LDFLAGS) $(ACE_ROOT)/etc/xlc_dummy.cpp $^ $(ACE_SHLIBS) $(LIBS); \
		$(RM) dummy; \
		$(AR) $(ARFLAGS) $@ $(TEMPINCDIR)/*.$(OBJEXT); \
	fi
endif
	-chmod a+r $@
ifneq (,$(RANLIB))
	-$(RANLIB) $@
endif # RANLIB

# Note that if you don't want to build shared libraries, just remove the
# $(VSHOBJS)
#
# The AIX shr.o build is out here to keep it away from the SOVERSION stuff.
# The SOVERSION transform is applied after building the library file.
ifeq (1,$(aix_build))
shr.o: $(VSHOBJS1)
	$(SHR_FILTER) $(SOLINK.cc) $(SO_OUTPUT_FLAG) $@ $(VSHOBJS1) $(LDFLAGS) $(ACE_SHLIBS) $(LIBS)

.INTERMEDIATE: shr.o
endif # aix_build

ifeq (1,$(aix_build))
$(VSHLIB): shr.o
	$(AR) $(ARFLAGS) $@ $?
  ifneq (,$(RANLIB))
	-$(RANLIB) $@
  endif # RANLIB
else # ! aix_build
  ifdef SHLIBBUILD
$(VSHLIB): $(VSHOBJS)
  else # ! SHLIBBUILD
$(VSHLIB): $(VSHOBJS1)
  endif  # SHLIBBUILD
  ifdef PRELIB
	$(PRELIB)
  endif # PRELIB
  ifdef ibmcxx_build
    # This is required to get AIX IBM C/C++ to instantiate and compile the needed
    # templates before building the library.
	if test -s ./$(TEMPINCDIR)/*.C; \
	then \
		$(LINK.cc) $(LINK_OUTPUT_FLAG) dummy $(LDFLAGS) $(ACE_ROOT)/etc/xlc_dummy.cpp $^ $(ACE_SHLIBS) $(LIBS); \
		$(RM) dummy; \
	fi
  endif # ibmcxx_build
  ifdef SHLIBBUILD
	$(SHLIBBUILD)
  else # ! SHLIBBUILD
    ifdef ibmcxx_build
	if test -s ./$(TEMPINCDIR)/*.$(OBJEXT); \
        then \
		$(SOLINK.cc) $(SO_OUTPUT_FLAG) $@ $(VSHOBJS1) $(TEMPINCDIR)/*.$(OBJEXT) $(LDFLAGS) $(ACE_SHLIBS) $(LIBS); \
        else \
		$(SOLINK.cc) $(SO_OUTPUT_FLAG) $@ $(VSHOBJS1) $(LDFLAGS) $(ACE_SHLIBS) $(LIBS); \
	fi
    else # !ibmcxx_build
      ifdef kylix
	$(SOLINK.cc) $(LDFLAGS) $(VSHOBJS1), $@,, $(ACE_SHLIBS) $(LIBS) $(BORINITSHAREDOBJ)
      else # ! kylix
        ifeq (1,$(ace_lib_prelink))
          #### Attempt a link (which will fail), to add template instantiations
          #### to lib files, for example.
	-$(LINK.cc) $(LINK_OUTPUT_FLAG) ace-templ_inst $(VSHOBJS1) $(LDFLAGS) $(ACE_NUL_STDERR)
        endif # ace_lib_prelink
        ifeq ($(split),1)
	$(SHR_FILTER) $(SOLINK.cc) $(SO_OUTPUT_FLAG) $@ $(VSHDIR)/*.$(OBJEXT) $(LDFLAGS) $(ACE_SHLIBS) $(LIBS)
        else # ! split
	$(SHR_FILTER) $(SOLINK.cc) $(SO_OUTPUT_FLAG) $@ $(VSHOBJS1) $(LDFLAGS) $(ACE_SHLIBS) $(LIBS)
        endif  # split
      endif  # kylix
    endif  # ibmcxx_build
  endif  # SHLIBBUILD
endif  # aix_build
ifneq ($(SOVERSION),)
	-$(RM) $(VSHLIB_NO_VER)
	-$(LN_S) $@ $(VSHLIB_NO_VER)
endif
	-chmod a+rx $@
