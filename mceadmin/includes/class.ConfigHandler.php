<?php

/* The ConfigHandler is a handle type class for the Configuration types for existing configs.
 * An additional benefit to this class is that a config can belong to a component or a
 * partition, and when a switch is needed to be done, it can be conveniently handled to
 * look like it is the same entitiy.
 */

require_once("common.php");
require_once("lib.ComponentUtils.php");
Utils::require_directory("configs");

class ConfigHandler {

/* CONFIG TYPE ENUMERATION */

    const COMPONENT = 0;
    const PARTITION = 1;


/* PROTECTED MEMBERS */

    protected $mDb;             // Database object
    protected $mId;             // Id of the handled configuration entry
    protected $mLevel;          // Level (component, partition) of the configuration
    protected $mConfig;         // Configuration object
    protected $mType;           // Type of the configuration object
    protected $mErrHandler;     // Error handling object


/* PUBLIC METHODS */

    //  BUILDING & CONSTRUCTING METHODS

    public function __construct() {
    // Initialize handler
        $this->mDb = new MceDb();
        $this->mId = NULL;
        $this->mLevel = self::COMPONENT;
        $this->mConfig = NULL;
        $this->mType = NULL;
        $this->mErrHandler = NULL;
    }

    public function SetId($id)
    // Set the id of the configuration to handle
    {
        if (NULL == $this->mId)
        {
            $this->mId = $id;
            return TRUE;
        }
        return FALSE;
    }

    public function SetErrorHandler(ErrorHandler $eh)
    // Set the ErrorHandler object for the handler to use
    {
        $this->mErrHandler = $eh;
    }

    public function Build()
    // Retrieve information on the configuration and build a Configuration object
    {
        // Retrieve a specific configuration entry
        if (NULL == $this->mConfig && NULL != $this->mId)
        {
            $data = $this->mDb->GetRow("SELECT * FROM mce_config_entries LEFT JOIN mce_config_entry_metas USING (mce_config_entry_metas_id) " .
                                "WHERE mce_config_entries_id = ?",
                                array($this->mId));
            return $this->BuildWithData($data);
        }
        return FALSE;
    }

    public function BuildWithData($data)
    // Build a configuration objects with the a hash of data
    {
        // Note which level this config is on ...
        if ($data['mce_application_partitions_id'] > 0)
            $this->mLevel = self::PARTITION;
        else
            $this->mLevel = self::COMPONENT;

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

        $this->mConfig->BuildWithData($data);
        return $this->mConfig->FetchValues();
    }


    public function AddContextParam($name, $value)
    // Adds a parameter $value labeled with $name that may be useful to the
    // configuration object.
    {
        $this->mConfig->AddContextParam($name, $value);
    }


    // MODIFCATION METHODS

    public function Validate($values)
    // Validate $values with the configuration's validator and return the results
    {
        if ($this->mConfig->GetData('read_only'))
            return TRUE;
        if (!$this->mConfig->ValidateValues($values))
        {
            if ($this->mErrHandler)
                $this->mErrHandler->Add($this->GetDisplayName() . " field was set to an invalid value.  " . $this->mConfig->GetError());
            return FALSE;
        }
        return TRUE;
    }

    public function Update($values, $partitionId = NULL)
    // Update the configuration with $values.  If the update is for a partition,
    // pass in the ID for partition as $partitionId.
    {
        if ($this->mConfig->GetData('read_only'))
            return TRUE;
        if (!$this->mConfig->IsSameValues($values))
        {
            // If this update is for a partition from a component entry, create a new entry and make the switch!
            if (self::COMPONENT == $this->mLevel && NULL != $partitionId)
            {
                $meta_id = $this->mConfig->GetData('mce_config_entry_metas_id');
                $this->mDb->Execute("INSERT INTO mce_config_entries (mce_config_entry_metas_id, mce_application_partitions_id) VALUES (?, ?)",
                                   array($meta_id, $partitionId));
                $this->mId = $this->mDb->Insert_ID();
                $this->mConfig = NULL;
                $this->mLevel = self::PARTITION;
                $this->Build();
                $this->mConfig->InsertValues($values);
            }
            else
            {
                 $this->mConfig->UpdateValues($values);
            }
            return TRUE;
        }
        return TRUE;
    }


    // RETRIEVE DATA

    public function GetId()
    {
        return $this->mId;
    }
    
    public function GetMetaId()
    {
        return $this->mConfig->GetData('mce_config_entry_metas_id');
    }

    public function GetName()
    {
        return $this->mConfig->GetData('name');
    }

    public function GetDisplayName()
    {
        $display_name = $this->mConfig->GetData('display_name');
        return $display_name ? $display_name : $this->GetName();
    }

    public function GetType()
    {
        return $this->mType;
    }

    public function GetComponentId()
    // Get the ID for the component that is associated with the configuration
    {
        $id = $this->mConfig->GetData('mce_components_id');
        if (empty($id))
        {
            $part_id = $this->mConfig->GetData('mce_application_partitions_id');
            if (!empty($part_id))
                $id = $this->mDb->GetOne("SELECT mce_components_id FROM mce_application_partitions " .
                                         "WHERE mce_application_partitions_id = ?",
                                         array($part_id));
            else
                return NULL;
        }
        return $id;
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
        return $this->mConfig->GetFields($values);
    }

    public function GetValues()
    // Get the current values for the configuration
    {
        return $this->mConfig->FetchValues();
    }

}

?>