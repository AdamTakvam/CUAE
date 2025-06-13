CREATE DATABASE IF NOT EXISTS demosite;

USE demosite;

CREATE TABLE users
(
	id				INT				NOT NULL	auto_increment,
	email			VARCHAR(255)	NOT NULL,
	password		VARCHAR(255)	NOT NULL,
	account_code	VARCHAR(255)	NOT NULL,
	pin				VARCHAR(255)    NOT NULL,
	first_name		VARCHAR(255)	NOT NULL,
	last_name		VARCHAR(255)	NOT NULL,
	company			VARCHAR(255)	NOT NULL,
	dayphone		VARCHAR(255)	DEFAULT '',
	status			INT				DEFAULT 1,         /* 1 active, 2 banned*/
	upcdemo			BIT				DEFAULT 1,
	remoteagent     BIT				DEFAULT 0,
	
	PRIMARY KEY(id)
);

/* User preferences for upc emails */
CREATE TABLE upc_addresses
(
	users_id		INT				NOT NULL,
	address1		varchar(255)	DEFAULT '',
	address2		varchar(255) DEFAULT '',
	address3		varchar(255) DEFAULT '',
	address4		varchar(255) DEFAULT '',
	address5		varchar(255) DEFAULT '',
	
	FOREIGN KEY(users_id) REFERENCES users(id)
);

CREATE TABLE usage_stats
(
	id				INT				NOT NULL	auto_increment,
	ip				VARCHAR(255)	DEFAULT '',
	users_id		INT,
	time		    TIMESTAMP		DEFAULT NOW(),
	url				VARCHAR(255),
	login			BIT				NOT NULL,
	
	PRIMARY KEY(id),
	FOREIGN KEY(users_id) REFERENCES users(id)
);