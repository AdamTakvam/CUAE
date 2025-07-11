# -*- Makefile -*-
#----------------------------------------------------------------------------
#       rules.local.GNU,v 4.95 2004/01/13 18:29:54 dhinton Exp
#
#       Local targets
#       GNU version
#       Requires GNU make
#----------------------------------------------------------------------------


CLEANUP_OBJDIRS =
CLEANUP_DIRS =

ifdef CLEANUP_BIN
  CLEANUP_OBJDIRS = $(VDIR)
else # ! CLEANUP_BIN
  ifdef CLEANUP_LIB
    CLEANUP_OBJDIRS = $(VDIR)
  endif # CLEANUP_LIB
endif # CLEANUP_BIN

ifdef CLEANUP_SHLIB
  CLEANUP_OBJDIRS += $(VSHDIR)
endif # CLEANUP_VSHLIB

ifdef TEMPINCDIR
  CLEANUP_DIRS += $(TEMPINCDIR)
endif 

ifdef sun
  CLEANUP_DIRS += SunWS_cache
endif

#### TEMPLATE_REPOSITORY can be set in individual platform files
#### to create template repository directories.  See
#### platform_sunos5_sunc++.GNU for an example.
ifdef TEMPLATE_REPOSITORY
CLEANUP_DIRS += $(TEMPLATE_REPOSITORY)
endif

makefile_name.local:
	@echo
	@echo Makefile: $(shell pwd)/$(MAKEFILE)
	@echo

all.local: makefile_name.local $(split_target) build.objdirs build.local install.local

#----------------------------------------------------------------------------
#       C/C++ compilation targets
#----------------------------------------------------------------------------


IDL_SRC ?=
VLIBS ?=
BUILD ?=
BIN ?=
VBIN ?= $(BIN:%=%$(VAR))
$(VBIN): $(VLIBS)

# Always build the V* targets in this order.  We control assignment to them
# elsewhere. We add the existing $(BUILD) targets to the end to allow the
# caller to set additional ones, e.g., when defining multiple targets in a
# single makefile--note that we filter out any that are already present.
VBUILD = $(IDL_SRC) $(VLIBS) $(VBIN) $(filter-out $(IDL_SRC) $(VLIBS) $(VBIN), $(BUILD))

build.local: $(VBUILD)

# By setting SUFFIXES to nothing, we turn off all implicit rules, and force
# make to only use the explicit ones we define.
.SUFFIXES:

# C++ related targets

STATIC_LINK_FLAG ?=
static_link ?= 0
ifeq ($(static_link),1)
  ifeq ($(static_libs_only),1)
    LDFLAGS += $(STATIC_LINK_FLAG)
  endif # static_libs_only = 1
endif # static_link = 1

OS ?=
ifeq ($(OS),"Windows_NT")
$(VDIR)%.obj: %.c
	$(COMPILE.c) $(CC_OUTPUT_FLAG) $@ $<
	${MVCMD}

$(VDIR)%.obj: %.cpp
	$(COMPILE.cc) $(CC_OUTPUT_FLAG) $@ $<
	${MVCMD}
else
$(VDIR)%.o: %.c $(VDIR).creation_stamp
	$(COMPILE.c) $(CC_OUTPUT_FLAG) $@ $<
	${MVCMD}

$(VDIR)%.o: %.cpp $(VDIR).creation_stamp
	$(COMPILE.cc) $(CC_OUTPUT_FLAG) $@ $<
	${MVCMD}
endif

$(VDIR)%.o: %.C $(VDIR).creation_stamp
	$(COMPILE.cc) $(CC_OUTPUT_FLAG) $@ $<
	${MVCMD}

$(VDIR)%.o: %.cc $(VDIR).creation_stamp
	$(COMPILE.cc) $(CC_OUTPUT_FLAG) $@ $<
	${MVCMD}

$(VDIR)%.o: %.cxx $(VDIR).creation_stamp
	$(COMPILE.cc) $(CC_OUTPUT_FLAG) $@ $<
	${MVCMD}

