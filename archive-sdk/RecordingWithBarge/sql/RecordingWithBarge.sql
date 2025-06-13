CREATE TABLE rec_with_barge
(
  rec_with_barge_id INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  line_id VARCHAR(25) NOT NULL,
  conference_id VARCHAR(25) NOT NULL,
	mms_id INT(10) UNSIGNED  NOT NULL,
	PRIMARY KEY(rec_with_barge_id)
);