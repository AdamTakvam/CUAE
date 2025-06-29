<Application name="MainMenu" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="MainMenu">
    <outline>
      <treenode type="evh" id="632497010124029431" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632279429647343960" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632279429647343959" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/LdapDirectory/MainMenu</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632497010124029432" level="1" text="Metreos.Providers.Http.GotRequest: LetterListing">
        <node type="function" name="LetterListing" id="632282841162880351" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632282841162880350" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/LdapDirectory/LetterListing</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632497010124029433" level="1" text="Metreos.Providers.Http.GotRequest: LetterListingPagedResults">
        <node type="function" name="LetterListingPagedResults" id="632282841162880616" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632282841162880615" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/LdapDirectory/LetterListingPagedResults</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632497010124029434" level="1" text="Metreos.Providers.Http.GotRequest: LetterListingResults">
        <node type="function" name="LetterListingResults" id="632282841162880365" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632282841162880364" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/LdapDirectory/LetterListingResults</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632497010124029435" level="1" text="Metreos.Providers.Http.GotRequest: NameSearch">
        <node type="function" name="NameSearch" id="632279476972418891" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632279476972418890" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/LdapDirectory/NameSearch</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632497010124029436" level="1" text="Metreos.Providers.Http.GotRequest: NameSearchPagedResults">
        <node type="function" name="NameSearchPagedResults" id="632280059685224044" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632280059685224043" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/LdapDirectory/NameSearchPagedResults</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632497010124029437" level="1" text="Metreos.Providers.Http.GotRequest: NameSearchResults">
        <node type="function" name="NameSearchResults" id="632279476972418905" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632279476972418904" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/LdapDirectory/NameSearchResults</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632497010124029438" level="1" text="Metreos.Providers.Http.SessionExpired: OnSessionExpired">
        <node type="function" name="OnSessionExpired" id="632280059685224086" path="Metreos.StockTools" />
        <node type="event" name="SessionExpired" id="632280059685224085" path="Metreos.Providers.Http.SessionExpired" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_is7970" id="632587487435493319" vid="632279442348281438">
        <Properties type="Metreos.Types.Bool">g_is7970</Properties>
      </treenode>
      <treenode text="g_mainMenuUrl" id="632587487435493321" vid="632279442348281442">
        <Properties type="Metreos.Types.String">g_mainMenuUrl</Properties>
      </treenode>
      <treenode text="g_firstNameAttr" id="632587487435493323" vid="632280059685223993">
        <Properties type="Metreos.Types.String" initWith="First_Name_Attribute">g_firstNameAttr</Properties>
      </treenode>
      <treenode text="g_lastNameAttr" id="632587487435493325" vid="632280059685223995">
        <Properties type="Metreos.Types.String" initWith="Last_Name_Attribute">g_lastNameAttr</Properties>
      </treenode>
      <treenode text="g_telephoneAttr" id="632587487435493327" vid="632280059685223997">
        <Properties type="Metreos.Types.String" initWith="Telephone_Attribute">g_telephoneAttr</Properties>
      </treenode>
      <treenode text="g_ldapHostname" id="632587487435493329" vid="632280059685223999">
        <Properties type="Metreos.Types.String" initWith="Ldap_Address">g_ldapHostname</Properties>
      </treenode>
      <treenode text="g_ldapPort" id="632587487435493331" vid="632280059685224001">
        <Properties type="Metreos.Types.UShort" initWith="Ldap_Port">g_ldapPort</Properties>
      </treenode>
      <treenode text="g_ldapUsername" id="632587487435493333" vid="632280059685224003">
        <Properties type="Metreos.Types.String" defaultInitWith="NONE" initWith="Ldap_Username">g_ldapUsername</Properties>
      </treenode>
      <treenode text="g_ldapPassword" id="632587487435493335" vid="632280059685224005">
        <Properties type="Metreos.Types.String" defaultInitWith="NONE" initWith="Ldap_Password">g_ldapPassword</Properties>
      </treenode>
      <treenode text="g_ldapSearchBase" id="632587487435493337" vid="632280059685224007">
        <Properties type="Metreos.Types.String" initWith="Search_Base">g_ldapSearchBase</Properties>
      </treenode>
      <treenode text="g_ldapUnknownNumber" id="632587487435493339" vid="632280059685224009">
        <Properties type="Metreos.Types.String" initWith="Unknown_Number">g_ldapUnknownNumber</Properties>
      </treenode>
      <treenode text="g_results" id="632587487435493341" vid="632280059685224012">
        <Properties type="DataTable">g_results</Properties>
      </treenode>
      <treenode text="g_letterResults" id="632587487435493343" vid="632280059685224014">
        <Properties type="DataTable">g_letterResults</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632279429647343962" treenode="632497010124029431" appnode="632279429647343960" handlerfor="632279429647343959">
    <node type="Start" id="632279429647343962" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="33" y="364">
      <linkto id="632279442348281425" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632279442348281425" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="200" y="363">
      <linkto id="632279442348281437" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">1st action</log>
