<?php

/* The ComponentPageHandler contains commonly taken actions required to update a component
 * and its configurations.  You assign a componet to it, and it handles updating, uninstalling,
 * and the display of the configurations.  It requires a component and, ideally, an ErrorHandler
 * object.
 */

require_once("init.php");
require_once("components/class.Component.php");

class ComponentPageHandler
{

/* PROTECTED MEMBERS */

    protected $mErrorHandler;
    protected $mComponent;
    protected $mConfigs;
    protected $mTemplateVars;
    protected $mPage;
    protected $mActionHandlerCalls;
    protected $mValidateCalls;
    protected $mUpdateCalls;


/* PUBLIC METHODS */

    public function __construct()
    // Initialize the page handler
    {
        $this->mErrorHandler = NULL;
        $this->mComponent = NULL;
        $this->mConfigs = NULL;
        $this->mTemplateVars = array();
        $this->mPage = new Layout();
        $this->mActionHandlerCalls = array();
        $this->mValidateCalls = array();
        $this->mUpdateCalls = array();
    }

    public function SetErrorHandler(ErrorHandler $eh)
    // Set the ErrorHandler object for the handler
    {
        $this->mErrorHandler = $eh;
        if (NULL != $this->mComponent)
            $this->mComponent->SetErrorHandler($this->mErrorHandler);
        return TRUE;
    }

    public function SetComponent(Component $comp)
    // Set the component for the handler
    {
        $this->mComponent = $comp;
        if (NULL != $this->mErrorHandler)
            $this->mComponent->SetErrorHandler($this->mErrorHandler);
        $this->mConfigs = $this->mComponent->GetConfigs();
        return TRUE;
    }

    public function IgnoreConfig($config_name)
    {
    // Tell the page handler to ignore a certain configuration
        if (is_null($this->mConfigs))
        {
            throw new Exception("Can't call IgnoreConfig() before assigning a component to the page handler");
        }
        unset($this->mConfigs[$config_name]);
        return TRUE;
    }
    
    public function AddActionHandlerFunction($function, $params)
    // Adds a user-defined function to be executed when actions are handled
    // Uses php's call_user_func_array() arguments
    {
        if (!function_exists($function))
            throw new Exception("function $function does not exist");
        if (!is_array($params))
            throw new Exception("params argument is not an array");
        $this->mActionHandlerCalls[] = array('function' => $function, 'parameters' => $params);
    }

    public function AddValidateFunction($function, $params)
    // Adds a user-defined function to be executed when validating values before an update
    // Uses php's call_user_func_array() arguments
    {
        if (!function_exists($function))
            throw new Exception("function $function does not exist");
        if (!is_array($params))
            throw new Exception("params argument is not an array");
        $this->mValidateCalls[] = array('function' => $function, 'parameters' => $params);
    }

    public function AddUpdateFunction($function, $params)
    // Adds a user-defined function to be executed when updating config values
    // Uses php's call_user_func_array() arguments
    {
        if (!function_exists($function))
            throw new Exception("function $function does not exist");
        if (!is_array($params))
            throw new Exception("params argument is not an array");
        $this->mUpdateCalls[] = array('function' => $function, 'parameters' => $params);
    }
    
    public function HandleActions()
    // Handles the typical user requested actions for the component
    {
        // Call custom developer action handler functions
        foreach ($this->mActionHandlerCalls as $ah_call)
        {
            call_user_func_array($ah_call['function'], $ah_call['parameters']);
        }

        if ($_POST['cancel'])
            Utils::redirect($this->mComponent->mListPage);

        if ($_POST['uninstall'])
            Utils::redirect("delete_component.php?id=" . $this->mComponent->GetId() . "&type=" . $this->mComponent->GetType());

        if ($_POST['update'])
        {
            // Validate the component configs
            foreach ($this->mConfigs as $key => $cfg_object)
            {
                $cfg_object->Validate($_POST[$key]);
            }

            // Call developer custom component validation functions
            foreach ($this->mValidateCalls as $v_call)
            {
                call_user_func_array($v_call['function'], $v_call['parameters']);
            }

            if ($this->mErrorHandler->IsEmpty())
            {
                $db = new MceDb();
                $db->StartTrans();
                // Update the component configs
                foreach ($this->mConfigs as $key => $cfg_object)
                {
                    if (isset($_POST[$key]))
                    {
                        $cfg_object->Update($_POST[$key]);
                        if ($cfg_object->GetType() != FormatType::CONFIG_PASSWORD)
                            $update_values[$key] = $_POST[$key];
                    }
                }

                // Call developer custom component updating functions
                foreach ($this->mUpdateCalls as $u_call)
                {
                    call_user_func_array($u_call['function'], $u_call['parameters']);
                }

                // Log the update
                $c_type = ComponentType::display($this->mComponent->GetType());
                $c_name = $this->mComponent->GetName();
                EventLog::log(LogMessageType::AUDIT, "Values for $c_type $c_name were updated.", LogMessageId::COMPONENT_CONFIG_MODIFIED, print_r($update_values, TRUE));

                $db->CompleteTrans();

                // Tell the application server to refresh
                if (MceUtils::is_app_server_running())
                {
                    if ($this->mComponent->Refresh())
                        $this->mPage->SetResponseMessage("The configuration has been updated.");
                }
                else
                {
                    $this->mPage->SetResponseMessage("The configuration has been updated and will be recognized by the application server when it restarts.");
                }
            }
        }
    }

