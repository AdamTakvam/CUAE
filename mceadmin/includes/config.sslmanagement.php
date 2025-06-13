<?php

abstract class SslManagementConfig
{

    const OPENSSL_PATH      = "C:/OpenSSL";
    const APACHE_SSL_PATH   = "C:/Program Files/Apache Group/Apache/conf/ssl";
    const APACHE_CONFIG_PATH= "/System/apache/conf";

    const CONFIG_FILE_BASE  = "/install/openssl_config_base";
    const APACHE_SSL_CONFIG = "/install/apache/conf/ssl.conf";

    const OPENSSL_EXEC      = "openssl.exe";
    const CONFIG_FILE       = "openssl_config";
    const CSR_FILE          = "my-server.csr";
    const PRIVATE_KEY_FILE  = "privkey.pem";
    const PUBLIC_KEY_FILE   = "my-server.key";
    const CERT_FILE         = "my-server.cert";

}

?>