public static string Execute(ref bool g_is7970, string acceptHeader, ref string sessionCookie, string routingGuid, ref string cacheValue, ref string g_ldapUsername, ref string g_ldapPassword)
{
      if(g_ldapUsername == "NONE")
	{
		g_ldapUsername = String.Empty;
	}
      if(g_ldapPassword == "NONE")
	{
		g_ldapPassword = String.Empty;
	}

	// If the incoming device accepts png images, it must be a 7970 in the world of
      // Cisco devices.
	if(acceptHeader != null &amp;&amp; -1 != acceptHeader.IndexOf("image/png"))
	{
		g_is7970 = true;
	}
	else
	{
		g_is7970 = false;
	}




	sessionCookie = @"MetreosSessionId=" + routingGuid + @"; path=/";

	cacheValue    = "no-cache";

	return "success";
}</Properties>
    </node>
    <node type="Comment" id="632279442348281435" text="Set:&#xD;&#xA;is7970&#xD;&#xA;sessionCookie&#xD;&#xA;Cache-Control = no-cache" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="127" y="244" />
    <node type="Action" id="632279442348281437" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="358" y="361">
      <linkto id="632279442348281440" type="Labeled" style="Bevel" label="equal" />
      <linkto id="632279442348281496" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_is7970</ap>
        <ap name="Value2" type="csharp">true</ap>
        <log condition="entry" on="true" level="Info" type="literal">Entering custom code</log>
        <log condition="exit" on="true" level="Info" type="literal">Exiting custom code</log>
      </Properties>
    </node>
    <node type="Action" id="632279442348281440" name="CreateGraphicFileMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="545" y="244">
      <linkto id="632279442348281497" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Directory</ap>
        <ap name="Prompt" type="literal">Choose a search</ap>
        <ap name="LocationX" type="literal">0</ap>
        <ap name="LocationY" type="literal">0</ap>
        <ap name="URL" type="variable">g_mainMenuUrl</ap>
        <rd field="ResultData">menu7970</rd>
      </Properties>
    </node>
    <node type="Comment" id="632279442348281441" text="Incoming 7970 " class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="345" y="186" />
    <node type="Action" id="632279442348281496" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="549" y="504">
      <linkto id="632279442348281502" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Directory</ap>
        <ap name="Prompt" type="literal">Choose a search</ap>
        <rd field="ResultData">menuNot7970</rd>
        <log condition="entry" on="true" level="Error" type="literal">Creating menu</log>
      </Properties>
    </node>
    <node type="Action" id="632279442348281497" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="705" y="244">
      <linkto id="632279442348281498" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Name Search</ap>
        <ap name="URL" type="csharp">host + "/LdapDirectory/NameSearch?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu7970</rd>
      </Properties>
    </node>
    <node type="Action" id="632279442348281498" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="827" y="245">
      <linkto id="632279442348281499" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Search by Letter</ap>
        <ap name="URL" type="csharp">host + "/LdapDirectory/LetterListing?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menu7970</rd>
      </Properties>
    </node>
    <node type="Action" id="632279442348281499" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="991" y="244">
      <linkto id="632279442348281505" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Cache-Control" type="variable">cacheValue</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">menu7970.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Comment" id="632279442348281500" text="Add 'Name Search' option" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="615" y="120" />
    <node type="Comment" id="632279442348281501" text="Add 'Search By Letter' option" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="763" y="120" />
    <node type="Action" id="632279442348281502" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="700" y="503">
      <linkto id="632279442348281503" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Name Search</ap>
        <ap name="URL" type="csharp">host + "/LdapDirectory/NameSearch?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menuNot7970</rd>
        <log condition="entry" on="true" level="Info" type="literal">Adding 'Name Search'</log>
      </Properties>
    </node>
    <node type="Action" id="632279442348281503" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="825" y="504">
      <linkto id="632279442348281504" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Search by Letter</ap>
        <ap name="URL" type="csharp">host + "/LdapDirectory/LetterListing?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">menuNot7970</rd>
        <log condition="entry" on="true" level="Info" type="literal">Adding 'Search by Level'</log>
      </Properties>
    </node>
    <node type="Action" id="632279442348281504" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="992.5892" y="503">
      <linkto id="632279442348281505" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="Cache-Control" type="variable">cacheValue</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">menuNot7970.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
        <log condition="entry" on="true" level="Info" type="literal">Sending Http Response</log>
        <log condition="exit" on="true" level="Info" type="literal">Done sending Http Response</log>
      </Properties>
    </node>
    <node type="Action" id="632279442348281505" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1133.58923" y="378">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">Exiting OnGotRequest</log>
      </Properties>
    </node>
    <node type="Comment" id="632279442348281506" text="Send response to the phone,&#xD;&#xA;sending a session cookie so &#xD;&#xA;that all subsequent requests&#xD;&#xA;are routed back to this session." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="945.589233" y="117" />
    <node type="Comment" id="632279442348281507" text="Incoming Non-7970" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="325" y="493" />
    <node type="Variable" id="632279442348281426" name="acceptHeader" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="Accept" refType="reference">acceptHeader</Properties>
    </node>
    <node type="Variable" id="632279442348281427" name="menu7970" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.GraphicFileMenu" refType="reference">menu7970</Properties>
    </node>
    <node type="Variable" id="632279442348281428" name="menuNot7970" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" refType="reference">menuNot7970</Properties>
    </node>
    <node type="Variable" id="632279442348281429" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632279442348281430" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632279442348281431" name="hostname" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="hostname" refType="reference">hostname</Properties>
    </node>
    <node type="Variable" id="632279442348281432" name="sessionCookie" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" refType="reference">sessionCookie</Properties>
    </node>
    <node type="Variable" id="632279442348281433" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632279442348281434" name="cacheValue" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" refType="reference">cacheValue</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="LetterListing" startnode="632282841162880353" treenode="632497010124029432" appnode="632282841162880351" handlerfor="632282841162880350">
    <node type="Loop" id="632282841162880357" name="Loop" text="loop 26x" cx="176" cy="164" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="364" y="207" mx="452" my="289">
      <linkto id="632282841162880358" fromport="1" type="Basic" style="Bevel" />
      <linkto id="632282841162880359" fromport="3" type="Labeled" style="Bevel" label="default" />
      <Properties iteratorType="int" type="literal">26</Properties>
    </node>
    <node type="Start" id="632282841162880353" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="288">
      <linkto id="632282841162880355" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632282841162880355" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="218" y="288">
      <linkto id="632282841162880357" port="1" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Search By Letter</ap>
        <ap name="Prompt" type="literal">Choose letter</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632282841162880358" name="AddMenuItem" container="632282841162880357" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="450" y="289">
      <linkto id="632282841162880357" port="3" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">new String((char)(65 + loopIndex), 1)</ap>
        <ap name="URL" type="csharp">host + "/LdapDirectory/LetterListingResults?letter=" +(char)(65 + loopIndex) + "&amp;metreosSessionId="  + routingGuid</ap>
        <rd field="ResultData">menu</rd>
      </Properties>
    </node>
    <node type="Action" id="632282841162880359" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="657" y="286">
      <linkto id="632282841162880362" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">menu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632282841162880362" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="783" y="284">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632282841162880356" name="menu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" refType="reference">menu</Properties>
    </node>
    <node type="Variable" id="632282841162880360" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632282841162880361" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632282841162880363" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="LetterListingPagedResults" startnode="632282841162880618" treenode="632497010124029433" appnode="632282841162880616" handlerfor="632282841162880615">
    <node type="Loop" id="632282841162880620" name="Loop" text="loop (expr)" cx="249" cy="117" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="751" y="157" mx="876" my="216">
      <linkto id="632282841162880621" fromport="1" type="Basic" style="Bevel" />
      <linkto id="632282841162880635" fromport="3" type="Labeled" style="Bevel" label="default" />
      <Properties iteratorType="int" type="csharp">g_letterResults.Rows.Count &lt; 32 ? g_letterResults.Rows.Count : 32</Properties>
    </node>
    <node type="Start" id="632282841162880618" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="216">
      <linkto id="632282841162880624" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632282841162880621" name="AddDirectoryEntry" container="632282841162880620" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="834" y="217">
      <linkto id="632282841162880622" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">(g_letterResults.Rows[index][g_firstNameAttr] as string) + " " + (g_letterResults.Rows[index][g_lastNameAttr] as string)</ap>
        <ap name="Telephone" type="csharp">g_letterResults.Rows[index][g_telephoneAttr]</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632282841162880622" name="CustomCode" container="632282841162880620" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="930" y="216">
      <linkto id="632282841162880620" port="3" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">
	public static string Execute(ref int index)
	{
		index++;
		return "success";
	}
