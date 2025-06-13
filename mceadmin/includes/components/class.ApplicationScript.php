<?php

require_once("common.php");

class ApplicationScript
{

    private $mDb;
    private $mData;
    private $mId;
    private $mPartitionId;
    private $mParameters;

    function __construct()
    {
        $this->mDb = new MceDb();
        $this->mId = NULL;
        $this->mPartitionId = NULL;
        $this->mData = array();
        $this->mParameters = array();
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

    function SetPartitionId($id)
    {
        if (NULL == $this->mPartitionId)
        {
            $this->mPartitionId = $id;
            return TRUE;
        }
        return FALSE;
    }

    function Build()
    {
        if (array() == $this->mData && NULL != $this->mId)
        {
            $this->mData = $this->mDb->GetRow("SELECT * FROM mce_application_scripts WHERE mce_application_scripts_id = ?", array($this->mId));
        }
        return FALSE;
    }

    function BuildWithData($data)
    {
        if (array() == $this->mData)
        {
            $this->mData = $data;
            $this->mId = $data['mce_application_scripts_id'];
            return TRUE;
        }
        return FALSE;
    }

    function GetId()
    {
        return $this->mData['mce_application_scripts_id'];
    }

    function GetName()
    {
        return $this->mData['name'];
    }

    function GetEventType()
    {
        return $this->mData['event_type'];
    }

    function GetTriggerParameters()
    {
        if (array() == $this->mParameters && NULL != $this->mId && NULL != $this->mPartitionId)
        {
            $myparams = $this->mDb->GetAll("SELECT * FROM mce_application_script_trigger_parameters " .
                                           "WHERE mce_application_scripts_id = ? AND mce_application_partitions_id = ?",
                                           array($this->mId, $this->mPartitionId));
            $params = array();
            for ($x = 0; $x < sizeof($myparams); ++$x)
            {
                $param_id = $myparams[$x]['mce_application_script_trigger_parameters_id'];
                $myvalues = $this->mDb->GetAll("SELECT * FROM mce_trigger_parameter_values WHERE mce_application_script_trigger_parameters_id = ?",
                                               array($param_id));
                $params[$param_id] = $myparams[$x];
                foreach ($myvalues as $myval)
                {
                    $params[$param_id]['values'][$myval['mce_trigger_parameter_values_id']] = $myval['value'];
                }
            }
            $this->mParameters = $params;
        }
        return $this->mParameters;
    }

    function UsesCallControl()
    {
        return $this->mData['uses_call_control'];
    }

    function UsesMediaControl()
    {
        return $this->mData['uses_media_control'];
    }

}

?>