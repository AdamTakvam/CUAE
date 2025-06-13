<?php

require_once("class.Config.php");

class IpAddressConfig extends Config
{
    function FetchValues()
    {
        return parent::FetchValues();
    }

    function IsSameValues($value)
    {
        return ($value == $this->mValues);
    }

    function ValidateValues($value)
    {
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
        if (NULL == $values)
            $values = $this->mValues;
        
        // If the field is read-only but has no value, it more than likely means that some kind of 
        // value is desired.  Give the user an opportunity to fill it in.
        if (0 == $this->mData["read_only"] || empty($this->mValues))
            $field = '<input type="text" name="' . $this->mData['name'] . '" value="' . $values . '" />';
        else
            $field = $values;

        return $field;
    }

}

?>