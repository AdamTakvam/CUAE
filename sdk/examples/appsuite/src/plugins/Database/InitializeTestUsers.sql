-- CREATE USERS

INSERT INTO as_users (username, password, account_code, pin, first_name,
                      last_name, status, created, lockout_threshold, lockout_duration,
                      failed_logins, max_concurrent_sessions, current_active_sessions,
                      pin_change_required, external_auth_enabled, gmt_offset)
VALUES
                      ('achaney','metreos', 11,       123,'Adam',     'Chaney',  1,
                       '1997-12-15 23:50:26',   3,     '00:39:38'      , 0            , 3,
                       0                      , 0                  , 0                    , -6);

INSERT INTO as_users (username, password, account_code, pin, first_name,
                      last_name, status, created, lockout_threshold, lockout_duration,
                      failed_logins, max_concurrent_sessions, current_active_sessions,
                       pin_change_required, external_auth_enabled, gmt_offset)
VALUES
                     ('johnz',  'metreos', '22',       '123','John',     'ZHachoo',  1,
                      '1997-12-15 23:50:27',   3, '00:39:37'      , 0            , 3,
                      0  , 0                , 0                    , -6);

INSERT INTO as_users (username, password, account_code, pin, first_name,
                      last_name, status, created, lockout_threshold, lockout_duration,
                      failed_logins, max_concurrent_sessions, current_active_sessions,
                      pin_change_required, external_auth_enabled, gmt_offset)
VALUES
                     ('hung',   'metreos', '33',       '123','Hung',     'Nguyen',  1,
                      '1997-12-15 23:50:26',   3, '00:39:37'      , 0            , 3,
                      0   , 0                , 0                    , -6);

INSERT INTO as_users (username, password, account_code, pin, first_name,
                      last_name, status, created, lockout_threshold, lockout_duration,
                      failed_logins, max_concurrent_sessions, current_active_sessions,
                      pin_change_required, external_auth_enabled, gmt_offset, record, recording_visible)
VALUES               
                      ('scall',  'metreos', '44',       '123','Seth',     'Call',  1,
                       '1997-12-15 23:50:26',   3, '00:39:37'      , 0            , 3,
                       0     , 0             , 0                    , -6, 1, 1);

INSERT INTO as_users (username, password, account_code, pin, first_name,
                      last_name, status, created, lockout_threshold, lockout_duration,
                      failed_logins, max_concurrent_sessions, current_active_sessions,
                      pin_change_required, external_auth_enabled, gmt_offset)
VALUES
                      ('marascio','metreos',   '55',       '123','Louis',     'Marascio',  4,
                       '1997-12-15 23:50:26',   3, '00:39:37'      , 0            , 3,
                       0 , 0                  , 0                    , -6);





-- CREATE PHONE DEVICES AND LINES

-- Add Seth 2 phones
INSERT INTO as_phone_devices (as_users_id, name, is_primary_device, mac_address)
VALUES                       (4,          'Seths Main Phone', 1, 'SEP000F23000882');

INSERT INTO as_directory_numbers (as_phone_devices_id, directory_number, is_primary_number)
VALUES                      (1, 1000, 1);
INSERT INTO as_directory_numbers (as_phone_devices_id, directory_number, is_primary_number)
VALUES                      (1, 1001, 0);


INSERT INTO as_phone_devices (as_users_id, name, is_primary_device, mac_address)
VALUES                       (4,         'Seths Other 7960', 0,                 'SEP0008A3263267C');

INSERT INTO as_directory_numbers (as_phone_devices_id, directory_number, is_primary_number)
VALUES                           (2, 1006, 1);
INSERT INTO as_directory_numbers (as_phone_devices_id, directory_number, is_primary_number)
VALUES                           (2, 1007, 0);


-- Add Adam 1 phone
INSERT INTO as_phone_devices (as_users_id, name, is_primary_device, mac_address)
VALUES                       (1,          'Adams Main Phone', 1, 'SEP0009B70A82DF');

INSERT INTO as_directory_numbers (as_phone_devices_id, directory_number, is_primary_number)
VALUES                      (3, 1010, 1);
INSERT INTO as_directory_numbers (as_phone_devices_id, directory_number, is_primary_number)
VALUES                      (3, 1011, 0);


-- Add John 2 phones
INSERT INTO as_phone_devices (as_users_id, name, is_primary_device, mac_address)
VALUES                       (2,         'JZs Color Phone', 1,                'SEP000E83E532F3');

INSERT INTO as_directory_numbers (as_phone_devices_id, directory_number, is_primary_number)
VALUES                      (4, 1004, 1);
INSERT INTO as_directory_numbers (as_phone_devices_id, directory_number, is_primary_number)
VALUES                      (4, 1005, 0);

INSERT INTO as_phone_devices (as_users_id, name, is_primary_device, mac_address)
VALUES                       (2,         'JZs other phone',0,                'SEP0008210F8A4F');

INSERT INTO as_directory_numbers (as_phone_devices_id, directory_number, is_primary_number)
VALUES                      (5, 1012, 1);
INSERT INTO as_directory_numbers (as_phone_devices_id, directory_number, is_primary_number)
VALUES                      (5, 1013, 0);

-- Add Hung 1 phone
INSERT INTO as_phone_devices (as_users_id, name, is_primary_device, mac_address)
VALUES                       (3,          'Hungs Main Phone', 1, 'SEP0007EB462C09');

INSERT INTO as_directory_numbers (as_phone_devices_id, directory_number, is_primary_number)
VALUES                      (6, 1002, 1);








-- config values

INSERT INTO as_configs (name, value, description, groupname, required)
VALUES                 ('webserver_root_filepath', 'C:\\Program Files\\Apache Group\\Apache\\htdocs', 
                        'File path on the web server which points to the root directory',
                        'Recording', 1);
                        
INSERT INTO as_configs (name, value, description, groupname, required)
VALUES                 ('webserver_host', '127.0.0.1', 
                        'IP or canonical name URL to the web server',
                        'Recording', 1);

INSERT INTO as_configs (name, value, description, groupname, required)
VALUES                 ('record_rel_path', 'recordings', 
                        'The path which corresponds the the file path and web server path which holds the recordings',
                        'Recording', 1);
                        
INSERT INTO as_configs (name, value, description, groupname, required)
VALUES                 ('recordings_expiration', '3', 
                        'Amount of days to expire the recording',
                        'Recording', 1);      
                        
INSERT INTO as_configs (name, value, description, groupname, required)
VALUES                 ('mediaserver_rel_path', 'media', 
                        'The portion of the URL found after the host name of the media server',
                        'Recording', 1);                                 