# If SOLINK is defined, then the .$(SOEXT) file is built from the
# .$(OBJEXT) file via separate rules in the same directory.
# Otherwise, the .$(SOEXT) and .$(OBJEXT) are built via the same rule.
# SOLINK is required for the repository under gcc.
ifndef SOLINK
# I added the "Executable Shared Object (ESO)" define to separate between
# normal shared object files and executable shared object files (the kind
# that the service configurator needs to be able to function).
# 970104 Marius Kjeldahl <mariusk@sn.no, marius@funcom.com>
  ifdef (ESOBUILD)
$(VSHDIR)%.$(SOEXT): %.cpp $(VSHDIR).creation_stamp
	$(ESOBUILD)
  else # ! ESOBUILD
$(VSHDIR)%.$(SOEXT): %.cpp $(VSHDIR).creation_stamp
	$(SOBUILD)
  endif # ESOBUILD

  ifeq ($(split), 1)
$(VSHDIR)%.$(OBJEXT): %.cpp $(VSHDIR).creation_stamp
	$(COMPILE.cc) $(PIC) SPLIT/`echo $< | sed "s/.cpp/_S\*/"`
	mv *.$(OBJEXT) $(VSHDIR)
  else # ! split

$(VSHDIR)%.$(OBJEXT): %.cpp $(VSHDIR).creation_stamp
	$(COMPILE.cc) $(PIC) $(CC_OUTPUT_FLAG) $@ $<
  endif # split

$(VSHDIR)%.$(OBJEXT): %.cc $(VSHDIR).creation_stamp
	$(COMPILE.cc) $(PIC) $(CC_OUTPUT_FLAG) $@ $<

$(VSHDIR)%.$(OBJEXT): %.C $(VSHDIR).creation_stamp
	$(COMPILE.cc) $(PIC) $(CC_OUTPUT_FLAG) $@ $<

$(VSHDIR)%.$(OBJEXT): %.cxx $(VSHDIR).creation_stamp
	$(COMPILE.cc) $(PIC) $(CC_OUTPUT_FLAG) $@ $<

$(VSHDIR)%.$(OBJEXT): %.c $(VSHDIR).creation_stamp
	$(COMPILE.c) $(PIC) $(CC_OUTPUT_FLAG) $@ $<

  ifneq (,$(COMPILE.rc))
$(VSHDIR)%.rc.$(OBJEXT): %.rc $(VSHDIR).creation_stamp
	$(COMPILE.rc) $< $@
  endif # COMPILE.rc

$(VSHDIR)%.$(SOEXT): $(VSHDIR)%.$(OBJEXT) $(VSHDIR).creation_stamp
	$(SOLINK)
endif # SOLINK

ifeq ($(OS),"Windows_NT")
$(VDIR).creation_stamp:
	@if not exist $(VDIR) mkdir $(VDIR)
	@echo > $@
else
$(VDIR).creation_stamp:
	@-test -d $(VDIR) || mkdir $(VDIR) $(ACE_NUL_STDERR)
	@touch $@
endif

ifneq ($(VDIR),$(VSHDIR))
  ifeq ($(OS),"Windows_NT")
$(VSHDIR).creation_stamp:
	@if not exist $(VSHDIR) mkdir $(VSHDIR)
	@echo > $@
  else
$(VSHDIR).creation_stamp:
	@-test -d $(VSHDIR) || mkdir $(VSHDIR) $(ACE_NUL_STDERR)
	@touch $@
  endif
endif

#----------------------------------------------------------------------------
#       Library generation targets
#----------------------------------------------------------------------------

#### show_statics shows static objects in locally-created object files.
#### It assumes that the object files were built using g++.
#### TOOLENV selects the proper nm in VxWorks host environments.
#### TOOLDIR allows specification of the full path to nm via definition
#### in the platform_macros.GNU file.
show_statics:
	-@$(TOOLDIR)nm$(TOOLENV) -Co $(VSHDIR)*o | \
	  egrep ' global destructors '; true

