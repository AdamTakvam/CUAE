<?php

require_once("constants.php");
require_once("lib.Resources.php");


class Utils
{

    public static function encrypt_password($password)
    {
        return base64_encode(md5($password, TRUE));
    }


    public static function extract_array_using_keys($keys, $array)
    {
        foreach ($keys as $key)
        {
            $new_array[$key] = $array[$key];
        }
        return $new_array;
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

    public static function execute($cmd)
    {
        // We briefly close sessions because it causes this bug: http://bugs.php.net/bug.php?id=22526
        session_write_close();
        exec($cmd, $output, $rval);
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
       if (file_exists($path . $exe)) {
            $oldpath = getcwd();
           chdir($path);
           if (substr(php_uname(), 0, 7) == "Windows"){
               pclose(popen("start \"bla\" \"" . $exe . "\" " . escapeshellarg($args), "r"));
           } else {
               exec("./" . $exe . " " . escapeshellarg($args) . " > /dev/null &");
           }
           chdir($oldpath);
       }
       else
            throw new Exception($path . $exe . ' could not be found.');
    }

    public static function get_first_array_key($array)
    {
        $keys = array_keys($array);
        return $keys[0];
    }


    public static function is_blank($string)
    {
        // PHP doesn't like empty(trim($string))
        return ("" == trim($string));
    }


    public static function is_pos_int($number, $strict = FALSE)
    {
        if ($strict)
            return (ctype_digit($number) && intval($number) > 0);
        else
            return (ctype_digit($number) && intval($number) >= 0);
    }
    
    public static function is_hex($string)
    {
        return eregi("^[a-f0-9]+$", $string);
    }


    public static function make_query($vars)
    {
        foreach ($vars as $k => $v)
        {
            $temp[] = $k . '=' . urlencode($v);
        }
        return '?' . implode('&',$temp);
    }


    public static function redirect($uri)
    {
        if (MceConfig::SHOW_SQL || MceConfig::SHOW_SQL_RS)
        {
            echo '<p><a href="' . $uri . '">Redirect to ' . $uri . '</a></p>';
        }
        else
        {
            header("Location: $uri");
        }
        exit(0);
    }


    public static function require_directory($dir)
    {
        $dh = opendir(MCE_CONSOLE_ROOT . "/includes/" . $dir);
        while (false !== ($file = readdir($dh)))
        {
            if (!in_array($file, array('.','..','CVS')))
            {
                require_once($dir . '/' . $file);
            }
        }
    }

    
    public static function require_post_var($var_name)
    {
        if (!isset($_POST[$var_name]))
            throw new Exception("Post variable '$var_name' is required.");
        else
            return $_POST[$var_name];
    }

    
    public static function require_req_var($var_name)
    {
        if (!isset($_REQUEST[$var_name]))
            throw new Exception("Request variable '$var_name' is required.");
        else
            return $_REQUEST[$var_name];
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
            {
                self::trim_array($array[$key]);
            }
            else
            {
                $array[$key]=trim($value);
            }
        }
    }


    public static function validate_ip($ip)
    {
        $nums = explode('.', $ip, 4);
        for ($i = 0; $i < 4; ++$i)
        {
            if (!is_numeric($nums[$i]) || intval($nums[$i]) < 0 || intval($nums[$i]) > 255)
            {
                return FALSE;
            }
        }
        return TRUE;
    }
    
    
    public static function validate_email($email)
    {
        throw new Exception("Utils::validate_email() deprecated.  Use Mail_RFC822::isValidInetAddress()");
    }
    

}

?>