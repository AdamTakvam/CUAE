CREATE TABLE sc_participants
(
	sc_participants_id	INT  AUTO_INCREMENT,
	sc_participants_conference_pin	INT UNSIGNED NOT NULL UNIQUE,
	sc_participants_count	INT unsigned NOT NULL DEFAULT '0',
  sc_participants_playing BIT NOT NULL DEFAULT 0,
  PRIMARY KEY(sc_participants_id)
);

CREATE TABLE sc_files /* Files outstanding to play to the conference */
(
    sc_files_id			INT				AUTO_INCREMENT,
    pinId				INT   UNSIGNED 			NOT NULL,
    time				TIMESTAMP		DEFAULT NOW(),
    type				INT				NOT NULL, /* The type of file.  1 = entering, 2 = exiting */
    filename			VARCHAR(255)	DEFAULT '',
    PRIMARY KEY(sc_files_id)
);