#### show_uninit shows uninitialized data in locally-created object files.
#### TOOLENV selects the proper nm in VxWorks host environments.
show_uninit:
	-@$(TOOLDIR)nm$(TOOLENV) -Co $(VSHDIR)*o | egrep ' b '

#----------------------------------------------------------------------------
#       Installation targets
#----------------------------------------------------------------------------

# Always add LIB_INSTALL to INSTALL, so libs will get installed even no BIN
# is built.  LIB_INSTALL is a dependency for BIN, so libs are installed prior
# to use.  Also, make LIB_INSTALL depend on VLIBS, so libs get built prior to
# installation.
ifdef LIB_INSTALL
  INSTALL ?= $(LIB_INSTALL)
  $(LIB_INSTALL): $(VLIBS)
endif

INSTALL ?=
install.local: $(INSTALL)

deinstall.local:
ifdef CLEANUP_INSTALL
  ifneq ($(CLEANUP_INSTALL),)
	$(RM) $(CLEANUP_INSTALL)
  endif # CLEANUP_INSTALL
endif # CLEANUP_INSTALL

ifndef LN_S
  ifndef COMSPEC
    ifdef ComSpec
      #### ACE+TAO use COMSPEC, but ComSpec is defined.
      COMSPEC = $(ComSpec)
    endif # ComSpec
  endif # ! COMPSPEC

  ifdef COMSPEC
    #### Assume we're on a WIN32 host.
    LN_S = cp -p
  else  # ! WIN32
    LN_S = ln -s
  endif # ! WIN32
endif # ! LN_S

$(INSBIN)/%$(VAR)$(EXEEXT) \
$(INSINC)/ace% \
$(INSLIB)/% \
$(INSMAN)/man1/% \
$(INSMAN)/man2/% \
$(INSMAN)/man3/% \
$(INSMAN)/man4/% \
$(INSMAN)/man5/% \
$(INSMAN)/man6/% \
$(INSMAN)/man7/% \
$(INSMAN)/man8/% \
$(INSMAN)/manl/% \
$(INSMAN)/mann/% :
	@if test $(shell pwd) != $(@D) -o -n "$(SOVERSION)" ; then \
		if test -s $@ ; then \
			echo "$(@F) already installed" ; \
		else \
			echo "Installing $(@F:%.$(SOEXT)$(SOVERSION)=%.$(SOEXT)) -> $(@D)" ; \
			$(RM) $(@:%.$(SOEXT)$(SOVERSION)=%.$(SOEXT)); \
			$(LN_S) $(shell pwd)/$(@F) $(@:%.$(SOEXT)$(SOVERSION)=%.$(SOEXT)); \
			echo "Installing $(@F) -> $(@D)" ; \
			$(RM) $@; \
			$(LN_S) $(shell pwd)/$(@F) $@; \
		fi ; \
	fi
ifdef MVSLIB
#special for MVS in order to use the .x files
	@if test -w $(@D) ; then \
		if test -s $(@:.$(SOEXT)=.x) ; then \
			echo "$(@F:.$(SOEXT)=.x) already installed" ; \
		else \
			echo "Installing $(@F:.$(SOEXT)=.x) -> $(@D)" ; \
			$(RM) $(@); $(LN_S) $(shell pwd)/$(@F:.$(SOEXT)=.x) $(@:.$(SOEXT)=.x) ; \
		fi \
	fi
endif

#----------------------------------------------------------------------------
#       Cleanup targets
#----------------------------------------------------------------------------

clean_idl_stubs.local: makefile_name.local
ifdef IDL_FILES
	-$(RM) $(foreach ext, $(IDL_EXT), $(addsuffix $(ext), $(IDL_FILES)))
endif