</Properties>
    </node>
    <node type="Comment" id="632282841162880623" text="Determine index,&#xD;&#xA;far paging,&#xD;&#xA;direction of paging" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="87" y="90" />
    <node type="Action" id="632282841162880624" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="140" y="215">
      <linkto id="632282841162880625" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">Entering NameSearchPagedResults request handler</log>
        <log condition="exit" on="true" level="Info" type="csharp">"index = " + queryParams["index"] + System.Environment.NewLine + "far = " + queryParams["far"] + System.Environment.NewLine + "forward = " + queryParams["forward"]</log>
public static string Execute(Metreos.Types.Http.QueryParamCollection queryParams, ref int index, ref bool forward, ref bool far)
{
	try
	{
		index = int.Parse(queryParams["index"]);
	}
	catch { index = 0; }

	try
	{
		forward = bool.Parse(queryParams["forward"]);
	}
	catch { forward = true; }

	try
	{
		far = bool.Parse(queryParams["far"]);
	}
	catch { far = false; } 

	return "success";
}
</Properties>
    </node>
    <node type="Action" id="632282841162880625" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="335" y="218">
      <linkto id="632282841162880627" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">
public static string Execute(bool far, ref int index, bool forward, DataTable g_letterResults)
{
	// Far paging requested by user
	if(far)
	{
		if(forward)
		{
			index += 64;   //32 * 2
		}
		else
		{
			index -= 128;  // 32 * 4
		}
	}		
	// Regular paging requested
	else
	{
		if(!forward)
		{
			index -= 64;   // 32 * 2 
		}
	}

	// If index is past the last 32 results, then position 32 back from the end
	// of the results
	if(index  &gt; g_letterResults.Rows.Count - 32)
	{
		index = g_letterResults.Rows.Count - 32;
	}

	// If index is below 0, reposition it to 0.
	if(index &lt; 0)
	{
		index = 0;
	}

	return "success";
}
</Properties>
    </node>
    <node type="Comment" id="632282841162880626" text="Code to validate incoming&#xD;&#xA;parameters, and to modify&#xD;&#xA;the index parameter as &#xD;&#xA;necessary" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="266" y="77" />
    <node type="Action" id="632282841162880627" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="523" y="217">
      <linkto id="632282841162880629" type="Labeled" style="Bevel" label="equal" />
      <linkto id="632282841162880634" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_letterResults.Rows.Count</ap>
        <ap name="Value2" type="csharp">0</ap>
      </Properties>
    </node>
    <node type="Comment" id="632282841162880628" text="Check that there are &#xD;&#xA;actually results to display" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="458" y="107" />
    <node type="Action" id="632282841162880629" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="523" y="320">
      <linkto id="632282841162880630" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Search Results</ap>
        <ap name="Prompt" type="literal">Try a new search</ap>
        <ap name="Text" type="literal">No results for this search</ap>
        <rd field="ResultData">noResultsText</rd>
      </Properties>
    </node>
    <node type="Action" id="632282841162880630" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="524" y="437">
      <linkto id="632282841162880631" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">MainMenu</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host+ "/LdapDirectory/MainMenu"</ap>
        <rd field="ResultData">noResultsText</rd>
      </Properties>
    </node>
    <node type="Action" id="632282841162880631" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="524" y="547">
      <linkto id="632282841162880632" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">noResultsText</rd>
      </Properties>
    </node>
    <node type="Action" id="632282841162880632" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="524" y="651">
      <linkto id="632282841162880633" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">noResultsText.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632282841162880633" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="524" y="745">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632282841162880634" name="CreateDirectory" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="675" y="217">
      <linkto id="632282841162880620" port="1" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Search Results</ap>
        <ap name="Prompt" type="literal">Choose a person to call</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632282841162880635" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1052.2168" y="216">
      <linkto id="632282841162880637" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Dial</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="literal">SoftKey:Dial</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Comment" id="632282841162880636" text="Add the standard softkeys, &#xD;&#xA;present in all responses.&#xD;&#xA;Dial / 1, EditDial / 2, Exit / 3" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1087.2168" y="84" />
    <node type="Action" id="632282841162880637" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1162.2168" y="216">
      <linkto id="632282841162880638" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">EditDial</ap>
        <ap name="Position" type="literal">2</ap>
        <ap name="URL" type="literal">SoftKey:EditDial</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632282841162880638" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1276.2168" y="216">
      <linkto id="632282841162880639" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Edit</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632282841162880639" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1407.2168" y="83">
      <linkto id="632282841162880640" type="Labeled" style="Bevel" label="equal" />
      <linkto id="632282841162880642" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">index &gt; 32</ap>
        <ap name="Value2" type="csharp">true</ap>
      </Properties>
    </node>
    <node type="Action" id="632282841162880640" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1524.39063" y="83">
      <linkto id="632282841162880642" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Prev</ap>
        <ap name="Position" type="literal">6</ap>
        <ap name="URL" type="csharp">host + "/LdapDirectory/LetterListingPagedResults?forward=false&amp;far=false&amp;index=" + index + "&amp;MetreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Comment" id="632282841162880641" text="If index is more than 32,&#xD;&#xA;then present the 'Prev' SoftKey" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1591.39063" y="32" />
    <node type="Action" id="632282841162880642" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1464.39063" y="198">
      <linkto id="632282841162880644" type="Labeled" style="Bevel" label="equal" />
      <linkto id="632282841162880647" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">index &gt; 64</ap>
        <ap name="Value2" type="csharp">true</ap>
      </Properties>
    </node>
    <node type="Comment" id="632282841162880643" text="If index is more than 64,&#xD;&#xA;then present the 'Far Prev' SoftKey" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1659.39063" y="143" />
    <node type="Action" id="632282841162880644" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1596.39063" y="199">
      <linkto id="632282841162880647" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Far Prev</ap>
        <ap name="Position" type="literal">7</ap>
        <ap name="URL" type="csharp">host + "/LdapDirectory/LetterListingPagedResults?forward=false&amp;far=true&amp;index=" + index + "&amp;MetreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Comment" id="632282841162880645" text="If index is at least&#xD;&#xA;smaller than the results rows &#xD;&#xA;left, then show the 'Next' SoftKey" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1754.39063" y="240" />
    <node type="Comment" id="632282841162880646" text="If index is at least 32 &#xD;&#xA;rows less than the number of result rows, &#xD;&#xA;then show the 'Far Next' SoftKey" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1763.39063" y="351" />
    <node type="Action" id="632282841162880647" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1530.82324" y="312">
      <linkto id="632282841162880649" type="Labeled" style="Bevel" label="equal" />
      <linkto id="632282841162880650" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">index &lt; g_letterResults.Rows.Count</ap>
        <ap name="Value2" type="csharp">true</ap>
      </Properties>
    </node>
    <node type="Comment" id="632282841162880648" text="Increment index" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="883" y="110" />
    <node type="Action" id="632282841162880649" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1664" y="312">
      <linkto id="632282841162880650" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Next</ap>
        <ap name="Position" type="literal">4</ap>
        <ap name="URL" type="csharp">host + "/LdapDirectory/LetterListingPagedResults?forward=true&amp;far=false&amp;index=" + index + "&amp;MetreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632282841162880650" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1599" y="412">
      <linkto id="632282841162880651" type="Labeled" style="Bevel" label="equal" />
      <linkto id="632282841162880652" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">index &lt; g_letterResults.Rows.Count - 32</ap>
        <ap name="Value2" type="csharp">true</ap>
      </Properties>
    </node>
    <node type="Action" id="632282841162880651" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1733" y="413">
      <linkto id="632282841162880652" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Far Next</ap>
        <ap name="Position" type="literal">5</ap>
        <ap name="URL" type="csharp">host + "/LdapDirectory/LetterListingPagedResults?forward=true&amp;far=true&amp;index=" + index + "&amp;MetreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632282841162880652" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1669" y="515">
      <linkto id="632282841162880653" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">directory.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632282841162880653" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1805" y="515">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632282841162880687" name="index" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Int" refType="reference">index</Properties>
    </node>
    <node type="Variable" id="632282841162880688" name="far" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Bool" refType="reference">far</Properties>
    </node>
    <node type="Variable" id="632282841162880689" name="forward" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Bool" refType="reference">forward</Properties>
    </node>
    <node type="Variable" id="632282841162880690" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632282841162880691" name="noResultsText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">noResultsText</Properties>
    </node>
    <node type="Variable" id="632282841162880692" name="directory" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Directory" refType="reference">directory</Properties>
    </node>
    <node type="Variable" id="632282841162880693" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632282841162880694" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632282841162880695" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">queryParams</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="LetterListingResults" activetab="true" startnode="632282841162880367" treenode="632497010124029434" appnode="632282841162880365" handlerfor="632282841162880364">
    <node type="Loop" id="632282841162880604" name="Loop" text="loop (expr)" cx="133" cy="120" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="683" y="268" mx="750" my="328">
      <linkto id="632282841162880605" fromport="1" type="Basic" style="Bevel" />
      <linkto id="632282841162880606" fromport="3" type="Labeled" style="Bevel" label="default" />
      <Properties iteratorType="int" type="csharp">g_letterResults.Rows.Count &gt; 32 ? 32 : g_letterResults.Rows.Count;</Properties>
    </node>
    <node type="Start" id="632282841162880367" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="326">
      <linkto id="632282841162880369" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632282841162880369" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="167" y="327">
      <linkto id="632282841162880583" type="Labeled" style="Bevel" label="false" />
      <linkto id="632586799191496361" type="Labeled" style="Bevel" label="true" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.Http.QueryParamCollection queryParams, ref string letter)
{
	letter = queryParams["letter"];

	if(letter != null &amp;&amp; letter != String.Empty)
	{
		return "true";
	}
	else
	{
		return "false";
	}
}
</Properties>
    </node>
    <node type="Comment" id="632282841162880372" text="set letter" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="114" y="247" />
    <node type="Action" id="632282841162880583" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="168" y="434">
      <linkto id="632282841162880585" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Directory</ap>
        <ap name="Prompt" type="literal">Return to MainMenu</ap>
        <ap name="Text" type="literal">This page was reached in an invalid manner. </ap>
        <rd field="ResultData">invalidPageText</rd>
      </Properties>
    </node>
    <node type="Action" id="632282841162880585" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="305" y="593">
      <linkto id="632282841162880586" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">MainMenu</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host + "/LdapDirectory/MainMenu?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">invalidPageText</rd>
      </Properties>
    </node>
    <node type="Action" id="632282841162880586" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="305" y="703">
      <linkto id="632282841162880590" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">invalidPageText</rd>
      </Properties>
    </node>
    <node type="Action" id="632282841162880590" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="305" y="814">
      <linkto id="632282841162880591" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">invalidPageText.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632282841162880591" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="306" y="922">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632282841162880593" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="449" y="329">
      <linkto id="632282841162880594" type="Labeled" style="Bevel" label="false" />
      <linkto id="632282841162880601" type="Labeled" style="Bevel" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_letterResults.Rows.Count &gt; 0</ap>
      </Properties>
    </node>
    <node type="Action" id="632282841162880594" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="449" y="434">
      <linkto id="632282841162880585" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Search Results</ap>
        <ap name="Prompt" type="literal">Try a new search</ap>
        <ap name="Text" type="literal">No results were found for your query.</ap>
        <rd field="ResultData">invalidPageText</rd>
      </Properties>
    </node>
    <node type="Action" id="632282841162880601" name="CreateDirectory" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="567" y="328">
      <linkto id="632282841162880604" port="1" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Search Results</ap>
        <ap name="Prompt" type="literal">Choose a person to call</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Comment" id="632282841162880603" text="As long as rowCount is less &#xD;&#xA;than the result count, and also &#xD;&#xA;index is less than the maximum &#xD;&#xA;number of directory entries" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="494" y="213" />
    <node type="Action" id="632282841162880605" name="AddDirectoryEntry" container="632282841162880604" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="745" y="328">
      <linkto id="632282841162880604" port="3" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">(g_letterResults.Rows[loopIndex][g_firstNameAttr] as string) + ", " + (g_letterResults.Rows[loopIndex][g_lastNameAttr] as string) </ap>
        <ap name="Telephone" type="csharp">g_letterResults.Rows[loopIndex][g_telephoneAttr] as string</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632282841162880606" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="924" y="327">
      <linkto id="632282841162880607" type="Labeled" style="Bevel" label="true" />
      <linkto id="632282841162880612" type="Labeled" style="Bevel" label="false" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_letterResults.Rows.Count &gt; 32</ap>
      </Properties>
    </node>
    <node type="Action" id="632282841162880607" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1056" y="326">
      <linkto id="632282841162880608" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Dial</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="literal">SoftKey:Dial</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632282841162880608" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1178" y="326">
      <linkto id="632282841162880609" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">EditDial</ap>
        <ap name="Position" type="literal">2</ap>
        <ap name="URL" type="literal">SoftKey:EditDial</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632282841162880609" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1306.17383" y="326">
      <linkto id="632282841162880610" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632282841162880610" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1459.17383" y="325">
      <linkto id="632282841162880611" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Next</ap>
        <ap name="Position" type="literal">4</ap>
        <ap name="URL" type="csharp">host + "/LdapDirectory/LetterListingPagedResults?forward=true&amp;far=false&amp;index=32&amp;MetreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632282841162880611" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1589.17383" y="324">
      <linkto id="632282841162880612" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Far Next</ap>
        <ap name="Position" type="literal">5</ap>
        <ap name="URL" type="csharp">host + "/LdapDirectory/LetterListingPagedResults?index=32&amp;forward=true&amp;far=true&amp;MetreosSessionId="  + routingGuid</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632282841162880612" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="927.1738" y="498">
      <linkto id="632282841162880613" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">directory.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
        <log condition="entry" on="true" level="Info" type="literal">Sending response</log>
      </Properties>
    </node>
    <node type="Action" id="632282841162880613" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1113.17383" y="494">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632586799191496361" name="Query" class="MaxActionNode" group="" path="Metreos.Native.Ldap" x="309" y="327">
      <linkto id="632282841162880593" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="LdapServerHost" type="variable">g_ldapHostname</ap>
        <ap name="LdapServerPort" type="variable">g_ldapPort</ap>
        <ap name="Username" type="variable">g_ldapUsername</ap>
        <ap name="Password" type="variable">g_ldapPassword</ap>
        <ap name="BaseDn" type="variable">g_ldapSearchBase</ap>
        <ap name="SearchFilter" type="csharp">g_lastNameAttr + "=" +  letter + "*"</ap>
        <ap name="Attributes" type="csharp">new string[] { g_firstNameAttr, g_lastNameAttr, g_telephoneAttr }</ap>
        <rd field="SearchResults">g_letterResults</rd>
        <log condition="exit" on="true" level="Info" type="csharp">"NameSearch resulted in " + g_letterResults.Rows.Count + " entries"</log>
      </Properties>
    </node>
    <node type="Variable" id="632282841162880584" name="invalidPageText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">invalidPageText</Properties>
    </node>
    <node type="Variable" id="632282841162880587" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632282841162880588" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632282841162880589" name="letter" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" refType="reference">letter</Properties>
    </node>
    <node type="Variable" id="632282841162880602" name="directory" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Directory" refType="reference">directory</Properties>
    </node>
    <node type="Variable" id="632282841162880614" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632282841162880696" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">queryParams</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="NameSearch" startnode="632279476972418893" treenode="632497010124029435" appnode="632279476972418891" handlerfor="632279476972418890">
    <node type="Start" id="632279476972418893" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="343">
      <linkto id="632279476972418898" type="Basic" style="Bevel" />
    </node>
    <node type="Action" id="632279476972418898" name="CreateInput" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="310" y="347">
      <linkto id="632279476972418899" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Name Search</ap>
        <ap name="Prompt" type="literal">Enter search criteria</ap>
        <ap name="URL" type="csharp">host + "/LdapDirectory/NameSearchResults?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632279476972418899" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="463" y="349">
      <linkto id="632279476972418900" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DisplayName" type="literal">First Name</ap>
        <ap name="QueryStringParam" type="literal">firstName</ap>
        <ap name="InputFlags" type="literal">A</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632279476972418900" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="606" y="347">
      <linkto id="632279476972418901" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DisplayName" type="literal">Last Name</ap>
        <ap name="QueryStringParam" type="literal">lastName</ap>
        <ap name="InputFlags" type="literal">A</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632279476972418901" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="759" y="346">
      <linkto id="632279476972418902" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">input.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632279476972418902" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="899" y="347">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632279476972418903" text="Create user input screen on Cisco IP Phone to enter &#xD;&#xA;first name and last name search criteria." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="424" y="201" />
    <node type="Variable" id="632279476972418895" name="input" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Input" refType="reference">input</Properties>
    </node>
    <node type="Variable" id="632279476972418896" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632279476972418897" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632282685318817849" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="NameSearchPagedResults" startnode="632280059685224046" treenode="632497010124029436" appnode="632280059685224044" handlerfor="632280059685224043">
    <node type="Loop" id="632280059685224063" name="Loop" text="loop (expr)" cx="249" cy="117" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="760" y="268" mx="884" my="326">
      <linkto id="632280059685224064" fromport="1" type="Basic" style="Bevel" />
      <linkto id="632280059685224065" fromport="3" type="Labeled" style="Bevel" label="default" />
      <Properties iteratorType="int" type="csharp">g_results.Rows.Count &lt; 32 ? g_results.Rows.Count : 32</Properties>
    </node>
    <node type="Start" id="632280059685224046" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="325">
      <linkto id="632280059685224049" type="Basic" style="Bevel" />
    </node>
    <node type="Comment" id="632280059685224048" text="Determine index,&#xD;&#xA;far paging,&#xD;&#xA;direction of paging" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="96" y="201" />
    <node type="Action" id="632280059685224049" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="149" y="326">
      <linkto id="632280059685224050" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">Entering NameSearchPagedResults request handler</log>
        <log condition="exit" on="true" level="Info" type="csharp">"index = " + queryParams["index"] + System.Environment.NewLine + "far = " + queryParams["far"] + System.Environment.NewLine + "forward = " + queryParams["forward"]</log>
