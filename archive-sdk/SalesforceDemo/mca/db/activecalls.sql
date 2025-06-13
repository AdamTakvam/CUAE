
CREATE TABLE contactinfo
(
  id INT unsigned NOT NULL auto_increment,
  phone VARCHAR(50),
	firstname VARCHAR(255),
  lastname VARCHAR(255),
  accountname VARCHAR(255),
  street VARCHAR(255),
  city VARCHAR(255),
  state VARCHAR(255),
  zip VARCHAR(255),
  country VARCHAR(255),
	PRIMARY KEY(id)
);


CREATE TABLE activecalls
(
  id INT unsigned NOT NULL auto_increment,
  devicename VARCHAR(25),
  to_number VARCHAR(25) NOT NULL DEFAULT '',
  from_number VARCHAR(25) NOT NULL DEFAULT '',
	active TIMESTAMP NOT NULL DEFAULT '0000-00-00 00:00:00',
  direction tinyint(1) unsigned NOT NULL default '0', /* 0=inbound, 1=outbound */
	state tinyint(1) unsigned NOT NULL default '0', /* 0=hold, 1=active */
	contactinfoid INT unsigned,
	PRIMARY KEY(id),
	FOREIGN KEY (contactinfoid) REFERENCES contactinfo(id)
);
