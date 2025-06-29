"Account","Origin Number","Destination Number","Application","Partition","Script","Start","Duration","End Reason"
{foreach from=$calls item=call}
"{if $call.as_users_id}{$call.name}{/if}","{$call.origin_number}","{$call.destination_number}","{$call.application_name}","{$call.partition_name}","{$call.script_name}","{$call.start|date_format:"%D %I:%M:%S %p"}","{$call.duration}","{$call.end_reason_display}"
{/foreach}