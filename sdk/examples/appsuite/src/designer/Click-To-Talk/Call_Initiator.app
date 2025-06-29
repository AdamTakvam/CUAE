<Application name="Call_Initiator" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Call_Initiator">
    <outline>
      <treenode type="evh" id="632133019785000120" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632116028259615666" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632116028259615665" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/click-to-talk/initiateCall</ep>
        </Properties>
      </treenode>
      <treenode type="fun" id="632133019785000122" level="1" text="MakeConferenceCall">
        <node type="function" name="MakeConferenceCall" id="632116612447955583" path="Metreos.StockTools" />
        <calls>
          <ref actid="632116612447955579" />
        </calls>
      </treenode>
      <treenode type="fun" id="632133019785000124" level="1" text="MakeSimpleCall">
        <node type="function" name="MakeSimpleCall" id="632116612447955586" path="Metreos.StockTools" />
        <calls>
          <ref actid="632116612447955578" />
        </calls>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_AppServerAddr" id="632544293255752717" vid="632116612447955599">
        <Properties type="Metreos.Types.Int" initWith="AppServerDN">g_AppServerAddr</Properties>
      </treenode>
      <treenode text="g_AS_DB_Server" id="632544293255752719" vid="632342375986719444">
        <Properties type="String" defaultInitWith="localhost" initWith="Server">g_AS_DB_Server</Properties>
      </treenode>
      <treenode text="g_AS_DB_Port" id="632544293255752721" vid="632342375986719446">
        <Properties type="String" defaultInitWith="3306" initWith="Port">g_AS_DB_Port</Properties>
      </treenode>
      <treenode text="g_AS_DB_Name" id="632544293255752723" vid="632342375986719525">
        <Properties type="String" defaultInitWith="application_suite" initWith="DatabaseName">g_AS_DB_Name</Properties>
      </treenode>
      <treenode text="g_AS_DB_Username" id="632544293255752725" vid="632342375986719718">
        <Properties type="String" defaultInitWith="root" initWith="Username">g_AS_DB_Username</Properties>
      </treenode>
      <treenode text="g_AS_DB_Password" id="632544293255752727" vid="632342375986719720">
        <Properties type="String" defaultInitWith="metreos" initWith="Password">g_AS_DB_Password</Properties>
      </treenode>
      <treenode text="g_CM_Username" id="632544293255752729" vid="632344770145937802">
        <Properties type="String" initWith="CCM_Device_Username">g_CM_Username</Properties>
      </treenode>
      <treenode text="g_CM_Password" id="632544293255752731" vid="632344770145937804">
        <Properties type="String" initWith="CCM_Device_Password">g_CM_Password</Properties>
      </treenode>
      <treenode text="g_DialPlan" id="632544293255752733" vid="632472703757844701">
        <Properties type="Hashtable" initWith="DialPlan">g_DialPlan</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" activetab="true" startnode="632116028259615667" treenode="632133019785000120" appnode="632116028259615666" handlerfor="632116028259615665">
    <node type="Start" id="632116028259615667" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632343863318125295" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632116612447955574" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="112.998352" y="518">
      <linkto id="632116612447955575" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632116612447955577" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native" log="On">
        <ap name="Name" type="variable">device</ap>
        <rd field="ResultData">addressLookupResult</rd>
      </Properties>
    </node>
    <node type="Action" id="632116612447955575" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="290.998352" y="77">
      <linkto id="632341438297501374" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">400</ap>
        <ap name="responsePhrase" type="literal">NOT FOUND</ap>
        <ap name="body" type="literal">error</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
        <log condition="entry" on="true" level="Error" type="literal">Could not resolve source phone address</log>
      </Properties>
    </node>
    <node type="Action" id="632116612447955577" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="573" y="73">
      <linkto id="632116612447955582" type="Labeled" style="Bezier" ortho="true" label="1" />
      <linkto id="632127022749568206" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632341438297502486" type="Labeled" style="Bezier" ortho="true" label="0" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">makeCallInfo.Callees.Length</ap>
        <log condition="0" on="true" level="Info" type="literal">No callees specified</log>
      </Properties>
    </node>
    <node type="Action" id="632116612447955578" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="528.5082" y="283" mx="576" my="299">
      <items count="1">
        <item text="MakeSimpleCall" />
      </items>
      <linkto id="632127022749568489" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632129879331250206" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="deviceAddress" type="csharp">addressLookupResult.Rows[0]["IP"]</ap>
        <ap name="calleeName" type="csharp">makeCallInfo.Callees[0].name</ap>
        <ap name="calleeAddr" type="csharp">makeCallInfo.Callees[0].Value</ap>
        <ap name="FunctionName" type="literal">MakeSimpleCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632116612447955579" name="CallFunction" class="MaxCallNode" group="" path="Metreos.ApplicationControl" x="767.070557" y="169" mx="827" my="185">
      <items count="1">
        <item text="MakeConferenceCall" />
      </items>
      <linkto id="632127022749568208" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632129879331250204" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl" log="On">
        <ap name="deviceAddress" type="csharp">addressLookupResult.Rows[0]["IP"]</ap>
        <ap name="makeCallInfo" type="variable">makeCallInfo</ap>
        <ap name="conferencesId" type="variable">conferencesId</ap>
        <ap name="FunctionName" type="literal">MakeConferenceCall</ap>
      </Properties>
    </node>
    <node type="Action" id="632116612447955582" name="Switch" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="574" y="186">
      <linkto id="632116612447955578" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632127022749568206" type="Labeled" style="Bezier" ortho="true" label="true" />
      <Properties final="false" type="native" log="On">
        <ap name="SwitchOn" type="csharp">makeCallInfo.Record</ap>
      </Properties>
    </node>
    <node type="Action" id="632127022749568206" name="SaveConferenceData" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="681.171143" y="186">
      <linkto id="632116612447955579" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632341438297501371" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="HostIP" type="csharp">addressLookupResult.Rows[0]["IP"]</ap>
        <ap name="HostDescription" type="csharp">addressLookupResult.Rows[0]["Description"]</ap>
        <ap name="Record" type="csharp">makeCallInfo.Record</ap>
        <ap name="Email" type="csharp">makeCallInfo.EmailAddress</ap>
        <ap name="HostUsername" type="csharp">makeCallInfo.Username</ap>
        <ap name="HostPassword" type="csharp">makeCallInfo.Password</ap>
        <rd field="ResultData">conferencesId</rd>
        <log condition="default" on="true" level="Error" type="literal">Could not access database</log>
      </Properties>
    </node>
    <node type="Action" id="632127022749568208" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="933.3424" y="183">
      <linkto id="632341438297501370" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="body" type="variable">conferencesId</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632127022749568489" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="522.999146" y="414">
      <linkto id="632341438297501373" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632129879331250204" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="824" y="307">
      <linkto id="632341438297501372" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">400</ap>
        <ap name="body" type="literal">error</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
        <log condition="default" on="true" level="Info" type="literal">Could not communicate with phone</log>
      </Properties>
    </node>
    <node type="Action" id="632129879331250206" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="616.3425" y="413">
      <linkto id="632341438297501373" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider" log="On">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">400</ap>
        <ap name="responsePhrase" type="literal">NOT FOUND</ap>
        <ap name="body" type="literal">error</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
      </Properties>
    </node>
    <node type="Action" id="632341438297501370" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1041.34253" y="184">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632341438297501371" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="680.3424" y="292">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632341438297501372" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="823.3424" y="431">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632341438297501373" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="567.3424" y="511">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632341438297501374" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="400.998352" y="77">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632341438297502486" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="686" y="73">
      <Properties final="true" type="appControl" log="On">
      </Properties>
    </node>
    <node type="Action" id="632343863318125294" name="WebLogin" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="112.809891" y="274">
      <linkto id="632343863318125297" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632116612447955575" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Username" type="csharp">makeCallInfo.Username</ap>
        <ap name="Password" type="csharp">makeCallInfo.Password</ap>
        <ap name="IpAddress" type="variable">remoteHost</ap>
        <rd field="UserId">userId</rd>
        <log condition="default" on="true" level="Info" type="literal">Could not authenticate user</log>
      </Properties>
    </node>
    <node type="Action" id="632343863318125295" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="111.809891" y="77">
      <linkto id="632343863318125296" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632116612447955575" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="variable">g_AS_DB_Name</ap>
        <ap name="Server" type="variable">g_AS_DB_Server</ap>
        <ap name="Port" type="variable">g_AS_DB_Port</ap>
        <ap name="Username" type="variable">g_AS_DB_Username</ap>
        <ap name="Password" type="variable">g_AS_DB_Password</ap>
        <rd field="DSN">dsn</rd>
        <log condition="entry" on="true" level="Info" type="variable">body</log>
      </Properties>
    </node>
    <node type="Action" id="632343863318125296" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="111.809891" y="173">
      <linkto id="632343863318125294" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632116612447955575" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="literal">ApplicationSuite</ap>
        <ap name="Type" type="literal">mysql</ap>
      </Properties>
    </node>
    <node type="Action" id="632343863318125297" name="GetPrimaryDeviceForUser" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="112.23893" y="387">
      <linkto id="632116612447955574" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632116612447955575" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="UserId" type="variable">userId</ap>
        <rd field="DeviceAddr">device</rd>
      </Properties>
    </node>
    <node type="Variable" id="632116628515559681" name="makeCallInfo" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.ClickToTalk.MakeCallInfo" initWith="body" refType="reference">makeCallInfo</Properties>
    </node>
    <node type="Variable" id="632116628515559682" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632116628515559683" name="device" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">device</Properties>
    </node>
    <node type="Variable" id="632116628515559684" name="addressLookupResult" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">addressLookupResult</Properties>
    </node>
    <node type="Variable" id="632127022749568488" name="conferencesId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" refType="reference">conferencesId</Properties>
    </node>
    <node type="Variable" id="632342351832656940" name="userId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">userId</Properties>
    </node>
    <node type="Variable" id="632342369630625258" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
    <node type="Variable" id="632344941593751229" name="body" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="body" refType="reference">body</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="MakeConferenceCall" startnode="632116612447955584" treenode="632133019785000122" appnode="632116612447955583" handlerfor="632116028259615665">
    <node type="Loop" id="632116636827711994" name="Loop" text="loop (expr)" cx="159" cy="107" entry="1" class="MaxLoopContainerNode" group="Application Components" path="Metreos.StockTools" x="221" y="69" mx="300" my="122">
      <linkto id="632116636827711991" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632116636827711991" fromport="1" type="Basic" style="Bezier" ortho="true" />
      <linkto id="632116636827711989" fromport="3" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632341438297501375" fromport="4" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties iteratorType="int" type="csharp">makeCallInfo.Callees.Length</Properties>
    </node>
    <node type="Start" id="632116612447955584" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632116636827711988" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632116636827711989" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="507" y="122">
      <linkto id="632341438297501377" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632341438297501375" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">ciscoExecute.ToString()</ap>
        <ap name="URL" type="variable">deviceAddress</ap>
        <ap name="Username" type="variable">g_CM_Username</ap>
        <ap name="Password" type="variable">g_CM_Password</ap>
        <log condition="entry" on="true" level="Info" type="csharp">String.Format("Placing call from IP: {0} to DN: {1} (Application Server)", deviceAddress, g_AppServerAddr)</log>
        <log condition="failure" on="true" level="Error" type="csharp">String.Format("Could not communicate with Cisco IP phone at {0}", deviceAddress)</log>
      </Properties>
    </node>
    <node type="Action" id="632116636827711988" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="132" y="124">
      <linkto id="632116636827711994" port="1" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"Dial:" + g_AppServerAddr</ap>
        <rd field="ResultData">ciscoExecute</rd>
      </Properties>
    </node>
    <node type="Comment" id="632129879331250209" text="Saves information pertaing to non-host participants, and causes the host's phone to call the Application Server" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="75" y="370" />
    <node type="Action" id="632341438297501375" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="301" y="301">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">failure</ap>
        <log condition="entry" on="true" level="Error" type="literal">Database Error: Failed to save callee info</log>
      </Properties>
    </node>
    <node type="Action" id="632341438297501377" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="660" y="122">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">success</ap>
      </Properties>
    </node>
    <node type="Action" id="632116636827711991" name="SaveCallee" container="632116636827711994" class="MaxActionNode" group="" path="Metreos.Native.ClickToTalk" x="304" y="120">
      <linkto id="632116636827711994" port="3" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="ConferencesID" type="variable">conferencesId</ap>
        <ap name="Name" type="csharp">makeCallInfo.Callees[loopIndex].name</ap>
        <ap name="Address" type="csharp">makeCallInfo.Callees[loopIndex].Value</ap>
      </Properties>
    </node>
    <node type="Comment" id="632543354937775947" text="Failure exit path" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="433.4707" y="267" />
    <node type="Variable" id="632116636827711983" name="deviceAddress" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="deviceAddress" refType="reference">deviceAddress</Properties>
    </node>
    <node type="Variable" id="632116636827711985" name="makeCallInfo" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.ClickToTalk.MakeCallInfo" initWith="makeCallInfo" refType="reference">makeCallInfo</Properties>
    </node>
    <node type="Variable" id="632116636827711986" name="ciscoExecute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">ciscoExecute</Properties>
    </node>
    <node type="Variable" id="632116636827711987" name="conferencesId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="conferencesId" refType="reference">conferencesId</Properties>
    </node>
  </canvas>
  <canvas type="Function" name="MakeSimpleCall" startnode="632116612447955587" treenode="632133019785000124" appnode="632116612447955586" handlerfor="632116028259615665">
    <node type="Start" id="632116612447955587" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="104">
      <linkto id="632130521889844800" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632116636827711979" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="254" y="106">
      <linkto id="632116636827711980" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="URL1" type="csharp">"Dial:" + formattedCallee</ap>
        <rd field="ResultData">ciscoExecute</rd>
        <log condition="exit" on="true" level="Info" type="csharp">String.Format("Placing call from IP: {0} to DN: {1} ({2})", deviceAddress, formattedCallee, calleeName)</log>
      </Properties>
    </node>
    <node type="Action" id="632116636827711980" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="361" y="106">
      <linkto id="632341438297501378" type="Labeled" style="Bezier" ortho="true" label="Success" />
      <linkto id="632543354937775948" type="Labeled" style="Bezier" ortho="true" label="Failure" />
      <Properties final="false" type="native" log="On">
        <ap name="Message" type="csharp">ciscoExecute.ToString()</ap>
        <ap name="URL" type="variable">deviceAddress</ap>
        <ap name="Username" type="variable">g_CM_Username</ap>
        <ap name="Password" type="variable">g_CM_Password</ap>
        <log condition="failure" on="false" level="Error" type="csharp">String.Format("Could not communicate with Cisco IP phone at {0}", deviceAddress)</log>
      </Properties>
    </node>
    <node type="Comment" id="632129879331250211" text="Causes the host's phone to call the other party" class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="226" y="59" />
    <node type="Action" id="632130521889844800" name="FormatAddress" class="MaxActionNode" group="" path="Metreos.Native.DialPlan" x="115" y="104">
      <linkto id="632116636827711979" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native" log="On">
        <ap name="DialedNumber" type="variable">calleeAddr</ap>
        <ap name="DialingRules" type="variable">g_DialPlan</ap>
        <rd field="ResultData">formattedCallee</rd>
      </Properties>
    </node>
    <node type="Action" id="632341438297501378" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="510" y="106">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">success</ap>
      </Properties>
    </node>
    <node type="Action" id="632543354937775948" name="EndFunction" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="360" y="233">
      <Properties final="true" type="appControl" log="On">
        <ap name="ReturnValue" type="literal">failure</ap>
      </Properties>
    </node>
    <node type="Variable" id="632130521889845267" name="formattedCallee" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" refType="reference">formattedCallee</Properties>
    </node>
    <node type="Variable" id="632116636827711973" name="deviceAddress" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="deviceAddress" refType="reference">deviceAddress</Properties>
    </node>
    <node type="Variable" id="632116636827711976" name="calleeName" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="calleeName" refType="reference">calleeName</Properties>
    </node>
    <node type="Variable" id="632116636827711977" name="calleeAddr" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="calleeAddr" refType="reference">calleeAddr</Properties>
    </node>
    <node type="Variable" id="632116636827711978" name="ciscoExecute" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">ciscoExecute</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>