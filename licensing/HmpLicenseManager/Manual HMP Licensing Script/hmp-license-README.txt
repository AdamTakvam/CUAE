hmp-license.bat

This script automates the process of applying an HMP license manually.

It takes the same actions as does the CUAE core installer in that regard.

You would use this script rather than the installer if for instance you
want to re-install HMP without going through a CUAE install.

All HMP licensing files are expected to be gathered into a temp directory,
c:\hmplicfiles, before this script is run. See comments in the bat file code
for a list of such files.

The name of the HMP license file is likewise embedded into the bat file code.
When the license file changes, do a search and replace on the bat file code.

When you run the script, it pauses after each licensing step, allowing you 
to observe the results of the action. At each such pause you can hit any key 
to continue, or hit CTRL+C to exit. 