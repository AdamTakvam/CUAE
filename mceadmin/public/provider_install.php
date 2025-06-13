<?php

require_once("init.php");

$errors = new ErrorHandler();

if ($_POST['upload'])
{
    if (!Utils::is_blank($_FILES['providerpackage']['name']))
    {
        $destination = getcwd() . '\\' . $_FILES['providerpackage']['name'];
        $asi = new AppServerInterface();
        if ($asi->Connected())
        {
            try
            {
                move_uploaded_file($_FILES['providerpackage']['tmp_name'], $destination);
                $output = Utils::execute_with_cmd("\"" . FRAMEWORK_ROOT . MceConfig::PROVIDER_INSTALLER . "\" \"" . substr($destination,2) . "\"");
                @unlink($destination);
                $response = "The provider was installed.";
                EventLog::log(LogMessageType::AUDIT, $response, LogMessageId::PROVIDER_INSTALLED, print_r($_FILES, TRUE));
            }
            catch (Exception $e)
            {
                $errors->Add('Could not install the provider.  The provider package may not be valid.');
                @unlink($destination);
                if (MceConfig::DEV_MODE)
                    $errors->Add($e->GetMessage());
                else
                    ErrorLog::raw_log(print_r($e, TRUE));
                EventLog::log(LogMessageType::AUDIT, 'Provider installation failed', LogMessageId::PROVIDER_INSTALLED_FAILED, print_r($e, TRUE), Severity::YELLOW);
            }
        }
        else
        {
            $errors->Add('Could not communicate with the application server.  Please try restarting the application server.');
        }
    }
    else
    {
        $errors->Add('No provider package was uploaded');
    }
}

if ($errors->IsEmpty())
    Utils::redirect("component_list.php?type=" . ComponentType::PROVIDER . "&s_response=" . Utils::safe_serialize($response));
else
    Utils::redirect("component_list.php?type=" . ComponentType::PROVIDER . "&s_errors=" . Utils::safe_serialize($errors));

?>