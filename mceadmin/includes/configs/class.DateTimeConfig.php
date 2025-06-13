<?php

require_once("class.Config.php");
require_once("lib.Utils.php");

class DateTimeConfig extends Config {

    function FetchValues()
    {
        parent::FetchValues();
        list($date, $time) = explode(' ', $this->mValues, 2);
        list($year, $month, $day) = explode('-', $date, 3);
        list($hour, $minute, $second) = explode(':', $time, 3);
        $values = array(
                    "year"  => $year,
                    "month" => $month,
                    "day"  => $day,
                    "hour"  => $hour,
                    "minute" => $minute,
                    "second" => $second,
                  );
        $this->mValues = $values;
        return $this->mValues;
    }

    function ValidateValues($values)
    {
        if (!checkdate($values['month'], $values['day'], $values['year']))
        {
            return FALSE;
        }

        if ($values['hour'] < 0 || $values['hour'] > 23)
        {
            return FALSE;
        }

        if ($values['minute'] < 0 || $values['minute'] > 59)
        {
            return FALSE;
        }

        if ($values['second'] < 0 || $values['second'] > 59)
        {
            return FALSE;
        }

        return TRUE;
    }

    function IsSameValues($value)
    {
        return ($values == $this->mValues);
    }

    function AssembleValues($values)
    {
        $date = implode('-', array($values['year'], $values['month'], $values['day']));
        $date .= " " . implode(':', array($values['hour'], $values['minute'], $values['second']));
        return $date;
    }

    function InsertValues($values)
    {
        $date = $this->AssembleValues($values);
        $this->mDb->Execute("INSERT INTO mce_config_values (mce_config_entries_id, value) VALUES (?, ?)",
                            array($this->mId, $date));
        $this->mValues = $values;
        return TRUE;
    }

    function UpdateValues($values)
    {
        $date = $this->AssembleValues($values);
        if (0 == $this->mData["read_only"])
        {
            $this->mDb->Execute("UPDATE mce_config_values SET value = ? WHERE mce_config_entries_id = ?",
                                array($date, $this->mId));
             if ($this->mDb->Affected_Rows() == 0)
            	return $this->InsertValues($values);
            $this->mValues = $values;
        }
        return TRUE;
    }

    function GetFields($values = NULL)
    {
        if (NULL == $values) { $values = $this->mValues; }
        if ($this->mData["read_only"] == 0)
        {
            $month = '<select name="' . $this->mData['name'] . '[month]">';
            for ($i = 1; $i < 13; ++$i)
            {
                if ($i == $values['month'])
                {
                    $select = 'selected="selected"';
                }
                else
                {
                    $select = "";
                }
                $month .= '<option value="' . $i . '" ' . $select . '>' . Utils::get_month_name($i) . '</option>';
            }
            $month .= '</select>';

            $day = '<input type="text" name="' . $this->mData['name'] . '[day]" value="' . $values['day'] . '" size="2" maxlength="2" />';
            $year = '<input type="text" name="' . $this->mData['name'] . '[year]" value="' . $values['year'] . '" size="4" maxlength="4" />';
            $hour = '<input type="text" name="' . $this->mData['name'] . '[hour]" value="' . $values['hour'] . '" size="2" maxlength="2" />';
            $minute = '<input type="text" name="' . $this->mData['name'] . '[minute]" value="' . $values['minute'] . '" size="2" maxlength="2" />';
            $second = '<input type="text" name="' . $this->mData['name'] . '[second]" value="' . $values['second'] . '" size="2" maxlength="2" />';
            return $month . ' ' . $day . ', ' . $year . ' ' . implode(":",array($hour,$minute,$second));
        }
        else
        {
            return Utils::get_month_name($values['month']) . ' ' . $values['day'] . ', ' . $values['year']
                   . implode(":", array($values['hour'], $values['minute'], $values['second']));
        }
    }

}

?>