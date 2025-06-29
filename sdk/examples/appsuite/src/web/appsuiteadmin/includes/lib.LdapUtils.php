<?php

abstract class LdapUtils
{

    function make_connection($host, $port, $secure, $user, $password, ErrorHandler $errors)
    {
        if ($secure)
            $host = "ldaps://" . $host;
        $ldap_conn = @ldap_connect($host, $port);
        if ($ldap_conn)
        {
            $ldap_bind = @ldap_bind($ldap_conn, $user, $password);
            if (!$ldap_bind)
            {
                $errors->Add("Could not bind to the LDAP server.  Check the configured User DN and password for this server.");
                return FALSE;
            }
        }
        else
        {
            $errors->Add("Could not connect to the LDAP server.  Please check the hostname and port of the server.");
            return FALSE;
        }
        return $ldap_conn;
    }

}

?>