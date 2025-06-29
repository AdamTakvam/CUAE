<Application name="Ping" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="Ping">
    <outline>
      <treenode type="evh" id="632129879331250307" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632128058030746512" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632128058030746511" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/click-to-talk/validate</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_AS_DB_Name" id="632507077904073603" vid="632343005715937781">
        <Properties type="String" defaultInitWith="application_suite" initWith="DatabaseName">g_AS_DB_Name</Properties>
      </treenode>
      <treenode text="g_AS_DB_Server" id="632507077904073605" vid="632343005715937783">
        <Properties type="String" defaultInitWith="localhost" initWith="Server">g_AS_DB_Server</Properties>
      </treenode>
      <treenode text="g_AS_DB_Port" id="632507077904073607" vid="632343005715937785">
        <Properties type="String" defaultInitWith="3306" initWith="Port">g_AS_DB_Port</Properties>
      </treenode>
      <treenode text="g_AS_DB_Username" id="632507077904073609" vid="632343005715937787">
        <Properties type="String" defaultInitWith="root" initWith="Username">g_AS_DB_Username</Properties>
      </treenode>
      <treenode text="g_AS_DB_Password" id="632507077904073611" vid="632343005715937791">
        <Properties type="String" defaultInitWith="metreos" initWith="Password">g_AS_DB_Password</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632128058030746513" treenode="632129879331250307" appnode="632128058030746512" handlerfor="632128058030746511">
    <node type="Start" id="632128058030746513" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="32">
      <linkto id="632342375986719634" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632128058030746516" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="404" y="435">
      <linkto id="632341438297501429" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">200</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Validation successful for " + makeCallInfo.Username</log>
      </Properties>
    </node>
    <node type="Action" id="632129001672657138" name="SendResponse" class="MaxActionNode" group="" path="Metreos.Providers.Http" x="248" y="126">
      <linkto id="632341438297501918" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="provider">
        <ap name="remoteHost" type="variable">remoteHost</ap>
        <ap name="responseCode" type="literal">400</ap>
        <ap name="body" type="literal">error</ap>
        <ap name="Content-Type" type="literal">text/plain</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Validation failed for " + makeCallInfo.Username</log>
      </Properties>
    </node>
    <node type="Action" id="632129001672657663" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="245.998627" y="436">
      <linkto id="632129001672657138" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632128058030746516" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native">
        <ap name="Name" type="variable">device</ap>
        <rd field="ResultData">addressLookupResult</rd>
        <log condition="default" on="true" level="Warning" type="csharp">"Failed to resolve device address to IP address"</log>
      </Properties>
    </node>
    <node type="Action" id="632341438297501429" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="528" y="435">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632341438297501918" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="360" y="126">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632342375986719633" name="WebLogin" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="92.57096" y="323">
      <linkto id="632129001672657138" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632343863318125211" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native">
        <ap name="Username" type="csharp">makeCallInfo.Username</ap>
        <ap name="Password" type="csharp">makeCallInfo.Password</ap>
        <ap name="IpAddress" type="variable">remoteHost</ap>
        <rd field="UserId">userId</rd>
        <log condition="default" on="true" level="Warning" type="csharp">"WebLogin Failed"</log>
      </Properties>
    </node>
    <node type="Action" id="632342375986719634" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="91.57096" y="126">
      <linkto id="632342375986719635" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632129001672657138" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="variable">g_AS_DB_Name</ap>
        <ap name="Server" type="variable">g_AS_DB_Server</ap>
        <ap name="Port" type="variable">g_AS_DB_Port</ap>
        <ap name="Username" type="variable">g_AS_DB_Username</ap>
        <ap name="Password" type="variable">g_AS_DB_Password</ap>
        <rd field="DSN">dsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632342375986719635" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="91.57096" y="222">
      <linkto id="632342375986719633" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632129001672657138" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="DSN" type="variable">dsn</ap>
        <ap name="Name" type="literal">ApplicationSuite</ap>
        <ap name="Type" type="literal">mysql</ap>
        <log condition="default" on="true" level="Warning" type="csharp">"Failed to open appsuite database"</log>
      </Properties>
    </node>
    <node type="Action" id="632343863318125211" name="GetPrimaryDeviceForUser" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="92" y="436">
      <linkto id="632129001672657663" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632129001672657138" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="UserId" type="variable">userId</ap>
        <rd field="DeviceAddr">device</rd>
        <log condition="default" on="true" level="Warning" type="csharp">"Failed to get primary device info"</log>
      </Properties>
    </node>
    <node type="Variable" id="632129001672657751" name="addressLookupResult" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">addressLookupResult</Properties>
    </node>
    <node type="Variable" id="632128058030746515" name="remoteHost" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.String" initWith="remoteHost" refType="reference">remoteHost</Properties>
    </node>
    <node type="Variable" id="632129001672657140" name="makeCallInfo" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.ClickToTalk.MakeCallInfo" initWith="body" refType="reference">makeCallInfo</Properties>
    </node>
    <node type="Variable" id="632129001672657215" name="device" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">device</Properties>
    </node>
    <node type="Variable" id="632343005715937793" name="dsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dsn</Properties>
    </node>
    <node type="Variable" id="632343005715937794" name="userId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" refType="reference">userId</Properties>
    </node>
    <node type="Variable" id="632343233749062712" name="body" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="body" refType="reference">body</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>