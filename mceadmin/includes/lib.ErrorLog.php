<?php

require_once("config.php");


abstract class ErrorLog
{

    public static function log($errno, $errstr, $errfile, $errline, $errcontext)
    {
        // The case that error suppression (with @) is called, don't log the error
        if (error_reporting()==0)
        {
            return;
        }
    
        $message[] = "Error No: $errno";
        $message[] = "Description: $errstr";
        $message[] = "From file: $errfile";
        $message[] = "Line: $errline";
        ErrorLog::raw_log(implode("\n",$message));
    }

    public static function raw_log($message)
    {
        $time = date("[ H:i:s ]\n");
        $dir = LOGS_ROOT . MceConfig::CONSOLE_LOG_DIR;
        $file = $dir . "/" . date('Ymd') . ".log";
        if (!file_exists($dir))
            mkdir($dir, 0755);
        if (!file_exists($file))
        {
            touch($file);
            chmod($file, 0755);
        }
        $fp = fopen($file, 'a');
        fwrite($fp, $time . $message . "\n\n");
        fclose($fp);
    }

}

?>