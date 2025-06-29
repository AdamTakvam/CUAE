CREATE DATABASE IF NOT EXISTS legacydb;

USE legacydb;

DROP TABLE IF EXISTS errors;

CREATE TABLE errors
(
	id				INT			 NOT NULL AUTO_INCREMENT,
	description		varchar(255) NOT NULL,
	time            DATETIME    NOT NULL,
	PRIMARY KEY(id)
) ENGINE=INNODB;

INSERT INTO errors (description, time) VALUES ('Some terrible error', NOW());
INSERT INTO errors (description, time) VALUES ('Not so bad, but bad enough to get logged', NOW());
INSERT INTO errors (description, time) VALUES ('You won\'t believe this but this is one bad error', NOW());