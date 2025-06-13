<Application name="Login" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Login">
    <outline>
      <treenode type="evh" id="632677519608120255" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632677519608120252" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632677519608120251" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/ccem/login</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632677519608120296" level="2" text="Metreos.Providers.Http.GotRequest: ProcessLogin">
        <node type="function" name="ProcessLogin" id="632677519608120293" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632677519608120292" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/ccem/processlogin</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632829724024067048" level="2" text="Metreos.Providers.Http.SessionExpired: OnSessionExpired">
        <node type="function" name="OnSessionExpired" id="632829724024067045" path="Metreos.StockTools" />
        <node type="event" name="SessionExpired" id="632829724024067044" path="Metreos.Providers.Http.SessionExpired" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="fun" id="632677519608120270" level="1" text="ReportErrorToUser">
        <node type="function" name="ReportErrorToUser" id="632677519608120267" path="Metreos.StockTools" />
        <calls>
          <ref actid="632677519608120266" />
          <ref actid="632828901707138477" />
          <ref actid="632830568290421082" />
        </calls>
      </treenode>
      <treenode type="fun" id="632828901707138486" level="1" text="PromptLogin">
        <node type="function" name="PromptLogin" id="632828901707138483" path="Metreos.StockTools" />
        <calls>
          <ref actid="632828901707138482" />
          <ref actid="632829724024067037" />
        </calls>
      </treenode>
      <treenode type="fun" id="632830275979427454" level="1" text="RemoteLogin">
        <node type="function" name="RemoteLogin" id="632830275979427451" path="Metreos.StockTools" />
        <calls>
          <ref actid="632830275979427450" />
        </calls>
      </treenode>
      <treenode type="fun" id="632830275979427460" level="1" text="DetermineHomeCluster">
        <node type="function" name="DetermineHomeCluster" id="632830275979427457" path="Metreos.StockTools" />
        <calls>
          <ref actid="632830275979427456" />
        </calls>
      </treenode>
      <treenode type="fun" id="632833710832447658" level="1" text="SetForward">
        <node type="function" name="SetForward" id="632833710832447655" path="Metreos.StockTools" />
        <calls>
          <ref actid="632833710832447654" />
        </calls>
      </treenode>
      <treenode type="fun" id="632834579943547156" level="1" text="SetHotelPhone">
        <node type="function" name="SetHotelPhone" id="632834579943547153" path="Metreos.StockTools" />
        <calls>
          <ref actid="632834579943547152" />
        </calls>
      </treenode>
      <treenode type="fun" id="632834579943547161" level="1" text="GetUserPhoneInfo">
        <node type="function" name="GetUserPhoneInfo" id="632834579943547158" path="Metreos.StockTools" />
        <calls>
          <ref actid="632834579943547157" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_devicename" id="632853535766813258" vid="632677519608120261">
        <Properties type="String">g_devicename</Properties>
      </treenode>
      <treenode text="g_routingGuid" id="632853535766813260" vid="632677519608120290">
        <Properties type="String">g_routingGuid</Properties>
      </treenode>
      <treenode text="g_isHome" id="632853535766813262" vid="632828901707138175">
        <Properties type="Bool">g_isHome</Properties>
      </treenode>
      <treenode text="g_user" id="632853535766813264" vid="632828901707138177">
        <Properties type="String">g_user</Properties>
      </treenode>
      <treenode text="g_pin" id="632853535766813266" vid="632828901707138179">
        <Properties type="String">g_pin</Properties>
      </treenode>
      <treenode text="g_ldapUsername" id="632853535766813268" vid="632828901707138246">
        <Properties type="String" initWith="LDAPUsername">g_ldapUsername</Properties>
      </treenode>
      <treenode text="g_ldapPassword" id="632853535766813270" vid="632828901707138248">
        <Properties type="String" initWith="LDAPPassword">g_ldapPassword</Properties>
      </treenode>
      <treenode text="g_ccmIP" id="632853535766813272" vid="632828901707138319">
        <Properties type="String" initWith="CallManagerPubIP">g_ccmIP</Properties>
      </treenode>
      <treenode text="g_ldapBaseSearch" id="632853535766813274" vid="632828901707138392">
        <Properties type="String" initWith="LDAPBaseSearch">g_ldapBaseSearch</Properties>
      </treenode>
      <treenode text="g_EMUser" id="632853535766813276" vid="632828901707138395">
        <Properties type="String" initWith="EMUsername">g_EMUser</Properties>
      </treenode>
      <treenode text="g_EMPass" id="632853535766813278" vid="632828901707138397">
        <Properties type="String" initWith="EMPassword">g_EMPass</Properties>
      </treenode>
      <treenode text="g_remoteCcmIp" id="632853535766813280" vid="632830275979427706">
        <Properties type="String" initWith="CallManagerRemoteIP">g_remoteCcmIp</Properties>
      </treenode>
      <treenode text="g_loginDeviceIP" id="632853535766813282" vid="632830568290421084">
        <Properties type="String">g_loginDeviceIP</Properties>
      </treenode>
      <treenode text="g_hotelPhoneUser" id="632853535766813284" vid="632830568290421096">
        <Properties type="String" initWith="HotelPhoneUser">g_hotelPhoneUser</Properties>
      </treenode>
      <treenode text="g_hotelPhonePass" id="632853535766813286" vid="632830568290421098">
        <Properties type="String" initWith="HotelPhonePassword">g_hotelPhonePass</Properties>
      </treenode>
      <treenode text="g_ccmAxlUser" id="632853535766813288" vid="632834579943546433">
        <Properties type="String" initWith="CcmAXLUsername">g_ccmAxlUser</Properties>
      </treenode>
      <treenode text="g_ccmAxlPass" id="632853535766813290" vid="632834579943546435">
        <Properties type="String" initWith="CcmAXLPassword">g_ccmAxlPass</Properties>
      </treenode>
      <treenode text="g_convertAllLines" id="632853535766813292" vid="632834579943546605">
        <Properties type="Bool" initWith="ConvertAllLines">g_convertAllLines</Properties>
      </treenode>
      <treenode text="g_cfaRoutingPrefix" id="632853535766813294" vid="632834579943546610">
        <Properties type="String" initWith="CFARoutingPrefix">g_cfaRoutingPrefix</Properties>
      </treenode>
      <treenode text="g_cfaCSS" id="632853535766813296" vid="632834579943546960">
        <Properties type="String" initWith="CFACSS">g_cfaCSS</Properties>
      </treenode>
      <treenode text="g_phoneData" id="632853535766813298" vid="632851283574846703">
        <Properties type="Metreos.Types.Ccem.PhoneData">g_phoneData</Properties>
      </treenode>
      <treenode text="g_remotePhoneData" id="632853535766813300" vid="632851794044157796">
        <Properties type="Metreos.Types.Ccem.PhoneData">g_remotePhoneData</Properties>
      </treenode>
      <treenode text="g_ccmEMIP" id="632853535766813467" vid="632853535766813466">
        <Properties type="String" initWith="CallManagerHomeEMIP">g_ccmEMIP</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632677519608120254" treenode="632677519608120255" appnode="632677519608120252" handlerfor="632677519608120251">
    <node type="Start" id="632677519608120254" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="360">
      <linkto id="632677519608120265" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632677519608120264" text="Sanity Checking:&#xD;&#xA;&#xD;&#xA;1. Check that Device name is present in request" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="44" y="251" />
    <node type="Action" id="632677519608120265" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="176" y="360">
      <linkto id="632677519608120273" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632828901707138482" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.Http.QueryParamCollection queryParams, ref string errorMessage, LogWriter log, ref string g_devicename, string routingGuid, string g_routingGuid)
{
	g_routingGuid = routingGuid;
	g_devicename = queryParams["device"];

	if(g_devicename == null || g_devicename == String.Empty)
	{
		log.Write(TraceLevel.Error, "Unable to locate 'device' query parameter.");
		errorMessage = "Unable to locate 'device' query parameter.  Contact your administrator.";
		return "failure";
	}	

	return "success";

}
</Properties>
    </node>
    <node type="Action" id="632677519608120266" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="176" y="512" mx="231" my="528">
      <items count="1">
        <item text="ReportErrorToUser" />
      </items>
      <linkto id="632677519608120275" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="title" type="literal">Fail to Initiate EM</ap>
        <ap name="errorMessage" type="variable">errorMessage</ap>
        <ap name="FunctionName" type="literal">ReportErrorToUser</ap>
      </Properties>
    </node>
    <node type="Label" id="632677519608120272" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="88" y="528">
      <linkto id="632677519608120266" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632677519608120273" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="176" y="459" />
    <node type="Action" id="632677519608120275" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="376" y="528">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632677519608120288" text="Respond to Request:&#xD;&#xA;&#xD;&#xA;Enter Credentials:&#xD;&#xA;User:&#xD;&#xA;Pass:&#xD;&#xA;Home:&#xD;&#xA;&#xD;&#xA;Submit" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="358" y="213" />
    <node type="Action" id="632677519608120306" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="640" y="359">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632828901707138482" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="364.942383" y="342" mx="403" my="358">
      <items count="1">
        <item text="PromptLogin" />
      </items>
      <linkto id="632830568290421086" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="title" type="literal">Extension Mobility</ap>
        <ap name="prompt" type="literal">Enter Credentials</ap>
        <ap name="url" type="csharp">"http://" + host + "/ccem/processlogin?metreosSessionId=" + routingGuid</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="FunctionName" type="literal">PromptLogin</ap>
      </Properties>
    </node>
    <node type="Action" id="632830568290421086" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="547" y="360">
      <linkto id="632677519608120306" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">remoteIP</ap>
        <rd field="ResultData">g_loginDeviceIP</rd>
      </Properties>
    </node>
    <node type="Variable" id="632677519608120256" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632677519608120257" name="queryParams" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">queryParams</Properties>
    </node>
    <node type="Variable" id="632677519608120260" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
    <node type="Variable" id="632677519608120271" name="errorMessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">errorMessage</Properties>
    </node>
    <node type="Variable" id="632677519608120289" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632830568290421087" name="remoteIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteIpAddress" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteIP</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ProcessLogin" activetab="true" startnode="632677519608120295" treenode="632677519608120296" appnode="632677519608120293" handlerfor="632677519608120292">
    <node type="Start" id="632677519608120295" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="360">
      <linkto id="632677519608120298" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632677519608120298" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="168" y="360">
      <linkto id="632677519608120302" type="Labeled" style="Bezier" ortho="true" label="foreign" />
      <linkto id="632828901707138174" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(Metreos.Types.Http.QueryParamCollection query, ref bool g_isHome, ref string g_user, ref string g_pin)
{
	// Grab user and pin
	g_user = query["user"];
	g_pin = query["pin"];

	// Determine if the login request is foreign or home
	string home = query["home"];

	// Do some safety checks
	if(home == null) home = String.Empty;

	home = home.ToLower();
	
	if(home == "y" || home == "yes" || home == "true")
	{
		// in any of these conditions, consider it a home request	
		g_isHome = true;	
		return "home";
	}
	else
	{
		g_isHome = false;
		return "foreign";
	}	
}
</Properties>
    </node>
    <node type="Comment" id="632677519608120299" text="Determine if the user is on&#xD;&#xA;home cluster or foreign cluster.&#xD;&#xA;&#xD;&#xA;Also pull out user and pin" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="48" y="256" />
    <node type="Action" id="632677519608120302" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="311" y="419">
      <linkto id="632677519608120304" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Extension Mobility</ap>
        <ap name="Prompt" type="literal">Please Wait</ap>
        <ap name="Text" type="csharp">"Performing login... This may take a while when you are not at your home location." + "\n\nYour phone will display a notification when login is complete."</ap>
        <rd field="ResultData">pleaseWaitXml</rd>
      </Properties>
    </node>
    <node type="Action" id="632677519608120304" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="451" y="420">
      <linkto id="632830275979427450" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">pleaseWaitXml.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632677519608120305" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="736" y="421">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632828901707138173" text="Validate the user's credentials.&#xD;&#xA;In this example, we assume the user&#xD;&#xA;entered their pin associated with their&#xD;&#xA;AD/CCM account.  &#xD;&#xA;&#xD;&#xA;The ValidatePin action must be used against&#xD;&#xA;the CCM LDAP or a CCM-integrated AD." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="98" y="577" />
    <node type="Action" id="632828901707138174" name="ValidatePin" class="MaxActionNode" group="" path="Metreos.Native.CiscoExtensionMobility" x="296" y="208">
      <linkto id="632828901707138394" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632829724024067037" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Username" type="variable">g_user</ap>
        <ap name="Pin" type="variable">g_pin</ap>
        <ap name="LdapServerHost" type="variable">g_ccmIP</ap>
        <ap name="LdapServerPort" type="literal">8404</ap>
        <ap name="LdapUsername" type="variable">g_ldapUsername</ap>
        <ap name="LdapPassword" type="variable">g_ldapPassword</ap>
        <ap name="LdapBaseDn" type="variable">g_ldapBaseSearch</ap>
      </Properties>
    </node>
    <node type="Action" id="632828901707138394" name="Login" class="MaxActionNode" group="" path="Metreos.Native.CiscoExtensionMobility" x="469" y="208">
      <linkto id="632828901707138477" type="Labeled" style="Bezier" ortho="true" label="Failure" />
      <linkto id="632830275979427446" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="AppId" type="variable">g_EMUser</ap>
        <ap name="AppCertificate" type="variable">g_EMPass</ap>
        <ap name="UserId" type="variable">g_user</ap>
        <ap name="DeviceName" type="variable">g_devicename</ap>
        <ap name="NoTimeout" type="literal">true</ap>
        <ap name="CallManagerIP" type="variable">g_remoteCcmIp</ap>
        <ap name="Version" type="literal">other</ap>
        <rd field="ErrorCode">errorCode</rd>
        <rd field="ErrorMessage">errorMessage</rd>
      </Properties>
    </node>
    <node type="Action" id="632828901707138477" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="416" y="48" mx="471" my="64">
      <items count="1">
        <item text="ReportErrorToUser" />
      </items>
      <linkto id="632828901707138481" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="title" type="literal">Fail to Login</ap>
        <ap name="errorMessage" type="csharp">"Unable to login\n\nCode: " + errorCode + " \nMessage: " + errorMessage</ap>
        <ap name="FunctionName" type="literal">ReportErrorToUser</ap>
      </Properties>
    </node>
    <node type="Action" id="632828901707138481" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="616" y="64">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632829724024067037" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="258.499176" y="41" mx="297" my="57">
      <items count="1">
        <item text="PromptLogin" />
      </items>
      <linkto id="632829724024067040" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="title" type="literal">Extension Mobility</ap>
        <ap name="prompt" type="literal">Reenter Credentials</ap>
        <ap name="url" type="csharp">"http://" + host + "/ccem/processlogin?metreosSessionId=" + routingGuid</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="FunctionName" type="literal">PromptLogin</ap>
      </Properties>
    </node>
    <node type="Action" id="632829724024067040" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="148" y="59">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632829724024067041" text="Prompt for reenter&#xD;&#xA;of credentials" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="200" y="112" />
    <node type="Action" id="632830275979427446" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="593.999268" y="208">
      <linkto id="632830275979427447" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Extension Mobility</ap>
        <ap name="Prompt" type="literal">Please Wait</ap>
        <ap name="Text" type="csharp">"Performing login... "</ap>
        <rd field="ResultData">pleaseWaitXml</rd>
      </Properties>
    </node>
    <node type="Action" id="632830275979427447" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="734" y="208">
      <linkto id="632677519608120305" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">pleaseWaitXml.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632830275979427450" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="558.272461" y="405" mx="598" my="421">
      <items count="1">
        <item text="RemoteLogin" />
      </items>
      <linkto id="632677519608120305" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">RemoteLogin</ap>
      </Properties>
    </node>
    <node type="Variable" id="632677519608120297" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference" name="Metreos.Providers.Http.GotRequest">remoteHost</Properties>
    </node>
    <node type="Variable" id="632677519608120300" name="pleaseWaitXml" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">pleaseWaitXml</Properties>
    </node>
    <node type="Variable" id="632828901707138172" name="query" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">query</Properties>
    </node>
    <node type="Variable" id="632828901707138479" name="errorCode" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">errorCode</Properties>
    </node>
    <node type="Variable" id="632828901707138480" name="errorMessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">errorMessage</Properties>
    </node>
    <node type="Variable" id="632829724024067039" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632829724024067043" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSessionExpired" startnode="632829724024067047" treenode="632829724024067048" appnode="632829724024067045" handlerfor="632829724024067044">
    <node type="Start" id="632829724024067047" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="44" y="306">
      <linkto id="632829724024067049" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632829724024067049" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="553" y="313">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ReportErrorToUser" startnode="632677519608120269" treenode="632677519608120270" appnode="632677519608120267" handlerfor="632829724024067044">
    <node type="Start" id="632677519608120269" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="47" y="358">
      <linkto id="632830568290421090" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632677519608120280" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="499" y="289">
      <linkto id="632677519608120283" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">errorXml.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632677519608120282" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="331" y="289">
      <linkto id="632677519608120280" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="variable">title</ap>
        <ap name="Prompt" type="variable">prompt</ap>
        <ap name="Text" type="variable">errorMessage</ap>
        <rd field="ResultData">errorXml</rd>
      </Properties>
    </node>
    <node type="Action" id="632677519608120283" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="667" y="289">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632830568290421090" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="169" y="358">
      <linkto id="632677519608120282" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632830568290421094" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">isPush</ap>
      </Properties>
    </node>
    <node type="Comment" id="632830568290421091" text="IF isPush is true, then send a Cisco IP Phone Execute command,&#xD;&#xA;otherwise send an HTTP response to the phone..." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="101" y="189" />
    <node type="Action" id="632830568290421094" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="328" y="485">
      <linkto id="632830568290421095" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="variable">title</ap>
        <ap name="Prompt" type="variable">prompt</ap>
        <ap name="Text" type="variable">text</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632830568290421095" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="490" y="486">
      <linkto id="632830568290421246" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">text.ToString()</ap>
        <ap name="URL" type="variable">remoteHost</ap>
        <ap name="Username" type="variable">g_hotelPhoneUser</ap>
        <ap name="Password" type="variable">g_hotelPhonePass</ap>
      </Properties>
    </node>
    <node type="Action" id="632830568290421246" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="656" y="486">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632677519608120276" name="title" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="title" defaultInitWith="Extension Mobility" refType="reference">title</Properties>
    </node>
    <node type="Variable" id="632677519608120277" name="prompt" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="prompt" defaultInitWith="Exit" refType="reference">prompt</Properties>
    </node>
    <node type="Variable" id="632677519608120278" name="errorMessage" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="errorMessage" defaultInitWith="An error occured." refType="reference">errorMessage</Properties>
    </node>
    <node type="Variable" id="632677519608120279" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632677519608120281" name="errorXml" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">errorXml</Properties>
    </node>
    <node type="Variable" id="632830568290421089" name="isPush" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Bool" initWith="isPush" defaultInitWith="false" refType="reference">isPush</Properties>
    </node>
    <node type="Variable" id="632830568290421092" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="PromptLogin" startnode="632828901707138485" treenode="632828901707138486" appnode="632828901707138483" handlerfor="632829724024067044">
    <node type="Start" id="632828901707138485" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="74" y="329">
      <linkto id="632829724024067021" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632829724024067021" name="CreateInput" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="224.608246" y="328">
      <linkto id="632829724024067023" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="variable">title</ap>
        <ap name="Prompt" type="variable">prompt</ap>
        <ap name="URL" type="variable">url</ap>
        <rd field="ResultData">credentialsXml</rd>
      </Properties>
    </node>
    <node type="Action" id="632829724024067022" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="664.6081" y="328">
      <linkto id="632829724024067042" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="csharp">credentialsXml.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632829724024067023" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="360.608215" y="328">
      <linkto id="632829724024067024" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DisplayName" type="literal">User</ap>
        <ap name="QueryStringParam" type="literal">user</ap>
        <ap name="InputFlags" type="literal">A</ap>
        <rd field="ResultData">credentialsXml</rd>
      </Properties>
    </node>
    <node type="Action" id="632829724024067024" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="464.608246" y="328">
      <linkto id="632829724024067025" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DisplayName" type="literal">Pin</ap>
        <ap name="QueryStringParam" type="literal">pin</ap>
        <ap name="InputFlags" type="literal">NP</ap>
        <rd field="ResultData">credentialsXml</rd>
      </Properties>
    </node>
    <node type="Action" id="632829724024067025" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="560.6081" y="328">
      <linkto id="632829724024067022" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DisplayName" type="literal">Home</ap>
        <ap name="QueryStringParam" type="literal">home</ap>
        <ap name="InputFlags" type="literal">A</ap>
        <rd field="ResultData">credentialsXml</rd>
      </Properties>
    </node>
    <node type="Comment" id="632829724024067033" text="Respond to Request:&#xD;&#xA;&#xD;&#xA;Enter Credentials:&#xD;&#xA;User:&#xD;&#xA;Pass:&#xD;&#xA;Home:&#xD;&#xA;&#xD;&#xA;Submit" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="202" y="183" />
    <node type="Comment" id="632829724024067034" text="The Home field is just for testing--&#xD;&#xA;there are various ways to determine if a&#xD;&#xA;user is home or remote automatically,&#xD;&#xA;but for this example app, we just ask...." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="404" y="386" />
    <node type="Action" id="632829724024067042" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="765" y="327">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632829724024067018" name="title" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="title" refType="reference">title</Properties>
    </node>
    <node type="Variable" id="632829724024067019" name="prompt" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="prompt" refType="reference">prompt</Properties>
    </node>
    <node type="Variable" id="632829724024067020" name="url" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="url" refType="reference">url</Properties>
    </node>
    <node type="Variable" id="632829724024067031" name="credentialsXml" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Input" refType="reference">credentialsXml</Properties>
    </node>
    <node type="Variable" id="632829724024067032" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="RemoteLogin" startnode="632830275979427453" treenode="632830275979427454" appnode="632830275979427451" handlerfor="632829724024067044">
    <node type="Start" id="632830275979427453" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="35" y="362">
      <linkto id="632830275979427456" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632830275979427455" text="Determine the user's home cluster..." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="89" y="313" />
    <node type="Action" id="632830275979427456" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="121.240234" y="345" mx="188" my="361">
      <items count="1">
        <item text="DetermineHomeCluster" />
      </items>
      <linkto id="632830275979427710" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">DetermineHomeCluster</ap>
        <rd field="remotePubIP">remotePubIP</rd>
      </Properties>
    </node>
    <node type="Comment" id="632830275979427708" text="Once we know their home cluster, how do we determine what their&#xD;&#xA;home device or device profile is?&#xD;&#xA;&#xD;&#xA;If they have a permanent desk phone, with a device profile that is always&#xD;&#xA;logged in there, then we could use EM QueryUsers, which returns the device &#xD;&#xA;that the user is logged into.&#xD;&#xA;&#xD;&#xA;There are other ways to do it--for instance, one can find the name of the default&#xD;&#xA;device profile associated with the user in LDAP, which doesn't require the user&#xD;&#xA;to be logged in anywhere (but of course does require one to know their home cluster&#xD;&#xA;if relying on the built-in CCM LDAP server)" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="232" y="97" />
    <node type="Action" id="632830275979427709" name="GetUserDevices" class="MaxActionNode" group="" path="Metreos.Native.CiscoExtensionMobility" x="553" y="364">
      <linkto id="632830568290421075" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Username" type="variable">g_user</ap>
        <ap name="QueryUserResults" type="variable">queryUserResults</ap>
        <rd field="Devices">devices</rd>
      </Properties>
    </node>
    <node type="Action" id="632830275979427710" name="QueryUsers" class="MaxActionNode" group="" path="Metreos.Native.CiscoExtensionMobility" x="385" y="363">
      <linkto id="632830275979427709" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="AppId" type="variable">g_EMUser</ap>
        <ap name="AppCertificate" type="variable">g_EMPass</ap>
        <ap name="Users" type="csharp">new string[] { g_user }</ap>
        <ap name="CallManagerIP" type="variable">g_ccmEMIP</ap>
        <ap name="Version" type="literal">other</ap>
        <rd field="QueryUsersResult">queryUserResults</rd>
      </Properties>
    </node>
    <node type="Action" id="632830568290421075" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="707" y="364">
      <linkto id="632830568290421082" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <linkto id="632834579943547157" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">0</ap>
        <ap name="Value2" type="csharp">devices.Count</ap>
      </Properties>
    </node>
    <node type="Comment" id="632830568290421076" text="Check to see if there are no associated devices" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="569" y="283" />
    <node type="Action" id="632830568290421082" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="654.7516" y="487" mx="710" my="503">
      <items count="1">
        <item text="ReportErrorToUser" />
      </items>
      <linkto id="632830568290421088" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="remoteHost" type="variable">g_loginDeviceIP</ap>
        <ap name="title" type="literal">Fail to Login</ap>
        <ap name="errorMessage" type="csharp">"Unable to determine your phone on the home cluster"</ap>
        <ap name="isPush" type="csharp">true</ap>
        <ap name="FunctionName" type="literal">ReportErrorToUser</ap>
      </Properties>
    </node>
    <node type="Action" id="632830568290421088" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="707.7516" y="641">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632833710832447654" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1064" y="352" mx="1101" my="368">
      <items count="1">
        <item text="SetForward" />
      </items>
      <linkto id="632834579943546963" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632834579943546964" type="Labeled" style="Bezier" ortho="true" label="failure" />
      <Properties final="false" type="appControl" log="On">
        <ap name="pubIP" type="variable">g_ccmIP</ap>
        <ap name="device" type="csharp">devices[0]</ap>
        <ap name="FunctionName" type="literal">SetForward</ap>
      </Properties>
    </node>
    <node type="Action" id="632834579943546963" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1232" y="368">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632834579943546964" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1096" y="504">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632834579943547152" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="928" y="352" mx="972" my="368">
      <items count="1">
        <item text="SetHotelPhone" />
      </items>
      <linkto id="632833710832447654" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="device" type="variable">g_devicename</ap>
        <ap name="pubIP" type="variable">g_remoteCcmIp</ap>
        <ap name="FunctionName" type="literal">SetHotelPhone</ap>
      </Properties>
    </node>
    <node type="Action" id="632834579943547157" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="777" y="352" mx="831" my="368">
      <items count="1">
        <item text="GetUserPhoneInfo" />
      </items>
      <linkto id="632834579943547152" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="device" type="csharp">devices[0]</ap>
        <ap name="pubIP" type="variable">g_ccmIP</ap>
        <ap name="FunctionName" type="literal">GetUserPhoneInfo</ap>
      </Properties>
    </node>
    <node type="Variable" id="632830275979427461" name="remotePubIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">remotePubIP</Properties>
    </node>
    <node type="Variable" id="632830275979427711" name="queryUserResults" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoExtensionMobility.QueryUserResults" refType="reference">queryUserResults</Properties>
    </node>
    <node type="Variable" id="632830568290421073" name="devices" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="StringCollection" refType="reference">devices</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="DetermineHomeCluster" startnode="632830275979427459" treenode="632830275979427460" appnode="632830275979427457" handlerfor="632829724024067044">
    <node type="Start" id="632830275979427459" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="361">
      <linkto id="632830275979427584" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632830275979427462" text="There are multiple ways to determine the home cluster.&#xD;&#xA;&#xD;&#xA;The prototype cheats in that it uses a config setting, and &#xD;&#xA;passes that back in the EndFunction action." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="183" y="165" />
    <node type="Action" id="632830275979427584" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="364" y="362">
      <Properties final="true" type="appControl" log="On">
        <ap name="remotePubIP" type="variable">g_remoteCcmIp</ap>
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="SetForward" startnode="632833710832447657" treenode="632833710832447658" appnode="632833710832447655" handlerfor="632829724024067044">
    <node type="Loop" id="632834579943546609" name="Loop" text="loop (expr)" cx="527" cy="235" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="210" y="248" mx="474" my="366">
      <linkto id="632834579943546783" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632834579943546962" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="csharp">g_phoneData.Data.Lines</Properties>
    </node>
    <node type="Start" id="632833710832447657" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="368">
      <linkto id="632834579943546609" port="1" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Comment" id="632834506736515728" text="Required Inputs:&#xD;&#xA;&#xD;&#xA;'device' to SetForward on&#xD;&#xA;'pubIP' of the device" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="64" y="72" />
    <node type="Comment" id="632834579943546781" text="Assumptions:&#xD;&#xA;&#xD;&#xA;g_cfaRoutingPrefix is sufficent to decide the prepend&#xD;&#xA;g_cfaCSS is sufficient to set the CFA CSS" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="344" y="80" />
    <node type="Action" id="632834579943546782" name="UpdateLine" container="632834579943546609" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="634" y="360">
      <linkto id="632834579943546609" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Uuid" type="csharp">(loopEnum.Current as LineInfo).LineId</ap>
        <ap name="CallForwardAll" type="variable">callForwardAll</ap>
        <ap name="CallManagerIP" type="variable">ccmIP</ap>
        <ap name="AdminUsername" type="variable">g_ccmAxlUser</ap>
        <ap name="AdminPassword" type="variable">g_ccmAxlPass</ap>
      </Properties>
    </node>
    <node type="Action" id="632834579943546783" name="CreateForward" container="632834579943546609" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="375" y="363">
      <linkto id="632834579943546782" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Destination" type="csharp">g_cfaRoutingPrefix + (loopEnum.Current as LineInfo).Pattern</ap>
        <ap name="CallingSearchSpaceName" type="variable">g_cfaCSS</ap>
        <rd field="Forward">callForwardAll</rd>
      </Properties>
    </node>
    <node type="Action" id="632834579943546962" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="901" y="366">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Comment" id="632834579943547144" text="Retrieve the user's logged into phone, &#xD;&#xA;and from that determine the profile name.&#xD;&#xA;&#xD;&#xA;This can be done from LDAP as well--this would be better." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="32" y="192" />
    <node type="Variable" id="632833710832447660" name="device" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="device" refType="reference">device</Properties>
    </node>
    <node type="Variable" id="632833710832447661" name="ccmIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="pubIP" refType="reference">ccmIP</Properties>
    </node>
    <node type="Variable" id="632834579943546437" name="fault" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">fault</Properties>
    </node>
    <node type="Variable" id="632834579943546438" name="code" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">code</Properties>
    </node>
    <node type="Variable" id="632834579943546439" name="getPhoneResponse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.GetPhoneResponse" refType="reference">getPhoneResponse</Properties>
    </node>
    <node type="Variable" id="632834579943546607" name="lineIds" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="ArrayList" refType="reference">lineIds</Properties>
    </node>
    <node type="Variable" id="632834579943546786" name="callForwardAll" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.CallForward" refType="reference">callForwardAll</Properties>
    </node>
    <node type="Variable" id="632834579943547146" name="getDeviceProfileResponse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.GetDeviceProfileResponse" refType="reference">getDeviceProfileResponse</Properties>
    </node>
    <node type="Variable" id="632834579943547151" name="getLineResponse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.GetLineResponse" refType="reference">getLineResponse</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="SetHotelPhone" startnode="632834579943547155" treenode="632834579943547156" appnode="632834579943547153" handlerfor="632829724024067044">
    <node type="Loop" id="632851794044157799" name="Loop" text="loop (expr)" cx="727" cy="471" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="544" y="104" mx="908" my="340">
      <linkto id="632851794044157810" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632851794044157835" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="csharp">g_phoneData.Data.Lines</Properties>
    </node>
    <node type="Start" id="632834579943547155" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="72" y="344">
      <linkto id="632851794044157795" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632851794044157795" name="GetPhone" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="216" y="344">
      <linkto id="632851794044157798" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="PhoneName" type="variable">device</ap>
        <ap name="CallManagerIP" type="variable">ccmIp</ap>
        <ap name="AdminUsername" type="variable">g_ccmAxlUser</ap>
        <ap name="AdminPassword" type="variable">g_ccmAxlPass</ap>
        <rd field="GetPhoneResponse">getPhoneResponse</rd>
      </Properties>
    </node>
    <node type="Action" id="632851794044157798" name="PopulatePhoneData" class="MaxActionNode" group="" path="Metreos.Native.Ccem" x="344" y="344">
      <linkto id="632851794044157799" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="AxlPhoneResponse" type="csharp">getPhoneResponse.Response</ap>
        <rd field="PhoneData">g_remotePhoneData</rd>
      </Properties>
    </node>
    <node type="Comment" id="632851794044157800" text="For each line defined on the home phone:&#xD;&#xA;&#xD;&#xA;1) Update the primary line on the remote device&#xD;&#xA;2) Remove this line from any other phones&#xD;&#xA;         because shared-line will cause the remote &#xD;&#xA;         phone and some other phone to ring. (Not possible today)&#xD;&#xA;" container="632851794044157799" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="576" y="184" />
    <node type="Action" id="632851794044157802" name="UpdateLine" container="632851794044157799" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="944" y="448">
      <linkto id="632851794044157836" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="NewRoutePartitionName" type="csharp">(loopEnum.Current as LineInfo).Partition.Name</ap>
        <ap name="ShareLineAppearanceCSSName" type="csharp">(loopEnum.Current as LineInfo).css.Name</ap>
        <ap name="NewPattern" type="csharp">(loopEnum.Current as LineInfo).Pattern</ap>
        <ap name="Uuid" type="csharp">g_remotePhoneData.Data.Lines[0].LineId</ap>
        <ap name="CallForwardBusy" type="variable">busyExt</ap>
        <ap name="CallForwardNoAnswer" type="variable">noAnswerExt</ap>
        <ap name="CallManagerIP" type="variable">ccmIp</ap>
        <ap name="AdminUsername" type="variable">g_ccmAxlUser</ap>
        <ap name="AdminPassword" type="variable">g_ccmAxlPass</ap>
      </Properties>
    </node>
    <node type="Action" id="632851794044157803" name="CreateForward" container="632851794044157799" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="736" y="144">
      <linkto id="632851794044157812" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Destination" type="csharp">(loopEnum.Current as LineInfo).BusyInt.Destination</ap>
        <ap name="Duration" type="csharp">(loopEnum.Current as LineInfo).BusyInt.Duration</ap>
        <ap name="ForwardToVoiceMail" type="csharp">(loopEnum.Current as LineInfo).BusyInt.ForwardToVoiceMail