public static string Execute(Metreos.Types.Http.QueryParamCollection queryParams, ref int index, ref bool forward, ref bool far)
{
	try
	{
		index = int.Parse(queryParams["index"]);
	}
	catch { index = 0; }

	try
	{
		forward = bool.Parse(queryParams["forward"]);
	}
	catch { forward = true; }

	try
	{
		far = bool.Parse(queryParams["far"]);
	}
	catch { far = false; } 

	return "success";
}
</Properties>
    </node>
    <node type="Action" id="632280059685224050" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="344" y="329">
      <linkto id="632280059685224052" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">
public static string Execute(bool far, ref int index, bool forward, DataTable g_results)
{
	// Far paging requested by user
	if(far)
	{
		if(forward)
		{
			index += 64;   //32 * 2
		}
		else
		{
			index -= 128;  // 32 * 4
		}
	}		
	// Regular paging requested
	else
	{
		if(!forward)
		{
			index -= 64;   // 32 * 2 
		}
	}

	// If index is past the last 32 results, then position 32 back from the end
	// of the results
	if(index  &gt; g_results.Rows.Count - 32)
	{
		index = g_results.Rows.Count - 32;
	}

	// If index is below 0, reposition it to 0.
	if(index &lt; 0)
	{
		index = 0;
	}

	return "success";
}
</Properties>
    </node>
    <node type="Comment" id="632280059685224051" text="Code to validate incoming&#xD;&#xA;parameters, and to modify&#xD;&#xA;the index parameter as &#xD;&#xA;necessary" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="275" y="188" />
    <node type="Action" id="632280059685224052" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="532" y="328">
      <linkto id="632280059685224055" type="Labeled" style="Bevel" label="equal" />
      <linkto id="632280059685224062" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_results.Rows.Count</ap>
        <ap name="Value2" type="csharp">0</ap>
      </Properties>
    </node>
    <node type="Comment" id="632280059685224053" text="Check that there are &#xD;&#xA;actually results to display" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="467" y="218" />
    <node type="Action" id="632280059685224055" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="532" y="431">
      <linkto id="632280059685224056" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Search Results</ap>
        <ap name="Prompt" type="literal">Try a new search</ap>
        <ap name="Text" type="literal">No results for this search</ap>
        <rd field="ResultData">noResultsText</rd>
      </Properties>
    </node>
    <node type="Action" id="632280059685224056" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="533" y="548">
      <linkto id="632280059685224057" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">MainMenu</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host+ "/LdapDirectory/MainMenu"</ap>
        <rd field="ResultData">noResultsText</rd>
      </Properties>
    </node>
    <node type="Action" id="632280059685224057" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="533" y="658">
      <linkto id="632280059685224058" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">noResultsText</rd>
      </Properties>
    </node>
    <node type="Action" id="632280059685224058" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="533" y="762">
      <linkto id="632280059685224060" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">noResultsText.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632280059685224060" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="533" y="856">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632280059685224062" name="CreateDirectory" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="684" y="328">
      <linkto id="632280059685224063" port="1" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Search Results</ap>
        <ap name="Prompt" type="literal">Choose a person to call</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632280059685224064" name="AddDirectoryEntry" container="632280059685224063" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="843" y="327">
      <linkto id="632280059685224078" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">(g_results.Rows[index][g_firstNameAttr] as string) + ", " + (g_results.Rows[index][g_lastNameAttr] as string)</ap>
        <ap name="Telephone" type="csharp">g_results.Rows[index][g_telephoneAttr] as string</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632280059685224065" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1061.2168" y="327">
      <linkto id="632280059685224067" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Dial</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="literal">SoftKey:Dial</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Comment" id="632280059685224066" text="Add the standard softkeys, &#xD;&#xA;present in all responses.&#xD;&#xA;Dial / 1, EditDial / 2, Exit / 3" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1096.2168" y="195" />
    <node type="Action" id="632280059685224067" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1171.2168" y="327">
      <linkto id="632280059685224068" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">EditDial</ap>
        <ap name="Position" type="literal">2</ap>
        <ap name="URL" type="literal">SoftKey:EditDial</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632280059685224068" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1285.2168" y="327">
      <linkto id="632280059685224069" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Edit</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632280059685224069" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1416.2168" y="194">
      <linkto id="632280059685224070" type="Labeled" style="Bevel" label="equal" />
      <linkto id="632280059685224072" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">index &gt; 32</ap>
        <ap name="Value2" type="csharp">true</ap>
      </Properties>
    </node>
    <node type="Action" id="632280059685224070" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1533.39063" y="194">
      <linkto id="632280059685224072" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Prev</ap>
        <ap name="Position" type="literal">6</ap>
        <ap name="URL" type="csharp">host + "/LdapDirectory/NameSearchPagedResults?forward=false&amp;far=false&amp;index=" + index + "&amp;MetreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Comment" id="632280059685224071" text="If index is more than 32,&#xD;&#xA;then present the 'Prev' SoftKey" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1600.39063" y="143" />
    <node type="Action" id="632280059685224072" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1473.39063" y="309">
      <linkto id="632280059685224074" type="Labeled" style="Bevel" label="equal" />
      <linkto id="632280059685224077" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">index &gt; 64</ap>
        <ap name="Value2" type="csharp">true</ap>
      </Properties>
    </node>
    <node type="Comment" id="632280059685224073" text="If index is more than 64,&#xD;&#xA;then present the 'Far Prev' SoftKey" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1668.39063" y="254" />
    <node type="Action" id="632280059685224074" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1605.39063" y="310">
      <linkto id="632280059685224077" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Far Prev</ap>
        <ap name="Position" type="literal">7</ap>
        <ap name="URL" type="csharp">host + "/LdapDirectory/NameSearchPagedResults?forward=false&amp;far=true&amp;index=" + index + "&amp;MetreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Comment" id="632280059685224075" text="If index is at least&#xD;&#xA;smaller than the results rows &#xD;&#xA;left, then show the 'Next' SoftKey" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1763.39063" y="351" />
    <node type="Comment" id="632280059685224076" text="If index is at least 32 &#xD;&#xA;rows less than the number of result rows, &#xD;&#xA;then show the 'Far Next' SoftKey" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1772.39063" y="462" />
    <node type="Action" id="632280059685224077" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1539.82324" y="423">
      <linkto id="632280059685224080" type="Labeled" style="Bevel" label="equal" />
      <linkto id="632280059685224081" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">index &lt; g_results.Rows.Count</ap>
        <ap name="Value2" type="csharp">true</ap>
      </Properties>
    </node>
    <node type="Action" id="632280059685224078" name="CustomCode" container="632280059685224063" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="939" y="327">
      <linkto id="632280059685224063" port="3" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">
	public static string Execute(ref int index)
	{
		index++;
		return "success";
	}
