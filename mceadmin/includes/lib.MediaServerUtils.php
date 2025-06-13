<?php

require_once("common.php");
require_once("lib.ComponentUtils.php");
require_once("components/class.Provider.php");


abstract class MediaServerUtils
{

    public static function clear_mrg_cache(ErrorHandler $eh)
    {
        if (MceUtils::is_app_server_running())
        {
            $id = ComponentUtils::find_component_id(MEDIA_CONTROL_PROVIDER);
            $mcp = new Provider();
            $mcp->SetId($id);
            $mcp->SetErrorHandler($eh);
            $mcp->Build();
            $exts = $mcp->GetExtensions();
            foreach ($exts as $ext)
            {
                if ($ext->GetName() == CLEAR_MRG_CACHE)
                    return $ext->InvokeExtension(array());
            }
            $eh->Add('Could not invoke the Media Control Provider\'s clear cache extension.');
            return FALSE;
        }
        return TRUE;
    }


}

?>