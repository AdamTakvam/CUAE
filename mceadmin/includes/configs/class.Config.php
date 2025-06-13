<?php

abstract class Config
{

    protected $mDb;
    protected $mId;
    protected $mData;
    protected $mValues;
    protected $mError;
    protected $mContext;

    function __construct()
    {
        $this->mDb = new MceDb();
        $this->mId = NULL;
        $this->mData = array();
        $this->mValues = NULL;
        $this->mError = NULL;
        $this->mContext = array();
    }

    function SetId($id)
    {
        if (NULL == $this->mId)
        {
            $this->mId = $id;
            return TRUE;
        }
        return FALSE;
    }

    function BuildWithData($data)
    {
        if (empty($this->mData) && empty($this->mValues))
        {
            $this->mId = $data["mce_config_entries_id"];
            $this->mData = $data;
            return TRUE;
        }
        return FALSE;
    }

    function AddContextParam($name, $value)
    {
        $this->mContext[$name] = $value;
        return TRUE;
    }

    function GetData($key = NULL)
    {
        if (NULL == $key)
        {
            return $this->mData;
        }
        else
        {
            if (isset($this->mData[$key]))
            {
                return $this->mData[$key];
            }
        }
        return FALSE;
    }

    function GetError()
    {
        return $this->mError;
    }


//    Abstract methods cannot be also be implemented in PHP5.  It would be nice
//    if we could. Then, I can force anyone inheriting this class to conciously
//    implement certain methods (even if it means explicitly calling the default).
//    So, you should conciously decide how to implement the methods below in
//    your child class.

    function FetchValues()
    {
        if (NULL != $this->mId && NULL == $this->mValues)
        {
            $this->mValues = $this->mDb->GetOne("SELECT value FROM mce_config_values WHERE mce_config_entries_id = ?", array($this->mId));
        }
        return $this->mValues;
    }

    abstract function ValidateValues($values);

    abstract function IsSameValues($values);

    function AssembleValues($values) { return htmlspecialchars($values); }

    function InsertValues($values)
    {
    	$values = trim($values);
    	if (!$this->mData['required'] && (strlen($values) == 0))
    		return TRUE;
    		
        $this->mDb->Execute("INSERT INTO mce_config_values (mce_config_entries_id, value) VALUES (?, ?)",
                            array($this->mId, $values));
        $this->mValues = $values;
        return TRUE;
    }

    function UpdateValues($values)
    {
    	$values = trim($values);
    	if (!$this->mData['required'] && (strlen($values) == 0))
    	{
    		$this->mDb->Execute("DELETE fROM mce_config_values WHERE mce_config_entries_id = ?", array($this->mId));
    		$this->mValues = NULL;
    		return TRUE;
    	}
        if (!$this->mData["read_only"])
        {
            $this->mDb->Execute("UPDATE mce_config_values SET value = ? WHERE mce_config_entries_id = ?",
                                array($values, $this->mId));
            if ($this->mDb->Affected_Rows() == 0)
            	return $this->InsertValues($values);
            $this->mValues = $values;
        }
        return TRUE;
    }

    function GetFields($values = NULL)
    {
        if (NULL == $values) { $values = $this->mValues; }
        if (0 == $this->mData["read_only"])
        {
            $field = '<input type="text" name="' . $this->mData["name"] . '" value="' . htmlspecialchars($values) . '" />';
        }
        else
        {
            $field = htmlspecialchars($values);
        }
        return $field;
    }

}

?>