CLEANUP_BIN ?=
ifneq ($(CLEANUP_BIN),)
  DO_CLEANUP = 1
  REALCLEAN_FILES = $(CLEANUP_BIN:%=%$(EXEEXT)) $(CLEANUP_BIN:%=%_debug$(EXEEXT)) $(CLEANUP_BIN:%=%_profile$(EXEEXT)) $(CLEANUP_BIN:%=%_optimize$(EXEEXT))
endif # !CLEANUP_BIN

ifdef static_libs
  CLEANUP_LIB ?=
  ifneq ($(CLEANUP_LIB),)
    DO_CLEANUP = 1
    REALCLEAN_FILES += $(CLEANUP_LIB:%=%) $(CLEANUP_LIB:%=%_debug) $(CLEANUP_LIB:%=%_profile) $(CLEANUP_LIB:%=%_optimize)
  endif # !CLEANUP_LIB
endif # static_libs

ifdef shared_libs
  CLEANUP_SHLIB ?=
  ifneq ($(CLEANUP_SHLIB),)
    DO_CLEANUP = 1
    REALCLEAN_FILES += $(CLEANUP_VSHLIB:%=%) $(CLEANUP_VSHLIB_NO_VER:%=%*) $(CLEANUP_VSHLIB_NO_VER:%=%_debug) $(CLEANUP_VSHLIB_NO_VER:%=%_profile) $(CLEANUP_VSHLIB_NO_VER:%=%_optimize)
  endif # !CLEANUP_SHLIB
endif # shared_libs

CLEANUP_OBJS += $(addsuffix .o, $(addprefix $(VDIR),$(CLEANUP_BIN)))

ifdef SRC
  CLEANUP_OBJS = $(addsuffix .$(OBJEXT),$(addprefix $(VDIR),$(basename $(notdir $(SRC)))))
endif # SRC
ifneq ($(CLEANUP_BIN),)
  CLEANUP_OBJS += $(addsuffix .$(OBJEXT),$(addprefix $(VDIR),$(basename $(notdir $(CLEANUP_BIN)))))
endif # CLEANUP_BIN
ifdef LSRC
  CLEANUP_OBJS += $(addsuffix .$(OBJEXT),$(addprefix $(VSHDIR),$(basename $(notdir $(LSRC)))))
endif # LSRC

clean.local: makefile_name.local
ifdef DO_CLEANUP
	-$(RM) -r \
	*.$(OBJEXT) *~ *.bak *.rpo *.sym lib*.*_pure_* \
	Makefile.old core-r $(CLEANUP_DIRS) \
	cxx_repository ptrepository ti_files \
	gcctemp.c gcctemp so_locations *.ics \
	templateregistry ir.out core.* $(CLEANUP_OBJS)
endif # DO_CLEANUP

realclean.local: clean.local deinstall.local clean_idl_stubs.local
ifdef DO_CLEANUP
	-$(RM) -r $(REALCLEAN_FILES) $(sort $(CLEANUP_OBJDIRS))
endif # DO_CLEANUP


#----------------------------------------------------------------------------
#       Dependency generation target
#----------------------------------------------------------------------------

MAKEFILE ?= Makefile
DEPENDENCY_FILE ?= $(MAKEFILE)

TAO_ROOT ?= $(ACE_ROOT)/TAO

# If no libraries are build in the current directory (actually, if
# rules.lib.GNU isn't included), then SOEXT might not be set.
SOEXT ?= so

