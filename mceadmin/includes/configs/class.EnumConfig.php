<?php

require_once("class.Config.php");

class EnumConfig extends Config
{

    private $mEnumValues;

    function __construct()
    {
        parent::__construct();
        $this->mEnumValues = array();
    }

    function BuildWithData($data)
    {
        $this->mEnumValues = $this->mDb->GetCol("SELECT value FROM mce_format_type_enum_values WHERE mce_format_types_id = ? ORDER BY mce_format_type_enum_values_id ASC",
                                                array($data["mce_format_types_id"]));
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
        foreach ($this->mEnumValues as $enum_value)
        {
            if ($enum_value == $value)
            {
                return TRUE;
            }
        }
        return FALSE;
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
        if (0 == $this->mData["read_only"] || empty($this->mValues))
        {
            $field = "<select name=\"" . $this->mData["name"] . "\">";
            foreach ($this->mEnumValues as $x)
            {
                if ($x == $values)
                {
                    $selected = "selected=\"selected\"";
                }
                else
                {
                    $selected = "";
                }
                $field .= "<option value=\"" . htmlspecialchars($x) . "\" $selected>" . htmlspecialchars($x) . "</option>";
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