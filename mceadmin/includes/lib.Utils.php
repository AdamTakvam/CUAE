<?php

require_once("constants.php");


abstract class Utils
{

    public static function require_post_var($var_name)
    {
        if (!isset($_REQUEST[$var_name]))
            throw new Exception("Post variable '$var_name' is required.");
    }

    public static function require_req_var($var_name)
    {
        if (!isset($_REQUEST[$var_name]))
            throw new Exception("Request variable '$var_name' is required.");
    }

    public static function file_size($file)
    {
        if (is_dir($file))
        {
            $size = 0;
            $dh = opendir($file);
            while (false !== ($afile = readdir($dh)))
            {
                if (!in_array($afile, array('.','..')))
                {
                    $afilename = $file . "/" . $afile;
                    $size += Utils::file_size($afilename);
                }
            }
            closedir($dh);
            return $size;
        }
        else
            return filesize($file);
    }


    public static function encrypt_password($password)
    {
        return base64_encode(md5($password, TRUE));
    }


    public static function execute($cmd)
    {
        // We briefly close sessions because it causes this bug: http://bugs.php.net/bug.php?id=22526
        session_write_close();
        exec($cmd . " 2>&1", $output, $rval);
        session_start();
        $output = implode("\n", $output);
        if ($rval <> 0)
        {
            $message  = "Executing system command failed: $cmd\n";
            $message .= "Output: $output\n";
            $message .= "Return Value: $rval";
            throw new Exception($message);
        }
        return $output;
    }

    public static function execute_with_cmd($cmd)
    {
        // This one is better than execute() when the path to the executable contains spaces.
        // Maybe we should deprecate the one above anyways and just use this one for everything?
        // We briefly close sessions because it causes this bug: http://bugs.php.net/bug.php?id=22526
        session_write_close();
        exec("cmd /C \"$cmd\" 2>&1", $output, $rval);
        session_start();
        $output = implode("\n", $output);
        if ($rval <> 0)
        {
            $message  = "Executing system command failed: $cmd\n";
            $message .= "Output: $output\n";
            $message .= "Return Value: $rval";
            throw new Exception($message);
        }
        return $output;    
    }

    public static function background_execute($path, $exe, $args = "") {
       if (file_exists($path . '/' . $exe)) {
            $oldpath = getcwd();
           chdir($path);
           if (substr(php_uname(), 0, 7) == "Windows")
               pclose(popen("start \"Management\" /D\"" . $path . "\" \"" . $exe . "\" " . escapeshellarg($args), "r"));
           else
               exec("./" . $exe . " " . escapeshellarg($args) . " > /dev/null &");
           chdir($oldpath);
       }
       else
            throw new Exception($path . $exe . ' could not be found.');
    }


    public static function extract_array_using_keys($keys, $array)
    {
        foreach ($keys as $key)
        {
            $new_array[$key] = $array[$key];
        }
        return $new_array;
    }


    public static function get_first_array_key($array)
    {
        if (is_array($array))
        {
            $keys = array_keys($array);
            return $keys[0];
        }
        return null;
    }


    public static function get_month_name($number)
    {
        return date('F', mktime(0,0,0,$number,1,80));
    }


    public static function is_blank($string)
    {
        return (strlen(trim($string)) == 0);
    }


    public static function is_hex($string)
    {
        return eregi("^[a-f0-9]+$", $string);
    }


    public static function is_pos_int($number, $strict = FALSE)
    {
        if ($strict)
            return (ctype_digit($number) && intval($number) > 0);
        else
            return (ctype_digit($number) && intval($number) >= 0);
    }

    public static function big_dechex($bigNumber)
    {
        if ($bigNumber > hexdec('FFFFFFFF'))
        {
            // The problem is dechex() cannot handle converting to hex numbers larger than
            // eight 'digits'.  Doing so causes the meter to rollover, so to speak.
            // i.e. what should be F00000001 is returned as 00000001
            // So this supports up to 16 'digits'
            $first_eight = dechex($bigNumber);
            $leftover = $bigNumber - hexdec($first_eight);
            $adjusted_leftover = $leftover / pow(16,8); 
            $last_eight = dechex($adjusted_leftover);
            return $last_eight . str_pad($first_eight, 8, '0', STR_PAD_LEFT);
        }
        return dechex($bigNumber);
    }
    
    public static function make_query($vars)
    {
        foreach ($vars as $k => $v)
        {
            $temp[] = $k . '=' . urlencode($v);
        }
        return '?' . implode('&',$temp);
    }


    public static function read_bytes($resource, $bytes)
    {
        $buffer = "";
        $start_time = time();
        $bytes_read = 0;
        while ($bytes_read < $bytes)
        {
            if ((time() - $start_time) > MceConfig::APP_SERVER_WAIT)
            {
                $data['time_length'] = time() - $start_time;
                $data['bytes_to_read'] = $bytes;
                $data['bytes_read'] = $bytes_read;
                $data['buffer_read'] = $buffer;
                throw new Exception(print_r($data, TRUE));
            }
            $buffer .= fread($resource, $bytes - $bytes_read);
            $bytes_read = strlen($buffer);
        }
        return $buffer;
    }


    public static function redirect($uri)
    {
        if (MceConfig::SHOW_SQL || MceConfig::SHOW_SQL_RS)
            echo '<p><a href="' . $uri . '">Redirect to ' . $uri . '</a></p>';
        else
            header("Location: $uri");
        exit(0);
    }


    public static function require_directory($dir)
    {
        $dh = opendir(MCE_CONSOLE_ROOT . "/includes/" . $dir);
        while (false !== ($file = readdir($dh)))
        {
            if (!in_array($file, array('.','..','.svn','CVS')))
                require_once($dir . '/' . $file);
        }
    }


    public static function safe_serialize($object)
    {
        return urlencode(base64_encode(serialize($object)));
    }


    public static function safe_unserialize($object)
    {
        return unserialize(base64_decode(urldecode($object)));
    }


    public static function trim_array(&$array)
    {
        foreach ($array as $key=>$value)
        {
            if (is_array($array[$key]))
                self::trim_array($array[$key]);
            else
                $array[$key] = trim($value);
        }
    }


    public static function validate_ip($ip)
    {
        if (empty($ip))
            return FALSE;
        $nums = explode('.', $ip, 4);
        for ($i = 0; $i < 4; ++$i)
        {
            if (!is_numeric($nums[$i]) || intval($nums[$i]) < 0 || intval($nums[$i]) > 255)
                return FALSE;
        }
        return TRUE;
    }

    public static function validate_mac_address($mac)
    {
        return eregi("^([a-f0-9]{2}:){5}[a-f0-9]{2}$", $mac);
    }
    
}

?>