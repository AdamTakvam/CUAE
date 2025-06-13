<?php

require_once("common.php");
require_once("class.ComponentExtensionParameterHandler.php");

abstract class ComponentExtension
{
    protected $mId;
    protected $mDb;
    protected $mData;
    protected $mParameters;
    protected $mStatus;
    protected $mErrorHandler;


    function __construct()
    {
        $this->mId = NULL;
        $this->mDb = new MceDb;
        $this->mData = array();
        $this->mParameters = array();
        $this->mErrorHandler = NULL;
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

    function SetErrorHandler(ErrorHandler $eh)
    {
        $this->mErrorHandler = $eh;
    }

    function Build($data)
    {
        if (array() == $this->mData && NULL != $this->mId)
        {
            $this->mData = $this->mDb->GetRow("SELECT * FROM mce_provider_extensions WHERE mce_provider_extensions_id = ?", array($this->mId));
            return TRUE;
        }
        return FALSE;
    }

    function BuildWithData($data)
    {
        if (array() == $this->mData)
        {
            $this->mId = $data['mce_provider_extensions_id'];
            $this->mData = $data;
            return TRUE;
        }
        return FALSE;
    }

    function GetId()
    {
        return $this->mId;
    }

    function GetName()
    {
        return $this->mData['name'];
    }

    function GetDescription()
    {
        return $this->mData['description'];
    }

    function GetParameters()
    {
        if (array() == $this->mParameters)
        {
            $param_rows = $this->mDb->GetAll("SELECT * FROM mce_provider_extensions_parameters WHERE mce_provider_extensions_id = ?", array($this->mId));
            for ($i = 0; $i < sizeof($param_rows); ++$i)
            {
                $this->mParameters[$i] = new ComponentExtensionParameterHandler();
                $this->mParameters[$i]->BuildWithData($param_rows[$i]);
                $this->mParameters[$i]->SetErrorHandler($this->mErrorHandler);
            }
        }
        return $this->mParameters;
    }

    function GetCompletionStatus()
    {
        return $this->mData['wait_for_completion'];
    }

    abstract function InvokeExtension($params = array());

}

?>