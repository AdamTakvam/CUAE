CREATE DATABASE demo;
USE demo;

CREATE TABLE users
(
    id          INT         NOT NULL    AUTO_INCREMENT,
    username    VARCHAR(50) DEFAULT '',
    firstname   VARCHAR(50) DEFAULT '',
    lastname    VARCHAR(50) DEFAULT '',
    phone       VARCHAR(50) DEFAULT '',
    PRIMARY KEY(id)
);