</ap>
        <ap name="CallingSearchSpaceName" type="csharp">(loopEnum.Current as LineInfo).BusyInt.css.Name</ap>
        <rd field="Forward">busyInt</rd>
      </Properties>
    </node>
    <node type="Action" id="632851794044157810" name="If" container="632851794044157799" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="592" y="240">
      <linkto id="632851794044157803" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632851794044157812" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">(loopEnum.Current as LineInfo).BusyInt.Destination != null &amp;&amp; (loopEnum.Current as LineInfo).BusyInt.Destination != String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="632851794044157811" name="CreateForward" container="632851794044157799" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="896" y="144">
      <linkto id="632851794044157816" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Destination" type="csharp">(loopEnum.Current as LineInfo).BusyInt.Destination</ap>
        <ap name="Duration" type="csharp">(loopEnum.Current as LineInfo).BusyInt.Duration</ap>
        <ap name="ForwardToVoiceMail" type="csharp">(loopEnum.Current as LineInfo).BusyInt.ForwardToVoiceMail
</ap>
        <ap name="CallingSearchSpaceName" type="csharp">(loopEnum.Current as LineInfo).BusyExt.css.Name</ap>
        <rd field="Forward">busyExt</rd>
      </Properties>
    </node>
    <node type="Action" id="632851794044157812" name="If" container="632851794044157799" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="736" y="240">
      <linkto id="632851794044157811" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632851794044157816" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">(loopEnum.Current as LineInfo).BusyExt.Destination != null &amp;&amp; (loopEnum.Current as LineInfo).BusyExt.Destination != String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="632851794044157815" name="CreateForward" container="632851794044157799" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="1048" y="144">
      <linkto id="632851794044157820" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Destination" type="csharp">(loopEnum.Current as LineInfo).NoAnswerExt.Destination</ap>
        <ap name="Duration" type="csharp">(loopEnum.Current as LineInfo).NoAnswerExt.Duration</ap>
        <ap name="ForwardToVoiceMail" type="csharp">(loopEnum.Current as LineInfo).NoAnswerExt.ForwardToVoiceMail
