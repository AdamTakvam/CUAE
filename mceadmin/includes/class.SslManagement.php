<?php

require_once("config.sslmanagement.php");
require_once("lib.SystemConfig.php");
require_once("lib.Utils.php");


class SslManagement
{

    public function __construct()
    {
    }

    public function CreateCertificate($password, $years_expire, $attributes)
    {
        $openssl_exec           = SslManagementConfig::OPENSSL_PATH . '/' . SslManagementConfig::OPENSSL_EXEC;
        $openssl_config         = SslManagementConfig::OPENSSL_PATH . '/' . SslManagementConfig::CONFIG_FILE;
        $openssl_private_key    = SslManagementConfig::OPENSSL_PATH . '/' . SslManagementConfig::PRIVATE_KEY_FILE;
        $openssl_public_key     = SslManagementConfig::OPENSSL_PATH . '/' . SslManagementConfig::PUBLIC_KEY_FILE;
        $openssl_csr            = SslManagementConfig::OPENSSL_PATH . '/' . SslManagementConfig::CSR_FILE;
        $openssl_cert           = SslManagementConfig::OPENSSL_PATH . '/' . SslManagementConfig::CERT_FILE;
    
        // Set up the execute commands
        if ($years_expire < 1)
            $years_expire = 1;
        $password = strtr($password, "'\"", "__");
        
        $step_one = $openssl_exec . ' req -config ' . $openssl_config . 
                    ' -new -out ' . $openssl_csr . ' -keyout ' . $openssl_private_key;
        $step_two = $openssl_exec . ' rsa -in ' . $openssl_private_key . 
                    ' -out ' . $openssl_public_key .
                    ' -passin pass:"' . $password . '"';
        $step_three = $openssl_exec . ' x509 -in ' . $openssl_csr . 
                      ' -out ' . $openssl_cert . 
                      ' -req -signkey ' . $openssl_public_key . 
                      ' -days ' . (($years_expire * 365) + floor($years_expire / 4));
        
        // Write the OpenSSL config file with certificate data
        $this->WriteConfigFile($password, $attributes);
     

        $olddir = getcwd();
        chdir(SslManagementConfig::OPENSSL_PATH);
        // Execute the steps required to generate the CSR, key, and certificate
        Utils::execute($step_one);
        if (!file_exists($openssl_csr))
            throw new Exception('Failed to create CSR file');
            
        Utils::execute($step_two);
        if (!file_exists($openssl_public_key))
            throw new Exception('Failed to create key');
        
        Utils::execute($step_three);
        if (!file_exists($openssl_cert))
            throw new Exception('Failed to create self-signed certificate');
        
        // Move certificate and key for Apache to use
        $this->MakeApacheSslDirectory();
            
        copy($openssl_cert, SslManagementConfig::APACHE_SSL_PATH . '/' . SslManagementConfig::CERT_FILE);
        copy($openssl_public_key, SslManagementConfig::APACHE_SSL_PATH . '/' . SslManagementConfig::PUBLIC_KEY_FILE);
       
        // Clean Up
        unlink($openssl_config);
        @unlink(SslManagementConfig::OPENSSL_PATH . '/.rnd');
        chdir($olddir);
        EventLog::log(LogMessageType::AUDIT, 'SSL Certificate and Key generated', LogMessageId::SSL_CERTIFICATE_GENERATED, print_r($attributes,TRUE));
        return TRUE;
    }
    
    public function VerifyCertificate($file)
    {
        try
        {
            Utils::execute(SslManagementConfig::OPENSSL_PATH . '/' . SslManagementConfig::OPENSSL_EXEC . ' verify -purpose sslserver ' . $file);
        }
        catch (Exception $e)
        {
            return FALSE;
        }
        return TRUE;
    }
    
    public function VerifyKey($file)
    {
        try
        {
            Utils::execute(SslManagementConfig::OPENSSL_PATH . '/' . SslManagementConfig::OPENSSL_EXEC . ' rsa -check -in ' . $file);
        }
        catch (Exception $e)
        {
            return FALSE;
        }
        return TRUE;
    }
    
    public function VerifyMatch($certificate, $key)
    {
        try
        {
            $cert_mod = Utils::execute(SslManagementConfig::OPENSSL_PATH . '/' . SslManagementConfig::OPENSSL_EXEC . ' x509 -noout -modulus -in "' . $certificate . '"');
            $key_mod = Utils::execute(SslManagementConfig::OPENSSL_PATH . '/' . SslManagementConfig::OPENSSL_EXEC . ' rsa -noout -modulus -in "' . $key . '"');
            return ($cert_mod == $key_mod);
        }
        catch (Exception $e)
        {
            return FALSE;
        }
    }
    
