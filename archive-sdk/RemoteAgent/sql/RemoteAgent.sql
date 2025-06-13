create table agent_records
(
	agent_records_id	INT(10) UNSIGNED PRIMARY KEY AUTO_INCREMENT,
	agent_dn			VARCHAR(50) UNIQUE NOT NULL,
	routing_guid		VARCHAR(100) NOT NULL,
	is_recorded           TINYINT(1) NOT NULL default '0'
);

	