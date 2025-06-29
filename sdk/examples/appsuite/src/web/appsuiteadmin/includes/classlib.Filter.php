<?php

require_once("classlib.FilterType.php");

interface Filter
{

    public function __construct($name, FilterType $type);
    public function SetValue($value);
    public function SetSqlColDef($colDef);
    public function RenderUI();
    public function GetSqlCondition();
    public function GetName();

}

class ExactFilter implements Filter
{

    private $mName = NULL;
    private $mFilterType = NULL;
    private $mColDef = NULL;
    
    public function __construct($name, FilterType $type)
    {
        $this->mColDef = $this->mName = $name;
        $this->mFilterType = $type;
        $this->mFilterType->SetName($name);
    }

    public function SetValue($value)
    {
        return $this->mFilterType->SetValue($value);
    }
    
    public function SetSqlColDef($colDef)
    {
        $this->mColDef = $colDef;
    }

    public function RenderUI()
    {
        return $this->mFilterType->RenderUI();
    }

    public function GetSqlCondition()
    {
        $value = $this->mFilterType->GetForSql();
        if (!is_null($value))
            return $this->mColDef.' = '.$value;
        else
            return NULL;
    }

    public function GetName()
    {
        return $this->mName;
    }
    
}


class SearchFilter implements Filter
{

    private $mOp = NULL;
    private $mName = NULL;
    private $mFilterType = NULL;
    private $mColDef = NULL;

    public function __construct($name, FilterType $type)
    {
        $this->mColDef = $this->mName = $name;
        $this->mFilterType = $type;
        $this->mFilterType->SetName("$name][search_value");
    }

    public function SetValue($value)
    {
        if (is_array($value))
        {
            $this->mOp = $value['search_op'];
            return $this->mFilterType->SetValue($value['search_value']);
        }
    }

    public function SetSqlColDef($colDef)
    {
        $this->mColDef = $colDef;
    }
    
    public function RenderUI()
    {
        $selected = ' selected="selected"';
        switch($this->mOp)
        {
            case "eq"  :
                $eq_selected = $selected;   break;
            case "contains" :
                $c_selected = $selected;  break;
            case "begins" :
                $b_selected = $selected;  break;
            case "ends" :
                $e_selected = $selected;  break;                
            default :
                break;
        }
        
        $out  = '<select name="__filter__['.$this->mName.'][search_op]">';
        $out .= '<option value="contains"'.$c_selected.'>contains</option>';
        $out .= '<option value="eq"'.$eq_selected.'>equals</option>';
        $out .= '<option value="begins"'.$b_selected.'>begins with</option>';
        $out .= '<option value="ends"'.$e_selected.'>ends with</option>';
        $out .= '</select> ';
        $out .= $this->mFilterType->RenderUI();
        return $out;
    }

    public function GetSqlCondition()
    {
        $value = $this->mFilterType->GetForSql();
        if (!is_null($value))
        {
            if ($value{0} == '"')
                $value = substr($value,1,strlen($value)-2);
                
            switch ($this->mOp)
            {
                case "contains" :
                    return $this->mColDef.' LIKE "%'.$value.'%"'; break;
                case "eq" :
                    return $this->mColDef.' = "'.$value.'"'; break;
                case "begins" :
                    return $this->mColDef.' LIKE "'.$value.'%"'; break;
                case "ends" :
                    return $this->mColDef.' LIKE "%'.$value.'"'; break;
                default :
                    break;
            }
        }
        else
            return NULL;
    }

    public function GetName()
    {
        return $this->mName;
    }

}

class CompareFilter implements Filter
{

    private $mOp = NULL;
    private $mName = NULL;
    private $mFilterType = NULL;
    private $mColDef = NULL;
    private $mEqEnable = NULL;

    public function __construct($name, FilterType $type)
    {
        $this->mColDef = $this->mName = $name;
        $this->mFilterType = $type;
        $this->mEqEnable = TRUE;
        $this->mFilterType->SetName("$name][compare_value");
    }

    public function SetValue($value)
    {
        if (is_array($value))
        {
            $this->mOp = $value['compare_op'];
            return $this->mFilterType->SetValue($value['compare_value']);
        }
        return TRUE;
    }

    public function SetSqlColDef($colDef)
    {
        $this->mColDef = $colDef;
    }
    
    public function ToggleEq($val)
    {
        $this->mEqEnable = (bool) $val;
    }

    public function RenderUI()
    {
        $selected = ' selected="selected"';
        switch($this->mOp)
        {
            case "eq"  :
                $eq_selected = $selected;   break;
            case "lte" :
                $lte_selected = $selected;  break;
            case "gte" :
                $gte_selected = $selected;  break;
            default :
                break;
        }
        
        $out  = '<select name="__filter__['.$this->mName.'][compare_op]">';
        $out .= '<option value="">(don\'t compare)</option>';
        if ($this->mEqEnable)
            $out .= '<option value="eq"'.$eq_selected.'>is</option>';
        $out .= '<option value="lte"'.$lte_selected.'>less than or</option>';
        $out .= '<option value="gte"'.$gte_selected.'>greater than or</option>';
        $out .= '</select> equal to ';
        $out .= $this->mFilterType->RenderUI();
        return $out;
    }

    public function GetSqlCondition()
    {
        $value = $this->mFilterType->GetForSql();
        switch($this->mOp)
        {
            case "eq"  :
                $op = '=';      break;
            case "lte" :
                $op = '<=';     break;
            case "gte" :
                $op = '>=';     break;
            default :
                $op = NULL;     break;
        }

        if ($value && !is_null($op))
            return $this->mColDef.' '.$op.' '.$value;
        else
            return NULL;
    }

    public function GetName()
    {
        return $this->mName;
    }

}


class LoginTypeFilter extends ExactFilter
{

    private $mValue = NULL;
    
    public function __construct($name, FilterType $type)
    {
        $login_type_enum['web'] = "Web";
        $login_type_enum['phone'] = "Phone";
        $login_type_enum['call'] = "Call";

		// Ignore $type
        $filter_enum = new FilterEnumeration();
        $filter_enum->SetEnumeration($login_type_enum);
        parent::__construct($name, $filter_enum);
    }

    public function SetValue($value)
    {
        $this->mValue = $value;
        return parent::SetValue($value);
    }
    
    public function GetSqlCondition()
    {
        switch ($this->mValue)
        {
            case 'phone' :
                return 'originating_number IS NOT NULL';
                break;
            case 'web' :
                return 'source_ip_address IS NOT NULL';
                break;
            case 'call' :
                return '(originating_number IS NULL AND source_ip_address IS NULL)';
                break;
            default :
                return NULL;
        }
    }
    
}
?>