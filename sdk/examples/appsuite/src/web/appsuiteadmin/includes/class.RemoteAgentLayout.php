<?php

require_once("class.Layout.php");

class RemoteAgentLayout extends Layout
{
	
	public function Display($template)
	{
        $this->mTemplate->assign('_head_extra', implode("\n", $this->mHeadInfo));
        $this->mTemplate->assign('_content_template', $template);
        $this->mTemplate->display(MceConfig::REMOTE_AGENT_TEMPLATE);		
	}
	
}

?>