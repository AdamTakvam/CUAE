<?php

require_once("class.Config.php");


// The logical table data is stored as such:
//     1. Each table "cell" is stored as a unique entry in the values table,
//        associated with a key and a ordinal_row value.
//     2. ordinal_row values indicate the row of the logical table cell
//     3. Key values indicate the column of the logical table cell
// I.E. a mce_config_value entry that returns as an array of:
//     array(
//         "mce_config_values_id" =>   "123",
//         "mce_config_entries_id" =>  "42",
//         "ordinal_row" =>            "3",
//         "key_column" =>             "5",
//         "value" =>                  "FooBar"
//     )
// is unique entry no. 123 in the database table, and holds the data for the cell
// in the 5th column of the 3rd row of the logical table .  The logical table is
// for the configuration entry with the id number "42".  The value of the cell is
// "FooBar".


class TableConfig extends Config
{

    private $mRows;
    private $mCols;

    function FetchValues()
    {
        // Get size of table
        $rs = $this->mDb->Execute("SELECT MAX(ordinal_row) AS max_row, MAX(key_column) AS max_column FROM mce_config_values WHERE mce_config_entries_id = ?", array($this->mId));
        $this->mRows = intval($rs->fields["max_row"]);
        $this->mCols = intval($rs->fields["max_column"]);

        // Construct Table
        $raw = $this->mDb->GetAll("SELECT * FROM mce_config_values WHERE mce_config_entries_id = ? ORDER BY ordinal_row ASC, key_column ASC", array($this->mId));
        $i = 0;
        for ($x = 0 ; $x <= $this->mRows; ++$x)
        {
            for ($y = 0; $y <= $this->mCols; ++$y)
            {
                $this->mValues[$x][$y] = $raw[$i]["value"];
                ++$i;
            }
        }
        return $this->mValues;
    }

    function IsSameValues($values)
    {
    	if (sizeof($values) != sizeof($this->mValues))
    		return FALSE;
    	else if (sizeof($values[0]) != sizeof($this->mValues[0]))
    		return FALSE;
    	
        $rows = sizeof($values);
        $cols = sizeof($values[0]);
        for ($x = 0; $x <= $rows; ++$x)
        {
            for ($y = 0; $y <= $cols; ++$y)
            {
                if ($values[$x][$y] != $this->mValues[$x][$y])
                {
                    return FALSE;
                }
            }
        }
        return TRUE;
    }

    function AssembleValues($values)
    {
        return parent::AssembleValues($values);
    }

    function ValidateValues($values)
    {
        // Validation is done on the page specifically made to edit the table
        return TRUE;
    }

    function InsertValues($values)
    {
        if (empty($this->mData["read_only"]))
        {
            $rows = sizeof($values);
            $cols = sizeof($values[0]);
            for ($x = 0; $x < $rows; ++$x)
            {
                for ($y = 0; $y < $cols; ++$y)
                {
                    $this->mDb->Execute("INSERT INTO mce_config_values (mce_config_entries_id, ordinal_row, key_column, value) VALUES (?,?,?,?)",
                                        array($this->mId, $x, $y, $values[$x][$y]));
                }
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

        $url = "config_edit_table.php?id=$this->mId&amp;part_id=$part_id";

        $button = "<input type=\"button\" value=\"$text\" onclick=\"var win = window.open('$url','config','width=600,height=400,resizable=yes,scrollbars=yes'); win.focus();\" $disabled />";
        return $button;
    }

}

?>