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

INSERT INTO `mce_sip_domains` 
(domain_name, primary_registrar, secondary_registrar, type) 
VALUES 
('${telephony_server_sip_domain_name}','${telephony_server_sip_primary_registrar}','${telephony_server_sip_secondary_registrar}',${telephony_server_sip_outbound_proxy});
-- ISGO