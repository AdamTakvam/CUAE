<?php

require_once("common.php");
require_once("lib.ConfigUtils.php");
require_once(SMARTY_DIR . "Smarty.class.php");

class Layout
{

    public $mTemplate;
    protected $mHeadInfo;

    function __construct()
    {
        $this->mHeadInfo = array();
        $this->mTemplate = new Smarty();
        $this->mTemplate->template_dir = SMARTY_TEMPLATE_DIR;
        $this->mTemplate->compile_dir = SMARTY_TEMPLATE_C_DIR;
        $this->mTemplate->config_dir = SMARTY_CONFIG_DIR;
        $this->mTemplate->cache_dir = SMARTY_CACHE_DIR;

        if (MceConfig::SHOW_SMARTY) { $this->mTemplate->debugging = TRUE; }

        $this->mTemplate->assign('_navigation', TRUE);
        $this->mTemplate->assign('_f_version', ConfigUtils::get_mce_global_config(GlobalConfigNames::MCE_FIRMWARE_VERSION));
        $this->mTemplate->assign('_s_version', ConfigUtils::get_mce_global_config(GlobalConfigNames::MCE_SOFTWARE_VERSION));
        $this->mTemplate->assign('_r_type', ConfigUtils::get_mce_global_config(GlobalConfigNames::MCE_RELEASE_TYPE));
    }

    function AddHeadInfo($data)
    {
        if (is_array($data))
            { $this->mHeadInfo += $data; }
        else
            { $this->mHeadInfo[] = $data; }
    }

    function SetBreadcrumbs($bc_navigation)
    {
        $this->mTemplate->assign('_breadcrumbs', implode(' &gt; ', $bc_navigation));
    }

    function SetErrorMessage($message)
    {
        $this->mTemplate->assign('_error_message', trim($message));
    }

    function SetPageTitle($title)
    {
        $this->mTemplate->assign('_title', $title);
    }

    function SetResponseMessage($message)
    {
        $this->mTemplate->assign('_response_message', trim($message));
    }

    function TurnOffNavigation()
    {
        $this->mTemplate->assign('_navigation', FALSE);
    }

    function Display($template)
    {
        $this->mTemplate->assign('_head_extra', implode("\n", $this->mHeadInfo));
        $this->mTemplate->assign('_content_template', $template);
        $this->mTemplate->assign('_custom_logo', ConfigUtils::get_global_config(GlobalConfigNames::CUSTOM_LOGO_FILE));
        $this->mTemplate->display(MceConfig::LAYOUT_TEMPLATE);
    }

}

?>