<?php

require_once("class.Config.php");

class HashConfig extends Config
{

    function FetchValues()
    {
        $this->mValues = $this->mDb->GetAll("SELECT * FROM mce_config_values WHERE mce_config_entries_id = ? ORDER BY ordinal_row ASC",
                                            array($this->mId));
        return $this->mValues;
    }

    function IsSameValues($values)
    {
    	if (sizeof($values) != sizeof($this->mValues))
    		return FALSE;
        for ($i = 0; $i < sizeof($values); ++$i)
        {
            if (($values[$i]["key_column"] != $this->mValues[$i]["key_column"]) ||
                ($values[$i]["value"] != $this->mValues[$i]["value"]))
            {
                return FALSE;
            }
        }
        return TRUE;
    }

    function ValidateValues($values)
    {
        // Validation is done on the page specifically made to edit the hash table
        return TRUE;
    }

    function AssembleValues($values)
    {
        return parent::AssembleValues($values);
    }

    function InsertValues($values)
    {
        if (empty($this->mData["read_only"]))
        {
            for ($i = 0; $i < sizeof($values); ++$i)
            {
                $this->mDb->Execute("INSERT INTO mce_config_values (mce_config_entries_id, ordinal_row, key_column, value) VALUES (?, ?, ?, ?)",
                                    array($this->mId, $i, $values[$i]["key_column"], $values[$i]["value"]));
            }
            $this->mValues = $values;
        }
        return TRUE;
    }

    function UpdateValues($values)
    {
        if (empty($this->mData["read_only"]))
        {
            $this->mDb->Execute("DELETE FROM mce_config_values WHERE mce_config_entries_id = ?", array($this->mId));
            return $this->InsertValues($values);
        }
        return TRUE;
    }

    function GetFields($values = NULL)
    {
        $can_edit = TRUE;

        if (empty($this->mId))
        {
            $can_edit = FALSE;
        }
        else if (isset($this->mContext['partition_id']))
        {
            if (!empty($this->mContext['partition_id']))
                $part_id = $this->mContext['partition_id'];
            else
                $can_edit = FALSE;
        }

        if (!$can_edit)
        {
            $disabled = 'disabled="disabled"';
            $text = "Edit After Creation";
        }
        else if ($this->mInfo['read_only'])
            $text = "View Values";
        else
            $text = "View and Edit Values";

        $url = "config_edit_hash.php?id=$this->mId&amp;part_id=$part_id";

        $button = "<input type=\"button\" value=\"$text\" onclick=\"var win = window.open('$url','config','width=600,height=400,resizable=yes,scrollbars=yes'); win.focus();\" $disabled />";
        return $button;
    }

}

?>