    public function AddTemplateVar($name, $vars)
    // Add a template variable to be displayed into the template (Smarty style)
    {
        $this->mTemplateVars[$name] = $vars;
    }

    public function AddTemplateVars($vars)
    // Add a collection of template variables to the template (Smarty style)
    {
        if (is_array($vars))
            $this->mTemplateVars += $vars;
        else
            throw new Exception("Passed in template variables not an array.");
    }

    public function SetResponseMessage($message)
    // Set the response message a user would see on the display page.
    // Ideal for notification associated with actions handled outside of the page handler.
    {
        $this->mPage->SetResponseMessage($message);
    }

    public function Display($template_file)
    // Process the variables, component, and template to generate the page for editing the
    // component.
    {
        // Get the component and determine the breadcrumbs
        $c_type = $this->mComponent->GetType();
        switch (ComponentUtils::determine_group_type($c_type))
        {
            case GroupType::ALARM_GROUP :
                $breadcrumb_display = "Alarm Management";
                break;
            case GroupType::SCCP_GROUP :
            case GroupType::CTI_SERVER_GROUP :
                $breadcrumb_display = "Unified Communications Manager";
                break;
            case GroupType::SIP_GROUP :
                $breadcrumb_display = "SIP Domain";
                break;
            case GroupType::H323_GATEWAY_GROUP :
            case GroupType::TEST_IPT_GROUP :
                $breadcrumb_display = "Telephony Servers";
                break;
            case GroupType::MEDIA_RESOURCE_GROUP :
                $breadcrumb_display = "Media Engines";
                break;
            default:
                $breadcrumb_display = ComponentType::describe($c_type) . 's';
        }

        // Go through the component configurations and put the info in an array
        foreach ($this->mConfigs as $key => $config_obj)
        {
            $config_data[] = array( 'display_name'      => $config_obj->GetDisplayName(),
                                    'fields'            => $this->mErrorHandler->IsEmpty() ? $config_obj->GetFields() : $config_obj->GetFields($_POST[$key]),
                                    'description'       => $config_obj->GetDescription(),
                                    'meta_description'  => $config_obj->GetMetaDescription(),
                                  );

        }
        // Gather the template variables
        $tpl_vars = array(  'id' => $this->mComponent->GetId(),
                            'type' => $this->mComponent->GetType(),
                            'name' => $this->mComponent->GetName(),
                            '_metadata' => $this->mComponent->GetMetaData(),
                            '_configs' => $config_data);
        $tpl_vars += $this->mTemplateVars;

        // Create breadcrumbs
        if ($c_type == ComponentType::RTP_RELAY)
            $breadcrumbs = array( '<a href="main.php">Main Control Panel</a>','RTP Relay');
        else
            $breadcrumbs = array( '<a href="main.php">Main Control Panel</a>',
                                  '<a href="' . $this->mComponent->mListPage . '">' . $breadcrumb_display . '</a>',
                                  htmlspecialchars($this->mComponent->GetName()));

        // Finally, parse the content into the main layout and display
        $this->mPage->SetPageTitle($this->mComponent->GetName());
        $this->mPage->SetBreadcrumbs($breadcrumbs);
        $this->mPage->SetErrorMessage($this->mErrorHandler->Dump());
        $this->mPage->mTemplate->assign($tpl_vars);
        $this->mPage->Display($template_file);
    }

}

?>