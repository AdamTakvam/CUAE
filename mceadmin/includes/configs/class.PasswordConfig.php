<?php

require_once("class.StringConfig.php");

class PasswordConfig extends StringConfig
{

    function ValidateValues($values)
    {
        if (is_array($values))
        {
            if  (   $this->mData['required']
                &&  (strlen($values['password']) == 0)
                &&  (strlen($values['password_verify']) == 0)
                )
            {
                return FALSE;
            }
            
            if ($values['password'] != $values['password_verify'])
            {
                $this->mError .= "The new password could not be verified.";
                return FALSE;
            }
        }
        return TRUE;
    }

    function InsertValues($values)
    {
        if (is_array($values))
            parent::InsertValues($values['password']);
        else
        	parent::InsertValues($values);
        return TRUE;
    }

    function UpdateValues($values)
    {
        if (is_array($values))
            parent::UpdateValues($values['password']);
        else
        	parent::UpdateValues($values);
        return TRUE;
    }

    function GetFields($values = NULL)
    {
        $can_edit = TRUE;

        if (isset($this->mContext['partition_id']))
        {
            if (!empty($this->mContext['partition_id']))
                $part_id = $this->mContext['partition_id'];
            else
                $can_edit = FALSE;
        }

        if ($this->mData["read_only"])
        {
            $text = 'Read Only';
            $disabled = 'disabled="disabled"';
        }
        else if (!$can_edit)
        {
            $text = 'Change Password After Creation';
            $disabled = 'disabled="disabled"';
        }
        else
        {
            $text = 'Change Password';
        }

        if (empty($this->mId))
        {
            $field = "<input type=\"password\" name=\"" . $this->mData[name]. "[password]\" value=\"$values\"/>" .
            		 "<br />Enter again to verify:<br />" .
            		 "<input type=\"password\" name=\"" . $this->mData[name]. "[password_verify]\" value=\"$values\"/>";
            if (!empty($values))
                $field = "<em>Default value: $values</em><br />" . $field;
            return $field;
        }
        else
        {
            $url = "config_edit_password.php?id=$this->mId&amp;part_id=$part_id";
            $button = "<input type=\"button\" value=\"$text\" onclick=\"var win = window.open('$url','config','width=600,height=400,resizable=yes,scrollbars=yes'); win.focus();\" $disabled />";
            return $button;
        }

    }

}

?>