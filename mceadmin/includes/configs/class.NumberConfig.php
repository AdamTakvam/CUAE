<?php

require_once("class.Config.php");

class NumberConfig extends Config
{

    function FetchValues()
    {
        return parent::FetchValues();
    }

    function IsSameValues($value)
    {
    	if ($this->mData['required'] || (strlen($value) > 0 && strlen($this->mValues) > 0))    	
        	return (floatval($value) == floatval($this->mValues));
        else
        	return ($value == $this->mValues);       
    }

    function ValidateValues($value)
    {
    	if (!$this->mData['required'] && (strlen($value) == 0))
    		return TRUE;
        if (!is_numeric($value))
        {
            $this->mError = "Value is not a number.";
            return FALSE;
        }
        if (isset($this->mData["min_value"]))
        {
            if (floatval($this->mData["min_value"]) > floatval($value))
            {
                $this->mError = "Value needs to be within the range of " . $this->mData["min_value"] . " to " . $this->mData["max_value"] . ".";
                return FALSE;
            }
        }
        if (isset($this->mData["max_value"]))
        {
            if (floatval($this->mData["max_value"]) < floatval($value))
            {
                $this->mError = "Value needs to be within the range of " . $this->mData["min_value"] . " to " . $this->mData["max_value"] . ".";
                return FALSE;
            }
        }
        return TRUE;
    }
    
    function AssembleValues($value) 
    { 
    	return parent::AssembleValues($value); 
    }

    function InsertValues($value)
    {
        return parent::InsertValues(floatval($value));
    }

    function UpdateValues($value)
    {
    	if ($this->mData['required'] || strlen($value) > 0)
    		$value = floatval($value);    	
        return parent::UpdateValues($value);
    }

    function GetFields($values = NULL)
    {
        return parent::GetFields($values);
    }

}

?>