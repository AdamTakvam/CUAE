This is a client side of the secure chat sample.

The sample uses System.Net.Sockets.Socket class for networking and 
TElSecureClient component for SSL support. 

Note, that the sample doesn't check the certificate. Also, the server side of 
this chat doesn't present any certificates (so Diffie-Hellman Anonymous cipher 
suite is used). 