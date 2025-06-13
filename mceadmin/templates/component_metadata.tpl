<p class="description">
    {$_metadata.description|escape:"html"}
</p>
<p>
    {if $_metadata.author}<strong>Developer</strong>: {$_metadata.author|escape:"html"} <br />{/if}
    {if $_metadata.copyright}<strong>Copyright</strong>: {$_metadata.copyright|escape:"html"} <br />{/if}
    {if $_metadata.author_url}[ <a href="{$_metadata.author_url|escape:"html"}">Developer Site</a> ]<br /> {/if}
    {if $_metadata.support_url}[ <a href="{$_metadata.support_url|escape:"html"}">Support Site</a> ]{/if}
</p>