ORIGdepend.local: $(MAKEFILE)
	@$(RM) $(MAKEFILE).old
	@cp $(MAKEFILE) $(MAKEFILE).old
	$(ACE_ROOT)/bin/g++dep -f $(MAKEFILE) $(CPPFLAGS) -DMAKEDEPEND $(LSRC) $(SRC) $(PSRC)
	@cat $(MAKEFILE) | \
	sed -e "s;$(TAO_ROOT);\$$(TAO_ROOT);g" \
	    -e "s;$(ACE_ROOT);\$$(ACE_ROOT);g" \
	    $(ACE_DEPEND_SED_CMD) \
	    -e '/$$(ACE_ROOT)\/ace\/config[^\.]*\.h/d' \
	    -e "s; /[-a-zA-Z0-9_./]*\.h;;g" \
	    -e "s;\([-a-zA-Z0-9._]*\)\.$(OBJEXT):;.obj/\1.$(OBJEXT) .obj/\1.$(SOEXT) $(VSHDIR)\1.$(OBJEXT) $(VSHDIR)\1.$(SOEXT):;" \
	> $(MAKEFILE).new
	@cat $(MAKEFILE).new >$(MAKEFILE)
	@$(RM) $(MAKEFILE).new
	@if	cmp -s $(MAKEFILE) $(MAKEFILE).old ;\
	then	echo "Makefile dependencies unchanged." ;\
	else \
		echo "Makefile dependencies updated." ;\
	fi ;\
	$(RM) $(MAKEFILE).old ;

ifneq ($(MAKEFILE),$(DEPENDENCY_FILE))
$(DEPENDENCY_FILE):
	touch $(DEPENDENCY_FILE)
depend.local: $(DEPENDENCY_FILE)
-include $(DEPENDENCY_FILE)
endif

ifndef DEPGEN
  DEPGEN = $(ACE_ROOT)/bin/g++dep
endif

depend.local: $(MAKEFILE) idl_stubs.local
	@$(RM) $(DEPENDENCY_FILE).old
	@cp $(DEPENDENCY_FILE) $(DEPENDENCY_FILE).old
	ACE_PLATFORM_CONFIG=$(ACE_PLATFORM_CONFIG) OBJEXT=$(OBJEXT) SOEXT=$(SOEXT) \
	VDIR=$(VDIR) VSHDIR=$(VSHDIR) ACE_DEPEND_SED_CMD="$(ACE_DEPEND_SED_CMD)" \
	$(DEPGEN) -A $(DEPEND_CMD_ARGS) -f $(DEPENDENCY_FILE) $(CPPFLAGS) \
	-DMAKEDEPEND $(LSRC) $(SRC) $(PSRC)
	@if cmp -s $(DEPENDENCY_FILE) $(DEPENDENCY_FILE).old ;\
	then echo "Makefile dependencies unchanged." ;\
	else \
		echo "Makefile dependencies updated." ;\
	fi ;\
	$(RM) $(DEPENDENCY_FILE).old ;

#----------------------------------------------------------------------------
#       RCS info target
#----------------------------------------------------------------------------

IDL_FILES ?=
IDL_CLIENT_HDR_EXT ?=

idl_stubs.local: $(foreach file, $(IDL_FILES), $(file)$(IDL_CLIENT_HDR_EXT))

ifdef $(TAO_IDL_DEP)
$(TAO_IDL_DEP):
	@if test ! -s $(TAO_IDL_DEP); then \
	  echo " ***"; \
	  echo " *** ERROR: $(TAO_IDL_DEP) not found."; \
	  echo " ***"; \
	  echo; /bin/false; fi
endif # TAO_IDL_DEP

#----------------------------------------------------------------------------
#       RCS info target
#----------------------------------------------------------------------------

rcs_info.local:
	@rcs info

#----------------------------------------------------------------------------
#       Variant targets and conditional macros
#----------------------------------------------------------------------------

build.objdirs: $(CLEANUP_DIRS)

ifeq ($(OS),"Windows_NT")
$(CLEANUP_DIRS):
	@if not exist $@ mkdir $@
else
$(CLEANUP_DIRS):
	@test -d $@ || mkdir -p $@ $(ACE_NUL_STDERR)
endif

#---------------------------------------------------------------------------
#       Source file splitting
#---------------------------------------------------------------------------
SPLIT:
	@echo "Splitting source files..."
	@$(ACE_ROOT)/bin/split-cpp -s Svc_Conf_l.cpp -s Svc_Conf_y.cpp $(LSRC)
	@echo "done."

