CREATE DATABASE Reserve;
USE Reserve;

CREATE TABLE Configuration
(
	CurrentDisplayGuid VARCHAR(50) DEFAULT 'NONE'
);

INSERT INTO Configuration (CurrentDisplayGuid) VALUES ('NONE');

CREATE TABLE SkipDisplayDevices
(
	Devicename VARCHAR(20) NOT NULL UNIQUE
);