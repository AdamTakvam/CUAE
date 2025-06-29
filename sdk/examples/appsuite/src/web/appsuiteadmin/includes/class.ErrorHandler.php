<?php

class ErrorHandler
{

    private $mErrors;

    function __construct()
    {
        $this->mErrors = array();
    }

    function Add($error)
    {
        $this->mErrors[] = $error;
    }

    function IsEmpty()
    {
        return (0 == sizeof($this->mErrors));
    }

    function Clear()
    {
        $this->mErrors = array();
    }

    function Dump()
    {
        if (array() != $this->mErrors)
        {
            $dump = "<ul>";
            foreach ($this->mErrors as $error)
            {
                $dump .= "<li>" . htmlspecialchars($error) . "</li>";
            }
            $dump .= "</ul>";
            return $dump;
        }
        return NULL;
    }
}

?>