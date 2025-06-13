<?php

/* The AppServerInterface class conducts the communication with the application server
 * via a socket connection.  It handles the process of formatting the message such that
 * the application server understands, and retrieves and parses the response of the server.
 */

require_once("common.php");

class AppServerInterface
{

/* PRIVATE MEMBERS */

    private $mConn;         // Socket stream object
    private $mError;        // Indicates error state of the object


/* PUBLIC METHODS */

    public function __construct()
    // Initialize object
    {
        $this->mConn = NULL;
        $this->mError = NULL;
        $this->Connect();
    }

    public function Connected()
    // Check the state of the connection
    {
        return $this->mConn ? TRUE : FALSE;
    }

    public function SetTimeout($seconds)
    // Set the timeout for waiting for a response to $seconds seconds
    {
        if ($seconds > 0)
            return stream_set_timeout($this->mConn, $seconds);
        else
            return FALSE;
    }

    public function Send($message)
    // Sends $message to the application server and waits for a response.  It parses
    // the response and reports any errors from the application server.
    {
        if (!$this->mConn)
            $this->Connect();

        if ($this->mConn)
        {
            try
            {
                // Send the message preceded with a 4-byte "header" describing the size of the message
                $msg_size_header = pack("L", strlen($message));
                $full_msg = $msg_size_header . $message;
                if (fwrite($this->mConn, $full_msg))
                {
                    // Get the size header of the response message
                    $sbytes = Utils::read_bytes($this->mConn, MceConfig::APP_MESSAGE_LENGTH_SIZE);
                    $unpacked = unpack("Lsize", $sbytes);
                    $msg_size = $unpacked['size'];
                    // Get the message itself
                    $response = Utils::read_bytes($this->mConn, $msg_size);
                    $xml_parse = xml_parser_create();
                    xml_parse_into_struct($xml_parse, $response, $values, $index);
                    xml_parser_free($xml_parse);
                    $response_id = $index['RESPONSE'][0];
                    if (strtolower($values[$response_id]['attributes']['TYPE']) == "success")
                        return TRUE;
                    else
                        $this->mError = "Application server failed to perform action.\nResponse: " . $values[$response_id]['value'];
                }
            }
            catch (Exception $e)
            {
                $this->mError = "There was no response message from the Application server.  Results are unknown.";
                if (MceConfig::DEV_MODE)
                {
                    $this->mError .= "\nMessage sent:\n<pre>" . htmlspecialchars($message) . "</pre>";
                    $this->mError .= "\nResponse data:\n<pre>" . $e->getMessage() . "</pre>";
                }
            }

        }
        return FALSE;
    }

    public function Error()
    // Returns the error state of the object
    {
        return $this->mError ? $this->mError : FALSE;
    }


/* PRIVATE METHODS */

    private function Connect()
    // Attempt to create/retrieve the socket stream object
    {
        try
        {
            $this->mConn = Resources::get_socket();
            stream_set_blocking($this->mConn, TRUE);
            stream_set_timeout($this->mConn, MceConfig::APP_SERVER_WAIT);
        }
        catch (Exception $e)
        {
            $this->mError = "Failed to connect to the Application Server.\n";
            $this->mError .= "Socket Error " . $e->GetCode() . ": " . $e->GetMessage() . "\n";
            $this->mError .= "Try restarting the Application server.";
            $_SESSION['app_server_running'] = FALSE;
        }
    }

}

?>