<?php

require_once("class.Config.php");

class BooleanConfig extends Config
{

    function FetchValues()
    {
        return parent::FetchValues();
    }

    function IsSameValues($value)
    {
        return ( 0 == strcasecmp($value,$this->mValues) );
    }

    function ValidateValues($value)
    {
        return ($value == "True" || $value == "False");
    }

    function AssembleValues($value)
    {
        return parent::AssembleValues();
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
                if ( 0 == strcasecmp($values,'True'))
                {
                    $no = "";
                    $yes = "checked=\"checked\"";
                }
                else
                {
                    $no = "checked=\"checked\"";
                    $yes = "";
                }
                $field  = "<input type=\"radio\" name=\"" . $this->mData["name"]  . "\" value=\"True\" $yes /> Yes ";
                $field .= "<input type=\"radio\" name=\"" . $this->mData["name"]  . "\" value=\"False\" $no /> No ";
        }
        else
        {
            $field = (0 == strcasecmp($values,'True')) ? "Yes" : "No";
        }
        return $field;
    }

}

?>