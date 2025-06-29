This sample illustrates use of ElSSHClient and supplementary classes 
(TElShellSSHTunnel and TElSSHTunnelConnection) with .NET FRamework sockets 
sockets for tunneling local-initiated (client) connection to remote host via 
SSH tunnel. 

Only password-based authentication is supported. Keyboard-interactive and 
key-based authentication are not implemented. 

To use the demo you must have the configured SSH Server available. The sample 
will connect to this server and create secured tunnel between your computer 
(running the sample) and the server. The server in turn will connect to 
specified remote address/port. And when your client connects locally to the 
sample, it will reach remote address/port via secured tunnel. 