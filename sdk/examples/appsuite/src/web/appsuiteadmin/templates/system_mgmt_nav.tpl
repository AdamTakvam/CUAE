<ul id="systemManagementNav">
    <li><a href="system_mgmt.php"{if !$selected} class="selected"{/if}>General Settings</a></li>
    <li><a href="findme_config.php"{if $selected == "findme"} class="selected"{/if}>Find Me Numbers</a></li>
    <li><a href="ar_blocklist.php"{if $selected == "blocklist"} class="selected"{/if}>ActiveRelay Blacklist</a></li>
    <li><a href="customize.php"{if $selected == "customize"} class="selected"{/if}>Customize Administrator</a></li>
    <li><a href="ldap.php"{if $selected == "ldap"} class="selected"{/if}>LDAP Servers</a></li>
</ul>

<div class="clear"></div>