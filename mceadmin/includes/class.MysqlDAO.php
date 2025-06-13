<?php
//
//  class.MysqlDAO.php
//
//  Database functions against mysql database
//
function _quote($msg) {
    if (is_array($msg)) {
        return "(" . implode(",",array_map("_quote",$msg)) . ")";
    } else {
        return "'" . mysql_real_escape_string($msg) . "'";
    }
}

class MysqlDAO {
    var $conn;
    var $hostname;
    var $database;
    var $username;
    var $password;
    
    function MysqlDAO($hostname, $database, $username, $password) {
        $this->hostname = $hostname;
        $this->database = $database;
        $this->username = $username;
        $this->password = $password;
    }
    
    function _connect() {
        $this->conn = mysql_connect($this->hostname, $this->username, $this->password);
        if ($this->database != '') {
            mysql_select_db($this->database, $this->conn);
        }    
    }
    
    function doSQL() {
        $this->_connect();
        
        $args = func_get_args();
        
        $n    = array_merge(array($args[0],), array_map("_quote", array_slice($args,1)));        
        $q    = call_user_func_array("sprintf", $n);
        $rs   = mysql_query($q, $this->conn);
        return $rs;
    }
        
    function doQuery() {
        $this->_connect();

        $args = func_get_args();
        
        $n    = array_merge(array($args[0],), array_map("_quote", array_slice($args,1)));        
        $q    = call_user_func_array("sprintf", $n);
        $rs   = mysql_query($q, $this->conn);
        $rval = array();
        while ($r = mysql_fetch_array($rs, MYSQL_BOTH )) {
            array_push($rval, $r);
        }
        mysql_free_result($rs);
        return $rval;
    }
    
}

?>
