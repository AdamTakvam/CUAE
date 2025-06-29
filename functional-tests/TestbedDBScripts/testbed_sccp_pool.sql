-- The ISGO lines are needed so that Installshield knows when to send a query to
-- the ODBC driver when installing.  The MySQL ODBC driver understANDs only
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

INSERT INTO `mce_components` (name, type, status, version, description) VALUES 
('${sccp_devicepool_name}', '${sccppool_type}','${sccp_devicepool_status}', '${sccppool_version}','${sccppool_description}');
-- ISGO
INSERT INTO `mce_call_manager_cluster_members' (mce_call_manager_clusters_id, mce_components_id) VALUES 
((SELECT mce_call_manager_clusters_id FROM mce_call_manager_clusters WHERE name = '${telephony_server_ccm_name}' AND publisher_ip_address = '${telephony_server_ccm_address}') , 
(SELECT mce_components_id FROM `mce_components` WHERE name = '${sccp_devicepool_name}' AND type = '${sccppool_type}'));
-- ISGO
INSERT INTO mce_component_group_members 
(mce_component_groups_id, mce_components_id) VALUES 
(${addtogroup},(SELECT max(mce_components_id) FROM `mce_components` WHERE name = '${sccp_devicepool_name}' AND type = '${sccppool_type}'));
-- ISGO
INSERT INTO `mce_config_entries` 
(mce_components_id, mce_application_partitions_id, mce_config_entry_metas_id) VALUES 
((SELECT max(mce_components_id) FROM `mce_components` WHERE name = '${sccp_devicepool_name}' AND type = '${sccppool_type}'),0,8),
((SELECT max(mce_components_id) FROM `mce_components` WHERE name = '${sccp_devicepool_name}' AND type = '${sccppool_type}'),0,9),
((SELECT max(mce_components_id) FROM `mce_components` WHERE name = '${sccp_devicepool_name}' AND type = '${sccppool_type}'),0,44),
((SELECT max(mce_components_id) FROM `mce_components` WHERE name = '${sccp_devicepool_name}' AND type = '${sccppool_type}'),0,45),
((SELECT max(mce_components_id) FROM `mce_components` WHERE name = '${sccp_devicepool_name}' AND type = '${sccppool_type}'),0,46);
-- ISGO
INSERT INTO `mce_config_values` (mce_config_entries_id, ordinal_row, value) VALUES 
((SELECT mce_config_entries_id FROM mce_config_entries WHERE mce_config_entry_metas_id = 8 AND mce_components_id = (SELECT max(mce_components_id) FROM mce_components WHERE name = '${sccp_devicepool_name}' AND type = '${sccppool_type}')),0,(SELECT mce_call_manager_cluster_subscribers_id FROM `mce_call_manager_cluster_subscribers` WHERE name = '${sccp_name}' AND ip_address = '${sccp_address}')),
((SELECT mce_config_entries_id FROM mce_config_entries WHERE mce_config_entry_metas_id = 9 AND mce_components_id = (SELECT max(mce_components_id) FROM mce_components WHERE name = '${sccp_devicepool_name}' AND type = '${sccppool_type}')),0,'${secondary_subscriber}'),
((SELECT mce_config_entries_id FROM mce_config_entries WHERE mce_config_entry_metas_id = 44 AND mce_components_id = (SELECT max(mce_components_id) FROM mce_components WHERE name = '${sccp_devicepool_name}' AND type = '${sccppool_type}')),0,'${teritiary_subscriber}'),
((SELECT mce_config_entries_id FROM mce_config_entries WHERE mce_config_entry_metas_id = 45 AND mce_components_id = (SELECT max(mce_components_id) FROM mce_components WHERE name = '${sccp_devicepool_name}' AND type = '${sccppool_type}')),0,'${quaternary_subscriber}'),
((SELECT mce_config_entries_id FROM mce_config_entries WHERE mce_config_entry_metas_id = 46 AND mce_components_id = (SELECT max(mce_components_id) FROM mce_components WHERE name = '${sccp_devicepool_name}' AND type = '${sccppool_type}')),0,'${srst}');
-- ISGO