<Application name="SwapScript" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="SwapScript">
    <outline>
      <treenode type="evh" id="632340365902188663" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632340365902188660" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632340365902188659" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/ActiveRelay</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632345018984531467" level="2" text="Metreos.Events.ActiveRelay.SwapRequestResponse: OnSwapRequestResponse">
        <node type="function" name="OnSwapRequestResponse" id="632345018984531464" path="Metreos.StockTools" />
        <node type="event" name="SwapRequestResponse" id="632345018984531463" path="Metreos.Events.ActiveRelay.SwapRequestResponse" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632345614710626236" level="2" text="Metreos.Providers.Http.GotRequest: OnLoginRequest">
        <node type="function" name="OnLoginRequest" id="632345614710626233" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632345614710626232" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/ActiveRelay/Login</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632346509622657336" level="2" text="Metreos.Providers.Http.GotRequest: OnDisplaySwapInfoResponse">
        <node type="function" name="OnDisplaySwapInfoResponse" id="632346509622657333" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632346509622657332" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/ActiveRelay/Swap</ep>
        </Properties>
      </treenode>
      <treenode type="evh" id="632348338276875932" level="2" text="Metreos.Providers.Http.SessionExpired: OnSessionExpired">
        <node type="function" name="OnSessionExpired" id="632348338276875929" path="Metreos.StockTools" />
        <node type="event" name="SessionExpired" id="632348338276875928" path="Metreos.Providers.Http.SessionExpired" />
        <references />
        <Properties type="nonTriggering">
        </Properties>
      </treenode>
      <treenode type="evh" id="632348488393752961" level="2" text="Metreos.Providers.Http.GotRequest: OnExit">
        <node type="function" name="OnExit" id="632348488393752958" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632348488393752957" path="Metreos.Providers.Http.GotRequest" />
        <references />
        <Properties type="hybrid">
          <ep name="url" type="literal">/ActiveRelay/Exit</ep>
        </Properties>
      </treenode>
      <treenode type="fun" id="632345022712031474" level="1" text="RequestLoginAction">
        <node type="function" name="RequestLoginAction" id="632345022712031471" path="Metreos.StockTools" />
        <calls>
          <ref actid="632345022712031470" />
          <ref actid="632345614710627153" />
          <ref actid="632345614710627164" />
          <ref actid="632346509622656609" />
          <ref actid="632346509622657871" />
        </calls>
      </treenode>
      <treenode type="fun" id="632345607743281582" level="1" text="DisplaySwapInfo">
        <node type="function" name="DisplaySwapInfo" id="632345607743281579" path="Metreos.StockTools" />
        <calls>
          <ref actid="632345607743281578" />
          <ref actid="632346509622657331" />
        </calls>
      </treenode>
      <treenode type="fun" id="632345614710627170" level="1" text="PerformSwap">
        <node type="function" name="PerformSwap" id="632345614710627167" path="Metreos.StockTools" />
        <calls>
          <ref actid="632346509622657869" />
        </calls>
      </treenode>
      <treenode type="fun" id="632345964352810676" level="1" text="GetNumberToDial">
        <node type="function" name="GetNumberToDial" id="632345964352810673" path="Metreos.StockTools" />
        <calls>
          <ref actid="632345614710627142" />
          <ref actid="632346509622656606" />
          <ref actid="632346509622656611" />
        </calls>
      </treenode>
      <treenode type="fun" id="632346509622657877" level="1" text="DisplaySwapFail">
        <node type="function" name="DisplaySwapFail" id="632346509622657874" path="Metreos.StockTools" />
        <calls>
          <ref actid="632489818190969978" />
          <ref actid="632348338276875920" />
          <ref actid="632346509622657873" />
          <ref actid="632347416185002379" />
        </calls>
      </treenode>
      <treenode type="fun" id="632674146742754458" level="1" text="OpenDBConnection">
        <node type="function" name="OpenDBConnection" id="632674146742754455" path="Metreos.StockTools" />
        <calls>
          <ref actid="632674146742754454" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="db_poolConnections" id="632766445783248820" vid="632674146742755441">
        <Properties type="Bool" defaultInitWith="true" initWith="DbConnPooling">db_poolConnections</Properties>
      </treenode>
      <treenode text="db_ConnectionName" id="632766445783248822" vid="632347619057191312">
        <Properties type="String" initWith="DbConnectionName">db_ConnectionName</Properties>
      </treenode>
      <treenode text="db_Master_DbName" id="632766445783248824" vid="632346722572969731">
        <Properties type="String" initWith="DbName">db_Master_DbName</Properties>
      </treenode>
      <treenode text="db_Master_DbServer" id="632766445783248826" vid="632346722572969733">
        <Properties type="String" initWith="Server">db_Master_DbServer</Properties>
      </treenode>
      <treenode text="db_Master_Port" id="632766445783248828" vid="632346722572969735">
        <Properties type="UInt" initWith="Port">db_Master_Port</Properties>
      </treenode>
      <treenode text="db_Master_Username" id="632766445783248830" vid="632346722572969737">
        <Properties type="String" initWith="Username">db_Master_Username</Properties>
      </treenode>
      <treenode text="db_Master_Password" id="632766445783248832" vid="632346722572969739">
        <Properties type="String" initWith="Password">db_Master_Password</Properties>
      </treenode>
      <treenode text="db_Slave_DbName" id="632766445783248834" vid="632346722572969731">
        <Properties type="String" initWith="SlaveDBName">db_Slave_DbName</Properties>
      </treenode>
      <treenode text="db_Slave_DbServer" id="632766445783248836" vid="632346722572969733">
        <Properties type="String" initWith="SlaveDBServerAddress">db_Slave_DbServer</Properties>
      </treenode>
      <treenode text="db_Slave_Port" id="632766445783248838" vid="632346722572969735">
        <Properties type="UInt" initWith="SlaveDBServerPort">db_Slave_Port</Properties>
      </treenode>
      <treenode text="db_Slave_Username" id="632766445783248840" vid="632346722572969737">
        <Properties type="String" initWith="SlaveDBServerUsername">db_Slave_Username</Properties>
      </treenode>
      <treenode text="db_Slave_Password" id="632766445783248842" vid="632346722572969739">
        <Properties type="String" initWith="SlaveDBServerPassword">db_Slave_Password</Properties>
      </treenode>
      <treenode text="g_userId" id="632766445783248844" vid="632344838654845368">
        <Properties type="UInt">g_userId</Properties>
      </treenode>
      <treenode text="g_deviceId" id="632766445783248846" vid="632344926321875205">
        <Properties type="UInt">g_deviceId</Properties>
      </treenode>
      <treenode text="g_userPrimaryDeviceMAC" id="632766445783248848" vid="632344838654845365">
        <Properties type="String">g_userPrimaryDeviceMAC</Properties>
      </treenode>
      <treenode text="g_thisRoutingGuid" id="632766445783248850" vid="632345964352810670">
        <Properties type="String">g_thisRoutingGuid</Properties>
      </treenode>
      <treenode text="g_callAppRoutingGuid" id="632766445783248852" vid="632344838654845370">
        <Properties type="String">g_callAppRoutingGuid</Properties>
      </treenode>
      <treenode text="g_primaryDN" id="632766445783248854" vid="632347717270000687">
        <Properties type="String" initWith="CCM_Device_Username">g_primaryDN</Properties>
      </treenode>
      <treenode text="g_callInfo_To" id="632766445783248856" vid="632348488393754798">
        <Properties type="String">g_callInfo_To</Properties>
      </treenode>
      <treenode text="g_from" id="632766445783248858" vid="632345614710625686">
        <Properties type="String">g_from</Properties>
      </treenode>
      <treenode text="g_timeStamp" id="632766445783248860" vid="632345614710625689">
        <Properties type="DateTime">g_timeStamp</Properties>
      </treenode>
      <treenode text="g_loginUrl" id="632766445783248862" vid="632345614710625694">
        <Properties type="String" defaultInitWith="/ActiveRelay/Login">g_loginUrl</Properties>
      </treenode>
      <treenode text="g_swapUrl" id="632766445783248864" vid="632346509622657903">
        <Properties type="String" defaultInitWith="/ActiveRelay/Swap">g_swapUrl</Properties>
      </treenode>
      <treenode text="g_exitUrl" id="632766445783248866" vid="632345614710626678">
        <Properties type="String" defaultInitWith="/ActiveRelay/Exit">g_exitUrl</Properties>
      </treenode>
      <treenode text="g_authenticated" id="632766445783248868" vid="632345022712031493">
        <Properties type="String" defaultInitWith="false">g_authenticated</Properties>
      </treenode>
      <treenode text="g_onRequestHit" id="632766445783248870" vid="632345022712031516">
        <Properties type="Bool" defaultInitWith="false">g_onRequestHit</Properties>
      </treenode>
      <treenode text="g_numberProvidedByUser" id="632766445783248872" vid="632346509622656804">
        <Properties type="Bool" defaultInitWith="false">g_numberProvidedByUser</Properties>
      </treenode>
      <treenode text="g_remoteHost" id="632766445783248874" vid="632346898435000824">
        <Properties type="String">g_remoteHost</Properties>
      </treenode>
      <treenode text="g_remoteIp" id="632766445783248876" vid="632347416185002372">
        <Properties type="String">g_remoteIp</Properties>
      </treenode>
      <treenode text="g_acceptDigit" id="632766445783248878" vid="632673314455190074">
        <Properties type="String" initWith="AcceptDigit">g_acceptDigit</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632340365902188662" treenode="632340365902188663" appnode="632340365902188660" handlerfor="632340365902188659">
    <node type="Start" id="632340365902188662" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="320">
      <linkto id="632345022712031518" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632340365902188972" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1082.707" y="635">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">success</ap>
      </Properties>
    </node>
    <node type="Action" id="632341317830782074" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="234" y="321">
      <linkto id="632381117669219739" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632674146742754454" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties language="csharp">
	public static string Execute(Metreos.Types.Http.QueryParamCollection queryParameters, 		ref string g_userPrimaryDeviceMAC, bool g_onRequestHit, ref string g_thisRoutingGuid, string routingGuid)
	{
		g_onRequestHit = true;
		g_thisRoutingGuid = routingGuid;
		g_userPrimaryDeviceMAC = queryParameters["device"];
		return g_userPrimaryDeviceMAC == null ? IApp.VALUE_FAILURE : IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632344838654845363" name="GetUserByDeviceMac" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="600" y="321">
      <linkto id="632344838654845367" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632345022712031470" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Mac" type="variable">g_userPrimaryDeviceMAC</ap>
        <ap name="IsPrimary" type="literal">true</ap>
        <rd field="UserId">g_userId</rd>
        <rd field="DeviceId">g_deviceId</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"Looking up information for device MAC: " + g_userPrimaryDeviceMAC;
</log>
      </Properties>
    </node>
    <node type="Action" id="632344838654845367" name="GetActiveRelayInfo" class="MaxActionNode" group="" path="Metreos.Native.ActiveRelay" x="741" y="320">
      <linkto id="632345022712031470" type="Labeled" style="Bezier" label="NoRecord" />
      <linkto id="632345022712031470" type="Labeled" style="Bezier" label="default" />
      <linkto id="632489818190969978" type="Labeled" style="Bezier" ortho="true" label="Failure" />
      <linkto id="632345614710627147" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_userId</ap>
        <rd field="AppRoutingGuid">g_callAppRoutingGuid</rd>
        <rd field="TimeStamp">g_timeStamp</rd>
        <rd field="FromNumber">g_from</rd>
        <rd field="ToNumber">g_callInfo_To</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnGotRequest: Retrieving active relay info</log>
      </Properties>
    </node>
    <node type="Action" id="632344926321875204" name="GetPrimaryDnForDevice" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="965" y="319">
      <linkto id="632345607743281578" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632345614710627142" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DeviceId" type="variable">g_deviceId</ap>
        <rd field="PrimaryDN">g_primaryDN</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnGotRequest: GetPrimaryDnForDevice triggered</log>
      </Properties>
    </node>
    <node type="Action" id="632345022712031470" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="684.222656" y="614" mx="742" my="630">
      <items count="1">
        <item text="RequestLoginAction" />
      </items>
      <linkto id="632340365902188972" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="reason" type="literal">authenticate</ap>
        <ap name="host" type="variable">host</ap>
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
    <node type="Action" id="632345607743281578" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1037.2373" y="304" mx="1086" my="320">
      <items count="1">
        <item text="DisplaySwapInfo" />
      </items>
      <linkto id="632340365902188972" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="host" type="variable">host</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="FunctionName" type="literal">DisplaySwapInfo</ap>
      </Properties>
    </node>
    <node type="Action" id="632345614710627142" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="916.0791" y="443" mx="968" my="459">
      <items count="1">
        <item text="GetNumberToDial" />
      </items>
      <linkto id="632340365902188972" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="host" type="variable">host</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="FunctionName" type="literal">GetNumberToDial</ap>
      </Properties>
    </node>
    <node type="Action" id="632345614710627147" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="858.177734" y="320">
      <linkto id="632344926321875204" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_authenticated</rd>
      </Properties>
    </node>
    <node type="Action" id="632381117669219739" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="235" y="453">
      <linkto id="632381117669219742" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Active Relay</ap>
        <ap name="Prompt" type="literal">A system error occured</ap>
        <ap name="Text" type="literal">The Active Relay service is not configured properly. Call swap failed.</ap>
        <rd field="ResultData">responseText</rd>
      </Properties>
    </node>
    <node type="Action" id="632381117669219740" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="454" y="453">
      <linkto id="632381117669219742" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Active Relay</ap>
        <ap name="Prompt" type="literal">A system error occured</ap>
        <ap name="Text" type="literal">Could not access database. Call swap failed.</ap>
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
    <node type="Action" id="632489818190969978" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="694.506836" y="147" mx="743" my="163">
      <items count="1">
        <item text="DisplaySwapFail" />
      </items>
      <linkto id="632489818190969979" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="reason" type="literal">db_failure</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="FunctionName" type="literal">DisplaySwapFail</ap>
      </Properties>
    </node>
    <node type="Action" id="632489818190969979" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="738" y="60">
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
    <node type="Variable" id="632340365902188968" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
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
  </canvas>
  <canvas type="Function" name="OnSwapRequestResponse" startnode="632345018984531466" treenode="632345018984531467" appnode="632345018984531464" handlerfor="632345018984531463">
    <node type="Start" id="632345018984531466" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="387">
      <linkto id="632346898435000814" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632346509622656626" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="684" y="384">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632346898435000814" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="147" y="383">
      <linkto id="632346898435000820" type="Labeled" style="Bezier" ortho="true" label="attempting" />
      <linkto id="632347532121407880" type="Labeled" style="Bezier" ortho="true" label="rejected" />
      <linkto id="632346898435000818" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632568047565415973" type="Labeled" style="Bezier" ortho="true" label="locked" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">response</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnSwapRequestResponse: Function entry</log>
      </Properties>
    </node>
    <node type="Action" id="632346898435000818" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="295" y="383">
      <linkto id="632346898435000821" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Active Relay</ap>
        <ap name="Prompt" type="literal">Call swap attempt failed</ap>
        <ap name="Text" type="literal">The attempt to swap the call to this device failed.</ap>
        <rd field="ResultData">ciscoText</rd>
      </Properties>
    </node>
    <node type="Action" id="632346898435000820" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="298" y="539">
      <linkto id="632346898435000821" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Active Relay</ap>
        <ap name="Prompt" type="literal">Call swap in progress</ap>
        <ap name="Text" type="literal">The system is attempting to swap the call. </ap>
        <rd field="ResultData">ciscoText</rd>
      </Properties>
    </node>
    <node type="Action" id="632346898435000821" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="434" y="383">
      <linkto id="632346898435000822" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">Key:Services</ap>
        <rd field="ResultData">ciscoText</rd>
      </Properties>
    </node>
    <node type="Action" id="632346898435000822" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="568" y="384">
      <linkto id="632346509622656626" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">g_remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">ciscoText.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632347532121407880" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="295" y="227">
      <linkto id="632346898435000821" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Active Relay</ap>
        <ap name="Prompt" type="literal">Call swap attempt rejected</ap>
        <ap name="Text" type="literal">The user failed to accept call swap request.</ap>
        <rd field="ResultData">ciscoText</rd>
      </Properties>
    </node>
    <node type="Action" id="632568047565415973" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="295" y="660">
      <linkto id="632346898435000821" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Active Relay</ap>
        <ap name="Prompt" type="literal">Call Swap not allowed</ap>
        <ap name="Text" type="literal">The system is already attempting to perform a previous swap.</ap>
        <rd field="ResultData">ciscoText</rd>
      </Properties>
    </node>
    <node type="Variable" id="632346898435000812" name="response" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="response" refType="reference">response</Properties>
    </node>
    <node type="Variable" id="632346898435000813" name="ciscoText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">ciscoText</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OnLoginRequest" startnode="632345614710626235" treenode="632345614710626236" appnode="632345614710626233" handlerfor="632345614710626232">
    <node type="Start" id="632345614710626235" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="570">
      <linkto id="632346509622656601" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632345614710627133" name="WebLogin" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="792" y="577">
      <linkto id="632345614710627157" type="Labeled" style="Bezier" label="InvalidAccountCodeOrPin" />
      <linkto id="632345614710627160" type="Labeled" style="Bezier" label="NotAllowedDueToDisabled" />
      <linkto id="632345614710627162" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632345614710627160" type="Labeled" style="Bezier" label="NotAllowedDueToDeleted" />
      <linkto id="632345614710627161" type="Labeled" style="Bezier" ortho="true" label="NotAllowedDueLocked" />
      <linkto id="632345614710627157" type="Labeled" style="Bezier" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Username" type="variable">username</ap>
        <ap name="Password" type="variable">password</ap>
        <ap name="IpAddress" type="variable">remoteIp</ap>
        <rd field="UserId">g_userId</rd>
      </Properties>
    </node>
    <node type="Action" id="632345614710627152" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="481" y="573">
      <linkto id="632345614710627133" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632345614710627153" type="Labeled" style="Bezier" ortho="true" label="default" />
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
        <ap name="host" type="variable">host</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="FunctionName" type="literal">RequestLoginAction</ap>
      </Properties>
    </node>
    <node type="Action" id="632345614710627154" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="487" y="966">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632345614710627157" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1001" y="789">
      <linkto id="632345614710627164" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">failed</ap>
        <rd field="ResultData">loginFailReason</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnLoginRequest: Authentication: Default or Invalid pass</log>
      </Properties>
    </node>
    <node type="Action" id="632345614710627160" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1221" y="583">
      <linkto id="632345614710627164" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">disabled</ap>
        <rd field="ResultData">loginFailReason</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnLoginRequest: Authentication: Disabled or Deleted</log>
      </Properties>
    </node>
    <node type="Action" id="632345614710627161" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="792" y="974">
      <linkto id="632345614710627164" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">too_many_attempts</ap>
        <rd field="ResultData">loginFailReason</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnLoginRequest: Authentication: Locked</log>
      </Properties>
    </node>
    <node type="Action" id="632345614710627162" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="792" y="465">
      <linkto id="632348488393751946" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_authenticated</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnLoginRequest: Authentication: Success</log>
      </Properties>
    </node>
    <node type="Action" id="632345614710627164" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="1171.22266" y="960" mx="1229" my="976">
      <items count="1">
        <item text="RequestLoginAction" />
      </items>
      <linkto id="632345614710627165" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="reason" type="variable">loginFailReason</ap>
        <ap name="host" type="variable">host</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="FunctionName" type="literal">RequestLoginAction</ap>
      </Properties>
    </node>
    <node type="Action" id="632345614710627165" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1348.04883" y="975">
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
      <linkto id="632346509622656806" type="Labeled" style="Bezier" ortho="true" label="getnum" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">action</ap>
      </Properties>
    </node>
    <node type="Action" id="632346509622656604" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="267" y="355">
      <linkto id="632346509622656609" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632346509622656610" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_authenticated</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnLoginRequest: The value of g_primaryDN is: " + g_primaryDN;</log>
      </Properties>
    </node>
    <node type="Action" id="632346509622656606" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="740.5781" y="16" mx="793" my="32">
      <items count="1">
        <item text="GetNumberToDial" />
      </items>
      <linkto id="632346509622656607" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="host" type="variable">host</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="FunctionName" type="literal">GetNumberToDial</ap>
      </Properties>
    </node>
    <node type="Action" id="632346509622656607" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="556" y="32">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632346509622656608" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="272" y="754">
      <Properties final="true" type="appControl" log="On">
        <log condition="entry" on="true" level="Info" type="literal">OnLoginRequest: Unknown action requested</log>
      </Properties>
    </node>
    <node type="Action" id="632346509622656609" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="213.222656" y="137" mx="271" my="153">
      <items count="1">
        <item text="RequestLoginAction" />
      </items>
      <linkto id="632346509622656607" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="host" type="variable">host</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="reason" type="literal">authenticate</ap>
        <ap name="FunctionName" type="literal">RequestLoginAction</ap>
      </Properties>
    </node>
    <node type="Action" id="632346509622656610" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="407" y="356">
      <linkto id="632346509622656611" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632348338276875919" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties language="csharp">
	public static string Execute(Metreos.Types.Http.QueryParamCollection queryParameters, ref string g_primaryDN)
	{
		g_primaryDN = queryParameters["number"];
		if (g_primaryDN == null)
		{
			g_primaryDN = string.Empty;
			return IApp.VALUE_FAILURE;
		}
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632346509622656611" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="357.578125" y="135" mx="410" my="151">
      <items count="1">
        <item text="GetNumberToDial" />
      </items>
      <linkto id="632346509622656607" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="host" type="variable">host</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="FunctionName" type="literal">GetNumberToDial</ap>
      </Properties>
    </node>
    <node type="Action" id="632346509622656614" name="GetPrimaryDnForDevice" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="792" y="152">
      <linkto id="632346509622656606" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632346509622657331" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="DeviceName" type="variable">g_userPrimaryDeviceMAC</ap>
        <rd field="PrimaryDN">g_primaryDN</rd>
        <log condition="entry" on="true" level="Info" type="csharp">"Looking up primary directory number for device with deviceId: " + g_deviceId;</log>
        <log condition="exit" on="true" level="Info" type="csharp">"OnLoginRequest: Obtained primary directory number: " + g_primaryDN;</log>
      </Properties>
    </node>
    <node type="Action" id="632346509622656806" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="268" y="461">
      <linkto id="632346509622656604" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">true</ap>
        <rd field="ResultData">g_numberProvidedByUser</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnLoginRequest: Requesting number from user</log>
      </Properties>
    </node>
    <node type="Action" id="632346509622656807" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="791" y="246">
      <linkto id="632346509622656614" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632346509622657331" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_numberProvidedByUser</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnLoginRequest: Checking to see if number was provided by user</log>
      </Properties>
    </node>
    <node type="Action" id="632346509622657331" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="510.2373" y="134" mx="559" my="150">
      <items count="1">
        <item text="DisplaySwapInfo" />
      </items>
      <linkto id="632346509622656607" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="host" type="variable">host</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="FunctionName" type="literal">DisplaySwapInfo</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnLoginRequest: Calling DisplaySwapInfo</log>
      </Properties>
    </node>
    <node type="Action" id="632348338276875919" name="GetActiveRelayInfo" class="MaxActionNode" group="" path="Metreos.Native.ActiveRelay" x="557" y="357">
      <linkto id="632346509622657331" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632348338276875927" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_userId</ap>
        <rd field="AppRoutingGuid">g_callAppRoutingGuid</rd>
        <rd field="TimeStamp">g_timeStamp</rd>
        <rd field="FromNumber">g_from</rd>
        <rd field="ToNumber">g_callInfo_To</rd>
      </Properties>
    </node>
    <node type="Action" id="632348338276875920" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="941.506836" y="332" mx="990" my="348">
      <items count="1">
        <item text="DisplaySwapFail" />
      </items>
      <linkto id="632348338276875926" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="reason" type="literal">no_info</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="FunctionName" type="literal">DisplaySwapFail</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnLoginRequest: Calling DisplaySwapFail</log>
      </Properties>
    </node>
    <node type="Label" id="632348338276875925" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="985.519531" y="247">
      <linkto id="632348338276875920" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632348338276875926" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1127.51953" y="348">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Label" id="632348338276875927" text="A" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="557" y="459" />
    <node type="Action" id="632348488393751946" name="GetActiveRelayInfo" class="MaxActionNode" group="" path="Metreos.Native.ActiveRelay" x="791" y="347">
      <linkto id="632346509622656807" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632348338276875920" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_userId</ap>
        <rd field="AppRoutingGuid">g_callAppRoutingGuid</rd>
        <rd field="TimeStamp">g_timeStamp</rd>
        <rd field="FromNumber">g_from</rd>
        <rd field="ToNumber">g_callInfo_To</rd>
        <log condition="entry" on="true" level="Info" type="literal">OnLoginRequest: Retrieving Active Relay Info</log>
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
  <canvas type="Function" name="OnDisplaySwapInfoResponse" startnode="632346509622657335" treenode="632346509622657336" appnode="632346509622657333" handlerfor="632346509622657332">
    <node type="Start" id="632346509622657335" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="280">
      <linkto id="632346509622657867" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632346509622657867" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="141" y="280">
      <linkto id="632346509622657871" type="Labeled" style="Bezier" ortho="true" label="false" />
      <linkto id="632347416185002377" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_authenticated</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnDisplaySwapInfoResponse: Function Entry</log>
      </Properties>
    </node>
    <node type="Action" id="632346509622657869" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="348.651367" y="258" mx="389" my="274">
      <items count="1">
        <item text="PerformSwap" />
      </items>
      <linkto id="632346509622657870" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="host" type="variable">host</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="FunctionName" type="literal">PerformSwap</ap>
        <log condition="entry" on="true" level="Info" type="literal">OnDisplaySwapInfoResponse: Calling PerformSwap</log>
      </Properties>
    </node>
    <node type="Action" id="632346509622657870" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="526" y="272">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632346509622657871" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="86.22266" y="359" mx="144" my="375">
      <items count="1">
        <item text="RequestLoginAction" />
      </items>
      <linkto id="632346509622657872" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="reason" type="literal">authenticate</ap>
        <ap name="host" type="variable">host</ap>
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="FunctionName" type="literal">RequestLoginAction</ap>
      </Properties>
    </node>
    <node type="Action" id="632346509622657872" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="141" y="504">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632347416185002377" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="281" y="275">
      <linkto id="632346509622657869" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">remoteIp</ap>
        <rd field="ResultData">g_remoteIp</rd>
      </Properties>
    </node>
    <node type="Variable" id="632346509622657865" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632346509622657866" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632347416185002378" name="remoteIp" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteIpAddress" refType="reference">remoteIp</Properties>
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
  <canvas type="Function" name="OnExit" startnode="632348488393752960" treenode="632348488393752961" appnode="632348488393752958" handlerfor="632348488393752957">
    <node type="Start" id="632348488393752960" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="419">
      <linkto id="632348488393753230" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632348488393753230" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="138" y="419">
      <linkto id="632348488393753236" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632348597268752026" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">OnExit: function entry</log>
	public static string Execute(Metreos.Types.Http.QueryParamCollection queryParameters, ref string result)
	{
		result = queryParameters["result"];
		if (result == null)
			return IApp.VALUE_FAILURE;
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632348488393753236" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="257" y="418">
      <linkto id="632348488393753237" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632348488393753238" type="Labeled" style="Bezier" ortho="true" label="fail" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">result</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"OnExit: the value of result is: " + result;</log>
      </Properties>
    </node>
    <node type="Action" id="632348488393753237" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="383" y="328">
      <linkto id="632348488393753239" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">Call swap completed</ap>
        <ap name="Value2" type="literal">The call swap was successful</ap>
        <rd field="ResultData">prompt</rd>
        <rd field="ResultData2">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632348488393753238" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="384" y="521">
      <linkto id="632348488393753239" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">Call swap failed</ap>
        <ap name="Value2" type="literal">The attempt to swap your call failed.</ap>
        <rd field="ResultData">prompt</rd>
        <rd field="ResultData2">text</rd>
      </Properties>
    </node>
    <node type="Action" id="632348488393753239" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="505" y="417">
      <linkto id="632348488393753240" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Active Relay</ap>
        <ap name="Prompt" type="variable">prompt</ap>
        <ap name="Text" type="variable">text</ap>
        <rd field="ResultData">ciscoText</rd>
      </Properties>
    </node>
    <node type="Action" id="632348488393753240" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="618" y="418">
      <linkto id="632348488393753241" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">Key:Services</ap>
        <rd field="ResultData">ciscoText</rd>
      </Properties>
    </node>
    <node type="Action" id="632348488393753241" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="753" y="419">
      <linkto id="632348488393754023" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">ciscoText.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632348488393754023" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="848.9414" y="420">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632348597268752026" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="139" y="566">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Variable" id="632348488393753227" name="queryParameters" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.Http.QueryParamCollection" initWith="query" refType="reference">queryParameters</Properties>
    </node>
    <node type="Variable" id="632348488393753229" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632348488393753231" name="ciscoText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">ciscoText</Properties>
    </node>
    <node type="Variable" id="632348488393753232" name="prompt" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">prompt</Properties>
    </node>
    <node type="Variable" id="632348488393753233" name="text" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">text</Properties>
    </node>
    <node type="Variable" id="632348488393753234" name="result" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">result</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="RequestLoginAction" startnode="632345022712031473" treenode="632345022712031474" appnode="632345022712031471" handlerfor="632348488393752957">
    <node type="Start" id="632345022712031473" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="379">
      <linkto id="632346509622656808" type="Basic" style="Vector" ortho="true" />
    </node>
    <node type="Action" id="632345022712031480" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="238" y="380">
      <linkto id="632345022712031481" type="Labeled" style="Bezier" ortho="true" label="failed" />
      <linkto id="632345022712031482" type="Labeled" style="Bezier" label="authenticate" />
      <linkto id="632345022712031483" type="Labeled" style="Bezier" ortho="true" label="disabled" />
      <linkto id="632345022712031482" type="Labeled" style="Bezier" label="default" />
      <linkto id="632345022712031504" type="Labeled" style="Bezier" ortho="true" label="too_many_attempts" />
      <linkto id="632345614710627159" type="Labeled" style="Bezier" ortho="true" label="no_username" />
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
        <ap name="Title" type="literal">Active Relay</ap>
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
    <node type="Action" id="632345022712031515" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1024.93689" y="544">
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
        <ap name="Title" type="literal">Active Relay</ap>
        <ap name="Prompt" type="variable">userReasonString</ap>
        <ap name="URL" type="csharp">host + g_loginUrl + "?action=auth&amp;metreosSessionId=" + g_thisRoutingGuid;</ap>
        <rd field="ResultData">inputCisco</rd>
      </Properties>
    </node>
    <node type="Action" id="632345614710627139" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="809" y="543">
      <linkto id="632345614710627140" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DisplayName" type="literal">Pass</ap>
        <ap name="QueryStringParam" type="literal">pass</ap>
        <ap name="InputFlags" type="literal">PA</ap>
        <rd field="ResultData">inputCisco</rd>
      </Properties>
    </node>
    <node type="Action" id="632345614710627140" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="912.4076" y="544">
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
        <ap name="Value2" type="literal">false</ap>
        <rd field="ResultData">g_numberProvidedByUser</rd>
        <rd field="ResultData2">g_authenticated</rd>
        <log condition="entry" on="true" level="Info" type="literal">RequestLoginAction: Function Entry</log>
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
    <node type="Variable" id="632345614710627128" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632345614710627129" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632345614710627137" name="inputCisco" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Input" refType="reference">inputCisco</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="DisplaySwapInfo" startnode="632345607743281581" treenode="632345607743281582" appnode="632345607743281579" handlerfor="632348488393752957">
    <node type="Start" id="632345607743281581" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="36" y="408">
      <linkto id="632345614710625688" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632345607743281583" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="250.000122" y="409">
      <linkto id="632345614710625691" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Active Relay</ap>
        <ap name="Prompt" type="literal">Choose your action</ap>
        <ap name="Text" type="variable">callInfoText</ap>
        <rd field="ResultData">ciscoText</rd>
        <log condition="entry" on="false" level="Info" type="csharp">"DisplaySwapInfo: The value of callInfoText is: " + callInfoText;</log>
      </Properties>
    </node>
    <node type="Action" id="632345607743281587" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="599" y="409">
      <linkto id="632345614710625693" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">ciscoText.ToString();</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632345614710625688" name="CustomCode" class="MaxCodeNode" group="Application Components" path="Metreos.StockTools" x="143" y="408">
      <linkto id="632345607743281583" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties language="csharp">
        <log condition="entry" on="true" level="Info" type="literal">DisplaySwapInfo: Function Entry</log>
	public static string Execute(DateTime g_timeStamp, string g_from, string g_callInfo_To, string g_acceptDigit, ref string callInfoText)
	{
		callInfoText = "From: " + g_from + "\n";
		callInfoText += "To: " + g_callInfo_To + "\n";
		System.Globalization.CultureInfo culture = 						      	System.Globalization.CultureInfo.CurrentCulture;
		IFormatProvider format = culture.DateTimeFormat;
		callInfoText += "Received on: " + g_timeStamp.ToString(format);
		callInfoText += "\nAfter pressing the swap key press '" + g_acceptDigit + "' on the phone currently holding the call to accept the call swap.";
		return IApp.VALUE_SUCCESS;
	}
</Properties>
    </node>
    <node type="Action" id="632345614710625691" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="377" y="408">
      <linkto id="632345614710625692" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Swap</ap>
        <ap name="Position" type="literal">1</ap>
        <ap name="URL" type="csharp">host + g_swapUrl + "?metreosSessionId=" + g_thisRoutingGuid;</ap>
        <rd field="ResultData">ciscoText</rd>
      </Properties>
    </node>
    <node type="Action" id="632345614710625692" name="AddSoftKeyItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="486" y="410">
      <linkto id="632345607743281587" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="literal">Exit</ap>
        <ap name="Position" type="literal">3</ap>
        <ap name="URL" type="literal">Key:Services</ap>
        <rd field="ResultData">ciscoText</rd>
      </Properties>
    </node>
    <node type="Action" id="632345614710625693" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="711" y="406">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Variable" id="632345607743281584" name="ciscoText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">ciscoText</Properties>
    </node>
    <node type="Variable" id="632345607743281585" name="callInfoText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">callInfoText</Properties>
    </node>
    <node type="Variable" id="632345607743281586" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632346509622656597" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="PerformSwap" startnode="632345614710627169" treenode="632345614710627170" appnode="632345614710627167" handlerfor="632348488393752957">
    <node type="Start" id="632345614710627169" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="39" y="302">
      <linkto id="632345614710627179" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632345614710627179" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="156" y="301">
      <linkto id="632345614710627181" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632346509622656619" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value1" type="variable">g_authenticated</ap>
        <log condition="entry" on="true" level="Info" type="literal">PerformSwap: Function Entry</log>
      </Properties>
    </node>
    <node type="Comment" id="632345614710627180" text="If we get here, then&#xD;&#xA;g_userId contains the&#xD;&#xA;correct userId" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="79" y="226" />
    <node type="Action" id="632345614710627181" name="GetActiveRelayInfo" class="MaxActionNode" group="" path="Metreos.Native.ActiveRelay" x="291" y="300">
      <linkto id="632346509622657873" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632346898435000823" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">g_userId</ap>
        <rd field="AppRoutingGuid">g_callAppRoutingGuid</rd>
        <rd field="TimeStamp">g_timeStamp</rd>
        <rd field="FromNumber">g_from</rd>
        <log condition="entry" on="true" level="Info" type="literal">PerformSwap: Retrieving ActiveRelay info</log>
      </Properties>
    </node>
    <node type="Action" id="632345870537966497" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="648" y="303">
      <linkto id="632345870537966498" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632347416185002379" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="userId" type="variable">g_userId</ap>
        <ap name="directoryNumber" type="variable">g_primaryDN</ap>
        <ap name="senderRoutingGuid" type="variable">g_thisRoutingGuid</ap>
        <ap name="ciscoPhoneIp" type="variable">g_remoteIp</ap>
        <ap name="responseUrl" type="variable">responseUrl</ap>
        <ap name="EventName" type="literal">Metreos.Events.ActiveRelay.SwapRequest</ap>
        <ap name="ToGuid" type="variable">g_callAppRoutingGuid</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"PerformSwap: Sending SwapRequest to call control script at routing guid: " + g_callAppRoutingGuid;</log>
      </Properties>
    </node>
    <node type="Action" id="632345870537966498" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="860.4707" y="302">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Comment" id="632345988226877767" text="If we fail here&#xD;&#xA;display message&#xD;&#xA;to user and endscript" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="229.4707" y="226" />
    <node type="Comment" id="632345988226877768" text="if we fail&#xD;&#xA;here, notify&#xD;&#xA;user that &#xD;&#xA;call was &#xD;&#xA;already&#xD;&#xA;swapped&#xD;&#xA;and end&#xD;&#xA;script" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="374.4707" y="155" />
    <node type="Action" id="632346509622656619" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="155" y="413">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Comment" id="632346509622656625" text="Will have to make sure event&#xD;&#xA;gets there" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="570.4707" y="238" />
    <node type="Action" id="632346509622657873" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="245.506836" y="382" mx="294" my="398">
      <items count="1">
        <item text="DisplaySwapFail" />
      </items>
      <linkto id="632346509622657890" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="reason" type="literal">no_info</ap>
        <ap name="FunctionName" type="literal">DisplaySwapFail</ap>
      </Properties>
    </node>
    <node type="Action" id="632346509622657890" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="296" y="565">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632346898435000823" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="478.4707" y="302">
      <linkto id="632345870537966497" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="variable">remoteHost</ap>
        <ap name="Value2" type="csharp">host + g_exitUrl</ap>
        <rd field="ResultData">g_remoteHost</rd>
        <rd field="ResultData2">responseUrl</rd>
      </Properties>
    </node>
    <node type="Action" id="632347416185002379" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="710.506836" y="437" mx="759" my="453">
      <items count="1">
        <item text="DisplaySwapFail" />
      </items>
      <linkto id="632345870537966498" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="FunctionName" type="literal">DisplaySwapFail</ap>
      </Properties>
    </node>
    <node type="Variable" id="632345614710627173" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632345614710627174" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632347532121407157" name="responseUrl" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">responseUrl</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="GetNumberToDial" startnode="632345964352810675" treenode="632345964352810676" appnode="632345964352810673" handlerfor="632348488393752957">
    <node type="Start" id="632345964352810675" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="36" y="390">
      <linkto id="632345988226877762" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632345988226877762" name="CreateInput" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="150" y="390">
      <linkto id="632345988226877763" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Active Relay</ap>
        <ap name="Prompt" type="literal">Specify swap number</ap>
        <ap name="URL" type="csharp">host + g_loginUrl + "?action=getnum&amp;metreosSessionId=" + g_thisRoutingGuid;</ap>
        <rd field="ResultData">input</rd>
        <log condition="entry" on="true" level="Info" type="literal">GetNumberToDial: Function Entry</log>
      </Properties>
    </node>
    <node type="Action" id="632345988226877763" name="AddInputItem" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="251" y="390">
      <linkto id="632346509622656596" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DisplayName" type="literal">Number</ap>
        <ap name="QueryStringParam" type="literal">number</ap>
        <ap name="InputFlags" type="literal">N</ap>
        <rd field="ResultData">input</rd>
      </Properties>
    </node>
    <node type="Action" id="632346509622656596" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="358" y="389">
      <linkto id="632346509622656600" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">input.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632346509622656600" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="463" y="389">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">default</ap>
      </Properties>
    </node>
    <node type="Variable" id="632345964352810868" name="host" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="host" refType="reference">host</Properties>
    </node>
    <node type="Variable" id="632345988226877760" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632345988226877761" name="input" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Input" refType="reference">input</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="DisplaySwapFail" startnode="632346509622657876" treenode="632346509622657877" appnode="632346509622657874" handlerfor="632348488393752957">
    <node type="Start" id="632346509622657876" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="299">
      <linkto id="632346509622657895" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632346509622657879" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="273.048828" y="232">
      <linkto id="632346509622657883" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">Call information not found.</ap>
        <ap name="Value2" type="literal">Did not find any active relay calls to swap.</ap>
        <rd field="ResultData">promptText</rd>
        <rd field="ResultData2">textText</rd>
      </Properties>
    </node>
    <node type="Action" id="632346509622657880" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="278.048828" y="546">
      <linkto id="632346509622657883" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">Call was already swapped</ap>
        <ap name="Value2" type="literal">Your call was already swapped to a desk device.</ap>
        <rd field="ResultData">promptText</rd>
        <rd field="ResultData2">textText</rd>
      </Properties>
    </node>
    <node type="Action" id="632346509622657883" name="CreateText" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="432.644531" y="300">
      <linkto id="632346509622657884" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Title" type="literal">Active Relay</ap>
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
    <node type="Action" id="632346509622657885" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="687.1738" y="302">
      <linkto id="632346509622657894" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="responsePhrase" type="literal">OK</ap>
        <ap name="body" type="csharp">ciscoText.ToString()</ap>
        <ap name="Content-Type" type="literal">text/xml</ap>
      </Properties>
    </node>
    <node type="Action" id="632346509622657894" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="787" y="303">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632346509622657895" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="123" y="298">
      <linkto id="632346509622657880" type="Labeled" style="Bezier" ortho="true" label="already_swapped" />
      <linkto id="632347416185002364" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632346509622657879" type="Labeled" style="Bezier" ortho="true" label="no_info" />
      <linkto id="632489818190969981" type="Labeled" style="Bezier" ortho="true" label="db_failure" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="variable">reason</ap>
        <log condition="entry" on="true" level="Info" type="literal">DisplaySwapFail: Function Entry</log>
      </Properties>
    </node>
    <node type="Action" id="632347416185002364" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="275" y="386">
      <linkto id="632346509622657883" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">Call swap failed</ap>
        <ap name="Value2" type="literal">Your call could not be swapped</ap>
        <rd field="ResultData">promptText</rd>
        <rd field="ResultData2">textText</rd>
      </Properties>
    </node>
    <node type="Action" id="632489818190969981" name="Assign" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="275" y="77">
      <linkto id="632346509622657883" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Value" type="literal">Failure</ap>
        <ap name="Value2" type="literal">Active Relay is currently unavailable.  Please try again later or contact your Systems Administrator.</ap>
        <rd field="ResultData">promptText</rd>
        <rd field="ResultData2">textText</rd>
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
    <node type="Variable" id="632346509622657898" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632346509622659070" name="ciscoText" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Text" refType="reference">ciscoText</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="OpenDBConnection" startnode="632674146742754457" treenode="632674146742754458" appnode="632674146742754455" handlerfor="632348488393752957">
    <node type="Start" id="632674146742754457" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="85" y="144">
      <linkto id="632674146742754460" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632674146742754459" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="422.188477" y="143.5">
      <linkto id="632674146742754463" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632674146742754466" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="csharp">dsn + "; connection timeout=1;"</ap>
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
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632674146742754461" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="706.1886" y="328.5">
      <linkto id="632674146742754463" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632674146742754465" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="csharp">dsn + "; connection timeout=1;"
</ap>
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
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>