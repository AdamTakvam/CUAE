<Application name="IntercomAddWorker" trigger="Metreos.Providers.Http.GotRequest" version="0.8" single="false">
  <!--//// app tree ////-->
  <global name="IntercomAddWorker">
    <outline>
      <treenode type="evh" id="632347578953310799" level="1" text="Metreos.Providers.Http.GotRequest (trigger): OnGotRequest">
        <node type="function" name="OnGotRequest" id="632347578953310796" path="Metreos.StockTools" />
        <node type="event" name="GotRequest" id="632347578953310795" path="Metreos.Providers.Http.GotRequest" trigger="true" />
        <Properties type="hybrid">
          <ep name="url" type="literal">/IntercomAddWorker</ep>
        </Properties>
      </treenode>
    </outline>
    <variables>
      <treenode text="g_dbHost" id="632373833087627398" vid="632348158383489389">
        <Properties type="String" initWith="Server">g_dbHost</Properties>
      </treenode>
      <treenode text="g_dbPort" id="632373833087627400" vid="632348158383489391">
        <Properties type="UInt" initWith="Port">g_dbPort</Properties>
      </treenode>
      <treenode text="g_dbUsername" id="632373833087627402" vid="632348158383489393">
        <Properties type="String" initWith="Username">g_dbUsername</Properties>
      </treenode>
      <treenode text="g_dbPassword" id="632373833087627404" vid="632348158383489395">
        <Properties type="String" initWith="Password">g_dbPassword</Properties>
      </treenode>
      <treenode text="g_dbName" id="632373833087627406" vid="632348158383489397">
        <Properties type="String" defaultInitWith="application_suite">g_dbName</Properties>
      </treenode>
      <treenode text="g_ccmDeviceUsername" id="632373833087627408" vid="632348158383489399">
        <Properties type="String" initWith="CCM_Device_Username">g_ccmDeviceUsername</Properties>
      </treenode>
      <treenode text="g_ccmDevicePassword" id="632373833087627410" vid="632348158383489401">
        <Properties type="String" initWith="CCM_Device_Password">g_ccmDevicePassword</Properties>
      </treenode>
    </variables>
  </global>
  <!--//// visible canvases ////-->
  <canvas type="Function" name="OnGotRequest" startnode="632347578953310798" treenode="632347578953310799" appnode="632347578953310796" handlerfor="632347578953310795">
    <node type="Start" id="632347578953310798" name="Start" class="MaxStartNode" group="Metreos.StockTools" path="Metreos.StockTools" x="32" y="200">
      <linkto id="632347578953311217" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Action" id="632347578953310802" name="GetPrimaryDeviceForUser" class="MaxActionNode" group="" path="Metreos.ApplicationSuite.Actions" x="384" y="200">
      <linkto id="632347578953310805" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632347578953310939" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="UserId" type="variable">userId</ap>
        <rd field="DeviceAddr">deviceId</rd>
        <log condition="default" on="true" level="Info" type="csharp">"Error: No devices for user with ID " + userId</log>
      </Properties>
    </node>
    <node type="Action" id="632347578953310805" name="Query" class="MaxActionNode" group="" path="Metreos.Native.CiscoDeviceList" x="560" y="200">
      <linkto id="632347578953310942" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632350316832032217" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native">
        <ap name="Name" type="variable">deviceId</ap>
        <ap name="Status" type="literal">2</ap>
        <rd field="ResultData">dlxResult</rd>
      </Properties>
    </node>
    <node type="Action" id="632347578953310933" name="CreateExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="824" y="200">
      <linkto id="632347578953310934" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="URL1" type="csharp">"http://" + appServerIpAddress + ":8000/IntercomAddOk?deviceId=" + deviceId + "&amp;metreosSessionId=" + fromGuid</ap>
        <ap name="Priority1" type="literal">2</ap>
        <rd field="ResultData">executeItem</rd>
      </Properties>
    </node>
    <node type="Action" id="632347578953310934" name="SendExecute" class="MaxActionNode" group="" path="Metreos.Native.CiscoIpPhone" x="952" y="200">
      <linkto id="632347578953310944" type="Labeled" style="Bezier" ortho="true" label="default" />
      <linkto id="632347578953310945" type="Labeled" style="Bezier" ortho="true" label="success" />
      <Properties final="false" type="native">
        <ap name="Message" type="variable">executeItem</ap>
        <ap name="URL" type="csharp">dlxResult.Rows[0]["IP"]</ap>
        <ap name="Username" type="variable">g_ccmDeviceUsername</ap>
        <ap name="Password" type="variable">g_ccmDevicePassword</ap>
        <log condition="entry" on="true" level="Info" type="csharp">"Send to : " + dlxResult.Rows[0]["IP"]</log>
      </Properties>
    </node>
    <node type="Action" id="632347578953310939" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="384" y="320">
      <linkto id="632347578953310940" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="status" type="literal">OK</ap>
        <ap name="url" type="literal">/IntercomAddWorkerComplete</ap>
        <ap name="userId" type="variable">userId</ap>
        <ap name="EventName" type="literal">Metreos.Providers.Http.GotRequest</ap>
        <ap name="ToGuid" type="variable">fromGuid</ap>
      </Properties>
    </node>
    <node type="Action" id="632347578953310940" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="384" y="424">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Label" id="632347578953310941" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="296" y="320">
      <linkto id="632347578953310939" type="Basic" style="Bezier" ortho="true" />
    </node>
    <node type="Label" id="632347578953310942" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="560" y="296" />
    <node type="Label" id="632347578953310944" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="952" y="296" />
    <node type="Action" id="632347578953310945" name="SendEvent" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1104" y="200">
      <linkto id="632347578953310946" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="appControl">
        <ap name="status" type="literal">OK</ap>
        <ap name="url" type="literal">/IntercomAddWorkerComplete</ap>
        <ap name="userId" type="variable">userId</ap>
        <ap name="EventName" type="literal">Metreos.Providers.Http.GotRequest</ap>
        <ap name="ToGuid" type="variable">fromGuid</ap>
      </Properties>
    </node>
    <node type="Action" id="632347578953310946" name="EndScript" class="MaxActionNode" group="" path="Metreos.ApplicationControl" x="1216" y="200">
      <Properties final="true" type="appControl">
      </Properties>
    </node>
    <node type="Action" id="632347578953311217" name="FormatDSN" class="MaxActionNode" group="" path="Metreos.Native.Database" x="128" y="200">
      <linkto id="632347578953311218" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Type" type="literal">mysql</ap>
        <ap name="DatabaseName" type="variable">g_dbName</ap>
        <ap name="Server" type="variable">g_dbHost</ap>
        <ap name="Port" type="variable">g_dbPort</ap>
        <ap name="Username" type="variable">g_dbUsername</ap>
        <ap name="Password" type="variable">g_dbPassword</ap>
        <rd field="DSN">dbDsn</rd>
      </Properties>
    </node>
    <node type="Action" id="632347578953311218" name="OpenDatabase" class="MaxActionNode" group="" path="Metreos.Native.Database" x="240" y="200">
      <linkto id="632347578953310802" type="Labeled" style="Bezier" ortho="true" label="success" />
      <linkto id="632347578953310939" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="DSN" type="variable">dbDsn</ap>
        <ap name="Name" type="literal">ApplicationSuite</ap>
        <ap name="Type" type="literal">mysql</ap>
      </Properties>
    </node>
    <node type="Comment" id="632348158383488597" text="This is a worker script that handles&#xD;&#xA;sending execute messages to IP&#xD;&#xA;phones.  We use a worker script&#xD;&#xA;because the SendExecute action is&#xD;&#xA;synchronous." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="224" y="32" />
    <node type="Comment" id="632348158383488598" text="Get the primary device&#xD;&#xA;for the user we want to&#xD;&#xA;add to the intercom." class="MaxCommentNode" group="Application Components" path="Metreos.StockTools" x="320" y="120" />
    <node type="Action" id="632350316832032217" name="If" class="MaxActionNode" group="" path="Metreos.Native.Conditional" x="704" y="200">
      <linkto id="632347578953310933" type="Labeled" style="Bezier" ortho="true" label="true" />
      <linkto id="632350316832032218" type="Labeled" style="Bezier" ortho="true" label="default" />
      <Properties final="false" type="native">
        <ap name="Value1" type="csharp">dlxResult.Rows.Count &gt; 0</ap>
      </Properties>
    </node>
    <node type="Label" id="632350316832032218" text="E" class="MaxLabelNode" group="Application Components" path="Metreos.StockTools" x="704" y="296" />
    <node type="Variable" id="632347578953310800" name="fromGuid" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="fromGuid" refType="reference">fromGuid</Properties>
    </node>
    <node type="Variable" id="632347578953310801" name="userId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="UInt" initWith="userId" refType="reference">userId</Properties>
    </node>
    <node type="Variable" id="632347578953310804" name="dlxResult" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="DataTable" refType="reference">dlxResult</Properties>
    </node>
    <node type="Variable" id="632347578953310808" name="executeItem" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="Metreos.Types.CiscoIpPhone.Execute" refType="reference">executeItem</Properties>
    </node>
    <node type="Variable" id="632347578953310947" name="appServerIpAddress" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" initWith="appServerIpAddress" refType="reference">appServerIpAddress</Properties>
    </node>
    <node type="Variable" id="632347578953311219" name="dbDsn" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">dbDsn</Properties>
    </node>
    <node type="Variable" id="632348158383488986" name="deviceId" class="MaxRecumbentVariableNode" group="Application Components" path="Metreos.StockTools" x="0" y="0">
      <Properties type="String" refType="reference">deviceId</Properties>
    </node>
  </canvas>
  <!--//// hidden canvases ////-->
  <Properties type="master" instanceType="multiInstance" desc="">
  </Properties>
</Application>