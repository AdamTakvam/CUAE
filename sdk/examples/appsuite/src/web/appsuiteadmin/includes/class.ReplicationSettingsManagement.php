<?php

require_once("config.replication.php");

class ReplicationSettingsManagement
{

    private $mServerId;
    private $mMasterSettings;
    private $mSlaveSettings;
    private $mDatabaseName;
    
    public function __construct()
    {
        $this->mServerId =  NULL;
        $this->mMasterSettings = array();
        $this->mSlaveSettings = array();
        $this->mDatabaseName = "application_suite";
        $this->ReadSettingsFile();
    }
    
    public function GetServerId()
    {
        return $this->mServerId;
    }
    
    public function SetServerId($id)
    {
        $this->mServerId = $id;
    }
    
    public function IsMasterHostSet()
    {
        return !empty($this->mSlaveSettings['HOST']);
    }
        
    public function GetMasterSettings()
    {
        return $this->mMasterSettings[$this->mDatabaseName];
    }
    
    public function GetSlaveSettings($unconditional = FALSE)
    {
        if (isset($this->mSlaveSettings['DATABASE']))
            if (in_array($this->mDatabaseName,$this->mSlaveSettings['DATABASE']) || $unconditional)
                return $this->mSlaveSettings;
        return array();
    }

    public function SetMasterSettings($user, $password)
    {
        $this->mMasterSettings[$this->mDatabaseName] = array('USER' => $user, 'PASSWORD' => $password, 'DATABASE' => $this->mDatabaseName);
        return TRUE;
    }
    
    public function SetSlaveSettings($user, $password, $host = NULL)
    {
        $this->mSlaveSettings['USER'] = $user;
        $this->mSlaveSettings['PASSWORD'] = $password;
        if (!empty($host))
            $this->mSlaveSettings['HOST'] = $host;
        if (!isset($this->mSlaveSettings['DATABASE']) || !in_array($this->mDatabaseName,$this->mSlaveSettings['DATABASE']))
        {
            $this->mSlaveSettings['DATABASE'][] = $this->mDatabaseName;
        }
    }
    
    public function DisableMaster()
    {
        unset($this->mMasterSettings[$this->mDatabaseName]);
    }
    
    public function DisableSlave()
    {
        if (sizeof($this->mSlaveSettings['DATABASE']) > 1)
        {
            $index = array_search($this->mDatabaseName, $this->mSlaveSettings['DATABASE']);
            if ($index !== FALSE)
                unset($this->mSlaveSettings['DATABASE'][$index]);
        }
        else
            unset($this->mSlaveSettings);
    }
    
    public function ApplySettings()
    {
        $this->WriteMySqlConfigFile();
        $this->WriteSettingsFile();
    }
    
    private function ReadSettingsFile()
    {
        if (!file_exists(ReplicationConfig::MYSQL_REPLICATION_SETTINGS))
            return TRUE;
    
        // Parse XML settings file into a set of arrays
		$settings = array();
		$settings_xml = file_get_contents(ReplicationConfig::MYSQL_REPLICATION_SETTINGS);
		if (!$settings_xml)
			throw new Exception("Could not open replication settings file");
		$parser = xml_parser_create();
        xml_parser_set_option($parser, XML_OPTION_SKIP_WHITE, 1);
		if (xml_parse_into_struct($parser, $settings_xml, $cf_values, $cf_indexes) == 0)
			throw new Exception("Replication config XML file could not be parsed.");
		xml_parser_free($parser);
        
        // Traverse through the set of arrays and build hashs
        $this->mServerId = $cf_values[$cf_indexes['SERVERID'][0]]['value'];
        
        foreach ($cf_indexes as $tag => $indexes)
        {
            if ($tag == "SLAVE")
            {
                $this->mSlaveSettings = $this->ReadNestedTags($cf_values, $indexes[0], $indexes[1]);
                if (!empty($this->mSlaveSettings) && !is_array($this->mSlaveSettings['DATABASE']))
                    $this->mSlaveSettings['DATABASE'] = array($this->mSlaveSettings['DATABASE']);
            }
            elseif ($tag == "MASTER")
            {
                for ($x = 0; $x < count($indexes); $x += 2)
                {
                    $inner_tags = $this->ReadNestedTags($cf_values, $indexes[$x], $indexes[$x+1]);
                    $this->mMasterSettings[$inner_tags['DATABASE']] = $inner_tags;
                }
            }
            else
            {
                continue;
            }
        }
    }

