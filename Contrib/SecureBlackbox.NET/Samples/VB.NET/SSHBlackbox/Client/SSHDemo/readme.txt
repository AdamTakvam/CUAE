This sample illustrates use of ElSSHClient and supplementary classes 
(TElShellSSHTunnel and TElSSHTunnelConnection) with .NET Framework sockets for 
shell access to remote host.

Password-based and key-based authentication is supported. Keyboard-interactive 
authentication is not implemented. 

Add the line contained in 'key.pub' file to the end of '~/.ssh/authorized_keys' 
file on a server side. Use 'key' file as a private key file for the SSH demo 
program. 
