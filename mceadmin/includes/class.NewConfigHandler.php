<?php

/* NewConfigHandler is the sister class to ConfigHandler.  It does what ConfigHandler does except
 * it handles the case that new configurations are being created.  It is created from preexisting
 * metadata about a config, and ultimately creates a config entry.  Since all config entries "created"
 * get associated with a component, there is no need to worry about the idea od partition/components.
 */

require_once("common.php");
Utils::require_directory("configs");

class NewConfigHandler {

/* PROTECTED MEMBERS */

    protected $mDb;             // Database object
    protected $mMetaId;         // Id of the configuration metadata
    protected $mConfig;         // Configuration object
    protected $mType;           // Type of the configuration object
    protected $mDefault;        // Default value for the new config
    protected $mErrHandler;     // Error handling object


/* PUBLIC METHODS */

    //  BUILDING & CONSTRUCTING METHODS

    public function __construct() {
    // Initialize handler
        $this->mDb = new MceDb();
        $this->mMetaId = NULL;
        $this->mConfig = NULL;
        $this->mType = NULL;
        $this->mErrHandler = NULL;
        $this->mDefault = NULL;
    }

    public function SetMetaId($id)
    // Set the id of the configuration metadata to construct config from
    {
        if (NULL == $this->mId)
        {
            $this->mId = $id;
            return TRUE;
        }
        return FALSE;
    }

    public function SetErrorHandler(ErrorHandler $eh)
    // Set the ErrorHandler object
    {
        $this->mErrHandler = $eh;
    }

    public function Build()
    // Retrieve the configuration metadata and build the configuration object
    {
        if (NULL == $this->mConfig && NULL != $this->mMetaId)
        {
            $data = $db->GetRow("SELECT * FROM mce_config_entry_metas WHERE mce_config_entry_metas_id = ?",
                               array($this->mMetaId));
            return $this->BuildWithData($data);
        }
        return FALSE;
    }

    public function BuildWithMetaData($data)
    // Build a new configuration object from the passed in $data
    {
        // Aggregate the config based on type
        // Would be nice to do this on sort of a protoype/factory pattern...
        $this->mType = $data["mce_format_types_id"];
        switch ($this->mType)
        {
            case FormatType::CONFIG_STRING :
                $this->mConfig = new StringConfig();
                break;
            case FormatType::CONFIG_DATETIME :
                $this->mConfig = new DateTimeConfig();
                break;
            case FormatType::CONFIG_NUMBER :
                $this->mConfig = new NumberConfig();
                break;
            case FormatType::CONFIG_BOOL :
                $this->mConfig = new BooleanConfig();
                break;
            case FormatType::CONFIG_IP_ADDRESS :
                $this->mConfig = new IpAddressConfig();
                break;
            case FormatType::CONFIG_ARRAY :
                $this->mConfig = new ArrayConfig();
                break;
            case FormatType::CONFIG_HASH :
                $this->mConfig = new HashConfig();
                break;
            case FormatType::CONFIG_TABLE :
                $this->mConfig = new TableConfig();
               break;
            case FormatType::CONFIG_PASSWORD :
                $this->mConfig = new PasswordConfig();
               break;
            default:
                $this->mConfig = new EnumConfig();
        }

        $this->mMetaId = $data["mce_config_entry_metas_id"];
        return $this->mConfig->BuildWithData($data);
    }


    // MODIFCATION METHODS

    public function Validate($values)
    // Validates $values using the configuration object and handles the result
    {
        if (!$this->mConfig->ValidateValues($values))
        {
            if ($this->mErrHandler)
                $this->mErrHandler->Add($this->GetDisplayName() . " field was set to an invalid value.  " . $this->mConfig->GetError());
            return FALSE;
        }
        return TRUE;
    }

    public function Create($id, $values)
    // Creates a new configuration entry from the meta data with the values $values and
    // associates it with the the component with the id number $id.
    {
        $this->mDb->Execute("INSERT INTO mce_config_entries (mce_config_entry_metas_id, mce_components_id) VALUES (?, ?)",
                           array($this->mMetaId, $id));
        $this->mConfig->SetId($this->mDb->Insert_ID());
        return $this->mConfig->InsertValues($values);
    }

    public function SetDefault($values)
    // Set the default value displayed when creating a new config
    {
        $this->mDefault = $values;
    }

    
    // ** Retrieve Properties/Information **

    public function GetMetaId()
    {
        return $this->mMetaId;
    }
    
    public function GetName()
    {
        return $this->mConfig->GetData('name');
    }

    public function GetDisplayName()
    {
        return $this->mConfig->GetData('display_name');
    }

    public function GetType()
    {
        return $this->mType;
    }

    public function GetDescription()
    {
        return $this->mConfig->GetData('description');
    }

    public function GetMetaDescription()
    {
        $meta_description = array();
        if (!$this->mConfig->GetData('required'))
            $meta_description[] = "Optional";
        if (FormatType::CONFIG_NUMBER == $this->mConfig->GetData('mce_format_types_id'))
        {
            $min = $this->mConfig->GetData('min_value');
            $max = $this->mConfig->GetData('max_value');
            if (!empty($min) || !empty($max))
            {
                $meta_description[] = 'Accepted Range: ' . $this->mConfig->GetData('min_value') . ' - ' . $this->mConfig->GetData('max_value');
            }
        }
        return implode($meta_description,', ');    
    }

    public function GetFields($values = NULL)
    // Get the input fields as displayed on the page, optionally using $values
    {
        if ($this->mDefault && is_null($values))
            $values = $this->mDefault;
        return $this->mConfig->GetFields($values);
    }

}

?>