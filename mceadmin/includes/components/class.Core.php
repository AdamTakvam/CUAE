<?php

require_once("class.Component.php");
require_once("class.CoreExtension.php");
require_once("class.CustomCoreExtension.php");


class Core extends Component
{

    function __construct()
    {
        parent::__construct();
        $this->mExtensions = array();
        $this->mListPage = "component_list.php?type=" . ComponentType::CORE;
        $this->mEditPage = "edit_core.php";
    }

    function GetExtensions()
    {
        if (array() == $this->mExtensions)
        {
            $rs = $this->mDb->Execute("SELECT * FROM mce_provider_extensions WHERE mce_components_id = ? AND name <> ?", 
                                       array($this->mId, PRINT_DIAG_EXTENSION_NAME));
            $i = 0;
            while ($extension_data = $rs->FetchRow())
            {
                $this->mExtensions[$i] = new CoreExtension();
                $this->mExtensions[$i]->BuildWithData($extension_data);
                $this->mExtensions[$i]->SetErrorHandler($this->mErrorHandler);
                ++$i;
            }
            
            $prdiag_data = $this->mDb->GetRow("SELECT * FROM mce_provider_extensions WHERE mce_components_id = ? AND name = ?",
                                              array($this->mId, PRINT_DIAG_EXTENSION_NAME));
            if (!empty($prdiag_data))
            {
                $prdiag = new CustomCoreExtension();
                $prdiag->BuildWithData($prdiag_data);
                $prdiag->SetErrorHandler($this->mErrorHandler);
                $prdiag->AddParameter('ComponentName',$this->GetName(FALSE));
                $this->mExtensions[] = $prdiag;
            }
        }
        return $this->mExtensions;
    }

    function Uninstall()
    {
        throw new Exception("A core component cannot be uninstalled.");
    }

}

?>