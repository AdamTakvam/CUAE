#----------------------------------------------------------------------------
#       Makefile,v 4.316 2003/02/05 02:32:19 kitty Exp
#
#       Makefile for the ACE library
#----------------------------------------------------------------------------

# This default rule is here so invoking make realclean will do
# work when recursing through subdirectories
.DEFAULT:
	@$(MAKE) -f Makefile.ace $@
	@$(MAKE) -f Makefile.dirs SUBDIR_MAKEFILE=Makefile $@

# Invoke Makefile.ace first to build libACE, and then
# invoke Makefile.dirs to recurse through subdirectories
all:
	@$(MAKE) -f Makefile.ace
	@$(MAKE) -f Makefile.dirs SUBDIR_MAKEFILE=Makefile
