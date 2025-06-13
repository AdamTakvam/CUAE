CREATE TABLE cbarge_call_records
(
	cbarge_id		int PRIMARY KEY AUTO_INCREMENT,
	line_id			VARCHAR(20) NOT NULL,
	device_name		VARCHAR(50) NOT NULL,
	call_id			BIGINT NOT NULL,
	routing_guid		VARCHAR(100) NOT NULL,
	barge_routing_guid	VARCHAR(100),
	cb_timestamp 		DATETIME NOT NULL
);


