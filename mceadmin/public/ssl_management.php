<?php

require_once("init.php");
require_once("class.SslManagement.php");


// ** Set up **

$access = new AccessControl();
$access->CheckPageAccess(AccessControl::ADMINISTRATOR);

$errors = new ErrorHandler();
$ssl = new SslManagement();


// ** Handle user requests **

if ($_POST['enable_ssl'])
{
    $ssl->EnableSsl();
    $feedback = "SSL configuration for the webserver has been added.  Please restart the webserver for the changes to take effect.";
}

if ($_POST['disable_ssl'])
{
    $ssl->DisableSsl();
    $feedback = "SSL configuration for the webserver has been removed.  Please restart the webserver for the changes to take effect.";
}

// Uploaded certificate and/or key

if ($_POST['upload'])
{
    $feedback = "";
    $cert_uploaded = FALSE;
    $key_uploaded = FALSE;
    if ($_FILES['cert_file']['size'] > 0)
    {
        $cert_uploaded = TRUE;
        if (!$ssl->VerifyCertificate($_FILES['cert_file']['tmp_name']))
            $errors->Add("The SSL certificate is not valid");
    }
    if ($_FILES['key_file']['size'] > 0)
    {
        $key_uploaded = TRUE;
        if (!$ssl->VerifyKey($_FILES['key_file']['tmp_name']))
            $errors->Add("The SSL key is not valid");
    }
    if ($errors->IsEmpty())
    {
        $cert = $cert_uploaded ? $_FILES['cert_file']['tmp_name'] : SslManagementConfig::APACHE_SSL_PATH . '/' . SslManagementConfig::CERT_FILE;
        $key = $key_uploaded ? $_FILES['key_file']['tmp_name'] : SslManagementConfig::APACHE_SSL_PATH . '/' . SslManagementConfig::PUBLIC_KEY_FILE;
        if ($ssl->VerifyMatch($cert, $key))
        {
            $feedback = "";
            if ($cert_uploaded)
            {
                $ssl->SetCertificate($cert);
                $feedback .= "New certificate uploaded. ";
            }
            if ($key_uploaded)
            {
                $ssl->SetKey($key);
                $feedback .= "New key uploaded.";
            }
        }
        else
        {
            $errors->Add("The certificate and the key are not compatible.");
        }
    }
}

if ($_POST['generate'])
{
    if (Utils::is_blank($_POST['passphrase']))
        $errors->Add("The passphrase is required");
    if (Utils::is_blank($_POST['organization']))
        $errors->Add("Organization name is required");
    if (Utils::is_blank($_POST['country']))
        $errors->Add("Country is required");
    if (Utils::is_blank($_POST['state']))
        $errors->Add("State/Province is required");
    if (Utils::is_blank($_POST['locality']))
        $errors->Add("City/Locality is required");
    if (Utils::is_blank($_POST['common_name']))
        $errors->Add("Domain/Common name is required");
    if (Utils::is_blank($_POST['email']))
        $errors->Add("E-mail Address is required");
    if (intval($_POST['years_expire']) < 1)
        $errors->Add("Years Until Expire must be greate than zero");
        
    if ($errors->IsEmpty())
    {
        $attributes['C']    = $_POST['country'];
        $attributes['ST']   = $_POST['state'];
        $attributes['L']    = $_POST['locality'];
        $attributes['O']    = $_POST['organization'];
        if (!Utils::is_blank($_POST['organizational_unit']))
            $attributes['OU']   = $_POST['organizational_unit'];
        $attributes['CN']   = $_POST['common_name'];
        $attributes['emailAddress'] = $_POST['email'];
        
        $ssl->CreateCertificate($_POST['passphrase'], $_POST['years_expire'], $attributes);
        $feedback = "A new SSL key and self-signed certificate was generated";
    }
}


// ** Render page **

$title = "SSL Management";
$breadcrumbs[] = '<a href="main.php">Main Control Panel</a>';
$breadcrumbs[] = $title;

$page = new Layout();
$page->SetPageTitle($title);
$page->SetBreadcrumbs($breadcrumbs);
$page->SetErrorMessage($errors->Dump());
$page->SetResponseMessage($feedback);
$page->mTemplate->Assign('cert_exists',$ssl->CertificateExists());
$page->mTemplate->Assign('key_exists',$ssl->KeyExists());
$page->mTemplate->Assign('csr_exists',$ssl->CsrExists());
$page->mTemplate->Assign('ssl_enabled',$ssl->IsSslEnabled());
$page->mTemplate->Assign('csr',$ssl->GetCsr());
if (!$errors->IsEmpty())
    $page->mTemplate->Assign($_POST);
$page->Display('ssl_management.tpl');

?>