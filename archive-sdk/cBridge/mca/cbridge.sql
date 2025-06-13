CREATE TABLE cbridge_conferences
(
	cbridge_id	int PRIMARY KEY AUTO_INCREMENT,
	line_id		VARCHAR(20) NOT NULL,
	routingGuid	VARCHAR(100) NOT NULL,
	cb_timestamp 	BIGINT NOT NULL default '0',
	is_recorded 	tinyint(1) NOT NULL default '0'
);
