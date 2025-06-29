<?php

require_once("common.php");
require_once(SMARTY_DIR . "Smarty.class.php");

class MceTemplate extends Smarty
{

    function __construct()
    {
        parent::Smarty();
        $this->template_dir = SMARTY_TEMPLATE_DIR;
        $this->compile_dir = SMARTY_TEMPLATE_C_DIR;
        $this->config_dir = SMARTY_CONFIG_DIR;
        $this->cache_dir = SMARTY_CACHE_DIR;

        if (MceConfig::SHOW_SMARTY)
        	$this->debugging = TRUE;
    }

}

?>