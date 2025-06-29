SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT;
SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS;
SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION;
SET NAMES utf8;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE=NO_AUTO_VALUE_ON_ZERO */;


CREATE DATABASE /*!32312 IF NOT EXISTS*/ `application_suite`;
USE `application_suite`;


DROP TABLE IF EXISTS `as_activerelay_filter_numbers`;
CREATE TABLE `as_activerelay_filter_numbers` (
  `as_activerelay_filter_numbers_id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `as_users_id` INTEGER UNSIGNED,
  `type` INTEGER UNSIGNED NOT NULL DEFAULT 0,
  `number` VARCHAR(255) NOT NULL DEFAULT '',
PRIMARY KEY(`as_activerelay_filter_numbers_id`),
CONSTRAINT `FK_as_activerelay_filter_numbers_1` FOREIGN KEY `FK_as_activerelay_filter_numbers_1` (`as_users_id`)
    REFERENCES `as_users` (`as_users_id`)
    ON DELETE CASCADE
    ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=latin1;;


DROP TABLE IF EXISTS `as_applications`;
CREATE TABLE `as_applications` (
  `as_applications_id` int(10) unsigned NOT NULL auto_increment,
  `name` varchar(255) NOT NULL default '',
  `installed` tinyint(1) NOT NULL default '0',
  PRIMARY KEY  (`as_applications_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO `as_applications` (`as_applications_id`,`name`,`installed`) VALUES 
 (1,'Scheduled Conferencing',1),
 (2,'Intercom and Talkback',1),
 (3,'RapidRecord',1),
 (4,'Remote Agent',1),
 (5,'ActiveRelay',1),
 (6,'VoiceTunnel',1),
 (7,'Click-to-Talk',1);
 

DROP TABLE IF EXISTS `as_auth_records`;
CREATE TABLE `as_auth_records` (
  `as_auth_records_id` int(10) unsigned NOT NULL auto_increment,
  `as_users_id` int(10) unsigned default NULL,
  `auth_timestamp` timestamp NOT NULL default CURRENT_TIMESTAMP on update CURRENT_TIMESTAMP,
  `status` int(11) default NULL,
  `originating_number` int(11) default NULL,
  `source_ip_address` varchar(16) default NULL,
  `username` varchar(255) default NULL,
  `pin` int(11) default NULL,
  `application_name` varchar(255) default NULL,
  `partition_name` varchar(255) default NULL,
  PRIMARY KEY  (`as_auth_records_id`),
  KEY `Refas_users4` (`as_users_id`),
  CONSTRAINT `Refas_users4` FOREIGN KEY (`as_users_id`) REFERENCES `as_users` (`as_users_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS `as_backups`;
CREATE TABLE `as_backups` (
  `as_backups_id` int(10) unsigned NOT NULL auto_increment,
  `backup_date` timestamp NOT NULL default CURRENT_TIMESTAMP on update CURRENT_TIMESTAMP,
  `name` varchar(128) NOT NULL default '',
  `status` int(10) unsigned NOT NULL default '0',
  PRIMARY KEY  (`as_backups_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS `as_call_records`;
CREATE TABLE `as_call_records` (
  `as_call_records_id` int(10) unsigned NOT NULL auto_increment,
  `as_users_id` int(10) unsigned default NULL,
  `as_auth_records_id` int(10) unsigned default NULL,
  `as_session_records_id` int(10) unsigned default NULL,
  `as_scheduled_conferences_id` int(10) unsigned default NULL,
  `origin_number` varchar(255) default NULL,
  `destination_number` varchar(255) default NULL,
  `application_name` varchar(255) default NULL,
  `partition_name` varchar(255) default NULL,
  `script_name` varchar(255) default NULL,
  `start` timestamp default '0000-00-00 00:00:00',
  `end` timestamp default '0000-00-00 00:00:00',
  `end_reason` int(11) default NULL,
  PRIMARY KEY  (`as_call_records_id`),
  KEY `Refas_auth_records11` (`as_auth_records_id`),
  KEY `Refas_session_records11` (`as_session_records_id`),
  KEY `Refas_scheduled_conferences12` (`as_scheduled_conferences_id`),
  KEY `Refas_users18` (`as_users_id`),
  CONSTRAINT `Refas_auth_records11` FOREIGN KEY (`as_auth_records_id`) REFERENCES `as_auth_records` (`as_auth_records_id`),
  CONSTRAINT `Refas_session_records11` FOREIGN KEY (`as_session_records_id`) REFERENCES `as_session_records` (`as_session_records_id`),
  CONSTRAINT `Refas_scheduled_conferences12` FOREIGN KEY (`as_scheduled_conferences_id`) REFERENCES `as_scheduled_conferences` (`as_scheduled_conferences_id`),
  CONSTRAINT `Refas_users18` FOREIGN KEY (`as_users_id`) REFERENCES `as_users` (`as_users_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS `as_configs`;
CREATE TABLE `as_configs` (
  `as_configs_id` int(10) unsigned NOT NULL auto_increment,
  `as_applications_id` int(10) unsigned default NULL,
  `name` varchar(255) default NULL,
  `value` text,
  `description` text,
  `groupname` varchar(255) default NULL,
  `required` tinyint(1) NOT NULL default '0',
  PRIMARY KEY  (`as_configs_id`),
  KEY `Refas_applications22` (`as_applications_id`),
  CONSTRAINT `Refas_applications22` FOREIGN KEY (`as_applications_id`) REFERENCES `as_applications` (`as_applications_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

INSERT INTO `as_configs` VALUES
 (1,NULL,'admin_password','cUAR3ZDDQ+zA3A9vGDkoeg==',NULL,NULL,0),
 (2,NULL,'webserver_root_filepath','C:\\Program Files\\Apache Group\\Apache\\htdocs','File path on the web server which points to the root directory',NULL,0),
 (4,NULL,'record_rel_path','recordings','Path which corresponds the file path and web server path which holds the recordings',NULL,0),
 (5,NULL,'recordings_expiration','0','Number of days to keep a recording (0 for indefinitely)','RapidRecord',1),
 (6,NULL,'media_rel_path','media','Path which corresponds the file path and web server path holding media',NULL,0),
 (7,NULL,'default_timezone_offset','0','Default time zone offset for user and administrator accounts','System',1),
 (9,NULL,'scheduled_conference_dn','0','Dialing number for all scheduled conferences','ScheduledConference',1),
 (10,NULL,'default_lockout_threshold','0','Default set lockout threshold for users (0 for no lockout) ','System',1),
 (11,NULL,'default_lockout_duration','0','Default lockout duration in minutes for users (0 to require an admin to unlock)','System',1),
 (12,NULL,'default_max_concurrent_sessions','0','Default max concurrent sessions for users (0 for infinite)','System',1),
 (13,NULL,'media_ports','0','Number of available media ports','System',1),
 (14,NULL,'smtp_server','','Mail server to use for sending notifications','SMTP Server',0),
 (15,NULL,'smtp_port','25','Mail server port','SMTP Server',0),
 (16,NULL,'smtp_user','','User login to access the mail server','SMTP Server',0),
 (17,NULL,'smtp_password','','User password to access the mail server','SMTP Server',0),
 (18,NULL,'ldap_account_code_attribute','account_code','Attribute to use for the account code when importing users from LDAP servers','System',0),
 (19,NULL,'hide_devices_from_users','0','Hide device management functions from users','System',0),
 (20,NULL,'max_find_me_numbers_per_user','0','Maximum amount of Find Me numbers per user (0 for unlimited)','Find Me Numbers', 0),
 (21,NULL,'custom_logo_file',NULL,NULL,NULL,0),
 (22,NULL,'find_me_numbers_validate_regexes',NULL,NULL,NULL,0),
 (23,NULL,'find_me_numbers_description','','A friendly description of allowed Find Me Numbers (if blank, a default is used)','Find Me Numbers',0),
 (24,NULL,'find_me_numbers_blacklist_regexes',NULL,NULL,NULL,0),
 (25,NULL,'ldap_username_attribute','cn','Attribute to use for the username when importing users from LDAP servers','System',0);


 
DROP TABLE IF EXISTS `as_directory_numbers`;
CREATE TABLE `as_directory_numbers` (
  `as_directory_numbers_id` int(10) unsigned NOT NULL auto_increment,
  `as_phone_devices_id` int(10) unsigned NOT NULL default '0',
  `directory_number` varchar(255) NOT NULL default '0',
  `is_primary_number` tinyint(1) NOT NULL default '0',
  PRIMARY KEY  (`as_directory_numbers_id`),
  UNIQUE KEY `directory_number` (`directory_number`),
  KEY `Refas_phone_devices3` (`as_phone_devices_id`),
  CONSTRAINT `Refas_phone_devices3` FOREIGN KEY (`as_phone_devices_id`) REFERENCES `as_phone_devices` (`as_phone_devices_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS `as_external_numbers`;
CREATE TABLE `as_external_numbers` (
  `as_external_numbers_id` int(10) unsigned NOT NULL auto_increment,
  `as_users_id` int(10) unsigned default NULL,
  `name` varchar(255) default NULL,
  `phone_number` varchar(255) NOT NULL default '',
  `delay_call_time` int(10) unsigned NOT NULL default '0',
  `call_attempt_timeout` int(10) unsigned NOT NULL default '0',
  `is_corporate` tinyint(1) NOT NULL default '0',
  `ar_enabled` tinyint(1) NOT NULL default '0',
  `is_blacklisted` tinyint(1) NOT NULL default '0',
  `timeofday_enabled` TINYINT(1) UNSIGNED NOT NULL default '0',  
  `timeofday_weekend` int(10) UNSIGNED NOT NULL default '0',
  `timeofday_start` TIME NOT NULL default '00:00:00',
  `timeofday_end` TIME NOT NULL default '00:00:00',
  PRIMARY KEY  (`as_external_numbers_id`),
  KEY `Refas_users13` (`as_users_id`),
  CONSTRAINT `Refas_users13` FOREIGN KEY (`as_users_id`) REFERENCES `as_users` (`as_users_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS `as_findme_call_records`;
CREATE TABLE `as_findme_call_records` (
  `as_findme_call_records_id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `as_call_records_id` INTEGER UNSIGNED NOT NULL DEFAULT 0,
  `call_type` INTEGER UNSIGNED NOT NULL DEFAULT 0,
  `calling_number` VARCHAR(255) NOT NULL DEFAULT '',
  `called_number` VARCHAR(255) NOT NULL DEFAULT '',
  `end_reason` INTEGER NOT NULL DEFAULT 0,
  PRIMARY KEY(`as_findme_call_records_id`),
  CONSTRAINT `FK_as_findme_call_records_1` FOREIGN KEY `FK_as_findme_call_records_1` (`as_call_records_id`)
    REFERENCES `as_call_records` (`as_call_records_id`)
    ON DELETE CASCADE
    ON UPDATE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS `as_intercom_group_members`;
CREATE TABLE `as_intercom_group_members` (
  `as_intercom_groups_id` int(10) unsigned NOT NULL default '0',
  `as_users_id` int(10) unsigned NOT NULL default '0',
  PRIMARY KEY  (`as_intercom_groups_id`,`as_users_id`),
  KEY `Refas_users17` (`as_users_id`),
  CONSTRAINT `Refas_intercom_groups16` FOREIGN KEY (`as_intercom_groups_id`) REFERENCES `as_intercom_groups` (`as_intercom_groups_id`),
  CONSTRAINT `Refas_users17` FOREIGN KEY (`as_users_id`) REFERENCES `as_users` (`as_users_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS `as_intercom_groups`;
CREATE TABLE `as_intercom_groups` (
  `as_intercom_groups_id` int(10) unsigned NOT NULL auto_increment,
  `name` varchar(255) NOT NULL default '',
  `is_enabled` tinyint(1) NOT NULL default '0',
  `is_talkback_enabled` tinyint(1) NOT NULL default '0',
  `is_private` tinyint(1) NOT NULL default '0',
  PRIMARY KEY  (`as_intercom_groups_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS `as_ldap_servers`;
CREATE TABLE `as_ldap_servers` (
  `as_ldap_servers_id` int(10) unsigned NOT NULL auto_increment,
  `hostname` varchar(255) NOT NULL default '',
  `port` int(11) NOT NULL default '0',
  `secure_connect` tinyint(1) NOT NULL default '0',
  `base_dn` varchar(255) default NULL,
  `user_dn` varchar(255) default NULL,
  `password` varchar(255) default NULL,
  PRIMARY KEY  (`as_ldap_servers_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS `as_phone_devices`;
CREATE TABLE `as_phone_devices` (
  `as_phone_devices_id` int(10) unsigned NOT NULL auto_increment,
  `as_users_id` int(10) unsigned default NULL,
  `is_primary_device` tinyint(1) NOT NULL default '0',
  `is_ip_phone` tinyint(1) unsigned NOT NULL default '0',
  `name` varchar(255) default NULL,
  `mac_address` varchar(16) default NULL,
  PRIMARY KEY  (`as_phone_devices_id`),
  UNIQUE KEY `mac_address` (`mac_address`),
  KEY `Refas_users2` (`as_users_id`),
  CONSTRAINT `Refas_users2` FOREIGN KEY (`as_users_id`) REFERENCES `as_users` (`as_users_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS `as_recordings`;
CREATE TABLE `as_recordings` (
  `as_recordings_id` int(10) unsigned NOT NULL auto_increment,
  `as_call_records_id` int(10) unsigned NOT NULL default '0',
  `as_users_id` int(10) unsigned NOT NULL default '0',
  `url` varchar(255) NOT NULL default '',
  `start` timestamp NOT NULL default '0000-00-00 00:00:00',
  `end` timestamp default '0000-00-00 00:00:00',
  `type` int(11) NOT NULL default '0',
  PRIMARY KEY  (`as_recordings_id`),
  KEY `Refas_call_records8` (`as_call_records_id`),
  KEY `Refas_users24` (`as_users_id`),
  CONSTRAINT `Refas_call_records8` FOREIGN KEY (`as_call_records_id`) REFERENCES `as_call_records` (`as_call_records_id`),
  CONSTRAINT `Refas_users24` FOREIGN KEY (`as_users_id`) REFERENCES `as_users` (`as_users_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS `as_remote_agents`;
CREATE TABLE `as_remote_agents` (
  `as_remote_agents_id` int(10) unsigned NOT NULL auto_increment,
  `as_users_id` int(10) unsigned NOT NULL default '0',
  `as_phone_devices_id` int(10) unsigned default NULL,
  `as_external_numbers_id` int(10) unsigned default NULL,
  `user_level` int(10) unsigned NOT NULL default '0',
  PRIMARY KEY  (`as_remote_agents_id`),
  UNIQUE KEY `Index_5` (`as_users_id`),
  KEY `FK_as_remote_agents_1` (`as_users_id`),
  KEY `FK_as_remote_agents_2` (`as_phone_devices_id`),
  KEY `FK_as_remote_agents_3` (`as_external_numbers_id`),
  CONSTRAINT `FK_as_remote_agents_1` FOREIGN KEY (`as_users_id`) REFERENCES `as_users` (`as_users_id`),
  CONSTRAINT `FK_as_remote_agents_2` FOREIGN KEY (`as_phone_devices_id`) REFERENCES `as_phone_devices` (`as_phone_devices_id`) ON DELETE SET NULL,
  CONSTRAINT `FK_as_remote_agents_3` FOREIGN KEY (`as_external_numbers_id`) REFERENCES `as_external_numbers` (`as_external_numbers_id`) ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='InnoDB free: 11264 kB; (`as_external_numbers_id`) REFER `app';


DROP TABLE IF EXISTS `as_scheduled_conferences`;
CREATE TABLE `as_scheduled_conferences` (
  `as_scheduled_conferences_id` int(10) unsigned NOT NULL auto_increment,
  `as_users_id` int(10) unsigned default NULL,
  `host_conf_id` int(11) NOT NULL default '0',
  `participant_conf_id` int(11) NOT NULL default '0',
  `mms_id` int(11) NOT NULL default '0',
  `mms_conf_id` int(11) NOT NULL default '0',
  `scheduled_timestamp` timestamp NOT NULL default '0000-00-00 00:00:00',
  `duration_minutes` int(11) NOT NULL default '0',
  `num_participants` int(11) NOT NULL default '0',
  `status` int(11) NOT NULL default '0',
  PRIMARY KEY  (`as_scheduled_conferences_id`),
  KEY `Refas_users23` (`as_users_id`),
  CONSTRAINT `Refas_users23` FOREIGN KEY (`as_users_id`) REFERENCES `as_users` (`as_users_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS `as_session_records`;
CREATE TABLE `as_session_records` (
  `as_session_records_id` int(10) unsigned NOT NULL auto_increment,
  `as_auth_records_id` int(10) unsigned default NULL,
  `start` timestamp NOT NULL default CURRENT_TIMESTAMP on update CURRENT_TIMESTAMP,
  `end` timestamp NOT NULL default '0000-00-00 00:00:00',
  PRIMARY KEY  (`as_session_records_id`),
  KEY `Refas_auth_records10` (`as_auth_records_id`),
  CONSTRAINT `Refas_auth_records10` FOREIGN KEY (`as_auth_records_id`) REFERENCES `as_auth_records` (`as_auth_records_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS `as_single_reach_numbers`;
CREATE TABLE `as_single_reach_numbers` (
  `as_single_reach_numbers_id` INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  `as_users_id` INTEGER UNSIGNED NOT NULL DEFAULT 0,
  `number` VARCHAR(255) NOT NULL DEFAULT '',
  PRIMARY KEY(`as_single_reach_numbers_id`),
  CONSTRAINT `FK_as_single_reach_numbers_1` FOREIGN KEY `FK_as_single_reach_numbers_1` (`as_users_id`)
    REFERENCES `as_users` (`as_users_id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT
)
ENGINE = InnoDB;


DROP TABLE IF EXISTS `as_user_group_members`;
CREATE TABLE `as_user_group_members` (
  `as_user_group_members_id` int(10) unsigned NOT NULL auto_increment,
  `as_user_groups_id` int(10) unsigned NOT NULL default '0',
  `as_users_id` int(10) unsigned NOT NULL default '0',
  `user_level` int(11) NOT NULL default '0',
  PRIMARY KEY  (`as_user_group_members_id`),
  KEY `FK_as_user_group_members_1` (`as_users_id`),
  KEY `FK_as_user_group_members_2` (`as_user_groups_id`),
  CONSTRAINT `FK_as_user_group_members_1` FOREIGN KEY (`as_users_id`) REFERENCES `as_users` (`as_users_id`),
  CONSTRAINT `FK_as_user_group_members_2` FOREIGN KEY (`as_user_groups_id`) REFERENCES `as_user_groups` (`as_user_groups_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS `as_user_groups`;
CREATE TABLE `as_user_groups` (
  `as_user_groups_id` int(10) unsigned NOT NULL auto_increment,
  `name` varchar(255) NOT NULL default '',
  PRIMARY KEY  (`as_user_groups_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS `as_users`;
CREATE TABLE `as_users` (
  `as_users_id` int(10) unsigned NOT NULL auto_increment,
  `as_ldap_servers_id` int(11) unsigned default NULL,
  `username` varchar(255) NOT NULL default '',
  `account_code` int(11) NOT NULL default '0',
  `pin` int(11) NOT NULL default '0',
  `password` varchar(255) NOT NULL default '',
  `first_name` varchar(64) NOT NULL default '',
  `last_name` varchar(64) NOT NULL default '',
  `email` varchar(255) default NULL,
  `status` int(11) NOT NULL default '0',
  `created` timestamp NOT NULL default CURRENT_TIMESTAMP on update CURRENT_TIMESTAMP,
  `last_used` timestamp NOT NULL default '0000-00-00 00:00:00',
  `lockout_threshold` int(11) NOT NULL default '0',
  `lockout_duration` time NOT NULL default '00:00:00',
  `last_lockout` timestamp NOT NULL default '0000-00-00 00:00:00',
  `failed_logins` int(11) NOT NULL default '0',
  `current_active_sessions` int(11) NOT NULL default '0',
  `max_concurrent_sessions` int(11) NOT NULL default '0',
  `pin_change_required` tinyint(1) NOT NULL default '0',
  `external_auth_enabled` tinyint(1) NOT NULL default '0',
  `record` tinyint(1) NOT NULL default '0',
  `recording_visible` tinyint(1) NOT NULL default '0',
  `external_auth_dn` text,
  `ldap_synched` tinyint(1) NOT NULL default '0',
  `time_zone` varchar(64) NOT NULL default '',
  `ar_transfer_number` varchar(64) NOT NULL default '',
  `placed_calls` int(11) NOT NULL default '0',
  `successfull_calls` int(11) NOT NULL default '0',
  `total_call_time` int(11) NOT NULL default '0',
  PRIMARY KEY  (`as_users_id`),
  KEY `FK_as_users_1` (`as_ldap_servers_id`),
  CONSTRAINT `FK_as_users_1` FOREIGN KEY (`as_ldap_servers_id`) REFERENCES `as_ldap_servers` (`as_ldap_servers_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS `as_voicemail_settings`;
CREATE TABLE `as_voicemail_settings` (
  `as_voicemail_settings_id` int(10) unsigned NOT NULL auto_increment,
  `as_users_id` int(10) unsigned NOT NULL default '0',
  `is_first_login` tinyint(1) NOT NULL default '1',
  `account_status` int(10) unsigned NOT NULL default '1',
  `greeting_filename` varchar(255) NOT NULL default 'DefaultVoiceMailGreeting.wav',
  `sort_order` int(10) unsigned NOT NULL default '1',
  `notification_method` int(10) unsigned NOT NULL default '1',
  `notification_address` varchar(100) NOT NULL default '',
  `max_message_length` int(10) unsigned NOT NULL default '240',
  `max_number_messages` int(10) unsigned NOT NULL default '40',
  `max_storage_days` int(10) unsigned NOT NULL default '30',
  `describe_each_message` tinyint(1) NOT NULL default '1',
  PRIMARY KEY  (`as_voicemail_settings_id`),
  KEY `as_users_id` (`as_users_id`),
  CONSTRAINT `as_voicemail_settings_ibfk_1` FOREIGN KEY (`as_users_id`) REFERENCES `as_users` (`as_users_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


DROP TABLE IF EXISTS `as_voicemails`;
CREATE TABLE `as_voicemails` (
  `as_voicemails_id` int(10) unsigned NOT NULL auto_increment,
  `as_users_id` int(10) unsigned NOT NULL default '0',
  `status` int(10) unsigned NOT NULL default '1',
  `filename` varchar(255) NOT NULL default '',
  `length` int(10) NOT NULL default '0',
  PRIMARY KEY  (`as_voicemails_id`),
  UNIQUE KEY `as_users_id` (`as_users_id`),
  CONSTRAINT `as_voicemails_ibfk_1` FOREIGN KEY (`as_users_id`) REFERENCES `as_users` (`as_users_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT;
SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS;
SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
