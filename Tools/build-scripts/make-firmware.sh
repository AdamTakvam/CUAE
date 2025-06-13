#!/bin/sh

usage() {
    echo "Usage: make-firmware [--component <componentName>] [--version <versionNumber>] [--product <productName>] --specfile </path/to/specfile> --basedir </path/to/basedir>"
    exit 1
}

build_comps() {
    DESTTOP=$1

    echo build_comps
    rpmbuild -bb --quiet ${INST_COMPSSPECFILE}  2>&1 >/dev/null
    set -
    cp /usr/src/redhat/RPMS/i386/comps*.rpm "${DESTTOP}"/RPMS
    if [ ! -z "$2" ] ; then 
        cp /usr/src/redhat/RPMS/i386/comps*.rpm "${DESTTOP}"/base/comps.rpm
    fi
    rm -f /usr/src/redhat/RPMS/i386/comps*.rpm

}

INST_COMPONENT="Metreos"
INST_PRODUCT="Metreos PhoneProxy"
INST_VERSION="1.0"

while [ $# -gt 0 ]; do
    case $1 in
    --component)
        INST_COMPONENT=$2
        shift;shift;
        ;;
    --product)
        INST_PRODUCT=$2
        shift;shift;
        ;;
    --version)
        INST_VERSION=$2
        shift;shift;
        ;;
    --specfile)
        INST_COMPSSPECFILE=$2
        shift;shift;
        ;;
    --basedir)
        INST_BASE=$2
        shift;shift;
        ;;
    *)
        usage
        ;;
    esac
done

INST_RELEASE="${INST_PRODUCT} ${INST_RELEASE}"

if [ -z "${INST_COMPSSPECFILE}" -o ! -f "${INST_COMPSSPECFILE}" ] ; then
    usage
fi

if [ -z "${INST_BASE}" -o ! -d "${INST_BASE}" ]; then
    usage
fi

for i in `find "${INST_BASE}" -name TRANS.TBL`; do rm -f $i; done

echo genhdlist
genhdlist --productpath=Fedora --withnumbers "${INST_BASE}"
if [ $? -gt 0 ] ; then 
    exit $? 
fi

touch "${INST_BASE}"/.discinfo
build_comps "${INST_BASE}"/Fedora

echo genhdlist
genhdlist --productpath=Fedora --withnumbers "${INST_BASE}"
if [ $? -gt 0 ] ; then 
    exit $? 
fi

echo pkgorder
pkgorder "${INST_BASE}" base Fedora > "${INST_BASE}/pkgorder"
if [ $? -gt 0 ] ; then 
    exit $? 
fi

echo buildinstall
buildinstall --pkgorder "${INST_BASE}/pkgorder" --version "${INST_VERSION}" --comp "${INST_COMPONENT}" --product "${INST_PRODUCT}" --release "${INST_RELEASE}" --prodpath Fedora "${INST_BASE}"
if [ $? -gt 0 ] ; then 
    exit $? 
fi

build_comps "${INST_BASE}"/Fedora base
