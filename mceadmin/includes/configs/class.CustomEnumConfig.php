<?php

require_once("class.Config.php");

class CustomEnumConfig extends Config
{

    private $mEnumValues;

    function __construct()
    {
        parent::__construct();
        $this->mEnumValues = array();
    }

	function SetEnum($array)
	{
		if (is_array($array))
			$this->mEnumValues = $array;
		else
			return FALSE;
	}

    function BuildWithData($data)
    {
        return parent::BuildWithData($data);
    }

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
    	$value = trim($value);
    	if ($this->mData['required'] && (strlen($value) == 0))
    		return TRUE;
        foreach ($this->mEnumValues as $enum_value)
        {
            if ($enum_value == $value)
                return TRUE;
        }
        return FALSE;
    }

    function AssembleValues($value)
    {
        return htmlspecialchars(array_search($value, $this->mEnumValues));
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
        if (NULL == $values) { $values = $this->mValues; }
        if (0 == $this->mData["read_only"])
        {
            $field = "<select name=\"" . $this->mData["name"] . "\">";
            foreach ($this->mEnumValues as $key => $val)
            {
                if ($val == $values)
                {
                    $selected = "selected=\"selected\"";
                }
                else
                {
                    $selected = "";
                }
                $field .= "<option value=\"" . htmlspecialchars($val) . "\" $selected>" . htmlspecialchars($key) . "</option>";
            }
            $field .= "</select>";
        }
        else
        {
            $field = htmlspecialchars($values);
        }
        return $field;
    }
}

?>