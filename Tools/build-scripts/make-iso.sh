#!/bin/sh

usage() {
    echo "Usage: make-iso [--label <string>] [--contact <email>] [--output </path/to/iso] </path/to/data>" >&2
    exit 1
}

ISO_LABEL="Metreos ISO"
ISO_CONTACT="support@metreos.com"
ISO_OUTPUT="metreos.iso"
ISO_BOOTABLE=true

while [ $# -gt 0 ]; do
    case $1 in
    --label)
        ISO_LABEL=$2
        shift; shift;
        ;;
    --contact)
        ISO_CONTACT=$2
        shift;shift;
        ;;
    --output)
        ISO_OUTPUT=$2
        shift;shift;
        ;;
    --bootable)
        ISO_BOOTABLE=$2
        shift;shift;
        ;;
    *)
        if [ -n "$DATADIR" -o ! -f $1/isolinux/isolinux.bin ]; then
            usage
        fi
        DATADIR=$1
        shift
        ;;
    esac
done

if [ -z "$DATADIR" ]; then
    usage
fi

ISO_OPTIONS="-J -r -T -v"
if [ "${ISO_BOOTABLE}" == "true" ]; then
    ISO_OPTIONS="${ISO_OPTIONS} -b isolinux/isolinux.bin -c isolinux/boot.cat -no-emul-boot -boot-load-size 4 -boot-info-table"
fi

echo "mkisofs ${ISO_OPTIONS} -p '${ISO_CONTACT}' -V '${ISO_LABEL}' -A '${ISO_LABEL}' -o '${ISO_OUTPUT}' ${DATADIR}"
mkisofs ${ISO_OPTIONS} -p "${ISO_CONTACT}" -V "${ISO_LABEL}" -A "${ISO_LABEL}" -o "${ISO_OUTPUT}" ${DATADIR}
implantisomd5 "${ISO_OUTPUT}"
checkisomd5 --verbose "${ISO_OUTPUT}"
