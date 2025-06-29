<?php
// $Id: autocomplete2_server.php,v 1.1 2004/12/12 00:15:38 jsweat Exp $
/**
* This is a remote script to call from Javascript
*/

define ('JPSPAN_ERROR_DEBUG',TRUE);
require_once '../JPSpan.php';
require_once JPSPAN . 'Server/PostOffice.php';

//-----------------------------------------------------------------------------------
class Autocomplete2 {

    function getCountryList() {
    	include 'data/countries.php';
		$ret = array_keys($countries);
		array_walk($ret, create_function('&$i,$k','$i = ucwords($i);'));
		return $ret;
    }
    function getAirports($country) {
    	include 'data/countries.php';
		if (array_key_exists($key = strtolower($country),$countries)) {
			return $countries[$key];
		}
		return array();
	}

}

$S = & new JPSpan_Server_PostOffice();
$S->addHandler(new Autocomplete2());

//-----------------------------------------------------------------------------------
// Generates the Javascript client by adding ?client to the server URL
//-----------------------------------------------------------------------------------
if (isset($_SERVER['QUERY_STRING']) && strcasecmp($_SERVER['QUERY_STRING'], 'client')==0) {
    // Compress the Javascript
    // define('JPSPAN_INCLUDE_COMPRESS',TRUE);
    $S->displayClient();
    
//-----------------------------------------------------------------------------------
} else {
    
    // Include error handler - PHP errors, warnings and notices serialized to JS
    require_once JPSPAN . 'ErrorHandler.php';
    $S->serve();

}

#?>
