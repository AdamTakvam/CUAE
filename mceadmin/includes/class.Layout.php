<?php

/* The Layout class handles the rendering of the pages of the management console and defines
 * the overall look and elements that are found on (almost) everypage.  Part of the Layout object
 * is the Template object ($mTemplate) which the programmer uses to set the actual content inside
 * of the main layout.
 */

require_once("common.php");
require_once("lib.SystemConfig.php");
require_once("lib.DateUtils.php");
require_once("lib.SystemUtils.php");
require_once("lib.RegistryUtils.php");
require_once("class.Template.php");
require_once("class.ServiceControl.php");

class Layout
{

/* PUBLIC MEMBERS */

    public $mTemplate;      // Template object (based on ADODB)

/* PRIVATE MEMBERS */

    private $mHeadInfo;     // Array of HTML tags to be added to the HEAD


/* PUBLIC MEMBERS */

    public function __construct()
    // Intialize the layout object, and retrieve elements that will be displayed
    // on the main template.
    {
        $this->mHeadInfo = array();
        $this->mTemplate = new Template();

        $this->mTemplate->assign('_navigation', TRUE);
        $this->mTemplate->assign('_f_version', SystemConfig::get_config(SystemConfig::FIRMWARE_VERSION));
        $this->mTemplate->assign('_s_version', SystemConfig::get_config(SystemConfig::SOFTWARE_VERSION));
        $this->mTemplate->assign('_r_type', SystemConfig::get_config(SystemConfig::RELEASE_TYPE));
        $this->mTemplate->assign('_dev_mode', MceConfig::DEV_MODE);
        $this->mTemplate->assign('_apache_needs_restart', SystemConfig::get_config(SystemConfig::APACHE_NEEDS_RESTART));
        $this->mTemplate->assign('_system_name', SystemUtils::get_hostname());
        
        $sdk_mode = TRUE;
        try
        {
			$os_ver = RegistryUtils::read_registry_value("HKLM\SOFTWARE\Cisco Systems, Inc.\System Info\OS Image","Version");
			if (strcmp($os_ver,"2003.1.1") >= 0)
				$sdk_mode = FALSE;
		}
		catch (Exception $ex)
		{
			// NULL ACTION
		}
		$this->mTemplate->assign('_sdk_mode', $sdk_mode);
    }

    public function AddHeadInfo($data)
    // Add tags that should be in the HEAD section of the HTML page
    {
        if (is_array($data))
            $this->mHeadInfo += $data;
        else
            $this->mHeadInfo[] = $data;
    }

    public function SetBreadcrumbs($bc_navigation)
    // Assign an array $bc_navigation of links that will form the breadcrumb navigation for that page
    {
        $this->mTemplate->assign('_breadcrumbs', implode(' &gt; ', $bc_navigation));
    }

    public function SetPageTitle($title)
    // Set the title of the page (will be put as a second header and the title bar of the browser)
    {
        $this->mTemplate->assign('_title', $title);
    }

    public function SetErrorMessage($message)
    // Set the message that will be displayed as an error message
    {
        $this->mTemplate->assign('_error_message', trim($message));
    }

    public function SetResponseMessage($message)
    // Set the message that will be displayed as a response notification
    {
        $this->mTemplate->assign('_response_message', trim($message));
    }

    public function SetFocusField($field)
    // Set a form field which will auto focus once the page is loaded
    {
        $this->mTemplate->assign('_focus_field', $field);
    }

    public function TurnOffNavigation()
    // Remove the navigation elements of the page.  Often this is convenient for pages
    // that simply tell the user something.
    {
        $this->mTemplate->assign('_navigation', FALSE);
    }

    public function Display($template)
    // Using $template as the template for the content of the page, generate the content
    // and insert it into the main template and display it to the user.
    {
        try
        {
            $app_server = new ServiceControl();
            $app_server->GetFromName(APP_SERVER_SERVICE_NAME);
            $this->mTemplate->assign('_app_server_enabled', $app_server->IsEnabled());
            if ($app_server->IsEnabled() || MceConfig::DEV_MODE)
                $this->mTemplate->assign('_app_server_on', MceUtils::is_app_server_running());
        }
        catch (Exception $ex)
        {
            $this->mTemplate->assign('_app_server_enabled', FALSE);
            $this->mTemplate->assign('_app_server_on', MceUtils::is_app_server_running());
        }

        $this->mTemplate->assign('_username', $_SESSION['user']);
        $this->mTemplate->assign('_head_extra', implode("\n", $this->mHeadInfo));
        $this->mTemplate->assign('_content_template', $template);
        $this->mTemplate->display(MceConfig::LAYOUT_TEMPLATE);
    }

}

?>