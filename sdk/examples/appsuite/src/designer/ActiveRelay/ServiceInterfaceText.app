<Application name="ServiceInterfaceText" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="ServiceInterfaceText">
    <outline>
      <treenode type="evh" id="632340365902188663" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632340365902188660" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632340365902188659" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/ActiveRelayFindMe</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632345018984531467" level="2" text="Metreos.Events.ActiveRelay.FindMeServiceRequestResp: OnFindMeServiceRequestResp">
        <node type="function" name="OnFindMeServiceRequestResp" id="632345018984531464" path="Metreos.StockTools" />
        <node type="event" name="FindMeServiceRequestResp" id="632345018984531463" path="Metreos.Events.ActiveRelay.FindMeServiceRequestResp" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632345614710626236" level="2" text="Metreos.Providers.Http.GotRequest: OnLoginRequest">
        <node type="function" name="OnLoginRequest" id="632345614710626233" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632345614710626232" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/ActiveRelayFindMe/Login</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632348338276875932" level="2" text="Metreos.Providers.Http.SessionExpired: OnSessionExpired">
        <node type="function" name="OnSessionExpired" id="632348338276875929" path="Metreos.StockTools" />
        <node type="event" name="SessionExpired" id="632348338276875928" path="Metreos.Providers.Http.SessionExpired" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632779604614957047" level="2" text="Metreos.Providers.TimerFacility.TimerFire: OnTimerFire">
        <node type="function" name="OnTimerFire" id="632779604614957044" path="Metreos.StockTools" />
        <node type="event" name="TimerFire" id="632779604614957043" path="Metreos.Providers.TimerFacility.TimerFire" />
        <references>
          <ref id="632802920178826884" actid="632779604614957048" />
        </references>
        <Properties type="hybrid">
        </Properties>
      </treenode>
      <treenode type="evh" id="632780151249223421" level="2" text="Metreos.Providers.Http.GotRequest: OnFindMeAction">
        <node type="function" name="OnFindMeAction" id="632780151249223418" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632780151249223417" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/ActiveRelayFindMe/Action</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632802867924430057" level="2" text="Metreos.Providers.Http.GotRequest: OnDisplayMenu">
        <node type="function" name="OnDisplayMenu" id="632802867924430054" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632802867924430053" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/ActiveRelayFindMe/Menu</ep>
        </Properties>
      </treenode>
      <treenode type="fun" id="632345022712031474" level="1" text="RequestLoginAction">
        <node type="function" name="RequestLoginAction" id="632345022712031471" path="Metreos.StockTools" />
        <calls>
          <ref actid="632345022712031470" />
          <ref actid="632345614710627153" />
          <ref actid="632345614710627164" />
          <ref actid="632778653446855108" />
        </calls>
      </treenode>
      <treenode type="fun" id="632346509622657877" level="1" text="DisplayFindMeFail">
        <node type="function" name="DisplayFindMeFail" id="632346509622657874" path="Metreos.StockTools" />
        <calls>
          <ref actid="632489818190969978" />
          <ref actid="632796852009378747" />
          <ref actid="632802867924430071" />
          <ref actid="632796852009378751" />
          <ref actid="632796852009378753" />
          <ref actid="632796852009378755" />
        </calls>
      </treenode>
      <treenode type="fun" id="632674146742754458" level="1" text="OpenDBConnection">
        <node type="function" name="OpenDBConnection" id="632674146742754455" path="Metreos.StockTools" />
        <calls>
          <ref actid="632674146742754454" />
        </calls>
      </treenode>
      <treenode type="fun" id="632778523788658265" level="1" text="ValidateUserCredentials">
        <node type="function" name="ValidateUserCredentials" id="632778523788658262" path="Metreos.StockTools" />
        <calls>
          <ref actid="632780151249223401" />
        </calls>
      </treenode>
      <treenode type="fun" id="632778653446853847" level="1" text="DisplayFindMeMenu">
        <node type="function" name="DisplayFindMeMenu" id="632778653446853844" path="Metreos.StockTools" />
        <calls>
          <ref actid="632778653446853843" />
          <ref actid="632780151249223800" />
          <ref actid="632780151249223409" />
          <ref actid="632802867924430062" />
        </calls>
      </treenode>
      <treenode type="fun" id="632779604614957042" level="1" text="PerformAction">
        <node type="function" name="PerformAction" id="632779604614957039" path="Metreos.StockTools" />
        <calls>
          <ref actid="632780151249223789" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="db_poolConnections" id="632802920178826650" vid="632674146742755441">
        <Properties type="Bool" defaultInitWith="true" initWith="DbConnPooling">db_poolConnections</Properties>
      </treenode>
      <treenode text="db_ConnectionName" id="632802920178826652" vid="632347619057191312">
        <Properties type="String" initWith="DbConnectionName">db_ConnectionName</Properties>
      </treenode>
      <treenode text="db_Master_DbName" id="632802920178826654" vid="632346722572969731">
        <Properties type="String" initWith="DbName">db_Master_DbName</Properties>
      </treenode>
      <treenode text="db_Master_DbServer" id="632802920178826656" vid="632346722572969733">
        <Properties type="String" initWith="Server">db_Master_DbServer</Properties>
      </treenode>
      <treenode text="db_Master_Port" id="632802920178826658" vid="632346722572969735">
        <Properties type="UInt" initWith="Port">db_Master_Port</Properties>
      </treenode>
      <treenode text="db_Master_Username" id="632802920178826660" vid="632346722572969737">
        <Properties type="String" initWith="Username">db_Master_Username</Properties>
      </treenode>
      <treenode text="db_Master_Password" id="632802920178826662" vid="632346722572969739">
        <Properties type="String" initWith="Password">db_Master_Password</Properties>
      </treenode>
      <treenode text="db_Slave_DbName" id="632802920178826664" vid="632346722572969731">
        <Properties type="String" initWith="SlaveDBName">db_Slave_DbName</Properties>
      </treenode>
      <treenode text="db_Slave_DbServer" id="632802920178826666" vid="632346722572969733">
        <Properties type="String" initWith="SlaveDBServerAddress">db_Slave_DbServer</Properties>
      </treenode>
      <treenode text="db_Slave_Port" id="632802920178826668" vid="632346722572969735">
        <Properties type="UInt" initWith="SlaveDBServerPort">db_Slave_Port</Properties>
      </treenode>
      <treenode text="db_Slave_Username" id="632802920178826670" vid="632346722572969737">
        <Properties type="String" initWith="SlaveDBServerUsername">db_Slave_Username</Properties>
      </treenode>
      <treenode text="db_Slave_Password" id="632802920178826672" vid="632346722572969739">
        <Properties type="String" initWith="SlaveDBServerPassword">db_Slave_Password</Properties>
      </treenode>
      <treenode text="g_userId" id="632802920178826674" vid="632344838654845368">
        <Properties type="UInt">g_userId</Properties>
      </treenode>
      <treenode text="g_deviceId" id="632802920178826676" vid="632344926321875205">
        <Properties type="UInt">g_deviceId</Properties>
      </treenode>
      <treenode text="g_userPrimaryDeviceMAC" id="632802920178826678" vid="632344838654845365">
        <Properties type="String">g_userPrimaryDeviceMAC</Properties>
      </treenode>
      <treenode text="g_routingGuid" id="632802920178826680" vid="632345964352810670">
        <Properties type="String">g_routingGuid</Properties>
      </treenode>
      <treenode text="g_primaryDN" id="632802920178826682" vid="632347717270000687">
        <Properties type="String" initWith="CCM_Device_Username">g_primaryDN</Properties>
      </treenode>
      <treenode text="g_timeStamp" id="632802920178826684" vid="632345614710625689">
        <Properties type="DateTime">g_timeStamp</Properties>
      </treenode>
      <treenode text="g_loginUrl" id="632802920178826686" vid="632345614710625694">
        <Properties type="String" defaultInitWith="/ActiveRelayFindMe/Login">g_loginUrl</Properties>
      </treenode>
      <treenode text="g_findMeAction" id="632802920178826688" vid="632346509622657903">
        <Properties type="String" defaultInitWith="/ActiveRelayFindMe/Action">g_findMeAction</Properties>
      </treenode>
      <treenode text="g_exitUrl" id="632802920178826690" vid="632345614710626678">
        <Properties type="String" defaultInitWith="/ActiveRelayFindMe/Exit">g_exitUrl</Properties>
      </treenode>
      <treenode text="g_authenticated" id="632802920178826692" vid="632345022712031493">
        <Properties type="String" defaultInitWith="false">g_authenticated</Properties>
      </treenode>
      <treenode text="g_onRequestHit" id="632802920178826694" vid="632345022712031516">
        <Properties type="Bool" defaultInitWith="false">g_onRequestHit</Properties>
      </treenode>
      <treenode text="g_remoteHost" id="632802920178826696" vid="632346898435000824">
        <Properties type="String">g_remoteHost</Properties>
      </treenode>
      <treenode text="g_remoteIp" id="632802920178826698" vid="632347416185002372">
        <Properties type="String">g_remoteIp</Properties>
      </treenode>
      <treenode text="g_acceptDigit" id="632802920178826700" vid="632673314455190074">
        <Properties type="String" initWith="AcceptDigit">g_acceptDigit</Properties>
      </treenode>
      <treenode text="g_actionUrl" id="632802920178826702" vid="632778653446853867">
        <Properties type="String" defaultInitWith="/ActiveRelayFindMe/Action">g_actionUrl</Properties>
      </treenode>
      <treenode text="g_host" id="632802920178826704" vid="632778653446853870">
        <Properties type="String">g_host</Properties>
      </treenode>
      <treenode text="g_timerFireDelay" id="632802920178826706" vid="632779604614955786">
        <Properties type="UInt" defaultInitWith="5">g_timerFireDelay</Properties>
      </treenode>
      <treenode text="g_outstandingEventsList" id="632802920178826708" vid="632779604614955788">
        <Properties type="ArrayList">g_outstandingEventsList</Properties>
      </treenode>
      <treenode text="g_selectActionPrompt" id="632802920178826710" vid="632780151249223413">
        <Properties type="String" defaultInitWith="Select action">g_selectActionPrompt</Properties>
      </treenode>
      <treenode text="g_changesSavedPrompt" id="632802920178826712" vid="632780151249223415">
        <Properties type="String" defaultInitWith="Changes saved.">g_changesSavedPrompt</Properties>
      </treenode>
      <treenode text="g_mainMenuUrl" id="632802920178826714" vid="632802867924430058">
        <Properties type="String" defaultInitWith="/ActiveRelayFindMe/Menu">g_mainMenuUrl</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632340365902188662" treenode="632340365902188663" appnode="632340365902188660" handlerfor="632340365902188659">
    <node type="Start" id="632340365902188662" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="320">
      <linkto id="632345022712031518" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632340365902188972" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1019.70703" y="628">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">success</ap>
      </Properties>
    </node>
    <node type="Action" id="632341317830782074" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="234" y="321">
      <linkto id="632381117669219739" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632674146742754454" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties language="csharp">
	public static string Execute(Metreos.Types.Http.QueryParamCollection queryParameters, 		ref string g_userPrimaryDeviceMAC, bool g_onRequestHit, ref string g_routingGuid, string routingGuid, ref string g_host, string host, ref string g_remoteHost, string remoteHost)
	{
		g_host = host;
		g_remoteHost = remoteHost;
		g_onRequestHit = true;
		g_routingGuid = routingGuid;
		g_userPrimaryDeviceMAC = queryParameters["device"];
		return g_userPrimaryDeviceMAC == null ? IApp.VALUE_FAILURE : IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632344838654845363" name="GetUserByDeviceMac" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="600" y="321">
      <linkto id="632345022712031470" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632778642736130071" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="Mac" type="variable">g_userPrimaryDeviceMAC</ap>
        <ap name="IsPrimary" type="literal">true</ap>
        <rd field="UserId">g_userId</rd>
        <rd field="DeviceId">g_deviceId</rd>
        <rd field="UserStatus">userStatus</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"Looking up information for device MAC: " + g_userPrimaryDeviceMAC;
</log>
      </Properties>
    </node>
    <node type="Action" id="632345022712031470" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="545.222656" y="613" mx="603" my="629">
      <items count="1">
        <item text="RequestLoginAction" />
      </items>
      <linkto id="632340365902188972" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="reason" type="literal">authenticate</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="FunctionName" type="literal">RequestLoginAction</ap>
      </Properties>
    </node>
    <node type="Action" id="632345022712031518" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="133" y="320">
      <linkto id="632341317830782074" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632345022712031519" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_onRequestHit</ap>
      </Properties>
    </node>
    <node type="Action" id="632345022712031519" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="133" y="457">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632345614710627147" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="874.177734" y="323">
      <linkto id="632778653446853843" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <ap name="Value2" type="variable">remoteHost</ap>
        <rd field="ResultData">g_authenticated</rd>
        <rd field="ResultData2">g_remoteHost</rd>
      </Properties>
    </node>
    <node type="Action" id="632381117669219739" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="235" y="453">
      <linkto id="632381117669219742" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Active Relay</ap>
        <ap name="Prompt" type="literal">A system error occured</ap>
        <ap name="Text" type="literal">The Active Relay FindMe Service is not configured properly.</ap>
        <rd field="ResultData">responseText</rd>
      </Properties>
    </node>
    <node type="Action" id="632381117669219740" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="454" y="453">
      <linkto id="632381117669219742" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Active Relay</ap>
        <ap name="Prompt" type="literal">A system error occured</ap>
        <ap name="Text" type="literal">Could not access database. </ap>
        <rd field="ResultData">responseText</rd>
      </Properties>
    </node>
    <node type="Action" id="632381117669219741" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="345" y="652">
      <linkto id="632381117669219744" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">responseText.ToString();</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632381117669219742" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="346.173828" y="546">
      <linkto id="632381117669219741" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">Key:Services</ap>
        <rd field="ResultData">responseText</rd>
      </Properties>
    </node>
    <node type="Action" id="632381117669219744" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="345" y="749">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632489818190969978" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="689.506836" y="159" mx="743" my="175">
      <items count="1">
        <item text="DisplayFindMeFail" />
      </items>
      <linkto id="632489818190969979" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="reason" type="literal">db_failure</ap>
        <ap name="FunctionName" type="literal">DisplayFindMeFail</ap>
      </Properties>
    </node>
    <node type="Action" id="632489818190969979" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="739" y="75">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632674146742754454" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="399.594727" y="305" mx="457" my="321">
      <items count="1">
        <item text="OpenDBConnection" />
      </items>
      <linkto id="632344838654845363" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632381117669219740" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="FunctionName" type="literal">OpenDBConnection</ap>
      </Properties>
    </node>
    <node type="Action" id="632778642736130071" name="Compare" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="740.756836" y="322">
      <linkto id="632489818190969978" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632345614710627147" type="Labeled" style="Bezier" ortho="true" label="equal" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">userStatus</ap>
        <ap name="Value2" type="literal">Ok</ap>
      </Properties>
    </node>
    <node type="Action" id="632778653446853843" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="959.026367" y="307" mx="1018" my="323">
      <items count="1">
        <item text="DisplayFindMeMenu" />
      </items>
      <linkto id="632340365902188972" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="RemoteHost" type="variable">remoteHost</ap>
        <ap name="Prompt" type="variable">g_selectActionPrompt</ap>
        <ap name="FunctionName" type="literal">DisplayFindMeMenu</ap>
      </Properties>
    </node>
    <node type="Variable" id="632340365902188968" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference" name="Metreos.Providers.Http.GotRequest">host</Properties>
    </node>
    <node type="Variable" id="632340365902188969" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632340365902188970" name="routingGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RoutingGuid" refType="reference">routingGuid</Properties>
    </node>
    <node type="Variable" id="632341317830782073" name="queryParameters" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">queryParameters</Properties>
    </node>
    <node type="Variable" id="632381117669219738" name="responseText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">responseText</Properties>
    </node>
    <node type="Variable" id="632778642736130070" name="userStatus" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">userStatus</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnFindMeServiceRequestResp" startnode="632345018984531466" treenode="632345018984531467" appnode="632345018984531464" handlerfor="632345018984531463">
    <node type="Start" id="632345018984531466" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="126" y="153">
      <linkto id="632780151249223389" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632780151249223389" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="286.90625" y="153.5">
      <linkto id="632780151249223393" type="Labeled" style="Bezier" label="StaleEvent" />
      <linkto id="632780151249223393" type="Labeled" style="Bezier" label="default" />
      <linkto id="632780151249223394" type="Labeled" style="Bezier" ortho="true" label="ValidEvent" />
      <Properties language="csharp">
        <log condition="entry" on="false" level="Info" type="literal">OnFindMeServiceRequestResp: function entry</log>
public static string Execute(LogWriter log, ArrayList g_outstandingEventsList, string timerId)
{
	if (g_outstandingEventsList == null)
	{
		log.Write(TraceLevel.Error, "OnFindMeServiceRequestResp: g_outstandingEventsList is null!");
		return IApp.VALUE_FAILURE;
	}

	if (g_outstandingEventsList.Contains(timerId))
	{
		log.Write(TraceLevel.Verbose, "OnFindMeServiceRequestResp: received valid event with TimerId: " + timerId + ". Removing from Outstanding list.");
		g_outstandingEventsList.Remove(timerId);
		return "ValidEvent";
	}
	else
	{
		log.Write(TraceLevel.Verbose, "OnFindMeServiceRequestResp: received STALE event with TimerId: " + timerId);				
		return "StaleEvent";
	}
}

</Properties>
    </node>
    <node type="Action" id="632780151249223390" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="507.90625" y="153.5">
      <linkto id="632780151249223392" type="Labeled" style="Bezier" ortho="true" label="Failure" />
      <linkto id="632802867924430050" type="Labeled" style="Bezier" ortho="true" label="MailboxNotDefined" />
      <linkto id="632780151249223391" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">response</ap>
      </Properties>
    </node>
    <node type="Label" id="632780151249223392" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="697.90625" y="154.5" />
    <node type="Action" id="632780151249223393" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="286.90625" y="348.5">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632780151249223394" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="406.90625" y="154.5">
      <linkto id="632780151249223390" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">timerId</ap>
        <log condition="entry" on="false" level="Info" type="csharp">"OnFindMeServiceRequestResp: Removing timer with timerId: " + timerId</log>
      </Properties>
    </node>
    <node type="Label" id="632780151249223797" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="216" y="532">
      <linkto id="632780151249223800" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632780151249223798" text="F" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="218" y="677">
      <linkto id="632796852009378747" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632780151249223799" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="484" y="676">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632780151249223800" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="305" y="516" mx="364" my="532">
      <items count="1">
        <item text="DisplayFindMeMenu" />
      </items>
      <linkto id="632780151249223802" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="RemoteHost" type="variable">g_remoteHost</ap>
        <ap name="Prompt" type="variable">g_changesSavedPrompt</ap>
        <ap name="FunctionName" type="literal">DisplayFindMeMenu</ap>
      </Properties>
    </node>
    <node type="Action" id="632780151249223802" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="526" y="530">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632796852009378747" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="295" y="661" mx="349" my="677">
      <items count="1">
        <item text="DisplayFindMeFail" />
      </items>
      <linkto id="632780151249223799" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="reason" type="literal">default</ap>
        <ap name="FunctionName" type="literal">DisplayFindMeFail</ap>
      </Properties>
    </node>
    <node type="Label" id="632802867924430050" text="N" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="633" y="250" />
    <node type="Label" id="632780151249223391" text="S" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="624.90625" y="59.5" />
    <node type="Label" id="632802867924430069" text="N" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="224" y="832">
      <linkto id="632802867924430071" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632802867924430070" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="490" y="831">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632802867924430071" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="301" y="816" mx="355" my="832">
      <items count="1">
        <item text="DisplayFindMeFail" />
      </items>
      <linkto id="632802867924430070" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="reason" type="literal">MailboxNotDefined</ap>
        <ap name="FunctionName" type="literal">DisplayFindMeFail</ap>
      </Properties>
    </node>
    <node type="Variable" id="632346898435000812" name="response" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Response" refType="reference" name="Metreos.Events.ActiveRelay.FindMeServiceRequestResp">response</Properties>
    </node>
    <node type="Variable" id="632346898435000813" name="ciscoText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">ciscoText</Properties>
    </node>
    <node type="Variable" id="632780151249223388" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="TimerId" refType="reference" name="Metreos.Events.ActiveRelay.FindMeServiceRequestResp">timerId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnLoginRequest" startnode="632345614710626235" treenode="632345614710626236" appnode="632345614710626233" handlerfor="632345614710626232">
    <node type="Start" id="632345614710626235" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="570">
      <linkto id="632346509622656601" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632345614710627152" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="481" y="573">
      <linkto id="632345614710627153" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632780151249223401" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">OnLoginRequest: Selected action was: auth</log>
	public static string Execute(Metreos.Types.Http.QueryParamCollection queryParameters, 		ref string username, ref string password)
	{
		username = queryParameters["user"];
		password = queryParameters["pass"];
		if (username == null)
			return IApp.VALUE_FAILURE;
		if (password == null)
			password = string.Empty;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632345614710627153" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="430.222656" y="744" mx="488" my="760">
      <items count="1">
        <item text="RequestLoginAction" />
      </items>
      <linkto id="632345614710627154" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="reason" type="literal">no_username</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="FunctionName" type="literal">RequestLoginAction</ap>
      </Properties>
    </node>
    <node type="Action" id="632345614710627154" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="487" y="966">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632345614710627157" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="958" y="765">
      <linkto id="632345614710627164" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">failed</ap>
        <rd field="ResultData">loginFailReason</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnLoginRequest: Authentication: Default or Invalid pass</log>
      </Properties>
    </node>
    <node type="Action" id="632345614710627160" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1147" y="576">
      <linkto id="632345614710627164" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">disabled</ap>
        <rd field="ResultData">loginFailReason</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnLoginRequest: Authentication: Disabled or Deleted</log>
      </Properties>
    </node>
    <node type="Action" id="632345614710627161" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="710" y="920">
      <linkto id="632345614710627164" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">too_many_attempts</ap>
        <rd field="ResultData">loginFailReason</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnLoginRequest: Authentication: Locked</log>
      </Properties>
    </node>
    <node type="Action" id="632345614710627162" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="709" y="456">
      <linkto id="632780151249223409" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <ap name="Value2" type="variable">remoteHost</ap>
        <rd field="ResultData">g_authenticated</rd>
        <rd field="ResultData2">g_remoteHost</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnLoginRequest: Authentication: Success</log>
      </Properties>
    </node>
    <node type="Action" id="632345614710627164" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1098.22266" y="906" mx="1156" my="922">
      <items count="1">
        <item text="RequestLoginAction" />
      </items>
      <linkto id="632345614710627165" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="reason" type="variable">loginFailReason</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="FunctionName" type="literal">RequestLoginAction</ap>
      </Properties>
    </node>
    <node type="Action" id="632345614710627165" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1288.04883" y="922">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632346509622656601" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="152" y="572">
      <linkto id="632346509622656603" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632346509622656608" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">OnLoginRequest: Function Entry</log>	public static string Execute(Metreos.Types.Http.QueryParamCollection queryParameters, 		ref string action)
	{
		action = queryParameters["action"];
		if (action == null)
			return IApp.VALUE_FAILURE;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632346509622656603" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="270" y="573">
      <linkto id="632345614710627152" type="Labeled" style="Bezier" ortho="true" label="auth" />
      <linkto id="632346509622656608" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">action</ap>
      </Properties>
    </node>
    <node type="Action" id="632346509622656608" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="272" y="754">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">OnLoginRequest: Unknown action requested</log>
      </Properties>
    </node>
    <node type="Action" id="632780151249223401" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="644" y="559" mx="713" my="575">
      <items count="1">
        <item text="ValidateUserCredentials" />
      </items>
      <linkto id="632345614710627162" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632345614710627160" type="Labeled" style="Bezier" label="NotAllowedDueToDisabled" />
      <linkto id="632345614710627157" type="Labeled" style="Bezier" label="InvalidAccountCodeOrPin" />
      <linkto id="632345614710627161" type="Labeled" style="Bezier" ortho="true" label="NotAllowedDueLocked" />
      <linkto id="632345614710627160" type="Labeled" style="Bezier" label="NotAllowedDueToDeleted" />
      <linkto id="632345614710627157" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="User" type="variable">username</ap>
        <ap name="Pass" type="variable">password</ap>
        <ap name="Source" type="variable">remoteIp</ap>
        <ap name="Type" type="literal">web</ap>
        <ap name="FunctionName" type="literal">ValidateUserCredentials</ap>
        <rd field="UserId">g_userId</rd>
      </Properties>
    </node>
    <node type="Action" id="632780151249223409" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="653" y="315" mx="712" my="331">
      <items count="1">
        <item text="DisplayFindMeMenu" />
      </items>
      <linkto id="632780151249223411" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="RemoteHost" type="variable">remoteHost</ap>
        <ap name="Prompt" type="variable">g_selectActionPrompt</ap>
        <ap name="FunctionName" type="literal">DisplayFindMeMenu</ap>
      </Properties>
    </node>
    <node type="Action" id="632780151249223411" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="709" y="196">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632345614710627130" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632345614710627131" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632345614710627132" name="remoteIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteIpAddress" refType="reference">remoteIp</Properties>
    </node>
    <node type="Variable" id="632345614710627149" name="username" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">username</Properties>
    </node>
    <node type="Variable" id="632345614710627150" name="password" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">password</Properties>
    </node>
    <node type="Variable" id="632345614710627151" name="queryParameters" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">queryParameters</Properties>
    </node>
    <node type="Variable" id="632345614710627158" name="loginFailReason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">loginFailReason</Properties>
    </node>
    <node type="Variable" id="632346509622656602" name="action" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">action</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnSessionExpired" startnode="632348338276875931" treenode="632348338276875932" appnode="632348338276875929" handlerfor="632348338276875928">
    <node type="Start" id="632348338276875931" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="37" y="376">
      <linkto id="632348338276875933" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632348338276875933" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="156" y="377">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnTimerFire" startnode="632779604614957046" treenode="632779604614957047" appnode="632779604614957044" handlerfor="632779604614957043">
    <node type="Start" id="632779604614957046" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="109" y="109">
      <linkto id="632779604614957062" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632779604614957062" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="225.897781" y="109">
      <linkto id="632779604614957063" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">timerId</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnTimerFire: Removing timer with ID: " + timerId</log>
      </Properties>
    </node>
    <node type="Action" id="632779604614957063" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="338.804016" y="108">
      <linkto id="632796852009378751" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(LogWriter log, ArrayList g_outstandingEventsList, string timerId)
{
	if (g_outstandingEventsList == null)
	{
		log.Write(TraceLevel.Error, "OnTimerFire: g_outstandingEventsList is null!");
		return IApp.VALUE_FAILURE;
	}

	if (g_outstandingEventsList.Contains(timerId))
	{
		log.Write(TraceLevel.Verbose, "OnTimerFire: timer fired for outstanding event with TimerId: " + timerId + ". Removing from Outstanding list.");
		g_outstandingEventsList.Remove(timerId);
		return "Valid";
	}
	else
	{
		log.Write(TraceLevel.Verbose, "OnTimerFire: timer fired for event that is not in outstanding event list. TimerId: " + timerId );
		return "Invalid";
	}
}
</Properties>
    </node>
    <node type="Action" id="632780151249223385" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="631" y="110">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632796852009378751" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="420" y="93" mx="474" my="109">
      <items count="1">
        <item text="DisplayFindMeFail" />
      </items>
      <linkto id="632780151249223385" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="reason" type="literal"> time_out</ap>
        <ap name="FunctionName" type="literal">DisplayFindMeFail</ap>
      </Properties>
    </node>
    <node type="Variable" id="632779604614957060" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="timerId" refType="reference" name="Metreos.Providers.TimerFacility.TimerFire">timerId</Properties>
    </node>
    <node type="Variable" id="632779604614957061" name="userData" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="timerUserData" refType="reference" name="Metreos.Providers.TimerFacility.TimerFire">userData</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnFindMeAction" startnode="632780151249223420" treenode="632780151249223421" appnode="632780151249223418" handlerfor="632780151249223417">
    <node type="Start" id="632780151249223420" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="257">
      <linkto id="632780151249223787" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632780151249223787" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="141" y="257">
      <linkto id="632780151249223789" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632796852009378753" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">	public static string Execute(Metreos.Types.Http.QueryParamCollection queryParameters, 		ref string action)
	{
		switch (queryParameters["action"])
		{
			case "ea"   : action = "enable_all"; break;
			case "da"   : action = "disable_all"; break;
			case "ev"   : action = "enable_vmail"; break;
			case "dabv" : action = "disable_except_vmail"; break;
			default     : action = null; break;
		}

		if (action == null)
			return IApp.VALUE_FAILURE;
		else
			return IApp.VALUE_SUCCESS;
	}

</Properties>
    </node>
    <node type="Action" id="632780151249223789" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="253" y="240" mx="295" my="256">
      <items count="1">
        <item text="PerformAction" />
      </items>
      <linkto id="632780151249223794" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Action" type="variable">action</ap>
        <ap name="FunctionName" type="literal">PerformAction</ap>
      </Properties>
    </node>
    <node type="Action" id="632780151249223793" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="142" y="507">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632780151249223794" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="426" y="256">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632796852009378753" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="91" y="350" mx="145" my="366">
      <items count="1">
        <item text="DisplayFindMeFail" />
      </items>
      <linkto id="632780151249223793" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="reason" type="literal">no_action</ap>
        <ap name="FunctionName" type="literal">DisplayFindMeFail</ap>
      </Properties>
    </node>
    <node type="Variable" id="632780151249223786" name="queryParameters" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference" name="Metreos.Providers.Http.GotRequest">queryParameters</Properties>
    </node>
    <node type="Variable" id="632780151249223788" name="action" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">action</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnDisplayMenu" startnode="632802867924430056" treenode="632802867924430057" appnode="632802867924430054" handlerfor="632802867924430053">
    <node type="Start" id="632802867924430056" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="46" y="175">
      <linkto id="632802867924430062" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632802867924430062" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="92.203125" y="159" mx="151" my="175">
      <items count="1">
        <item text="DisplayFindMeMenu" />
      </items>
      <linkto id="632802867924430063" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="Prompt" type="variable">g_selectActionPrompt</ap>
        <ap name="RemoteHost" type="variable">g_remoteHost</ap>
        <ap name="FunctionName" type="literal">DisplayFindMeMenu</ap>
      </Properties>
    </node>
    <node type="Action" id="632802867924430063" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="282" y="175">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
  </canvas>
  <canvas type="Function" name="RequestLoginAction" startnode="632345022712031473" treenode="632345022712031474" appnode="632345022712031471" handlerfor="632802867924430053">
    <node type="Start" id="632345022712031473" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="379">
      <linkto id="632346509622656808" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632345022712031480" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="238" y="380">
      <linkto id="632345022712031481" type="Labeled" style="Bezier" ortho="true" label="failed" />
      <linkto id="632345022712031482" type="Labeled" style="Bezier" ortho="true" label="authenticate" />
      <linkto id="632345022712031483" type="Labeled" style="Bezier" ortho="true" label="disabled" />
      <linkto id="632345022712031504" type="Labeled" style="Bezier" ortho="true" label="too_many_attempts" />
      <linkto id="632345614710627159" type="Labeled" style="Bezier" ortho="true" label="no_username" />
      <linkto id="632778523788658259" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">reasonForLogin</ap>
      </Properties>
    </node>
    <node type="Action" id="632345022712031481" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="483" y="426">
      <linkto id="632345614710627136" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">Log-in failed.</ap>
        <ap name="Value2" type="literal">Please try again.</ap>
        <rd field="ResultData">userReasonString</rd>
        <rd field="ResultData2">userText</rd>
      </Properties>
    </node>
    <node type="Action" id="632345022712031482" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="485" y="545">
      <linkto id="632345614710627136" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">Log-in required.</ap>
        <ap name="Value2" type="literal">This service requires that you first authenticate. Press 'Next' to continue.</ap>
        <rd field="ResultData">userReasonString</rd>
        <rd field="ResultData2">userText</rd>
      </Properties>
    </node>
    <node type="Action" id="632345022712031483" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="477" y="187">
      <linkto id="632345022712031502" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">Log-in failed.</ap>
        <ap name="Value2" type="literal">Your account is disabled. Please contact the System Administrator.</ap>
        <ap name="Value3" type="literal">3</ap>
        <rd field="ResultData">userReasonString</rd>
        <rd field="ResultData2">userText</rd>
        <rd field="ResultData3">exitKeyPos</rd>
      </Properties>
    </node>
    <node type="Action" id="632345022712031502" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="602" y="186">
      <linkto id="632345022712031503" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">FindMe Service</ap>
        <ap name="Prompt" type="variable">userReasonString</ap>
        <ap name="Text" type="variable">userText</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632345022712031503" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="726" y="188">
      <linkto id="632345022712031514" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="variable">exitKeyPos</ap>
        <ap name="URL" type="literal">Key:Services</ap>
        <rd field="ResultData">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632345022712031504" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="473" y="308">
      <linkto id="632345022712031502" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">Log-in failed.</ap>
        <ap name="Value2" type="literal">Your account is locked. Please contact the System Administrator.</ap>
        <ap name="Value3" type="literal">3</ap>
        <rd field="ResultData">userReasonString</rd>
        <rd field="ResultData2">userText</rd>
        <rd field="ResultData3">exitKeyPos</rd>
      </Properties>
    </node>
    <node type="Action" id="632345022712031514" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="851.347656" y="189">
      <linkto id="632345614710627155" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">text.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
        <log condition="entry" on="true" level="Info" type="literal">RequestLoginAction: Sending text response</log>
      </Properties>
    </node>
    <node type="Action" id="632345022712031515" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1025.93689" y="544">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632345614710627135" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="702" y="545">
      <linkto id="632345614710627139" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DisplayName" type="literal">Username</ap>
        <ap name="QueryStringParam" type="literal">user</ap>
        <ap name="InputFlags" type="literal">A</ap>
        <rd field="ResultData">inputCisco</rd>
      </Properties>
    </node>
    <node type="Action" id="632345614710627136" name="CreateInput" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="591" y="544">
      <linkto id="632345614710627135" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">FindMe Service</ap>
        <ap name="Prompt" type="variable">userReasonString</ap>
        <ap name="URL" type="csharp">g_host + g_loginUrl + "?action=auth&amp;metreosSessionId=" + g_routingGuid;</ap>
        <rd field="ResultData">inputCisco</rd>
      </Properties>
    </node>
    <node type="Action" id="632345614710627139" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="812" y="546">
      <linkto id="632345614710627140" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DisplayName" type="literal">Pass</ap>
        <ap name="QueryStringParam" type="literal">pass</ap>
        <ap name="InputFlags" type="literal">PA</ap>
        <rd field="ResultData">inputCisco</rd>
      </Properties>
    </node>
    <node type="Action" id="632345614710627140" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="916.4076" y="545">
      <linkto id="632345022712031515" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">inputCisco.ToString();</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632345614710627155" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1027.40759" y="191">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632345614710627159" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="488" y="690">
      <linkto id="632345614710627136" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">No user specified.</ap>
        <ap name="Value2" type="literal">Please try again.</ap>
        <rd field="ResultData">userReasonString</rd>
        <rd field="ResultData2">userText</rd>
      </Properties>
    </node>
    <node type="Action" id="632346509622656808" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="129" y="380">
      <linkto id="632345022712031480" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">false</ap>
        <rd field="ResultData">g_authenticated</rd>
        <log condition="entry" on="true" level="Info" type="literal">RequestLoginAction: Function Entry</log>
      </Properties>
    </node>
    <node type="Action" id="632778523788658259" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="466" y="32">
      <linkto id="632345022712031502" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">Log-in failed.</ap>
        <ap name="Value2" type="literal">Invalid account status. Please contact the System Administrator.</ap>
        <ap name="Value3" type="literal">3</ap>
        <rd field="ResultData">userReasonString</rd>
        <rd field="ResultData2">userText</rd>
        <rd field="ResultData3">exitKeyPos</rd>
      </Properties>
    </node>
    <node type="Variable" id="632345022712031479" name="reasonForLogin" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="reason" refType="reference">reasonForLogin</Properties>
    </node>
    <node type="Variable" id="632345022712031484" name="userReasonString" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">userReasonString</Properties>
    </node>
    <node type="Variable" id="632345022712031497" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632345022712031505" name="userText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">userText</Properties>
    </node>
    <node type="Variable" id="632345022712031507" name="exitKeyPos" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UShort" refType="reference">exitKeyPos</Properties>
    </node>
    <node type="Variable" id="632345614710627129" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632345614710627137" name="inputCisco" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Input" refType="reference">inputCisco</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="DisplayFindMeFail" activetab="true" startnode="632346509622657876" treenode="632346509622657877" appnode="632346509622657874" handlerfor="632802867924430053">
    <node type="Start" id="632346509622657876" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="299">
      <linkto id="632346509622657895" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632346509622657879" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="281.048828" y="299">
      <linkto id="632346509622657883" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">Operation timed out.</ap>
        <ap name="Value2" type="literal">Your operation timed out.  Please try again later or contact your Systems Administrator.
</ap>
        <rd field="ResultData">promptText</rd>
        <rd field="ResultData2">textText</rd>
      </Properties>
    </node>
    <node type="Action" id="632346509622657883" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="432.644531" y="300">
      <linkto id="632346509622657884" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">FindMe Service</ap>
        <ap name="Prompt" type="variable">promptText</ap>
        <ap name="Text" type="variable">textText</ap>
        <rd field="ResultData">ciscoText</rd>
      </Properties>
    </node>
    <node type="Action" id="632346509622657884" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="563.1738" y="302">
      <linkto id="632346509622657885" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">Key:Services</ap>
        <rd field="ResultData">ciscoText</rd>
      </Properties>
    </node>
    <node type="Action" id="632346509622657885" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="700.1738" y="303">
      <linkto id="632346509622657894" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">g_remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">ciscoText.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632346509622657894" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="855" y="301">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632346509622657895" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="123" y="298">
      <linkto id="632347416185002364" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632346509622657879" type="Labeled" style="Bezier" ortho="true" label="time_out" />
      <linkto id="632489818190969981" type="Labeled" style="Bezier" ortho="true" label="db_failure" />
      <linkto id="632802867924430068" type="Labeled" style="Vector" ortho="true" label="MailboxNotDefined" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">reason</ap>
        <log condition="entry" on="true" level="Info" type="literal">DisplayFindMeFail: Function Entry</log>
      </Properties>
    </node>
    <node type="Action" id="632347416185002364" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="289" y="414">
      <linkto id="632346509622657883" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">Operation failed.</ap>
        <ap name="Value2" type="literal">The attempt to process your action failed.  Please try again later or contact your Systems Administrator.

</ap>
        <rd field="ResultData">promptText</rd>
        <rd field="ResultData2">textText</rd>
      </Properties>
    </node>
    <node type="Action" id="632489818190969981" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="295" y="174">
      <linkto id="632346509622657883" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">Operation failed.</ap>
        <ap name="Value2" type="literal">Could not connect to database.  Please try again later or contact your Systems Administrator.</ap>
        <rd field="ResultData">promptText</rd>
        <rd field="ResultData2">textText</rd>
      </Properties>
    </node>
    <node type="Action" id="632802867924430066" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="517.1738" y="645">
      <linkto id="632802920178826894" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Menu</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">g_host + g_mainMenuUrl + "?metreosSessionId=" + g_routingGuid</ap>
        <rd field="ResultData">ciscoText</rd>
      </Properties>
    </node>
    <node type="Action" id="632802867924430068" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="287" y="646">
      <linkto id="632802920178826893" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">Operation failed.</ap>
        <ap name="Value2" type="literal">Your operation failed because your account does not have a voicemail box defined.Please either define a voicemail box, or select either the Enable or Disable All FindMe Numbers options instead.</ap>
        <rd field="ResultData">promptText</rd>
        <rd field="ResultData2">textText</rd>
      </Properties>
    </node>
    <node type="Action" id="632802920178826893" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="400.644531" y="646">
      <linkto id="632802867924430066" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">FindMe Service</ap>
        <ap name="Prompt" type="variable">promptText</ap>
        <ap name="Text" type="variable">textText</ap>
        <rd field="ResultData">ciscoText</rd>
      </Properties>
    </node>
    <node type="Action" id="632802920178826894" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="644.1738" y="645">
      <linkto id="632802920178826897" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">Key:Services</ap>
        <rd field="ResultData">ciscoText</rd>
      </Properties>
    </node>
    <node type="Action" id="632802920178826897" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="772.1738" y="645">
      <linkto id="632802920178826899" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">g_remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">ciscoText.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632802920178826899" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="892.942" y="646">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Variable" id="632346509622657892" name="promptText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">promptText</Properties>
    </node>
    <node type="Variable" id="632346509622657893" name="textText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">textText</Properties>
    </node>
    <node type="Variable" id="632346509622657896" name="reason" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="reason" refType="reference">reason</Properties>
    </node>
    <node type="Variable" id="632346509622659070" name="ciscoText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">ciscoText</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OpenDBConnection" startnode="632674146742754457" treenode="632674146742754458" appnode="632674146742754455" handlerfor="632802867924430053">
    <node type="Start" id="632674146742754457" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="85" y="144">
      <linkto id="632674146742754460" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632674146742754459" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="422.188477" y="143.5">
      <linkto id="632674146742754463" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632674146742754466" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="variable">db_ConnectionName</ap>
        <ap name="Type" type="literal">mysql</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OpenDBConnection: attempting connect to Primary</log>
        <log condition="default" on="true" level="Warning" type="literal">OpenDBConnection: Connection to Primary failed.</log>
      </Properties>
    </node>
    <node type="Action" id="632674146742754460" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="250.1888" y="144.5">
      <linkto id="632674146742754459" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632674146742754466" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="variable">db_Master_DbName</ap>
        <ap name="Server" type="variable">db_Master_DbServer</ap>
        <ap name="Port" type="variable">db_Master_Port</ap>
        <ap name="Username" type="variable">db_Master_Username</ap>
        <ap name="Password" type="variable">db_Master_Password</ap>
        <ap name="Pooling" type="variable">db_poolConnections</ap>
        <ap name="ConnectionTimeout" type="literal">1</ap>
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632674146742754461" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="706.1886" y="328.5">
      <linkto id="632674146742754463" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632674146742754465" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="variable">db_ConnectionName</ap>
        <ap name="Type" type="literal">mysql</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OpenDBConnection: attempting connect to Secondary</log>
        <log condition="default" on="true" level="Info" type="literal">OpenDBConnection: Connection to Secondary failed.
</log>
      </Properties>
    </node>
    <node type="Action" id="632674146742754462" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="554.188843" y="330.5">
      <linkto id="632674146742754461" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632674146742754465" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="variable">db_Slave_DbName</ap>
        <ap name="Server" type="variable">db_Slave_DbServer</ap>
        <ap name="Port" type="variable">db_Slave_Port</ap>
        <ap name="Username" type="variable">db_Slave_Username</ap>
        <ap name="Password" type="variable">db_Slave_Password</ap>
        <ap name="Pooling" type="variable">db_poolConnections</ap>
        <ap name="ConnectionTimeout" type="literal">1</ap>
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632674146742754463" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="704.9997" y="143">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
        <log condition="entry" on="true" level="Verbose" type="literal">OpenDBConnection: connection to database established.</log>
      </Properties>
    </node>
    <node type="Action" id="632674146742754465" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="707.9997" y="539">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Failure</ap>
        <log condition="entry" on="true" level="Error" type="literal">OpenDBConnection: AppSuite DB connections failed. Check application settings.</log>
      </Properties>
    </node>
    <node type="Action" id="632674146742754466" name="SetSessionData" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="419.9997" y="331">
      <linkto id="632674146742754462" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="AllowDBWrite" type="csharp">false</ap>
      </Properties>
    </node>
    <node type="Variable" id="632674146742754475" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="ValidateUserCredentials" startnode="632778523788658264" treenode="632778523788658265" appnode="632778523788658262" handlerfor="632802867924430053">
    <node type="Start" id="632778523788658264" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="85" y="234">
      <linkto id="632778642736130062" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632778523788658266" name="WebLogin" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="372.164063" y="233">
      <linkto id="632778642736130068" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Username" type="variable">user</ap>
        <ap name="Password" type="variable">pass</ap>
        <ap name="IpAddress" type="variable">source</ap>
        <rd field="UserId">userId</rd>
        <rd field="AuthenticationResult">authResult</rd>
      </Properties>
    </node>
    <node type="Action" id="632778642736130062" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="179" y="234">
      <linkto id="632778523788658266" type="Labeled" style="Bezier" ortho="true" label="web" />
      <linkto id="632778642736130063" type="Labeled" style="Bezier" ortho="true" label="phone" />
      <linkto id="632778642736130064" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">authType</ap>
      </Properties>
    </node>
    <node type="Action" id="632778642736130063" name="PhoneLogin" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="179" y="396">
      <linkto id="632778642736130068" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="AccountCode" type="variable">user</ap>
        <ap name="Pin" type="variable">pass</ap>
        <ap name="UserPhoneNumber" type="variable">source</ap>
        <rd field="UserId">userId</rd>
        <rd field="AuthenticationResult">authResult</rd>
      </Properties>
    </node>
    <node type="Action" id="632778642736130064" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="179" y="89">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="variable">authResult</ap>
      </Properties>
    </node>
    <node type="Action" id="632778642736130068" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="373" y="397">
      <Properties final="true" type="appControl" log="On">
        <ap name="UserId" type="variable">userId</ap>
        <ap name="ReturnValue" type="variable">authResult</ap>
      </Properties>
    </node>
    <node type="Variable" id="632778642736130060" name="authType" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Type" refType="reference">authType</Properties>
    </node>
    <node type="Variable" id="632778642736130061" name="authResult" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" defaultInitWith="failure" refType="reference">authResult</Properties>
    </node>
    <node type="Variable" id="632778642736130065" name="user" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="User" refType="reference">user</Properties>
    </node>
    <node type="Variable" id="632778642736130066" name="pass" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Pass" refType="reference">pass</Properties>
    </node>
    <node type="Variable" id="632778642736130067" name="source" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Source" refType="reference">source</Properties>
    </node>
    <node type="Variable" id="632778642736130069" name="userId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">userId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="DisplayFindMeMenu" startnode="632778653446853846" treenode="632778653446853847" appnode="632778653446853844" handlerfor="632802867924430053">
    <node type="Start" id="632778653446853846" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="55" y="306">
      <linkto id="632778653446855107" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632778653446853849" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="1099.90625" y="301">
      <linkto id="632778653446853866" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632778653446853872" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">findMeMenu.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632778653446853860" name="CreateMenu" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="280" y="305">
      <linkto id="632778653446853862" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">FindMe Service</ap>
        <ap name="Prompt" type="variable">prompt</ap>
        <rd field="ResultData">findMeMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632778653446853862" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="396" y="304">
      <linkto id="632778653446853863" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Enable All</ap>
        <ap name="URL" type="csharp">g_host + g_actionUrl + "?metreosSessionId=" + g_routingGuid + "&amp;action=ea";
</ap>
        <rd field="ResultData">findMeMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632778653446853863" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="521" y="303">
      <linkto id="632778653446853864" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Disable All</ap>
        <ap name="URL" type="csharp">g_host + g_actionUrl + "?metreosSessionId=" + g_routingGuid + "&amp;action=da"</ap>
        <rd field="ResultData">findMeMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632778653446853864" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="631" y="302">
      <linkto id="632778653446853865" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Enable Voicemail</ap>
        <ap name="URL" type="csharp">g_host + g_actionUrl + "?metreosSessionId=" + g_routingGuid + "&amp;action=ev"
</ap>
        <rd field="ResultData">findMeMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632778653446853865" name="AddMenuItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="747" y="302">
      <linkto id="632778653446855105" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Disable all but Voicemail</ap>
        <ap name="URL" type="csharp">g_host + g_actionUrl + "?metreosSessionId=" + g_routingGuid + "&amp;action=dabv"
</ap>
        <rd field="ResultData">findMeMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632778653446853866" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1220" y="300">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Success</ap>
      </Properties>
    </node>
    <node type="Action" id="632778653446853872" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1100.4707" y="413">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">Failure</ap>
      </Properties>
    </node>
    <node type="Action" id="632778653446855105" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="863.1738" y="301">
      <linkto id="632797562403353236" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Select</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="literal">SoftKey:Select</ap>
        <rd field="ResultData">findMeMenu</rd>
      </Properties>
    </node>
    <node type="Action" id="632778653446855107" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="160" y="305">
      <linkto id="632778653446853860" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632778653446855108" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_authenticated</ap>
      </Properties>
    </node>
    <node type="Action" id="632778653446855108" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="106" y="400" mx="164" my="416">
      <items count="1">
        <item text="RequestLoginAction" />
      </items>
      <linkto id="632778653446855110" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="reason" type="literal">authenticate</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="FunctionName" type="literal">RequestLoginAction</ap>
      </Properties>
    </node>
    <node type="Action" id="632778653446855110" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="161" y="569">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632797562403353236" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="985.1738" y="301">
      <linkto id="632778653446853849" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">Key:Services</ap>
        <rd field="ResultData">findMeMenu</rd>
      </Properties>
    </node>
    <node type="Variable" id="632778653446853861" name="findMeMenu" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Menu" refType="reference">findMeMenu</Properties>
    </node>
    <node type="Variable" id="632778653446853869" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="RemoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632780151249223412" name="prompt" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Prompt" refType="reference">prompt</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="PerformAction" startnode="632779604614957041" treenode="632779604614957042" appnode="632779604614957039" handlerfor="632802867924430053">
    <node type="Start" id="632779604614957041" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="77" y="162">
      <linkto id="632779604614957048" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632779604614957048" name="AddNonTriggerTimer" class="MaxAsyncActionNode" group="" path="Metreos.Providers.TimerFacility" x="150" y="146" mx="212" my="162">
      <items count="1">
        <item text="OnTimerFire" treenode="632779604614957047" />
      </items>
      <linkto id="632779604614957053" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerDateTime" type="csharp">System.DateTime.Now.AddSeconds(g_timerFireDelay)</ap>
        <ap name="timerUserData" type="variable">action</ap>
        <ap name="UserData" type="literal">none</ap>
        <rd field="timerId">timerId</rd>
      </Properties>
    </node>
    <node type="Action" id="632779604614957052" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="464" y="161">
      <linkto id="632779604614957054" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632779604614957055" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="SenderRoutingGuid" type="variable">g_routingGuid</ap>
        <ap name="TimerId" type="variable">timerId</ap>
        <ap name="UserId" type="variable">g_userId</ap>
        <ap name="OperationType" type="variable">action</ap>
        <ap name="EventName" type="literal">Metreos.Events.ActiveRelay.FindMeServiceRequest</ap>
      </Properties>
    </node>
    <node type="Action" id="632779604614957053" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="347.90625" y="161">
      <linkto id="632779604614957052" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
public static string Execute(LogWriter log, ArrayList g_outstandingEventsList, string timerId)
{
	if (g_outstandingEventsList == null)
	{
		log.Write(TraceLevel.Error, "PerformAction: g_outstandingEventsList is null!");
		return IApp.VALUE_FAILURE;
	}

	g_outstandingEventsList.Add(timerId);
	return IApp.VALUE_SUCCESS;
}
</Properties>
    </node>
    <node type="Action" id="632779604614957054" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="603" y="160">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Action" id="632779604614957055" name="RemoveTimer" class="MaxActionNode" group="" path="Metreos.Providers.TimerFacility" x="465" y="256">
      <linkto id="632796852009378755" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="timerId" type="variable">timerId</ap>
      </Properties>
    </node>
    <node type="Action" id="632780151249223384" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="606" y="360">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632796852009378755" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="416" y="345" mx="470" my="361">
      <items count="1">
        <item text="DisplayFindMeFail" />
      </items>
      <linkto id="632780151249223384" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="reason" type="literal"> time_out</ap>
        <ap name="FunctionName" type="literal">DisplayFindMeFail</ap>
      </Properties>
    </node>
    <node type="Variable" id="632779604614957050" name="timerId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">timerId</Properties>
    </node>
    <node type="Variable" id="632779604614957051" name="action" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="Action" refType="reference">action</Properties>
    </node>
  </canvas>
  <Properties desc="">
  </Properties>
</Application>