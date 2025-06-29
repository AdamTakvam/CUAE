<?php

require_once("lib.Utils.php");

interface FilterType
{
    
    public function SetName($name);
    public function SetValue($value);
    public function RenderUI();
    public function GetForSql();

}


class FilterString implements FilterType
{
    
    private $mValue = NULL;
    private $mName = NULL;
    
    public function SetName($name)
    {
        $this->mName = $name;
    }
    
    public function SetValue($value)
    {
        if (!empty($value))
            $this->mValue = $value;
        return TRUE;
    }
    
    public function RenderUI()
    {
        return '<input type="text" name="__filter__['.$this->mName.']" value="'.htmlspecialchars($this->mValue).'" />';
    }
    
    public function GetForSql()
    {
        if (!empty($this->mValue))
            return '"'.$this->mValue.'"';
        else
            return NULL;
    }
    
}


class FilterNumber implements FilterType
{
    
    private $mValue = NULL;
    private $mName = NULL;
    
    public function SetName($name)
    {
        $this->mName = $name;
    }
    
    public function SetValue($value)
    {
        if (is_numeric($value))
        {
            $this->mValue = $value;
            return TRUE;
        }
        if (empty($value))
            return TRUE;
        return FALSE;
    }
    
    public function RenderUI()
    {
        $out  = '<input type="text" name="__filter__['.$this->mName.']" value="'.$this->mValue.'" />';
        return $out;
    }
    
    public function GetForSql()
    {
        return $this->mValue;
    }
    
}


class FilterIP implements FilterType
{
    
    private $mValue = NULL;
    private $mName = NULL;
    
    public function SetName($name)
    {
        $this->mName = $name;
    }
    
    public function SetValue($value)
    {
        if (empty($value))
            return TRUE;
        else if (Utils::validate_ip($value))
        {    
            $this->mValue = $value;
            return TRUE;
        }
        return FALSE;
    }
    
    public function RenderUI()
    {
        return '<input type="text" name="__filter__['.$this->mName.']" value="'.$this->mValue.'" />';
    }
    
    public function GetForSql()
    {
        if (!empty($this->mValue))
            return '"'.$this->mValue.'"';
        else
            return NULL;
    }
    
}


class FilterEnumeration implements FilterType
{
    
    private $mValue = NULL;
    private $mChoiceSet = array();
    private $mName = NULL;
    
    public function SetName($name)
    {
        $this->mName = $name;
    }
    
    public function SetValue($value)
    {
        if (!empty($value))
            $this->mValue = $value;
        return TRUE;
    }
    
    public function SetEnumeration($data)
    {
        if (is_array($data))
        {
            $this->mChoiceSet = $data;
            return TRUE;
        }
        return FALSE;
    }
    
    public function RenderUI()
    {
        $out  = '<select name="__filter__['.$this->mName.']">';
        $out .= '<option value="">All</option>';
        foreach ($this->mChoiceSet as $val => $name)
        {
            if ($val == $this->mValue)
                $selected = ' selected="selected"';
            else
                $selected = '';
            $out .= '<option value="'.$val.'"'.$selected.'>'.htmlspecialchars($name).'</option>';
        }
        $out .= '</select>';
        return $out;
    }
    
    public function GetForSql()
    {
        if (!empty($this->mValue))
            return '"'.$this->mValue.'"';
        else
            return NULL;
    }
    
}


class FilterTimestamp implements FilterType
{
    
    private $mValue = NULL;
    private $mName = NULL;
    private $mTimeZone = 0;
    
    public function __construct($timeZone = 'SYSTEM')
    {
        $this->mTimeZone = $timeZone;
    }
    
    public function SetName($name)
    {
        $this->mName = $name;
    }
    
    public function SetValue($value)
    {
        if (is_array($value))
        {   
            if ($value['Meridian'] == 'pm')
                $value['Hour'] += 12;
            
            if ($value['Month'] && $value['Day'] && $value['Year'])
                $this->mValue = mktime($value['Hour'], $value['Minute'], 0, $value['Month'], $value['Day'], $value['Year']);
            return TRUE;
        }
        return FALSE;
    }
    
    public function RenderUI()
    {   
        $out  = '{html_select_date prefix="" time="'.$this->mValue.'" year_as_text=true field_array="__filter__['.$this->mName.']" month_empty="" day_empty="" year_empty=""}';
        $out .= ' at ';
        $out .= '{html_select_time display_seconds=false prefix="" time="'.$this->mValue.'" minute_interval=15 use_24_hours=false field_array="__filter__['.$this->mName.']"}';
        return $out;
    }
    
    public function GetForSql()
    {
        if (!is_null($this->mValue))
        {
            return "CONVERT_TZ(FROM_UNIXTIME(".$this->mValue."),'".$this->mTimeZone."','SYSTEM')";
        }
        else
            return NULL;
    }
    
}

class FilterMinutes implements FilterType
{
    
    private $mValue = NULL;
    private $mName = NULL;
    
    public function SetName($name)
    {
        $this->mName = $name;
    }
        
    public function SetValue($value)
    {
        if (is_numeric($value))
        {
            $this->mValue = $value;
            return TRUE;
        }
        if (empty($value))
            return TRUE;
        return FALSE;
    }
    
    public function RenderUI()
    {
        $out  = '<input type="text" name="__filter__['.$this->mName.']" value="'.$this->mValue.'" /> minute(s)';
        return $out;
    }
    
    public function GetForSql()
    {
        if (!is_null($this->mValue))
            return intval($this->mValue) * 60;
        else
            return NULL;
    }
    
}


?>