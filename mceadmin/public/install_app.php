<?php

require_once("init.php");

$db = new MceDb();
$errors = new ErrorHandler();


// ** INSTALL/UPDATE APPLICATION **

if (eregi("mca$", $_FILES['mca_file']['name']))
{
    $source = $_FILES['mca_file']['tmp_name'];
    $destination = APP_SERVER_ROOT . MceConfig::APP_INSTALL_DIRECTORY . '/' . $_FILES['mca_file']['name'];
    if (move_uploaded_file($source, $destination))
    {
        $asi = new AppServerInterface();
        if ($asi->Connected())
        {
            $asi->SetTimeout(MceConfig::APP_SERVER_INSTALL_WAIT);

            if ($_POST['install'])
            {
                $command = 'InstallApplication';
                $params['ComponentName'] = $_FILES['mca_file']['name'];
                $xml_command = MceUtils::generate_xml_command($command, $params);
                $asi->Send($xml_command);
                if ($asi->Error())
                    $errors->Add("Application install failed.\nReason: " . $asi->Error());
                else
                    $response = "The application was installed successfully.";
            }
            else if ($_POST['update'])
            {
                Utils::require_post_var('application_id');
                $db = new MceDb();
                $comp_name = $db->GetOne("SELECT name FROM mce_components WHERE mce_components_id = ?", array($_POST['application_id']));
                $command = 'UpdateApplication';
                $params['ComponentName'] = $_FILES['mca_file']['name'];
                $params['ApplicationName'] = $comp_name;
                $xml_command = MceUtils::generate_xml_command($command, $params);
                $asi->Send($xml_command);
                if ($asi->Error())
                    $errors->Add("Application update failed.\nReason: " . $asi->Error());
                else
                    $response = "The application was updated successfully.";
            }
        }
        else
        {
            $response = "The application will be installed when the application server starts.";
        }

        if ($errors->IsEmpty())
            EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::APPLICATION_INSTALLED, print_r($_FILES, TRUE));
        else
            EventLog::log(LogMessageType::AUDIT, 'Application install failed', LogMessageId::APPLICATION_INSTALL_FAILED, $errors->Dump(), Severity::RED);		
    }
    else
    {
        $errors->Add('Could not install application.');
    }
}
else
{
    $errors->Add('Uploaded file is not an MCA application file.');
}


// ** REDIRECT USER BACK TO APPROPIATE PAGE **

if ($_POST['install'])
    $return_url = "component_list.php?type=" . ComponentType::APPLICATION;
else if ($_POST['update'])
    $return_url = "edit_app.php?id=" . $_POST['application_id'] . "&type=" . ComponentType::APPLICATION;

if ($errors->IsEmpty())
    Utils::redirect($return_url . "&s_response=" . Utils::safe_serialize($response));
else
    Utils::redirect($return_url . "&s_errors=" . Utils::safe_serialize($errors));

?>