    public function WriteSettingsFile()
    {
        $out[] = '<?xml version="1.0" encoding="utf-8" ?>';
        $out[] = '<config>';
        $out[] = '<serverid>' . $this->mServerId . '</serverid>';
        foreach ($this->mMasterSettings as $master_settings)
        {
            $out[] = '<master>';
            foreach ($master_settings as $name => $val)
            {
                if ($name == "PASSWORD")
                    $val = base64_encode($val);
                $out[] = '<'.strtolower($name).'>'.$val.'</'.strtolower($name).'>';
            }
            $out[] = '</master>';
        }
        if (!empty($this->mSlaveSettings))
        {
            $out[] = '<slave>';
            foreach ($this->mSlaveSettings as $name => $val)
            {
                if ($name == "PASSWORD")
                    $val = base64_encode($val);
                if (is_array($val))
                {
                    foreach ($val as $one_val)
                    {
                        $out[] = '<'.strtolower($name).'>'.$one_val.'</'.strtolower($name).'>';
                    }
                }
                else
                    $out[] = '<'.strtolower($name).'>'.$val.'</'.strtolower($name).'>';
            }            
            $out[] = '</slave>';
        }
        $out[] = '</config>';
        
        file_put_contents(ReplicationConfig::MYSQL_REPLICATION_SETTINGS, implode("\n",$out));
    }
    
    private function WriteMySqlConfigFile()
    {
		if (!file_exists(ReplicationConfig::MYSQL_CONF_BACKUP))
			copy(ReplicationConfig::MYSQL_CONF, ReplicationConfig::MYSQL_CONF_BACKUP);
		else
			copy(ReplicationConfig::MYSQL_CONF_BACKUP, ReplicationConfig::MYSQL_CONF);
 
		$fp = fopen(ReplicationConfig::MYSQL_CONF, 'a');
		if (!$fp)
			throw new Exception("Could not open " . ReplicationConfig::MYSQL_CONF . " for editing");
		fwrite($fp, "\n");
        
        fwrite($fp, "[mysqld]\n");
        
        if (!empty($this->mMasterSettings) || !empty($this->mSlaveSettings))
            fwrite($fp, "server-id=".$this->mServerId."\n");
            
        if (!empty($this->mMasterSettings))
        {
    		fwrite($fp, "log-bin\n");
            foreach ($this->mMasterSettings as $db_name => $settings)
            {
                fwrite($fp, "binlog-do-db=".$db_name."\n");
            }
        }
        
        if (!empty($this->mSlaveSettings))
        {
            foreach ($this->mSlaveSettings['DATABASE'] as $db_name)
            {
                if ($db_name == "mce")
                {
                    fwrite($fp, "replicate-rewrite-db=mce->mce_standby\n");
                    fwrite($fp, "replicate-do-db=mce_standby\n");
                }
                else
                {
                    fwrite($fp, "replicate-do-db=".$db_name."\n");
                }
            }
        }
        
		fclose($fp);
    }
    
    private function ReadNestedTags($values, $start, $end)
    {
        $tags = array();
        $offset = $start + 1;
        for ($x = $offset; $x < $end; $x++)
        {
            $tag_name = $values[$x]['tag'];
            $tag_value = $values[$x]['value'];
            $tag_type = $values[$x]['type'];
            if ($tag_name == "PASSWORD")
                $tag_value = base64_decode($tag_value);
            if ($tag_type != 'complete')
                throw new Exception('The config file parser is not currently prepared to parse non-complete XML nested tags');
            if (isset($tags[$tag_name]))
            {
                if (!is_array($tags[$tag_name]))
                    $tags[$tag_name] = array($tags[$tag_name]);
                $tags[$tag_name][] = $tag_value;
            }
            else
                $tags[$tag_name] = $tag_value;
        }
        return $tags;
    }
    
}

?>