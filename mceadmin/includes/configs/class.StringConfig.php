<?php

require_once("class.Config.php");

class StringConfig extends Config {

    function FetchValues()
    {
        return parent::FetchValues();
    }

    function IsSameValues($value)
    {
        return (trim($value) == trim($this->mValues));
    }

    function ValidateValues($value)
    {
    	$value = trim($value);
    	if (!$this->mData['required'] && (strlen($value) == 0))
    		return TRUE;
    	else
        	return (strlen($value) > 0);
    }

    function AssembleValues($value)
    {
        return parent::AssembleValues($value);
    }

    function InsertValues($value)
    {
        return parent::InsertValues($value);
    }

    function UpdateValues($value)
    {
        return parent::UpdateValues($value);
    }

    function GetFields($values = NULL)
    {
        return parent::GetFields($values);
    }

}

?>