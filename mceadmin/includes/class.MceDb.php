<?php

/* The MceDB object acts a lot like an ADODB database abstracation object,
 * except that the connection is made simply by invoking the object and some
 * extra, convenient methods have been addded.
 */

// ADODB doesn't like being constructed explicitly in the client code, so
// merely inheriting it won't do the trick.  Fortunately, PHP5 has a cute
// mechanism for dealing with calling methods that don't actually exist in
// the object.  So we can pass those calls to the contained ADOConnection
// object - so it acts like a child of that class!

// So, now you can create a connection to the database like you would create
// an object.  Then, you can treat the object like you would an ADOConnection
// object (see http://phplens.com/adodb/) with built in checking and debugging
// actions.  I've also overloaded some of the functions to take advantage of
// variable binding.

require_once("common.php");

class MceDb
{

/* PRIVATE MEMBERS */

    private $mAdo;      // Native ADODB object


/* PUBLIC METHODS */

    function __construct()
    // Initialize the object.  If there is trouble connecting to the database,
    // use a db connect exception handler.
    {
        try
        {
            $this->mAdo = DbResource::retrieve();
        }
        catch (Exception $e)
        {
            set_exception_handler("_mce_db_connect_exception_handler");
            throw $e;
        }
    }

    function __call($method, $args)
    // This magical function allows any method calls on the obect to be passed to the
    // ADODB object if it does not exist for this object.
    {
        return call_user_func_array(array(&$this->mAdo,$method),$args);
    }

    function Execute($sql, $inputarr = FALSE)
    // A version of ADODB's Execute with convenient debugging information
    {
        $rs = $this->mAdo->Execute($sql, $inputarr);
        if (!$rs->EOF && MceConfig::SHOW_SQL_RS)
        {
            rs2html($rs);
            // Reset the result set iterator
            $rs->Move(0);
        }
        return $rs;
    }

    function GetOne($sql, $inputarr = FALSE)
    // A variable binding version of ADODB's GetOne()
    {
        $rs = $this->Execute($sql, $inputarr);
        $foo = $rs->FetchRow();
        if (is_array($foo))
            return current($foo);
        else
            return NULL;
    }

    function GetCol($sql, $inputarr = FALSE)
    // A variable binding version of ADODB's GetCol()
    {
        $rs = $this->Execute($sql, $inputarr);
        if ($rs) 
        {
            while ($foo = $rs->FetchRow())
                $bar[] = current($foo);
        }
        if (!$bar) 
            $bar = array();
        return $bar;
    }

    function GetRow($sql, $inputarr = FALSE)
    // A variable binding version of ADODB's GetRow()
    {
        $rs = $this->Execute($sql, $inputarr);
        if ($rs)
            return $rs->FetchRow();
        else
            return NULL;
    }

    function GetAll($sql, $inputarr = FALSE)
    // A variable binding version of ADODB's GetAll()
    {
        $rs = $this->Execute($sql, $inputarr);
        if ($rs)
            return $rs->GetRows();
        else
            return NULL;
    }

    public function SetSessionTimeZone($timezone)
    {
        if (!empty($timezone))
            return $this->Execute("SET time_zone = ?", $timezone);
        else
            return FALSE;
    }
    
    public function ResetSessionTimeZone()
    {
        return $this->SetSessionTimeZone('SYSTEM');
    }
    
    public function MakeAndExecuteInsert($table, $name_values)
    {
        $this->ValidateName($table);
        foreach ($name_values as $name => $value)
        {
            $this->ValidateName($name);
            $names[] = "`" . $name . "`";
            $values[] = $value;
            $placeholders[] = '?';    
        }
        $query = "INSERT INTO `$table` (" . implode(',',$names) . ") VALUES (" . implode(',', array_fill(0,count($values),'?')) . ")";
        return $this->Execute($query, $values);
    }

    public function MakeAndExecuteUpdate($table, $where_condition, $name_values)
    {
        $this->ValidateName($table);
        foreach ($name_values as $name => $value)
        {
            $this->ValidateName($name);
            $queryparts[] = "`" . $name . "` = ?";
            $values[] = $value;
        } 
        $query = "UPDATE `$table` SET " . implode(',',$queryparts) . " WHERE $where_condition";
        return $this->Execute($query, $values);
    }
    
    public function Close()
    {
        return DbResource::close();
    }
    
    
/* PRIVATE METHODS */
        
    private function ValidateName($name)
    // Kind of a hacky way to prevent SQL hijacking...on the rare occasion the user
    // has the ability to define this value (usually, they don't)
    {
        if (strpos($name,"`") !== FALSE)
        {
            throw new Exception("Invalid syntax for a database table or column name: $name");
        }
    }
    
}

?>