</ap>
        <ap name="CallingSearchSpaceName" type="csharp">(loopEnum.Current as LineInfo).NoAnswerExt.css.Name</ap>
        <rd field="Forward">noAnswerExt</rd>
      </Properties>
    </node>
    <node type="Action" id="632851794044157816" name="If" container="632851794044157799" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="896" y="240">
      <linkto id="632851794044157815" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632851794044157820" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">(loopEnum.Current as LineInfo).NoAnswerExt.Destination != null &amp;&amp; (loopEnum.Current as LineInfo).NoAnswerExt.Destination != String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="632851794044157819" name="CreateForward" container="632851794044157799" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="1208" y="144">
      <linkto id="632851794044157828" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Destination" type="csharp">(loopEnum.Current as LineInfo).NoAnswerInt.Destination</ap>
        <ap name="Duration" type="csharp">(loopEnum.Current as LineInfo).NoAnswerInt.Duration</ap>
        <ap name="ForwardToVoiceMail" type="csharp">(loopEnum.Current as LineInfo).NoAnswerInt.ForwardToVoiceMail
</ap>
        <ap name="CallingSearchSpaceName" type="csharp">(loopEnum.Current as LineInfo).NoAnswerInt.css.Name</ap>
        <rd field="Forward">noAnswerInt</rd>
      </Properties>
    </node>
    <node type="Action" id="632851794044157820" name="If" container="632851794044157799" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="1048" y="240">
      <linkto id="632851794044157819" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632851794044157829" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">(loopEnum.Current as LineInfo).NoAnswerInt.Destination != null &amp;&amp; (loopEnum.Current as LineInfo).NoAnswerInt.Destination != String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="632851794044157824" name="If" container="632851794044157799" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="624" y="448">
      <linkto id="632851794044157825" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632851794044157832" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">(loopEnum.Current as LineInfo).NoCoverageExt.Destination != null &amp;&amp; (loopEnum.Current as LineInfo).NoCoverageExt.Destination != String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="632851794044157825" name="CreateForward" container="632851794044157799" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="784" y="352">
      <linkto id="632851794044157832" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Destination" type="csharp">(loopEnum.Current as LineInfo).NoCoverageExt.Destination</ap>
        <ap name="Duration" type="csharp">(loopEnum.Current as LineInfo).NoCoverageExt.Duration</ap>
        <ap name="ForwardToVoiceMail" type="csharp">(loopEnum.Current as LineInfo).NoCoverageExt.ForwardToVoiceMail
