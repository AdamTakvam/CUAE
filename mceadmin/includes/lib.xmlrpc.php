<?php
// lib.xmlrpc.php
//
// XML-RPC Service
//
function _instanceHandlerFunction($method, $params, $objInstance) {
    $a = call_user_func_array(array(&$objInstance, $method), $params);
    return $a;
}

function XMLRPCService($request, $objInstance, $classname='') {
    $server = xmlrpc_server_create();
    if ($server) {
        foreach (get_class_methods(get_class($objInstance)) as $method) {
            # Ignore '_<name>' methods
            if (preg_match("/^_/", $method)) {
                continue;
            }
            # If a <classname> is provided, prepend to the <method> name 
            if ($classname != '') {
                $name = $classname . "." . $method;
            } else {
                $name = $method;
            }
            xmlrpc_server_register_method($server, $name, "_instanceHandlerFunction");
        }
        $response = xmlrpc_server_call_method($server, $request, $objInstance, array('output_type' => 'xml'));
        header("Content-type: text/xml");
        echo $response;
        xmlrpc_server_destroy($server);
    }
}

?>
