CREATE DATABASE `enroll_speaker`;
USE `enroll_speaker`;

DROP TABLE IF EXISTS `enrolled_speakers`;
CREATE TABLE enrolled_speakers
(
	`es_enrolled_speaker_id`	int(10) unsigned NOT NULL AUTO_INCREMENT,
	`es_enrolled_speaker_passcode`	VARCHAR(24) NOT NULL default '',
	`es_enrolled_group_name`	VARCHAR(24) NOT NULL default '',
	`es_enrolled_speaker_name`	VARCHAR(24) NOT NULL default '',
	`es_enrolled_sub_name`		VARCHAR(24) NOT NULL default '',	
	`es_trained`			VARCHAR(2) NOT NULL default '0',
	`es_user_name`			VARCHAR(24) NOT NULL default '',
	`es_password`			VARCHAR(24) NOT NULL default '',	
	PRIMARY KEY  (`es_enrolled_speaker_id`)
) ENGINE=MyISAM DEFAULT CHARSET=latin1;
