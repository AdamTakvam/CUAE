This is a server side of the secure chat sample.

The sample uses System.Net.Sockets.Socket class for networking and 
TElSecureServer component for SSL support. 

Note, that the sample does not expose certificate management (so Diffie-Hellman
Anonymous cipher suite is used).
