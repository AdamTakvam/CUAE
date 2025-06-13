<p>
    The webserver
    {if not $cert_exists}<strong>does not</strong> have{else}has{/if} an SSL certificate and
    {if not $key_exists}<strong>does not</strong> have{else}has{/if} an SSL key.
    <br />
    {if not ($cert_exists && $key_exists)}
    You can not enable SSL on the webserver until you have a certificate and key.  
    You may upload or generate a certificate and key.
    {else}
    SSL is currently <strong>{if $ssl_enabled}enabled{else}not enabled{/if}</strong>.
    {/if}
</p>

{if $cert_exists && $key_exists}
<form action="{$SCRIPT_NAME}" method="post">
    {if $ssl_enabled}
        <input type="submit" name="disable_ssl" value="Disable SSL" />
    {else}
        <input type="submit" name="enable_ssl" value="Enable SSL" />
    {/if}
</form>
{/if}


<h3>Upload SSL Certificate/Key</h3>

<p>
If you already have your own SSL certificate and key, you may upload them.  
The certificate and key will be verified to make sure they are compatible, so make sure both are uploaded at the same time
if there is not already a certificate and key.
</p>

<form action="{$SCRIPT_NAME}" method="post" enctype="multipart/form-data">

<table style="width:30em;">
    <col class="inputLabels" />
    <col class="inputFields" />
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Certificate</th>
        <td><input type="file" name="cert_file" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Key</th>
        <td><input type="file" name="key_file" /></td>
    </tr>    
</table>
<input type="submit" name="upload" value="Upload" />
</form>


<h3>Generate SSL Certificate/Key</h3>

<p>
A self-signed certificate and key will be generated for you when you fill out and submit the form below.  A certificate signature request (CSR) will
also be generated from the information you supply.  The CSR may be used to purchase a secure certificate from a secure certificate authority.
</p>

<form action="{$SCRIPT_NAME}" method="post">
<table style="width: 30em;">
    <col class="inputLabels" />
    <col class="inputFields" />
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Passphrase</th>
        <td><input type="text" name="passphrase" value="{$passphrase}" size="30" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Organization Name</th>
        <td><input type="text" name="organization" value="{$organization}" size="30" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Organizational Unit</th>
        <td><input type="text" name="organziational_unit" value="{$organziational_unit}" size="30" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Country</th>
        <td><input type="text" name="country" size="2" value="{$country}" maxlength="2" /> (2-Letter Code)</td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>State/Province</th>
        <td><input type="text" name="state" value="{$state}" size="30" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>City/Locality</th>
        <td><input type="text" name="locality" value="{$locality}" size="30" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Domain/Common Name</th>
        <td><input type="text" name="common_name" value="{$common_name}" size="30" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>E-mail Address</th>
        <td><input type="text" name="email" value="{$email}" size="30" /></td>
    </tr>
    <tr class="{cycle values='rowOne,rowTwo'}">
        <th>Years Until Expire</th>
        <td><input type="text" name="years_expire" size="2" value="{if $years_expire > 0}{$years_expire}{else}1{/if}" /></td>
    </tr>
</table>
<input type="submit" name="generate" value="Generate Certificate/Key" />
</form>

{if $csr_exists}
<h4>Certificate Signature Request</h4>
<p>You can copy and paste the following into a text file to use as a CSR file.</p>
<pre style="border: 1px solid gray; padding: 1em;">
{$csr|escape:"htmlall"}
</pre>
{/if}
