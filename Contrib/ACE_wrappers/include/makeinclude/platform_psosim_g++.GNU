# platform_psosim_g++.GNU,v 4.4 2002/06/12 23:34:18 kitty Exp
# PSOSim with GNU g++ 2.7.2

####
#### Notes:
####
####   1) This file assumes that the PSS_ROOT environment variable is set.
####
####   2) There is a separate Makefile in $(PSS_CFG_DIR) that must be used
####      to create $(PSS_CFG_DIR)/acepsos.a
####
####

CPU_FAMILY      = psosim
CPU             = psosim
CPU_TOOLS       = psosim
BOARD           = psosim

PSS_CFG_DIR     = $(ACE_ROOT)/psosim
PSS_SYS_DIR     = $(PSS_ROOT)/sys/os
PSS_SYS_LIBS    = $(PSS_SYS_DIR)/prepc.a $(PSS_SYS_DIR)/psos.a $(PSS_SYS_DIR)/pna.a
PSS_BSP         = $(PSS_ROOT)/bsps/psosim
PSS_BSP_LIB     = $(PSS_BSP)/bsp.a

debug = 1
optimize = 1

CC              = gcc
CXX             = g++
CFLAGS          += -W -Wall -Wpointer-arith -pipe #### -Winline

# Test for template instantiation.
#
include $(ACE_ROOT)/include/makeinclude/platform_g++_common.GNU

CCFLAGS         += $(CFLAGS) $(TEMPLATES_FLAG)

# -w suppresses warnings
# -H tells you which header files are being included
# (switch between these)
#DCFLAGS         += -g
#DCFLAGS         += -g -H
#DCFLAGS         += -g -H -w
DCFLAGS         += -g -w

DLD             = $(CXX)
LD              = $(CXX)
LDFLAGS         += # -z muldefs
LIBS            += $(PSS_SYS_LIBS) $(PSS_BSP_LIB) $(PSS_CFG_DIR)/acepsos.a -lsocket -lnsl -lm
OCFLAGS         += -O2
#OCFLAGS         += -O0
PIC             = -fpic
AR              = ar
ARFLAGS         = rsuv
RANLIB          = @true
SOFLAGS         += -G $(CPPFLAGS)
# SOBUILD - compile into .so directly
SOBUILD         = $(COMPILE.cc) $(PIC) -o $(VSHDIR)$*.so $<
PRELIB          = @true
INCLDIRS        += -I$(PSS_ROOT)/include -I$(PSS_BSP) -I$(ACE_ROOT)/ace
