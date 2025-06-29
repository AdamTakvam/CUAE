This is a client side of the secure chat sample. The sample uses ROInternetPack 
classes for networking and SSLClientConnection (created via 
SSLClientConnectionFactory) component for SSL support.   

Note, that the sample doesn't check the certificate. Also, the server side of 
this chat doesn't present any certificates (so Diffie-Hellman Anonymous cipher 
suite is used). 