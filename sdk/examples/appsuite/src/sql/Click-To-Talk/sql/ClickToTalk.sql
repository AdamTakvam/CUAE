CREATE TABLE Callees 
(
	ID				INTEGER		PRIMARY KEY,
	ConferencesID	      VARCHAR(20),
	Address			VARCHAR(20),
	Name			      VARCHAR(20)	
);

CREATE TABLE Conferences 
(
	ID				VARCHAR(20)		PRIMARY KEY,
	HostIP			VARCHAR(20),
	HostDescription	      VARCHAR(20),
	HostUsername            VARCHAR(20),
	HostPassword            VARCHAR(20),
	Record			VARCHAR(20),
      RecordEnded             VARCHAR(20),
      RecordConnectionId      INTEGER,
	Email			      VARCHAR(20),
	TimeStamp		      DATETIME
);

CREATE TABLE Errors
(
	ID				INTEGER		PRIMARY KEY,
	ConferencesID           VARCHAR(20),
	Error			      VARCHAR(20)
);