<?php

require_once("common.php");
require_once("class.ConfigHandler.php");


abstract class Component
{

    protected $mId;
    protected $mDb;
    protected $mData;
    protected $mGroups;
    protected $mConfigs;
    public $mListPage;
    public $mEditPage;

    protected $mErrorHandler;

    function __construct()
    {
        $this->mId = NULL;
        $this->mDb = new MceDb();
        $this->mData = array();
        $this->mGroups = array();
        $this->mConfigs = array();
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

    function Build()
    {
        if (NULL != $this->mId)
        {
            $this->mData = $this->mDb->GetRow("SELECT * FROM mce_components WHERE mce_components_id = ?", array($this->mId));
            if (empty($this->mData))
                Utils::redirect($this->mListPage);
            return TRUE;
        }
        return FALSE;
    }

    function GetConfigs()
    {
        if (array() == $this->mConfigs)
        {
            $query  = "SELECT * FROM mce_config_entries LEFT JOIN mce_config_entry_metas USING (mce_config_entry_metas_id) ";
            $query .= "WHERE mce_components_id = ? ORDER BY mce_config_entries_id ASC";

            $rs = $this->mDb->Execute($query, array($this->mData['mce_components_id']));
            while ($config_row = $rs->FetchRow())
            {
                if (!empty($config_row['name']))
                {
                    $this->mConfigs[$config_row['name']] = new ConfigHandler();
                    $this->mConfigs[$config_row['name']]->BuildWithData($config_row);
                    if (NULL != $this->mErrorHandler)
                        $this->mConfigs[$config_row['name']]->SetErrorHandler($this->mErrorHandler);
                }
            }
        }
        return $this->mConfigs;
    }

    function GetId()
    {
        return $this->mId;
    }

    function GetName($display_name = TRUE)
    {
        if ($display_name)
            return Utils::is_blank($this->mData['display_name']) ? $this->mData['name'] : $this->mData['display_name'];
        else
            return $this->mData['name'];
    }

    function GetType()
    {
        return $this->mData['type'];
    }

    function GetStatus()
    {
        return $this->mData['status'];
    }

    function GetVersion()
    {
        return $this->mData['version'];
    }

    function GetMetaData()
    {
        $keys = array("author","copyright","author_url","support_url","description");
        return Utils::extract_array_using_keys($keys, $this->mData);
    }

    function Refresh()
    {
        $socket = new AppServerInterface();
        if ($socket->Connected())
        {
            $params = array('ComponentType' => ComponentType::display($this->mData['type']), 'ComponentName' => $this->mData['name']);
            $socket->Send(MceUtils::generate_xml_command('RefreshConfiguration', $params));
        }
        if ($socket->Error())
        {
            $this->mErrorHandler->Add($socket->Error());
            return FALSE;
        }
        return TRUE;
    }

    function Uninstall()
    {
        if (NULL != $this->mId)
        {
            $this->mDb->StartTrans();
            $entries = $this->mDb->GetAll("SELECT * FROM mce_config_entries WHERE mce_components_id = ?", array($this->mId));
            foreach ($entries as $entry)
            {
                $this->mDb->Execute("DELETE FROM mce_config_values WHERE mce_config_entries_id = ?", array($entry['mce_config_entries_id']));
                if ($entry['mce_config_entry_metas_id'] >= CONFIG_META_DATA_START)
                    $this->mDb->Execute("DELETE FROM mce_config_entry_metas WHERE mce_config_entry_metas_id = ?", array($entry['mce_config_entry_metas_id']));
            }
            $this->mDb->Execute("DELETE FROM mce_config_entries WHERE mce_components_id = ?", array($this->mId));

            $format_ids = $this->mDb->GetCol("SELECT mce_format_types_id FROM mce_format_types WHERE mce_components_id = ?", array($this->mId));
            foreach ($format_ids as $format_id)
            {
                $this->mDb->Execute("DELETE FROM mce_format_type_enum_values WHERE mce_format_types_id = ?", array($format_id));
            }
            $this->mDb->Execute("DELETE FROM mce_format_types WHERE mce_components_id = ?", array($this->mId));

            $this->mDb->Execute("DELETE FROM mce_component_group_members WHERE mce_components_id = ?", array($this->mId));
            $this->mDb->Execute("DELETE FROM mce_components WHERE mce_components_id = ?", array($this->mId));
            $this->mDb->CompleteTrans();
            return TRUE;
        }
        return FALSE;
    }


}

?>