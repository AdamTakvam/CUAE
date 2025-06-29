-- The ISGO lines are needed so that Installshield knows when to send a query to
-- the ODBC driver when installing.  The MySQL ODBC driver understands only
-- one query at a time.  So, generally speaking, you put "-- ISGO" on its own line
-- after each query terminated with a ";".


-- SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT;
-- ISGO
-- SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS;
-- ISGO
-- SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION;
-- ISGO
-- SET NAMES utf8;
-- ISGO

INSERT INTO `mce_call_manager_clusters` 
(name, version, publisher_ip_address, publisher_username, publisher_password, snmp_community, description) 
VALUES 
('${testbed.telephony_server_ccm_name}','${testbed.telephony_server_ccm_version}','${testbed.telephony_server_ccm_publisher_address}',
'${testbed.telephony_server_ccm_adminstrator}','${testbed.telephony_server_ccm_administrator_password}',
'${testbed.telephony_server_ccm_snmp_community}','${testbed.telephony_server_ccm_description}');
-- ISGO