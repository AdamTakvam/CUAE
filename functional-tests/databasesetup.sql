CREATE DATABASE IF NOT EXISTS testframework;

USE testframework;

DROP TABLE IF EXISTS main;

CREATE TABLE main(
    id INT NOT NULL auto_increment,
    name VARCHAR(30),
    value VARCHAR(30),
    PRIMARY KEY (id)
);

INSERT INTO main (name, value) VALUES('name1', 'value1');