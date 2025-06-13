<?php

/**
 * A list of defined constants pertaining to the requesting and displaying
 * of statistics as it pertains to the CUAE Stats Server.
 */

// OID numbers for desired statisitcal metrics

define('STATS_OID_CPULOAD',        2000);
define('STATS_OID_CUASMEMORY',     2001);
define('STATS_OID_CUMEMEMORY',     2002);

define('STATS_OID_APPSESSIONS',    2010);
define('STATS_OID_CALLS',          2011);

define('STATS_OID_ROUTERMESSAGES', 2020);
define('STATS_OID_ROUTEREVENTS',   2021);
define('STATS_OID_ROUTERACTIONS',  2022);

define('STATS_OID_VOICE',          2100);
define('STATS_OID_RTP',            2101);
define('STATS_OID_ERTP',           2102);
define('STATS_OID_CONFERENCE',     2103);
define('STATS_OID_SPEECHINTEG',    2104);
define('STATS_OID_TTS',            2105);
define('STATS_OID_CONFERENCE_SLOTS',2106);
define('STATS_OID_CONFERENCE_USE', 2107);


// Available graph intervals for retrieval

define('STATS_INTERVAL_HOUR',          'Hour');
define('STATS_INTERVAL_6HOURS',        'SixHour');
define('STATS_INTERVAL_12HOURS',       'TwelveHour');
define('STATS_INTERVAL_DAY',           'Day');
define('STATS_INTERVAL_WEEK',          'Week');
define('STATS_INTERVAL_MONTH',         'Month');
define('STATS_INTERVAL_YEAR',          'Year');

?>