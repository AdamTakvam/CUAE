--Execute this script file inside of MySQL admin tool (mysql) as a user with GRANT, CREATE, UPDATE privilages
--(most likely database admin user)

--Create the metreos database
create database metreos;

--
--Here we create a table for the scheduledconference application. confNumber is a 5-digit pin
--number, confId is the internal Metreos Media Server conference number, scheduledFor is the
--date that the conference was scheduled for, numHours is the number of hours that the conference
--is scheduled to last, and numParticipants is used to store the number of participants currently
--in the conference. 
--
create table metreos.scheduledconferences ( confNumber INT PRIMARY KEY, confId INT NOT NULL,
	scheduledFor DATETIME NOT NULL, numHours INT NOT NULL, numParticipants INT NOT NULL);
	
--
--In the following line we create a user named metreos, with password metreos, that is 
--allowed to connect from any host (represented by '%'). This user has the power to
--perform deletes, inserts, selects, and updates on any tables in the metreos database
--
GRANT DELETE,INSERT,SELECT,UPDATE on metreos.* to 'metreos'@'%';
GRANT DELETE,INSERT,SELECT,UPDATE on metreos.* to 'metreos'@'localhost';

--
--Here we update the password column of the metreos user to use the old MySQL password format.
--If this is not done, MySQL ODBC 3.51 will not function properly
--
UPDATE mysql.user SET Password = OLD_PASSWORD('metreos') where user='metreos';

--
--flush privileges - makes MySQL re-read it's grants table
--
FLUSH PRIVILEGES;
