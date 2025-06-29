use application_suite;
update as_users set ldap_synched = 0;
update as_users set external_auth_enabled = 0;
update as_users set as_ldap_servers_id = NULL;
update as_users set external_auth_dn = NULL;
