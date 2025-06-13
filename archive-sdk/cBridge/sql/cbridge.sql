CREATE TABLE cbridge_conferences
(
	cbridge_id	int PRIMARY KEY AUTO_INCREMENT,
	line_id		VARCHAR(20) NOT NULL,
	routingGuid	VARCHAR(100) NOT NULL,
	cb_timestamp 	DATETIME NOT NULL,
	is_recorded 	tinyint(1) NOT NULL default '0'
);
--routingGuid is the routingGuid of a participants cBridge instance
--CREATE TABLE cbridge_conference_participants
--(
--	cbridge_participants_id	int PRIMARY KEY AUTO_INCREMENT,
--	line_id			VARCHAR(20),
--	from_number 		VARCHAR(20) NOT NULL,
--	callId 			VARCHAR(100) NOT NULL,
--	timestamp 		DATETIME,
--	is_recorded 		tinyint(1) NOT NULL default '0',
--	is_moderator 		tinyint(1) NOT NULL default '0',
--	is_muted 		tinyint(1) NOT NULL default '0'
--);