</Properties>
    </node>
    <node type="Comment" id="632280059685224079" text="Increment index" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="892" y="221" />
    <node type="Action" id="632280059685224080" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1673" y="423">
      <linkto id="632280059685224081" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Next</ap>
        <ap name="Position" type="literal">4</ap>
        <ap name="URL" type="csharp">host + "/LdapDirectory/NameSearchPagedResults?forward=true&amp;far=false&amp;index=" + index + "&amp;MetreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632280059685224081" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1608" y="523">
      <linkto id="632280059685224082" type="Labeled" style="Bevel" label="equal" />
      <linkto id="632280059685224083" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">index &lt; g_results.Rows.Count - 32</ap>
        <ap name="Value2" type="csharp">true</ap>
      </Properties>
    </node>
    <node type="Action" id="632280059685224082" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1742" y="524">
      <linkto id="632280059685224083" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Far Next</ap>
        <ap name="Position" type="literal">5</ap>
        <ap name="URL" type="csharp">host + "/LdapDirectory/NameSearchPagedResults?forward=true&amp;far=true&amp;index=" + index + "&amp;MetreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632280059685224083" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1678" y="626">
      <linkto id="632280059685224084" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">directory.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632280059685224084" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1814" y="626">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632280059685224054" name="noResultsText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">noResultsText</Properties>
    </node>
    <node type="Variable" id="632280059685224059" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632280059685224061" name="directory" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Directory" refType="reference">directory</Properties>
    </node>
    <node type="Variable" id="632280059685224092" name="far" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Bool" refType="reference">far</Properties>
    </node>
    <node type="Variable" id="632280059685224093" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">queryParams</Properties>
    </node>
    <node type="Variable" id="632280059685224094" name="index" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Int" refType="reference">index</Properties>
    </node>
    <node type="Variable" id="632280059685224095" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632280059685224096" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632280130832411595" name="forward" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Bool" refType="reference">forward</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="NameSearchResults" startnode="632279476972418907" treenode="632497010124029437" appnode="632279476972418905" handlerfor="632279476972418904">
    <node type="Loop" id="632280059685224029" name="Loop" text="loop (expr)" cx="151.6565" cy="138" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="1223.34351" y="241" mx="1299" my="310">
      <linkto id="632280059685224030" fromport="1" type="Basic" style="Bevel" />
      <linkto id="632280059685224033" fromport="3" type="Labeled" style="Bevel" label="default" />
      <Properties iteratorType="int" type="csharp">g_results.Rows.Count &gt; 32 ? 32 : g_results.Rows.Count;</Properties>
    </node>
    <node type="Start" id="632279476972418907" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="309">
      <linkto id="632279476972418918" type="Basic" style="Bevel" />
    </node>
    <node type="Comment" id="632279476972418917" text="Determine bools:&#xD;&#xA;firstNameIsDefined&#xD;&#xA;lastNameIsDefined&#xD;&#xA;Define cookie:&#xD;&#xA;sessionCookie" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="79" y="163" />
    <node type="Action" id="632279476972418918" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="135" y="310">
      <linkto id="632279476972418932" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">Entering NameSearchResults request handler</log>
