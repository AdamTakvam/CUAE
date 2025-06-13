CREATE DATABASE RetailDemo;
USE RetailDemo;

CREATE TABLE Users
(
	UserID VARCHAR(50) DEFAULT '',
	Username VARCHAR(50) DEFAULT '',
	Extension VARCHAR(50) DEFAULT '',
	CheckedIn TINYINT(1) UNSIGNED NOT NULL DEFAULT '0',
	PRIMARY KEY(UserID)
);


INSERT INTO Users (UserID, Username, Extension, CheckedIn) VALUES ('11111', 'David Smith', '', 0);
INSERT INTO Users (UserID, Username, Extension, CheckedIn) VALUES ('22222', 'Karen Pond', '', 0);
INSERT INTO Users (UserID, Username, Extension, CheckedIn) VALUES ('33333', 'John Doe', '', 0);


CREATE TABLE Preferences
(
	device VARCHAR(50) DEFAULT '',
	open TINYINT(1) UNSIGNED NOT NULL DEFAULT '0',
	PRIMARY KEY(device)
);