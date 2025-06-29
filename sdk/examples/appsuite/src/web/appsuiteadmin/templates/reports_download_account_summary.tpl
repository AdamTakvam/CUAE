"Account","Status","Total Call Duration","Placed Calls","Successful Calls","Average Call Length","Failed Logins","Last Login"
{foreach from=$accounts item=account}
"{$account.name}","{$account.status_display}","{$account.total_call_duration}","{$account.placed_calls}","{$account.successfull_calls}","{$account.avg_call_length}","{$account.failed_logins}","{if $account.last_used neq '0000-00-00 00:00:00'}{$account.last_used|date_format:"%D %I:%M:%S %p"}{else}N/A{/if}"
{/foreach}