public static string Execute(Metreos.Types.Http.QueryParamCollection queryParams, ref bool isFirstNameDefined, ref bool isLastNameDefined, ref string sessionCookie, string routingGuid)
{
	string firstName = queryParams["firstName"];

	if(firstName != null &amp;&amp; firstName != String.Empty)
	{
		isFirstNameDefined = true;
	}
	else
	{
		isFirstNameDefined = false;
	}

	string lastName = queryParams["lastName"];

	if(lastName != null &amp;&amp; lastName != String.Empty)
	{
		isLastNameDefined = true;
	}
	else
	{
		isLastNameDefined = false;
	}

	sessionCookie = "sessionId=" + routingGuid;

	return "success";
}
</Properties>
    </node>
    <node type="Comment" id="632279476972418919" text="Check for which combination of &#xD;&#xA;first name and last name is defined" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="225" y="148" />
    <node type="Action" id="632279476972418921" name="CreateInput" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="295" y="559">
      <linkto id="632279476972418923" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Name Search</ap>
        <ap name="Prompt" type="literal">Must specify a name</ap>
        <ap name="URL" type="csharp">host + "/LdapDirectory/NameSearchResults?metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">input</rd>
        <log condition="entry" on="true" level="Warning" type="literal">First Name and Last Name parameters not defined.</log>
      </Properties>
    </node>
    <node type="Action" id="632279476972418923" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="458" y="559">
      <linkto id="632279476972418924" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DisplayName" type="literal">First Name</ap>
        <ap name="QueryStringParam" type="literal">firstName</ap>
        <ap name="InputFlags" type="literal">A</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632279476972418924" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="592" y="559">
      <linkto id="632279476972418925" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DisplayName" type="literal">Last Name</ap>
        <ap name="QueryStringParam" type="literal">lastName</ap>
        <ap name="InputFlags" type="literal">a</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632279476972418925" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="710" y="559">
      <linkto id="632279476972418926" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">input.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632279476972418926" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="828.2363" y="558">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632279476972418932" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="291" y="311">
      <linkto id="632279476972418921" type="Labeled" style="Bevel" label="neither" />
      <linkto id="632279476972418933" type="Labeled" style="Bevel" label="both" />
      <linkto id="632279476972418934" type="Labeled" style="Bevel" label="first" />
      <linkto id="632279476972418935" type="Labeled" style="Bevel" label="last" />
      <Properties language="csharp">
