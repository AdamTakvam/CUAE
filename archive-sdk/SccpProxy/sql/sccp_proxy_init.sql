CREATE DATABASE sccp_proxy_activity;

USE sccp_proxy_activity;

CREATE TABLE registrations
(
	id INT NOT NULL auto_increment,
	sid varchar(255) NOT NULL,
	ccmaddress varchar(255) NOT NULL,
	starttime DATETIME NOT NULL,
	endtime DATETIME NOT NULL DEFAULT '0000-00-00 00:00:00',
	num_ring_in INT NOT NULL DEFAULT 0,
	num_ring_out INT NOT NULL DEFAULT 0,
	num_busy INT NOT NULL DEFAULT 0,
	num_connected INT NOT NULL DEFAULT 0,
	PRIMARY KEY(id)	 	
);