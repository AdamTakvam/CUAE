# $Id: Makefile,v 1.9 2004/05/10 01:12:46 jason Exp $
VERSION:=$(shell cat VERSION)
REPRO_USER?=repro

# repro is not GPL so if you want to link in MySQL, you needs a 
# a comercial license for MySQL that is not GPL 
USE_MYSQL = false

BUILD = ../build
include $(BUILD)/Makefile.pre

CXXFLAGS += -I../
PACKAGES += DUM RESIP RUTIL OPENSSL ARES DB4CXX PTHREAD POPT

ifeq ($(USE_MYSQL),yes)
PACKAGES += MYSQL
SRC += MySqlDb.cxx
endif

CODE_SUBDIRS = monkeys stateAgents
TARGET_LIBRARY = librepro
TARGET_BINARY = repro

TESTPROGRAMS = \
#	repro.cxx \
#	genUsers.cxx \
#	userAdmin.cxx 

SRC 	+=   \
	RouteStore.cxx \
	UserStore.cxx \
	ConfigStore.cxx \
	AclStore.cxx \
	Store.cxx \
	AbstractDb.cxx \
	BerkeleyDb.cxx \
	\
	CommandLineParser.cxx \
	ReproVersion.cxx \
	HttpBase.cxx \
	HttpConnection.cxx \
	WebAdmin.cxx \
	WebAdminThread.cxx \
	\
	Proxy.cxx \
	Registrar.cxx \
	ReproServerAuthManager.cxx \
	RequestContext.cxx \
	ResponseContext.cxx \
        Processor.cxx \
        ProcessorChain.cxx \
		  Target.cxx \
		  WorkerThread.cxx \
		  Dispatcher.cxx \
		  QValueTarget.cxx \
	\
        monkeys/DigestAuthenticator.cxx \
	monkeys/StrictRouteFixup.cxx \
	monkeys/AmIResponsible.cxx \
	monkeys/IsTrustedNode.cxx \
	monkeys/ConstantLocationMonkey.cxx \
	monkeys/LocationServer.cxx \
	monkeys/RecursiveRedirect.cxx \
	monkeys/SimpleStaticRoute.cxx \
	monkeys/StaticRoute.cxx \
	monkeys/StrictRouteFixup.cxx \
	monkeys/QValueTargetHandler.cxx \
	monkeys/SimpleTargetHandler.cxx \
	monkeys/SetTargetConnection.cxx \
	\


ifeq ($(USE_SSL), yes)
SRC +=	stateAgents/CertServer.cxx \
	stateAgents/CertPublicationHandler.cxx \
	stateAgents/CertSubscriptionHandler.cxx \
	stateAgents/PrivateKeyPublicationHandler.cxx \
	stateAgents/PrivateKeySubscriptionHandler.cxx
endif

reproSvnVersion=$(shell cat ../SVN-VERSION)
reproBuildTime=$(shell date -u --iso-8601=seconds | sed 's/+0000//')
reproBuildHost=$(shell hostname --short)

.PHONY: BUILDSTAMP
BUILDSTAMP: ../SVN-VERSION
	echo "$(reproSvnVersion) $(reproBuildTime) $(reproBuildHost)" > BUILDSTAMP

ReproVersion.cxx: BUILDSTAMP

CXXFLAGS += "-DREPRO_BUILDSTAMP=\"$(reproSvnVersion) $(reproBuildTime) $(reproBuildHost)\""
CXXFLAGS += "-DREPRO_VERSION=\"$(VERSION)\""

../SVN-VERSION: 
	$(MAKE) -C .. SVN-VERSION

include $(BUILD)/Makefile.post

#####################################################################
# Redhat-esque system configuration install target 
# (Should work with fairly recent versions of Redhat, RHEL, FC)
#
#   Create configuration file: /etc/repro.conf
#   Create rc file: /etc/rc.d/init.d
#   Create db files directory: /var/lib/repro
#

install-rh-config: \
	$(DESTDIR)/etc/repro.conf \
	$(DESTDIR)/etc/init.d/repro \
	$(DESTDIR)/var/lib/repro \
	$(DESTDIR)/var/run/repro \
	$(DESTDIR)$(INSTALL_PREFIX)/share/man/man8/repro.8

$(DESTDIR)/etc/repro.conf:  etc/repro.conf.inst
	if [ -e $@ ]; \
	then \
		echo " Using existing $@"; \
		install -D etc/repro.conf.inst $(DESTDIR)/etc/repro.conf.NEW; \
	else \
		install -D etc/repro.conf.inst $(DESTDIR)/etc/repro.conf; \
	fi

$(DESTDIR)/etc/init.d/repro: etc/init.d/repro.inst
	install -D etc/init.d/repro.inst $(DESTDIR)/etc/init.d/repro

localize = sed \
	-e 's,@REPROUSER@,$(REPRO_USER),g' \
	-e 's,@REPRO_INSTALL@,$(INSTALL_PREFIX),g' \
	-e 's,@REPRO_RUNDIR@,/var/run/repro,g' \
	-e 's,@REPRO_CONFDIR@,/etc,g' \
	-e 's,@REPRO_CWD@,/var/lib/repro,g' \
	-e 's,@bindir@,$(INSTALL_PREFIX)/sbin,g' \
	-e 's,@VERSION@,$(VERSION),g' \

%.inst: %
	$(localize) < $< > $@

$(DESTDIR)/var/run/repro:
	install -d $(DESTDIR)/var/run/repro

$(DESTDIR)/var/lib/repro:
	install -d $(DESTDIR)/var/lib/repro

$(DESTDIR)$(INSTALL_PREFIX)/share/man/man8/repro.8: doc/repro.8
	install -D $< $@

clean: clean-inst clean-build

clean-build:
	rm -f BUILDSTAMP

clean-inst:
	rm -f etc/init.d/repro.inst etc/repro.conf.inst
