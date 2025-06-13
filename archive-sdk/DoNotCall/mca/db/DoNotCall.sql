
CREATE TABLE latestCall
(
  fromNumber VARCHAR(50) NOT NULL,
	toNumber  VARCHAR(50) NOT NULL,
  PRIMARY KEY(fromNumber)
) ENGINE=InnoDB;

CREATE TABLE metreosDnc
(
	toNumber  VARCHAR(50) NOT NULL,
  PRIMARY KEY(toNumber)
) ENGINE=InnoDB;
