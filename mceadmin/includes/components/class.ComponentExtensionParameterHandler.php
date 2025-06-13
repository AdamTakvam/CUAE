<?php

require_once("common.php");
Utils::require_directory("configs");


class ComponentExtensionParameterHandler
{

    private $mData;
    private $mParam;
    private $mErrorHandler;

    function __construct()
    {
        $this->mData = array();
        $this->mErrorHandler = NULL;
        $this->mParam = NULL;
    }

    function SetErrorHandler(ErrorHandler $eh)
    {
        $this->mErrorHandler = $eh;
        return TRUE;
    }

    function BuildWithData($data)
    {
        $dummyDb = new MceDb();
        if (array() == $this->mData)
        {
            $this->mData = $data;
            switch ($data['mce_format_types_id'])
            {
                case FormatType::CONFIG_STRING :
                    $this->mParam = new StringConfig();
                    break;
                case FormatType::CONFIG_DATETIME :
                    $this->mParam = new DateTimeConfig();
                    break;
                case FormatType::CONFIG_NUMBER :
                    $this->mParam = new NumberConfig();
                    break;
                case FormatType::CONFIG_BOOL :
                    $this->mParam = new BooleanConfig();
                    break;
                case FormatType::CONFIG_IP_ADDRESS :
                    $this->mParam = new IpAddressConfig();
                    break;
                case FormatType::CONFIG_ARRAY :
                    $this->mParam = new ArrayConfig();
                    break;
                case FormatType::CONFIG_HASH :
                    $this->mParam = new HashConfig();
                    break;
                case FormatType::CONFIG_TABLE :
                    $this->mParam = new TableConfig();
                   break;
                case FormatType::CONFIG_PASSWORD :
                    $this->mParam = new PasswordConfig();
                   break;
                default:
                    $this->mParam = new EnumConfig();
            }
            $this->mParam->BuildWithData($data);
            return TRUE;
        }
        return FALSE;
    }

    function GetId()
    {
        return $this->mData['mce_provider_extenstions_parameters_id'];
    }

    function GetName()
    {
        return $this->mData['name'];
    }

    function GetDescription()
    {
        return $this->mData['description'];
    }

    function GetFields()
    {
        return $this->mParam->GetFields();
    }

    function Validate($value)
    {
        if (!$this->mParam->ValidateValues($value))
        {
            if ($this->mErrorHandler)
            {
                $this->mErrorHandler->Add($this->GetName() . " field was set to an invalid value.  " . $this->mParam->GetError());
            }
            return FALSE;
        }
        return TRUE;
    }

    function Assemble($value)
    {
        return $this->mParam->AssembleValues($value);
    }
}

?>