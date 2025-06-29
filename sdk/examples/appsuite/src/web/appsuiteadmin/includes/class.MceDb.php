<?php

require_once("common.php");


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


class MceDb
{

    private $mAdo;

    public function __construct()
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

    public function __call($method, $args)
    {
        return call_user_func_array(array(&$this->mAdo,$method),$args);
    }

    public function Execute($sql, $inputarr = FALSE)
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

    public function GetOne($sql, $inputarr = FALSE)
    {
        $rs = $this->Execute($sql, $inputarr);
        $foo = $rs->FetchRow();
        if (is_array($foo))
        {
            return current($foo);
        }
        else
        {
            return NULL;
        }
    }

    public function GetCol($sql, $inputarr = FALSE)
    {
        $rs = $this->Execute($sql, $inputarr);
        if ($rs) {
            while ($foo = $rs->FetchRow())
            {
                $bar[] = current($foo);
            }
        }
        if (!isset($bar)) $bar = array();
        return $bar;
    }

    public function GetRow($sql, $inputarr = FALSE)
    {
        $rs = $this->Execute($sql, $inputarr);
        if ($rs)
        {
            return $rs->FetchRow();
        }
        else
        {
            return NULL;
        }
    }

    public function GetAll($sql, $inputarr = FALSE)
    {
        $rs = $this->Execute($sql, $inputarr);
        if ($rs)
        {
            return $rs->GetRows();
        }
        else
        {
            return NULL;
        }
    }    
    
    public function MakeAndExecuteInsert($table, $name_values)
    {
        foreach ($name_values as $name => $value)
        {
            $names[] = $name;
            $values[] = $value;
            $placeholders[] = '?';    
        }
        $query = "INSERT INTO $table (" . implode(',',$names) . ") VALUES (" . implode(',',$placeholders) . ")";
        return $this->Execute($query, $values);
    }

    public function MakeAndExecuteUpdate($table, $where_condition, $name_values)
    {
        foreach ($name_values as $name => $value)
        {
            $queryparts[] = "$name = ?";
            $values[] = $value;
        } 
        $query = "UPDATE $table SET " . implode(',',$queryparts) . " WHERE $where_condition";
        return $this->Execute($query, $values);
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
    
    public function Close()
    {
        return DbResource::close();
    }
    
}

?>