public static string Execute(bool isFirstNameDefined, bool isLastNameDefined)
{
	if(isFirstNameDefined &amp;&amp; isLastNameDefined)
	{
		return "both";
	}

	if(!isFirstNameDefined &amp;&amp; !isLastNameDefined)
	{
		return "neither";	
	}

	if(isFirstNameDefined &amp;&amp; !isLastNameDefined)
	{
		return "first";
	}

	if(!isFirstNameDefined &amp;&amp; isLastNameDefined)
	{
		return "last";
	}

	// This code can't hit, but needed for c# compilation
	return "failure";
}
</Properties>
    </node>
    <node type="Action" id="632279476972418933" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="512" y="175">
      <linkto id="632586799191496362" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">
public static string Execute(ref string searchFilter, string g_firstNameAttr, string g_lastNameAttr, Metreos.Types.Http.QueryParamCollection queryParams, LogWriter log)
{
	string firstName = queryParams["firstName"];
	string lastName  = queryParams["lastName"];
	
log.Write(TraceLevel.Info, "g_firstNameAttr: " + g_firstNameAttr);
log.Write(TraceLevel.Info, "g_lastNameAttr: " + g_lastNameAttr);
log.Write(TraceLevel.Info, "firstName: " + firstName);
log.Write(TraceLevel.Info, "lastName: " + lastName);


	object[] args = new object[] { g_firstNameAttr, firstName, g_lastNameAttr, lastName };

	searchFilter = String.Format( "(&amp;({0}={1}*)({2}={3}*))", args );
	
	return "success";
	
}
</Properties>
    </node>
    <node type="Action" id="632279476972418934" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="513" y="310">
      <linkto id="632586799191496362" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">
public static string Execute(ref string searchFilter, string g_firstNameAttr, Metreos.Types.Http.QueryParamCollection queryParams)
{
	string firstName = queryParams["firstName"];
	searchFilter = String.Format( "{0}={1}*" , g_firstNameAttr, firstName );

	return "success";
}
</Properties>
    </node>
    <node type="Action" id="632279476972418935" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="512" y="413">
      <linkto id="632586799191496362" type="Labeled" style="Bevel" label="default" />
      <Properties language="csharp">
