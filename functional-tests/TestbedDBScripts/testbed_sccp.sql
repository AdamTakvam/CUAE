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

INSERT INTO `mce_call_manager_cluster_subscribers` (name, ip_address, mce_call_manager_clusters_id) VALUES 
('${sccp_name}','${sccp_address}',(SELECT mce_call_manager_clusters_id FROM `mce_call_manager_clusters` 
WHERE publisher_ip_address = '${sccp_address}'));
-- ISGO