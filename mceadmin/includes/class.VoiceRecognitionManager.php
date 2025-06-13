<?php

abstract class VoiceRecognitionManager
{

    const SERVER_CONFIG_FILE        = "C:\Program Files\SpeechWorks\OpenSpeech Recognizer\config\SWIsvrConfig.xml";
    const SPEECHWORKS_CONFIG_FILE   = "C:\Program Files\SpeechWorks\OpenSpeech Recognizer\config\SpeechWorks.cfg";
    
    public static function read_server_list()
    {
        $fp = fopen(VoiceRecognitionManager::SERVER_CONFIG_FILE,'r');
        if (!$fp)
            throw new Exception('Could not open the resource configuration file');
        
        while (!feof($fp))
        {
            $line = fgets($fp);
            if (eregi('<server host="(.+:[0-9]+)"/>',$line,$regs))
            {
                list($host,$port) = explode(':', $regs[1]);
                $servers[] = array('host' => $host, 'port' => $port);
            }
        }
        fclose($fp);
        
        return $servers;
    }

    public static function store_server_list($servers)
    {
        if (!is_array($servers))
            throw new Exception('Server list is not in an array');

        $fp = fopen(VoiceRecognitionManager::SERVER_CONFIG_FILE,'r');
        if (!$fp)
            throw new Exception('Could not open the resource configuration file');

        $data = "";
        while (!feof($fp))
        {
            $line = fgets($fp);
            $data .= $line;
            if (eregi('<server-group',$line))
                break;
        }
        
        foreach ($servers as $server)
        {
            $data .= "\t\t<server host=\"" . $server['host'] . ":" . $server['port'] ."\"/>\n";
        }
        
        while (!feof($fp) && !eregi('</server-group', $line))
            $line = fgets($fp);
        
        $data .= $line;
        while (!feof($fp))
        {
            $data .= fgets($fp);
        }
        fclose($fp);

        $bytes = file_put_contents(VoiceRecognitionManager::SERVER_CONFIG_FILE, $data);
        
        return ($bytes > 0);
        
    }
    
    public static function read_license_server_list()
    {
        $fp = fopen(VoiceRecognitionManager::SPEECHWORKS_CONFIG_FILE,'r');
        if (!$fp)
            throw new Exception('Could not open the SpeechWorks configuration file');
        
        while (!feof($fp))
        {
            $line = fgets($fp);
            if (ereg('^SWILicenseServerList=(.+)',$line,$regs))
            {
                $temp_list = explode(';',$regs[1]);
            }
        }
        fclose($fp);
        foreach ($temp_list as $list_item)
        {
            if (!Utils::is_blank($list_item))
            {
                list($port,$host) = explode('@',$list_item);
                $servers[] = array('host' => $host, 'port' => $port);
            }
        }
        return $servers;
    }
    
    public static function store_license_server_list($servers)
    {
        if (!is_array($servers))
            throw new Exception('Server list is not in an array');

        $fp = fopen(VoiceRecognitionManager::SPEECHWORKS_CONFIG_FILE,'r');
        if (!$fp)
            throw new Exception('Could not open the SpeechWorks configuration file');

            
        foreach ($servers as $s)
        {
            $formatted_s[] = trim($s['port'] . '@' . $s['host']);
        }
        $data = "";
        while (!feof($fp))
        {
            $line = fgets($fp);
            if (ereg('^SWILicenseServerList=',$line))
                $data .= "SWILicenseServerList=" . @implode(';',$formatted_s) . "\n";
            else
                $data .= $line;
        }        
        fclose($fp);
        
        $bytes = file_put_contents(VoiceRecognitionManager::SPEECHWORKS_CONFIG_FILE, $data);
        
        return ($bytes > 0);
    }
    
}

?>