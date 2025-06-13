<?php

/* The ErrorHandler class is a very simple class which collects errors.  It is
 * ideal to have one per page and let accumulate all the errors that would ultimately
 * be displayed to the user.  It can also be used to check to make sure that everything
 * is in a valid state.
 */

class ErrorHandler
{

/* PRIVATE MEMBERS */

    private $mErrors;   // Array that holds error messages


/* PUBLIC METHODS */

    public function __construct()
    {
        $this->mErrors = array();
    }

    public function Add($error)
    {
        if (!empty($error))
            $this->mErrors[] = $error;
    }

    public function IsEmpty()
    {
        return (0 == sizeof($this->mErrors));
    }

    public function Clear()
    {
        $this->mErrors = array();
    }

    public function Dump()
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