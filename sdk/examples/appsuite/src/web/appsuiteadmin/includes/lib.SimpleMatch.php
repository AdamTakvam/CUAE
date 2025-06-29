<?php

require_once("enums.php");

abstract class SimpleMatch
{

    static public function complex_list_format($list)
    {
        if (empty($list))
            return array();
        foreach ($list as $index => $value)
        {
            $list2[$index]['number'] = $value;
        }
        return $list2;
    }

    static public function translate_from_regex($set)
    {        
        if (empty($set))
            return array();
        function translate_regex_bits($row)
        {
            $first = $row['number']{0};
            $last = $row['number']{strlen($row['number']) - 1};
            if ($first == '^' && $last == '$')
                $row['type'] = SimpleMatchType::EQUALS;
            elseif ($first == '^')
                $row['type'] = SimpleMatchType::BEGINS_WITH;
            elseif ($last == '$')
                $row['type'] = SimpleMatchType::ENDS_WITH;
            else
                $row['type'] = SimpleMatchType::CONTAINS;
            $row['number'] = stripcslashes(trim($row['number'],"^$"));
            return $row;
         }

        return array_map('translate_regex_bits',$set);
    }

    static public function form_regex($type, $number)
    {
        $number = addcslashes($number,"+*");
        switch ($type)
        {
            case SimpleMatchType::BEGINS_WITH :
                return '^'.$number;
            case SimpleMatchType::ENDS_WITH :
                return $number.'$';
            case SimpleMatchType::EQUALS :
                return '^'.$number.'$';
            default:
                return $number;
        }
    }
    
}

?>