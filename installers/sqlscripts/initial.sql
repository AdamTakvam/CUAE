-- The ISGO lines are needed so that Installshield knows when to send a query to
-- the ODBC driver when installing.  The MySQL ODBC driver understands only
-- one query at a time.  So, generally speaking, you put "-- ISGO" on its own line
-- after each query terminated with a ";".


SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT;
-- ISGO
SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS;
-- ISGO
SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION;
-- ISGO
SET NAMES utf8;
-- ISGO

CREATE DATABASE IF NOT EXISTS `mce_standby`;
-- ISGO
CREATE DATABASE IF NOT EXISTS `mce`;
-- ISGO
USE `mce`;
-- ISGO


DROP TABLE IF EXISTS `mce_application_partitions`;
-- ISGO
CREATE TABLE `mce_application_partitions` (
  `mce_application_partitions_id` int(10) unsigned NOT NULL auto_increment,
  `mce_components_id` int(10) unsigned NOT NULL default '0',
  `name` varchar(128) NOT NULL default '',
  `description` text,
  `enabled` tinyint(1) unsigned NOT NULL default '0',
  `created_timestamp` timestamp NOT NULL default CURRENT_TIMESTAMP,
  `preferred_codec` varchar(128) NOT NULL default '',
  `locale` varchar(128) NOT NULL default 'en-US',
  `use_early_media` tinyint(1) unsigned NOT NULL default '0',
  `mce_call_route_group_id` int(10) unsigned NOT NULL default '0',
  `mce_alarm_group_id` int(10) unsigned NOT NULL default '0',
  `mce_media_resource_group_id` int(10) unsigned NOT NULL default '0',
  PRIMARY KEY  (`mce_application_partitions_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO


DROP TABLE IF EXISTS `mce_application_script_trigger_parameters`;
-- ISGO
CREATE TABLE `mce_application_script_trigger_parameters` (
  `mce_application_script_trigger_parameters_id` int(10) unsigned NOT NULL auto_increment,
  `mce_application_scripts_id` int(10) unsigned NOT NULL default '0',
  `mce_application_partitions_id` int(10) unsigned NOT NULL default '0',
  `name` varchar(128) NOT NULL default '',
  PRIMARY KEY  (`mce_application_script_trigger_parameters_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO


DROP TABLE IF EXISTS `mce_application_scripts`;
-- ISGO
CREATE TABLE `mce_application_scripts` (
  `mce_application_scripts_id` int(10) unsigned NOT NULL auto_increment,
  `event_type` varchar(128) NOT NULL default '',
  `name` varchar(128) NOT NULL default '',
  `uses_call_control` tinyint(1) unsigned NOT NULL default '0',
  `uses_media_control` tinyint(1) unsigned NOT NULL default '0',
  `mce_components_id` int(10) unsigned NOT NULL default '0',
  PRIMARY KEY  (`mce_application_scripts_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO


DROP TABLE IF EXISTS `mce_backups`;
-- ISGO
CREATE TABLE `mce_backups` (
  `mce_backups_id` int(10) unsigned NOT NULL auto_increment,
  `backup_date` timestamp NOT NULL default CURRENT_TIMESTAMP on update CURRENT_TIMESTAMP,
  `name` varchar(128) NOT NULL default '',
  `status` int(10) unsigned NOT NULL default '0',
  PRIMARY KEY  (`mce_backups_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO


DROP TABLE IF EXISTS `mce_call_manager_cluster_cti_managers`;
-- ISGO
CREATE TABLE `mce_call_manager_cluster_cti_managers` (
  `mce_call_manager_cluster_cti_managers_id` int(10) unsigned NOT NULL auto_increment,
  `mce_call_manager_clusters_id` int(10) unsigned NOT NULL default '0',
  `name` varchar(255) NOT NULL default '',
  `ip_address` varchar(16) NOT NULL default '',
  PRIMARY KEY  (`mce_call_manager_cluster_cti_managers_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO


DROP TABLE IF EXISTS `mce_call_manager_cluster_members`;
-- ISGO
CREATE TABLE `mce_call_manager_cluster_members` (
  `mce_call_manager_clusters_id` int(10) unsigned NOT NULL default '0',
  `mce_components_id` int(10) unsigned NOT NULL default '0',
  PRIMARY KEY  (`mce_call_manager_clusters_id`,`mce_components_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO


DROP TABLE IF EXISTS `mce_call_manager_cluster_subscribers`;
-- ISGO
CREATE TABLE `mce_call_manager_cluster_subscribers` (
  `mce_call_manager_cluster_subscribers_id` int(10) unsigned NOT NULL auto_increment,
  `mce_call_manager_clusters_id` int(10) unsigned NOT NULL default '0',
  `name` varchar(128) NOT NULL default '',
  `ip_address` varchar(16) NOT NULL default '',
  PRIMARY KEY  (`mce_call_manager_cluster_subscribers_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO


DROP TABLE IF EXISTS `mce_call_manager_clusters`;
-- ISGO
CREATE TABLE `mce_call_manager_clusters` (
  `mce_call_manager_clusters_id` int(10) unsigned NOT NULL auto_increment,
  `name` varchar(128) NOT NULL default '',
  `version` varchar(16) NOT NULL default '',
  `description` text,
  `publisher_ip_address` varchar(16) NOT NULL default '',
  `publisher_username` varchar(128) NOT NULL default '',
  `publisher_password` varchar(128) NOT NULL default '',
  `snmp_community` varchar(128) NOT NULL default '',
  PRIMARY KEY  (`mce_call_manager_clusters_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO


DROP TABLE IF EXISTS `mce_call_manager_devices`;
-- ISGO
CREATE TABLE `mce_call_manager_devices` (
  `mce_call_manager_devices_id` int(10) unsigned NOT NULL auto_increment,
  `mce_components_id` int(10) unsigned NOT NULL default '0',
  `device_name` varchar(32) NOT NULL default '',
  `directory_number` int(10) unsigned NOT NULL default '0',
  `status` int(10) unsigned NOT NULL default '0',
  `device_type` int(10) unsigned NOT NULL default '0',
  PRIMARY KEY  (`mce_call_manager_devices_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO


DROP TABLE IF EXISTS `mce_component_group_members`;
-- ISGO
CREATE TABLE `mce_component_group_members` (
  `mce_components_id` int(10) unsigned NOT NULL default '0',
  `mce_component_groups_id` int(10) unsigned NOT NULL default '0',
  `ordinal` INTEGER(10) UNSIGNED NOT NULL default '0',
  PRIMARY KEY  (`mce_components_id`,`mce_component_groups_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO

INSERT INTO `mce_component_group_members` VALUES
(1,1,0),
(2,1,0),
(3,1,0),
(4,1,0),
(5,1,0),
(6,1,0),
(7,1,0),
(8,1,0);
-- ISGO


DROP TABLE IF EXISTS `mce_component_groups`;
-- ISGO
CREATE TABLE `mce_component_groups` (
  `mce_component_groups_id` int(10) unsigned NOT NULL auto_increment,
  `name` varchar(128) NOT NULL default '',
  `component_type` int(10) unsigned NOT NULL default '0',
  `default_group` tinyint(1) unsigned NOT NULL default '0',
  `description` text,
  `mce_alarm_group_id` int(10) unsigned NOT NULL default '0',
  `mce_failover_group_id` int(10) unsigned NOT NULL default '0',
  PRIMARY KEY  (`mce_component_groups_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO

INSERT INTO `mce_component_groups` VALUES
(1,'Core',1,1,'Core MCE appliance components',6,0),
(2,'Applications',2,1,'Applications installed on the application server',6,0),
(3,'Providers',3,1,'Protocol providers loaded on the application server',6,0),
(4,'Default',4,1,'Default media resource group',6,0),
(5,'Default',50,1,'Default alarm group',0,0),
(6,'Default SCCP',100,1,'Default SCCP Device Pool resource group',6,0),
(7,'Default H.323',101,1,'Default H323 Gateway resource group',6,0),
(8,'Default SIP',102,1,'Default SIP resource group',6,0),
(9,'Default CTI',103,1,'Default CTI Server route resource group',6,0);
-- ISGO

DROP TABLE IF EXISTS `mce_components`;
-- ISGO
CREATE TABLE `mce_components` (
  `mce_components_id` int(10) unsigned NOT NULL auto_increment,
  `name` varchar(128) NOT NULL default '',
  `display_name` varchar(128) NOT NULL default '',
  `type` int(10) unsigned NOT NULL default '0',
  `status` int(10) unsigned NOT NULL default '0',
  `version` varchar(16) NOT NULL default '1.0',
  `copyright` varchar(128) default '',
  `description` text,
  `author` varchar(128) default '',
  `author_url` varchar(128) default '',
  `support_url` varchar(128) default '',
  `created_timestamp` timestamp NOT NULL default CURRENT_TIMESTAMP,
  PRIMARY KEY  (`mce_components_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO

INSERT INTO `mce_components` VALUES
(1,'ApplicationServer','Application Server',1,4,'2.4','2002-2007 Cisco Systems, Inc.','Core runtime','Cisco Systems, Inc.','http://www.cisco.com','http://www.cisco.com',CURRENT_TIMESTAMP),
(2,'Router','',1,4,'2.4','2002-2007 Cisco Systems, Inc.','Action/Event router','Cisco Systems, Inc.','http://www.cisco.com','http://www.cisco.com',CURRENT_TIMESTAMP),
(3,'AppManager','Application Manager',1,4,'2.4','2002-2007 Cisco Systems, Inc.','Application Manager','Cisco Systems, Inc.','http://www.cisco.com','http://www.cisco.com',CURRENT_TIMESTAMP),
(4,'ApplicationEnvironment','Application Environment',1,4,'2.4','2002-2007 Cisco Systems, Inc.','Application Runtime Environment','Cisco Systems, Inc.','http://www.cisco.com','http://www.cisco.com',CURRENT_TIMESTAMP),
(5,'ProviderManager','Provider Manager',1,4,'2.4','2002-2007 Cisco Systems, Inc.','Provider Manager','Cisco Systems, Inc.','http://www.cisco.com','http://www.cisco.com',CURRENT_TIMESTAMP),
(6,'Logger','',1,4,'2.4','2002-2007 Cisco Systems, Inc.','Configurable trace logger','Cisco Systems, Inc.','http://www.cisco.com','http://www.cisco.com',CURRENT_TIMESTAMP),
(7,'Management','Management Interface',1,4,'2.4','2002-2007 Cisco Systems, Inc.','The application server interface for the management console','Cisco Systems, Inc.','http://www.cisco.com','http://www.cisco.com',CURRENT_TIMESTAMP),
(8,'TelephonyManager','Telephony Manager',1,4,'2.4','2002-2007 Cisco Systems, Inc.','Telephony Manager','Cisco Systems, Inc.','http://www.cisco.com','http://www.cisco.com',CURRENT_TIMESTAMP),
(9,'ClusterInterface','Cluster Interface',1,4,'2.4','2002-2007 Cisco Systems, Inc.','Cluster Interface','Cisco Systems, Inc.','http://www.cisco.com','http://www.cisco.com',CURRENT_TIMESTAMP),
(10,'LogServer','Logging Service',5,4,'2.4','2002-2007 Cisco Systems, Inc.','Logging Service','Cisco Systems, Inc.','http://www.cisco.com','http://www.cisco.com',CURRENT_TIMESTAMP),
(12,'LicenseManager','License Manager',1,4,'2.4','2005-2007 Cisco Systems, Inc.','License Manager','Cisco Systems, Inc.','http://www.cisco.com','http://www.cisco.com',CURRENT_TIMESTAMP);
-- ISGO


DROP TABLE IF EXISTS `mce_config_entries`;
-- ISGO
CREATE TABLE `mce_config_entries` (
  `mce_config_entries_id` int(10) unsigned NOT NULL auto_increment,
  `mce_components_id` int(10) unsigned default '0',
  `mce_application_partitions_id` int(10) unsigned default '0',
  `mce_config_entry_metas_id` int(10) unsigned NOT NULL default '0',
  PRIMARY KEY  (`mce_config_entries_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO

INSERT INTO `mce_config_entries` VALUES
(1, 1, 0, 1),       -- AppServer.LogLevel
(2, 1, 0, 101),     -- AppServer.ServerName
(3, 2, 0, 1),       -- Router.LogLevel
(4, 2, 0, 106),     -- Router.DefaultActionTimeout
(7, 3, 0, 1),       -- AppManager.LogLevel
(8, 3, 0, 117),     -- AppManager.DebugListenPort
(9, 4, 0, 1),       -- AppEnvironment.LogLevel
(10, 4, 0, 108),    -- AppEnvironment.ShutdownTimeout
(11, 4, 0, 105),    -- AppEnvironment.MaxThreads
(12, 5, 0, 1),      -- ProviderManager.LogLevel
(13, 5, 0, 102),    -- ProviderManager.ShutdownTimeout
(14, 5, 0, 103),    -- ProviderManager.StartupTimeout
(20, 6, 0, 115),    -- Logger.TcpLoggerLevel
(21, 6, 0, 116),    -- Logger.TcpLoggerPort
(22, 7, 0, 1),      -- IPC.LogLevel
(23, 7, 0, 118),    -- IPC.ManagementPort
(24, 8, 0, 1),      -- TelephonyManager.LogLevel
(25, 10, 0, 119),   -- LogServer.FilePath
(26, 10, 0, 114),   -- LogServer.MaxFileLogLines
(27, 10, 0, 120),   -- LogServer.MaxFiles
(28, 10, 0, 121),   -- LogServer.ListenPort
(29, 6, 0, 122),    -- Logger.LogServerSinkLevel
(30, 8, 0, 123),    -- TelephonyManager.SandboxEnabled
(31, 3, 0, 107),    -- AppManager.DefaultLocale
-- RTP Relay configs formerly here --
(45, 6, 0, 124),    -- Logger.EnableLoggerQueueDiag
(46, 9, 0, 1),      -- ClusterInterface.LogLevel
(47, 8, 0, 125),    -- TelephonyManager.DiagCallTable
(48, 12, 0, 1)		-- LicenseManager.LogLevel
;
-- ISGO


DROP TABLE IF EXISTS `mce_config_entry_metas`;
-- ISGO
CREATE TABLE `mce_config_entry_metas` (
  `mce_config_entry_metas_id` int(10) unsigned NOT NULL auto_increment,
  `name` varchar(128) NOT NULL default '',
  `display_name` varchar(128) NOT NULL default '',
  `min_value` int(11) default NULL,
  `max_value` int(11) default NULL,
  `read_only` tinyint(1) NOT NULL default '0',
  `required` tinyint(1) NOT NULL default '1',
  `description` text,
  `component_type` int(10) unsigned NOT NULL default '0',
  `mce_format_types_id` int(10) unsigned NOT NULL default '0',
  PRIMARY KEY  (`mce_config_entry_metas_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO

INSERT INTO `mce_config_entry_metas` VALUES
(1,'MetreosReserved_LogLevel','Log Level',NULL,NULL,0,1,'Filters all debug output below the specified level',3,100),
(2,'MetreosReserved_EmailRecipient','Recipient',NULL,NULL,0,1,'The address to send email messages to',50,1),
(3,'MetreosReserved_EmailSender','Sender',NULL,NULL,0,1,'The email address to use as the sender of the email',50,1),
(4,'MetreosReserved_EmailServer','Server',NULL,NULL,0,1,'SMTP Server address',50,1),
(5,'MetreosReserved_EmailUsername','Username',NULL,NULL,0,0,'Name to use for outbound SMTP authentication',50,1),
(6,'MetreosReserved_EmailPassword','Password',NULL,NULL,0,0,'Password to use for SMTP authentication',50,9),
(7,'MetreosReserved_Address','Address',NULL,NULL,1,1,'Address to the Media Engine',4,5),
(8,'MetreosReserved_PrimarySubscriberId','Primary Subscriber',NULL,NULL,0,1,'Primary subscriber for the SCCP Device Pool',100,3),
(9,'MetreosReserved_SecondarySubscriberId','Secondary Subscriber',NULL,NULL,0,0,'Secondary subscriber for the SCCP Device Pool',100,3),
(10,'MetreosReserved_IPAddress','Address',NULL,NULL,0,1,'Address to the H.323 Gateway',101,5),
(11,'MetreosReserved_PrimaryCtiManagerId','Primary CTI Manager',NULL,NULL,0,1,'Primary CTI Manager for the CTI Device Pool',103,3),
(12,'MetreosReserved_SecondaryCtiManagerId','Secondary CTI Manager',NULL,NULL,0,0,'Secondary CTI Manager for the CTI Device Pool',103,3),
(13,'MetreosReserved_Username','Username',NULL,NULL,0,1,'Username to allow monitoring of the CTI Device Pool',103,1),
(14,'MetreosReserved_Password','Password',NULL,NULL,0,1,'Password for monitoring the CTI Device Pool',103,9),
(15,'MetreosReserved_PrimaryCtiManagerId','Primary CTI Manager',NULL,NULL,0,1,'Primary CTI Manager for the CTI Route Point',104,3),
(16,'MetreosReserved_SecondaryCtiManagerId','Secondary CTI Manager',NULL,NULL,0,0,'Secondary CTI Manager for the CTI Route Point',104,3),
(17,'MetreosReserved_Username','Username',NULL,NULL,0,1,'Username to allow monitoring of the CTI Route Point',104,1),
(18,'MetreosReserved_Password','Password',NULL,NULL,0,1,'Password for monitoring the CTI Route Point',104,9),
(19,'MetreosReserved_EmailPort','Server Port',NULL,NULL,0,1,'SMTP Server port (Usually 25)',50,3),
(20,'MetreosReserved_TriggerLevel','Trigger Level',NULL,NULL,0,1,'Error trigger level(s) for the alarm',50,101),
(21,'MetreosReserved_ManagerIp','SNMP Manager',NULL,NULL,0,1,'Address to the SNMP manager',51,5),
(22,'MetreosReserved_TriggerLevel','Trigger Level',NULL,NULL,0,1,'Error trigger level(s) for the alarm',51,101),
-- RTP Relay configs formerly here --
-- Additional media engine configs
(37,'MetreosReserved_Password','Password',NULL,NULL,0,1,'Password to access the Media Engine',4,9),
(38,'MetreosReserved_ConnectionType','Connection Type',NULL,NULL,0,1,'Connection method to the Media Engine',4,102),
(39,'HasMedia','Has Media',NULL,NULL,0,0,'Indicates that media has been provisioned to this server',4,2),
-- Configs for SIP Device Pools and Trunk Interface
(40,'MetreosReserved_Username','Username',NULL,NULL,0,1,'Username to allow monitoring the SIP Device Pool',102,1),
(41,'MetreosReserved_Password','Password',NULL,NULL,0,1,'Password for monitoring the SIP Device Pool',102,9),
(42,'MetreosReserved_OutboundProxyId','Outbound Proxy',NULL,NULL,0,1,'Outbound proxy for the SIP Device Pool',102,3),
(43,'MetreosReserved_RegistrarIpAddress','Registrar Address',NULL,NULL,0,1,'Address of the registrar the trunk interface uses',105,5),
-- Configs for SCCP Device Pools
(44,'MetreosReserved_TertiarySubscriberId','Tertiary Subscriber',NULL,NULL,0,0,'Tertiary subscriber for the SCCP Device Pool',100,3),
(45,'MetreosReserved_QuaternarySubscriberId','Quaternary Subscriber',NULL,NULL,0,0,'Quaternary subscriber for the SCCP Device Pool',100,3),
(46,'MetreosReserved_SRST','SRST',NULL,NULL,0,0,'Subscriber assigned as SRST for the SCCP Device Pool',100,3),
(47,'MetreosReserved_Username','Username',NULL,NULL,0,1,'Username to allow monitoring the SIP Device Pool',106,1),
(48,'MetreosReserved_Password','Password',NULL,NULL,0,1,'Password for monitoring the SIP Device Pool',106,9),
(49,'MetreosReserved_OutboundProxyId','Outbound Proxy',NULL,NULL,0,1,'Outbound proxy for the SIP Device Pool',106,3),
-- Configs for Monitored CTI Device Pools
(50,'MetreosReserved_PrimaryCtiManagerId','Primary CTI Manager',NULL,NULL,0,1,'Primary CTI Manager for the Monitored CTI Devices',107,3),
(51,'MetreosReserved_SecondaryCtiManagerId','Secondary CTI Manager',NULL,NULL,0,0,'Secondary CTI Manager for the Monitored CTI Devices',107,3),
(52,'MetreosReserved_Username','Username',NULL,NULL,0,1,'Username to allow monitoring of the Monitored CTI Devices',107,1),
(53,'MetreosReserved_Password','Password',NULL,NULL,0,1,'Password for monitoring the Monitored CTI Devices',107,9),
(54,'MetreosReserved_SecureConnection','Secure Connnection',NULL,NULL,0,1,'Connect to SMTP Server securely',50,2),
-- Configs for specific components
(101,'ServerName','Server Name',NULL,NULL,0,1,'Unique identifier for this server',0,1),
(102,'ShutdownTimeout','Shutdown Timeout',5000,120000,0,1,'Interval in milliseconds to wait for providers to shut down completely',0,3),
(103,'StartupTimeout','Startup Timeout',5000,120000,0,1,'Interval in milliseconds to wait for providers to start up completely',0,3),
-- (104,'MonsterInterval','GC Interval',1,3600,0,1,'The interval in seconds between advanced garbage collection events',0,3),
(105,'MaxThreads','Max Threads',5,50,0,1,'Represents the max number of actions that can be executed simultaneously',0,3),
(106,'DefaultActionTimeout','Default Action Timeout',1000,30000,0,1,'Interval in milliseconds to wait for providers to respond to an action',0,3),
(107,'DefaultLocale','Default Locale',NULL,NULL,0,1,'Locale which will be applied to all newly-installed applications by default',0,103),
(108,'AppShutdownTimeout','Shutdown Timeout',5,120,0,1,'Interval in seconds to wait for applications to shut down gracefully',0,3),
(114,'MaxFileLogLines','Max File Log Lines',100,1000000,0,1,'The maximum number of lines written to the log file before starting a new file',0,3),
(115,'TcpLoggerLevel','TCP Logger Level',NULL,NULL,0,1,'Filters remote console debug output below specified level',0,100),
(116,'TcpLoggerPort','TCP Logger Port',NULL,NULL,0,1,'Port that the TCP remote console logger remoting server listens on for connections',0,3),
(117,'DebugListenPort','Debug Listen Port',NULL,NULL,0,1,'Port on which the application debugger will listen for connections',0,3),
(118,'ManagementPort','Management Port',NULL,NULL,1,1,'Port on which to listen for management commands',0,3),
(119,'FilePath','File Path',NULL,NULL,0,1,'Log file location',0,1),
(120,'MaxFiles','Max Files',NULL,NULL,0,1,'Maximum number of log files to save before overwriting',0,3),
(121,'ListenPort','Listen Port',NULL,NULL,0,1,'Port on which to listen for incoming log messages',0,3),
(122,'LogServerSinkLevel','Log Server Sink Logger Level',NULL,NULL,0,1,'Filters log server debug output below specified level',0,100),
(123,'SandboxEnabled','Enable Call/Connection Sandboxing',NULL,NULL,0,1,'Clears all remaining calls and media connections created by a script when that script exits',0,2),
(124,'EnableLoggerQueueDiag','Enable Logger Queue Diagnostics',NULL,NULL,0,1,'If enabled, queue size and object generation will be output in log messages', 0, 2),
(125,'OutputDiagnosticCallTable','Enable Periodic Diagnostics',NULL,NULL,0,1,'If enabled, the Telephony Manager will occasionally output diagnostics about calls and performance',0,2);
-- ISGO


DROP TABLE IF EXISTS `mce_config_values`;
-- ISGO
CREATE TABLE `mce_config_values` (
  `mce_config_values_id` int(10) unsigned NOT NULL auto_increment,
  `mce_config_entries_id` int(10) unsigned NOT NULL default '0',
  `ordinal_row` int(10) unsigned default '0',
  `key_column` varchar(64) default NULL,
  `value` text NOT NULL,
  PRIMARY KEY  (`mce_config_values_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO

INSERT INTO `mce_config_values` VALUES
(1, 1, 0, NULL, 'Warning'),     -- AppServer.LogLevel
(2, 2, 0, NULL, 'AppServer'),   -- AppServer.ServerName
(3, 3, 0, NULL, 'Warning'),     -- Router.LogLevel
(4, 4, 0, NULL, '10000'),       -- Router.DefaultActionTimeout
(7, 7, 0, NULL, 'Warning'),     -- AppManager.LogLevel
(8, 8, 0, NULL, '8130'),        -- AppManager.DebugListenPort
(9, 9, 0, NULL, 'Warning'),     -- AppEnvironment.LogLevel
(10, 10, 0, NULL, '30'),        -- AppEnvironment.ShutdownTimeout
(11, 11, 0, NULL, '10'),        -- AppEnvironment.MaxThreads
(12, 12, 0, NULL, 'Warning'),   -- ProviderManager.LogLevel
(13, 13, 0, NULL, '30000'),     -- ProviderManager.ShutdownTimeout
(14, 14, 0, NULL, '30000'),     -- ProviderManager.StartupTimeout
(20, 20, 0, NULL, 'Info'),      -- Logger.TcpLoggerLevel
(21, 21, 0, NULL, '8140'),      -- Logger.TcpLoggerPort
(22, 22, 0, NULL, 'Warning'),   -- IPC.LogLevel
(23, 23, 0, NULL, '8120'),      -- IPC.ManagementPort
(24, 24, 0, NULL, 'Warning'),   -- TelephonyManager.LogLevel
(25, 25, 0, NULL, 'c:\\Program Files\\Metreos\\Logs'), -- LogServer.FilePath
(26, 26, 0, NULL, '4000'),      -- LogServer.MaxFileLogLines
(27, 27, 0, NULL, '50'),        -- LogServer.MaxFiles
(28, 28, 0, NULL, '8400'),      -- LogServer.ListenPort
(29, 29, 0, NULL, 'Info'),      -- Logger.LogServerSinkLevel
(30, 30, 0, NULL, 'false'),     -- TelephonyManager.SandboxEnabled
(31, 31, 0, NULL, 'en-US'),     -- ApplicationEnvironment.DefaultLocale
-- RTP Relay configs formerly here --
(45, 45, 0, NULL, 'false'),     -- Logger.EnableLoggerQueueDiag
(46, 46, 0, NULL, 'Warning'),   -- ClusterInterface.LogLevel
(47, 47, 0, NULL, 'false'),     -- TelephonyManager.DiagCallTable
(48, 48, 0, NULL, 'Warning')	-- LicenseManager.LogLevel
;
-- ISGO


DROP TABLE IF EXISTS `mce_event_log`;
-- ISGO
CREATE TABLE `mce_event_log` (
  `mce_event_log_id` int(10) unsigned NOT NULL auto_increment,
  `type` int(10) unsigned NOT NULL default '0',
  `message_id` int(10) unsigned NOT NULL default '0',
  `message` text,
  `data` text,
  `severity` int(10) unsigned NOT NULL default '0',
  `status` int(10) unsigned NOT NULL default '0',
  `created_timestamp` timestamp NOT NULL default CURRENT_TIMESTAMP,
  `recovered_timestamp` timestamp NOT NULL default '0000-00-00 00:00:00',
  PRIMARY KEY  (`mce_event_log_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO


DROP TABLE IF EXISTS `mce_format_type_enum_values`;
-- ISGO
CREATE TABLE `mce_format_type_enum_values` (
  `mce_format_type_enum_values_id` int(10) unsigned NOT NULL auto_increment,
  `mce_format_types_id` int(10) unsigned NOT NULL default '0',
  `value` varchar(128) NOT NULL default '',
  PRIMARY KEY  (`mce_format_type_enum_values_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO

INSERT INTO `mce_format_type_enum_values` VALUES
(1,100,'Off'),
(2,100,'Error'),
(3,100,'Warning'),
(4,100,'Info'),
(5,100,'Verbose'),
(6,101,'Yellow'),
(7,101,'Red'),
(8,101,'Both'),
(9,102,'IPC'),
(10, 103, 'af-ZA'),
(11, 103, 'ar-AE'),
(12, 103, 'ar-BH'),
(13, 103, 'ar-DZ'),
(14, 103, 'ar-EG'),
(15, 103, 'ar-IQ'),
(16, 103, 'ar-JO'),
(17, 103, 'ar-KW'),
(18, 103, 'ar-LB'),
(19, 103, 'ar-LY'),
(20, 103, 'ar-MA'),
(21, 103, 'ar-OM'),
(22, 103, 'ar-QA'),
(23, 103, 'ar-SA'),
(24, 103, 'ar-SY'),
(25, 103, 'ar-TN'),
(26, 103, 'ar-YE'),
(27, 103, 'az-Cyrl-AZ'),
(28, 103, 'az-Latn-AZ'),
(29, 103, 'be-BY'),
(30, 103, 'bg-BG'),
(31, 103, 'bs-Latn-BA'),
(32, 103, 'ca-ES'),
(33, 103, 'cs-CZ'),
(34, 103, 'cy-GB'),
(35, 103, 'da-DK'),
(36, 103, 'de-AT'),
(37, 103, 'de-CH'),
(38, 103, 'de-DE'),
(39, 103, 'de-LI'),
(40, 103, 'de-LU'),
(41, 103, 'div-MV'),
(42, 103, 'el-GR'),
(43, 103, 'en-029'),
(44, 103, 'en-AU'),
(45, 103, 'en-BZ'),
(46, 103, 'en-CA'),
(47, 103, 'en-GB'),
(48, 103, 'en-IE'),
(49, 103, 'en-JM'),
(50, 103, 'en-NZ'),
(51, 103, 'en-PH'),
(52, 103, 'en-TT'),
(53, 103, 'en-US'),
(54, 103, 'en-ZA'),
(55, 103, 'en-ZW'),
(56, 103, 'es-AR'),
(57, 103, 'es-BO'),
(58, 103, 'es-CL'),
(59, 103, 'es-CO'),
(60, 103, 'es-CR'),
(61, 103, 'es-DO'),
(62, 103, 'es-EC'),
(63, 103, 'es-ES'),
(64, 103, 'es-GT'),
(65, 103, 'es-HN'),
(66, 103, 'es-MX'),
(67, 103, 'es-NI'),
(68, 103, 'es-PA'),
(69, 103, 'es-PE'),
(70, 103, 'es-PR'),
(71, 103, 'es-PY'),
(72, 103, 'es-SV'),
(73, 103, 'es-UY'),
(74, 103, 'es-VE'),
(75, 103, 'et-EE'),
(76, 103, 'eu-ES'),
(77, 103, 'fa-IR'),
(78, 103, 'fi-FI'),
(79, 103, 'fo-FO'),
(80, 103, 'fr-BE'),
(81, 103, 'fr-CA'),
(82, 103, 'fr-CH'),
(83, 103, 'fr-FR'),
(84, 103, 'fr-LU'),
(85, 103, 'fr-MC'),
(86, 103, 'gl-ES'),
(87, 103, 'gu-IN'),
(88, 103, 'he-IL'),
(89, 103, 'hi-IN'),
(90, 103, 'hr-BA'),
(91, 103, 'hr-HR'),
(92, 103, 'hu-HU'),
(93, 103, 'hy-AM'),
(94, 103, 'id-ID'),
(95, 103, 'is-IS'),
(96, 103, 'it-CH'),
(97, 103, 'it-IT'),
(98, 103, 'ja-JP'),
(99, 103, 'ka-GE'),
(100, 103, 'kk-KZ'),
(101, 103, 'kn-IN'),
(102, 103, 'kok-IN'),
(103, 103, 'ko-KR'),
(104, 103, 'ky-KG'),
(105, 103, 'lt-LT'),
(106, 103, 'lv-LV'),
(107, 103, 'mi-NZ'),
(108, 103, 'mk-MK'),
(109, 103, 'mn-MN'),
(110, 103, 'mr-IN'),
(111, 103, 'ms-BN'),
(112, 103, 'ms-MY'),
(113, 103, 'mt-MT'),
(114, 103, 'nb-NO'),
(115, 103, 'nl-BE'),
(116, 103, 'nl-NL'),
(117, 103, 'nn-NO'),
(118, 103, 'ns-ZA'),
(119, 103, 'pa-IN'),
(120, 103, 'pl-PL'),
(121, 103, 'pt-BR'),
(122, 103, 'pt-PT'),
(123, 103, 'quz-BO'),
(124, 103, 'quz-EC'),
(125, 103, 'quz-PE'),
(126, 103, 'ro-RO'),
(127, 103, 'ru-RU'),
(128, 103, 'sa-IN'),
(129, 103, 'se-FI'),
(130, 103, 'se-NO'),
(131, 103, 'se-SE'),
(132, 103, 'sk-SK'),
(133, 103, 'sl-SI'),
(134, 103, 'sma-NO'),
(135, 103, 'sma-SE'),
(136, 103, 'smj-NO'),
(137, 103, 'smj-SE'),
(138, 103, 'smn-FI'),
(139, 103, 'sms-FI'),
(140, 103, 'sq-AL'),
(141, 103, 'sr-Cyrl-BA'),
(142, 103, 'sr-Cyrl-SP'),
(143, 103, 'sr-Latn-BA'),
(144, 103, 'sr-Latn-SP'),
(145, 103, 'sv-FI'),
(146, 103, 'sv-SE'),
(147, 103, 'sw-KE'),
(148, 103, 'syr-SY'),
(149, 103, 'ta-IN'),
(150, 103, 'te-IN'),
(151, 103, 'th-TH'),
(152, 103, 'tn-ZA'),
(153, 103, 'tr-TR'),
(154, 103, 'tt-RU'),
(155, 103, 'uk-UA'),
(156, 103, 'ur-PK'),
(157, 103, 'uz-Cyrl-UZ'),
(158, 103, 'uz-Latn-UZ'),
(159, 103, 'vi-VN'),
(160, 103, 'xh-ZA'),
(161, 103, 'zh-CN'),
(162, 103, 'zh-HK'),
(163, 103, 'zh-MO'),
(164, 103, 'zh-SG'),
(165, 103, 'zh-TW'),
(166, 103, 'zu-ZA');
-- ISGO


DROP TABLE IF EXISTS `mce_format_types`;
-- ISGO
CREATE TABLE `mce_format_types` (
  `mce_format_types_id` int(10) unsigned NOT NULL auto_increment,
  `mce_components_id` int(10) unsigned default '0',
  `name` varchar(128) NOT NULL default '',
  `description` text,
  PRIMARY KEY  (`mce_format_types_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO

INSERT INTO `mce_format_types` VALUES
(1,NULL,'String',NULL),
(2,NULL,'Bool',NULL),
(3,NULL,'Number',NULL),
(4,NULL,'DateTime',NULL),
(5,NULL,'IP_Address',NULL),
(6,NULL,'Array',NULL),
(7,NULL,'HashTable',NULL),
(8,NULL,'DataTable',NULL),
(9,NULL,'Password',NULL),
(100,NULL,'TraceLevel',NULL),
(101,NULL,'TriggerLevel',NULL),
(102,NULL,'ConnectionType',NULL),
(103,NULL,'Locale',NULL);
-- ISGO


DROP TABLE IF EXISTS `mce_ietf_sip_devices`;
-- ISGO
CREATE TABLE `mce_ietf_sip_devices` (
  `mce_ietf_sip_devices_id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `mce_components_id` INTEGER UNSIGNED NOT NULL DEFAULT 0,
  `username` VARCHAR(128) NOT NULL DEFAULT '',
  `password` VARCHAR(255) NOT NULL DEFAULT '',
  `status` INTEGER UNSIGNED NOT NULL DEFAULT 0,
  PRIMARY KEY(`mce_ietf_sip_devices_id`)
)
ENGINE = InnoDB DEFAULT CHARSET=utf8;
-- ISGO


DROP TABLE IF EXISTS `mce_license`;
-- ISGO
CREATE TABLE `mce_license` (
  `mce_license_id` int(10) unsigned NOT NULL auto_increment,
  `license_data` text NOT NULL,
  `active` tinyint(1) unsigned NOT NULL default '0',
  `created_timestamp` timestamp NOT NULL default CURRENT_TIMESTAMP on update CURRENT_TIMESTAMP,
  PRIMARY KEY  (`mce_license_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO


DROP TABLE IF EXISTS `mce_provider_extensions`;
-- ISGO
CREATE TABLE `mce_provider_extensions` (
  `mce_provider_extensions_id` int(10) unsigned NOT NULL auto_increment,
  `mce_components_id` int(10) unsigned NOT NULL default '0',
  `name` varchar(128) NOT NULL default '',
  `description` text,
  `wait_for_completion` varchar(128) NOT NULL default '',
  PRIMARY KEY  (`mce_provider_extensions_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO

INSERT INTO `mce_provider_extensions` (`mce_provider_extensions_id`,`mce_components_id`,`name`,`description`,`wait_for_completion`) VALUES 
 (1,2,'PrintDiags','Prints diagnostic information to logging',0),
 (2,3,'PrintDiags','Prints diagnostic information to logging',0),
 (3,4,'PrintDiags','Prints diagnostic information to logging',0),
 (4,5,'PrintDiags','Prints diagnostic information to logging',0),
 (5,8,'ClearCrgCache','Clears the call route group cache',0),
 (6,8,'ClearCallTable','Clears the call table',0),
 (7,8,'EndAllCalls','Terminates all calls in the call table gracefully',0),
 (8,8,'PrintDiags','Prints diagnostic information to logging',0),
 (9,9,'PrintDiags','Prints diagnostic information to logging',0);
-- ISGO
 
 
DROP TABLE IF EXISTS `mce_provider_extensions_parameters`;
-- ISGO
CREATE TABLE `mce_provider_extensions_parameters` (
  `mce_provider_extensions_parameters_id` int(10) unsigned NOT NULL auto_increment,
  `mce_provider_extensions_id` int(10) unsigned NOT NULL default '0',
  `name` varchar(128) NOT NULL default '',
  `description` text,
  `mce_format_types_id` int(10) unsigned NOT NULL default '0',
  PRIMARY KEY  (`mce_provider_extensions_parameters_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO


DROP TABLE IF EXISTS `mce_services`;
-- ISGO
CREATE TABLE `mce_services` (
  `mce_services_id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(128) NOT NULL,
  `display_name` VARCHAR(128) NOT NULL,
  `enabled` TINYINT(1) NOT NULL default '0',
  `all_use` TINYINT(1) NOT NULL default '0',
  `app_server_use` TINYINT(1) NOT NULL default '0',
  `media_server_use` TINYINT(1) NOT NULL default '0',
  `rtprelay_server_use` TINYINT(1) NOT NULL default '0',
  `user_stopped` TINYINT(1) NOT NULL default '0',
  `description` TEXT NOT NULL,
  PRIMARY KEY(`mce_services_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO

INSERT INTO `mce_services` VALUES
(1,'MetreosAppServerService','Application Server',0,0,1,0,0,0,'Application server'),
(2,'MediaServerService','Media Engine',0,0,0,1,0,0,'Media engine'),
(4,'MetreosH323Stack','H.323 Stack',0,0,1,0,0,0,'H.323 stack'),
(5,'MetreosWatchDog','Watchdog Server',0,1,0,0,0,0,'Watches all of the services and handles failovers'),
(6,'MetreosLogServer','Log Server',0,1,0,0,0,0,'Maintains logs of services'),
-- (7,'MetreosAlarmServer','Alarm Server',0,1,0,0,0,0,'Sends out alarm notifications'),
-- (8,'MetreosSMIServer','SMI Server',0,1,0,0,0,0,'SMI'),
-- (9,'MetreosRtpRelayService', 'RTP Relay', 0,0,0,0,1,0,'RTP Relay Service'),
(10,'MetreosJTAPIStack_CCM-3-3','JTAPI Stack CCM-3-3',0,0,1,0,0,0,'JTAPI for CallManager 3.3'),
(11,'MetreosJTAPIStack_CCM-4-0','JTAPI Stack CCM-4-0',0,0,1,0,0,0,'JTAPI for CallManager 4.0'),
(12,'MetreosJTAPIStack_CCM-4-1','JTAPI Stack CCM-4-1',0,0,1,0,0,0,'JTAPI for CallManager 4.1'),
-- (3,'MetreosJTAPIStack','JTAPI Stack',0,0,1,0,0,0,'JTAPI'),
-- (13,'MetreosPCapService','PCap Service',0,0,1,0,0,0,'Packet capturing service'),
(14,'MetreosSipStack','SIP Stack',0,0,1,0,0,0,'SIP stack'),
(15,'MetreosJTAPIStack_CCM-4-2','JTAPI Stack CCM-4-2',0,0,1,0,0,0,'JTAPI for CallManager 4.2'),
(16,'MetreosJTAPIStack_CCM-5-0','JTAPI Stack CCM-5-0',0,0,1,0,0,0,'JTAPI for CallManager 5.0'),
(17,'SftpServerService','SFTP Server',0,1,0,0,0,0,'Secure file transfer server'),
(18,'MetreosJTAPIStack_CCM-5-1','JTAPI Stack CCM-5-1',0,0,1,0,0,0,'JTAPI for CallManager 5.1'),
(19,'MetreosJTAPIStack_CCM-6-0','JTAPI Stack CUCM-6-0',0,0,1,0,0,0,'JTAPI for Unified Communications Manager 6.0'),
(20,'CuaeStatsServer','Stats Server',0,1,0,0,0,0,'Maintains usage statistics and sends out alarms'),
(21,'MetreosPresenceStack','Presence Stack',0,0,1,0,0,0,'SIP Presence stack');
-- ISGO


DROP TABLE IF EXISTS `mce_sip_domains`;
-- ISGO
CREATE TABLE `mce_sip_domains` (
  `mce_sip_domains_id` INTEGER(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `domain_name` VARCHAR(128) NOT NULL,
  `primary_registrar` VARCHAR(128) NOT NULL,
  `secondary_registrar` VARCHAR(128),
  `type` INTEGER UNSIGNED NOT NULL default '0',
  PRIMARY KEY(`mce_sip_domains_id`)
)
ENGINE = InnoDB DEFAULT CHARSET=utf8;
-- ISGO


DROP TABLE IF EXISTS `mce_sip_domain_members`;
-- ISGO
CREATE TABLE `mce`.`mce_sip_domain_members` (
  `mce_sip_domains_id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `mce_components_id` INTEGER UNSIGNED NOT NULL,
  PRIMARY KEY(`mce_sip_domains_id`, `mce_components_id`)
)
ENGINE = InnoDB DEFAULT CHARSET=utf8;
-- ISGO


DROP TABLE IF EXISTS `mce_sip_outbound_proxies`;
-- ISGO
CREATE TABLE `mce_sip_outbound_proxies` (
  `mce_sip_outbound_proxies_id` INTEGER(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `mce_sip_domains_id` INTEGER UNSIGNED NOT NULL,
  `ip_address` VARCHAR(16) NOT NULL,
  PRIMARY KEY(`mce_sip_outbound_proxies_id`)
)
ENGINE = InnoDB DEFAULT CHARSET=utf8;
-- ISGO


DROP TABLE IF EXISTS `mce_snmp_mib_defs`;
-- ISGO
CREATE TABLE `mce_snmp_mib_defs` (
  `mce_snmp_mib_defs_id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `oid` VARCHAR(64) NOT NULL,
  `type` INT(10) NOT NULL DEFAULT 0,        -- Maps to IConfig.SnmpOidType enum 
  `name` VARCHAR(64) NOT NULL,
  `description` VARCHAR(256) NOT NULL,
  `data_type` INT(10) NOT NULL DEFAULT 0,   -- Maps to IConfig.SnmpSyntax enum
  `ignore` TINYINT(1) UNSIGNED NOT NULL DEFAULT 0,
  PRIMARY KEY(`mce_snmp_mib_defs_id`)
) ENGINE = InnoDB DEFAULT CHARSET=utf8;
-- ISGO

INSERT INTO `mce_snmp_mib_defs` VALUES
(1, 100, 1, 'ServiceUnavailable', 'A CUAE Service is not available.', 0, 0),
(2, 101, 1, 'MediaServerUnavailable', 'A Media Server is not available.', 0, 0),
(3, 102, 1, 'OutOfMemory', 'CUAE is running out of memory.', 0, 0),
(4, 200, 1, 'MsCompromised', 'Media server compromised', 0, 0),
(5, 201, 1, 'MsUnexpectedCond', 'Unexpected condition', 0, 0),
(6, 202, 1, 'MsErrorShutdown', 'Media server unscheduled shutdown', 0, 0),
(7, 203, 1, 'MsNoResource', 'Resource type not deployed on this server (e.g. no voice rec)', 0, 0),
(8, 210, 1, 'MsOutOfRTP', 'Out of connections (G.711)', 0, 0),
(9, 211, 1, 'MsRtpHW', 'High water connections', 0, 0),
(10, 212, 1, 'MsRtpLW', 'Low water connections', 0, 0),
(11, 220, 1, 'MsOutOfVoice', 'Out of voice', 0, 0),
(12, 221, 1, 'MsVoiceHW', 'High water voice', 0, 0),
(13, 222, 1, 'MsVoiceLW', 'Low water voice', 0, 0),
(14, 230, 1, 'MsOutOfErtp', 'Out of low bitrate', 0, 0),
(15, 231, 1, 'MsErtpHW', 'High water low bitrate', 0, 0),
(16, 232, 1, 'MsErtpLW', 'Low water low bitrate', 0, 0),
(17, 240, 1, 'MsOutOfConfRes', 'Out of conference resources for service instance', 0, 0),
(18, 241, 1, 'MsConfResHW', 'Conference resources for service instance high water', 0, 0),
(19, 242, 1, 'MsConfResLW', 'Conference resources for service instance low water', 0, 0),
(20, 243, 1, 'MsOutOfConfSlots', 'Out of conference slots for conference', 0, 0),
(21, 244, 1, 'MsConfSlotsHW', 'Conference slots for conference high water', 0, 0),
(22, 245, 1, 'MsConfSlotsLW', 'Conference slots for conference low water', 0, 0),
(23, 246, 1, 'MsOutOfConf', 'Out of conferences', 0, 0),
(24, 247, 1, 'MsConfHW', 'Conferences high water', 0, 0),
(25, 248, 1, 'MsConfLW', 'Conferences low water', 0, 0),
(26, 250, 1, 'MsOutOfTts', 'Out of TTS ports (request fails)', 0, 0),
(27, 251, 1, 'MsOutOfTtsQueued', 'Out of TTS ports (request queues)', 0, 0),
(28, 252, 1, 'MsTtsHW', 'High water TTS', 0, 0),
(29, 253, 1, 'MsTtsLW', 'Low water TTS', 0, 0),
(30, 260, 1, 'MsOutOfVoiceRec', 'Out of voice rec resources (request fails)', 0, 0),
(31, 261, 1, 'MsVoiceRecHW', 'High water voice rec', 0, 0),
(32, 262, 1, 'MsVoiceRecLW', 'Low water voice rec', 0, 0),
(33, 300, 1, 'ServerErrorShutdown', 'CUAE Server shutdown unexpectedly', 0, 0),
(34, 301, 1, 'ServerStartupFailed', 'CUAE Server failed to start', 0, 0),
(35, 302, 1, 'AppLoadFailed', 'Application failed to load', 0, 0),
(36, 303, 1, 'ProviderLoadFailed', 'Provider failed to load', 0, 0),
(37, 304, 1, 'AppReloaded', 'Application reloaded due to failure', 0, 0),
(38, 305, 1, 'ProviderReloaded', 'Provider reloaded due to failure', 0, 0),
(39, 306, 1, 'MediaDeployFailed', 'Media deploy failure', 0, 0),
(40, 310, 1, 'AppSessionHW', 'High water application sessions', 0, 0),
(41, 311, 1, 'AppSessionLW', 'Low water application sessions', 0, 0),
(42, 320, 1, 'MgmtLoginFailure', 'Management login failure', 0, 0),
(43, 321, 1, 'MgmtLoginSuccess', 'Management login success', 0, 1),
(44, 400, 1, 'AppSessionsExceeded', 'Number of licensed application sessions exceeded.', 0, 0),
(45, 401, 1, 'AppSessionsExceededFinal', 'Number of licensed application sessions exceeded; licenses are now strictly enforced.', 0, 0),
(46, 402, 1, 'AppSessionDenied', 'An attempt has been made to exceed the maximum number of licensed application sessions.', 0, 0),
(47, 403, 1, 'VoiceExceeded', 'Number of licensed voice resources exceeded.', 0, 0),
(48, 404, 1, 'VoiceExceededFinal', 'Number of licensed voice resources exceeded; licenses are now strictly enforced.', 0, 0),
(49, 405, 1, 'VoiceDenied', 'An attempt has been made to exceed the maximum number of licensed voice resources.', 0, 0),
(50, 406, 1, 'RtpExceeded', 'Number of licensed RTP resources exceeded.', 0, 0),
(51, 407, 1, 'RtpExceededFinal', 'Number of licensed RTP resources exceeded; licenses are now strictly enforced.', 0, 0),
(52, 408, 1, 'RtpDenied', 'An attempt has been made to exceed the maximum number of licensed RTP resources.', 0, 0),
(53, 409, 1, 'ErtpExceeded', 'Number of licensed enhanced RTP resources exceeded.', 0, 0),
(54, 410, 1, 'ErtpExceededFinal', 'Number of licensed enhanced RTP resources exceeded; licenses are now strictly enforced.', 0, 0),
(55, 411, 1, 'ErtpDenied', 'An attempt has been made to exceed the maximum number of licensed enhanced RTP resources.', 0, 0),
(56, 412, 1, 'ConfExceeded', 'Number of licensed conference resources exceeded.', 0, 0),
(57, 413, 1, 'ConfExceededFinal', 'Number of licensed conference resources exceeded; licenses are now strictly enforced.', 0, 0),
(58, 414, 1, 'ConfDenied', 'An attempt has been made to exceed the maximum number of licensed conference resources.', 0, 0),
(59, 415, 1, 'SpeechExceeded', 'Number of licensed speech integration resources exceeded.', 0, 0),
(60, 416, 1, 'SpeechExceededFinal', 'Number of licensed speech integration resources exceeded; licenses are now strictly enforced.', 0, 0),
(61, 417, 1, 'SpeechDenied', 'An attempt has been made to exceed the maximum number of licensed speech integration resources.', 0, 0),
(62, 418, 1, 'TtsExceeded', 'Number of licensed TTS resources exceeded.', 0, 0),
(63, 419, 1, 'TtsExceededFinal', 'Number of licensed TTS resources exceeded; licenses are now strictly enforced.', 0, 0),
(64, 420, 1, 'TtsDenied', 'An attempt has been made to exceed the maximum number of licensed TTS resources.', 0, 0);
-- ISGO

INSERT INTO `mce_snmp_mib_defs` VALUES
(1001, 1100, 1, 'ClearedServiceUnavailable', 'Alarm Cleared: A CUAE Service is not available.', 0, 0),
(1002, 1101, 1, 'ClearedMediaServerUnavailable', 'Alarm Cleared: A Media Server is not available.', 0, 0),
(1003, 1102, 1, 'ClearedOutOfMemory', 'Alarm Cleared: CUAE is running out of memory.', 0, 0),
(1004, 1200, 1, 'ClearedMsCompromised', 'Alarm Cleared: Media server compromised', 0, 0),
(1005, 1201, 1, 'ClearedMsUnexpectedCond', 'Alarm Cleared: Unexpected condition', 0, 0),
(1006, 1202, 1, 'ClearedMsErrorShutdown', 'Alarm Cleared: Media server unscheduled shutdown', 0, 0),
(1007, 1203, 1, 'ClearedMsNoResource', 'Alarm Cleared: Resource type not deployed on this server (e.g. no voice rec)', 0, 0),
(1008, 1210, 1, 'ClearedMsOutOfRTP', 'Alarm Cleared: Out of connections (G.711)', 0, 0),
(1009, 1211, 1, 'ClearedMsRtpHW', 'Alarm Cleared: High water connections', 0, 0),
(1010, 1212, 1, 'ClearedMsRtpLW', 'Alarm Cleared: Low water connections', 0, 0),
(1011, 1220, 1, 'ClearedMsOutOfVoice', 'Alarm Cleared: Out of voice', 0, 0),
(1012, 1221, 1, 'ClearedMsVoiceHW', 'Alarm Cleared: High water voice', 0, 0),
(1013, 1222, 1, 'ClearedMsVoiceLW', 'Alarm Cleared: Low water voice', 0, 0),
(1014, 1230, 1, 'ClearedMsOutOfErtp', 'Alarm Cleared: Out of low bitrate', 0, 0),
(1015, 1231, 1, 'ClearedMsErtpHW', 'Alarm Cleared: High water low bitrate', 0, 0),
(1016, 1232, 1, 'ClearedMsErtpLW', 'Alarm Cleared: Low water low bitrate', 0, 0),
(1017, 1240, 1, 'ClearedMsOutOfConfRes', 'Alarm Cleared: Out of conference resources for service instance', 0, 0),
(1018, 1241, 1, 'ClearedMsConfResHW', 'Alarm Cleared: Conference resources for service instance high water', 0, 0),
(1019, 1242, 1, 'ClearedMsConfResLW', 'Alarm Cleared: Conference resources for service instance low water', 0, 0),
(1020, 1243, 1, 'ClearedMsOutOfConfSlots', 'Alarm Cleared: Out of conference slots for conference', 0, 0),
(1021, 1244, 1, 'ClearedMsConfSlotsHW', 'Alarm Cleared: Conference slots for conference high water', 0, 0),
(1022, 1245, 1, 'ClearedMsConfSlotsLW', 'Alarm Cleared: Conference slots for conference low water', 0, 0),
(1023, 1246, 1, 'ClearedMsOutOfConf', 'Alarm Cleared: Out of conferences', 0, 0),
(1024, 1247, 1, 'ClearedMsConfHW', 'Alarm Cleared: Conferences high water', 0, 0),
(1025, 1248, 1, 'ClearedMsConfLW', 'Alarm Cleared: Conferences low water', 0, 0),
(1026, 1250, 1, 'ClearedMsOutOfTts', 'Alarm Cleared: Out of TTS ports (request fails)', 0, 0),
(1027, 1251, 1, 'ClearedMsOutOfTtsQueued', 'Alarm Cleared: Out of TTS ports (request queues)', 0, 0),
(1028, 1252, 1, 'ClearedMsTtsHW', 'Alarm Cleared: High water TTS', 0, 0),
(1029, 1253, 1, 'ClearedMsTtsLW', 'Alarm Cleared: Low water TTS', 0, 0),
(1030, 1260, 1, 'ClearedMsOutOfVoiceRec', 'Alarm Cleared: Out of voice rec resources (request fails)', 0, 0),
(1031, 1261, 1, 'ClearedMsVoiceRecHW', 'Alarm Cleared: High water voice rec', 0, 0),
(1032, 1262, 1, 'ClearedMsVoiceRecLW', 'Alarm Cleared: Low water voice rec', 0, 0),
(1033, 1300, 1, 'ClearedServerErrorShutdown', 'Alarm Cleared: CUAE Server shutdown unexpectedly', 0, 0),
(1034, 1301, 1, 'ClearedServerStartupFailed', 'Alarm Cleared: CUAE Server failed to start', 0, 0),
(1035, 1302, 1, 'ClearedAppLoadFailed', 'Alarm Cleared: Application failed to load', 0, 0),
(1036, 1303, 1, 'ClearedProviderLoadFailed', 'Alarm Cleared: Provider failed to load', 0, 0),
(1037, 1304, 1, 'ClearedAppReloaded', 'Alarm Cleared: Application reloaded due to failure', 0, 0),
(1038, 1305, 1, 'ClearedProviderReloaded', 'Alarm Cleared: Provider reloaded due to failure', 0, 0),
(1039, 1306, 1, 'ClearedMediaDeployFailed', 'Alarm Cleared: Media deploy failure', 0, 0),
(1040, 1310, 1, 'ClearedAppSessionHW', 'Alarm Cleared: High water application sessions', 0, 0),
(1041, 1311, 1, 'ClearedAppSessionLW', 'Alarm Cleared: Low water application sessions', 0, 0),
(1042, 1320, 1, 'ClearedMgmtLoginFailure', 'Alarm Cleared: Management login failure', 0, 0),
(1043, 1321, 1, 'ClearedMgmtLoginSuccess', 'Alarm Cleared: Management login success', 0, 1),
(1044, 1400, 1, 'ClearedAppSessionsExceeded', 'Alarm Cleared: Number of licensed application sessions exceeded.', 0, 0),
(1045, 1401, 1, 'ClearedAppSessionsExceededFinal', 'Alarm Cleared: Number of licensed application sessions exceeded; licenses are now strictly enforced.', 0, 0),
(1046, 1402, 1, 'ClearedAppSessionDenied', 'Alarm Cleared: An attempt has been made to exceed the maximum number of licensed application sessions.', 0, 0),
(1047, 1403, 1, 'ClearedVoiceExceeded', 'Alarm Cleared: Number of licensed voice resources exceeded.', 0, 0),
(1048, 1404, 1, 'ClearedVoiceExceededFinal', 'Alarm Cleared: Number of licensed voice resources exceeded; licenses are now strictly enforced.', 0, 0),
(1049, 1405, 1, 'ClearedVoiceDenied', 'Alarm Cleared: An attempt has been made to exceed the maximum number of licensed voice resources.', 0, 0),
(1050, 1406, 1, 'ClearedRtpExceeded', 'Alarm Cleared: Number of licensed RTP resources exceeded.', 0, 0),
(1051, 1407, 1, 'ClearedRtpExceededFinal', 'Alarm Cleared: Number of licensed RTP resources exceeded; licenses are now strictly enforced.', 0, 0),
(1052, 1408, 1, 'ClearedRtpDenied', 'Alarm Cleared: An attempt has been made to exceed the maximum number of licensed RTP resources.', 0, 0),
(1053, 1409, 1, 'ClearedErtpExceeded', 'Alarm Cleared: Number of licensed enhanced RTP resources exceeded.', 0, 0),
(1054, 1410, 1, 'ClearedErtpExceededFinal', 'Alarm Cleared: Number of licensed enhanced RTP resources exceeded; licenses are now strictly enforced.', 0, 0),
(1055, 1411, 1, 'ClearedErtpDenied', 'Alarm Cleared: An attempt has been made to exceed the maximum number of licensed enhanced RTP resources.', 0, 0),
(1056, 1412, 1, 'ClearedConfExceeded', 'Alarm Cleared: Number of licensed conference resources exceeded.', 0, 0),
(1057, 1413, 1, 'ClearedConfExceededFinal', 'Alarm Cleared: Number of licensed conference resources exceeded; licenses are now strictly enforced.', 0, 0),
(1058, 1414, 1, 'ClearedConfDenied', 'Alarm Cleared: An attempt has been made to exceed the maximum number of licensed conference resources.', 0, 0),
(1059, 1415, 1, 'ClearedSpeechExceeded', 'Alarm Cleared: Number of licensed speech integration resources exceeded.', 0, 0),
(1060, 1416, 1, 'ClearedSpeechExceededFinal', 'Alarm Cleared: Number of licensed speech integration resources exceeded; licenses are now strictly enforced.', 0, 0),
(1061, 1417, 1, 'ClearedSpeechDenied', 'Alarm Cleared: An attempt has been made to exceed the maximum number of licensed speech integration resources.', 0, 0),
(1062, 1418, 1, 'ClearedTtsExceeded', 'Alarm Cleared: Number of licensed TTS resources exceeded.', 0, 0),
(1063, 1419, 1, 'ClearedTtsExceededFinal', 'Alarm Cleared: Number of licensed TTS resources exceeded; licenses are now strictly enforced.', 0, 0),
(1064, 1420, 1, 'ClearedTtsDenied', 'Alarm Cleared: An attempt has been made to exceed the maximum number of licensed TTS resources.', 0, 0);
-- ISGO

INSERT INTO `mce_snmp_mib_defs` VALUES
(2001, 2000, 2, 'StatCPULoad', 'CPU load %', 1, 0),
(2002, 2001, 2, 'StatServerMemory', 'CUAE Server memory usage', 1, 0),
(2003, 2002, 2, 'StatMsMemory', 'Media Engine memory usage', 1, 0),
(2004, 2010, 2, 'StatAppSessions', 'Number of active application sessions', 1, 0),
(2005, 2011, 2, 'StatActiveCalls', 'Number of active calls', 1, 0),
(2006, 2020, 2, 'StatRouterMsgs', 'Router: Messages/sec', 1, 0),
(2007, 2021, 2, 'StatRouterEvents', 'Router: Events/sec', 1, 0),
(2008, 2022, 2, 'StatRouterActions', 'Router: Actions/sec', 1, 0),
(2009, 2100, 2, 'StatMsVoice', 'Number of voice resources in use', 1, 0),
(2010, 2101, 2, 'StatMsRtp', 'Number of RTP resources in use', 1, 0),
(2011, 2102, 2, 'StatMsErtp', 'Number of enhanced RTP resources in use', 1, 0),
(2012, 2103, 2, 'StatMsConfRes', 'Number of conference resources in use', 1, 0),
(2013, 2104, 2, 'StatMsSpeech', 'Number of speech integration resources in use', 1, 0),
(2014, 2105, 2, 'StatMsTts', 'Number of TTS resources in use', 1, 0),
(2015, 2106, 2, 'StatMsConfSlots', 'Number of conference slots in use', 1, 0),
(2016, 2107, 2, 'StatMsConf', 'Number of conferences in use', 1, 0);
-- ISGO


DROP TABLE IF EXISTS `mce_system_configs`;
-- ISGO
CREATE TABLE `mce_system_configs` (
  `mce_system_configs_id` int(10) unsigned NOT NULL auto_increment,
  `name` varchar(128) NOT NULL default '',
  `value` varchar(128) NOT NULL default '',
  PRIMARY KEY  (`mce_system_configs_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO

INSERT INTO `mce_system_configs` (`mce_system_configs_id`,`name`,`value`) VALUES
(1,'firmware_version','2-3-0'),
(2,'software_version','@BuildID@'),
(4,'routine_backup_databases',''),
(5,'routine_backup_database_data',''),
(7,'release_type','@BuildReleaseType@'),
(8,'apache_restart_needed','0'),
(14,'redundancy_master_ip',''),
(15,'redundancy_master_heartbeat','5'),
(16,'redundancy_master_max_missed_heartbeat','2'),
(17,'redundancy_standby_ip',''),
(18,'redundancy_standby_startup_sync_time','5'),
(19,'media_provisioning_password',''),
(20,'database_version','12'),
(21,'rollback_version',''),
(22,'oid_root','1.3.6.1.4.1.22720.1.');
-- ISGO


DROP TABLE IF EXISTS `mce_trigger_parameter_values`;
-- ISGO
CREATE TABLE `mce_trigger_parameter_values` (
  `mce_trigger_parameter_values_id` int(10) unsigned NOT NULL auto_increment,
  `mce_application_script_trigger_parameters_id` int(10) unsigned NOT NULL default '0',
  `value` varchar(128) NOT NULL default '',
  PRIMARY KEY  (`mce_trigger_parameter_values_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO


DROP TABLE IF EXISTS `mce_users`;
-- ISGO
CREATE TABLE `mce_users` (
  `mce_users_id` int(10) unsigned NOT NULL auto_increment,
  `username` varchar(128) NOT NULL default '',
  `password` varchar(128) NOT NULL default '',
  `creator_mce_users_id` int(10) unsigned NOT NULL default '0',
  `created_timestamp` timestamp NOT NULL default CURRENT_TIMESTAMP,
  `updated_timestamp` timestamp NOT NULL default '0000-00-00 00:00:00',
  `access_level` int(10) unsigned NOT NULL default '0',
  PRIMARY KEY  (`mce_users_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO

INSERT INTO `mce_users` VALUES
(1,'Administrator','cUAR3ZDDQ+zA3A9vGDkoeg==',0,CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,1);
-- ISGO


DROP TABLE IF EXISTS `mce_users_acl_list`;
-- ISGO
CREATE TABLE `mce_users_acl_list` (
  `mce_users_id` int(10) unsigned NOT NULL default '0',
  `mce_component_groups_id` int(10) unsigned NOT NULL default '0',
  PRIMARY KEY  (`mce_users_id`,`mce_component_groups_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
-- ISGO


SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT;
-- ISGO
SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS;
-- ISGO
SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION;
-- ISGO