</ap>
        <ap name="CallingSearchSpaceName" type="csharp">(loopEnum.Current as LineInfo).NoCoverageExt.css.Name</ap>
        <rd field="Forward">noCoverageExt</rd>
      </Properties>
    </node>
    <node type="Label" id="632851794044157828" text="a" container="632851794044157799" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1216" y="224" />
    <node type="Label" id="632851794044157829" text="a" container="632851794044157799" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="1200" y="240" />
    <node type="Label" id="632851794044157830" text="a" container="632851794044157799" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="568" y="448">
      <linkto id="632851794044157824" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632851794044157831" name="CreateForward" container="632851794044157799" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="944" y="352">
      <linkto id="632851794044157802" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Destination" type="csharp">(loopEnum.Current as LineInfo).NoCoverageInt.Destination</ap>
        <ap name="Duration" type="csharp">(loopEnum.Current as LineInfo).NoCoverageInt.Duration</ap>
        <ap name="ForwardToVoiceMail" type="csharp">(loopEnum.Current as LineInfo).NoCoverageInt.ForwardToVoiceMail
</ap>
        <ap name="CallingSearchSpaceName" type="csharp">(loopEnum.Current as LineInfo).NoCoverageInt.css.Name</ap>
        <rd field="Forward">noCoverageInt</rd>
      </Properties>
    </node>
    <node type="Action" id="632851794044157832" name="If" container="632851794044157799" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="784" y="448">
      <linkto id="632851794044157831" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632851794044157802" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="csharp">(loopEnum.Current as LineInfo).NoCoverageInt.Destination != null &amp;&amp; (loopEnum.Current as LineInfo).NoCoverageInt.Destination != String.Empty</ap>
      </Properties>
    </node>
    <node type="Action" id="632851794044157835" name="UpdatePhone" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="1424" y="336">
      <linkto id="632851794044157838" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Description" type="literal">LOGGED IN USING CCEM</ap>
        <ap name="PhoneName" type="variable">device</ap>
        <ap name="CallingSearchSpaceName" type="csharp">g_phoneData.Data.DeviceCSSName</ap>
        <ap name="Lines" type="variable">lines</ap>
        <ap name="CallManagerIP" type="variable">ccmIp</ap>
        <ap name="AdminUsername" type="variable">g_ccmAxlUser</ap>
        <ap name="AdminPassword" type="variable">g_ccmAxlPass</ap>
      </Properties>
    </node>
    <node type="Action" id="632851794044157836" name="AddLineItem" container="632851794044157799" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="1080" y="448">
      <linkto id="632851794044157799" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Display" type="csharp">(loopEnum.Current as LineInfo).DisplayName</ap>
        <ap name="E164Mask" type="csharp">(loopEnum.Current as LineInfo).E164Mask</ap>
        <ap name="Index" type="csharp">(loopEnum.Current as LineInfo).Index</ap>
        <ap name="Label" type="csharp">(loopEnum.Current as LineInfo).Label</ap>
        <ap name="DirectoryNumberId" type="csharp">g_remotePhoneData.Data.Lines[0].LineId</ap>
        <rd field="Line">lines</rd>
      </Properties>
    </node>
    <node type="Action" id="632851794044157838" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1560" y="336">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632851794044157791" name="device" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="device" refType="reference">device</Properties>
    </node>
    <node type="Variable" id="632851794044157792" name="ccmIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="PubIP" refType="reference">ccmIp</Properties>
    </node>
    <node type="Variable" id="632851794044157793" name="getPhoneResponse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.GetPhoneResponse" refType="reference">getPhoneResponse</Properties>
    </node>
    <node type="Variable" id="632851794044157794" name="getLineResponse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.UpdateLineResponse" refType="reference">getLineResponse</Properties>
    </node>
    <node type="Variable" id="632851794044157804" name="busyInt" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.CallForward" refType="reference">busyInt</Properties>
    </node>
    <node type="Variable" id="632851794044157805" name="busyExt" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.CallForward" refType="reference">busyExt</Properties>
    </node>
    <node type="Variable" id="632851794044157806" name="noAnswerInt" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.CallForward" refType="reference">noAnswerInt</Properties>
    </node>
    <node type="Variable" id="632851794044157807" name="noAnswerExt" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.CallForward" refType="reference">noAnswerExt</Properties>
    </node>
    <node type="Variable" id="632851794044157808" name="noCoverageInt" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.CallForward" refType="reference">noCoverageInt</Properties>
    </node>
    <node type="Variable" id="632851794044157809" name="noCoverageExt" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.CallForward" refType="reference">noCoverageExt</Properties>
    </node>
    <node type="Variable" id="632851794044157837" name="lines" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.Lines" refType="reference">lines</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="GetUserPhoneInfo" startnode="632834579943547160" treenode="632834579943547161" appnode="632834579943547158" handlerfor="632829724024067044">
    <node type="Loop" id="632834840330576905" name="Loop" text="loop (var)" cx="576.957031" cy="235" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="551" y="219" mx="839" my="336">
      <linkto id="632834840330576908" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632851283574846711" fromport="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="enum" type="variable">lineIds</Properties>
    </node>
    <node type="Start" id="632834579943547160" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="64" y="336">
      <linkto id="632834579943547162" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632834579943547162" name="GetPhone" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="176" y="336">
      <linkto id="632851283574846705" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="PhoneName" type="variable">device</ap>
        <ap name="CallManagerIP" type="variable">ccmIP</ap>
        <ap name="AdminUsername" type="variable">g_ccmAxlUser</ap>
        <ap name="AdminPassword" type="variable">g_ccmAxlPass</ap>
        <rd field="GetPhoneResponse">getPhoneResponse</rd>
        <rd field="FaultMessage">fault</rd>
        <rd field="FaultCode">code</rd>
      </Properties>
    </node>
    <node type="Action" id="632834579943547163" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="433" y="335">
      <linkto id="632834840330576905" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
	public static string Execute(Metreos.Types.AxlSoap413.GetPhoneResponse getPhoneResponse, ref bool g_convertAllLines, ArrayList lineIds, LogWriter log)
	{
		// if we can find no line information, then this device must have
            // no lines, and so is not useful for this demo.
		if(getPhoneResponse.Response.@return.device.lines == null ||
               getPhoneResponse.Response.@return.device.lines.Items == null ||
               getPhoneResponse.Response.@return.device.lines.Items.Length == 0)
            {
		     log.Write(TraceLevel.Error, "Home phone was determined to have 0 lines... no CFA set");
                 return "nolines";
            }

		if(g_convertAllLines)
		{
			foreach(object lineObj in getPhoneResponse.Response.@return.device.lines.Items)
			{
				Metreos.AxlSoap413.XLine lineInfo = (Metreos.AxlSoap413.XLine) lineObj;
				string pattern = lineInfo.Item.pattern;
				log.Write(TraceLevel.Verbose, "Pattern: " + pattern);		
				lineIds.Add( lineInfo.Item.uuid );
			}
		}
		else
		{
			Metreos.AxlSoap413.XLine lineInfo = (Metreos.AxlSoap413.XLine) getPhoneResponse.Response.@return.device.lines.Items[0]; 
			string pattern = lineInfo.Item.pattern;
			log.Write(TraceLevel.Verbose, "Pattern: " + pattern);
			lineIds.Add( lineInfo.Item.uuid );
		}	

		return String.Empty;
	}
