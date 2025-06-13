Metreos Communications
AutoBuild and Developer Workspace Setup

Last Updated: 09/24/2004 (marascio)

----

================================================================================
TODO
================================================================================

1. Add a target for validating the developer's workspace set (i.e., all 
   necessary projects are checked out, etc).


================================================================================
OVERVIEW
================================================================================

This auto-build script will build the entire Metreos product line. An open
source tool called NAnt is used to managed the command line build. Batch files
are invoked to build each solution, allowing Visual Studio to still handle the
internal logistics of each build.

The build script can run in manual or automatic (read: nightly) mode. Invoked
from the command line, the script will create a directory structure under 
X:\Build that will mirror the normal installed (end-user) directory structure.

To execute a manual build, simply invoke 'build.bat'. The build will execute 
and output status messages as it works its way through the process. Should an
error occur, log files can be found in 'X:\Build\BuildLogs'.

Nightly builds will automatically update all of the source files for the
required projects, build the projects in debug and release mode, and archive
the build output into folders according to: X:\Nightly\{BuildNum}.  Also, the 
build script will copy the latest nightly build to the S:\Builds drive for
easy consumption.


Valid build targets are:

Target Name                          Description
------------------------------------------------
debug (default)                      Builds everything in debug mode.
release                              Builds everything in release mode.
clean                                Cleans all projects.

rebuild                              Same as 'debug-rebuild'.
debug-rebuild                        Clean and rebuild the 'debug' target.
release-rebuild                      Clean and rebuild the 'release' target.

debug-apps                           Builds all MCA applications, debug mode.
release-apps                         Builds all MCA applications, release mode.

nightly                              Execute a full nightly build.

debug-appserver                      Complete application server, debug mode.
release-appserver                    Complete application server, release mode.

debug-samoa-addins                   Samoa Addins only, debug mode.
release-samoa-addins                 Samoa Addins only, release mode.

debug-samoa-tests                    Samoa functional tests only, debug mode.
release-samoa-tests                  Samoa functional tests only, release mode.

debug-management                     Management console only, debug mode.
release-management                   Management console only, release mode.

debug-mediaserver                    Media server only, debug mode.
release-mediaserver                  Media server only, release mode.

debug-max                            MAX Designer only, debug mode.
release-max                          MAX Designer only, release mode.


================================================================================
SETUP PRE-REQUESITES
================================================================================

1. The local workspace directory (i.e., the place where your source tree
   lives) must be mapped, using the 'subst' command, to the x:\ drive.

2. NAnt must be installed in X:\Tools\Nant.

3. 3rd party software (ACE, OpenH323, etc) must be in 'x:\Contrib'.


================================================================================
METREOS AUTOBUILD FILE MANIFEST
================================================================================

Name                                 Description
------------------------------------------------
metreos.build.xml                    Primary NAnt build script. Executes all
                                     build tasks in appropriate order. NAnt
                                     is required to run this script.
                                     
vs-net-task.bat                      Simple batch file that is invoked from
                                     NAnt to build VS.NET solutions. All
                                     inter-solution dependencies, etc are
                                     handled by Visual Studio and must be
                                     set in the solution properly.
                                     
vs-6-task.bat                        Same as above except targeted as Visual
                                     Studio 6.0.
                                     
DevDirSetup.reg                      Simple registry file that will add the
                                     required 'subst' command to the machine
                                     startup. This will cause c:\workspace
                                     to be mapped to the virtual drive 'x:'.

MetreosFrameworkDirSetup.reg         Registry file that will add the Metreos
                                     framework files that are inside the directory
                                     "Framework\1.0\CoreAssemblies"
                                     to .NET's assemlbly resolution path. This
                                     allows these components to be referenced
                                     easily from Visual Studio .NET.
                                     
build.bat                            Primary build script to bootstrap the
                                     build process. Sets up initial 
                                     environment variables, paths, etc and
                                     then invokes the NAnt build process.

README.txt                           This text file.

builder\                             Contains files that support the nightly
                                     build and integration machine.

contrib\                             Contains files that are used to automatically
                                     build contributed source files.

