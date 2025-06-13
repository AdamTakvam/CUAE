<?php

require_once("lib.Utils.php");

abstract class RegistryUtils
{

    static public function read_registry_value($keyname, $valuename)
    {
        try
        {
            $out = Utils::execute("REG QUERY \"$keyname\" /v \"$valuename\"");
            if (ereg($valuename . '[[:space:]]+REG_[A-Z]+[[:space:]]+(.+)', $out, $regs))
                return trim($regs[1]);
        } 
        catch (Exception $e)
        {
            throw new Exception("Could not find registry value at $keyname\\$valuename");
        }
        return NULL;
    }
    
    static public function set_registry_value($keyname, $valuename, $data)
    {
        try
        {
            Utils::execute("REG ADD \"$keyname\" /v \"$valuename\" /d \"$data\" /f");
            return TRUE;
        }
        catch (Exception $e)
        {
            return FALSE;
        }
    }

    static public function delete_registry_value($keyname, $valuename)
    {
        try
        {
            Utils::execute("REG DELETE \"$keyname\" /v \"$valuename\" /f");
            return TRUE;
        }
        catch (Exception $e)
        {
            return FALSE;
        }
    }
    
    static public function import_file($filename)
    {
        try
        {
            Utils::execute("REG IMPORT \"$filename\"");
            return TRUE;
        }
        catch (Exception $e)
        {
            return FALSE;
        }
    }
    
}

?>