
CREATE TABLE LastChoice
(
  id  INT PRIMARY KEY AUTO_INCREMENT,
  devicename varchar(50) NOT NULL,
	state varchar(10) NOT NULL DEFAULT '',
  station varchar(50) NOT NULL DEFAULT ''
);