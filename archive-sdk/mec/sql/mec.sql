CREATE TABLE conferences
(
    id                 integer     NOT NULL AUTO_INCREMENT,
    mms_conf_id        varchar(100) DEFAULT 0 NOT NULL,
    conf_session_id    varchar(40)  NOT NULL,
    is_public          bit         DEFAULT 0 NOT NULL,
    is_scheduled       bit         DEFAULT 0 NOT NULL,
    is_recorded        bit         DEFAULT 0 NOT NULL,
    start		     datetime    DEFAULT '0000-00-00 00:00:00',
    end                datetime    DEFAULT '0000-00-00 00:00:00',

       PRIMARY KEY(id)
) ENGINE = InnoDB;

CREATE TABLE participants
(
    id							integer      NOT NULL AUTO_INCREMENT,
    conferences_id		        integer      NOT NULL DEFAULT 0,
    mms_connection_id			varchar(50)  NOT NULL DEFAULT '',
    callid						varchar(50)  NOT NULL DEFAULT '',
    phone_number				varchar(50)  NOT NULL DEFAULT '',
    description					varchar(50)  NOT NULL DEFAULT '',
    ishost						bit          NOT NULL DEFAULT 0,
    status						int          NOT NULL DEFAULT 0,
    first_connected				datetime     NOT NULL  DEFAULT '0000-00-00 00:00:00',
    last_connected				datetime     NOT NULL  DEFAULT '0000-00-00 00:00:00',
    disconnected				datetime     NOT NULL DEFAULT '0000-00-00 00:00:00',

         PRIMARY KEY(id),
         INDEX(conferences_id),
         FOREIGN KEY(conferences_id) REFERENCES conferences(id)
            ON DELETE CASCADE
) ENGINE = InnoDB;