</Properties>
    </node>
    <node type="Comment" id="632834579943547168" text="Required Inputs:&#xD;&#xA;&#xD;&#xA;'device' to SetForward on&#xD;&#xA;'pubIP' of the device" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="33" y="87" />
    <node type="Action" id="632834840330576908" name="GetLine" container="632834840330576905" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="670" y="331">
      <linkto id="632851283574846707" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="Uuid" type="csharp">loopEnum.Current as string</ap>
        <ap name="CallManagerIP" type="variable">ccmIP</ap>
        <ap name="AdminUsername" type="variable">g_ccmAxlUser</ap>
        <ap name="AdminPassword" type="variable">g_ccmAxlPass</ap>
        <rd field="GetLineResponse">getLineResponse</rd>
      </Properties>
    </node>
    <node type="Comment" id="632834840330576912" text="getPhone does return CSS name" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="226" y="161" />
    <node type="Action" id="632851283574846705" name="PopulatePhoneData" class="MaxActionNode" group="" path="Metreos.Native.Ccem" x="298" y="338">
      <linkto id="632834579943547163" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="AxlPhoneResponse" type="csharp">getPhoneResponse.Response</ap>
        <rd field="PhoneData">g_phoneData</rd>
      </Properties>
    </node>
    <node type="Action" id="632851283574846707" name="PopulateLineData" container="632834840330576905" class="MaxActionNode" group="" path="Metreos.Native.Ccem" x="780.1791" y="331">
      <linkto id="632851773524857342" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="AxlLineResponse" type="csharp">getLineResponse.Response</ap>
        <rd field="LineData">g_phoneData</rd>
      </Properties>
    </node>
    <node type="Action" id="632851283574846709" name="PopulatePartitionData" container="632834840330576905" class="MaxActionNode" group="" path="Metreos.Native.Ccem" x="1054.5" y="335">
      <linkto id="632834840330576905" port="2" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="AxlPartitionResponse" type="variable">partitionResponse</ap>
        <rd field="PartitionData">g_cfaCSS</rd>
      </Properties>
    </node>
    <node type="Action" id="632851283574846710" name="GetPartition" container="632834840330576905" class="MaxActionNode" group="" path="Metreos.Native.AxlSoap413" x="914.5" y="331">
      <linkto id="632851283574846709" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="Uuid" type="csharp">(getLineResponse.Response.@return.directoryNumber.Item as Metreos.AxlSoap413.XRoutePartition).uuid</ap>
        <ap name="CallManagerIP" type="variable">ccmIP</ap>
        <ap name="AdminUsername" type="variable">g_ccmAxlUser</ap>
        <ap name="AdminPassword" type="variable">g_ccmAxlPass</ap>
        <rd field="GetRoutePartitionResponse">partitionResponse</rd>
      </Properties>
    </node>
    <node type="Action" id="632851283574846711" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1298.36914" y="337">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632851773524857342" name="Sleep" container="632834840330576905" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="843" y="278">
      <linkto id="632851283574846710" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SleepTime" type="literal">200</ap>
      </Properties>
    </node>
    <node type="Variable" id="632834579943547166" name="device" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="device" refType="reference">device</Properties>
    </node>
    <node type="Variable" id="632834579943547167" name="ccmIP" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="pubIP" refType="reference">ccmIP</Properties>
    </node>
    <node type="Variable" id="632834579943547170" name="code" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">code</Properties>
    </node>
    <node type="Variable" id="632834579943547171" name="fault" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">fault</Properties>
    </node>
    <node type="Variable" id="632834579943547172" name="getPhoneResponse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.GetPhoneResponse" refType="reference">getPhoneResponse</Properties>
    </node>
    <node type="Variable" id="632834579943547173" name="lineIds" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="ArrayList" refType="reference">lineIds</Properties>
    </node>
    <node type="Variable" id="632851283574846694" name="cssResponse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.GetCSSResponse" refType="reference">cssResponse</Properties>
    </node>
    <node type="Variable" id="632851283574846695" name="partitionResponse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.GetPartitionResponse" refType="reference">partitionResponse</Properties>
    </node>
    <node type="Variable" id="632851283574846708" name="getLineResponse" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.AxlSoap413.GetLineResponse" refType="reference">getLineResponse</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>