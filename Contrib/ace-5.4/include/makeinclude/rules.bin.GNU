# -*- Makefile -*-

#----------------------------------------------------------------------------
#       rules.bin.GNU,v 4.25 2004/01/12 23:40:47 dhinton Exp
#
#       Build binaries
#       GNU version
#       Requires GNU make
#----------------------------------------------------------------------------

ifdef BIN
  VBIN = $(BIN:%=%$(VAR))
  # VBIN always depends on idl_stubs and libraries, if they exist.
  ifdef IDL_SRC
    $(VBIN): $(IDL_SRC)
  endif
  ifdef VLIBS
    $(VBIN): $(LIB_INSTALL) $(VLIBS)
  endif
else # !BIN
  ifndef BIN_UNCHECKED
    @echo Warning: rules.bin.GNU included, but no BIN targets detected.
  endif # !BIN_UNCHECKED
endif # BIN

VOBJS = $(addsuffix .$(OBJEXT),$(addprefix $(VDIR),$(basename $(notdir $(SRC))))) 

# Needed for depend.
PSRC += $(addsuffix .cpp, $(CLEANUP_BIN))

CLEANUP_INSTALL += $(CLEANUP_BIN:%=$(INSBIN)/%$(VAR)$(EXEEXT))

CHORUSLINK ?=
using_aix_vacpp ?=

ifeq ($(CHORUSLINK),true)
$(BIN): %: $(VDIR)%.$(OBJEXT) $(VOBJS)
	$(LINK.cc) $(CC_OUTPUT_FLAG) $@ $(sort $(VDIR)$@.$(OBJEXT) $(VOBJS)) $(VLDLIBS) $(LDFLAGS) $(POSTLINK)
#	$(MERGEDIR)/dtool/mkctors $@.xp.o > $@.ct.s
#	$(CXX) -c $@.ct.s
#	$(RM) $@.ct.s
#	$(LD) $(LDOPTIONS) -e _start -o $@ $(MERGEDIR)/lib/crt/crth.u.o $(MERGEDIR)/lib/crt/crt0.o $(MERGEDIR)/lib/crt/crti.o $@.xp.o $@.ct.o $(MERGEDIR)/lib/crt/crtn.o -dn -T $(MERGEDIR)/src/act/slots/act_u.ld
#	$(RM) $@.xp.o
#	$(RM) $@.ct.o
else # !CHORUSLINK
  ifeq (1,$(using_aix_vacpp))
$(BIN): %: %.icc $(SRC)
	vacbld_r $< -severitylevel=warning
  else # !using_aix_vacpp
    ifdef kylix
$(BIN): %: $(VDIR)%.$(OBJEXT) $(VOBJS)
	$(LINK.cc) $(LDFLAGS) $(CC_OUTPUT_FLAG) $(VLDLIBS) $(BORINITEXEOBJ) $(POSTLINK) $(sort $(VDIR)$@.$(OBJEXT) $(VOBJS)) , $@,,
    else # !kylix
$(BIN): %: $(VDIR)%.$(OBJEXT) $(VOBJS)
	$(LINK.cc) $(LDFLAGS) $(CC_OUTPUT_FLAG) $@ $(sort $(VDIR)$@.$(OBJEXT) $(VOBJS)) $(VLDLIBS) $(POSTLINK)
    endif # kylix
  endif # using_aix_vacpp
endif

