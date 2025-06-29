CREATE TABLE ar_active_calls
(
	ar_active_calls_id	int PRIMARY KEY AUTO_INCREMENT,
	as_user_id	int unsigned NOT NULL,
	ar_call_from VARCHAR(20) NOT NULL,
	ar_call_to   VARCHAR(20) NOT NULL,
	ar_call_timestamp DATETIME NOT NULL,
	ar_routing_guid VARCHAR(50) NOT NULL,
	ar_was_swapped tinyint(1) NOT NULL default '0'
);
