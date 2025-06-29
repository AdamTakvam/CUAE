CREATE TABLE cbarge_call_records
(
	cbarge_id		int PRIMARY KEY AUTO_INCREMENT,
	directory_number	VARCHAR(20) NOT NULL,
	sid			VARCHAR(50) NOT NULL,
	call_instance		int NOT NULL,
	call_reference		int unique NOT NULL,
	line_instance		int NOT NULL,
  	dn_li			VARCHAR(40) unique NOT NULL,
	mms_id			unsigned int NOT NULL,
	conference_id		VARCHAR(20),
	routing_guid		VARCHAR(100) NOT NULL,
	barge_routing_guid	VARCHAR(100),
	cb_timestamp 		DATETIME NOT NULL
);

CREATE TABLE cbarge_records
(
    cbarge_id           	int PRIMARY KEY AUTO_INCREMENT,
    directory_number    	VARCHAR(20) NOT NULL,
    line_instance		int NOT NULL,
    dn_li			VARCHAR(40) unique NOT NULL,
    mms_id              	unsigned int default '0',
    conference_id       	VARCHAR(20),
    num_participants    	int default '0',
    cb_timestamp 		DATETIME NOT NULL
);