    public function SetCertificate($file)
    {
        $this->MakeApacheSslDirectory();
        if ($file != SslManagementConfig::APACHE_SSL_PATH . '/' . SslManagementConfig::CERT_FILE)
        {
            if (!copy($file, SslManagementConfig::APACHE_SSL_PATH . '/' . SslManagementConfig::CERT_FILE))
                return FALSE;
            @unlink(SslManagementConfig::OPENSSL_PATH . '/' . SslManagementConfig::CSR_FILE);
            EventLog::log(LogMessageType::AUDIT, 'SSL Certificate Uploaded', LogMessageId::SSL_CERTIFICATE_UPLOADED);
        }
        return TRUE;
    }
    
    public function SetKey($file)
    {
        $this->MakeApacheSslDirectory();
        if ($file != SslManagementConfig::APACHE_SSL_PATH . '/' . SslManagementConfig::PUBLIC_KEY_FILE)
        {
            if (!copy($file, SslManagementConfig::APACHE_SSL_PATH . '/' . SslManagementConfig::PUBLIC_KEY_FILE))
                return FALSE;
            @unlink(SslManagementConfig::OPENSSL_PATH . '/' . SslManagementConfig::CSR_FILE);
            EventLog::log(LogMessageType::AUDIT, 'SSL Key Uploaded', LogMessageId::SSL_KEY_UPLOADED);
        }
        return TRUE;
    }
    
    public function CertificateExists()
    {
        return file_exists(SslManagementConfig::APACHE_SSL_PATH . '/' . SslManagementConfig::CERT_FILE);
    }
    
    public function KeyExists()
    {
        return file_exists(SslManagementConfig::APACHE_SSL_PATH . '/' . SslManagementConfig::PUBLIC_KEY_FILE);
    }
    
    public function CsrExists()
    {
        return file_exists(SslManagementConfig::OPENSSL_PATH . '/' . SslManagementConfig::CSR_FILE);
    }
    
    public function GetCsr()
    {
        if ($this->CsrExists())
            return file_get_contents(SslManagementConfig::OPENSSL_PATH . '/' . SslManagementConfig::CSR_FILE);
        else
            return NULL;
    }
    
    public function IsSslEnabled()
    {
        return file_exists(METREOS_ROOT . SslManagementConfig::APACHE_CONFIG_PATH . '/' . basename(SslManagementConfig::APACHE_SSL_CONFIG));
    }
    
    public function EnableSsl()
    {
        if (!($this->CertificateExists() && $this->KeyExists()))
            throw new Exception('An SSL certificate and key must be generated before enabling SSL');
        
        copy(MCE_CONSOLE_ROOT . '/' . SslManagementConfig::APACHE_SSL_CONFIG, METREOS_ROOT . SslManagementConfig::APACHE_CONFIG_PATH . '/' . basename(SslManagementConfig::APACHE_SSL_CONFIG));
        SystemConfig::store_config(SystemConfig::APACHE_NEEDS_RESTART, 1);
        EventLog::log(LogMessageType::AUDIT, 'SSL Enabled', LogMessageId::SSL_ENABLED);
    }
    
    public function DisableSsl()
    {
        @unlink(METREOS_ROOT . SslManagementConfig::APACHE_CONFIG_PATH . '/' . basename(SslManagementConfig::APACHE_SSL_CONFIG));
        SystemConfig::store_config(SystemConfig::APACHE_NEEDS_RESTART, 1);
        EventLog::log(LogMessageType::AUDIT, 'SSL Disabled', LogMessageId::SSL_DISABLED);
    }
    
    private function MakeApacheSslDirectory()
    {
        if (!file_exists(SslManagementConfig::APACHE_SSL_PATH))
            return mkdir(SslManagementConfig::APACHE_SSL_PATH);
        return TRUE;
    }
    
    private function WriteConfigFile($password, $attributes)
    {
        $config_file = SslManagementConfig::OPENSSL_PATH . '/' . SslManagementConfig::CONFIG_FILE;
        copy(MCE_CONSOLE_ROOT . '/' . SslManagementConfig::CONFIG_FILE_BASE, $config_file);
        
        $escape_chars = "$#{}";
        $amendment[] = "output_password     = " . addcslashes($password, $escape_chars);
        $amendment[] = "";
        $amendment[] = "[ req_distinguished_name ]";
        
        foreach ($attributes as $key => $val)
        {
            $amendment[] = "$key        = " . addcslashes($val, $escape_chars);
        }
        
        $amendment[] = "";
        $amendment[] = "[ req_attributes ]";
        
        $fp = fopen($config_file, 'a');
        if (!$fp)
            throw new Exception("Could not open OpenSSL configuration file: " . $config_file);
        $bytes = fwrite($fp, implode("\n", $amendment));
        fclose($fp);
        
        if ($bytes <= 0)
            throw new Exception("Could not write to OpenSSL configuration file: " . $config_file);
        else
            return TRUE;
    }

}

?>