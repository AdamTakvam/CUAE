CREATE TABLE `Callees` 
(
	`ID`			int(10) unsigned NOT NULL auto_increment,
	`ConferencesID`	      int(10),
	`Address`			varchar(100),
	`Name`			varchar(100),
  PRIMARY KEY (`ID`)
);

CREATE TABLE `Conferences` 
(
	`ID`			int(10) unsigned NOT NULL AUTO_INCREMENT,
	`HostIP`			varchar(25),
	`HostDescription`     varchar(255),
	`HostUsername`        varchar(25),
	`HostPassword`        varchar(25),
	`Record`			varchar(10),
  `RecordEnded`         varchar(10),
  `RecordConnectionId`	int(32) DEFAULT 0,
	`Email`			varchar(100),
	`TimeStamp`		timestamp 	DEFAULT NOW(),
  PRIMARY KEY (`ID`)
);

CREATE TABLE `Errors`
(
	`ID`			int(10) unsigned auto_increment,
	`ConferencesID`       int(10),
	`Error`			varchar(255),
  PRIMARY KEY (`ID`)
);