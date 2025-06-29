<?php
// Just a utility to help the example work out where the server URL is...
function path() {
    if ( isset($_SERVER['PATH_INFO']) && $_SERVER['PATH_INFO'] == $_SERVER['PHP_SELF'] ) {
        $script_name = $_SERVER['PATH_INFO'];
    } else {
        $script_name = $_SERVER['SCRIPT_NAME'];
    }
    $basePath = explode('/',$script_name);
    $script = array_pop($basePath);
    $basePath = implode('/',$basePath);
    if ( isset($_SERVER['HTTPS']) ) {
        $scheme = 'https';
    } else {
        $scheme = 'http';
    }
    echo $scheme.'://'.$_SERVER['SERVER_NAME'].$basePath;
}