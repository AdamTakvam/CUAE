# Makefile,v 1.15 2003/07/14 18:47:58 jwillemsen Exp
#----------------------------------------------------------------------------
#       GNU ACE Workspace
#----------------------------------------------------------------------------

include $(ACE_ROOT)/include/makeinclude/macros.GNU
TARGETS_NESTED := $(TARGETS_NESTED:.nested=)

$(TARGETS_NESTED):
	@$(MAKE) -f Makefile.ACEXML_Parser -C . $(@);