public static string Execute(ref string searchFilter, string g_lastNameAttr, Metreos.Types.Http.QueryParamCollection queryParams)
{
	string lastName = queryParams["lastName"];
	searchFilter = String.Format( "{0}={1}*", g_lastNameAttr, lastName );
 	
	return "success";
}
</Properties>
    </node>
    <node type="Action" id="632280059685224019" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="950.7999" y="311">
      <linkto id="632280059685224021" type="Labeled" style="Bevel" label="equal" />
      <linkto id="632280059685224027" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_results.Rows.Count</ap>
        <ap name="Value2" type="csharp">0</ap>
      </Properties>
    </node>
    <node type="Comment" id="632280059685224020" text="In the case of no results,&#xD;&#xA;send a text object which also&#xD;&#xA;has softkeys to return to the &#xD;&#xA;search screen" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="748.7999" y="340" />
    <node type="Action" id="632280059685224021" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="950.7999" y="414">
      <linkto id="632280059685224023" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Search Results</ap>
        <ap name="Prompt" type="literal">Try a new search</ap>
        <ap name="Text" type="literal">No results were found for your query.</ap>
        <rd field="ResultData">noResultsText</rd>
        <log condition="entry" on="true" level="Info" type="literal">No results for query</log>
      </Properties>
    </node>
    <node type="Action" id="632280059685224023" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="952.4106" y="529">
      <linkto id="632280059685224024" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">MainMenu</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host+ "/LdapDirectory/MainMenu"</ap>
        <rd field="ResultData">noResultsText</rd>
      </Properties>
    </node>
    <node type="Action" id="632280059685224024" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="953.4106" y="631">
      <linkto id="632280059685224025" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">noResultsText</rd>
      </Properties>
    </node>
    <node type="Action" id="632280059685224025" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="954.4106" y="740">
      <linkto id="632280059685224026" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">noResultsText.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632280059685224026" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="955.4106" y="849">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632280059685224027" name="CreateDirectory" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1111.62476" y="310">
      <linkto id="632280059685224029" port="1" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Search Results</ap>
        <ap name="Prompt" type="literal">Choose a person to call</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632280059685224030" name="AddDirectoryEntry" container="632280059685224029" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1297.34351" y="311">
      <linkto id="632280059685224029" port="3" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="csharp">(g_results.Rows[loopIndex][g_firstNameAttr] as string) + ", " + (g_results.Rows[loopIndex][g_lastNameAttr] as string)</ap>
        <ap name="Telephone" type="csharp">g_results.Rows[loopIndex][g_telephoneAttr] as string</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Comment" id="632280059685224031" text="Directories have a maximum amount&#xD;&#xA; of 32 for display on Cisco IP Phones" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1215.34351" y="177" />
    <node type="Comment" id="632280059685224032" text="Check if there are more&#xD;&#xA; than 32 results" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1458.34351" y="183" />
    <node type="Action" id="632280059685224033" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1520.34351" y="311">
      <linkto id="632280059685224034" type="Labeled" style="Bevel" label="default" />
      <linkto id="632280059685224036" type="Labeled" style="Bevel" label="equal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">g_results.Rows.Count &gt; 32</ap>
        <ap name="Value2" type="csharp">true</ap>
      </Properties>
    </node>
    <node type="Action" id="632280059685224034" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1520.34351" y="463">
      <linkto id="632280059685224035" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">directory.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632280059685224035" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1670.34351" y="463">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632280059685224036" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1658.81421" y="310">
      <linkto id="632280059685224037" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Dial</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="literal">SoftKey:Dial</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632280059685224037" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1784.81421" y="309">
      <linkto id="632280059685224038" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">EditDial</ap>
        <ap name="Position" type="literal">2</ap>
        <ap name="URL" type="literal">SoftKey:EditDial</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632280059685224038" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="1903.81421" y="309">
      <linkto id="632280059685224039" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">SoftKey:Exit</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632280059685224039" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="2028.81421" y="310">
      <linkto id="632280059685224042" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Next</ap>
        <ap name="Position" type="literal">4</ap>
        <ap name="URL" type="csharp">host + "/LdapDirectory/NameSearchPagedResults?index=32&amp;forward=true&amp;far=false&amp;metreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Comment" id="632280059685224040" text="If there are more results,&#xD;&#xA;then we must add the &#xD;&#xA;page forward hotkeys" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1684.81421" y="181" />
    <node type="Comment" id="632280059685224041" text="Far Next/Prev&#xD;&#xA;will move by 3 extra pages&#xD;&#xA;of results" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="1879.81421" y="182" />
    <node type="Action" id="632280059685224042" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="2153.30347" y="305">
      <linkto id="632280059685224034" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Far Next</ap>
        <ap name="Position" type="literal">5</ap>
        <ap name="URL" type="csharp">host + "/LdapDirectory/NameSearchPagedResults?index=32&amp;forward=true&amp;far=true&amp;MetreosSessionId=" + routingGuid</ap>
        <rd field="ResultData">directory</rd>
      </Properties>
    </node>
    <node type="Action" id="632586799191496362" name="Query" class="MaxActionNode" group="" path="Metreos.Native.Ldap" x="695.1543" y="314">
      <linkto id="632280059685224019" type="Labeled" style="Bevel" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="LdapServerHost" type="variable">g_ldapHostname</ap>
        <ap name="LdapServerPort" type="variable">g_ldapPort</ap>
        <ap name="Username" type="variable">g_ldapUsername</ap>
        <ap name="Password" type="variable">g_ldapPassword</ap>
        <ap name="BaseDn" type="variable">g_ldapSearchBase</ap>
        <ap name="SearchFilter" type="variable">searchFilter</ap>
        <ap name="Attributes" type="csharp">new string[] { g_firstNameAttr, g_lastNameAttr, g_telephoneAttr }</ap>
        <rd field="SearchResults">g_results</rd>
        <log condition="exit" on="true" level="Info" type="csharp">"NameSearch resulted in " + g_results.Rows.Count + " entries"</log>
      </Properties>
    </node>
    <node type="Variable" id="632279476972418909" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">queryParams</Properties>
    </node>
    <node type="Variable" id="632279476972418910" name="isLastNameDefined" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Bool" refType="reference">isLastNameDefined</Properties>
    </node>
    <node type="Variable" id="632279476972418911" name="isFirstNameDefined" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Bool" refType="reference">isFirstNameDefined</Properties>
    </node>
    <node type="Variable" id="632279476972418912" name="searchFilter" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" refType="reference">searchFilter</Properties>
    </node>
    <node type="Variable" id="632279476972418913" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632279476972418914" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632279476972418915" name="input" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Input" refType="reference">input</Properties>
    </node>
    <node type="Variable" id="632280059685224022" name="noResultsText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">noResultsText</Properties>
    </node>
    <node type="Variable" id="632280059685224028" name="directory" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Directory" refType="reference">directory</Properties>
    </node>
    <node type="Variable" id="632280130832411596" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632281330439911598" name="sessionCookie" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" refType="reference">sessionCookie</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSessionExpired" startnode="632280059685224088" treenode="632497010124029438" appnode="632280059685224086" handlerfor="632280059685224085">
    <node type="Start" id="632280059685224088" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="271">
      <linkto id="632280059685224091" type="Basic" style="Bevel" />
    </node>
    <node type="Comment" id="632280059685224090" text="End this script instance if the user has not made a request in the configured session time period." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="192" />
    <node type="Action" id="632280059685224091" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="221" y="271">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>