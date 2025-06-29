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

INSERT INTO `mce_components` (name, type, status) VALUES 
('${mediaengine_name}', ${component_type}, ${mediaengine_status});
-- ISGO
INSERT INTO mce_component_group_members 
(mce_component_groups_id, mce_components_id) VALUES 
(4,(SELECT max(mce_components_id) FROM `mce_components` WHERE name = '${mediaengine_name}' AND type = 4));
-- ISGO
INSERT INTO `mce_config_entries` 
(mce_components_id, mce_application_partitions_id, mce_config_entry_metas_id) VALUES 
((SELECT max(mce_components_id) FROM `mce_components` WHERE name = '${mediaengine_name}' AND type = 4),0,7),
((SELECT max(mce_components_id) FROM `mce_components` WHERE name = '${mediaengine_name}' AND type = 4),0,37),
((SELECT max(mce_components_id) FROM `mce_components` WHERE name = '${mediaengine_name}' AND type = 4),0,38),
((SELECT max(mce_components_id) FROM `mce_components` WHERE name = '${mediaengine_name}' AND type = 4),0,39);
-- ISGO
INSERT INTO `mce_config_values` (mce_config_entries_id, ordinal_row, value) VALUES 
((select mce_config_entries_id from mce_config_entries where mce_config_entry_metas_id = 7 and mce_components_id = (select max(mce_components_id) from mce_components where name = '${mediaengine_name}' and type = 4)),0,'${mediaengine_ip_address}'),
((select mce_config_entries_id from mce_config_entries where mce_config_entry_metas_id = 37 and mce_components_id = (select max(mce_components_id) from mce_components where name = '${mediaengine_name}' and type = 4)),0,'${mediaengine_password}'),
((select mce_config_entries_id from mce_config_entries where mce_config_entry_metas_id = 38 and mce_components_id = (select max(mce_components_id) from mce_components where name = '${mediaengine_name}' and type = 4)),0,'${connect_type}'),
((select mce_config_entries_id from mce_config_entries where mce_config_entry_metas_id = 39 and mce_components_id = (select max(mce_components_id) from mce_components where name = '${mediaengine_name}' and type = 4)),0,'true');
-- ISGO