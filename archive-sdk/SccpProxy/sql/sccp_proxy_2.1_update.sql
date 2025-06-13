USE sccp_proxy_activity;

ALTER TABLE registrations MODIFY starttime DATETIME NOT NULL;
ALTER TABLE registrations MODIFY endtime DATETIME NOT NULL;
