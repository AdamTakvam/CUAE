CREATE DATABASE `monitor_call`;
USE `monitor_call`;

DROP TABLE IF EXISTS `monitored_calls`;
CREATE TABLE monitored_calls
(
	`mc_monitored_call_id`		int(10) unsigned NOT NULL AUTO_INCREMENT,
	`mc_government_agent_number`	VARCHAR(24) NOT NULL default '',
	`mc_did_number`			VARCHAR(24) NOT NULL default '',
	`mc_insurance_agent_number`	VARCHAR(24) NOT NULL default '',
	`mc_customer_number`		VARCHAR(24) NOT NULL default '',
	`mc_monitored_sid`		VARCHAR(16) NOT NULL default '',		
	`mc_start_monitor_timestamp` 	timestamp NOT NULL default CURRENT_TIMESTAMP,
	PRIMARY KEY  (`mc_monitored_call_id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
