#!/bin/sh

usage() {
    echo "huh?"
}

fiximage() {
    base=$1
    image=$2
    patchpath=$3/firmware/build/patch
    

    tmpdir=/tmp/fix-${image}.$$
    tmpimg=/tmp/fix-${image}.img.$$

    echo Patching ${base}/Fedora/base/${image}.img ...
    mkdir -p $tmpdir
    mount -o loop ${base}/Fedora/base/${image}.img /mnt
    rsync -a /mnt/ $tmpdir/
    umount /mnt
    # DO Fixes
    cp /usr/lib/python2.4/encodings/ascii.py ${tmpdir}/usr/lib/python2.4/encodings
    cp ${INST_BUILDROOT}/firmware/patches/*.py ${tmpdir}/usr/lib/anaconda/

    if [ ! -z "${patchpath}" ] ; then
        if [ ! -f ${tmpdir}/usr/bin/anaconda.orig ] ; then
            mv ${tmpdir}/usr/bin/anaconda ${tmpdir}/usr/bin/anaconda.orig
        fi 
        rm -f ${tmpdir}/usr/bin/anaconda
        rm -f ${tmpdir}/usr/bin/metreos.py
        rm -f ${tmpdir}/usr/lib/anaconda/metreos.py
        cp ${patchpath}/anaconda.wrapper ${tmpdir}/usr/bin/anaconda
        cp ${patchpath}/metreos.py ${tmpdir}/usr/bin/metreos.py
        cp ${INST_BUILDROOT}/Contrib/partimage/partimage ${tmpdir}/usr/bin/partimage
        chmod +x ${tmpdir}/usr/bin/anaconda
    fi
    # Make new image and replace
    mkcramfs ${tmpdir} ${tmpimg}
    mv ${tmpimg} ${base}/Fedora/base/${image}.img
    rm -rf $tmpdir
    rm -rf $tmpimg
}

while [ $# -gt 0 ]; do
    case $1 in
    --basedir)
        INST_BASE=$2
        shift;shift;
        ;;
    --buildroot)
        INST_BUILDROOT=$2
        shift;shift;
        ;;
    --image)
        INST_IMAGE=$2
        shift;shift;
        ;;
    *)
        usage
        ;;
    esac
done

if [ -z "${INST_BASE}" -o ! -d "${INST_BASE}" ]; then
    usage
fi

fiximage ${INST_BASE} ${INST_IMAGE} ${INST_BUILDROOT}
