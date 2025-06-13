{* Page navigation will be placed above and below the user listing *}
{capture name=navigation assign=navigation}
<p>
    <strong>Page</strong>:
    {if $this_page > 0}
        <a href="{$SCRIPT_NAME}?{if $this_letter}a={$this_letter}&amp;{/if}p={$this_page-1}" class="pageNumber">Previous</a>
    {/if}
    {foreach from=$pages item=page}
        {if $this_page != $page}<a href="{$SCRIPT_NAME}?{if $this_letter}a={$this_letter}&amp;{/if}p={$page}" class="pageNumber">{/if}{$page+1}{if $this_page != $page}</a>{/if}
    {/foreach}
    {if $this_page < $last_page}
        <a href="{$SCRIPT_NAME}?{if $this_letter}a={$this_letter}&amp;{/if}p={$this_page+1}" class="pageNumber">Next</a>
    {/if}
</p>
{/capture}

<p>
    <strong>By Alphabet</strong>:<a href="{$SCRIPT_NAME}" class="pageNumber">*</a>
    {foreach from=$alphabet item=letter}
        <a href="{$SCRIPT_NAME}?a={$letter}" class="pageNumber">{$letter|capitalize}</a>
    {/foreach}
</p>

{$navigation}

<table>
    <col />
    <col style="width: 15em;"/>
    <col style="width: 15em;" />
    <tr>
        <th>Username</th>
        <th>Access Level</th>
        <th>Actions</th>
    </tr>
{foreach from=$users item=user}
    <tr class="{cycle values='rowOne,rowTwo'}">
        <td>{$user.username}</td>
        <td>{if $user.access_level eq 1}Administrator{else}Normal User{/if}</td>
        <td>
            <a href="edit_user.php?id={$user.mce_users_id}" class="button">Edit User</a>
            <a href="delete_user.php?id={$user.mce_users_id}&amp;list=1" class="button">Delete User</a>
        </td>
    </tr>
{/foreach}
</table>

{$navigation}

<form action="add_user.php" method="post">
    <input type="submit" name="submit" value="Add a User" class="submit" />
</form>