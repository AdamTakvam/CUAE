#----------------------------------------------------------------------------
#       rules.nested.GNU,v 4.11 2002/08/09 04:13:32 bala Exp
#
#       Nested directory targets makefile definitions
#----------------------------------------------------------------------------

ifeq ($(MAKEFILE),)
  MAKEFILE = Makefile
endif # ! MAKEFILE

$(TARGETS_NESTED):
ifneq ($(DIRS),)
  ifeq (Windows,$(findstring Windows,$(OS)))
	@cmd /c "FOR /D %i IN ($(DIRS)) DO $(MAKE) -C %i $(@:.nested=)"
  else  # ! Windows
	@for dir in $(DIRS); do \
        $(MAKE) -f $(MAKEFILE) -C $$dir $(@:.nested=) $(ACE_NESTED_COMMAND); \
        done
  endif # ! Windows
endif # DIRS
