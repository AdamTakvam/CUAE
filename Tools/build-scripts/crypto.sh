#!/bin/bash
# $Id: crypto.sh 22629 2006-05-11 06:17:48Z builder $
#
#  Crypto functions for use by build.common.xml
#
#  Copyright 2006 (c) - Metreos Corporation
#

gpg_encrypt="--passphrase-fd 0 --batch --no-tty --cipher-algo AES256 --output - --symmetric"
gpg_decrypt="--passphrase-fd 0 --batch --no-tty --decrypt"
gpg_sign="--passphrase-fd 0 --batch --no-tty --armor --detach-sign --output -"
gpg_verify="--batch --no-tty --verify"
gpg_sha512="--no-tty --batch  --print-md sha512"

function doSign {
    local key=$1
    local passphrase=$2
    local src=$3
    local dest=$4

    local tmp=/tmp/sha512.$$
    gpg ${gpg_sha512} ${src} > ${tmp} 2>/dev/null   
    echo ${passphrase} | gpg ${gpg_sign} -u ${key} ${tmp} > ${dest} 2>/dev/null
    rm -f $tmp
}

function doVerify {
    local src=$1
    local sig=$2

    local tmp=/tmp/sha512.$$
    gpg ${gpg_sha512} ${src} > ${tmp} 2>/dev/null
    gpg ${gpg_verify} ${sig} ${tmp} 2>/dev/null
    rc=$?
    rm -f ${tmp}
    if [ $rc -ne 0 ]; then
        echo "ERROR - invalid signature"
        exit 1
    fi
}

function doEncrypt {
    local keyfile=$1
    local src=$2
    local dest=$3
    cat ${keyfile} | gpg ${gpg_encrypt} ${src} > ${dest} 2>/dev/null
}

function doDecrypt {
    local keyfile=$1
    local src=$2
    local dest=$3
    cat ${keyfile} | gpg ${gpg_decrypt} ${src} > ${dest} 2>/dev/null
}

case "$1" in
    encrypt)
        keyfile="$2"
        src="$3"
        dest="$4"

        if [ -z "$dest" ]; then
            dest=${src}.bin
        fi
        if [ ! -f "$src" ]; then
            echo "ERROR - no such file '$src'"
            exit 1
        fi
        doEncrypt "$keyfile" "$src" "$dest"
        ;;
    decrypt)
        keyfile="$2"
        src="$3"
        dest="$4"

        if [ -z "$dest" ]; then
            dest="output.blah"
        fi
        if [ ! -f "$src" ]; then
            echo "ERROR - no such file '$src'"
            exit 1
        fi
        doDecrypt "$keyfile" "$src" "$dest"
        ;;
    sign)
        key="$2"
        passphrase="$3"
        src="$4"
        dest="$5"

        doSign "$key" "$passphrase" "$src" "$dest"
        ;;
    verify)
        src="$2"
        sig="$3"
        
        doVerify "$src" "$sig"
        ;;
    *)
        echo "ERROR - unknown option '$1'"
        exit 1
        ;;
esac
exit 0
