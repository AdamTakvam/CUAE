# Build Environment Initialization for ANT-based builds

find_root() {
    start=`pwd`
    while [ ! -f ${start}/BUILDROOT ] ; do
        start=`dirname ${start}`
        if [ "$start" == "/" ] ; then
            echo "Cannot determine build root"
            exit 1
        fi
    done
    echo $start
}

export MetreosWorkspaceRoot=`find_root`
export MetreosContribRoot=${MetreosWorkspaceRoot}/Contrib
export MetreosToolsRoot=${MetreosWorkspaceRoot}/Tools
if [ -z "${MetreosBuildPlatform}" ] ; then
    export MetreosBuildPlatform=linux
fi    
if [ -z "${MetreosReleaseType}" ] ; then
    export MetreosReleaseType=DEV
fi
if [ -z "${MetreosBuildNumber}" ] ; then
    export MetreosBuildNumber=0000
fi

export ANT_HOME=${MetreosToolsRoot}/apache-ant-1.6.5
export ANTCMD=${ANT_HOME}/bin/ant

#echo ${ANTCMD}
# export PATH=${PATH}:${ANT_HOME}/bin

