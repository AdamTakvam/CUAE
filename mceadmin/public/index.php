<?php

require_once("init.php");

$access = new AccessControl();
$errors = new ErrorHandler();

// If a user has submitted a log in
if ($_POST['submit'])
{
    if ($access->LogIn($_POST['username'], $_POST['passwd']))
    {
        MceUtils::is_app_server_running();    
        EventLog::log(LogMessageType::AUDIT, $_POST['username'] . " logged in", LogMessageId::USER_LOGIN);
        Utils::redirect("main.php");
    }
    else
    {
        EventLog::log(LogMessageType::AUDIT, $_POST['username'] . " failed to log in", LogMessageId::USER_LOGIN_FAILED);
        $errors->Add("That username and password combination is invalid.");
    }
}
// If the user's session is still good
else if ($access->CheckSession())
{
    MceUtils::is_app_server_running();
    Utils::redirect("main.php");
}

if ($_REQUEST['message'])
    $feedback = Utils::safe_unserialize($_REQUEST['message']);
if ($_REQUEST['expired'])
    $feedback = "We're sorry, but your session has expired.  Please log in again to start a new session.";
if ($_REQUEST['logout'])
    $feedback = "You have been logged out.";
    

// -- RENDER LOG IN PAGE --
    
$page = new Layout();
$page->SetPageTitle("Login");
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($feedback);
$page->SetFocusField("username");
$page->TurnOffNavigation();
$page->Display("login.tpl");

?>